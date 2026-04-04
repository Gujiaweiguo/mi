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

type LocationFixture = {
  id: number
  store_id: number
  floor_id: number
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
  code: string
  floor_area: number
  use_area: number
  rent_area: number
  is_rentable: boolean
  status: string
  created_at: string
  updated_at: string
}

const now = '2026-04-03T08:00:00Z'

const selectByTestId = async (page: Page, testId: string, label: string) => {
  await page.getByTestId(testId).click()
  await page.locator('.el-select-dropdown__item').filter({ hasText: label }).first().click()
}

const attachStructureMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'structure.admin',
      permission_level: 'approve',
      can_print: false,
      can_export: false,
    },
  ]

  let stores: StoreFixture[] = []
  let buildings: BuildingFixture[] = []
  let floors: FloorFixture[] = []
  let areas: AreaFixture[] = []
  let locations: LocationFixture[] = []
  let units: UnitFixture[] = []

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'structure-admin',
          display_name: 'Structure Admin',
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

  await page.route('**/api/base-info/store-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        store_types: [
          {
            id: 201,
            code: 'retail',
            name: 'Retail store',
            status: 'active',
            created_at: now,
            updated_at: now,
          },
        ],
      }),
    })
  })

  await page.route('**/api/base-info/store-management-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        store_management_types: [
          {
            id: 301,
            code: 'self',
            name: 'Self operated',
            status: 'active',
            created_at: now,
            updated_at: now,
          },
        ],
      }),
    })
  })

  await page.route('**/api/base-info/area-levels**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        area_levels: [
          {
            id: 401,
            code: 'mall-zone',
            name: 'Mall zone',
            status: 'active',
            created_at: now,
            updated_at: now,
          },
        ],
      }),
    })
  })

  await page.route('**/api/base-info/unit-types**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        unit_types: [
          {
            id: 501,
            code: 'shop',
            name: 'Shop unit',
            status: 'active',
            created_at: now,
            updated_at: now,
          },
        ],
      }),
    })
  })

  await page.route('**/api/structure/stores**', async (route) => {
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as {
        department_id: number
        store_type_id: number
        management_type_id: number
        code: string
        name: string
        short_name: string
        status?: string
      }

      const store: StoreFixture = {
        id: stores.length + 1001,
        department_id: payload.department_id,
        store_type_id: payload.store_type_id,
        management_type_id: payload.management_type_id,
        code: payload.code,
        name: payload.name,
        short_name: payload.short_name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      stores = [store, ...stores]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ store }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as {
        department_id: number
        store_type_id: number
        management_type_id: number
        code: string
        name: string
        short_name: string
        status?: string
      }

      const updated: StoreFixture = {
        id,
        department_id: payload.department_id,
        store_type_id: payload.store_type_id,
        management_type_id: payload.management_type_id,
        code: payload.code,
        name: payload.name,
        short_name: payload.short_name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      stores = stores.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ store: updated }),
      })
      return
    }

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ stores }),
    })
  })

  await page.route('**/api/structure/buildings**', async (route) => {
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as {
        store_id: number
        code: string
        name: string
        status?: string
      }

      const building: BuildingFixture = {
        id: buildings.length + 2001,
        store_id: payload.store_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      buildings = [building, ...buildings]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ building }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as {
        store_id: number
        code: string
        name: string
        status?: string
      }

      const updated: BuildingFixture = {
        id,
        store_id: payload.store_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      buildings = buildings.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ building: updated }),
      })
      return
    }

    const url = new URL(request.url())
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
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as {
        building_id: number
        code: string
        name: string
        status?: string
        floor_plan_image_url?: string | null
      }

      const floor: FloorFixture = {
        id: floors.length + 3001,
        building_id: payload.building_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        floor_plan_image_url: payload.floor_plan_image_url ?? null,
        created_at: now,
        updated_at: now,
      }
      floors = [floor, ...floors]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ floor }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as {
        building_id: number
        code: string
        name: string
        status?: string
        floor_plan_image_url?: string | null
      }

      const updated: FloorFixture = {
        id,
        building_id: payload.building_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        floor_plan_image_url: payload.floor_plan_image_url ?? null,
        created_at: now,
        updated_at: now,
      }
      floors = floors.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ floor: updated }),
      })
      return
    }

    const url = new URL(request.url())
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
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as {
        store_id: number
        area_level_id: number
        code: string
        name: string
        status?: string
      }

      const area: AreaFixture = {
        id: areas.length + 4001,
        store_id: payload.store_id,
        area_level_id: payload.area_level_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      areas = [area, ...areas]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ area }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as {
        store_id: number
        area_level_id: number
        code: string
        name: string
        status?: string
      }

      const updated: AreaFixture = {
        id,
        store_id: payload.store_id,
        area_level_id: payload.area_level_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      areas = areas.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ area: updated }),
      })
      return
    }

    const url = new URL(request.url())
    const storeIdParam = url.searchParams.get('store_id')
    const storeId = storeIdParam ? Number(storeIdParam) : null
    const filtered = storeId === null ? areas : areas.filter((item) => item.store_id === storeId)

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ areas: filtered }),
    })
  })

  await page.route('**/api/structure/locations**', async (route) => {
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as {
        store_id: number
        floor_id: number
        code: string
        name: string
        status?: string
      }

      const location: LocationFixture = {
        id: locations.length + 5001,
        store_id: payload.store_id,
        floor_id: payload.floor_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      locations = [location, ...locations]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ location }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as {
        store_id: number
        floor_id: number
        code: string
        name: string
        status?: string
      }

      const updated: LocationFixture = {
        id,
        store_id: payload.store_id,
        floor_id: payload.floor_id,
        code: payload.code,
        name: payload.name,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      locations = locations.map((item) => (item.id === id ? updated : item))

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ location: updated }),
      })
      return
    }

    const url = new URL(request.url())
    const storeIdParam = url.searchParams.get('store_id')
    const floorIdParam = url.searchParams.get('floor_id')
    const storeId = storeIdParam ? Number(storeIdParam) : null
    const floorId = floorIdParam ? Number(floorIdParam) : null
    const filtered = locations.filter(
      (item) => (storeId === null || item.store_id === storeId) && (floorId === null || item.floor_id === floorId),
    )

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ locations: filtered }),
    })
  })

  await page.route('**/api/structure/units**', async (route) => {
    const request = route.request()
    const method = request.method()

    if (method === 'POST') {
      const payload = request.postDataJSON() as {
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
        status?: string
      }

      const unit: UnitFixture = {
        id: units.length + 6001,
        building_id: payload.building_id,
        floor_id: payload.floor_id,
        location_id: payload.location_id,
        area_id: payload.area_id,
        unit_type_id: payload.unit_type_id,
        code: payload.code,
        floor_area: payload.floor_area,
        use_area: payload.use_area,
        rent_area: payload.rent_area,
        is_rentable: payload.is_rentable,
        status: payload.status ?? 'active',
        created_at: now,
        updated_at: now,
      }
      units = [unit, ...units]

      await route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ unit }),
      })
      return
    }

    if (method === 'PUT') {
      const url = new URL(request.url())
      const parts = url.pathname.split('/')
      const id = Number(parts[parts.length - 1])
      const payload = request.postDataJSON() as {
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
        status?: string
      }

      const updated: UnitFixture = {
        id,
        building_id: payload.building_id,
        floor_id: payload.floor_id,
        location_id: payload.location_id,
        area_id: payload.area_id,
        unit_type_id: payload.unit_type_id,
        code: payload.code,
        floor_area: payload.floor_area,
        use_area: payload.use_area,
        rent_area: payload.rent_area,
        is_rentable: payload.is_rentable,
        status: payload.status ?? 'active',
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
    const locationIdParam = url.searchParams.get('location_id')
    const areaIdParam = url.searchParams.get('area_id')
    const buildingId = buildingIdParam ? Number(buildingIdParam) : null
    const floorId = floorIdParam ? Number(floorIdParam) : null
    const locationId = locationIdParam ? Number(locationIdParam) : null
    const areaId = areaIdParam ? Number(areaIdParam) : null
    const filtered = units.filter(
      (item) =>
        (buildingId === null || item.building_id === buildingId) &&
        (floorId === null || item.floor_id === floorId) &&
        (locationId === null || item.location_id === locationId) &&
        (areaId === null || item.area_id === areaId),
    )

    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ units: filtered }),
    })
  })
}

