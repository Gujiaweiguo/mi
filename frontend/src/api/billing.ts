import { http } from './http'
import type { PaginatedResponse } from './types'

export interface ChargeLine {
  id: number
  billing_run_id: number
  lease_contract_id: number
  lease_no: string
  tenant_name: string
  lease_term_id: number | null
  charge_type: string
  charge_source: string
  overtime_bill_id: number | null
  overtime_formula_id: number | null
  overtime_charge_id: number | null
  period_start: string
  period_end: string
  quantity_days: number
  unit_amount: number
  amount: number
  currency_type_id: number
  source_effective_version: number
  created_at: string
}

export interface BillingRun {
  id: number
  period_start: string
  period_end: string
  status: string
  triggered_by: number
  generated_count: number
  skipped_count: number
  created_at: string
  updated_at: string
}

export interface GenerateResult {
  run: BillingRun
  lines: ChargeLine[]
  totals: {
    generated: number
    skipped: number
  }
}

export interface ListChargesParams {
  lease_contract_id?: number
  period_start?: string
  period_end?: string
  page?: number
  page_size?: number
}

export const generateCharges = (data: { period_start: string; period_end: string }) =>
  http.post<GenerateResult>('/billing/charges/generate', data)
export const listCharges = (params?: ListChargesParams) =>
  http.get<PaginatedResponse<ChargeLine>>('/billing/charges', { params })
