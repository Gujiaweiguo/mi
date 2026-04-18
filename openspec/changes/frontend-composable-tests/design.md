## Design

### Scope

This change adds tests only. It does not modify production behavior unless tiny testability adjustments are needed to match existing frontend patterns.

### Target Test Areas

1. **Composable tests**
   - `usePagination.ts`
   - `useErrorMessage.ts`
   - `useDownload.ts`

2. **Dashboard aggregation helper tests**
   - `frontend/src/api/dashboard.ts`

### Coverage Goals

- `usePagination`
  - initial state
  - page and page-size updates
  - reset behavior if present
- `useErrorMessage`
  - `Error` input returns `.message`
  - non-Error input falls back to provided text
- `useDownload`
  - creates object URL
  - configures anchor correctly
  - triggers click
  - revokes object URL
- `dashboard.ts`
  - aggregates all successful endpoint counts correctly
  - records partial failures without crashing
  - returns empty/null metrics for failed requests

### Test Strategy

- Follow the repo's existing Vitest conventions and colocated `*.test.ts` patterns.
- Prefer direct unit tests with module mocks over browser-style tests.
- Mock API modules for `dashboard.ts` so tests stay deterministic and fast.
- Keep assertions focused on behavior, not implementation trivia.

### Verification

- Run targeted frontend tests
- Run full frontend test suite if available
- Run frontend build to ensure test additions do not affect production compilation
