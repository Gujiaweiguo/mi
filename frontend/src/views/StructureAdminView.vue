<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'

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
  type StructureArea,
  type StructureBuilding,
  type StructureFloor,
  type StructureLocation,
  type StructureStore,
  type StructureUnit,
} from '../api/structure'
import { listDepartments, type Department } from '../api/org'
import {
  listAreaLevels,
  listStoreManagementTypes,
  listStoreTypes,
  listUnitTypes,
  type ReferenceCatalogItem,
} from '../api/baseinfo'
import PageSection from '../components/platform/PageSection.vue'
import { getErrorMessage } from '../composables/useErrorMessage'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type StoreForm = {
  department_id: number | undefined
  store_type_id: number | undefined
  management_type_id: number | undefined
  code: string
  name: string
  short_name: string
  status: string
}

type BuildingForm = {
  code: string
  name: string
  status: string
}

type FloorForm = {
  code: string
  name: string
  status: string
  floor_plan_image_url: string
}

type AreaForm = {
  area_level_id: number | undefined
  code: string
  name: string
  status: string
}

type LocationForm = {
  floor_id: number | undefined
  code: string
  name: string
  status: string
}

type UnitForm = {
  unit_type_id: number | undefined
  area_id: number | undefined
  location_id: number | undefined
  code: string
  floor_area: number | undefined
  use_area: number | undefined
  rent_area: number | undefined
  is_rentable: boolean
  status: string
}

const stores = ref<StructureStore[]>([])
const buildings = ref<StructureBuilding[]>([])
const floors = ref<StructureFloor[]>([])
const areas = ref<StructureArea[]>([])
const locations = ref<StructureLocation[]>([])
const units = ref<StructureUnit[]>([])
const { t, locale } = useI18n()

const departments = ref<Department[]>([])
const storeTypes = ref<ReferenceCatalogItem[]>([])
const managementTypes = ref<ReferenceCatalogItem[]>([])
const areaLevels = ref<ReferenceCatalogItem[]>([])
const unitTypes = ref<ReferenceCatalogItem[]>([])

const selectedStoreId = ref<number | null>(null)
const selectedBuildingId = ref<number | null>(null)
const selectedFloorId = ref<number | null>(null)
const selectedAreaId = ref<number | null>(null)
const selectedLocationId = ref<number | null>(null)

const pageFeedback = ref<Feedback | null>(null)
const storeFeedback = ref<Feedback | null>(null)
const buildingFeedback = ref<Feedback | null>(null)
const floorFeedback = ref<Feedback | null>(null)
const areaFeedback = ref<Feedback | null>(null)
const locationFeedback = ref<Feedback | null>(null)
const unitFeedback = ref<Feedback | null>(null)

const isBootstrapping = ref(false)
const isStoresLoading = ref(false)
const isBuildingsLoading = ref(false)
const isFloorsLoading = ref(false)
const isAreasLoading = ref(false)
const isLocationsLoading = ref(false)
const isUnitsLoading = ref(false)

const isStoreSaving = ref(false)
const isBuildingSaving = ref(false)
const isFloorSaving = ref(false)
const isAreaSaving = ref(false)
const isLocationSaving = ref(false)
const isUnitSaving = ref(false)

const storeEditDialogOpen = ref(false)
const buildingEditDialogOpen = ref(false)
const floorEditDialogOpen = ref(false)
const areaEditDialogOpen = ref(false)
const locationEditDialogOpen = ref(false)
const unitEditDialogOpen = ref(false)
const storeFormRef = ref()
const buildingFormRef = ref()
const floorFormRef = ref()
const areaFormRef = ref()
const locationFormRef = ref()
const unitFormRef = ref()
const storeEditFormRef = ref()
const buildingEditFormRef = ref()
const floorEditFormRef = ref()
const areaEditFormRef = ref()
const locationEditFormRef = ref()
const unitEditFormRef = ref()

const isStoreUpdating = ref(false)
const isBuildingUpdating = ref(false)
const isFloorUpdating = ref(false)
const isAreaUpdating = ref(false)
const isLocationUpdating = ref(false)
const isUnitUpdating = ref(false)

const statusOptions = ['active', 'inactive'] as const
const requiredFieldMessage = 'This field is required'
const codeNameRules = {
  code: [{ required: true, message: requiredFieldMessage, trigger: 'blur' }],
  name: [{ required: true, message: requiredFieldMessage, trigger: 'blur' }],
}
const codeOnlyRules = {
  code: [{ required: true, message: requiredFieldMessage, trigger: 'blur' }],
}

const storeForm = reactive<StoreForm>({
  department_id: undefined,
  store_type_id: undefined,
  management_type_id: undefined,
  code: '',
  name: '',
  short_name: '',
  status: 'active',
})

const buildingForm = reactive<BuildingForm>({
  code: '',
  name: '',
  status: 'active',
})

const floorForm = reactive<FloorForm>({
  code: '',
  name: '',
  status: 'active',
  floor_plan_image_url: '',
})

const areaForm = reactive<AreaForm>({
  area_level_id: undefined,
  code: '',
  name: '',
  status: 'active',
})

const locationForm = reactive<LocationForm>({
  floor_id: undefined,
  code: '',
  name: '',
  status: 'active',
})

const unitForm = reactive<UnitForm>({
  unit_type_id: undefined,
  area_id: undefined,
  location_id: undefined,
  code: '',
  floor_area: undefined,
  use_area: undefined,
  rent_area: undefined,
  is_rentable: true,
  status: 'active',
})

const storeEdit = reactive<StoreForm & { id: number | null }>({
  id: null,
  department_id: undefined,
  store_type_id: undefined,
  management_type_id: undefined,
  code: '',
  name: '',
  short_name: '',
  status: 'active',
})

const buildingEdit = reactive<BuildingForm & { id: number | null; store_id: number | undefined }>({
  id: null,
  store_id: undefined,
  code: '',
  name: '',
  status: 'active',
})

const floorEdit = reactive<FloorForm & { id: number | null; building_id: number | undefined }>({
  id: null,
  building_id: undefined,
  code: '',
  name: '',
  status: 'active',
  floor_plan_image_url: '',
})

const areaEdit = reactive<AreaForm & { id: number | null; store_id: number | undefined }>({
  id: null,
  store_id: undefined,
  area_level_id: undefined,
  code: '',
  name: '',
  status: 'active',
})

const locationEdit = reactive<LocationForm & { id: number | null; store_id: number | undefined }>({
  id: null,
  store_id: undefined,
  floor_id: undefined,
  code: '',
  name: '',
  status: 'active',
})

const unitEdit = reactive<UnitForm & { id: number | null; building_id: number | undefined; floor_id: number | undefined }>({
  id: null,
  building_id: undefined,
  floor_id: undefined,
  unit_type_id: undefined,
  area_id: undefined,
  location_id: undefined,
  code: '',
  floor_area: undefined,
  use_area: undefined,
  rent_area: undefined,
  is_rentable: true,
  status: 'active',
})

const isPositiveInteger = (value: number | undefined): value is number =>
  typeof value === 'number' && Number.isInteger(value) && value > 0

const canCreateStore = computed(() => {
  return (
    isPositiveInteger(storeForm.department_id) &&
    isPositiveInteger(storeForm.store_type_id) &&
    isPositiveInteger(storeForm.management_type_id) &&
    Boolean(storeForm.code.trim()) &&
    Boolean(storeForm.name.trim()) &&
    Boolean(storeForm.short_name.trim())
  )
})

const canCreateBuilding = computed(() => {
  return selectedStoreId.value !== null && Boolean(buildingForm.code.trim()) && Boolean(buildingForm.name.trim())
})

const canCreateFloor = computed(() => {
  return selectedBuildingId.value !== null && Boolean(floorForm.code.trim()) && Boolean(floorForm.name.trim())
})

const isFiniteNumber = (value: number | undefined): value is number => typeof value === 'number' && Number.isFinite(value)

const canCreateArea = computed(() => {
  return selectedStoreId.value !== null && isPositiveInteger(areaForm.area_level_id) && Boolean(areaForm.code.trim()) && Boolean(areaForm.name.trim())
})

const canCreateLocation = computed(() => {
  return (
    selectedStoreId.value !== null &&
    isPositiveInteger(locationForm.floor_id) &&
    Boolean(locationForm.code.trim()) &&
    Boolean(locationForm.name.trim())
  )
})

const canCreateUnit = computed(() => {
  return (
    selectedBuildingId.value !== null &&
    selectedFloorId.value !== null &&
    isPositiveInteger(unitForm.unit_type_id) &&
    isPositiveInteger(unitForm.area_id) &&
    isPositiveInteger(unitForm.location_id) &&
    Boolean(unitForm.code.trim()) &&
    isFiniteNumber(unitForm.floor_area) &&
    isFiniteNumber(unitForm.use_area) &&
    isFiniteNumber(unitForm.rent_area)
  )
})

