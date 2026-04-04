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

test('queries and exports the visual shop analysis report (R19)', async ({ page }) => {
  await attachReportingMocks(page)

  let latestQueryPayload: unknown = null
  let latestExportPayload: unknown = null

  const reportBody = {
    report_id: 'r19',
    columns: [
      { key: 'unit_code', label: 'Unit code' },
      { key: 'rent_status', label: 'Rent status' },
      { key: 'brand_name', label: 'Brand' },
      { key: 'customer_name', label: 'Customer' },
    ],
    rows: [
      {
        unit_code: 'A-101',
        rent_status: 'leased',
        brand_name: 'North Atelier',
        customer_name: 'North Retail Group',
      },
      {
        unit_code: 'A-102',
        rent_status: 'vacant',
        brand_name: null,
        customer_name: null,
      },
    ],
    generated_at: '2026-04-02T10:15:00Z',
    visual: {
      floor: {
        id: 8,
        name: 'Level 2 retail arcade',
        floor_plan_image_url: 'https://example.com/floor-plan-r19.png',
      },
      units: [
        {
          unit_id: 101,
          unit_code: 'A-101',
          unit_name: 'Arcade 101',
          floor_area: 82.5,
          rent_area: 78.2,
          rent_status: 'leased',
          brand_name: 'North Atelier',
          customer_name: 'North Retail Group',
          shop_type_name: 'Fashion',
          pos_x: 120,
          pos_y: 90,
          color_hex: '#2563eb',
        },
        {
          unit_id: 102,
          unit_code: 'A-102',
          unit_name: 'Arcade 102',
          floor_area: 76.4,
          rent_area: 74.1,
          rent_status: 'vacant',
          brand_name: null,
          customer_name: null,
          shop_type_name: 'Lifestyle',
          pos_x: 260,
          pos_y: 150,
          color_hex: '#b91c1c',
        },
      ],
      legend: [
        { label: 'leased', color_hex: '#2563eb' },
        { label: 'vacant', color_hex: '#b91c1c' },
      ],
    },
  }

  await page.route('**/api/reports/r19/query', async (route) => {
    latestQueryPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(reportBody),
    })
  })

  await page.route('**/api/reports/r19/export', async (route) => {
    latestExportPayload = route.request().postDataJSON()

    await route.fulfill({
      status: 200,
      contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      body: Buffer.from('mock-r19-export'),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('reporter')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('nav--reports-visual-shop')).toBeVisible()

  await page.getByTestId('nav--reports-visual-shop').click()
  await expect(page).toHaveURL(/\/reports\/visual-shop/)
  await expect(page.getByTestId('visual-shop-analysis-view')).toBeVisible()

  await page.getByTestId('visual-shop-store-input').fill('101')
  await page.getByTestId('visual-shop-floor-input').fill('8')
  await page.getByTestId('visual-shop-area-input').fill('3')
  await page.getByTestId('visual-shop-query-button').click()

  await expect.poll(() => latestQueryPayload).toEqual({ store_id: 101, floor_id: 8, area_id: 3 })
  await expect(page.getByTestId('visual-shop-canvas')).toBeVisible()
  await expect(page.getByTestId('visual-shop-legend')).toContainText('leased')
  await expect(page.getByTestId('visual-unit-marker')).toHaveCount(2)
  await expect(page.getByText('North Atelier')).toBeVisible()
  await expect(page.getByText('North Retail Group')).toBeVisible()

  await page.getByTestId('visual-unit-marker').nth(1).click()
  const detailPanel = page.locator('.visual-shop-analysis-view__detail-panel')
  await expect(detailPanel.getByTestId('visual-shop-selected-unit-code')).toHaveText('A-102')
  await expect(detailPanel.getByText('vacant')).toBeVisible()

  await page.getByTestId('visual-shop-export-button').click()
  await expect.poll(() => latestExportPayload).toEqual({ store_id: 101, floor_id: 8, area_id: 3 })
})
