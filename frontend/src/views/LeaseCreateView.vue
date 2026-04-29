<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'

import { listBrands, listCustomers, type Brand, type Customer } from '../api/masterdata'
import {
  amendLease,
  createLease,
  getLease,
  type CreateLeaseRequest,
  type LeaseAreaGroundDetail,
  type LeaseAdBoardDetail,
  type LeaseContract,
  type LeaseContractSubtype,
  type LeaseJointOperationFields,
} from '../api/lease'
import { listDepartments, listStores, type Department, type Store } from '../api/org'
import PageSection from '../components/platform/PageSection.vue'
import { getErrorMessage } from '../composables/useErrorMessage'

type JointOperationForm = Omit<LeaseJointOperationFields, 'lease_contract_id' | 'created_at' | 'updated_at'>

type AdBoardForm = Omit<LeaseAdBoardDetail, 'id' | 'lease_contract_id' | 'created_at' | 'updated_at'>

type AreaGroundForm = Omit<LeaseAreaGroundDetail, 'id' | 'lease_contract_id' | 'created_at' | 'updated_at'>

type LeaseUnitDraft = CreateLeaseRequest['units'][number]

type LeaseTermDraft = CreateLeaseRequest['terms'][number]

type LeaseCreateForm = {
  lease_no: string
  subtype: LeaseContractSubtype
  tenant_name: string
  department_id: number | null
  store_id: number | null
  building_id: number | null
  customer_id: number | null
  brand_id: number | null
  trade_id: number | null
  management_type_id: number | null
  start_date: string
  end_date: string
  unit_id: number | null
  rent_area: number | null
  term_type: string
  billing_cycle: string
  currency_type_id: number | null
  amount: number | null
  effective_from: string
  effective_to: string
  joint_operation: JointOperationForm
  ad_boards: AdBoardForm[]
  area_grounds: AreaGroundForm[]
}

const createJointOperationForm = (): JointOperationForm => ({
  bill_cycle: 30,
  rent_inc: '',
  account_cycle: 30,
  tax_rate: 0,
  tax_type: 0,
  settlement_currency_type_id: 1,
  in_tax_rate: 0,
  out_tax_rate: 0,
  month_settle_days: 0,
  late_pay_interest_rate: 0,
  interest_grace_days: 0,
})

const createAdBoardForm = (): AdBoardForm => ({
  ad_board_id: 0,
  description: '',
  status: 1,
  start_date: '',
  end_date: '',
  rent_area: 0,
  airtime: 0,
  frequency: 'W',
  frequency_days: 0,
  frequency_mon: true,
  frequency_tue: false,
  frequency_wed: false,
  frequency_thu: false,
  frequency_fri: false,
  frequency_sat: false,
  frequency_sun: false,
  between_from: 0,
  between_to: 0,
  store_id: null,
  building_id: null,
})

const createAreaGroundForm = (): AreaGroundForm => ({
  code: '',
  name: '',
  type_id: 0,
  description: '',
  status: 1,
  start_date: '',
  end_date: '',
  rent_area: 0,
})

const route = useRoute()
const router = useRouter()
const { t } = useI18n()

const formRef = ref<FormInstance>()
const isSaving = ref(false)
const errorMessage = ref('')
const sourceErrorMessage = ref('')
const setupErrorMessage = ref('')
const isLoadingOptions = ref(false)
const isLoadingSourceLease = ref(false)
const customers = ref<Customer[]>([])
const brands = ref<Brand[]>([])
const departments = ref<Department[]>([])
const stores = ref<Store[]>([])
const sourceLease = ref<LeaseContract | null>(null)
const amendmentUnits = ref<LeaseUnitDraft[]>([])
const amendmentTerms = ref<LeaseTermDraft[]>([])

const form = reactive<LeaseCreateForm>({
  lease_no: '',
  subtype: 'standard',
  tenant_name: '',
  department_id: null,
  store_id: null,
  building_id: null,
  customer_id: null,
  brand_id: null,
  trade_id: null,
  management_type_id: null,
  start_date: '',
  end_date: '',
  unit_id: null,
  rent_area: null,
  term_type: 'rent',
  billing_cycle: 'monthly',
  currency_type_id: 1,
  amount: null,
  effective_from: '',
  effective_to: '',
  joint_operation: createJointOperationForm(),
  ad_boards: [createAdBoardForm()],
  area_grounds: [createAreaGroundForm()],
})

const isAmendmentMode = computed(() => route.name === 'lease-contracts-amend')

const sourceLeaseId = computed(() => {
  if (!isAmendmentMode.value) {
    return null
  }

  const rawId = route.params.id
  const normalizedId = Array.isArray(rawId) ? rawId[0] : rawId
  const parsedId = Number(normalizedId)

  return Number.isFinite(parsedId) && parsedId > 0 ? parsedId : null
})

const isInitializing = computed(() => isLoadingOptions.value || isLoadingSourceLease.value)
const pageTitle = computed(() => (isAmendmentMode.value ? t('leaseCreate.modes.amendment.title') : t('leaseCreate.title')))
const pageSummary = computed(() => (isAmendmentMode.value ? t('leaseCreate.modes.amendment.summary') : t('leaseCreate.summary')))
const setupCardTitle = computed(() =>
  isAmendmentMode.value ? t('leaseCreate.modes.amendment.cards.setup') : t('leaseCreate.cards.setup'),
)
const submitActionLabel = computed(() =>
  isAmendmentMode.value ? t('leaseCreate.modes.amendment.actions.submit') : t('leaseCreate.actions.create'),
)
const submissionErrorTitle = computed(() =>
  isAmendmentMode.value ? t('leaseCreate.errors.amendmentFailed') : t('leaseCreate.errors.creationFailed'),
)

