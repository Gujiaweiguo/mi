export type PermissionLevel = '' | 'view' | 'edit' | 'approve'

export type PermissionAction = 'view' | 'edit' | 'approve' | 'print' | 'export'

export type SessionPermission = {
  function_code: string
  permission_level: PermissionLevel
  can_print: boolean
  can_export: boolean
}

export type SessionUser = {
  id: number | string
  username: string
  display_name: string
  department_id: number | string
  roles: string[]
  permissions: SessionPermission[]
}
