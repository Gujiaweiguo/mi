## Problem

Two minor backend inconsistencies:

1. **Reporting handler response format**: The reporting `Query` handler returns the raw `result` directly (`c.JSON(200, result)`), while every other handler wraps responses in `gin.H{"key": value}`. The frontend expects a consistent response shape.

2. **IN-clause placeholder builder duplication**: Both `billing/repository.go` and `invoice/repository.go` manually build `IN (?)` placeholder strings with the same loop pattern. The `sqlutil` package exists for shared SQL helpers but has no IN-clause builder.

## Proposal

1. Wrap the reporting handler response in `gin.H{"report": result}` for API consistency.
2. Add `sqlutil.InPlaceholders(n)` to the shared `sqlutil` package and refactor both call sites.

## Scope

- `backend/internal/http/handlers/reporting.go` — wrap response
- `backend/internal/sqlutil/helpers.go` — add InPlaceholders
- `backend/internal/billing/repository.go` — use sqlutil.InPlaceholders
- `backend/internal/invoice/repository.go` — use sqlutil.InPlaceholders
