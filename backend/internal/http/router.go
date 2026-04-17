package http

import (
	"context"
	"database/sql"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/baseinfo"
	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/Gujiaweiguo/mi/backend/internal/docoutput"
	"github.com/Gujiaweiguo/mi/backend/internal/excelio"
	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/masterdata"
	"github.com/Gujiaweiguo/mi/backend/internal/reporting"
	"github.com/Gujiaweiguo/mi/backend/internal/sales"
	"github.com/Gujiaweiguo/mi/backend/internal/structure"
	"github.com/Gujiaweiguo/mi/backend/internal/taxexport"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type workflowSyncers struct {
	syncers []handlers.WorkflowStateSyncer
}

func (w workflowSyncers) SyncWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error {
	for _, syncer := range w.syncers {
		if err := syncer.SyncWorkflowState(ctx, instance, actorUserID); err != nil {
			return err
		}
	}
	return nil
}

func NewRouter(cfg *config.Config, db *sql.DB) *gin.Engine {
	router := gin.New()
	router.Use(gin.Logger(), gin.Recovery())

	healthHandler := handlers.NewHealthHandler(cfg, db)
	authRepository := auth.NewRepository(db)
	authService := auth.NewService(authRepository, cfg.Auth)
	authHandler := handlers.NewAuthHandler(authService)
	orgHandler := handlers.NewOrgHandler(authRepository)
	workflowRepository := workflow.NewRepository(db)
	workflowService := workflow.NewService(db, workflowRepository)
	leaseRepository := lease.NewRepository(db)
	leaseService := lease.NewService(db, leaseRepository, workflowService)
	billingRepository := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepository)
	invoiceRepository := invoice.NewRepository(db)
	invoiceService := invoice.NewService(db, invoiceRepository, billingRepository, workflowService)
	docOutputRepository := docoutput.NewRepository(db)
	docOutputService := docoutput.NewService(docOutputRepository, invoiceService, cfg.Storage)
	excelIORepository := excelio.NewRepository(db)
	reportingRepository := reporting.NewRepository(db)
	reportingService := reporting.NewService(reportingRepository)
	salesRepository := sales.NewRepository(db)
	salesService := sales.NewService(salesRepository)
	excelIOService := excelio.NewService(excelIORepository, salesService)
	structureRepository := structure.NewRepository(db)
	structureService := structure.NewService(structureRepository)
	baseInfoRepository := baseinfo.NewRepository(db)
	baseInfoService := baseinfo.NewService(baseInfoRepository)
	masterDataRepository := masterdata.NewRepository(db)
	masterDataService := masterdata.NewService(masterDataRepository)
	taxExportRepository := taxexport.NewRepository(db)
	taxExportService := taxexport.NewService(taxExportRepository)
	leaseHandler := handlers.NewLeaseHandler(leaseService)
	billingHandler := handlers.NewBillingHandler(billingService)
	invoiceHandler := handlers.NewInvoiceHandler(invoiceService)
	docOutputHandler := handlers.NewDocOutputHandler(docOutputService)
	excelIOHandler := handlers.NewExcelIOHandler(excelIOService)
	masterDataHandler := handlers.NewMasterDataHandler(masterDataService)
	reportingHandler := handlers.NewReportingHandler(reportingService)
	salesHandler := handlers.NewSalesHandler(salesService)
	structureHandler := handlers.NewStructureHandler(structureService)
	baseInfoHandler := handlers.NewBaseInfoHandler(baseInfoService)
	taxExportHandler := handlers.NewTaxExportHandler(taxExportService)
	workflowHandler := handlers.NewWorkflowHandler(workflowService, workflowSyncers{syncers: []handlers.WorkflowStateSyncer{leaseService, invoiceService}})
	router.GET("/health", healthHandler.Get)
	router.GET("/healthz", healthHandler.Get)

	api := router.Group("/api")
	api.GET("/health", healthHandler.Get)
	api.GET("/healthz", healthHandler.Get)
	authGroup := api.Group("/auth")
	authGroup.POST("/login", authHandler.Login)
	authGroup.GET("/me", middleware.RequireAuth(authService, authRepository), authHandler.Me)

	orgGroup := api.Group("/org")
	orgGroup.Use(middleware.RequireAuth(authService, authRepository))
	orgGroup.GET("/departments", middleware.RequirePermission("workflow.admin", "view", authService), orgHandler.Departments)
	orgGroup.GET("/stores", middleware.RequirePermission("workflow.admin", "view", authService), orgHandler.Stores)

	masterDataGroup := api.Group("/master-data")
	masterDataGroup.Use(middleware.RequireAuth(authService, authRepository))
	masterDataGroup.GET("/customers", middleware.RequirePermission("masterdata.admin", "view", authService), masterDataHandler.ListCustomers)
	masterDataGroup.POST("/customers", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.CreateCustomer)
	masterDataGroup.PUT("/customers/:id", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.UpdateCustomer)
	masterDataGroup.GET("/brands", middleware.RequirePermission("masterdata.admin", "view", authService), masterDataHandler.ListBrands)
	masterDataGroup.POST("/brands", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.CreateBrand)
	masterDataGroup.PUT("/brands/:id", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.UpdateBrand)
	masterDataGroup.GET("/unit-rent-budgets", middleware.RequirePermission("masterdata.admin", "view", authService), masterDataHandler.ListUnitRentBudgets)
	masterDataGroup.POST("/unit-rent-budgets", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.CreateUnitRentBudget)
	masterDataGroup.PUT("/unit-rent-budgets/:unitId/:fiscalYear", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.UpdateUnitRentBudget)
	masterDataGroup.GET("/store-rent-budgets", middleware.RequirePermission("masterdata.admin", "view", authService), masterDataHandler.ListStoreRentBudgets)
	masterDataGroup.POST("/store-rent-budgets", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.CreateStoreRentBudget)
	masterDataGroup.PUT("/store-rent-budgets/:storeId/:fiscalYear/:fiscalMonth", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.UpdateStoreRentBudget)
	masterDataGroup.GET("/unit-prospects", middleware.RequirePermission("masterdata.admin", "view", authService), masterDataHandler.ListUnitProspects)
	masterDataGroup.POST("/unit-prospects", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.CreateUnitProspect)
	masterDataGroup.PUT("/unit-prospects/:unitId/:fiscalYear", middleware.RequirePermission("masterdata.admin", "edit", authService), masterDataHandler.UpdateUnitProspect)

	baseInfoGroup := api.Group("/base-info")
	baseInfoGroup.Use(middleware.RequireAuth(authService, authRepository))
	baseInfoGroup.GET("/store-types", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListStoreTypes)
	baseInfoGroup.POST("/store-types", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateStoreType)
	baseInfoGroup.PUT("/store-types/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateStoreType)
	baseInfoGroup.GET("/store-management-types", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListStoreManagementTypes)
	baseInfoGroup.POST("/store-management-types", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateStoreManagementType)
	baseInfoGroup.PUT("/store-management-types/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateStoreManagementType)
	baseInfoGroup.GET("/area-levels", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListAreaLevels)
	baseInfoGroup.POST("/area-levels", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateAreaLevel)
	baseInfoGroup.PUT("/area-levels/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateAreaLevel)
	baseInfoGroup.GET("/unit-types", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListUnitTypes)
	baseInfoGroup.POST("/unit-types", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateUnitType)
	baseInfoGroup.PUT("/unit-types/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateUnitType)
	baseInfoGroup.GET("/shop-types", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListShopTypes)
	baseInfoGroup.POST("/shop-types", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateShopType)
	baseInfoGroup.PUT("/shop-types/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateShopType)
	baseInfoGroup.GET("/currency-types", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListCurrencyTypes)
	baseInfoGroup.POST("/currency-types", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateCurrencyType)
	baseInfoGroup.PUT("/currency-types/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateCurrencyType)
	baseInfoGroup.GET("/trade-definitions", middleware.RequirePermission("baseinfo.admin", "view", authService), baseInfoHandler.ListTradeDefinitions)
	baseInfoGroup.POST("/trade-definitions", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.CreateTradeDefinition)
	baseInfoGroup.PUT("/trade-definitions/:id", middleware.RequirePermission("baseinfo.admin", "edit", authService), baseInfoHandler.UpdateTradeDefinition)

	leaseGroup := api.Group("/leases")
	leaseGroup.Use(middleware.RequireAuth(authService, authRepository))
	leaseGroup.POST("", middleware.RequirePermission("lease.contract", "edit", authService), leaseHandler.Create)
	leaseGroup.GET("", middleware.RequirePermission("lease.contract", "view", authService), leaseHandler.List)
	leaseGroup.GET("/:id", middleware.RequirePermission("lease.contract", "view", authService), leaseHandler.Get)
	leaseGroup.POST("/:id/amend", middleware.RequirePermission("lease.contract", "edit", authService), leaseHandler.Amend)
	leaseGroup.POST("/:id/submit", middleware.RequirePermission("lease.contract", "edit", authService), leaseHandler.Submit)
	leaseGroup.POST("/:id/terminate", middleware.RequirePermission("lease.contract", "edit", authService), leaseHandler.Terminate)

	billingGroup := api.Group("/billing")
	billingGroup.Use(middleware.RequireAuth(authService, authRepository))
	billingGroup.POST("/charges/generate", middleware.RequirePermission("billing.charge", "edit", authService), billingHandler.GenerateCharges)
	billingGroup.GET("/charges", middleware.RequirePermission("billing.charge", "view", authService), billingHandler.ListCharges)

	invoiceGroup := api.Group("/invoices")
	invoiceGroup.Use(middleware.RequireAuth(authService, authRepository))
	invoiceGroup.POST("", middleware.RequirePermission("billing.invoice", "edit", authService), invoiceHandler.Create)
	invoiceGroup.GET("", middleware.RequirePermission("billing.invoice", "view", authService), invoiceHandler.List)
	invoiceGroup.GET("/:id", middleware.RequirePermission("billing.invoice", "view", authService), invoiceHandler.Get)
	invoiceGroup.POST("/:id/submit", middleware.RequirePermission("billing.invoice", "edit", authService), invoiceHandler.Submit)
	invoiceGroup.POST("/:id/cancel", middleware.RequirePermission("billing.invoice", "edit", authService), invoiceHandler.Cancel)
	invoiceGroup.POST("/:id/adjust", middleware.RequirePermission("billing.invoice", "edit", authService), invoiceHandler.Adjust)
	invoiceGroup.GET("/:id/receivable", middleware.RequirePermission("billing.invoice", "view", authService), invoiceHandler.GetReceivable)
	invoiceGroup.POST("/:id/payments", middleware.RequirePermission("billing.invoice", "edit", authService), invoiceHandler.RecordPayment)

	receivableGroup := api.Group("/receivables")
	receivableGroup.Use(middleware.RequireAuth(authService, authRepository))
	receivableGroup.GET("", middleware.RequirePermission("billing.invoice", "view", authService), invoiceHandler.ListReceivables)

	printGroup := api.Group("/print")
	printGroup.Use(middleware.RequireAuth(authService, authRepository))
	printGroup.POST("/templates", middleware.RequirePermission("billing.invoice", "edit", authService), docOutputHandler.UpsertTemplate)
	printGroup.GET("/templates", middleware.RequirePermission("billing.invoice", "view", authService), docOutputHandler.ListTemplates)
	printGroup.POST("/render/html", middleware.RequirePermission("billing.invoice", "print", authService), docOutputHandler.RenderHTML)
	printGroup.POST("/render/pdf", middleware.RequirePermission("billing.invoice", "print", authService), docOutputHandler.RenderPDF)

	taxGroup := api.Group("/tax")
	taxGroup.Use(middleware.RequireAuth(authService, authRepository))
	taxGroup.POST("/rule-sets", middleware.RequirePermission("tax.export", "edit", authService), taxExportHandler.UpsertRuleSet)
	taxGroup.GET("/rule-sets", middleware.RequirePermission("tax.export", "view", authService), taxExportHandler.ListRuleSets)
	taxGroup.POST("/exports/vouchers", middleware.RequirePermission("tax.export", "export", authService), taxExportHandler.ExportVoucherWorkbook)

	excelGroup := api.Group("/excel")
	excelGroup.Use(middleware.RequireAuth(authService, authRepository))
	excelGroup.GET("/templates/unit-data", middleware.RequirePermission("excel.io", "view", authService), excelIOHandler.DownloadUnitTemplate)
	excelGroup.GET("/templates/daily-sales", middleware.RequirePermission("excel.io", "view", authService), excelIOHandler.DownloadDailySalesTemplate)
	excelGroup.GET("/templates/customer-traffic", middleware.RequirePermission("excel.io", "view", authService), excelIOHandler.DownloadTrafficTemplate)
	excelGroup.POST("/imports/unit-data", middleware.RequirePermission("excel.io", "edit", authService), excelIOHandler.ImportUnits)
	excelGroup.POST("/imports/daily-sales", middleware.RequirePermission("excel.io", "edit", authService), excelIOHandler.ImportDailySales)
	excelGroup.POST("/imports/customer-traffic", middleware.RequirePermission("excel.io", "edit", authService), excelIOHandler.ImportTraffic)
	excelGroup.GET("/exports/operational", middleware.RequirePermission("excel.io", "export", authService), excelIOHandler.ExportOperationalDataset)

	reportGroup := api.Group("/reports")
	reportGroup.Use(middleware.RequireAuth(authService, authRepository))
	reportGroup.POST("/:reportId/query", middleware.RequirePermission("reporting.generalize", "view", authService), reportingHandler.Query)
	reportGroup.POST("/:reportId/export", middleware.RequirePermission("reporting.generalize", "export", authService), reportingHandler.Export)

	salesGroup := api.Group("/sales")
	salesGroup.Use(middleware.RequireAuth(authService, authRepository))
	salesGroup.GET("/daily", middleware.RequirePermission("sales.admin", "view", authService), salesHandler.ListDailySales)
	salesGroup.POST("/daily", middleware.RequirePermission("sales.admin", "edit", authService), salesHandler.CreateDailySale)
	salesGroup.GET("/traffic", middleware.RequirePermission("sales.admin", "view", authService), salesHandler.ListTraffic)
	salesGroup.POST("/traffic", middleware.RequirePermission("sales.admin", "edit", authService), salesHandler.CreateTraffic)

	structureGroup := api.Group("/structure")
	structureGroup.Use(middleware.RequireAuth(authService, authRepository))
	structureGroup.GET("/stores", middleware.RequirePermission("structure.admin", "view", authService), structureHandler.ListStores)
	structureGroup.POST("/stores", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.CreateStore)
	structureGroup.PUT("/stores/:id", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.UpdateStore)
	structureGroup.GET("/buildings", middleware.RequirePermission("structure.admin", "view", authService), structureHandler.ListBuildings)
	structureGroup.POST("/buildings", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.CreateBuilding)
	structureGroup.PUT("/buildings/:id", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.UpdateBuilding)
	structureGroup.GET("/floors", middleware.RequirePermission("structure.admin", "view", authService), structureHandler.ListFloors)
	structureGroup.POST("/floors", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.CreateFloor)
	structureGroup.PUT("/floors/:id", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.UpdateFloor)
	structureGroup.GET("/areas", middleware.RequirePermission("structure.admin", "view", authService), structureHandler.ListAreas)
	structureGroup.POST("/areas", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.CreateArea)
	structureGroup.PUT("/areas/:id", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.UpdateArea)
	structureGroup.GET("/locations", middleware.RequirePermission("structure.admin", "view", authService), structureHandler.ListLocations)
	structureGroup.POST("/locations", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.CreateLocation)
	structureGroup.PUT("/locations/:id", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.UpdateLocation)
	structureGroup.GET("/units", middleware.RequirePermission("structure.admin", "view", authService), structureHandler.ListUnits)
	structureGroup.POST("/units", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.CreateUnit)
	structureGroup.PUT("/units/:id", middleware.RequirePermission("structure.admin", "edit", authService), structureHandler.UpdateUnit)

	workflowGroup := api.Group("/workflow")
	workflowGroup.Use(middleware.RequireAuth(authService, authRepository))
	workflowGroup.GET("/definitions", middleware.RequirePermission("workflow.admin", "view", authService), workflowHandler.ListDefinitions)
	workflowGroup.GET("/instances", middleware.RequirePermission("workflow.admin", "view", authService), workflowHandler.ListInstances)
	workflowGroup.POST("/instances", middleware.RequirePermission("workflow.admin", "approve", authService), workflowHandler.Start)
	workflowGroup.GET("/instances/:id", middleware.RequirePermission("workflow.admin", "view", authService), workflowHandler.GetInstance)
	workflowGroup.GET("/instances/:id/audit", middleware.RequirePermission("workflow.admin", "view", authService), workflowHandler.AuditHistory)
	workflowGroup.GET("/instances/:id/reminders", middleware.RequirePermission("workflow.admin", "view", authService), workflowHandler.ReminderHistory)
	workflowGroup.POST("/reminders/run", middleware.RequirePermission("workflow.admin", "approve", authService), workflowHandler.RunReminders)
	workflowGroup.POST("/instances/:id/approve", middleware.RequirePermission("workflow.admin", "approve", authService), workflowHandler.Approve)
	workflowGroup.POST("/instances/:id/reject", middleware.RequirePermission("workflow.admin", "approve", authService), workflowHandler.Reject)
	workflowGroup.POST("/instances/:id/resubmit", middleware.RequirePermission("workflow.admin", "approve", authService), workflowHandler.Resubmit)

	return router
}
