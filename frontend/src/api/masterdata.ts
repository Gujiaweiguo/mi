import { http } from './http'

export interface Customer {
  id: number
  code: string
  name: string
  trade_id: number | null
  department_id: number | null
  status: string
  created_at: string
  updated_at: string
}

export interface PaginatedCustomersResponse {
  customers: Customer[]
  total: number
  page: number
  page_size: number
}

export interface Brand {
  id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

export interface PaginatedBrandsResponse {
  brands: Brand[]
  total: number
  page: number
  page_size: number
}

export interface UnitRentBudget {
  unit_id: number
  fiscal_year: number
  budget_price: number
  created_at: string
  updated_at: string
}

export interface StoreRentBudget {
  store_id: number
  fiscal_year: number
  fiscal_month: number
  monthly_budget: number
  created_at: string
  updated_at: string
}

export interface UnitProspect {
  unit_id: number
  fiscal_year: number
  potential_customer_id: number | null
  prospect_brand_id: number | null
  prospect_trade_id: number | null
  avg_transaction: number | null
  prospect_rent_price: number | null
  rent_increment: string | null
  prospect_term_months: number | null
  created_at: string
  updated_at: string
}

export interface CreateCustomerRequest {
  code: string
  name: string
  trade_id?: number | null
  department_id?: number | null
  status?: string
}

export interface CreateBrandRequest {
  code: string
  name: string
  status?: string
}

export interface UpdateCustomerRequest extends CreateCustomerRequest {
  id: number
}

export interface UpdateBrandRequest extends CreateBrandRequest {
  id: number
}

export interface UpsertUnitRentBudgetRequest {
  unit_id: number
  fiscal_year: number
  budget_price: number
}

export interface UpsertStoreRentBudgetRequest {
  store_id: number
  fiscal_year: number
  fiscal_month: number
  monthly_budget: number
}

export interface UpsertUnitProspectRequest {
  unit_id: number
  fiscal_year: number
  potential_customer_id?: number | null
  prospect_brand_id?: number | null
  prospect_trade_id?: number | null
  avg_transaction?: number | null
  prospect_rent_price?: number | null
  rent_increment?: string | null
  prospect_term_months?: number | null
}

export const listCustomers = (params?: { query?: string; page?: number; page_size?: number }) =>
  http.get<PaginatedCustomersResponse>('/master-data/customers', { params })
export const createCustomer = (data: CreateCustomerRequest) =>
  http.post<{ customer: Customer }>('/master-data/customers', data)
export const updateCustomer = (data: UpdateCustomerRequest) =>
  http.put<{ customer: Customer }>(`/master-data/customers/${data.id}`, data)
export const listBrands = (params?: { query?: string; page?: number; page_size?: number }) =>
  http.get<PaginatedBrandsResponse>('/master-data/brands', { params })
export const createBrand = (data: CreateBrandRequest) =>
  http.post<{ brand: Brand }>('/master-data/brands', data)
export const updateBrand = (data: UpdateBrandRequest) =>
  http.put<{ brand: Brand }>(`/master-data/brands/${data.id}`, data)

export const listUnitRentBudgets = () =>
  http.get<{ unit_rent_budgets: UnitRentBudget[] }>('/master-data/unit-rent-budgets')
export const createUnitRentBudget = (data: UpsertUnitRentBudgetRequest) =>
  http.post<{ unit_rent_budget: UnitRentBudget }>('/master-data/unit-rent-budgets', data)
export const updateUnitRentBudget = (data: UpsertUnitRentBudgetRequest) =>
  http.put<{ unit_rent_budget: UnitRentBudget }>(`/master-data/unit-rent-budgets/${data.unit_id}/${data.fiscal_year}`, data)

export const listStoreRentBudgets = () =>
  http.get<{ store_rent_budgets: StoreRentBudget[] }>('/master-data/store-rent-budgets')
export const createStoreRentBudget = (data: UpsertStoreRentBudgetRequest) =>
  http.post<{ store_rent_budget: StoreRentBudget }>('/master-data/store-rent-budgets', data)
export const updateStoreRentBudget = (data: UpsertStoreRentBudgetRequest) =>
  http.put<{ store_rent_budget: StoreRentBudget }>(`/master-data/store-rent-budgets/${data.store_id}/${data.fiscal_year}/${data.fiscal_month}`, data)

export const listUnitProspects = () =>
  http.get<{ unit_prospects: UnitProspect[] }>('/master-data/unit-prospects')
export const createUnitProspect = (data: UpsertUnitProspectRequest) =>
  http.post<{ unit_prospect: UnitProspect }>('/master-data/unit-prospects', data)
export const updateUnitProspect = (data: UpsertUnitProspectRequest) =>
  http.put<{ unit_prospect: UnitProspect }>(`/master-data/unit-prospects/${data.unit_id}/${data.fiscal_year}`, data)
