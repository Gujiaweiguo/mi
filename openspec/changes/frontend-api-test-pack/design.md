## Design

### Test Pattern

Follow existing `dashboard.test.ts` pattern:

```typescript
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { functionA, functionB } from './module'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('module helpers', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('functionA', () => {
    it('calls GET /correct-url and returns data', async () => {
      vi.mocked(http.get).mockResolvedValue({ data: { key: 'value' } } as never)
      const result = await functionA()
      expect(http.get).toHaveBeenCalledWith('/correct-url')
      expect(result).toEqual(expectedValue)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)
      await expect(functionA()).rejects.toThrow('fail')
    })
  })
})
```

### Per-Module Test Plan

| Module | Functions | Key Test Points |
|---|---|---|
| `lease` | 6 | CRUD URLs with :id interpolation, list params |
| `invoice` | 9 | CRUD + submit/cancel/adjust/payment, list receivables via `/receivables` |
| `billing` | 2 | Generate POST, list GET with params |
| `workflow` | 7 | List definitions/instances, approve/reject/resubmit POSTs |
| `reports` | 2 | POST with reportId in URL, Blob responseType for export |
| `tax` | 3 | List/upsert rule sets, Blob export |
| `masterdata` | 15 | 5 entity types × 3 CRUD ops, composite key PUT URLs |
| `print` | 4 | Template CRUD, text/Blob responseTypes |
| `excel` | 3 | Blob downloads, FormData upload, GET export with params |
| `org` | 2 | Simple list GETs |
| `baseinfo` | 21 | 7 catalog types × 3 CRUD ops |
| `structure` | 18 | 6 entity types × 3 CRUD ops, list params |
| `sales` | 8 | Daily sales + traffic CRUD, FormData imports with validateStatus |

### Verification

- `npm run test:unit -- src/api/` passes
- `npm run build` passes
