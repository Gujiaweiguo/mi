import { http } from './http'

export interface Department {
  id: number
  code: string
  name: string
  level: number
  status: string
  parent_id: number | null
  type_id: number
}

export interface Store {
  id: number
  department_id: number
  code: string
  name: string
  short_name: string
  status: string
}

export const listDepartments = () => http.get<{ departments: Department[] }>('/org/departments')
export const listStores = () => http.get<{ stores: Store[] }>('/org/stores')
