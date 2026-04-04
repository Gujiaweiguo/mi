<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'

import {
  createCustomerTraffic,
  createDailySale,
  listCustomerTraffic,
  listDailySales,
  type CustomerTraffic,
  type DailySale,
} from '../api/sales'
import { listStructureStores, listStructureUnits, type StructureStore, type StructureUnit } from '../api/structure'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type DailySaleCreateForm = {
  store_id: number | undefined
  unit_id: number | undefined
  sale_date: string
  sales_amount: number | undefined
}

type TrafficCreateForm = {
  store_id: number | undefined
  traffic_date: string
  inbound_count: number | undefined
}

const dailySales = ref<DailySale[]>([])
const customerTraffic = ref<CustomerTraffic[]>([])
const stores = ref<StructureStore[]>([])
const units = ref<StructureUnit[]>([])

const isMasterDataLoading = ref(false)
const isDailyLoading = ref(false)
const isTrafficLoading = ref(false)
const isDailySaving = ref(false)
const isTrafficSaving = ref(false)

const pageFeedback = ref<Feedback | null>(null)
const dailyFeedback = ref<Feedback | null>(null)
const trafficFeedback = ref<Feedback | null>(null)

const { filters: dailyFilters, isDirty: isDailyFiltersDirty, reset: resetDailyFilterState } = useFilterForm({
  store_id: '',
  unit_id: '',
  date_from: '',
  date_to: '',
})

const { filters: trafficFilters, isDirty: isTrafficFiltersDirty, reset: resetTrafficFilterState } = useFilterForm({
  store_id: '',
  date_from: '',
  date_to: '',
})

const isPositiveInteger = (value: number | undefined): value is number =>
  typeof value === 'number' && Number.isInteger(value) && value > 0

const dailyForm = reactive<DailySaleCreateForm>({
  store_id: undefined,
  unit_id: undefined,
  sale_date: '',
  sales_amount: undefined,
})

const trafficForm = reactive<TrafficCreateForm>({
  store_id: undefined,
  traffic_date: '',
  inbound_count: undefined,
})

const getErrorMessage = (error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)

const normalizeFilterValue = (value: string | null | undefined) => value?.trim() ?? ''

const toOptionalNumber = (value: string | null | undefined) => {
  const trimmed = normalizeFilterValue(value)
  if (!trimmed) {
    return undefined
  }

  const parsed = Number(trimmed)
  return Number.isInteger(parsed) && parsed > 0 ? parsed : undefined
}

const matchesDateRange = (value: string, dateFrom?: string, dateTo?: string) => {
  if (dateFrom && value < dateFrom) {
    return false
  }

  if (dateTo && value > dateTo) {
    return false
  }

  return true
}

const buildStoreLabel = (store: StructureStore) => `${store.code} — ${store.name}`
const buildUnitLabel = (unit: StructureUnit) => `${unit.code} (#${unit.id})`

const matchesDailyFilters = (record: DailySale) => {
  const storeId = toOptionalNumber(dailyFilters.store_id)
  if (storeId !== undefined && record.store_id !== storeId) {
    return false
  }

  const unitId = toOptionalNumber(dailyFilters.unit_id)
  if (unitId !== undefined && record.unit_id !== unitId) {
    return false
  }

  return matchesDateRange(
    record.sale_date,
    normalizeFilterValue(dailyFilters.date_from) || undefined,
    normalizeFilterValue(dailyFilters.date_to) || undefined,
  )
}

const matchesTrafficFilters = (record: CustomerTraffic) => {
  const storeId = toOptionalNumber(trafficFilters.store_id)
  if (storeId !== undefined && record.store_id !== storeId) {
    return false
  }

  return matchesDateRange(
    record.traffic_date,
    normalizeFilterValue(trafficFilters.date_from) || undefined,
    normalizeFilterValue(trafficFilters.date_to) || undefined,
  )
}

