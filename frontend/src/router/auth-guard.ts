import type { RouteLocationNormalized } from 'vue-router'

import { canAccessFunction, resolveAuthorizedHomePath } from '../auth/permissions'
import type { PermissionAction, SessionUser } from '../types/auth'

export const DEFAULT_AUTHENTICATED_PATH = '/health'
export const LOGIN_PATH = '/login'
export const FORBIDDEN_PATH = '/forbidden'

type AuthGuardState = {
  isAuthenticated: boolean
  user?: SessionUser | null
}

type RoutePermissionMeta = {
  guestOnly?: boolean
  requiresAuth?: boolean
  permissionCode?: string
  permissionAction?: PermissionAction
}

export const resolveRootRedirect = (authState: AuthGuardState) =>
  authState.isAuthenticated ? resolveAuthorizedHomePath(authState.user ?? null) : LOGIN_PATH

export const hasRoutePermission = (
  to: Pick<RouteLocationNormalized, 'meta'>,
  authState: AuthGuardState,
) => {
  const meta = to.meta as RoutePermissionMeta

  return canAccessFunction(authState.user?.permissions, meta.permissionCode, meta.permissionAction ?? 'view')
}

export const resolveAuthRedirect = (
  to: Pick<RouteLocationNormalized, 'fullPath' | 'path' | 'meta'>,
  authState: AuthGuardState,
) => {
  const meta = to.meta as RoutePermissionMeta

  if (to.meta.requiresAuth && !authState.isAuthenticated) {
    return {
      path: LOGIN_PATH,
      query: {
        redirect: to.fullPath,
      },
    }
  }

  if (meta.guestOnly && authState.isAuthenticated) {
    return {
      path: resolveAuthorizedHomePath(authState.user ?? null),
    }
  }

  if (meta.requiresAuth && !hasRoutePermission(to, authState)) {
    return {
      path: FORBIDDEN_PATH,
      query: {
        redirect: to.fullPath,
      },
    }
  }

  return true
}
