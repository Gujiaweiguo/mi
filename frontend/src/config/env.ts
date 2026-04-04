const defaultAppName = 'MI Migration Frontend'
const defaultAppVersion = '0.1.0'
const defaultApiBaseUrl = '/api'
const defaultApiTimeoutMs = 10000

export const normalizeBaseUrl = (value: string) => {
  const trimmedValue = value.trim()

  if (!trimmedValue) {
    return defaultApiBaseUrl
  }

  if (trimmedValue.length > 1 && trimmedValue.endsWith('/')) {
    return trimmedValue.slice(0, -1)
  }

  return trimmedValue
}

export const parseNumber = (value: string | undefined, fallback: number) => {
  const parsedValue = Number(value)

  return Number.isFinite(parsedValue) && parsedValue > 0 ? parsedValue : fallback
}

export type AppEnv = Readonly<{
  mode: string
  appName: string
  appVersion: string
  apiBaseUrl: string
  apiTimeoutMs: number
}>

export const env: AppEnv = Object.freeze({
  mode: import.meta.env.MODE,
  appName: import.meta.env.VITE_APP_NAME?.trim() || defaultAppName,
  appVersion: import.meta.env.VITE_APP_VERSION?.trim() || defaultAppVersion,
  apiBaseUrl: normalizeBaseUrl(import.meta.env.VITE_API_BASE_URL ?? defaultApiBaseUrl),
  apiTimeoutMs: parseNumber(import.meta.env.VITE_API_TIMEOUT_MS, defaultApiTimeoutMs),
})
