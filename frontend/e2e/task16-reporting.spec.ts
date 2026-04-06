import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

const attachReportingMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'reporting.generalize',
      permission_level: 'approve',
      can_print: false,
      can_export: true,
    },
  ]

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'reporter',
          display_name: 'Report Operator',
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

  await page.route('**/api/health', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ status: 'ok', service: 'frontend-platform-test' }),
    })
  })
}

const selectGeneralizeReport = async (page: Page, reportCode: string) => {
  await page.getByTestId('generalize-report-select').click()
  await page.getByRole('option', { name: new RegExp(`^${reportCode}\\b`) }).click()
}

test('queries and exports the generalize reporting slice', async ({ page }) => {
  await attachReportingMocks(page)

  let exportRequested = false

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [
          { key: 'store_name', label: 'Store' },
          { key: 'department_name', label: 'Department' },
          { key: 'rent_status', label: 'Rent status' },
          { key: 'use_area_total', label: 'Use area total' },
        ],
        rows: [
          {
            store_name: 'MI Demo Mall',
            department_name: 'Operations',
            rent_status: 'leased',
            use_area_total: 118,
          },
        ],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r12/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r12',
        columns: [
          { key: 'store_name', label: 'Store' },
          { key: 'period', label: 'Period' },
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'occupancy_status', label: 'Occupancy status' },
          { key: 'area_total', label: 'Area total' },
        ],
        rows: [
          {
            store_name: 'MI Demo Mall',
            period: '2026-04',
            shop_type_name: 'Fashion',
            occupancy_status: 'leased',
            area_total: 118,
          },
        ],
        generated_at: '2026-04-02T08:32:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r12/export', async (route) => {
    exportRequested = true
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-report-export'),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('nav--reports-generalize')).toBeVisible()

  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)
  await expect(page.getByTestId('generalize-reports-view')).toBeVisible()
  await expect(page.getByText('MI Demo Mall')).toBeVisible()

  await selectGeneralizeReport(page, 'R12')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('leased')

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => exportRequested).toBe(true)
})

