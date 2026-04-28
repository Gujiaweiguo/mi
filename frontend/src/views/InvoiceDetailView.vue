<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'

import {
  applyInvoiceDeposit,
  applyInvoiceDiscount,
  applyInvoiceSurplus,
  generateInvoiceInterest,
  getInvoiceReceivable,
  getInvoice,
  listReceivables,
  recordInvoicePayment,
  refundInvoiceDeposit,
  submitInvoice,
  cancelInvoice,
  type InvoiceDocument,
  type InvoiceReceivable,
  type ReceivableListItem,
} from '../api/invoice'
import PageSection from '../components/platform/PageSection.vue'
import { getErrorMessage } from '../composables/useErrorMessage'
import { useAppStore } from '../stores/app'
import { formatDate } from '../utils/format'

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
const isApplyingDiscount = ref(false)
const isApplyingSurplus = ref(false)
const isGeneratingInterest = ref(false)
const isLoadingDepositSources = ref(false)
const isLoadingDepositSourceReceivable = ref(false)
const isApplyingDeposit = ref(false)
const isRefundingDeposit = ref(false)
const paymentAmount = ref<number | null>(null)
const paymentDate = ref('')
const paymentNote = ref('')
const paymentFormRef = ref<FormInstance>()
const discountLineId = ref<number | null>(null)
const discountAmount = ref<number | null>(null)
const discountReason = ref('')
const discountFormRef = ref<FormInstance>()
const surplusLineId = ref<number | null>(null)
const surplusAmount = ref<number | null>(null)
const surplusNote = ref('')
const surplusFormRef = ref<FormInstance>()
const interestLineId = ref<number | null>(null)
const interestAsOfDate = ref('')
const interestFormRef = ref<FormInstance>()
const depositSourceOptions = ref<ReceivableListItem[]>([])
const depositSourceErrorMessage = ref('')
const depositSourceDocumentId = ref<number | null>(null)
const depositSourceLineId = ref<number | null>(null)
const depositTargetLineId = ref<number | null>(null)
const depositAmount = ref<number | null>(null)
const depositNote = ref('')
const depositSourceReceivable = ref<InvoiceReceivable | null>(null)
const depositFormRef = ref<FormInstance>()
const refundLineId = ref<number | null>(null)
const refundAmount = ref<number | null>(null)
const refundReason = ref('')
const refundFormRef = ref<FormInstance>()
const depositSourceListPageSize = 100
const paymentForm = reactive({
  get paymentAmount() {
    return paymentAmount.value
  },
  set paymentAmount(value: number | null) {
    paymentAmount.value = value
  },
  get paymentDate() {
    return paymentDate.value
  },
  set paymentDate(value: string) {
    paymentDate.value = value
  },
  get paymentNote() {
    return paymentNote.value
  },
  set paymentNote(value: string) {
    paymentNote.value = value
  },
})

const discountForm = reactive({
  get billingDocumentLineId() {
    return discountLineId.value
  },
  set billingDocumentLineId(value: number | null) {
    discountLineId.value = value
  },
  get discountAmount() {
    return discountAmount.value
  },
  set discountAmount(value: number | null) {
    discountAmount.value = value
  },
  get discountReason() {
    return discountReason.value
  },
  set discountReason(value: string) {
    discountReason.value = value
  },
})

const surplusForm = reactive({
  get billingDocumentLineId() {
    return surplusLineId.value
  },
  set billingDocumentLineId(value: number | null) {
    surplusLineId.value = value
  },
  get surplusAmount() {
    return surplusAmount.value
  },
  set surplusAmount(value: number | null) {
    surplusAmount.value = value
  },
  get surplusNote() {
    return surplusNote.value
  },
  set surplusNote(value: string) {
    surplusNote.value = value
  },
})

const interestForm = reactive({
  get billingDocumentLineId() {
    return interestLineId.value
  },
  set billingDocumentLineId(value: number | null) {
    interestLineId.value = value
  },
  get interestAsOfDate() {
    return interestAsOfDate.value
  },
  set interestAsOfDate(value: string) {
    interestAsOfDate.value = value
  },
})

const depositForm = reactive({
  get sourceDocumentId() {
    return depositSourceDocumentId.value
  },
  set sourceDocumentId(value: number | null) {
    depositSourceDocumentId.value = value
  },
  get billingDocumentLineId() {
    return depositSourceLineId.value
  },
  set billingDocumentLineId(value: number | null) {
    depositSourceLineId.value = value
  },
  get targetBillingDocumentLineId() {
    return depositTargetLineId.value
  },
  set targetBillingDocumentLineId(value: number | null) {
    depositTargetLineId.value = value
  },
  get depositAmount() {
    return depositAmount.value
  },
  set depositAmount(value: number | null) {
    depositAmount.value = value
  },
  get depositNote() {
    return depositNote.value
  },
  set depositNote(value: string) {
    depositNote.value = value
  },
})

const refundForm = reactive({
  get billingDocumentLineId() {
    return refundLineId.value
  },
  set billingDocumentLineId(value: number | null) {
    refundLineId.value = value
  },
  get depositAmount() {
    return refundAmount.value
  },
  set depositAmount(value: number | null) {
    refundAmount.value = value
  },
  get depositReason() {
    return refundReason.value
  },
  set depositReason(value: string) {
    refundReason.value = value
  },
})

