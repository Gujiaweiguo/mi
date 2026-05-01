import { beforeEach, describe, expect, it, vi } from 'vitest'

import { listNotifications } from './notification'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
  },
}))

import { http } from './http'

describe('notification api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listNotifications', () => {
    it('calls GET /notifications without params and returns the response', async () => {
      const response = {
        data: {
          items: [],
          total: 0,
        },
      } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listNotifications()

      expect(http.get).toHaveBeenCalledWith('/notifications', { params: undefined })
      expect(result).toEqual(response)
    })

    it('calls GET /notifications with params and returns the response', async () => {
      const params = {
        page: 2,
        page_size: 10,
        event_type: 'lease.submitted',
        status: 'pending',
      }
      const response = {
        data: {
          items: [
            {
              id: 1,
              event_type: 'lease.submitted',
              aggregate_type: 'lease',
              aggregate_id: 42,
              recipient_to: 'admin@example.com',
              recipient_cc: '',
              subject: 'Lease Submitted',
              template_name: 'lease_submitted',
              status: 'pending',
              attempt_count: 0,
              max_attempts: 3,
              sent_at: null,
              last_error: null,
              created_at: '2026-01-01T00:00:00Z',
              updated_at: '2026-01-01T00:00:00Z',
            },
          ],
          total: 1,
        },
      } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listNotifications(params)

      expect(http.get).toHaveBeenCalledWith('/notifications', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listNotifications({ status: 'failed' })).rejects.toThrow('fail')
    })
  })
})
