<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

import { listLeases, type LeaseSummary } from '../api/lease'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

const router = useRouter()

const rows = ref<LeaseSummary[]>([])
const total = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')

const { filters, isDirty, reset } = useFilterForm({
  lease_no: '',
  status: '',
})

const statusOptions = [
  { label: 'Draft', value: 'draft' },
  { label: 'Pending approval', value: 'pending_approval' },
  { label: 'Active', value: 'active' },
  { label: 'Rejected', value: 'rejected' },
  { label: 'Terminated', value: 'terminated' },
]

const loadLeases = async () => {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await listLeases({
      lease_no: filters.lease_no.trim() || undefined,
      status: filters.status || undefined,
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unable to load lease contracts.'
    rows.value = []
    total.value = 0
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  void loadLeases()
}

const handleCreate = async () => {
  await router.push({ name: 'lease-contracts-new' })
}

const openLease = async (leaseId: number) => {
  await router.push({ name: 'lease-contract-detail', params: { id: String(leaseId) } })
}

const handleRowClick = (row: LeaseSummary) => {
  void openLease(row.id)
}

onMounted(() => {
  void loadLeases()
})
</script>

<template>
  <div class="lease-list-view" data-testid="lease-list-view">
    <div data-testid="lease-contracts-view">
      <PageSection
        eyebrow="Lease delivery runway"
        title="Lease contracts"
        summary="Track lease contracts, filter operational records, and jump directly into creation or review workflows."
      >
        <template #actions>
          <el-button type="primary" data-testid="lease-create-button" @click="handleCreate">
            Create lease
          </el-button>
        </template>
      </PageSection>

      <el-alert
        v-if="errorMessage"
        :closable="false"
        class="lease-list-view__alert"
        title="Lease records unavailable"
        type="error"
        show-icon
        :description="errorMessage"
      />

      <FilterForm
        title="Lease filters"
        :busy="isLoading"
        :reset-disabled="!isDirty"
        @reset="handleReset"
        @submit="loadLeases"
      >
        <el-form-item label="Lease number">
          <el-input
            v-model="filters.lease_no"
            placeholder="Search by lease number"
            clearable
            data-testid="lease-contracts-view-query-input"
          />
        </el-form-item>

        <el-form-item label="Status">
          <el-select v-model="filters.status" placeholder="All statuses" clearable>
            <el-option
              v-for="option in statusOptions"
              :key="option.value"
              :label="option.label"
              :value="option.value"
            />
          </el-select>
        </el-form-item>
      </FilterForm>

      <el-card class="lease-list-view__table-card" shadow="never">
        <template #header>
          <div class="lease-list-view__table-header">
            <span>Lease queue</span>
            <el-tag effect="plain" type="info">{{ total }} total</el-tag>
          </div>
        </template>

        <el-table
          :data="rows"
          row-key="id"
          class="lease-list-view__table"
          empty-text="No lease contracts match the current filters."
          data-testid="lease-table"
          @row-click="handleRowClick"
        >
          <el-table-column prop="lease_no" label="Lease no." min-width="180" />
          <el-table-column prop="tenant_name" label="Tenant" min-width="220" />
          <el-table-column prop="department_id" label="Department" min-width="120" />
          <el-table-column prop="store_id" label="Store" min-width="120" />
          <el-table-column prop="start_date" label="Start date" min-width="140" />
          <el-table-column prop="end_date" label="End date" min-width="140" />
          <el-table-column prop="status" label="Status" min-width="140" />
          <el-table-column label="Actions" min-width="140" fixed="right">
            <template #default="scope">
              <el-button link type="primary" @click.stop="openLease(scope.row.id)">View</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </div>
  </div>
</template>

<style scoped>
.lease-list-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.lease-list-view > div {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.lease-list-view__alert {
  margin-bottom: 0;
}

.lease-list-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.lease-list-view__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.lease-list-view__table {
  width: 100%;
}

.lease-list-view__table :deep(.el-table__row) {
  cursor: pointer;
}
</style>
