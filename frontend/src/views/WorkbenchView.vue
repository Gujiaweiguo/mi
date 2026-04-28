<script setup lang="ts">
import { useRouter } from 'vue-router'

import type { WorkbenchPreviewRow } from '../api/dashboard'
import PageSection from '../components/platform/PageSection.vue'

type WorkbenchSection = {
  key: string
  title: string
  summary: string
  count: number | null
  routeTarget: string
  previewRows: WorkbenchPreviewRow[]
}

const props = defineProps<{
  eyebrow: string
  title: string
  summary: string
  sections: WorkbenchSection[]
  isLoading: boolean
  lastUpdatedAt: string
  testId: string
}>()

const emit = defineEmits<{
  refresh: []
}>()

const router = useRouter()

const navigateTo = async (path: string) => {
  await router.push(path)
}
</script>

<template>
  <div class="workbench-view" :data-testid="testId">
    <PageSection :eyebrow="eyebrow" :title="title" :summary="summary">
      <template #actions>
        <div class="workbench-view__hero-actions">
          <el-tag effect="plain" type="info">{{ $t('workbench.actions.lastUpdated', { value: lastUpdatedAt }) }}</el-tag>
          <el-button type="primary" plain :loading="isLoading" @click="emit('refresh')">
            {{ $t('workbench.actions.refresh') }}
          </el-button>
        </div>
      </template>
    </PageSection>

    <section class="workbench-view__sections">
      <el-card
        v-for="section in sections"
        :key="section.key"
        class="workbench-view__section-card"
        shadow="never"
        :data-testid="`${testId}-${section.key}`"
      >
        <div class="workbench-view__section-header">
          <div>
            <h2 class="workbench-view__section-title">{{ section.title }}</h2>
            <p class="workbench-view__section-summary">{{ section.summary }}</p>
          </div>
          <div class="workbench-view__section-metrics">
            <el-statistic v-if="section.count !== null" :value="section.count" />
            <span v-else class="workbench-view__unavailable">{{ $t('common.notAvailable') }}</span>
            <el-button link type="primary" @click="navigateTo(section.routeTarget)">
              {{ $t('workbench.actions.openQueue') }}
            </el-button>
          </div>
        </div>

        <div v-if="section.previewRows.length" class="workbench-view__preview-list">
          <button
            v-for="row in section.previewRows"
            :key="`${section.key}-${row.id}`"
            type="button"
            class="workbench-view__preview-item"
            @click="navigateTo(row.routeTarget)"
          >
            <div class="workbench-view__preview-copy">
              <span class="workbench-view__preview-title">{{ row.title }}</span>
              <span class="workbench-view__preview-subtitle">{{ row.subtitle }}</span>
            </div>
            <div class="workbench-view__preview-meta">
              <el-tag size="small" effect="plain">{{ row.status }}</el-tag>
              <span>{{ row.meta }}</span>
            </div>
          </button>
        </div>

        <div v-else class="workbench-view__empty" :data-testid="`${testId}-${section.key}-empty`">
          {{ $t('workbench.empty.section') }}
        </div>
      </el-card>
    </section>
  </div>
</template>

<style scoped>
.workbench-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.workbench-view__hero-actions {
  display: flex;
  gap: var(--mi-space-3);
  align-items: center;
}

.workbench-view__sections {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: var(--mi-space-4);
}

.workbench-view__section-card {
  border: 1px solid var(--mi-border-color, #dcdfe6);
}

.workbench-view__section-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: var(--mi-space-4);
  margin-bottom: var(--mi-space-4);
}

.workbench-view__section-title {
  margin: 0;
  font-size: 1rem;
}

.workbench-view__section-summary {
  margin: var(--mi-space-1) 0 0;
  color: var(--mi-text-secondary, #606266);
}

.workbench-view__section-metrics {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: var(--mi-space-2);
}

.workbench-view__preview-list {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.workbench-view__preview-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  border: 1px solid var(--mi-border-color, #ebeef5);
  border-radius: 10px;
  background: #fff;
  padding: var(--mi-space-3);
  text-align: left;
  cursor: pointer;
}

.workbench-view__preview-copy,
.workbench-view__preview-meta {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
}

.workbench-view__preview-title {
  font-weight: 600;
}

.workbench-view__preview-subtitle,
.workbench-view__preview-meta,
.workbench-view__empty,
.workbench-view__unavailable {
  color: var(--mi-text-secondary, #606266);
}

.workbench-view__empty {
  border: 1px dashed var(--mi-border-color, #dcdfe6);
  border-radius: 10px;
  padding: var(--mi-space-4);
}
@media (max-width: 768px) {
  .workbench-view__section-header,
  .workbench-view__preview-item {
    flex-direction: column;
    align-items: flex-start;
  }

  .workbench-view__section-metrics {
    align-items: flex-start;
  }
}
</style>
