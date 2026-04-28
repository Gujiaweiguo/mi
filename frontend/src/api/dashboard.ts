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

export interface WorkbenchPreviewRow {
	id: number
	title: string
	subtitle: string
	status: string
	meta: string
	routeTarget: string
}

export interface WorkbenchQueueSection {
	count: number | null
	routeTarget: string
	previewRows: WorkbenchPreviewRow[]
}

export interface WorkbenchAggregate {
	pendingApprovals: WorkbenchQueueSection
	receivables: WorkbenchQueueSection
	overdueReceivables: WorkbenchQueueSection
	activeLeases: WorkbenchQueueSection
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

type ApiWorkbenchPreviewRow = {
	id: number
	title: string
	subtitle: string
	status: string
	meta: string
	route_target: string
}

type ApiWorkbenchQueueSection = {
	count: number
	route_target: string
	preview_rows: ApiWorkbenchPreviewRow[]
}

type ApiWorkbenchAggregate = {
	pending_approvals: ApiWorkbenchQueueSection
	receivables: ApiWorkbenchQueueSection
	overdue_receivables: ApiWorkbenchQueueSection
	active_leases: ApiWorkbenchQueueSection
}

type WorkbenchAggregateResponse = {
	workbench: ApiWorkbenchAggregate
}

const toCamelCase = (api: ApiDashboardSummary): DashboardSummary => ({
  activeLeases: api.active_leases,
  pendingLeaseApprovals: api.pending_lease_approvals,
  pendingInvoiceApprovals: api.pending_invoice_approvals,
  openReceivables: api.open_receivables,
  overdueReceivables: api.overdue_receivables,
  pendingWorkflows: api.pending_workflows,
})

const toWorkbenchPreviewRow = (api: ApiWorkbenchPreviewRow): WorkbenchPreviewRow => ({
	id: api.id,
	title: api.title,
	subtitle: api.subtitle,
	status: api.status,
	meta: api.meta,
	routeTarget: api.route_target,
})

const toWorkbenchQueueSection = (api: ApiWorkbenchQueueSection): WorkbenchQueueSection => ({
	count: api.count,
	routeTarget: api.route_target,
	previewRows: api.preview_rows.map(toWorkbenchPreviewRow),
})

const toWorkbenchAggregate = (api: ApiWorkbenchAggregate): WorkbenchAggregate => ({
	pendingApprovals: toWorkbenchQueueSection(api.pending_approvals),
	receivables: toWorkbenchQueueSection(api.receivables),
	overdueReceivables: toWorkbenchQueueSection(api.overdue_receivables),
	activeLeases: toWorkbenchQueueSection(api.active_leases),
})

export const getEmptyDashboardSummary = (): DashboardSummary => ({
  activeLeases: null,
  pendingLeaseApprovals: null,
  pendingInvoiceApprovals: null,
  openReceivables: null,
  overdueReceivables: null,
  pendingWorkflows: null,
})

const getEmptyWorkbenchQueueSection = (routeTarget: string): WorkbenchQueueSection => ({
	count: null,
	routeTarget,
	previewRows: [],
})

export const getEmptyWorkbenchAggregate = (): WorkbenchAggregate => ({
	pendingApprovals: getEmptyWorkbenchQueueSection('/workflow/admin'),
	receivables: getEmptyWorkbenchQueueSection('/billing/receivables'),
	overdueReceivables: getEmptyWorkbenchQueueSection('/billing/receivables'),
	activeLeases: getEmptyWorkbenchQueueSection('/lease/contracts'),
})

export const getDashboardSummary = async (): Promise<DashboardSummary> => {
  const response: AxiosResponse<DashboardSummaryResponse> = await http.get<DashboardSummaryResponse>(
    '/dashboard/summary',
  )

  return toCamelCase(response.data.summary)
}

export const getWorkbenchAggregate = async (): Promise<WorkbenchAggregate> => {
	const response: AxiosResponse<WorkbenchAggregateResponse> = await http.get<WorkbenchAggregateResponse>(
		'/dashboard/workbench',
	)

	return toWorkbenchAggregate(response.data.workbench)
}
