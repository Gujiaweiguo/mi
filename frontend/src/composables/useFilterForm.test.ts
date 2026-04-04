import { describe, expect, it } from 'vitest'

import { useFilterForm } from './useFilterForm'

describe('useFilterForm', () => {
  it('tracks dirty state and resets to initial values', () => {
    const { filters, isDirty, reset } = useFilterForm({
      query: '',
      status: 'all',
    })

    expect(isDirty.value).toBe(false)

    filters.query = 'lease'
    expect(isDirty.value).toBe(true)

    reset()
    expect(filters.query).toBe('')
    expect(filters.status).toBe('all')
    expect(isDirty.value).toBe(false)
  })
})
