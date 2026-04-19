import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
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

type BuildingFixture = {
  id: number
  store_id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

type FloorFixture = {
  id: number
  building_id: number
  code: string
  name: string
  status: string
  floor_plan_image_url: string | null
  created_at: string
  updated_at: string
}

type AreaFixture = {
  id: number
  store_id: number
  area_level_id: number
  code: string
  name: string
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
  shop_type_id: number | null
  code: string
  floor_area: number
  use_area: number
  rent_area: number
  is_rentable: boolean
  status: string
  created_at: string
  updated_at: string
}

type UnitTypeFixture = {
  id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

type ShopTypeFixture = {
  id: number
  code: string
  name: string
  status: string
  color_hex: string
  created_at: string
  updated_at: string
}

const now = '2026-04-03T08:00:00Z'

const selectByTestId = async (page: Page, testId: string, label: string) => {
  await page.getByTestId(testId).click()
  await page.locator('.el-select-dropdown__item').filter({ hasText: label }).first().click()
}

const attachRentableAreaMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'structure.admin',
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
  ]

  const buildings: BuildingFixture[] = [
    {
      id: 201,
      store_id: 101,
      code: 'BLD-1',
      name: 'East Wing',
      status: 'active',
      created_at: now,
      updated_at: now,
    },
  ]

  const floors: FloorFixture[] = [
    {
      id: 301,
      building_id: 201,
      code: 'F-1',
      name: 'Floor 1',
      status: 'active',
      floor_plan_image_url: null,
      created_at: now,
      updated_at: now,
    },
  ]

  const areas: AreaFixture[] = [
    {
      id: 401,
      store_id: 101,
      area_level_id: 1,
      code: 'AR-1',
      name: 'North Arcade',
      status: 'active',
      created_at: now,
      updated_at: now,
    },
  ]

  const unitTypes: UnitTypeFixture[] = [
    {
      id: 501,
      code: 'shop',
      name: 'Shop unit',
      status: 'active',
      created_at: now,
      updated_at: now,
    },
  ]

  const shopTypes: ShopTypeFixture[] = [
    {
      id: 601,
      code: 'fashion',
      name: 'Fashion',
      status: 'active',
      color_hex: '#4CAF50',
      created_at: now,
      updated_at: now,
    },
  ]

  let units: UnitFixture[] = [
    {
      id: 601,
      building_id: 201,
      floor_id: 301,
        location_id: 701,
        area_id: 401,
        unit_type_id: 501,
        shop_type_id: null,
        code: 'RA-201',
      floor_area: 155.5,
      use_area: 140.25,
      rent_area: 132.75,
      is_rentable: true,
      status: 'active',
      created_at: now,
      updated_at: now,
    },
  ]

  let updatePayload: Record<string, unknown> | null = null

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'rentable-admin',
          display_name: 'Rentable Admin',
          department_id: 12,
          roles: ['admin'],
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

  await page.route('**/api/base-info/unit-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ unit_types: unitTypes }),
    })
  })

  await page.route('**/api/base-info/shop-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ shop_types: shopTypes }),
    })
  })

  await page.route('**/api/structure/stores**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ stores }),
    })
  })

  await page.route('**/api/structure/buildings**', async (route) => {
    const url = new URL(route.request().url())
    const storeIdParam = url.searchParams.get('store_id')
    const storeId = storeIdParam ? Number(storeIdParam) : null
    const filtered = storeId === null ? buildings : buildings.filter((item) => item.store_id === storeId)

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ buildings: filtered }),
    })
  })

  await page.route('**/api/structure/floors**', async (route) => {
    const url = new URL(route.request().url())
    const buildingIdParam = url.searchParams.get('building_id')
    const buildingId = buildingIdParam ? Number(buildingIdParam) : null
    const filtered = buildingId === null ? floors : floors.filter((item) => item.building_id === buildingId)

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ floors: filtered }),
    })
  })

  await page.route('**/api/structure/areas**', async (route) => {
    const url = new URL(route.request().url())
    const storeIdParam = url.searchParams.get('store_id')
    const storeId = storeIdParam ? Number(storeIdParam) : null
    const filtered = storeId === null ? areas : areas.filter((item) => item.store_id === storeId)

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ areas: filtered }),
    })
  })

  await page.route('**/api/structure/units**', async (route) => {
    const request = route.request()

    if (request.method() === 'PUT') {
      const payload = request.postDataJSON() as Record<string, unknown>
      updatePayload = payload

      const url = new URL(request.url())
      const id = Number(url.pathname.split('/').at(-1))
      const updated: UnitFixture = {
        id,
        building_id: Number(payload.building_id),
        floor_id: Number(payload.floor_id),
        location_id: Number(payload.location_id),
        area_id: Number(payload.area_id),
        unit_type_id: Number(payload.unit_type_id),
        shop_type_id: payload.shop_type_id === null ? null : Number(payload.shop_type_id),
        code: String(payload.code),
        floor_area: Number(payload.floor_area),
        use_area: Number(payload.use_area),
        rent_area: Number(payload.rent_area),
        is_rentable: payload.is_rentable === true,
        status: String(payload.status),
        created_at: now,
        updated_at: now,
      }
      units = units.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ unit: updated }),
      })
      return
    }

    const url = new URL(request.url())
    const buildingIdParam = url.searchParams.get('building_id')
    const floorIdParam = url.searchParams.get('floor_id')
    const areaIdParam = url.searchParams.get('area_id')
    const buildingId = buildingIdParam ? Number(buildingIdParam) : null
    const floorId = floorIdParam ? Number(floorIdParam) : null
    const areaId = areaIdParam ? Number(areaIdParam) : null
    const filtered = units.filter(
      (item) =>
        (buildingId === null || item.building_id === buildingId) &&
        (floorId === null || item.floor_id === floorId) &&
        (areaId === null || item.area_id === areaId),
    )

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ units: filtered }),
    })
  })

  return {
    getUpdatePayload: () => updatePayload,
  }
}

