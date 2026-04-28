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

const attachLeaseMocks = async (page: Page) => {
  const lease = {
    id: 1,
    amended_from_id: null,
    lease_no: 'L-2026-001',
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
    created_at: '2026-04-01T09:00:00Z',
    updated_at: '2026-04-01T09:00:00Z',
    units: [
      {
        id: 201,
        lease_contract_id: 1,
        unit_id: 301,
        rent_area: 120,
        created_at: '2026-04-01T09:00:00Z',
        updated_at: '2026-04-01T09:00:00Z',
      },
    ],
    terms: [
      {
        id: 401,
        lease_contract_id: 1,
        term_type: 'rent',
        billing_cycle: 'monthly',
        currency_type_id: 1,
        amount: 8000,
        effective_from: '2026-01-01',
        effective_to: '2026-12-31',
        created_at: '2026-04-01T09:00:00Z',
        updated_at: '2026-04-01T09:00:00Z',
      },
    ],
  }

  await page.route('**/api/leases**', async (route) => {
    const pathname = new URL(route.request().url()).pathname

    if (pathname.endsWith('/api/leases/1')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ lease }),
      })
      return
    }

    if (pathname.endsWith('/api/leases')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          items: [
            {
              id: lease.id,
              lease_no: lease.lease_no,
              tenant_name: lease.tenant_name,
              department_id: lease.department_id,
              store_id: lease.store_id,
              start_date: lease.start_date,
              end_date: lease.end_date,
              status: lease.status,
            },
          ],
          total: 1,
          page: 1,
          page_size: 20,
        }),
      })
      return
    }

    await route.fallback()
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
            created_at: '2026-04-01T09:00:00Z',
            updated_at: '2026-04-01T09:00:00Z',
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
            created_at: '2026-04-01T09:00:00Z',
            updated_at: '2026-04-01T09:00:00Z',
          },
        ],
      }),
    })
  })
}

const attachInvoiceMocks = async (page: Page) => {
  const invoice = {
    id: 501,
    document_type: 'invoice',
    document_no: 'INV-2026-0001',
    billing_run_id: 701,
    lease_contract_id: 101,
    tenant_name: 'Harbor Foods',
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
    created_at: '2026-04-01T09:00:00Z',
    updated_at: '2026-04-01T09:00:00Z',
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
        created_at: '2026-04-01T09:00:00Z',
      },
    ],
  }

  await page.route('**/api/invoices**', async (route) => {
    const pathname = new URL(route.request().url()).pathname

    if (pathname.endsWith('/api/invoices/501')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ document: invoice }),
      })
      return
    }

    if (pathname.endsWith('/api/invoices')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ items: [invoice], total: 1, page: 1, page_size: 20 }),
      })
      return
    }

    await route.fallback()
  })
}

const login = async (page: Page) => {
  await page.goto('/login')
  await page.evaluate(() => {
    window.localStorage.clear()
  })
  await page.reload()
  await expect(page.getByTestId('login-view')).toBeVisible({ timeout: 10000 })
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()
  await expect(page).toHaveURL(/\/dashboard/)
}

const switchLocale = async (page: Page, testId: string, optionLabel: string) => {
  await page.getByTestId(testId).locator('.el-select').click()
  await page.locator('.el-select-dropdown:visible .el-select-dropdown__item').filter({ hasText: optionLabel }).first().click()
}

const expectCardHeader = async (page: Page, headerText: string) => {
  await expect(page.locator('.el-card__header').filter({ hasText: headerText }).first()).toBeVisible()
}

