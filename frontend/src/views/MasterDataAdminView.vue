<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  createBrand,
  createCustomer,
  createStoreRentBudget,
  createUnitProspect,
  createUnitRentBudget,
  listBrands,
  listCustomers,
  listStoreRentBudgets,
  listUnitProspects,
  listUnitRentBudgets,
  updateBrand,
  updateCustomer,
  updateStoreRentBudget,
  updateUnitProspect,
  updateUnitRentBudget,
  type Brand,
  type Customer,
  type StoreRentBudget,
  type UnitProspect,
  type UnitRentBudget,
} from '../api/masterdata'
import { listDepartments, type Department } from '../api/org'
import PageSection from '../components/platform/PageSection.vue'
import { useAppStore } from '../stores/app'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type CustomerForm = {
  id: number | null
  code: string
  name: string
  trade_id: number | undefined
  department_id: number | undefined
  status: 'active' | 'inactive'
}

type BrandForm = {
  id: number | null
  code: string
  name: string
  status: 'active' | 'inactive'
}

type UnitBudgetForm = {
  unit_id: number | undefined
  fiscal_year: number | undefined
  budget_price: number | undefined
}

type StoreBudgetForm = {
  store_id: number | undefined
  fiscal_year: number | undefined
  fiscal_month: number | undefined
  monthly_budget: number | undefined
}

type UnitProspectForm = {
  unit_id: number | undefined
  fiscal_year: number | undefined
  potential_customer_id: number | undefined
  prospect_brand_id: number | undefined
  prospect_trade_id: number | undefined
  avg_transaction: number | undefined
  prospect_rent_price: number | undefined
  rent_increment: string
  prospect_term_months: number | undefined
}

const { t } = useI18n()
const appStore = useAppStore()

const customers = ref<Customer[]>([])
const customerTotal = ref(0)
const customerPage = ref(1)
const customerPageSize = 10
const customerQuery = ref('')

const brands = ref<Brand[]>([])
const brandTotal = ref(0)
const brandPage = ref(1)
const brandPageSize = 10
const brandQuery = ref('')

const unitRentBudgets = ref<UnitRentBudget[]>([])
const storeRentBudgets = ref<StoreRentBudget[]>([])
const unitProspects = ref<UnitProspect[]>([])
const departments = ref<Department[]>([])

const isLoading = ref(false)
const isCustomerSaving = ref(false)
const isBrandSaving = ref(false)
const isUnitBudgetSaving = ref(false)
const isStoreBudgetSaving = ref(false)
const isUnitProspectSaving = ref(false)

const pageFeedback = ref<Feedback | null>(null)
const customerFeedback = ref<Feedback | null>(null)
const brandFeedback = ref<Feedback | null>(null)
const budgetFeedback = ref<Feedback | null>(null)
const prospectFeedback = ref<Feedback | null>(null)

const customerForm = reactive<CustomerForm>({
  id: null,
  code: '',
  name: '',
  trade_id: undefined,
  department_id: undefined,
  status: 'active',
})

const brandForm = reactive<BrandForm>({
  id: null,
  code: '',
  name: '',
  status: 'active',
})

const unitBudgetForm = reactive<UnitBudgetForm>({
  unit_id: undefined,
  fiscal_year: undefined,
  budget_price: undefined,
})

const storeBudgetForm = reactive<StoreBudgetForm>({
  store_id: undefined,
  fiscal_year: undefined,
  fiscal_month: undefined,
  monthly_budget: undefined,
})

const unitProspectForm = reactive<UnitProspectForm>({
  unit_id: undefined,
  fiscal_year: undefined,
  potential_customer_id: undefined,
  prospect_brand_id: undefined,
  prospect_trade_id: undefined,
  avg_transaction: undefined,
  prospect_rent_price: undefined,
  rent_increment: '',
  prospect_term_months: undefined,
})

const canSaveCustomer = computed(() => Boolean(customerForm.code.trim() && customerForm.name.trim()))
const canSaveBrand = computed(() => Boolean(brandForm.code.trim() && brandForm.name.trim()))
const canSaveUnitBudget = computed(
  () => unitBudgetForm.unit_id !== undefined && unitBudgetForm.fiscal_year !== undefined && unitBudgetForm.budget_price !== undefined,
)
const canSaveStoreBudget = computed(
  () =>
    storeBudgetForm.store_id !== undefined &&
    storeBudgetForm.fiscal_year !== undefined &&
    storeBudgetForm.fiscal_month !== undefined &&
    storeBudgetForm.monthly_budget !== undefined,
)
const canSaveUnitProspect = computed(
  () => unitProspectForm.unit_id !== undefined && unitProspectForm.fiscal_year !== undefined,
)

