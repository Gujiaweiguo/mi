package handlers

import (
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/docoutput"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/gin-gonic/gin"
)

type DocOutputHandler struct{ service *docoutput.Service }

func NewDocOutputHandler(service *docoutput.Service) *DocOutputHandler {
	return &DocOutputHandler{service: service}
}

type upsertPrintTemplateRequest struct {
	Code         string   `json:"code" binding:"required"`
	Name         string   `json:"name" binding:"required"`
	DocumentType string   `json:"document_type" binding:"required"`
	OutputMode   string   `json:"output_mode" binding:"required"`
	Title        string   `json:"title" binding:"required"`
	Subtitle     string   `json:"subtitle"`
	HeaderLines  []string `json:"header_lines"`
	FooterLines  []string `json:"footer_lines"`
}

type renderDocumentRequest struct {
	TemplateCode string  `json:"template_code" binding:"required"`
	DocumentIDs  []int64 `json:"document_ids" binding:"required"`
}

func (h *DocOutputHandler) UpsertTemplate(c *gin.Context) {
	var request upsertPrintTemplateRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid print template request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	templateValue, err := h.service.UpsertTemplate(c.Request.Context(), docoutput.UpsertTemplateInput{Code: request.Code, Name: request.Name, DocumentType: request.DocumentType, OutputMode: docoutput.OutputMode(request.OutputMode), Title: request.Title, Subtitle: request.Subtitle, HeaderLines: request.HeaderLines, FooterLines: request.FooterLines, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderDocOutputError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"template": templateValue})
}

func (h *DocOutputHandler) ListTemplates(c *gin.Context) {
	filter := docoutput.ListFilter{}
	if pageText := c.DefaultQuery("page", strconv.Itoa(docoutput.DefaultPage)); pageText != "" {
		page, err := strconv.Atoi(pageText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page"})
			return
		}
		filter.Page = page
	}
	if pageSizeText := c.DefaultQuery("page_size", strconv.Itoa(docoutput.DefaultPageSize)); pageSizeText != "" {
		pageSize, err := strconv.Atoi(pageSizeText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page size"})
			return
		}
		filter.PageSize = pageSize
	}
	result, err := h.service.ListTemplates(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list print templates"})
		return
	}
	if result == nil {
		result = &docoutput.ListResult{Items: []docoutput.Template{}, Total: 0, Page: docoutput.DefaultPage, PageSize: docoutput.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []docoutput.Template{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

func (h *DocOutputHandler) RenderHTML(c *gin.Context) {
	rendered := h.render(c, true)
	if rendered == nil {
		return
	}
	c.Data(http.StatusOK, rendered.ContentType, rendered.Body)
}

func (h *DocOutputHandler) RenderPDF(c *gin.Context) {
	rendered := h.render(c, false)
	if rendered == nil {
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+rendered.FileName)
	c.Data(http.StatusOK, rendered.ContentType, rendered.Body)
}

func (h *DocOutputHandler) render(c *gin.Context, html bool) *docoutput.Artifact {
	var request renderDocumentRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid document render request"})
		return nil
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return nil
	}
	input := docoutput.RenderInput{TemplateCode: request.TemplateCode, DocumentIDs: request.DocumentIDs, ActorUserID: sessionUser.ID}
	var (
		artifact *docoutput.Artifact
		err      error
	)
	if html {
		artifact, err = h.service.RenderHTML(c.Request.Context(), input)
	} else {
		artifact, err = h.service.RenderPDF(c.Request.Context(), input)
	}
	if err != nil {
		h.renderDocOutputError(c, err)
		return nil
	}
	return artifact
}

func (h *DocOutputHandler) renderDocOutputError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, docoutput.ErrTemplateNotFound):
		status = http.StatusNotFound
	case errors.Is(err, docoutput.ErrInvalidTemplate), errors.Is(err, docoutput.ErrInvalidRenderInput), errors.Is(err, docoutput.ErrChromeUnavailable):
		status = http.StatusBadRequest
	}
	c.JSON(status, gin.H{"message": err.Error()})
}
