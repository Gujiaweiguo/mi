import { beforeEach, describe, expect, it } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

import { DEFAULT_APP_LOCALE, i18n } from '../i18n'
import { useAppStore } from './app'
import { APP_LOCALE_STORAGE_KEY } from './locale-storage'

describe('app store', () => {
  beforeEach(() => {
    window.localStorage.clear()
    setActivePinia(createPinia())
    i18n.global.locale.value = DEFAULT_APP_LOCALE
  })

  it('exposes scaffold metadata', () => {
    const store = useAppStore()

    expect(store.appName).toBeTruthy()
    expect(store.appVersion).toBeTruthy()
    expect(store.apiBaseUrl).toBeTruthy()
  })

  it('defaults locale to simplified Chinese when no preference is stored', () => {
    const store = useAppStore()

    expect(store.locale).toBe('zh-CN')
  })

  it('restores and persists the selected locale', () => {
    window.localStorage.setItem(APP_LOCALE_STORAGE_KEY, 'en-US')
    const store = useAppStore()

    expect(store.locale).toBe('en-US')

    store.setLocale('zh-CN')

    expect(window.localStorage.getItem(APP_LOCALE_STORAGE_KEY)).toBe('zh-CN')
    expect(i18n.global.locale.value).toBe('zh-CN')
  })
})