test('switches language on the login page at runtime', async ({ page }) => {
  await page.goto('/login')
  await page.evaluate(() => {
    window.localStorage.clear()
  })
  await page.reload()
  await expect(page.getByTestId('login-view')).toBeVisible({ timeout: 10000 })

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
  await expect(page.getByTestId('nav--dashboard')).toHaveText('工作台概览')
  await expect(page.getByTestId('nav--health')).toHaveText('平台健康')
  await expect(page.getByTestId('nav--lease-contracts')).toHaveText('租赁合同')
  await expect(page.getByTestId('app-logout-button')).toHaveText('退出登录')
  await expect(page.getByTestId('dashboard-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '工作台概览' })).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByText('Legacy system migration')).toBeVisible()
  await expect(page.getByTestId('nav--dashboard')).toHaveText('Dashboard')
  await expect(page.getByTestId('nav--health')).toHaveText('Platform health')
  await expect(page.getByTestId('nav--lease-contracts')).toHaveText('Lease contracts')
  await expect(page.getByTestId('app-logout-button')).toHaveText('Logout')
  await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
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

test('shows zh-CN by default and switches billing invoice views to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, [
    {
      function_code: 'billing.invoice',
      permission_level: 'edit',
      can_print: true,
      can_export: true,
    },
  ])
  await attachInvoiceMocks(page)

  await login(page)

  await page.getByTestId('nav--billing-invoices').click()
  await expect(page).toHaveURL(/\/billing\/invoices/)
  await expect(page.getByRole('heading', { name: '账单发票' })).toBeVisible()
  await expect(page.getByText('单据筛选')).toBeVisible()
  await expect(page.getByText('单据队列')).toBeVisible()
  await expect(page.getByTestId('invoices-table')).toContainText('查看')

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Billing invoices' })).toBeVisible()
  await expect(page.getByText('Document filters')).toBeVisible()
  await expect(page.getByText('Invoice queue')).toBeVisible()
  await expect(page.getByTestId('invoices-table')).toContainText('View')

  await page
    .getByTestId('invoices-table')
    .locator('.el-table__row')
    .filter({ hasText: 'INV-2026-0001' })
    .locator('button')
    .first()
    .click()

  await expect(page).toHaveURL(/\/billing\/invoices\/501/)
  await expect(page.getByTestId('invoice-detail-view')).toBeVisible()
  await expect(page.getByText('Document overview')).toBeVisible()
  await expect(page.getByTestId('invoice-submit-button')).toHaveText('Submit for approval')
  await expect(page.getByText('Back to list')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByText('单据概览')).toBeVisible()
  await expect(page.getByTestId('invoice-submit-button')).toHaveText('提交审批')
  await expect(page.getByText('返回列表')).toBeVisible()
})

// ---------------------------------------------------------------------------
// Second-wave mock helpers: reporting / tax / print / admin
// ---------------------------------------------------------------------------

const attachReportMocks = async (page: Page) => {
  await page.route('**/api/reports/**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        columns: [],
        rows: [],
        generated_at: '2026-04-06T10:00:00Z',
      }),
    })
  })
}

const attachVisualShopMocks = async (page: Page) => {
  await page.route('**/api/reports/r19**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        visual: {
          floor: { id: 1, code: 'F-1', name: 'Floor 1', floor_plan_image_url: null },
          units: [],
          legend: [],
        },
        generated_at: '2026-04-06T10:00:00Z',
      }),
    })
  })
}

const attachTaxMocks = async (page: Page) => {
  await page.route('**/api/tax/rule-sets**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: [
          {
            id: 1,
            code: 'VAT-CN',
            name: 'China VAT',
            document_type: 'invoice',
            status: 'active',
            rules: [],
            created_at: '2026-04-01T09:00:00Z',
          },
        ],
        total: 1,
      }),
    })
  })

  await page.route('**/api/tax/export**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/octet-stream',
      body: Buffer.from('tax-export-mock'),
    })
  })
}

const attachPrintMocks = async (page: Page) => {
  await page.route('**/api/print/templates**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        items: [
          {
            id: 1,
            code: 'BILL-PDF',
            name: 'Billing PDF',
            document_type: 'invoice',
            output_mode: 'pdf',
            status: 'active',
            created_at: '2026-04-01T09:00:00Z',
          },
        ],
        total: 1,
      }),
    })
  })
}

const attachMasterDataMocks = async (page: Page) => {
  await page.route('**/api/master-data/customers**', async (route) => {
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
            created_at: '2026-04-01T09:00:00Z',
            updated_at: '2026-04-01T09:00:00Z',
          },
        ],
      }),
    })
  })

  await page.route('**/api/master-data/brands**', async (route) => {
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
            created_at: '2026-04-01T09:00:00Z',
            updated_at: '2026-04-01T09:00:00Z',
          },
        ],
      }),
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
}

const attachSalesMocks = async (page: Page) => {
  await page.route('**/api/sales/daily**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ daily_sales: [] }),
    })
  })

  await page.route('**/api/sales/traffic**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ customer_traffic: [] }),
    })
  })

  await page.route('**/api/structure/stores**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ stores: [] }),
    })
  })

  await page.route('**/api/structure/units**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ units: [] }),
    })
  })
}

