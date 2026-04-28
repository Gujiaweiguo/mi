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

  await page.route('**/api/dashboard/summary', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        activeLeases: 1,
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
      body: JSON.stringify({ status: 'ok', service: 'frontend-playwright-test' }),
    })
  })
}

const attachReferenceDataMocks = async (page: Page) => {
  await page.route('**/api/org/departments', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        departments: [{ id: 10, code: 'OPS', name: 'Operations', level: 1, status: 'active', parent_id: null, type_id: 1 }],
      }),
    })
  })

  await page.route('**/api/org/stores', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        stores: [{ id: 5, department_id: 10, code: 'MI-001', name: 'MI Demo Mall', short_name: 'MI Mall', status: 'active' }],
      }),
    })
  })

  await page.route('**/api/master-data/customers', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        customers: [{ id: 101, code: 'CUST-101', name: 'Harbor Foods', trade_id: 102, department_id: 10, status: 'active' }],
      }),
    })
  })

  await page.route('**/api/master-data/brands', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        brands: [{ id: 101, code: 'BR-101', name: 'Harbor Signature', status: 'active' }],
      }),
    })
  })
}

const login = async (page: Page) => {
  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('operator')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()
  await expect(page).toHaveURL(/\/dashboard/)
}

const selectComboboxOption = async (page: Page, name: string | RegExp, optionLabel: string, index = 0) => {
  const combobox = page.getByRole('combobox', { name }).nth(index)
  await combobox.click({ force: true })
  await combobox.press('ArrowDown')
  await page.locator('.el-select-dropdown:visible .el-select-dropdown__item').filter({ hasText: optionLabel }).first().click()
}

