import type { PermissionAction, PermissionLevel, SessionPermission, SessionUser } from '../types/auth'

export const FUNCTION_CODES = {
  workflowAdmin: 'workflow.admin',
  notificationAdmin: 'notification.admin',
  masterdataAdmin: 'masterdata.admin',
  salesAdmin: 'sales.admin',
  baseinfoAdmin: 'baseinfo.admin',
  structureAdmin: 'structure.admin',
  leaseContract: 'lease.contract',
  billingCharge: 'billing.charge',
  billingInvoice: 'billing.invoice',
  taxExport: 'tax.export',
  generalizeReport: 'reporting.generalize',
  excelIo: 'excel.io',
} as const

export type FunctionCode = (typeof FUNCTION_CODES)[keyof typeof FUNCTION_CODES]

export type NavigationItem = {
  path: string
  labelKey: string
  functionCode?: FunctionCode
  action?: PermissionAction
}

export type ResolvedNavigationItem = Omit<NavigationItem, 'labelKey'> & {
  label: string
}

export const NAVIGATION_ITEMS: NavigationItem[] = [
  {
    path: '/dashboard',
    labelKey: 'app.navigation.dashboard',
  },
  {
    path: '/workbench',
    labelKey: 'app.navigation.workbench',
  },
  {
    path: '/health',
    labelKey: 'app.navigation.health',
  },
  {
    path: '/workflow/admin',
    labelKey: 'app.navigation.workflowAdmin',
    functionCode: FUNCTION_CODES.workflowAdmin,
  },
  {
    path: '/notifications',
    labelKey: 'app.navigation.notifications',
    functionCode: FUNCTION_CODES.notificationAdmin,
  },
  {
    path: '/admin/master-data',
    labelKey: 'app.navigation.masterdataAdmin',
    functionCode: FUNCTION_CODES.masterdataAdmin,
  },
  {
    path: '/admin/sales',
    labelKey: 'app.navigation.salesAdmin',
    functionCode: FUNCTION_CODES.salesAdmin,
  },
  {
    path: '/admin/base-info',
    labelKey: 'app.navigation.baseinfoAdmin',
    functionCode: FUNCTION_CODES.baseinfoAdmin,
  },
  {
    path: '/admin/structure',
    labelKey: 'app.navigation.structureAdmin',
    functionCode: FUNCTION_CODES.structureAdmin,
  },
  {
    path: '/admin/rentable-areas',
    labelKey: 'app.navigation.rentableAreas',
    functionCode: FUNCTION_CODES.structureAdmin,
  },
  {
    path: '/lease/contracts',
    labelKey: 'app.navigation.leaseContracts',
    functionCode: FUNCTION_CODES.leaseContract,
  },
  {
    path: '/billing/charges',
    labelKey: 'app.navigation.billingCharges',
    functionCode: FUNCTION_CODES.billingCharge,
  },
  {
    path: '/billing/invoices',
    labelKey: 'app.navigation.billingInvoices',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
  {
    path: '/billing/receivables',
    labelKey: 'app.navigation.receivables',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
  {
    path: '/tax/exports',
    labelKey: 'app.navigation.taxExports',
    functionCode: FUNCTION_CODES.taxExport,
  },
  {
    path: '/reports/generalize',
    labelKey: 'app.navigation.generalizeReports',
    functionCode: FUNCTION_CODES.generalizeReport,
  },
  {
    path: '/reports/visual-shop',
    labelKey: 'app.navigation.visualShopAnalysis',
    functionCode: FUNCTION_CODES.generalizeReport,
  },
  {
    path: '/excel/io',
    labelKey: 'app.navigation.excelIo',
    functionCode: FUNCTION_CODES.excelIo,
  },
  {
    path: '/admin/users',
    labelKey: 'app.navigation.userManagement',
    functionCode: FUNCTION_CODES.baseinfoAdmin,
  },
]

const normalizePermissionLevel = (value: unknown): PermissionLevel => {
  switch (value) {
    case 'view':
    case 'edit':
    case 'approve':
      return value
    default:
      return ''
  }
}

const toBoolean = (value: unknown) => value === true

export const normalizeSessionPermission = (value: unknown): SessionPermission | null => {
  if (!value || typeof value !== 'object') {
    return null
  }

  const permission = value as Record<string, unknown>
  const functionCode = permission.function_code
  const permissionLevel = permission.permission_level

  if (typeof functionCode !== 'string' || !functionCode.trim()) {
    return null
  }

  return {
    function_code: functionCode.trim(),
    permission_level: normalizePermissionLevel(permissionLevel),
    can_print: toBoolean(permission.can_print),
    can_export: toBoolean(permission.can_export),
  }
}

export const getPermissionForFunction = (
  permissions: SessionPermission[] | undefined,
  functionCode: string,
) => permissions?.find((permission) => permission.function_code === functionCode) ?? null

export const canPerformAction = (
  permission: SessionPermission | null | undefined,
  action: PermissionAction = 'view',
) => {
  if (!permission) {
    return false
  }

  switch (action) {
    case 'view':
      return permission.permission_level !== ''
    case 'edit':
      return permission.permission_level === 'edit' || permission.permission_level === 'approve'
    case 'approve':
      return permission.permission_level === 'approve'
    case 'print':
      return permission.can_print
    case 'export':
      return permission.can_export
  }
}

export const canAccessFunction = (
  permissions: SessionPermission[] | undefined,
  functionCode?: string,
  action: PermissionAction = 'view',
) => {
  if (!functionCode) {
    return true
  }

  return canPerformAction(getPermissionForFunction(permissions, functionCode), action)
}

export const filterNavigationItems = (user: SessionUser | null) =>
  NAVIGATION_ITEMS.filter((item) => canAccessFunction(user?.permissions, item.functionCode, item.action))

export const resolveNavigationItems = (user: SessionUser | null, resolveLabel: (key: string) => string): ResolvedNavigationItem[] =>
  filterNavigationItems(user).map(({ labelKey, ...item }) => ({
    ...item,
    label: resolveLabel(labelKey),
  }))

export const resolveAuthorizedHomePath = (user: SessionUser | null) =>
  filterNavigationItems(user)[0]?.path ?? '/health'
