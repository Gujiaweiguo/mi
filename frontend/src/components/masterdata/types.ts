import type { Brand, Customer, StoreRentBudget, UnitProspect, UnitRentBudget } from '../../api/masterdata'

export type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

export type CustomerBrandSnapshot = {
  customers: Customer[]
  customerTotal: number
  brands: Brand[]
  brandTotal: number
}

export type BudgetProspectSnapshot = {
  unitRentBudgets: UnitRentBudget[]
  storeRentBudgets: StoreRentBudget[]
  unitProspects: UnitProspect[]
}
