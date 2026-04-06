<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { listShopTypes, listUnitTypes, type ReferenceCatalogItem } from '../api/baseinfo'
import {
  listStructureAreas,
  listStructureBuildings,
  listStructureFloors,
  listStructureStores,
  listStructureUnits,
  updateStructureUnit,
  type StructureArea,
  type StructureBuilding,
  type StructureFloor,
  type StructureStore,
  type StructureUnit,
} from '../api/structure'
import PageSection from '../components/platform/PageSection.vue'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type FilterState = {
  storeId: number | undefined
  buildingId: number | undefined
  floorId: number | undefined
  areaId: number | undefined
  rentableOnly: boolean
  status: string
}

type UnitEditForm = {
  id: number | null
  building_id: number | undefined
  floor_id: number | undefined
  location_id: number | undefined
  area_id: number | undefined
  unit_type_id: number | undefined
  shop_type_id: number | undefined
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
const units = ref<StructureUnit[]>([])
const unitTypes = ref<ReferenceCatalogItem[]>([])
const shopTypes = ref<ReferenceCatalogItem[]>([])
const { t } = useI18n()

const isBootstrapping = ref(false)
const isStoresLoading = ref(false)
const isBuildingsLoading = ref(false)
const isFloorsLoading = ref(false)
const isAreasLoading = ref(false)
const isUnitsLoading = ref(false)
const isSaving = ref(false)

const feedback = ref<Feedback | null>(null)
const editDialogOpen = ref(false)

const filters = reactive<FilterState>({
  storeId: undefined,
  buildingId: undefined,
  floorId: undefined,
  areaId: undefined,
  rentableOnly: false,
  status: '',
})

const filterBaseline = ref<FilterState>({
  storeId: undefined,
  buildingId: undefined,
  floorId: undefined,
  areaId: undefined,
  rentableOnly: false,
  status: '',
})

const unitEdit = reactive<UnitEditForm>({
  id: null,
  building_id: undefined,
  floor_id: undefined,
  location_id: undefined,
  area_id: undefined,
  unit_type_id: undefined,
  shop_type_id: undefined,
  code: '',
  floor_area: undefined,
  use_area: undefined,
  rent_area: undefined,
  is_rentable: true,
  status: 'active',
})

const statusOptions = ['active', 'inactive'] as const

const isPositiveInteger = (value: number | undefined): value is number =>
  typeof value === 'number' && Number.isInteger(value) && value > 0

const isFiniteNumber = (value: number | undefined): value is number => typeof value === 'number' && Number.isFinite(value)

const getErrorMessage = (error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)

const resolveStatusLabel = (status: string) => {
  if (status === 'active') {
    return t('common.statuses.active')
  }

  if (status === 'inactive') {
    return t('common.statuses.inactive')
  }

  return status || t('common.emptyValue')
}

const syncFilterBaseline = () => {
  filterBaseline.value = {
    storeId: filters.storeId,
    buildingId: filters.buildingId,
    floorId: filters.floorId,
    areaId: filters.areaId,
    rentableOnly: filters.rentableOnly,
    status: filters.status,
  }
}

const restoreBaselineFilters = () => {
  filters.storeId = filterBaseline.value.storeId
  filters.buildingId = filterBaseline.value.buildingId
  filters.floorId = filterBaseline.value.floorId
  filters.areaId = filterBaseline.value.areaId
  filters.rentableOnly = filterBaseline.value.rentableOnly
  filters.status = filterBaseline.value.status
}

const isDirty = computed(() => {
  return (
    filters.storeId !== filterBaseline.value.storeId ||
    filters.buildingId !== filterBaseline.value.buildingId ||
    filters.floorId !== filterBaseline.value.floorId ||
    filters.areaId !== filterBaseline.value.areaId ||
    filters.rentableOnly !== filterBaseline.value.rentableOnly ||
    filters.status !== filterBaseline.value.status
  )
})

const selectedStore = computed(() => stores.value.find((item) => item.id === filters.storeId) ?? null)

const canSaveEdit = computed(() => {
  return (
    unitEdit.id !== null &&
    isPositiveInteger(unitEdit.building_id) &&
    isPositiveInteger(unitEdit.floor_id) &&
    isPositiveInteger(unitEdit.location_id) &&
    isPositiveInteger(unitEdit.area_id) &&
    isPositiveInteger(unitEdit.unit_type_id) &&
    Boolean(unitEdit.code.trim()) &&
    isFiniteNumber(unitEdit.floor_area) &&
    isFiniteNumber(unitEdit.use_area) &&
    isFiniteNumber(unitEdit.rent_area)
  )
})

const resolveBuildingLabel = (buildingId: number) => {
  const building = buildings.value.find((item) => item.id === buildingId)
  return building ? `${building.code} — ${building.name}` : `#${buildingId}`
}

const resolveFloorLabel = (floorId: number) => {
  const floor = floors.value.find((item) => item.id === floorId)
  return floor ? `${floor.code} — ${floor.name}` : `#${floorId}`
}

const resolveAreaLabel = (areaId: number) => {
  const area = areas.value.find((item) => item.id === areaId)
  return area ? `${area.code} — ${area.name}` : `#${areaId}`
}

const resolveUnitTypeLabel = (unitTypeId: number) => {
  const unitType = unitTypes.value.find((item) => item.id === unitTypeId)
  return unitType ? `${unitType.code} — ${unitType.name}` : `#${unitTypeId}`
}

const resolveShopTypeLabel = (shopTypeId: number | null) => {
  if (shopTypeId === null) {
    return t('common.emptyValue')
  }

  const shopType = shopTypes.value.find((item) => item.id === shopTypeId)
  return shopType ? `${shopType.code} — ${shopType.name}` : `#${shopTypeId}`
}

const resolveStatusTag = (status: string) => {
  if (status === 'active') {
    return { label: resolveStatusLabel(status), type: 'success' as const }
  }

  if (status === 'inactive') {
    return { label: resolveStatusLabel(status), type: 'info' as const }
  }

  return { label: resolveStatusLabel(status), type: 'warning' as const }
}

const formatArea = (value: number) => value.toFixed(2)

const replaceUnit = (updated: StructureUnit) => {
  const index = units.value.findIndex((item) => item.id === updated.id)
  if (index === -1) {
    units.value = [updated, ...units.value]
    return
  }

  const next = units.value.slice()
  next.splice(index, 1, updated)
  units.value = next
}

const filteredUnits = computed(() => {
  return units.value.filter((unit) => {
    if (filters.rentableOnly && !unit.is_rentable) {
      return false
    }

    if (filters.status && unit.status !== filters.status) {
      return false
    }

    return true
  })
})

const loadUnitTypes = async () => {
  try {
    const response = await listUnitTypes()
    unitTypes.value = response.data.unit_types ?? []
  } catch (error) {
    unitTypes.value = []
    feedback.value = {
      type: 'warning',
      title: t('rentableAreaAdmin.errors.unitTypesUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadUnitTypes')),
    }
  }
}

