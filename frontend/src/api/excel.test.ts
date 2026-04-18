import { beforeEach, describe, expect, it, vi } from 'vitest'

import { downloadUnitTemplate, exportOperational, importUnits } from './excel'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('excel api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('downloadUnitTemplate', () => {
    it('calls GET /excel/templates/unit-data with blob response type and returns the response', async () => {
      const response = { data: new Blob(['template']) } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await downloadUnitTemplate()

      expect(http.get).toHaveBeenCalledWith('/excel/templates/unit-data', { responseType: 'blob' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(downloadUnitTemplate()).rejects.toThrow('fail')
    })
  })

  describe('importUnits', () => {
    it('calls POST /excel/imports/unit-data with form data and returns the response', async () => {
      const file = new File(['unit'], 'units.xlsx')
      const response = { data: { imported_count: 1, diagnostics: [] } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await importUnits(file)

      expect(http.post).toHaveBeenCalledWith('/excel/imports/unit-data', expect.any(FormData))
      expect(vi.mocked(http.post).mock.calls[0]?.[1]).toBeInstanceOf(FormData)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(importUnits(new File(['unit'], 'units.xlsx'))).rejects.toThrow('fail')
    })
  })

  describe('exportOperational', () => {
    it('calls GET /excel/exports/operational with params and blob response type and returns the response', async () => {
      const response = { data: new Blob(['export']) } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await exportOperational('units')

      expect(http.get).toHaveBeenCalledWith('/excel/exports/operational', {
        params: { dataset: 'units' },
        responseType: 'blob',
      })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(exportOperational('units')).rejects.toThrow('fail')
    })
  })
})
