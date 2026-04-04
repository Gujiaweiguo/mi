import { http } from './http'

export interface StructureStore {
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

export interface StructureBuilding {
  id: number
  store_id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

export interface StructureFloor {
  id: number
  building_id: number
  code: string
  name: string
  status: string
  floor_plan_image_url?: string | null
  created_at: string
  updated_at: string
}

export interface StructureArea {
  id: number
  store_id: number
  area_level_id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

export interface StructureLocation {
  id: number
  store_id: number
  floor_id: number
  code: string
  name: string
  status: string
  created_at: string
  updated_at: string
}

export interface StructureUnit {
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

export type StructureStoreUpsertPayload = {
  department_id: number
  store_type_id: number
  management_type_id: number
  code: string
  name: string
  short_name: string
  status?: string
}

export type StructureBuildingUpsertPayload = {
  store_id: number
  code: string
  name: string
  status?: string
}

export type StructureFloorUpsertPayload = {
  building_id: number
  code: string
  name: string
  status?: string
  floor_plan_image_url?: string | null
}

export type StructureAreaUpsertPayload = {
  store_id: number
  area_level_id: number
  code: string
  name: string
  status?: string
}

export type StructureLocationUpsertPayload = {
  store_id: number
  floor_id: number
  code: string
  name: string
  status?: string
}

export type StructureUnitUpsertPayload = {
  building_id: number
  floor_id: number
  location_id: number
  area_id: number
  unit_type_id: number
  shop_type_id?: number | null
  code: string
  floor_area: number
  use_area: number
  rent_area: number
  is_rentable: boolean
  status?: string
}

export type ListStructureBuildingsParams = {
  store_id?: number
}

export type ListStructureFloorsParams = {
  building_id?: number
}

export type ListStructureAreasParams = {
  store_id?: number
}

export type ListStructureLocationsParams = {
  store_id?: number
  floor_id?: number
}

export type ListStructureUnitsParams = {
  building_id?: number
  floor_id?: number
  location_id?: number
  area_id?: number
}

export const listStructureStores = () => http.get<{ stores: StructureStore[] }>('/structure/stores')

export const createStructureStore = (payload: StructureStoreUpsertPayload) =>
  http.post<{ store: StructureStore }>('/structure/stores', payload)

export const updateStructureStore = (id: number, payload: StructureStoreUpsertPayload) =>
  http.put<{ store: StructureStore }>(`/structure/stores/${id}`, payload)

export const listStructureBuildings = (params?: ListStructureBuildingsParams) =>
  http.get<{ buildings: StructureBuilding[] }>('/structure/buildings', { params })

export const createStructureBuilding = (payload: StructureBuildingUpsertPayload) =>
  http.post<{ building: StructureBuilding }>('/structure/buildings', payload)

export const updateStructureBuilding = (id: number, payload: StructureBuildingUpsertPayload) =>
  http.put<{ building: StructureBuilding }>(`/structure/buildings/${id}`, payload)

export const listStructureFloors = (params?: ListStructureFloorsParams) =>
  http.get<{ floors: StructureFloor[] }>('/structure/floors', { params })

export const createStructureFloor = (payload: StructureFloorUpsertPayload) =>
  http.post<{ floor: StructureFloor }>('/structure/floors', payload)

export const updateStructureFloor = (id: number, payload: StructureFloorUpsertPayload) =>
  http.put<{ floor: StructureFloor }>(`/structure/floors/${id}`, payload)

export const listStructureAreas = (params?: ListStructureAreasParams) =>
  http.get<{ areas: StructureArea[] }>('/structure/areas', { params })

export const createStructureArea = (payload: StructureAreaUpsertPayload) =>
  http.post<{ area: StructureArea }>('/structure/areas', payload)

export const updateStructureArea = (id: number, payload: StructureAreaUpsertPayload) =>
  http.put<{ area: StructureArea }>(`/structure/areas/${id}`, payload)

export const listStructureLocations = (params?: ListStructureLocationsParams) =>
  http.get<{ locations: StructureLocation[] }>('/structure/locations', { params })

export const createStructureLocation = (payload: StructureLocationUpsertPayload) =>
  http.post<{ location: StructureLocation }>('/structure/locations', payload)

export const updateStructureLocation = (id: number, payload: StructureLocationUpsertPayload) =>
  http.put<{ location: StructureLocation }>(`/structure/locations/${id}`, payload)

export const listStructureUnits = (params?: ListStructureUnitsParams) =>
  http.get<{ units: StructureUnit[] }>('/structure/units', { params })

export const createStructureUnit = (payload: StructureUnitUpsertPayload) =>
  http.post<{ unit: StructureUnit }>('/structure/units', payload)

export const updateStructureUnit = (id: number, payload: StructureUnitUpsertPayload) =>
  http.put<{ unit: StructureUnit }>(`/structure/units/${id}`, payload)
