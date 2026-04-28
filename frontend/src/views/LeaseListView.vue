<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

import { listLeases, type LeaseSummary } from '../api/lease'
import { listDepartments, listStores } from '../api/org'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { getErrorMessage } from '../composables/useErrorMessage'
import { usePagination } from '../composables/usePagination'
import { formatDate } from '../utils/format'

const router = useRouter()
const { t } = useI18n()

const rows = ref<LeaseSummary[]>([])
const deptMap = ref(new Map<number, string>())
const storeMap = ref(new Map<number, string>())
const { page, pageSize, total, paginationParams, resetPage, handlePageChange, handleSizeChange } = usePagination()
const isLoading = ref(false)
const errorMessage = ref('')

const { filters, isDirty, reset } = useFilterForm({
  lease_no: '',
  status: '',
})

const statusOptions = computed(() => [
  { label: t('common.statuses.draft'), value: 'draft' },
  { label: t('common.statuses.pendingApproval'), value: 'pending_approval' },
  { label: t('common.statuses.active'), value: 'active' },
  { label: t('common.statuses.rejected'), value: 'rejected' },
  { label: t('common.statuses.terminated'), value: 'terminated' },
])

const resolveSubtypeLabel = (subtype: string) => {
  switch (subtype) {
    case 'joint_operation':
      return t('lease.subtypes.jointOperation')
    case 'ad_board':
      return t('lease.subtypes.adBoard')
    case 'area_ground':
      return t('lease.subtypes.areaGround')
    default:
      return t('lease.subtypes.standard')
  }
}

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
    case 'terminated':
      return t('common.statuses.terminated')
    default:
      return status
  }
}

const loadLeases = async () => {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await listLeases({
      lease_no: filters.lease_no.trim() || undefined,
      status: filters.status || undefined,
      ...paginationParams.value,
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    errorMessage.value = getErrorMessage(error, t('lease.errors.unableToLoad'))
    rows.value = []
    total.value = 0
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  resetPage()
  void loadLeases()
}

const handlePaginationPageChange = (newPage: number) => {
  handlePageChange(newPage)
  void loadLeases()
}

const handlePaginationSizeChange = (newSize: number) => {
  handleSizeChange(newSize)
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

onMounted(async () => {
  void loadLeases()
  try {
    const [deptResp, storeResp] = await Promise.all([listDepartments(), listStores()])
    deptResp.data.departments?.forEach((d: any) => deptMap.value.set(d.id, d.name))
    storeResp.data.stores?.forEach((s: any) => storeMap.value.set(s.id, s.name))
  } catch { /* non-critical */ }
})
</script>

<template>
  <div class="lease-list-view" v-loading="isLoading" data-testid="lease-list-view">
    <div data-testid="lease-contracts-view">
      <PageSection
        :eyebrow="t('lease.eyebrow')"
        :title="t('lease.title')"
        :summary="t('lease.summary')"
      >
        <template #actions>
          <el-button type="primary" data-testid="lease-create-button" @click="handleCreate">
            {{ t('lease.actions.create') }}
          </el-button>
        </template>
      </PageSection>

      <el-alert
        v-if="errorMessage"
        :closable="false"
        class="lease-list-view__alert"
        :title="t('lease.errors.recordsUnavailable')"
        type="error"
        show-icon
        :description="errorMessage"
      />

      <FilterForm
        :title="t('lease.filters.title')"
        :busy="isLoading"
        :reset-disabled="!isDirty"
        @reset="handleReset"
        @submit="resetPage(); loadLeases()"
      >
        <el-form-item :label="t('lease.fields.leaseNumber')">
          <el-input
            v-model="filters.lease_no"
            :placeholder="t('lease.placeholders.searchLeaseNumber')"
            clearable
            data-testid="lease-contracts-view-query-input"
          />
        </el-form-item>

        <el-form-item :label="t('lease.fields.status')">
          <el-select v-model="filters.status" :placeholder="t('lease.placeholders.allStatuses')" clearable>
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
            <span>{{ t('lease.table.queueTitle') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
          </div>
        </template>

        <el-table
          :data="rows"
          row-key="id"
          class="lease-list-view__table"
          :empty-text="t('lease.table.empty')"
          data-testid="lease-table"
          @row-click="handleRowClick"
        >
          <el-table-column prop="lease_no" :label="t('lease.columns.leaseNo')" min-width="180" />
          <el-table-column prop="tenant_name" :label="t('lease.columns.tenant')" min-width="220" />
          <el-table-column :label="t('lease.columns.subtype')" min-width="160">
            <template #default="scope">{{ resolveSubtypeLabel(scope.row.subtype) }}</template>
          </el-table-column>
          <el-table-column :label="t('lease.columns.department')" min-width="120">
            <template #default="scope">{{ deptMap.get(scope.row.department_id) ?? scope.row.department_id }}</template>
          </el-table-column>
          <el-table-column :label="t('lease.columns.store')" min-width="120">
            <template #default="scope">{{ storeMap.get(scope.row.store_id) ?? scope.row.store_id }}</template>
          </el-table-column>
          <el-table-column :label="t('lease.columns.startDate')" min-width="120">
            <template #default="scope">{{ formatDate(scope.row.start_date) }}</template>
          </el-table-column>
          <el-table-column :label="t('lease.columns.endDate')" min-width="120">
            <template #default="scope">{{ formatDate(scope.row.end_date) }}</template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="140">
            <template #default="scope">
              {{ resolveStatusLabel(scope.row.status) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="140" fixed="right">
            <template #default="scope">
              <el-button link type="primary" @click.stop="openLease(scope.row.id)">{{ t('common.actions.view') }}</el-button>
            </template>
          </el-table-column>
        </el-table>

        <div class="lease-list-view__pagination">
          <el-pagination
            v-model:current-page="page"
            v-model:page-size="pageSize"
            :total="total"
            :page-sizes="[10, 20, 50, 100]"
            layout="total, sizes, prev, pager, next"
            @current-change="handlePaginationPageChange"
            @size-change="handlePaginationSizeChange"
          />
        </div>
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

.lease-list-view__pagination {
  display: flex;
  justify-content: flex-end;
  margin-top: var(--mi-space-4);
}

.lease-list-view__table :deep(.el-table__row) {
  cursor: pointer;
}
</style>
