import { http } from './http'

export type ReportId =
  | 'r01'
  | 'r02'
  | 'r03'
  | 'r04'
  | 'r05'
  | 'r06'
  | 'r07'
  | 'r08'
  | 'r09'
  | 'r10'
  | 'r11'
  | 'r12'
  | 'r13'
  | 'r14'
  | 'r15'
  | 'r16'
  | 'r17'
  | 'r18'
  | 'r19'

export interface ReportQueryPayload {
  period?: string
  store_id?: number
  floor_id?: number
  area_id?: number
  unit_id?: number
  department_id?: number
  shop_type_id?: number
  customer_id?: number
  brand_id?: number
  trade_id?: number
  charge_type?: string
  management_type_id?: number
  status?: string
}

export interface ReportColumn {
  key: string
  label: string
}

export type ReportRowValue = string | number | boolean | null
export type ReportRow = Record<string, ReportRowValue>

export interface ReportQueryResponse<TRow extends ReportRow = ReportRow> {
  report_id: ReportId
  columns: ReportColumn[]
  rows: TRow[]
  generated_at: string
}

export interface VisualShopFloor {
  id: number
  name: string
  floor_plan_image_url?: string
}

export interface VisualShopUnit {
  unit_id: number
  unit_code: string
  unit_name: string
  floor_area: number | null
  rent_area: number | null
  rent_status: string
  brand_name: string | null
  customer_name: string | null
  shop_type_name: string | null
  pos_x: number
  pos_y: number
  color_hex: string | null
}

export interface VisualShopLegendItem {
  label: string
  color_hex: string | null
}

export interface VisualShopPayload {
  floor: VisualShopFloor | null
  units: VisualShopUnit[]
  legend: VisualShopLegendItem[]
}

export interface VisualShopReportResponse extends ReportQueryResponse {
  report_id: 'r19'
  visual: VisualShopPayload
}

export const queryReport = <TResponse extends ReportQueryResponse = ReportQueryResponse>(
  reportId: ReportId,
  payload: ReportQueryPayload,
) => http.post<TResponse>(`/reports/${reportId}/query`, payload)

export const exportReport = (reportId: ReportId, payload: ReportQueryPayload) =>
  http.post<Blob>(`/reports/${reportId}/export`, payload, { responseType: 'blob' })
