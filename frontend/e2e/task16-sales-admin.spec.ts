import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

type DailySaleFixture = {
  id: number
  store_id: number
  unit_id: number
  sale_date: string
  sales_amount: number
  created_at: string
  updated_at: string
}

type CustomerTrafficFixture = {
  id: number
  store_id: number
  traffic_date: string
  inbound_count: number
  created_at: string
  updated_at: string
}

type StoreFixture = {
  id: number
  department_id: number
  store_type_id: number
  management_type_id: number
  code: string
  name: string
  short_name: string
  status: string
  created_at: string
  updated_at: string
}

type UnitFixture = {
  id: number
  building_id: number
  floor_id: number
  location_id: number
  area_id: number
  unit_type_id: number
  code: string
  floor_area: number
  use_area: number
  rent_area: number
  is_rentable: boolean
  status: string
  created_at: string
  updated_at: string
}

type ImportDiagnosticFixture = {
  row: number
  field: string
  message: string
}

type ImportMockFixture<Row> = {
  imported_count: number
  diagnostics?: ImportDiagnosticFixture[]
  importedRows?: Row[]
  status?: number
}

const now = '2026-04-02T08:00:00Z'

const workbookMimeType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'

const selectByTestId = async (page: Page, testId: string, label: string) => {
  await page.getByTestId(testId).click()
  await page.locator('.el-select-dropdown:visible .el-select-dropdown__item').filter({ hasText: label }).first().click()
}

const attachWorkbook = async (page: Page, testId: string, filename: string) => {
  await page.getByTestId(testId).locator('input[type="file"]').setInputFiles({
    name: filename,
    mimeType: workbookMimeType,
    buffer: Buffer.from(`mock workbook:${filename}`),
  })
}