test('creates store, building, floor, area, location, and unit records from structure admin view', async ({ page }) => {
  await attachStructureMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('structure-admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('nav--admin-structure')).toBeVisible()

  await page.getByTestId('nav--admin-structure').click()
  await expect(page).toHaveURL(/\/admin\/structure/)
  await expect(page.getByTestId('structure-admin-view')).toBeVisible()

  await page.getByTestId('structure-store-code-input').fill('MI-101')
  await page.getByTestId('structure-store-name-input').fill('Harbor Center')
  await page.getByTestId('structure-store-short-name-input').fill('Harbor')
  await page.getByTestId('structure-store-department-input').locator('input').fill('101')
  await page.getByTestId('structure-store-type-input').locator('input').fill('201')
  await page.getByTestId('structure-store-management-type-input').locator('input').fill('301')
  await page.getByTestId('structure-store-create-button').click()

  await expect(page.getByTestId('structure-stores-table')).toContainText('MI-101')
  await expect(page.getByTestId('structure-stores-table')).toContainText('Harbor Center')

  await page.getByTestId('structure-building-code-input').fill('BLD-101')
  await page.getByTestId('structure-building-name-input').fill('East Tower')
  await page.getByTestId('structure-building-create-button').click()

  await expect(page.getByTestId('structure-buildings-table')).toContainText('BLD-101')
  await expect(page.getByTestId('structure-buildings-table')).toContainText('East Tower')

  await page.getByTestId('structure-floor-code-input').fill('F-1')
  await page.getByTestId('structure-floor-name-input').fill('Floor 1')
  await page.getByTestId('structure-floor-create-button').click()

  await expect(page.getByTestId('structure-floors-table')).toContainText('F-1')
  await expect(page.getByTestId('structure-floors-table')).toContainText('Floor 1')

  await page.getByTestId('structure-area-code-input').fill('AR-101')
  await page.getByTestId('structure-area-name-input').fill('Main Atrium')
  await selectByTestId(page, 'structure-area-level-input', 'mall-zone — Mall zone')
  await page.getByTestId('structure-area-create-button').click()

  await expect(page.getByTestId('structure-areas-table')).toContainText('AR-101')
  await expect(page.getByTestId('structure-areas-table')).toContainText('Main Atrium')

  await selectByTestId(page, 'structure-location-floor-input', 'F-1 — Floor 1')
  await page.getByTestId('structure-location-code-input').fill('LOC-101')
  await page.getByTestId('structure-location-name-input').fill('East Corridor')
  await page.getByTestId('structure-location-create-button').click()

  await expect(page.getByTestId('structure-locations-table')).toContainText('LOC-101')
  await expect(page.getByTestId('structure-locations-table')).toContainText('East Corridor')

  await page.getByTestId('structure-unit-code-input').fill('U-101')
  await selectByTestId(page, 'structure-unit-type-input', 'shop — Shop unit')
  await selectByTestId(page, 'structure-unit-area-input', 'AR-101 — Main Atrium')
  await selectByTestId(page, 'structure-unit-location-input', 'LOC-101 — East Corridor')
  await page.getByTestId('structure-unit-floor-area-input').locator('input').fill('120.5')
  await page.getByTestId('structure-unit-use-area-input').locator('input').fill('100.25')
  await page.getByTestId('structure-unit-rent-area-input').locator('input').fill('98.75')
  await page.getByTestId('structure-unit-create-button').click()

  await expect(page.getByTestId('structure-units-table')).toContainText('U-101')
  await expect(page.getByTestId('structure-units-table')).toContainText('AR-101')
  await expect(page.getByTestId('structure-units-table')).toContainText('LOC-101')
})
