//go:build integration

package dashboard_test

import (
	"context"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/dashboard"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
)

func TestDashboardServiceIntegrationQueries(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := dashboard.NewDashboardService(db)

	summary, err := service.GetSummary(ctx)
	if err != nil {
		t.Fatalf("get dashboard summary: %v", err)
	}
	if summary == nil {
		t.Fatal("expected non-nil dashboard summary")
	}
	if summary.ActiveLeases < 0 || summary.PendingLeaseApprovals < 0 || summary.PendingInvoiceApprovals < 0 || summary.OpenReceivables < 0 || summary.OverdueReceivables < 0 || summary.PendingWorkflows < 0 {
		t.Fatalf("expected non-negative dashboard counts, got %+v", summary)
	}

	aggregate, err := service.GetWorkbenchAggregate(ctx)
	if err != nil {
		t.Fatalf("get workbench aggregate: %v", err)
	}
	if aggregate == nil {
		t.Fatal("expected non-nil workbench aggregate")
	}
	if aggregate.PendingApprovals.RouteTarget == "" || aggregate.Receivables.RouteTarget == "" || aggregate.OverdueReceivables.RouteTarget == "" || aggregate.ActiveLeases.RouteTarget == "" {
		t.Fatalf("expected workbench route targets, got %+v", aggregate)
	}
}
