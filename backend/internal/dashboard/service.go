package dashboard

import (
	"context"
	"database/sql"

	"golang.org/x/sync/errgroup"
)

// DashboardSummary holds the aggregated counts displayed on the dashboard.
type DashboardSummary struct {
	ActiveLeases            int64 `json:"active_leases"`
	PendingLeaseApprovals   int64 `json:"pending_lease_approvals"`
	PendingInvoiceApprovals int64 `json:"pending_invoice_approvals"`
	OpenReceivables         int64 `json:"open_receivables"`
	OverdueReceivables      int64 `json:"overdue_receivables"`
	PendingWorkflows        int64 `json:"pending_workflows"`
}

// DashboardService provides dashboard-level aggregated queries.
type DashboardService struct {
	db *sql.DB
}

// NewDashboardService creates a new DashboardService.
func NewDashboardService(db *sql.DB) *DashboardService {
	return &DashboardService{db: db}
}

// GetSummary runs 6 COUNT queries in parallel and returns the aggregated result.
func (s *DashboardService) GetSummary(ctx context.Context) (*DashboardSummary, error) {
	g, ctx := errgroup.WithContext(ctx)

	var (
		activeLeases            int64
		pendingLeaseApprovals   int64
		pendingInvoiceApprovals int64
		openReceivables         int64
		overdueReceivables      int64
		pendingWorkflows        int64
	)

	g.Go(func() error {
		var err error
		activeLeases, err = countQuery(ctx, s.db,
			"SELECT COUNT(*) FROM lease_contracts WHERE status = 'active'")
		return err
	})

	g.Go(func() error {
		var err error
		pendingLeaseApprovals, err = countQuery(ctx, s.db,
			"SELECT COUNT(*) FROM lease_contracts WHERE status = 'pending_approval'")
		return err
	})

	g.Go(func() error {
		var err error
		pendingInvoiceApprovals, err = countQuery(ctx, s.db,
			"SELECT COUNT(*) FROM billing_documents WHERE document_type = 'invoice' AND status = 'pending_approval'")
		return err
	})

	g.Go(func() error {
		var err error
		openReceivables, err = countQuery(ctx, s.db,
			"SELECT COUNT(*) FROM ar_open_items WHERE settlement_status = 'outstanding'")
		return err
	})

	g.Go(func() error {
		var err error
		overdueReceivables, err = countQuery(ctx, s.db,
			"SELECT COUNT(*) FROM ar_open_items WHERE settlement_status = 'outstanding' AND due_date < CURDATE()")
		return err
	})

	g.Go(func() error {
		var err error
		pendingWorkflows, err = countQuery(ctx, s.db,
			"SELECT COUNT(*) FROM workflow_instances WHERE status = 'pending'")
		return err
	})

	if err := g.Wait(); err != nil {
		return nil, err
	}

	return &DashboardSummary{
		ActiveLeases:            activeLeases,
		PendingLeaseApprovals:   pendingLeaseApprovals,
		PendingInvoiceApprovals: pendingInvoiceApprovals,
		OpenReceivables:         openReceivables,
		OverdueReceivables:      overdueReceivables,
		PendingWorkflows:        pendingWorkflows,
	}, nil
}

func countQuery(ctx context.Context, db *sql.DB, query string) (int64, error) {
	var count int64
	err := db.QueryRowContext(ctx, query).Scan(&count)
	return count, err
}
