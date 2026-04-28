<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  getEmptyWorkbenchAggregate,
  getWorkbenchAggregate,
  type WorkbenchAggregate,
} from '../api/dashboard'
import { getErrorMessage } from '../composables/useErrorMessage'
import { useAppStore } from '../stores/app'
import WorkbenchView from './WorkbenchView.vue'

const { t } = useI18n()
const appStore = useAppStore()

const aggregate = ref<WorkbenchAggregate>(getEmptyWorkbenchAggregate())
const isLoading = ref(false)
const errorMessage = ref('')
const lastUpdatedAt = ref(t('common.notCheckedYet'))

const formatTimestamp = (value: Date) =>
  new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(value)

const loadWorkbench = async () => {
  isLoading.value = true
  errorMessage.value = ''

  try {
    aggregate.value = await getWorkbenchAggregate()
  } catch (error) {
    aggregate.value = getEmptyWorkbenchAggregate()
    errorMessage.value = getErrorMessage(error, t('workbench.errors.unableToLoad'))
  } finally {
    lastUpdatedAt.value = formatTimestamp(new Date())
    isLoading.value = false
  }
}

const sections = computed(() => [
  {
    key: 'pending-approvals',
    title: t('workbench.sections.pendingApprovals.title'),
    summary: t('workbench.sections.pendingApprovals.summary'),
    count: aggregate.value.pendingApprovals.count,
    routeTarget: aggregate.value.pendingApprovals.routeTarget,
    previewRows: aggregate.value.pendingApprovals.previewRows,
  },
  {
    key: 'receivables',
    title: t('workbench.sections.receivables.title'),
    summary: t('workbench.sections.receivables.summary'),
    count: aggregate.value.receivables.count,
    routeTarget: aggregate.value.receivables.routeTarget,
    previewRows: aggregate.value.receivables.previewRows,
  },
  {
    key: 'overdue-receivables',
    title: t('workbench.sections.overdueReceivables.title'),
    summary: t('workbench.sections.overdueReceivables.summary'),
    count: aggregate.value.overdueReceivables.count,
    routeTarget: aggregate.value.overdueReceivables.routeTarget,
    previewRows: aggregate.value.overdueReceivables.previewRows,
  },
  {
    key: 'active-leases',
    title: t('workbench.sections.activeLeases.title'),
    summary: t('workbench.sections.activeLeases.summary'),
    count: aggregate.value.activeLeases.count,
    routeTarget: aggregate.value.activeLeases.routeTarget,
    previewRows: aggregate.value.activeLeases.previewRows,
  },
])

onMounted(() => {
  void loadWorkbench()
})
</script>

<template>
  <div class="workbench-page-view" v-loading="isLoading" data-testid="workbench-page-view">
    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="workbench-page-view__alert"
      show-icon
      :title="t('workbench.alerts.dataUnavailable')"
      type="error"
      :description="errorMessage"
    />

    <WorkbenchView
      :eyebrow="t('workbench.page.eyebrow')"
      :title="t('workbench.page.title')"
      :summary="t('workbench.page.summary')"
      :sections="sections"
      :is-loading="isLoading"
      :last-updated-at="lastUpdatedAt"
      test-id="workbench-view"
      @refresh="loadWorkbench"
    />
  </div>
</template>

<style scoped>
.workbench-page-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}
</style>
