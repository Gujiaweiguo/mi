<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'
import type { FormInstance, FormRules } from 'element-plus'

import { FUNCTION_CODES } from '../auth/permissions'
import {
  cancelOvertimeBill,
  createOvertimeBill,
  generateOvertimeBillCharges,
  getLease,
  listOvertimeBills,
  stopOvertimeBill,
  submitLease,
  submitOvertimeBill,
  terminateLease,
  type LeaseContract,
  type OvertimeBill,
  type OvertimeFormula,
  type OvertimeFormulaType,
  type OvertimeRateType,
} from '../api/lease'
import PageSection from '../components/platform/PageSection.vue'
import { getErrorMessage } from '../composables/useErrorMessage'
import { useAppStore } from '../stores/app'
import { useAuthStore } from '../stores/auth'
import { formatDate } from '../utils/format'

type OvertimePercentTierForm = {
  sales_to: number
  percentage: number
}

type OvertimeMinimumTierForm = {
  sales_to: number
  minimum_sum: number
}

type OvertimeFormulaForm = Omit<
  OvertimeFormula,
  'id' | 'overtime_bill_id' | 'sort_order' | 'created_at' | 'updated_at' | 'percentage_tiers' | 'minimum_tiers'
> & {
  percentage_tiers: OvertimePercentTierForm[]
  minimum_tiers: OvertimeMinimumTierForm[]
}

const createPercentTier = (): OvertimePercentTierForm => ({
  sales_to: 0,
  percentage: 0,
})

const createMinimumTier = (): OvertimeMinimumTierForm => ({
  sales_to: 0,
  minimum_sum: 0,
})

const createFormulaForm = (): OvertimeFormulaForm => ({
  charge_type: '',
  formula_type: 'fixed',
  rate_type: 'daily',
  effective_from: '',
  effective_to: '',
  currency_type_id: 1,
  total_area: 0,
  unit_price: 0,
  base_amount: 0,
  fixed_rental: 0,
  percentage_option: '',
  minimum_option: '',
  percentage_tiers: [createPercentTier()],
  minimum_tiers: [createMinimumTier()],
})

const route = useRoute()
const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()
const authStore = useAuthStore()

const lease = ref<LeaseContract | null>(null)
const errorMessage = ref('')
const successMessage = ref('')
const isLoading = ref(false)
const isSubmitting = ref(false)
const isTerminating = ref(false)
const overtimeBills = ref<OvertimeBill[]>([])
const overtimeErrorMessage = ref('')
const isOvertimeLoading = ref(false)
const isCreatingOvertime = ref(false)
const overtimeActionBillId = ref<number | null>(null)

const terminateForm = reactive({
  terminated_at: '',
})

const overtimeCreateForm = reactive({
  period_start: '',
  period_end: '',
  note: '',
  formulas: [createFormulaForm()],
})

const terminateFormRef = ref<FormInstance>()
const terminateFormRules: FormRules = {
  terminated_at: [{ required: true, message: '请选择终止日期', trigger: 'change' }],
}

const today = () => new Date().toISOString().slice(0, 10)

const leaseId = computed(() => {
  const rawId = route.params.id
  const normalizedId = Array.isArray(rawId) ? rawId[0] : rawId
  const parsedId = Number(normalizedId)

  return Number.isFinite(parsedId) && parsedId > 0 ? parsedId : null
})

const pageTitle = computed(() => lease.value?.lease_no ?? t('leaseDetail.title'))
const canViewOvertime = computed(() => authStore.canAccess(FUNCTION_CODES.billingCharge, 'view'))
const canEditOvertime = computed(() => authStore.canAccess(FUNCTION_CODES.billingCharge, 'edit'))
const canAmendLease = computed(() => lease.value?.status === 'active')
const createOvertimeDisabled = computed(
  () =>
    !canEditOvertime.value ||
    !lease.value ||
    (lease.value.status !== 'active' && lease.value.status !== 'terminated') ||
    isCreatingOvertime.value,
)

const submitDisabled = computed(() => !lease.value || isSubmitting.value || lease.value.status !== 'draft')
const terminateDisabled = computed(() => !lease.value || isTerminating.value || lease.value.status !== 'active')

const overtimeFormulaTypeOptions = computed<{ label: string; value: OvertimeFormulaType }[]>(() => [
  { label: t('leaseDetail.overtime.options.formulaTypes.fixed'), value: 'fixed' },
  { label: t('leaseDetail.overtime.options.formulaTypes.oneTime'), value: 'one_time' },
  { label: t('leaseDetail.overtime.options.formulaTypes.percentage'), value: 'percentage' },
])

