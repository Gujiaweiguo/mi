import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  getDashboardSummary,
  getEmptyDashboardSummary,
  getEmptyWorkbenchAggregate,
  getWorkbenchAggregate,
} from './dashboard'

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

  describe('getEmptyWorkbenchAggregate', () => {
    it('returns stable empty queue sections', () => {
      expect(getEmptyWorkbenchAggregate()).toEqual({
        pendingApprovals: { count: null, routeTarget: '/workflow/admin', previewRows: [] },
        receivables: { count: null, routeTarget: '/billing/receivables', previewRows: [] },
        overdueReceivables: { count: null, routeTarget: '/billing/receivables', previewRows: [] },
        activeLeases: { count: null, routeTarget: '/lease/contracts', previewRows: [] },
      })
    })
  })

  describe('getWorkbenchAggregate', () => {
    it('returns camelCase queue sections from the backend workbench response', async () => {
      vi.mocked(http.get).mockResolvedValue({
        data: {
          workbench: {
            pending_approvals: {
              count: 2,
              route_target: '/workflow/admin',
              preview_rows: [
                {
                  id: 11,
                  title: 'Lease LC-001',
                  subtitle: 'Tenant A',
                  status: 'pending_approval',
                  meta: '2026-04-29 09:30',
                  route_target: '/lease/contracts',
                },
              ],
            },
            receivables: {
              count: 3,
              route_target: '/billing/receivables',
              preview_rows: [],
            },
            overdue_receivables: {
              count: 1,
              route_target: '/billing/receivables',
              preview_rows: [],
            },
            active_leases: {
              count: 5,
              route_target: '/lease/contracts',
              preview_rows: [],
            },
          },
        },
      } as never)

      const result = await getWorkbenchAggregate()

      expect(http.get).toHaveBeenCalledWith('/dashboard/workbench')
      expect(result.pendingApprovals.count).toBe(2)
      expect(result.pendingApprovals.previewRows[0]).toEqual({
        id: 11,
        title: 'Lease LC-001',
        subtitle: 'Tenant A',
        status: 'pending_approval',
        meta: '2026-04-29 09:30',
        routeTarget: '/lease/contracts',
      })
      expect(result.receivables.routeTarget).toBe('/billing/receivables')
      expect(result.overdueReceivables.count).toBe(1)
      expect(result.activeLeases.count).toBe(5)
    })

    it('throws when the workbench request fails', async () => {
      const error = new Error('workbench unavailable')
      vi.mocked(http.get).mockRejectedValue(error)

      await expect(getWorkbenchAggregate()).rejects.toThrow('workbench unavailable')
    })
  })
})