const customerSubmitLabel = computed(() =>
  customerForm.id ? t('masterdataAdmin.actions.updateCustomer') : t('masterdataAdmin.actions.createCustomer'),
)
const brandSubmitLabel = computed(() =>
  brandForm.id ? t('masterdataAdmin.actions.updateBrand') : t('masterdataAdmin.actions.createBrand'),
)
const unitBudgetSubmitLabel = computed(() =>
  hasExistingUnitBudget(unitBudgetForm.unit_id, unitBudgetForm.fiscal_year)
    ? t('masterdataAdmin.actions.updateUnitBudget')
    : t('masterdataAdmin.actions.createUnitBudget'),
)
const storeBudgetSubmitLabel = computed(() =>
  hasExistingStoreBudget(storeBudgetForm.store_id, storeBudgetForm.fiscal_year, storeBudgetForm.fiscal_month)
    ? t('masterdataAdmin.actions.updateStoreBudget')
    : t('masterdataAdmin.actions.createStoreBudget'),
)
const unitProspectSubmitLabel = computed(() =>
  hasExistingUnitProspect(unitProspectForm.unit_id, unitProspectForm.fiscal_year)
    ? t('masterdataAdmin.actions.updateUnitProspect')
    : t('masterdataAdmin.actions.createUnitProspect'),
)

const getErrorMessage = (error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)

const formatDate = (value: string) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(appStore.locale, { dateStyle: 'medium' }).format(new Date(value))
}

const resolveDepartmentLabel = (departmentId: number | null | undefined) => {
  if (!departmentId) {
    return t('common.emptyValue')
  }

  const department = departments.value.find((item) => item.id === departmentId)
  if (!department) {
    return `#${departmentId}`
  }

  return `${department.code} — ${department.name}`
}

const resolveStatusLabel = (status: string) => {
  switch (status) {
    case 'active':
      return t('common.statuses.active')
    case 'inactive':
      return t('common.statuses.inactive')
    default:
      return status || t('common.emptyValue')
  }
}

const resetCustomerForm = () => {
  customerForm.id = null
  customerForm.code = ''
  customerForm.name = ''
  customerForm.trade_id = undefined
  customerForm.department_id = undefined
  customerForm.status = 'active'
}

const resetBrandForm = () => {
  brandForm.id = null
  brandForm.code = ''
  brandForm.name = ''
  brandForm.status = 'active'
}

const resetUnitBudgetForm = () => {
  unitBudgetForm.unit_id = undefined
  unitBudgetForm.fiscal_year = undefined
  unitBudgetForm.budget_price = undefined
}

const resetStoreBudgetForm = () => {
  storeBudgetForm.store_id = undefined
  storeBudgetForm.fiscal_year = undefined
  storeBudgetForm.fiscal_month = undefined
  storeBudgetForm.monthly_budget = undefined
}

const resetUnitProspectForm = () => {
  unitProspectForm.unit_id = undefined
  unitProspectForm.fiscal_year = undefined
  unitProspectForm.potential_customer_id = undefined
  unitProspectForm.prospect_brand_id = undefined
  unitProspectForm.prospect_trade_id = undefined
  unitProspectForm.avg_transaction = undefined
  unitProspectForm.prospect_rent_price = undefined
  unitProspectForm.rent_increment = ''
  unitProspectForm.prospect_term_months = undefined
}

const hasExistingUnitBudget = (unitId?: number, fiscalYear?: number) =>
  unitRentBudgets.value.some((item) => item.unit_id === unitId && item.fiscal_year === fiscalYear)

const hasExistingStoreBudget = (storeId?: number, fiscalYear?: number, fiscalMonth?: number) =>
  storeRentBudgets.value.some(
    (item) => item.store_id === storeId && item.fiscal_year === fiscalYear && item.fiscal_month === fiscalMonth,
  )

const hasExistingUnitProspect = (unitId?: number, fiscalYear?: number) =>
  unitProspects.value.some((item) => item.unit_id === unitId && item.fiscal_year === fiscalYear)

const loadCustomers = async () => {
  const response = await listCustomers({
    query: customerQuery.value.trim() || undefined,
    page: customerPage.value,
    page_size: customerPageSize,
  })

  customers.value = response.data.customers ?? []
  customerTotal.value = response.data.total ?? customers.value.length
}

const loadBrands = async () => {
  const response = await listBrands({
    query: brandQuery.value.trim() || undefined,
    page: brandPage.value,
    page_size: brandPageSize,
  })

  brands.value = response.data.brands ?? []
  brandTotal.value = response.data.total ?? brands.value.length
}