const rules = computed<FormRules<LeaseCreateForm>>(() => ({
  lease_no: [{ required: true, message: t('leaseCreate.validation.leaseNumberRequired'), trigger: 'blur' }],
  subtype: [{ required: true, message: t('leaseCreate.validation.subtypeRequired'), trigger: 'change' }],
  tenant_name: [{ required: true, message: t('leaseCreate.validation.tenantNameRequired'), trigger: 'blur' }],
  department_id: [{ required: true, message: t('leaseCreate.validation.departmentRequired'), trigger: 'change' }],
  store_id: [{ required: true, message: t('leaseCreate.validation.storeRequired'), trigger: 'change' }],
  start_date: [{ required: true, message: t('leaseCreate.validation.startDateRequired'), trigger: 'change' }],
  end_date: [{ required: true, message: t('leaseCreate.validation.endDateRequired'), trigger: 'change' }],
  unit_id: [{ required: true, message: t('leaseCreate.validation.unitIdRequired'), trigger: 'change' }],
  rent_area: [{ required: true, message: t('leaseCreate.validation.rentAreaRequired'), trigger: 'change' }],
  term_type: [{ required: true, message: t('leaseCreate.validation.termTypeRequired'), trigger: 'change' }],
  billing_cycle: [{ required: true, message: t('leaseCreate.validation.billingCycleRequired'), trigger: 'change' }],
  currency_type_id: [{ required: true, message: t('leaseCreate.validation.currencyTypeIdRequired'), trigger: 'change' }],
  amount: [{ required: true, message: t('leaseCreate.validation.amountRequired'), trigger: 'change' }],
  effective_from: [{ required: true, message: t('leaseCreate.validation.effectiveFromRequired'), trigger: 'change' }],
  effective_to: [{ required: true, message: t('leaseCreate.validation.effectiveToRequired'), trigger: 'change' }],
}))

const subtypeOptions = computed(() => [
  { label: t('leaseCreate.options.subtypes.standard'), value: 'standard' },
  { label: t('leaseCreate.options.subtypes.jointOperation'), value: 'joint_operation' },
  { label: t('leaseCreate.options.subtypes.adBoard'), value: 'ad_board' },
  { label: t('leaseCreate.options.subtypes.areaGround'), value: 'area_ground' },
])

const termTypeOptions = computed(() => [
  { label: t('leaseCreate.options.termTypes.rent'), value: 'rent' },
  { label: t('leaseCreate.options.termTypes.deposit'), value: 'deposit' },
])

const billingCycleOptions = computed(() => [
  { label: t('leaseCreate.options.billingCycles.monthly'), value: 'monthly' },
])

const adBoardFrequencyOptions = computed(() => [
  { label: t('leaseCreate.options.adBoardFrequencies.daily'), value: 'D' },
  { label: t('leaseCreate.options.adBoardFrequencies.monthly'), value: 'M' },
  { label: t('leaseCreate.options.adBoardFrequencies.weekly'), value: 'W' },
])

const weekdayOptions = computed(() => [
  { key: 'frequency_mon', label: t('leaseCreate.options.weekdays.monday') },
  { key: 'frequency_tue', label: t('leaseCreate.options.weekdays.tuesday') },
  { key: 'frequency_wed', label: t('leaseCreate.options.weekdays.wednesday') },
  { key: 'frequency_thu', label: t('leaseCreate.options.weekdays.thursday') },
  { key: 'frequency_fri', label: t('leaseCreate.options.weekdays.friday') },
  { key: 'frequency_sat', label: t('leaseCreate.options.weekdays.saturday') },
  { key: 'frequency_sun', label: t('leaseCreate.options.weekdays.sunday') },
])

const availableStores = computed(() => {
  if (form.department_id === null) {
    return stores.value
  }

  return stores.value.filter((store) => store.department_id === form.department_id)
})

const isJointOperationSubtype = computed(() => form.subtype === 'joint_operation')
const isAdBoardSubtype = computed(() => form.subtype === 'ad_board')
const isAreaGroundSubtype = computed(() => form.subtype === 'area_ground')

const resetFormState = () => {
  form.lease_no = ''
  form.subtype = 'standard'
  form.tenant_name = ''
  form.department_id = null
  form.store_id = null
  form.building_id = null
  form.customer_id = null
  form.brand_id = null
  form.trade_id = null
  form.management_type_id = null
  form.start_date = ''
  form.end_date = ''
  form.unit_id = null
  form.rent_area = null
  form.term_type = 'rent'
  form.billing_cycle = 'monthly'
  form.currency_type_id = 1
  form.amount = null
  form.effective_from = ''
  form.effective_to = ''
  form.joint_operation = createJointOperationForm()
  form.ad_boards = [createAdBoardForm()]
  form.area_grounds = [createAreaGroundForm()]
  amendmentUnits.value = []
  amendmentTerms.value = []
  sourceLease.value = null
}