const loadShopTypes = async () => {
  try {
    const response = await listShopTypes()
    shopTypes.value = response.data.shop_types ?? []
  } catch (error) {
    shopTypes.value = []
    feedback.value = {
      type: 'warning',
      title: t('rentableAreaAdmin.errors.shopTypesUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadShopTypes')),
    }
  }
}

const loadFloors = async () => {
  if (!isPositiveInteger(filters.buildingId)) {
    floors.value = []
    filters.floorId = undefined
    return
  }

  isFloorsLoading.value = true

  try {
    const response = await listStructureFloors({ building_id: filters.buildingId })
    floors.value = response.data.floors ?? []

    const floorStillAvailable = floors.value.some((item) => item.id === filters.floorId)
    if (!floorStillAvailable) {
      filters.floorId = floors.value[0]?.id
    }
  } catch (error) {
    floors.value = []
    filters.floorId = undefined
    feedback.value = {
      type: 'error',
      title: t('rentableAreaAdmin.errors.floorsUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadFloors')),
    }
  } finally {
    isFloorsLoading.value = false
  }
}

const loadBuildings = async () => {
  if (!isPositiveInteger(filters.storeId)) {
    buildings.value = []
    floors.value = []
    filters.buildingId = undefined
    filters.floorId = undefined
    return
  }

  isBuildingsLoading.value = true

  try {
    const response = await listStructureBuildings({ store_id: filters.storeId })
    buildings.value = response.data.buildings ?? []

    const buildingStillAvailable = buildings.value.some((item) => item.id === filters.buildingId)
    if (!buildingStillAvailable) {
      filters.buildingId = buildings.value[0]?.id
    }

    await loadFloors()
  } catch (error) {
    buildings.value = []
    floors.value = []
    filters.buildingId = undefined
    filters.floorId = undefined
    feedback.value = {
      type: 'error',
      title: t('rentableAreaAdmin.errors.buildingsUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadBuildings')),
    }
  } finally {
    isBuildingsLoading.value = false
  }
}

