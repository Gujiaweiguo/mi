<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { listNotifications, type NotificationOutboxEntry } from '../api/notification'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { getErrorMessage } from '../composables/useErrorMessage'
import { usePagination } from '../composables/usePagination'
import { formatDate } from '../utils/format'

const { t } = useI18n()

const rows = ref<NotificationOutboxEntry[]>([])
const createdAtRange = ref<[string, string] | null>(null)
const { page, pageSize, total, paginationParams, resetPage, handlePageChange, handleSizeChange } = usePagination()
const isLoading = ref(false)
const errorMessage = ref('')

const { filters, isDirty: isFilterDirty, reset } = useFilterForm({
  event_type: '',
  status: '',
})

const isDirty = computed(() => isFilterDirty.value || Boolean(createdAtRange.value?.length))

const eventTypeOptions = computed(() =>
  [...new Set(rows.value.map((row) => row.event_type).filter((value) => value.trim().length > 0))].map((value) => ({
    label: value,
    value,
  })),
)

const statusOptions = computed(() => [
  { label: t('notifications.statuses.pending'), value: 'pending' },
  { label: t('notifications.statuses.sending'), value: 'sending' },
  { label: t('notifications.statuses.sent'), value: 'sent' },
  { label: t('notifications.statuses.failed'), value: 'failed' },
  { label: t('notifications.statuses.dead'), value: 'dead' },
])

const resolveStatusLabel = (status: string) => {
  switch (status) {
    case 'pending':
      return t('notifications.statuses.pending')
    case 'sending':
      return t('notifications.statuses.sending')
    case 'sent':
      return t('notifications.statuses.sent')
    case 'failed':
      return t('notifications.statuses.failed')
    case 'dead':
      return t('notifications.statuses.dead')
    default:
      return status
  }
}

const statusTagType = (status: string) => {
  switch (status) {
    case 'sent':
      return 'success'
    case 'pending':
      return 'warning'
    case 'failed':
      return 'danger'
    case 'dead':
      return 'info'
    case 'sending':
      return 'primary'
    default:
      return 'info'
  }
}

const isWithinCreatedAtRange = (value: string) => {
  if (!value || !createdAtRange.value) {
    return true
  }

  const [start, end] = createdAtRange.value
  const current = new Date(value).getTime()
  const startTime = new Date(`${start}T00:00:00`).getTime()
  const endTime = new Date(`${end}T23:59:59`).getTime()

  return current >= startTime && current <= endTime
}

const visibleRows = computed(() => rows.value.filter((row) => isWithinCreatedAtRange(row.created_at)))

const loadNotifications = async () => {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await listNotifications({
      event_type: filters.event_type || undefined,
      status: filters.status || undefined,
      ...paginationParams.value,
    })

    rows.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    rows.value = []
    total.value = 0
    errorMessage.value = getErrorMessage(error, t('notifications.errors.unableToLoad'))
  } finally {
    isLoading.value = false
  }
}

const handleReset = () => {
  reset()
  createdAtRange.value = null
  resetPage()
  void loadNotifications()
}

const handlePaginationPageChange = (newPage: number) => {
  handlePageChange(newPage)
  void loadNotifications()
}

const handlePaginationSizeChange = (newSize: number) => {
  handleSizeChange(newSize)
  void loadNotifications()
}

onMounted(() => {
  void loadNotifications()
})
</script>

<template>
  <div class="notifications-view" v-loading="isLoading" data-testid="notifications-view">
    <PageSection
      :eyebrow="t('notifications.eyebrow')"
      :title="t('notifications.title')"
      :summary="t('notifications.summary')"
    />

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="notifications-view__alert"
      :title="t('notifications.errors.recordsUnavailable')"
      type="error"
      show-icon
      :description="errorMessage"
      data-testid="notifications-error-alert"
    />

    <FilterForm
      :title="t('notifications.filters.title')"
      :busy="isLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="resetPage(); loadNotifications()"
    >
      <el-form-item :label="t('notifications.fields.eventType')">
        <el-select
          v-model="filters.event_type"
          clearable
          class="notifications-view__filter-input"
          :placeholder="t('notifications.placeholders.allEventTypes')"
          data-testid="notifications-event-type-filter"
        >
          <el-option
            v-for="option in eventTypeOptions"
            :key="option.value"
            :label="option.label"
            :value="option.value"
          />
        </el-select>
      </el-form-item>

      <el-form-item :label="t('notifications.fields.status')">
        <el-select
          v-model="filters.status"
          clearable
          class="notifications-view__filter-input"
          :placeholder="t('notifications.placeholders.allStatuses')"
          data-testid="notifications-status-filter"
        >
          <el-option
            v-for="option in statusOptions"
            :key="option.value"
            :label="option.label"
            :value="option.value"
          />
        </el-select>
      </el-form-item>

      <el-form-item :label="t('notifications.fields.createdAtRange')">
        <el-date-picker
          v-model="createdAtRange"
          type="daterange"
          value-format="YYYY-MM-DD"
          format="YYYY-MM-DD"
          class="notifications-view__filter-input"
          range-separator="→"
          :start-placeholder="t('notifications.placeholders.createdAtStart')"
          :end-placeholder="t('notifications.placeholders.createdAtEnd')"
          data-testid="notifications-created-at-range-filter"
        />
      </el-form-item>
    </FilterForm>

    <el-card class="notifications-view__table-card" shadow="never">
      <template #header>
        <div class="notifications-view__table-header">
          <span>{{ t('notifications.table.title') }}</span>
          <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
        </div>
      </template>

      <el-table
        :data="visibleRows"
        row-key="id"
        class="notifications-view__table"
        :empty-text="t('notifications.table.empty')"
        data-testid="notifications-table"
      >
        <el-table-column prop="event_type" :label="t('notifications.columns.eventType')" min-width="180" />
        <el-table-column prop="aggregate_type" :label="t('notifications.columns.aggregateType')" min-width="160" />
        <el-table-column prop="subject" :label="t('notifications.columns.subject')" min-width="260" />
        <el-table-column prop="recipient_to" :label="t('notifications.columns.recipientTo')" min-width="220" />
        <el-table-column :label="t('notifications.columns.status')" min-width="140">
          <template #default="scope">
            <el-tag :type="statusTagType(scope.row.status)" effect="plain">
              {{ resolveStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('notifications.columns.createdAt')" min-width="200">
          <template #default="scope">
            {{ formatDate(scope.row.created_at) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('notifications.columns.sentAt')" min-width="200">
          <template #default="scope">
            {{ formatDate(scope.row.sent_at) }}
          </template>
        </el-table-column>
      </el-table>

      <div class="notifications-view__pagination">
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
</template>

<style scoped>
.notifications-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.notifications-view__alert {
  margin-bottom: 0;
}

.notifications-view__filter-input,
.notifications-view__table {
  width: 100%;
}

.notifications-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.notifications-view__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.notifications-view__pagination {
  display: flex;
  justify-content: flex-end;
  margin-top: var(--mi-space-4);
}

@media (max-width: 52rem) {
  .notifications-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
