import { http } from './http'
import type { IdempotencyRequest, PaginatedResponse } from './types'

export interface InvoiceLine {
  id: number
  billing_document_id: number
  billing_charge_line_id: number
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

export type ReceivableSettlementStatus = 'outstanding' | 'settled'

export interface ReceivableOpenItem {
  id: number
  lease_contract_id: number
  billing_document_id: number
  billing_document_line_id: number | null
  customer_id: number
  department_id: number
  trade_id: number | null
  charge_type: string
  charge_source: string
  overtime_bill_id: number | null
  overtime_formula_id: number | null
  overtime_charge_id: number | null
  due_date: string
  outstanding_amount: number
  settled_at: string | null
  is_deposit: boolean
  created_at: string
  updated_at: string
}

export interface ReceivablePaymentEntry {
  id: number
  billing_document_id: number
  lease_contract_id: number
  payment_date: string
  amount: number
  note: string | null
  recorded_by: number
  idempotency_key: string
  created_at: string
}

export type InvoiceDiscountStatus = 'draft' | 'pending_approval' | 'approved' | 'rejected'

export interface InvoiceDiscount {
  id: number
  billing_document_id: number
  billing_document_line_id: number
  lease_contract_id: number
  charge_type: string
  requested_amount: number
  requested_rate: number
  reason: string
  status: InvoiceDiscountStatus
  workflow_instance_id: number | null
  idempotency_key: string
  submitted_at: string | null
  approved_at: string | null
  rejected_at: string | null
  created_by: number
  updated_by: number
  created_at: string
  updated_at: string
}

export type SurplusEntryType = 'overpayment' | 'application'

export interface InvoiceSurplusEntry {
  id: number
  surplus_balance_id: number
  entry_type: SurplusEntryType
  customer_id: number
  billing_document_id: number | null
  ar_open_item_id: number | null
  amount: number
  note: string | null
  idempotency_key: string
  recorded_by: number
  created_at: string
}

export interface InvoiceInterestEntry {
  id: number
  source_ar_open_item_id: number
  source_billing_document_id: number
  source_billing_document_line_id: number
  generated_billing_document_id: number
  generated_billing_document_line_id: number
  charge_type: string
  principal_amount: number
  daily_rate: number
  grace_days: number
  covered_start_date: string
  covered_end_date: string
  interest_days: number
  interest_amount: number
  idempotency_key: string
  created_by: number
  created_at: string
}

export interface InvoiceDepositApplicationEntry {
  id: number
  source_billing_document_id: number
  source_billing_document_line_id: number
  source_ar_open_item_id: number
  target_billing_document_id: number
  target_billing_document_line_id: number
  target_ar_open_item_id: number
  lease_contract_id: number
  amount: number
  note: string | null
  idempotency_key: string
  applied_by: number
  created_at: string
}

export interface InvoiceDepositRefundEntry {
  id: number
  billing_document_id: number
  billing_document_line_id: number
  ar_open_item_id: number
  lease_contract_id: number
  amount: number
  reason: string
  idempotency_key: string
  refunded_by: number
  created_at: string
}

export interface InvoiceReceivable {
  billing_document_id: number
  document_no: string | null
  document_type: string
  tenant_name: string
  lease_contract_id: number
  outstanding_amount: number
  customer_surplus_available: number
  settlement_status: ReceivableSettlementStatus
  items: ReceivableOpenItem[]
  payment_history: ReceivablePaymentEntry[]
  discount_history: InvoiceDiscount[]
  surplus_history: InvoiceSurplusEntry[]
  interest_history: InvoiceInterestEntry[]
  deposit_application_history: InvoiceDepositApplicationEntry[]
  deposit_refund_history: InvoiceDepositRefundEntry[]
}

export interface ReceivableListItem {
  billing_document_id: number
  document_type: string
  document_no: string | null
  tenant_name: string
  document_status: string
  lease_contract_id: number
  customer_id: number
  department_id: number
  trade_id: number | null
  earliest_due_date: string
  latest_due_date: string
  outstanding_amount: number
  settlement_status: ReceivableSettlementStatus
}

export interface ListReceivablesParams {
  customer_id?: number
  department_id?: number
  due_date_start?: string
  due_date_end?: string
  page?: number
  page_size?: number
}

export interface RecordInvoicePaymentRequest extends IdempotencyRequest {
  amount: number
  payment_date?: string
  note?: string
}

export interface ApplyInvoiceDiscountRequest extends IdempotencyRequest {
  billing_document_line_id: number
  amount: number
  reason: string
}

export interface ApplyInvoiceSurplusRequest extends IdempotencyRequest {
  billing_document_line_id: number
  amount: number
  note?: string
}

export interface GenerateInvoiceInterestRequest extends IdempotencyRequest {
  billing_document_line_id: number
  as_of_date?: string
}

export interface ApplyInvoiceDepositRequest extends IdempotencyRequest {
  billing_document_line_id: number
  target_document_id: number
  target_billing_document_line_id: number
  amount: number
  note?: string
}

export interface RefundInvoiceDepositRequest extends IdempotencyRequest {
  billing_document_line_id: number
  amount: number
  reason: string
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
export const getInvoiceReceivable = (id: number) => http.get<{ receivable: InvoiceReceivable }>(`/invoices/${id}/receivable`)
export const recordInvoicePayment = (id: number, data: RecordInvoicePaymentRequest) =>
  http.post<{ receivable: InvoiceReceivable }>(`/invoices/${id}/payments`, data)
export const applyInvoiceDiscount = (id: number, data: ApplyInvoiceDiscountRequest) =>
  http.post<{ receivable: InvoiceReceivable }>(`/invoices/${id}/discounts`, data)
export const applyInvoiceSurplus = (id: number, data: ApplyInvoiceSurplusRequest) =>
  http.post<{ receivable: InvoiceReceivable }>(`/invoices/${id}/surplus-applications`, data)
export const generateInvoiceInterest = (id: number, data: GenerateInvoiceInterestRequest) =>
  http.post<{ receivable: InvoiceReceivable }>(`/invoices/${id}/interest`, data)
export const applyInvoiceDeposit = (id: number, data: ApplyInvoiceDepositRequest) =>
  http.post<{ receivable: InvoiceReceivable }>(`/invoices/${id}/deposit-applications`, data)
export const refundInvoiceDeposit = (id: number, data: RefundInvoiceDepositRequest) =>
  http.post<{ receivable: InvoiceReceivable }>(`/invoices/${id}/deposit-refunds`, data)
export const listReceivables = (params?: ListReceivablesParams) =>
  http.get<PaginatedResponse<ReceivableListItem>>('/receivables', { params })