const syncMasterDataSelections = () => {
  const storeIds = new Set(stores.value.map((store) => store.id))
  if (isPositiveInteger(dailyForm.store_id) && !storeIds.has(dailyForm.store_id)) {
    dailyForm.store_id = undefined
  }
  if (isPositiveInteger(trafficForm.store_id) && !storeIds.has(trafficForm.store_id)) {
    trafficForm.store_id = undefined
  }
  if (dailyFilters.store_id && !storeIds.has(toOptionalNumber(dailyFilters.store_id) ?? -1)) {
    dailyFilters.store_id = ''
  }
  if (trafficFilters.store_id && !storeIds.has(toOptionalNumber(trafficFilters.store_id) ?? -1)) {
    trafficFilters.store_id = ''
  }

  const unitIds = new Set(units.value.map((unit) => unit.id))
  if (isPositiveInteger(dailyForm.unit_id) && !unitIds.has(dailyForm.unit_id)) {
    dailyForm.unit_id = undefined
  }
  if (dailyFilters.unit_id && !unitIds.has(toOptionalNumber(dailyFilters.unit_id) ?? -1)) {
    dailyFilters.unit_id = ''
  }
}

const loadMasterData = async () => {
  isMasterDataLoading.value = true
  pageFeedback.value = null

  const [storesResult, unitsResult] = await Promise.allSettled([listStructureStores(), listStructureUnits()])
  const loadErrors: string[] = []

  if (storesResult.status === 'fulfilled') {
    stores.value = storesResult.value.data.stores ?? []
  } else {
    stores.value = []
    loadErrors.push(getErrorMessage(storesResult.reason, 'Unable to load stores.'))
  }

  if (unitsResult.status === 'fulfilled') {
    units.value = unitsResult.value.data.units ?? []
  } else {
    units.value = []
    loadErrors.push(getErrorMessage(unitsResult.reason, 'Unable to load units.'))
  }

  syncMasterDataSelections()

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'warning',
      title: 'Sales master data partially unavailable',
      description: loadErrors.join(' '),
    }
  }

  isMasterDataLoading.value = false
}

const resetDailyForm = () => {
  dailyForm.store_id = undefined
  dailyForm.unit_id = undefined
  dailyForm.sale_date = ''
  dailyForm.sales_amount = undefined
}

const resetTrafficForm = () => {
  trafficForm.store_id = undefined
  trafficForm.traffic_date = ''
  trafficForm.inbound_count = undefined
}

const canCreateDailySale = computed(() => {
  return (
    typeof dailyForm.store_id === 'number' &&
    dailyForm.store_id > 0 &&
    typeof dailyForm.unit_id === 'number' &&
    dailyForm.unit_id > 0 &&
    Boolean(dailyForm.sale_date.trim()) &&
    typeof dailyForm.sales_amount === 'number' &&
    Number.isFinite(dailyForm.sales_amount)
  )
})

const canCreateTraffic = computed(() => {
  return (
    typeof trafficForm.store_id === 'number' &&
    trafficForm.store_id > 0 &&
    Boolean(trafficForm.traffic_date.trim()) &&
    typeof trafficForm.inbound_count === 'number' &&
    Number.isFinite(trafficForm.inbound_count)
  )
})

const handleResetDailyFilters = () => {
  resetDailyFilterState()
  void loadDailySales()
}

const handleResetTrafficFilters = () => {
  resetTrafficFilterState()
  void loadTraffic()
}

const loadDailySales = async () => {
  isDailyLoading.value = true
  dailyFeedback.value = null

  try {
    const response = await listDailySales({
      store_id: toOptionalNumber(dailyFilters.store_id),
      unit_id: toOptionalNumber(dailyFilters.unit_id),
      date_from: normalizeFilterValue(dailyFilters.date_from) || undefined,
      date_to: normalizeFilterValue(dailyFilters.date_to) || undefined,
    })
    dailySales.value = response.data.daily_sales ?? []
  } catch (error) {
    dailySales.value = []
    dailyFeedback.value = {
      type: 'error',
      title: 'Daily sales unavailable',
      description: getErrorMessage(error, 'Unable to load daily sales.'),
    }
  } finally {
    isDailyLoading.value = false
  }
}

const loadTraffic = async () => {
  isTrafficLoading.value = true
  trafficFeedback.value = null

  try {
    const response = await listCustomerTraffic({
      store_id: toOptionalNumber(trafficFilters.store_id),
      date_from: normalizeFilterValue(trafficFilters.date_from) || undefined,
      date_to: normalizeFilterValue(trafficFilters.date_to) || undefined,
    })
    customerTraffic.value = response.data.customer_traffic ?? []
  } catch (error) {
    customerTraffic.value = []
    trafficFeedback.value = {
      type: 'error',
      title: 'Customer traffic unavailable',
      description: getErrorMessage(error, 'Unable to load customer traffic.'),
    }
  } finally {
    isTrafficLoading.value = false
  }
}

