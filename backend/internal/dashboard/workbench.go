package dashboard

import (
	"context"
	"database/sql"
	"fmt"

	"golang.org/x/sync/errgroup"
)

const workbenchPreviewLimit = 5

type WorkbenchPreviewRow struct {
	ID          int64  `json:"id"`
	Title       string `json:"title"`
	Subtitle    string `json:"subtitle"`
	Status      string `json:"status"`
	Meta        string `json:"meta"`
	RouteTarget string `json:"route_target"`
}

type WorkbenchQueueSection struct {
	Count       int64                 `json:"count"`
	RouteTarget string                `json:"route_target"`
	PreviewRows []WorkbenchPreviewRow `json:"preview_rows"`
}

type WorkbenchAggregate struct {
	PendingApprovals   WorkbenchQueueSection `json:"pending_approvals"`
	Receivables        WorkbenchQueueSection `json:"receivables"`
	OverdueReceivables WorkbenchQueueSection `json:"overdue_receivables"`
	ActiveLeases       WorkbenchQueueSection `json:"active_leases"`
}

func (s *DashboardService) GetWorkbenchAggregate(ctx context.Context) (*WorkbenchAggregate, error) {
	g, ctx := errgroup.WithContext(ctx)

	aggregate := &WorkbenchAggregate{
		PendingApprovals:   WorkbenchQueueSection{RouteTarget: "/workflow/admin", PreviewRows: make([]WorkbenchPreviewRow, 0)},
		Receivables:        WorkbenchQueueSection{RouteTarget: "/billing/receivables", PreviewRows: make([]WorkbenchPreviewRow, 0)},
		OverdueReceivables: WorkbenchQueueSection{RouteTarget: "/billing/receivables", PreviewRows: make([]WorkbenchPreviewRow, 0)},
		ActiveLeases:       WorkbenchQueueSection{RouteTarget: "/lease/contracts", PreviewRows: make([]WorkbenchPreviewRow, 0)},
	}

	g.Go(func() error {
		count, err := countQuery(ctx, s.db, `
			SELECT (
				(SELECT COUNT(*) FROM lease_contracts WHERE status = 'pending_approval') +
				(SELECT COUNT(*) FROM billing_documents WHERE document_type = 'invoice' AND status = 'pending_approval')
			)
		`)
		if err != nil {
			return err
		}
		aggregate.PendingApprovals.Count = count
		return nil
	})

	g.Go(func() error {
		rows, err := previewRowsQuery(ctx, s.db, fmt.Sprintf(`
			SELECT item_id, title, subtitle, status, meta, route_target
			FROM (
				SELECT
					id AS item_id,
					CONCAT('Lease ', lease_no) AS title,
					tenant_name AS subtitle,
					status,
					COALESCE(DATE_FORMAT(submitted_at, '%%Y-%%m-%%d %%H:%%i'), 'Pending approval') AS meta,
					'/lease/contracts' AS route_target,
					submitted_at AS sort_at
				FROM lease_contracts
				WHERE status = 'pending_approval'

				UNION ALL

				SELECT
					id AS item_id,
					CONCAT('Invoice ', COALESCE(document_no, CONCAT('#', id))) AS title,
					tenant_name AS subtitle,
					status,
					COALESCE(DATE_FORMAT(submitted_at, '%%Y-%%m-%%d %%H:%%i'), 'Pending approval') AS meta,
					'/billing/invoices' AS route_target,
					submitted_at AS sort_at
				FROM billing_documents
				WHERE document_type = 'invoice' AND status = 'pending_approval'
			) approvals
			ORDER BY sort_at DESC, item_id DESC
			LIMIT %d
		`, workbenchPreviewLimit))
		if err != nil {
			return err
		}
		aggregate.PendingApprovals.PreviewRows = rows
		return nil
	})

	g.Go(func() error {
		count, err := countQuery(ctx, s.db, "SELECT COUNT(*) FROM ar_open_items WHERE outstanding_amount > 0 AND is_deposit = FALSE")
		if err != nil {
			return err
		}
		aggregate.Receivables.Count = count
		return nil
	})

	g.Go(func() error {
		rows, err := previewRowsQuery(ctx, s.db, fmt.Sprintf(`
			SELECT
				ai.id AS item_id,
				COALESCE(bd.document_no, CONCAT('AR #', ai.id)) AS title,
				lc.tenant_name AS subtitle,
				ai.charge_type AS status,
				CONCAT('Due ', DATE_FORMAT(ai.due_date, '%%Y-%%m-%%d'), ' • ', FORMAT(ai.outstanding_amount, 2)) AS meta,
				'/billing/receivables' AS route_target
			FROM ar_open_items ai
			INNER JOIN lease_contracts lc ON lc.id = ai.lease_contract_id
			LEFT JOIN billing_documents bd ON bd.id = ai.billing_document_id
			WHERE ai.outstanding_amount > 0 AND ai.is_deposit = FALSE
			ORDER BY ai.due_date ASC, ai.id DESC
			LIMIT %d
		`, workbenchPreviewLimit))
		if err != nil {
			return err
		}
		aggregate.Receivables.PreviewRows = rows
		return nil
	})

	g.Go(func() error {
		count, err := countQuery(ctx, s.db, "SELECT COUNT(*) FROM ar_open_items WHERE outstanding_amount > 0 AND is_deposit = FALSE AND due_date < CURDATE()")
		if err != nil {
			return err
		}
		aggregate.OverdueReceivables.Count = count
		return nil
	})

	g.Go(func() error {
		rows, err := previewRowsQuery(ctx, s.db, fmt.Sprintf(`
			SELECT
				ai.id AS item_id,
				COALESCE(bd.document_no, CONCAT('AR #', ai.id)) AS title,
				lc.tenant_name AS subtitle,
				ai.charge_type AS status,
				CONCAT('Overdue since ', DATE_FORMAT(ai.due_date, '%%Y-%%m-%%d'), ' • ', FORMAT(ai.outstanding_amount, 2)) AS meta,
				'/billing/receivables' AS route_target
			FROM ar_open_items ai
			INNER JOIN lease_contracts lc ON lc.id = ai.lease_contract_id
			LEFT JOIN billing_documents bd ON bd.id = ai.billing_document_id
			WHERE ai.outstanding_amount > 0 AND ai.is_deposit = FALSE AND ai.due_date < CURDATE()
			ORDER BY ai.due_date ASC, ai.id DESC
			LIMIT %d
		`, workbenchPreviewLimit))
		if err != nil {
			return err
		}
		aggregate.OverdueReceivables.PreviewRows = rows
		return nil
	})

	g.Go(func() error {
		count, err := countQuery(ctx, s.db, "SELECT COUNT(*) FROM lease_contracts WHERE status = 'active'")
		if err != nil {
			return err
		}
		aggregate.ActiveLeases.Count = count
		return nil
	})

	g.Go(func() error {
		rows, err := previewRowsQuery(ctx, s.db, fmt.Sprintf(`
			SELECT
				id AS item_id,
				CONCAT('Lease ', lease_no) AS title,
				tenant_name AS subtitle,
				status,
				CONCAT(DATE_FORMAT(start_date, '%%Y-%%m-%%d'), ' → ', DATE_FORMAT(end_date, '%%Y-%%m-%%d')) AS meta,
				'/lease/contracts' AS route_target
			FROM lease_contracts
			WHERE status = 'active'
			ORDER BY end_date ASC, id DESC
			LIMIT %d
		`, workbenchPreviewLimit))
		if err != nil {
			return err
		}
		aggregate.ActiveLeases.PreviewRows = rows
		return nil
	})

	if err := g.Wait(); err != nil {
		return nil, err
	}

	return aggregate, nil
}

func previewRowsQuery(ctx context.Context, db *sql.DB, query string) ([]WorkbenchPreviewRow, error) {
	rows, err := db.QueryContext(ctx, query)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	result := make([]WorkbenchPreviewRow, 0)
	for rows.Next() {
		var row WorkbenchPreviewRow
		if err := rows.Scan(&row.ID, &row.Title, &row.Subtitle, &row.Status, &row.Meta, &row.RouteTarget); err != nil {
			return nil, err
		}
		result = append(result, row)
	}

	if err := rows.Err(); err != nil {
		return nil, err
	}

	return result, nil
}
