<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

import { generateCharges, listCharges, type BillingRun, type ChargeLine } from '../api/billing'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const rows = ref<ChargeLine[]>([])
const total = ref(0)
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
    return 'Period start must be on or before period end.'
  }

  if (!filters.period_start || !filters.period_end) {
    return 'Select both period start and period end to generate charges.'
  }

  return ''
})

const selectionStatusMessage = computed(() => {
  if (isPeriodRangeInvalid.value) {
    return 'Period start must be on or before period end.'
  }

  if (filters.period_start && filters.period_end) {
    return `Selected period ${filters.period_start} → ${filters.period_end}`
  }

  if (filters.period_start || filters.period_end) {
    return 'Filters can use either period bound; generation requires both dates.'
  }

  return 'Add a period range to narrow results or prepare a generation run.'
})

const canGenerate = computed(() => Boolean(filters.period_start && filters.period_end) && !isPeriodRangeInvalid.value)

const formatAmount = (value: number) =>
  new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const formatTimestamp = (value: string) => {
  if (!value) {
    return '—'
  }

  return new Intl.DateTimeFormat('en-US', {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value))
}

const loadCharges = async () => {
  if (isPeriodRangeInvalid.value) {
    feedback.value = {
      type: 'warning',
      title: 'Invalid charge filter range',
      description: 'Period start must be on or before period end.',
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
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    rows.value = []
    total.value = 0
    feedback.value = {
      type: 'error',
      title: 'Charge records unavailable',
      description: error instanceof Error ? error.message : 'Unable to load billing charges.',
    }
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  feedback.value = null
  void loadCharges()
}

const handleGenerate = async () => {
  if (!canGenerate.value) {
    feedback.value = {
      type: 'warning',
      title: 'Generation period required',
      description: generationValidationMessage.value || 'Select both period start and period end before generating charges.',
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
      title: 'Charge generation completed',
      description: `Generated ${response.data.totals.generated} lines and skipped ${response.data.totals.skipped} lines for the selected period.`,
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: 'Charge generation failed',
      description: error instanceof Error ? error.message : 'Unable to generate billing charges.',
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
  <div class="billing-charges-view" data-testid="billing-charges-view">
    <PageSection
      eyebrow="Billing operations"
      title="Billing charges"
      summary="Review generated charge lines by billing period and trigger a fresh generation run when a lease period is ready for billing."
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
          Generate charges
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
      title="Charge filters"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="loadCharges"
    >
      <el-form-item label="Period start">
        <el-date-picker
          v-model="filters.period_start"
          type="date"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          placeholder="Select start date"
          class="billing-charges-view__filter-input"
          data-testid="charge-period-start"
        />
      </el-form-item>

      <el-form-item label="Period end">
        <el-date-picker
          v-model="filters.period_end"
          type="date"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          placeholder="Select end date"
          class="billing-charges-view__filter-input"
          data-testid="charge-period-end"
        />
      </el-form-item>

      <el-form-item label="Selection status">
        <el-tag :type="isPeriodRangeInvalid ? 'warning' : 'info'" effect="plain" class="billing-charges-view__status-tag">
          {{ selectionStatusMessage }}
        </el-tag>
      </el-form-item>
    </FilterForm>

    <el-card v-if="lastGeneratedRun" class="billing-charges-view__summary-card" shadow="never">
      <template #header>
        <div class="billing-charges-view__table-header">
          <span>Latest billing run</span>
          <el-tag effect="plain" type="success">{{ lastGeneratedRun.generated_count }} generated</el-tag>
        </div>
      </template>

      <el-descriptions :column="2" border>
        <el-descriptions-item label="Run ID">{{ lastGeneratedRun.id }}</el-descriptions-item>
        <el-descriptions-item label="Status">{{ lastGeneratedRun.status }}</el-descriptions-item>
        <el-descriptions-item label="Period start">{{ lastGeneratedRun.period_start }}</el-descriptions-item>
        <el-descriptions-item label="Period end">{{ lastGeneratedRun.period_end }}</el-descriptions-item>
        <el-descriptions-item label="Generated lines">{{ lastGeneratedRun.generated_count }}</el-descriptions-item>
        <el-descriptions-item label="Skipped lines">{{ lastGeneratedRun.skipped_count }}</el-descriptions-item>
        <el-descriptions-item label="Created at">{{ formatTimestamp(lastGeneratedRun.created_at) }}</el-descriptions-item>
        <el-descriptions-item label="Updated at">{{ formatTimestamp(lastGeneratedRun.updated_at) }}</el-descriptions-item>
      </el-descriptions>
    </el-card>

    <el-card class="billing-charges-view__table-card" shadow="never">
      <template #header>
        <div class="billing-charges-view__table-header">
          <span>{{ lastGeneratedRun ? 'Generated charge lines' : 'Charge line results' }}</span>
          <el-tag effect="plain" type="info">{{ total }} total</el-tag>
        </div>
      </template>

      <el-table
        :data="rows"
        row-key="id"
        class="billing-charges-view__table"
        empty-text="No billing charge lines match the current selection."
        data-testid="charges-table"
      >
        <el-table-column prop="billing_run_id" label="Run ID" min-width="100" />
        <el-table-column prop="lease_no" label="Lease no." min-width="150" />
        <el-table-column prop="tenant_name" label="Tenant" min-width="220" />
        <el-table-column prop="charge_type" label="Charge type" min-width="150" />
        <el-table-column label="Period" min-width="220">
          <template #default="scope">
            {{ scope.row.period_start }} → {{ scope.row.period_end }}
          </template>
        </el-table-column>
        <el-table-column prop="quantity_days" label="Days" min-width="90" />
        <el-table-column label="Unit amount" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.unit_amount) }}
          </template>
        </el-table-column>
        <el-table-column label="Amount" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.amount) }}
          </template>
        </el-table-column>
        <el-table-column label="Created at" min-width="180">
          <template #default="scope">
            {{ formatTimestamp(scope.row.created_at) }}
          </template>
        </el-table-column>
      </el-table>
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

@media (max-width: 52rem) {
  .billing-charges-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
