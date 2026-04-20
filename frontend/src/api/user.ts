import { http } from './http'

export interface UserItem {
  id: number
  department_id: number
  username: string
  display_name: string
  status: string
}

export interface RoleItem {
  ID: number
  Code: string
  Name: string
  Status: string
  IsLeader: boolean
}

export const listUsers = () => http.get<{ users: UserItem[] }>('/users')
export const createUser = (data: { username: string; display_name: string; password: string; department_id: number; role_ids: number[] }) => http.post<{ user: UserItem }>('/users', data)
export const updateUser = (id: number, data: { display_name?: string; department_id?: number; status?: string }) => http.put<{ user: UserItem }>(`/users/${id}`, data)
export const resetPassword = (id: number, data: { new_password: string }) => http.post(`/users/${id}/reset-password`, data)
export const setUserRoles = (id: number, data: { role_ids: number[]; department_id: number }) => http.put(`/users/${id}/roles`, data)
export const listRoles = () => http.get<{ roles: RoleItem[] }>('/roles')
