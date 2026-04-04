import { beforeEach, describe, expect, it, vi } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

import { http } from '../api/http'
import { useAuthStore } from './auth'
import { AUTH_TOKEN_STORAGE_KEY } from './auth-storage'

vi.mock('../api/http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
  },
}))

const mockedHttp = http as unknown as {
  get: ReturnType<typeof vi.fn>
  post: ReturnType<typeof vi.fn>
}

describe('auth store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    window.localStorage.clear()
    vi.clearAllMocks()
  })

  it('stores the access token and user during login', async () => {
    mockedHttp.post.mockResolvedValue({
      data: {
        token: 'token-123',
      },
    })
    mockedHttp.get.mockResolvedValue({
      data: {
        user: {
          id: 1,
          username: 'alex',
          department_id: 12,
          displayName: 'Alex Chen',
          roles: ['ops'],
          permissions: [
            {
              function_code: 'lease.contract',
              permission_level: 'edit',
              can_print: true,
              can_export: false,
            },
          ],
        },
      },
    })

    const store = useAuthStore()

    await store.login({
      username: 'alex',
      password: 'secret',
    })

    expect(store.isAuthenticated).toBe(true)
    expect(store.userDisplayName).toBe('Alex Chen')
    expect(store.sessionUser?.permissions).toHaveLength(1)
    expect(store.canAccess('lease.contract', 'edit')).toBe(true)
    expect(window.localStorage.getItem(AUTH_TOKEN_STORAGE_KEY)).toBe('token-123')
    expect(mockedHttp.get).toHaveBeenCalledWith('/auth/me')
  })

  it('clears an invalid persisted session during initialization', async () => {
    window.localStorage.setItem(AUTH_TOKEN_STORAGE_KEY, 'stale-token')
    mockedHttp.get.mockRejectedValue(new Error('Unauthorized'))

    const store = useAuthStore()

    await store.initialize()

    expect(store.initialized).toBe(true)
    expect(store.isAuthenticated).toBe(false)
    expect(window.localStorage.getItem(AUTH_TOKEN_STORAGE_KEY)).toBeNull()
  })
})