const loadBudgetProspects = async () => {
  const [unitBudgetsResult, storeBudgetsResult, unitProspectsResult] = await Promise.allSettled([
    listUnitRentBudgets(),
    listStoreRentBudgets(),
    listUnitProspects(),
  ])

  const loadErrors: string[] = []

  if (unitBudgetsResult.status === 'fulfilled') {
    unitRentBudgets.value = unitBudgetsResult.value.data.unit_rent_budgets ?? []
  } else {
    unitRentBudgets.value = []
    loadErrors.push(getErrorMessage(unitBudgetsResult.reason, t('masterdataAdmin.errors.unableToLoadUnitBudgets')))
  }

  if (storeBudgetsResult.status === 'fulfilled') {
    storeRentBudgets.value = storeBudgetsResult.value.data.store_rent_budgets ?? []
  } else {
    storeRentBudgets.value = []
    loadErrors.push(getErrorMessage(storeBudgetsResult.reason, t('masterdataAdmin.errors.unableToLoadStoreBudgets')))
  }

  if (unitProspectsResult.status === 'fulfilled') {
    unitProspects.value = unitProspectsResult.value.data.unit_prospects ?? []
  } else {
    unitProspects.value = []
    loadErrors.push(getErrorMessage(unitProspectsResult.reason, t('masterdataAdmin.errors.unableToLoadUnitProspects')))
  }

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.masterDataUnavailable'),
      description: loadErrors.join(' '),
    }
  }
}

const loadMasterData = async () => {
  isLoading.value = true
  pageFeedback.value = null

  const [customerResult, brandResult, departmentsResult] = await Promise.allSettled([
    loadCustomers(),
    loadBrands(),
    listDepartments(),
  ])

  const loadErrors: string[] = []

  if (customerResult.status === 'rejected') {
    customers.value = []
    customerTotal.value = 0
    loadErrors.push(getErrorMessage(customerResult.reason, t('masterdataAdmin.errors.unableToLoadCustomers')))
  }

  if (brandResult.status === 'rejected') {
    brands.value = []
    brandTotal.value = 0
    loadErrors.push(getErrorMessage(brandResult.reason, t('masterdataAdmin.errors.unableToLoadBrands')))
  }

  if (departmentsResult.status === 'fulfilled') {
    departments.value = departmentsResult.value.data.departments ?? []
  } else {
    departments.value = []
    loadErrors.push(getErrorMessage(departmentsResult.reason, t('masterdataAdmin.errors.unableToLoadDepartments')))
  }

  await loadBudgetProspects()

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.masterDataUnavailable'),
      description: loadErrors.join(' '),
    }
  }

  isLoading.value = false
}

const handleCustomerPageChange = async (page: number) => {
  customerPage.value = page
  await loadCustomers()
}

const handleBrandPageChange = async (page: number) => {
  brandPage.value = page
  await loadBrands()
}

const handleCustomerSearch = async () => {
  customerPage.value = 1
  await loadCustomers()
}

const handleBrandSearch = async () => {
  brandPage.value = 1
  await loadBrands()
}

const handleSaveCustomer = async () => {
  if (!canSaveCustomer.value) {
    customerFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.customerDetailsRequiredTitle'),
      description: t('masterdataAdmin.feedback.customerDetailsRequiredDescription'),
    }
    return
  }

  isCustomerSaving.value = true
  customerFeedback.value = null

  try {
    if (customerForm.id) {
      const response = await updateCustomer({
        id: customerForm.id,
        code: customerForm.code.trim(),
        name: customerForm.name.trim(),
        trade_id: customerForm.trade_id ?? null,
        department_id: customerForm.department_id ?? null,
        status: customerForm.status,
      })
      customerFeedback.value = {
        type: 'success',
        title: t('masterdataAdmin.feedback.customerUpdatedTitle'),
        description: t('masterdataAdmin.feedback.customerUpdatedDescription', { code: response.data.customer.code }),
      }
    } else {
      const response = await createCustomer({
        code: customerForm.code.trim(),
        name: customerForm.name.trim(),
        trade_id: customerForm.trade_id ?? null,
        department_id: customerForm.department_id ?? null,
        status: customerForm.status,
      })
      customerFeedback.value = {
        type: 'success',
        title: t('masterdataAdmin.feedback.customerCreatedTitle'),
        description: t('masterdataAdmin.feedback.customerCreatedDescription', { code: response.data.customer.code }),
      }
    }

    await loadCustomers()
    resetCustomerForm()
  } catch (error) {
    customerFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.customerSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveCustomer')),
    }
  } finally {
    isCustomerSaving.value = false
  }
}

const handleSaveBrand = async () => {
  if (!canSaveBrand.value) {
    brandFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.brandDetailsRequiredTitle'),
      description: t('masterdataAdmin.feedback.brandDetailsRequiredDescription'),
    }
    return
  }

  isBrandSaving.value = true
  brandFeedback.value = null

  try {
    if (brandForm.id) {
      const response = await updateBrand({
        id: brandForm.id,
        code: brandForm.code.trim(),
        name: brandForm.name.trim(),
        status: brandForm.status,
      })
      brandFeedback.value = {
        type: 'success',
        title: t('masterdataAdmin.feedback.brandUpdatedTitle'),
        description: t('masterdataAdmin.feedback.brandUpdatedDescription', { code: response.data.brand.code }),
      }
    } else {
      const response = await createBrand({
        code: brandForm.code.trim(),
        name: brandForm.name.trim(),
        status: brandForm.status,
      })
      brandFeedback.value = {
        type: 'success',
        title: t('masterdataAdmin.feedback.brandCreatedTitle'),
        description: t('masterdataAdmin.feedback.brandCreatedDescription', { code: response.data.brand.code }),
      }
    }

    await loadBrands()
    resetBrandForm()
  } catch (error) {
    brandFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.brandSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveBrand')),
    }
  } finally {
    isBrandSaving.value = false
  }
}

