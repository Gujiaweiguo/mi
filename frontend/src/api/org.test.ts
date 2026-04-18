import { beforeEach, describe, expect, it, vi } from 'vitest'

import { listDepartments, listStores } from './org'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('org api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listDepartments', () => {
    it('calls GET /org/departments and returns the response', async () => {
      const response = { data: { departments: [{ id: 1, name: 'Operations' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listDepartments()

      expect(http.get).toHaveBeenCalledWith('/org/departments')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listDepartments()).rejects.toThrow('fail')
    })
  })

  describe('listStores', () => {
    it('calls GET /org/stores and returns the response', async () => {
      const response = { data: { stores: [{ id: 1, name: 'North Plaza' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStores()

      expect(http.get).toHaveBeenCalledWith('/org/stores')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStores()).rejects.toThrow('fail')
    })
  })
})