const loadAreas = async () => {
  if (!isPositiveInteger(filters.storeId)) {
    areas.value = []
    filters.areaId = undefined
    return
  }

  isAreasLoading.value = true

  try {
    const response = await listStructureAreas({ store_id: filters.storeId })
    areas.value = response.data.areas ?? []

    const areaStillAvailable = areas.value.some((item) => item.id === filters.areaId)
    if (!areaStillAvailable) {
      filters.areaId = undefined
    }
  } catch (error) {
    areas.value = []
    filters.areaId = undefined
    feedback.value = {
      type: 'error',
      title: t('rentableAreaAdmin.errors.areasUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadAreas')),
    }
  } finally {
    isAreasLoading.value = false
  }
}

const loadStores = async () => {
  isStoresLoading.value = true

  try {
    const response = await listStructureStores()
    stores.value = response.data.stores ?? []

    const storeStillAvailable = stores.value.some((item) => item.id === filters.storeId)
    if (!storeStillAvailable) {
      filters.storeId = stores.value[0]?.id
    }

    await Promise.all([loadBuildings(), loadAreas()])
  } catch (error) {
    stores.value = []
    buildings.value = []
    floors.value = []
    areas.value = []
    filters.storeId = undefined
    filters.buildingId = undefined
    filters.floorId = undefined
    filters.areaId = undefined
    feedback.value = {
      type: 'error',
      title: t('rentableAreaAdmin.errors.storesUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadStores')),
    }
  } finally {
    isStoresLoading.value = false
  }
}

