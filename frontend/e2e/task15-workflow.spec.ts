import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'
import type { ChargeLine } from '../src/api/billing'
import type { InvoiceDocument } from '../src/api/invoice'
import type { LeaseContract } from '../src/api/lease'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

const now = '2026-04-01T09:00:00Z'

const attachTask15Mocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    { function_code: 'lease.contract', permission_level: 'edit', can_print: true, can_export: false },
    { function_code: 'billing.charge', permission_level: 'edit', can_print: false, can_export: true },
    { function_code: 'billing.invoice', permission_level: 'edit', can_print: true, can_export: true },
    { function_code: 'tax.export', permission_level: 'edit', can_print: false, can_export: true },
    { function_code: 'excel.io', permission_level: 'edit', can_print: false, can_export: true },
  ]

  let lease: LeaseContract = {
    id: 101,
    amended_from_id: null,
    lease_no: 'LS-2026-001',
    department_id: 10,
    store_id: 5,
    building_id: null,
    customer_id: 101,
    brand_id: 101,
    trade_id: 102,
    management_type_id: 101,
    tenant_name: 'Harbor Foods',
    start_date: '2026-01-01',
    end_date: '2026-12-31',
    status: 'draft',
    workflow_instance_id: null,
    effective_version: 1,
    submitted_at: null,
    approved_at: null,
    billing_effective_at: null,
    terminated_at: null,
    created_by: 1,
    updated_by: 1,
    created_at: now,
    updated_at: now,
    units: [
      {
        id: 201,
        lease_contract_id: 101,
        unit_id: 301,
        rent_area: 120,
        created_at: now,
        updated_at: now,
      },
    ],
    terms: [
      {
        id: 401,
        lease_contract_id: 101,
        term_type: 'rent',
        billing_cycle: 'monthly',
        currency_type_id: 1,
        amount: 8000,
        effective_from: '2026-01-01',
        effective_to: '2026-12-31',
        created_at: now,
        updated_at: now,
      },
    ],
  }

  let chargeLines: ChargeLine[] = [
    {
      id: 601,
      billing_run_id: 701,
      lease_contract_id: lease.id,
      lease_no: lease.lease_no,
      tenant_name: lease.tenant_name,
      lease_term_id: 401,
      charge_type: 'rent',
      period_start: '2026-01-01',
      period_end: '2026-01-31',
      quantity_days: 31,
      unit_amount: 8000,
      amount: 8000,
      currency_type_id: 1,
      source_effective_version: 1,
      created_at: now,
    },
  ]

  let invoice: InvoiceDocument = {
    id: 501,
    document_type: 'invoice',
    document_no: 'INV-2026-0001',
    billing_run_id: 701,
    lease_contract_id: lease.id,
    tenant_name: lease.tenant_name,
    period_start: '2026-01-01',
    period_end: '2026-01-31',
    total_amount: 8000,
    currency_type_id: 1,
    status: 'draft',
    workflow_instance_id: null,
    adjusted_from_id: null,
    submitted_at: null,
    approved_at: null,
    cancelled_at: null,
    created_by: 1,
    updated_by: 1,
    created_at: now,
    updated_at: now,
    lines: [
      {
        id: 801,
        billing_document_id: 501,
        billing_charge_line_id: 601,
        charge_type: 'rent',
        period_start: '2026-01-01',
        period_end: '2026-01-31',
        quantity_days: 31,
        unit_amount: 8000,
        amount: 8000,
        created_at: now,
      },
    ],
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
          department_id: 100,
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

  await page.route('**/api/health', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ status: 'ok', service: 'frontend-platform-test' }),
    })
  })

  await page.route('**/api/org/departments', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        departments: [
          {
            id: 10,
            code: 'OPS',
            name: 'Operations',
            level: 1,
            status: 'active',
            parent_id: null,
            type_id: 1,
          },
        ],
      }),
    })
  })

  await page.route('**/api/org/stores', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        stores: [
          {
            id: 5,
            department_id: 10,
            code: 'MI-001',
            name: 'MI Demo Mall',
            short_name: 'MI Mall',
            status: 'active',
          },
        ],
      }),
    })
  })

  await page.route('**/api/master-data/customers', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        customers: [
          {
            id: 101,
            code: 'CUST-101',
            name: 'Harbor Foods',
            trade_id: 102,
            department_id: 10,
            status: 'active',
            created_at: now,
            updated_at: now,
          },
        ],
      }),
    })
  })

  await page.route('**/api/master-data/brands', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        brands: [
          {
            id: 101,
            code: 'BR-101',
            name: 'Harbor Signature',
            status: 'active',
            created_at: now,
            updated_at: now,
          },
        ],
      }),
    })
  })

  await page.route('**/api/leases**', async (route) => {
    const url = new URL(route.request().url())
    const pathname = url.pathname
    const method = route.request().method()

    if (method === 'POST' && pathname.endsWith('/api/leases')) {
      const request = route.request().postDataJSON() as {
        lease_no: string
        tenant_name: string
        department_id: number
        store_id: number
        customer_id?: number | null
        brand_id?: number | null
        trade_id?: number | null
        management_type_id?: number | null
        start_date: string
        end_date: string
        units: Array<{ unit_id: number; rent_area: number }>
        terms: Array<{
          term_type: string
          billing_cycle: string
          currency_type_id: number
          amount: number
          effective_from: string
          effective_to: string
        }>
      }

      lease = {
        ...lease,
        lease_no: request.lease_no,
        tenant_name: request.tenant_name,
        department_id: request.department_id,
        store_id: request.store_id,
        customer_id: request.customer_id ?? null,
        brand_id: request.brand_id ?? null,
        trade_id: request.trade_id ?? null,
        management_type_id: request.management_type_id ?? null,
        start_date: request.start_date,
        end_date: request.end_date,
        units: [
          {
            id: 201,
            lease_contract_id: lease.id,
            unit_id: request.units[0].unit_id,
            rent_area: request.units[0].rent_area,
            created_at: now,
            updated_at: now,
          },
        ],
        terms: [
          {
            id: 401,
            lease_contract_id: lease.id,
            term_type: request.terms[0].term_type,
            billing_cycle: request.terms[0].billing_cycle,
            currency_type_id: request.terms[0].currency_type_id,
            amount: request.terms[0].amount,
            effective_from: request.terms[0].effective_from,
            effective_to: request.terms[0].effective_to,
            created_at: now,
            updated_at: now,
          },
        ],
      }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ lease }),
      })
      return
    }

    if (method === 'POST' && pathname.endsWith(`/api/leases/${lease.id}/submit`)) {
      lease = {
        ...lease,
        status: 'pending_approval',
        workflow_instance_id: 9001,
        submitted_at: now,
      }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ lease }),
      })
      return
    }

    if (method === 'GET' && pathname.endsWith(`/api/leases/${lease.id}`)) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ lease }),
      })
      return
    }

    if (method === 'GET' && pathname.endsWith('/api/leases')) {
      const leaseNo = url.searchParams.get('lease_no')?.toLowerCase() ?? ''
      const items = leaseNo
        ? [lease].filter(
            (item) =>
              item.lease_no.toLowerCase().includes(leaseNo) || item.tenant_name.toLowerCase().includes(leaseNo),
          )
        : [lease]

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          items: items.map((item) => ({
            id: item.id,
            lease_no: item.lease_no,
            tenant_name: item.tenant_name,
            department_id: item.department_id,
            store_id: item.store_id,
            building_id: item.building_id,
            start_date: item.start_date,
            end_date: item.end_date,
            status: item.status,
            workflow_instance_id: item.workflow_instance_id,
            billing_effective_at: item.billing_effective_at,
            updated_at: item.updated_at,
          })),
          total: items.length,
          page: 1,
          page_size: 20,
        }),
      })
      return
    }

    await route.fallback()
  })

  await page.route('**/api/billing/charges**', async (route) => {
    const pathname = new URL(route.request().url()).pathname
    const method = route.request().method()

    if (method === 'POST' && pathname.endsWith('/api/billing/charges/generate')) {
      lease = {
        ...lease,
        billing_effective_at: now,
      }

      chargeLines = [
        {
          id: 601,
          billing_run_id: 701,
          lease_contract_id: lease.id,
          lease_no: lease.lease_no,
          tenant_name: lease.tenant_name,
          lease_term_id: 401,
          charge_type: 'rent',
          period_start: '2026-01-01',
          period_end: '2026-01-31',
          quantity_days: 31,
          unit_amount: 8000,
          amount: 8000,
          currency_type_id: 1,
          source_effective_version: 1,
          created_at: now,
        },
      ]

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          run: {
            id: 701,
            period_start: '2026-01-01',
            period_end: '2026-01-31',
            status: 'completed',
            triggered_by: 1,
            generated_count: 1,
            skipped_count: 0,
            created_at: now,
            updated_at: now,
          },
          lines: chargeLines,
          totals: { generated: 1, skipped: 0 },
        }),
      })
      return
    }

    if (method === 'GET' && pathname.endsWith('/api/billing/charges')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ items: chargeLines, total: chargeLines.length, page: 1, page_size: 20 }),
      })
      return
    }

    await route.fallback()
  })

  await page.route('**/api/invoices**', async (route) => {
    const pathname = new URL(route.request().url()).pathname
    const method = route.request().method()

    if (method === 'POST' && pathname.endsWith(`/api/invoices/${invoice.id}/submit`)) {
      invoice = {
        ...invoice,
        status: 'pending_approval',
        workflow_instance_id: 9101,
        submitted_at: now,
      }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ document: invoice }),
      })
      return
    }

    if (method === 'GET' && pathname.endsWith(`/api/invoices/${invoice.id}`)) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ document: invoice }),
      })
      return
    }

    if (method === 'GET' && pathname.endsWith('/api/invoices')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ items: [invoice], total: 1, page: 1, page_size: 20 }),
      })
      return
    }

    await route.fallback()
  })

  await page.route('**/api/tax/rule-sets**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: [
          {
            id: 1001,
            code: 'TAX-LEASE',
            name: 'Lease voucher export',
            document_type: 'invoice',
            status: 'active',
            created_by: 1,
            updated_by: 1,
            created_at: now,
            updated_at: now,
            rules: [
              {
                id: 1101,
                rule_set_id: 1001,
                sequence_no: 1,
                entry_side: 'debit',
                charge_type_filter: 'rent',
                account_number: '6001',
                account_name: 'Rent revenue',
                explanation_template: 'Lease rent',
                use_tenant_name: true,
                is_balancing_entry: false,
                created_at: now,
                updated_at: now,
              },
            ],
          },
        ],
        total: 1,
        page: 1,
        page_size: 20,
      }),
    })
  })

  await page.route('**/api/tax/exports/vouchers', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/octet-stream',
      body: Buffer.from('mock tax export'),
    })
  })

  await page.route('**/api/print/templates**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: [
          {
            id: 1201,
            code: 'INVOICE_STD',
            name: 'Invoice standard',
            document_type: 'invoice',
            output_mode: 'pdf',
            status: 'active',
            title: 'Invoice',
            subtitle: 'Billing document',
            header_lines: ['MI Migration'],
            footer_lines: ['Generated in test'],
            created_by: 1,
            updated_by: 1,
            created_at: now,
            updated_at: now,
          },
        ],
        total: 1,
        page: 1,
        page_size: 20,
      }),
    })
  })

  await page.route('**/api/print/render/pdf', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/pdf',
      body: Buffer.from('%PDF-1.4 mock pdf'),
    })
  })

  await page.route('**/api/excel/exports/operational**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/octet-stream',
      body: Buffer.from('mock excel export'),
    })
  })
}

