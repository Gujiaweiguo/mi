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
}

const selectGeneralizeReport = async (page: Page, reportCode: string) => {
  const reportSelect = page.getByTestId('generalize-report-select')

  await reportSelect.click()
  await page.getByRole('option', { name: new RegExp(`^${reportCode}\\b`) }).click()
  await expect(reportSelect).toContainText(reportCode)
}

const loginAndOpenGeneralize = async (page: Page) => {
  await page.goto('/login', { waitUntil: 'domcontentloaded' })
  await expect(page.getByTestId('login-view')).toBeVisible({ timeout: 15000 })

  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/dashboard/)
  await expect(page.getByTestId('dashboard-view')).toBeVisible()
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)
  await expect(page.getByTestId('generalize-reports-view')).toBeVisible()
}

test('queries and exports the sales analysis report family (R03/R04/R13/R14)', async ({ page }) => {
  await attachReportingMocks(page)

  let r03QueryPayload: unknown = null
  let r03ExportPayload: unknown = null
  let r03ExportRequested = false
  let r04QueryPayload: unknown = null
  let r04ExportPayload: unknown = null
  let r04ExportRequested = false
  let r13QueryPayload: unknown = null
  let r13ExportPayload: unknown = null
  let r13ExportRequested = false
  let r14QueryPayload: unknown = null
  let r14ExportPayload: unknown = null
  let r14ExportRequested = false

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

  await page.route('**/api/reports/r03/query', async (route) => {
    r03QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r03',
        columns: [
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'sales_total', label: 'Sales total' },
          { key: 'rent_income', label: 'Rent income' },
          { key: 'sales_rent_ratio', label: 'Sales to rent ratio' },
        ],
        rows: [{ shop_type_name: 'Fashion', sales_total: 18500, rent_income: 6200, sales_rent_ratio: 2.98 }],
        generated_at: '2026-04-02T09:04:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r03/export', async (route) => {
    r03ExportRequested = true
    r03ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r03-export'),
    })
  })

  await page.route('**/api/reports/r04/query', async (route) => {
    r04QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r04',
        columns: [
          { key: 'sale_date', label: 'Sale date' },
          { key: 'shop_name', label: 'Shop' },
          { key: 'sales_total', label: 'Sales total' },
        ],
        rows: [{ sale_date: '2026-04-15', shop_name: 'Fashion Atrium', sales_total: 38000 }],
        generated_at: '2026-04-02T09:06:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r04/export', async (route) => {
    r04ExportRequested = true
    r04ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r04-export'),
    })
  })

  await page.route('**/api/reports/r13/query', async (route) => {
    r13QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r13',
        columns: [
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'current_sales', label: 'Current sales' },
          { key: 'previous_year_sales', label: 'Previous year sales' },
          { key: 'yoy_growth', label: 'YoY growth' },
        ],
        rows: [{ shop_type_name: 'Fashion', current_sales: 24000, previous_year_sales: 20000, yoy_growth: 0.2 }],
        generated_at: '2026-04-02T09:08:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r13/export', async (route) => {
    r13ExportRequested = true
    r13ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r13-export'),
    })
  })

  await page.route('**/api/reports/r14/query', async (route) => {
    r14QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r14',
        columns: [
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'sales_total', label: 'Sales total' },
          { key: 'rent_area', label: 'Rent area' },
          { key: 'sales_efficiency', label: 'Sales efficiency' },
        ],
        rows: [{ shop_type_name: 'Lifestyle', sales_total: 24000, rent_area: 75, sales_efficiency: 320 }],
        generated_at: '2026-04-02T09:10:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r14/export', async (route) => {
    r14ExportRequested = true
    r14ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r14-export'),
    })
  })

  await loginAndOpenGeneralize(page)

  await selectGeneralizeReport(page, 'R03')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('501')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('6200')
  await expect(page.getByTestId('generalize-report-table')).toContainText('2.98')
  await expect.poll(() => r03QueryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 501 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r03ExportRequested).toBe(true)
  await expect.poll(() => r03ExportPayload).toEqual(r03QueryPayload)

  await selectGeneralizeReport(page, 'R04')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('2026-04-15')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion Atrium')
  await expect(page.getByTestId('generalize-report-table')).toContainText('38000')
  await expect.poll(() => r04QueryPayload).toEqual({ period: '2026-04', store_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r04ExportRequested).toBe(true)
  await expect.poll(() => r04ExportPayload).toEqual(r04QueryPayload)

  await selectGeneralizeReport(page, 'R13')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('501')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('24000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('20000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('0.2')
  await expect.poll(() => r13QueryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 501 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r13ExportRequested).toBe(true)
  await expect.poll(() => r13ExportPayload).toEqual(r13QueryPayload)

  await selectGeneralizeReport(page, 'R14')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('501')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Lifestyle')
  await expect(page.getByTestId('generalize-report-table')).toContainText('75')
  await expect(page.getByTestId('generalize-report-table')).toContainText('320')
  await expect.poll(() => r14QueryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 501 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r14ExportRequested).toBe(true)
  await expect.poll(() => r14ExportPayload).toEqual(r14QueryPayload)
})