const overtimeRateTypeOptions = computed<{ label: string; value: OvertimeRateType }[]>(() => [
  { label: t('leaseDetail.overtime.options.rateTypes.daily'), value: 'daily' },
  { label: t('leaseDetail.overtime.options.rateTypes.monthly'), value: 'monthly' },
])

const formatDecimal = (value: number) =>
  new Intl.NumberFormat(appStore.locale, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const resolveStatusLabel = (status: string) => {
  switch (status) {
    case 'draft':
      return t('common.statuses.draft')
    case 'pending_approval':
      return t('common.statuses.pendingApproval')
    case 'active':
      return t('common.statuses.active')
    case 'rejected':
      return t('common.statuses.rejected')
    case 'approved':
      return t('common.statuses.approved')
    case 'terminated':
      return t('common.statuses.terminated')
    case 'cancelled':
      return t('common.statuses.cancelled')
    case 'stopped':
      return t('leaseDetail.overtime.statuses.stopped')
    case 'generated':
      return t('leaseDetail.overtime.statuses.generated')
    default:
      return status
  }
}

const resolveSubtypeLabel = (subtype: string) => {
  switch (subtype) {
    case 'joint_operation':
      return t('leaseCreate.options.subtypes.jointOperation')
    case 'ad_board':
      return t('leaseCreate.options.subtypes.adBoard')
    case 'area_ground':
      return t('leaseCreate.options.subtypes.areaGround')
    default:
      return t('leaseCreate.options.subtypes.standard')
  }
}

const resolveTermTypeLabel = (termType: string) => {
  switch (termType) {
    case 'rent':
      return t('leaseDetail.options.termTypes.rent')
    case 'deposit':
      return t('leaseDetail.options.termTypes.deposit')
    default:
      return termType
  }
}

const resolveBillingCycleLabel = (billingCycle: string) => {
  switch (billingCycle) {
    case 'monthly':
      return t('leaseDetail.options.billingCycles.monthly')
    default:
      return billingCycle
  }
}

const statusTagType = (status: string) => {
  switch (status) {
    case 'active':
    case 'approved':
    case 'generated':
      return 'success'
    case 'pending_approval':
      return 'warning'
    case 'rejected':
    case 'cancelled':
      return 'danger'
    case 'terminated':
    case 'stopped':
      return 'info'
    default:
      return 'info'
  }
}

const loadOvertimeBills = async () => {
  if (!leaseId.value || !canViewOvertime.value) {
    overtimeBills.value = []
    overtimeErrorMessage.value = ''
    return
  }

  isOvertimeLoading.value = true
  overtimeErrorMessage.value = ''

  try {
    const response = await listOvertimeBills({ lease_contract_id: leaseId.value, page: 1, page_size: 100 })
    overtimeBills.value = response.data.items
  } catch (error) {
    overtimeBills.value = []
    overtimeErrorMessage.value = getErrorMessage(error, t('leaseDetail.overtime.errors.unableToLoad'))
  } finally {
    isOvertimeLoading.value = false
  }
}

const loadLease = async () => {
  if (!leaseId.value) {
    errorMessage.value = t('leaseDetail.errors.invalidId')
    lease.value = null
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await getLease(leaseId.value)
    lease.value = response.data.lease
    terminateForm.terminated_at = response.data.lease.terminated_at?.slice(0, 10) ?? today()
  } catch (error) {
    lease.value = null
    errorMessage.value = getErrorMessage(error, t('leaseDetail.errors.unableToLoad'))
  } finally {
    isLoading.value = false
  }
}

const resetOvertimeCreateForm = () => {
  overtimeCreateForm.period_start = ''
  overtimeCreateForm.period_end = ''
  overtimeCreateForm.note = ''
  overtimeCreateForm.formulas = [createFormulaForm()]
}

const addFormulaRow = () => {
  overtimeCreateForm.formulas.push(createFormulaForm())
}

const removeFormulaRow = (index: number) => {
  if (overtimeCreateForm.formulas.length === 1) {
    overtimeCreateForm.formulas.splice(0, 1, createFormulaForm())
    return
  }

  overtimeCreateForm.formulas.splice(index, 1)
}

const addPercentageTier = (formulaIndex: number) => {
  overtimeCreateForm.formulas[formulaIndex].percentage_tiers.push(createPercentTier())
}

const removePercentageTier = (formulaIndex: number, tierIndex: number) => {
  const tiers = overtimeCreateForm.formulas[formulaIndex].percentage_tiers
  if (tiers.length === 1) {
    tiers.splice(0, 1, createPercentTier())
    return
  }

  tiers.splice(tierIndex, 1)
}

const addMinimumTier = (formulaIndex: number) => {
  overtimeCreateForm.formulas[formulaIndex].minimum_tiers.push(createMinimumTier())
}

const removeMinimumTier = (formulaIndex: number, tierIndex: number) => {
  const tiers = overtimeCreateForm.formulas[formulaIndex].minimum_tiers
  if (tiers.length === 1) {
    tiers.splice(0, 1, createMinimumTier())
    return
  }

  tiers.splice(tierIndex, 1)
}

const validateOvertimeCreateForm = () => {
  const issues: string[] = []

  if (!overtimeCreateForm.period_start || !overtimeCreateForm.period_end || overtimeCreateForm.period_start > overtimeCreateForm.period_end) {
    issues.push(t('leaseDetail.overtime.validation.periodRangeRequired'))
  }

  if (overtimeCreateForm.formulas.length === 0) {
    issues.push(t('leaseDetail.overtime.validation.formulaRequired'))
  }

  overtimeCreateForm.formulas.forEach((formula, index) => {
    const rowLabel = t('leaseDetail.overtime.validation.rowLabel', { index: index + 1 })
    if (!formula.charge_type.trim()) {
      issues.push(t('leaseDetail.overtime.validation.chargeTypeRequired', { row: rowLabel }))
    }
    if (formula.currency_type_id <= 0) {
      issues.push(t('leaseDetail.overtime.validation.currencyTypeRequired', { row: rowLabel }))
    }
    if (formula.effective_from && formula.effective_to && formula.effective_from > formula.effective_to) {
      issues.push(t('leaseDetail.overtime.validation.effectiveRangeInvalid', { row: rowLabel }))
    }
    if (formula.formula_type === 'percentage' && formula.percentage_tiers.length === 0) {
      issues.push(t('leaseDetail.overtime.validation.percentageTierRequired', { row: rowLabel }))
    }
  })

  return issues
}

const handleBack = async () => {
  await router.push({ name: 'lease-contracts' })
}

const handleAmend = async () => {
  if (!lease.value || !canAmendLease.value) {
    return
  }

  await router.push({
    name: 'lease-contracts-amend',
    params: { id: String(lease.value.id) },
  })
}

const handleSubmitForApproval = async () => {
  if (!lease.value) {
    return
  }

  isSubmitting.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const response = await submitLease(lease.value.id, {
      idempotency_key: crypto.randomUUID(),
    })

    lease.value = response.data.lease
    successMessage.value = t('leaseDetail.feedback.submitted')
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('leaseDetail.errors.unableToSubmit'))
  } finally {
    isSubmitting.value = false
  }
}

