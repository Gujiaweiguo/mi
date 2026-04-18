import { beforeEach, describe, expect, it, vi } from 'vitest'

import { getDashboardSummary, getEmptyDashboardSummary } from './dashboard'

vi.mock('./lease', () => ({
  listLeases: vi.fn(),
}))

vi.mock('./invoice', () => ({
  listInvoices: vi.fn(),
  listReceivables: vi.fn(),
}))

vi.mock('./workflow', () => ({
  listWorkflowInstances: vi.fn(),
}))

import { listInvoices, listReceivables } from './invoice'
import { listLeases } from './lease'
import { listWorkflowInstances } from './workflow'

const paged = (total: number) => ({ data: { total, items: [] as never[], page: 1, page_size: 1 } })
const workflowInstances = (n: number) => ({ data: { instances: Array(n).fill({ id: 1, status: 'pending' }) } })

describe('dashboard helpers', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('getEmptyDashboardSummary', () => {
    it('returns a summary with all metrics set to null', () => {
      const summary = getEmptyDashboardSummary()

      expect(summary).toEqual({
        activeLeases: null,
        pendingLeaseApprovals: null,
        pendingInvoiceApprovals: null,
        openReceivables: null,
        overdueReceivables: null,
        pendingWorkflows: null,
      })
    })
  })

  describe('getDashboardSummary', () => {
    it('aggregates all metrics on success', async () => {
      vi.mocked(listLeases)
        .mockResolvedValueOnce(paged(12) as never)   // active leases
        .mockResolvedValueOnce(paged(3) as never)    // pending lease approvals

      vi.mocked(listInvoices)
        .mockResolvedValueOnce(paged(4) as never)    // pending invoice approvals

      vi.mocked(listReceivables)
        .mockResolvedValueOnce(paged(9) as never)    // open receivables
        .mockResolvedValueOnce(paged(2) as never)    // overdue receivables

      vi.mocked(listWorkflowInstances)
        .mockResolvedValueOnce(workflowInstances(5) as never) // pending workflows

      const result = await getDashboardSummary()

      expect(result.summary).toEqual({
        activeLeases: 12,
        pendingLeaseApprovals: 3,
        pendingInvoiceApprovals: 4,
        openReceivables: 9,
        overdueReceivables: 2,
        pendingWorkflows: 5,
      })
      expect(result.failedMetrics).toEqual([])
      expect(result.error).toBeUndefined()
    })

    it('passes correct query parameters to API calls', async () => {
      vi.mocked(listLeases).mockResolvedValue(paged(0) as never)
      vi.mocked(listInvoices).mockResolvedValue(paged(0) as never)
      vi.mocked(listReceivables).mockResolvedValue(paged(0) as never)
      vi.mocked(listWorkflowInstances).mockResolvedValue(workflowInstances(0) as never)

      await getDashboardSummary()

      // Active leases
      expect(listLeases).toHaveBeenNthCalledWith(1, { status: 'active', page_size: 1 })
      // Pending lease approvals
      expect(listLeases).toHaveBeenNthCalledWith(2, { status: 'pending_approval', page_size: 1 })
      // Pending invoice approvals
      expect(listInvoices).toHaveBeenCalledWith({ status: 'pending_approval', page_size: 1 })
      // Open receivables
      expect(listReceivables).toHaveBeenNthCalledWith(1, { page_size: 1 })
      // Overdue receivables — has due_date_end with today's date and page_size
      expect(listReceivables).toHaveBeenNthCalledWith(2, expect.objectContaining({ page_size: 1 }))
      const overdueCall = vi.mocked(listReceivables).mock.calls[1][0]!
      expect(overdueCall.due_date_end).toMatch(/^\d{4}-\d{2}-\d{2}$/)
      // Pending workflows
      expect(listWorkflowInstances).toHaveBeenCalledWith({ status: 'pending' })
    })

    it('handles partial failures gracefully', async () => {
      const leaseError = new Error('lease service down')

      vi.mocked(listLeases)
        .mockRejectedValueOnce(leaseError)                    // active leases FAIL
        .mockResolvedValueOnce(paged(3) as never)             // pending lease approvals OK

      vi.mocked(listInvoices)
        .mockRejectedValueOnce(new Error('invoice down'))     // pending invoices FAIL

      vi.mocked(listReceivables)
        .mockResolvedValueOnce(paged(9) as never)             // open receivables OK
        .mockResolvedValueOnce(paged(2) as never)             // overdue receivables OK

      vi.mocked(listWorkflowInstances)
        .mockResolvedValueOnce(workflowInstances(5) as never) // pending workflows OK

      const result = await getDashboardSummary()

      // Successful metrics populated
      expect(result.summary.pendingLeaseApprovals).toBe(3)
      expect(result.summary.openReceivables).toBe(9)
      expect(result.summary.overdueReceivables).toBe(2)
      expect(result.summary.pendingWorkflows).toBe(5)

      // Failed metrics are null
      expect(result.summary.activeLeases).toBeNull()
      expect(result.summary.pendingInvoiceApprovals).toBeNull()

      // failedMetrics lists the right keys
      expect(result.failedMetrics).toContain('activeLeases')
      expect(result.failedMetrics).toContain('pendingInvoiceApprovals')
      expect(result.failedMetrics).not.toContain('pendingLeaseApprovals')

      // error is the first rejection captured
      expect(result.error).toBe(leaseError)
    })
  })
})
