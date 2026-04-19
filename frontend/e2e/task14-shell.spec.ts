import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

const attachPlatformMocks = async (
  page: Page,
  permissions: PermissionFixture[],
  options?: { healthStatus?: number },
) => {
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

  await page.route('**/api/auth/login', async (route) => { await route.fulfill({
    status: 200,
    contentType: 'application/json',
    body: JSON.stringify({
      token: 'playwright-token',
    }),
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
    const status = options?.healthStatus ?? 200

    await route.fulfill({
      status,
      contentType: 'application/json',
      body: JSON.stringify(
        status >= 400
          ? { message: 'backend unavailable' }
          : { status: 'ok', service: 'frontend-platform-test' },
      ),
    })
  })
}

test('redirects protected dashboard route to login for guests', async ({ page }) => {
  await page.goto('/dashboard')

  await expect(page).toHaveURL(/\/login/)
  await expect(page.getByTestId('login-view')).toBeVisible()
  await expect(page.getByTestId('login-username-input')).toBeVisible()
  await expect(page.getByTestId('login-password-input')).toBeVisible()
  await expect(page.getByTestId('login-submit-button')).toBeVisible()
})

test('shows permission-aware navigation after login and supports shared workbench filtering', async ({ page }) => {
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

  await page.route('**/api/leases**', async (route) => {
    const url = new URL(route.request().url())
    const leaseNo = url.searchParams.get('lease_no') ?? ''

    const allItems = [
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
      {
        id: 2,
        lease_no: 'L-2026-002',
        tenant_name: 'North Arcade',
        department_id: 20,
        store_id: 3,
        start_date: '2026-02-01',
        end_date: '2026-12-31',
        status: 'draft',
      },
    ]

    const filtered = leaseNo
      ? allItems.filter(
          (item) =>
            item.lease_no.toLowerCase().includes(leaseNo.toLowerCase()) ||
            item.tenant_name.toLowerCase().includes(leaseNo.toLowerCase()),
        )
      : allItems

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: filtered,
        total: filtered.length,
        page: 1,
        page_size: 20,
      }),
    })
  })

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/dashboard/)
  await expect(page.getByTestId('nav--dashboard')).toBeVisible()
  await expect(page.getByTestId('nav--health')).toBeVisible()
  await expect(page.getByTestId('nav--lease-contracts')).toBeVisible()
  await expect(page.getByTestId('nav--excel-io')).toBeVisible()
  await expect(page.getByTestId('nav--workflow-admin')).toHaveCount(0)

  await page.getByTestId('nav--lease-contracts').click()
  await expect(page).toHaveURL(/\/lease\/contracts/)
  await expect(page.getByTestId('lease-contracts-view')).toBeVisible()
  await page.getByTestId('lease-contracts-view-query-input').fill('harbor')
  await page.getByRole('button', { name: '应用筛选' }).click()
  await expect(page.getByText('Harbor Foods')).toBeVisible()
  await expect(page.getByText('North Arcade')).toHaveCount(0)
})

test('redirects unauthorized routes to forbidden and surfaces centralized API errors', async ({ page }) => {
  await page.addInitScript(() => {
    window.localStorage.setItem('mi.auth.token', 'playwright-token')
  })
  await attachPlatformMocks(
    page,
    [
      {
        function_code: 'lease.contract',
        permission_level: 'view',
        can_print: false,
        can_export: false,
      },
    ],
    { healthStatus: 503 },
  )

  await page.goto('/tax/exports')
  await expect(page).toHaveURL(/\/forbidden/)
  await expect(page.getByTestId('forbidden-view')).toBeVisible()

  await page.goto('/health')
  await expect(page.getByTestId('global-error-alert')).toContainText('backend unavailable')
  await expect(page.getByTestId('health-error-alert')).toContainText('backend unavailable')
})
