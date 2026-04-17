## 1. Create shared test database helper

- [ ] 1.1 Create `backend/internal/platform/database/testdb.go` with `//go:build integration` build tag. Define `NewTestDB(t *testing.T, ctx context.Context) *sql.DB` that starts a MySQL 8.0 testcontainer (`mysql:8.0`, exposed port `3306/tcp`, env vars `MYSQL_DATABASE=mi_integration`, `MYSQL_USER=mi_user`, `MYSQL_PASSWORD=mi_password`, `MYSQL_ROOT_PASSWORD=mi_root_password`, wait for listening port with 3-minute timeout). Resolve host and mapped port, open `sql.DB` via `platformdb.Config.DSN()`, call `WaitForDatabase`, apply migrations via `platformdb.NewMigrator(db, os.DirFS("."), "migrations")`, run bootstrap via `platformdb.NewBootstrapRunner(db, bootstrap.All()...)`, register `t.Cleanup` for container termination and DB close, return `*sql.DB`.
- [ ] 1.2 Define `WaitForDatabase(ctx context.Context, db *sql.DB) error` in the same file: 30-second deadline, 500ms poll interval, 5-second ping timeout via `db.PingContext`.

## 2. Replace boilerplate in service-level integration test files

- [ ] 2.1 In `backend/internal/billing/service_integration_test.go`: remove `newBillingTestDB` and `waitForDatabase`, add import of the shared helper, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.2 In `backend/internal/lease/service_integration_test.go`: remove `newLeaseTestDB` and `waitForDatabase`, add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.3 In `backend/internal/invoice/service_integration_test.go`: remove `newInvoiceTestDB` and `waitForDatabase`, add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.4 In `backend/internal/workflow/service_integration_test.go`: remove `newWorkflowTestDB` and `waitForDatabase`, add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.5 In `backend/internal/docoutput/service_integration_test.go`: remove `newDocOutputTestDB` and `waitForDatabase`, add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.6 In `backend/internal/taxexport/service_integration_test.go`: remove `newTaxExportTestDB` and `waitForDatabase`, add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.7 In `backend/internal/excelio/service_integration_test.go`: remove `newExcelIOTestDB` and `waitForDatabase`, add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.
- [ ] 2.8 In `backend/internal/reporting/service_integration_test.go`: remove `newReportingTestDB` (and any `waitForDatabase` if present), add import, replace call sites with `platformdb.NewTestDB(t, ctx)`.

## 3. Replace boilerplate in HTTP-level and app-level integration test files

- [ ] 3.1 In `backend/internal/http/router_integration_test.go`: remove inline container setup, `waitForDatabase`, migration, and bootstrap blocks from each test function, replace with `platformdb.NewTestDB(t, ctx)`. Remove the file-local `waitForDatabase` function.
- [ ] 3.2 In `backend/internal/http/invoice_receivable_integration_test.go`: remove inline container setup, migration, and bootstrap blocks, replace with `platformdb.NewTestDB(t, ctx)`.
- [ ] 3.3 In `backend/internal/http/workflow_reminder_integration_test.go`: remove inline container setup, migration, and bootstrap blocks, replace with `platformdb.NewTestDB(t, ctx)`.
- [ ] 3.4 In `backend/internal/http/masterdata_closure_integration_test.go`: remove inline container setup, migration, and bootstrap blocks, replace with `platformdb.NewTestDB(t, ctx)`.
- [ ] 3.5 In `backend/internal/app/app_integration_test.go`: remove file-local `waitForDatabase` and any inline container setup, replace with `platformdb.NewTestDB(t, ctx)` or `platformdb.WaitForDatabase` as appropriate for the test's needs.

## 4. Verification

- [ ] 4.1 Run `go build ./...` in the backend and confirm no compilation errors.
- [ ] 4.2 Run `go test -tags=integration -count=1 ./...` in the backend and confirm all integration tests pass with the shared helper.
- [ ] 4.3 Confirm no `newBillingTestDB`, `newLeaseTestDB`, `newInvoiceTestDB`, `newWorkflowTestDB`, `newDocOutputTestDB`, `newTaxExportTestDB`, `newExcelIOTestDB`, `newReportingTestDB`, or file-local `waitForDatabase` copies remain in any integration test file.
