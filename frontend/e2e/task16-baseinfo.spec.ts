import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

type StoreTypeFixture = {
  id: number
  code: string
  name: string
  created_at: string
  updated_at: string
}

const now = '2026-04-02T08:00:00Z'

const attachBaseInfoMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'baseinfo.admin',
      permission_level: 'approve',
      can_print: false,
      can_export: false,
    },
  ]

  let storeTypes: StoreTypeFixture[] = [
    {
      id: 101,
      code: 'retail',
      name: 'Retail store',
      created_at: now,
      updated_at: now,
    },
  ]

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'baseinfo-admin',
          display_name: 'Base Info Admin',
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

  await page.route('**/api/base-info/shop-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ shop_types: [] }),
    })
  })

  await page.route('**/api/base-info/currency-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ currency_types: [] }),
    })
  })

  await page.route('**/api/base-info/trade-definitions**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ trade_definitions: [] }),
    })
  })

  await page.route('**/api/base-info/store-types**', async (route) => {
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as { code: string; name: string }

      const storeType: StoreTypeFixture = {
        id: storeTypes.length + 1000,
        code: payload.code,
        name: payload.name,
        created_at: now,
        updated_at: now,
      }
      storeTypes = [storeType, ...storeTypes]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ store_type: storeType }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as { code: string; name: string }

      const updated: StoreTypeFixture = {
        id,
        code: payload.code,
        name: payload.name,
        created_at: now,
        updated_at: now,
      }
      storeTypes = storeTypes.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ store_type: updated }),
      })
      return
    }

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ store_types: storeTypes }),
    })
  })
}

test('creates store type records from the base info admin view', async ({ page }) => {
  await attachBaseInfoMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('baseinfo-admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('nav--admin-base-info')).toBeVisible()

  await page.getByTestId('nav--admin-base-info').click()
  await expect(page).toHaveURL(/\/admin\/base-info/)
  await expect(page.getByTestId('baseinfo-admin-view')).toBeVisible()
  await expect(page.getByText('Retail store')).toBeVisible()

  await page.getByTestId('baseinfo-store-type-code-input').fill('outlet')
  await page.getByTestId('baseinfo-store-type-name-input').fill('Outlet')
  await page.getByTestId('baseinfo-store-type-create-button').click()

  await expect(page.getByText('Outlet', { exact: true })).toBeVisible()
  await expect(page.getByText('outlet', { exact: true })).toBeVisible()
})
