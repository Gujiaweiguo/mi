<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

import { exportReport, queryReport, type ReportId, type ReportQueryPayload, type ReportQueryResponse } from '../api/reports'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

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

const reportOptions: ReportOption[] = [
  { value: 'r01', label: 'R01 Lease Status Area Summary' },
  { value: 'r02', label: 'R02 Contract Ledger' },
  { value: 'r03', label: 'R03 Shop Type Sales & Rent Analysis' },
  { value: 'r04', label: 'R04 Daily Shop Sales Analysis' },
  { value: 'r05', label: 'R05 Unit Budget vs Lease/Prospect Price' },
  { value: 'r06', label: 'R06 Store Rent Budget Execution' },
  { value: 'r07', label: 'R07 Brand annual sales distribution' },
  { value: 'r08', label: 'R08 Customer AR Aging Summary' },
  { value: 'r09', label: 'R09 Customer AR Aging by Charge Type' },
  { value: 'r10', label: 'R10 Traffic Annual/Monthly Summary' },
  { value: 'r11', label: 'R11 Lease Area vs Total Area' },
  { value: 'r12', label: 'R12 Occupancy / Unit Status Structure' },
  { value: 'r13', label: 'R13 Shop type sales YoY/MoM' },
  { value: 'r14', label: 'R14 Sales efficiency' },
  { value: 'r15', label: 'R15 Sales vs Rent Income Comparison by Shop Type' },
  { value: 'r16', label: 'R16 Subsidiary AR Aging Summary' },
  { value: 'r17', label: 'R17 Subsidiary AR Aging by Charge Type' },
  { value: 'r18', label: 'R18 Customer / Store / Brand Composite Report' },
]

const reportSummaries: Record<GeneralizeReportId, string> = {
  r01: 'Summarize lease status by area for a selected operating period and store.',
  r02: 'Review contract ledger rows with status, organizational, customer, and management filters.',
  r03: 'Analyze sales and rent by shop type for a selected reporting slice.',
  r04: 'Review daily shop sales totals for a selected reporting slice.',
  r05: 'Compare unit budget price, current lease price, and prospect pricing for the selected fiscal year.',
  r06: 'Compare store rent budget against period receivable/received values for the selected period.',
  r07: 'Distribute annual sales by brand within the selected store.',
  r08: 'Summarize customer receivables by aging bucket as of the selected cutoff period.',
  r09: 'Break customer receivables aging down by charge type for the selected cutoff period.',
  r10: 'Summarize traffic metrics for the selected year (annual and monthly breakdown).',
  r11: 'Compare leased area against total area for the selected operating slice.',
  r12: 'Review occupancy and unit-status structure for the selected operating slice.',
  r13: 'Compare shop-type sales year-over-year and month-over-month for the selected slice.',
  r14: 'Review sales efficiency metrics by shop type for the selected slice.',
  r15: 'Compare sales and rent income by shop type for the selected operating period.',
  r16: 'Summarize subsidiary receivables aging totals as of the selected cutoff period.',
  r17: 'Break subsidiary receivables aging down by charge type for the selected cutoff period.',
  r18: 'Review combined sales, receivable, arrears, and efficiency metrics by customer, store, brand, and unit.',
}

const statusOptions = [
  { label: 'Draft', value: 'draft' },
  { label: 'Pending approval', value: 'pending_approval' },
  { label: 'Active', value: 'active' },
  { label: 'Rejected', value: 'rejected' },
  { label: 'Terminated', value: 'terminated' },
]

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
  () => reportOptions.find((option) => option.value === filters.report_id)?.label ?? reportOptions[0].label,
)

const selectedReportSummary = computed(() => reportSummaries[filters.report_id])