const loginToSalesAdmin = async (page: Page) => {
  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('sales-admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('nav--admin-sales')).toBeVisible()

  await page.getByTestId('nav--admin-sales').click()
  await expect(page).toHaveURL(/\/admin\/sales/)
  await expect(page.getByTestId('sales-admin-view')).toBeVisible()
}

const toOptionalInteger = (value: string | null) => {
  if (!value) {
    return undefined
  }

  const parsed = Number(value)
  return Number.isInteger(parsed) ? parsed : undefined
}

const attachSalesAdminMocks = async (
  page: Page,
  options: {
    dailySales?: DailySaleFixture[]
    trafficRows?: CustomerTrafficFixture[]
    dailySalesImport?: ImportMockFixture<DailySaleFixture>
    trafficImport?: ImportMockFixture<CustomerTrafficFixture>
  } = {},
) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'sales.admin',
      permission_level: 'approve',
      can_print: false,
      can_export: false,
    },
  ]

  const stores: StoreFixture[] = [
    {
      id: 101,
      department_id: 12,
      store_type_id: 22,
      management_type_id: 32,
      code: 'MI-101',
      name: 'Harbor Center',
      short_name: 'Harbor',
      status: 'active',
      created_at: now,
      updated_at: now,
    },
    {
      id: 102,
      department_id: 13,
      store_type_id: 22,
      management_type_id: 33,
      code: 'MI-102',
      name: 'Garden Plaza',
      short_name: 'Garden',
      status: 'active',
      created_at: now,
      updated_at: now,
    },
  ]

  const units: UnitFixture[] = [
    {
      id: 201,
      building_id: 301,
      floor_id: 401,
      location_id: 501,
      area_id: 601,
      unit_type_id: 701,
      code: 'U-201',
      floor_area: 120,
      use_area: 112,
      rent_area: 110,
      is_rentable: true,
      status: 'active',
      created_at: now,
      updated_at: now,
    },
    {
      id: 202,
      building_id: 302,
      floor_id: 402,
      location_id: 502,
      area_id: 602,
      unit_type_id: 701,
      code: 'U-202',
      floor_area: 140,
      use_area: 130,
      rent_area: 128,
      is_rentable: true,
      status: 'active',
      created_at: now,
      updated_at: now,
    },
  ]

  let dailySales: DailySaleFixture[] = options.dailySales ? [...options.dailySales] : []
  let trafficRows: CustomerTrafficFixture[] = options.trafficRows ? [...options.trafficRows] : []
  let dailyTemplateDownloads = 0
  let trafficTemplateDownloads = 0
  let dailyImportRequests = 0
  let trafficImportRequests = 0

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'sales-admin',
          display_name: 'Sales Admin',
          department_id: 101,
          roles: ['admin'],
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

  await page.route('**/api/structure/stores*', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ stores }),
    })
  })

  await page.route('**/api/structure/units*', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ units }),
    })
  })

  await page.route('**/api/excel/templates/daily-sales', async (route) => {
    dailyTemplateDownloads += 1
    await route.fulfill({
      status: 200,
      contentType: workbookMimeType,
      body: 'daily-sales-template',
    })
  })

  await page.route('**/api/excel/templates/customer-traffic', async (route) => {
    trafficTemplateDownloads += 1
    await route.fulfill({
      status: 200,
      contentType: workbookMimeType,
      body: 'customer-traffic-template',
    })
  })

  await page.route('**/api/excel/imports/daily-sales', async (route) => {
    dailyImportRequests += 1
    const result = options.dailySalesImport ?? { imported_count: 0, diagnostics: [] }
    const status = result.status ?? (result.diagnostics?.length ? 400 : 200)

    if (status < 400 && result.importedRows?.length) {
      dailySales = [...result.importedRows, ...dailySales]
    }

    await route.fulfill({
      status,
      contentType: 'application/json',
      body: JSON.stringify({
        imported_count: result.imported_count,
        diagnostics: result.diagnostics ?? [],
      }),
    })
  })

  await page.route('**/api/excel/imports/customer-traffic', async (route) => {
    trafficImportRequests += 1
    const result = options.trafficImport ?? { imported_count: 0, diagnostics: [] }
    const status = result.status ?? (result.diagnostics?.length ? 400 : 200)

    if (status < 400 && result.importedRows?.length) {
      trafficRows = [...result.importedRows, ...trafficRows]
    }

    await route.fulfill({
      status,
      contentType: 'application/json',
      body: JSON.stringify({
        imported_count: result.imported_count,
        diagnostics: result.diagnostics ?? [],
      }),
    })
  })

  await page.route('**/api/sales/daily*', async (route) => {
    if (route.request().method() === 'POST') {
      const payload = route.request().postDataJSON() as {
        store_id: number
        unit_id: number
        sale_date: string
        sales_amount: number
      }

      const record: DailySaleFixture = {
        id: dailySales.length + 1001,
        store_id: payload.store_id,
        unit_id: payload.unit_id,
        sale_date: payload.sale_date,
        sales_amount: payload.sales_amount,
        created_at: now,
        updated_at: now,
      }

      dailySales = [record, ...dailySales]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ daily_sale: record }),
      })
      return
    }

    const url = new URL(route.request().url())
    const storeId = toOptionalInteger(url.searchParams.get('store_id'))
    const unitId = toOptionalInteger(url.searchParams.get('unit_id'))
    const dateFrom = url.searchParams.get('date_from') ?? undefined
    const dateTo = url.searchParams.get('date_to') ?? undefined

    const filteredDailySales = dailySales.filter((record) => {
      if (storeId !== undefined && record.store_id !== storeId) {
        return false
      }

      if (unitId !== undefined && record.unit_id !== unitId) {
        return false
      }

      if (dateFrom && record.sale_date < dateFrom) {
        return false
      }

      if (dateTo && record.sale_date > dateTo) {
        return false
      }

      return true
    })

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ daily_sales: filteredDailySales }),
    })
  })

  await page.route('**/api/sales/traffic*', async (route) => {
    if (route.request().method() === 'POST') {
      const payload = route.request().postDataJSON() as {
        store_id: number
        traffic_date: string
        inbound_count: number
      }

      const record: CustomerTrafficFixture = {
        id: trafficRows.length + 2001,
        store_id: payload.store_id,
        traffic_date: payload.traffic_date,
        inbound_count: payload.inbound_count,
        created_at: now,
        updated_at: now,
      }

      trafficRows = [record, ...trafficRows]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ traffic: record }),
      })
      return
    }

    const url = new URL(route.request().url())
    const storeId = toOptionalInteger(url.searchParams.get('store_id'))
    const dateFrom = url.searchParams.get('date_from') ?? undefined
    const dateTo = url.searchParams.get('date_to') ?? undefined

    const filteredTrafficRows = trafficRows.filter((record) => {
      if (storeId !== undefined && record.store_id !== storeId) {
        return false
      }

      if (dateFrom && record.traffic_date < dateFrom) {
        return false
      }

      if (dateTo && record.traffic_date > dateTo) {
        return false
      }

      return true
    })

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ customer_traffic: filteredTrafficRows }),
    })
  })

  return {
    dailyTemplateDownloads: () => dailyTemplateDownloads,
    trafficTemplateDownloads: () => trafficTemplateDownloads,
    dailyImportRequests: () => dailyImportRequests,
    trafficImportRequests: () => trafficImportRequests,
  }
}

