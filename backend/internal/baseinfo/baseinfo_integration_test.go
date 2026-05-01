//go:build integration

package baseinfo_test

import (
	"context"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/baseinfo"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
)

func TestBaseinfoServiceIntegrationCatalogOperations(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := baseinfo.NewService(baseinfo.NewRepository(db))

	t.Run("store types", func(t *testing.T) {
		created, err := service.CreateStoreType(ctx, baseinfo.CatalogInput{Code: "store-type-it", Name: "Integration Store Type"})
		if err != nil {
			t.Fatalf("create store type: %v", err)
		}
		items, err := service.ListStoreTypes(ctx)
		assertCatalogItemPresent(t, mustListCatalogItems(t, items, err), created.ID, "Integration Store Type")

		updated, err := service.UpdateStoreType(ctx, created.ID, baseinfo.CatalogInput{Code: created.Code, Name: "Integration Store Type Updated"})
		if err != nil {
			t.Fatalf("update store type: %v", err)
		}
		if updated.Name != "Integration Store Type Updated" {
			t.Fatalf("expected updated store type name, got %+v", updated)
		}
	})

	t.Run("store management types", func(t *testing.T) {
		created, err := service.CreateStoreManagementType(ctx, baseinfo.CatalogInput{Code: "mgmt-it", Name: "Integration Management", Status: "inactive"})
		if err != nil {
			t.Fatalf("create store management type: %v", err)
		}
		items, err := service.ListStoreManagementTypes(ctx)
		items = mustListCatalogItems(t, items, err)
		item := assertCatalogItemPresent(t, items, created.ID, "Integration Management")
		if item.Status != "inactive" {
			t.Fatalf("expected inactive store management type, got %+v", item)
		}
	})

	t.Run("area levels", func(t *testing.T) {
		created, err := service.CreateAreaLevel(ctx, baseinfo.CatalogInput{Code: "AL-IT", Name: "Integration Area Level"})
		if err != nil {
			t.Fatalf("create area level: %v", err)
		}
		items, err := service.ListAreaLevels(ctx)
		assertCatalogItemPresent(t, mustListCatalogItems(t, items, err), created.ID, "Integration Area Level")
	})

	t.Run("unit types", func(t *testing.T) {
		created, err := service.CreateUnitType(ctx, baseinfo.CatalogInput{Code: "unit-it", Name: "Integration Unit Type"})
		if err != nil {
			t.Fatalf("create unit type: %v", err)
		}
		items, err := service.ListUnitTypes(ctx)
		items = mustListCatalogItems(t, items, err)
		item := assertCatalogItemPresent(t, items, created.ID, "Integration Unit Type")
		if item.Status != "active" {
			t.Fatalf("expected default active unit type, got %+v", item)
		}
	})

	t.Run("shop types", func(t *testing.T) {
		color := "#112233"
		created, err := service.CreateShopType(ctx, baseinfo.CatalogInput{Code: "shop-it", Name: "Integration Shop Type", ColorHex: &color, Status: "active"})
		if err != nil {
			t.Fatalf("create shop type: %v", err)
		}
		items, err := service.ListShopTypes(ctx)
		item := assertCatalogItemPresent(t, mustListCatalogItems(t, items, err), created.ID, "Integration Shop Type")
		if item.ColorHex == nil || *item.ColorHex != color {
			t.Fatalf("expected persisted color_hex, got %+v", item)
		}
	})

	t.Run("currency types", func(t *testing.T) {
		isLocal := false
		created, err := service.CreateCurrencyType(ctx, baseinfo.CatalogInput{Code: "USD-IT", Name: "Integration Dollar", IsLocal: &isLocal, Status: "inactive"})
		if err != nil {
			t.Fatalf("create currency type: %v", err)
		}
		items, err := service.ListCurrencyTypes(ctx)
		item := assertCatalogItemPresent(t, mustListCatalogItems(t, items, err), created.ID, "Integration Dollar")
		if item.IsLocal == nil || *item.IsLocal {
			t.Fatalf("expected non-local currency, got %+v", item)
		}
	})

	t.Run("trade definitions", func(t *testing.T) {
		parentID := int64(101)
		level := 2
		created, err := service.CreateTradeDefinition(ctx, baseinfo.CatalogInput{Code: "TRADE-IT", Name: "Integration Trade", ParentID: &parentID, Level: &level, Status: "active"})
		if err != nil {
			t.Fatalf("create trade definition: %v", err)
		}
		items, err := service.ListTradeDefinitions(ctx)
		item := assertCatalogItemPresent(t, mustListCatalogItems(t, items, err), created.ID, "Integration Trade")
		if item.ParentID == nil || *item.ParentID != parentID || item.Level == nil || *item.Level != level {
			t.Fatalf("expected parent and level to persist, got %+v", item)
		}
	})
}

func mustListCatalogItems(t *testing.T, items []baseinfo.ReferenceCatalogItem, err error) []baseinfo.ReferenceCatalogItem {
	t.Helper()
	if err != nil {
		t.Fatalf("list catalog items: %v", err)
	}
	return items
}

func assertCatalogItemPresent(t *testing.T, items []baseinfo.ReferenceCatalogItem, id int64, wantName string) baseinfo.ReferenceCatalogItem {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected item %d name %q, got %+v", id, wantName, item)
			}
			return item
		}
	}
	t.Fatalf("expected item %d in list", id)
	return baseinfo.ReferenceCatalogItem{}
}