const attachBaseInfoMocks = async (page: Page) => {
  await page.route('**/api/base-info/**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        store_types: [],
        shop_types: [],
        currency_types: [],
        trade_definitions: [],
        store_management_types: [],
        area_levels: [],
        unit_types: [],
      }),
    })
  })
}

const attachStructureMocks = async (page: Page) => {
  await page.route('**/api/structure/**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        stores: [],
        buildings: [],
        floors: [],
        areas: [],
        locations: [],
        units: [],
      }),
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
}

// ---------------------------------------------------------------------------
// Second-wave Playwright tests: reporting / tax / print / admin
// ---------------------------------------------------------------------------

const allSecondWavePermissions: PermissionFixture[] = [
  { function_code: 'reporting.generalize', permission_level: 'view', can_print: true, can_export: true },
  { function_code: 'tax.export', permission_level: 'view', can_print: false, can_export: true },
  { function_code: 'masterdata.admin', permission_level: 'edit', can_print: false, can_export: false },
  { function_code: 'sales.admin', permission_level: 'edit', can_print: false, can_export: false },
  { function_code: 'baseinfo.admin', permission_level: 'edit', can_print: false, can_export: false },
  { function_code: 'structure.admin', permission_level: 'edit', can_print: false, can_export: false },
]

test('shows zh-CN by default and switches GeneralizeReportsView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachReportMocks(page)

  await login(page)
  await page.goto('/reports/generalize')
  await expect(page).toHaveURL(/\/reports\/generalize/)

  await expect(page.getByTestId('generalize-reports-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '总务报表' })).toBeVisible()
  await expect(page.getByText('报表参数')).toBeVisible()
  await expect(page.getByTestId('generalize-query-button')).toHaveText('查询')
  await expect(page.getByTestId('generalize-export-button')).toHaveText('导出')

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Generalize reports' })).toBeVisible()
  await expect(page.getByText('Report parameters')).toBeVisible()
  await expect(page.getByTestId('generalize-query-button')).toHaveText('Query')
  await expect(page.getByTestId('generalize-export-button')).toHaveText('Export')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '总务报表' })).toBeVisible()
  await expect(page.getByTestId('generalize-query-button')).toHaveText('查询')
})

test('shows zh-CN by default and switches VisualShopAnalysisView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachReportMocks(page)
  await attachVisualShopMocks(page)

  await login(page)
  await page.goto('/reports/visual-shop')
  await expect(page).toHaveURL(/\/reports\/visual-shop/)

  await expect(page.getByTestId('visual-shop-analysis-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '商铺视觉分析' })).toBeVisible()
  await expect(page.getByText('图形筛选')).toBeVisible()
  await expect(page.getByTestId('visual-shop-query-button')).toHaveText('查询')
  await expect(page.getByTestId('visual-shop-export-button')).toHaveText('导出')
  await expect(page.getByText('楼层图形')).toBeVisible()
  await expect(page.getByTestId('visual-shop-legend')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Visual shop analysis' })).toBeVisible()
  await expect(page.getByText('Graphic filters')).toBeVisible()
  await expect(page.getByTestId('visual-shop-query-button')).toHaveText('Query')
  await expect(page.getByTestId('visual-shop-export-button')).toHaveText('Export')
  await expect(page.getByText('Floor graphic')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '商铺视觉分析' })).toBeVisible()
})

test('shows zh-CN by default and switches TaxExportsView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachTaxMocks(page)

  await login(page)
  await page.goto('/tax/exports')
  await expect(page).toHaveURL(/\/tax\/exports/)

  await expect(page.getByTestId('tax-exports-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '税务导出' })).toBeVisible()
  await expect(page.getByText('规则集筛选')).toBeVisible()
  await expect(page.getByText('凭证导出')).toBeVisible()
  await expect(page.getByTestId('tax-export-button')).toHaveText('导出凭证')
  await expect(page.getByText('可用规则集')).toBeVisible()
  await expect(page.getByTestId('tax-rulesets-table')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Tax exports' })).toBeVisible()
  await expect(page.getByText('Rule set filters')).toBeVisible()
  await expect(page.getByText('Voucher export')).toBeVisible()
  await expect(page.getByTestId('tax-export-button')).toHaveText('Export vouchers')
  await expect(page.getByText('Available rule sets')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '税务导出' })).toBeVisible()
  await expect(page.getByTestId('tax-export-button')).toHaveText('导出凭证')
})