const validateForm = async (formRef: { value?: { validate?: () => Promise<unknown> } }) => {
  try {
    await formRef.value?.validate?.()
    return true
  } catch {
    return false
  }
}

const formatDate = (value: string) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(locale.value, { dateStyle: 'medium' }).format(new Date(value))
}

const resolveStatusLabel = (status: string) => {
  if (status === 'active') {
    return t('common.statuses.active')
  }

  if (status === 'inactive') {
    return t('common.statuses.inactive')
  }

  return status || t('common.emptyValue')
}

const replaceRow = <T extends { id: number }>(rows: T[], updated: T) => {
  const index = rows.findIndex((row) => row.id === updated.id)
  if (index === -1) {
    return [updated, ...rows]
  }
  const next = rows.slice()
  next.splice(index, 1, updated)
  return next
}

const resolveDepartmentLabel = (departmentId: number) => {
  const department = departments.value.find((item) => item.id === departmentId)
  if (!department) {
    return `#${departmentId}`
  }

  return `${department.code} — ${department.name}`
}

const resolveStoreTypeLabel = (storeTypeId: number) => {
  const storeType = storeTypes.value.find((item) => item.id === storeTypeId)
  if (!storeType) {
    return `#${storeTypeId}`
  }

  return `${storeType.code} — ${storeType.name}`
}

const resolveManagementTypeLabel = (managementTypeId: number) => {
  const managementType = managementTypes.value.find((item) => item.id === managementTypeId)
  if (!managementType) {
    return `#${managementTypeId}`
  }

  return `${managementType.code} — ${managementType.name}`
}

const resolveAreaLevelLabel = (areaLevelId: number) => {
  const areaLevel = areaLevels.value.find((item) => item.id === areaLevelId)
  if (!areaLevel) {
    return `#${areaLevelId}`
  }

  return `${areaLevel.code} — ${areaLevel.name}`
}

const resolveUnitTypeLabel = (unitTypeId: number) => {
  const unitType = unitTypes.value.find((item) => item.id === unitTypeId)
  if (!unitType) {
    return `#${unitTypeId}`
  }

  return `${unitType.code} — ${unitType.name}`
}

const resolveAreaLabel = (areaId: number) => {
  const area = areas.value.find((item) => item.id === areaId)
  if (!area) {
    return `#${areaId}`
  }

  return `${area.code} — ${area.name}`
}

const resolveLocationLabel = (locationId: number) => {
  const location = locations.value.find((item) => item.id === locationId)
  if (!location) {
    return `#${locationId}`
  }

  return `${location.code} — ${location.name}`
}

const resetStoreForm = () => {
  storeForm.department_id = undefined
  storeForm.store_type_id = undefined
  storeForm.management_type_id = undefined
  storeForm.code = ''
  storeForm.name = ''
  storeForm.short_name = ''
  storeForm.status = 'active'
}

const resetBuildingForm = () => {
  buildingForm.code = ''
  buildingForm.name = ''
  buildingForm.status = 'active'
}

const resetFloorForm = () => {
  floorForm.code = ''
  floorForm.name = ''
  floorForm.status = 'active'
  floorForm.floor_plan_image_url = ''
}

const resetAreaForm = () => {
  areaForm.area_level_id = undefined
  areaForm.code = ''
  areaForm.name = ''
  areaForm.status = 'active'
}

const resetLocationForm = () => {
  locationForm.floor_id = selectedFloorId.value ?? undefined
  locationForm.code = ''
  locationForm.name = ''
  locationForm.status = 'active'
}

const resetUnitForm = () => {
  unitForm.unit_type_id = undefined
  unitForm.area_id = undefined
  unitForm.location_id = undefined
  unitForm.code = ''
  unitForm.floor_area = undefined
  unitForm.use_area = undefined
  unitForm.rent_area = undefined
  unitForm.is_rentable = true
  unitForm.status = 'active'
}

const loadBuildings = async () => {
  if (selectedStoreId.value === null) {
    buildings.value = []
    selectedBuildingId.value = null
    selectedFloorId.value = null
    floors.value = []
    locationForm.floor_id = undefined
    locations.value = []
    selectedLocationId.value = null
    units.value = []
    return
  }

  isBuildingsLoading.value = true

  try {
    const response = await listStructureBuildings({ store_id: selectedStoreId.value })
    buildings.value = response.data.buildings ?? []

    const selectedBuildingStillAvailable =
      selectedBuildingId.value !== null && buildings.value.some((item) => item.id === selectedBuildingId.value)

    if (!selectedBuildingStillAvailable) {
      selectedBuildingId.value = buildings.value[0]?.id ?? null
    }

    await loadFloors()
  } catch (error) {
    buildings.value = []
    selectedBuildingId.value = null
    selectedFloorId.value = null
    floors.value = []
    locationForm.floor_id = undefined
    locations.value = []
    selectedLocationId.value = null
    units.value = []
    buildingFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.buildingsUnavailable'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToLoadBuildings')),
    }
  } finally {
    isBuildingsLoading.value = false
  }
}

const loadFloors = async () => {
  if (selectedBuildingId.value === null) {
    floors.value = []
    selectedFloorId.value = null
    locationForm.floor_id = undefined
    locations.value = []
    selectedLocationId.value = null
    units.value = []
    return
  }

  isFloorsLoading.value = true

  try {
    const response = await listStructureFloors({ building_id: selectedBuildingId.value })
    floors.value = response.data.floors ?? []

    const selectedFloorStillAvailable =
      selectedFloorId.value !== null && floors.value.some((item) => item.id === selectedFloorId.value)

    if (!selectedFloorStillAvailable) {
      selectedFloorId.value = floors.value[0]?.id ?? null
    }

    locationForm.floor_id = selectedFloorId.value ?? undefined
    await Promise.all([loadLocations(), loadUnits()])
  } catch (error) {
    floors.value = []
    selectedFloorId.value = null
    locationForm.floor_id = undefined
    locations.value = []
    selectedLocationId.value = null
    units.value = []
    floorFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.floorsUnavailable'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToLoadFloors')),
    }
  } finally {
    isFloorsLoading.value = false
  }
}

const loadAreas = async () => {
  if (selectedStoreId.value === null) {
    areas.value = []
    selectedAreaId.value = null
    return
  }

  isAreasLoading.value = true

  try {
    const response = await listStructureAreas({ store_id: selectedStoreId.value })
    areas.value = response.data.areas ?? []

    const selectedAreaStillAvailable = selectedAreaId.value !== null && areas.value.some((item) => item.id === selectedAreaId.value)
    if (!selectedAreaStillAvailable) {
      selectedAreaId.value = null
    }
  } catch (error) {
    areas.value = []
    selectedAreaId.value = null
    areaFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.areasUnavailable'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToLoadAreas')),
    }
  } finally {
    isAreasLoading.value = false
  }
}

const loadLocations = async () => {
  if (selectedStoreId.value === null || selectedFloorId.value === null) {
    locations.value = []
    selectedLocationId.value = null
    return
  }

  isLocationsLoading.value = true

  try {
    const response = await listStructureLocations({
      store_id: selectedStoreId.value,
      floor_id: selectedFloorId.value,
    })
    locations.value = response.data.locations ?? []

    const selectedLocationStillAvailable =
      selectedLocationId.value !== null && locations.value.some((item) => item.id === selectedLocationId.value)
    if (!selectedLocationStillAvailable) {
      selectedLocationId.value = null
    }
  } catch (error) {
    locations.value = []
    selectedLocationId.value = null
    locationFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.locationsUnavailable'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToLoadLocations')),
    }
  } finally {
    isLocationsLoading.value = false
  }
}

const loadUnits = async () => {
  if (selectedBuildingId.value === null) {
    units.value = []
    return
  }

  isUnitsLoading.value = true

  try {
    const response = await listStructureUnits({
      building_id: selectedBuildingId.value,
      floor_id: selectedFloorId.value ?? undefined,
      location_id: selectedLocationId.value ?? undefined,
      area_id: selectedAreaId.value ?? undefined,
    })
    units.value = response.data.units ?? []
  } catch (error) {
    units.value = []
    unitFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.unitsUnavailable'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToLoadUnits')),
    }
  } finally {
    isUnitsLoading.value = false
  }
}

const loadStores = async () => {
  isStoresLoading.value = true

  try {
    const response = await listStructureStores()
    stores.value = response.data.stores ?? []

    const selectedStoreStillAvailable =
      selectedStoreId.value !== null && stores.value.some((item) => item.id === selectedStoreId.value)

    if (!selectedStoreStillAvailable) {
      selectedStoreId.value = stores.value[0]?.id ?? null
      selectedBuildingId.value = null
      selectedFloorId.value = null
      selectedAreaId.value = null
      selectedLocationId.value = null
      floors.value = []
      locations.value = []
      areas.value = []
      units.value = []
      await Promise.all([loadBuildings(), loadAreas()])
    }
  } catch (error) {
    stores.value = []
    selectedStoreId.value = null
    selectedBuildingId.value = null
    selectedFloorId.value = null
    selectedAreaId.value = null
    selectedLocationId.value = null
    buildings.value = []
    floors.value = []
    areas.value = []
    locations.value = []
    units.value = []
    storeFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.storesUnavailable'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToLoadStores')),
    }
  } finally {
    isStoresLoading.value = false
  }
}

