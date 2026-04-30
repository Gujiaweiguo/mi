<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { exportReport, queryReport, type ReportId, type ReportQueryPayload, type ReportQueryResponse } from '../api/reports'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { downloadBlob } from '../composables/useDownload'
import { useFilterForm } from '../composables/useFilterForm'
import { getErrorMessage } from '../composables/useErrorMessage'
import { useAppStore } from '../stores/app'

type GeneralizeReportId = Exclude<ReportId, 'r19'>

type ReportOption = {
  value: GeneralizeReportId
  label: string
}

type ReportFilters = {
  report_id: GeneralizeReportId
  period: string
  year: string
  store_id: string
  floor_id: string
  unit_id: string
  shop_type_id: string
  brand_id: string
  charge_type: string
  status: string
  department_id: string
  customer_id: string
  trade_id: string
  management_type_id: string
}

const appStore = useAppStore()
const { t } = useI18n()

const reportIds: GeneralizeReportId[] = [
  'r01',
  'r02',
  'r03',
  'r04',
  'r05',
  'r06',
  'r07',
  'r08',
  'r09',
  'r10',
  'r11',
  'r12',
  'r13',
  'r14',
  'r15',
  'r16',
  'r17',
  'r18',
]

const reportOptions = computed<ReportOption[]>(() =>
  reportIds.map((reportId) => ({
    value: reportId,
    label: t(`generalizeReports.reportOptions.${reportId}`),
  })),
)

const reportSummaries = computed<Record<GeneralizeReportId, string>>(() => ({
  r01: t('generalizeReports.reportSummaries.r01'),
  r02: t('generalizeReports.reportSummaries.r02'),
  r03: t('generalizeReports.reportSummaries.r03'),
  r04: t('generalizeReports.reportSummaries.r04'),
  r05: t('generalizeReports.reportSummaries.r05'),
  r06: t('generalizeReports.reportSummaries.r06'),
  r07: t('generalizeReports.reportSummaries.r07'),
  r08: t('generalizeReports.reportSummaries.r08'),
  r09: t('generalizeReports.reportSummaries.r09'),
  r10: t('generalizeReports.reportSummaries.r10'),
  r11: t('generalizeReports.reportSummaries.r11'),
  r12: t('generalizeReports.reportSummaries.r12'),
  r13: t('generalizeReports.reportSummaries.r13'),
  r14: t('generalizeReports.reportSummaries.r14'),
  r15: t('generalizeReports.reportSummaries.r15'),
  r16: t('generalizeReports.reportSummaries.r16'),
  r17: t('generalizeReports.reportSummaries.r17'),
  r18: t('generalizeReports.reportSummaries.r18'),
}))

const statusOptions = computed(() => [
  { label: t('common.statuses.draft'), value: 'draft' },
  { label: t('common.statuses.pendingApproval'), value: 'pending_approval' },
  { label: t('common.statuses.active'), value: 'active' },
  { label: t('common.statuses.rejected'), value: 'rejected' },
  { label: t('common.statuses.terminated'), value: 'terminated' },
])

const result = ref<ReportQueryResponse | null>(null)
const errorMessage = ref('')
const isQuerying = ref(false)
const isExporting = ref(false)
const defaultPeriod = new Date().toISOString().slice(0, 7)
const defaultYear = new Date().getFullYear().toString()

const { filters, isDirty, reset } = useFilterForm<ReportFilters>({
  report_id: 'r01',
  period: defaultPeriod,
  year: defaultYear,
  store_id: '',
  floor_id: '',
  unit_id: '',
  shop_type_id: '',
  brand_id: '',
  charge_type: '',
  status: '',
  department_id: '',
  customer_id: '',
  trade_id: '',
  management_type_id: '',
})

const isR02Report = computed(() => filters.report_id === 'r02')
const isYearReport = computed(() => ['r05', 'r07', 'r10'].includes(filters.report_id))
const needsShopTypeFilter = computed(() => ['r03', 'r13', 'r14', 'r15'].includes(filters.report_id))
const needsBrandFilter = computed(() => ['r07', 'r18'].includes(filters.report_id))
const needsFloorFilter = computed(() => filters.report_id === 'r05')
const needsUnitFilter = computed(() => ['r05', 'r18'].includes(filters.report_id))
const needsDepartmentFilter = computed(() => ['r02', 'r08', 'r09', 'r16', 'r17'].includes(filters.report_id))
const needsCustomerFilter = computed(() => ['r02', 'r08', 'r09', 'r18'].includes(filters.report_id))
const needsTradeFilter = computed(() => ['r02', 'r08', 'r09'].includes(filters.report_id))
const needsChargeTypeFilter = computed(() => ['r09', 'r17'].includes(filters.report_id))

