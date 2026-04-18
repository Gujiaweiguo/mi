import { beforeEach, describe, expect, it, vi } from 'vitest'

import { getDashboardSummary, getEmptyDashboardSummary } from './dashboard'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
  },
}))

import { http } from './http'

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
    it('returns camelCase metrics from the backend summary response', async () => {
      vi.mocked(http.get).mockResolvedValue({
        data: {
          summary: {
            active_leases: 12,
            pending_lease_approvals: 3,
            pending_invoice_approvals: 4,
            open_receivables: 9,
            overdue_receivables: 2,
            pending_workflows: 5,
          },
        },
      } as never)

      const result = await getDashboardSummary()

      expect(http.get).toHaveBeenCalledWith('/dashboard/summary')
      expect(result).toEqual({
        activeLeases: 12,
        pendingLeaseApprovals: 3,
        pendingInvoiceApprovals: 4,
        openReceivables: 9,
        overdueReceivables: 2,
        pendingWorkflows: 5,
      })
    })

    it('throws when the summary request fails', async () => {
      const error = new Error('dashboard unavailable')
      vi.mocked(http.get).mockRejectedValue(error)

      await expect(getDashboardSummary()).rejects.toThrow('dashboard unavailable')
    })
  })
})