test('supports workbook download/import alongside single-record maintenance', async ({ page }) => {
  const tracker = await attachSalesAdminMocks(page, {
    dailySalesImport: {
      imported_count: 2,
      importedRows: [
        {
          id: 1201,
          store_id: 101,
          unit_id: 201,
          sale_date: '2026-05-02',
          sales_amount: 1800,
          created_at: now,
          updated_at: now,
        },
        {
          id: 1202,
          store_id: 102,
          unit_id: 202,
          sale_date: '2026-05-03',
          sales_amount: 2200,
          created_at: now,
          updated_at: now,
        },
      ],
    },
    trafficImport: {
      imported_count: 1,
      importedRows: [
        {
          id: 2201,
          store_id: 101,
          traffic_date: '2026-05-02',
          inbound_count: 340,
          created_at: now,
          updated_at: now,
        },
      ],
    },
  })

  await loginToSalesAdmin(page)

  await page.getByTestId('sales-daily-download-template').click()
  await expect(page.getByTestId('sales-daily-feedback')).toContainText('日销售模板已下载')
  expect(tracker.dailyTemplateDownloads()).toBe(1)

  await attachWorkbook(page, 'sales-daily-upload-input', 'daily-sales.xlsx')
  await expect(page.getByTestId('sales-daily-selected-file')).toContainText('daily-sales.xlsx')
  await page.getByTestId('sales-daily-import-button').click()

  await expect(page.getByTestId('sales-daily-feedback')).toContainText('日销售导入完成')
  await expect(page.getByTestId('sales-daily-import-summary')).toContainText('2')
  await expect(page.getByText('2026-05-02')).toBeVisible()
  await expect(page.getByText('1800')).toBeVisible()
  expect(tracker.dailyImportRequests()).toBe(1)

  await selectByTestId(page, 'sales-daily-store-select', 'MI-101 — Harbor Center')
  await selectByTestId(page, 'sales-daily-unit-select', 'U-201 (#201)')
  await page.getByTestId('sales-daily-date-input').locator('input').fill('2026-05-05')
  await page.getByTestId('sales-daily-date-input').locator('input').press('Tab')
  await page.getByTestId('sales-daily-amount-input').locator('input').fill('6000')
  await page.getByTestId('sales-daily-create-button').click()

  await expect(page.getByTestId('sales-daily-feedback')).toContainText('日销售已创建')
  await expect(page.getByText('2026-05-05')).toBeVisible()
  await expect(page.getByText('6000')).toBeVisible()

  await page.getByTestId('sales-traffic-download-template').click()
  await expect(page.getByTestId('sales-traffic-feedback')).toContainText('客流模板已下载')
  expect(tracker.trafficTemplateDownloads()).toBe(1)

  await attachWorkbook(page, 'sales-traffic-upload-input', 'customer-traffic.xlsx')
  await expect(page.getByTestId('sales-traffic-selected-file')).toContainText('customer-traffic.xlsx')
  await page.getByTestId('sales-traffic-import-button').click()

  await expect(page.getByTestId('sales-traffic-feedback')).toContainText('客流导入完成')
  await expect(page.getByTestId('sales-traffic-import-summary')).toContainText('1')
  await expect(page.getByText('340')).toBeVisible()
  expect(tracker.trafficImportRequests()).toBe(1)

  await selectByTestId(page, 'sales-traffic-store-select', 'MI-101 — Harbor Center')
  await page.getByTestId('sales-traffic-date-input').locator('input').fill('2026-05-05')
  await page.getByTestId('sales-traffic-date-input').locator('input').press('Tab')
  await page.getByTestId('sales-traffic-count-input').locator('input').fill('500')
  await page.getByTestId('sales-traffic-create-button').click()

  await expect(page.getByTestId('sales-traffic-feedback')).toContainText('客流记录已创建')
  await expect(page.getByText('500')).toBeVisible()
})

