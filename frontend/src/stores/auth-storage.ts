export const AUTH_TOKEN_STORAGE_KEY = 'mi.auth.token'

const hasStorage = () => typeof window !== 'undefined' && typeof window.localStorage !== 'undefined'

export const readStoredAuthToken = () => {
  if (!hasStorage()) {
    return null
  }

  return window.localStorage.getItem(AUTH_TOKEN_STORAGE_KEY)
}

export const writeStoredAuthToken = (token: string) => {
  if (!hasStorage()) {
    return
  }

  window.localStorage.setItem(AUTH_TOKEN_STORAGE_KEY, token)
}

export const clearStoredAuthToken = () => {
  if (!hasStorage()) {
    return
  }

  window.localStorage.removeItem(AUTH_TOKEN_STORAGE_KEY)
}
