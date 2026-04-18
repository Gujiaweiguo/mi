## Design

### 1. getErrorMessage adoption

For each of the 15 views that still use the inline ternary, replace:
```typescript
// OLD (inline):
description: error instanceof Error ? error.message : t('some.fallback')
```
With:
```typescript
// NEW (imported):
import { getErrorMessage } from '../composables/useErrorMessage'
// ...
description: getErrorMessage(error, t('some.fallback'))
```

This is a mechanical find-and-replace. No logic changes.

### 2. downloadBlob extraction

Create `frontend/src/composables/useDownload.ts`:
```typescript
export function downloadBlob(blob: Blob, filename: string): void {
  const url = URL.createObjectURL(blob)
  const a = document.createElement('a')
  a.href = url
  a.download = filename
  a.click()
  URL.revokeObjectURL(url)
}
```

Replace all inline download functions with this import.
