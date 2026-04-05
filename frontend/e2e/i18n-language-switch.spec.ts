import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

const attachPlatformMocks = async (page: Page, permissions: PermissionFixture[]) => {
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
}

const attachLeaseMocks = async (page: Page) => {
  await page.route('**/api/leases**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: [
          {
            id: 1,
            lease_no: 'L-2026-001',
            tenant_name: 'Harbor Foods',
            department_id: 10,
            store_id: 5,
            start_date: '2026-01-01',
            end_date: '2026-12-31',
            status: 'active',
          },
        ],
        total: 1,
        page: 1,
        page_size: 20,
      }),
    })
  })
}

const login = async (page: Page) => {
  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()
  await expect(page).toHaveURL(/\/health/)
}

const switchLocale = async (page: Page, testId: string, optionLabel: string) => {
  await page.getByTestId(testId).locator('.el-select').click()
  await page.locator('.el-select-dropdown:visible .el-select-dropdown__item').filter({ hasText: optionLabel }).first().click()
}

test('switches language on the login page at runtime', async ({ page }) => {
  await page.goto('/login')

  await expect(page.getByText('登录以继续')).toBeVisible()
  await expect(page.getByText('用户名')).toBeVisible()
  await expect(page.getByTestId('login-submit-button')).toHaveText('登录')

  await switchLocale(page, 'login-locale-switcher', 'English')

  await expect(page.getByText('Sign in to continue')).toBeVisible()
  await expect(page.getByText('Username')).toBeVisible()
  await expect(page.getByTestId('login-submit-button')).toHaveText('Sign in')

  await switchLocale(page, 'login-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByText('登录以继续')).toBeVisible()
  await expect(page.getByTestId('login-submit-button')).toHaveText('登录')
})

test('shows zh-CN by default and switches shell, navigation, and health view to English', async ({ page }) => {
  await attachPlatformMocks(page, [
    {
      function_code: 'lease.contract',
      permission_level: 'edit',
      can_print: true,
      can_export: false,
    },
    {
      function_code: 'excel.io',
      permission_level: 'view',
      can_print: false,
      can_export: true,
    },
  ])

  await login(page)

  await expect(page.getByText('遗留系统迁移')).toBeVisible()
  await expect(page.getByTestId('nav--health')).toHaveText('平台健康')
  await expect(page.getByTestId('nav--lease-contracts')).toHaveText('租赁合同')
  await expect(page.getByTestId('app-logout-button')).toHaveText('退出登录')
  await expect(page.getByRole('heading', { name: '平台健康检查' })).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByText('Legacy system migration')).toBeVisible()
  await expect(page.getByTestId('nav--health')).toHaveText('Platform health')
  await expect(page.getByTestId('nav--lease-contracts')).toHaveText('Lease contracts')
  await expect(page.getByTestId('app-logout-button')).toHaveText('Logout')
  await expect(page.getByRole('heading', { name: 'Platform health' })).toBeVisible()
})

test('persists the selected locale across navigation and refresh on a first-wave route', async ({ page }) => {
  await attachPlatformMocks(page, [
    {
      function_code: 'lease.contract',
      permission_level: 'edit',
      can_print: true,
      can_export: false,
    },
  ])
  await attachLeaseMocks(page)

  await login(page)
  await switchLocale(page, 'app-locale-switcher', 'English')

  await page.getByTestId('nav--lease-contracts').click()
  await expect(page).toHaveURL(/\/lease\/contracts/)
  await expect(page.getByRole('heading', { name: 'Lease contracts' })).toBeVisible()
  await expect(page.getByText('Contract queue')).toBeVisible()
  await expect(page.getByTestId('lease-create-button')).toHaveText('Create contract')

  await page.reload()

  await expect(page.getByRole('heading', { name: 'Lease contracts' })).toBeVisible()
  await expect(page.getByTestId('nav--lease-contracts')).toHaveText('Lease contracts')
  expect(await page.evaluate(() => window.localStorage.getItem('mi.app.locale'))).toBe('en-US')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')
  await expect(page.getByRole('heading', { name: '租赁合同' })).toBeVisible()

  await page.reload()

  await expect(page.getByRole('heading', { name: '租赁合同' })).toBeVisible()
  expect(await page.evaluate(() => window.localStorage.getItem('mi.app.locale'))).toBe('zh-CN')
})
