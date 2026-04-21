<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { exportTaxVouchers, listTaxRuleSets, upsertTaxRuleSet, type TaxRuleSet, type UpsertTaxRuleSetRequest } from '../api/tax'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { downloadBlob } from '../composables/useDownload'
import { useFilterForm } from '../composables/useFilterForm'
import { getErrorMessage } from '../composables/useErrorMessage'
import { formatDate } from '../utils/format'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

interface RuleEntry {
  sequence_no: number
  entry_side: string
  charge_type_filter: string
  account_number: string
  account_name: string
  explanation_template: string
  use_tenant_name: boolean
  is_balancing_entry: boolean
}

const { t } = useI18n()

const ruleSets = ref<TaxRuleSet[]>([])
const total = ref(0)
const isLoading = ref(false)
const isExporting = ref(false)
const feedback = ref<Feedback | null>(null)
const selectedRuleSetCode = ref('')
const exportFrom = ref('')
const exportTo = ref('')
const dialogVisible = ref(false)
const isSubmitting = ref(false)
const isEditMode = ref(false)

const formModel = ref({
  code: '',
  name: '',
  document_type: '',
  rules: [] as RuleEntry[],
})

const { filters, isDirty, reset } = useFilterForm({
  search: '',
})

const documentTypeOptions = ['invoice', 'receipt'] as const
const entrySideOptions = ['debit', 'credit'] as const

const dialogTitle = computed(() =>
  t(isEditMode.value ? 'taxExports.dialog.title.edit' : 'taxExports.dialog.title.create'),
)

const canSubmitDialog = computed(() => {
  if (!formModel.value.code.trim() || !formModel.value.name.trim() || !formModel.value.document_type) {
    return false
  }

  if (formModel.value.rules.length === 0) {
    return false
  }

  return formModel.value.rules.every((rule) => {
    return (
      rule.sequence_no > 0 &&
      Boolean(rule.entry_side) &&
      Boolean(rule.charge_type_filter.trim()) &&
      Boolean(rule.account_number.trim()) &&
      Boolean(rule.account_name.trim()) &&
      Boolean(rule.explanation_template.trim())
    )
  })
})

const resolveStatusLabel = (status: string) => {
  if (status === 'active') {
    return t('common.statuses.active')
  }

  return status
}

const resolveDocumentTypeLabel = (documentType: string) => {
  if (documentType === 'invoice') {
    return t('taxExports.dialog.documentTypeOptions.invoice')
  }

  if (documentType === 'receipt') {
    return t('taxExports.dialog.documentTypeOptions.receipt')
  }

  return documentType || t('common.emptyValue')
}

const resetFormModel = () => {
  formModel.value = {
    code: '',
    name: '',
    document_type: '',
    rules: [],
  }
}

const nextSequenceNo = () => {
  return formModel.value.rules.reduce((max, rule) => Math.max(max, rule.sequence_no), 0) + 1
}

const addRuleRow = () => {
  formModel.value.rules.push({
    sequence_no: nextSequenceNo(),
    entry_side: 'debit',
    charge_type_filter: '',
    account_number: '',
    account_name: '',
    explanation_template: '',
    use_tenant_name: false,
    is_balancing_entry: false,
  })
}

const openCreateDialog = () => {
  isEditMode.value = false
  resetFormModel()
  addRuleRow()
  dialogVisible.value = true
}

const openEditDialog = (ruleSet: TaxRuleSet) => {
  isEditMode.value = true
  formModel.value = {
    code: ruleSet.code,
    name: ruleSet.name,
    document_type: ruleSet.document_type,
    rules:
      ruleSet.rules?.map((rule) => ({
        sequence_no: rule.sequence_no,
        entry_side: rule.entry_side,
        charge_type_filter: rule.charge_type_filter,
        account_number: rule.account_number,
        account_name: rule.account_name,
        explanation_template: rule.explanation_template,
        use_tenant_name: rule.use_tenant_name,
        is_balancing_entry: rule.is_balancing_entry,
      })) ?? [],
  }

  if (formModel.value.rules.length === 0) {
    addRuleRow()
  }

  dialogVisible.value = true
}

const removeRuleRow = (index: number) => {
  formModel.value.rules.splice(index, 1)
}

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
      title: t('taxExports.errors.ruleSetsUnavailable'),
      description: getErrorMessage(error, t('taxExports.errors.unableToLoadRuleSets')),
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
      title: t('taxExports.feedback.parametersRequiredTitle'),
      description: t('taxExports.feedback.parametersRequiredDescription'),
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
    downloadBlob(blob, `tax-vouchers-${selectedRuleSetCode.value}.xlsx`)

    feedback.value = {
      type: 'success',
      title: t('taxExports.feedback.completedTitle'),
      description: t('taxExports.feedback.completedDescription', {
        ruleSetCode: selectedRuleSetCode.value,
        fromDate: exportFrom.value,
        toDate: exportTo.value,
      }),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('taxExports.errors.exportFailed'),
      description: getErrorMessage(error, t('taxExports.errors.unableToExport')),
    }
  } finally {
    isExporting.value = false
  }
}

