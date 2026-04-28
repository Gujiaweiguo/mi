import { http } from './http'
import type { IdempotencyRequest, PaginatedResponse } from './types'

export type LeaseContractSubtype = 'standard' | 'joint_operation' | 'ad_board' | 'area_ground'

export type AdBoardFrequency = 'D' | 'M' | 'W'

export type OvertimeBillStatus =
  | 'draft'
  | 'pending_approval'
  | 'approved'
  | 'rejected'
  | 'cancelled'
  | 'stopped'
  | 'generated'

export type OvertimeFormulaType = 'fixed' | 'one_time' | 'percentage'

export type OvertimeRateType = 'daily' | 'monthly'

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

export interface LeaseJointOperationFields {
  lease_contract_id?: number
  bill_cycle: number
  rent_inc: string
  account_cycle: number
  tax_rate: number
  tax_type: number
  settlement_currency_type_id: number
  in_tax_rate: number
  out_tax_rate: number
  month_settle_days: number
  late_pay_interest_rate: number
  interest_grace_days: number
  created_at?: string
  updated_at?: string
}

export interface LeaseAdBoardDetail {
  id?: number
  lease_contract_id?: number
  ad_board_id: number
  description: string
  status: number
  start_date: string
  end_date: string
  rent_area: number
  airtime: number
  frequency: AdBoardFrequency
  frequency_days: number
  frequency_mon: boolean
  frequency_tue: boolean
  frequency_wed: boolean
  frequency_thu: boolean
  frequency_fri: boolean
  frequency_sat: boolean
  frequency_sun: boolean
  between_from: number
  between_to: number
  store_id: number | null
  building_id: number | null
  created_at?: string
  updated_at?: string
}

export interface LeaseAreaGroundDetail {
  id?: number
  lease_contract_id?: number
  code: string
  name: string
  type_id: number
  description: string
  status: number
  start_date: string
  end_date: string
  rent_area: number
  created_at?: string
  updated_at?: string
}

export interface LeaseContract {
  id: number
  amended_from_id: number | null
  lease_no: string
  subtype: LeaseContractSubtype
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
  joint_operation: LeaseJointOperationFields | null
  ad_boards: LeaseAdBoardDetail[]
  area_grounds: LeaseAreaGroundDetail[]
  units: LeaseUnit[]
  terms: LeaseTerm[]
}

export interface LeaseSummary {
  id: number
  lease_no: string
  tenant_name: string
  subtype: LeaseContractSubtype
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
  subtype?: LeaseContractSubtype
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
  joint_operation?: LeaseJointOperationFields | null
  ad_boards?: LeaseAdBoardDetail[]
  area_grounds?: LeaseAreaGroundDetail[]
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

export interface OvertimePercentTier {
  id?: number
  formula_id?: number
  sales_to: number
  percentage: number
  sort_order?: number
  created_at?: string
}

export interface OvertimeMinimumTier {
  id?: number
  formula_id?: number
  sales_to: number
  minimum_sum: number
  sort_order?: number
  created_at?: string
}

export interface OvertimeFormula {
  id?: number
  overtime_bill_id?: number
  charge_type: string
  formula_type: OvertimeFormulaType
  rate_type: OvertimeRateType
  effective_from: string
  effective_to: string
  currency_type_id: number
  total_area: number
  unit_price: number
  base_amount: number
  fixed_rental: number
  percentage_option: string
  minimum_option: string
  sort_order?: number
  created_at?: string
  updated_at?: string
  percentage_tiers: OvertimePercentTier[]
  minimum_tiers: OvertimeMinimumTier[]
}

export interface OvertimeGeneratedCharge {
  id: number
  billing_run_id: number
  overtime_bill_id: number
  overtime_formula_id: number
  lease_contract_id: number
  workflow_instance_id: number | null
  charge_type: string
  formula_type: OvertimeFormulaType
  rate_type: OvertimeRateType
  period_start: string
  period_end: string
  quantity: number
  total_area: number
  unit_price: number
  base_amount: number
  fixed_rental: number
  percentage_option: string
  minimum_option: string
  applied_percentage_rate: number
  applied_minimum_amount: number
  unit_amount: number
  amount: number
  currency_type_id: number
  generated_by: number
  created_at: string
}

export interface OvertimeBill {
  id: number
  lease_contract_id: number
  lease_no: string
  tenant_name: string
  period_start: string
  period_end: string
  status: OvertimeBillStatus
  workflow_instance_id: number | null
  note: string
  submitted_at: string | null
  approved_at: string | null
  rejected_at: string | null
  cancelled_at: string | null
  stopped_at: string | null
  generated_at: string | null
  created_by: number
  updated_by: number
  created_at: string
  updated_at: string
  formulas: OvertimeFormula[]
  generated_charges: OvertimeGeneratedCharge[]
}

export interface CreateOvertimeBillRequest {
  lease_contract_id: number
  period_start: string
  period_end: string
  note?: string
  formulas: OvertimeFormula[]
}

export interface OvertimeGenerateSkippedItem {
  formula_id: number
  reason: string
}

export interface OvertimeGenerateTotals {
  generated: number
  skipped: number
}

export interface OvertimeGenerateResponse {
  run: Record<string, unknown> | null
  charges: OvertimeGeneratedCharge[]
  skipped: OvertimeGenerateSkippedItem[]
  totals: OvertimeGenerateTotals
}

export interface ListLeasesParams {
  lease_no?: string
  status?: string
  store_id?: number
  page?: number
  page_size?: number
}

export interface ListOvertimeBillsParams {
  lease_contract_id?: number
  status?: OvertimeBillStatus
  period_start?: string
  period_end?: string
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

export const createOvertimeBill = (data: CreateOvertimeBillRequest) =>
  http.post<{ bill: OvertimeBill }>('/overtime/bills', data)
export const listOvertimeBills = (params?: ListOvertimeBillsParams) =>
  http.get<PaginatedResponse<OvertimeBill>>('/overtime/bills', { params })
export const getOvertimeBill = (id: number) => http.get<{ bill: OvertimeBill }>(`/overtime/bills/${id}`)
export const submitOvertimeBill = (id: number, data: IdempotencyRequest & { comment?: string }) =>
  http.post<{ bill: OvertimeBill }>(`/overtime/bills/${id}/submit`, data)
export const cancelOvertimeBill = (id: number) => http.post<{ bill: OvertimeBill }>(`/overtime/bills/${id}/cancel`)
export const stopOvertimeBill = (id: number) => http.post<{ bill: OvertimeBill }>(`/overtime/bills/${id}/stop`)
export const generateOvertimeBillCharges = (id: number) =>
  http.post<OvertimeGenerateResponse>(`/overtime/bills/${id}/generate`, {})
