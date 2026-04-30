package handlers

import (
	"errors"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
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
	DocumentType         invoice.DocumentType `json:"document_type" binding:"required,oneof=invoice bill"`
	BillingChargeLineIDs []int64              `json:"billing_charge_line_ids" binding:"required,min=1,dive,gt=0"`
}

type submitInvoiceRequest struct {
	IdempotencyKey string `json:"idempotency_key" binding:"required,min=1,max=100"`
	Comment        string `json:"comment" binding:"omitempty,max=500"`
}

type adjustInvoiceRequest struct {
	Lines []struct {
		BillingChargeLineID int64   `json:"billing_charge_line_id" binding:"required,gt=0"`
		Amount              float64 `json:"amount" binding:"required,gt=0"`
	} `json:"lines" binding:"required,min=1,dive"`
}

type recordPaymentRequest struct {
	Amount         float64 `json:"amount" binding:"required,gt=0"`
	PaymentDate    string  `json:"payment_date"`
	IdempotencyKey string  `json:"idempotency_key" binding:"required,min=1,max=100"`
	Note           string  `json:"note" binding:"omitempty,max=500"`
}

type applyDiscountRequest struct {
	BillingDocumentLineID int64   `json:"billing_document_line_id" binding:"required,gt=0"`
	Amount                float64 `json:"amount" binding:"required,gt=0"`
	Reason                string  `json:"reason" binding:"required,min=1,max=255"`
	IdempotencyKey        string  `json:"idempotency_key" binding:"required,min=1,max=100"`
}

type applySurplusRequest struct {
	BillingDocumentLineID int64   `json:"billing_document_line_id" binding:"required,gt=0"`
	Amount                float64 `json:"amount" binding:"required,gt=0"`
	Note                  string  `json:"note" binding:"omitempty,max=255"`
	IdempotencyKey        string  `json:"idempotency_key" binding:"required,min=1,max=100"`
}

type generateInterestRequest struct {
	BillingDocumentLineID int64  `json:"billing_document_line_id" binding:"required,gt=0"`
	AsOfDate              string `json:"as_of_date"`
	IdempotencyKey        string `json:"idempotency_key" binding:"required,min=1,max=100"`
}

type applyDepositRequest struct {
	BillingDocumentLineID       int64   `json:"billing_document_line_id" binding:"required,gt=0"`
	TargetDocumentID            int64   `json:"target_document_id" binding:"required,gt=0"`
	TargetBillingDocumentLineID int64   `json:"target_billing_document_line_id" binding:"required,gt=0"`
	Amount                      float64 `json:"amount" binding:"required,gt=0"`
	Note                        string  `json:"note" binding:"omitempty,max=255"`
	IdempotencyKey              string  `json:"idempotency_key" binding:"required,min=1,max=100"`
}

type refundDepositRequest struct {
	BillingDocumentLineID int64   `json:"billing_document_line_id" binding:"required,gt=0"`
	Amount                float64 `json:"amount" binding:"required,gt=0"`
	Reason                string  `json:"reason" binding:"required,min=1,max=255"`
	IdempotencyKey        string  `json:"idempotency_key" binding:"required,min=1,max=100"`
}

// Create godoc
//
//	@Summary		Create billing document
//	@Description	Creates a bill or invoice from billing charge lines.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createInvoiceRequest	true	"Billing document create request"
//	@Success		201		{object}	swaggerEnvelope{document=invoice.Document}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices [post]
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

// Get godoc
//
//	@Summary		Get billing document
//	@Description	Returns a billing document with its line items by ID.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Billing document ID"
//	@Success		200	{object}	swaggerEnvelope{document=invoice.Document}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id} [get]
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

// List godoc
//
//	@Summary		List billing documents
//	@Description	Returns paginated billing documents filtered by document type, status, lease, and billing run.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			document_type		query		string	false	"Document type"
//	@Param			status				query		string	false	"Document status"
//	@Param			lease_contract_id	query		int		false	"Lease contract ID"
//	@Param			billing_run_id		query		int		false	"Billing run ID"
//	@Param			page				query		int		false	"Page number"
//	@Param			page_size			query		int		false	"Page size"
//	@Success		200					{object}	swaggerEnvelope{items=[]invoice.Document,total=int,page=int,page_size=int}
//	@Failure		400					{object}	swaggerMessageResponse
//	@Failure		401					{object}	swaggerMessageResponse
//	@Failure		500					{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices [get]
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

