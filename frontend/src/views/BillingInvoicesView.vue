<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

import { cancelInvoice, listInvoices, submitInvoice, type InvoiceDocument } from '../api/invoice'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { getErrorMessage } from '../composables/useErrorMessage'
import { usePagination } from '../composables/usePagination'
import { useAppStore } from '../stores/app'
import { formatDate } from '../utils/format'

const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()

const rows = ref<InvoiceDocument[]>([])
const { page, pageSize, total, paginationParams, resetPage, handlePageChange, handleSizeChange } = usePagination()
const isLoading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')
const actionDocumentId = ref<number | null>(null)

const { filters, isDirty, reset } = useFilterForm({
  document_type: '',
  status: '',
})

const documentTypeOptions = computed(() => [
  { label: t('billingInvoices.documentTypes.bill'), value: 'bill' },
  { label: t('billingInvoices.documentTypes.invoice'), value: 'invoice' },
])

const statusOptions = computed(() => [
  { label: t('common.statuses.draft'), value: 'draft' },
  { label: t('common.statuses.pendingApproval'), value: 'pending_approval' },
  { label: t('common.statuses.approved'), value: 'approved' },
  { label: t('common.statuses.adjusted'), value: 'adjusted' },
  { label: t('common.statuses.cancelled'), value: 'cancelled' },
])

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

const resolveStatusLabel = (status: string) => {
  switch (status) {
    case 'draft':
      return t('common.statuses.draft')
    case 'pending_approval':
      return t('common.statuses.pendingApproval')
    case 'approved':
      return t('common.statuses.approved')
    case 'adjusted':
      return t('common.statuses.adjusted')
    case 'cancelled':
      return t('common.statuses.cancelled')
    default:
      return status
  }
}

const documentTypeTag = (type: string) => (type === 'bill' ? 'warning' : 'primary')
const statusTag = (status: string) => {
  switch (status) {
    case 'draft':
      return 'info'
    case 'pending_approval':
      return 'warning'
    case 'approved':
      return 'success'
    case 'adjusted':
      return 'warning'
    case 'cancelled':
      return 'danger'
    default:
      return 'info'
  }
}

const loadInvoices = async () => {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await listInvoices({
      document_type: filters.document_type || undefined,
      status: filters.status || undefined,
      ...paginationParams.value,
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('billingInvoices.errors.unableToLoad'))
    rows.value = []
    total.value = 0
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  resetPage()
  void loadInvoices()
}

const handlePaginationPageChange = (newPage: number) => {
  handlePageChange(newPage)
  void loadInvoices()
}

const handlePaginationSizeChange = (newSize: number) => {
  handleSizeChange(newSize)
  void loadInvoices()
}

const openInvoice = (id: number) => {
  router.push({ name: 'billing-invoice-detail', params: { id: String(id) } })
}

const openInvoiceAdjustment = (id: number) => {
  router.push({ name: 'billing-invoice-detail', params: { id: String(id) } })
}

const handleSubmit = async (row: InvoiceDocument) => {
  actionDocumentId.value = row.id
  errorMessage.value = ''
  successMessage.value = ''

  try {
    await submitInvoice(row.id, { idempotency_key: crypto.randomUUID() })
    successMessage.value = t('billingInvoices.actions.submit')
    await loadInvoices()
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('billingInvoices.errors.unableToSubmit'))
  } finally {
    actionDocumentId.value = null
  }
}

const handleCancel = async (row: InvoiceDocument) => {
  actionDocumentId.value = row.id
  errorMessage.value = ''
  successMessage.value = ''

  try {
    await cancelInvoice(row.id)
    successMessage.value = t('billingInvoices.actions.cancel')
    await loadInvoices()
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('billingInvoices.errors.unableToCancel'))
  } finally {
    actionDocumentId.value = null
  }
}

onMounted(() => {
  void loadInvoices()
})
</script>

