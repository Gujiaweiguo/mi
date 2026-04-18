import { describe, expect, it } from 'vitest'

import { usePagination } from './usePagination'

describe('usePagination', () => {
  it('initialises with default page size 20', () => {
    const { page, pageSize, total, paginationParams } = usePagination()

    expect(page.value).toBe(1)
    expect(pageSize.value).toBe(20)
    expect(total.value).toBe(0)
    expect(paginationParams.value).toEqual({ page: 1, page_size: 20 })
  })

  it('accepts a custom default page size', () => {
    const { pageSize, paginationParams } = usePagination(50)

    expect(pageSize.value).toBe(50)
    expect(paginationParams.value).toEqual({ page: 1, page_size: 50 })
  })

  it('handlePageChange updates page and paginationParams', () => {
    const { page, paginationParams, handlePageChange } = usePagination()

    handlePageChange(3)

    expect(page.value).toBe(3)
    expect(paginationParams.value).toEqual({ page: 3, page_size: 20 })
  })

  it('handleSizeChange updates pageSize and resets page to 1', () => {
    const { page, pageSize, paginationParams, handleSizeChange } = usePagination()

    handleSizeChange(100)

    expect(pageSize.value).toBe(100)
    expect(page.value).toBe(1)
    expect(paginationParams.value).toEqual({ page: 1, page_size: 100 })
  })

  it('handleSizeChange resets page even after navigating', () => {
    const { page, pageSize, handlePageChange, handleSizeChange } = usePagination()

    handlePageChange(5)
    expect(page.value).toBe(5)

    handleSizeChange(50)
    expect(pageSize.value).toBe(50)
    expect(page.value).toBe(1)
  })

  it('resetPage sets page back to 1', () => {
    const { page, resetPage, handlePageChange } = usePagination()

    handlePageChange(4)
    expect(page.value).toBe(4)

    resetPage()
    expect(page.value).toBe(1)
  })
})
