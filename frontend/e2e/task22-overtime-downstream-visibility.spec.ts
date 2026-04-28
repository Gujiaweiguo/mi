import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

const attachCommonMocks = async (page: Page) => {
  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'operator',
          display_name: 'Operator One',
          department_id: 100,
          roles: ['ops'],
          permissions: [
            { function_code: 'billing.charge', permission_level: 'edit', can_print: false, can_export: false },
            { function_code: 'billing.invoice', permission_level: 'edit', can_print: true, can_export: false },
          ],
        },
      }),
    })
  })

  await page.route('**/api/auth/login', async (route) => {
    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ token: 'playwright-token' }) })
  })

  await page.route('**/api/dashboard/summary', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        activeLeases: 1,
        pendingLeaseApprovals: 0,
        pendingInvoiceApprovals: 1,
        openReceivables: 1,
        overdueReceivables: 0,
        pendingWorkflows: 0,
      }),
    })
  })

  await page.route('**/api/health', async (route) => {
    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ status: 'ok' }) })
  })
}

const login = async (page: Page) => {
  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()
  await expect(page).toHaveURL(/\/dashboard/)
}

test('shows overtime attribution in billing charges and invoice receivables', async ({ page }) => {
  await attachCommonMocks(page)

  await page.route('**/api/billing/charges**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: [
          {
            id: 11,
            billing_run_id: 301,
            lease_contract_id: 42,
            lease_no: 'L-042',
            tenant_name: 'Harbor Foods',
            lease_term_id: null,
            charge_type: 'overtime_rent',
            charge_source: 'overtime',
            overtime_bill_id: 701,
            overtime_formula_id: 801,
            overtime_charge_id: 901,
            period_start: '2026-05-01',
            period_end: '2026-05-31',
            quantity_days: 31,
            unit_amount: 20,
            amount: 620,
            currency_type_id: 101,
            source_effective_version: 1,
            created_at: '2026-05-03T09:00:00Z',
          },
        ],
        total: 1,
        page: 1,
        page_size: 20,
      }),
    })
  })

  await page.route('**/api/invoices/42/receivable', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        receivable: {
          billing_document_id: 42,
          document_no: 'INV-042',
          document_type: 'invoice',
          tenant_name: 'Harbor Foods',
          lease_contract_id: 42,
          outstanding_amount: 620,
          customer_surplus_available: 0,
          settlement_status: 'outstanding',
          items: [
            {
              id: 501,
              lease_contract_id: 42,
              billing_document_id: 42,
              billing_document_line_id: 4201,
              customer_id: 101,
              department_id: 10,
              trade_id: 102,
              charge_type: 'overtime_rent',
              charge_source: 'overtime',
              overtime_bill_id: 701,
              overtime_formula_id: 801,
              overtime_charge_id: 901,
              due_date: '2026-05-31',
              outstanding_amount: 620,
              settled_at: null,
              is_deposit: false,
              created_at: '2026-05-03T09:00:00Z',
              updated_at: '2026-05-03T09:00:00Z',
            },
          ],
          payment_history: [],
          discount_history: [],
          surplus_history: [],
          interest_history: [],
          deposit_application_history: [],
          deposit_refund_history: [],
        },
      }),
    })
  })

  await page.route('**/api/invoices/42', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        document: {
          id: 42,
          document_type: 'invoice',
          document_no: 'INV-042',
          billing_run_id: 301,
          lease_contract_id: 42,
          tenant_name: 'Harbor Foods',
          period_start: '2026-05-01',
          period_end: '2026-05-31',
          total_amount: 620,
          currency_type_id: 101,
          status: 'approved',
          workflow_instance_id: 81,
          adjusted_from_id: null,
          submitted_at: '2026-05-03T09:00:00Z',
          approved_at: '2026-05-03T09:10:00Z',
          cancelled_at: null,
          created_by: 1,
          updated_by: 1,
          created_at: '2026-05-03T09:00:00Z',
          updated_at: '2026-05-03T09:10:00Z',
          lines: [
            {
              id: 4201,
              billing_document_id: 42,
              billing_charge_line_id: 11,
              charge_type: 'overtime_rent',
              charge_source: 'overtime',
              overtime_bill_id: 701,
              overtime_formula_id: 801,
              overtime_charge_id: 901,
              period_start: '2026-05-01',
              period_end: '2026-05-31',
              quantity_days: 31,
              unit_amount: 20,
              amount: 620,
              created_at: '2026-05-03T09:00:00Z',
            },
          ],
        },
      }),
    })
  })

  await login(page)

  await page.goto('/billing/charges')
  await expect(page.getByTestId('charges-table')).toContainText('OT')
  await expect(page.getByTestId('charges-table')).toContainText('overtime_rent')

  await page.goto('/billing/invoices/42')
  await expect(page.getByTestId('invoice-detail-view')).toContainText('OT')
  await expect(page.getByTestId('invoice-open-items-table')).toContainText('overtime_rent')
})