const selectedReportLabel = computed(
  () => reportOptions.value.find((option) => option.value === filters.report_id)?.label ?? reportOptions.value[0]?.label ?? '',
)

const selectedReportSummary = computed(() => reportSummaries.value[filters.report_id])

const generatedAtLabel = computed(() => {
  if (!result.value?.generated_at) {
    return ''
  }

  const value = new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(result.value.generated_at))

  return t('generalizeReports.meta.generatedAt', { value })
})

const parsePositiveInteger = (value: string) => {
  const normalized = value.trim()

  if (!normalized) {
    return undefined
  }

  const parsedValue = Number(normalized)
  if (Number.isInteger(parsedValue) && parsedValue > 0) {
    return parsedValue
  }

  return undefined
}

const parseYear = (value: string) => {
  const normalized = value.trim()

  if (!normalized) {
    return undefined
  }

  if (/^\d{4}$/.test(normalized)) {
    return normalized
  }

  return undefined
}

const resolvePeriod = () => {
  if (isYearReport.value) {
    const year = parseYear(filters.year)
    return year ? `${year}-01` : ''
  }

  return filters.period.trim()
}

const buildPayload = (): ReportQueryPayload => {
  const payload: ReportQueryPayload = {}
  const period = resolvePeriod()
  const storeId = parsePositiveInteger(filters.store_id)

  if (period) {
    payload.period = period
  }

  if (storeId !== undefined) {
    payload.store_id = storeId
  }

  if (needsFloorFilter.value) {
    const floorId = parsePositiveInteger(filters.floor_id)

    if (floorId !== undefined) {
      payload.floor_id = floorId
    }
  }

  if (needsUnitFilter.value) {
    const unitId = parsePositiveInteger(filters.unit_id)

    if (unitId !== undefined) {
      payload.unit_id = unitId
    }
  }

  if (needsShopTypeFilter.value) {
    const shopTypeId = parsePositiveInteger(filters.shop_type_id)

    if (shopTypeId !== undefined) {
      payload.shop_type_id = shopTypeId
    }
  }

  if (needsBrandFilter.value) {
    const brandId = parsePositiveInteger(filters.brand_id)

    if (brandId !== undefined) {
      payload.brand_id = brandId
    }
  }

  if (needsChargeTypeFilter.value) {
    const chargeType = filters.charge_type.trim()

    if (chargeType) {
      payload.charge_type = chargeType
    }
  }

  if (needsDepartmentFilter.value) {
    const departmentId = parsePositiveInteger(filters.department_id)

    if (departmentId !== undefined) {
      payload.department_id = departmentId
    }
  }

  if (needsCustomerFilter.value) {
    const customerId = parsePositiveInteger(filters.customer_id)

    if (customerId !== undefined) {
      payload.customer_id = customerId
    }
  }

  if (needsTradeFilter.value) {
    const tradeId = parsePositiveInteger(filters.trade_id)

    if (tradeId !== undefined) {
      payload.trade_id = tradeId
    }
  }

  if (isR02Report.value) {
    const status = filters.status.trim()
    const managementTypeId = parsePositiveInteger(filters.management_type_id)

    if (status) {
      payload.status = status
    }

    if (managementTypeId !== undefined) {
      payload.management_type_id = managementTypeId
    }
  }

  return payload
}

const loadReport = async () => {
  isQuerying.value = true
  errorMessage.value = ''

  try {
    const response = await queryReport(filters.report_id, buildPayload())
    result.value = response.data
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('generalizeReports.errors.unableToLoad'))
    result.value = null
  } finally {
    isQuerying.value = false
  }
}

const handleReset = () => {
  reset()
  errorMessage.value = ''
  void loadReport()
}