test('shows zh-CN by default and switches PrintPreviewView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachPrintMocks(page)

  await login(page)
  await page.goto('/print/preview')
  await expect(page).toHaveURL(/\/print\/preview/)

  await expect(page.getByTestId('print-preview-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '打印预览' })).toBeVisible()
  await expectCardHeader(page, '生成 PDF')
  await expect(page.getByTestId('print-render-pdf-button')).toHaveText('生成 PDF')
  await expect(page.getByText('可用模板')).toBeVisible()
  await expect(page.getByTestId('print-templates-table')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Print preview' })).toBeVisible()
  await expectCardHeader(page, 'Render PDF')
  await expect(page.getByTestId('print-render-pdf-button')).toHaveText('Generate PDF')
  await expect(page.getByText('Available templates')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '打印预览' })).toBeVisible()
  await expect(page.getByTestId('print-render-pdf-button')).toHaveText('生成 PDF')
})

test('shows zh-CN by default and switches MasterDataAdminView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachMasterDataMocks(page)

  await login(page)
  await page.goto('/admin/master-data')
  await expect(page).toHaveURL(/\/admin\/master-data/)

  await expect(page.getByTestId('masterdata-admin-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '主数据管理' })).toBeVisible()
  await expectCardHeader(page, '新建客户')
  await expectCardHeader(page, '新建品牌')
  await expect(page.getByTestId('customer-create-button')).toHaveText('创建客户')
  await expect(page.getByTestId('brand-create-button')).toHaveText('创建品牌')
  await expect(page.getByTestId('customers-table')).toBeVisible()
  await expect(page.getByTestId('brands-table')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Master data admin' })).toBeVisible()
  await expectCardHeader(page, 'Create customer')
  await expectCardHeader(page, 'Create brand')
  await expect(page.getByTestId('customer-create-button')).toHaveText('Create customer')
  await expect(page.getByTestId('brand-create-button')).toHaveText('Create brand')
  await expectCardHeader(page, 'Customers')
  await expectCardHeader(page, 'Brands')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '主数据管理' })).toBeVisible()
  await expect(page.getByTestId('customer-create-button')).toHaveText('创建客户')
})

test('shows zh-CN by default and switches SalesAdminView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachSalesMocks(page)

  await login(page)
  await page.goto('/admin/sales')
  await expect(page).toHaveURL(/\/admin\/sales/)

  await expect(page.getByTestId('sales-admin-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '销售数据管理' })).toBeVisible()
  await expectCardHeader(page, '日销售')
  await expectCardHeader(page, '客流数据')
  await expect(page.getByTestId('sales-daily-create-button')).toHaveText('创建日销售')
  await expect(page.getByTestId('sales-traffic-create-button')).toHaveText('创建客流记录')

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Sales data admin' })).toBeVisible()
  await expectCardHeader(page, 'Daily sales')
  await expectCardHeader(page, 'Customer traffic')
  await expect(page.getByTestId('sales-daily-create-button')).toHaveText('Create daily sale')
  await expect(page.getByTestId('sales-traffic-create-button')).toHaveText('Create traffic record')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '销售数据管理' })).toBeVisible()
  await expect(page.getByTestId('sales-daily-create-button')).toHaveText('创建日销售')
})

test('shows zh-CN by default and switches BaseInfoAdminView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachBaseInfoMocks(page)

  await login(page)
  await page.goto('/admin/base-info')
  await expect(page).toHaveURL(/\/admin\/base-info/)

  await expect(page.getByTestId('baseinfo-admin-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '基础资料管理' })).toBeVisible()
  await expectCardHeader(page, '门店类型')
  await expectCardHeader(page, '业态类型')
  await expectCardHeader(page, '币种类型')
  await expectCardHeader(page, '业态定义')
  await expect(page.getByTestId('baseinfo-store-type-create-button')).toHaveText('创建门店类型')

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Base info admin' })).toBeVisible()
  await expectCardHeader(page, 'Store types')
  await expectCardHeader(page, 'Shop types')
  await expectCardHeader(page, 'Currency types')
  await expectCardHeader(page, 'Trade definitions')
  await expect(page.getByTestId('baseinfo-store-type-create-button')).toHaveText('Create store type')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '基础资料管理' })).toBeVisible()
  await expect(page.getByTestId('baseinfo-store-type-create-button')).toHaveText('创建门店类型')
})

