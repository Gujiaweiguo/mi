package handlers

import (
	"errors"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type InvoiceHandler struct{ service *invoice.Service }

func NewInvoiceHandler(service *invoice.Service) *InvoiceHandler {
	return &InvoiceHandler{service: service}
}

type createInvoiceRequest struct {
	DocumentType         invoice.DocumentType `json:"document_type" binding:"required"`
	BillingChargeLineIDs []int64              `json:"billing_charge_line_ids" binding:"required"`
}

type submitInvoiceRequest struct {
	IdempotencyKey string `json:"idempotency_key" binding:"required"`
	Comment        string `json:"comment"`
}

type adjustInvoiceRequest struct {
	Lines []struct {
		BillingChargeLineID int64   `json:"billing_charge_line_id" binding:"required"`
		Amount              float64 `json:"amount" binding:"required"`
	} `json:"lines"`
}

type recordPaymentRequest struct {
	Amount         float64 `json:"amount" binding:"required"`
	PaymentDate    string  `json:"payment_date"`
	IdempotencyKey string  `json:"idempotency_key" binding:"required"`
	Note           string  `json:"note"`
}

func (h *InvoiceHandler) Create(c *gin.Context) {
	var request createInvoiceRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document create request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	document, err := h.service.CreateFromCharges(c.Request.Context(), invoice.CreateInput{DocumentType: request.DocumentType, BillingChargeLineIDs: request.BillingChargeLineIDs, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"document": document})
}

func (h *InvoiceHandler) Get(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	document, err := h.service.GetDocument(c.Request.Context(), documentID)
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"document": document})
}

func (h *InvoiceHandler) List(c *gin.Context) {
	filter := invoice.ListFilter{}
	if documentType := c.Query("document_type"); documentType != "" {
		dt := invoice.DocumentType(documentType)
		filter.DocumentType = &dt
	}
	if status := c.Query("status"); status != "" {
		st := invoice.Status(status)
		filter.Status = &st
	}
	if leaseIDText := c.Query("lease_contract_id"); leaseIDText != "" {
		leaseID, err := strconv.ParseInt(leaseIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease contract id"})
			return
		}
		filter.LeaseContractID = &leaseID
	}
	if runIDText := c.Query("billing_run_id"); runIDText != "" {
		runID, err := strconv.ParseInt(runIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing run id"})
			return
		}
		filter.BillingRunID = &runID
	}
	if pageText := c.DefaultQuery("page", strconv.Itoa(pagination.DefaultPage)); pageText != "" {
		page, err := strconv.Atoi(pageText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page"})
			return
		}
		filter.Page = page
	}
	if pageSizeText := c.DefaultQuery("page_size", strconv.Itoa(pagination.DefaultPageSize)); pageSizeText != "" {
		pageSize, err := strconv.Atoi(pageSizeText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page size"})
			return
		}
		filter.PageSize = pageSize
	}
	result, err := h.service.ListDocuments(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list billing documents"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[invoice.Document]{Items: []invoice.Document{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []invoice.Document{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

func (h *InvoiceHandler) Submit(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request submitInvoiceRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document submit request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	document, err := h.service.SubmitForApproval(c.Request.Context(), invoice.SubmitInput{DocumentID: documentID, ActorUserID: sessionUser.ID, DepartmentID: sessionUser.DepartmentID, IdempotencyKey: request.IdempotencyKey, Comment: request.Comment})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"document": document})
}

func (h *InvoiceHandler) Cancel(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	document, err := h.service.Cancel(c.Request.Context(), invoice.CancelInput{DocumentID: documentID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"document": document})
}

func (h *InvoiceHandler) Adjust(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request adjustInvoiceRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document adjust request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	lines := make([]invoice.AdjustLineInput, 0, len(request.Lines))
	for _, line := range request.Lines {
		lines = append(lines, invoice.AdjustLineInput{BillingChargeLineID: line.BillingChargeLineID, Amount: line.Amount})
	}
	document, err := h.service.Adjust(c.Request.Context(), invoice.AdjustInput{DocumentID: documentID, ActorUserID: sessionUser.ID, Lines: lines})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"document": document})
}

func (h *InvoiceHandler) RecordPayment(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request recordPaymentRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document payment request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	var paymentDate time.Time
	if request.PaymentDate != "" {
		parsed, err := time.Parse(invoice.DateLayout, request.PaymentDate)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid payment date"})
			return
		}
		paymentDate = parsed
	}
	receivable, err := h.service.RecordPayment(c.Request.Context(), invoice.RecordPaymentInput{DocumentID: documentID, ActorUserID: sessionUser.ID, Amount: request.Amount, PaymentDate: paymentDate, IdempotencyKey: request.IdempotencyKey, Note: request.Note})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

func (h *InvoiceHandler) GetReceivable(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	receivable, err := h.service.GetReceivable(c.Request.Context(), documentID)
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

func (h *InvoiceHandler) ListReceivables(c *gin.Context) {
	filter := invoice.ReceivableFilter{}
	if customerIDText := c.Query("customer_id"); customerIDText != "" {
		customerID, err := strconv.ParseInt(customerIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid customer id"})
			return
		}
		filter.CustomerID = &customerID
	}
	if departmentIDText := c.Query("department_id"); departmentIDText != "" {
		departmentID, err := strconv.ParseInt(departmentIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid department id"})
			return
		}
		filter.DepartmentID = &departmentID
	}
	if dueDateStart := c.Query("due_date_start"); dueDateStart != "" {
		parsed, err := time.Parse(invoice.DateLayout, dueDateStart)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid due date start"})
			return
		}
		filter.DueDateStart = &parsed
	}
	if dueDateEnd := c.Query("due_date_end"); dueDateEnd != "" {
		parsed, err := time.Parse(invoice.DateLayout, dueDateEnd)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid due date end"})
			return
		}
		filter.DueDateEnd = &parsed
	}
	if pageText := c.DefaultQuery("page", strconv.Itoa(pagination.DefaultPage)); pageText != "" {
		page, err := strconv.Atoi(pageText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page"})
			return
		}
		filter.Page = page
	}
	if pageSizeText := c.DefaultQuery("page_size", strconv.Itoa(pagination.DefaultPageSize)); pageSizeText != "" {
		pageSize, err := strconv.Atoi(pageSizeText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page size"})
			return
		}
		filter.PageSize = pageSize
	}
	result, err := h.service.ListReceivables(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list receivables"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[invoice.ReceivableListItem]{Items: []invoice.ReceivableListItem{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []invoice.ReceivableListItem{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

func (h *InvoiceHandler) renderInvoiceError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, invoice.ErrDocumentNotFound):
		status = http.StatusNotFound
	case errors.Is(err, invoice.ErrInvalidDocumentInput), errors.Is(err, invoice.ErrReceivableContextInvalid), errors.Is(err, invoice.ErrPaymentAmountInvalid), errors.Is(err, workflow.ErrDefinitionNotFound), errors.Is(err, workflow.ErrInvalidState):
		status = http.StatusBadRequest
	case errors.Is(err, invoice.ErrInvalidDocumentState), errors.Is(err, invoice.ErrDocumentAlreadySubmitted), errors.Is(err, invoice.ErrPaymentNotAllowed), errors.Is(err, invoice.ErrPaymentOverApplication), errors.Is(err, invoice.ErrDocumentHasRecordedPayments):
		status = http.StatusConflict
	}
	c.JSON(status, gin.H{"message": err.Error()})
}
