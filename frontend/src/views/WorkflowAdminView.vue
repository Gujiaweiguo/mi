<script setup lang="ts">
import { computed, ref, watchEffect } from 'vue'
import { useI18n } from 'vue-i18n'

import { FUNCTION_CODES } from '../auth/permissions'
import WorkflowDefinitionAdminPanel from '../components/workflow/WorkflowDefinitionAdminPanel.vue'
import WorkflowRuntimeAdminPanel from '../components/workflow/WorkflowRuntimeAdminPanel.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useAuthStore } from '../stores/auth'

const { t } = useI18n()
const authStore = useAuthStore()

const activeTab = ref<'definitions' | 'runtime'>('definitions')
const canAccessDefinitions = computed(() => authStore.canAccess(FUNCTION_CODES.workflowDefinition, 'view'))
const canAccessRuntime = computed(() => authStore.canAccess(FUNCTION_CODES.workflowAdmin, 'view'))

watchEffect(() => {
  if (canAccessDefinitions.value) {
    if (activeTab.value !== 'definitions' && !canAccessRuntime.value) {
      activeTab.value = 'definitions'
    }
    return
  }

  if (canAccessRuntime.value) {
    activeTab.value = 'runtime'
  }
})
</script>

<template>
  <div class="workflow-admin-view" data-testid="workflow-admin-view">
    <PageSection
      :eyebrow="t('workflow.eyebrow')"
      :title="t('workflow.title')"
      :summary="t('workflow.summary')"
    />

    <el-card class="workflow-admin-view__tabs-card" shadow="never">
      <el-tabs v-model="activeTab" class="workflow-admin-view__tabs" data-testid="workflow-admin-tabs">
        <el-tab-pane v-if="canAccessDefinitions" name="definitions">
          <template #label>
            <span data-testid="workflow-admin-tab-label-definitions">{{ t('workflow.tabs.definitions') }}</span>
          </template>

          <WorkflowDefinitionAdminPanel v-if="activeTab === 'definitions'" />
        </el-tab-pane>

        <el-tab-pane v-if="canAccessRuntime" name="runtime">
          <template #label>
            <span data-testid="workflow-admin-tab-label-runtime">{{ t('workflow.tabs.runtime') }}</span>
          </template>

          <WorkflowRuntimeAdminPanel v-if="activeTab === 'runtime'" />
        </el-tab-pane>
      </el-tabs>
    </el-card>
  </div>
</template>

<style scoped>
.workflow-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.workflow-admin-view__tabs-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

:deep(.workflow-admin-view__tabs .el-tabs__header) {
  margin-bottom: var(--mi-space-5);
}

:deep(.workflow-admin-view__tabs .el-tabs__nav-wrap::after) {
  background-color: var(--mi-color-border);
}
</style>
