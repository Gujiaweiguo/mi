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

// Departments godoc
//
//	@Summary		List departments
//	@Description	Returns departments available to workflow administration endpoints.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{departments=[]auth.Department}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/org/departments [get]
func (h *OrgHandler) Departments(c *gin.Context) {
	departments, err := h.repository.ListDepartments(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load departments"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"departments": departments})
}

// Stores godoc
//
//	@Summary		List organization stores
//	@Description	Returns organization stores available to workflow administration endpoints.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{stores=[]auth.Store}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/org/stores [get]
func (h *OrgHandler) Stores(c *gin.Context) {
	stores, err := h.repository.ListStores(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load stores"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"stores": stores})
}
