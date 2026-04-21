package handlers

import (
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/docoutput"
	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
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

// UpsertTemplate godoc
//
//	@Summary		Upsert print template
//	@Description	Creates or updates a print template used for HTML and PDF document rendering.
//	@Tags			Print
//	@Accept			json
//	@Produce		json
//	@Param			request	body		upsertPrintTemplateRequest	true	"Template request"
//	@Success		201		{object}	swaggerEnvelope{template=docoutput.Template}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/print/templates [post]
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

// ListTemplates godoc
//
//	@Summary		List print templates
//	@Description	Returns paginated print templates available for document output.
//	@Tags			Print
//	@Accept			json
//	@Produce		json
//	@Param			page		query		int	false	"Page number"
//	@Param			page_size	query		int	false	"Page size"
//	@Success		200			{object}	swaggerEnvelope{items=[]docoutput.Template,total=int,page=int,page_size=int}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/print/templates [get]
func (h *DocOutputHandler) ListTemplates(c *gin.Context) {
	filter := docoutput.ListFilter{}
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
	result, err := h.service.ListTemplates(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list print templates"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[docoutput.Template]{Items: []docoutput.Template{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []docoutput.Template{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

// RenderHTML godoc
//
//	@Summary		Render HTML document
//	@Description	Renders the requested documents into HTML using the selected print template.
//	@Tags			Print
//	@Accept			json
//	@Produce		text/html
//	@Param			request	body		renderDocumentRequest	true	"Render request"
//	@Success		200		{string}	string					"Rendered HTML"
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/print/render/html [post]
func (h *DocOutputHandler) RenderHTML(c *gin.Context) {
	rendered := h.render(c, true)
	if rendered == nil {
		return
	}
	c.Data(http.StatusOK, rendered.ContentType, rendered.Body)
}

// RenderPDF godoc
//
//	@Summary		Render PDF document
//	@Description	Renders the requested documents into a downloadable PDF using the selected print template.
//	@Tags			Print
//	@Accept			json
//	@Produce		application/octet-stream
//	@Param			request	body		renderDocumentRequest	true	"Render request"
//	@Success		200		{file}		file					"Rendered PDF"
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/print/render/pdf [post]
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
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
