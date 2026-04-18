<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'

import {
  getInvoiceReceivable,
  getInvoice,
  recordInvoicePayment,
  submitInvoice,
  cancelInvoice,
  type InvoiceDocument,
  type InvoiceReceivable,
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
const receivable = ref<InvoiceReceivable | null>(null)
const receivableErrorMessage = ref('')
const isReceivableLoading = ref(false)
const isRecordingPayment = ref(false)
const paymentAmount = ref<number | null>(null)
const paymentDate = ref('')
const paymentNote = ref('')

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

const submitDisabled = computed(() => !invoice.value || isSubmitting.value || invoice.value.status !== 'draft')

const cancelDisabled = computed(
  () =>
    !invoice.value ||
    isCancelling.value ||
    invoice.value.status !== 'approved' ||
    isReceivableLoading.value ||
    (receivable.value?.payment_history.length ?? 0) > 0,
)

const hasReceivableAccess = computed(() => invoice.value?.status === 'approved')

const recordPaymentDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !receivable.value ||
    receivable.value.outstanding_amount <= 0 ||
    paymentAmount.value === null ||
    paymentAmount.value <= 0 ||
    isRecordingPayment.value,
)

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

const settlementTag = (status: string) => (status === 'settled' ? 'success' : 'warning')

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

const resetPaymentForm = () => {
  paymentAmount.value = null
  paymentDate.value = ''
  paymentNote.value = ''
}

