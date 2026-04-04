<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

import {
  getInvoice,
  submitInvoice,
  cancelInvoice,
  type InvoiceDocument,
} from '../api/invoice'
import PageSection from '../components/platform/PageSection.vue'

const route = useRoute()
const router = useRouter()

const invoice = ref<InvoiceDocument | null>(null)
const errorMessage = ref('')
const successMessage = ref('')
const isLoading = ref(false)
const isSubmitting = ref(false)
const isCancelling = ref(false)

const invoiceId = computed(() => {
  const rawId = route.params.id
  const normalizedId = Array.isArray(rawId) ? rawId[0] : rawId
  const parsedId = Number(normalizedId)

  return Number.isFinite(parsedId) && parsedId > 0 ? parsedId : null
})

const formatAmount = (value: number) =>
  new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const formatTimestamp = (value: string | null) => {
  if (!value) {
    return '—'
  }

  return new Intl.DateTimeFormat('en-US', {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value))
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

const loadInvoice = async () => {
  if (!invoiceId.value) {
    errorMessage.value = 'The requested invoice id is invalid.'
    invoice.value = null
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await getInvoice(invoiceId.value)
    invoice.value = response.data.document
  } catch (error) {
    invoice.value = null
    errorMessage.value = error instanceof Error ? error.message : 'Unable to load the invoice document.'
  } finally {
    isLoading.value = false
  }
}

const handleBack = async () => {
  await router.push({ name: 'billing-invoices' })
}

const handleSubmit = async () => {
  if (!invoice.value) {
    return
  }

  isSubmitting.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const response = await submitInvoice(invoice.value.id, {
      idempotency_key: crypto.randomUUID(),
    })

    invoice.value = response.data.document
    successMessage.value = 'Invoice submitted for approval.'
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unable to submit the invoice.'
  } finally {
    isSubmitting.value = false
  }
}

const handleCancel = async () => {
  if (!invoice.value) {
    return
  }

  isCancelling.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const response = await cancelInvoice(invoice.value.id)
    invoice.value = response.data.document
    successMessage.value = 'Invoice cancelled.'
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unable to cancel the invoice.'
  } finally {
    isCancelling.value = false
  }
}

onMounted(() => {
  void loadInvoice()
})

watch(
  () => route.params.id,
  () => {
    void loadInvoice()
  },
)
</script>

<template>
  <div class="invoice-detail-view" data-testid="invoice-detail-view">
    <PageSection
      eyebrow="Billing operations"
      :title="invoice ? (invoice.document_no ?? `Document #${invoice.id}`) : 'Invoice detail'"
      summary="Review document details, lifecycle state, and line items for the selected billing document."
    >
      <template #actions>
        <el-tag v-if="invoice" :type="statusTag(invoice.status)" effect="plain">
          {{ invoice.status }}
        </el-tag>
        <el-button @click="handleBack">Back to list</el-button>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      title="Invoice detail unavailable"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <el-alert
      v-if="successMessage"
      :closable="false"
      title="Invoice action completed"
      type="success"
      show-icon
      :description="successMessage"
    />

    <el-skeleton v-if="isLoading" :rows="6" animated />

    <template v-else-if="invoice">
      <section class="invoice-detail-view__grid">
        <el-card class="invoice-detail-view__card" shadow="never">
          <template #header>
            <div class="invoice-detail-view__card-header">
              <span>Document overview</span>
            </div>
          </template>

          <el-descriptions :column="2" border>
            <el-descriptions-item label="Document no.">
              {{ invoice.document_no ?? 'Not assigned' }}
            </el-descriptions-item>
            <el-descriptions-item label="Type">
              <el-tag :type="documentTypeTag(invoice.document_type)" effect="plain">
                {{ invoice.document_type }}
              </el-tag>
            </el-descriptions-item>
            <el-descriptions-item label="Tenant">{{ invoice.tenant_name }}</el-descriptions-item>
            <el-descriptions-item label="Lease contract">{{ invoice.lease_contract_id }}</el-descriptions-item>
            <el-descriptions-item label="Billing run">{{ invoice.billing_run_id }}</el-descriptions-item>
            <el-descriptions-item label="Total amount">
              {{ formatAmount(invoice.total_amount) }}
            </el-descriptions-item>
            <el-descriptions-item label="Period start">{{ invoice.period_start }}</el-descriptions-item>
            <el-descriptions-item label="Period end">{{ invoice.period_end }}</el-descriptions-item>
            <el-descriptions-item label="Status">{{ invoice.status }}</el-descriptions-item>
            <el-descriptions-item label="Workflow instance">
              {{ invoice.workflow_instance_id ?? 'Not created yet' }}
            </el-descriptions-item>
            <el-descriptions-item label="Submitted at">
              {{ formatTimestamp(invoice.submitted_at) }}
            </el-descriptions-item>
            <el-descriptions-item label="Approved at">
              {{ formatTimestamp(invoice.approved_at) }}
            </el-descriptions-item>
            <el-descriptions-item label="Cancelled at">
              {{ formatTimestamp(invoice.cancelled_at) }}
            </el-descriptions-item>
            <el-descriptions-item label="Created at">
              {{ formatTimestamp(invoice.created_at) }}
            </el-descriptions-item>
          </el-descriptions>
        </el-card>

        <el-card class="invoice-detail-view__card" shadow="never">
          <template #header>
            <div class="invoice-detail-view__card-header">
              <span>Workflow actions</span>
            </div>
          </template>

          <div class="invoice-detail-view__actions">
            <el-button
              v-if="invoice.status === 'draft'"
              type="primary"
              :loading="isSubmitting"
              data-testid="invoice-submit-button"
              @click="handleSubmit"
            >
              Submit for approval
            </el-button>

            <el-button
              v-if="invoice.status === 'draft' || invoice.status === 'pending_approval'"
              type="danger"
              plain
              :loading="isCancelling"
              data-testid="invoice-cancel-button"
              @click="handleCancel"
            >
              Cancel invoice
            </el-button>

            <el-tag
              v-if="invoice.status === 'approved' || invoice.status === 'cancelled'"
              effect="plain"
              :type="invoice.status === 'approved' ? 'success' : 'danger'"
            >
              No further actions available — document is {{ invoice.status }}.
            </el-tag>
          </div>
        </el-card>
      </section>

      <el-card class="invoice-detail-view__card" shadow="never">
        <template #header>
          <div class="invoice-detail-view__card-header">
            <span>Line items</span>
            <el-tag effect="plain" type="info">{{ invoice.lines.length }} lines</el-tag>
          </div>
        </template>

        <el-table
          :data="invoice.lines"
          row-key="id"
          class="invoice-detail-view__table"
          empty-text="No line items attached."
        >
          <el-table-column prop="charge_type" label="Charge type" min-width="140" />
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
        </el-table>
      </el-card>
    </template>
  </div>
</template>

<style scoped>
.invoice-detail-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.invoice-detail-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.invoice-detail-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.invoice-detail-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.invoice-detail-view__actions {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.invoice-detail-view__table {
  width: 100%;
}

@media (max-width: 52rem) {
  .invoice-detail-view__grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