const loadReferenceData = async () => {
  isLoadingOptions.value = true
  setupErrorMessage.value = ''

  try {
    const [customersResponse, brandsResponse, departmentsResponse, storesResponse] = await Promise.all([
      listCustomers(),
      listBrands(),
      listDepartments(),
      listStores(),
    ])

    customers.value = customersResponse.data.customers
    brands.value = brandsResponse.data.brands
    departments.value = departmentsResponse.data.departments
    stores.value = storesResponse.data.stores
  } catch (error) {
    customers.value = []
    brands.value = []
    departments.value = []
    stores.value = []
    setupErrorMessage.value = getErrorMessage(error, t('leaseCreate.errors.unableToLoadReferenceData'))
  } finally {
    isLoadingOptions.value = false
  }
}

const normalizeAdBoardForm = (detail: LeaseAdBoardDetail): AdBoardForm => ({
  ad_board_id: detail.ad_board_id,
  description: detail.description,
  status: detail.status,
  start_date: detail.start_date,
  end_date: detail.end_date,
  rent_area: detail.rent_area,
  airtime: detail.airtime,
  frequency: detail.frequency,
  frequency_days: detail.frequency_days,
  frequency_mon: detail.frequency_mon,
  frequency_tue: detail.frequency_tue,
  frequency_wed: detail.frequency_wed,
  frequency_thu: detail.frequency_thu,
  frequency_fri: detail.frequency_fri,
  frequency_sat: detail.frequency_sat,
  frequency_sun: detail.frequency_sun,
  between_from: detail.between_from,
  between_to: detail.between_to,
  store_id: detail.store_id,
  building_id: detail.building_id,
})

const normalizeAreaGroundForm = (detail: LeaseAreaGroundDetail): AreaGroundForm => ({
  code: detail.code,
  name: detail.name,
  type_id: detail.type_id,
  description: detail.description,
  status: detail.status,
  start_date: detail.start_date,
  end_date: detail.end_date,
  rent_area: detail.rent_area,
})

const applySourceLeaseToForm = (lease: LeaseContract) => {
  const primaryUnit = lease.units[0]
  const primaryTerm = lease.terms[0]

  sourceLease.value = lease
  amendmentUnits.value = lease.units.map((unit) => ({
    unit_id: unit.unit_id,
    rent_area: unit.rent_area,
  }))
  amendmentTerms.value = lease.terms.map((term) => ({
    term_type: term.term_type,
    billing_cycle: term.billing_cycle,
    currency_type_id: term.currency_type_id,
    amount: term.amount,
    effective_from: term.effective_from,
    effective_to: term.effective_to,
  }))

  form.lease_no = lease.lease_no
  form.subtype = lease.subtype
  form.tenant_name = lease.tenant_name
  form.department_id = lease.department_id
  form.store_id = lease.store_id
  form.building_id = lease.building_id
  form.customer_id = lease.customer_id
  form.brand_id = lease.brand_id
  form.trade_id = lease.trade_id
  form.management_type_id = lease.management_type_id
  form.start_date = lease.start_date
  form.end_date = lease.end_date
  form.unit_id = primaryUnit?.unit_id ?? null
  form.rent_area = primaryUnit?.rent_area ?? null
  form.term_type = primaryTerm?.term_type ?? 'rent'
  form.billing_cycle = primaryTerm?.billing_cycle ?? 'monthly'
  form.currency_type_id = primaryTerm?.currency_type_id ?? 1
  form.amount = primaryTerm?.amount ?? null
  form.effective_from = primaryTerm?.effective_from ?? ''
  form.effective_to = primaryTerm?.effective_to ?? ''
  form.joint_operation = lease.joint_operation
    ? {
        bill_cycle: lease.joint_operation.bill_cycle,
        rent_inc: lease.joint_operation.rent_inc,
        account_cycle: lease.joint_operation.account_cycle,
        tax_rate: lease.joint_operation.tax_rate,
        tax_type: lease.joint_operation.tax_type,
        settlement_currency_type_id: lease.joint_operation.settlement_currency_type_id,
        in_tax_rate: lease.joint_operation.in_tax_rate,
        out_tax_rate: lease.joint_operation.out_tax_rate,
        month_settle_days: lease.joint_operation.month_settle_days,
        late_pay_interest_rate: lease.joint_operation.late_pay_interest_rate,
        interest_grace_days: lease.joint_operation.interest_grace_days,
      }
    : createJointOperationForm()
  form.ad_boards = lease.ad_boards.length > 0 ? lease.ad_boards.map(normalizeAdBoardForm) : [createAdBoardForm()]
  form.area_grounds = lease.area_grounds.length > 0 ? lease.area_grounds.map(normalizeAreaGroundForm) : [createAreaGroundForm()]
}

const loadSourceLease = async () => {
  resetFormState()
  sourceErrorMessage.value = ''

  if (!isAmendmentMode.value) {
    return
  }

  if (!sourceLeaseId.value) {
    sourceErrorMessage.value = t('leaseCreate.errors.invalidSourceLeaseId')
    return
  }

  isLoadingSourceLease.value = true

  try {
    const response = await getLease(sourceLeaseId.value)
    applySourceLeaseToForm(response.data.lease)
  } catch (error) {
    sourceErrorMessage.value = getErrorMessage(error, t('leaseCreate.errors.unableToLoadAmendmentDraft'))
  } finally {
    isLoadingSourceLease.value = false
  }
}

watch(
  () => form.department_id,
  (departmentId) => {
    if (departmentId === null) {
      return
    }

    const hasMatchingStore = stores.value.some(
      (store) => store.id === form.store_id && store.department_id === departmentId,
    )

    if (!hasMatchingStore) {
      form.store_id = null
    }
  },
)

