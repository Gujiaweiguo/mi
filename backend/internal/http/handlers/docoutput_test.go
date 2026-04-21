package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestDocOutputUpsertTemplateRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/templates", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpsertTemplate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputUpsertTemplateRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/templates", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpsertTemplate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputUpsertTemplateRejectsMissingCode(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/templates", bytes.NewBufferString(`{"name":"n","document_type":"dt","output_mode":"om","title":"t"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpsertTemplate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputUpsertTemplateRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/templates", bytes.NewBufferString(`{"code":"c","name":"n","document_type":"dt","output_mode":"om","title":"t"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpsertTemplate(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

func TestDocOutputListTemplatesRejectsInvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/docoutput/templates?page=bad", nil)

	handler.ListTemplates(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputListTemplatesRejectsInvalidPageSize(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/docoutput/templates?page_size=bad", nil)

	handler.ListTemplates(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputRenderHTMLRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/render/html", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RenderHTML(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputRenderHTMLRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/render/html", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RenderHTML(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputRenderHTMLRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/render/html", bytes.NewBufferString(`{"template_code":"c","document_ids":[1]}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RenderHTML(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

func TestDocOutputRenderPDFRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/render/pdf", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RenderPDF(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputRenderPDFRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/render/pdf", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RenderPDF(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestDocOutputRenderPDFRejectsMissingTemplateCode(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewDocOutputHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/docoutput/render/pdf", bytes.NewBufferString(`{"document_ids":[1]}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.RenderPDF(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