<template>
  <div class="billing-invoices-view" v-loading="isLoading" data-testid="billing-invoices-view">
    <PageSection
      :eyebrow="t('billingInvoices.eyebrow')"
      :title="t('billingInvoices.title')"
      :summary="t('billingInvoices.summary')"
    />

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="billing-invoices-view__alert"
      :title="t('billingInvoices.errors.recordsUnavailable')"
      type="error"
      show-icon
      :description="errorMessage"
      data-testid="billing-invoices-error-alert"
    />

    <el-alert
      v-if="successMessage"
      :closable="false"
      class="billing-invoices-view__alert"
      :title="t('invoiceDetail.feedback.actionCompleted')"
      type="success"
      show-icon
      :description="successMessage"
      data-testid="billing-invoices-success-alert"
    />

    <FilterForm
      :title="t('billingInvoices.filters.title')"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="resetPage(); loadInvoices()"
    >
      <el-form-item :label="t('billingInvoices.fields.documentType')">
        <el-select
          v-model="filters.document_type"
          :placeholder="t('billingInvoices.placeholders.allTypes')"
          clearable
          data-testid="invoice-document-type-filter"
        >
          <el-option
            v-for="option in documentTypeOptions"
            :key="option.value"
            :label="option.label"
            :value="option.value"
          />
        </el-select>
      </el-form-item>

      <el-form-item :label="t('common.columns.status')">
        <el-select
          v-model="filters.status"
          :placeholder="t('billingInvoices.placeholders.allStatuses')"
          clearable
          data-testid="invoice-status-filter"
        >
          <el-option
            v-for="option in statusOptions"
            :key="option.value"
            :label="option.label"
            :value="option.value"
          />
        </el-select>
      </el-form-item>
    </FilterForm>

    <el-card class="billing-invoices-view__table-card" shadow="never">
      <template #header>
        <div class="billing-invoices-view__table-header">
          <span>{{ t('billingInvoices.table.queueTitle') }}</span>
          <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
        </div>
      </template>

      <el-table
        :data="rows"
        row-key="id"
        class="billing-invoices-view__table"
        :empty-text="t('billingInvoices.table.empty')"
        data-testid="invoices-table"
      >
        <el-table-column prop="document_no" :label="t('billingInvoices.fields.documentNumber')" min-width="180" />
        <el-table-column :label="t('billingInvoices.fields.type')" min-width="100">
          <template #default="scope">
            <el-tag :type="documentTypeTag(scope.row.document_type)" effect="plain">
              {{ resolveDocumentTypeLabel(scope.row.document_type) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="tenant_name" :label="t('billingInvoices.fields.tenant')" min-width="220" />
        <el-table-column :label="t('billingInvoices.fields.period')" min-width="220">
          <template #default="scope">
            {{ formatDate(scope.row.period_start) }} → {{ formatDate(scope.row.period_end) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('billingInvoices.fields.total')" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.total_amount) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('billingInvoices.fields.adjustedFrom')" min-width="160">
          <template #default="scope">
            <template v-if="scope.row.adjusted_from_id !== null">
              {{ t('invoiceDetail.defaults.documentId', { id: scope.row.adjusted_from_id }) }}
            </template>
            <template v-else>{{ t('common.emptyValue') }}</template>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.status')" min-width="140">
          <template #default="scope">
            <el-tag :type="statusTag(scope.row.status)" effect="plain">
              {{ resolveStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="260" fixed="right">
          <template #default="scope">
            <el-button link type="primary" :data-testid="`invoice-row-view-button-${scope.row.id}`" @click.stop="openInvoice(scope.row.id)">
              {{ t('common.actions.view') }}
            </el-button>
            <el-button
              link
              type="primary"
              :disabled="scope.row.status !== 'draft' || actionDocumentId === scope.row.id"
              :data-testid="`invoice-row-submit-button-${scope.row.id}`"
              @click.stop="handleSubmit(scope.row)"
            >
              {{ t('billingInvoices.actions.submit') }}
            </el-button>
            <el-button
              link
              type="warning"
              :disabled="scope.row.status !== 'approved'"
              :data-testid="`invoice-row-adjust-button-${scope.row.id}`"
              @click.stop="openInvoiceAdjustment(scope.row.id)"
            >
              {{ t('billingInvoices.actions.adjust') }}
            </el-button>
            <el-button
              link
              type="danger"
              :disabled="scope.row.status !== 'approved' || actionDocumentId === scope.row.id"
              :data-testid="`invoice-row-cancel-button-${scope.row.id}`"
              @click.stop="handleCancel(scope.row)"
            >
              {{ t('billingInvoices.actions.cancel') }}
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="billing-invoices-view__pagination">
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
.billing-invoices-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.billing-invoices-view__alert {
  margin-bottom: 0;
}

.billing-invoices-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.billing-invoices-view__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.billing-invoices-view__table {
  width: 100%;
}

.billing-invoices-view__pagination {
  display: flex;
  justify-content: flex-end;
  margin-top: var(--mi-space-4);
}

@media (max-width: 52rem) {
  .billing-invoices-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
