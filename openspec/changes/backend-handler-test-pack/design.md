## Design

### Test Pattern

Follow the existing pattern from `auth_test.go`, `health_test.go`, and `workflow_test.go`:

```go
func TestXxxRejectsInvalidPayload(t *testing.T) {
    gin.SetMode(gin.TestMode)
    handler := NewXxxHandler(nil) // nil service — test fails before service call

    recorder := httptest.NewRecorder()
    ctx, _ := gin.CreateTestContext(recorder)
    ctx.Request = httptest.NewRequest(http.MethodPost, "/api/...", bytes.NewBufferString(`{invalid}`))
    ctx.Request.Header.Set("Content-Type", "application/json")

    handler.Method(ctx)

    if recorder.Code != http.StatusBadRequest {
        t.Fatalf("expected 400, got %d", recorder.Code)
    }
}
```

### Test Categories Per Handler

**1. Invalid JSON / Missing Required Fields** (all handlers with POST/PUT endpoints)
- Send `{invalid json` → expect 400
- Send `{}` (missing required fields) → expect 400
- Send valid JSON but with empty required string → expect 400

**2. Invalid Route Parameters** (all handlers with `:id` routes)
- Set `ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}` → expect 400

**3. Invalid Query Parameters** (handlers with query-string filters)
- Send `?lease_contract_id=bad` → expect 400
- Send `?page=bad` → expect 400
- Send `?page_size=bad` → expect 400
- Send invalid date format `?period_start=not-a-date` → expect 400

**4. Dashboard Handler** (special case — no service mock needed)
- Verify `Summary` returns 500 when service is nil (will panic without nil check, so verify it handles gracefully)

### Per-Handler Test Plan

| Handler File | Tests |
|---|---|
| `billing_test.go` | GenerateCharges rejects invalid JSON, invalid period_start, invalid period_end; ListCharges rejects invalid lease_contract_id, invalid period_start, invalid page, invalid page_size |
| `invoice_test.go` | Create rejects invalid JSON; Get rejects invalid id; Submit rejects invalid id, invalid JSON; Cancel rejects invalid id; Adjust rejects invalid id, invalid JSON; RecordPayment rejects invalid id, invalid JSON; List rejects invalid lease_contract_id, invalid page; ListReceivables rejects invalid customer_id, invalid due_date_start; GetReceivable rejects invalid id |
| `lease_test.go` | Create rejects invalid JSON, invalid start_date, invalid end_date, invalid term dates; Get rejects invalid id; Submit rejects invalid id, invalid JSON; Terminate rejects invalid id, invalid JSON, invalid terminated_at; Amend rejects invalid id; List rejects invalid store_id, invalid page |
| `reporting_test.go` | Query rejects invalid JSON; Export rejects invalid JSON |
| `sales_test.go` | CreateDailySale rejects invalid JSON, invalid sale_date; CreateTraffic rejects invalid JSON, invalid traffic_date; ListDailySales rejects invalid store_id, invalid date_from; ListTraffic rejects invalid store_id, invalid date_from |
| `structure_test.go` | CreateStore rejects invalid JSON; UpdateStore rejects invalid id; CreateBuilding rejects invalid JSON; UpdateBuilding rejects invalid id; CreateFloor rejects invalid JSON; UpdateFloor rejects invalid id; CreateArea rejects invalid JSON; UpdateArea rejects invalid id; CreateLocation rejects invalid JSON; UpdateLocation rejects invalid id; CreateUnit rejects invalid JSON; UpdateUnit rejects invalid id |
| `taxexport_test.go` | UpsertRuleSet rejects invalid JSON; ListRuleSets rejects invalid page; ExportVoucherWorkbook rejects invalid JSON, invalid from_date, invalid to_date |
| `dashboard_test.go` | Summary returns 500 on nil service error |
| `docoutput_test.go` | UpsertTemplate rejects invalid JSON; ListTemplates rejects invalid page; RenderHTML rejects invalid JSON; RenderPDF rejects invalid JSON |
| `excelio_test.go` | DownloadUnitTemplate (GET, minimal test); DownloadDailySalesTemplate (GET, minimal test); DownloadTrafficTemplate (GET, minimal test); ImportUnits rejects invalid content-type; ExportOperationalDataset (GET, minimal test) |
| `masterdata_test.go` | ListCustomers, CreateCustomer rejects invalid JSON; UpdateCustomer rejects invalid id; ListBrands (GET, minimal test); CreateBrand rejects invalid JSON; UpdateBrand rejects invalid id; similar for remaining endpoints |
| `baseinfo_test.go` | ListStoreTypes (GET, minimal test); CreateStoreType rejects invalid JSON; UpdateStoreType rejects invalid id; similar for remaining endpoints |

### Verification

- `go test ./internal/http/handlers/` passes
- `go build ./...` passes
