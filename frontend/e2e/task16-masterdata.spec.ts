import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

type CustomerFixture = {
  id: number
  code: string
  name: string
  trade_id: number | null
  department_id: number | null
  status: string
  created_at: string
  updated_at: string
}

type BrandFixture = {
  id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

const attachMasterdataMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'masterdata.admin',
      permission_level: 'approve',
      can_print: false,
      can_export: false,
    },
  ]

  let customers: CustomerFixture[] = [
    {
      id: 101,
      code: 'CUST-101',
      name: 'ACME Retail',
      trade_id: 102,
      department_id: 101,
      status: 'active',
      created_at: '2026-04-02T08:00:00Z',
      updated_at: '2026-04-02T08:00:00Z',
    },
  ]

  let brands: BrandFixture[] = [
    {
      id: 101,
      code: 'BR-101',
      name: 'ACME Fashion',
      status: 'active',
      created_at: '2026-04-02T08:00:00Z',
      updated_at: '2026-04-02T08:00:00Z',
    },
  ]

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'masterdata',
          display_name: 'Master Data Admin',
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

  await page.route('**/api/org/departments', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        departments: [
          {
            id: 101,
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

  await page.route('**/api/master-data/customers', async (route) => {
    if (route.request().method() === 'POST') {
      const payload = route.request().postDataJSON() as {
        code: string
        name: string
        trade_id?: number | null
        department_id?: number | null
        status?: string
      }

      const customer = {
        id: customers.length + 1000,
        code: payload.code,
        name: payload.name,
        trade_id: payload.trade_id ?? null,
        department_id: payload.department_id ?? null,
        status: payload.status ?? 'active',
        created_at: '2026-04-02T08:30:00Z',
        updated_at: '2026-04-02T08:30:00Z',
      }
      customers = [...customers, customer]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ customer }),
      })
      return
    }

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ customers }),
    })
  })

  await page.route('**/api/master-data/brands', async (route) => {
    if (route.request().method() === 'POST') {
      const payload = route.request().postDataJSON() as {
        code: string
        name: string
        status?: string
      }

      const brand = {
        id: brands.length + 1000,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: '2026-04-02T08:31:00Z',
        updated_at: '2026-04-02T08:31:00Z',
      }
      brands = [...brands, brand]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ brand }),
      })
      return
    }

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ brands }),
    })
  })
}

test('creates customer and brand records from the master data admin view', async ({ page }) => {
  await attachMasterdataMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('masterdata')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('nav--admin-master-data')).toBeVisible()

  await page.getByTestId('nav--admin-master-data').click()
  await expect(page).toHaveURL(/\/admin\/master-data/)
  await expect(page.getByTestId('masterdata-admin-view')).toBeVisible()
  await expect(page.getByTestId('customers-table')).toContainText('ACME Retail')
  await expect(page.getByTestId('brands-table')).toContainText('ACME Fashion')

  await page.getByTestId('customer-code-input').fill('CUST-102')
  await page.getByTestId('customer-name-input').fill('Harbor Retail')
  await page.getByTestId('customer-trade-input').locator('input').fill('102')
  await page.getByTestId('customer-department-input').click()
  await page.getByRole('option', { name: /Operations/ }).click()
  await page.getByTestId('customer-create-button').click()

  await expect(page.getByTestId('customers-table')).toContainText('Harbor Retail')
  await expect(page.getByTestId('customers-table')).toContainText('CUST-102')

  await page.getByTestId('brand-code-input').fill('BR-102')
  await page.getByTestId('brand-name-input').fill('Harbor Signature')
  await page.getByTestId('brand-create-button').click()

  await expect(page.getByTestId('brands-table')).toContainText('Harbor Signature')
  await expect(page.getByTestId('brands-table')).toContainText('BR-102')
})
