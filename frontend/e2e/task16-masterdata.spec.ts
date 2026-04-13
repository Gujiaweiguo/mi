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

type UnitRentBudgetFixture = {
  unit_id: number
  fiscal_year: number
  budget_price: number
  created_at: string
  updated_at: string
}

type StoreRentBudgetFixture = {
  store_id: number
  fiscal_year: number
  fiscal_month: number
  monthly_budget: number
  created_at: string
  updated_at: string
}

type UnitProspectFixture = {
  unit_id: number
  fiscal_year: number
  potential_customer_id: number | null
  prospect_brand_id: number | null
  prospect_trade_id: number | null
  avg_transaction: number | null
  prospect_rent_price: number | null
  rent_increment: string | null
  prospect_term_months: number | null
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

  let unitBudgets: UnitRentBudgetFixture[] = [
    {
      unit_id: 101,
      fiscal_year: 2026,
      budget_price: 95,
      created_at: '2026-04-02T08:00:00Z',
      updated_at: '2026-04-02T08:00:00Z',
    },
  ]

  let storeBudgets: StoreRentBudgetFixture[] = [
    {
      store_id: 101,
      fiscal_year: 2026,
      fiscal_month: 4,
      monthly_budget: 10000,
      created_at: '2026-04-02T08:00:00Z',
      updated_at: '2026-04-02T08:00:00Z',
    },
  ]

  let unitProspects: UnitProspectFixture[] = [
    {
      unit_id: 101,
      fiscal_year: 2026,
      potential_customer_id: 101,
      prospect_brand_id: 101,
      prospect_trade_id: 102,
      avg_transaction: 280,
      prospect_rent_price: 110,
      rent_increment: '5% yearly',
      prospect_term_months: 36,
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

  await page.route(/.*\/api\/master-data\/customers(\/\d+)?(\?.*)?$/, async (route) => {
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

    if (route.request().method() === 'PUT') {
      const payload = route.request().postDataJSON() as CustomerFixture
      customers = customers.map((customer) => (customer.id === payload.id ? { ...customer, ...payload, updated_at: '2026-04-02T09:00:00Z' } : customer))
      const customer = customers.find((item) => item.id === payload.id)
      await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ customer }) })
      return
    }

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ customers, total: customers.length, page: 1, page_size: 10 }),
    })
  })

  await page.route(/.*\/api\/master-data\/brands(\/\d+)?(\?.*)?$/, async (route) => {
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

    if (route.request().method() === 'PUT') {
      const payload = route.request().postDataJSON() as BrandFixture
      brands = brands.map((brand) => (brand.id === payload.id ? { ...brand, ...payload, updated_at: '2026-04-02T09:01:00Z' } : brand))
      const brand = brands.find((item) => item.id === payload.id)
      await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ brand }) })
      return
    }

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ brands, total: brands.length, page: 1, page_size: 10 }),
    })
  })

  await page.route(/.*\/api\/master-data\/unit-rent-budgets(\/.*)?$/, async (route) => {
    if (route.request().method() === 'POST' || route.request().method() === 'PUT') {
      const payload = route.request().postDataJSON() as UnitRentBudgetFixture
      unitBudgets = [...unitBudgets.filter((item) => !(item.unit_id === payload.unit_id && item.fiscal_year === payload.fiscal_year)), { ...payload, created_at: '2026-04-02T09:10:00Z', updated_at: '2026-04-02T09:10:00Z' }]
      const unit_rent_budget = unitBudgets.find((item) => item.unit_id === payload.unit_id && item.fiscal_year === payload.fiscal_year)
      await route.fulfill({ status: route.request().method() === 'POST' ? 201 : 200, contentType: 'application/json', body: JSON.stringify({ unit_rent_budget }) })
      return
    }

    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ unit_rent_budgets: unitBudgets }) })
  })

  await page.route(/.*\/api\/master-data\/store-rent-budgets(\/.*)?$/, async (route) => {
    if (route.request().method() === 'POST' || route.request().method() === 'PUT') {
      const payload = route.request().postDataJSON() as StoreRentBudgetFixture
      storeBudgets = [...storeBudgets.filter((item) => !(item.store_id === payload.store_id && item.fiscal_year === payload.fiscal_year && item.fiscal_month === payload.fiscal_month)), { ...payload, created_at: '2026-04-02T09:11:00Z', updated_at: '2026-04-02T09:11:00Z' }]
      const store_rent_budget = storeBudgets.find((item) => item.store_id === payload.store_id && item.fiscal_year === payload.fiscal_year && item.fiscal_month === payload.fiscal_month)
      await route.fulfill({ status: route.request().method() === 'POST' ? 201 : 200, contentType: 'application/json', body: JSON.stringify({ store_rent_budget }) })
      return
    }

    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ store_rent_budgets: storeBudgets }) })
  })

  await page.route(/.*\/api\/master-data\/unit-prospects(\/.*)?$/, async (route) => {
    if (route.request().method() === 'POST' || route.request().method() === 'PUT') {
      const payload = route.request().postDataJSON() as UnitProspectFixture
      unitProspects = [...unitProspects.filter((item) => !(item.unit_id === payload.unit_id && item.fiscal_year === payload.fiscal_year)), { ...payload, created_at: '2026-04-02T09:12:00Z', updated_at: '2026-04-02T09:12:00Z' }]
      const unit_prospect = unitProspects.find((item) => item.unit_id === payload.unit_id && item.fiscal_year === payload.fiscal_year)
      await route.fulfill({ status: route.request().method() === 'POST' ? 201 : 200, contentType: 'application/json', body: JSON.stringify({ unit_prospect }) })
      return
    }

    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ unit_prospects: unitProspects }) })
  })
}

