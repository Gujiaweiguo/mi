import { describe, expect, it } from 'vitest'

import { getErrorMessage } from './useErrorMessage'

describe('getErrorMessage', () => {
  it('returns the message from an Error instance', () => {
    expect(getErrorMessage(new Error('boom'), 'fallback')).toBe('boom')
  })

  it('returns fallback for a string input', () => {
    expect(getErrorMessage('something broke', 'fallback')).toBe('fallback')
  })

  it('returns fallback for an object input', () => {
    expect(getErrorMessage({ code: 500 }, 'fallback')).toBe('fallback')
  })

  it('returns fallback for null', () => {
    expect(getErrorMessage(null, 'fallback')).toBe('fallback')
  })

  it('returns fallback for undefined', () => {
    expect(getErrorMessage(undefined, 'fallback')).toBe('fallback')
  })
})
