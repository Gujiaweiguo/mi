<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { generateCharges, listCharges, type BillingRun, type ChargeLine } from '../api/billing'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { getErrorMessage } from '../composables/useErrorMessage'
import { usePagination } from '../composables/usePagination'
import { useAppStore } from '../stores/app'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const rows = ref<ChargeLine[]>([])
const { t } = useI18n()
const appStore = useAppStore()
const { page, pageSize, total, paginationParams, resetPage, handlePageChange, handleSizeChange } = usePagination()
const isLoading = ref(false)
const isGenerating = ref(false)
const feedback = ref<Feedback | null>(null)
const lastGeneratedRun = ref<BillingRun | null>(null)

const { filters, isDirty, reset } = useFilterForm({
  period_start: '',
  period_end: '',
})

const isPeriodRangeInvalid = computed(
  () => Boolean(filters.period_start && filters.period_end) && filters.period_start > filters.period_end,
)

const generationValidationMessage = computed(() => {
  if (!filters.period_start && !filters.period_end) {
    return ''
  }

  if (isPeriodRangeInvalid.value) {
    return t('billingCharges.validation.periodStartBeforeEnd')
  }

  if (!filters.period_start || !filters.period_end) {
    return t('billingCharges.validation.bothDatesRequired')
  }

  return ''
})

const selectionStatusMessage = computed(() => {
  if (isPeriodRangeInvalid.value) {
    return t('billingCharges.validation.periodStartBeforeEnd')
  }

  if (filters.period_start && filters.period_end) {
    return t('billingCharges.selection.selectedPeriod', { start: filters.period_start, end: filters.period_end })
  }

  if (filters.period_start || filters.period_end) {
    return t('billingCharges.selection.partialHint')
  }

  return t('billingCharges.selection.defaultHint')
})

const canGenerate = computed(() => Boolean(filters.period_start && filters.period_end) && !isPeriodRangeInvalid.value)

const formatAmount = (value: number) =>
  new Intl.NumberFormat(appStore.locale, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const formatTimestamp = (value: string) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value))
}