const loadUnits = async () => {
  if (!isPositiveInteger(filters.buildingId)) {
    units.value = []
    return
  }

  isUnitsLoading.value = true
  feedback.value = null

  try {
    const response = await listStructureUnits({
      building_id: filters.buildingId,
      floor_id: filters.floorId,
      area_id: filters.areaId,
    })
    units.value = response.data.units ?? []
  } catch (error) {
    units.value = []
    feedback.value = {
      type: 'error',
      title: t('rentableAreaAdmin.errors.unitsUnavailable'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToLoadUnits')),
    }
  } finally {
    isUnitsLoading.value = false
  }
}

const handleStoreChange = async () => {
  filters.buildingId = undefined
  filters.floorId = undefined
  filters.areaId = undefined
  units.value = []
  await Promise.all([loadBuildings(), loadAreas()])
}

const handleBuildingChange = async () => {
  filters.floorId = undefined
  units.value = []
  await loadFloors()
}

const applyFilters = async () => {
  await loadUnits()
}

const handleReset = async () => {
  restoreBaselineFilters()
  await Promise.all([loadBuildings(), loadAreas()])
  await loadUnits()
}

const openEditDialog = (unit: StructureUnit) => {
  unitEdit.id = unit.id
  unitEdit.building_id = unit.building_id
  unitEdit.floor_id = unit.floor_id
  unitEdit.location_id = unit.location_id
  unitEdit.area_id = unit.area_id
  unitEdit.unit_type_id = unit.unit_type_id
  unitEdit.shop_type_id = unit.shop_type_id ?? undefined
  unitEdit.code = unit.code
  unitEdit.floor_area = unit.floor_area
  unitEdit.use_area = unit.use_area
  unitEdit.rent_area = unit.rent_area
  unitEdit.is_rentable = unit.is_rentable
  unitEdit.status = unit.status
  editDialogOpen.value = true
}

const handleSave = async () => {
  if (!canSaveEdit.value || unitEdit.id === null) {
    return
  }

  const buildingId = unitEdit.building_id
  const floorId = unitEdit.floor_id
  const locationId = unitEdit.location_id
  const areaId = unitEdit.area_id
  const unitTypeId = unitEdit.unit_type_id
  const shopTypeId = unitEdit.shop_type_id
  const floorArea = unitEdit.floor_area
  const useArea = unitEdit.use_area
  const rentArea = unitEdit.rent_area

  if (
    !isPositiveInteger(buildingId) ||
    !isPositiveInteger(floorId) ||
    !isPositiveInteger(locationId) ||
    !isPositiveInteger(areaId) ||
    !isPositiveInteger(unitTypeId) ||
    !isFiniteNumber(floorArea) ||
    !isFiniteNumber(useArea) ||
    !isFiniteNumber(rentArea)
  ) {
    return
  }

  isSaving.value = true

  try {
    const response = await updateStructureUnit(unitEdit.id, {
      building_id: buildingId,
      floor_id: floorId,
      location_id: locationId,
      area_id: areaId,
      unit_type_id: unitTypeId,
      shop_type_id: shopTypeId ?? null,
      code: unitEdit.code.trim(),
      floor_area: floorArea,
      use_area: useArea,
      rent_area: rentArea,
      is_rentable: unitEdit.is_rentable,
      status: unitEdit.status,
    })

    replaceUnit(response.data.unit)
    editDialogOpen.value = false
    feedback.value = {
      type: 'success',
      title: t('rentableAreaAdmin.feedback.unitUpdatedTitle'),
      description: t('rentableAreaAdmin.feedback.unitUpdatedDescription', { code: response.data.unit.code }),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('rentableAreaAdmin.errors.updateFailed'),
      description: getErrorMessage(error, t('rentableAreaAdmin.errors.unableToUpdateUnit')),
    }
  } finally {
    isSaving.value = false
  }
}

onMounted(async () => {
  isBootstrapping.value = true
    await Promise.all([loadUnitTypes(), loadShopTypes(), loadStores()])
  await loadUnits()
  syncFilterBaseline()
  isBootstrapping.value = false
})
</script>

<template>
  <div class="rentable-area-admin-view" data-testid="rentable-area-admin-view">
    <PageSection
      :eyebrow="t('rentableAreaAdmin.eyebrow')"
      :title="t('rentableAreaAdmin.title')"
      :summary="t('rentableAreaAdmin.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ selectedStore?.short_name ?? t('rentableAreaAdmin.tags.noStore') }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('rentableAreaAdmin.tags.visibleUnits', { count: filteredUnits.length }) }}</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="feedback"
      data-testid="rentable-area-feedback-alert"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <el-card class="rentable-area-admin-view__card" shadow="never">
      <template #header>
        <div class="rentable-area-admin-view__card-header">
          <span>{{ t('rentableAreaAdmin.cards.filters') }}</span>
        </div>
      </template>

      <el-form label-position="top" @submit.prevent>
        <div class="rentable-area-admin-view__filter-grid">
          <el-form-item :label="t('rentableAreaAdmin.fields.store')">
            <el-select
              v-model="filters.storeId"
              filterable
              :placeholder="t('rentableAreaAdmin.placeholders.selectStore')"
              :loading="isStoresLoading || isBootstrapping"
              data-testid="rentable-area-store-filter"
              @change="handleStoreChange"
            >
              <el-option
                v-for="store in stores"
                :key="store.id"
                :label="`${store.code} — ${store.name}`"
                :value="store.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('rentableAreaAdmin.fields.building')">
            <el-select
              v-model="filters.buildingId"
              filterable
              :placeholder="t('rentableAreaAdmin.placeholders.selectBuilding')"
              :disabled="!filters.storeId"
              :loading="isBuildingsLoading"
              data-testid="rentable-area-building-filter"
              @change="handleBuildingChange"
            >
              <el-option
                v-for="building in buildings"
                :key="building.id"
                :label="`${building.code} — ${building.name}`"
                :value="building.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('rentableAreaAdmin.fields.floor')">
            <el-select
              v-model="filters.floorId"
              clearable
              filterable
              :placeholder="t('rentableAreaAdmin.placeholders.allFloors')"
              :disabled="!filters.buildingId"
              :loading="isFloorsLoading"
              data-testid="rentable-area-floor-filter"
            >
              <el-option
                v-for="floor in floors"
                :key="floor.id"
                :label="`${floor.code} — ${floor.name}`"
                :value="floor.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('rentableAreaAdmin.fields.area')">
            <el-select
              v-model="filters.areaId"
              clearable
              filterable
              :placeholder="t('rentableAreaAdmin.placeholders.allAreas')"
              :disabled="!filters.storeId"
              :loading="isAreasLoading"
              data-testid="rentable-area-area-filter"
            >
              <el-option
                v-for="area in areas"
                :key="area.id"
                :label="`${area.code} — ${area.name}`"
                :value="area.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('rentableAreaAdmin.fields.rentableOnly')" class="rentable-area-admin-view__switch-field">
            <el-switch v-model="filters.rentableOnly" data-testid="rentable-area-rentable-switch" />
          </el-form-item>

          <el-form-item :label="t('rentableAreaAdmin.fields.status')">
            <el-select
              v-model="filters.status"
              clearable
              :placeholder="t('rentableAreaAdmin.placeholders.allStatuses')"
              data-testid="rentable-area-status-filter"
            >
              <el-option
                v-for="option in statusOptions"
                :key="option"
                :label="resolveStatusLabel(option)"
                :value="option"
                :data-testid="`rentable-area-status-option-${option}`"
              />
            </el-select>
          </el-form-item>
        </div>

        <div class="rentable-area-admin-view__filter-actions">
          <el-button :disabled="!isDirty" data-testid="rentable-area-reset-button" @click="handleReset">{{ t('filterForm.reset') }}</el-button>
          <el-button
            type="primary"
            :loading="isUnitsLoading"
            data-testid="rentable-area-apply-button"
            @click="applyFilters"
          >
            {{ t('filterForm.submit') }}
          </el-button>
        </div>
      </el-form>
    </el-card>

    <el-card class="rentable-area-admin-view__card" shadow="never">
      <template #header>
        <div class="rentable-area-admin-view__card-header">
          <span>{{ t('rentableAreaAdmin.cards.units') }}</span>
          <el-tag effect="plain" type="info">{{ t('rentableAreaAdmin.tags.matchingUnits', { count: filteredUnits.length }) }}</el-tag>
        </div>
      </template>

      <el-table
        :data="filteredUnits"
        row-key="id"
        class="rentable-area-admin-view__table"
        :empty-text="t('rentableAreaAdmin.table.empty')"
        data-testid="rentable-area-units-table"
      >
        <el-table-column prop="code" :label="t('rentableAreaAdmin.fields.unit')" min-width="140" />
        <el-table-column :label="t('rentableAreaAdmin.fields.building')" min-width="180">
          <template #default="scope">
            {{ resolveBuildingLabel(scope.row.building_id) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.floor')" min-width="160">
          <template #default="scope">
            {{ resolveFloorLabel(scope.row.floor_id) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.area')" min-width="180">
          <template #default="scope">
            {{ resolveAreaLabel(scope.row.area_id) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.type')" min-width="160">
          <template #default="scope">
            {{ resolveUnitTypeLabel(scope.row.unit_type_id) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.shopType')" min-width="180">
          <template #default="scope">
            {{ resolveShopTypeLabel(scope.row.shop_type_id) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.floorArea')" min-width="120" align="right">
          <template #default="scope">
            {{ formatArea(scope.row.floor_area) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.useArea')" min-width="120" align="right">
          <template #default="scope">
            {{ formatArea(scope.row.use_area) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.rentArea')" min-width="120" align="right">
          <template #default="scope">
            {{ formatArea(scope.row.rent_area) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('rentableAreaAdmin.fields.rentable')" min-width="120">
          <template #default="scope">
            <el-tag :type="scope.row.is_rentable ? 'success' : 'info'" effect="plain">
              {{ scope.row.is_rentable ? t('rentableAreaAdmin.values.rentable') : t('rentableAreaAdmin.values.nonRentable') }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.status')" min-width="110">
          <template #default="scope">
            <el-tag :type="resolveStatusTag(scope.row.status).type" effect="plain">
              {{ resolveStatusTag(scope.row.status).label }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
          <template #default="scope">
            <el-button text type="primary" :data-testid="`rentable-area-edit-button-${scope.row.id}`" @click="openEditDialog(scope.row)">{{ t('common.actions.edit') }}</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="editDialogOpen" :title="t('rentableAreaAdmin.dialogs.editUnit')" width="36rem">
      <div class="rentable-area-admin-view__dialog" data-testid="rentable-area-edit-dialog">
        <div class="rentable-area-admin-view__dialog-meta">
          <div class="rentable-area-admin-view__meta-block">
            <span class="rentable-area-admin-view__meta-label">{{ t('rentableAreaAdmin.fields.unit') }}</span>
            <strong>{{ unitEdit.code || t('common.emptyValue') }}</strong>
          </div>
          <div class="rentable-area-admin-view__meta-block">
            <span class="rentable-area-admin-view__meta-label">{{ t('rentableAreaAdmin.fields.placement') }}</span>
            <strong>
              {{ isPositiveInteger(unitEdit.floor_id) ? resolveFloorLabel(unitEdit.floor_id) : t('common.emptyValue') }}
            </strong>
          </div>
          <div class="rentable-area-admin-view__meta-block">
            <span class="rentable-area-admin-view__meta-label">{{ t('rentableAreaAdmin.fields.type') }}</span>
            <strong>
              {{ isPositiveInteger(unitEdit.unit_type_id) ? resolveUnitTypeLabel(unitEdit.unit_type_id) : t('common.emptyValue') }}
            </strong>
          </div>
          <div class="rentable-area-admin-view__meta-block">
            <span class="rentable-area-admin-view__meta-label">{{ t('rentableAreaAdmin.fields.shopType') }}</span>
            <strong>{{ resolveShopTypeLabel(unitEdit.shop_type_id ?? null) }}</strong>
          </div>
        </div>

        <el-form label-position="top" @submit.prevent>
          <div class="rentable-area-admin-view__dialog-grid">
            <el-form-item :label="t('rentableAreaAdmin.fields.rentArea')">
              <el-input-number
                v-model="unitEdit.rent_area"
                :min="0"
                :precision="2"
                controls-position="right"
                data-testid="rentable-area-unit-edit-rent-area"
              />
            </el-form-item>

            <el-form-item :label="t('rentableAreaAdmin.fields.status')">
              <el-select v-model="unitEdit.status" data-testid="rentable-area-unit-edit-status">
                <el-option
                  v-for="option in statusOptions"
                  :key="option"
                  :label="resolveStatusLabel(option)"
                  :value="option"
                  :data-testid="`rentable-area-edit-status-option-${option}`"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('rentableAreaAdmin.fields.shopType')">
              <el-select
                v-model="unitEdit.shop_type_id"
                clearable
                filterable
                :placeholder="t('rentableAreaAdmin.placeholders.selectShopType')"
                data-testid="rentable-area-edit-shop-type-select"
              >
                <el-option
                  v-for="shopType in shopTypes"
                  :key="shopType.id"
                  :label="`${shopType.code} — ${shopType.name}`"
                  :value="shopType.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('rentableAreaAdmin.fields.rentable')" class="rentable-area-admin-view__switch-field">
              <el-switch v-model="unitEdit.is_rentable" data-testid="rentable-area-unit-edit-rentable" />
            </el-form-item>
          </div>
        </el-form>
      </div>

      <template #footer>
        <el-button @click="editDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button
          type="primary"
          :disabled="!canSaveEdit"
          :loading="isSaving"
          data-testid="rentable-area-unit-edit-save"
          @click="handleSave"
        >
          {{ t('common.actions.save') }}
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<style scoped>
.rentable-area-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.rentable-area-admin-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.rentable-area-admin-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.rentable-area-admin-view__filter-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.rentable-area-admin-view__filter-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.rentable-area-admin-view__table {
  width: 100%;
}

.rentable-area-admin-view__dialog {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.rentable-area-admin-view__dialog-meta {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-3);
}

.rentable-area-admin-view__meta-block {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-2);
  padding: var(--mi-space-3);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-sm);
  background: rgba(255, 255, 255, 0.72);
}

.rentable-area-admin-view__meta-label {
  font-size: var(--mi-font-size-100);
  letter-spacing: var(--mi-letter-spacing-wide);
  text-transform: uppercase;
  color: var(--mi-color-muted);
}

.rentable-area-admin-view__dialog-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.rentable-area-admin-view__switch-field {
  align-self: end;
}

@media (max-width: 52rem) {
  .rentable-area-admin-view__card-header,
  .rentable-area-admin-view__filter-actions {
    align-items: flex-start;
    flex-direction: column;
  }

  .rentable-area-admin-view__filter-grid,
  .rentable-area-admin-view__dialog-meta,
  .rentable-area-admin-view__dialog-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
