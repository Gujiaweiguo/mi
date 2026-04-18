import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  createBrand,
  createCustomer,
  createStoreRentBudget,
  createUnitProspect,
  createUnitRentBudget,
  listBrands,
  listCustomers,
  listStoreRentBudgets,
  listUnitProspects,
  listUnitRentBudgets,
  updateBrand,
  updateCustomer,
  updateStoreRentBudget,
  updateUnitProspect,
  updateUnitRentBudget,
} from './masterdata'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('masterdata api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('customers', () => {
    it('listCustomers calls GET and returns the response', async () => {
      const params = { query: 'Acme', page: 1, page_size: 20 }
      const response = { data: { customers: [{ id: 1, name: 'Acme' }], total: 1, page: 1, page_size: 20 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listCustomers(params)

      expect(http.get).toHaveBeenCalledWith('/master-data/customers', { params })
      expect(result).toEqual(response)
    })

    it('listCustomers propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listCustomers({ query: 'Acme' })).rejects.toThrow('fail')
    })

    it('createCustomer calls POST and returns the response', async () => {
      const payload = {
        code: 'C001',
        name: 'Acme',
        trade_id: 10,
        department_id: 20,
        status: 'active',
      }
      const response = { data: { customer: { id: 1, code: 'C001' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createCustomer(payload)

      expect(http.post).toHaveBeenCalledWith('/master-data/customers', payload)
      expect(result).toEqual(response)
    })

    it('createCustomer propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createCustomer({ code: 'C001', name: 'Acme' })).rejects.toThrow('fail')
    })

    it('updateCustomer calls PUT with id in the URL and returns the response', async () => {
      const payload = {
        id: 1,
        code: 'C001',
        name: 'Acme Updated',
        trade_id: 10,
        department_id: 20,
        status: 'inactive',
      }
      const response = { data: { customer: { id: 1, code: 'C001' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateCustomer(payload)

      expect(http.put).toHaveBeenCalledWith('/master-data/customers/1', payload)
      expect(result).toEqual(response)
    })

    it('updateCustomer propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateCustomer({ id: 1, code: 'C001', name: 'Acme' })).rejects.toThrow('fail')
    })
  })

  describe('brands', () => {
    it('listBrands calls GET and returns the response', async () => {
      const params = { query: 'Brand', page: 1, page_size: 20 }
      const response = { data: { brands: [{ id: 1, name: 'Brand' }], total: 1, page: 1, page_size: 20 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listBrands(params)

      expect(http.get).toHaveBeenCalledWith('/master-data/brands', { params })
      expect(result).toEqual(response)
    })

    it('listBrands propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listBrands({ query: 'Brand' })).rejects.toThrow('fail')
    })

    it('createBrand calls POST and returns the response', async () => {
      const payload = { code: 'B001', name: 'Brand', status: 'active' }
      const response = { data: { brand: { id: 1, code: 'B001' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createBrand(payload)

      expect(http.post).toHaveBeenCalledWith('/master-data/brands', payload)
      expect(result).toEqual(response)
    })

    it('createBrand propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createBrand({ code: 'B001', name: 'Brand' })).rejects.toThrow('fail')
    })

    it('updateBrand calls PUT with id in the URL and returns the response', async () => {
      const payload = { id: 1, code: 'B001', name: 'Brand Updated', status: 'inactive' }
      const response = { data: { brand: { id: 1, code: 'B001' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateBrand(payload)

      expect(http.put).toHaveBeenCalledWith('/master-data/brands/1', payload)
      expect(result).toEqual(response)
    })

    it('updateBrand propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateBrand({ id: 1, code: 'B001', name: 'Brand' })).rejects.toThrow('fail')
    })
  })

  describe('unit rent budgets', () => {
    it('listUnitRentBudgets calls GET and returns the response', async () => {
      const response = { data: { unit_rent_budgets: [{ unit_id: 10, fiscal_year: 2026 }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listUnitRentBudgets()

      expect(http.get).toHaveBeenCalledWith('/master-data/unit-rent-budgets')
      expect(result).toEqual(response)
    })

    it('listUnitRentBudgets propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listUnitRentBudgets()).rejects.toThrow('fail')
    })

    it('createUnitRentBudget calls POST and returns the response', async () => {
      const payload = { unit_id: 10, fiscal_year: 2026, budget_price: 1000 }
      const response = { data: { unit_rent_budget: { unit_id: 10, fiscal_year: 2026 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createUnitRentBudget(payload)

      expect(http.post).toHaveBeenCalledWith('/master-data/unit-rent-budgets', payload)
      expect(result).toEqual(response)
    })

    it('createUnitRentBudget propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createUnitRentBudget({ unit_id: 10, fiscal_year: 2026, budget_price: 1000 })).rejects.toThrow(
        'fail',
      )
    })

    it('updateUnitRentBudget calls PUT with composite key in the URL and returns the response', async () => {
      const payload = { unit_id: 10, fiscal_year: 2026, budget_price: 1200 }
      const response = { data: { unit_rent_budget: { unit_id: 10, fiscal_year: 2026 } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateUnitRentBudget(payload)

      expect(http.put).toHaveBeenCalledWith('/master-data/unit-rent-budgets/10/2026', payload)
      expect(result).toEqual(response)
    })

    it('updateUnitRentBudget propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateUnitRentBudget({ unit_id: 10, fiscal_year: 2026, budget_price: 1000 })).rejects.toThrow(
        'fail',
      )
    })
  })

  describe('store rent budgets', () => {
    it('listStoreRentBudgets calls GET and returns the response', async () => {
      const response = { data: { store_rent_budgets: [{ store_id: 3, fiscal_year: 2026, fiscal_month: 4 }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listStoreRentBudgets()

      expect(http.get).toHaveBeenCalledWith('/master-data/store-rent-budgets')
      expect(result).toEqual(response)
    })

    it('listStoreRentBudgets propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listStoreRentBudgets()).rejects.toThrow('fail')
    })

    it('createStoreRentBudget calls POST and returns the response', async () => {
      const payload = { store_id: 3, fiscal_year: 2026, fiscal_month: 4, monthly_budget: 5000 }
      const response = { data: { store_rent_budget: { store_id: 3, fiscal_year: 2026, fiscal_month: 4 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createStoreRentBudget(payload)

      expect(http.post).toHaveBeenCalledWith('/master-data/store-rent-budgets', payload)
      expect(result).toEqual(response)
    })

    it('createStoreRentBudget propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createStoreRentBudget({ store_id: 3, fiscal_year: 2026, fiscal_month: 4, monthly_budget: 5000 }),
      ).rejects.toThrow('fail')
    })

    it('updateStoreRentBudget calls PUT with composite key in the URL and returns the response', async () => {
      const payload = { store_id: 3, fiscal_year: 2026, fiscal_month: 4, monthly_budget: 5500 }
      const response = { data: { store_rent_budget: { store_id: 3, fiscal_year: 2026, fiscal_month: 4 } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateStoreRentBudget(payload)

      expect(http.put).toHaveBeenCalledWith('/master-data/store-rent-budgets/3/2026/4', payload)
      expect(result).toEqual(response)
    })

    it('updateStoreRentBudget propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(
        updateStoreRentBudget({ store_id: 3, fiscal_year: 2026, fiscal_month: 4, monthly_budget: 5000 }),
      ).rejects.toThrow('fail')
    })
  })

  describe('unit prospects', () => {
    it('listUnitProspects calls GET and returns the response', async () => {
      const response = { data: { unit_prospects: [{ unit_id: 10, fiscal_year: 2026 }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listUnitProspects()

      expect(http.get).toHaveBeenCalledWith('/master-data/unit-prospects')
      expect(result).toEqual(response)
    })

    it('listUnitProspects propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listUnitProspects()).rejects.toThrow('fail')
    })

    it('createUnitProspect calls POST and returns the response', async () => {
      const payload = {
        unit_id: 10,
        fiscal_year: 2026,
        potential_customer_id: 1,
        prospect_brand_id: 2,
        prospect_trade_id: 3,
        avg_transaction: 100,
        prospect_rent_price: 1200,
        rent_increment: '5%',
        prospect_term_months: 12,
      }
      const response = { data: { unit_prospect: { unit_id: 10, fiscal_year: 2026 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createUnitProspect(payload)

      expect(http.post).toHaveBeenCalledWith('/master-data/unit-prospects', payload)
      expect(result).toEqual(response)
    })

    it('createUnitProspect propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createUnitProspect({ unit_id: 10, fiscal_year: 2026 })).rejects.toThrow('fail')
    })

    it('updateUnitProspect calls PUT with composite key in the URL and returns the response', async () => {
      const payload = {
        unit_id: 10,
        fiscal_year: 2026,
        potential_customer_id: 1,
        prospect_brand_id: 2,
        prospect_trade_id: 3,
        avg_transaction: 100,
        prospect_rent_price: 1200,
        rent_increment: '5%',
        prospect_term_months: 12,
      }
      const response = { data: { unit_prospect: { unit_id: 10, fiscal_year: 2026 } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateUnitProspect(payload)

      expect(http.put).toHaveBeenCalledWith('/master-data/unit-prospects/10/2026', payload)
      expect(result).toEqual(response)
    })

    it('updateUnitProspect propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateUnitProspect({ unit_id: 10, fiscal_year: 2026 })).rejects.toThrow('fail')
    })
  })
})
