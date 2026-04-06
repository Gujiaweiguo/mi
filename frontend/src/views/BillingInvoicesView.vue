<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

import { cancelInvoice, listInvoices, submitInvoice, type InvoiceDocument } from '../api/invoice'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { useAppStore } from '../stores/app'

const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()

const rows = ref<InvoiceDocument[]>([])
const total = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')

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
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('billingInvoices.errors.unableToLoad')
    rows.value = []
    total.value = 0
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  void loadInvoices()
}

const openInvoice = (id: number) => {
  router.push({ name: 'billing-invoice-detail', params: { id: String(id) } })
}

const handleSubmit = async (row: InvoiceDocument) => {
  try {
    await submitInvoice(row.id, { idempotency_key: crypto.randomUUID() })
    void loadInvoices()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('billingInvoices.errors.unableToSubmit')
  }
}

const handleCancel = async (row: InvoiceDocument) => {
  try {
    await cancelInvoice(row.id)
    void loadInvoices()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('billingInvoices.errors.unableToCancel')
  }
}

onMounted(() => {
  void loadInvoices()
})
</script>

<template>
  <div class="billing-invoices-view" data-testid="billing-invoices-view">
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
    />

    <FilterForm
      :title="t('billingInvoices.filters.title')"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="loadInvoices"
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
            {{ scope.row.period_start }} → {{ scope.row.period_end }}
          </template>
        </el-table-column>
        <el-table-column :label="t('billingInvoices.fields.total')" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.total_amount) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.status')" min-width="140">
          <template #default="scope">
            <el-tag :type="statusTag(scope.row.status)" effect="plain">
              {{ resolveStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="200" fixed="right">
          <template #default="scope">
            <el-button link type="primary" @click.stop="openInvoice(scope.row.id)">
              {{ t('common.actions.view') }}
            </el-button>
            <el-button
              v-if="scope.row.status === 'draft'"
              link
              type="primary"
              @click.stop="handleSubmit(scope.row)"
            >
              {{ t('billingInvoices.actions.submit') }}
            </el-button>
            <el-button
              v-if="scope.row.status === 'draft' || scope.row.status === 'pending_approval'"
              link
              type="danger"
              @click.stop="handleCancel(scope.row)"
            >
              {{ t('billingInvoices.actions.cancel') }}
            </el-button>
          </template>
        </el-table-column>
      </el-table>
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

@media (max-width: 52rem) {
  .billing-invoices-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
