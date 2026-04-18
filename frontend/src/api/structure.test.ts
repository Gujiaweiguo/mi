import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  createStructureArea,
  createStructureBuilding,
  createStructureFloor,
  createStructureLocation,
  createStructureStore,
  createStructureUnit,
  listStructureAreas,
  listStructureBuildings,
  listStructureFloors,
  listStructureLocations,
  listStructureStores,
  listStructureUnits,
  updateStructureArea,
  updateStructureBuilding,
  updateStructureFloor,
  updateStructureLocation,
  updateStructureStore,
  updateStructureUnit,
} from './structure'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('structure api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('stores', () => {
    it('listStructureStores calls GET and returns the response', async () => {
      const response = { data: { stores: [{ id: 1, code: 'S01' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStructureStores()

      expect(http.get).toHaveBeenCalledWith('/structure/stores')
      expect(result).toEqual(response)
    })

    it('listStructureStores propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStructureStores()).rejects.toThrow('fail')
    })

    it('createStructureStore calls POST and returns the response', async () => {
      const payload = {
        department_id: 1,
        store_type_id: 2,
        management_type_id: 3,
        code: 'S01',
        name: 'North Plaza',
        short_name: 'North',
        status: 'active',
      }
      const response = { data: { store: { id: 1, code: 'S01' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStructureStore(payload)

      expect(http.post).toHaveBeenCalledWith('/structure/stores', payload)
      expect(result).toEqual(response)
    })

    it('createStructureStore propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createStructureStore({
          department_id: 1,
          store_type_id: 2,
          management_type_id: 3,
          code: 'S01',
          name: 'North Plaza',
          short_name: 'North',
        }),
      ).rejects.toThrow('fail')
    })

    it('updateStructureStore calls PUT with id in the URL and returns the response', async () => {
      const payload = {
        department_id: 1,
        store_type_id: 2,
        management_type_id: 3,
        code: 'S01',
        name: 'North Plaza Updated',
        short_name: 'North',
        status: 'inactive',
      }
      const response = { data: { store: { id: 1, code: 'S01' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStructureStore(1, payload)

      expect(http.put).toHaveBeenCalledWith('/structure/stores/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStructureStore propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(
        updateStructureStore(1, {
          department_id: 1,
          store_type_id: 2,
          management_type_id: 3,
          code: 'S01',
          name: 'North Plaza',
          short_name: 'North',
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('buildings', () => {
    it('listStructureBuildings calls GET with params and returns the response', async () => {
      const params = { store_id: 1 }
      const response = { data: { buildings: [{ id: 1, code: 'B01' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStructureBuildings(params)

      expect(http.get).toHaveBeenCalledWith('/structure/buildings', { params })
      expect(result).toEqual(response)
    })

    it('listStructureBuildings propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStructureBuildings({ store_id: 1 })).rejects.toThrow('fail')
    })

    it('createStructureBuilding calls POST and returns the response', async () => {
      const payload = { store_id: 1, code: 'B01', name: 'Tower A', status: 'active' }
      const response = { data: { building: { id: 1, code: 'B01' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStructureBuilding(payload)

      expect(http.post).toHaveBeenCalledWith('/structure/buildings', payload)
      expect(result).toEqual(response)
    })

    it('createStructureBuilding propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createStructureBuilding({ store_id: 1, code: 'B01', name: 'Tower A' })).rejects.toThrow('fail')
    })

    it('updateStructureBuilding calls PUT with id in the URL and returns the response', async () => {
      const payload = { store_id: 1, code: 'B01', name: 'Tower A Updated', status: 'inactive' }
      const response = { data: { building: { id: 1, code: 'B01' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStructureBuilding(1, payload)

      expect(http.put).toHaveBeenCalledWith('/structure/buildings/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStructureBuilding propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateStructureBuilding(1, { store_id: 1, code: 'B01', name: 'Tower A' })).rejects.toThrow('fail')
    })
  })

  describe('floors', () => {
    it('listStructureFloors calls GET with params and returns the response', async () => {
      const params = { building_id: 1 }
      const response = { data: { floors: [{ id: 1, code: 'F01' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStructureFloors(params)

      expect(http.get).toHaveBeenCalledWith('/structure/floors', { params })
      expect(result).toEqual(response)
    })

    it('listStructureFloors propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStructureFloors({ building_id: 1 })).rejects.toThrow('fail')
    })

    it('createStructureFloor calls POST and returns the response', async () => {
      const payload = {
        building_id: 1,
        code: 'F01',
        name: 'First Floor',
        status: 'active',
        floor_plan_image_url: 'https://example.com/floor.png',
      }
      const response = { data: { floor: { id: 1, code: 'F01' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStructureFloor(payload)

      expect(http.post).toHaveBeenCalledWith('/structure/floors', payload)
      expect(result).toEqual(response)
    })

    it('createStructureFloor propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createStructureFloor({ building_id: 1, code: 'F01', name: 'First Floor' })).rejects.toThrow('fail')
    })

    it('updateStructureFloor calls PUT with id in the URL and returns the response', async () => {
      const payload = {
        building_id: 1,
        code: 'F01',
        name: 'First Floor Updated',
        status: 'inactive',
        floor_plan_image_url: 'https://example.com/floor.png',
      }
      const response = { data: { floor: { id: 1, code: 'F01' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStructureFloor(1, payload)

      expect(http.put).toHaveBeenCalledWith('/structure/floors/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStructureFloor propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateStructureFloor(1, { building_id: 1, code: 'F01', name: 'First Floor' })).rejects.toThrow('fail')
    })
  })

  describe('areas', () => {
    it('listStructureAreas calls GET with params and returns the response', async () => {
      const params = { store_id: 1 }
      const response = { data: { areas: [{ id: 1, code: 'A01' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStructureAreas(params)

      expect(http.get).toHaveBeenCalledWith('/structure/areas', { params })
      expect(result).toEqual(response)
    })

    it('listStructureAreas propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStructureAreas({ store_id: 1 })).rejects.toThrow('fail')
    })

    it('createStructureArea calls POST and returns the response', async () => {
      const payload = { store_id: 1, area_level_id: 2, code: 'A01', name: 'Atrium', status: 'active' }
      const response = { data: { area: { id: 1, code: 'A01' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStructureArea(payload)

      expect(http.post).toHaveBeenCalledWith('/structure/areas', payload)
      expect(result).toEqual(response)
    })

    it('createStructureArea propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createStructureArea({ store_id: 1, area_level_id: 2, code: 'A01', name: 'Atrium' })).rejects.toThrow(
        'fail',
      )
    })

    it('updateStructureArea calls PUT with id in the URL and returns the response', async () => {
      const payload = { store_id: 1, area_level_id: 2, code: 'A01', name: 'Atrium Updated', status: 'inactive' }
      const response = { data: { area: { id: 1, code: 'A01' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStructureArea(1, payload)

      expect(http.put).toHaveBeenCalledWith('/structure/areas/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStructureArea propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateStructureArea(1, { store_id: 1, area_level_id: 2, code: 'A01', name: 'Atrium' })).rejects.toThrow(
        'fail',
      )
    })
  })

  describe('locations', () => {
    it('listStructureLocations calls GET with params and returns the response', async () => {
      const params = { store_id: 1, floor_id: 2 }
      const response = { data: { locations: [{ id: 1, code: 'L01' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStructureLocations(params)

      expect(http.get).toHaveBeenCalledWith('/structure/locations', { params })
      expect(result).toEqual(response)
    })

    it('listStructureLocations propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStructureLocations({ store_id: 1, floor_id: 2 })).rejects.toThrow('fail')
    })

    it('createStructureLocation calls POST and returns the response', async () => {
      const payload = { store_id: 1, floor_id: 2, code: 'L01', name: 'North Wing', status: 'active' }
      const response = { data: { location: { id: 1, code: 'L01' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStructureLocation(payload)

      expect(http.post).toHaveBeenCalledWith('/structure/locations', payload)
      expect(result).toEqual(response)
    })

    it('createStructureLocation propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createStructureLocation({ store_id: 1, floor_id: 2, code: 'L01', name: 'North Wing' })).rejects.toThrow(
        'fail',
      )
    })

    it('updateStructureLocation calls PUT with id in the URL and returns the response', async () => {
      const payload = { store_id: 1, floor_id: 2, code: 'L01', name: 'North Wing Updated', status: 'inactive' }
      const response = { data: { location: { id: 1, code: 'L01' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStructureLocation(1, payload)

      expect(http.put).toHaveBeenCalledWith('/structure/locations/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStructureLocation propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(
        updateStructureLocation(1, { store_id: 1, floor_id: 2, code: 'L01', name: 'North Wing' }),
      ).rejects.toThrow('fail')
    })
  })

  describe('units', () => {
    it('listStructureUnits calls GET with params and returns the response', async () => {
      const params = { building_id: 1, floor_id: 2, location_id: 3, area_id: 4 }
      const response = { data: { units: [{ id: 1, code: 'U01' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStructureUnits(params)

      expect(http.get).toHaveBeenCalledWith('/structure/units', { params })
      expect(result).toEqual(response)
    })

    it('listStructureUnits propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStructureUnits({ building_id: 1 })).rejects.toThrow('fail')
    })

    it('createStructureUnit calls POST and returns the response', async () => {
      const payload = {
        building_id: 1,
        floor_id: 2,
        location_id: 3,
        area_id: 4,
        unit_type_id: 5,
        shop_type_id: 6,
        code: 'U01',
        floor_area: 100,
        use_area: 90,
        rent_area: 80,
        is_rentable: true,
        status: 'active',
      }
      const response = { data: { unit: { id: 1, code: 'U01' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStructureUnit(payload)

      expect(http.post).toHaveBeenCalledWith('/structure/units', payload)
      expect(result).toEqual(response)
    })

    it('createStructureUnit propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createStructureUnit({
          building_id: 1,
          floor_id: 2,
          location_id: 3,
          area_id: 4,
          unit_type_id: 5,
          code: 'U01',
          floor_area: 100,
          use_area: 90,
          rent_area: 80,
          is_rentable: true,
        }),
      ).rejects.toThrow('fail')
    })

    it('updateStructureUnit calls PUT with id in the URL and returns the response', async () => {
      const payload = {
        building_id: 1,
        floor_id: 2,
        location_id: 3,
        area_id: 4,
        unit_type_id: 5,
        shop_type_id: 6,
        code: 'U01',
        floor_area: 100,
        use_area: 90,
        rent_area: 80,
        is_rentable: false,
        status: 'inactive',
      }
      const response = { data: { unit: { id: 1, code: 'U01' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStructureUnit(1, payload)

      expect(http.put).toHaveBeenCalledWith('/structure/units/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStructureUnit propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(
        updateStructureUnit(1, {
          building_id: 1,
          floor_id: 2,
          location_id: 3,
          area_id: 4,
          unit_type_id: 5,
          code: 'U01',
          floor_area: 100,
          use_area: 90,
          rent_area: 80,
          is_rentable: true,
        }),
      ).rejects.toThrow('fail')
    })
  })
})
