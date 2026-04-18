import type { AxiosResponse } from 'axios'

import { http } from './http'

export interface DashboardSummary {
  activeLeases: number | null
  pendingLeaseApprovals: number | null
  pendingInvoiceApprovals: number | null
  openReceivables: number | null
  overdueReceivables: number | null
  pendingWorkflows: number | null
}

type ApiDashboardSummary = {
  active_leases: number
  pending_lease_approvals: number
  pending_invoice_approvals: number
  open_receivables: number
  overdue_receivables: number
  pending_workflows: number
}

type DashboardSummaryResponse = {
  summary: ApiDashboardSummary
}

const toCamelCase = (api: ApiDashboardSummary): DashboardSummary => ({
  activeLeases: api.active_leases,
  pendingLeaseApprovals: api.pending_lease_approvals,
  pendingInvoiceApprovals: api.pending_invoice_approvals,
  openReceivables: api.open_receivables,
  overdueReceivables: api.overdue_receivables,
  pendingWorkflows: api.pending_workflows,
})

export const getEmptyDashboardSummary = (): DashboardSummary => ({
  activeLeases: null,
  pendingLeaseApprovals: null,
  pendingInvoiceApprovals: null,
  openReceivables: null,
  overdueReceivables: null,
  pendingWorkflows: null,
})

export const getDashboardSummary = async (): Promise<DashboardSummary> => {
  const response: AxiosResponse<DashboardSummaryResponse> = await http.get<DashboardSummaryResponse>(
    '/dashboard/summary',
  )

  return toCamelCase(response.data.summary)
}