test('creates and updates supporting-domain records from the master data admin view', async ({ page }) => {
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

  await page.getByTestId('customer-edit-button').first().click()
  await page.getByTestId('customer-name-input').fill('ACME Retail Updated')
  await page.getByTestId('customer-create-button').click()
  await expect(page.getByTestId('customers-table')).toContainText('ACME Retail Updated')

  await page.getByTestId('brand-edit-button').first().click()
  await page.getByTestId('brand-name-input').fill('ACME Fashion Updated')
  await page.getByTestId('brand-create-button').click()
  await expect(page.getByTestId('brands-table')).toContainText('ACME Fashion Updated')

  await page.getByTestId('unit-budget-unit-input').locator('input').fill('102')
  await page.getByTestId('unit-budget-year-input').locator('input').fill('2027')
  await page.getByTestId('unit-budget-price-input').locator('input').fill('123.45')
  await page.getByTestId('unit-budget-save-button').click()
  await expect(page.getByTestId('unit-budgets-table')).toContainText('123.45')

  await page.getByTestId('store-budget-store-input').locator('input').fill('101')
  await page.getByTestId('store-budget-year-input').locator('input').fill('2027')
  await page.getByTestId('store-budget-month-input').locator('input').fill('5')
  await page.getByTestId('store-budget-value-input').locator('input').fill('12500')
  await page.getByTestId('store-budget-save-button').click()
  await expect(page.getByTestId('store-budgets-table')).toContainText('12500')

  await page.getByTestId('unit-prospect-unit-input').locator('input').fill('102')
  await page.getByTestId('unit-prospect-year-input').locator('input').fill('2027')
  await page.getByTestId('unit-prospect-customer-input').click()
  await page.getByRole('option', { name: /ACME Retail/ }).click()
  await page.getByTestId('unit-prospect-brand-input').click()
  await page.getByRole('option', { name: /ACME Fashion/ }).click()
  await page.getByTestId('unit-prospect-price-input').locator('input').fill('140')
  await page.getByTestId('unit-prospect-term-input').locator('input').fill('24')
  await page.getByTestId('unit-prospect-save-button').click()
  await expect(page.getByTestId('unit-prospects-table')).toContainText('140')
})