const handleTerminate = async () => {
  const valid = await terminateFormRef.value?.validate().catch(() => false)
  if (!valid || !lease.value) {
    return
  }

  isTerminating.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const response = await terminateLease(lease.value.id, {
      terminated_at: terminateForm.terminated_at,
    })

    lease.value = response.data.lease
    terminateForm.terminated_at = response.data.lease.terminated_at?.slice(0, 10) ?? terminateForm.terminated_at
    successMessage.value = t('leaseDetail.feedback.terminated')
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('leaseDetail.errors.unableToTerminate'))
  } finally {
    isTerminating.value = false
  }
}

const handleCreateOvertime = async () => {
  if (!lease.value) {
    return
  }

  const issues = validateOvertimeCreateForm()
  if (issues.length > 0) {
    overtimeErrorMessage.value = issues.join(' ')
    return
  }

  isCreatingOvertime.value = true
  overtimeErrorMessage.value = ''
  errorMessage.value = ''
  successMessage.value = ''

  try {
    await createOvertimeBill({
      lease_contract_id: lease.value.id,
      period_start: overtimeCreateForm.period_start,
      period_end: overtimeCreateForm.period_end,
      note: overtimeCreateForm.note.trim(),
      formulas: overtimeCreateForm.formulas.map((formula) => ({
        ...formula,
        charge_type: formula.charge_type.trim(),
        percentage_option: formula.formula_type === 'percentage' ? formula.percentage_option.trim() : '',
        minimum_option: formula.formula_type === 'percentage' ? formula.minimum_option.trim() : '',
        percentage_tiers: formula.formula_type === 'percentage' ? formula.percentage_tiers : [],
        minimum_tiers: formula.formula_type === 'percentage' ? formula.minimum_tiers : [],
      })),
    })

    resetOvertimeCreateForm()
    successMessage.value = t('leaseDetail.overtime.feedback.created')
    await loadOvertimeBills()
  } catch (error) {
    overtimeErrorMessage.value = getErrorMessage(error, t('leaseDetail.overtime.errors.unableToCreate'))
  } finally {
    isCreatingOvertime.value = false
  }
}