const loadReceivable = async () => {
  if (!invoiceId.value || !hasReceivableAccess.value) {
    receivable.value = null
    receivableErrorMessage.value = ''
    resetPaymentForm()
    return
  }

  isReceivableLoading.value = true
  receivableErrorMessage.value = ''

  try {
    const response = await getInvoiceReceivable(invoiceId.value)
    receivable.value = response.data.receivable
  } catch (error) {
    receivable.value = null
    receivableErrorMessage.value = error instanceof Error ? error.message : t('invoiceDetail.errors.unableToLoadReceivable')
  } finally {
    isReceivableLoading.value = false
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
    await loadReceivable()
  } catch (error) {
    invoice.value = null
    receivable.value = null
    receivableErrorMessage.value = ''
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

const handleRecordPayment = async () => {
  if (!invoice.value || !receivable.value || paymentAmount.value === null || paymentAmount.value <= 0) {
    return
  }

  isRecordingPayment.value = true
  errorMessage.value = ''
  successMessage.value = ''
  receivableErrorMessage.value = ''

  try {
    const response = await recordInvoicePayment(invoice.value.id, {
      amount: paymentAmount.value,
      payment_date: paymentDate.value || undefined,
      note: paymentNote.value.trim() || undefined,
      idempotency_key: crypto.randomUUID(),
    })

    receivable.value = response.data.receivable
    successMessage.value = t('invoiceDetail.feedback.paymentRecorded')
    resetPaymentForm()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('invoiceDetail.errors.unableToRecordPayment')
  } finally {
    isRecordingPayment.value = false
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
  <div class="invoice-detail-view" v-loading="isLoading || isReceivableLoading" data-testid="invoice-detail-view">
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
      data-testid="invoice-detail-error-alert"
    />

    <el-alert
      v-if="successMessage"
      :closable="false"
      :title="t('invoiceDetail.feedback.actionCompleted')"
      type="success"
      show-icon
      :description="successMessage"
      data-testid="invoice-detail-success-alert"
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
              type="primary"
              :loading="isSubmitting"
              :disabled="submitDisabled"
              data-testid="invoice-submit-button"
              @click="handleSubmit"
            >
              {{ t('invoiceDetail.actions.submitForApproval') }}
            </el-button>

            <el-button
              type="danger"
              plain
              :loading="isCancelling"
              :disabled="cancelDisabled"
              data-testid="invoice-cancel-button"
              @click="handleCancel"
            >
              {{ t('invoiceDetail.actions.cancelDocument') }}
            </el-button>

            <el-tag
              v-if="submitDisabled && cancelDisabled"
              effect="plain"
              :type="invoice.status === 'approved' ? 'success' : 'info'"
              data-testid="invoice-no-actions-tag"
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

      <el-card class="invoice-detail-view__card" shadow="never" data-testid="invoice-receivable-card">
        <template #header>
          <div class="invoice-detail-view__card-header">
            <span>{{ t('invoiceDetail.cards.receivable') }}</span>
            <el-tag v-if="receivable" effect="plain" :type="settlementTag(receivable.settlement_status)">
              {{ resolveSettlementLabel(receivable.settlement_status) }}
            </el-tag>
          </div>
        </template>

        <el-alert
          v-if="!hasReceivableAccess"
          :closable="false"
          type="info"
          show-icon
          :title="t('invoiceDetail.receivable.approvedOnlyTitle')"
          :description="t('invoiceDetail.receivable.approvedOnlyDescription')"
          data-testid="invoice-receivable-awaiting-approval"
        />

        <el-skeleton v-else-if="isReceivableLoading" :rows="4" animated />

        <el-alert
          v-else-if="receivableErrorMessage"
          :closable="false"
          type="error"
          show-icon
          :title="t('invoiceDetail.errors.receivableUnavailable')"
          :description="receivableErrorMessage"
          data-testid="invoice-receivable-error-alert"
        />

        <template v-else-if="receivable">
          <el-descriptions :column="2" border class="invoice-detail-view__receivable-descriptions" data-testid="invoice-receivable-summary">
            <el-descriptions-item :label="t('invoiceDetail.fields.receivableOutstanding')">
              {{ formatAmount(receivable.outstanding_amount) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.receivableSettlementStatus')">
              {{ resolveSettlementLabel(receivable.settlement_status) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.receivableItemCount')">
              {{ receivable.items.length }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.receivablePaymentCount')">
              {{ receivable.payment_history.length }}
            </el-descriptions-item>
          </el-descriptions>

          <el-table
            :data="receivable.items"
            row-key="id"
            class="invoice-detail-view__table"
            :empty-text="t('invoiceDetail.table.receivableItemsEmpty')"
            data-testid="invoice-open-items-table"
          >
            <el-table-column prop="charge_type" :label="t('invoiceDetail.fields.chargeType')" min-width="140" />
            <el-table-column prop="due_date" :label="t('invoiceDetail.fields.dueDate')" min-width="140" />
            <el-table-column :label="t('invoiceDetail.fields.outstandingAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.outstanding_amount) }}
              </template>
            </el-table-column>
          </el-table>

          <div class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.paymentEntry') }}</h3>

            <el-form label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.paymentAmount')">
                  <el-input-number
                    v-model="paymentAmount"
                    :min="0.01"
                    :precision="2"
                    :step="100"
                    :controls="false"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-payment-amount-input"
                  />
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.paymentDate')">
                  <el-date-picker
                    v-model="paymentDate"
                    type="date"
                    value-format="YYYY-MM-DD"
                    format="YYYY-MM-DD"
                    :placeholder="t('invoiceDetail.placeholders.selectPaymentDate')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-payment-date-input"
                  />
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.paymentNote')">
                  <el-input
                    v-model="paymentNote"
                    maxlength="120"
                    show-word-limit
                    :placeholder="t('invoiceDetail.placeholders.enterPaymentNote')"
                    data-testid="invoice-payment-note-input"
                  />
                </el-form-item>
              </div>

              <div class="invoice-detail-view__payment-actions">
                <el-button
                  type="primary"
                  :loading="isRecordingPayment"
                  :disabled="recordPaymentDisabled"
                  data-testid="invoice-payment-submit-button"
                  @click="handleRecordPayment"
                >
                  {{ t('invoiceDetail.actions.recordPayment') }}
                </el-button>

                <el-tag v-if="receivable.outstanding_amount <= 0" effect="plain" type="success" data-testid="invoice-receivable-settled-tag">
                  {{ t('invoiceDetail.feedback.fullySettled') }}
                </el-tag>
              </div>
            </el-form>
          </div>

          <el-table
            :data="receivable.payment_history"
            row-key="id"
            class="invoice-detail-view__table"
            :empty-text="t('invoiceDetail.table.paymentHistoryEmpty')"
            data-testid="invoice-payment-history-table"
          >
            <el-table-column prop="payment_date" :label="t('invoiceDetail.fields.paymentDate')" min-width="140" />
            <el-table-column :label="t('invoiceDetail.fields.paymentAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.amount) }}
              </template>
            </el-table-column>
            <el-table-column prop="note" :label="t('invoiceDetail.fields.paymentNote')" min-width="220" />
            <el-table-column :label="t('common.columns.createdAt')" min-width="180">
              <template #default="scope">
                {{ formatTimestamp(scope.row.created_at) }}
              </template>
            </el-table-column>
          </el-table>
        </template>
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

.invoice-detail-view__receivable-descriptions {
  margin-bottom: var(--mi-space-4);
}

.invoice-detail-view__payment-entry {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  margin: var(--mi-space-5) 0;
}

.invoice-detail-view__payment-title {
  margin: 0;
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.invoice-detail-view__payment-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.invoice-detail-view__payment-input {
  width: 100%;
}

.invoice-detail-view__payment-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.invoice-detail-view__table {
  width: 100%;
}

@media (max-width: 52rem) {
  .invoice-detail-view__grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .invoice-detail-view__payment-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .invoice-detail-view__payment-actions {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