const paymentRules: FormRules = {
  paymentAmount: [
    { required: true, message: '请输入金额', trigger: 'blur' },
    { type: 'number', min: 0.01, message: '金额必须大于 0', trigger: 'blur' },
  ],

}

const discountRules: FormRules = {
  billingDocumentLineId: [{ required: true, message: '请选择折扣行', trigger: 'change' }],
  discountAmount: [
    { required: true, message: '请输入折扣金额', trigger: 'blur' },
    { type: 'number', min: 0.01, message: '折扣金额必须大于 0', trigger: 'blur' },
  ],
  discountReason: [{ required: true, message: '请输入折扣原因', trigger: 'blur' }],
}

const surplusRules: FormRules = {
  billingDocumentLineId: [{ required: true, message: '请选择冲抵行', trigger: 'change' }],
  surplusAmount: [
    { required: true, message: '请输入冲抵金额', trigger: 'blur' },
    { type: 'number', min: 0.01, message: '冲抵金额必须大于 0', trigger: 'blur' },
  ],
}

const interestRules: FormRules = {
  billingDocumentLineId: [{ required: true, message: '请选择利息生成行', trigger: 'change' }],
}

const depositRules: FormRules = {
  sourceDocumentId: [{ required: true, message: '请选择保证金来源单据', trigger: 'change' }],
  billingDocumentLineId: [{ required: true, message: '请选择保证金来源行', trigger: 'change' }],
  targetBillingDocumentLineId: [{ required: true, message: '请选择保证金冲抵目标行', trigger: 'change' }],
  depositAmount: [
    { required: true, message: '请输入保证金金额', trigger: 'blur' },
    { type: 'number', min: 0.01, message: '保证金金额必须大于 0', trigger: 'blur' },
  ],
}

const refundRules: FormRules = {
  billingDocumentLineId: [{ required: true, message: '请选择退款保证金行', trigger: 'change' }],
  depositAmount: [
    { required: true, message: '请输入退款金额', trigger: 'blur' },
    { type: 'number', min: 0.01, message: '退款金额必须大于 0', trigger: 'blur' },
  ],
  depositReason: [{ required: true, message: '请输入退款原因', trigger: 'blur' }],
}

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

const sourceTagType = (source: string) => (source === 'overtime' ? 'warning' : 'info')

const sourceTagLabel = (source: string) => (source === 'overtime' ? 'OT' : 'STD')

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

const isDepositDocument = computed(
  () => (receivable.value?.items ?? []).some((item) => item.is_deposit) || invoice.value?.lines.some((line) => line.charge_type === 'deposit') || false,
)

const recordPaymentDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !receivable.value ||
    receivable.value.outstanding_amount <= 0 ||
    paymentAmount.value === null ||
    paymentAmount.value <= 0 ||
    isRecordingPayment.value,
)

const discountableOpenItems = computed(() =>
  (receivable.value?.items ?? []).filter(
    (item) => item.billing_document_line_id !== null && !item.is_deposit && item.outstanding_amount > 0,
  ),
)

const hasPendingDiscount = computed(() =>
  (receivable.value?.discount_history ?? []).some((discount) => discount.status === 'pending_approval'),
)

const hasAvailableSurplus = computed(() => (receivable.value?.customer_surplus_available ?? 0) > 0)

const interestEligibleOpenItems = computed(() =>
  discountableOpenItems.value.filter((item) => item.charge_type !== 'late_payment_interest'),
)

const depositTargetOpenItems = computed(() =>
  (receivable.value?.items ?? []).filter(
    (item) => item.billing_document_line_id !== null && !item.is_deposit && item.outstanding_amount > 0,
  ),
)

const refundableDepositItems = computed(() =>
  (receivable.value?.items ?? []).filter(
    (item) => item.billing_document_line_id !== null && item.is_deposit && item.outstanding_amount > 0,
  ),
)

const depositSourceAvailableItems = computed(() =>
  (depositSourceReceivable.value?.items ?? []).filter(
    (item) => item.billing_document_line_id !== null && item.is_deposit && item.outstanding_amount > 0,
  ),
)

const selectedSurplusOpenItem = computed(() =>
  discountableOpenItems.value.find((item) => item.billing_document_line_id === surplusLineId.value) ?? null,
)

const selectedDepositTargetOpenItem = computed(() =>
  depositTargetOpenItems.value.find((item) => item.billing_document_line_id === depositTargetLineId.value) ?? null,
)

const selectedDepositSourceOpenItem = computed(() =>
  depositSourceAvailableItems.value.find((item) => item.billing_document_line_id === depositSourceLineId.value) ?? null,
)

const selectedRefundDepositItem = computed(() =>
  refundableDepositItems.value.find((item) => item.billing_document_line_id === refundLineId.value) ?? null,
)

const applyDiscountDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !receivable.value ||
    receivable.value.outstanding_amount <= 0 ||
    hasPendingDiscount.value ||
    discountLineId.value === null ||
    discountAmount.value === null ||
    discountAmount.value <= 0 ||
    !discountReason.value.trim() ||
    isApplyingDiscount.value,
)

const applySurplusDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !receivable.value ||
    receivable.value.outstanding_amount <= 0 ||
    !hasAvailableSurplus.value ||
    surplusLineId.value === null ||
    surplusAmount.value === null ||
    surplusAmount.value <= 0 ||
    surplusAmount.value > (receivable.value?.customer_surplus_available ?? 0) ||
    surplusAmount.value > (selectedSurplusOpenItem.value?.outstanding_amount ?? 0) ||
    isApplyingSurplus.value,
)

const generateInterestDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !receivable.value ||
    interestEligibleOpenItems.value.length === 0 ||
    isGeneratingInterest.value,
)

const applyDepositDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !invoice.value ||
    !receivable.value ||
    isDepositDocument.value ||
    depositTargetOpenItems.value.length === 0 ||
    depositSourceDocumentId.value === null ||
    depositSourceLineId.value === null ||
    depositTargetLineId.value === null ||
    depositSourceAvailableItems.value.length === 0 ||
    depositAmount.value === null ||
    depositAmount.value <= 0 ||
    depositAmount.value > (selectedDepositSourceOpenItem.value?.outstanding_amount ?? 0) ||
    depositAmount.value > (selectedDepositTargetOpenItem.value?.outstanding_amount ?? 0) ||
    isApplyingDeposit.value,
)

const refundDepositDisabled = computed(
  () =>
    !hasReceivableAccess.value ||
    !receivable.value ||
    !isDepositDocument.value ||
    refundableDepositItems.value.length === 0 ||
    refundLineId.value === null ||
    refundAmount.value === null ||
    refundAmount.value <= 0 ||
    refundAmount.value > (selectedRefundDepositItem.value?.outstanding_amount ?? 0) ||
    !refundReason.value.trim() ||
    isRefundingDeposit.value,
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

const resolveDiscountStatusLabel = (status: string) => {
  switch (status) {
    case 'draft':
      return t('common.statuses.draft')
    case 'pending_approval':
      return t('common.statuses.pendingApproval')
    case 'approved':
      return t('common.statuses.approved')
    case 'rejected':
      return t('common.statuses.rejected')
    default:
      return status
  }
}

const resetPaymentForm = () => {
  paymentAmount.value = null
  paymentDate.value = ''
  paymentNote.value = ''
  paymentFormRef.value?.clearValidate()
}

const resetDiscountForm = () => {
  discountAmount.value = null
  discountReason.value = ''
  if (discountableOpenItems.value.length === 1) {
    discountLineId.value = discountableOpenItems.value[0].billing_document_line_id
  }
  discountFormRef.value?.clearValidate()
}

const resetSurplusForm = () => {
  surplusAmount.value = null
  surplusNote.value = ''
  if (discountableOpenItems.value.length === 1) {
    surplusLineId.value = discountableOpenItems.value[0].billing_document_line_id
  } else if (!discountableOpenItems.value.some((item) => item.billing_document_line_id === surplusLineId.value)) {
    surplusLineId.value = discountableOpenItems.value[0]?.billing_document_line_id ?? null
  }
  surplusFormRef.value?.clearValidate()
}

const resetInterestForm = () => {
  interestAsOfDate.value = ''
  if (interestEligibleOpenItems.value.length === 1) {
    interestLineId.value = interestEligibleOpenItems.value[0].billing_document_line_id
  } else if (!interestEligibleOpenItems.value.some((item) => item.billing_document_line_id === interestLineId.value)) {
    interestLineId.value = interestEligibleOpenItems.value[0]?.billing_document_line_id ?? null
  }
  interestFormRef.value?.clearValidate()
}

const resetDepositForm = () => {
  depositAmount.value = null
  depositNote.value = ''

  if (depositTargetOpenItems.value.length === 1) {
    depositTargetLineId.value = depositTargetOpenItems.value[0].billing_document_line_id
  } else if (!depositTargetOpenItems.value.some((item) => item.billing_document_line_id === depositTargetLineId.value)) {
    depositTargetLineId.value = depositTargetOpenItems.value[0]?.billing_document_line_id ?? null
  }

  if (depositSourceAvailableItems.value.length === 1) {
    depositSourceLineId.value = depositSourceAvailableItems.value[0].billing_document_line_id
  } else if (!depositSourceAvailableItems.value.some((item) => item.billing_document_line_id === depositSourceLineId.value)) {
    depositSourceLineId.value = depositSourceAvailableItems.value[0]?.billing_document_line_id ?? null
  }

  depositFormRef.value?.clearValidate()
}

const resetRefundForm = () => {
  refundAmount.value = null
  refundReason.value = ''

  if (refundableDepositItems.value.length === 1) {
    refundLineId.value = refundableDepositItems.value[0].billing_document_line_id
  } else if (!refundableDepositItems.value.some((item) => item.billing_document_line_id === refundLineId.value)) {
    refundLineId.value = refundableDepositItems.value[0]?.billing_document_line_id ?? null
  }

  refundFormRef.value?.clearValidate()
}

const clearDepositSourceContext = () => {
  depositSourceOptions.value = []
  depositSourceErrorMessage.value = ''
  depositSourceDocumentId.value = null
  depositSourceLineId.value = null
  depositSourceReceivable.value = null
  resetDepositForm()
}

const loadDepositSourceOptions = async () => {
  if (!invoice.value || !hasReceivableAccess.value || isDepositDocument.value) {
    clearDepositSourceContext()
    return
  }

  isLoadingDepositSources.value = true
  depositSourceErrorMessage.value = ''

  try {
    const response = await listReceivables({
      page: 1,
      page_size: depositSourceListPageSize,
    })

    depositSourceOptions.value = response.data.items.filter(
      (item) => item.lease_contract_id === invoice.value?.lease_contract_id && item.billing_document_id !== invoice.value?.id,
    )

    if (depositSourceOptions.value.length === 1) {
      depositSourceDocumentId.value = depositSourceOptions.value[0].billing_document_id
    } else if (!depositSourceOptions.value.some((item) => item.billing_document_id === depositSourceDocumentId.value)) {
      depositSourceDocumentId.value = null
      depositSourceLineId.value = null
      depositSourceReceivable.value = null
      resetDepositForm()
    }
  } catch (error) {
    depositSourceOptions.value = []
    depositSourceDocumentId.value = null
    depositSourceLineId.value = null
    depositSourceReceivable.value = null
    resetDepositForm()
    depositSourceErrorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToLoadDepositSources'))
  } finally {
    isLoadingDepositSources.value = false
  }
}

const loadDepositSourceReceivable = async (sourceDocumentId: number | null) => {
  if (!sourceDocumentId || isDepositDocument.value) {
    depositSourceLineId.value = null
    depositSourceReceivable.value = null
    depositSourceErrorMessage.value = ''
    resetDepositForm()
    return
  }

  isLoadingDepositSourceReceivable.value = true
  depositSourceErrorMessage.value = ''

  try {
    const response = await getInvoiceReceivable(sourceDocumentId)
    depositSourceReceivable.value = response.data.receivable
    resetDepositForm()
  } catch (error) {
    depositSourceLineId.value = null
    depositSourceReceivable.value = null
    resetDepositForm()
    depositSourceErrorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToLoadDepositSourceDocument'))
  } finally {
    isLoadingDepositSourceReceivable.value = false
  }
}

const loadReceivable = async () => {
  if (!invoiceId.value || !hasReceivableAccess.value) {
    receivable.value = null
    receivableErrorMessage.value = ''
    resetPaymentForm()
    resetDiscountForm()
    resetSurplusForm()
    resetInterestForm()
    clearDepositSourceContext()
    resetRefundForm()
    return
  }

  isReceivableLoading.value = true
  receivableErrorMessage.value = ''

  try {
    const response = await getInvoiceReceivable(invoiceId.value)
    receivable.value = response.data.receivable
    if (!discountLineId.value && discountableOpenItems.value.length === 1) {
      discountLineId.value = discountableOpenItems.value[0].billing_document_line_id
    }
    if (!surplusLineId.value && discountableOpenItems.value.length === 1) {
      surplusLineId.value = discountableOpenItems.value[0].billing_document_line_id
    } else if (!discountableOpenItems.value.some((item) => item.billing_document_line_id === surplusLineId.value)) {
      surplusLineId.value = discountableOpenItems.value[0]?.billing_document_line_id ?? null
    }
    if (!interestLineId.value && interestEligibleOpenItems.value.length === 1) {
      interestLineId.value = interestEligibleOpenItems.value[0].billing_document_line_id
    } else if (!interestEligibleOpenItems.value.some((item) => item.billing_document_line_id === interestLineId.value)) {
      interestLineId.value = interestEligibleOpenItems.value[0]?.billing_document_line_id ?? null
    }
    if (!depositTargetLineId.value && depositTargetOpenItems.value.length === 1) {
      depositTargetLineId.value = depositTargetOpenItems.value[0].billing_document_line_id
    } else if (!depositTargetOpenItems.value.some((item) => item.billing_document_line_id === depositTargetLineId.value)) {
      depositTargetLineId.value = depositTargetOpenItems.value[0]?.billing_document_line_id ?? null
    }
    if (!refundLineId.value && refundableDepositItems.value.length === 1) {
      refundLineId.value = refundableDepositItems.value[0].billing_document_line_id
    } else if (!refundableDepositItems.value.some((item) => item.billing_document_line_id === refundLineId.value)) {
      refundLineId.value = refundableDepositItems.value[0]?.billing_document_line_id ?? null
    }
  } catch (error) {
    receivable.value = null
    receivableErrorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToLoadReceivable'))
    clearDepositSourceContext()
    resetRefundForm()
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
    await loadDepositSourceOptions()
  } catch (error) {
    invoice.value = null
    receivable.value = null
    receivableErrorMessage.value = ''
    clearDepositSourceContext()
    resetRefundForm()
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToLoad'))
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
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToSubmit'))
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
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToCancel'))
  } finally {
    isCancelling.value = false
  }
}

const handleRecordPayment = async () => {
  const valid = await paymentFormRef.value?.validate().catch(() => false)

  if (!valid) {
    return
  }

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
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToRecordPayment'))
  } finally {
    isRecordingPayment.value = false
  }
}

