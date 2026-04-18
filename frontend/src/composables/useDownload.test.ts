import { beforeEach, describe, expect, it, vi } from 'vitest'

import { downloadBlob } from './useDownload'

describe('downloadBlob', () => {
  let createObjectURLMock: ReturnType<typeof vi.fn>
  let revokeObjectURLMock: ReturnType<typeof vi.fn>

  beforeEach(() => {
    createObjectURLMock = vi.fn(() => 'blob:http://localhost/fake')
    revokeObjectURLMock = vi.fn()
    vi.spyOn(URL, 'createObjectURL' as never).mockImplementation(createObjectURLMock)
    vi.spyOn(URL, 'revokeObjectURL' as never).mockImplementation(revokeObjectURLMock)
  })

  it('creates an object URL from the blob', () => {
    const blob = new Blob(['data'], { type: 'text/plain' })
    downloadBlob(blob, 'file.txt')

    expect(createObjectURLMock).toHaveBeenCalledTimes(1)
    expect(createObjectURLMock).toHaveBeenCalledWith(blob)
  })

  it('creates an anchor element with correct href and download attributes and clicks it', () => {
    const clickSpy = vi.fn()
    const anchor = document.createElement('a')
    vi.spyOn(document, 'createElement').mockReturnValue(anchor)
    anchor.click = clickSpy

    downloadBlob(new Blob(['data']), 'report.xlsx')

    expect(anchor.href).toBe('blob:http://localhost/fake')
    expect(anchor.download).toBe('report.xlsx')
    expect(clickSpy).toHaveBeenCalledTimes(1)
  })

  it('revokes the object URL after clicking', () => {
    downloadBlob(new Blob(['data']), 'file.csv')

    expect(revokeObjectURLMock).toHaveBeenCalledTimes(1)
    expect(revokeObjectURLMock).toHaveBeenCalledWith('blob:http://localhost/fake')
  })
})