test('queries and exports the receivable aging report family (R08/R16/R17)', async ({ page }) => {
  await attachReportingMocks(page)

  let r08QueryPayload: unknown = null
  let r08ExportPayload: unknown = null
  let r08ExportRequested = false
  let r16QueryPayload: unknown = null
  let r16ExportPayload: unknown = null
  let r16ExportRequested = false
  let r17QueryPayload: unknown = null
  let r17ExportPayload: unknown = null
  let r17ExportRequested = false

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

  await page.route('**/api/reports/r08/query', async (route) => {
    r08QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r08',
        columns: [
          { key: 'customer_name', label: 'Customer' },
          { key: 'current_bucket', label: 'Current' },
          { key: 'bucket_31_60', label: '31-60 days' },
          { key: 'total_balance', label: 'Total balance' },
        ],
        rows: [{ customer_name: 'ACME Retail', current_bucket: 800, bucket_31_60: 300, total_balance: 1100 }],
        generated_at: '2026-04-02T09:12:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r08/export', async (route) => {
    r08ExportRequested = true
    r08ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r08-export'),
    })
  })

  await page.route('**/api/reports/r16/query', async (route) => {
    r16QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r16',
        columns: [
          { key: 'department_name', label: 'Department' },
          { key: 'current_bucket', label: 'Current' },
          { key: 'over_90_bucket', label: 'Over 90 days' },
          { key: 'total_balance', label: 'Total balance' },
        ],
        rows: [{ department_name: 'Operations', current_bucket: 650, over_90_bucket: 150, total_balance: 800 }],
        generated_at: '2026-04-02T09:14:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r16/export', async (route) => {
    r16ExportRequested = true
    r16ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r16-export'),
    })
  })

  await page.route('**/api/reports/r17/query', async (route) => {
    r17QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r17',
        columns: [
          { key: 'department_name', label: 'Department' },
          { key: 'charge_type', label: 'Charge type' },
          { key: 'over_90_bucket', label: 'Over 90 days' },
          { key: 'total_balance', label: 'Total balance' },
        ],
        rows: [{ department_name: 'Operations', charge_type: 'utility', over_90_bucket: 90, total_balance: 540 }],
        generated_at: '2026-04-02T09:16:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r17/export', async (route) => {
    r17ExportRequested = true
    r17ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r17-export'),
    })
  })

  await loginAndOpenGeneralize(page)

  await selectGeneralizeReport(page, 'R08')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-customer-input').fill('201')
  await page.getByTestId('generalize-trade-input').fill('301')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('ACME Retail')
  await expect(page.getByTestId('generalize-report-table')).toContainText('31-60 days')
  await expect(page.getByTestId('generalize-report-table')).toContainText('1100')
  await expect.poll(() => r08QueryPayload).toEqual({ period: '2026-04', department_id: 101, customer_id: 201, trade_id: 301 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r08ExportRequested).toBe(true)
  await expect.poll(() => r08ExportPayload).toEqual(r08QueryPayload)

  await selectGeneralizeReport(page, 'R16')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Operations')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Over 90 days')
  await expect(page.getByTestId('generalize-report-table')).toContainText('800')
  await expect.poll(() => r16QueryPayload).toEqual({ period: '2026-04', department_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r16ExportRequested).toBe(true)
  await expect.poll(() => r16ExportPayload).toEqual(r16QueryPayload)

  await selectGeneralizeReport(page, 'R17')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-charge-type-input').fill('utility')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('utility')
  await expect(page.getByTestId('generalize-report-table')).toContainText('90')
  await expect(page.getByTestId('generalize-report-table')).toContainText('540')
  await expect.poll(() => r17QueryPayload).toEqual({ period: '2026-04', department_id: 101, charge_type: 'utility' })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r17ExportRequested).toBe(true)
  await expect.poll(() => r17ExportPayload).toEqual(r17QueryPayload)
})