const loadReferenceData = async () => {
  const [departmentsResult, storeTypesResult, managementTypesResult, areaLevelsResult, unitTypesResult] =
    await Promise.allSettled([
    listDepartments(),
    listStoreTypes(),
    listStoreManagementTypes(),
    listAreaLevels(),
    listUnitTypes(),
  ])

  const loadErrors: string[] = []

  if (departmentsResult.status === 'fulfilled') {
    departments.value = departmentsResult.value.data.departments ?? []
  } else {
    departments.value = []
    loadErrors.push(getErrorMessage(departmentsResult.reason, t('structureAdmin.errors.unableToLoadDepartments')))
  }

  if (storeTypesResult.status === 'fulfilled') {
    storeTypes.value = storeTypesResult.value.data.store_types ?? []
  } else {
    storeTypes.value = []
    loadErrors.push(getErrorMessage(storeTypesResult.reason, t('structureAdmin.errors.unableToLoadStoreTypes')))
  }

  if (managementTypesResult.status === 'fulfilled') {
    managementTypes.value = managementTypesResult.value.data.store_management_types ?? []
  } else {
    managementTypes.value = []
    loadErrors.push(getErrorMessage(managementTypesResult.reason, t('structureAdmin.errors.unableToLoadManagementTypes')))
  }

  if (areaLevelsResult.status === 'fulfilled') {
    areaLevels.value = areaLevelsResult.value.data.area_levels ?? []
  } else {
    areaLevels.value = []
    loadErrors.push(getErrorMessage(areaLevelsResult.reason, t('structureAdmin.errors.unableToLoadAreaLevels')))
  }

  if (unitTypesResult.status === 'fulfilled') {
    unitTypes.value = unitTypesResult.value.data.unit_types ?? []
  } else {
    unitTypes.value = []
    loadErrors.push(getErrorMessage(unitTypesResult.reason, t('structureAdmin.errors.unableToLoadUnitTypes')))
  }

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.errors.referenceLookupsPartiallyUnavailable'),
      description: loadErrors.join(' '),
    }
  }
}

const handleSelectStore = async (storeId: number) => {
  if (selectedStoreId.value === storeId) {
    return
  }

  selectedStoreId.value = storeId
  selectedBuildingId.value = null
  selectedFloorId.value = null
  selectedAreaId.value = null
  selectedLocationId.value = null
  floors.value = []
  locations.value = []
  areas.value = []
  units.value = []
  locationForm.floor_id = undefined
  resetLocationForm()
  resetUnitForm()

  await Promise.all([loadBuildings(), loadAreas()])
}

const handleSelectBuilding = async (buildingId: number) => {
  if (selectedBuildingId.value === buildingId) {
    return
  }

  selectedBuildingId.value = buildingId
  selectedFloorId.value = null
  selectedLocationId.value = null
  locationForm.floor_id = undefined
  resetLocationForm()
  resetUnitForm()
  await loadFloors()
}

const handleSelectFloor = async (floorId: number) => {
  if (selectedFloorId.value === floorId) {
    return
  }

  selectedFloorId.value = floorId
  selectedLocationId.value = null
  locationForm.floor_id = floorId
  resetLocationForm()
  resetUnitForm()
  await Promise.all([loadLocations(), loadUnits()])
}

const handleToggleAreaFilter = async (areaId: number) => {
  selectedAreaId.value = selectedAreaId.value === areaId ? null : areaId
  await loadUnits()
}

const handleToggleLocationFilter = async (locationId: number) => {
  selectedLocationId.value = selectedLocationId.value === locationId ? null : locationId
  await loadUnits()
}

const handleCreateStore = async () => {
  if (!canCreateStore.value) {
    storeFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.storeDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.storeDetailsRequiredDescription'),
    }
    return
  }

  if (!(await validateForm(storeFormRef))) {
    return
  }

  const departmentId = storeForm.department_id
  const storeTypeId = storeForm.store_type_id
  const managementTypeId = storeForm.management_type_id

  if (!departmentId || !storeTypeId || !managementTypeId) {
    storeFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.storeDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.storeIdsRequiredDescription'),
    }
    return
  }

  isStoreSaving.value = true
  storeFeedback.value = null

  try {
    const response = await createStructureStore({
      department_id: departmentId,
      store_type_id: storeTypeId,
      management_type_id: managementTypeId,
      code: storeForm.code.trim(),
      name: storeForm.name.trim(),
      short_name: storeForm.short_name.trim(),
      status: storeForm.status,
    })

    stores.value = [response.data.store, ...stores.value]
    selectedStoreId.value = response.data.store.id
    selectedBuildingId.value = null
    selectedFloorId.value = null
    selectedAreaId.value = null
    selectedLocationId.value = null
    buildings.value = []
    floors.value = []
    areas.value = []
    locations.value = []
    units.value = []
    locationForm.floor_id = undefined
    resetLocationForm()
    resetUnitForm()

    storeFeedback.value = {
      type: 'success',
      title: t('structureAdmin.feedback.storeCreatedTitle'),
      description: t('structureAdmin.feedback.storeCreatedDescription', { code: response.data.store.code }),
    }
    resetStoreForm()
    await Promise.all([loadBuildings(), loadAreas()])
  } catch (error) {
    storeFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.storeCreationFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToCreateStore')),
    }
  } finally {
    isStoreSaving.value = false
  }
}

const handleCreateBuilding = async () => {
  if (!canCreateBuilding.value || selectedStoreId.value === null) {
    buildingFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.buildingDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.buildingDetailsRequiredDescription'),
    }
    return
  }

  if (!(await validateForm(buildingFormRef))) {
    return
  }

  isBuildingSaving.value = true
  buildingFeedback.value = null

  try {
    const response = await createStructureBuilding({
      store_id: selectedStoreId.value,
      code: buildingForm.code.trim(),
      name: buildingForm.name.trim(),
      status: buildingForm.status,
    })

    buildings.value = [response.data.building, ...buildings.value]
    selectedBuildingId.value = response.data.building.id

    buildingFeedback.value = {
      type: 'success',
      title: t('structureAdmin.feedback.buildingCreatedTitle'),
      description: t('structureAdmin.feedback.buildingCreatedDescription', { code: response.data.building.code }),
    }
    resetBuildingForm()
    await loadFloors()
  } catch (error) {
    buildingFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.buildingCreationFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToCreateBuilding')),
    }
  } finally {
    isBuildingSaving.value = false
  }
}

const handleCreateFloor = async () => {
  if (!canCreateFloor.value || selectedBuildingId.value === null) {
    floorFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.floorDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.floorDetailsRequiredDescription'),
    }
    return
  }

  if (!(await validateForm(floorFormRef))) {
    return
  }

  isFloorSaving.value = true
  floorFeedback.value = null

  try {
    const response = await createStructureFloor({
      building_id: selectedBuildingId.value,
      code: floorForm.code.trim(),
      name: floorForm.name.trim(),
      status: floorForm.status,
      floor_plan_image_url: floorForm.floor_plan_image_url.trim() ? floorForm.floor_plan_image_url.trim() : null,
    })

    floors.value = [response.data.floor, ...floors.value]
    selectedFloorId.value = response.data.floor.id
    selectedLocationId.value = null
    locationForm.floor_id = response.data.floor.id
    resetLocationForm()
    resetUnitForm()

    floorFeedback.value = {
      type: 'success',
      title: t('structureAdmin.feedback.floorCreatedTitle'),
      description: t('structureAdmin.feedback.floorCreatedDescription', { code: response.data.floor.code }),
    }
    resetFloorForm()
    await Promise.all([loadLocations(), loadUnits()])
  } catch (error) {
    floorFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.floorCreationFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToCreateFloor')),
    }
  } finally {
    isFloorSaving.value = false
  }
}

const handleCreateArea = async () => {
  if (!canCreateArea.value || selectedStoreId.value === null || !isPositiveInteger(areaForm.area_level_id)) {
    areaFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.areaDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.areaDetailsRequiredDescription'),
    }
    return
  }

  if (!(await validateForm(areaFormRef))) {
    return
  }

  isAreaSaving.value = true
  areaFeedback.value = null

  try {
    const response = await createStructureArea({
      store_id: selectedStoreId.value,
      area_level_id: areaForm.area_level_id,
      code: areaForm.code.trim(),
      name: areaForm.name.trim(),
      status: areaForm.status,
    })

    areas.value = [response.data.area, ...areas.value]
    areaFeedback.value = {
      type: 'success',
      title: t('structureAdmin.feedback.areaCreatedTitle'),
      description: t('structureAdmin.feedback.areaCreatedDescription', { code: response.data.area.code }),
    }
    resetAreaForm()
    await loadUnits()
  } catch (error) {
    areaFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.areaCreationFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToCreateArea')),
    }
  } finally {
    isAreaSaving.value = false
  }
}

