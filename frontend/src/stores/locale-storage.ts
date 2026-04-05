import type { AppLocale } from '../i18n'

export const APP_LOCALE_STORAGE_KEY = 'mi.app.locale'

const hasStorage = () => typeof window !== 'undefined' && typeof window.localStorage !== 'undefined'

export const readStoredAppLocale = () => {
  if (!hasStorage()) {
    return null
  }

  return window.localStorage.getItem(APP_LOCALE_STORAGE_KEY)
}

export const writeStoredAppLocale = (locale: AppLocale) => {
  if (!hasStorage()) {
    return
  }

  window.localStorage.setItem(APP_LOCALE_STORAGE_KEY, locale)
}
