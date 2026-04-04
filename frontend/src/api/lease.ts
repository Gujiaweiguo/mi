import { http } from './http'
import type { IdempotencyRequest, PaginatedResponse } from './types'

export interface LeaseUnit {
  id: number
  lease_contract_id: number
  unit_id: number
  rent_area: number
  created_at: string
  updated_at: string
}

export interface LeaseTerm {
  id: number
  lease_contract_id: number
  term_type: string
  billing_cycle: string
  currency_type_id: number
  amount: number
  effective_from: string
  effective_to: string
  created_at: string
  updated_at: string
}

export interface LeaseContract {
  id: number
  amended_from_id: number | null
  lease_no: string
  department_id: number
  store_id: number
  building_id: number | null
  customer_id: number | null
  brand_id: number | null
  trade_id: number | null
  management_type_id: number | null
  tenant_name: string
  start_date: string
  end_date: string
  status: string
  workflow_instance_id: number | null
  effective_version: number
  submitted_at: string | null
  approved_at: string | null
  billing_effective_at: string | null
  terminated_at: string | null
  created_by: number
  updated_by: number
  created_at: string
  updated_at: string
  units: LeaseUnit[]
  terms: LeaseTerm[]
}

export interface LeaseSummary {
  id: number
  lease_no: string
  tenant_name: string
  department_id: number
  store_id: number
  building_id: number | null
  customer_id: number | null
  brand_id: number | null
  trade_id: number | null
  management_type_id: number | null
  start_date: string
  end_date: string
  status: string
  workflow_instance_id: number | null
  billing_effective_at: string | null
  updated_at: string
}

export interface CreateLeaseRequest {
  lease_no: string
  department_id: number
  store_id: number
  building_id?: number | null
  customer_id?: number | null
  brand_id?: number | null
  trade_id?: number | null
  management_type_id?: number | null
  tenant_name: string
  start_date: string
  end_date: string
  units: Array<{ unit_id: number; rent_area: number }>
  terms: Array<{
    term_type: string
    billing_cycle: string
    currency_type_id: number
    amount: number
    effective_from: string
    effective_to: string
  }>
}

export interface ListLeasesParams {
  lease_no?: string
  status?: string
  store_id?: number
  page?: number
  page_size?: number
}

export const createLease = (data: CreateLeaseRequest) => http.post<{ lease: LeaseContract }>('/leases', data)
export const listLeases = (params?: ListLeasesParams) =>
  http.get<PaginatedResponse<LeaseSummary>>('/leases', { params })
export const getLease = (id: number) => http.get<{ lease: LeaseContract }>(`/leases/${id}`)
export const submitLease = (id: number, data: IdempotencyRequest) =>
  http.post<{ lease: LeaseContract }>(`/leases/${id}/submit`, data)
export const amendLease = (id: number, data: CreateLeaseRequest) =>
  http.post<{ lease: LeaseContract }>(`/leases/${id}/amend`, data)
export const terminateLease = (id: number, data: { terminated_at: string }) =>
  http.post<{ lease: LeaseContract }>(`/leases/${id}/terminate`, data)
