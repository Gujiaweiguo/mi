import { http } from './http'

export type BaseInfoStatus = 'active' | 'inactive' | (string & {})

export interface ReferenceCatalogItem {
  id: number
  code: string
  name: string
  status?: string
  color_hex?: string
  is_local?: boolean
  parent_id?: number
  level?: number
  created_at: string
  updated_at: string
}

export type StoreTypeUpsertPayload = {
  code: string
  name: string
}

export type StoreManagementTypeUpsertPayload = {
  code: string
  name: string
  status?: BaseInfoStatus
}

export type AreaLevelUpsertPayload = {
  code: string
  name: string
}

export type UnitTypeUpsertPayload = {
  code: string
  name: string
  status?: BaseInfoStatus
}

export type ShopTypeUpsertPayload = {
  code: string
  name: string
  color_hex?: string | null
  status?: BaseInfoStatus
}

export type CurrencyTypeUpsertPayload = {
  code: string
  name: string
  is_local?: boolean | null
  status?: BaseInfoStatus
}

export type TradeDefinitionUpsertPayload = {
  code: string
  name: string
  parent_id?: number | null
  level?: number | null
  status?: BaseInfoStatus
}

export const listStoreTypes = () => http.get<{ store_types: ReferenceCatalogItem[] }>('/base-info/store-types')
export const createStoreType = (payload: StoreTypeUpsertPayload) =>
  http.post<{ store_type: ReferenceCatalogItem }>('/base-info/store-types', payload)
export const updateStoreType = (id: number, payload: StoreTypeUpsertPayload) =>
  http.put<{ store_type: ReferenceCatalogItem }>(`/base-info/store-types/${id}`, payload)

export const listStoreManagementTypes = () =>
  http.get<{ store_management_types: ReferenceCatalogItem[] }>('/base-info/store-management-types')
export const createStoreManagementType = (payload: StoreManagementTypeUpsertPayload) =>
  http.post<{ store_management_type: ReferenceCatalogItem }>('/base-info/store-management-types', payload)
export const updateStoreManagementType = (id: number, payload: StoreManagementTypeUpsertPayload) =>
  http.put<{ store_management_type: ReferenceCatalogItem }>(`/base-info/store-management-types/${id}`, payload)

export const listAreaLevels = () => http.get<{ area_levels: ReferenceCatalogItem[] }>('/base-info/area-levels')
export const createAreaLevel = (payload: AreaLevelUpsertPayload) =>
  http.post<{ area_level: ReferenceCatalogItem }>('/base-info/area-levels', payload)
export const updateAreaLevel = (id: number, payload: AreaLevelUpsertPayload) =>
  http.put<{ area_level: ReferenceCatalogItem }>(`/base-info/area-levels/${id}`, payload)

export const listUnitTypes = () => http.get<{ unit_types: ReferenceCatalogItem[] }>('/base-info/unit-types')
export const createUnitType = (payload: UnitTypeUpsertPayload) =>
  http.post<{ unit_type: ReferenceCatalogItem }>('/base-info/unit-types', payload)
export const updateUnitType = (id: number, payload: UnitTypeUpsertPayload) =>
  http.put<{ unit_type: ReferenceCatalogItem }>(`/base-info/unit-types/${id}`, payload)

export const listShopTypes = () => http.get<{ shop_types: ReferenceCatalogItem[] }>('/base-info/shop-types')
export const createShopType = (payload: ShopTypeUpsertPayload) =>
  http.post<{ shop_type: ReferenceCatalogItem }>('/base-info/shop-types', payload)
export const updateShopType = (id: number, payload: ShopTypeUpsertPayload) =>
  http.put<{ shop_type: ReferenceCatalogItem }>(`/base-info/shop-types/${id}`, payload)

export const listCurrencyTypes = () =>
  http.get<{ currency_types: ReferenceCatalogItem[] }>('/base-info/currency-types')
export const createCurrencyType = (payload: CurrencyTypeUpsertPayload) =>
  http.post<{ currency_type: ReferenceCatalogItem }>('/base-info/currency-types', payload)
export const updateCurrencyType = (id: number, payload: CurrencyTypeUpsertPayload) =>
  http.put<{ currency_type: ReferenceCatalogItem }>(`/base-info/currency-types/${id}`, payload)

export const listTradeDefinitions = () =>
  http.get<{ trade_definitions: ReferenceCatalogItem[] }>('/base-info/trade-definitions')
export const createTradeDefinition = (payload: TradeDefinitionUpsertPayload) =>
  http.post<{ trade_definition: ReferenceCatalogItem }>('/base-info/trade-definitions', payload)
export const updateTradeDefinition = (id: number, payload: TradeDefinitionUpsertPayload) =>
  http.put<{ trade_definition: ReferenceCatalogItem }>(`/base-info/trade-definitions/${id}`, payload)
