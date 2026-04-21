package handlers

import (
	"errors"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/taxexport"
	"github.com/gin-gonic/gin"
)

type TaxExportHandler struct{ service *taxexport.Service }

func NewTaxExportHandler(service *taxexport.Service) *TaxExportHandler {
	return &TaxExportHandler{service: service}
}

type upsertTaxRuleSetRequest struct {
	Code         string `json:"code" binding:"required,min=1,max=50"`
	Name         string `json:"name" binding:"required,min=1,max=100"`
	DocumentType string `json:"document_type" binding:"required,oneof=invoice bill"`
	Rules        []struct {
		SequenceNo          int    `json:"sequence_no" binding:"required,min=1"`
		EntrySide           string `json:"entry_side" binding:"required,oneof=debit credit"`
		ChargeTypeFilter    string `json:"charge_type_filter" binding:"required,oneof=rent deposit utility other"`
		AccountNumber       string `json:"account_number" binding:"required,min=1,max=50"`
		AccountName         string `json:"account_name" binding:"required,min=1,max=100"`
		ExplanationTemplate string `json:"explanation_template" binding:"required,min=1,max=200"`
		UseTenantName       bool   `json:"use_tenant_name"`
		IsBalancingEntry    bool   `json:"is_balancing_entry"`
	} `json:"rules" binding:"required,min=1,dive"`
}

type exportVoucherRequest struct {
	RuleSetCode string `json:"rule_set_code" binding:"required,min=1,max=50"`
	FromDate    string `json:"from_date" binding:"required"`
	ToDate      string `json:"to_date" binding:"required"`
}

// UpsertRuleSet godoc
//
//	@Summary		Upsert tax rule set
//	@Description	Creates or updates a tax voucher rule set with its accounting rules.
//	@Tags			Tax
//	@Accept			json
//	@Produce		json
//	@Param			request	body		upsertTaxRuleSetRequest	true	"Tax rule set request"
//	@Success		201		{object}	swaggerEnvelope{rule_set=taxexport.RuleSet}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/tax/rule-sets [post]
func (h *TaxExportHandler) UpsertRuleSet(c *gin.Context) {
	var request upsertTaxRuleSetRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid tax voucher rule set request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	rules := make([]taxexport.UpsertRuleInput, 0, len(request.Rules))
	for _, rule := range request.Rules {
		rules = append(rules, taxexport.UpsertRuleInput{SequenceNo: rule.SequenceNo, EntrySide: taxexport.EntrySide(rule.EntrySide), ChargeTypeFilter: rule.ChargeTypeFilter, AccountNumber: rule.AccountNumber, AccountName: rule.AccountName, ExplanationTemplate: rule.ExplanationTemplate, UseTenantName: rule.UseTenantName, IsBalancingEntry: rule.IsBalancingEntry})
	}
	ruleSet, err := h.service.UpsertRuleSet(c.Request.Context(), taxexport.UpsertRuleSetInput{Code: request.Code, Name: request.Name, DocumentType: request.DocumentType, Status: taxexport.RuleSetStatusActive, Rules: rules, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderTaxExportError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"rule_set": ruleSet})
}

// ListRuleSets godoc
//
//	@Summary		List tax rule sets
//	@Description	Returns paginated tax voucher rule sets.
//	@Tags			Tax
//	@Accept			json
//	@Produce		json
//	@Param			page		query		int	false	"Page number"
//	@Param			page_size	query		int	false	"Page size"
//	@Success		200			{object}	swaggerEnvelope{items=[]taxexport.RuleSet,total=int,page=int,page_size=int}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/tax/rule-sets [get]
func (h *TaxExportHandler) ListRuleSets(c *gin.Context) {
	filter := taxexport.ListFilter{}
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
	result, err := h.service.ListRuleSets(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list tax voucher rule sets"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[taxexport.RuleSet]{Items: []taxexport.RuleSet{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []taxexport.RuleSet{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

// ExportVoucherWorkbook godoc
//
//	@Summary		Export voucher workbook
//	@Description	Exports an accounting voucher workbook for approved billing documents in the requested date range.
//	@Tags			Tax
//	@Accept			json
//	@Produce		application/octet-stream
//	@Param			request	body		exportVoucherRequest	true	"Tax export request"
//	@Success		200		{file}		file					"Voucher workbook"
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/tax/exports/vouchers [post]
func (h *TaxExportHandler) ExportVoucherWorkbook(c *gin.Context) {
	var request exportVoucherRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid tax export request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	fromDate, err := time.Parse(taxexport.DateLayout, request.FromDate)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid tax export from_date"})
		return
	}
	toDate, err := time.Parse(taxexport.DateLayout, request.ToDate)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid tax export to_date"})
		return
	}
	artifact, err := h.service.ExportVoucherWorkbook(c.Request.Context(), taxexport.ExportInput{RuleSetCode: request.RuleSetCode, FromDate: fromDate, ToDate: toDate, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderTaxExportError(c, err)
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Bytes)
}

func (h *TaxExportHandler) renderTaxExportError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, taxexport.ErrRuleSetNotFound):
		status = http.StatusNotFound
	case errors.Is(err, taxexport.ErrInvalidRuleSet), errors.Is(err, taxexport.ErrInvalidExportWindow), errors.Is(err, taxexport.ErrInvalidTaxSetup):
		status = http.StatusBadRequest
	}
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
