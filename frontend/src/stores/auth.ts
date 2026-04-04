import type { AxiosError } from 'axios'
import { defineStore } from 'pinia'

import { canAccessFunction, normalizeSessionPermission } from '../auth/permissions'
import { http } from '../api/http'
import type { PermissionAction, SessionUser } from '../types/auth'
import { clearStoredAuthToken, readStoredAuthToken, writeStoredAuthToken } from './auth-storage'

export type LoginCredentials = {
  username: string
  password: string
}

export type AuthUser = Record<string, unknown> & {
  id?: number | string
  department_id?: number | string
  username?: string
  userName?: string
  display_name?: string
  name?: string
  displayName?: string
  fullName?: string
  email?: string
  roles?: unknown[]
  permissions?: unknown[]
}

type AuthResponse = {
  token?: unknown
  accessToken?: unknown
  data?: {
    token?: unknown
    accessToken?: unknown
  }
  user?: unknown
}

const tokenCandidates = (payload: AuthResponse) => [
  payload.token,
  payload.accessToken,
  payload.data?.token,
  payload.data?.accessToken,
]

export const extractAccessToken = (payload: unknown) => {
  if (!payload || typeof payload !== 'object') {
    return null
  }

  const responsePayload = payload as AuthResponse

  for (const value of tokenCandidates(responsePayload)) {
    if (typeof value === 'string' && value.trim()) {
      return value.trim()
    }
  }

  return null
}

export const normalizeAuthUser = (payload: unknown): AuthUser | null => {
  if (!payload || typeof payload !== 'object') {
    return null
  }

  const responsePayload = payload as AuthResponse
  const candidate = responsePayload.user && typeof responsePayload.user === 'object'
    ? responsePayload.user
    : payload

  return candidate as AuthUser
}

export const toSessionUser = (user: AuthUser | null): SessionUser | null => {
  if (!user) {
    return null
  }

  const id = user.id
  const username = user.username ?? user.userName
  const displayName = user.display_name ?? user.displayName ?? user.fullName ?? user.name ?? user.username ?? user.userName
  const departmentId = user.department_id
  const roles = Array.isArray(user.roles) ? user.roles.filter((value): value is string => typeof value === 'string') : []
  const permissions = Array.isArray(user.permissions)
    ? user.permissions
        .map((permission) => normalizeSessionPermission(permission))
        .filter((permission): permission is NonNullable<typeof permission> => permission !== null)
    : []

  if ((typeof id !== 'number' && typeof id !== 'string') || typeof username !== 'string' || typeof displayName !== 'string') {
    return null
  }

  if (typeof departmentId !== 'number' && typeof departmentId !== 'string') {
    return null
  }

  return {
    id,
    username,
    display_name: displayName,
    department_id: departmentId,
    roles,
    permissions,
  }
}

export const getAuthErrorMessage = (error: unknown) => {
  const axiosError = error as AxiosError<{ message?: string; error?: string }>

  return (
    axiosError.response?.data?.message ??
    axiosError.response?.data?.error ??
    axiosError.message ??
    'Unable to complete authentication request.'
  )
}

export const getUserDisplayName = (user: AuthUser | null) => {
  if (!user) {
    return ''
  }

  const candidates = [user.display_name, user.displayName, user.fullName, user.name, user.username, user.userName, user.email]

  for (const value of candidates) {
    if (typeof value === 'string' && value.trim()) {
      return value.trim()
    }
  }

  return 'Authenticated user'
}

let initializeRequest: Promise<void> | null = null

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: readStoredAuthToken() as string | null,
    user: null as AuthUser | null,
    initialized: false,
    isBusy: false,
  }),

  getters: {
    isAuthenticated: (state) => Boolean(state.token),
    userDisplayName: (state) => getUserDisplayName(state.user),
    sessionUser: (state) => toSessionUser(state.user),
  },

  actions: {
    canAccess(functionCode?: string, action: PermissionAction = 'view') {
      return canAccessFunction(this.sessionUser?.permissions, functionCode, action)
    },

    setSessionToken(token: string) {
      this.token = token
      writeStoredAuthToken(token)
    },

    clearSession() {
      this.token = null
      this.user = null
      clearStoredAuthToken()
    },

    async fetchCurrentUser() {
      const response = await http.get('/auth/me')
      const user = normalizeAuthUser(response.data)

      if (!user) {
        throw new Error('Authenticated user profile is missing from the response.')
      }

      this.user = user
      return user
    },

    async initialize() {
      if (this.initialized) {
        return
      }

      if (!this.token) {
        this.initialized = true
        return
      }

      if (initializeRequest) {
        return initializeRequest
      }

      this.isBusy = true
      initializeRequest = (async () => {
        try {
          await this.fetchCurrentUser()
        } catch {
          this.clearSession()
        } finally {
          this.initialized = true
          this.isBusy = false
          initializeRequest = null
        }
      })()

      return initializeRequest
    },

    async login(credentials: LoginCredentials) {
      this.isBusy = true

      try {
        const response = await http.post('/auth/login', credentials)
        const token = extractAccessToken(response.data)

        if (!token) {
          throw new Error('Login response did not include an access token.')
        }

        this.setSessionToken(token)
        const user = await this.fetchCurrentUser()
        this.initialized = true

        return user
      } catch (error) {
        this.clearSession()
        this.initialized = true

        throw new Error(getAuthErrorMessage(error))
      } finally {
        this.isBusy = false
      }
    },

    logout() {
      this.clearSession()
      this.initialized = true
    },
  },
})
