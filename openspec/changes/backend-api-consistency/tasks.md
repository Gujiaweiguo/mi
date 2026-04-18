## 1. Add sqlutil.InPlaceholders

- [ ] 1.1 Add `InPlaceholders(n int) string` to `backend/internal/sqlutil/helpers.go`
- [ ] 1.2 Verify `go build ./internal/sqlutil/` passes

## 2. Refactor billing/repository.go

- [ ] 2.1 Replace manual IN-clause placeholder loop with `sqlutil.InPlaceholders(len(ids))`
- [ ] 2.2 Verify `go build ./internal/billing/` passes

## 3. Refactor invoice/repository.go

- [ ] 3.1 Replace manual IN-clause placeholder loop with `sqlutil.InPlaceholders(len(chargeLineIDs))`
- [ ] 3.2 Verify `go build ./internal/invoice/` passes

## 4. Fix reporting handler response

- [ ] 4.1 Change `c.JSON(http.StatusOK, result)` to `c.JSON(http.StatusOK, gin.H{"report": result})` in `reporting.go`
- [ ] 4.2 Verify `go build ./internal/http/handlers/` passes

## 5. Verification

- [ ] 5.1 Run `go build ./...` from `backend/` — compiles cleanly
- [ ] 5.2 Run `go test ./...` from `backend/` — all tests pass
