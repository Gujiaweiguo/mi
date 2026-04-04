<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'

import { createBrand, createCustomer, listBrands, listCustomers, type Brand, type Customer } from '../api/masterdata'
import { listDepartments, type Department } from '../api/org'
import PageSection from '../components/platform/PageSection.vue'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type CustomerCreateForm = {
  code: string
  name: string
  trade_id: number | undefined
  department_id: number | undefined
}

type BrandCreateForm = {
  code: string
  name: string
}

const customers = ref<Customer[]>([])
const brands = ref<Brand[]>([])
const departments = ref<Department[]>([])

const isLoading = ref(false)
const isCustomerSaving = ref(false)
const isBrandSaving = ref(false)

const pageFeedback = ref<Feedback | null>(null)
const customerFeedback = ref<Feedback | null>(null)
const brandFeedback = ref<Feedback | null>(null)

const customerForm = reactive<CustomerCreateForm>({
  code: '',
  name: '',
  trade_id: undefined,
  department_id: undefined,
})

const brandForm = reactive<BrandCreateForm>({
  code: '',
  name: '',
})

const canCreateCustomer = computed(() => Boolean(customerForm.code.trim() && customerForm.name.trim()))
const canCreateBrand = computed(() => Boolean(brandForm.code.trim() && brandForm.name.trim()))

const getErrorMessage = (error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)

const formatDate = (value: string) => {
  if (!value) {
    return '—'
  }

  return new Intl.DateTimeFormat('en-US', { dateStyle: 'medium' }).format(new Date(value))
}

const resolveDepartmentLabel = (departmentId: number | null) => {
  if (!departmentId) {
    return '—'
  }

  const department = departments.value.find((item) => item.id === departmentId)
  if (!department) {
    return `#${departmentId}`
  }

  return `${department.code} — ${department.name}`
}

const resetCustomerForm = () => {
  customerForm.code = ''
  customerForm.name = ''
  customerForm.trade_id = undefined
  customerForm.department_id = undefined
}

const resetBrandForm = () => {
  brandForm.code = ''
  brandForm.name = ''
}

const loadMasterData = async () => {
  isLoading.value = true
  pageFeedback.value = null

  const [customersResult, brandsResult, departmentsResult] = await Promise.allSettled([
    listCustomers(),
    listBrands(),
    listDepartments(),
  ])

  const loadErrors: string[] = []

  if (customersResult.status === 'fulfilled') {
    customers.value = customersResult.value.data.customers ?? []
  } else {
    customers.value = []
    loadErrors.push(getErrorMessage(customersResult.reason, 'Unable to load customers.'))
  }

  if (brandsResult.status === 'fulfilled') {
    brands.value = brandsResult.value.data.brands ?? []
  } else {
    brands.value = []
    loadErrors.push(getErrorMessage(brandsResult.reason, 'Unable to load brands.'))
  }

  if (departmentsResult.status === 'fulfilled') {
    departments.value = departmentsResult.value.data.departments ?? []
  } else {
    departments.value = []
    loadErrors.push(getErrorMessage(departmentsResult.reason, 'Unable to load departments.'))
  }

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'error',
      title: 'Master data unavailable',
      description: loadErrors.join(' '),
    }
  }

  isLoading.value = false
}

const handleCreateCustomer = async () => {
  if (!canCreateCustomer.value) {
    customerFeedback.value = {
      type: 'warning',
      title: 'Customer details required',
      description: 'Enter both a customer code and customer name before creating a record.',
    }
    return
  }

  isCustomerSaving.value = true
  customerFeedback.value = null

  try {
    const response = await createCustomer({
      code: customerForm.code.trim(),
      name: customerForm.name.trim(),
      trade_id: customerForm.trade_id ?? null,
      department_id: customerForm.department_id ?? null,
    })

    customers.value = [response.data.customer, ...customers.value]
    customerFeedback.value = {
      type: 'success',
      title: 'Customer created',
      description: `Customer "${response.data.customer.code}" is now available for master-data maintenance.`,
    }
    resetCustomerForm()
  } catch (error) {
    customerFeedback.value = {
      type: 'error',
      title: 'Customer creation failed',
      description: getErrorMessage(error, 'Unable to create the customer.'),
    }
  } finally {
    isCustomerSaving.value = false
  }
}

const handleCreateBrand = async () => {
  if (!canCreateBrand.value) {
    brandFeedback.value = {
      type: 'warning',
      title: 'Brand details required',
      description: 'Enter both a brand code and brand name before creating a record.',
    }
    return
  }

  isBrandSaving.value = true
  brandFeedback.value = null

  try {
    const response = await createBrand({
      code: brandForm.code.trim(),
      name: brandForm.name.trim(),
    })

    brands.value = [response.data.brand, ...brands.value]
    brandFeedback.value = {
      type: 'success',
      title: 'Brand created',
      description: `Brand "${response.data.brand.code}" is now available for assignment and lookup.`,
    }
    resetBrandForm()
  } catch (error) {
    brandFeedback.value = {
      type: 'error',
      title: 'Brand creation failed',
      description: getErrorMessage(error, 'Unable to create the brand.'),
    }
  } finally {
    isBrandSaving.value = false
  }
}

onMounted(() => {
  void loadMasterData()
})
</script>