const handleCreateDailySale = async () => {
  if (!canCreateDailySale.value) {
    dailyFeedback.value = {
      type: 'warning',
      title: 'Daily sale details required',
      description: 'Provide store, unit, sale date, and sales amount before creating a record.',
    }
    return
  }

  const storeId = dailyForm.store_id
  const unitId = dailyForm.unit_id
  const salesAmount = dailyForm.sales_amount
  const saleDate = dailyForm.sale_date.trim()

  if (storeId === undefined || unitId === undefined || salesAmount === undefined || !saleDate) {
    dailyFeedback.value = {
      type: 'warning',
      title: 'Daily sale details required',
      description: 'Provide store, unit, sale date, and sales amount before creating a record.',
    }
    return
  }

  isDailySaving.value = true
  dailyFeedback.value = null

  try {
    const response = await createDailySale({
      store_id: storeId,
      unit_id: unitId,
      sale_date: saleDate,
      sales_amount: salesAmount,
    })

    if (matchesDailyFilters(response.data.daily_sale)) {
      dailySales.value = [response.data.daily_sale, ...dailySales.value]
    }
    dailyFeedback.value = {
      type: 'success',
      title: 'Daily sale created',
      description: 'The daily sale record is now available for reporting and review.',
    }
    resetDailyForm()
  } catch (error) {
    dailyFeedback.value = {
      type: 'error',
      title: 'Daily sale creation failed',
      description: getErrorMessage(error, 'Unable to create the daily sale.'),
    }
  } finally {
    isDailySaving.value = false
  }
}

const handleCreateTraffic = async () => {
  if (!canCreateTraffic.value) {
    trafficFeedback.value = {
      type: 'warning',
      title: 'Traffic details required',
      description: 'Provide store, traffic date, and inbound count before creating a record.',
    }
    return
  }

  const storeId = trafficForm.store_id
  const inboundCount = trafficForm.inbound_count
  const trafficDate = trafficForm.traffic_date.trim()

  if (storeId === undefined || inboundCount === undefined || !trafficDate) {
    trafficFeedback.value = {
      type: 'warning',
      title: 'Traffic details required',
      description: 'Provide store, traffic date, and inbound count before creating a record.',
    }
    return
  }

  isTrafficSaving.value = true
  trafficFeedback.value = null

  try {
    const response = await createCustomerTraffic({
      store_id: storeId,
      traffic_date: trafficDate,
      inbound_count: inboundCount,
    })

    if (matchesTrafficFilters(response.data.traffic)) {
      customerTraffic.value = [response.data.traffic, ...customerTraffic.value]
    }
    trafficFeedback.value = {
      type: 'success',
      title: 'Traffic record created',
      description: 'The customer traffic record is now available for reporting and review.',
    }
    resetTrafficForm()
  } catch (error) {
    trafficFeedback.value = {
      type: 'error',
      title: 'Traffic creation failed',
      description: getErrorMessage(error, 'Unable to create the traffic record.'),
    }
  } finally {
    isTrafficSaving.value = false
  }
}

onMounted(() => {
  void loadMasterData()
  void loadDailySales()
  void loadTraffic()
})
</script>

