//go:build integration

package masterdata_test

import (
	"context"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/masterdata"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
)

func TestMasterdataServiceIntegrationOperations(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := masterdata.NewService(masterdata.NewRepository(db))

	t.Run("customers", func(t *testing.T) {
		tradeID := int64(102)
		departmentID := int64(102)
		created, err := service.CreateCustomer(ctx, masterdata.CreateCustomerInput{Code: "CUST-IT", Name: "Integration Customer", TradeID: &tradeID, DepartmentID: &departmentID})
		if err != nil {
			t.Fatalf("create customer: %v", err)
		}

		customers, err := service.ListCustomers(ctx, masterdata.ListFilter{Query: "CUST-IT", Page: 1, PageSize: 20})
		if err != nil {
			t.Fatalf("list customers: %v", err)
		}
		customer := assertCustomerPresent(t, customers.Items, created.ID, "Integration Customer")
		if customer.TradeID == nil || *customer.TradeID != tradeID {
			t.Fatalf("expected trade id %d, got %+v", tradeID, customer)
		}

		updatedDepartmentID := int64(103)
		updated, err := service.UpdateCustomer(ctx, masterdata.UpdateCustomerInput{ID: created.ID, Code: "CUST-IT-2", Name: "Integration Customer Updated", TradeID: &tradeID, DepartmentID: &updatedDepartmentID, Status: "inactive"})
		if err != nil {
			t.Fatalf("update customer: %v", err)
		}
		if updated.Code != "CUST-IT-2" || updated.Name != "Integration Customer Updated" || updated.Status != "inactive" || updated.DepartmentID == nil || *updated.DepartmentID != updatedDepartmentID {
			t.Fatalf("unexpected updated customer: %+v", updated)
		}
	})

	t.Run("brands", func(t *testing.T) {
		created, err := service.CreateBrand(ctx, masterdata.CreateBrandInput{Code: "BR-IT", Name: "Integration Brand"})
		if err != nil {
			t.Fatalf("create brand: %v", err)
		}

		brands, err := service.ListBrands(ctx, masterdata.ListFilter{Query: "BR-IT", Page: 1, PageSize: 20})
		if err != nil {
			t.Fatalf("list brands: %v", err)
		}
		assertBrandPresent(t, brands.Items, created.ID, "Integration Brand")

		updated, err := service.UpdateBrand(ctx, masterdata.UpdateBrandInput{ID: created.ID, Code: "BR-IT-2", Name: "Integration Brand Updated", Status: "inactive"})
		if err != nil {
			t.Fatalf("update brand: %v", err)
		}
		if updated.Code != "BR-IT-2" || updated.Name != "Integration Brand Updated" || updated.Status != "inactive" {
			t.Fatalf("unexpected updated brand: %+v", updated)
		}
	})

	t.Run("unit rent budgets", func(t *testing.T) {
		budget, err := service.UpsertUnitRentBudget(ctx, masterdata.UpsertUnitRentBudgetInput{UnitID: 101, FiscalYear: 2030, BudgetPrice: 188.5})
		if err != nil {
			t.Fatalf("upsert unit rent budget: %v", err)
		}
		if budget.UnitID != 101 || budget.FiscalYear != 2030 || budget.BudgetPrice != 188.5 {
			t.Fatalf("unexpected unit rent budget: %+v", budget)
		}
		budgets, err := service.ListUnitRentBudgets(ctx)
		if err != nil {
			t.Fatalf("list unit rent budgets: %v", err)
		}
		if !hasUnitRentBudget(budgets, 101, 2030, 188.5) {
			t.Fatalf("expected upserted unit rent budget in list: %+v", budgets)
		}
	})

	t.Run("store rent budgets", func(t *testing.T) {
		budget, err := service.UpsertStoreRentBudget(ctx, masterdata.UpsertStoreRentBudgetInput{StoreID: 101, FiscalYear: 2030, FiscalMonth: 7, MonthlyBudget: 12345.67})
		if err != nil {
			t.Fatalf("upsert store rent budget: %v", err)
		}
		if budget.StoreID != 101 || budget.FiscalYear != 2030 || budget.FiscalMonth != 7 || budget.MonthlyBudget != 12345.67 {
			t.Fatalf("unexpected store rent budget: %+v", budget)
		}
		budgets, err := service.ListStoreRentBudgets(ctx)
		if err != nil {
			t.Fatalf("list store rent budgets: %v", err)
		}
		if !hasStoreRentBudget(budgets, 101, 2030, 7, 12345.67) {
			t.Fatalf("expected upserted store rent budget in list: %+v", budgets)
		}
	})

	t.Run("unit prospects", func(t *testing.T) {
		potentialCustomerID := int64(101)
		prospectBrandID := int64(101)
		prospectTradeID := int64(102)
		avgTransaction := 321.45
		prospectRentPrice := 188.88
		rentIncrement := "3% yearly"
		termMonths := 24

		prospect, err := service.UpsertUnitProspect(ctx, masterdata.UpsertUnitProspectInput{
			UnitID:              101,
			FiscalYear:          2031,
			PotentialCustomerID: &potentialCustomerID,
			ProspectBrandID:     &prospectBrandID,
			ProspectTradeID:     &prospectTradeID,
			AvgTransaction:      &avgTransaction,
			ProspectRentPrice:   &prospectRentPrice,
			RentIncrement:       &rentIncrement,
			ProspectTermMonths:  &termMonths,
		})
		if err != nil {
			t.Fatalf("upsert unit prospect: %v", err)
		}
		if prospect.UnitID != 101 || prospect.FiscalYear != 2031 {
			t.Fatalf("unexpected unit prospect: %+v", prospect)
		}

		prospects, err := service.ListUnitProspects(ctx)
		if err != nil {
			t.Fatalf("list unit prospects: %v", err)
		}
		found := assertUnitProspectPresent(t, prospects, 101, 2031)
		if found.ProspectTermMonths == nil || *found.ProspectTermMonths != 24 {
			t.Fatalf("expected created unit prospect term, got %+v", found)
		}

		updatedAvgTransaction := 654.32
		updatedRentIncrement := "5% every 2 years"
		updatedTermMonths := 36
		updated, err := service.UpsertUnitProspect(ctx, masterdata.UpsertUnitProspectInput{
			UnitID:              101,
			FiscalYear:          2031,
			PotentialCustomerID: &potentialCustomerID,
			ProspectBrandID:     &prospectBrandID,
			ProspectTradeID:     &prospectTradeID,
			AvgTransaction:      &updatedAvgTransaction,
			ProspectRentPrice:   &prospectRentPrice,
			RentIncrement:       &updatedRentIncrement,
			ProspectTermMonths:  &updatedTermMonths,
		})
		if err != nil {
			t.Fatalf("update unit prospect via upsert: %v", err)
		}
		if updated.AvgTransaction == nil || *updated.AvgTransaction != updatedAvgTransaction || updated.RentIncrement == nil || *updated.RentIncrement != updatedRentIncrement || updated.ProspectTermMonths == nil || *updated.ProspectTermMonths != updatedTermMonths {
			t.Fatalf("unexpected updated unit prospect: %+v", updated)
		}
	})
}