test('renders daily sales import diagnostics and preserves manual fallback', async ({ page }) => {
  const tracker = await attachSalesAdminMocks(page, {
    dailySalesImport: {
      imported_count: 0,
      status: 400,
      diagnostics: [
        {
          row: 4,
          field: 'unit_code',
          message: 'Unknown unit code U-999.',
        },
        {
          row: 7,
          field: 'sales_amount',
          message: 'Sales amount must be greater than or equal to zero.',
        },
      ],
    },
  })

  await loginToSalesAdmin(page)

  await attachWorkbook(page, 'sales-daily-upload-input', 'invalid-daily-sales.xlsx')
  await page.getByTestId('sales-daily-import-button').click()

  await expect(page.getByTestId('sales-daily-feedback')).toContainText('日销售导入需要处理')
  await expect(page.getByTestId('sales-daily-import-summary')).toContainText('0')
  await expect(page.getByTestId('sales-daily-diagnostics-table')).toContainText('unit_code')
  await expect(page.getByTestId('sales-daily-diagnostics-table')).toContainText('Unknown unit code U-999.')
  await expect(page.getByTestId('sales-daily-diagnostics-table')).toContainText('sales_amount')
  expect(tracker.dailyImportRequests()).toBe(1)

  await selectByTestId(page, 'sales-daily-store-select', 'MI-101 — Harbor Center')
  await selectByTestId(page, 'sales-daily-unit-select', 'U-201 (#201)')
  await page.getByTestId('sales-daily-date-input').locator('input').fill('2026-05-06')
  await page.getByTestId('sales-daily-date-input').locator('input').press('Tab')
  await page.getByTestId('sales-daily-amount-input').locator('input').fill('4100')
  await page.getByTestId('sales-daily-create-button').click()

  await expect(page.getByTestId('sales-daily-feedback')).toContainText('日销售已创建')
  await expect(page.getByText('4100')).toBeVisible()
})

test('filters daily sales rows by store and date range', async ({ page }) => {
  await attachSalesAdminMocks(page, {
    dailySales: [
      {
        id: 1001,
        store_id: 101,
        unit_id: 201,
        sale_date: '2026-05-01',
        sales_amount: 1100,
        created_at: now,
        updated_at: now,
      },
      {
        id: 1002,
        store_id: 102,
        unit_id: 202,
        sale_date: '2026-05-03',
        sales_amount: 2200,
        created_at: now,
        updated_at: now,
      },
    ],
  })

  await loginToSalesAdmin(page)

  await expect(page.getByText('2026-05-01')).toBeVisible()
  await expect(page.getByText('2026-05-03')).toBeVisible()

  await selectByTestId(page, 'sales-daily-filter-store', 'MI-101 — Harbor Center')
  await page.getByTestId('sales-daily-filter-date-from').locator('input').fill('2026-05-01')
  await page.getByTestId('sales-daily-filter-date-from').locator('input').press('Tab')
  await page.getByTestId('sales-daily-filter-date-to').locator('input').fill('2026-05-02')
  await page.getByTestId('sales-daily-filter-date-to').locator('input').press('Tab')
  await page.getByTestId('sales-daily-filter-submit').click()

  await expect(page.getByText('2026-05-01')).toBeVisible()
  await expect(page.getByText('1100')).toBeVisible()
  await expect(page.getByText('2026-05-03')).not.toBeVisible()
  await expect(page.getByText('2200')).not.toBeVisible()
})