test('shows zh-CN by default and switches StructureAdminView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachStructureMocks(page)
  await attachBaseInfoMocks(page)

  await login(page)
  await page.goto('/admin/structure')
  await expect(page).toHaveURL(/\/admin\/structure/)

  await expect(page.getByTestId('structure-admin-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '业态结构管理' })).toBeVisible()
  await expectCardHeader(page, '门店')
  await expect(page.getByTestId('structure-store-create-button')).toHaveText('创建门店')
  await expect(page.getByTestId('structure-stores-table')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Structure admin' })).toBeVisible()
  await expectCardHeader(page, 'Stores')
  await expect(page.getByTestId('structure-store-create-button')).toHaveText('Create store')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '业态结构管理' })).toBeVisible()
  await expect(page.getByTestId('structure-store-create-button')).toHaveText('创建门店')
})

test('shows zh-CN by default and switches RentableAreaAdminView to English at runtime', async ({ page }) => {
  await attachPlatformMocks(page, allSecondWavePermissions)
  await attachStructureMocks(page)
  await attachBaseInfoMocks(page)

  await login(page)
  await page.goto('/admin/rentable-areas')
  await expect(page).toHaveURL(/\/admin\/rentable-areas/)

  await expect(page.getByTestId('rentable-area-admin-view')).toBeVisible()
  await expect(page.getByRole('heading', { name: '可租区域' })).toBeVisible()
  await expect(page.getByText('可租区域筛选')).toBeVisible()
  await expect(page.getByTestId('rentable-area-apply-button')).toHaveText('应用筛选')
  await expect(page.getByTestId('rentable-area-reset-button')).toHaveText('重置')
  await expect(page.getByTestId('rentable-area-units-table')).toBeVisible()

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Rentable areas' })).toBeVisible()
  await expect(page.getByText('Rentable area filters')).toBeVisible()
  await expect(page.getByTestId('rentable-area-apply-button')).toHaveText('Apply filters')
  await expect(page.getByTestId('rentable-area-reset-button')).toHaveText('Reset')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByRole('heading', { name: '可租区域' })).toBeVisible()
  await expect(page.getByTestId('rentable-area-apply-button')).toHaveText('应用筛选')
})

test('shows zh-CN by default and switches lease create/detail views to English at runtime', async ({ page }) => {
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

  await page.getByTestId('nav--lease-contracts').click()
  await expect(page).toHaveURL(/\/lease\/contracts/)
  await page.getByTestId('lease-create-button').click()

  await expect(page).toHaveURL(/\/lease\/contracts\/new/)
  await expect(page.getByRole('heading', { name: '新建租赁合同' })).toBeVisible()
  await expect(page.getByText('合同设定')).toBeVisible()
  await expect(page.getByTestId('lease-create-submit-button')).toHaveText('创建合同')
  await expect(page.getByTestId('lease-create-back-button')).toHaveText('返回列表')

  await switchLocale(page, 'app-locale-switcher', 'English')

  await expect(page.getByRole('heading', { name: 'Create lease contract' })).toBeVisible()
  await expect(page.getByText('Lease contract setup')).toBeVisible()
  await expect(page.getByTestId('lease-create-submit-button')).toHaveText('Create lease')
  await expect(page.getByTestId('lease-create-back-button')).toHaveText('Back to list')

  await page.goto('/lease/contracts/1')

  await expect(page.getByTestId('lease-detail-view')).toBeVisible()
  await expect(page.getByText('Contract overview')).toBeVisible()
  await expect(page.getByText('Workflow actions')).toBeVisible()
  await expect(page.getByTestId('lease-submit-button')).toHaveText('Submit for approval')

  await switchLocale(page, 'app-locale-switcher', 'Chinese (Simplified)')

  await expect(page.getByText('合同概览')).toBeVisible()
  await expect(page.getByText('流程操作')).toBeVisible()
  await expect(page.getByTestId('lease-submit-button')).toHaveText('提交审批')
  await expect(page.getByTestId('lease-detail-back-button')).toHaveText('返回列表')
})
