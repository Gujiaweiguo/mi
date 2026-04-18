import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  createAreaLevel,
  createCurrencyType,
  createShopType,
  createStoreManagementType,
  createStoreType,
  createTradeDefinition,
  createUnitType,
  listAreaLevels,
  listCurrencyTypes,
  listShopTypes,
  listStoreManagementTypes,
  listStoreTypes,
  listTradeDefinitions,
  listUnitTypes,
  updateAreaLevel,
  updateCurrencyType,
  updateShopType,
  updateStoreManagementType,
  updateStoreType,
  updateTradeDefinition,
  updateUnitType,
} from './baseinfo'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('baseinfo api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('store types', () => {
    it('listStoreTypes calls GET and returns data', async () => {
      const response = { data: { store_types: [{ id: 1, code: 'MALL' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStoreTypes()

      expect(http.get).toHaveBeenCalledWith('/base-info/store-types')
      expect(result).toEqual(response)
    })

    it('listStoreTypes propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStoreTypes()).rejects.toThrow('fail')
    })

    it('createStoreType calls POST and returns data', async () => {
      const payload = { code: 'MALL', name: 'Mall' }
      const response = { data: { store_type: { id: 1, code: 'MALL' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStoreType(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/store-types', payload)
      expect(result).toEqual(response)
    })

    it('createStoreType propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createStoreType({ code: 'MALL', name: 'Mall' })).rejects.toThrow('fail')
    })

    it('updateStoreType calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'MALL', name: 'Mall Updated' }
      const response = { data: { store_type: { id: 1, code: 'MALL' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStoreType(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/store-types/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStoreType propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateStoreType(1, { code: 'MALL', name: 'Mall' })).rejects.toThrow('fail')
    })
  })

  describe('store management types', () => {
    it('listStoreManagementTypes calls GET and returns data', async () => {
      const response = { data: { store_management_types: [{ id: 1, code: 'SELF' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStoreManagementTypes()

      expect(http.get).toHaveBeenCalledWith('/base-info/store-management-types')
      expect(result).toEqual(response)
    })

    it('listStoreManagementTypes propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStoreManagementTypes()).rejects.toThrow('fail')
    })

    it('createStoreManagementType calls POST and returns data', async () => {
      const payload = { code: 'SELF', name: 'Self Operated', status: 'active' }
      const response = { data: { store_management_type: { id: 1, code: 'SELF' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStoreManagementType(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/store-management-types', payload)
      expect(result).toEqual(response)
    })

    it('createStoreManagementType propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createStoreManagementType({ code: 'SELF', name: 'Self Operated' })).rejects.toThrow('fail')
    })

    it('updateStoreManagementType calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'SELF', name: 'Self Operated Updated', status: 'inactive' }
      const response = { data: { store_management_type: { id: 1, code: 'SELF' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStoreManagementType(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/store-management-types/1', payload)
      expect(result).toEqual(response)
    })

    it('updateStoreManagementType propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateStoreManagementType(1, { code: 'SELF', name: 'Self Operated' })).rejects.toThrow('fail')
    })
  })

  describe('area levels', () => {
    it('listAreaLevels calls GET and returns data', async () => {
      const response = { data: { area_levels: [{ id: 1, code: 'L1' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listAreaLevels()

      expect(http.get).toHaveBeenCalledWith('/base-info/area-levels')
      expect(result).toEqual(response)
    })

    it('listAreaLevels propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listAreaLevels()).rejects.toThrow('fail')
    })

    it('createAreaLevel calls POST and returns data', async () => {
      const payload = { code: 'L1', name: 'Level 1' }
      const response = { data: { area_level: { id: 1, code: 'L1' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createAreaLevel(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/area-levels', payload)
      expect(result).toEqual(response)
    })

    it('createAreaLevel propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createAreaLevel({ code: 'L1', name: 'Level 1' })).rejects.toThrow('fail')
    })

    it('updateAreaLevel calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'L1', name: 'Level 1 Updated' }
      const response = { data: { area_level: { id: 1, code: 'L1' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateAreaLevel(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/area-levels/1', payload)
      expect(result).toEqual(response)
    })

    it('updateAreaLevel propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateAreaLevel(1, { code: 'L1', name: 'Level 1' })).rejects.toThrow('fail')
    })
  })

  describe('unit types', () => {
    it('listUnitTypes calls GET and returns data', async () => {
      const response = { data: { unit_types: [{ id: 1, code: 'SHOP' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listUnitTypes()

      expect(http.get).toHaveBeenCalledWith('/base-info/unit-types')
      expect(result).toEqual(response)
    })

    it('listUnitTypes propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listUnitTypes()).rejects.toThrow('fail')
    })

    it('createUnitType calls POST and returns data', async () => {
      const payload = { code: 'SHOP', name: 'Shop', status: 'active' }
      const response = { data: { unit_type: { id: 1, code: 'SHOP' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createUnitType(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/unit-types', payload)
      expect(result).toEqual(response)
    })

    it('createUnitType propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createUnitType({ code: 'SHOP', name: 'Shop' })).rejects.toThrow('fail')
    })

    it('updateUnitType calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'SHOP', name: 'Shop Updated', status: 'inactive' }
      const response = { data: { unit_type: { id: 1, code: 'SHOP' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateUnitType(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/unit-types/1', payload)
      expect(result).toEqual(response)
    })

    it('updateUnitType propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateUnitType(1, { code: 'SHOP', name: 'Shop' })).rejects.toThrow('fail')
    })
  })

  describe('shop types', () => {
    it('listShopTypes calls GET and returns data', async () => {
      const response = { data: { shop_types: [{ id: 1, code: 'FNB' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listShopTypes()

      expect(http.get).toHaveBeenCalledWith('/base-info/shop-types')
      expect(result).toEqual(response)
    })

    it('listShopTypes propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listShopTypes()).rejects.toThrow('fail')
    })

    it('createShopType calls POST and returns data', async () => {
      const payload = { code: 'FNB', name: 'Food & Beverage', color_hex: '#ffffff', status: 'active' }
      const response = { data: { shop_type: { id: 1, code: 'FNB' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createShopType(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/shop-types', payload)
      expect(result).toEqual(response)
    })

    it('createShopType propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createShopType({ code: 'FNB', name: 'Food & Beverage' })).rejects.toThrow('fail')
    })

    it('updateShopType calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'FNB', name: 'Food Updated', color_hex: '#000000', status: 'inactive' }
      const response = { data: { shop_type: { id: 1, code: 'FNB' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateShopType(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/shop-types/1', payload)
      expect(result).toEqual(response)
    })

    it('updateShopType propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateShopType(1, { code: 'FNB', name: 'Food & Beverage' })).rejects.toThrow('fail')
    })
  })

  describe('currency types', () => {
    it('listCurrencyTypes calls GET and returns data', async () => {
      const response = { data: { currency_types: [{ id: 1, code: 'CNY' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listCurrencyTypes()

      expect(http.get).toHaveBeenCalledWith('/base-info/currency-types')
      expect(result).toEqual(response)
    })

    it('listCurrencyTypes propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listCurrencyTypes()).rejects.toThrow('fail')
    })

    it('createCurrencyType calls POST and returns data', async () => {
      const payload = { code: 'CNY', name: 'Chinese Yuan', is_local: true, status: 'active' }
      const response = { data: { currency_type: { id: 1, code: 'CNY' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createCurrencyType(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/currency-types', payload)
      expect(result).toEqual(response)
    })

    it('createCurrencyType propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createCurrencyType({ code: 'CNY', name: 'Chinese Yuan' })).rejects.toThrow('fail')
    })

    it('updateCurrencyType calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'CNY', name: 'Chinese Yuan Updated', is_local: false, status: 'inactive' }
      const response = { data: { currency_type: { id: 1, code: 'CNY' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateCurrencyType(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/currency-types/1', payload)
      expect(result).toEqual(response)
    })

    it('updateCurrencyType propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateCurrencyType(1, { code: 'CNY', name: 'Chinese Yuan' })).rejects.toThrow('fail')
    })
  })

  describe('trade definitions', () => {
    it('listTradeDefinitions calls GET and returns data', async () => {
      const response = { data: { trade_definitions: [{ id: 1, code: 'FASHION' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listTradeDefinitions()

      expect(http.get).toHaveBeenCalledWith('/base-info/trade-definitions')
      expect(result).toEqual(response)
    })

    it('listTradeDefinitions propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listTradeDefinitions()).rejects.toThrow('fail')
    })

    it('createTradeDefinition calls POST and returns data', async () => {
      const payload = { code: 'FASHION', name: 'Fashion', parent_id: null, level: 1, status: 'active' }
      const response = { data: { trade_definition: { id: 1, code: 'FASHION' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createTradeDefinition(payload)

      expect(http.post).toHaveBeenCalledWith('/base-info/trade-definitions', payload)
      expect(result).toEqual(response)
    })

    it('createTradeDefinition propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createTradeDefinition({ code: 'FASHION', name: 'Fashion' })).rejects.toThrow('fail')
    })

    it('updateTradeDefinition calls PUT with id in URL and returns data', async () => {
      const payload = { code: 'FASHION', name: 'Fashion Updated', parent_id: 2, level: 2, status: 'inactive' }
      const response = { data: { trade_definition: { id: 1, code: 'FASHION' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateTradeDefinition(1, payload)

      expect(http.put).toHaveBeenCalledWith('/base-info/trade-definitions/1', payload)
      expect(result).toEqual(response)
    })

    it('updateTradeDefinition propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateTradeDefinition(1, { code: 'FASHION', name: 'Fashion' })).rejects.toThrow('fail')
    })
  })
})
