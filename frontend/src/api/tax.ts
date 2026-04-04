import { http } from './http'
import type { PaginatedResponse } from './types'

export interface TaxRule {
  id: number
  rule_set_id: number
  sequence_no: number
  entry_side: string
  charge_type_filter: string
  account_number: string
  account_name: string
  explanation_template: string
  use_tenant_name: boolean
  is_balancing_entry: boolean
  created_at: string
  updated_at: string
}

export interface TaxRuleSet {
  id: number
  code: string
  name: string
  document_type: string
  status: string
  created_by: number
  updated_by: number
  created_at: string
  updated_at: string
  rules: TaxRule[]
}

export const listTaxRuleSets = (params?: { page?: number; page_size?: number }) =>
  http.get<PaginatedResponse<TaxRuleSet>>('/tax/rule-sets', { params })
export const exportTaxVouchers = (data: { rule_set_code: string; from_date: string; to_date: string }) =>
  http.post('/tax/exports/vouchers', data, { responseType: 'blob' })
