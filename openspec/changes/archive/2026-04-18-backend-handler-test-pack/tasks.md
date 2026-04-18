## 1. Create Handler Tests

- [x] 1.1 Create `backend/internal/http/handlers/billing_test.go` — GenerateCharges + ListCharges validation tests
- [x] 1.2 Create `backend/internal/http/handlers/invoice_test.go` — Create, Get, Submit, Cancel, Adjust, RecordPayment, List, ListReceivables, GetReceivable validation tests
- [x] 1.3 Create `backend/internal/http/handlers/lease_test.go` — Create, Get, Submit, Terminate, Amend, List validation tests
- [x] 1.4 Create `backend/internal/http/handlers/reporting_test.go` — Query + Export validation tests
- [x] 1.5 Create `backend/internal/http/handlers/sales_test.go` — CreateDailySale, CreateTraffic, ListDailySales, ListTraffic validation tests
- [x] 1.6 Create `backend/internal/http/handlers/structure_test.go` — CRUD validation tests for all 6 entity types
- [x] 1.7 Create `backend/internal/http/handlers/taxexport_test.go` — UpsertRuleSet, ListRuleSets, ExportVoucherWorkbook validation tests
- [x] 1.8 Create `backend/internal/http/handlers/dashboard_test.go` — Summary endpoint test

## 2. Secondary Handler Tests

- [x] 2.1 Create `backend/internal/http/handlers/docoutput_test.go` — UpsertTemplate, ListTemplates, RenderHTML, RenderPDF validation tests
- [x] 2.2 Create `backend/internal/http/handlers/excelio_test.go` — Template download and import validation tests
- [x] 2.3 Create `backend/internal/http/handlers/masterdata_test.go` — CRUD validation tests for all entity types
- [x] 2.4 Create `backend/internal/http/handlers/baseinfo_test.go` — CRUD validation tests for all entity types

## 3. Verification

- [x] 3.1 `go test ./internal/http/handlers/` passes
- [x] 3.2 `go build ./...` passes
