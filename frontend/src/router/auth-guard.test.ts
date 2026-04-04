import { describe, expect, it } from 'vitest'

import { FUNCTION_CODES } from '../auth/permissions'
import { resolveAuthRedirect, resolveRootRedirect } from './auth-guard'

const permittedUser = {
  id: 1,
  username: 'mika',
  display_name: 'Mika Xu',
  department_id: 1,
  roles: ['ops'],
  permissions: [
    {
      function_code: FUNCTION_CODES.workflowAdmin,
      permission_level: 'approve' as const,
      can_print: true,
      can_export: true,
    },
  ],
}

const createRoute = ({
  fullPath = '/health',
  path = '/health',
  meta = {},
}: {
  fullPath?: string
  path?: string
  meta?: Record<string, unknown>
}) => ({
  fullPath,
  path,
  meta,
})

describe('auth guard helpers', () => {
  it('redirects guests to login for protected routes', () => {
    expect(
      resolveAuthRedirect(createRoute({ meta: { requiresAuth: true } }), {
        isAuthenticated: false,
      }),
    ).toEqual({
      path: '/login',
      query: {
        redirect: '/health',
      },
    })
  })

  it('redirects authenticated users away from login', () => {
    expect(
      resolveAuthRedirect(
        createRoute({ fullPath: '/login', path: '/login', meta: { guestOnly: true } }),
        { isAuthenticated: true, user: permittedUser },
      ),
    ).toEqual({
      path: '/health',
    })
  })

  it('redirects authenticated users without required permission to forbidden', () => {
    expect(
      resolveAuthRedirect(
        createRoute({
          fullPath: '/tax/exports',
          path: '/tax/exports',
          meta: { requiresAuth: true, permissionCode: FUNCTION_CODES.taxExport },
        }),
        { isAuthenticated: true, user: permittedUser },
      ),
    ).toEqual({
      path: '/forbidden',
      query: {
        redirect: '/tax/exports',
      },
    })
  })

  it('maps the root path to the expected home route', () => {
    expect(resolveRootRedirect({ isAuthenticated: false })).toBe('/login')
    expect(resolveRootRedirect({ isAuthenticated: true, user: permittedUser })).toBe('/health')
  })
})
