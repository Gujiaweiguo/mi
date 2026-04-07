import { http } from './http'

export interface ImportDiagnostic {
  row: number
  field: string
  message: string
}

export interface SalesImportResult {
  imported_count: number
  diagnostics: ImportDiagnostic[]
}

export interface DailySale {
  id: number
  store_id: number
  unit_id: number
  sale_date: string
  sales_amount: number
  created_at: string
  updated_at: string
}

export interface CustomerTraffic {
  id: number
  store_id: number
  traffic_date: string
  inbound_count: number
  created_at: string
  updated_at: string
}

export type ListDailySalesParams = {
  store_id?: number
  unit_id?: number
  date_from?: string
  date_to?: string
}

export type CreateDailySalePayload = {
  store_id: number
  unit_id: number
  sale_date: string
  sales_amount: number
}

export type ListCustomerTrafficParams = {
  store_id?: number
  date_from?: string
  date_to?: string
}

export type CreateCustomerTrafficPayload = {
  store_id: number
  traffic_date: string
  inbound_count: number
}

const createWorkbookImportRequest = (path: string, file: File) => {
  const form = new FormData()
  form.append('file', file)

  return http.post<SalesImportResult>(path, form, {
    validateStatus: (status) => (status >= 200 && status < 300) || status === 400,
  })
}

export const listDailySales = (params?: ListDailySalesParams) =>
  http.get<{ daily_sales: DailySale[] }>('/sales/daily', { params })

export const createDailySale = (payload: CreateDailySalePayload) =>
  http.post<{ daily_sale: DailySale }>('/sales/daily', payload)

export const downloadDailySalesTemplate = () =>
  http.get('/excel/templates/daily-sales', { responseType: 'blob' })

export const importDailySalesWorkbook = (file: File) =>
  createWorkbookImportRequest('/excel/imports/daily-sales', file)

export const listCustomerTraffic = (params?: ListCustomerTrafficParams) =>
  http.get<{ customer_traffic: CustomerTraffic[] }>('/sales/traffic', { params })

export const createCustomerTraffic = (payload: CreateCustomerTrafficPayload) =>
  http.post<{ traffic: CustomerTraffic }>('/sales/traffic', payload)

export const downloadCustomerTrafficTemplate = () =>
  http.get('/excel/templates/customer-traffic', { responseType: 'blob' })

export const importCustomerTrafficWorkbook = (file: File) =>
  createWorkbookImportRequest('/excel/imports/customer-traffic', file)
