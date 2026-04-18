<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  createCustomerTraffic,
  createDailySale,
  downloadCustomerTrafficTemplate,
  downloadDailySalesTemplate,
  importCustomerTrafficWorkbook,
  importDailySalesWorkbook,
  listCustomerTraffic,
  listDailySales,
  type CustomerTraffic,
  type DailySale,
  type SalesImportResult,
} from '../api/sales'
import { listStructureStores, listStructureUnits, type StructureStore, type StructureUnit } from '../api/structure'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { getErrorMessage } from '../composables/useErrorMessage'
import { useFilterForm } from '../composables/useFilterForm'

const { t } = useI18n()

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
const isDailyTemplateDownloading = ref(false)
const isTrafficTemplateDownloading = ref(false)
const isDailyImporting = ref(false)
const isTrafficImporting = ref(false)

const pageFeedback = ref<Feedback | null>(null)
const dailyFeedback = ref<Feedback | null>(null)
const trafficFeedback = ref<Feedback | null>(null)
const dailyImportFile = ref<File | null>(null)
const trafficImportFile = ref<File | null>(null)
const dailyImportResult = ref<SalesImportResult | null>(null)
const trafficImportResult = ref<SalesImportResult | null>(null)

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

const downloadBlob = (data: unknown, filename: string) => {
  const blob = new Blob([data as BlobPart], { type: 'application/octet-stream' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = filename
  link.click()
  URL.revokeObjectURL(url)
}

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
    loadErrors.push(getErrorMessage(storesResult.reason, t('salesAdmin.errors.unableToLoadStores')))
  }

  if (unitsResult.status === 'fulfilled') {
    units.value = unitsResult.value.data.units ?? []
  } else {
    units.value = []
    loadErrors.push(getErrorMessage(unitsResult.reason, t('salesAdmin.errors.unableToLoadUnits')))
  }

  syncMasterDataSelections()

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'warning',
      title: t('salesAdmin.errors.masterDataPartiallyUnavailable'),
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

const loadDailySales = async (options?: { preserveFeedback?: boolean }) => {
  isDailyLoading.value = true

  if (!options?.preserveFeedback) {
    dailyFeedback.value = null
  }

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
      title: t('salesAdmin.errors.dailySalesUnavailable'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToLoadDailySales')),
    }
  } finally {
    isDailyLoading.value = false
  }
}

const loadTraffic = async (options?: { preserveFeedback?: boolean }) => {
  isTrafficLoading.value = true

  if (!options?.preserveFeedback) {
    trafficFeedback.value = null
  }

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
      title: t('salesAdmin.errors.customerTrafficUnavailable'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToLoadTraffic')),
    }
  } finally {
    isTrafficLoading.value = false
  }
}

