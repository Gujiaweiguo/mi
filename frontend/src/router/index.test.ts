import { describe, expect, it } from 'vitest'

import router from './index'

describe('router', () => {
  it('registers the authenticated workbench route', () => {
    const route = router.resolve('/workbench')

    expect(route.name).toBe('workbench')
    expect(route.meta.requiresAuth).toBe(true)
  })

  it('registers the lease amendment route that reuses the create flow', () => {
    const route = router.resolve('/lease/contracts/42/amend')

    expect(route.name).toBe('lease-contracts-amend')
    expect(route.params.id).toBe('42')
    expect(route.meta.requiresAuth).toBe(true)
  })
})
