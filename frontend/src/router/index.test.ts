import { describe, expect, it } from 'vitest'

import router from './index'

describe('router', () => {
  it('registers the authenticated workbench route', () => {
    const route = router.resolve('/workbench')

    expect(route.name).toBe('workbench')
    expect(route.meta.requiresAuth).toBe(true)
  })
})
