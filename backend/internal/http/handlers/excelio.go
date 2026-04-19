package handlers

import (
	"errors"
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/excelio"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/gin-gonic/gin"
)

type ExcelIOHandler struct{ service *excelio.Service }

func NewExcelIOHandler(service *excelio.Service) *ExcelIOHandler {
	return &ExcelIOHandler{service: service}
}

// DownloadUnitTemplate godoc
//
//	@Summary		Download unit import template
//	@Description	Downloads the Excel template used for unit master data imports.
//	@Tags			Excel
//	@Accept			json
//	@Produce		application/octet-stream
//	@Success		200	{file}		file	"Unit template workbook"
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/templates/unit-data [get]
func (h *ExcelIOHandler) DownloadUnitTemplate(c *gin.Context) {
	artifact, err := h.service.DownloadUnitTemplate(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to generate unit template"})
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Body)
}

// DownloadDailySalesTemplate godoc
//
//	@Summary		Download daily sales template
//	@Description	Downloads the Excel template used for daily sales imports.
//	@Tags			Excel
//	@Accept			json
//	@Produce		application/octet-stream
//	@Success		200	{file}		file	"Daily sales template workbook"
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/templates/daily-sales [get]
func (h *ExcelIOHandler) DownloadDailySalesTemplate(c *gin.Context) {
	artifact, err := h.service.DownloadDailySalesTemplate(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to generate daily sales template"})
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Body)
}

// DownloadTrafficTemplate godoc
//
//	@Summary		Download traffic template
//	@Description	Downloads the Excel template used for customer traffic imports.
//	@Tags			Excel
//	@Accept			json
//	@Produce		application/octet-stream
//	@Success		200	{file}		file	"Customer traffic template workbook"
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/templates/customer-traffic [get]
func (h *ExcelIOHandler) DownloadTrafficTemplate(c *gin.Context) {
	artifact, err := h.service.DownloadTrafficTemplate(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to generate customer traffic template"})
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Body)
}

// ImportUnits godoc
//
//	@Summary		Import units from Excel
//	@Description	Imports unit master data from an uploaded Excel workbook.
//	@Tags			Excel
//	@Accept			multipart/form-data
//	@Produce		json
//	@Param			file	formData	file	true	"Unit import workbook"
//	@Success		200		{object}	excelio.ImportResult
//	@Failure		400		{object}	excelio.ImportResult
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/imports/unit-data [post]
func (h *ExcelIOHandler) ImportUnits(c *gin.Context) {
	if _, ok := middleware.CurrentSessionUser(c); !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	file, _, err := c.Request.FormFile("file")
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "missing unit import file"})
		return
	}
	defer file.Close()
	result, importErr := h.service.ImportUnits(c.Request.Context(), file)
	if importErr != nil {
		if errors.Is(importErr, excelio.ErrInvalidImport) {
			c.JSON(http.StatusBadRequest, gin.H{"imported_count": result.ImportedCount, "diagnostics": result.Diagnostics})
			return
		}
		c.JSON(http.StatusInternalServerError, gin.H{"message": importErr.Error()})
		return
	}
	c.JSON(http.StatusOK, gin.H{"imported_count": result.ImportedCount, "diagnostics": result.Diagnostics})
}

// ImportDailySales godoc
//
//	@Summary		Import daily sales from Excel
//	@Description	Imports daily shop sales records from an uploaded Excel workbook.
//	@Tags			Excel
//	@Accept			multipart/form-data
//	@Produce		json
//	@Param			file	formData	file	true	"Daily sales import workbook"
//	@Success		200		{object}	excelio.ImportResult
//	@Failure		400		{object}	excelio.ImportResult
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/imports/daily-sales [post]
func (h *ExcelIOHandler) ImportDailySales(c *gin.Context) {
	if _, ok := middleware.CurrentSessionUser(c); !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	file, _, err := c.Request.FormFile("file")
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "missing daily sales import file"})
		return
	}
	defer file.Close()
	result, importErr := h.service.ImportDailySales(c.Request.Context(), file)
	if importErr != nil {
		if errors.Is(importErr, excelio.ErrInvalidImport) {
			c.JSON(http.StatusBadRequest, gin.H{"imported_count": result.ImportedCount, "diagnostics": result.Diagnostics})
			return
		}
		c.JSON(http.StatusInternalServerError, gin.H{"message": importErr.Error()})
		return
	}
	c.JSON(http.StatusOK, gin.H{"imported_count": result.ImportedCount, "diagnostics": result.Diagnostics})
}

// ImportTraffic godoc
//
//	@Summary		Import customer traffic from Excel
//	@Description	Imports customer traffic records from an uploaded Excel workbook.
//	@Tags			Excel
//	@Accept			multipart/form-data
//	@Produce		json
//	@Param			file	formData	file	true	"Customer traffic import workbook"
//	@Success		200		{object}	excelio.ImportResult
//	@Failure		400		{object}	excelio.ImportResult
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/imports/customer-traffic [post]
func (h *ExcelIOHandler) ImportTraffic(c *gin.Context) {
	if _, ok := middleware.CurrentSessionUser(c); !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	file, _, err := c.Request.FormFile("file")
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "missing customer traffic import file"})
		return
	}
	defer file.Close()
	result, importErr := h.service.ImportTraffic(c.Request.Context(), file)
	if importErr != nil {
		if errors.Is(importErr, excelio.ErrInvalidImport) {
			c.JSON(http.StatusBadRequest, gin.H{"imported_count": result.ImportedCount, "diagnostics": result.Diagnostics})
			return
		}
		c.JSON(http.StatusInternalServerError, gin.H{"message": importErr.Error()})
		return
	}
	c.JSON(http.StatusOK, gin.H{"imported_count": result.ImportedCount, "diagnostics": result.Diagnostics})
}

// ExportOperationalDataset godoc
//
//	@Summary		Export operational dataset
//	@Description	Exports an operational dataset workbook for the requested dataset code.
//	@Tags			Excel
//	@Accept			json
//	@Produce		application/octet-stream
//	@Param			dataset	query		string	true	"Dataset identifier"
//	@Success		200		{file}		file	"Operational dataset workbook"
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/excel/exports/operational [get]
func (h *ExcelIOHandler) ExportOperationalDataset(c *gin.Context) {
	artifact, err := h.service.ExportOperationalDataset(c.Request.Context(), excelio.ExportInput{Dataset: c.Query("dataset")})
	if err != nil {
		status := http.StatusInternalServerError
		if errors.Is(err, excelio.ErrInvalidDataset) {
			status = http.StatusBadRequest
		}
		c.JSON(status, gin.H{"message": err.Error()})
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Body)
}