const handleApplyDiscount = async () => {
  const valid = await discountFormRef.value?.validate().catch(() => false)

  if (!valid) {
    return
  }

  if (!invoice.value || !receivable.value || discountLineId.value === null || discountAmount.value === null || discountAmount.value <= 0) {
    return
  }

  isApplyingDiscount.value = true
  errorMessage.value = ''
  successMessage.value = ''
  receivableErrorMessage.value = ''

  try {
    const response = await applyInvoiceDiscount(invoice.value.id, {
      billing_document_line_id: discountLineId.value,
      amount: discountAmount.value,
      reason: discountReason.value.trim(),
      idempotency_key: crypto.randomUUID(),
    })

    receivable.value = response.data.receivable
    successMessage.value = t('invoiceDetail.feedback.discountRequested')
    resetDiscountForm()
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToApplyDiscount'))
  } finally {
    isApplyingDiscount.value = false
  }
}

const handleApplySurplus = async () => {
  const valid = await surplusFormRef.value?.validate().catch(() => false)

  if (!valid) {
    return
  }

  if (!invoice.value || !receivable.value || surplusLineId.value === null || surplusAmount.value === null || surplusAmount.value <= 0) {
    return
  }

  isApplyingSurplus.value = true
  errorMessage.value = ''
  successMessage.value = ''
  receivableErrorMessage.value = ''

  try {
    const response = await applyInvoiceSurplus(invoice.value.id, {
      billing_document_line_id: surplusLineId.value,
      amount: surplusAmount.value,
      note: surplusNote.value.trim() || undefined,
      idempotency_key: crypto.randomUUID(),
    })

    receivable.value = response.data.receivable
    successMessage.value = t('invoiceDetail.feedback.surplusApplied')
    resetSurplusForm()
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToApplySurplus'))
  } finally {
    isApplyingSurplus.value = false
  }
}