watch(
  () => form.subtype,
  (subtype) => {
    errorMessage.value = ''

    if (subtype === 'ad_board' && form.ad_boards.length === 0) {
      form.ad_boards.push(createAdBoardForm())
    }

    if (subtype === 'area_ground' && form.area_grounds.length === 0) {
      form.area_grounds.push(createAreaGroundForm())
    }
  },
)

const addAdBoardRow = () => {
  form.ad_boards.push(createAdBoardForm())
}

const removeAdBoardRow = (index: number) => {
  if (form.ad_boards.length === 1) {
    form.ad_boards.splice(0, 1, createAdBoardForm())
    return
  }

  form.ad_boards.splice(index, 1)
}

const addAreaGroundRow = () => {
  form.area_grounds.push(createAreaGroundForm())
}

const removeAreaGroundRow = (index: number) => {
  if (form.area_grounds.length === 1) {
    form.area_grounds.splice(0, 1, createAreaGroundForm())
    return
  }

  form.area_grounds.splice(index, 1)
}

const validateJointOperation = () => {
  const issues: string[] = []
  const joint = form.joint_operation

  if (joint.bill_cycle <= 0) issues.push(t('leaseCreate.validation.jointOperationBillCycleRequired'))
  if (!joint.rent_inc.trim()) issues.push(t('leaseCreate.validation.jointOperationRentIncrementRequired'))
  if (joint.account_cycle <= 0) issues.push(t('leaseCreate.validation.jointOperationAccountCycleRequired'))
  if (joint.tax_rate <= 0) issues.push(t('leaseCreate.validation.jointOperationTaxRateRequired'))
  if (joint.tax_type <= 0) issues.push(t('leaseCreate.validation.jointOperationTaxTypeRequired'))
  if (joint.settlement_currency_type_id <= 0) {
    issues.push(t('leaseCreate.validation.jointOperationSettlementCurrencyRequired'))
  }
  if (joint.in_tax_rate <= 0) issues.push(t('leaseCreate.validation.jointOperationInTaxRateRequired'))
  if (joint.out_tax_rate <= 0) issues.push(t('leaseCreate.validation.jointOperationOutTaxRateRequired'))

  return issues
}

const validateAdBoards = () => {
  const issues: string[] = []

  if (form.ad_boards.length === 0) {
    issues.push(t('leaseCreate.validation.adBoardRowRequired'))
    return issues
  }

  form.ad_boards.forEach((detail, index) => {
    const rowLabel = t('leaseCreate.validation.rowLabel', { index: index + 1 })
    if (detail.ad_board_id <= 0) issues.push(t('leaseCreate.validation.adBoardIdRequired', { row: rowLabel }))
    if (!detail.start_date || !detail.end_date || detail.start_date >= detail.end_date) {
      issues.push(t('leaseCreate.validation.adBoardDateRangeRequired', { row: rowLabel }))
    }
    if (detail.rent_area <= 0) issues.push(t('leaseCreate.validation.adBoardRentAreaRequired', { row: rowLabel }))
    if (detail.airtime <= 0) issues.push(t('leaseCreate.validation.adBoardAirtimeRequired', { row: rowLabel }))
    if (detail.between_from < 0 || detail.between_to < 0 || (detail.between_to > 0 && detail.between_from > detail.between_to)) {
      issues.push(t('leaseCreate.validation.adBoardBetweenRangeInvalid', { row: rowLabel }))
    }

    if ((detail.frequency === 'D' || detail.frequency === 'M') && detail.frequency_days <= 0) {
      issues.push(t('leaseCreate.validation.adBoardFrequencyDaysRequired', { row: rowLabel }))
    }

    if (
      detail.frequency === 'W' &&
      !detail.frequency_mon &&
      !detail.frequency_tue &&
      !detail.frequency_wed &&
      !detail.frequency_thu &&
      !detail.frequency_fri &&
      !detail.frequency_sat &&
      !detail.frequency_sun
    ) {
      issues.push(t('leaseCreate.validation.adBoardWeekdayRequired', { row: rowLabel }))
    }
  })

  return issues
}

const validateAreaGrounds = () => {
  const issues: string[] = []

  if (form.area_grounds.length === 0) {
    issues.push(t('leaseCreate.validation.areaGroundRowRequired'))
    return issues
  }

  form.area_grounds.forEach((detail, index) => {
    const rowLabel = t('leaseCreate.validation.rowLabel', { index: index + 1 })
    if (!detail.code.trim()) issues.push(t('leaseCreate.validation.areaGroundCodeRequired', { row: rowLabel }))
    if (!detail.name.trim()) issues.push(t('leaseCreate.validation.areaGroundNameRequired', { row: rowLabel }))
    if (detail.type_id <= 0) issues.push(t('leaseCreate.validation.areaGroundTypeRequired', { row: rowLabel }))
    if (!detail.start_date || !detail.end_date || detail.start_date >= detail.end_date) {
      issues.push(t('leaseCreate.validation.areaGroundDateRangeRequired', { row: rowLabel }))
    }
    if (detail.rent_area <= 0) issues.push(t('leaseCreate.validation.areaGroundRentAreaRequired', { row: rowLabel }))
  })

  return issues
}

