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

func (h *ExcelIOHandler) DownloadUnitTemplate(c *gin.Context) {
	artifact, err := h.service.DownloadUnitTemplate(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to generate unit template"})
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Body)
}

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
