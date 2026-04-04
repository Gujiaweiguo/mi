import type { AxiosError } from 'axios'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { AUTH_TOKEN_STORAGE_KEY } from '../stores/auth-storage'
import { attachAuthHeader, createApiError, handleResponseError } from './http'

describe('http platform helpers', () => {
  beforeEach(() => {
    window.localStorage.clear()
    vi.restoreAllMocks()
  })

  it('attaches the persisted bearer token to outgoing requests', () => {
    window.localStorage.setItem(AUTH_TOKEN_STORAGE_KEY, 'token-123')

    const config = attachAuthHeader({ headers: {} } as never)

    expect((config.headers as Record<string, string>).Authorization).toBe('Bearer token-123')
  })

  it('normalizes API errors and emits auth expiry events', async () => {
    const dispatchEvent = vi.spyOn(window, 'dispatchEvent')
    window.localStorage.setItem(AUTH_TOKEN_STORAGE_KEY, 'stale-token')

    const error = {
      message: 'Request failed with status code 401',
      config: { url: '/auth/me' },
      response: {
        status: 401,
        data: {
          message: 'missing session user',
        },
      },
    } as AxiosError<{ message: string }>

    await expect(handleResponseError(error)).rejects.toMatchObject({
      message: 'missing session user',
      status: 401,
      code: 'unauthorized',
    })
    expect(window.localStorage.getItem(AUTH_TOKEN_STORAGE_KEY)).toBeNull()
    expect(dispatchEvent).toHaveBeenCalledTimes(1)
  })

  it('creates fallback network errors for unavailable responses', () => {
    expect(createApiError({ message: 'Network Error' })).toMatchObject({
      message: 'Network Error',
      status: null,
      code: 'network_error',
      isApiError: true,
    })
  })
})