const inferFileExtension = (contentType: string | undefined) => {
  if (!contentType) {
    return 'xlsx'
  }

  const normalized = contentType.toLowerCase()

  if (normalized.includes('csv')) {
    return 'csv'
  }

  if (normalized.includes('pdf')) {
    return 'pdf'
  }

  if (normalized.includes('json')) {
    return 'json'
  }

  if (normalized.includes('excel') || normalized.includes('spreadsheetml')) {
    return 'xlsx'
  }

  return 'xlsx'
}

const buildFileName = () => {
  const periodLabel = resolvePeriod() || 'all-periods'
  const storeLabel = filters.store_id.trim() || 'all-stores'
  const stamp = new Date().toISOString().slice(0, 10)

  return `generalize-${filters.report_id}-${periodLabel}-${storeLabel}-${stamp}`
}

const handleExport = async () => {
  isExporting.value = true
  errorMessage.value = ''

  try {
    const response = await exportReport(filters.report_id, buildPayload())
    const blob = response.data instanceof Blob ? response.data : new Blob([response.data], { type: 'application/octet-stream' })
    const rawContentType = response.headers['content-type']
    const contentType = typeof rawContentType === 'string' ? rawContentType : Array.isArray(rawContentType) ? rawContentType[0] : undefined
    const extension = inferFileExtension(contentType)
    downloadBlob(blob, `${buildFileName()}.${extension}`)
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('generalizeReports.errors.unableToExport'))
  } finally {
    isExporting.value = false
  }
}

onMounted(() => {
  void loadReport()
})
</script>