const handleSubmit = async () => {
  if (!canSubmitDialog.value) {
    return
  }

  isSubmitting.value = true
  feedback.value = null

  const payload: UpsertTaxRuleSetRequest = {
    code: formModel.value.code.trim(),
    name: formModel.value.name.trim(),
    document_type: formModel.value.document_type,
    rules: formModel.value.rules.map((rule) => ({
      sequence_no: rule.sequence_no,
      entry_side: rule.entry_side,
      charge_type_filter: rule.charge_type_filter.trim(),
      account_number: rule.account_number.trim(),
      account_name: rule.account_name.trim(),
      explanation_template: rule.explanation_template.trim(),
      use_tenant_name: rule.use_tenant_name,
      is_balancing_entry: rule.is_balancing_entry,
    })),
  }

  try {
    await upsertTaxRuleSet(payload)
    dialogVisible.value = false
    selectedRuleSetCode.value = payload.code
    await loadRuleSets()
    feedback.value = {
      type: 'success',
      title: isEditMode.value ? t('taxExports.dialog.feedback.updated') : t('taxExports.dialog.feedback.created'),
      description: payload.name,
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('taxExports.dialog.feedback.failed'),
      description: getErrorMessage(error, t('taxExports.errors.unableToLoadRuleSets')),
    }
  } finally {
    isSubmitting.value = false
  }
}

onMounted(() => {
  void loadRuleSets()
})
</script>

