<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

import { cancelInvoice, listInvoices, submitInvoice, type InvoiceDocument } from '../api/invoice'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

const router = useRouter()

const rows = ref<InvoiceDocument[]>([])
const total = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')

const { filters, isDirty, reset } = useFilterForm({
  document_type: '',
  status: '',
})

const documentTypeOptions = [
  { label: 'Bill', value: 'bill' },
  { label: 'Invoice', value: 'invoice' },
]

const statusOptions = [
  { label: 'Draft', value: 'draft' },
  { label: 'Pending approval', value: 'pending_approval' },
  { label: 'Approved', value: 'approved' },
  { label: 'Cancelled', value: 'cancelled' },
]

const formatAmount = (value: number) =>
  new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

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
    errorMessage.value = error instanceof Error ? error.message : 'Unable to load billing invoices.'
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
    errorMessage.value = error instanceof Error ? error.message : 'Failed to submit invoice.'
  }
}

const handleCancel = async (row: InvoiceDocument) => {
  try {
    await cancelInvoice(row.id)
    void loadInvoices()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Failed to cancel invoice.'
  }
}

onMounted(() => {
  void loadInvoices()
})
</script>

<template>
  <div class="billing-invoices-view" data-testid="billing-invoices-view">
    <PageSection
      eyebrow="Billing operations"
      title="Billing invoices"
      summary="Review billing documents, manage lifecycle from draft through approval, and navigate to detail for adjustments."
    />

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="billing-invoices-view__alert"
      title="Invoice records unavailable"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <FilterForm
      title="Invoice filters"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="loadInvoices"
    >
      <el-form-item label="Document type">
        <el-select
          v-model="filters.document_type"
          placeholder="All types"
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

      <el-form-item label="Status">
        <el-select
          v-model="filters.status"
          placeholder="All statuses"
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
          <span>Invoice queue</span>
          <el-tag effect="plain" type="info">{{ total }} total</el-tag>
        </div>
      </template>

      <el-table
        :data="rows"
        row-key="id"
        class="billing-invoices-view__table"
        empty-text="No billing invoices match the current filters."
        data-testid="invoices-table"
      >
        <el-table-column prop="document_no" label="Document no." min-width="180" />
        <el-table-column label="Type" min-width="100">
          <template #default="scope">
            <el-tag :type="documentTypeTag(scope.row.document_type)" effect="plain">
              {{ scope.row.document_type }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="tenant_name" label="Tenant" min-width="220" />
        <el-table-column label="Period" min-width="220">
          <template #default="scope">
            {{ scope.row.period_start }} → {{ scope.row.period_end }}
          </template>
        </el-table-column>
        <el-table-column label="Total" min-width="140" align="right" header-align="right">
          <template #default="scope">
            {{ formatAmount(scope.row.total_amount) }}
          </template>
        </el-table-column>
        <el-table-column label="Status" min-width="140">
          <template #default="scope">
            <el-tag :type="statusTag(scope.row.status)" effect="plain">
              {{ scope.row.status }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="Actions" min-width="200" fixed="right">
          <template #default="scope">
            <el-button link type="primary" @click.stop="openInvoice(scope.row.id)">View</el-button>
            <el-button
              v-if="scope.row.status === 'draft'"
              link
              type="primary"
              @click.stop="handleSubmit(scope.row)"
            >
              Submit
            </el-button>
            <el-button
              v-if="scope.row.status === 'draft' || scope.row.status === 'pending_approval'"
              link
              type="danger"
              @click.stop="handleCancel(scope.row)"
            >
              Cancel
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
