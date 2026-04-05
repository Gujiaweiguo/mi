import { defineStore } from 'pinia'

import { env } from '../config/env'
import { resolveAppLocale, setI18nLocale } from '../i18n'
import type { AppLocale } from '../i18n'
import { writeStoredAppLocale } from './locale-storage'

export const useAppStore = defineStore('app', {
  state: () => ({
    appName: env.appName,
    appVersion: env.appVersion,
    apiBaseUrl: env.apiBaseUrl,
    mode: env.mode,
    locale: resolveAppLocale() as AppLocale,
  }),

  actions: {
    setLocale(locale: AppLocale) {
      this.locale = locale
      writeStoredAppLocale(locale)
      setI18nLocale(locale)
    },
  },
})
