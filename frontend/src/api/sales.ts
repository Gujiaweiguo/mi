import { http } from './http'

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

export const listDailySales = (params?: ListDailySalesParams) =>
  http.get<{ daily_sales: DailySale[] }>('/sales/daily', { params })

export const createDailySale = (payload: CreateDailySalePayload) =>
  http.post<{ daily_sale: DailySale }>('/sales/daily', payload)

export const listCustomerTraffic = (params?: ListCustomerTrafficParams) =>
  http.get<{ customer_traffic: CustomerTraffic[] }>('/sales/traffic', { params })

export const createCustomerTraffic = (payload: CreateCustomerTrafficPayload) =>
  http.post<{ traffic: CustomerTraffic }>('/sales/traffic', payload)
