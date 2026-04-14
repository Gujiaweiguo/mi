<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  createBrand,
  createCustomer,
  listBrands,
  listCustomers,
  updateBrand,
  updateCustomer,
  type Brand,
  type Customer,
} from '../../api/masterdata'
import { listDepartments, type Department } from '../../api/org'
import { useAppStore } from '../../stores/app'
import type { CustomerBrandSnapshot, Feedback } from './types'

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

const emit = defineEmits<{
  'summary-change': [snapshot: CustomerBrandSnapshot]
  'load-feedback-change': [feedback: Feedback | null]
}>()

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

const departments = ref<Department[]>([])

const isLoading = ref(false)
const isCustomerSaving = ref(false)
const isBrandSaving = ref(false)

const loadFeedback = ref<Feedback | null>(null)
const customerFeedback = ref<Feedback | null>(null)
const brandFeedback = ref<Feedback | null>(null)

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

const canSaveCustomer = computed(() => Boolean(customerForm.code.trim() && customerForm.name.trim()))
const canSaveBrand = computed(() => Boolean(brandForm.code.trim() && brandForm.name.trim()))

const customerSubmitLabel = computed(() =>
  customerForm.id ? t('masterdataAdmin.actions.updateCustomer') : t('masterdataAdmin.actions.createCustomer'),
)
const brandSubmitLabel = computed(() =>
  brandForm.id ? t('masterdataAdmin.actions.updateBrand') : t('masterdataAdmin.actions.createBrand'),
)

const emitSnapshot = () => {
  emit('summary-change', {
    customers: customers.value,
    customerTotal: customerTotal.value,
    brands: brands.value,
    brandTotal: brandTotal.value,
  })
}

watch([customers, customerTotal, brands, brandTotal], emitSnapshot, { immediate: true })
watch(loadFeedback, (value) => emit('load-feedback-change', value), { immediate: true })

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

const loadSection = async () => {
  isLoading.value = true
  loadFeedback.value = null

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

  if (loadErrors.length > 0) {
    loadFeedback.value = {
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

onMounted(() => {
  void loadSection()
})
</script>

<template>
  <div class="masterdata-customer-brand-section">
    <div class="masterdata-customer-brand-section__forms-grid">
      <el-card class="masterdata-customer-brand-section__card" shadow="never">
        <template #header>
          <div class="masterdata-customer-brand-section__card-header">
            <span>{{ t('masterdataAdmin.cards.createCustomer') }}</span>
          </div>
        </template>

        <el-alert
          v-if="customerFeedback"
          :closable="false"
          class="masterdata-customer-brand-section__feedback"
          :title="customerFeedback.title"
          :type="customerFeedback.type"
          :description="customerFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="masterdata-customer-brand-section__form" @submit.prevent>
          <div class="masterdata-customer-brand-section__form-grid">
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

          <div class="masterdata-customer-brand-section__form-actions">
            <el-button type="primary" :loading="isCustomerSaving" :disabled="!canSaveCustomer" data-testid="customer-create-button" @click="handleSaveCustomer">
              {{ customerSubmitLabel }}
            </el-button>
            <el-button v-if="customerForm.id" data-testid="customer-cancel-button" @click="resetCustomerForm">
              {{ t('common.actions.cancel') }}
            </el-button>
          </div>
        </el-form>
      </el-card>

      <el-card class="masterdata-customer-brand-section__card" shadow="never">
        <template #header>
          <div class="masterdata-customer-brand-section__card-header">
            <span>{{ t('masterdataAdmin.cards.createBrand') }}</span>
          </div>
        </template>

        <el-alert
          v-if="brandFeedback"
          :closable="false"
          class="masterdata-customer-brand-section__feedback"
          :title="brandFeedback.title"
          :type="brandFeedback.type"
          :description="brandFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="masterdata-customer-brand-section__form" @submit.prevent>
          <div class="masterdata-customer-brand-section__form-grid">
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

          <div class="masterdata-customer-brand-section__form-actions">
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

    <div class="masterdata-customer-brand-section__tables-grid">
      <el-card class="masterdata-customer-brand-section__card" shadow="never">
        <template #header>
          <div class="masterdata-customer-brand-section__card-header">
            <span>{{ t('masterdataAdmin.cards.customers') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: customerTotal }) }}</el-tag>
          </div>
        </template>

        <div class="masterdata-customer-brand-section__toolbar">
          <el-input v-model="customerQuery" data-testid="customers-query-input" :placeholder="t('masterdataAdmin.placeholders.searchCustomers')" clearable @keyup.enter="handleCustomerSearch" />
          <el-button data-testid="customers-query-button" @click="handleCustomerSearch">{{ t('common.actions.query') }}</el-button>
        </div>

        <el-table :data="customers" row-key="id" class="masterdata-customer-brand-section__table" :empty-text="isLoading ? t('masterdataAdmin.table.loadingCustomers') : t('masterdataAdmin.table.emptyCustomers')" data-testid="customers-table">
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

      <el-card class="masterdata-customer-brand-section__card" shadow="never">
        <template #header>
          <div class="masterdata-customer-brand-section__card-header">
            <span>{{ t('masterdataAdmin.cards.brands') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: brandTotal }) }}</el-tag>
          </div>
        </template>

        <div class="masterdata-customer-brand-section__toolbar">
          <el-input v-model="brandQuery" data-testid="brands-query-input" :placeholder="t('masterdataAdmin.placeholders.searchBrands')" clearable @keyup.enter="handleBrandSearch" />
          <el-button data-testid="brands-query-button" @click="handleBrandSearch">{{ t('common.actions.query') }}</el-button>
        </div>

        <el-table :data="brands" row-key="id" class="masterdata-customer-brand-section__table" :empty-text="isLoading ? t('masterdataAdmin.table.loadingBrands') : t('masterdataAdmin.table.emptyBrands')" data-testid="brands-table">
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
  </div>
</template>

<style scoped>
.masterdata-customer-brand-section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.masterdata-customer-brand-section__forms-grid,
.masterdata-customer-brand-section__tables-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.masterdata-customer-brand-section__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.masterdata-customer-brand-section__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.masterdata-customer-brand-section__feedback {
  margin-bottom: var(--mi-space-4);
}

.masterdata-customer-brand-section__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.masterdata-customer-brand-section__form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.masterdata-customer-brand-section__form-actions,
.masterdata-customer-brand-section__toolbar {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.masterdata-customer-brand-section__table,
.masterdata-customer-brand-section__toolbar :deep(.el-input),
.masterdata-customer-brand-section__form-grid :deep(.el-input-number),
.masterdata-customer-brand-section__form-grid :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .masterdata-customer-brand-section__forms-grid,
  .masterdata-customer-brand-section__tables-grid,
  .masterdata-customer-brand-section__form-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .masterdata-customer-brand-section__card-header,
  .masterdata-customer-brand-section__form-actions,
  .masterdata-customer-brand-section__toolbar {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
