import { describe, expect, it } from 'vitest'

import { FUNCTION_CODES } from '../auth/permissions'
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

  it('registers the workflow admin route with dual permission access', () => {
    const route = router.resolve('/workflow/admin')

    expect(route.name).toBe('workflow-admin')
    expect(route.meta.requiresAuth).toBe(true)
    expect(route.meta.permissionAnyOf).toEqual([FUNCTION_CODES.workflowDefinition, FUNCTION_CODES.workflowAdmin])
  })
})
