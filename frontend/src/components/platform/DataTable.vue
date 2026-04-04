<script setup lang="ts">
export type DataTableColumn = {
  key: string
  label: string
  minWidth?: number
}

defineProps<{
  title?: string
  rows: Record<string, unknown>[]
  columns: DataTableColumn[]
  emptyText?: string
}>()
</script>

<template>
  <el-card class="data-table" shadow="never">
    <template #header>
      <div class="data-table__header">
        <span>{{ title ?? 'Results' }}</span>
        <el-tag effect="plain" type="info">{{ rows.length }} rows</el-tag>
      </div>
    </template>

    <el-table :data="rows" row-key="id" class="data-table__table" :empty-text="emptyText ?? 'No records found.'">
      <el-table-column
        v-for="column in columns"
        :key="column.key"
        :prop="column.key"
        :label="column.label"
        :min-width="column.minWidth ?? 140"
      />
    </el-table>
  </el-card>
</template>

<style scoped>
.data-table {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.data-table__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.data-table__table {
  width: 100%;
}
</style>
