import { describe, expect, it } from 'vitest'

import {
  FUNCTION_CODES,
  canAccessFunction,
  filterNavigationItems,
  normalizeSessionPermission,
  resolveAuthorizedHomePath,
} from './permissions'
import type { SessionUser } from '../types/auth'

const baseUser: SessionUser = {
  id: 1,
  username: 'casey',
  display_name: 'Casey Wang',
  department_id: 10,
  roles: ['ops'],
  permissions: [
    {
      function_code: FUNCTION_CODES.leaseContract,
      permission_level: 'edit',
      can_print: true,
      can_export: false,
    },
    {
      function_code: FUNCTION_CODES.excelIo,
      permission_level: 'view',
      can_print: false,
      can_export: true,
    },
  ],
}

describe('permission helpers', () => {
  it('normalizes backend permission payloads', () => {
    expect(
      normalizeSessionPermission({
        function_code: FUNCTION_CODES.billingInvoice,
        permission_level: 'approve',
        can_print: true,
        can_export: true,
      }),
    ).toEqual({
      function_code: FUNCTION_CODES.billingInvoice,
      permission_level: 'approve',
      can_print: true,
      can_export: true,
    })
  })

  it('evaluates actions against backend permission semantics', () => {
    expect(canAccessFunction(baseUser.permissions, FUNCTION_CODES.leaseContract, 'view')).toBe(true)
    expect(canAccessFunction(baseUser.permissions, FUNCTION_CODES.leaseContract, 'edit')).toBe(true)
    expect(canAccessFunction(baseUser.permissions, FUNCTION_CODES.leaseContract, 'approve')).toBe(false)
    expect(canAccessFunction(baseUser.permissions, FUNCTION_CODES.excelIo, 'export')).toBe(true)
    expect(canAccessFunction(baseUser.permissions, FUNCTION_CODES.taxExport, 'view')).toBe(false)
  })

  it('filters navigation items to authorized entries', () => {
    const paths = filterNavigationItems(baseUser).map((item) => item.path)

    expect(paths).toEqual(['/health', '/lease/contracts', '/excel/io'])
    expect(resolveAuthorizedHomePath(baseUser)).toBe('/health')
  })
})