const handleCreateLocation = async () => {
  if (!canCreateLocation.value || selectedStoreId.value === null || !isPositiveInteger(locationForm.floor_id)) {
    locationFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.locationDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.locationDetailsRequiredDescription'),
    }
    return
  }

  if (!(await validateForm(locationFormRef))) {
    return
  }

  isLocationSaving.value = true
  locationFeedback.value = null

  try {
    const response = await createStructureLocation({
      store_id: selectedStoreId.value,
      floor_id: locationForm.floor_id,
      code: locationForm.code.trim(),
      name: locationForm.name.trim(),
      status: locationForm.status,
    })

    if (response.data.location.floor_id === selectedFloorId.value) {
      locations.value = [response.data.location, ...locations.value]
    }

    locationFeedback.value = {
      type: 'success',
      title: t('structureAdmin.feedback.locationCreatedTitle'),
      description: t('structureAdmin.feedback.locationCreatedDescription', { code: response.data.location.code }),
    }
    resetLocationForm()
    await loadUnits()
  } catch (error) {
    locationFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.locationCreationFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToCreateLocation')),
    }
  } finally {
    isLocationSaving.value = false
  }
}

const handleCreateUnit = async () => {
  if (
    !canCreateUnit.value ||
    selectedBuildingId.value === null ||
    selectedFloorId.value === null ||
    !isPositiveInteger(unitForm.unit_type_id) ||
    !isPositiveInteger(unitForm.area_id) ||
    !isPositiveInteger(unitForm.location_id) ||
    !isFiniteNumber(unitForm.floor_area) ||
    !isFiniteNumber(unitForm.use_area) ||
    !isFiniteNumber(unitForm.rent_area)
  ) {
    unitFeedback.value = {
      type: 'warning',
      title: t('structureAdmin.feedback.unitDetailsRequiredTitle'),
      description: t('structureAdmin.feedback.unitDetailsRequiredDescription'),
    }
    return
  }

  if (!(await validateForm(unitFormRef))) {
    return
  }

  isUnitSaving.value = true
  unitFeedback.value = null

  try {
    const response = await createStructureUnit({
      building_id: selectedBuildingId.value,
      floor_id: selectedFloorId.value,
      location_id: unitForm.location_id,
      area_id: unitForm.area_id,
      unit_type_id: unitForm.unit_type_id,
      code: unitForm.code.trim(),
      floor_area: unitForm.floor_area,
      use_area: unitForm.use_area,
      rent_area: unitForm.rent_area,
      is_rentable: unitForm.is_rentable,
      status: unitForm.status,
    })

    units.value = [response.data.unit, ...units.value]
    unitFeedback.value = {
      type: 'success',
      title: t('structureAdmin.feedback.unitCreatedTitle'),
      description: t('structureAdmin.feedback.unitCreatedDescription', { code: response.data.unit.code }),
    }
    resetUnitForm()
  } catch (error) {
    unitFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.unitCreationFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToCreateUnit')),
    }
  } finally {
    isUnitSaving.value = false
  }
}

const openStoreEdit = (item: StructureStore) => {
  storeEdit.id = item.id
  storeEdit.department_id = item.department_id
  storeEdit.store_type_id = item.store_type_id
  storeEdit.management_type_id = item.management_type_id
  storeEdit.code = item.code
  storeEdit.name = item.name
  storeEdit.short_name = item.short_name
  storeEdit.status = item.status
  storeEditDialogOpen.value = true
}

const openBuildingEdit = (item: StructureBuilding) => {
  buildingEdit.id = item.id
  buildingEdit.store_id = item.store_id
  buildingEdit.code = item.code
  buildingEdit.name = item.name
  buildingEdit.status = item.status
  buildingEditDialogOpen.value = true
}

const openFloorEdit = (item: StructureFloor) => {
  floorEdit.id = item.id
  floorEdit.building_id = item.building_id
  floorEdit.code = item.code
  floorEdit.name = item.name
  floorEdit.status = item.status
  floorEdit.floor_plan_image_url = item.floor_plan_image_url ?? ''
  floorEditDialogOpen.value = true
}

const openAreaEdit = (item: StructureArea) => {
  areaEdit.id = item.id
  areaEdit.store_id = item.store_id
  areaEdit.area_level_id = item.area_level_id
  areaEdit.code = item.code
  areaEdit.name = item.name
  areaEdit.status = item.status
  areaEditDialogOpen.value = true
}

const openLocationEdit = (item: StructureLocation) => {
  locationEdit.id = item.id
  locationEdit.store_id = item.store_id
  locationEdit.floor_id = item.floor_id
  locationEdit.code = item.code
  locationEdit.name = item.name
  locationEdit.status = item.status
  locationEditDialogOpen.value = true
}

const openUnitEdit = (item: StructureUnit) => {
  unitEdit.id = item.id
  unitEdit.building_id = item.building_id
  unitEdit.floor_id = item.floor_id
  unitEdit.location_id = item.location_id
  unitEdit.area_id = item.area_id
  unitEdit.unit_type_id = item.unit_type_id
  unitEdit.code = item.code
  unitEdit.floor_area = item.floor_area
  unitEdit.use_area = item.use_area
  unitEdit.rent_area = item.rent_area
  unitEdit.is_rentable = item.is_rentable
  unitEdit.status = item.status
  unitEditDialogOpen.value = true
}

const handleUpdateStore = async () => {
  if (
    storeEdit.id === null ||
    !isPositiveInteger(storeEdit.department_id) ||
    !isPositiveInteger(storeEdit.store_type_id) ||
    !isPositiveInteger(storeEdit.management_type_id) ||
    !storeEdit.code.trim() ||
    !storeEdit.name.trim() ||
    !storeEdit.short_name.trim()
  ) {
    return
  }

  if (!(await validateForm(storeEditFormRef))) {
    return
  }

  isStoreUpdating.value = true

  try {
    const response = await updateStructureStore(storeEdit.id, {
      department_id: storeEdit.department_id,
      store_type_id: storeEdit.store_type_id,
      management_type_id: storeEdit.management_type_id,
      code: storeEdit.code.trim(),
      name: storeEdit.name.trim(),
      short_name: storeEdit.short_name.trim(),
      status: storeEdit.status,
    })

    stores.value = replaceRow(stores.value, response.data.store)
    storeEditDialogOpen.value = false
  } catch (error) {
    storeFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.storeUpdateFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToUpdateStore')),
    }
  } finally {
    isStoreUpdating.value = false
  }
}

const handleUpdateBuilding = async () => {
  if (
    buildingEdit.id === null ||
    !isPositiveInteger(buildingEdit.store_id) ||
    !buildingEdit.code.trim() ||
    !buildingEdit.name.trim()
  ) {
    return
  }

  if (!(await validateForm(buildingEditFormRef))) {
    return
  }

  isBuildingUpdating.value = true

  try {
    await updateStructureBuilding(buildingEdit.id, {
      store_id: buildingEdit.store_id,
      code: buildingEdit.code.trim(),
      name: buildingEdit.name.trim(),
      status: buildingEdit.status,
    })

    buildingEditDialogOpen.value = false
    await loadBuildings()
  } catch (error) {
    buildingFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.buildingUpdateFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToUpdateBuilding')),
    }
  } finally {
    isBuildingUpdating.value = false
  }
}

const handleUpdateFloor = async () => {
  if (
    floorEdit.id === null ||
    !isPositiveInteger(floorEdit.building_id) ||
    !floorEdit.code.trim() ||
    !floorEdit.name.trim()
  ) {
    return
  }

  if (!(await validateForm(floorEditFormRef))) {
    return
  }

  isFloorUpdating.value = true

  try {
    await updateStructureFloor(floorEdit.id, {
      building_id: floorEdit.building_id,
      code: floorEdit.code.trim(),
      name: floorEdit.name.trim(),
      status: floorEdit.status,
      floor_plan_image_url: floorEdit.floor_plan_image_url.trim() ? floorEdit.floor_plan_image_url.trim() : null,
    })

    floorEditDialogOpen.value = false
    await loadFloors()
  } catch (error) {
    floorFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.floorUpdateFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToUpdateFloor')),
    }
  } finally {
    isFloorUpdating.value = false
  }
}