// Submit godoc
//
//	@Summary		Submit billing document
//	@Description	Submits a billing document into the workflow approval process.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Billing document ID"
//	@Param			request	body		submitInvoiceRequest	true	"Billing document submit request"
//	@Success		200		{object}	swaggerEnvelope{document=invoice.Document}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/submit [post]
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

// Cancel godoc
//
//	@Summary		Cancel billing document
//	@Description	Cancels a billing document that is still eligible for cancellation.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Billing document ID"
//	@Success		200	{object}	swaggerEnvelope{document=invoice.Document}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/cancel [post]
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

// Adjust godoc
//
//	@Summary		Adjust billing document
//	@Description	Creates an adjusted billing document with replacement line amounts.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Billing document ID"
//	@Param			request	body		adjustInvoiceRequest	true	"Billing document adjust request"
//	@Success		201		{object}	swaggerEnvelope{document=invoice.Document}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/adjust [post]
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

// RecordPayment godoc
//
//	@Summary		Record payment
//	@Description	Records a payment against a billing document receivable.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Billing document ID"
//	@Param			request	body		recordPaymentRequest	true	"Payment request"
//	@Success		200		{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/payments [post]
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

// ApplyDiscount godoc
//
//	@Summary		Request invoice discount
//	@Description	Creates an invoice discount request for approval and returns the current receivable summary.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Billing document ID"
//	@Param			request	body		applyDiscountRequest	true	"Discount request"
//	@Success		200		{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/discounts [post]
func (h *InvoiceHandler) ApplyDiscount(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request applyDiscountRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document discount request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	receivable, err := h.service.ApplyDiscount(c.Request.Context(), invoice.ApplyDiscountInput{DocumentID: documentID, BillingDocumentLineID: request.BillingDocumentLineID, Amount: request.Amount, Reason: request.Reason, ActorUserID: sessionUser.ID, DepartmentID: sessionUser.DepartmentID, IdempotencyKey: request.IdempotencyKey})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

// ApplySurplus godoc
//
//	@Summary		Apply customer surplus
//	@Description	Applies available customer surplus to a billing document receivable line.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Billing document ID"
//	@Param			request	body		applySurplusRequest	true	"Surplus application request"
//	@Success		200		{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/surplus-applications [post]
func (h *InvoiceHandler) ApplySurplus(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request applySurplusRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document surplus request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	receivable, err := h.service.ApplySurplus(c.Request.Context(), invoice.ApplySurplusInput{DocumentID: documentID, BillingDocumentLineID: request.BillingDocumentLineID, Amount: request.Amount, Note: request.Note, ActorUserID: sessionUser.ID, IdempotencyKey: request.IdempotencyKey})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

// GenerateInterest godoc
//
//	@Summary		Generate late-payment interest
//	@Description	Generates a late-payment interest invoice for an overdue receivable line.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Billing document ID"
//	@Param			request	body		generateInterestRequest	true	"Interest generation request"
//	@Success		200		{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/interest [post]
func (h *InvoiceHandler) GenerateInterest(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request generateInterestRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document interest request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	var asOfDate time.Time
	if request.AsOfDate != "" {
		parsed, err := time.Parse(invoice.DateLayout, request.AsOfDate)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid interest as-of date"})
			return
		}
		asOfDate = parsed
	}
	receivable, err := h.service.GenerateInterest(c.Request.Context(), invoice.GenerateInterestInput{DocumentID: documentID, BillingDocumentLineID: request.BillingDocumentLineID, AsOfDate: asOfDate, ActorUserID: sessionUser.ID, DepartmentID: sessionUser.DepartmentID, IdempotencyKey: request.IdempotencyKey})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

// ApplyDeposit godoc
//
//	@Summary		Apply deposit to receivable
//	@Description	Applies an available deposit from this billing document to an outstanding receivable on a target document.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Source billing document ID (contains the deposit)"
//	@Param			request	body		applyDepositRequest	true	"Deposit application request"
//	@Success		200		{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		409		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/deposit-applications [post]
func (h *InvoiceHandler) ApplyDeposit(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request applyDepositRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid deposit application request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	receivable, err := h.service.ApplyDeposit(c.Request.Context(), invoice.ApplyDepositInput{
		DocumentID:                  documentID,
		BillingDocumentLineID:       request.BillingDocumentLineID,
		TargetDocumentID:            request.TargetDocumentID,
		TargetBillingDocumentLineID: request.TargetBillingDocumentLineID,
		Amount:                      request.Amount,
		Note:                        request.Note,
		ActorUserID:                 sessionUser.ID,
		IdempotencyKey:              request.IdempotencyKey,
	})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

// RefundDeposit godoc
//
//	@Summary		Refund or release deposit
//	@Description	Refunds or releases a deposit if no outstanding obligations block the operation.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Billing document ID (contains the deposit)"
//	@Param			request	body		refundDepositRequest	true	"Deposit refund request"
//	@Success		200		{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		409		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/deposit-refunds [post]
func (h *InvoiceHandler) RefundDeposit(c *gin.Context) {
	documentID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing document id"})
		return
	}
	var request refundDepositRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid deposit refund request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	receivable, err := h.service.RefundDeposit(c.Request.Context(), invoice.RefundDepositInput{
		DocumentID:            documentID,
		BillingDocumentLineID: request.BillingDocumentLineID,
		Amount:                request.Amount,
		Reason:                request.Reason,
		ActorUserID:           sessionUser.ID,
		IdempotencyKey:        request.IdempotencyKey,
	})
	if err != nil {
		h.renderInvoiceError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"receivable": receivable})
}

// GetReceivable godoc
//
//	@Summary		Get receivable summary
//	@Description	Returns receivable status and payment history for a billing document.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Billing document ID"
//	@Success		200	{object}	swaggerEnvelope{receivable=invoice.ReceivableSummary}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/invoices/{id}/receivable [get]
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

// ListReceivables godoc
//
//	@Summary		List receivables
//	@Description	Returns paginated receivable summary records filtered by customer, department, lease, and due-date window.
//	@Tags			Invoice
//	@Accept			json
//	@Produce		json
//	@Param			customer_id			query		int		false	"Customer ID"
//	@Param			department_id		query		int		false	"Department ID"
//	@Param			lease_contract_id	query		int		false	"Lease contract ID"
//	@Param			due_date_start		query		string	false	"Due date start (YYYY-MM-DD)"
//	@Param			due_date_end		query		string	false	"Due date end (YYYY-MM-DD)"
//	@Param			page				query		int		false	"Page number"
//	@Param			page_size			query		int		false	"Page size"
//	@Success		200					{object}	swaggerEnvelope{items=[]invoice.ReceivableListItem,total=int,page=int,page_size=int}
//	@Failure		400					{object}	swaggerMessageResponse
//	@Failure		401					{object}	swaggerMessageResponse
//	@Failure		500					{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/receivables [get]
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
	if leaseIDText := c.Query("lease_contract_id"); leaseIDText != "" {
		leaseID, err := strconv.ParseInt(leaseIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease contract id"})
			return
		}
		filter.LeaseContractID = &leaseID
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
	case errors.Is(err, invoice.ErrInvalidDocumentInput), errors.Is(err, invoice.ErrReceivableContextInvalid), errors.Is(err, invoice.ErrPaymentAmountInvalid), errors.Is(err, invoice.ErrDiscountAmountInvalid), errors.Is(err, invoice.ErrDiscountReasonRequired), errors.Is(err, invoice.ErrSurplusAmountInvalid), errors.Is(err, invoice.ErrInterestNotConfigured), errors.Is(err, invoice.ErrInterestNotDue), errors.Is(err, invoice.ErrDepositAmountInvalid), errors.Is(err, invoice.ErrDepositRefundAmountInvalid), errors.Is(err, invoice.ErrDepositRefundReasonRequired), errors.Is(err, workflow.ErrDefinitionNotFound), errors.Is(err, workflow.ErrInvalidState):
		status = http.StatusBadRequest
	case errors.Is(err, invoice.ErrInvalidDocumentState), errors.Is(err, invoice.ErrDocumentAlreadySubmitted), errors.Is(err, invoice.ErrPaymentNotAllowed), errors.Is(err, invoice.ErrPaymentOverApplication), errors.Is(err, invoice.ErrDocumentHasRecordedPayments), errors.Is(err, invoice.ErrDiscountNotAllowed), errors.Is(err, invoice.ErrDiscountOverApplication), errors.Is(err, invoice.ErrDiscountPendingApproval), errors.Is(err, invoice.ErrSurplusNotAvailable), errors.Is(err, invoice.ErrSurplusInsufficient), errors.Is(err, invoice.ErrSurplusTargetNotAllowed), errors.Is(err, invoice.ErrDepositNotAvailable), errors.Is(err, invoice.ErrDepositTargetNotAllowed), errors.Is(err, invoice.ErrDepositRefundBlocked):
		status = http.StatusConflict
	}
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
