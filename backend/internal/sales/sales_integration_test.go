//go:build integration

package sales_test

import (
	"context"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/sales"
)

func TestSalesServiceIntegrationOperations(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := database.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := sales.NewService(sales.NewRepository(db))
	storeID := int64(101)
	unitID := int64(101)

	t.Run("daily sales create list batch and dedupe", func(t *testing.T) {
		saleDate := time.Date(2030, 1, 2, 0, 0, 0, 0, time.UTC)
		created, err := service.CreateDailySale(ctx, sales.CreateDailySaleInput{StoreID: storeID, UnitID: unitID, SaleDate: saleDate, SalesAmount: 100.25})
		if err != nil {
			t.Fatalf("create daily sale: %v", err)
		}
		items, err := service.ListDailySales(ctx, sales.DailySaleFilter{StoreID: &storeID, DateFrom: &saleDate, DateTo: &saleDate, Limit: 20})
		if err != nil {
			t.Fatalf("list daily sales: %v", err)
		}
		assertDailySalePresent(t, items, created.ID, 100.25)

		batchDateA := time.Date(2030, 1, 3, 0, 0, 0, 0, time.UTC)
		batchDateB := time.Date(2030, 1, 4, 0, 0, 0, 0, time.UTC)
		processed, err := service.BatchUpsertDailySales(ctx, []sales.BatchDailySaleInput{{StoreID: storeID, UnitID: unitID, SaleDate: batchDateA, SalesAmount: 200.5}, {StoreID: storeID, UnitID: unitID, SaleDate: batchDateB, SalesAmount: 300.75}})
		if err != nil {
			t.Fatalf("batch upsert daily sales: %v", err)
		}
		if processed != 2 {
			t.Fatalf("expected 2 processed daily sales, got %d", processed)
		}

		dateFrom := saleDate
		dateTo := batchDateB
		items, err = service.ListDailySales(ctx, sales.DailySaleFilter{StoreID: &storeID, UnitID: &unitID, DateFrom: &dateFrom, DateTo: &dateTo, Limit: 20})
		if err != nil {
			t.Fatalf("list batched daily sales: %v", err)
		}
		if countDailySalesOnDate(items, batchDateA) != 1 || countDailySalesOnDate(items, batchDateB) != 1 {
			t.Fatalf("expected batch daily sales in list, got %+v", items)
		}

		dedupeDate := time.Date(2030, 1, 5, 0, 0, 0, 0, time.UTC)
		processed, err = service.BatchUpsertDailySales(ctx, []sales.BatchDailySaleInput{{StoreID: storeID, UnitID: unitID, SaleDate: dedupeDate, SalesAmount: 444}, {StoreID: storeID, UnitID: unitID, SaleDate: dedupeDate, SalesAmount: 555}})
		if err != nil {
			t.Fatalf("batch upsert duplicate daily sale: %v", err)
		}
		if processed != 2 {
			t.Fatalf("expected duplicate batch to report 2 processed rows, got %d", processed)
		}
		items, err = service.ListDailySales(ctx, sales.DailySaleFilter{StoreID: &storeID, UnitID: &unitID, DateFrom: &dedupeDate, DateTo: &dedupeDate, Limit: 20})
		if err != nil {
			t.Fatalf("list deduped daily sales: %v", err)
		}
		if len(items) != 1 || items[0].SalesAmount != 555 {
			t.Fatalf("expected one deduped daily sale with latest amount, got %+v", items)
		}
	})

	t.Run("traffic create list batch and dedupe", func(t *testing.T) {
		trafficDate := time.Date(2030, 2, 1, 0, 0, 0, 0, time.UTC)
		created, err := service.CreateTraffic(ctx, sales.CreateTrafficInput{StoreID: storeID, TrafficDate: trafficDate, InboundCount: 111})
		if err != nil {
			t.Fatalf("create traffic: %v", err)
		}
		items, err := service.ListTraffic(ctx, sales.TrafficFilter{StoreID: &storeID, DateFrom: &trafficDate, DateTo: &trafficDate, Limit: 20})
		if err != nil {
			t.Fatalf("list traffic: %v", err)
		}
		assertTrafficPresent(t, items, created.ID, 111)

		dateA := time.Date(2030, 2, 2, 0, 0, 0, 0, time.UTC)
		dateB := time.Date(2030, 2, 3, 0, 0, 0, 0, time.UTC)
		processed, err := service.BatchUpsertTraffic(ctx, []sales.BatchTrafficInput{{StoreID: storeID, TrafficDate: dateA, InboundCount: 222}, {StoreID: storeID, TrafficDate: dateB, InboundCount: 333}})
		if err != nil {
			t.Fatalf("batch upsert traffic: %v", err)
		}
		if processed != 2 {
			t.Fatalf("expected 2 processed traffic rows, got %d", processed)
		}
		items, err = service.ListTraffic(ctx, sales.TrafficFilter{StoreID: &storeID, DateFrom: &trafficDate, DateTo: &dateB, Limit: 20})
		if err != nil {
			t.Fatalf("list batched traffic: %v", err)
		}
		if countTrafficOnDate(items, dateA) != 1 || countTrafficOnDate(items, dateB) != 1 {
			t.Fatalf("expected batched traffic in list, got %+v", items)
		}

		dedupeDate := time.Date(2030, 2, 4, 0, 0, 0, 0, time.UTC)
		processed, err = service.BatchUpsertTraffic(ctx, []sales.BatchTrafficInput{{StoreID: storeID, TrafficDate: dedupeDate, InboundCount: 444}, {StoreID: storeID, TrafficDate: dedupeDate, InboundCount: 555}})
		if err != nil {
			t.Fatalf("batch upsert duplicate traffic: %v", err)
		}
		if processed != 2 {
			t.Fatalf("expected duplicate traffic batch to report 2 processed rows, got %d", processed)
		}
		items, err = service.ListTraffic(ctx, sales.TrafficFilter{StoreID: &storeID, DateFrom: &dedupeDate, DateTo: &dedupeDate, Limit: 20})
		if err != nil {
			t.Fatalf("list deduped traffic: %v", err)
		}
		if len(items) != 1 || items[0].InboundCount != 555 {
			t.Fatalf("expected one deduped traffic row with latest count, got %+v", items)
		}
	})
}

func assertDailySalePresent(t *testing.T, items []sales.DailySale, id int64, wantAmount float64) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.SalesAmount != wantAmount {
				t.Fatalf("expected daily sale %d amount %.2f, got %+v", id, wantAmount, item)
			}
			return
		}
	}
	t.Fatalf("expected daily sale %d in list", id)
}

func countDailySalesOnDate(items []sales.DailySale, day time.Time) int {
	count := 0
	for _, item := range items {
		if sameDate(item.SaleDate, day) {
			count++
		}
	}
	return count
}

func assertTrafficPresent(t *testing.T, items []sales.CustomerTraffic, id int64, wantCount int) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.InboundCount != wantCount {
				t.Fatalf("expected traffic %d count %d, got %+v", id, wantCount, item)
			}
			return
		}
	}
	t.Fatalf("expected traffic %d in list", id)
}

func countTrafficOnDate(items []sales.CustomerTraffic, day time.Time) int {
	count := 0
	for _, item := range items {
		if sameDate(item.TrafficDate, day) {
			count++
		}
	}
	return count
}

func sameDate(left time.Time, right time.Time) bool {
	ly, lm, ld := left.Date()
	ry, rm, rd := right.Date()
	return ly == ry && lm == rm && ld == rd
}