const generatedAtLabel = computed(() => {
  if (!result.value?.generated_at) {
    return ''
  }

  return new Intl.DateTimeFormat('en-US', {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(result.value.generated_at))
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
    errorMessage.value = error instanceof Error ? error.message : 'Unable to load the selected Generalize report.'
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
    const extension = inferFileExtension(response.headers['content-type'])
    const url = URL.createObjectURL(blob)
    const link = document.createElement('a')

    link.href = url
    link.download = `${buildFileName()}.${extension}`
    link.click()
    URL.revokeObjectURL(url)
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unable to export the selected Generalize report.'
  } finally {
    isExporting.value = false
  }
}

onMounted(() => {
  void loadReport()
})
</script>

<template>
  <div class="generalize-reports-view" data-testid="generalize-reports-view">
    <PageSection
      eyebrow="Generalize reporting"
      title="Generalize reports"
      summary="Run the current reporting slice for lease, budget, sales, traffic, and AR aging reports across the currently implemented Generalize inventory."
    >
      <template #actions>
        <el-tag effect="plain" type="info">Task 16 slice</el-tag>
        <el-tag effect="plain" type="success">R01 / R02 / R03 / R04 / R05 / R06 / R07 / R08 / R09 / R10 / R11 / R12 / R13 / R14 / R15 / R16 / R17 / R18</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="generalize-reports-view__alert"
      title="Generalize report request failed"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <FilterForm title="Report parameters" :show-actions="false">
      <el-form-item label="Report">
        <el-select
          v-model="filters.report_id"
          placeholder="Select a report"
          data-testid="generalize-report-select"
        >
          <el-option v-for="option in reportOptions" :key="option.value" :label="option.label" :value="option.value" />
        </el-select>
      </el-form-item>

        <el-form-item v-if="!isYearReport" label="Period">
          <el-input
            v-model="filters.period"
            placeholder="Enter a period, e.g. 2026-03"
            clearable
            data-testid="generalize-period-input"
          />
        </el-form-item>

        <el-form-item v-else label="Year">
          <el-input
            v-model="filters.year"
            placeholder="Enter a year, e.g. 2026"
            clearable
            data-testid="generalize-year-input"
          />
        </el-form-item>

       <el-form-item label="Store ID">
         <el-input
           v-model="filters.store_id"
           placeholder="Enter a store identifier"
           clearable
           data-testid="generalize-store-input"
         />
       </el-form-item>

        <el-form-item v-if="needsShopTypeFilter" label="Shop type ID">
          <el-input
            v-model="filters.shop_type_id"
            placeholder="Enter a shop type identifier"
            clearable
            data-testid="generalize-shop-type-input"
          />
        </el-form-item>

        <el-form-item v-if="needsBrandFilter" label="Brand ID">
          <el-input
            v-model="filters.brand_id"
            placeholder="Enter a brand identifier"
            clearable
            data-testid="generalize-brand-input"
          />
        </el-form-item>

        <el-form-item v-if="needsFloorFilter" label="Floor ID">
          <el-input
            v-model="filters.floor_id"
            placeholder="Enter a floor identifier"
            clearable
            data-testid="generalize-floor-input"
          />
        </el-form-item>

        <el-form-item v-if="needsUnitFilter" label="Unit ID">
          <el-input
            v-model="filters.unit_id"
            placeholder="Enter a unit identifier"
            clearable
            data-testid="generalize-unit-input"
          />
        </el-form-item>

      <el-form-item v-if="needsDepartmentFilter" label="Department ID">
        <el-input
          v-model="filters.department_id"
          placeholder="Enter a department identifier"
          clearable
          data-testid="generalize-department-input"
        />
      </el-form-item>

      <el-form-item v-if="needsCustomerFilter" label="Customer ID">
        <el-input
          v-model="filters.customer_id"
          placeholder="Enter a customer identifier"
          clearable
          data-testid="generalize-customer-input"
        />
      </el-form-item>

      <el-form-item v-if="needsTradeFilter" label="Trade ID">
        <el-input
          v-model="filters.trade_id"
          placeholder="Enter a trade identifier"
          clearable
          data-testid="generalize-trade-input"
        />
      </el-form-item>

      <el-form-item v-if="needsChargeTypeFilter" label="Charge type">
        <el-input
          v-model="filters.charge_type"
          placeholder="Enter a charge type, e.g. rent"
          clearable
          data-testid="generalize-charge-type-input"
        />
      </el-form-item>

      <template v-if="isR02Report">
        <el-form-item label="Status">
          <el-select
            v-model="filters.status"
            placeholder="All statuses"
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

        <el-form-item label="Management type ID">
          <el-input
            v-model="filters.management_type_id"
            placeholder="Enter a management type identifier"
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
            <el-button :disabled="!isDirty" @click="handleReset">Reset</el-button>
            <el-button data-testid="generalize-query-button" :loading="isQuerying" @click="loadReport">
              Query report
            </el-button>
            <el-button
              type="primary"
              data-testid="generalize-export-button"
              :loading="isExporting"
              @click="handleExport"
            >
              Export report
            </el-button>
          </div>
        </div>
      </template>

      <div v-if="generatedAtLabel" class="generalize-reports-view__meta">
        <el-tag effect="plain" type="info">Generated {{ generatedAtLabel }}</el-tag>
      </div>

      <el-table
        :data="result?.rows ?? []"
        class="generalize-reports-view__table"
        empty-text="Run a report to view results."
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
