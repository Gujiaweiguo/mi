package handlers

import (
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

type OrgHandler struct {
	repository *auth.Repository
}

func NewOrgHandler(repository *auth.Repository) *OrgHandler {
	return &OrgHandler{repository: repository}
}

func (h *OrgHandler) Departments(c *gin.Context) {
	departments, err := h.repository.ListDepartments(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load departments"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"departments": departments})
}

func (h *OrgHandler) Stores(c *gin.Context) {
	stores, err := h.repository.ListStores(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load stores"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"stores": stores})
}
