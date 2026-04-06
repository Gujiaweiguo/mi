<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

import DataTable from '../components/platform/DataTable.vue'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

const props = defineProps<{
  eyebrow: string
  title: string
  summary: string
  filterLabel: string
  filterPlaceholder: string
  rows: Record<string, unknown>[]
  columns: { key: string; label: string; minWidth?: number }[]
  testId: string
}>()

const { t } = useI18n()

const { filters, isDirty, reset } = useFilterForm({
  query: '',
})

const normalizedQuery = computed(() => filters.query.trim().toLowerCase())

const visibleRows = computed(() => {
  if (!normalizedQuery.value) {
    return props.rows
  }

  return props.rows.filter((row) =>
    Object.values(row).some((value) => String(value).toLowerCase().includes(normalizedQuery.value)),
  )
})

const queueTitle = computed(() => t('workbench.table.queueTitle', { title: props.title }))
</script>

<template>
  <div class="workbench-view" :data-testid="testId">
    <PageSection :eyebrow="eyebrow" :title="title" :summary="summary">
      <template #actions>
        <el-tag type="info" effect="plain">{{ t('workbench.tags.shellScaffold') }}</el-tag>
        <el-tag type="success" effect="plain">{{ t('workbench.tags.readyForTask15') }}</el-tag>
      </template>
    </PageSection>

    <FilterForm :title="filterLabel" :reset-disabled="!isDirty" @reset="reset" @submit="() => undefined">
      <el-form-item :label="filterLabel">
        <el-input v-model="filters.query" :placeholder="filterPlaceholder" :data-testid="`${testId}-query-input`" clearable />
      </el-form-item>
    </FilterForm>

    <DataTable :title="queueTitle" :rows="visibleRows" :columns="columns" :empty-text="t('workbench.table.empty')" />
  </div>
</template>

<style scoped>
.workbench-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}
</style>
