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

const roundAmount = (value: number) => Math.round(value * 100) / 100

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

  const depositInvoice: InvoiceDocument = {
    id: 601,
    document_type: 'invoice',
    document_no: 'INV-DEP-0601',
    billing_run_id: 7002,
    lease_contract_id: 3001,
    tenant_name: 'Harbor Foods',
    period_start: '2026-04-01',
    period_end: '2026-04-30',
    total_amount: 5000,
    currency_type_id: 1,
    status: 'approved',
    workflow_instance_id: 9102,
    adjusted_from_id: null,
    submitted_at: '2026-04-01T09:30:00Z',
    approved_at: '2026-04-02T10:30:00Z',
    cancelled_at: null,
    created_by: 1,
    updated_by: 1,
    created_at: '2026-04-01T09:30:00Z',
    updated_at: '2026-04-02T10:30:00Z',
    lines: [
      {
        id: 802,
        billing_document_id: 601,
        billing_charge_line_id: 602,
        charge_type: 'deposit',
        period_start: '2026-04-01',
        period_end: '2026-04-30',
        quantity_days: 30,
        unit_amount: 5000,
        amount: 5000,
        created_at: '2026-04-01T09:30:00Z',
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
    customer_surplus_available: 1000,
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
    discount_history: [],
    surplus_history: [
      {
        id: 9801,
        surplus_balance_id: 7001,
        entry_type: 'overpayment',
        customer_id: 101,
        billing_document_id: 499,
        ar_open_item_id: null,
        amount: 1000,
        note: 'prior overpayment',
        idempotency_key: 'seed-surplus-1',
        recorded_by: 1,
        created_at: now,
      },
    ],
    interest_history: [],
    deposit_application_history: [],
    deposit_refund_history: [],
  }

  const depositReceivable: InvoiceReceivable = {
    billing_document_id: 601,
    document_no: depositInvoice.document_no,
    document_type: depositInvoice.document_type,
    tenant_name: depositInvoice.tenant_name,
    lease_contract_id: depositInvoice.lease_contract_id,
    outstanding_amount: 5000,
    customer_surplus_available: 1000,
    settlement_status: 'outstanding',
    items: [
      {
        id: 4002,
        lease_contract_id: depositInvoice.lease_contract_id,
        billing_document_id: depositInvoice.id,
        billing_document_line_id: 802,
        customer_id: 101,
        department_id: 101,
        trade_id: 102,
        charge_type: 'deposit',
        due_date: '2026-04-15',
        outstanding_amount: 5000,
        settled_at: null,
        is_deposit: true,
        created_at: now,
        updated_at: now,
      },
    ],
    payment_history: [],
    discount_history: [],
    surplus_history: [],
    interest_history: [],
    deposit_application_history: [],
    deposit_refund_history: [],
  }

  const documents = new Map<number, InvoiceDocument>([
    [invoice.id, invoice],
    [depositInvoice.id, depositInvoice],
  ])

  const receivables = new Map<number, InvoiceReceivable>([
    [invoice.id, receivable],
    [depositInvoice.id, depositReceivable],
  ])

  const syncReceivableSettlement = (targetReceivable: InvoiceReceivable, nextOutstanding: number) => {
    const outstanding = roundAmount(nextOutstanding)
    targetReceivable.outstanding_amount = outstanding
    targetReceivable.items[0].outstanding_amount = outstanding
    targetReceivable.items[0].settled_at = outstanding === 0 ? now : null
    targetReceivable.settlement_status = outstanding === 0 ? 'settled' : 'outstanding'
  }

  const buildReceivableListItems = (): ReceivableListItem[] =>
    Array.from(receivables.values())
      .filter((item) => item.outstanding_amount > 0)
      .map((item) => ({
        billing_document_id: item.billing_document_id,
        document_type: item.document_type,
        document_no: item.document_no,
        tenant_name: item.tenant_name,
        document_status: 'approved',
        lease_contract_id: item.lease_contract_id,
        customer_id: item.items[0]?.customer_id ?? 101,
        department_id: item.items[0]?.department_id ?? 101,
        trade_id: item.items[0]?.trade_id ?? 102,
        earliest_due_date: item.items[0]?.due_date ?? '2026-04-30',
        latest_due_date: item.items[0]?.due_date ?? '2026-04-30',
        outstanding_amount: item.outstanding_amount,
        settlement_status: item.settlement_status,
      }))

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

  await page.route('**/api/auth/login', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ token: 'playwright-token' }),
    })
  })

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
    const items = buildReceivableListItems()

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
    const invoiceIdMatch = pathname.match(/\/api\/invoices\/(\d+)/)
    const documentId = invoiceIdMatch ? Number(invoiceIdMatch[1]) : null

    if (method === 'GET' && documentId !== null && pathname.endsWith(`/api/invoices/${documentId}`)) {
      const document = documents.get(documentId)
      if (!document) {
        await route.fallback()
        return
      }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ document }),
      })
      return
    }

    if (method === 'GET' && documentId !== null && pathname.endsWith(`/api/invoices/${documentId}/receivable`)) {
      const targetReceivable = receivables.get(documentId)
      if (!targetReceivable) {
        await route.fallback()
        return
      }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable: targetReceivable }),
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

      const applied = Math.min(request.amount, receivable.outstanding_amount)
      const surplusCreated = roundAmount(request.amount - applied)

      syncReceivableSettlement(receivable, receivable.outstanding_amount - applied)
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

      if (surplusCreated > 0) {
        receivable.customer_surplus_available = roundAmount(receivable.customer_surplus_available + surplusCreated)
        receivable.surplus_history.unshift({
          id: 9800 + receivable.surplus_history.length + 1,
          surplus_balance_id: 7001,
          entry_type: 'overpayment',
          customer_id: 101,
          billing_document_id: invoice.id,
          ar_open_item_id: null,
          amount: surplusCreated,
          note: request.note ?? null,
          idempotency_key: request.idempotency_key,
          recorded_by: 1,
          created_at: now,
        })
      }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${invoice.id}/surplus-applications`)) {
      const request = route.request().postDataJSON() as {
        billing_document_line_id: number
        amount: number
        note?: string
        idempotency_key: string
      }

      if (request.amount > receivable.customer_surplus_available) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'customer surplus balance is insufficient' }),
        })
        return
      }
      if (request.amount > receivable.items[0].outstanding_amount) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'surplus application target is not allowed' }),
        })
        return
      }

      receivable.customer_surplus_available = roundAmount(receivable.customer_surplus_available - request.amount)
      syncReceivableSettlement(receivable, receivable.outstanding_amount - request.amount)
      receivable.surplus_history.unshift({
        id: 9800 + receivable.surplus_history.length + 1,
        surplus_balance_id: 7001,
        entry_type: 'application',
        customer_id: 101,
        billing_document_id: invoice.id,
        ar_open_item_id: receivable.items[0].id,
        amount: request.amount,
        note: request.note ?? null,
        idempotency_key: request.idempotency_key,
        recorded_by: 1,
        created_at: now,
      })

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${invoice.id}/interest`)) {
      const request = route.request().postDataJSON() as {
        billing_document_line_id: number
        as_of_date?: string
        idempotency_key: string
      }

      receivable.interest_history.unshift({
        id: 9950 + receivable.interest_history.length + 1,
        source_ar_open_item_id: receivable.items[0].id,
        source_billing_document_id: invoice.id,
        source_billing_document_line_id: request.billing_document_line_id,
        generated_billing_document_id: 8800 + receivable.interest_history.length + 1,
        generated_billing_document_line_id: 8900 + receivable.interest_history.length + 1,
        charge_type: 'late_payment_interest',
        principal_amount: receivable.items[0].outstanding_amount,
        daily_rate: 0.001,
        grace_days: 7,
        covered_start_date: '2026-05-08',
        covered_end_date: request.as_of_date ?? '2026-05-10',
        interest_days: 3,
        interest_amount: 33,
        idempotency_key: request.idempotency_key,
        created_by: 1,
        created_at: now,
      })

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${invoice.id}/discounts`)) {
      const request = route.request().postDataJSON() as {
        billing_document_line_id: number
        amount: number
        reason: string
        idempotency_key: string
      }

      if (request.amount > receivable.items[0].outstanding_amount) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'discount amount exceeds outstanding receivable balance' }),
        })
        return
      }

      receivable.discount_history.unshift({
        id: 9500 + receivable.discount_history.length + 1,
        billing_document_id: invoice.id,
        billing_document_line_id: request.billing_document_line_id,
        lease_contract_id: invoice.lease_contract_id,
        charge_type: 'rent',
        requested_amount: request.amount,
        requested_rate: request.amount / invoice.lines[0].amount,
        reason: request.reason,
        status: 'pending_approval',
        workflow_instance_id: 9900 + receivable.discount_history.length + 1,
        idempotency_key: request.idempotency_key,
        submitted_at: now,
        approved_at: null,
        rejected_at: null,
        created_by: 1,
        updated_by: 1,
        created_at: now,
        updated_at: now,
      })

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${depositInvoice.id}/deposit-applications`)) {
      const request = route.request().postDataJSON() as {
        billing_document_line_id: number
        target_document_id: number
        target_billing_document_line_id: number
        amount: number
        note?: string
        idempotency_key: string
      }

      if (request.amount > depositReceivable.items[0].outstanding_amount) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'deposit balance is insufficient' }),
        })
        return
      }
      if (request.target_document_id !== invoice.id || request.amount > receivable.items[0].outstanding_amount) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'deposit application target is not allowed' }),
        })
        return
      }

      syncReceivableSettlement(depositReceivable, depositReceivable.outstanding_amount - request.amount)
      depositReceivable.deposit_application_history.unshift({
        id: 9970 + depositReceivable.deposit_application_history.length + 1,
        source_billing_document_id: depositInvoice.id,
        source_billing_document_line_id: request.billing_document_line_id,
        source_ar_open_item_id: depositReceivable.items[0].id,
        target_billing_document_id: request.target_document_id,
        target_billing_document_line_id: request.target_billing_document_line_id,
        target_ar_open_item_id: receivable.items[0].id,
        lease_contract_id: invoice.lease_contract_id,
        amount: request.amount,
        note: request.note ?? null,
        idempotency_key: request.idempotency_key,
        applied_by: 1,
        created_at: now,
      })

      syncReceivableSettlement(receivable, receivable.outstanding_amount - request.amount)

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${depositInvoice.id}/deposit-refunds`)) {
      const request = route.request().postDataJSON() as {
        billing_document_line_id: number
        amount: number
        reason: string
        idempotency_key: string
      }

      if (receivable.outstanding_amount > 0) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'deposit refund is blocked by outstanding obligations' }),
        })
        return
      }
      if (request.amount > depositReceivable.items[0].outstanding_amount) {
        await route.fulfill({
          status: 409,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'deposit refund amount exceeds available balance' }),
        })
        return
      }

      syncReceivableSettlement(depositReceivable, depositReceivable.outstanding_amount - request.amount)
      depositReceivable.deposit_refund_history.unshift({
        id: 9980 + depositReceivable.deposit_refund_history.length + 1,
        billing_document_id: depositInvoice.id,
        billing_document_line_id: request.billing_document_line_id,
        ar_open_item_id: depositReceivable.items[0].id,
        lease_contract_id: depositInvoice.lease_contract_id,
        amount: request.amount,
        reason: request.reason,
        idempotency_key: request.idempotency_key,
        refunded_by: 1,
        created_at: now,
      })

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ receivable: depositReceivable }),
      })
      return
    }

    await route.fallback()
  })
}

test('supports receivable review, deposit application, payment entry, settlement updates, and over-application feedback', async ({ page }) => {
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
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('1,000.00')

  await page.getByTestId('invoice-interest-submit-button').click()

  await expect(page.getByTestId('invoice-interest-history-table')).toContainText('33.00')
  await expect(page.getByTestId('invoice-interest-history-table')).toContainText('2026-05-08')

  const depositAmountInput = page.getByTestId('invoice-deposit-amount-input').locator('input')
  await depositAmountInput.fill('500')
  await page.getByTestId('invoice-deposit-note-input').fill('apply deposit')
  await page.getByTestId('invoice-deposit-submit-button').click()

  await expect(page.getByTestId('invoice-detail-success-alert')).toContainText('保证金冲抵成功。')
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('11,500.00')
  await expect(page.getByTestId('invoice-deposit-application-history-table')).toContainText('apply deposit')
  await expect(page.getByTestId('invoice-deposit-application-history-table')).toContainText('501')

  const surplusAmountInput = page.getByTestId('invoice-surplus-amount-input').locator('input')
  await surplusAmountInput.fill('1000')
  await page.getByTestId('invoice-surplus-note-input').fill('apply surplus')
  await page.getByTestId('invoice-surplus-submit-button').click()

  await expect(page.getByTestId('invoice-detail-success-alert')).toContainText('余额冲抵成功。')
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('10,500.00')
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('0.00')
  await expect(page.getByTestId('invoice-surplus-history-table')).toContainText('apply surplus')

  const discountAmountInput = page.getByTestId('invoice-discount-amount-input').locator('input')
  await discountAmountInput.fill('1200')
  await page.getByTestId('invoice-discount-reason-input').fill('launch support')
  await page.getByTestId('invoice-discount-submit-button').click()

  await expect(page.getByTestId('invoice-detail-success-alert')).toContainText('折扣申请已提交审批。')
  await expect(page.getByTestId('invoice-discount-pending-tag')).toBeVisible()
  await expect(page.getByTestId('invoice-discount-history-table')).toContainText('launch support')
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('10,500.00')

  const paymentAmountInput = page.getByTestId('invoice-payment-amount-input').locator('input')
  await paymentAmountInput.fill('7000')
  await page.getByTestId('invoice-payment-note-input').fill('first payment')
  await page.getByTestId('invoice-payment-submit-button').click()

  await expect(page.getByTestId('invoice-detail-success-alert')).toBeVisible()
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('3,500.00')
  await expect(page.getByTestId('invoice-cancel-button')).toBeDisabled()

  await paymentAmountInput.fill('5500')
  await page.getByTestId('invoice-payment-submit-button').click()

  await expect(page.getByTestId('invoice-receivable-settled-tag')).toBeVisible()
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('2,000.00')
  await expect(page.getByTestId('invoice-surplus-history-table')).toContainText('超额收款')
  await expect(page.getByTestId('invoice-surplus-history-table')).toContainText('2,000.00')

  await page.getByTestId('nav--billing-receivables').click()
  await expect(page.getByTestId('receivables-view')).toBeVisible()
  await expect(page.getByTestId('receivable-row-view-button-501')).toHaveCount(0)
})

test('blocks deposit refund until obligations clear, then allows refunding the remaining deposit', async ({ page }) => {
  await attachPaymentReceivableMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/dashboard/)
  await page.goto('/billing/invoices/601')

  await expect(page.getByTestId('invoice-detail-view')).toBeVisible()
  await expect(page.getByTestId('invoice-deposit-refund-history-table')).toBeVisible()

  const refundAmountInput = page.getByTestId('invoice-deposit-refund-amount-input').locator('input')
  await refundAmountInput.fill('500')
  await page.getByTestId('invoice-deposit-refund-reason-input').fill('lease ended')
  await page.getByTestId('invoice-deposit-refund-submit-button').click()

  await expect(page.getByTestId('invoice-detail-error-alert')).toContainText('deposit refund is blocked by outstanding obligations')

  await page.goto('/billing/invoices/501')
  await expect(page.getByTestId('invoice-detail-view')).toBeVisible()

  const depositAmountInput = page.getByTestId('invoice-deposit-amount-input').locator('input')
  await depositAmountInput.fill('1000')
  await page.getByTestId('invoice-deposit-note-input').fill('clear obligations')
  await page.getByTestId('invoice-deposit-submit-button').click()
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('11,000.00')

  const paymentAmountInput = page.getByTestId('invoice-payment-amount-input').locator('input')
  await paymentAmountInput.fill('11000')
  await page.getByTestId('invoice-payment-submit-button').click()
  await expect(page.getByTestId('invoice-receivable-settled-tag')).toBeVisible()

  await page.goto('/billing/invoices/601')
  await expect(page.getByTestId('invoice-detail-view')).toBeVisible()

  await refundAmountInput.fill('4000')
  await page.getByTestId('invoice-deposit-refund-reason-input').fill('lease ended')
  await page.getByTestId('invoice-deposit-refund-submit-button').click()

  await expect(page.getByTestId('invoice-detail-success-alert')).toContainText('保证金退款成功。')
  await expect(page.getByTestId('invoice-deposit-refund-history-table')).toContainText('lease ended')
  await expect(page.getByTestId('invoice-deposit-refund-history-table')).toContainText('4,000.00')
  await expect(page.getByTestId('invoice-receivable-summary')).toContainText('0.00')
  await expect(page.getByTestId('invoice-deposit-refund-empty-tag')).toBeVisible()
})