const handleUpdateArea = async () => {
  if (
    areaEdit.id === null ||
    !isPositiveInteger(areaEdit.store_id) ||
    !isPositiveInteger(areaEdit.area_level_id) ||
    !areaEdit.code.trim() ||
    !areaEdit.name.trim()
  ) {
    return
  }

  if (!(await validateForm(areaEditFormRef))) {
    return
  }

  isAreaUpdating.value = true

  try {
    await updateStructureArea(areaEdit.id, {
      store_id: areaEdit.store_id,
      area_level_id: areaEdit.area_level_id,
      code: areaEdit.code.trim(),
      name: areaEdit.name.trim(),
      status: areaEdit.status,
    })

    areaEditDialogOpen.value = false
    await Promise.all([loadAreas(), loadUnits()])
  } catch (error) {
    areaFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.areaUpdateFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToUpdateArea')),
    }
  } finally {
    isAreaUpdating.value = false
  }
}

const handleUpdateLocation = async () => {
  if (
    locationEdit.id === null ||
    !isPositiveInteger(locationEdit.store_id) ||
    !isPositiveInteger(locationEdit.floor_id) ||
    !locationEdit.code.trim() ||
    !locationEdit.name.trim()
  ) {
    return
  }

  if (!(await validateForm(locationEditFormRef))) {
    return
  }

  isLocationUpdating.value = true

  try {
    await updateStructureLocation(locationEdit.id, {
      store_id: locationEdit.store_id,
      floor_id: locationEdit.floor_id,
      code: locationEdit.code.trim(),
      name: locationEdit.name.trim(),
      status: locationEdit.status,
    })

    locationEditDialogOpen.value = false
    await Promise.all([loadLocations(), loadUnits()])
  } catch (error) {
    locationFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.locationUpdateFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToUpdateLocation')),
    }
  } finally {
    isLocationUpdating.value = false
  }
}

const handleUpdateUnit = async () => {
  if (
    unitEdit.id === null ||
    !isPositiveInteger(unitEdit.building_id) ||
    !isPositiveInteger(unitEdit.floor_id) ||
    !isPositiveInteger(unitEdit.location_id) ||
    !isPositiveInteger(unitEdit.area_id) ||
    !isPositiveInteger(unitEdit.unit_type_id) ||
    !unitEdit.code.trim() ||
    !isFiniteNumber(unitEdit.floor_area) ||
    !isFiniteNumber(unitEdit.use_area) ||
    !isFiniteNumber(unitEdit.rent_area)
  ) {
    return
  }

  if (!(await validateForm(unitEditFormRef))) {
    return
  }

  isUnitUpdating.value = true

  try {
    await updateStructureUnit(unitEdit.id, {
      building_id: unitEdit.building_id,
      floor_id: unitEdit.floor_id,
      location_id: unitEdit.location_id,
      area_id: unitEdit.area_id,
      unit_type_id: unitEdit.unit_type_id,
      code: unitEdit.code.trim(),
      floor_area: unitEdit.floor_area,
      use_area: unitEdit.use_area,
      rent_area: unitEdit.rent_area,
      is_rentable: unitEdit.is_rentable,
      status: unitEdit.status,
    })

    unitEditDialogOpen.value = false
    await loadUnits()
  } catch (error) {
    unitFeedback.value = {
      type: 'error',
      title: t('structureAdmin.errors.unitUpdateFailed'),
      description: getErrorMessage(error, t('structureAdmin.errors.unableToUpdateUnit')),
    }
  } finally {
    isUnitUpdating.value = false
  }
}

onMounted(async () => {
  isBootstrapping.value = true
  pageFeedback.value = null

  await Promise.all([loadReferenceData(), loadStores()])

  if (selectedStoreId.value !== null) {
    await Promise.all([loadBuildings(), loadAreas()])
  }

  isBootstrapping.value = false
})
</script>

