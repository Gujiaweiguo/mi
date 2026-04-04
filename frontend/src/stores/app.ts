import { defineStore } from 'pinia'

import { env } from '../config/env'

export const useAppStore = defineStore('app', {
  state: () => ({
    appName: env.appName,
    appVersion: env.appVersion,
    apiBaseUrl: env.apiBaseUrl,
    mode: env.mode,
  }),
})
