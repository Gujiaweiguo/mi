import { describe, expect, it } from 'vitest'

import { normalizeBaseUrl, parseNumber } from './env'

describe('env helpers', () => {
  it('normalizes trailing slashes', () => {
    expect(normalizeBaseUrl('/api/')).toBe('/api')
  })

  it('falls back to default api path for empty values', () => {
    expect(normalizeBaseUrl('   ')).toBe('/api')
  })

  it('parses positive finite numbers', () => {
    expect(parseNumber('2500', 1000)).toBe(2500)
  })

  it('falls back for invalid numeric input', () => {
    expect(parseNumber('invalid', 1000)).toBe(1000)
  })
})
