<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'

import {
  getInvoice,
  submitInvoice,
  cancelInvoice,
  type InvoiceDocument,
} from '../api/invoice'
import PageSection from '../components/platform/PageSection.vue'
import { useAppStore } from '../stores/app'

const route = useRoute()
const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()

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

const pageTitle = computed(() => {
  if (!invoice.value) {
    return t('invoiceDetail.title')
  }

  return invoice.value.document_no ?? t('invoiceDetail.defaults.documentId', { id: invoice.value.id })
})

const formatAmount = (value: number) =>
  new Intl.NumberFormat(appStore.locale, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const formatTimestamp = (value: string | null) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value))
}

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

const loadInvoice = async () => {
  if (!invoiceId.value) {
    errorMessage.value = t('invoiceDetail.errors.invalidId')
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
    errorMessage.value = error instanceof Error ? error.message : t('invoiceDetail.errors.unableToLoad')
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
    successMessage.value = t('invoiceDetail.feedback.submitted')
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('invoiceDetail.errors.unableToSubmit')
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
    successMessage.value = t('invoiceDetail.feedback.cancelled')
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('invoiceDetail.errors.unableToCancel')
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
      :eyebrow="t('billingInvoices.eyebrow')"
      :title="pageTitle"
      :summary="t('invoiceDetail.summary')"
    >
      <template #actions>
        <el-tag v-if="invoice" :type="statusTag(invoice.status)" effect="plain">
          {{ resolveStatusLabel(invoice.status) }}
        </el-tag>
        <el-button @click="handleBack">{{ t('invoiceDetail.actions.backToList') }}</el-button>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      :title="t('invoiceDetail.errors.detailUnavailable')"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <el-alert
      v-if="successMessage"
      :closable="false"
      :title="t('invoiceDetail.feedback.actionCompleted')"
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
              <span>{{ t('invoiceDetail.cards.overview') }}</span>
            </div>
          </template>

          <el-descriptions :column="2" border>
            <el-descriptions-item :label="t('invoiceDetail.fields.documentNumber')">
              {{ invoice.document_no ?? t('invoiceDetail.defaults.notAssigned') }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.type')">
              <el-tag :type="documentTypeTag(invoice.document_type)" effect="plain">
                {{ resolveDocumentTypeLabel(invoice.document_type) }}
              </el-tag>
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.tenant')">{{ invoice.tenant_name }}</el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.leaseContract')">{{ invoice.lease_contract_id }}</el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.billingRun')">{{ invoice.billing_run_id }}</el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.totalAmount')">
              {{ formatAmount(invoice.total_amount) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.periodStart')">{{ invoice.period_start }}</el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.periodEnd')">{{ invoice.period_end }}</el-descriptions-item>
            <el-descriptions-item :label="t('common.columns.status')">{{ resolveStatusLabel(invoice.status) }}</el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.workflowInstance')">
              {{ invoice.workflow_instance_id ?? t('invoiceDetail.defaults.notCreatedYet') }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.submittedAt')">
              {{ formatTimestamp(invoice.submitted_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.approvedAt')">
              {{ formatTimestamp(invoice.approved_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.cancelledAt')">
              {{ formatTimestamp(invoice.cancelled_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('common.columns.createdAt')">
              {{ formatTimestamp(invoice.created_at) }}
            </el-descriptions-item>
          </el-descriptions>
        </el-card>

        <el-card class="invoice-detail-view__card" shadow="never">
          <template #header>
            <div class="invoice-detail-view__card-header">
              <span>{{ t('invoiceDetail.cards.workflowActions') }}</span>
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
              {{ t('invoiceDetail.actions.submitForApproval') }}
            </el-button>

            <el-button
              v-if="invoice.status === 'draft' || invoice.status === 'pending_approval'"
              type="danger"
              plain
              :loading="isCancelling"
              data-testid="invoice-cancel-button"
              @click="handleCancel"
            >
              {{ t('invoiceDetail.actions.cancelDocument') }}
            </el-button>

            <el-tag
              v-if="invoice.status === 'approved' || invoice.status === 'cancelled'"
              effect="plain"
              :type="invoice.status === 'approved' ? 'success' : 'danger'"
            >
              {{ t('invoiceDetail.actions.noFurtherActions', { status: resolveStatusLabel(invoice.status) }) }}
            </el-tag>
          </div>
        </el-card>
      </section>

      <el-card class="invoice-detail-view__card" shadow="never">
        <template #header>
          <div class="invoice-detail-view__card-header">
            <span>{{ t('invoiceDetail.cards.lineItems') }}</span>
            <el-tag effect="plain" type="info">{{ t('invoiceDetail.table.lineCount', { count: invoice.lines.length }) }}</el-tag>
          </div>
        </template>

        <el-table
          :data="invoice.lines"
          row-key="id"
          class="invoice-detail-view__table"
          :empty-text="t('invoiceDetail.table.empty')"
        >
          <el-table-column prop="charge_type" :label="t('invoiceDetail.fields.chargeType')" min-width="140" />
          <el-table-column :label="t('invoiceDetail.fields.period')" min-width="220">
            <template #default="scope">
              {{ scope.row.period_start }} → {{ scope.row.period_end }}
            </template>
          </el-table-column>
          <el-table-column prop="quantity_days" :label="t('invoiceDetail.fields.days')" min-width="90" />
          <el-table-column :label="t('invoiceDetail.fields.unitAmount')" min-width="140" align="right" header-align="right">
            <template #default="scope">
              {{ formatAmount(scope.row.unit_amount) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('invoiceDetail.fields.amount')" min-width="140" align="right" header-align="right">
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