test('covers acceptance-visible ledger and lease-area fields for R02 and R11', async ({ page }) => {
  await attachReportingMocks(page)

  let r02QueryPayload: unknown = null
  let r02ExportPayload: unknown = null
  let r02ExportRequested = false
  let r11QueryPayload: unknown = null
  let r11ExportPayload: unknown = null
  let r11ExportRequested = false

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

  await page.route('**/api/reports/r02/query', async (route) => {
    r02QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r02',
        columns: [
          { key: 'contract_no', label: 'Contract no.' },
          { key: 'customer_code', label: 'Customer code' },
          { key: 'customer_name', label: 'Customer' },
          { key: 'trade_name', label: 'Trade' },
          { key: 'management_type_name', label: 'Management type' },
          { key: 'shop_code', label: 'Shop code' },
          { key: 'shop_name', label: 'Shop' },
          { key: 'lease_area', label: 'Lease area' },
          { key: 'brand_name', label: 'Brand' },
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'department_name', label: 'Department' },
          { key: 'store_name', label: 'Store' },
        ],
        rows: [
          {
            contract_no: 'CT-2026-002',
            customer_code: 'CUST-201',
            customer_name: 'Lotus Retail',
            trade_name: 'Beverage',
            management_type_name: 'Franchise',
            shop_code: 'SHOP-08',
            shop_name: 'Tea Avenue',
            lease_area: 86,
            brand_name: 'Lotus Tea',
            shop_type_name: 'Kiosk',
            department_name: 'North Division',
            store_name: 'MI Demo Mall',
          },
        ],
        generated_at: '2026-04-02T09:20:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r02/export', async (route) => {
    r02ExportRequested = true
    r02ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r02-acceptance-export'),
    })
  })

  await page.route('**/api/reports/r11/query', async (route) => {
    r11QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r11',
        columns: [
          { key: 'store_name', label: 'Store' },
          { key: 'period', label: 'Period' },
          { key: 'leased_area_total', label: 'Leased area total' },
          { key: 'total_area', label: 'Total area' },
        ],
        rows: [{ store_name: 'MI Demo Mall', period: '2026-04', leased_area_total: 860, total_area: 1000 }],
        generated_at: '2026-04-02T09:22:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r11/export', async (route) => {
    r11ExportRequested = true
    r11ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r11-acceptance-export'),
    })
  })

  await loginAndOpenGeneralize(page)

  await selectGeneralizeReport(page, 'R02')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-customer-input').fill('201')
  await page.getByTestId('generalize-trade-input').fill('301')
  await page.getByTestId('generalize-status-input').click()
  await page.keyboard.press('ArrowDown')
  await page.keyboard.press('ArrowDown')
  await page.keyboard.press('ArrowDown')
  await page.keyboard.press('Enter')
  await page.getByTestId('generalize-management-type-input').fill('401')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('CT-2026-002')
  await expect(page.getByTestId('generalize-report-table')).toContainText('CUST-201')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Lotus Retail')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Beverage')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Franchise')
  await expect(page.getByTestId('generalize-report-table')).toContainText('SHOP-08')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Tea Avenue')
  await expect(page.getByTestId('generalize-report-table')).toContainText('86')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Lotus Tea')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Kiosk')
  await expect(page.getByTestId('generalize-report-table')).toContainText('North Division')
  await expect(page.getByTestId('generalize-report-table')).toContainText('MI Demo Mall')
  await expect.poll(() => r02QueryPayload).toEqual({
    period: '2026-04',
    store_id: 101,
    department_id: 101,
    customer_id: 201,
    trade_id: 301,
    status: 'active',
    management_type_id: 401,
  })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r02ExportRequested).toBe(true)
  await expect.poll(() => r02ExportPayload).toEqual(r02QueryPayload)

  await selectGeneralizeReport(page, 'R11')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('MI Demo Mall')
  await expect(page.getByTestId('generalize-report-table')).toContainText('2026-04')
  await expect(page.getByTestId('generalize-report-table')).toContainText('860')
  await expect(page.getByTestId('generalize-report-table')).toContainText('1000')
  await expect.poll(() => r11QueryPayload).toEqual({ period: '2026-04', store_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r11ExportRequested).toBe(true)
  await expect.poll(() => r11ExportPayload).toEqual(r11QueryPayload)
})

