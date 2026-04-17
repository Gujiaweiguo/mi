<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

import { listReceivables, type ReceivableListItem } from '../api/invoice'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { usePagination } from '../composables/usePagination'
import { useAppStore } from '../stores/app'

const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()

const rows = ref<ReceivableListItem[]>([])
const { page, pageSize, total, paginationParams, resetPage, handlePageChange, handleSizeChange } = usePagination()
const isLoading = ref(false)
const errorMessage = ref('')

const { filters, isDirty, reset } = useFilterForm({
  customer_id: '',
  department_id: '',
  due_date_start: '',
  due_date_end: '',
})

const isDueDateRangeInvalid = computed(
  () => Boolean(filters.due_date_start && filters.due_date_end) && filters.due_date_start > filters.due_date_end,
)

const parseOptionalPositiveInteger = (value: string) => {
  const normalized = value.trim()
  if (!normalized) {
    return undefined
  }

  const parsed = Number(normalized)
  if (Number.isInteger(parsed) && parsed > 0) {
    return parsed
  }

  return undefined
}

const formatAmount = (value: number) =>
  new Intl.NumberFormat(appStore.locale, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const resolveDocumentTypeLabel = (type: string) => {
  switch (type) {
    case 'bill':
      return t('billingInvoices.documentTypes.bill')
    case 'invoice':
      return t('billingInvoices.documentTypes.invoice')
    default:
      return type
  }
}

const resolveDocumentStatusLabel = (status: string) => {
  switch (status) {
    case 'draft':
      return t('common.statuses.draft')
    case 'pending_approval':
      return t('common.statuses.pendingApproval')
    case 'approved':
      return t('common.statuses.approved')
    case 'rejected':
      return t('common.statuses.rejected')
    case 'cancelled':
      return t('common.statuses.cancelled')
    default:
      return status
  }
}

const resolveSettlementLabel = (status: string) => {
  switch (status) {
    case 'outstanding':
      return t('receivables.settlement.outstanding')
    case 'settled':
      return t('receivables.settlement.settled')
    default:
      return status
  }
}

const documentStatusTag = (status: string) => {
  switch (status) {
    case 'approved':
      return 'success'
    case 'pending_approval':
      return 'warning'
    case 'cancelled':
      return 'danger'
    default:
      return 'info'
  }
}

const settlementTag = (status: string) => (status === 'settled' ? 'success' : 'warning')

const loadReceivables = async () => {
  if (isDueDateRangeInvalid.value) {
    errorMessage.value = t('receivables.errors.invalidDueDateRange')
    rows.value = []
    total.value = 0
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await listReceivables({
      customer_id: parseOptionalPositiveInteger(filters.customer_id),
      department_id: parseOptionalPositiveInteger(filters.department_id),
      due_date_start: filters.due_date_start || undefined,
      due_date_end: filters.due_date_end || undefined,
      ...paginationParams.value,
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    rows.value = []
    total.value = 0
    errorMessage.value = error instanceof Error ? error.message : t('receivables.errors.unableToLoad')
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  resetPage()
  void loadReceivables()
}

const handlePaginationPageChange = (newPage: number) => {
  handlePageChange(newPage)
  void loadReceivables()
}

const handlePaginationSizeChange = (newSize: number) => {
  handleSizeChange(newSize)
  void loadReceivables()
}

const openInvoice = (id: number) => {
  router.push({ name: 'billing-invoice-detail', params: { id: String(id) } })
}

onMounted(() => {
  void loadReceivables()
})
</script>

<template>
  <div class="receivables-view" data-testid="receivables-view">
    <PageSection :eyebrow="t('receivables.eyebrow')" :title="t('receivables.title')" :summary="t('receivables.summary')" />

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="receivables-view__alert"
      :title="t('receivables.errors.recordsUnavailable')"
      type="error"
      show-icon
      :description="errorMessage"
      data-testid="receivables-error-alert"
    />

    <FilterForm
      :title="t('receivables.filters.title')"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="resetPage(); loadReceivables()"
    >
      <el-form-item :label="t('receivables.fields.customerId')">
        <el-input
          v-model="filters.customer_id"
          :placeholder="t('receivables.placeholders.customerId')"
          clearable
          data-testid="receivables-customer-id-filter"
        />
      </el-form-item>

      <el-form-item :label="t('receivables.fields.departmentId')">
        <el-input
          v-model="filters.department_id"
          :placeholder="t('receivables.placeholders.departmentId')"
          clearable
          data-testid="receivables-department-id-filter"
        />
      </el-form-item>

      <el-form-item :label="t('receivables.fields.dueDateStart')">
        <el-date-picker
          v-model="filters.due_date_start"
          type="date"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          :placeholder="t('receivables.placeholders.dueDateStart')"
          class="receivables-view__filter-input"
          data-testid="receivables-due-date-start-filter"
        />
      </el-form-item>

      <el-form-item :label="t('receivables.fields.dueDateEnd')">
        <el-date-picker
          v-model="filters.due_date_end"
          type="date"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          :placeholder="t('receivables.placeholders.dueDateEnd')"
          class="receivables-view__filter-input"
          data-testid="receivables-due-date-end-filter"
        />
      </el-form-item>
    </FilterForm>

    <el-card class="receivables-view__table-card" shadow="never">
      <template #header>
        <div class="receivables-view__table-header">
          <span>{{ t('receivables.table.title') }}</span>
          <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
        </div>
      </template>

      <el-table
        :data="rows"
        row-key="billing_document_id"
        class="receivables-view__table"
        :empty-text="t('receivables.table.empty')"
        data-testid="receivables-table"
      >
        <el-table-column prop="document_no" :label="t('receivables.fields.documentNumber')" min-width="170" />
        <el-table-column :label="t('receivables.fields.documentType')" min-width="120">
          <template #default="scope">
            <el-tag effect="plain" :type="scope.row.document_type === 'invoice' ? 'primary' : 'warning'">
              {{ resolveDocumentTypeLabel(scope.row.document_type) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="tenant_name" :label="t('receivables.fields.tenant')" min-width="180" />
        <el-table-column :label="t('receivables.fields.documentStatus')" min-width="140">
          <template #default="scope">
            <el-tag effect="plain" :type="documentStatusTag(scope.row.document_status)">
              {{ resolveDocumentStatusLabel(scope.row.document_status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('receivables.fields.dueDateWindow')" min-width="220">
          <template #default="scope">
            {{ scope.row.earliest_due_date }} → {{ scope.row.latest_due_date }}
          </template>
        </el-table-column>
        <el-table-column :label="t('receivables.fields.outstandingAmount')" min-width="170" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.outstanding_amount) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('receivables.fields.settlementStatus')" min-width="150">
          <template #default="scope">
            <el-tag effect="plain" :type="settlementTag(scope.row.settlement_status)">
              {{ resolveSettlementLabel(scope.row.settlement_status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="130" fixed="right">
          <template #default="scope">
            <el-button
              link
              type="primary"
              :data-testid="`receivable-row-view-button-${scope.row.billing_document_id}`"
              @click.stop="openInvoice(scope.row.billing_document_id)"
            >
              {{ t('common.actions.view') }}
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="receivables-view__pagination">
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
.receivables-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.receivables-view__alert {
  margin-bottom: 0;
}

.receivables-view__filter-input,
.receivables-view__table {
  width: 100%;
}

.receivables-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.receivables-view__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.receivables-view__pagination {
  display: flex;
  justify-content: flex-end;
  margin-top: var(--mi-space-4);
}

@media (max-width: 52rem) {
  .receivables-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