func assertCustomerPresent(t *testing.T, items []masterdata.Customer, id int64, wantName string) masterdata.Customer {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected customer %d name %q, got %+v", id, wantName, item)
			}
			return item
		}
	}
	t.Fatalf("expected customer %d in list", id)
	return masterdata.Customer{}
}

func assertBrandPresent(t *testing.T, items []masterdata.Brand, id int64, wantName string) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected brand %d name %q, got %+v", id, wantName, item)
			}
			return
		}
	}
	t.Fatalf("expected brand %d in list", id)
}

func hasUnitRentBudget(items []masterdata.UnitRentBudget, unitID int64, fiscalYear int, budgetPrice float64) bool {
	for _, item := range items {
		if item.UnitID == unitID && item.FiscalYear == fiscalYear && item.BudgetPrice == budgetPrice {
			return true
		}
	}
	return false
}

func hasStoreRentBudget(items []masterdata.StoreRentBudget, storeID int64, fiscalYear int, fiscalMonth int, monthlyBudget float64) bool {
	for _, item := range items {
		if item.StoreID == storeID && item.FiscalYear == fiscalYear && item.FiscalMonth == fiscalMonth && item.MonthlyBudget == monthlyBudget {
			return true
		}
	}
	return false
}

func assertUnitProspectPresent(t *testing.T, items []masterdata.UnitProspect, unitID int64, fiscalYear int) masterdata.UnitProspect {
	t.Helper()
	for _, item := range items {
		if item.UnitID == unitID && item.FiscalYear == fiscalYear {
			return item
		}
	}
	t.Fatalf("expected unit prospect for unit %d year %d", unitID, fiscalYear)
	return masterdata.UnitProspect{}
}
