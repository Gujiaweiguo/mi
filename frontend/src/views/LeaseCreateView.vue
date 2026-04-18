<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

import { listBrands, listCustomers, type Brand, type Customer } from '../api/masterdata'
import { createLease } from '../api/lease'
import { listDepartments, listStores, type Department, type Store } from '../api/org'
import PageSection from '../components/platform/PageSection.vue'

type LeaseCreateForm = {
  lease_no: string
  tenant_name: string
  department_id: number | null
  store_id: number | null
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
}

const router = useRouter()
const { t } = useI18n()

const formRef = ref<FormInstance>()
const isSaving = ref(false)
const errorMessage = ref('')
const setupErrorMessage = ref('')
const isLoadingOptions = ref(false)
const customers = ref<Customer[]>([])
const brands = ref<Brand[]>([])
const departments = ref<Department[]>([])
const stores = ref<Store[]>([])

const form = reactive<LeaseCreateForm>({
  lease_no: '',
  tenant_name: '',
  department_id: null,
  store_id: null,
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
})

const rules = computed<FormRules<LeaseCreateForm>>(() => ({
  lease_no: [{ required: true, message: t('leaseCreate.validation.leaseNumberRequired'), trigger: 'blur' }],
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

const termTypeOptions = computed(() => [
  { label: t('leaseCreate.options.termTypes.rent'), value: 'rent' },
  { label: t('leaseCreate.options.termTypes.deposit'), value: 'deposit' },
])

const billingCycleOptions = computed(() => [
  { label: t('leaseCreate.options.billingCycles.monthly'), value: 'monthly' },
  { label: t('leaseCreate.options.billingCycles.quarterly'), value: 'quarterly' },
  { label: t('leaseCreate.options.billingCycles.yearly'), value: 'yearly' },
])

const availableStores = computed(() => {
  if (form.department_id === null) {
    return stores.value
  }

  return stores.value.filter((store) => store.department_id === form.department_id)
})

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
    setupErrorMessage.value =
      error instanceof Error ? error.message : t('leaseCreate.errors.unableToLoadReferenceData')
  } finally {
    isLoadingOptions.value = false
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

const handleCancel = async () => {
  await router.push({ name: 'lease-contracts' })
}

const handleSubmit = async () => {
  errorMessage.value = ''
  const isValid = await formRef.value?.validate().catch(() => false)

  if (isValid !== true) {
    return
  }

  isSaving.value = true

  try {
    const response = await createLease({
      lease_no: form.lease_no.trim(),
      tenant_name: form.tenant_name.trim(),
      department_id: form.department_id ?? 0,
      store_id: form.store_id ?? 0,
      customer_id: form.customer_id,
      brand_id: form.brand_id,
      trade_id: form.trade_id,
      management_type_id: form.management_type_id,
      start_date: form.start_date,
      end_date: form.end_date,
      units: [
        {
          unit_id: form.unit_id ?? 0,
          rent_area: form.rent_area ?? 0,
        },
      ],
      terms: [
        {
          term_type: form.term_type,
          billing_cycle: form.billing_cycle,
          currency_type_id: form.currency_type_id ?? 0,
          amount: form.amount ?? 0,
          effective_from: form.effective_from,
          effective_to: form.effective_to,
        },
      ],
    })

    await router.replace({
      name: 'lease-contract-detail',
      params: { id: String(response.data.lease.id) },
    })
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('leaseCreate.errors.unableToCreate')
  } finally {
    isSaving.value = false
  }
}

onMounted(() => {
  void loadReferenceData()
})
</script>

<template>
  <div class="lease-create-view" v-loading="isLoadingOptions" data-testid="lease-create-view">
    <PageSection
      :eyebrow="t('lease.eyebrow')"
      :title="t('leaseCreate.title')"
      :summary="t('leaseCreate.summary')"
    >
      <template #actions>
        <el-button data-testid="lease-create-back-button" @click="handleCancel">{{ t('leaseCreate.actions.backToList') }}</el-button>
      </template>
    </PageSection>

    <el-card class="lease-create-view__card" shadow="never">
      <template #header>
        <div class="lease-create-view__card-header">
          <span>{{ t('leaseCreate.cards.setup') }}</span>
        </div>
      </template>

      <el-alert
        v-if="errorMessage"
        :closable="false"
        class="lease-create-view__alert"
        :title="t('leaseCreate.errors.creationFailed')"
        type="error"
        show-icon
        :description="errorMessage"
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
        <div class="lease-create-view__grid">
          <el-form-item :label="t('leaseCreate.fields.leaseNumber')" prop="lease_no">
            <el-input
              v-model="form.lease_no"
              :placeholder="t('leaseCreate.placeholders.enterLeaseNumber')"
              data-testid="lease-number-input"
            />
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

          <el-form-item :label="t('leaseCreate.fields.unitId')" prop="unit_id">
            <el-input-number v-model="form.unit_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('leaseCreate.fields.rentArea')" prop="rent_area">
            <el-input-number v-model="form.rent_area" :min="0" :precision="2" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('leaseCreate.fields.termType')" prop="term_type">
            <el-select v-model="form.term_type" :placeholder="t('leaseCreate.placeholders.selectTermType')">
              <el-option
                v-for="option in termTypeOptions"
                :key="option.value"
                :label="option.label"
                :value="option.value"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('leaseCreate.fields.billingCycle')" prop="billing_cycle">
            <el-select v-model="form.billing_cycle" :placeholder="t('leaseCreate.placeholders.selectBillingCycle')">
              <el-option
                v-for="option in billingCycleOptions"
                :key="option.value"
                :label="option.label"
                :value="option.value"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="t('leaseCreate.fields.currencyTypeId')" prop="currency_type_id">
            <el-input-number v-model="form.currency_type_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item :label="t('leaseCreate.fields.amount')" prop="amount">
            <el-input-number v-model="form.amount" :min="0" :precision="2" controls-position="right" />
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

        <div class="lease-create-view__actions">
          <el-button data-testid="lease-create-cancel-button" @click="handleCancel">{{ t('leaseCreate.actions.cancel') }}</el-button>
          <el-button type="primary" :loading="isSaving" data-testid="lease-create-submit-button" @click="handleSubmit">
            {{ t('leaseCreate.actions.create') }}
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

.lease-create-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
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
  gap: var(--mi-space-4);
}

.lease-create-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.lease-create-view__actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.lease-create-view__grid :deep(.el-input-number),
.lease-create-view__grid :deep(.el-input),
.lease-create-view__grid :deep(.el-date-editor),
.lease-create-view__grid :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .lease-create-view__grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .lease-create-view__actions {
    justify-content: flex-start;
    flex-wrap: wrap;
  }
}
</style>