const loadCharges = async () => {
  if (isPeriodRangeInvalid.value) {
    feedback.value = {
      type: 'warning',
      title: t('billingCharges.errors.invalidRange'),
      description: t('billingCharges.validation.periodStartBeforeEnd'),
    }

    return
  }

  isLoading.value = true
  feedback.value = null
  lastGeneratedRun.value = null

  try {
    const response = await listCharges({
      period_start: filters.period_start || undefined,
      period_end: filters.period_end || undefined,
      ...paginationParams.value,
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    rows.value = []
    total.value = 0
    feedback.value = {
      type: 'error',
      title: t('billingCharges.errors.recordsUnavailable'),
      description: getErrorMessage(error, t('billingCharges.errors.unableToLoad')),
    }
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  resetPage()
  feedback.value = null
  void loadCharges()
}

const handlePaginationPageChange = (newPage: number) => {
  handlePageChange(newPage)
  void loadCharges()
}

const handlePaginationSizeChange = (newSize: number) => {
  handleSizeChange(newSize)
  void loadCharges()
}

const handleGenerate = async () => {
  if (!canGenerate.value) {
    feedback.value = {
      type: 'warning',
      title: t('billingCharges.errors.generationPeriodRequired'),
      description: generationValidationMessage.value || t('billingCharges.errors.generationBothDates'),
    }

    return
  }

  isGenerating.value = true
  feedback.value = null

  try {
    const response = await generateCharges({
      period_start: filters.period_start,
      period_end: filters.period_end,
    })

    rows.value = response.data.lines
    total.value = response.data.lines.length
    lastGeneratedRun.value = response.data.run
    feedback.value = {
      type: 'success',
      title: t('billingCharges.feedback.generationCompleted'),
      description: t('billingCharges.feedback.generationResult', {
        generated: response.data.totals.generated,
        skipped: response.data.totals.skipped,
      }),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('billingCharges.errors.generationFailed'),
      description: getErrorMessage(error, t('billingCharges.errors.unableToGenerate')),
    }
  } finally {
    isGenerating.value = false
  }
}

onMounted(() => {
  void loadCharges()
})
</script>

<template>
  <div class="billing-charges-view" v-loading="isLoading" data-testid="billing-charges-view">
    <PageSection
      :eyebrow="t('billingCharges.eyebrow')"
      :title="t('billingCharges.title')"
      :summary="t('billingCharges.summary')"
    >
      <template #actions>
        <el-tag v-if="lastGeneratedRun" effect="plain" type="success">
          Run #{{ lastGeneratedRun.id }} · {{ lastGeneratedRun.status }}
        </el-tag>
        <el-button
          type="primary"
          :loading="isGenerating"
          :disabled="!canGenerate"
          data-testid="charge-generate-button"
          @click="handleGenerate"
        >
          {{ t('billingCharges.actions.generate') }}
        </el-button>
      </template>
    </PageSection>

    <el-alert
      v-if="feedback"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <FilterForm
      :title="t('billingCharges.filters.title')"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="resetPage(); loadCharges()"
    >
      <el-form-item :label="t('billingCharges.fields.periodStart')">
        <el-date-picker
          v-model="filters.period_start"
          type="date"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          :placeholder="t('billingCharges.placeholders.selectStartDate')"
          class="billing-charges-view__filter-input"
          data-testid="charge-period-start"
        />
      </el-form-item>

      <el-form-item :label="t('billingCharges.fields.periodEnd')">
        <el-date-picker
          v-model="filters.period_end"
          type="date"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          :placeholder="t('billingCharges.placeholders.selectEndDate')"
          class="billing-charges-view__filter-input"
          data-testid="charge-period-end"
        />
      </el-form-item>

      <el-form-item :label="t('billingCharges.fields.selectionStatus')">
        <el-tag :type="isPeriodRangeInvalid ? 'warning' : 'info'" effect="plain" class="billing-charges-view__status-tag">
          {{ selectionStatusMessage }}
        </el-tag>
      </el-form-item>
    </FilterForm>

    <el-card v-if="lastGeneratedRun" class="billing-charges-view__summary-card" shadow="never">
        <template #header>
          <div class="billing-charges-view__table-header">
            <span>{{ t('billingCharges.feedback.latestRun') }}</span>
            <el-tag effect="plain" type="success">{{ t('billingCharges.feedback.generatedCount', { count: lastGeneratedRun.generated_count }) }}</el-tag>
          </div>
        </template>

      <el-descriptions :column="2" border>
        <el-descriptions-item :label="t('billingCharges.fields.runId')">{{ lastGeneratedRun.id }}</el-descriptions-item>
        <el-descriptions-item :label="t('common.columns.status')">{{ lastGeneratedRun.status }}</el-descriptions-item>
        <el-descriptions-item :label="t('billingCharges.fields.periodStart')">{{ lastGeneratedRun.period_start }}</el-descriptions-item>
        <el-descriptions-item :label="t('billingCharges.fields.periodEnd')">{{ lastGeneratedRun.period_end }}</el-descriptions-item>
        <el-descriptions-item :label="t('billingCharges.fields.generatedLines')">{{ lastGeneratedRun.generated_count }}</el-descriptions-item>
        <el-descriptions-item :label="t('billingCharges.fields.skippedLines')">{{ lastGeneratedRun.skipped_count }}</el-descriptions-item>
        <el-descriptions-item :label="t('common.columns.createdAt')">{{ formatTimestamp(lastGeneratedRun.created_at) }}</el-descriptions-item>
        <el-descriptions-item :label="t('billingCharges.fields.updatedAt')">{{ formatTimestamp(lastGeneratedRun.updated_at) }}</el-descriptions-item>
      </el-descriptions>
    </el-card>

    <el-card class="billing-charges-view__table-card" shadow="never">
        <template #header>
          <div class="billing-charges-view__table-header">
            <span>{{ lastGeneratedRun ? t('billingCharges.table.generatedLines') : t('billingCharges.table.lineResults') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
          </div>
        </template>

      <el-table
          :data="rows"
          row-key="id"
          class="billing-charges-view__table"
          :empty-text="t('billingCharges.table.empty')"
          data-testid="charges-table"
        >
          <el-table-column prop="billing_run_id" :label="t('billingCharges.fields.runId')" min-width="100" />
          <el-table-column prop="lease_no" :label="t('billingCharges.columns.leaseNo')" min-width="150" />
          <el-table-column prop="tenant_name" :label="t('billingCharges.columns.tenant')" min-width="220" />
          <el-table-column prop="charge_type" :label="t('billingCharges.columns.chargeType')" min-width="150" />
          <el-table-column :label="t('billingCharges.fields.period')" min-width="220">
          <template #default="scope">
            {{ scope.row.period_start }} → {{ scope.row.period_end }}
          </template>
        </el-table-column>
          <el-table-column prop="quantity_days" :label="t('billingCharges.fields.days')" min-width="90" />
          <el-table-column :label="t('billingCharges.fields.unitAmount')" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.unit_amount) }}
          </template>
        </el-table-column>
          <el-table-column :label="t('billingCharges.fields.amount')" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.amount) }}
          </template>
        </el-table-column>
          <el-table-column :label="t('common.columns.createdAt')" min-width="180">
            <template #default="scope">
              {{ formatTimestamp(scope.row.created_at) }}
            </template>
        </el-table-column>
      </el-table>

      <div class="billing-charges-view__pagination">
        <el-pagination
          v-model:current-page="page"
          v-model:page-size="pageSize"
          :total="total"
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next"
          @current-change="handlePaginationPageChange"
          @size-change="handlePaginationSizeChange"
        />
      </div>
    </el-card>
  </div>
</template>

<style scoped>
.billing-charges-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.billing-charges-view__filter-input,
.billing-charges-view__status-tag,
.billing-charges-view__table {
  width: 100%;
}

.billing-charges-view__summary-card,
.billing-charges-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.billing-charges-view__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.billing-charges-view__pagination {
  display: flex;
  justify-content: flex-end;
  margin-top: var(--mi-space-4);
}

@media (max-width: 52rem) {
  .billing-charges-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
