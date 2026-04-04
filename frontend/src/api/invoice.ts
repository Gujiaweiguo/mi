import { http } from './http'
import type { IdempotencyRequest, PaginatedResponse } from './types'

export interface InvoiceLine {
  id: number
  billing_document_id: number
  billing_charge_line_id: number
  charge_type: string
  period_start: string
  period_end: string
  quantity_days: number
  unit_amount: number
  amount: number
  created_at: string
}

export interface InvoiceDocument {
  id: number
  document_type: string
  document_no: string | null
  billing_run_id: number
  lease_contract_id: number
  tenant_name: string
  period_start: string
  period_end: string
  total_amount: number
  currency_type_id: number
  status: string
  workflow_instance_id: number | null
  adjusted_from_id: number | null
  submitted_at: string | null
  approved_at: string | null
  cancelled_at: string | null
  created_by: number
  updated_by: number
  created_at: string
  updated_at: string
  lines: InvoiceLine[]
}

export interface CreateInvoiceRequest {
  document_type: 'bill' | 'invoice'
  billing_charge_line_ids: number[]
}

export interface ListInvoicesParams {
  document_type?: string
  status?: string
  lease_contract_id?: number
  billing_run_id?: number
  page?: number
  page_size?: number
}

export interface AdjustInvoiceRequest {
  lines: Array<{ billing_charge_line_id: number; amount: number }>
}

export const createInvoice = (data: CreateInvoiceRequest) =>
  http.post<{ document: InvoiceDocument }>('/invoices', data)
export const listInvoices = (params?: ListInvoicesParams) =>
  http.get<PaginatedResponse<InvoiceDocument>>('/invoices', { params })
export const getInvoice = (id: number) => http.get<{ document: InvoiceDocument }>(`/invoices/${id}`)
export const submitInvoice = (id: number, data: IdempotencyRequest) =>
  http.post<{ document: InvoiceDocument }>(`/invoices/${id}/submit`, data)
export const cancelInvoice = (id: number) => http.post<{ document: InvoiceDocument }>(`/invoices/${id}/cancel`)
export const adjustInvoice = (id: number, data: AdjustInvoiceRequest) =>
  http.post<{ document: InvoiceDocument }>(`/invoices/${id}/adjust`, data)