const handleSaveUnitBudget = async () => {
  if (!canSaveUnitBudget.value) {
    budgetFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.unitBudgetRequiredTitle'),
      description: t('masterdataAdmin.feedback.unitBudgetRequiredDescription'),
    }
    return
  }

  isUnitBudgetSaving.value = true
  budgetFeedback.value = null

  try {
    const payload = {
      unit_id: unitBudgetForm.unit_id!,
      fiscal_year: unitBudgetForm.fiscal_year!,
      budget_price: unitBudgetForm.budget_price!,
    }

    if (hasExistingUnitBudget(payload.unit_id, payload.fiscal_year)) {
      await updateUnitRentBudget(payload)
    } else {
      await createUnitRentBudget(payload)
    }

    budgetFeedback.value = {
      type: 'success',
      title: t('masterdataAdmin.feedback.unitBudgetSavedTitle'),
      description: t('masterdataAdmin.feedback.unitBudgetSavedDescription', {
        unitId: payload.unit_id,
        fiscalYear: payload.fiscal_year,
      }),
    }
    await loadBudgetProspects()
    resetUnitBudgetForm()
  } catch (error) {
    budgetFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.unitBudgetSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveUnitBudget')),
    }
  } finally {
    isUnitBudgetSaving.value = false
  }
}

const handleSaveStoreBudget = async () => {
  if (!canSaveStoreBudget.value) {
    budgetFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.storeBudgetRequiredTitle'),
      description: t('masterdataAdmin.feedback.storeBudgetRequiredDescription'),
    }
    return
  }

  isStoreBudgetSaving.value = true
  budgetFeedback.value = null

  try {
    const payload = {
      store_id: storeBudgetForm.store_id!,
      fiscal_year: storeBudgetForm.fiscal_year!,
      fiscal_month: storeBudgetForm.fiscal_month!,
      monthly_budget: storeBudgetForm.monthly_budget!,
    }

    if (hasExistingStoreBudget(payload.store_id, payload.fiscal_year, payload.fiscal_month)) {
      await updateStoreRentBudget(payload)
    } else {
      await createStoreRentBudget(payload)
    }

    budgetFeedback.value = {
      type: 'success',
      title: t('masterdataAdmin.feedback.storeBudgetSavedTitle'),
      description: t('masterdataAdmin.feedback.storeBudgetSavedDescription', {
        storeId: payload.store_id,
        fiscalYear: payload.fiscal_year,
        fiscalMonth: payload.fiscal_month,
      }),
    }
    await loadBudgetProspects()
    resetStoreBudgetForm()
  } catch (error) {
    budgetFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.storeBudgetSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveStoreBudget')),
    }
  } finally {
    isStoreBudgetSaving.value = false
  }
}

const handleSaveUnitProspect = async () => {
  if (!canSaveUnitProspect.value) {
    prospectFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.unitProspectRequiredTitle'),
      description: t('masterdataAdmin.feedback.unitProspectRequiredDescription'),
    }
    return
  }

  isUnitProspectSaving.value = true
  prospectFeedback.value = null

  try {
    const payload = {
      unit_id: unitProspectForm.unit_id!,
      fiscal_year: unitProspectForm.fiscal_year!,
      potential_customer_id: unitProspectForm.potential_customer_id ?? null,
      prospect_brand_id: unitProspectForm.prospect_brand_id ?? null,
      prospect_trade_id: unitProspectForm.prospect_trade_id ?? null,
      avg_transaction: unitProspectForm.avg_transaction ?? null,
      prospect_rent_price: unitProspectForm.prospect_rent_price ?? null,
      rent_increment: unitProspectForm.rent_increment.trim() || null,
      prospect_term_months: unitProspectForm.prospect_term_months ?? null,
    }

    if (hasExistingUnitProspect(payload.unit_id, payload.fiscal_year)) {
      await updateUnitProspect(payload)
    } else {
      await createUnitProspect(payload)
    }

    prospectFeedback.value = {
      type: 'success',
      title: t('masterdataAdmin.feedback.unitProspectSavedTitle'),
      description: t('masterdataAdmin.feedback.unitProspectSavedDescription', {
        unitId: payload.unit_id,
        fiscalYear: payload.fiscal_year,
      }),
    }
    await loadBudgetProspects()
    resetUnitProspectForm()
  } catch (error) {
    prospectFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.unitProspectSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveUnitProspect')),
    }
  } finally {
    isUnitProspectSaving.value = false
  }
}

