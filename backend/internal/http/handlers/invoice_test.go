package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestInvoiceCreateRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceCreateRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceGetRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices/bad-id", nil)

	handler.Get(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceSubmitRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/submit", bytes.NewBufferString(`{"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Submit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceSubmitRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/submit", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Submit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceCancelRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/cancel", nil)

	handler.Cancel(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceAdjustRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/adjust", bytes.NewBufferString(`{"lines":[{"billing_charge_line_id":1,"amount":100}]}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Adjust(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceAdjustRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/adjust", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Adjust(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceRecordPaymentRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/payments", bytes.NewBufferString(`{"amount":100,"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RecordPayment(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceRecordPaymentRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/payments", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RecordPayment(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceApplyDiscountRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/discounts", bytes.NewBufferString(`{"billing_document_line_id":1,"amount":100,"reason":"promo","idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ApplyDiscount(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceApplyDiscountRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/discounts", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ApplyDiscount(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceApplySurplusRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/surplus-applications", bytes.NewBufferString(`{"billing_document_line_id":1,"amount":100,"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ApplySurplus(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceApplySurplusRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/surplus-applications", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ApplySurplus(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceGenerateInterestRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/interest", bytes.NewBufferString(`{"billing_document_line_id":1,"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.GenerateInterest(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceGenerateInterestRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/interest", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.GenerateInterest(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceApplyDepositRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/deposit-applications", bytes.NewBufferString(`{"billing_document_line_id":1,"target_document_id":2,"target_billing_document_line_id":3,"amount":100,"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ApplyDeposit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceApplyDepositRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/deposit-applications", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ApplyDeposit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceRefundDepositRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/bad-id/deposit-refunds", bytes.NewBufferString(`{"billing_document_line_id":1,"amount":100,"reason":"lease ended","idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RefundDeposit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceRefundDepositRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/invoices/1/deposit-refunds", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RefundDeposit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceGetReceivableRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices/bad-id/receivable", nil)

	handler.GetReceivable(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceListRejectsInvalidLeaseContractID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices?lease_contract_id=bad", nil)

	handler.List(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceListRejectsInvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices?page=bad", nil)

	handler.List(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceListReceivablesRejectsInvalidCustomerID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices/receivables?customer_id=bad", nil)

	handler.ListReceivables(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceListReceivablesRejectsInvalidDueDateStart(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices/receivables?due_date_start=bad", nil)

	handler.ListReceivables(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceListReceivablesRejectsInvalidDueDateEnd(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices/receivables?due_date_end=bad", nil)

	handler.ListReceivables(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestInvoiceListReceivablesRejectsInvalidLeaseContractID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewInvoiceHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/invoices/receivables?lease_contract_id=bad", nil)

	handler.ListReceivables(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
