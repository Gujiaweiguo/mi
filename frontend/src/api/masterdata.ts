import { http } from './http'

export interface Customer {
  id: number
  code: string
  name: string
  trade_id: number | null
  department_id: number | null
  status: string
  created_at: string
  updated_at: string
}

export interface Brand {
  id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

export interface CreateCustomerRequest {
  code: string
  name: string
  trade_id?: number | null
  department_id?: number | null
  status?: string
}

export interface CreateBrandRequest {
  code: string
  name: string
  status?: string
}

export const listCustomers = () => http.get<{ customers: Customer[] }>('/master-data/customers')
export const createCustomer = (data: CreateCustomerRequest) =>
  http.post<{ customer: Customer }>('/master-data/customers', data)
export const listBrands = () => http.get<{ brands: Brand[] }>('/master-data/brands')
export const createBrand = (data: CreateBrandRequest) =>
  http.post<{ brand: Brand }>('/master-data/brands', data)