const editCustomer = (customer: Customer) => {
  customerForm.id = customer.id
  customerForm.code = customer.code
  customerForm.name = customer.name
  customerForm.trade_id = customer.trade_id ?? undefined
  customerForm.department_id = customer.department_id ?? undefined
  customerForm.status = customer.status === 'inactive' ? 'inactive' : 'active'
}

const editBrand = (brand: Brand) => {
  brandForm.id = brand.id
  brandForm.code = brand.code
  brandForm.name = brand.name
  brandForm.status = brand.status === 'inactive' ? 'inactive' : 'active'
}

const editUnitBudget = (item: UnitRentBudget) => {
  unitBudgetForm.unit_id = item.unit_id
  unitBudgetForm.fiscal_year = item.fiscal_year
  unitBudgetForm.budget_price = item.budget_price
}

const editStoreBudget = (item: StoreRentBudget) => {
  storeBudgetForm.store_id = item.store_id
  storeBudgetForm.fiscal_year = item.fiscal_year
  storeBudgetForm.fiscal_month = item.fiscal_month
  storeBudgetForm.monthly_budget = item.monthly_budget
}

const editUnitProspect = (item: UnitProspect) => {
  unitProspectForm.unit_id = item.unit_id
  unitProspectForm.fiscal_year = item.fiscal_year
  unitProspectForm.potential_customer_id = item.potential_customer_id ?? undefined
  unitProspectForm.prospect_brand_id = item.prospect_brand_id ?? undefined
  unitProspectForm.prospect_trade_id = item.prospect_trade_id ?? undefined
  unitProspectForm.avg_transaction = item.avg_transaction ?? undefined
  unitProspectForm.prospect_rent_price = item.prospect_rent_price ?? undefined
  unitProspectForm.rent_increment = item.rent_increment ?? ''
  unitProspectForm.prospect_term_months = item.prospect_term_months ?? undefined
}

onMounted(() => {
  void loadMasterData()
})
</script>

