import axios from 'axios'
import type { AxiosError, AxiosRequestConfig, InternalAxiosRequestConfig } from 'axios'

import { env } from '../config/env'
import { clearStoredAuthToken, readStoredAuthToken } from '../stores/auth-storage'

export type ApiError = Error & {
  status: number | null
  code: string
  isApiError: true
}

export const createApiError = (error: unknown): ApiError => {
  const axiosError = error as AxiosError<{ message?: string; error?: string }>
  const status = axiosError.response?.status ?? null
  const message =
    axiosError.response?.data?.message ??
    axiosError.response?.data?.error ??
    axiosError.message ??
    'Request failed.'
  const normalizedError = new Error(message) as ApiError

  normalizedError.status = status
  normalizedError.code = status === 401 ? 'unauthorized' : status ? `http_${status}` : 'network_error'
  normalizedError.isApiError = true

  return normalizedError
}

export const attachAuthHeader = (config: InternalAxiosRequestConfig) => {
  const token = readStoredAuthToken()

  if (token) {
    config.headers = config.headers ?? {}
    ;(config.headers as Record<string, string>).Authorization = `Bearer ${token}`
  }

  return config
}

const emitWindowEvent = (name: string, detail?: unknown) => {
  if (typeof window !== 'undefined') {
    window.dispatchEvent(new CustomEvent(name, { detail }))
  }
}

const isAuthLoginRequest = (config: AxiosRequestConfig | undefined) =>
  typeof config?.url === 'string' && config.url.includes('/auth/login')

export const handleResponseError = (error: unknown) => {
  const normalizedError = createApiError(error)

  if (normalizedError.status === 401) {
    clearStoredAuthToken()

    if (!isAuthLoginRequest((error as AxiosError).config)) {
      emitWindowEvent('mi:http-auth-expired', normalizedError)
    }
  } else if (normalizedError.status === null || normalizedError.status >= 500) {
    emitWindowEvent('mi:http-error', normalizedError)
  }

  return Promise.reject(normalizedError)
}

export const http = axios.create({
  baseURL: env.apiBaseUrl,
  timeout: env.apiTimeoutMs,
  headers: {
    'X-Requested-With': 'XMLHttpRequest',
  },
})

http.interceptors.request.use(attachAuthHeader)
http.interceptors.response.use((response) => response, handleResponseError)