<template>
  <div class="sales-admin-view" data-testid="sales-admin-view">
    <PageSection
      eyebrow="Sales data maintenance"
      title="Sales data admin"
      summary="Capture daily sales and customer traffic records for reporting slices, using a minimal admin console guarded by the sales.admin permission."
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ dailySales.length }} daily sales</el-tag>
        <el-tag effect="plain" type="success">{{ customerTraffic.length }} traffic rows</el-tag>
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

    <el-card class="sales-admin-view__card" shadow="never">
      <template #header>
        <div class="sales-admin-view__card-header">
          <span>Daily sales</span>
          <div class="sales-admin-view__card-actions">
            <el-button
              :loading="isDailyLoading"
              data-testid="sales-daily-refresh-button"
              @click="loadDailySales"
            >
              Refresh
            </el-button>
          </div>
        </div>
      </template>

      <el-alert
        v-if="dailyFeedback"
        :closable="false"
        class="sales-admin-view__feedback"
        :title="dailyFeedback.title"
        :type="dailyFeedback.type"
        :description="dailyFeedback.description"
        show-icon
      />

      <FilterForm class="sales-admin-view__filter-form" title="Daily sales filters" :show-actions="false">
        <el-form-item label="Store">
          <el-select
            v-model="dailyFilters.store_id"
            clearable
            filterable
            placeholder="All stores"
            :loading="isMasterDataLoading"
            data-testid="sales-daily-filter-store"
          >
            <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="String(store.id)" />
          </el-select>
        </el-form-item>

        <el-form-item label="Unit">
          <el-select
            v-model="dailyFilters.unit_id"
            clearable
            filterable
            placeholder="All units"
            :loading="isMasterDataLoading"
            data-testid="sales-daily-filter-unit"
          >
            <el-option v-for="unit in units" :key="unit.id" :label="buildUnitLabel(unit)" :value="String(unit.id)" />
          </el-select>
        </el-form-item>

        <el-form-item label="Date from">
          <div data-testid="sales-daily-filter-date-from">
            <el-date-picker
              v-model="dailyFilters.date_from"
              type="date"
              placeholder="Start date"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <el-form-item label="Date to">
          <div data-testid="sales-daily-filter-date-to">
            <el-date-picker
              v-model="dailyFilters.date_to"
              type="date"
              placeholder="End date"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <div class="sales-admin-view__filter-actions-row">
          <el-button :disabled="!isDailyFiltersDirty" data-testid="sales-daily-filter-reset" @click="handleResetDailyFilters">
            Reset
          </el-button>
          <el-button
            type="primary"
            :loading="isDailyLoading"
            data-testid="sales-daily-filter-submit"
            @click="loadDailySales"
          >
            Apply filters
          </el-button>
        </div>
      </FilterForm>

      <el-form label-position="top" class="sales-admin-view__form" @submit.prevent>
        <div class="sales-admin-view__form-grid">
          <el-form-item label="Store">
            <el-select
              v-model="dailyForm.store_id"
              filterable
              placeholder="Select a store"
              :loading="isMasterDataLoading"
              data-testid="sales-daily-store-select"
            >
              <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="store.id" />
            </el-select>
          </el-form-item>

          <el-form-item label="Unit">
            <el-select
              v-model="dailyForm.unit_id"
              filterable
              placeholder="Select a unit"
              :loading="isMasterDataLoading"
              data-testid="sales-daily-unit-select"
            >
              <el-option v-for="unit in units" :key="unit.id" :label="buildUnitLabel(unit)" :value="unit.id" />
            </el-select>
          </el-form-item>

          <el-form-item label="Sale date">
            <div data-testid="sales-daily-date-input">
              <el-date-picker
                v-model="dailyForm.sale_date"
                type="date"
                placeholder="Select a date"
                value-format="YYYY-MM-DD"
              />
            </div>
          </el-form-item>

          <el-form-item label="Sales amount">
            <el-input-number
              v-model="dailyForm.sales_amount"
              :min="0"
              :precision="2"
              :step="1"
              controls-position="right"
              data-testid="sales-daily-amount-input"
            />
          </el-form-item>
        </div>

        <div class="sales-admin-view__form-actions">
          <el-button
            type="primary"
            :loading="isDailySaving"
            :disabled="!canCreateDailySale"
            data-testid="sales-daily-create-button"
            @click="handleCreateDailySale"
          >
            Create daily sale
          </el-button>
        </div>
      </el-form>

      <el-table
        :data="dailySales"
        row-key="id"
        class="sales-admin-view__table"
        :empty-text="isDailyLoading ? 'Loading daily sales…' : 'No daily sales available.'"
      >
        <el-table-column prop="store_id" label="Store" min-width="100" />
        <el-table-column prop="unit_id" label="Unit" min-width="100" />
        <el-table-column prop="sale_date" label="Sale date" min-width="140" />
        <el-table-column label="Sales amount" min-width="140">
          <template #default="scope">
            {{ String(scope.row.sales_amount) }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-card class="sales-admin-view__card" shadow="never">
      <template #header>
        <div class="sales-admin-view__card-header">
          <span>Customer traffic</span>
          <div class="sales-admin-view__card-actions">
              <el-button
                :loading="isTrafficLoading"
                data-testid="sales-traffic-refresh-button"
                @click="loadTraffic"
              >
                Refresh
              </el-button>
          </div>
        </div>
      </template>

      <el-alert
        v-if="trafficFeedback"
        :closable="false"
        class="sales-admin-view__feedback"
        :title="trafficFeedback.title"
        :type="trafficFeedback.type"
        :description="trafficFeedback.description"
        show-icon
      />

      <FilterForm class="sales-admin-view__filter-form" title="Traffic filters" :show-actions="false">
        <el-form-item label="Store">
          <el-select
            v-model="trafficFilters.store_id"
            clearable
            filterable
            placeholder="All stores"
            :loading="isMasterDataLoading"
            data-testid="sales-traffic-filter-store"
          >
            <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="String(store.id)" />
          </el-select>
        </el-form-item>

        <el-form-item label="Date from">
          <div data-testid="sales-traffic-filter-date-from">
            <el-date-picker
              v-model="trafficFilters.date_from"
              type="date"
              placeholder="Start date"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <el-form-item label="Date to">
          <div data-testid="sales-traffic-filter-date-to">
            <el-date-picker
              v-model="trafficFilters.date_to"
              type="date"
              placeholder="End date"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <div class="sales-admin-view__filter-actions-row">
          <el-button
            :disabled="!isTrafficFiltersDirty"
            data-testid="sales-traffic-filter-reset"
            @click="handleResetTrafficFilters"
          >
            Reset
          </el-button>
          <el-button
            type="primary"
            :loading="isTrafficLoading"
            data-testid="sales-traffic-filter-submit"
            @click="loadTraffic"
          >
            Apply filters
          </el-button>
        </div>
      </FilterForm>

      <el-form label-position="top" class="sales-admin-view__form" @submit.prevent>
        <div class="sales-admin-view__form-grid">
          <el-form-item label="Store">
            <el-select
              v-model="trafficForm.store_id"
              filterable
              placeholder="Select a store"
              :loading="isMasterDataLoading"
              data-testid="sales-traffic-store-select"
            >
              <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="store.id" />
            </el-select>
          </el-form-item>

          <el-form-item label="Traffic date">
            <div data-testid="sales-traffic-date-input">
              <el-date-picker
                v-model="trafficForm.traffic_date"
                type="date"
                placeholder="Select a date"
                value-format="YYYY-MM-DD"
              />
            </div>
          </el-form-item>

          <el-form-item label="Inbound count">
            <el-input-number
              v-model="trafficForm.inbound_count"
              :min="0"
              :step="1"
              controls-position="right"
              data-testid="sales-traffic-count-input"
            />
          </el-form-item>
        </div>

        <div class="sales-admin-view__form-actions">
          <el-button
            type="primary"
            :loading="isTrafficSaving"
            :disabled="!canCreateTraffic"
            data-testid="sales-traffic-create-button"
            @click="handleCreateTraffic"
          >
            Create traffic record
          </el-button>
        </div>
      </el-form>

      <el-table
        :data="customerTraffic"
        row-key="id"
        class="sales-admin-view__table"
        :empty-text="isTrafficLoading ? 'Loading traffic…' : 'No customer traffic available.'"
      >
        <el-table-column prop="store_id" label="Store" min-width="100" />
        <el-table-column prop="traffic_date" label="Traffic date" min-width="140" />
        <el-table-column label="Inbound count" min-width="140">
          <template #default="scope">
            {{ String(scope.row.inbound_count) }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.sales-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.sales-admin-view__filter-form {
  margin-bottom: var(--mi-space-4);
}

.sales-admin-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.sales-admin-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.sales-admin-view__card-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.sales-admin-view__feedback {
  margin-bottom: var(--mi-space-4);
}

.sales-admin-view__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  margin-bottom: var(--mi-space-4);
}

.sales-admin-view__form-grid {
  display: grid;
  grid-template-columns: repeat(4, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.sales-admin-view__form-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.sales-admin-view__filter-actions-row {
  grid-column: 1 / -1;
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.sales-admin-view__table,
.sales-admin-view :deep(.el-select),
.sales-admin-view__form-grid :deep(.el-input-number),
.sales-admin-view :deep(.el-date-editor) {
  width: 100%;
}

@media (max-width: 52rem) {
  .sales-admin-view__form-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .sales-admin-view__card-header,
  .sales-admin-view__filter-actions-row,
  .sales-admin-view__form-actions {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