test('queries and exports the traffic annual/monthly summary (R10)', async ({ page }) => {
  await attachReportingMocks(page)

  let exportRequested = false
  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r10/query', async (route) => {
    queryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r10',
        columns: [
          { key: 'year', label: 'Year' },
          { key: 'month', label: 'Month' },
          { key: 'traffic_total', label: 'Traffic total' },
        ],
        rows: [
          { year: 2025, month: 1, traffic_total: 1200 },
          { year: 2025, month: 2, traffic_total: 1400 },
        ],
        generated_at: '2026-04-02T08:40:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r10/export', async (route) => {
    exportRequested = true

    const payload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from(JSON.stringify(payload)),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R10')

  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-year-input').fill('2025')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('1400')

  await expect.poll(() => queryPayload).toEqual({ period: '2025-01', store_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => exportRequested).toBe(true)
})

test('queries and exports the brand annual sales distribution (R07)', async ({ page }) => {
  await attachReportingMocks(page)

  let exportRequested = false
  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r07/query', async (route) => {
    queryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r07',
        columns: [
          { key: 'brand_name', label: 'Brand' },
          { key: 'sales_total', label: 'Sales total' },
        ],
        rows: [{ brand_name: 'Example Brand', sales_total: 125000 }],
        generated_at: '2026-04-02T08:50:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r07/export', async (route) => {
    exportRequested = true
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-report-export'),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R07')

  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-year-input').fill('2026')
  await page.getByTestId('generalize-brand-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Example Brand')
  await expect.poll(() => queryPayload).toEqual({ period: '2026-01', store_id: 101, brand_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => exportRequested).toBe(true)
})

test('queries AR aging by charge type (R09) with department, customer, trade, and charge filters', async ({ page }) => {
  await attachReportingMocks(page)

  let exportRequested = false
  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r09/query', async (route) => {
    queryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r09',
        columns: [
          { key: 'customer_name', label: 'Customer' },
          { key: 'charge_type', label: 'Charge type' },
          { key: 'total', label: 'Total' },
        ],
        rows: [{ customer_name: 'ACME Retail', charge_type: 'rent', total: 1500 }],
        generated_at: '2026-04-02T09:10:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r09/export', async (route) => {
    exportRequested = true
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-ar-aging-export'),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R09')

  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-customer-input').fill('101')
  await page.getByTestId('generalize-trade-input').fill('102')
  await page.getByTestId('generalize-charge-type-input').fill('rent')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('ACME Retail')
  await expect.poll(() => queryPayload).toEqual({ period: '2026-04', department_id: 101, customer_id: 101, trade_id: 102, charge_type: 'rent' })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => exportRequested).toBe(true)
})

test('queries the unit budget comparison report (R05) using year, store, floor, and unit filters', async ({ page }) => {
  await attachReportingMocks(page)

  let exportRequested = false
  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r05/query', async (route) => {
    queryPayload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r05',
        columns: [
          { key: 'unit_code', label: 'Unit code' },
          { key: 'budget_unit_price', label: 'Budget unit price' },
          { key: 'current_lease_price', label: 'Current lease price' },
        ],
        rows: [{ unit_code: 'U-101', budget_unit_price: 95, current_lease_price: 100 }],
        generated_at: '2026-04-02T09:20:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r05/export', async (route) => {
    exportRequested = true
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-budget-export'),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R05')

  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-year-input').fill('2026')
  await page.getByTestId('generalize-floor-input').fill('101')
  await page.getByTestId('generalize-unit-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('U-101')
  await expect.poll(() => queryPayload).toEqual({ period: '2026-01', store_id: 101, floor_id: 101, unit_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => exportRequested).toBe(true)
})

test('queries the store rent budget execution report (R06)', async ({ page }) => {
  await attachReportingMocks(page)

  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r06/query', async (route) => {
    queryPayload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r06',
        columns: [
          { key: 'store_name', label: 'Store' },
          { key: 'monthly_budget', label: 'Monthly budget' },
          { key: 'annual_budget', label: 'Annual budget' },
        ],
        rows: [{ store_name: 'MI Demo Mall', monthly_budget: 10000, annual_budget: 120000 }],
        generated_at: '2026-04-02T09:22:00Z',
      }),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R06')

  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('120000')
  await expect.poll(() => queryPayload).toEqual({ period: '2026-04', store_id: 101 })
})

test('queries the sales vs rent income report (R15)', async ({ page }) => {
  await attachReportingMocks(page)

  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r15/query', async (route) => {
    queryPayload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r15',
        columns: [
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'sales_amount', label: 'Sales' },
          { key: 'rent_income', label: 'Rent income' },
        ],
        rows: [{ shop_type_name: 'Fashion', sales_amount: 25000, rent_income: 12000 }],
        generated_at: '2026-04-02T09:24:00Z',
      }),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R15')

  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('101')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('12000')
  await expect.poll(() => queryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 101 })
})

test('queries the customer/store/brand composite report (R18)', async ({ page }) => {
  await attachReportingMocks(page)

  let queryPayload: unknown = null

  await page.route('**/api/reports/r01/query', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r01',
        columns: [{ key: 'store_name', label: 'Store' }],
        rows: [{ store_name: 'MI Demo Mall' }],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r18/query', async (route) => {
    queryPayload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r18',
        columns: [
          { key: 'customer_name', label: 'Customer' },
          { key: 'brand_name', label: 'Brand' },
          { key: 'current_sales', label: 'Current sales' },
        ],
        rows: [{ customer_name: 'ACME Retail', brand_name: 'ACME Fashion', current_sales: 25000 }],
        generated_at: '2026-04-02T09:25:00Z',
      }),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R18')

  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-customer-input').fill('101')
  await page.getByTestId('generalize-brand-input').fill('101')
  await page.getByTestId('generalize-unit-input').fill('101')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('ACME Retail')
  await expect.poll(() => queryPayload).toEqual({ period: '2026-04', store_id: 101, customer_id: 101, brand_id: 101, unit_id: 101 })
})
