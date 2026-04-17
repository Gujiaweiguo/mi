import { http } from './http'
import type { PaginatedResponse } from './types'

export interface PrintTemplate {
  id: number
  code: string
  name: string
  document_type: string
  output_mode: string
  status: string
  title: string
  subtitle: string
  header_lines: string[]
  footer_lines: string[]
  created_by: number
  updated_by: number
  created_at: string
  updated_at: string
}

export interface UpsertPrintTemplateRequest {
  code: string
  name: string
  document_type: string
  output_mode: string
  title: string
  subtitle: string
  header_lines: string[]
  footer_lines: string[]
}

export const listPrintTemplates = (params?: { page?: number; page_size?: number }) =>
  http.get<PaginatedResponse<PrintTemplate>>('/print/templates', { params })
export const upsertPrintTemplate = (data: UpsertPrintTemplateRequest) => http.post('/print/templates', data)
export const renderPrintHtml = (data: { template_code: string; document_ids: number[] }) =>
  http.post('/print/render/html', data, { responseType: 'text' })
export const renderPrintPdf = (data: { template_code: string; document_ids: number[] }) =>
  http.post('/print/render/pdf', data, { responseType: 'blob' })