const handleDownloadDailyTemplate = async () => {
  isDailyTemplateDownloading.value = true
  dailyFeedback.value = null

  try {
    const response = await downloadDailySalesTemplate()
    downloadBlob(response.data, 'daily-sales-template.xlsx')
    dailyFeedback.value = {
      type: 'success',
      title: t('salesAdmin.feedback.dailySalesTemplateDownloadedTitle'),
      description: t('salesAdmin.feedback.dailySalesTemplateDownloadedDescription'),
    }
  } catch (error) {
    dailyFeedback.value = {
      type: 'error',
      title: t('salesAdmin.errors.dailySalesTemplateDownloadFailed'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToDownloadDailySalesTemplate')),
    }
  } finally {
    isDailyTemplateDownloading.value = false
  }
}

const handleDownloadTrafficTemplate = async () => {
  isTrafficTemplateDownloading.value = true
  trafficFeedback.value = null

  try {
    const response = await downloadCustomerTrafficTemplate()
    downloadBlob(response.data, 'customer-traffic-template.xlsx')
    trafficFeedback.value = {
      type: 'success',
      title: t('salesAdmin.feedback.customerTrafficTemplateDownloadedTitle'),
      description: t('salesAdmin.feedback.customerTrafficTemplateDownloadedDescription'),
    }
  } catch (error) {
    trafficFeedback.value = {
      type: 'error',
      title: t('salesAdmin.errors.customerTrafficTemplateDownloadFailed'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToDownloadCustomerTrafficTemplate')),
    }
  } finally {
    isTrafficTemplateDownloading.value = false
  }
}

const handleDailyImportFileChange = (file: File | null) => {
  dailyImportFile.value = file
}

const handleTrafficImportFileChange = (file: File | null) => {
  trafficImportFile.value = file
}

const handleDailyUploadChange = (file: { raw?: File }) => {
  handleDailyImportFileChange(file.raw ?? null)
}

const handleTrafficUploadChange = (file: { raw?: File }) => {
  handleTrafficImportFileChange(file.raw ?? null)
}

const handleImportDailySales = async () => {
  if (!dailyImportFile.value) {
    dailyFeedback.value = {
      type: 'warning',
      title: t('salesAdmin.feedback.workbookRequiredTitle'),
      description: t('salesAdmin.feedback.workbookRequiredDescription'),
    }
    return
  }

  isDailyImporting.value = true
  dailyFeedback.value = null
  dailyImportResult.value = null

  try {
    const response = await importDailySalesWorkbook(dailyImportFile.value)
    dailyImportResult.value = response.data

    if (response.status >= 400 || response.data.diagnostics.length > 0) {
      dailyFeedback.value = {
        type: 'warning',
        title: t('salesAdmin.feedback.dailySalesImportDiagnosticsTitle'),
        description: t('salesAdmin.feedback.dailySalesImportDiagnosticsDescription'),
      }
      return
    }

    await loadDailySales({ preserveFeedback: true })
    dailyFeedback.value = {
      type: 'success',
      title: t('salesAdmin.feedback.dailySalesImportCompletedTitle'),
      description: t('salesAdmin.feedback.dailySalesImportCompletedDescription', {
        count: response.data.imported_count,
      }),
    }
  } catch (error) {
    dailyImportResult.value = null
    dailyFeedback.value = {
      type: 'error',
      title: t('salesAdmin.errors.dailySalesImportFailed'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToImportDailySales')),
    }
  } finally {
    isDailyImporting.value = false
  }
}

const handleImportTraffic = async () => {
  if (!trafficImportFile.value) {
    trafficFeedback.value = {
      type: 'warning',
      title: t('salesAdmin.feedback.workbookRequiredTitle'),
      description: t('salesAdmin.feedback.workbookRequiredDescription'),
    }
    return
  }

  isTrafficImporting.value = true
  trafficFeedback.value = null
  trafficImportResult.value = null

  try {
    const response = await importCustomerTrafficWorkbook(trafficImportFile.value)
    trafficImportResult.value = response.data

    if (response.status >= 400 || response.data.diagnostics.length > 0) {
      trafficFeedback.value = {
        type: 'warning',
        title: t('salesAdmin.feedback.customerTrafficImportDiagnosticsTitle'),
        description: t('salesAdmin.feedback.customerTrafficImportDiagnosticsDescription'),
      }
      return
    }

    await loadTraffic({ preserveFeedback: true })
    trafficFeedback.value = {
      type: 'success',
      title: t('salesAdmin.feedback.customerTrafficImportCompletedTitle'),
      description: t('salesAdmin.feedback.customerTrafficImportCompletedDescription', {
        count: response.data.imported_count,
      }),
    }
  } catch (error) {
    trafficImportResult.value = null
    trafficFeedback.value = {
      type: 'error',
      title: t('salesAdmin.errors.customerTrafficImportFailed'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToImportCustomerTraffic')),
    }
  } finally {
    isTrafficImporting.value = false
  }
}

const handleCreateDailySale = async () => {
  if (!canCreateDailySale.value) {
    dailyFeedback.value = {
      type: 'warning',
      title: t('salesAdmin.feedback.dailySaleDetailsRequiredTitle'),
      description: t('salesAdmin.feedback.dailySaleDetailsRequiredDescription'),
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
      title: t('salesAdmin.feedback.dailySaleDetailsRequiredTitle'),
      description: t('salesAdmin.feedback.dailySaleDetailsRequiredDescription'),
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
      title: t('salesAdmin.feedback.dailySaleCreatedTitle'),
      description: t('salesAdmin.feedback.dailySaleCreatedDescription'),
    }
    resetDailyForm()
  } catch (error) {
    dailyFeedback.value = {
      type: 'error',
      title: t('salesAdmin.errors.dailySaleCreationFailed'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToCreateDailySale')),
    }
  } finally {
    isDailySaving.value = false
  }
}

const handleCreateTraffic = async () => {
  if (!canCreateTraffic.value) {
    trafficFeedback.value = {
      type: 'warning',
      title: t('salesAdmin.feedback.trafficDetailsRequiredTitle'),
      description: t('salesAdmin.feedback.trafficDetailsRequiredDescription'),
    }
    return
  }

  const storeId = trafficForm.store_id
  const inboundCount = trafficForm.inbound_count
  const trafficDate = trafficForm.traffic_date.trim()

  if (storeId === undefined || inboundCount === undefined || !trafficDate) {
    trafficFeedback.value = {
      type: 'warning',
      title: t('salesAdmin.feedback.trafficDetailsRequiredTitle'),
      description: t('salesAdmin.feedback.trafficDetailsRequiredDescription'),
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
      title: t('salesAdmin.feedback.trafficRecordCreatedTitle'),
      description: t('salesAdmin.feedback.trafficRecordCreatedDescription'),
    }
    resetTrafficForm()
  } catch (error) {
    trafficFeedback.value = {
      type: 'error',
      title: t('salesAdmin.errors.trafficCreationFailed'),
      description: getErrorMessage(error, t('salesAdmin.errors.unableToCreateTraffic')),
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
  <div
    class="sales-admin-view"
    v-loading="isMasterDataLoading || isDailyLoading || isTrafficLoading"
    data-testid="sales-admin-view"
  >
    <PageSection
      :eyebrow="t('salesAdmin.eyebrow')"
      :title="t('salesAdmin.title')"
      :summary="t('salesAdmin.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('salesAdmin.tags.dailySales', { count: dailySales.length }) }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('salesAdmin.tags.trafficRows', { count: customerTraffic.length }) }}</el-tag>
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
          <span>{{ t('salesAdmin.cards.dailySales') }}</span>
          <div class="sales-admin-view__card-actions">
            <el-button :loading="isDailyLoading" data-testid="sales-daily-refresh-button" @click="loadDailySales">
              {{ t('common.actions.refresh') }}
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
        data-testid="sales-daily-feedback"
        show-icon
      />

      <section class="sales-admin-view__import-panel" data-testid="sales-daily-import-panel">
        <div class="sales-admin-view__import-copy">
          <p class="sales-admin-view__import-label">{{ t('salesAdmin.imports.dailySalesTitle') }}</p>
          <p class="sales-admin-view__import-description">{{ t('salesAdmin.imports.dailySalesDescription') }}</p>
        </div>

        <div class="sales-admin-view__import-actions">
          <el-button
            :loading="isDailyTemplateDownloading"
            data-testid="sales-daily-download-template"
            @click="handleDownloadDailyTemplate"
          >
            {{ t('salesAdmin.actions.downloadTemplate') }}
          </el-button>

          <el-upload
            :auto-upload="false"
            :limit="1"
            accept=".xlsx,.xls"
            :show-file-list="false"
            :on-change="handleDailyUploadChange"
            data-testid="sales-daily-upload-input"
          >
            <template #trigger>
              <el-button>{{ t('salesAdmin.actions.selectWorkbook') }}</el-button>
            </template>
          </el-upload>

          <el-button
            type="primary"
            :loading="isDailyImporting"
            :disabled="!dailyImportFile"
            data-testid="sales-daily-import-button"
            @click="handleImportDailySales"
          >
            {{ t('salesAdmin.actions.importWorkbook') }}
          </el-button>
        </div>

        <p v-if="dailyImportFile" class="sales-admin-view__selected-file" data-testid="sales-daily-selected-file">
          {{ t('salesAdmin.feedback.selectedWorkbook', { name: dailyImportFile.name }) }}
        </p>
      </section>

      <div v-if="dailyImportResult" class="sales-admin-view__import-result" data-testid="sales-daily-import-result">
        <el-tag
          effect="plain"
          :type="dailyImportResult.diagnostics.length === 0 ? 'success' : 'warning'"
          data-testid="sales-daily-import-summary"
        >
          {{ t('salesAdmin.feedback.importedTag', { count: dailyImportResult.imported_count }) }}
        </el-tag>

        <el-table
          v-if="dailyImportResult.diagnostics.length > 0"
          :data="dailyImportResult.diagnostics"
          class="sales-admin-view__table"
          data-testid="sales-daily-diagnostics-table"
        >
          <el-table-column prop="row" :label="t('salesAdmin.columns.row')" min-width="96" />
          <el-table-column prop="field" :label="t('salesAdmin.columns.field')" min-width="160" />
          <el-table-column prop="message" :label="t('salesAdmin.columns.message')" min-width="320" />
        </el-table>
      </div>

      <FilterForm class="sales-admin-view__filter-form" :title="t('salesAdmin.filters.dailySales')" :show-actions="false">
        <el-form-item :label="t('salesAdmin.fields.store')">
          <el-select
            v-model="dailyFilters.store_id"
            clearable
            filterable
            :placeholder="t('salesAdmin.placeholders.allStores')"
            :loading="isMasterDataLoading"
            data-testid="sales-daily-filter-store"
          >
            <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="String(store.id)" />
          </el-select>
        </el-form-item>

        <el-form-item :label="t('salesAdmin.fields.unit')">
          <el-select
            v-model="dailyFilters.unit_id"
            clearable
            filterable
            :placeholder="t('salesAdmin.placeholders.allUnits')"
            :loading="isMasterDataLoading"
            data-testid="sales-daily-filter-unit"
          >
            <el-option v-for="unit in units" :key="unit.id" :label="buildUnitLabel(unit)" :value="String(unit.id)" />
          </el-select>
        </el-form-item>

        <el-form-item :label="t('salesAdmin.fields.dateFrom')">
          <div data-testid="sales-daily-filter-date-from">
            <el-date-picker
              v-model="dailyFilters.date_from"
              type="date"
              :placeholder="t('salesAdmin.placeholders.startDate')"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <el-form-item :label="t('salesAdmin.fields.dateTo')">
          <div data-testid="sales-daily-filter-date-to">
            <el-date-picker
              v-model="dailyFilters.date_to"
              type="date"
              :placeholder="t('salesAdmin.placeholders.endDate')"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <div class="sales-admin-view__filter-actions-row">
          <el-button :disabled="!isDailyFiltersDirty" data-testid="sales-daily-filter-reset" @click="handleResetDailyFilters">
            {{ t('common.actions.reset') }}
          </el-button>
          <el-button type="primary" :loading="isDailyLoading" data-testid="sales-daily-filter-submit" @click="loadDailySales">
            {{ t('common.actions.query') }}
          </el-button>
        </div>
      </FilterForm>

      <el-form label-position="top" class="sales-admin-view__form" @submit.prevent>
        <div class="sales-admin-view__form-grid">
          <el-form-item :label="t('salesAdmin.fields.store')">
            <el-select
              v-model="dailyForm.store_id"
              filterable
              :placeholder="t('salesAdmin.placeholders.selectStore')"
              :loading="isMasterDataLoading"
              data-testid="sales-daily-store-select"
            >
              <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="store.id" />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('salesAdmin.fields.unit')">
            <el-select
              v-model="dailyForm.unit_id"
              filterable
              :placeholder="t('salesAdmin.placeholders.selectUnit')"
              :loading="isMasterDataLoading"
              data-testid="sales-daily-unit-select"
            >
              <el-option v-for="unit in units" :key="unit.id" :label="buildUnitLabel(unit)" :value="unit.id" />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('salesAdmin.fields.saleDate')">
            <div data-testid="sales-daily-date-input">
              <el-date-picker
                v-model="dailyForm.sale_date"
                type="date"
                :placeholder="t('salesAdmin.placeholders.selectDate')"
                value-format="YYYY-MM-DD"
              />
            </div>
          </el-form-item>

          <el-form-item :label="t('salesAdmin.fields.salesAmount')">
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
            {{ t('salesAdmin.actions.createDailySale') }}
          </el-button>
        </div>
      </el-form>

      <el-table
        :data="dailySales"
        row-key="id"
        class="sales-admin-view__table"
        :empty-text="isDailyLoading ? t('salesAdmin.table.loadingDailySales') : t('salesAdmin.table.emptyDailySales')"
      >
        <el-table-column prop="store_id" :label="t('salesAdmin.fields.store')" min-width="100" />
        <el-table-column prop="unit_id" :label="t('salesAdmin.fields.unit')" min-width="100" />
        <el-table-column prop="sale_date" :label="t('salesAdmin.fields.saleDate')" min-width="140" />
        <el-table-column :label="t('salesAdmin.fields.salesAmount')" min-width="140">
          <template #default="scope">
            {{ String(scope.row.sales_amount) }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-card class="sales-admin-view__card" shadow="never">
      <template #header>
        <div class="sales-admin-view__card-header">
          <span>{{ t('salesAdmin.cards.customerTraffic') }}</span>
          <div class="sales-admin-view__card-actions">
            <el-button :loading="isTrafficLoading" data-testid="sales-traffic-refresh-button" @click="loadTraffic">
              {{ t('common.actions.refresh') }}
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
        data-testid="sales-traffic-feedback"
        show-icon
      />

      <section class="sales-admin-view__import-panel" data-testid="sales-traffic-import-panel">
        <div class="sales-admin-view__import-copy">
          <p class="sales-admin-view__import-label">{{ t('salesAdmin.imports.customerTrafficTitle') }}</p>
          <p class="sales-admin-view__import-description">{{ t('salesAdmin.imports.customerTrafficDescription') }}</p>
        </div>

        <div class="sales-admin-view__import-actions">
          <el-button
            :loading="isTrafficTemplateDownloading"
            data-testid="sales-traffic-download-template"
            @click="handleDownloadTrafficTemplate"
          >
            {{ t('salesAdmin.actions.downloadTemplate') }}
          </el-button>

          <el-upload
            :auto-upload="false"
            :limit="1"
            accept=".xlsx,.xls"
            :show-file-list="false"
            :on-change="handleTrafficUploadChange"
            data-testid="sales-traffic-upload-input"
          >
            <template #trigger>
              <el-button>{{ t('salesAdmin.actions.selectWorkbook') }}</el-button>
            </template>
          </el-upload>

          <el-button
            type="primary"
            :loading="isTrafficImporting"
            :disabled="!trafficImportFile"
            data-testid="sales-traffic-import-button"
            @click="handleImportTraffic"
          >
            {{ t('salesAdmin.actions.importWorkbook') }}
          </el-button>
        </div>

        <p v-if="trafficImportFile" class="sales-admin-view__selected-file" data-testid="sales-traffic-selected-file">
          {{ t('salesAdmin.feedback.selectedWorkbook', { name: trafficImportFile.name }) }}
        </p>
      </section>

      <div v-if="trafficImportResult" class="sales-admin-view__import-result" data-testid="sales-traffic-import-result">
        <el-tag
          effect="plain"
          :type="trafficImportResult.diagnostics.length === 0 ? 'success' : 'warning'"
          data-testid="sales-traffic-import-summary"
        >
          {{ t('salesAdmin.feedback.importedTag', { count: trafficImportResult.imported_count }) }}
        </el-tag>

        <el-table
          v-if="trafficImportResult.diagnostics.length > 0"
          :data="trafficImportResult.diagnostics"
          class="sales-admin-view__table"
          data-testid="sales-traffic-diagnostics-table"
        >
          <el-table-column prop="row" :label="t('salesAdmin.columns.row')" min-width="96" />
          <el-table-column prop="field" :label="t('salesAdmin.columns.field')" min-width="160" />
          <el-table-column prop="message" :label="t('salesAdmin.columns.message')" min-width="320" />
        </el-table>
      </div>

      <FilterForm class="sales-admin-view__filter-form" :title="t('salesAdmin.filters.customerTraffic')" :show-actions="false">
        <el-form-item :label="t('salesAdmin.fields.store')">
          <el-select
            v-model="trafficFilters.store_id"
            clearable
            filterable
            :placeholder="t('salesAdmin.placeholders.allStores')"
            :loading="isMasterDataLoading"
            data-testid="sales-traffic-filter-store"
          >
            <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="String(store.id)" />
          </el-select>
        </el-form-item>

        <el-form-item :label="t('salesAdmin.fields.dateFrom')">
          <div data-testid="sales-traffic-filter-date-from">
            <el-date-picker
              v-model="trafficFilters.date_from"
              type="date"
              :placeholder="t('salesAdmin.placeholders.startDate')"
              value-format="YYYY-MM-DD"
            />
          </div>
        </el-form-item>

        <el-form-item :label="t('salesAdmin.fields.dateTo')">
          <div data-testid="sales-traffic-filter-date-to">
            <el-date-picker
              v-model="trafficFilters.date_to"
              type="date"
              :placeholder="t('salesAdmin.placeholders.endDate')"
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
            {{ t('common.actions.reset') }}
          </el-button>
          <el-button type="primary" :loading="isTrafficLoading" data-testid="sales-traffic-filter-submit" @click="loadTraffic">
            {{ t('common.actions.query') }}
          </el-button>
        </div>
      </FilterForm>

      <el-form label-position="top" class="sales-admin-view__form" @submit.prevent>
        <div class="sales-admin-view__form-grid sales-admin-view__form-grid--traffic">
          <el-form-item :label="t('salesAdmin.fields.store')">
            <el-select
              v-model="trafficForm.store_id"
              filterable
              :placeholder="t('salesAdmin.placeholders.selectStore')"
              :loading="isMasterDataLoading"
              data-testid="sales-traffic-store-select"
            >
              <el-option v-for="store in stores" :key="store.id" :label="buildStoreLabel(store)" :value="store.id" />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('salesAdmin.fields.trafficDate')">
            <div data-testid="sales-traffic-date-input">
              <el-date-picker
                v-model="trafficForm.traffic_date"
                type="date"
                :placeholder="t('salesAdmin.placeholders.selectDate')"
                value-format="YYYY-MM-DD"
              />
            </div>
          </el-form-item>

          <el-form-item :label="t('salesAdmin.fields.inboundCount')">
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
            {{ t('salesAdmin.actions.createTrafficRecord') }}
          </el-button>
        </div>
      </el-form>

      <el-table
        :data="customerTraffic"
        row-key="id"
        class="sales-admin-view__table"
        :empty-text="isTrafficLoading ? t('salesAdmin.table.loadingTraffic') : t('salesAdmin.table.emptyTraffic')"
      >
        <el-table-column prop="store_id" :label="t('salesAdmin.fields.store')" min-width="100" />
        <el-table-column prop="traffic_date" :label="t('salesAdmin.fields.trafficDate')" min-width="140" />
        <el-table-column :label="t('salesAdmin.fields.inboundCount')" min-width="140">
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

.sales-admin-view__filter-form,
.sales-admin-view__import-result {
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

.sales-admin-view__card-actions,
.sales-admin-view__import-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.sales-admin-view__feedback {
  margin-bottom: var(--mi-space-4);
}

.sales-admin-view__import-panel,
.sales-admin-view__import-result {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.sales-admin-view__import-panel {
  margin-bottom: var(--mi-space-4);
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-md);
  background: var(--mi-color-surface);
  box-shadow: var(--mi-shadow-sm);
}

.sales-admin-view__import-copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-2);
}

.sales-admin-view__import-label,
.sales-admin-view__selected-file {
  margin: 0;
}

.sales-admin-view__import-label {
  font-size: var(--mi-font-size-200);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.sales-admin-view__import-description,
.sales-admin-view__selected-file {
  font-size: var(--mi-font-size-100);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
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

.sales-admin-view__form-grid--traffic {
  grid-template-columns: repeat(3, minmax(0, 1fr));
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
.sales-admin-view :deep(.el-upload),
.sales-admin-view__form-grid :deep(.el-input-number),
.sales-admin-view :deep(.el-date-editor) {
  width: 100%;
}

@media (max-width: 52rem) {
  .sales-admin-view__form-grid,
  .sales-admin-view__form-grid--traffic {
    grid-template-columns: minmax(0, 1fr);
  }

  .sales-admin-view__card-header,
  .sales-admin-view__filter-actions-row,
  .sales-admin-view__form-actions,
  .sales-admin-view__import-actions {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