const handleApplyDeposit = async () => {
  const valid = await depositFormRef.value?.validate().catch(() => false)

  if (!valid) {
    return
  }

  if (
    !invoice.value ||
    !receivable.value ||
    depositSourceDocumentId.value === null ||
    depositSourceLineId.value === null ||
    depositTargetLineId.value === null ||
    depositAmount.value === null ||
    depositAmount.value <= 0
  ) {
    return
  }

  isApplyingDeposit.value = true
  errorMessage.value = ''
  successMessage.value = ''
  receivableErrorMessage.value = ''
  depositSourceErrorMessage.value = ''

  try {
    const response = await applyInvoiceDeposit(depositSourceDocumentId.value, {
      billing_document_line_id: depositSourceLineId.value,
      target_document_id: invoice.value.id,
      target_billing_document_line_id: depositTargetLineId.value,
      amount: depositAmount.value,
      note: depositNote.value.trim() || undefined,
      idempotency_key: crypto.randomUUID(),
    })

    receivable.value = response.data.receivable
    successMessage.value = t('invoiceDetail.feedback.depositApplied')
    resetDepositForm()
    await loadDepositSourceOptions()
    if (depositSourceDocumentId.value) {
      await loadDepositSourceReceivable(depositSourceDocumentId.value)
    }
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToApplyDeposit'))
  } finally {
    isApplyingDeposit.value = false
  }
}

const handleGenerateInterest = async () => {
  const valid = await interestFormRef.value?.validate().catch(() => false)

  if (!valid) {
    return
  }

  const targetLineId = interestLineId.value ?? interestEligibleOpenItems.value[0]?.billing_document_line_id ?? null

  if (!invoice.value || !receivable.value || targetLineId === null) {
    return
  }

  isGeneratingInterest.value = true
  errorMessage.value = ''
  successMessage.value = ''
  receivableErrorMessage.value = ''

  try {
    const response = await generateInvoiceInterest(invoice.value.id, {
      billing_document_line_id: targetLineId,
      as_of_date: interestAsOfDate.value,
      idempotency_key: crypto.randomUUID(),
    })

    receivable.value = response.data.receivable
    successMessage.value = t('invoiceDetail.feedback.interestGenerated')
    resetInterestForm()
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToGenerateInterest'))
  } finally {
    isGeneratingInterest.value = false
  }
}

