import { http } from './http'

export interface ImportDiagnostic {
  row: number
  field: string
  message: string
}

export interface ImportResult {
  imported_count: number
  diagnostics: ImportDiagnostic[]
}

export const downloadUnitTemplate = () => http.get('/excel/templates/unit-data', { responseType: 'blob' })
export const importUnits = (file: File) => {
  const form = new FormData()
  form.append('file', file)
  return http.post<ImportResult>('/excel/imports/unit-data', form)
}
export const exportOperational = (dataset: string) =>
  http.get('/excel/exports/operational', { params: { dataset }, responseType: 'blob' })