test('completes the task 15 lease to billing output workflow with stable test ids', async ({ page }) => {
  await attachTask15Mocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)

  await page.getByTestId('nav--lease-contracts').click()
  await expect(page.getByTestId('lease-contracts-view')).toBeVisible()
  await page.getByTestId('lease-create-button').click()

  await expect(page.getByTestId('lease-create-view')).toBeVisible()
  const leaseForm = page.getByTestId('lease-create-form')
  await leaseForm.getByPlaceholder('Enter lease number').fill('LS-2026-001')
  await leaseForm.getByPlaceholder('Enter tenant name').fill('Harbor Foods')
  await leaseForm.getByTestId('lease-department-select').click()
  await page.getByRole('option', { name: /OPS — Operations/ }).click()
  await leaseForm.getByTestId('lease-store-select').click()
  await page.getByRole('option', { name: /MI-001 — MI Demo Mall/ }).click()
  await leaseForm.getByTestId('lease-customer-select').click()
  await page.getByRole('option', { name: /CUST-101 — Harbor Foods/ }).click()
  await leaseForm.getByTestId('lease-brand-select').click()
  await page.getByRole('option', { name: /BR-101 — Harbor Signature/ }).click()
  await leaseForm.getByRole('spinbutton').nth(0).fill('102')
  await leaseForm.getByRole('spinbutton').nth(1).fill('101')
  await leaseForm.getByPlaceholder('Select start date').fill('2026-01-01')
  await leaseForm.getByPlaceholder('Select end date').fill('2026-12-31')
  await leaseForm.getByRole('spinbutton').nth(2).fill('301')
  await leaseForm.getByRole('spinbutton').nth(3).fill('120')
  await leaseForm.getByRole('spinbutton').nth(4).fill('1')
  await leaseForm.getByRole('spinbutton').nth(5).fill('8000')
  await leaseForm.getByPlaceholder('Select effective from').fill('2026-01-01')
  await leaseForm.getByPlaceholder('Select effective to').fill('2026-12-31')
  await page.getByRole('button', { name: 'Create lease' }).click()

  await expect(page).toHaveURL(/\/lease\/contracts\/101/)
  await expect(page.getByTestId('lease-detail-view')).toBeVisible()
  await expect(page.getByText('Harbor Foods')).toBeVisible()
  await page.getByTestId('lease-submit-button').click()
  await expect(page.getByText('Lease submitted for approval.')).toBeVisible()
  await expect(page.getByText('pending_approval').first()).toBeVisible()

  await page.goto('/billing/charges')
  await expect(page.getByTestId('billing-charges-view')).toBeVisible()
  await page.locator('.billing-charges-view__filter-input').nth(0).locator('input').fill('2026-01-01')
  await page.locator('.billing-charges-view__filter-input').nth(0).locator('input').press('Tab')
  await page.locator('.billing-charges-view__filter-input').nth(1).locator('input').fill('2026-01-31')
  await page.locator('.billing-charges-view__filter-input').nth(1).locator('input').press('Tab')
  await expect(page.getByTestId('charge-generate-button')).toBeEnabled()
  await page.getByTestId('charge-generate-button').click()
  await expect(page.getByText('费用生成完成')).toBeVisible()
  await expect(page.getByText('Harbor Foods')).toBeVisible()

  await page.goto('/billing/invoices')
  await expect(page.getByTestId('billing-invoices-view')).toBeVisible()
  await expect(page.getByText('INV-2026-0001')).toBeVisible()
  await page.getByRole('button', { name: 'View' }).click()

  await expect(page).toHaveURL(/\/billing\/invoices\/501/)
  await expect(page.getByTestId('invoice-detail-view')).toBeVisible()
  await page.getByTestId('invoice-submit-button').click()
  await expect(page.getByText('Invoice submitted for approval.')).toBeVisible()

  await page.goto('/tax/exports')
  await expect(page.getByTestId('tax-exports-view')).toBeVisible()
  await page.getByTestId('tax-ruleset-select').click()
  await page.getByRole('option', { name: /TAX-LEASE/ }).click()
  await page.getByPlaceholder('Select start date').click()
  await page.getByPlaceholder('Select start date').fill('2026-01-01')
  await page.getByPlaceholder('Select start date').press('Tab')
  await page.getByPlaceholder('Select end date').click()
  await page.getByPlaceholder('Select end date').fill('2026-01-31')
  await page.getByPlaceholder('Select end date').press('Tab')
  await page.getByTestId('tax-export-button').click()
  await expect(page.getByText('Tax export completed')).toBeVisible()

  await page.goto('/print/preview')
  await expect(page.getByTestId('print-preview-view')).toBeVisible()
  await page.getByTestId('print-template-select').click()
  await page.getByRole('option', { name: /INVOICE_STD/ }).click()
  await page.getByTestId('print-document-ids').fill('501')
  await page.getByTestId('print-render-pdf-button').click()
  await expect(page.getByText('PDF generated')).toBeVisible()

  await page.goto('/excel/io')
  await expect(page.getByTestId('excel-io-view')).toBeVisible()
  await page.getByTestId('excel-export-dataset').click()
  await page.getByRole('option', { name: '账单费用' }).click()
  await page.getByTestId('excel-export-button').click()
  await expect(page.getByText('导出完成', { exact: true })).toBeVisible()
})