test('filters and updates rentable units from the dedicated admin view', async ({ page }) => {
  const mocks = await attachRentableAreaMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('rentable-admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/dashboard/)
  await expect(page.getByTestId('nav--admin-rentable-areas')).toBeVisible()

  await page.getByTestId('nav--admin-rentable-areas').click()
  await expect(page).toHaveURL(/\/admin\/rentable-areas/)
  await expect(page.getByTestId('rentable-area-admin-view')).toBeVisible()

  await selectByTestId(page, 'rentable-area-store-filter', 'MI-101 — Harbor Center')
  await selectByTestId(page, 'rentable-area-building-filter', 'BLD-1 — East Wing')
  await selectByTestId(page, 'rentable-area-floor-filter', 'F-1 — Floor 1')
  await selectByTestId(page, 'rentable-area-area-filter', 'AR-1 — North Arcade')
  await page.getByTestId('rentable-area-rentable-switch').click()
  await page.getByTestId('rentable-area-status-filter').click()
  await page.getByTestId('rentable-area-status-option-active').click()
  await page.getByTestId('rentable-area-apply-button').click()

  await expect(page.getByTestId('rentable-area-units-table')).toContainText('RA-201')
  await page.getByTestId('rentable-area-edit-button-601').click()

  await expect(page.getByTestId('rentable-area-edit-dialog')).toBeVisible()
  await page.getByTestId('rentable-area-unit-edit-rent-area').locator('input').fill('140.5')
  await selectByTestId(page, 'rentable-area-edit-shop-type-select', 'fashion — Fashion')
  await page.getByTestId('rentable-area-unit-edit-status').click()
  await page.getByTestId('rentable-area-edit-status-option-inactive').click()
  await page.getByTestId('rentable-area-unit-edit-save').click()

  await expect.poll(mocks.getUpdatePayload).not.toBeNull()
  await expect.poll(() => mocks.getUpdatePayload()?.rent_area).toBe(140.5)
  expect(mocks.getUpdatePayload()).toEqual({
    building_id: 201,
    floor_id: 301,
    location_id: 701,
    area_id: 401,
    unit_type_id: 501,
    shop_type_id: 601,
    code: 'RA-201',
    floor_area: 155.5,
    use_area: 140.25,
    rent_area: 140.5,
    is_rentable: true,
    status: 'inactive',
  })
  await expect(page.getByTestId('rentable-area-feedback-alert')).toContainText('RA-201')
})