const buildSubtypeIssues = () => {
  switch (form.subtype) {
    case 'joint_operation':
      return validateJointOperation()
    case 'ad_board':
      return validateAdBoards()
    case 'area_ground':
      return validateAreaGrounds()
    default:
      return []
  }
}

const buildLeasePayload = (): CreateLeaseRequest => {
  const units = amendmentUnits.value.length > 0
    ? amendmentUnits.value.map((unit) => ({ ...unit }))
    : [
        {
          unit_id: form.unit_id ?? 0,
          rent_area: form.rent_area ?? 0,
        },
      ]

  units[0] = {
    unit_id: form.unit_id ?? 0,
    rent_area: form.rent_area ?? 0,
  }

  const terms = amendmentTerms.value.length > 0
    ? amendmentTerms.value.map((term) => ({ ...term }))
    : [
        {
          term_type: form.term_type,
          billing_cycle: form.billing_cycle,
          currency_type_id: form.currency_type_id ?? 0,
          amount: form.amount ?? 0,
          effective_from: form.effective_from,
          effective_to: form.effective_to,
        },
      ]

  terms[0] = {
    term_type: form.term_type,
    billing_cycle: form.billing_cycle,
    currency_type_id: form.currency_type_id ?? 0,
    amount: form.amount ?? 0,
    effective_from: form.effective_from,
    effective_to: form.effective_to,
  }

  const payload: CreateLeaseRequest = {
    lease_no: form.lease_no.trim(),
    subtype: form.subtype,
    tenant_name: form.tenant_name.trim(),
    department_id: form.department_id ?? 0,
    store_id: form.store_id ?? 0,
    building_id: form.building_id,
    customer_id: form.customer_id,
    brand_id: form.brand_id,
    trade_id: form.trade_id,
    management_type_id: form.management_type_id,
    start_date: form.start_date,
    end_date: form.end_date,
    units,
    terms,
  }

  if (form.subtype === 'joint_operation') {
    payload.joint_operation = {
      ...form.joint_operation,
      rent_inc: form.joint_operation.rent_inc.trim(),
    }
  }

  if (form.subtype === 'ad_board') {
    payload.ad_boards = form.ad_boards.map((detail) => ({
      ...detail,
      description: detail.description.trim(),
      store_id: detail.store_id,
      building_id: detail.building_id,
    }))
  }

  if (form.subtype === 'area_ground') {
    payload.area_grounds = form.area_grounds.map((detail) => ({
      ...detail,
      code: detail.code.trim(),
      name: detail.name.trim(),
      description: detail.description.trim(),
    }))
  }

  return payload
}

const handleCancel = async () => {
  await router.push({ name: 'lease-contracts' })
}

const handleSubmit = async () => {
  errorMessage.value = ''

  if (isAmendmentMode.value && (!sourceLeaseId.value || !sourceLease.value)) {
    sourceErrorMessage.value = t('leaseCreate.errors.unableToLoadAmendmentDraft')
    return
  }

  const isValid = await formRef.value?.validate().catch(() => false)
  if (isValid !== true) {
    return
  }

  const subtypeIssues = buildSubtypeIssues()
  if (subtypeIssues.length > 0) {
    errorMessage.value = subtypeIssues.join(' ')
    return
  }

  isSaving.value = true

  try {
    const payload = buildLeasePayload()
    const response = isAmendmentMode.value && sourceLeaseId.value
      ? await amendLease(sourceLeaseId.value, payload)
      : await createLease(payload)

    await router.replace({
      name: 'lease-contract-detail',
      params: { id: String(response.data.lease.id) },
    })
  } catch (error) {
    errorMessage.value = getErrorMessage(
      error,
      isAmendmentMode.value ? t('leaseCreate.errors.unableToAmend') : t('leaseCreate.errors.unableToCreate'),
    )
  } finally {
    isSaving.value = false
  }
}

onMounted(() => {
  void Promise.all([loadReferenceData(), loadSourceLease()])
})

watch(
  () => route.fullPath,
  () => {
    errorMessage.value = ''
    void loadSourceLease()
  },
)
</script>