<template>
  <div class="tax-exports-view" v-loading="isLoading || isExporting" data-testid="tax-exports-view">
    <PageSection
      :eyebrow="t('taxExports.eyebrow')"
      :title="t('taxExports.title')"
      :summary="t('taxExports.summary')"
    />

    <el-alert
      v-if="feedback"
      data-testid="tax-exports-feedback"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <FilterForm
      :title="t('taxExports.filters.title')"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="loadRuleSets"
    >
      <el-form-item :label="t('taxExports.fields.search')">
        <el-input
          v-model="filters.search"
          :placeholder="t('taxExports.placeholders.searchRuleSets')"
          clearable
          data-testid="tax-ruleset-search"
        />
      </el-form-item>
    </FilterForm>

    <el-card class="tax-exports-view__card" shadow="never">
      <template #header>
        <div class="tax-exports-view__card-header">
          <span>{{ t('taxExports.cards.voucherExport') }}</span>
        </div>
      </template>

      <div class="tax-exports-view__export-form">
        <el-form label-position="top">
          <div class="tax-exports-view__export-grid">
            <el-form-item :label="t('taxExports.fields.ruleSetCode')">
              <el-select
                v-model="selectedRuleSetCode"
                :placeholder="t('taxExports.placeholders.selectRuleSet')"
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

            <el-form-item :label="t('taxExports.fields.fromDate')">
              <el-date-picker
                v-model="exportFrom"
                type="date"
                value-format="YYYY-MM-DD"
                format="YYYY-MM-DD"
                :placeholder="t('taxExports.placeholders.selectStartDate')"
                data-testid="tax-export-from"
              />
            </el-form-item>

            <el-form-item :label="t('taxExports.fields.toDate')">
              <el-date-picker
                v-model="exportTo"
                type="date"
                value-format="YYYY-MM-DD"
                format="YYYY-MM-DD"
                :placeholder="t('taxExports.placeholders.selectEndDate')"
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
            {{ t('taxExports.actions.exportVouchers') }}
          </el-button>
        </el-form>
      </div>
    </el-card>

    <el-card class="tax-exports-view__card" shadow="never">
      <template #header>
        <div class="tax-exports-view__card-header">
          <span>{{ t('taxExports.cards.availableRuleSets') }}</span>
          <div class="tax-exports-view__card-header-actions">
            <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
            <el-button type="primary" @click="openCreateDialog">
              {{ t('taxExports.dialog.actions.addRuleSet') }}
            </el-button>
          </div>
        </div>
      </template>

      <el-table
        :data="ruleSets"
        row-key="id"
        class="tax-exports-view__table"
        :empty-text="t('taxExports.table.empty')"
        data-testid="tax-rulesets-table"
        @row-click="openEditDialog"
      >
        <el-table-column prop="code" :label="t('taxExports.columns.code')" min-width="160" />
        <el-table-column prop="name" :label="t('taxExports.columns.name')" min-width="220" />
        <el-table-column :label="t('taxExports.columns.documentType')" min-width="140">
          <template #default="scope">
            {{ resolveDocumentTypeLabel(scope.row.document_type) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.status')" min-width="120">
          <template #default="scope">
            <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
              {{ resolveStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('taxExports.columns.rules')" min-width="100">
          <template #default="scope">
            {{ scope.row.rules?.length ?? 0 }}
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.createdAt')" min-width="160">
          <template #default="scope">
            {{ formatDate(scope.row.created_at) }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="80rem">
      <div class="tax-exports-view__dialog-body">
        <el-form label-position="top" @submit.prevent>
          <div class="tax-exports-view__dialog-grid">
            <el-form-item :label="t('taxExports.dialog.fields.code')">
              <el-input v-model="formModel.code" />
            </el-form-item>

            <el-form-item :label="t('taxExports.dialog.fields.name')">
              <el-input v-model="formModel.name" />
            </el-form-item>

            <el-form-item :label="t('taxExports.dialog.fields.documentType')">
              <el-select v-model="formModel.document_type">
                <el-option
                  v-for="option in documentTypeOptions"
                  :key="option"
                  :label="t(`taxExports.dialog.documentTypeOptions.${option}`)"
                  :value="option"
                />
              </el-select>
            </el-form-item>
          </div>
        </el-form>

        <div class="tax-exports-view__dialog-section">
          <div class="tax-exports-view__dialog-section-header">
            <span>{{ t('taxExports.columns.rules') }}</span>
          </div>

          <el-table :data="formModel.rules" row-key="sequence_no" class="tax-exports-view__dialog-table">
            <el-table-column :label="t('taxExports.dialog.fields.rules.sequenceNo')" min-width="120">
              <template #default="scope">
                <el-input-number v-model="scope.row.sequence_no" :min="1" controls-position="right" />
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.entrySide')" min-width="160">
              <template #default="scope">
                <el-select v-model="scope.row.entry_side">
                  <el-option
                    v-for="option in entrySideOptions"
                    :key="option"
                    :label="t(`taxExports.dialog.entrySideOptions.${option}`)"
                    :value="option"
                  />
                </el-select>
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.chargeTypeFilter')" min-width="180">
              <template #default="scope">
                <el-input v-model="scope.row.charge_type_filter" />
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.accountNumber')" min-width="160">
              <template #default="scope">
                <el-input v-model="scope.row.account_number" />
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.accountName')" min-width="180">
              <template #default="scope">
                <el-input v-model="scope.row.account_name" />
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.explanationTemplate')" min-width="220">
              <template #default="scope">
                <el-input v-model="scope.row.explanation_template" />
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.useTenantName')" min-width="160">
              <template #default="scope">
                <el-checkbox v-model="scope.row.use_tenant_name" />
              </template>
            </el-table-column>

            <el-table-column :label="t('taxExports.dialog.fields.rules.isBalancingEntry')" min-width="160">
              <template #default="scope">
                <el-checkbox v-model="scope.row.is_balancing_entry" />
              </template>
            </el-table-column>

            <el-table-column :label="t('common.columns.actions')" min-width="140" fixed="right">
              <template #default="scope">
                <el-button link type="danger" @click="removeRuleRow(scope.$index)">
                  {{ t('taxExports.dialog.actions.removeRow') }}
                </el-button>
              </template>
            </el-table-column>
          </el-table>

          <div class="tax-exports-view__dialog-row-actions">
            <el-button @click="addRuleRow">{{ t('taxExports.dialog.actions.addRow') }}</el-button>
          </div>
        </div>
      </div>

      <template #footer>
        <el-button @click="dialogVisible = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isSubmitting" :disabled="!canSubmitDialog" @click="handleSubmit">
          {{ t('taxExports.dialog.actions.submit') }}
        </el-button>
      </template>
    </el-dialog>
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

.tax-exports-view__card-header-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
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

.tax-exports-view__table :deep(.el-table__row) {
  cursor: pointer;
}

.tax-exports-view__dialog-body {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
  max-height: calc(100vh - var(--mi-space-6) * 4);
  overflow: auto;
}

.tax-exports-view__dialog-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.tax-exports-view__dialog-section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.tax-exports-view__dialog-section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.tax-exports-view__dialog-table {
  width: 100%;
}

.tax-exports-view__dialog-row-actions {
  display: flex;
  justify-content: flex-end;
}

@media (max-width: 52rem) {
  .tax-exports-view__export-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .tax-exports-view__card-header,
  .tax-exports-view__card-header-actions {
    align-items: flex-start;
    flex-direction: column;
  }

  .tax-exports-view__dialog-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .tax-exports-view__dialog-row-actions {
    justify-content: flex-start;
  }
}
</style>
