<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

import WorkbenchView from './WorkbenchView.vue'

const { t } = useI18n()

const columns = computed(() => [
  { key: 'queue', label: t('workbench.columns.queue'), minWidth: 180 },
  { key: 'owner', label: t('workbench.columns.owner'), minWidth: 160 },
  { key: 'status', label: t('workbench.columns.status'), minWidth: 140 },
  { key: 'priority', label: t('workbench.columns.priority'), minWidth: 120 },
])

const rows = computed(() => [
  {
    queue: t('workbench.rows.leaseApprovals.queue'),
    owner: t('workbench.rows.leaseApprovals.owner'),
    status: t('workbench.rows.leaseApprovals.status'),
    priority: t('workbench.rows.leaseApprovals.priority'),
  },
  {
    queue: t('workbench.rows.invoiceApprovals.queue'),
    owner: t('workbench.rows.invoiceApprovals.owner'),
    status: t('workbench.rows.invoiceApprovals.status'),
    priority: t('workbench.rows.invoiceApprovals.priority'),
  },
  {
    queue: t('workbench.rows.receivables.queue'),
    owner: t('workbench.rows.receivables.owner'),
    status: t('workbench.rows.receivables.status'),
    priority: t('workbench.rows.receivables.priority'),
  },
])
</script>

<template>
  <WorkbenchView
    :eyebrow="t('workbench.page.eyebrow')"
    :title="t('workbench.page.title')"
    :summary="t('workbench.page.summary')"
    :filter-label="t('workbench.filters.title')"
    :filter-placeholder="t('workbench.filters.placeholder')"
    :rows="rows"
    :columns="columns"
    test-id="workbench-view"
  />
</template>