<template>
  <div class="lease-create-view" v-loading="isInitializing" data-testid="lease-create-view">
    <PageSection
      :eyebrow="t('lease.eyebrow')"
      :title="pageTitle"
      :summary="pageSummary"
    >
      <template #actions>
        <el-tag v-if="isAmendmentMode && sourceLease" effect="plain" type="warning" data-testid="lease-amendment-source-tag">
          {{ t('leaseCreate.modes.amendment.sourceLeaseTag', { leaseNo: sourceLease.lease_no }) }}
        </el-tag>
        <el-button data-testid="lease-create-back-button" @click="handleCancel">{{ t('leaseCreate.actions.backToList') }}</el-button>
      </template>
    </PageSection>

    <el-card class="lease-create-view__card" shadow="never">
      <template #header>
        <div class="lease-create-view__card-header">
          <span>{{ setupCardTitle }}</span>
        </div>
      </template>

      <el-alert
        v-if="isAmendmentMode && sourceLease"
        :closable="false"
        class="lease-create-view__alert"
        :title="t('leaseCreate.modes.amendment.alertTitle')"
        type="info"
        show-icon
        :description="t('leaseCreate.modes.amendment.alertDescription')"
        data-testid="lease-amendment-info-alert"
      />

      <el-alert
        v-if="sourceErrorMessage"
        :closable="false"
        class="lease-create-view__alert"
        :title="t('leaseCreate.errors.amendmentDraftUnavailable')"
        type="error"
        show-icon
        :description="sourceErrorMessage"
        data-testid="lease-amendment-error-alert"
      />

      <el-alert
        v-if="errorMessage"
        :closable="false"
        class="lease-create-view__alert"
        :title="submissionErrorTitle"
        type="error"
        show-icon
        :description="errorMessage"
        data-testid="lease-create-error-alert"
      />

      <el-alert
        v-if="setupErrorMessage"
        :closable="false"
        class="lease-create-view__alert"
        :title="t('leaseCreate.errors.referenceDataUnavailable')"
        type="warning"
        show-icon
        :description="setupErrorMessage"
      />

      <el-form
        ref="formRef"
        :model="form"
        :rules="rules"
        label-position="top"
        class="lease-create-view__form"
        data-testid="lease-create-form"
      >
        <section class="lease-create-view__section">
          <div class="lease-create-view__section-header">
            <h2>{{ t('leaseCreate.cards.contract') }}</h2>
            <p>{{ t('leaseCreate.cards.contractSummary') }}</p>
          </div>

          <div class="lease-create-view__grid">
            <el-form-item :label="t('leaseCreate.fields.leaseNumber')" prop="lease_no">
              <el-input
                v-model="form.lease_no"
                :placeholder="t('leaseCreate.placeholders.enterLeaseNumber')"
                data-testid="lease-number-input"
              />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.subtype')" prop="subtype">
              <el-select v-model="form.subtype" data-testid="lease-subtype-select">
                <el-option v-for="option in subtypeOptions" :key="option.value" :label="option.label" :value="option.value" />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.tenantName')" prop="tenant_name">
              <el-input
                v-model="form.tenant_name"
                :placeholder="t('leaseCreate.placeholders.enterTenantName')"
                data-testid="lease-tenant-name-input"
              />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.department')" prop="department_id">
              <el-select
                v-model="form.department_id"
                :placeholder="t('leaseCreate.placeholders.selectDepartment')"
                filterable
                :loading="isLoadingOptions"
                data-testid="lease-department-select"
              >
                <el-option
                  v-for="department in departments"
                  :key="department.id"
                  :label="`${department.code} — ${department.name}`"
                  :value="department.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.store')" prop="store_id">
              <el-select
                v-model="form.store_id"
                :placeholder="t('leaseCreate.placeholders.selectStore')"
                filterable
                :loading="isLoadingOptions"
                :disabled="form.department_id === null"
                data-testid="lease-store-select"
              >
                <el-option
                  v-for="store in availableStores"
                  :key="store.id"
                  :label="`${store.code} — ${store.name}`"
                  :value="store.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.customer')">
              <el-select
                v-model="form.customer_id"
                :placeholder="t('leaseCreate.placeholders.selectCustomer')"
                clearable
                filterable
                :loading="isLoadingOptions"
                data-testid="lease-customer-select"
              >
                <el-option
                  v-for="customer in customers"
                  :key="customer.id"
                  :label="`${customer.code} — ${customer.name}`"
                  :value="customer.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.brand')">
              <el-select
                v-model="form.brand_id"
                :placeholder="t('leaseCreate.placeholders.selectBrand')"
                clearable
                filterable
                :loading="isLoadingOptions"
                data-testid="lease-brand-select"
              >
                <el-option
                  v-for="brand in brands"
                  :key="brand.id"
                  :label="`${brand.code} — ${brand.name}`"
                  :value="brand.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.tradeId')">
              <el-input-number v-model="form.trade_id" :min="1" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.managementTypeId')">
              <el-input-number v-model="form.management_type_id" :min="1" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.startDate')" prop="start_date">
              <el-date-picker
                v-model="form.start_date"
                type="date"
                value-format="YYYY-MM-DD"
                :placeholder="t('leaseCreate.placeholders.selectStartDate')"
                data-testid="lease-start-date-input"
              />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.endDate')" prop="end_date">
              <el-date-picker
                v-model="form.end_date"
                type="date"
                value-format="YYYY-MM-DD"
                :placeholder="t('leaseCreate.placeholders.selectEndDate')"
                data-testid="lease-end-date-input"
              />
            </el-form-item>
          </div>
        </section>

        <section class="lease-create-view__section">
          <div class="lease-create-view__section-header">
            <h2>{{ t('leaseCreate.cards.billing') }}</h2>
            <p>{{ t('leaseCreate.cards.billingSummary') }}</p>
          </div>

          <div class="lease-create-view__grid">
            <el-form-item :label="t('leaseCreate.fields.unitId')" prop="unit_id">
              <el-input-number v-model="form.unit_id" :min="1" controls-position="right" data-testid="lease-unit-id-input" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.rentArea')" prop="rent_area">
              <el-input-number v-model="form.rent_area" :min="0" :precision="2" controls-position="right" data-testid="lease-rent-area-input" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.termType')" prop="term_type">
              <el-select v-model="form.term_type" :placeholder="t('leaseCreate.placeholders.selectTermType')" data-testid="lease-term-type-select">
                <el-option v-for="option in termTypeOptions" :key="option.value" :label="option.label" :value="option.value" />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.billingCycle')" prop="billing_cycle">
              <el-select v-model="form.billing_cycle" :placeholder="t('leaseCreate.placeholders.selectBillingCycle')" data-testid="lease-billing-cycle-select">
                <el-option
                  v-for="option in billingCycleOptions"
                  :key="option.value"
                  :label="option.label"
                  :value="option.value"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.currencyTypeId')" prop="currency_type_id">
              <el-input-number v-model="form.currency_type_id" :min="1" controls-position="right" data-testid="lease-currency-type-id-input" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.amount')" prop="amount">
              <el-input-number v-model="form.amount" :min="0" :precision="2" controls-position="right" data-testid="lease-amount-input" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.effectiveFrom')" prop="effective_from">
              <el-date-picker
                v-model="form.effective_from"
                type="date"
                value-format="YYYY-MM-DD"
                :placeholder="t('leaseCreate.placeholders.selectEffectiveFrom')"
                data-testid="lease-effective-from-input"
              />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.effectiveTo')" prop="effective_to">
              <el-date-picker
                v-model="form.effective_to"
                type="date"
                value-format="YYYY-MM-DD"
                :placeholder="t('leaseCreate.placeholders.selectEffectiveTo')"
                data-testid="lease-effective-to-input"
              />
            </el-form-item>
          </div>
        </section>

        <section class="lease-create-view__section lease-create-view__section--subtype" data-testid="lease-subtype-section">
          <div class="lease-create-view__section-header">
            <h2>{{ t('leaseCreate.cards.subtype') }}</h2>
            <p>{{ t('leaseCreate.cards.subtypeSummary') }}</p>
          </div>

          <div v-if="isJointOperationSubtype" class="lease-create-view__grid">
            <el-form-item :label="t('leaseCreate.fields.jointOperationBillCycle')">
              <el-input-number v-model="form.joint_operation.bill_cycle" :min="1" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationAccountCycle')">
              <el-input-number v-model="form.joint_operation.account_cycle" :min="1" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationRentIncrement')">
              <el-input v-model="form.joint_operation.rent_inc" data-testid="lease-joint-rent-inc-input" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationTaxType')">
              <el-input-number v-model="form.joint_operation.tax_type" :min="1" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationTaxRate')">
              <el-input-number v-model="form.joint_operation.tax_rate" :min="0" :precision="4" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationSettlementCurrencyTypeId')">
              <el-input-number v-model="form.joint_operation.settlement_currency_type_id" :min="1" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationInTaxRate')">
              <el-input-number v-model="form.joint_operation.in_tax_rate" :min="0" :precision="4" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationOutTaxRate')">
              <el-input-number v-model="form.joint_operation.out_tax_rate" :min="0" :precision="4" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationMonthSettleDays')">
              <el-input-number v-model="form.joint_operation.month_settle_days" :min="0" :precision="2" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationLatePayInterestRate')">
              <el-input-number v-model="form.joint_operation.late_pay_interest_rate" :min="0" :precision="4" controls-position="right" />
            </el-form-item>

            <el-form-item :label="t('leaseCreate.fields.jointOperationInterestGraceDays')">
              <el-input-number v-model="form.joint_operation.interest_grace_days" :min="0" controls-position="right" />
            </el-form-item>
          </div>

          <div v-else-if="isAdBoardSubtype" class="lease-create-view__rows">
            <div class="lease-create-view__row-toolbar">
              <el-tag effect="plain" type="info">{{ t('leaseCreate.meta.rowCount', { count: form.ad_boards.length }) }}</el-tag>
              <el-button type="primary" plain data-testid="lease-ad-board-add-button" @click="addAdBoardRow">
                {{ t('leaseCreate.actions.addAdBoardRow') }}
              </el-button>
            </div>

            <article v-for="(detail, index) in form.ad_boards" :key="index" class="lease-create-view__row-card">
              <div class="lease-create-view__row-card-header">
                <span>{{ t('leaseCreate.cards.adBoardRow', { index: index + 1 }) }}</span>
                <el-button link type="danger" @click="removeAdBoardRow(index)">
                  {{ t('leaseCreate.actions.removeRow') }}
                </el-button>
              </div>

              <div class="lease-create-view__grid">
                <el-form-item :label="t('leaseCreate.fields.adBoardId')">
                  <el-input-number v-model="detail.ad_board_id" :min="1" controls-position="right" :data-testid="`lease-ad-board-id-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardStatus')">
                  <el-input-number v-model="detail.status" :min="0" controls-position="right" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardDescription')">
                  <el-input v-model="detail.description" :data-testid="`lease-ad-board-description-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardFrequency')">
                  <el-select v-model="detail.frequency" :data-testid="`lease-ad-board-frequency-select-${index}`">
                    <el-option
                      v-for="option in adBoardFrequencyOptions"
                      :key="option.value"
                      :label="option.label"
                      :value="option.value"
                    />
                  </el-select>
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.startDate')">
                  <el-date-picker v-model="detail.start_date" type="date" value-format="YYYY-MM-DD" :data-testid="`lease-ad-board-start-date-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.endDate')">
                  <el-date-picker v-model="detail.end_date" type="date" value-format="YYYY-MM-DD" :data-testid="`lease-ad-board-end-date-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.rentArea')">
                  <el-input-number v-model="detail.rent_area" :min="0" :precision="2" controls-position="right" :data-testid="`lease-ad-board-rent-area-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardAirtime')">
                  <el-input-number v-model="detail.airtime" :min="0" controls-position="right" :data-testid="`lease-ad-board-airtime-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardFrequencyDays')">
                  <el-input-number v-model="detail.frequency_days" :min="0" controls-position="right" :data-testid="`lease-ad-board-frequency-days-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardBetweenFrom')">
                  <el-input-number v-model="detail.between_from" :min="0" controls-position="right" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.adBoardBetweenTo')">
                  <el-input-number v-model="detail.between_to" :min="0" controls-position="right" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.store')">
                  <el-select v-model="detail.store_id" clearable filterable>
                    <el-option
                      v-for="store in availableStores"
                      :key="store.id"
                      :label="`${store.code} — ${store.name}`"
                      :value="store.id"
                    />
                  </el-select>
                </el-form-item>
              </div>

              <div v-if="detail.frequency === 'W'" class="lease-create-view__weekday-grid">
                <el-checkbox v-for="weekday in weekdayOptions" :key="weekday.key" v-model="detail[weekday.key as keyof AdBoardForm]">
                  {{ weekday.label }}
                </el-checkbox>
              </div>
            </article>
          </div>

          <div v-else-if="isAreaGroundSubtype" class="lease-create-view__rows">
            <div class="lease-create-view__row-toolbar">
              <el-tag effect="plain" type="info">{{ t('leaseCreate.meta.rowCount', { count: form.area_grounds.length }) }}</el-tag>
              <el-button type="primary" plain data-testid="lease-area-ground-add-button" @click="addAreaGroundRow">
                {{ t('leaseCreate.actions.addAreaGroundRow') }}
              </el-button>
            </div>

            <article v-for="(detail, index) in form.area_grounds" :key="index" class="lease-create-view__row-card">
              <div class="lease-create-view__row-card-header">
                <span>{{ t('leaseCreate.cards.areaGroundRow', { index: index + 1 }) }}</span>
                <el-button link type="danger" @click="removeAreaGroundRow(index)">
                  {{ t('leaseCreate.actions.removeRow') }}
                </el-button>
              </div>

              <div class="lease-create-view__grid">
                <el-form-item :label="t('leaseCreate.fields.areaGroundCode')">
                  <el-input v-model="detail.code" :data-testid="`lease-area-ground-code-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.areaGroundName')">
                  <el-input v-model="detail.name" :data-testid="`lease-area-ground-name-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.areaGroundTypeId')">
                  <el-input-number v-model="detail.type_id" :min="1" controls-position="right" :data-testid="`lease-area-ground-type-id-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.areaGroundStatus')">
                  <el-input-number v-model="detail.status" :min="0" controls-position="right" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.areaGroundDescription')">
                  <el-input v-model="detail.description" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.rentArea')">
                  <el-input-number v-model="detail.rent_area" :min="0" :precision="2" controls-position="right" :data-testid="`lease-area-ground-rent-area-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.startDate')">
                  <el-date-picker v-model="detail.start_date" type="date" value-format="YYYY-MM-DD" :data-testid="`lease-area-ground-start-date-input-${index}`" />
                </el-form-item>

                <el-form-item :label="t('leaseCreate.fields.endDate')">
                  <el-date-picker v-model="detail.end_date" type="date" value-format="YYYY-MM-DD" :data-testid="`lease-area-ground-end-date-input-${index}`" />
                </el-form-item>
              </div>
            </article>
          </div>

          <el-empty v-else :description="t('leaseCreate.emptyStates.standardSubtype')" />
        </section>

        <div class="lease-create-view__actions">
          <el-button data-testid="lease-create-cancel-button" @click="handleCancel">{{ t('leaseCreate.actions.cancel') }}</el-button>
          <el-button
            type="primary"
            :loading="isSaving"
            :disabled="isAmendmentMode && !sourceLease"
            data-testid="lease-create-submit-button"
            @click="handleSubmit"
          >
            {{ submitActionLabel }}
          </el-button>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<style scoped>
