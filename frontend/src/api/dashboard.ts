import { listInvoices, listReceivables } from './invoice'
import { listLeases } from './lease'
import { listWorkflowInstances } from './workflow'

export interface DashboardSummary {
  activeLeases: number | null
  pendingLeaseApprovals: number | null
  pendingInvoiceApprovals: number | null
  openReceivables: number | null
  overdueReceivables: number | null
  pendingWorkflows: number | null
}

export type DashboardSummaryMetric = keyof DashboardSummary

export interface DashboardSummaryResult {
  summary: DashboardSummary
  failedMetrics: DashboardSummaryMetric[]
  error?: unknown
}

type DashboardMetricResult = {
  key: DashboardSummaryMetric
  count: number | null
  error?: unknown
}

const createEmptyDashboardSummary = (): DashboardSummary => ({
  activeLeases: null,
  pendingLeaseApprovals: null,
  pendingInvoiceApprovals: null,
  openReceivables: null,
  overdueReceivables: null,
  pendingWorkflows: null,
})

const formatApiDate = (value: Date) => {
  const year = value.getFullYear()
  const month = String(value.getMonth() + 1).padStart(2, '0')
  const day = String(value.getDate()).padStart(2, '0')

  return `${year}-${month}-${day}`
}

export const getEmptyDashboardSummary = () => createEmptyDashboardSummary()

export const getDashboardSummary = async (): Promise<DashboardSummaryResult> => {
  const today = formatApiDate(new Date())

  const results = await Promise.all<DashboardMetricResult>([
    listLeases({ status: 'active', page_size: 1 })
      .then((response) => ({ key: 'activeLeases' as const, count: response.data.total }))
      .catch((error: unknown) => ({ key: 'activeLeases' as const, count: null, error })),
    listLeases({ status: 'pending_approval', page_size: 1 })
      .then((response) => ({ key: 'pendingLeaseApprovals' as const, count: response.data.total }))
      .catch((error: unknown) => ({ key: 'pendingLeaseApprovals' as const, count: null, error })),
    listInvoices({ status: 'pending_approval', page_size: 1 })
      .then((response) => ({ key: 'pendingInvoiceApprovals' as const, count: response.data.total }))
      .catch((error: unknown) => ({ key: 'pendingInvoiceApprovals' as const, count: null, error })),
    listReceivables({ page_size: 1 })
      .then((response) => ({ key: 'openReceivables' as const, count: response.data.total }))
      .catch((error: unknown) => ({ key: 'openReceivables' as const, count: null, error })),
    listReceivables({ due_date_end: today, page_size: 1 })
      .then((response) => ({ key: 'overdueReceivables' as const, count: response.data.total }))
      .catch((error: unknown) => ({ key: 'overdueReceivables' as const, count: null, error })),
    listWorkflowInstances({ status: 'pending' })
      .then((response) => ({ key: 'pendingWorkflows' as const, count: response.data.instances.length }))
      .catch((error: unknown) => ({ key: 'pendingWorkflows' as const, count: null, error })),
  ])

  const summary = createEmptyDashboardSummary()
  const failedMetrics: DashboardSummaryMetric[] = []
  let firstError: unknown = undefined

  results.forEach(({ key, count, error }) => {
    summary[key] = count

    if (count === null) {
      failedMetrics.push(key)

      if (firstError === undefined) {
        firstError = error
      }
    }
  })

  return {
    summary,
    failedMetrics,
    error: firstError,
  }
}
