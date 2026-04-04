<script setup lang="ts">
import { onMounted, ref } from 'vue'

import { listTaxRuleSets, exportTaxVouchers, type TaxRuleSet } from '../api/tax'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const ruleSets = ref<TaxRuleSet[]>([])
const total = ref(0)
const isLoading = ref(false)
const isExporting = ref(false)
const feedback = ref<Feedback | null>(null)
const selectedRuleSetCode = ref('')
const exportFrom = ref('')
const exportTo = ref('')

const { filters, isDirty, reset } = useFilterForm({
  search: '',
})

const loadRuleSets = async () => {
  isLoading.value = true
  feedback.value = null

  try {
    const response = await listTaxRuleSets()
    ruleSets.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    ruleSets.value = []
    total.value = 0
    feedback.value = {
      type: 'error',
      title: 'Tax rule sets unavailable',
      description: error instanceof Error ? error.message : 'Unable to load tax rule sets.',
    }
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  feedback.value = null
  void loadRuleSets()
}

const canExport = () => Boolean(selectedRuleSetCode.value && exportFrom.value && exportTo.value)

const handleExport = async () => {
  if (!canExport()) {
    feedback.value = {
      type: 'warning',
      title: 'Export parameters required',
      description: 'Select a rule set code and specify both from/to dates before exporting.',
    }
    return
  }

  isExporting.value = true
  feedback.value = null

  try {
    const response = await exportTaxVouchers({
      rule_set_code: selectedRuleSetCode.value,
      from_date: exportFrom.value,
      to_date: exportTo.value,
    })

    const blob = new Blob([response.data as unknown as BlobPart], { type: 'application/octet-stream' })
    const url = URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `tax-vouchers-${selectedRuleSetCode.value}.xlsx`
    link.click()
    URL.revokeObjectURL(url)

    feedback.value = {
      type: 'success',
      title: 'Tax export completed',
      description: `Vouchers exported for rule set "${selectedRuleSetCode.value}" from ${exportFrom.value} to ${exportTo.value}.`,
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: 'Tax export failed',
      description: error instanceof Error ? error.message : 'Unable to export tax vouchers.',
    }
  } finally {
    isExporting.value = false
  }
}

const formatDate = (value: string) => {
  if (!value) return '—'
  return new Intl.DateTimeFormat('en-US', { dateStyle: 'medium' }).format(new Date(value))
}

onMounted(() => {
  void loadRuleSets()
})
</script>

<template>
  <div class="tax-exports-view" data-testid="tax-exports-view">
    <PageSection
      eyebrow="Tax output"
      title="Tax exports"
      summary="Select a tax rule set and date range to export accounting vouchers for tax reporting."
    />

    <el-alert
      v-if="feedback"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <FilterForm
      title="Rule set filters"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="loadRuleSets"
    >
      <el-form-item label="Search">
        <el-input
          v-model="filters.search"
          placeholder="Filter rule sets by code or name"
          clearable
          data-testid="tax-ruleset-search"
        />
      </el-form-item>
    </FilterForm>

    <el-card class="tax-exports-view__card" shadow="never">
      <template #header>
        <div class="tax-exports-view__card-header">
          <span>Voucher export</span>
        </div>
      </template>

      <div class="tax-exports-view__export-form">
        <el-form label-position="top">
          <div class="tax-exports-view__export-grid">
            <el-form-item label="Rule set code">
              <el-select
                v-model="selectedRuleSetCode"
                placeholder="Select a rule set"
                clearable
                filterable
                data-testid="tax-ruleset-select"
              >
                <el-option
                  v-for="rs in ruleSets"
                  :key="rs.id"
                  :label="`${rs.code} — ${rs.name}`"
                  :value="rs.code"
                />
              </el-select>
            </el-form-item>

            <el-form-item label="From date">
              <el-date-picker
                v-model="exportFrom"
                type="date"
                value-format="YYYY-MM-DD"
                format="YYYY-MM-DD"
                placeholder="Select start date"
                data-testid="tax-export-from"
              />
            </el-form-item>

            <el-form-item label="To date">
              <el-date-picker
                v-model="exportTo"
                type="date"
                value-format="YYYY-MM-DD"
                format="YYYY-MM-DD"
                placeholder="Select end date"
                data-testid="tax-export-to"
              />
            </el-form-item>
          </div>

          <el-button
            type="primary"
            :loading="isExporting"
            :disabled="!canExport()"
            data-testid="tax-export-button"
            @click="handleExport"
          >
            Export vouchers
          </el-button>
        </el-form>
      </div>
    </el-card>

    <el-card class="tax-exports-view__card" shadow="never">
      <template #header>
        <div class="tax-exports-view__card-header">
          <span>Available rule sets</span>
          <el-tag effect="plain" type="info">{{ total }} total</el-tag>
        </div>
      </template>

      <el-table
        :data="ruleSets"
        row-key="id"
        class="tax-exports-view__table"
        empty-text="No tax rule sets available."
        data-testid="tax-rulesets-table"
      >
        <el-table-column prop="code" label="Code" min-width="160" />
        <el-table-column prop="name" label="Name" min-width="220" />
        <el-table-column prop="document_type" label="Document type" min-width="140" />
        <el-table-column prop="status" label="Status" min-width="120">
          <template #default="scope">
            <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
              {{ scope.row.status }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="Rules" min-width="100">
          <template #default="scope">
            {{ scope.row.rules?.length ?? 0 }}
          </template>
        </el-table-column>
        <el-table-column label="Created" min-width="160">
          <template #default="scope">
            {{ formatDate(scope.row.created_at) }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.tax-exports-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.tax-exports-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.tax-exports-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.tax-exports-view__export-form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.tax-exports-view__export-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.tax-exports-view__table {
  width: 100%;
}

@media (max-width: 52rem) {
  .tax-exports-view__export-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