test('creates an ad-board lease with repeated subtype rows', async ({ page }) => {
  await attachPlatformMocks(page, [
    { function_code: 'lease.contract', permission_level: 'edit', can_print: true, can_export: false },
    { function_code: 'billing.charge', permission_level: 'edit', can_print: false, can_export: false },
  ])
  await attachReferenceDataMocks(page)

  let createdPayload: Record<string, unknown> | null = null
  const createdLeaseId = 900

  await page.route('**/api/leases**', async (route) => {
    const request = route.request()
    const pathname = new URL(request.url()).pathname

    if (request.method() === 'POST' && pathname.endsWith('/api/leases')) {
      createdPayload = request.postDataJSON() as Record<string, unknown>
      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({
          lease: {
            id: createdLeaseId,
            amended_from_id: null,
            lease_no: createdPayload.lease_no,
            subtype: createdPayload.subtype,
            department_id: 10,
            store_id: 5,
            building_id: null,
            customer_id: 101,
            brand_id: 101,
            trade_id: null,
            management_type_id: null,
            tenant_name: createdPayload.tenant_name,
            start_date: createdPayload.start_date,
            end_date: createdPayload.end_date,
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
            joint_operation: null,
            ad_boards: createdPayload.ad_boards ?? [],
            area_grounds: [],
            units: [{ id: 1, lease_contract_id: createdLeaseId, unit_id: 501, rent_area: 120, created_at: '2026-04-01T09:00:00Z', updated_at: '2026-04-01T09:00:00Z' }],
            terms: [{ id: 1, lease_contract_id: createdLeaseId, term_type: 'rent', billing_cycle: 'monthly', currency_type_id: 1, amount: 5000, effective_from: '2026-01-01', effective_to: '2026-12-31', created_at: '2026-04-01T09:00:00Z', updated_at: '2026-04-01T09:00:00Z' }],
          },
        }),
      })
      return
    }

    if (request.method() === 'GET' && pathname.endsWith(`/api/leases/${createdLeaseId}`)) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          lease: {
            id: createdLeaseId,
            amended_from_id: null,
            lease_no: createdPayload?.lease_no ?? 'L-AD-001',
            subtype: createdPayload?.subtype ?? 'ad_board',
            department_id: 10,
            store_id: 5,
            building_id: null,
            customer_id: 101,
            brand_id: 101,
            trade_id: null,
            management_type_id: null,
            tenant_name: createdPayload?.tenant_name ?? 'Harbor Foods',
            start_date: createdPayload?.start_date ?? '2026-01-01',
            end_date: createdPayload?.end_date ?? '2026-12-31',
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
            joint_operation: null,
            ad_boards: createdPayload?.ad_boards ?? [],
            area_grounds: [],
            units: [{ id: 1, lease_contract_id: createdLeaseId, unit_id: 501, rent_area: 120, created_at: '2026-04-01T09:00:00Z', updated_at: '2026-04-01T09:00:00Z' }],
            terms: [{ id: 1, lease_contract_id: createdLeaseId, term_type: 'rent', billing_cycle: 'monthly', currency_type_id: 1, amount: 5000, effective_from: '2026-01-01', effective_to: '2026-12-31', created_at: '2026-04-01T09:00:00Z', updated_at: '2026-04-01T09:00:00Z' }],
          },
        }),
      })
      return
    }

    if (request.method() === 'GET' && pathname.endsWith('/api/overtime/bills')) {
      await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ items: [], total: 0, page: 1, page_size: 100 }) })
      return
    }

    await route.fallback()
  })

  await login(page)
  await page.goto('/lease/contracts/new')

  await page.getByTestId('lease-number-input').fill('L-AD-001')
  await page.getByTestId('lease-tenant-name-input').fill('Harbor Foods')
  const subtypeCombobox = page.getByRole('combobox', { name: /合同子类型/ })
  await subtypeCombobox.click({ force: true })
  await subtypeCombobox.press('ArrowDown')
  await page.waitForTimeout(100)
  await subtypeCombobox.press('ArrowDown')
  await page.waitForTimeout(100)
  await subtypeCombobox.press('Enter')
  await selectComboboxOption(page, /部门/, 'OPS — Operations')
  await selectComboboxOption(page, /门店/, 'MI-001 — MI Demo Mall', 0)
  await page.getByRole('combobox', { name: /开始日期/ }).nth(0).fill('2026-01-01')
  await page.getByRole('combobox', { name: /结束日期/ }).nth(0).fill('2026-12-31')
  await page.getByRole('spinbutton', { name: /单元 ID/ }).fill('501')
  await page.getByRole('spinbutton', { name: /租赁面积/ }).nth(0).fill('120')
  await page.getByRole('spinbutton', { name: /金额/ }).fill('5000')
  await page.getByRole('combobox', { name: /生效开始/ }).fill('2026-01-01')
  await page.getByRole('combobox', { name: /生效结束/ }).fill('2026-12-31')

  await page.getByRole('spinbutton', { name: /广告位 ID/ }).nth(0).fill('901')
  await page.getByRole('textbox', { name: /广告位说明/ }).nth(0).fill('North atrium screen')
  await page.getByRole('combobox', { name: /开始日期/ }).nth(1).fill('2026-01-01')
  await page.getByRole('combobox', { name: /结束日期/ }).nth(1).fill('2026-03-31')
  await page.getByRole('spinbutton', { name: /租赁面积/ }).nth(1).fill('16')
  await page.getByRole('spinbutton', { name: /投放时长/ }).nth(0).fill('20')

  await page.getByTestId('lease-ad-board-add-button').click()
  await page.getByRole('spinbutton', { name: /广告位 ID/ }).nth(1).fill('902')
  await page.getByRole('textbox', { name: /广告位说明/ }).nth(1).fill('South atrium screen')
  await page.getByRole('combobox', { name: /开始日期/ }).nth(2).fill('2026-04-01')
  await page.getByRole('combobox', { name: /结束日期/ }).nth(2).fill('2026-06-30')
  await page.getByRole('spinbutton', { name: /租赁面积/ }).nth(2).fill('18')
  await page.getByRole('spinbutton', { name: /投放时长/ }).nth(1).fill('10')

  await page.getByTestId('lease-create-submit-button').click()

  await expect(page).toHaveURL(new RegExp(`/lease/contracts/${createdLeaseId}$`))
  expect(createdPayload?.subtype).toBe('ad_board')
  expect(Array.isArray(createdPayload?.ad_boards)).toBe(true)
  expect((createdPayload?.ad_boards as Array<unknown>).length).toBe(2)
})