test('covers acceptance-visible sales analysis fields for R03, R04, R13, and R14', async ({ page }) => {
  await attachReportingMocks(page)

  let r03QueryPayload: unknown = null
  let r03ExportPayload: unknown = null
  let r03ExportRequested = false
  let r04QueryPayload: unknown = null
  let r04ExportPayload: unknown = null
  let r04ExportRequested = false
  let r13QueryPayload: unknown = null
  let r13ExportPayload: unknown = null
  let r13ExportRequested = false
  let r14QueryPayload: unknown = null
  let r14ExportPayload: unknown = null
  let r14ExportRequested = false

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

  await page.route('**/api/reports/r03/query', async (route) => {
    r03QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r03',
        columns: [
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'lease_area_total', label: 'Lease area' },
          { key: 'current_sales_total', label: 'Current sales' },
          { key: 'comparable_sales_total', label: 'Comparable sales' },
          { key: 'previous_year_sales_total', label: 'Previous year sales' },
          { key: 'rent_income', label: 'Rent income' },
        ],
        rows: [
          {
            shop_type_name: 'Fashion',
            lease_area_total: 180,
            current_sales_total: 31000,
            comparable_sales_total: 29500,
            previous_year_sales_total: 28000,
            rent_income: 9300,
          },
        ],
        generated_at: '2026-04-02T09:24:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r03/export', async (route) => {
    r03ExportRequested = true
    r03ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r03-acceptance-export'),
    })
  })

  await page.route('**/api/reports/r04/query', async (route) => {
    r04QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r04',
        columns: [
          { key: 'shop_code', label: 'Shop code' },
          { key: 'shop_name', label: 'Shop' },
          { key: 'lease_area', label: 'Lease area' },
          { key: 'trade_name', label: 'Trade' },
          { key: 'day_1_sales', label: 'Day 1' },
          { key: 'day_2_sales', label: 'Day 2' },
          { key: 'day_3_sales', label: 'Day 3' },
          { key: 'sales_total', label: 'Sales total' },
        ],
        rows: [
          {
            shop_code: 'A-101',
            shop_name: 'Atrium Flagship',
            lease_area: 120,
            trade_name: 'Fashion',
            day_1_sales: 3000,
            day_2_sales: 3100,
            day_3_sales: 3200,
            sales_total: 9300,
          },
        ],
        generated_at: '2026-04-02T09:26:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r04/export', async (route) => {
    r04ExportRequested = true
    r04ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r04-acceptance-export'),
    })
  })

  await page.route('**/api/reports/r13/query', async (route) => {
    r13QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r13',
        columns: [
          { key: 'store_name', label: 'Store' },
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'month', label: 'Month' },
          { key: 'current_sales', label: 'Current sales' },
          { key: 'year_to_date_sales', label: 'Year to date sales' },
          { key: 'previous_month_sales', label: 'Previous month sales' },
          { key: 'previous_year_to_date_sales', label: 'Previous year to date sales' },
        ],
        rows: [
          {
            store_name: 'MI Demo Mall',
            shop_type_name: 'Fashion',
            month: '2026-04',
            current_sales: 31000,
            year_to_date_sales: 118000,
            previous_month_sales: 29500,
            previous_year_to_date_sales: 102000,
          },
        ],
        generated_at: '2026-04-02T09:28:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r13/export', async (route) => {
    r13ExportRequested = true
    r13ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r13-acceptance-export'),
    })
  })

  await page.route('**/api/reports/r14/query', async (route) => {
    r14QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r14',
        columns: [
          { key: 'store_name', label: 'Store' },
          { key: 'shop_type_name', label: 'Shop type' },
          { key: 'month', label: 'Month' },
          { key: 'sales_total', label: 'Sales total' },
          { key: 'area_total', label: 'Area' },
          { key: 'day_count', label: 'Day count' },
          { key: 'sales_efficiency', label: 'Sales efficiency' },
        ],
        rows: [
          {
            store_name: 'MI Demo Mall',
            shop_type_name: 'Lifestyle',
            month: '2026-04',
            sales_total: 24800,
            area_total: 80,
            day_count: 31,
            sales_efficiency: 10,
          },
        ],
        generated_at: '2026-04-02T09:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r14/export', async (route) => {
    r14ExportRequested = true
    r14ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r14-acceptance-export'),
    })
  })

  await loginAndOpenGeneralize(page)

  await selectGeneralizeReport(page, 'R03')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('501')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('180')
  await expect(page.getByTestId('generalize-report-table')).toContainText('31000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('29500')
  await expect(page.getByTestId('generalize-report-table')).toContainText('28000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('9300')
  await expect.poll(() => r03QueryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 501 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r03ExportRequested).toBe(true)
  await expect.poll(() => r03ExportPayload).toEqual(r03QueryPayload)

  await selectGeneralizeReport(page, 'R04')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('A-101')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Atrium Flagship')
  await expect(page.getByTestId('generalize-report-table')).toContainText('120')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('3000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('3100')
  await expect(page.getByTestId('generalize-report-table')).toContainText('3200')
  await expect(page.getByTestId('generalize-report-table')).toContainText('9300')
  await expect.poll(() => r04QueryPayload).toEqual({ period: '2026-04', store_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r04ExportRequested).toBe(true)
  await expect.poll(() => r04ExportPayload).toEqual(r04QueryPayload)

  await selectGeneralizeReport(page, 'R13')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('501')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('MI Demo Mall')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('2026-04')
  await expect(page.getByTestId('generalize-report-table')).toContainText('31000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('118000')
  await expect(page.getByTestId('generalize-report-table')).toContainText('29500')
  await expect(page.getByTestId('generalize-report-table')).toContainText('102000')
  await expect.poll(() => r13QueryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 501 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r13ExportRequested).toBe(true)
  await expect.poll(() => r13ExportPayload).toEqual(r13QueryPayload)

  await selectGeneralizeReport(page, 'R14')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-shop-type-input').fill('501')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('MI Demo Mall')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Lifestyle')
  await expect(page.getByTestId('generalize-report-table')).toContainText('2026-04')
  await expect(page.getByTestId('generalize-report-table')).toContainText('24800')
  await expect(page.getByTestId('generalize-report-table')).toContainText('80')
  await expect(page.getByTestId('generalize-report-table')).toContainText('31')
  await expect(page.getByTestId('generalize-report-table')).toContainText('10')
  await expect.poll(() => r14QueryPayload).toEqual({ period: '2026-04', store_id: 101, shop_type_id: 501 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r14ExportRequested).toBe(true)
  await expect.poll(() => r14ExportPayload).toEqual(r14QueryPayload)
})

test('covers acceptance-visible aging fields and reconciliation for R08, R16, and R17', async ({ page }) => {
  await attachReportingMocks(page)

  let r08QueryPayload: unknown = null
  let r08ExportPayload: unknown = null
  let r08ExportRequested = false
  let r16QueryPayload: unknown = null
  let r16ExportPayload: unknown = null
  let r16ExportRequested = false
  let r17QueryPayload: unknown = null
  let r17ExportPayload: unknown = null
  let r17ExportRequested = false

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

  await page.route('**/api/reports/r08/query', async (route) => {
    r08QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r08',
        columns: [
          { key: 'shop_codes', label: 'Shops' },
          { key: 'customer_name', label: 'Customer' },
          { key: 'trade_name', label: 'Trade' },
          { key: 'department_name', label: 'Department' },
          { key: 'contract_no', label: 'Contract no.' },
          { key: 'deposit_amount', label: 'Deposit' },
          { key: 'bucket_within_1_month', label: 'Within 1 month' },
          { key: 'bucket_1_2_months', label: '1-2 months' },
          { key: 'bucket_2_3_months', label: '2-3 months' },
          { key: 'total_balance', label: 'Total balance' },
        ],
        rows: [
          {
            shop_codes: 'A-101,A-102',
            customer_name: 'ACME Retail',
            trade_name: 'Fashion',
            department_name: 'North Division',
            contract_no: 'CT-2026-002',
            deposit_amount: 500,
            bucket_within_1_month: 200,
            bucket_1_2_months: 300,
            bucket_2_3_months: 100,
            total_balance: 600,
          },
        ],
        generated_at: '2026-04-02T09:32:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r08/export', async (route) => {
    r08ExportRequested = true
    r08ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r08-acceptance-export'),
    })
  })

  await page.route('**/api/reports/r16/query', async (route) => {
    r16QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r16',
        columns: [
          { key: 'department_name', label: 'Department' },
          { key: 'deposit_amount', label: 'Deposit' },
          { key: 'bucket_within_1_month', label: 'Within 1 month' },
          { key: 'bucket_1_2_months', label: '1-2 months' },
          { key: 'bucket_2_3_months', label: '2-3 months' },
          { key: 'total_balance', label: 'Total balance' },
        ],
        rows: [
          {
            department_name: 'North Division',
            deposit_amount: 500,
            bucket_within_1_month: 200,
            bucket_1_2_months: 300,
            bucket_2_3_months: 100,
            total_balance: 600,
          },
        ],
        generated_at: '2026-04-02T09:34:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r16/export', async (route) => {
    r16ExportRequested = true
    r16ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r16-acceptance-export'),
    })
  })

  await page.route('**/api/reports/r17/query', async (route) => {
    r17QueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        report_id: 'r17',
        columns: [
          { key: 'department_name', label: 'Department' },
          { key: 'charge_type', label: 'Charge type' },
          { key: 'deposit_amount', label: 'Deposit' },
          { key: 'bucket_within_1_month', label: 'Within 1 month' },
          { key: 'bucket_1_2_months', label: '1-2 months' },
          { key: 'bucket_2_3_months', label: '2-3 months' },
          { key: 'total_balance', label: 'Total balance' },
        ],
        rows: [
          {
            department_name: 'North Division',
            charge_type: 'rent',
            deposit_amount: 500,
            bucket_within_1_month: 200,
            bucket_1_2_months: 300,
            bucket_2_3_months: 100,
            total_balance: 600,
          },
        ],
        generated_at: '2026-04-02T09:36:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r17/export', async (route) => {
    r17ExportRequested = true
    r17ExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r17-acceptance-export'),
    })
  })

  await loginAndOpenGeneralize(page)

  await selectGeneralizeReport(page, 'R08')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-customer-input').fill('201')
  await page.getByTestId('generalize-trade-input').fill('301')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('A-101,A-102')
  await expect(page.getByTestId('generalize-report-table')).toContainText('ACME Retail')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('North Division')
  await expect(page.getByTestId('generalize-report-table')).toContainText('CT-2026-002')
  await expect(page.getByTestId('generalize-report-table')).toContainText('500')
  await expect(page.getByTestId('generalize-report-table')).toContainText('200')
  await expect(page.getByTestId('generalize-report-table')).toContainText('300')
  await expect(page.getByTestId('generalize-report-table')).toContainText('100')
  await expect(page.getByTestId('generalize-report-table')).toContainText('600')
  await expect.poll(() => r08QueryPayload).toEqual({ period: '2026-04', department_id: 101, customer_id: 201, trade_id: 301 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r08ExportRequested).toBe(true)
  await expect.poll(() => r08ExportPayload).toEqual(r08QueryPayload)

  await selectGeneralizeReport(page, 'R16')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('North Division')
  await expect(page.getByTestId('generalize-report-table')).toContainText('500')
  await expect(page.getByTestId('generalize-report-table')).toContainText('200')
  await expect(page.getByTestId('generalize-report-table')).toContainText('300')
  await expect(page.getByTestId('generalize-report-table')).toContainText('100')
  await expect(page.getByTestId('generalize-report-table')).toContainText('600')
  await expect.poll(() => r16QueryPayload).toEqual({ period: '2026-04', department_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r16ExportRequested).toBe(true)
  await expect.poll(() => r16ExportPayload).toEqual(r16QueryPayload)

  await selectGeneralizeReport(page, 'R17')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-department-input').fill('101')
  await page.getByTestId('generalize-charge-type-input').fill('rent')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('North Division')
  await expect(page.getByTestId('generalize-report-table')).toContainText('rent')
  await expect(page.getByTestId('generalize-report-table')).toContainText('500')
  await expect(page.getByTestId('generalize-report-table')).toContainText('200')
  await expect(page.getByTestId('generalize-report-table')).toContainText('300')
  await expect(page.getByTestId('generalize-report-table')).toContainText('100')
  await expect(page.getByTestId('generalize-report-table')).toContainText('600')
  await expect.poll(() => r17QueryPayload).toEqual({ period: '2026-04', department_id: 101, charge_type: 'rent' })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r17ExportRequested).toBe(true)
  await expect.poll(() => r17ExportPayload).toEqual(r17QueryPayload)
})

test('covers acceptance-visible occupancy fields for R01 and R12', async ({ page }) => {
  await attachReportingMocks(page)

  let r01QueryPayload: unknown = null
  let r01ExportPayload: unknown = null
  let r01ExportRequested = false
  let r12QueryPayload: unknown = null
  let r12ExportPayload: unknown = null
  let r12ExportRequested = false

  await page.route('**/api/reports/r01/query', async (route) => {
    r01QueryPayload = route.request().postDataJSON()
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
          {
            store_name: 'MI Demo Mall',
            department_name: 'Operations',
            rent_status: 'vacant',
            use_area_total: 64,
          },
        ],
        generated_at: '2026-04-02T08:30:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r01/export', async (route) => {
    r01ExportRequested = true
    r01ExportPayload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r01-export'),
    })
  })

  await page.route('**/api/reports/r12/query', async (route) => {
    r12QueryPayload = route.request().postDataJSON()
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
          {
            store_name: 'MI Demo Mall',
            period: '2026-04',
            shop_type_name: 'Fashion',
            occupancy_status: 'vacant',
            area_total: 64,
          },
        ],
        generated_at: '2026-04-02T08:32:00Z',
      }),
    })
  })

  await page.route('**/api/reports/r12/export', async (route) => {
    r12ExportRequested = true
    r12ExportPayload = route.request().postDataJSON()
    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-report-export'),
    })
  })

  await loginAndOpenGeneralize(page)
  await expect(page.getByTestId('generalize-reports-view')).toBeVisible()

  await selectGeneralizeReport(page, 'R01')
  await page.getByTestId('generalize-period-input').fill('2026-04')
  await page.getByTestId('generalize-query-button').click()
  await expect(page.getByTestId('generalize-report-table')).toContainText('MI Demo Mall')
  await expect(page.getByTestId('generalize-report-table')).toContainText('Operations')
  await expect(page.getByTestId('generalize-report-table')).toContainText('leased')
  await expect(page.getByTestId('generalize-report-table')).toContainText('vacant')
  await expect(page.getByTestId('generalize-report-table')).toContainText('118')
  await expect(page.getByTestId('generalize-report-table')).toContainText('64')
  await expect.poll(() => r01QueryPayload).toEqual({ period: '2026-04' })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r01ExportRequested).toBe(true)
  await expect.poll(() => r01ExportPayload).toEqual(r01QueryPayload)

  await selectGeneralizeReport(page, 'R12')
  await page.getByTestId('generalize-store-input').fill('101')
  await page.getByTestId('generalize-query-button').click()

  await expect(page.getByTestId('generalize-report-table')).toContainText('Fashion')
  await expect(page.getByTestId('generalize-report-table')).toContainText('leased')
  await expect(page.getByTestId('generalize-report-table')).toContainText('vacant')
  await expect(page.getByTestId('generalize-report-table')).toContainText('118')
  await expect(page.getByTestId('generalize-report-table')).toContainText('64')
  await expect.poll(() => r12QueryPayload).toEqual({ period: '2026-04', store_id: 101 })

  await page.getByTestId('generalize-export-button').click()
  await expect.poll(() => r12ExportRequested).toBe(true)
  await expect.poll(() => r12ExportPayload).toEqual(r12QueryPayload)
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

  await expect(page).toHaveURL(/\/dashboard/)
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

  await expect(page).toHaveURL(/\/dashboard/)
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

  await expect(page).toHaveURL(/\/dashboard/)
  await page.getByTestId('nav--reports-generalize').click()
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await selectGeneralizeReport(page, 'R09')

  await page.getByTestId('generalize-period-input').fill('2026-04')
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

  await expect(page).toHaveURL(/\/dashboard/)
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

  await expect(page).toHaveURL(/\/dashboard/)
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

  await expect(page).toHaveURL(/\/dashboard/)
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

  await expect(page).toHaveURL(/\/dashboard/)
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