const handleOvertimeAction = async (billId: number, action: () => Promise<void>, successKey: string) => {
  overtimeActionBillId.value = billId
  overtimeErrorMessage.value = ''
  errorMessage.value = ''
  successMessage.value = ''

  try {
    await action()
    successMessage.value = successKey
    await loadOvertimeBills()
  } catch (error) {
    overtimeErrorMessage.value = getErrorMessage(error, t('leaseDetail.overtime.errors.actionFailed'))
  } finally {
    overtimeActionBillId.value = null
  }
}

const handleSubmitOvertime = async (bill: OvertimeBill) => {
  await handleOvertimeAction(
    bill.id,
    async () => {
      await submitOvertimeBill(bill.id, { idempotency_key: crypto.randomUUID(), comment: t('leaseDetail.overtime.feedback.submitted') })
    },
    t('leaseDetail.overtime.feedback.submitted'),
  )
}

const handleCancelOvertime = async (bill: OvertimeBill) => {
  await handleOvertimeAction(bill.id, async () => {
    await cancelOvertimeBill(bill.id)
  }, t('leaseDetail.overtime.feedback.cancelled'))
}

const handleStopOvertime = async (bill: OvertimeBill) => {
  await handleOvertimeAction(bill.id, async () => {
    await stopOvertimeBill(bill.id)
  }, t('leaseDetail.overtime.feedback.stopped'))
}

const handleGenerateOvertime = async (bill: OvertimeBill) => {
  await handleOvertimeAction(bill.id, async () => {
    await generateOvertimeBillCharges(bill.id)
  }, t('leaseDetail.overtime.feedback.generated'))
}

onMounted(async () => {
  await loadLease()
  await loadOvertimeBills()
})

watch(
  () => route.params.id,
  async () => {
    await loadLease()
    await loadOvertimeBills()
  },
)
</script>