<template>
  <div class="masterdata-admin-view" data-testid="masterdata-admin-view">
    <PageSection
      :eyebrow="t('masterdataAdmin.eyebrow')"
      :title="t('masterdataAdmin.title')"
      :summary="t('masterdataAdmin.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('masterdataAdmin.tags.customers', { count: customerTotal }) }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('masterdataAdmin.tags.brands', { count: brandTotal }) }}</el-tag>
        <el-tag effect="plain" type="warning">{{ t('masterdataAdmin.tags.unitBudgets', { count: unitRentBudgets.length }) }}</el-tag>
        <el-tag effect="plain" type="danger">{{ t('masterdataAdmin.tags.unitProspects', { count: unitProspects.length }) }}</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="pageFeedback"
      :closable="false"
      :title="pageFeedback.title"
      :type="pageFeedback.type"
      :description="pageFeedback.description"
      show-icon
    />

    <div class="masterdata-admin-view__forms-grid">
      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>{{ t('masterdataAdmin.cards.customerMaintenance') }}</span>
          </div>
        </template>

        <el-alert v-if="customerFeedback" :closable="false" class="masterdata-admin-view__feedback" :title="customerFeedback.title" :type="customerFeedback.type" :description="customerFeedback.description" show-icon />

        <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
          <div class="masterdata-admin-view__form-grid">
            <el-form-item :label="t('masterdataAdmin.fields.customerCode')">
              <el-input v-model="customerForm.code" data-testid="customer-code-input" :placeholder="t('masterdataAdmin.placeholders.enterCustomerCode')" />
            </el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.customerName')">
              <el-input v-model="customerForm.name" data-testid="customer-name-input" :placeholder="t('masterdataAdmin.placeholders.enterCustomerName')" />
            </el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.tradeId')">
              <el-input-number v-model="customerForm.trade_id" :min="1" controls-position="right" data-testid="customer-trade-input" />
            </el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.department')">
              <el-select v-model="customerForm.department_id" clearable filterable data-testid="customer-department-input" :placeholder="t('masterdataAdmin.placeholders.selectDepartment')">
                <el-option v-for="department in departments" :key="department.id" :label="`${department.code} — ${department.name}`" :value="department.id" />
              </el-select>
            </el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.status')">
              <el-select v-model="customerForm.status" data-testid="customer-status-input">
                <el-option value="active" :label="t('common.statuses.active')" />
                <el-option value="inactive" :label="t('common.statuses.inactive')" />
              </el-select>
            </el-form-item>
          </div>

          <div class="masterdata-admin-view__form-actions">
            <el-button type="primary" :loading="isCustomerSaving" :disabled="!canSaveCustomer" data-testid="customer-create-button" @click="handleSaveCustomer">
              {{ customerSubmitLabel }}
            </el-button>
            <el-button v-if="customerForm.id" data-testid="customer-cancel-button" @click="resetCustomerForm">
              {{ t('common.actions.cancel') }}
            </el-button>
          </div>
        </el-form>
      </el-card>

      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>{{ t('masterdataAdmin.cards.brandMaintenance') }}</span>
          </div>
        </template>

        <el-alert v-if="brandFeedback" :closable="false" class="masterdata-admin-view__feedback" :title="brandFeedback.title" :type="brandFeedback.type" :description="brandFeedback.description" show-icon />

        <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
          <div class="masterdata-admin-view__form-grid">
            <el-form-item :label="t('masterdataAdmin.fields.brandCode')">
              <el-input v-model="brandForm.code" data-testid="brand-code-input" :placeholder="t('masterdataAdmin.placeholders.enterBrandCode')" />
            </el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.brandName')">
              <el-input v-model="brandForm.name" data-testid="brand-name-input" :placeholder="t('masterdataAdmin.placeholders.enterBrandName')" />
            </el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.status')">
              <el-select v-model="brandForm.status" data-testid="brand-status-input">
                <el-option value="active" :label="t('common.statuses.active')" />
                <el-option value="inactive" :label="t('common.statuses.inactive')" />
              </el-select>
            </el-form-item>
          </div>

          <div class="masterdata-admin-view__form-actions">
            <el-button type="primary" :loading="isBrandSaving" :disabled="!canSaveBrand" data-testid="brand-create-button" @click="handleSaveBrand">
              {{ brandSubmitLabel }}
            </el-button>
            <el-button v-if="brandForm.id" data-testid="brand-cancel-button" @click="resetBrandForm">
              {{ t('common.actions.cancel') }}
            </el-button>
          </div>
        </el-form>
      </el-card>
    </div>

    <div class="masterdata-admin-view__tables-grid">
      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>{{ t('masterdataAdmin.cards.customers') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: customerTotal }) }}</el-tag>
          </div>
        </template>

        <div class="masterdata-admin-view__toolbar">
          <el-input v-model="customerQuery" data-testid="customers-query-input" :placeholder="t('masterdataAdmin.placeholders.searchCustomers')" clearable @keyup.enter="handleCustomerSearch" />
          <el-button data-testid="customers-query-button" @click="handleCustomerSearch">{{ t('common.actions.query') }}</el-button>
        </div>

        <el-table :data="customers" row-key="id" class="masterdata-admin-view__table" :empty-text="isLoading ? t('masterdataAdmin.table.loadingCustomers') : t('masterdataAdmin.table.emptyCustomers')" data-testid="customers-table">
          <el-table-column prop="code" :label="t('masterdataAdmin.columns.code')" min-width="140" />
          <el-table-column prop="name" :label="t('masterdataAdmin.columns.name')" min-width="220" />
          <el-table-column :label="t('masterdataAdmin.columns.tradeId')" min-width="110">
            <template #default="scope">{{ scope.row.trade_id ?? t('common.emptyValue') }}</template>
          </el-table-column>
          <el-table-column :label="t('masterdataAdmin.columns.department')" min-width="220">
            <template #default="scope">{{ resolveDepartmentLabel(scope.row.department_id) }}</template>
          </el-table-column>
          <el-table-column prop="status" :label="t('common.columns.status')" min-width="120">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">{{ resolveStatusLabel(scope.row.status) }}</el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.createdAt')" min-width="160">
            <template #default="scope">{{ formatDate(scope.row.created_at) }}</template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120">
            <template #default="scope">
              <el-button link type="primary" data-testid="customer-edit-button" @click="editCustomer(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>

        <el-pagination background layout="prev, pager, next" :total="customerTotal" :page-size="customerPageSize" :current-page="customerPage" @current-change="handleCustomerPageChange" />
      </el-card>

      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>{{ t('masterdataAdmin.cards.brands') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: brandTotal }) }}</el-tag>
          </div>
        </template>

        <div class="masterdata-admin-view__toolbar">
          <el-input v-model="brandQuery" data-testid="brands-query-input" :placeholder="t('masterdataAdmin.placeholders.searchBrands')" clearable @keyup.enter="handleBrandSearch" />
          <el-button data-testid="brands-query-button" @click="handleBrandSearch">{{ t('common.actions.query') }}</el-button>
        </div>

        <el-table :data="brands" row-key="id" class="masterdata-admin-view__table" :empty-text="isLoading ? t('masterdataAdmin.table.loadingBrands') : t('masterdataAdmin.table.emptyBrands')" data-testid="brands-table">
          <el-table-column prop="code" :label="t('masterdataAdmin.columns.code')" min-width="140" />
          <el-table-column prop="name" :label="t('masterdataAdmin.columns.name')" min-width="220" />
          <el-table-column prop="status" :label="t('common.columns.status')" min-width="120">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">{{ resolveStatusLabel(scope.row.status) }}</el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.createdAt')" min-width="160">
            <template #default="scope">{{ formatDate(scope.row.created_at) }}</template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120">
            <template #default="scope">
              <el-button link type="primary" data-testid="brand-edit-button" @click="editBrand(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>

        <el-pagination background layout="prev, pager, next" :total="brandTotal" :page-size="brandPageSize" :current-page="brandPage" @current-change="handleBrandPageChange" />
      </el-card>
    </div>

    <div class="masterdata-admin-view__budget-grid">
      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header><div class="masterdata-admin-view__card-header"><span>{{ t('masterdataAdmin.cards.unitBudgets') }}</span></div></template>
        <el-alert v-if="budgetFeedback" :closable="false" class="masterdata-admin-view__feedback" :title="budgetFeedback.title" :type="budgetFeedback.type" :description="budgetFeedback.description" show-icon />
        <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
          <div class="masterdata-admin-view__form-grid">
            <el-form-item :label="t('masterdataAdmin.fields.unitId')"><el-input-number v-model="unitBudgetForm.unit_id" :min="1" controls-position="right" data-testid="unit-budget-unit-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.fiscalYear')"><el-input-number v-model="unitBudgetForm.fiscal_year" :min="2000" controls-position="right" data-testid="unit-budget-year-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.budgetPrice')"><el-input-number v-model="unitBudgetForm.budget_price" :min="0" :precision="2" controls-position="right" data-testid="unit-budget-price-input" /></el-form-item>
          </div>
          <div class="masterdata-admin-view__form-actions">
            <el-button type="primary" :loading="isUnitBudgetSaving" :disabled="!canSaveUnitBudget" data-testid="unit-budget-save-button" @click="handleSaveUnitBudget">{{ unitBudgetSubmitLabel }}</el-button>
            <el-button @click="resetUnitBudgetForm">{{ t('common.actions.reset') }}</el-button>
          </div>
        </el-form>
        <el-table :data="unitRentBudgets" :row-key="(row: UnitRentBudget) => `${row.unit_id}-${row.fiscal_year}`" class="masterdata-admin-view__table" data-testid="unit-budgets-table">
          <el-table-column prop="unit_id" :label="t('masterdataAdmin.columns.unitId')" min-width="110" />
          <el-table-column prop="fiscal_year" :label="t('masterdataAdmin.columns.fiscalYear')" min-width="120" />
          <el-table-column prop="budget_price" :label="t('masterdataAdmin.columns.budgetPrice')" min-width="140" />
          <el-table-column :label="t('common.columns.actions')" min-width="120"><template #default="scope"><el-button link type="primary" data-testid="unit-budget-edit-button" @click="editUnitBudget(scope.row)">{{ t('common.actions.edit') }}</el-button></template></el-table-column>
        </el-table>
      </el-card>

      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header><div class="masterdata-admin-view__card-header"><span>{{ t('masterdataAdmin.cards.storeBudgets') }}</span></div></template>
        <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
          <div class="masterdata-admin-view__form-grid">
            <el-form-item :label="t('masterdataAdmin.fields.storeId')"><el-input-number v-model="storeBudgetForm.store_id" :min="1" controls-position="right" data-testid="store-budget-store-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.fiscalYear')"><el-input-number v-model="storeBudgetForm.fiscal_year" :min="2000" controls-position="right" data-testid="store-budget-year-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.fiscalMonth')"><el-input-number v-model="storeBudgetForm.fiscal_month" :min="1" :max="12" controls-position="right" data-testid="store-budget-month-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.monthlyBudget')"><el-input-number v-model="storeBudgetForm.monthly_budget" :min="0" :precision="2" controls-position="right" data-testid="store-budget-value-input" /></el-form-item>
          </div>
          <div class="masterdata-admin-view__form-actions">
            <el-button type="primary" :loading="isStoreBudgetSaving" :disabled="!canSaveStoreBudget" data-testid="store-budget-save-button" @click="handleSaveStoreBudget">{{ storeBudgetSubmitLabel }}</el-button>
            <el-button @click="resetStoreBudgetForm">{{ t('common.actions.reset') }}</el-button>
          </div>
        </el-form>
        <el-table :data="storeRentBudgets" :row-key="(row: StoreRentBudget) => `${row.store_id}-${row.fiscal_year}-${row.fiscal_month}`" class="masterdata-admin-view__table" data-testid="store-budgets-table">
          <el-table-column prop="store_id" :label="t('masterdataAdmin.columns.storeId')" min-width="110" />
          <el-table-column prop="fiscal_year" :label="t('masterdataAdmin.columns.fiscalYear')" min-width="120" />
          <el-table-column prop="fiscal_month" :label="t('masterdataAdmin.columns.fiscalMonth')" min-width="120" />
          <el-table-column prop="monthly_budget" :label="t('masterdataAdmin.columns.monthlyBudget')" min-width="150" />
          <el-table-column :label="t('common.columns.actions')" min-width="120"><template #default="scope"><el-button link type="primary" data-testid="store-budget-edit-button" @click="editStoreBudget(scope.row)">{{ t('common.actions.edit') }}</el-button></template></el-table-column>
        </el-table>
      </el-card>
    </div>

    <el-card class="masterdata-admin-view__card" shadow="never">
      <template #header><div class="masterdata-admin-view__card-header"><span>{{ t('masterdataAdmin.cards.unitProspects') }}</span></div></template>
      <el-alert v-if="prospectFeedback" :closable="false" class="masterdata-admin-view__feedback" :title="prospectFeedback.title" :type="prospectFeedback.type" :description="prospectFeedback.description" show-icon />
      <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
        <div class="masterdata-admin-view__form-grid masterdata-admin-view__form-grid--wide">
          <el-form-item :label="t('masterdataAdmin.fields.unitId')"><el-input-number v-model="unitProspectForm.unit_id" :min="1" controls-position="right" data-testid="unit-prospect-unit-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.fiscalYear')"><el-input-number v-model="unitProspectForm.fiscal_year" :min="2000" controls-position="right" data-testid="unit-prospect-year-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.potentialCustomer')"><el-select v-model="unitProspectForm.potential_customer_id" clearable filterable data-testid="unit-prospect-customer-input"><el-option v-for="customer in customers" :key="customer.id" :label="`${customer.code} — ${customer.name}`" :value="customer.id" /></el-select></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectBrand')"><el-select v-model="unitProspectForm.prospect_brand_id" clearable filterable data-testid="unit-prospect-brand-input"><el-option v-for="brand in brands" :key="brand.id" :label="`${brand.code} — ${brand.name}`" :value="brand.id" /></el-select></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectTradeId')"><el-input-number v-model="unitProspectForm.prospect_trade_id" :min="1" controls-position="right" data-testid="unit-prospect-trade-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.avgTransaction')"><el-input-number v-model="unitProspectForm.avg_transaction" :min="0" :precision="2" controls-position="right" data-testid="unit-prospect-avg-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectRentPrice')"><el-input-number v-model="unitProspectForm.prospect_rent_price" :min="0" :precision="2" controls-position="right" data-testid="unit-prospect-price-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.rentIncrement')"><el-input v-model="unitProspectForm.rent_increment" data-testid="unit-prospect-increment-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectTermMonths')"><el-input-number v-model="unitProspectForm.prospect_term_months" :min="1" controls-position="right" data-testid="unit-prospect-term-input" /></el-form-item>
        </div>
        <div class="masterdata-admin-view__form-actions">
          <el-button type="primary" :loading="isUnitProspectSaving" :disabled="!canSaveUnitProspect" data-testid="unit-prospect-save-button" @click="handleSaveUnitProspect">{{ unitProspectSubmitLabel }}</el-button>
          <el-button @click="resetUnitProspectForm">{{ t('common.actions.reset') }}</el-button>
        </div>
      </el-form>

      <el-table :data="unitProspects" :row-key="(row: UnitProspect) => `${row.unit_id}-${row.fiscal_year}`" class="masterdata-admin-view__table" data-testid="unit-prospects-table">
        <el-table-column prop="unit_id" :label="t('masterdataAdmin.columns.unitId')" min-width="110" />
        <el-table-column prop="fiscal_year" :label="t('masterdataAdmin.columns.fiscalYear')" min-width="120" />
        <el-table-column :label="t('masterdataAdmin.columns.potentialCustomer')" min-width="180"><template #default="scope">{{ scope.row.potential_customer_id ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('masterdataAdmin.columns.prospectBrand')" min-width="160"><template #default="scope">{{ scope.row.prospect_brand_id ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('masterdataAdmin.columns.prospectRentPrice')" min-width="160"><template #default="scope">{{ scope.row.prospect_rent_price ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('masterdataAdmin.columns.prospectTermMonths')" min-width="160"><template #default="scope">{{ scope.row.prospect_term_months ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="120"><template #default="scope"><el-button link type="primary" data-testid="unit-prospect-edit-button" @click="editUnitProspect(scope.row)">{{ t('common.actions.edit') }}</el-button></template></el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.masterdata-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.masterdata-admin-view__forms-grid,
.masterdata-admin-view__tables-grid,
.masterdata-admin-view__budget-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.masterdata-admin-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.masterdata-admin-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.masterdata-admin-view__feedback {
  margin-bottom: var(--mi-space-4);
}

.masterdata-admin-view__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.masterdata-admin-view__form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.masterdata-admin-view__form-grid--wide {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.masterdata-admin-view__form-actions,
.masterdata-admin-view__toolbar {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.masterdata-admin-view__table,
.masterdata-admin-view__toolbar :deep(.el-input),
.masterdata-admin-view__form-grid :deep(.el-input-number),
.masterdata-admin-view__form-grid :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .masterdata-admin-view__forms-grid,
  .masterdata-admin-view__tables-grid,
  .masterdata-admin-view__budget-grid,
  .masterdata-admin-view__form-grid,
  .masterdata-admin-view__form-grid--wide {
    grid-template-columns: minmax(0, 1fr);
  }

  .masterdata-admin-view__card-header,
  .masterdata-admin-view__form-actions,
  .masterdata-admin-view__toolbar {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