test('creates and actions overtime bills from lease detail', async ({ page }) => {
  await attachPlatformMocks(page, [
    { function_code: 'lease.contract', permission_level: 'edit', can_print: true, can_export: false },
    { function_code: 'billing.charge', permission_level: 'edit', can_print: false, can_export: false },
  ])

  const overtimeBills: Array<Record<string, unknown>> = [
    {
      id: 701,
      lease_contract_id: 42,
      lease_no: 'L-042',
      tenant_name: 'Harbor Foods',
      period_start: '2026-05-01',
      period_end: '2026-05-31',
      status: 'approved',
      workflow_instance_id: 81,
      note: 'Approved overtime',
      submitted_at: '2026-05-02T09:00:00Z',
      approved_at: '2026-05-03T09:00:00Z',
      rejected_at: null,
      cancelled_at: null,
      stopped_at: null,
      generated_at: null,
      created_by: 1,
      updated_by: 1,
      created_at: '2026-05-01T09:00:00Z',
      updated_at: '2026-05-03T09:00:00Z',
      formulas: [],
      generated_charges: [],
    },
  ]

  await page.route('**/api/leases/42', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        lease: {
          id: 42,
          amended_from_id: null,
          lease_no: 'L-042',
          subtype: 'joint_operation',
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
          status: 'active',
          workflow_instance_id: 77,
          effective_version: 1,
          submitted_at: '2026-01-02T09:00:00Z',
          approved_at: '2026-01-03T09:00:00Z',
          billing_effective_at: '2026-01-03T09:00:00Z',
          terminated_at: null,
          created_by: 1,
          updated_by: 1,
          created_at: '2026-01-01T09:00:00Z',
          updated_at: '2026-01-03T09:00:00Z',
          joint_operation: {
            bill_cycle: 30,
            rent_inc: '5% yearly',
            account_cycle: 30,
            tax_rate: 0.09,
            tax_type: 1,
            settlement_currency_type_id: 1,
            in_tax_rate: 0.03,
            out_tax_rate: 0.06,
            month_settle_days: 25,
            late_pay_interest_rate: 0.01,
            interest_grace_days: 5,
          },
          ad_boards: [],
          area_grounds: [],
          units: [{ id: 1, lease_contract_id: 42, unit_id: 501, rent_area: 120, created_at: '2026-01-01T09:00:00Z', updated_at: '2026-01-01T09:00:00Z' }],
          terms: [{ id: 1, lease_contract_id: 42, term_type: 'rent', billing_cycle: 'monthly', currency_type_id: 1, amount: 5000, effective_from: '2026-01-01', effective_to: '2026-12-31', created_at: '2026-01-01T09:00:00Z', updated_at: '2026-01-01T09:00:00Z' }],
        },
      }),
    })
  })

  await page.route('**/api/overtime/bills**', async (route) => {
    const request = route.request()
    const url = new URL(request.url())
    const pathname = url.pathname

    if (request.method() === 'GET' && pathname.endsWith('/api/overtime/bills')) {
      await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ items: overtimeBills, total: overtimeBills.length, page: 1, page_size: 100 }) })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/api/overtime/bills')) {
      const payload = request.postDataJSON() as Record<string, unknown>
      overtimeBills.push({
        id: 702,
        lease_contract_id: 42,
        lease_no: 'L-042',
        tenant_name: 'Harbor Foods',
        period_start: payload.period_start,
        period_end: payload.period_end,
        status: 'draft',
        workflow_instance_id: null,
        note: payload.note,
        submitted_at: null,
        approved_at: null,
        rejected_at: null,
        cancelled_at: null,
        stopped_at: null,
        generated_at: null,
        created_by: 1,
        updated_by: 1,
        created_at: '2026-04-01T09:00:00Z',
        updated_at: '2026-04-01T09:00:00Z',
        formulas: payload.formulas,
        generated_charges: [],
      })
      await route.fulfill({ status: 201, contentType: 'application/json', body: JSON.stringify({ bill: overtimeBills[overtimeBills.length - 1] }) })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/api/overtime/bills/702/submit')) {
      const bill = overtimeBills.find((item) => item.id === 702)
      if (bill) bill.status = 'pending_approval'
      await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ bill }) })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/api/overtime/bills/701/generate')) {
      const bill = overtimeBills.find((item) => item.id === 701)
      if (bill) bill.status = 'generated'
      if (bill) bill.generated_at = '2026-05-04T09:00:00Z'
      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ run: { id: 88 }, charges: [], skipped: [], totals: { generated: 1, skipped: 0 } }),
      })
      return
    }

    await route.fallback()
  })

  await login(page)
  await page.goto('/lease/contracts/42')

  await page.getByRole('combobox', { name: /期间开始/ }).fill('2026-04-01')
  await page.getByRole('combobox', { name: /期间结束/ }).fill('2026-04-30')
  await page.getByRole('textbox', { name: /操作备注/ }).fill('April overtime')
  await page.getByRole('textbox', { name: /费用类型/ }).fill('overtime_rent')
  await page.getByTestId('lease-overtime-create-button').click()

  await expect(page.getByTestId('lease-detail-success-alert')).toContainText('加班账单草稿已创建')
  await page.getByTestId('overtime-row-submit-button-702').click()
  await expect(page.getByTestId('lease-detail-success-alert')).toContainText('加班账单已提交审批')
  await page.getByTestId('overtime-row-generate-button-701').click()
  await expect(page.getByTestId('lease-detail-success-alert')).toContainText('加班费用已生成')
})