<template>
  <div class="generalize-reports-view" v-loading="isQuerying || isExporting" data-testid="generalize-reports-view">
    <PageSection
      :eyebrow="t('generalizeReports.eyebrow')"
      :title="t('generalizeReports.title')"
      :summary="t('generalizeReports.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('generalizeReports.tags.batch') }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('generalizeReports.tags.coverage') }}</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="generalize-reports-view__alert"
      :title="t('generalizeReports.errors.requestFailed')"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <FilterForm :title="t('generalizeReports.filters.title')" :show-actions="false">
      <el-form-item :label="t('generalizeReports.fields.report')">
        <el-select
          v-model="filters.report_id"
          :placeholder="t('generalizeReports.placeholders.selectReport')"
          data-testid="generalize-report-select"
        >
          <el-option v-for="option in reportOptions" :key="option.value" :label="option.label" :value="option.value" />
        </el-select>
      </el-form-item>

      <el-form-item v-if="!isYearReport" :label="t('generalizeReports.fields.period')">
        <el-input
          v-model="filters.period"
          :placeholder="t('generalizeReports.placeholders.enterPeriod')"
          clearable
          data-testid="generalize-period-input"
        />
      </el-form-item>

      <el-form-item v-else :label="t('generalizeReports.fields.year')">
        <el-input
          v-model="filters.year"
          :placeholder="t('generalizeReports.placeholders.enterYear')"
          clearable
          data-testid="generalize-year-input"
        />
      </el-form-item>

      <el-form-item :label="t('generalizeReports.fields.storeId')">
        <el-input
          v-model="filters.store_id"
          :placeholder="t('generalizeReports.placeholders.enterStoreId')"
          clearable
          data-testid="generalize-store-input"
        />
      </el-form-item>

      <el-form-item v-if="needsShopTypeFilter" :label="t('generalizeReports.fields.shopTypeId')">
        <el-input
          v-model="filters.shop_type_id"
          :placeholder="t('generalizeReports.placeholders.enterShopTypeId')"
          clearable
          data-testid="generalize-shop-type-input"
        />
      </el-form-item>

      <el-form-item v-if="needsBrandFilter" :label="t('generalizeReports.fields.brandId')">
        <el-input
          v-model="filters.brand_id"
          :placeholder="t('generalizeReports.placeholders.enterBrandId')"
          clearable
          data-testid="generalize-brand-input"
        />
      </el-form-item>

      <el-form-item v-if="needsFloorFilter" :label="t('generalizeReports.fields.floorId')">
        <el-input
          v-model="filters.floor_id"
          :placeholder="t('generalizeReports.placeholders.enterFloorId')"
          clearable
          data-testid="generalize-floor-input"
        />
      </el-form-item>

      <el-form-item v-if="needsUnitFilter" :label="t('generalizeReports.fields.unitId')">
        <el-input
          v-model="filters.unit_id"
          :placeholder="t('generalizeReports.placeholders.enterUnitId')"
          clearable
          data-testid="generalize-unit-input"
        />
      </el-form-item>

      <el-form-item v-if="needsDepartmentFilter" :label="t('generalizeReports.fields.departmentId')">
        <el-input
          v-model="filters.department_id"
          :placeholder="t('generalizeReports.placeholders.enterDepartmentId')"
          clearable
          data-testid="generalize-department-input"
        />
      </el-form-item>

      <el-form-item v-if="needsCustomerFilter" :label="t('generalizeReports.fields.customerId')">
        <el-input
          v-model="filters.customer_id"
          :placeholder="t('generalizeReports.placeholders.enterCustomerId')"
          clearable
          data-testid="generalize-customer-input"
        />
      </el-form-item>

      <el-form-item v-if="needsTradeFilter" :label="t('generalizeReports.fields.tradeId')">
        <el-input
          v-model="filters.trade_id"
          :placeholder="t('generalizeReports.placeholders.enterTradeId')"
          clearable
          data-testid="generalize-trade-input"
        />
      </el-form-item>

      <el-form-item v-if="needsChargeTypeFilter" :label="t('generalizeReports.fields.chargeType')">
        <el-input
          v-model="filters.charge_type"
          :placeholder="t('generalizeReports.placeholders.enterChargeType')"
          clearable
          data-testid="generalize-charge-type-input"
        />
      </el-form-item>

      <template v-if="isR02Report">
        <el-form-item :label="t('common.columns.status')">
          <el-select
            v-model="filters.status"
            :placeholder="t('generalizeReports.placeholders.allStatuses')"
            clearable
            data-testid="generalize-status-input"
          >
            <el-option
              v-for="option in statusOptions"
              :key="option.value"
              :label="option.label"
              :value="option.value"
            />
          </el-select>
        </el-form-item>

        <el-form-item :label="t('generalizeReports.fields.managementTypeId')">
          <el-input
            v-model="filters.management_type_id"
            :placeholder="t('generalizeReports.placeholders.enterManagementTypeId')"
            clearable
            data-testid="generalize-management-type-input"
          />
        </el-form-item>
      </template>
    </FilterForm>

    <el-card class="generalize-reports-view__card" shadow="never">
      <template #header>
        <div class="generalize-reports-view__card-header">
          <div class="generalize-reports-view__card-copy">
            <span>{{ selectedReportLabel }}</span>
            <small>{{ selectedReportSummary }}</small>
          </div>

          <div class="generalize-reports-view__actions">
            <el-button :disabled="!isDirty" @click="handleReset">{{ t('filterForm.reset') }}</el-button>
            <el-button data-testid="generalize-query-button" :loading="isQuerying" @click="loadReport">
              {{ t('common.actions.query') }}
            </el-button>
            <el-button
              type="primary"
              data-testid="generalize-export-button"
              :loading="isExporting"
              @click="handleExport"
            >
              {{ t('common.actions.export') }}
            </el-button>
          </div>
        </div>
      </template>

      <div v-if="generatedAtLabel" class="generalize-reports-view__meta">
        <el-tag effect="plain" type="info">{{ generatedAtLabel }}</el-tag>
      </div>

      <el-table
        :data="result?.rows ?? []"
        class="generalize-reports-view__table"
        :empty-text="t('generalizeReports.table.empty')"
        data-testid="generalize-report-table"
      >
        <el-table-column
          v-for="column in result?.columns ?? []"
          :key="column.key"
          :prop="column.key"
          :label="column.label"
          min-width="160"
        />
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.generalize-reports-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.generalize-reports-view__alert {
  margin-bottom: 0;
}

.generalize-reports-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.generalize-reports-view__card-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: var(--mi-space-4);
}

.generalize-reports-view__card-copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.generalize-reports-view__card-copy small {
  font-size: var(--mi-font-size-100);
  font-weight: var(--mi-font-weight-regular);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
}

.generalize-reports-view__actions {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: var(--mi-space-3);
  flex-wrap: wrap;
}

.generalize-reports-view__meta {
  display: flex;
  margin-bottom: var(--mi-space-4);
}

.generalize-reports-view__table {
  width: 100%;
}

.generalize-reports-view :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .generalize-reports-view__card-header {
    flex-direction: column;
  }

  .generalize-reports-view__actions {
    justify-content: flex-start;
  }
}
</style>