<template>
  <div
    class="structure-admin-view"
    v-loading="isBootstrapping || isStoresLoading || isBuildingsLoading || isFloorsLoading || isAreasLoading || isLocationsLoading || isUnitsLoading"
    data-testid="structure-admin-view"
  >
    <PageSection
      :eyebrow="t('structureAdmin.eyebrow')"
      :title="t('structureAdmin.title')"
      :summary="t('structureAdmin.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('structureAdmin.tags.stores', { count: stores.length }) }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('structureAdmin.tags.buildings', { count: buildings.length }) }}</el-tag>
        <el-tag effect="plain" type="warning">{{ t('structureAdmin.tags.floors', { count: floors.length }) }}</el-tag>
        <el-tag effect="plain" type="info">{{ t('structureAdmin.tags.areas', { count: areas.length }) }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('structureAdmin.tags.locations', { count: locations.length }) }}</el-tag>
        <el-tag effect="plain" type="warning">{{ t('structureAdmin.tags.units', { count: units.length }) }}</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="pageFeedback"
      :closable="false"
      :title="pageFeedback.title"
      :type="pageFeedback.type"
      :description="pageFeedback.description"
      show-icon
    />

    <div class="structure-admin-view__grid">
      <el-card class="structure-admin-view__card" shadow="never">
        <template #header>
          <div class="structure-admin-view__card-header">
            <span>{{ t('structureAdmin.cards.stores') }}</span>
            <div class="structure-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: stores.length }) }}</el-tag>
              <el-button :loading="isStoresLoading || isBootstrapping" @click="loadStores">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="storeFeedback"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="storeFeedback.title"
          :type="storeFeedback.type"
          :description="storeFeedback.description"
          show-icon
        />

        <el-form ref="storeFormRef" :model="storeForm" :rules="codeNameRules" label-position="top" class="structure-admin-view__form" @submit.prevent>
          <div class="structure-admin-view__form-grid structure-admin-view__form-grid--store">
            <el-form-item :label="t('structureAdmin.fields.storeCode')" prop="code">
              <el-input
                v-model="storeForm.code"
                :placeholder="t('structureAdmin.placeholders.storeCode')"
                data-testid="structure-store-code-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.storeName')" prop="name">
              <el-input
                v-model="storeForm.name"
                :placeholder="t('structureAdmin.placeholders.storeName')"
                data-testid="structure-store-name-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.storeShortName')">
              <el-input
                v-model="storeForm.short_name"
                :placeholder="t('structureAdmin.placeholders.storeShortName')"
                data-testid="structure-store-short-name-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.departmentId')">
              <el-input-number
                v-model="storeForm.department_id"
                :min="1"
                controls-position="right"
                data-testid="structure-store-department-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.storeTypeId')">
              <el-input-number
                v-model="storeForm.store_type_id"
                :min="1"
                controls-position="right"
                data-testid="structure-store-type-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.managementTypeId')">
              <el-input-number
                v-model="storeForm.management_type_id"
                :min="1"
                controls-position="right"
                data-testid="structure-store-management-type-input"
              />
            </el-form-item>

            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="storeForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <p class="structure-admin-view__hint">
            {{
              t('structureAdmin.hints.lookupsLoaded', {
                departments: departments.length,
                storeTypes: storeTypes.length,
                managementTypes: managementTypes.length,
                areaLevels: areaLevels.length,
                unitTypes: unitTypes.length,
              })
            }}
          </p>

          <div class="structure-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isStoreSaving"
              :disabled="!canCreateStore"
              data-testid="structure-store-create-button"
              @click="handleCreateStore"
            >
              {{ t('structureAdmin.actions.createStore') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="stores"
          row-key="id"
          class="structure-admin-view__table"
          :empty-text="isStoresLoading || isBootstrapping ? t('structureAdmin.table.loadingStores') : t('structureAdmin.table.emptyStores')"
          data-testid="structure-stores-table"
        >
          <el-table-column prop="code" :label="t('structureAdmin.fields.code')" min-width="130" />
          <el-table-column prop="name" :label="t('structureAdmin.fields.name')" min-width="180" />
          <el-table-column prop="short_name" :label="t('structureAdmin.fields.short')" min-width="120" />
          <el-table-column :label="t('structureAdmin.fields.department')" min-width="180">
            <template #default="scope">
              {{ resolveDepartmentLabel(scope.row.department_id) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.storeType')" min-width="180">
            <template #default="scope">
              {{ resolveStoreTypeLabel(scope.row.store_type_id) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.management')" min-width="180">
            <template #default="scope">
              {{ resolveManagementTypeLabel(scope.row.management_type_id) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="110">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ resolveStatusLabel(scope.row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.updated')" min-width="150">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="160" fixed="right">
            <template #default="scope">
              <div class="structure-admin-view__row-actions">
                <el-button
                  size="small"
                  :type="scope.row.id === selectedStoreId ? 'primary' : 'default'"
                  plain
                  @click="handleSelectStore(scope.row.id)"
                >
                  {{ scope.row.id === selectedStoreId ? t('structureAdmin.actions.selected') : t('structureAdmin.actions.select') }}
                </el-button>
                <el-button size="small" @click="openStoreEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="structure-admin-view__card" shadow="never">
        <template #header>
          <div class="structure-admin-view__card-header">
            <span>{{ t('structureAdmin.cards.buildings') }}</span>
            <div class="structure-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: buildings.length }) }}</el-tag>
              <el-button :loading="isBuildingsLoading" :disabled="selectedStoreId === null" @click="loadBuildings">
                {{ t('common.actions.refresh') }}
              </el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="buildingFeedback"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="buildingFeedback.title"
          :type="buildingFeedback.type"
          :description="buildingFeedback.description"
          show-icon
        />

        <el-alert
          v-if="selectedStoreId === null"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="t('structureAdmin.alerts.selectStoreForBuildingsTitle')"
          type="info"
          :description="t('structureAdmin.alerts.selectStoreForBuildingsDescription')"
          show-icon
        />

        <el-form ref="buildingFormRef" :model="buildingForm" :rules="codeNameRules" label-position="top" class="structure-admin-view__form" @submit.prevent>
          <div class="structure-admin-view__form-grid">
            <el-form-item :label="t('structureAdmin.fields.selectedStoreId')">
              <el-input :model-value="selectedStoreId === null ? '' : String(selectedStoreId)" readonly />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.buildingCode')" prop="code">
              <el-input
                v-model="buildingForm.code"
                :placeholder="t('structureAdmin.placeholders.buildingCode')"
                data-testid="structure-building-code-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.buildingName')" prop="name">
              <el-input
                v-model="buildingForm.name"
                :placeholder="t('structureAdmin.placeholders.buildingName')"
                data-testid="structure-building-name-input"
              />
            </el-form-item>

            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="buildingForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="structure-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isBuildingSaving"
              :disabled="!canCreateBuilding"
              data-testid="structure-building-create-button"
              @click="handleCreateBuilding"
            >
              {{ t('structureAdmin.actions.createBuilding') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="buildings"
          row-key="id"
          class="structure-admin-view__table"
          :empty-text="selectedStoreId === null ? t('structureAdmin.table.selectStoreFirst') : isBuildingsLoading ? t('structureAdmin.table.loadingBuildings') : t('structureAdmin.table.emptyBuildings')"
          data-testid="structure-buildings-table"
        >
          <el-table-column prop="code" :label="t('structureAdmin.fields.code')" min-width="130" />
          <el-table-column prop="name" :label="t('structureAdmin.fields.name')" min-width="180" />
          <el-table-column prop="store_id" :label="t('structureAdmin.fields.storeId')" min-width="100" />
          <el-table-column :label="t('common.columns.status')" min-width="110">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ resolveStatusLabel(scope.row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.updated')" min-width="150">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="160" fixed="right">
            <template #default="scope">
              <div class="structure-admin-view__row-actions">
                <el-button
                  size="small"
                  :type="scope.row.id === selectedBuildingId ? 'primary' : 'default'"
                  plain
                  @click="handleSelectBuilding(scope.row.id)"
                >
                  {{ scope.row.id === selectedBuildingId ? t('structureAdmin.actions.selected') : t('structureAdmin.actions.select') }}
                </el-button>
                <el-button size="small" @click="openBuildingEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="structure-admin-view__card" shadow="never">
        <template #header>
          <div class="structure-admin-view__card-header">
            <span>{{ t('structureAdmin.cards.floors') }}</span>
            <div class="structure-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: floors.length }) }}</el-tag>
              <el-button :loading="isFloorsLoading" :disabled="selectedBuildingId === null" @click="loadFloors">
                {{ t('common.actions.refresh') }}
              </el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="floorFeedback"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="floorFeedback.title"
          :type="floorFeedback.type"
          :description="floorFeedback.description"
          show-icon
        />

        <el-alert
          v-if="selectedBuildingId === null"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="t('structureAdmin.alerts.selectBuildingForFloorsTitle')"
          type="info"
          :description="t('structureAdmin.alerts.selectBuildingForFloorsDescription')"
          show-icon
        />

        <el-form ref="floorFormRef" :model="floorForm" :rules="codeNameRules" label-position="top" class="structure-admin-view__form" @submit.prevent>
          <div class="structure-admin-view__form-grid">
            <el-form-item :label="t('structureAdmin.fields.selectedBuildingId')">
              <el-input :model-value="selectedBuildingId === null ? '' : String(selectedBuildingId)" readonly />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.floorCode')" prop="code">
              <el-input
                v-model="floorForm.code"
                :placeholder="t('structureAdmin.placeholders.floorCode')"
                data-testid="structure-floor-code-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.floorName')" prop="name">
              <el-input
                v-model="floorForm.name"
                :placeholder="t('structureAdmin.placeholders.floorName')"
                data-testid="structure-floor-name-input"
              />
            </el-form-item>

            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="floorForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.floorPlanUrl')">
              <el-input
                v-model="floorForm.floor_plan_image_url"
                :placeholder="t('structureAdmin.placeholders.floorPlanUrl')"
                data-testid="structure-floor-plan-input"
              />
            </el-form-item>
          </div>

          <div class="structure-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isFloorSaving"
              :disabled="!canCreateFloor"
              data-testid="structure-floor-create-button"
              @click="handleCreateFloor"
            >
              {{ t('structureAdmin.actions.createFloor') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="floors"
          row-key="id"
          class="structure-admin-view__table"
          :empty-text="selectedBuildingId === null ? t('structureAdmin.table.selectBuildingFirst') : isFloorsLoading ? t('structureAdmin.table.loadingFloors') : t('structureAdmin.table.emptyFloors')"
          data-testid="structure-floors-table"
        >
          <el-table-column prop="code" :label="t('structureAdmin.fields.code')" min-width="120" />
          <el-table-column prop="name" :label="t('structureAdmin.fields.name')" min-width="180" />
          <el-table-column prop="building_id" :label="t('structureAdmin.fields.buildingId')" min-width="110" />
          <el-table-column :label="t('structureAdmin.fields.planUrl')" min-width="200">
            <template #default="scope">
              <span class="structure-admin-view__muted">{{ scope.row.floor_plan_image_url ?? t('common.emptyValue') }}</span>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="110">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ resolveStatusLabel(scope.row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.updated')" min-width="150">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="170" fixed="right">
            <template #default="scope">
              <div class="structure-admin-view__row-actions">
                <el-button
                  size="small"
                  :type="scope.row.id === selectedFloorId ? 'primary' : 'default'"
                  plain
                  @click="handleSelectFloor(scope.row.id)"
                >
                  {{ scope.row.id === selectedFloorId ? t('structureAdmin.actions.selected') : t('structureAdmin.actions.select') }}
                </el-button>
                <el-button size="small" @click="openFloorEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="structure-admin-view__card" shadow="never">
        <template #header>
          <div class="structure-admin-view__card-header">
            <span>{{ t('structureAdmin.cards.areas') }}</span>
            <div class="structure-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: areas.length }) }}</el-tag>
              <el-button :loading="isAreasLoading" :disabled="selectedStoreId === null" @click="loadAreas">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="areaFeedback"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="areaFeedback.title"
          :type="areaFeedback.type"
          :description="areaFeedback.description"
          show-icon
        />

        <el-alert
          v-if="selectedStoreId === null"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="t('structureAdmin.alerts.selectStoreForAreasTitle')"
          type="info"
          :description="t('structureAdmin.alerts.selectStoreForAreasDescription')"
          show-icon
        />

        <el-form ref="areaFormRef" :model="areaForm" :rules="codeNameRules" label-position="top" class="structure-admin-view__form" @submit.prevent>
          <div class="structure-admin-view__form-grid">
            <el-form-item :label="t('structureAdmin.fields.selectedStoreId')">
              <el-input :model-value="selectedStoreId === null ? '' : String(selectedStoreId)" readonly />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.areaCode')" prop="code">
              <el-input v-model="areaForm.code" :placeholder="t('structureAdmin.placeholders.areaCode')" data-testid="structure-area-code-input" />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.areaName')" prop="name">
              <el-input v-model="areaForm.name" :placeholder="t('structureAdmin.placeholders.areaName')" data-testid="structure-area-name-input" />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.areaLevelId')">
              <el-select v-model="areaForm.area_level_id" :placeholder="t('structureAdmin.placeholders.selectAreaLevel')" data-testid="structure-area-level-input">
                <el-option
                  v-for="option in areaLevels"
                  :key="option.id"
                  :label="`${option.code} — ${option.name}`"
                  :value="option.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="areaForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="structure-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isAreaSaving"
              :disabled="!canCreateArea"
              data-testid="structure-area-create-button"
              @click="handleCreateArea"
            >
              {{ t('structureAdmin.actions.createArea') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="areas"
          row-key="id"
          class="structure-admin-view__table"
          :empty-text="selectedStoreId === null ? t('structureAdmin.table.selectStoreFirst') : isAreasLoading ? t('structureAdmin.table.loadingAreas') : t('structureAdmin.table.emptyAreas')"
          data-testid="structure-areas-table"
        >
          <el-table-column prop="code" :label="t('structureAdmin.fields.code')" min-width="130" />
          <el-table-column prop="name" :label="t('structureAdmin.fields.name')" min-width="180" />
          <el-table-column :label="t('structureAdmin.fields.areaLevel')" min-width="180">
            <template #default="scope">
              {{ resolveAreaLevelLabel(scope.row.area_level_id) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="110">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ resolveStatusLabel(scope.row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.updated')" min-width="150">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="180" fixed="right">
            <template #default="scope">
              <div class="structure-admin-view__row-actions">
                <el-button
                  size="small"
                  :type="scope.row.id === selectedAreaId ? 'primary' : 'default'"
                  plain
                  @click="handleToggleAreaFilter(scope.row.id)"
                >
                  {{ scope.row.id === selectedAreaId ? t('structureAdmin.actions.filtered') : t('structureAdmin.actions.filter') }}
                </el-button>
                <el-button size="small" @click="openAreaEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="structure-admin-view__card" shadow="never">
        <template #header>
          <div class="structure-admin-view__card-header">
            <span>{{ t('structureAdmin.cards.locations') }}</span>
            <div class="structure-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: locations.length }) }}</el-tag>
              <el-button
                :loading="isLocationsLoading"
                :disabled="selectedStoreId === null || selectedFloorId === null"
                @click="loadLocations"
              >
                {{ t('common.actions.refresh') }}
              </el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="locationFeedback"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="locationFeedback.title"
          :type="locationFeedback.type"
          :description="locationFeedback.description"
          show-icon
        />

        <el-alert
          v-if="selectedStoreId === null || selectedFloorId === null"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="t('structureAdmin.alerts.selectStoreAndFloorForLocationsTitle')"
          type="info"
          :description="t('structureAdmin.alerts.selectStoreAndFloorForLocationsDescription')"
          show-icon
        />

        <el-form ref="locationFormRef" :model="locationForm" :rules="codeNameRules" label-position="top" class="structure-admin-view__form" @submit.prevent>
          <div class="structure-admin-view__form-grid">
            <el-form-item :label="t('structureAdmin.fields.selectedStoreId')">
              <el-input :model-value="selectedStoreId === null ? '' : String(selectedStoreId)" readonly />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.floorId')">
              <el-select v-model="locationForm.floor_id" :placeholder="t('structureAdmin.placeholders.selectFloor')" data-testid="structure-location-floor-input">
                <el-option v-for="item in floors" :key="item.id" :label="`${item.code} — ${item.name}`" :value="item.id" />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.locationCode')" prop="code">
              <el-input
                v-model="locationForm.code"
                :placeholder="t('structureAdmin.placeholders.locationCode')"
                data-testid="structure-location-code-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.locationName')" prop="name">
              <el-input
                v-model="locationForm.name"
                :placeholder="t('structureAdmin.placeholders.locationName')"
                data-testid="structure-location-name-input"
              />
            </el-form-item>

            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="locationForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="structure-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isLocationSaving"
              :disabled="!canCreateLocation"
              data-testid="structure-location-create-button"
              @click="handleCreateLocation"
            >
              {{ t('structureAdmin.actions.createLocation') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="locations"
          row-key="id"
          class="structure-admin-view__table"
          :empty-text="selectedFloorId === null ? t('structureAdmin.table.selectFloorFirst') : isLocationsLoading ? t('structureAdmin.table.loadingLocations') : t('structureAdmin.table.emptyLocations')"
          data-testid="structure-locations-table"
        >
          <el-table-column prop="code" :label="t('structureAdmin.fields.code')" min-width="130" />
          <el-table-column prop="name" :label="t('structureAdmin.fields.name')" min-width="180" />
          <el-table-column prop="floor_id" :label="t('structureAdmin.fields.floorId')" min-width="110" />
          <el-table-column :label="t('common.columns.status')" min-width="110">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ resolveStatusLabel(scope.row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.updated')" min-width="150">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="180" fixed="right">
            <template #default="scope">
              <div class="structure-admin-view__row-actions">
                <el-button
                  size="small"
                  :type="scope.row.id === selectedLocationId ? 'primary' : 'default'"
                  plain
                  @click="handleToggleLocationFilter(scope.row.id)"
                >
                  {{ scope.row.id === selectedLocationId ? t('structureAdmin.actions.filtered') : t('structureAdmin.actions.filter') }}
                </el-button>
                <el-button size="small" @click="openLocationEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="structure-admin-view__card" shadow="never">
        <template #header>
          <div class="structure-admin-view__card-header">
            <span>{{ t('structureAdmin.cards.units') }}</span>
            <div class="structure-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: units.length }) }}</el-tag>
              <el-button :loading="isUnitsLoading" :disabled="selectedBuildingId === null" @click="loadUnits">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="unitFeedback"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="unitFeedback.title"
          :type="unitFeedback.type"
          :description="unitFeedback.description"
          show-icon
        />

        <el-alert
          v-if="selectedBuildingId === null || selectedFloorId === null"
          :closable="false"
          class="structure-admin-view__feedback"
          :title="t('structureAdmin.alerts.selectBuildingAndFloorForUnitsTitle')"
          type="info"
          :description="t('structureAdmin.alerts.selectBuildingAndFloorForUnitsDescription')"
          show-icon
        />

        <el-form ref="unitFormRef" :model="unitForm" :rules="codeOnlyRules" label-position="top" class="structure-admin-view__form" @submit.prevent>
          <div class="structure-admin-view__form-grid structure-admin-view__form-grid--unit">
            <el-form-item :label="t('structureAdmin.fields.selectedBuildingId')">
              <el-input :model-value="selectedBuildingId === null ? '' : String(selectedBuildingId)" readonly />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.selectedFloorId')">
              <el-input :model-value="selectedFloorId === null ? '' : String(selectedFloorId)" readonly />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.unitCode')" prop="code">
              <el-input v-model="unitForm.code" :placeholder="t('structureAdmin.placeholders.unitCode')" data-testid="structure-unit-code-input" />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.unitTypeId')">
              <el-select v-model="unitForm.unit_type_id" :placeholder="t('structureAdmin.placeholders.selectUnitType')" data-testid="structure-unit-type-input">
                <el-option
                  v-for="option in unitTypes"
                  :key="option.id"
                  :label="`${option.code} — ${option.name}`"
                  :value="option.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.areaId')">
              <el-select v-model="unitForm.area_id" :placeholder="t('structureAdmin.placeholders.selectArea')" data-testid="structure-unit-area-input">
                <el-option v-for="item in areas" :key="item.id" :label="`${item.code} — ${item.name}`" :value="item.id" />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.locationId')">
              <el-select v-model="unitForm.location_id" :placeholder="t('structureAdmin.placeholders.selectLocation')" data-testid="structure-unit-location-input">
                <el-option
                  v-for="item in locations"
                  :key="item.id"
                  :label="`${item.code} — ${item.name}`"
                  :value="item.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.floorArea')">
              <el-input-number
                v-model="unitForm.floor_area"
                :min="0"
                :precision="2"
                controls-position="right"
                data-testid="structure-unit-floor-area-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.useArea')">
              <el-input-number
                v-model="unitForm.use_area"
                :min="0"
                :precision="2"
                controls-position="right"
                data-testid="structure-unit-use-area-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.rentArea')">
              <el-input-number
                v-model="unitForm.rent_area"
                :min="0"
                :precision="2"
                controls-position="right"
                data-testid="structure-unit-rent-area-input"
              />
            </el-form-item>

            <el-form-item :label="t('structureAdmin.fields.rentable')">
              <el-switch v-model="unitForm.is_rentable" data-testid="structure-unit-rentable-input" />
            </el-form-item>

            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="unitForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="structure-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isUnitSaving"
              :disabled="!canCreateUnit"
              data-testid="structure-unit-create-button"
              @click="handleCreateUnit"
            >
              {{ t('structureAdmin.actions.createUnit') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="units"
          row-key="id"
          class="structure-admin-view__table"
          :empty-text="selectedBuildingId === null || selectedFloorId === null ? t('structureAdmin.table.selectBuildingAndFloorFirst') : isUnitsLoading ? t('structureAdmin.table.loadingUnits') : t('structureAdmin.table.emptyUnits')"
          data-testid="structure-units-table"
        >
          <el-table-column prop="code" :label="t('structureAdmin.fields.code')" min-width="130" />
          <el-table-column prop="building_id" :label="t('structureAdmin.fields.buildingId')" min-width="110" />
          <el-table-column prop="floor_id" :label="t('structureAdmin.fields.floorId')" min-width="110" />
          <el-table-column :label="t('structureAdmin.fields.area')" min-width="170">
            <template #default="scope">
              {{ resolveAreaLabel(scope.row.area_id) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.location')" min-width="170">
            <template #default="scope">
              {{ resolveLocationLabel(scope.row.location_id) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.unitType')" min-width="170">
            <template #default="scope">
              {{ resolveUnitTypeLabel(scope.row.unit_type_id) }}
            </template>
          </el-table-column>
          <el-table-column prop="floor_area" :label="t('structureAdmin.fields.floorArea')" min-width="110" />
          <el-table-column prop="use_area" :label="t('structureAdmin.fields.useArea')" min-width="110" />
          <el-table-column prop="rent_area" :label="t('structureAdmin.fields.rentArea')" min-width="110" />
          <el-table-column :label="t('structureAdmin.fields.rentable')" min-width="100">
            <template #default="scope">
              <el-tag :type="scope.row.is_rentable ? 'success' : 'info'" effect="plain">
                {{ scope.row.is_rentable ? t('common.values.yes') : t('common.values.no') }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="110">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ resolveStatusLabel(scope.row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('structureAdmin.fields.updated')" min-width="150">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
              <el-button size="small" @click="openUnitEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </div>

    <el-dialog v-model="storeEditDialogOpen" :title="t('structureAdmin.dialogs.editStore')" width="42rem">
      <el-form ref="storeEditFormRef" :model="storeEdit" :rules="codeNameRules" label-position="top" @submit.prevent>
        <div class="structure-admin-view__dialog-grid structure-admin-view__dialog-grid--store">
          <el-form-item :label="t('structureAdmin.fields.departmentId')">
            <el-input-number v-model="storeEdit.department_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.storeTypeId')">
            <el-input-number v-model="storeEdit.store_type_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.managementTypeId')">
            <el-input-number v-model="storeEdit.management_type_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.code')" prop="code">
            <el-input v-model="storeEdit.code" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.name')" prop="name">
            <el-input v-model="storeEdit.name" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.shortName')">
            <el-input v-model="storeEdit.short_name" />
          </el-form-item>

          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="storeEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>

      <template #footer>
        <el-button @click="storeEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isStoreUpdating" @click="handleUpdateStore">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="buildingEditDialogOpen" :title="t('structureAdmin.dialogs.editBuilding')" width="36rem">
      <el-form ref="buildingEditFormRef" :model="buildingEdit" :rules="codeNameRules" label-position="top" @submit.prevent>
        <div class="structure-admin-view__dialog-grid">
          <el-form-item :label="t('structureAdmin.fields.storeId')">
            <el-input-number v-model="buildingEdit.store_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.code')" prop="code">
            <el-input v-model="buildingEdit.code" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.name')" prop="name">
            <el-input v-model="buildingEdit.name" />
          </el-form-item>

          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="buildingEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>

      <template #footer>
        <el-button @click="buildingEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isBuildingUpdating" @click="handleUpdateBuilding">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="floorEditDialogOpen" :title="t('structureAdmin.dialogs.editFloor')" width="38rem">
      <el-form ref="floorEditFormRef" :model="floorEdit" :rules="codeNameRules" label-position="top" @submit.prevent>
        <div class="structure-admin-view__dialog-grid">
          <el-form-item :label="t('structureAdmin.fields.buildingId')">
            <el-input-number v-model="floorEdit.building_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.code')" prop="code">
            <el-input v-model="floorEdit.code" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.name')" prop="name">
            <el-input v-model="floorEdit.name" />
          </el-form-item>

          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="floorEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.floorPlanUrl')">
            <el-input v-model="floorEdit.floor_plan_image_url" />
          </el-form-item>
        </div>
      </el-form>

      <template #footer>
        <el-button @click="floorEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isFloorUpdating" @click="handleUpdateFloor">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="areaEditDialogOpen" :title="t('structureAdmin.dialogs.editArea')" width="36rem">
      <el-form ref="areaEditFormRef" :model="areaEdit" :rules="codeNameRules" label-position="top" @submit.prevent>
        <div class="structure-admin-view__dialog-grid">
          <el-form-item :label="t('structureAdmin.fields.storeId')">
            <el-input-number v-model="areaEdit.store_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.areaLevelId')">
            <el-select v-model="areaEdit.area_level_id">
              <el-option
                v-for="option in areaLevels"
                :key="option.id"
                :label="`${option.code} — ${option.name}`"
                :value="option.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.code')" prop="code">
            <el-input v-model="areaEdit.code" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.name')" prop="name">
            <el-input v-model="areaEdit.name" />
          </el-form-item>

          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="areaEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>

      <template #footer>
        <el-button @click="areaEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isAreaUpdating" @click="handleUpdateArea">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="locationEditDialogOpen" :title="t('structureAdmin.dialogs.editLocation')" width="36rem">
      <el-form ref="locationEditFormRef" :model="locationEdit" :rules="codeNameRules" label-position="top" @submit.prevent>
        <div class="structure-admin-view__dialog-grid">
          <el-form-item :label="t('structureAdmin.fields.storeId')">
            <el-input-number v-model="locationEdit.store_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.floorId')">
            <el-select v-model="locationEdit.floor_id">
              <el-option v-for="item in floors" :key="item.id" :label="`${item.code} — ${item.name}`" :value="item.id" />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.code')" prop="code">
            <el-input v-model="locationEdit.code" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.name')" prop="name">
            <el-input v-model="locationEdit.name" />
          </el-form-item>

          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="locationEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>

      <template #footer>
        <el-button @click="locationEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isLocationUpdating" @click="handleUpdateLocation">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="unitEditDialogOpen" :title="t('structureAdmin.dialogs.editUnit')" width="42rem">
      <el-form ref="unitEditFormRef" :model="unitEdit" :rules="codeOnlyRules" label-position="top" @submit.prevent>
        <div class="structure-admin-view__dialog-grid structure-admin-view__dialog-grid--unit">
          <el-form-item :label="t('structureAdmin.fields.buildingId')">
            <el-input-number v-model="unitEdit.building_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.floorId')">
            <el-input-number v-model="unitEdit.floor_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.areaId')">
            <el-select v-model="unitEdit.area_id">
              <el-option v-for="item in areas" :key="item.id" :label="`${item.code} — ${item.name}`" :value="item.id" />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.locationId')">
            <el-select v-model="unitEdit.location_id">
              <el-option
                v-for="item in locations"
                :key="item.id"
                :label="`${item.code} — ${item.name}`"
                :value="item.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.unitTypeId')">
            <el-select v-model="unitEdit.unit_type_id">
              <el-option
                v-for="option in unitTypes"
                :key="option.id"
                :label="`${option.code} — ${option.name}`"
                :value="option.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.code')" prop="code">
            <el-input v-model="unitEdit.code" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.floorArea')">
            <el-input-number v-model="unitEdit.floor_area" :min="0" :precision="2" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.useArea')">
            <el-input-number v-model="unitEdit.use_area" :min="0" :precision="2" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.rentArea')">
            <el-input-number v-model="unitEdit.rent_area" :min="0" :precision="2" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('structureAdmin.fields.rentable')">
            <el-switch v-model="unitEdit.is_rentable" />
          </el-form-item>

          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="unitEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>

      <template #footer>
        <el-button @click="unitEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isUnitUpdating" @click="handleUpdateUnit">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<style scoped>
.structure-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.structure-admin-view__grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.structure-admin-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.structure-admin-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.structure-admin-view__card-actions {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: var(--mi-space-3);
  flex-wrap: wrap;
}

.structure-admin-view__feedback {
  margin-bottom: var(--mi-space-4);
}

.structure-admin-view__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  margin-bottom: var(--mi-space-4);
}

.structure-admin-view__form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.structure-admin-view__form-grid--store {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.structure-admin-view__form-grid--unit {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.structure-admin-view__form-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.structure-admin-view__hint {
  margin: 0;
  font-size: var(--mi-font-size-100);
  color: var(--mi-color-muted);
}

.structure-admin-view__muted {
  color: var(--mi-color-muted);
}

.structure-admin-view__table,
.structure-admin-view__form-grid :deep(.el-input-number),
.structure-admin-view__form-grid :deep(.el-select),
.structure-admin-view__dialog-grid :deep(.el-input-number),
.structure-admin-view__dialog-grid :deep(.el-select) {
  width: 100%;
}

.structure-admin-view__row-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-2);
}

.structure-admin-view__dialog-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.structure-admin-view__dialog-grid--store {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.structure-admin-view__dialog-grid--unit {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

@media (max-width: 52rem) {
  .structure-admin-view__grid,
  .structure-admin-view__form-grid,
  .structure-admin-view__form-grid--store,
  .structure-admin-view__form-grid--unit,
  .structure-admin-view__dialog-grid,
  .structure-admin-view__dialog-grid--store,
  .structure-admin-view__dialog-grid--unit {
    grid-template-columns: minmax(0, 1fr);
  }

  .structure-admin-view__card-header,
  .structure-admin-view__form-actions {
    align-items: flex-start;
    flex-direction: column;
  }

  .structure-admin-view__card-actions {
    justify-content: flex-start;
  }

  .structure-admin-view__row-actions {
    flex-wrap: wrap;
  }
}
</style>