const handleRefundDeposit = async () => {
  const valid = await refundFormRef.value?.validate().catch(() => false)

  if (!valid) {
    return
  }

  if (!invoice.value || !receivable.value || refundLineId.value === null || refundAmount.value === null || refundAmount.value <= 0) {
    return
  }

  isRefundingDeposit.value = true
  errorMessage.value = ''
  successMessage.value = ''
  receivableErrorMessage.value = ''

  try {
    const response = await refundInvoiceDeposit(invoice.value.id, {
      billing_document_line_id: refundLineId.value,
      amount: refundAmount.value,
      reason: refundReason.value.trim(),
      idempotency_key: crypto.randomUUID(),
    })

    receivable.value = response.data.receivable
    successMessage.value = t('invoiceDetail.feedback.depositRefunded')
    resetRefundForm()
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('invoiceDetail.errors.unableToRefundDeposit'))
  } finally {
    isRefundingDeposit.value = false
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

watch(
  () => depositSourceDocumentId.value,
  (sourceDocumentId) => {
    void loadDepositSourceReceivable(sourceDocumentId)
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
              {{ formatDate(invoice.submitted_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.approvedAt')">
              {{ formatDate(invoice.approved_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.cancelledAt')">
              {{ formatDate(invoice.cancelled_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('common.columns.createdAt')">
              {{ formatDate(invoice.created_at) }}
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
          <el-table-column label="Source" min-width="120">
            <template #default="scope">
              <el-tag :type="sourceTagType(scope.row.charge_source)" effect="plain">
                {{ sourceTagLabel(scope.row.charge_source) }}
              </el-tag>
            </template>
          </el-table-column>
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
            <el-descriptions-item :label="t('invoiceDetail.fields.customerSurplusAvailable')">
              {{ formatAmount(receivable.customer_surplus_available) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('invoiceDetail.fields.receivableSurplusCount')">
              {{ receivable.surplus_history.length }}
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
            <el-table-column label="Source" min-width="120">
              <template #default="scope">
                <el-tag :type="sourceTagType(scope.row.charge_source)" effect="plain">
                  {{ sourceTagLabel(scope.row.charge_source) }}
                </el-tag>
              </template>
            </el-table-column>
            <el-table-column prop="due_date" :label="t('invoiceDetail.fields.dueDate')" min-width="140" />
            <el-table-column :label="t('invoiceDetail.fields.outstandingAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.outstanding_amount) }}
              </template>
            </el-table-column>
          </el-table>

          <div class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.discountEntry') }}</h3>

            <el-form ref="discountFormRef" :model="discountForm" :rules="discountRules" label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.discountLine')" prop="billingDocumentLineId">
                  <el-select
                    v-model="discountLineId"
                    :placeholder="t('invoiceDetail.placeholders.selectDiscountLine')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-discount-line-select"
                  >
                    <el-option
                      v-for="item in discountableOpenItems"
                      :key="item.id"
                      :label="`${item.charge_type} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_line_id!"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.discountAmount')" prop="discountAmount">
                  <el-input-number
                    v-model="discountAmount"
                    :min="0.01"
                    :precision="2"
                    :step="100"
                    :controls="false"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-discount-amount-input"
                  />
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.discountReason')" prop="discountReason">
                  <el-input
                    v-model="discountReason"
                    maxlength="120"
                    show-word-limit
                    :placeholder="t('invoiceDetail.placeholders.enterDiscountReason')"
                    data-testid="invoice-discount-reason-input"
                  />
                </el-form-item>
              </div>

              <div class="invoice-detail-view__payment-actions">
                <el-button
                  type="warning"
                  :loading="isApplyingDiscount"
                  :disabled="applyDiscountDisabled"
                  data-testid="invoice-discount-submit-button"
                  @click="handleApplyDiscount"
                >
                  {{ t('invoiceDetail.actions.submitDiscountRequest') }}
                </el-button>

                <el-tag v-if="hasPendingDiscount" effect="plain" type="warning" data-testid="invoice-discount-pending-tag">
                  {{ t('invoiceDetail.feedback.discountPendingApproval') }}
                </el-tag>
              </div>
            </el-form>
          </div>

          <div class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.surplusEntry') }}</h3>

            <el-form ref="surplusFormRef" :model="surplusForm" :rules="surplusRules" label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.surplusLine')" prop="billingDocumentLineId">
                  <el-select
                    v-model="surplusLineId"
                    :placeholder="t('invoiceDetail.placeholders.selectSurplusLine')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-surplus-line-select"
                  >
                    <el-option
                      v-for="item in discountableOpenItems"
                      :key="item.id"
                      :label="`${item.charge_type} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_line_id!"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.surplusAmount')" prop="surplusAmount">
                  <el-input-number
                    v-model="surplusAmount"
                    :min="0.01"
                    :precision="2"
                    :step="100"
                    :controls="false"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-surplus-amount-input"
                  />
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.surplusNote')" prop="surplusNote">
                  <el-input
                    v-model="surplusNote"
                    maxlength="120"
                    show-word-limit
                    :placeholder="t('invoiceDetail.placeholders.enterSurplusNote')"
                    data-testid="invoice-surplus-note-input"
                  />
                </el-form-item>
              </div>

              <div class="invoice-detail-view__payment-actions">
                <el-button
                  type="success"
                  :loading="isApplyingSurplus"
                  :disabled="applySurplusDisabled"
                  data-testid="invoice-surplus-submit-button"
                  @click="handleApplySurplus"
                >
                  {{ t('invoiceDetail.actions.applySurplus') }}
                </el-button>

                <el-tag v-if="!hasAvailableSurplus" effect="plain" type="info" data-testid="invoice-surplus-empty-tag">
                  {{ t('invoiceDetail.feedback.noSurplusAvailable') }}
                </el-tag>
              </div>
            </el-form>
          </div>

          <div v-if="!isDepositDocument" class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.depositApplicationEntry') }}</h3>

            <el-form ref="depositFormRef" :model="depositForm" :rules="depositRules" label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.depositSourceDocument')" prop="sourceDocumentId">
                  <el-select
                    v-model="depositSourceDocumentId"
                    :placeholder="t('invoiceDetail.placeholders.selectDepositSourceDocument')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-deposit-source-document-select"
                  >
                    <el-option
                      v-for="item in depositSourceOptions"
                      :key="item.billing_document_id"
                      :label="`${item.document_no ?? t('invoiceDetail.defaults.documentId', { id: item.billing_document_id })} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_id"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.depositSourceLine')" prop="billingDocumentLineId">
                  <el-select
                    v-model="depositSourceLineId"
                    :placeholder="t('invoiceDetail.placeholders.selectDepositSourceLine')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-deposit-source-line-select"
                  >
                    <el-option
                      v-for="item in depositSourceAvailableItems"
                      :key="item.id"
                      :label="`${item.charge_type} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_line_id!"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.depositTargetLine')" prop="targetBillingDocumentLineId">
                  <el-select
                    v-model="depositTargetLineId"
                    :placeholder="t('invoiceDetail.placeholders.selectDepositTargetLine')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-deposit-target-line-select"
                  >
                    <el-option
                      v-for="item in depositTargetOpenItems"
                      :key="item.id"
                      :label="`${item.charge_type} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_line_id!"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.depositAmount')" prop="depositAmount">
                  <el-input-number
                    v-model="depositAmount"
                    :min="0.01"
                    :precision="2"
                    :step="100"
                    :controls="false"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-deposit-amount-input"
                  />
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.depositNote')" prop="depositNote">
                  <el-input
                    v-model="depositNote"
                    maxlength="120"
                    show-word-limit
                    :placeholder="t('invoiceDetail.placeholders.enterDepositNote')"
                    data-testid="invoice-deposit-note-input"
                  />
                </el-form-item>
              </div>

              <el-skeleton v-if="isLoadingDepositSources || isLoadingDepositSourceReceivable" :rows="2" animated />

              <el-alert
                v-else-if="depositSourceErrorMessage"
                :closable="false"
                type="error"
                show-icon
                :description="depositSourceErrorMessage"
                data-testid="invoice-deposit-source-error-alert"
              />

              <div class="invoice-detail-view__payment-actions">
                <el-button
                  type="success"
                  plain
                  :loading="isApplyingDeposit"
                  :disabled="applyDepositDisabled"
                  data-testid="invoice-deposit-submit-button"
                  @click="handleApplyDeposit"
                >
                  {{ t('invoiceDetail.actions.applyDeposit') }}
                </el-button>

                <el-tag
                  v-if="!depositSourceOptions.length && !isLoadingDepositSources"
                  effect="plain"
                  type="info"
                  data-testid="invoice-deposit-source-empty-tag"
                >
                  {{ t('invoiceDetail.feedback.noDepositSourceDocuments') }}
                </el-tag>

                <el-tag
                  v-else-if="depositSourceDocumentId !== null && !depositSourceErrorMessage && !isLoadingDepositSourceReceivable && !depositSourceAvailableItems.length"
                  effect="plain"
                  type="warning"
                  data-testid="invoice-deposit-source-unavailable-tag"
                >
                  {{ t('invoiceDetail.feedback.selectedSourceHasNoDeposit') }}
                </el-tag>
              </div>
            </el-form>

            <template v-if="depositSourceReceivable">
              <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.depositApplicationHistory') }}</h3>

              <el-table
                :data="depositSourceReceivable.deposit_application_history"
                row-key="id"
                class="invoice-detail-view__table"
                :empty-text="t('invoiceDetail.table.depositApplicationHistoryEmpty')"
                data-testid="invoice-deposit-application-history-table"
              >
                <el-table-column :label="t('invoiceDetail.fields.depositApplicationTargetDocument')" min-width="180">
                  <template #default="scope">
                    {{ scope.row.target_billing_document_id }}
                  </template>
                </el-table-column>
                <el-table-column :label="t('invoiceDetail.fields.depositAmount')" min-width="160" align="right" header-align="right">
                  <template #default="scope">
                    {{ formatAmount(scope.row.amount) }}
                  </template>
                </el-table-column>
                <el-table-column prop="note" :label="t('invoiceDetail.fields.depositNote')" min-width="220" />
                <el-table-column :label="t('common.columns.createdAt')" min-width="180">
                  <template #default="scope">
                    {{ formatDate(scope.row.created_at) }}
                  </template>
                </el-table-column>
              </el-table>
            </template>
          </div>

          <div v-else class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.depositRefundEntry') }}</h3>

            <el-form ref="refundFormRef" :model="refundForm" :rules="refundRules" label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.depositRefundLine')" prop="billingDocumentLineId">
                  <el-select
                    v-model="refundLineId"
                    :placeholder="t('invoiceDetail.placeholders.selectDepositRefundLine')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-deposit-refund-line-select"
                  >
                    <el-option
                      v-for="item in refundableDepositItems"
                      :key="item.id"
                      :label="`${item.charge_type} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_line_id!"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.depositAmount')" prop="depositAmount">
                  <el-input-number
                    v-model="refundAmount"
                    :min="0.01"
                    :precision="2"
                    :step="100"
                    :controls="false"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-deposit-refund-amount-input"
                  />
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.depositRefundReason')" prop="depositReason">
                  <el-input
                    v-model="refundReason"
                    maxlength="120"
                    show-word-limit
                    :placeholder="t('invoiceDetail.placeholders.enterDepositRefundReason')"
                    data-testid="invoice-deposit-refund-reason-input"
                  />
                </el-form-item>
              </div>

              <div class="invoice-detail-view__payment-actions">
                <el-button
                  type="danger"
                  plain
                  :loading="isRefundingDeposit"
                  :disabled="refundDepositDisabled"
                  data-testid="invoice-deposit-refund-submit-button"
                  @click="handleRefundDeposit"
                >
                  {{ t('invoiceDetail.actions.refundDeposit') }}
                </el-button>

                <el-tag
                  v-if="!refundableDepositItems.length"
                  effect="plain"
                  type="info"
                  data-testid="invoice-deposit-refund-empty-tag"
                >
                  {{ t('invoiceDetail.feedback.noRefundableDeposit') }}
                </el-tag>
              </div>
            </el-form>

            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.depositRefundHistory') }}</h3>

            <el-table
              :data="receivable.deposit_refund_history"
              row-key="id"
              class="invoice-detail-view__table"
              :empty-text="t('invoiceDetail.table.depositRefundHistoryEmpty')"
              data-testid="invoice-deposit-refund-history-table"
            >
              <el-table-column :label="t('invoiceDetail.fields.depositAmount')" min-width="160" align="right" header-align="right">
                <template #default="scope">
                  {{ formatAmount(scope.row.amount) }}
                </template>
              </el-table-column>
              <el-table-column prop="reason" :label="t('invoiceDetail.fields.depositRefundReason')" min-width="220" />
              <el-table-column :label="t('common.columns.createdAt')" min-width="180">
                <template #default="scope">
                  {{ formatDate(scope.row.created_at) }}
                </template>
              </el-table-column>
            </el-table>
          </div>

          <div class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.interestEntry') }}</h3>

            <el-form ref="interestFormRef" :model="interestForm" :rules="interestRules" label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.interestLine')" prop="billingDocumentLineId">
                  <el-select
                    v-model="interestLineId"
                    :placeholder="t('invoiceDetail.placeholders.selectInterestLine')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-interest-line-select"
                  >
                    <el-option
                      v-for="item in interestEligibleOpenItems"
                      :key="item.id"
                      :label="`${item.charge_type} · ${formatAmount(item.outstanding_amount)}`"
                      :value="item.billing_document_line_id!"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('invoiceDetail.fields.interestAsOfDate')" prop="interestAsOfDate">
                  <el-date-picker
                    v-model="interestAsOfDate"
                    type="date"
                    value-format="YYYY-MM-DD"
                    format="YYYY-MM-DD"
                    :placeholder="t('invoiceDetail.placeholders.selectInterestAsOfDate')"
                    class="invoice-detail-view__payment-input"
                    data-testid="invoice-interest-as-of-date-input"
                  />
                </el-form-item>
              </div>

              <div class="invoice-detail-view__payment-actions">
                <el-button
                  type="warning"
                  plain
                  :loading="isGeneratingInterest"
                  :disabled="generateInterestDisabled"
                  data-testid="invoice-interest-submit-button"
                  @click="handleGenerateInterest"
                >
                  {{ t('invoiceDetail.actions.generateInterest') }}
                </el-button>
              </div>
            </el-form>
          </div>

          <div class="invoice-detail-view__payment-entry">
            <h3 class="invoice-detail-view__payment-title">{{ t('invoiceDetail.cards.paymentEntry') }}</h3>

            <el-form ref="paymentFormRef" :model="paymentForm" :rules="paymentRules" label-position="top">
              <div class="invoice-detail-view__payment-grid">
                <el-form-item :label="t('invoiceDetail.fields.paymentAmount')" prop="paymentAmount">
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

                <el-form-item :label="t('invoiceDetail.fields.paymentDate')" prop="paymentDate">
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

                <el-form-item :label="t('invoiceDetail.fields.paymentNote')" prop="paymentNote">
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
                {{ formatDate(scope.row.created_at) }}
              </template>
            </el-table-column>
          </el-table>

          <el-table
            :data="receivable.discount_history"
            row-key="id"
            class="invoice-detail-view__table"
            :empty-text="t('invoiceDetail.table.discountHistoryEmpty')"
            data-testid="invoice-discount-history-table"
          >
            <el-table-column prop="charge_type" :label="t('invoiceDetail.fields.chargeType')" min-width="140" />
            <el-table-column :label="t('invoiceDetail.fields.discountAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.requested_amount) }}
              </template>
            </el-table-column>
            <el-table-column prop="reason" :label="t('invoiceDetail.fields.discountReason')" min-width="220" />
            <el-table-column :label="t('invoiceDetail.fields.discountStatus')" min-width="160">
              <template #default="scope">
                {{ resolveDiscountStatusLabel(scope.row.status) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('invoiceDetail.fields.discountSubmittedAt')" min-width="180">
              <template #default="scope">
                {{ formatDate(scope.row.submitted_at) }}
              </template>
            </el-table-column>
          </el-table>

          <el-table
            :data="receivable.surplus_history"
            row-key="id"
            class="invoice-detail-view__table"
            :empty-text="t('invoiceDetail.table.surplusHistoryEmpty')"
            data-testid="invoice-surplus-history-table"
          >
            <el-table-column :label="t('invoiceDetail.fields.surplusType')" min-width="140">
              <template #default="scope">
                {{ scope.row.entry_type === 'overpayment' ? t('invoiceDetail.values.surplusTypes.overpayment') : t('invoiceDetail.values.surplusTypes.application') }}
              </template>
            </el-table-column>
            <el-table-column :label="t('invoiceDetail.fields.surplusAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.amount) }}
              </template>
            </el-table-column>
            <el-table-column prop="note" :label="t('invoiceDetail.fields.surplusNote')" min-width="220" />
            <el-table-column :label="t('common.columns.createdAt')" min-width="180">
              <template #default="scope">
                {{ formatDate(scope.row.created_at) }}
              </template>
            </el-table-column>
          </el-table>

          <el-table
            :data="receivable.interest_history"
            row-key="id"
            class="invoice-detail-view__table"
            :empty-text="t('invoiceDetail.table.interestHistoryEmpty')"
            data-testid="invoice-interest-history-table"
          >
            <el-table-column :label="t('invoiceDetail.fields.interestCoveredPeriod')" min-width="220">
              <template #default="scope">
                {{ scope.row.covered_start_date }} → {{ scope.row.covered_end_date }}
              </template>
            </el-table-column>
            <el-table-column :label="t('invoiceDetail.fields.interestDays')" min-width="120">
              <template #default="scope">
                {{ scope.row.interest_days }}
              </template>
            </el-table-column>
            <el-table-column :label="t('invoiceDetail.fields.interestPrincipalAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.principal_amount) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('invoiceDetail.fields.interestAmount')" min-width="160" align="right" header-align="right">
              <template #default="scope">
                {{ formatAmount(scope.row.interest_amount) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('invoiceDetail.fields.interestGeneratedDocument')" min-width="160">
              <template #default="scope">
                {{ scope.row.generated_billing_document_id }}
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
