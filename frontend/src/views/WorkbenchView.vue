<script setup lang="ts">
import { computed } from 'vue'

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
</script>

<template>
  <div class="workbench-view" :data-testid="testId">
    <PageSection :eyebrow="eyebrow" :title="title" :summary="summary">
      <template #actions>
        <el-tag type="info" effect="plain">Shell scaffold</el-tag>
        <el-tag type="success" effect="plain">Ready for Task 15</el-tag>
      </template>
    </PageSection>

    <FilterForm :title="filterLabel" :reset-disabled="!isDirty" @reset="reset" @submit="() => undefined">
      <el-form-item :label="filterLabel">
        <el-input v-model="filters.query" :placeholder="filterPlaceholder" :data-testid="`${testId}-query-input`" clearable />
      </el-form-item>
    </FilterForm>

    <DataTable :title="`${title} queue`" :rows="visibleRows" :columns="columns" :empty-text="'No stub records match the current filters.'" />
  </div>
</template>

<style scoped>
.workbench-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}
</style>
