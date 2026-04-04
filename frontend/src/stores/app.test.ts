import { beforeEach, describe, expect, it } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

import { useAppStore } from './app'

describe('app store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('exposes scaffold metadata', () => {
    const store = useAppStore()

    expect(store.appName).toBeTruthy()
    expect(store.appVersion).toBeTruthy()
    expect(store.apiBaseUrl).toBeTruthy()
  })
})
