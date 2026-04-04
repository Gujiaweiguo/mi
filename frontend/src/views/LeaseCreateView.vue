<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
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

const rules: FormRules<LeaseCreateForm> = {
  lease_no: [{ required: true, message: 'Enter the lease number.', trigger: 'blur' }],
  tenant_name: [{ required: true, message: 'Enter the tenant name.', trigger: 'blur' }],
  department_id: [{ required: true, message: 'Select the department.', trigger: 'change' }],
  store_id: [{ required: true, message: 'Select the store.', trigger: 'change' }],
  start_date: [{ required: true, message: 'Select the lease start date.', trigger: 'change' }],
  end_date: [{ required: true, message: 'Select the lease end date.', trigger: 'change' }],
  unit_id: [{ required: true, message: 'Enter the unit id.', trigger: 'change' }],
  rent_area: [{ required: true, message: 'Enter the rent area.', trigger: 'change' }],
  term_type: [{ required: true, message: 'Select the term type.', trigger: 'change' }],
  billing_cycle: [{ required: true, message: 'Select the billing cycle.', trigger: 'change' }],
  currency_type_id: [{ required: true, message: 'Enter the currency type id.', trigger: 'change' }],
  amount: [{ required: true, message: 'Enter the term amount.', trigger: 'change' }],
  effective_from: [{ required: true, message: 'Select the term start date.', trigger: 'change' }],
  effective_to: [{ required: true, message: 'Select the term end date.', trigger: 'change' }],
}

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
      error instanceof Error ? error.message : 'Unable to load customer, brand, department, and store selections.'
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
    errorMessage.value = error instanceof Error ? error.message : 'Unable to create the lease contract.'
  } finally {
    isSaving.value = false
  }
}

onMounted(() => {
  void loadReferenceData()
})
</script>

<template>
  <div class="lease-create-view" data-testid="lease-create-view">
    <PageSection
      eyebrow="Lease delivery runway"
      title="Create lease contract"
      summary="Capture the contract header, one initial unit, and one billing term to open the lease workflow."
    >
      <template #actions>
        <el-button @click="handleCancel">Back to list</el-button>
      </template>
    </PageSection>

    <el-card class="lease-create-view__card" shadow="never">
      <template #header>
        <div class="lease-create-view__card-header">
          <span>Lease contract setup</span>
        </div>
      </template>

      <el-alert
        v-if="errorMessage"
        :closable="false"
        class="lease-create-view__alert"
        title="Lease creation failed"
        type="error"
        show-icon
        :description="errorMessage"
      />

      <el-alert
        v-if="setupErrorMessage"
        :closable="false"
        class="lease-create-view__alert"
        title="Reference data unavailable"
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
          <el-form-item label="Lease number" prop="lease_no">
            <el-input v-model="form.lease_no" placeholder="Enter lease number" />
          </el-form-item>

          <el-form-item label="Tenant name" prop="tenant_name">
            <el-input v-model="form.tenant_name" placeholder="Enter tenant name" />
          </el-form-item>

          <el-form-item label="Department" prop="department_id">
            <el-select
              v-model="form.department_id"
              placeholder="Select department"
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

          <el-form-item label="Store" prop="store_id">
            <el-select
              v-model="form.store_id"
              placeholder="Select store"
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

          <el-form-item label="Customer">
            <el-select
              v-model="form.customer_id"
              placeholder="Select customer"
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

          <el-form-item label="Brand">
            <el-select
              v-model="form.brand_id"
              placeholder="Select brand"
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

          <el-form-item label="Trade ID">
            <el-input-number v-model="form.trade_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item label="Management type ID">
            <el-input-number v-model="form.management_type_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item label="Start date" prop="start_date">
            <el-date-picker v-model="form.start_date" type="date" value-format="YYYY-MM-DD" placeholder="Select start date" />
          </el-form-item>

          <el-form-item label="End date" prop="end_date">
            <el-date-picker v-model="form.end_date" type="date" value-format="YYYY-MM-DD" placeholder="Select end date" />
          </el-form-item>

          <el-form-item label="Unit id" prop="unit_id">
            <el-input-number v-model="form.unit_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item label="Rent area" prop="rent_area">
            <el-input-number v-model="form.rent_area" :min="0" :precision="2" controls-position="right" />
          </el-form-item>

          <el-form-item label="Term type" prop="term_type">
            <el-select v-model="form.term_type">
              <el-option label="Rent" value="rent" />
              <el-option label="Deposit" value="deposit" />
            </el-select>
          </el-form-item>

          <el-form-item label="Billing cycle" prop="billing_cycle">
            <el-select v-model="form.billing_cycle">
              <el-option label="Monthly" value="monthly" />
              <el-option label="Quarterly" value="quarterly" />
              <el-option label="Yearly" value="yearly" />
            </el-select>
          </el-form-item>

          <el-form-item label="Currency type id" prop="currency_type_id">
            <el-input-number v-model="form.currency_type_id" :min="1" controls-position="right" />
          </el-form-item>

          <el-form-item label="Amount" prop="amount">
            <el-input-number v-model="form.amount" :min="0" :precision="2" controls-position="right" />
          </el-form-item>

          <el-form-item label="Term effective from" prop="effective_from">
            <el-date-picker
              v-model="form.effective_from"
              type="date"
              value-format="YYYY-MM-DD"
              placeholder="Select effective from"
            />
          </el-form-item>

          <el-form-item label="Term effective to" prop="effective_to">
            <el-date-picker
              v-model="form.effective_to"
              type="date"
              value-format="YYYY-MM-DD"
              placeholder="Select effective to"
            />
          </el-form-item>
        </div>

        <div class="lease-create-view__actions">
          <el-button @click="handleCancel">Cancel</el-button>
          <el-button type="primary" :loading="isSaving" @click="handleSubmit">Create lease</el-button>
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