<template>
  <div class="masterdata-admin-view" data-testid="masterdata-admin-view">
    <PageSection
      eyebrow="Master data operations"
      title="Master data admin"
      summary="Maintain customer and brand reference records for the new operational workflow, with department-aware customer setup from the admin console."
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ customers.length }} customers</el-tag>
        <el-tag effect="plain" type="success">{{ brands.length }} brands</el-tag>
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
            <span>Create customer</span>
          </div>
        </template>

        <el-alert
          v-if="customerFeedback"
          :closable="false"
          class="masterdata-admin-view__feedback"
          :title="customerFeedback.title"
          :type="customerFeedback.type"
          :description="customerFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
          <div class="masterdata-admin-view__form-grid">
            <el-form-item label="Customer code">
              <el-input
                v-model="customerForm.code"
                placeholder="Enter customer code"
                data-testid="customer-code-input"
              />
            </el-form-item>

            <el-form-item label="Customer name">
              <el-input
                v-model="customerForm.name"
                placeholder="Enter customer name"
                data-testid="customer-name-input"
              />
            </el-form-item>

            <el-form-item label="Trade id">
              <el-input-number
                v-model="customerForm.trade_id"
                :min="1"
                controls-position="right"
                data-testid="customer-trade-input"
              />
            </el-form-item>

            <el-form-item label="Department">
              <el-select
                v-model="customerForm.department_id"
                clearable
                filterable
                placeholder="Select a department"
                data-testid="customer-department-input"
              >
                <el-option
                  v-for="department in departments"
                  :key="department.id"
                  :label="`${department.code} — ${department.name}`"
                  :value="department.id"
                />
              </el-select>
            </el-form-item>
          </div>

          <div class="masterdata-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isCustomerSaving"
              :disabled="!canCreateCustomer"
              data-testid="customer-create-button"
              @click="handleCreateCustomer"
            >
              Create customer
            </el-button>
          </div>
        </el-form>
      </el-card>

      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>Create brand</span>
          </div>
        </template>

        <el-alert
          v-if="brandFeedback"
          :closable="false"
          class="masterdata-admin-view__feedback"
          :title="brandFeedback.title"
          :type="brandFeedback.type"
          :description="brandFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="masterdata-admin-view__form" @submit.prevent>
          <div class="masterdata-admin-view__form-grid">
            <el-form-item label="Brand code">
              <el-input v-model="brandForm.code" placeholder="Enter brand code" data-testid="brand-code-input" />
            </el-form-item>

            <el-form-item label="Brand name">
              <el-input v-model="brandForm.name" placeholder="Enter brand name" data-testid="brand-name-input" />
            </el-form-item>
          </div>

          <div class="masterdata-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isBrandSaving"
              :disabled="!canCreateBrand"
              data-testid="brand-create-button"
              @click="handleCreateBrand"
            >
              Create brand
            </el-button>
          </div>
        </el-form>
      </el-card>
    </div>

    <div class="masterdata-admin-view__tables-grid">
      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>Customers</span>
            <el-tag effect="plain" type="info">{{ customers.length }} total</el-tag>
          </div>
        </template>

        <el-table
          :data="customers"
          row-key="id"
          class="masterdata-admin-view__table"
          :empty-text="isLoading ? 'Loading customers…' : 'No customers available.'"
          data-testid="customers-table"
        >
          <el-table-column prop="code" label="Code" min-width="140" />
          <el-table-column prop="name" label="Name" min-width="220" />
          <el-table-column label="Trade id" min-width="110">
            <template #default="scope">
              {{ scope.row.trade_id ?? '—' }}
            </template>
          </el-table-column>
          <el-table-column label="Department" min-width="220">
            <template #default="scope">
              {{ resolveDepartmentLabel(scope.row.department_id) }}
            </template>
          </el-table-column>
          <el-table-column prop="status" label="Status" min-width="120">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ scope.row.status }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column label="Created" min-width="160">
            <template #default="scope">
              {{ formatDate(scope.row.created_at) }}
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="masterdata-admin-view__card" shadow="never">
        <template #header>
          <div class="masterdata-admin-view__card-header">
            <span>Brands</span>
            <el-tag effect="plain" type="info">{{ brands.length }} total</el-tag>
          </div>
        </template>

        <el-table
          :data="brands"
          row-key="id"
          class="masterdata-admin-view__table"
          :empty-text="isLoading ? 'Loading brands…' : 'No brands available.'"
          data-testid="brands-table"
        >
          <el-table-column prop="code" label="Code" min-width="140" />
          <el-table-column prop="name" label="Name" min-width="220" />
          <el-table-column prop="status" label="Status" min-width="120">
            <template #default="scope">
              <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
                {{ scope.row.status }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column label="Created" min-width="160">
            <template #default="scope">
              {{ formatDate(scope.row.created_at) }}
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </div>
  </div>
</template>

<style scoped>
.masterdata-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.masterdata-admin-view__forms-grid,
.masterdata-admin-view__tables-grid {
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

.masterdata-admin-view__form-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.masterdata-admin-view__table,
.masterdata-admin-view__form-grid :deep(.el-input-number),
.masterdata-admin-view__form-grid :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .masterdata-admin-view__forms-grid,
  .masterdata-admin-view__tables-grid,
  .masterdata-admin-view__form-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .masterdata-admin-view__card-header,
  .masterdata-admin-view__form-actions {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
