import type { Component } from 'vue'
import { createI18n } from 'vue-i18n'
import en from 'element-plus/es/locale/lang/en'
import zhCn from 'element-plus/es/locale/lang/zh-cn'

import { readStoredAppLocale } from '../stores/locale-storage'
import { enUSMessages } from './messages/en-US'
import { zhCNMessages } from './messages/zh-CN'

export const APP_LOCALES = ['zh-CN', 'en-US'] as const
export type AppLocale = (typeof APP_LOCALES)[number]

export const DEFAULT_APP_LOCALE: AppLocale = 'zh-CN'

export const APP_LOCALE_OPTIONS: Array<{ value: AppLocale; labelKey: string }> = [
  {
    value: 'zh-CN',
    labelKey: 'common.languages.zh-CN',
  },
  {
    value: 'en-US',
    labelKey: 'common.languages.en-US',
  },
]

export const isAppLocale = (value: unknown): value is AppLocale =>
  typeof value === 'string' && APP_LOCALES.includes(value as AppLocale)

export const resolveAppLocale = (value: unknown = readStoredAppLocale()): AppLocale =>
  isAppLocale(value) ? value : DEFAULT_APP_LOCALE

export const messages = {
  'zh-CN': zhCNMessages,
  'en-US': enUSMessages,
} as const

export const i18n = createI18n({
  legacy: false,
  locale: resolveAppLocale(),
  fallbackLocale: DEFAULT_APP_LOCALE,
  messages,
})

export const setI18nLocale = (locale: AppLocale) => {
  i18n.global.locale.value = locale
}

export const translateMessage = (key: string, values?: Record<string, unknown>) => i18n.global.t(key, values ?? {})

export const resolveElementPlusLocale = (locale: AppLocale): Component => (locale === 'en-US' ? en : zhCn)