.lease-create-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.lease-create-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.lease-create-view__card-header,
.lease-create-view__section-header,
.lease-create-view__row-card-header,
.lease-create-view__row-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
}

.lease-create-view__card-header {
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.lease-create-view__alert {
  margin-bottom: var(--mi-space-4);
}

.lease-create-view__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.lease-create-view__section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-md);
  background: rgba(255, 255, 255, 0.74);
}

.lease-create-view__section--subtype {
  background: linear-gradient(135deg, rgba(29, 78, 216, 0.06), rgba(15, 118, 110, 0.08));
}

.lease-create-view__section-header {
  align-items: flex-start;
}

.lease-create-view__section-header h2,
.lease-create-view__row-card-header span {
  margin: 0;
  font-size: var(--mi-font-size-300);
  color: var(--mi-color-text);
}

.lease-create-view__section-header p {
  margin: 0;
  color: var(--mi-color-muted);
}

.lease-create-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.lease-create-view__rows {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.lease-create-view__row-card {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid color-mix(in srgb, var(--mi-color-primary) 20%, var(--mi-color-border));
  border-radius: var(--mi-radius-md);
  background: rgba(255, 255, 255, 0.82);
}

.lease-create-view__weekday-grid {
  display: grid;
  grid-template-columns: repeat(4, minmax(0, 1fr));
  gap: var(--mi-space-3);
}

.lease-create-view__actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.lease-create-view :deep(.el-input-number),
.lease-create-view :deep(.el-input),
.lease-create-view :deep(.el-date-editor),
.lease-create-view :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .lease-create-view__grid,
  .lease-create-view__weekday-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .lease-create-view__section-header,
  .lease-create-view__row-card-header,
  .lease-create-view__row-toolbar,
  .lease-create-view__actions {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
