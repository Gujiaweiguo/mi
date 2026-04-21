import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'
import type { InvoiceDocument, InvoiceReceivable, ReceivableListItem } from '../src/api/invoice'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

const now = '2026-04-20T09:00:00Z'

const attachPaymentReceivableMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    { function_code: 'billing.invoice', permission_level: 'edit', can_print: true, can_export: true },
  ]

  const invoice: InvoiceDocument = {
    id: 501,
    document_type: 'invoice',
    document_no: 'INV-2026-0501',
    billing_run_id: 7001,
    lease_contract_id: 3001,
    tenant_name: 'Harbor Foods',
    period_start: '2026-04-01',
    period_end: '2026-04-30',
    total_amount: 12000,
    currency_type_id: 1,
    status: 'approved',
    workflow_instance_id: 9101,
    adjusted_from_id: null,
    submitted_at: '2026-04-01T09:00:00Z',
    approved_at: '2026-04-02T10:00:00Z',
    cancelled_at: null,
    created_by: 1,
    updated_by: 1,
    created_at: '2026-04-01T09:00:00Z',
    updated_at: '2026-04-02T10:00:00Z',
    lines: [
      {
        id: 801,
        billing_document_id: 501,
        billing_charge_line_id: 601,
        charge_type: 'rent',
        period_start: '2026-04-01',
        period_end: '2026-04-30',
        quantity_days: 30,
        unit_amount: 12000,
        amount: 12000,
        created_at: '2026-04-01T09:00:00Z',
      },
    ],
  }

  const receivable: InvoiceReceivable = {
    billing_document_id: 501,
    document_no: invoice.document_no,
    document_type: invoice.document_type,
    tenant_name: invoice.tenant_name,
    lease_contract_id: invoice.lease_contract_id,
    outstanding_amount: 12000,
    settlement_status: 'outstanding',
    items: [
      {
        id: 4001,
        lease_contract_id: invoice.lease_contract_id,
        billing_document_id: invoice.id,
        billing_document_line_id: 801,
        customer_id: 101,
        department_id: 101,
        trade_id: 102,
        charge_type: 'rent',
        due_date: '2026-04-30',
        outstanding_amount: 12000,
        settled_at: null,
        is_deposit: false,
        created_at: now,
        updated_at: now,
      },
    ],
    payment_history: [],
  }

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'operator',
          display_name: 'Operator One',
          department_id: 101,
          roles: ['ops'],
          permissions,
        },
      }),
    })
  })

  await page.route('**/api/auth/login', async (route) => { await route.fulfill({
    status: 200,
    contentType: 'application/json',
    body: JSON.stringify({ token: 'playwright-token' }),
  }) })
  
  await page.route('**/api/dashboard/summary', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        activeLeases: 0,
        pendingLeaseApprovals: 0,
        pendingInvoiceApprovals: 0,
        openReceivables: 0,
        overdueReceivables: 0,
        pendingWorkflows: 0,
      }),
    })
  })

  await page.route('**/api/health', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ status: 'ok', service: 'frontend-platform-test' }),
    })
  })

  await page.route('**/api/receivables**', async (route) => {
    const items: ReceivableListItem[] =
      receivable.outstanding_amount > 0
        ? [
            {
              billing_document_id: invoice.id,
              document_type: invoice.document_type,
              document_no: invoice.document_no,
              tenant_name: invoice.tenant_name,
              document_status: invoice.status,
              lease_contract_id: invoice.lease_contract_id,
              customer_id: 101,
              department_id: 101,
              trade_id: 102,
              earliest_due_date: '2026-04-30',
              latest_due_date: '2026-04-30',
              outstanding_amount: receivable.outstanding_amount,
              settlement_status: receivable.settlement_status,
            },
          ]
        : []

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items,
        total: items.length,
        page: 1,
        page_size: 20,
      }),
    })
  })

  await page.route('**/api/invoices**', async (route) => {
    const requestUrl = new URL(route.request().url())
    const pathname = requestUrl.pathname
    const method = route.request().method()

    if (method === 'GET' && pathname.endsWith(`/api/invoices/${invoice.id}`)) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ document: invoice }),
      })
      return
    }

    if (method === 'GET' && pathname.endsWith(`/api/invoices/${invoice.id}/receivable`)) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${invoice.id}/payments`)) {
      const request = route.request().postDataJSON() as {
        amount: number
        payment_date?: string
        note?: string
        idempotency_key: string
      }

      if (request.amount > receivable.outstanding_amount) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'payment amount exceeds outstanding receivable balance' }),
        })
        return
      }

      receivable.outstanding_amount = Math.round((receivable.outstanding_amount - request.amount) * 100) / 100
      receivable.items[0].outstanding_amount = receivable.outstanding_amount
      receivable.items[0].settled_at = receivable.outstanding_amount === 0 ? now : null
      receivable.settlement_status = receivable.outstanding_amount === 0 ? 'settled' : 'outstanding'
      receivable.payment_history.unshift({
        id: 9000 + receivable.payment_history.length + 1,
        billing_document_id: invoice.id,
        lease_contract_id: invoice.lease_contract_id,
        payment_date: request.payment_date ?? '2026-04-20',
        amount: request.amount,
        note: request.note ?? null,
        recorded_by: 1,
        idempotency_key: request.idempotency_key,
        created_at: now,
      })

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    await route.fallback()
  })
}

test('supports receivable review, payment entry, settlement updates, and over-application feedback', async ({ page }) => {
  await attachPaymentReceivableMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/dashboard/)
  await page.getByTestId('nav--billing-receivables').click()

  await expect(page.getByTestId('receivables-view')).toBeVisible()
  await expect(page.getByTestId('receivables-table')).toContainText('INV-2026-0501')
  await expect(page.getByTestId('receivables-table')).toContainText('12,000.00')

  await page.getByTestId('receivable-row-view-button-501').click()

  await expect(page).toHaveURL(/\/billing\/invoices\/501/)
  await expect(page.getByTestId('invoice-detail-view')).toBeVisible()
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('12,000.00')

  const paymentAmountInput = page.getByTestId('invoice-payment-amount-input').locator('input')
  await paymentAmountInput.fill('7000')
  await page.getByTestId('invoice-payment-note-input').fill('first payment')
  await page.getByTestId('invoice-payment-date-input').locator('input').fill('2026-04-21')
  await page.getByTestId('invoice-payment-submit-button').click()

  await expect(page.getByTestId('invoice-detail-success-alert')).toBeVisible()
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('5,000.00')
  await expect(page.getByTestId('invoice-cancel-button')).toBeDisabled()

  await paymentAmountInput.fill('6000')
  await page.getByTestId('invoice-payment-date-input').locator('input').fill('2026-04-21')
  await page.getByTestId('invoice-payment-submit-button').click()

  await expect(page.getByTestId('invoice-detail-error-alert')).toContainText(
    'payment amount exceeds outstanding receivable balance',
  )
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('5,000.00')

  await paymentAmountInput.fill('5000')
  await page.getByTestId('invoice-payment-date-input').locator('input').fill('2026-04-21')
  await page.getByTestId('invoice-payment-submit-button').click()

  await expect(page.getByTestId('invoice-receivable-settled-tag')).toBeVisible()

  await page.getByTestId('nav--billing-receivables').click()
  await expect(page.getByTestId('receivables-view')).toBeVisible()
  await expect(page.getByTestId('receivable-row-view-button-501')).toHaveCount(0)
})
