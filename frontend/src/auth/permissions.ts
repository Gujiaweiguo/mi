import type { PermissionAction, PermissionLevel, SessionPermission, SessionUser } from '../types/auth'

export const FUNCTION_CODES = {
  workflowAdmin: 'workflow.admin',
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
  label: string
  path: string
  functionCode?: FunctionCode
  action?: PermissionAction
}

export const NAVIGATION_ITEMS: NavigationItem[] = [
  {
    label: 'Platform health',
    path: '/health',
  },
  {
    label: 'Workflow administration',
    path: '/workflow/admin',
    functionCode: FUNCTION_CODES.workflowAdmin,
  },
  {
    label: 'Master data admin',
    path: '/admin/master-data',
    functionCode: FUNCTION_CODES.masterdataAdmin,
  },
  {
    label: 'Sales data admin',
    path: '/admin/sales',
    functionCode: FUNCTION_CODES.salesAdmin,
  },
  {
    label: 'Base info admin',
    path: '/admin/base-info',
    functionCode: FUNCTION_CODES.baseinfoAdmin,
  },
  {
    label: 'Structure admin',
    path: '/admin/structure',
    functionCode: FUNCTION_CODES.structureAdmin,
  },
  {
    label: 'Rentable areas',
    path: '/admin/rentable-areas',
    functionCode: FUNCTION_CODES.structureAdmin,
  },
  {
    label: 'Lease contracts',
    path: '/lease/contracts',
    functionCode: FUNCTION_CODES.leaseContract,
  },
  {
    label: 'Billing charges',
    path: '/billing/charges',
    functionCode: FUNCTION_CODES.billingCharge,
  },
  {
    label: 'Billing invoices',
    path: '/billing/invoices',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
  {
    label: 'Tax exports',
    path: '/tax/exports',
    functionCode: FUNCTION_CODES.taxExport,
  },
  {
    label: 'Generalize reports',
    path: '/reports/generalize',
    functionCode: FUNCTION_CODES.generalizeReport,
  },
  {
    label: 'Visual shop analysis',
    path: '/reports/visual-shop',
    functionCode: FUNCTION_CODES.generalizeReport,
  },
  {
    label: 'Excel import/export',
    path: '/excel/io',
    functionCode: FUNCTION_CODES.excelIo,
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

export const resolveAuthorizedHomePath = (user: SessionUser | null) =>
  filterNavigationItems(user)[0]?.path ?? '/health'
