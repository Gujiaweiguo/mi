import { beforeEach, describe, expect, it, vi } from 'vitest'

import { listPrintTemplates, renderPrintHtml, renderPrintPdf, upsertPrintTemplate } from './print'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('print api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listPrintTemplates', () => {
    it('calls GET /print/templates with params and returns the response', async () => {
      const params = { page: 1, page_size: 20 }
      const response = { data: { items: [{ id: 1 }], total: 1, page: 1, page_size: 20 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listPrintTemplates(params)

      expect(http.get).toHaveBeenCalledWith('/print/templates', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listPrintTemplates({ page: 2 })).rejects.toThrow('fail')
    })
  })

  describe('upsertPrintTemplate', () => {
    it('calls POST /print/templates and returns the response', async () => {
      const payload = {
        code: 'LEASE',
        name: 'Lease Template',
        document_type: 'lease',
        output_mode: 'pdf',
        title: 'Lease',
        subtitle: 'Contract',
        header_lines: ['Header'],
        footer_lines: ['Footer'],
      }
      const response = { data: { print_template: { id: 1, code: 'LEASE' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await upsertPrintTemplate(payload)

      expect(http.post).toHaveBeenCalledWith('/print/templates', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        upsertPrintTemplate({
          code: 'LEASE',
          name: 'Lease Template',
          document_type: 'lease',
          output_mode: 'pdf',
          title: 'Lease',
          subtitle: 'Contract',
          header_lines: [],
          footer_lines: [],
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('renderPrintHtml', () => {
    it('calls POST /print/render/html with text response type and returns the response', async () => {
      const payload = { template_code: 'LEASE', document_ids: [1, 2] }
      const response = { data: '<html>preview</html>' } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await renderPrintHtml(payload)

      expect(http.post).toHaveBeenCalledWith('/print/render/html', payload, { responseType: 'text' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(renderPrintHtml({ template_code: 'LEASE', document_ids: [1] })).rejects.toThrow('fail')
    })
  })

  describe('renderPrintPdf', () => {
    it('calls POST /print/render/pdf with blob response type and returns the response', async () => {
      const payload = { template_code: 'LEASE', document_ids: [1, 2] }
      const response = { data: new Blob(['pdf']) } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await renderPrintPdf(payload)

      expect(http.post).toHaveBeenCalledWith('/print/render/pdf', payload, { responseType: 'blob' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(renderPrintPdf({ template_code: 'LEASE', document_ids: [1] })).rejects.toThrow('fail')
    })
  })
})