<template>
  <div class="lease-detail-view" v-loading="isLoading" data-testid="lease-detail-view">
    <PageSection :eyebrow="t('lease.eyebrow')" :title="pageTitle" :summary="t('leaseDetail.summary')">
      <template #actions>
        <el-tag v-if="lease" :type="statusTagType(lease.status)" effect="plain">{{ resolveStatusLabel(lease.status) }}</el-tag>
        <el-button data-testid="lease-detail-back-button" @click="handleBack">{{ t('leaseDetail.actions.backToList') }}</el-button>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      :title="t('leaseDetail.errors.detailUnavailable')"
      type="error"
      show-icon
      :description="errorMessage"
      data-testid="lease-detail-error-alert"
    />

    <el-alert
      v-if="successMessage"
      :closable="false"
      :title="t('leaseDetail.feedback.actionCompleted')"
      type="success"
      show-icon
      :description="successMessage"
      data-testid="lease-detail-success-alert"
    />

    <el-skeleton v-if="isLoading" :rows="6" animated />

    <template v-else-if="lease">
      <section class="lease-detail-view__grid">
        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.overview') }}</span>
            </div>
          </template>

          <el-descriptions :column="2" border>
            <el-descriptions-item :label="t('leaseDetail.fields.leaseNumber')">{{ lease.lease_no }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.subtype')">{{ resolveSubtypeLabel(lease.subtype) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.tenant')">{{ lease.tenant_name }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.department')">{{ lease.department_id }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.store')">{{ lease.store_id }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.startDate')">{{ formatDate(lease.start_date) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.endDate')">{{ formatDate(lease.end_date) }}</el-descriptions-item>
            <el-descriptions-item :label="t('common.columns.status')">{{ resolveStatusLabel(lease.status) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.workflowInstance')">
              {{ lease.workflow_instance_id ?? t('leaseDetail.defaults.notCreatedYet') }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.submittedAt')">{{ formatDate(lease.submitted_at) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.approvedAt')">{{ formatDate(lease.approved_at) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.billingEffectiveAt')">{{ formatDate(lease.billing_effective_at) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.terminatedAt')">{{ formatDate(lease.terminated_at) }}</el-descriptions-item>
          </el-descriptions>
        </el-card>

        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.workflowActions') }}</span>
            </div>
          </template>

          <div class="lease-detail-view__actions">
            <el-button
              v-if="canAmendLease"
              type="warning"
              plain
              data-testid="lease-amend-button"
              @click="handleAmend"
            >
              {{ t('leaseDetail.actions.amendLease') }}
            </el-button>

            <el-button
              type="primary"
              :loading="isSubmitting"
              :disabled="submitDisabled"
              data-testid="lease-submit-button"
              @click="handleSubmitForApproval"
            >
              {{ t('leaseDetail.actions.submitForApproval') }}
            </el-button>

            <div class="lease-detail-view__terminate-panel">
              <el-form ref="terminateFormRef" :model="terminateForm" :rules="terminateFormRules" label-position="top">
                <el-form-item :label="t('leaseDetail.fields.terminateOn')" prop="terminated_at">
                  <el-date-picker
                    v-model="terminateForm.terminated_at"
                    type="date"
                    value-format="YYYY-MM-DD"
                    :placeholder="t('leaseDetail.placeholders.selectTerminationDate')"
                    data-testid="lease-terminate-date-input"
                  />
                </el-form-item>
              </el-form>

              <el-button
                type="danger"
                plain
                :loading="isTerminating"
                :disabled="terminateDisabled"
                data-testid="lease-terminate-button"
                @click="handleTerminate"
              >
                {{ t('leaseDetail.actions.terminateLease') }}
              </el-button>
            </div>
          </div>
        </el-card>
      </section>

      <section class="lease-detail-view__grid lease-detail-view__grid--secondary">
        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.units') }}</span>
            </div>
          </template>

          <el-table :data="lease.units" row-key="id" :empty-text="t('leaseDetail.table.unitsEmpty')">
            <el-table-column prop="unit_id" :label="t('leaseDetail.fields.unitId')" min-width="120" />
            <el-table-column :label="t('leaseDetail.fields.rentArea')" min-width="140">
              <template #default="scope">
                {{ formatDecimal(scope.row.rent_area) }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>

        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.terms') }}</span>
            </div>
          </template>

          <el-table :data="lease.terms" row-key="id" :empty-text="t('leaseDetail.table.termsEmpty')">
            <el-table-column :label="t('leaseDetail.fields.termType')" min-width="140">
              <template #default="scope">
                {{ resolveTermTypeLabel(scope.row.term_type) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('leaseDetail.fields.billingCycle')" min-width="140">
              <template #default="scope">
                {{ resolveBillingCycleLabel(scope.row.billing_cycle) }}
              </template>
            </el-table-column>
            <el-table-column prop="currency_type_id" :label="t('leaseDetail.fields.currencyTypeId')" min-width="120" />
            <el-table-column :label="t('leaseDetail.fields.amount')" min-width="120">
              <template #default="scope">
                {{ formatDecimal(scope.row.amount) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('leaseDetail.fields.effectiveFrom')" min-width="140">
              <template #default="scope">
                {{ formatDate(scope.row.effective_from) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('leaseDetail.fields.effectiveTo')" min-width="140">
              <template #default="scope">
                {{ formatDate(scope.row.effective_to) }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </section>

      <el-card class="lease-detail-view__card" shadow="never" data-testid="lease-subtype-details">
        <template #header>
          <div class="lease-detail-view__card-header">
            <span>{{ t('leaseDetail.cards.subtype') }}</span>
          </div>
        </template>

        <el-descriptions v-if="lease.subtype === 'joint_operation' && lease.joint_operation" :column="3" border>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationBillCycle')">{{ lease.joint_operation.bill_cycle }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationAccountCycle')">{{ lease.joint_operation.account_cycle }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationRentIncrement')">{{ lease.joint_operation.rent_inc }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationTaxType')">{{ lease.joint_operation.tax_type }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationTaxRate')">{{ formatDecimal(lease.joint_operation.tax_rate) }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationSettlementCurrencyTypeId')">{{ lease.joint_operation.settlement_currency_type_id }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationInTaxRate')">{{ formatDecimal(lease.joint_operation.in_tax_rate) }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationOutTaxRate')">{{ formatDecimal(lease.joint_operation.out_tax_rate) }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationMonthSettleDays')">{{ formatDecimal(lease.joint_operation.month_settle_days) }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationLatePayInterestRate')">{{ formatDecimal(lease.joint_operation.late_pay_interest_rate) }}</el-descriptions-item>
          <el-descriptions-item :label="t('leaseCreate.fields.jointOperationInterestGraceDays')">{{ lease.joint_operation.interest_grace_days }}</el-descriptions-item>
        </el-descriptions>

        <el-table
          v-else-if="lease.subtype === 'ad_board'"
          :data="lease.ad_boards"
          row-key="id"
          :empty-text="t('leaseDetail.table.adBoardsEmpty')"
        >
          <el-table-column prop="ad_board_id" :label="t('leaseCreate.fields.adBoardId')" min-width="120" />
          <el-table-column prop="description" :label="t('leaseCreate.fields.adBoardDescription')" min-width="180" />
          <el-table-column prop="frequency" :label="t('leaseCreate.fields.adBoardFrequency')" min-width="120" />
          <el-table-column :label="t('leaseCreate.fields.startDate')" min-width="140">
            <template #default="scope">{{ formatDate(scope.row.start_date) }}</template>
          </el-table-column>
          <el-table-column :label="t('leaseCreate.fields.endDate')" min-width="140">
            <template #default="scope">{{ formatDate(scope.row.end_date) }}</template>
          </el-table-column>
          <el-table-column :label="t('leaseCreate.fields.rentArea')" min-width="120">
            <template #default="scope">{{ formatDecimal(scope.row.rent_area) }}</template>
          </el-table-column>
          <el-table-column prop="airtime" :label="t('leaseCreate.fields.adBoardAirtime')" min-width="120" />
        </el-table>

        <el-table
          v-else-if="lease.subtype === 'area_ground'"
          :data="lease.area_grounds"
          row-key="id"
          :empty-text="t('leaseDetail.table.areaGroundsEmpty')"
        >
          <el-table-column prop="code" :label="t('leaseCreate.fields.areaGroundCode')" min-width="140" />
          <el-table-column prop="name" :label="t('leaseCreate.fields.areaGroundName')" min-width="180" />
          <el-table-column prop="type_id" :label="t('leaseCreate.fields.areaGroundTypeId')" min-width="120" />
          <el-table-column :label="t('leaseCreate.fields.startDate')" min-width="140">
            <template #default="scope">{{ formatDate(scope.row.start_date) }}</template>
          </el-table-column>
          <el-table-column :label="t('leaseCreate.fields.endDate')" min-width="140">
            <template #default="scope">{{ formatDate(scope.row.end_date) }}</template>
          </el-table-column>
          <el-table-column :label="t('leaseCreate.fields.rentArea')" min-width="120">
            <template #default="scope">{{ formatDecimal(scope.row.rent_area) }}</template>
          </el-table-column>
        </el-table>

        <el-empty v-else :description="t('leaseDetail.emptyStates.standardSubtype')" />
      </el-card>

      <el-card class="lease-detail-view__card" shadow="never" data-testid="lease-overtime-section">
        <template #header>
          <div class="lease-detail-view__card-header">
            <span>{{ t('leaseDetail.cards.overtime') }}</span>
            <el-tag effect="plain" type="info">{{ t('leaseDetail.overtime.meta.billCount', { count: overtimeBills.length }) }}</el-tag>
          </div>
        </template>

        <el-alert
          v-if="!canViewOvertime"
          :closable="false"
          type="info"
          show-icon
          :title="t('leaseDetail.overtime.permissionTitle')"
          :description="t('leaseDetail.overtime.permissionDescription')"
        />

        <template v-else>
          <el-alert
            v-if="overtimeErrorMessage"
            :closable="false"
            class="lease-detail-view__overtime-alert"
            type="error"
            show-icon
            :title="t('leaseDetail.overtime.errors.sectionUnavailable')"
            :description="overtimeErrorMessage"
          />

          <section class="lease-detail-view__overtime-create">
            <div class="lease-detail-view__section-header">
              <div>
                <h3>{{ t('leaseDetail.overtime.cards.create') }}</h3>
                <p>{{ t('leaseDetail.overtime.cards.createSummary') }}</p>
              </div>
              <el-button type="primary" plain :disabled="createOvertimeDisabled" @click="addFormulaRow">
                {{ t('leaseDetail.overtime.actions.addFormulaRow') }}
              </el-button>
            </div>

            <div class="lease-detail-view__grid">
              <el-form-item :label="t('leaseDetail.overtime.fields.periodStart')">
                <el-date-picker v-model="overtimeCreateForm.period_start" type="date" value-format="YYYY-MM-DD" data-testid="lease-overtime-period-start-input" />
              </el-form-item>
              <el-form-item :label="t('leaseDetail.overtime.fields.periodEnd')">
                <el-date-picker v-model="overtimeCreateForm.period_end" type="date" value-format="YYYY-MM-DD" data-testid="lease-overtime-period-end-input" />
              </el-form-item>
              <el-form-item :label="t('leaseDetail.overtime.fields.note')" class="lease-detail-view__grid-span-2">
                <el-input v-model="overtimeCreateForm.note" type="textarea" :rows="2" data-testid="overtime-note-input" />
              </el-form-item>
            </div>

            <article
              v-for="(formula, formulaIndex) in overtimeCreateForm.formulas"
              :key="formulaIndex"
              class="lease-detail-view__formula-card"
            >
              <div class="lease-detail-view__formula-header">
                <span>{{ t('leaseDetail.overtime.cards.formulaRow', { index: formulaIndex + 1 }) }}</span>
                <el-button link type="danger" @click="removeFormulaRow(formulaIndex)">
                  {{ t('leaseDetail.overtime.actions.removeRow') }}
                </el-button>
              </div>

              <div class="lease-detail-view__grid">
                <el-form-item :label="t('leaseDetail.overtime.fields.chargeType')">
                  <el-input v-model="formula.charge_type" :data-testid="`overtime-charge-type-input-${formulaIndex}`" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.formulaType')">
                  <el-select v-model="formula.formula_type" :data-testid="`overtime-formula-type-select-${formulaIndex}`">
                    <el-option
                      v-for="option in overtimeFormulaTypeOptions"
                      :key="option.value"
                      :label="option.label"
                      :value="option.value"
                    />
                  </el-select>
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.rateType')">
                  <el-select v-model="formula.rate_type" :data-testid="`overtime-rate-type-select-${formulaIndex}`">
                    <el-option
                      v-for="option in overtimeRateTypeOptions"
                      :key="option.value"
                      :label="option.label"
                      :value="option.value"
                    />
                  </el-select>
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.currencyTypeId')">
                  <el-input-number v-model="formula.currency_type_id" :min="1" controls-position="right" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.effectiveFrom')">
                  <el-date-picker v-model="formula.effective_from" type="date" value-format="YYYY-MM-DD" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.effectiveTo')">
                  <el-date-picker v-model="formula.effective_to" type="date" value-format="YYYY-MM-DD" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.totalArea')">
                  <el-input-number v-model="formula.total_area" :min="0" :precision="2" controls-position="right" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.unitPrice')">
                  <el-input-number v-model="formula.unit_price" :min="0" :precision="2" controls-position="right" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.baseAmount')">
                  <el-input-number v-model="formula.base_amount" :min="0" :precision="2" controls-position="right" />
                </el-form-item>
                <el-form-item :label="t('leaseDetail.overtime.fields.fixedRental')">
                  <el-input-number v-model="formula.fixed_rental" :min="0" :precision="2" controls-position="right" />
                </el-form-item>
              </div>

              <div v-if="formula.formula_type === 'percentage'" class="lease-detail-view__formula-subsection">
                <div class="lease-detail-view__grid">
                  <el-form-item :label="t('leaseDetail.overtime.fields.percentageOption')">
                    <el-input v-model="formula.percentage_option" />
                  </el-form-item>
                  <el-form-item :label="t('leaseDetail.overtime.fields.minimumOption')">
                    <el-input v-model="formula.minimum_option" />
                  </el-form-item>
                </div>

                <div class="lease-detail-view__tier-block">
                  <div class="lease-detail-view__formula-header">
                    <span>{{ t('leaseDetail.overtime.cards.percentageTiers') }}</span>
                    <el-button link type="primary" @click="addPercentageTier(formulaIndex)">
                      {{ t('leaseDetail.overtime.actions.addTier') }}
                    </el-button>
                  </div>

                  <div v-for="(tier, tierIndex) in formula.percentage_tiers" :key="`pct-${tierIndex}`" class="lease-detail-view__tier-row">
                    <el-input-number v-model="tier.sales_to" :min="0" :precision="2" controls-position="right" />
                    <el-input-number v-model="tier.percentage" :min="0" :precision="4" controls-position="right" />
                    <el-button link type="danger" @click="removePercentageTier(formulaIndex, tierIndex)">
                      {{ t('leaseDetail.overtime.actions.removeRow') }}
                    </el-button>
                  </div>
                </div>

                <div class="lease-detail-view__tier-block">
                  <div class="lease-detail-view__formula-header">
                    <span>{{ t('leaseDetail.overtime.cards.minimumTiers') }}</span>
                    <el-button link type="primary" @click="addMinimumTier(formulaIndex)">
                      {{ t('leaseDetail.overtime.actions.addTier') }}
                    </el-button>
                  </div>

                  <div v-for="(tier, tierIndex) in formula.minimum_tiers" :key="`min-${tierIndex}`" class="lease-detail-view__tier-row">
                    <el-input-number v-model="tier.sales_to" :min="0" :precision="2" controls-position="right" />
                    <el-input-number v-model="tier.minimum_sum" :min="0" :precision="2" controls-position="right" />
                    <el-button link type="danger" @click="removeMinimumTier(formulaIndex, tierIndex)">
                      {{ t('leaseDetail.overtime.actions.removeRow') }}
                    </el-button>
                  </div>
                </div>
              </div>
            </article>

            <div class="lease-detail-view__actions">
              <el-button
                type="primary"
                :loading="isCreatingOvertime"
                :disabled="createOvertimeDisabled"
                data-testid="lease-overtime-create-button"
                @click="handleCreateOvertime"
              >
                {{ t('leaseDetail.overtime.actions.create') }}
              </el-button>
            </div>
          </section>

          <el-table
            :data="overtimeBills"
            row-key="id"
            class="lease-detail-view__overtime-table"
            :empty-text="t('leaseDetail.overtime.table.empty')"
            data-testid="lease-overtime-table"
          >
            <el-table-column prop="id" label="ID" min-width="80" />
            <el-table-column :label="t('leaseDetail.overtime.fields.period')" min-width="220">
              <template #default="scope">
                {{ formatDate(scope.row.period_start) }} → {{ formatDate(scope.row.period_end) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('common.columns.status')" min-width="150">
              <template #default="scope">
                <el-tag :type="statusTagType(scope.row.status)" effect="plain">{{ resolveStatusLabel(scope.row.status) }}</el-tag>
              </template>
            </el-table-column>
            <el-table-column prop="workflow_instance_id" :label="t('leaseDetail.fields.workflowInstance')" min-width="150" />
            <el-table-column :label="t('leaseDetail.overtime.fields.generatedAt')" min-width="160">
              <template #default="scope">{{ formatDate(scope.row.generated_at) }}</template>
            </el-table-column>
            <el-table-column :label="t('common.columns.actions')" min-width="320" fixed="right">
              <template #default="scope">
                <el-button
                  link
                  type="primary"
                  :disabled="!canEditOvertime || scope.row.status !== 'draft' || overtimeActionBillId === scope.row.id"
                  :data-testid="`overtime-row-submit-button-${scope.row.id}`"
                  @click="handleSubmitOvertime(scope.row)"
                >
                  {{ t('leaseDetail.overtime.actions.submit') }}
                </el-button>
                <el-button
                  link
                  type="danger"
                  :disabled="!canEditOvertime || !['draft', 'pending_approval', 'rejected', 'approved'].includes(scope.row.status) || overtimeActionBillId === scope.row.id"
                  :data-testid="`overtime-row-cancel-button-${scope.row.id}`"
                  @click="handleCancelOvertime(scope.row)"
                >
                  {{ t('leaseDetail.overtime.actions.cancel') }}
                </el-button>
                <el-button
                  link
                  type="warning"
                  :disabled="!canEditOvertime || scope.row.status !== 'approved' || overtimeActionBillId === scope.row.id"
                  :data-testid="`overtime-row-stop-button-${scope.row.id}`"
                  @click="handleStopOvertime(scope.row)"
                >
                  {{ t('leaseDetail.overtime.actions.stop') }}
                </el-button>
                <el-button
                  link
                  type="success"
                  :disabled="!canEditOvertime || !['approved', 'generated'].includes(scope.row.status) || overtimeActionBillId === scope.row.id"
                  :data-testid="`overtime-row-generate-button-${scope.row.id}`"
                  @click="handleGenerateOvertime(scope.row)"
                >
                  {{ t('leaseDetail.overtime.actions.generate') }}
                </el-button>
              </template>
            </el-table-column>
          </el-table>
        </template>
      </el-card>
    </template>
  </div>
</template>

<style scoped>
.lease-detail-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.lease-detail-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.lease-detail-view__grid--secondary {
  align-items: start;
}

.lease-detail-view__grid-span-2 {
  grid-column: 1 / -1;
}

.lease-detail-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.lease-detail-view__card-header,
.lease-detail-view__section-header,
.lease-detail-view__formula-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
}

.lease-detail-view__card-header {
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.lease-detail-view__section-header {
  align-items: flex-start;
}

.lease-detail-view__section-header h3,
.lease-detail-view__formula-header span {
  margin: 0;
  font-size: var(--mi-font-size-300);
  color: var(--mi-color-text);
}

.lease-detail-view__section-header p {
  margin: 0;
  color: var(--mi-color-muted);
}

.lease-detail-view__actions,
.lease-detail-view__terminate-panel,
.lease-detail-view__overtime-create,
.lease-detail-view__formula-subsection,
.lease-detail-view__tier-block {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.lease-detail-view__formula-card {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid color-mix(in srgb, var(--mi-color-primary) 18%, var(--mi-color-border));
  border-radius: var(--mi-radius-md);
  background: rgba(29, 78, 216, 0.04);
}

.lease-detail-view__tier-row {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr)) auto;
  gap: var(--mi-space-3);
  align-items: center;
}

.lease-detail-view__overtime-alert {
  margin-bottom: var(--mi-space-4);
}

.lease-detail-view__overtime-table {
  margin-top: var(--mi-space-4);
}

.lease-detail-view :deep(.el-input-number),
.lease-detail-view :deep(.el-input),
.lease-detail-view :deep(.el-date-editor),
.lease-detail-view :deep(.el-select) {
  width: 100%;
}

.lease-detail-view__terminate-panel :deep(.el-date-editor) {
  width: 100%;
}

@media (max-width: 52rem) {
  .lease-detail-view__grid,
  .lease-detail-view__tier-row {
    grid-template-columns: minmax(0, 1fr);
  }

  .lease-detail-view__card-header,
  .lease-detail-view__section-header,
  .lease-detail-view__formula-header {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
