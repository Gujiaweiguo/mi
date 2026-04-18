<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

import { getDashboardSummary, getEmptyDashboardSummary, type DashboardSummary } from '../api/dashboard'
import { FUNCTION_CODES, canAccessFunction, type FunctionCode } from '../auth/permissions'
import PageSection from '../components/platform/PageSection.vue'
import { getErrorMessage } from '../composables/useErrorMessage'
import { useAppStore } from '../stores/app'
import { useAuthStore } from '../stores/auth'

type NavigationAction = {
  title: string
  summary: string
  path: string
  functionCode?: FunctionCode
}

type SummaryCard = {
  key: keyof DashboardSummary
  label: string
  description: string
  value: number | null
  path: string
  functionCode?: FunctionCode
  tone: 'primary' | 'accent' | 'warning' | 'success' | 'danger' | 'info'
}

type QueueRow = {
  label: string
  value: number | null
  path: string
  functionCode?: FunctionCode
}

const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()
const authStore = useAuthStore()

const summary = ref<DashboardSummary>(getEmptyDashboardSummary())
const isLoading = ref(false)
const errorMessage = ref('')
const lastUpdatedAt = ref(t('common.notCheckedYet'))

const formatTimestamp = (value: Date) =>
  new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(value)

const canOpen = (functionCode?: FunctionCode) => canAccessFunction(authStore.sessionUser?.permissions, functionCode)

const navigateTo = async (path: string) => {
  await router.push(path)
}

const loadDashboard = async () => {
  isLoading.value = true
  errorMessage.value = ''

  try {
    summary.value = await getDashboardSummary()
  } catch (error) {
    summary.value = getEmptyDashboardSummary()
    errorMessage.value = getErrorMessage(error, t('dashboard.errors.unableToLoad'))
  } finally {
    lastUpdatedAt.value = formatTimestamp(new Date())
    isLoading.value = false
  }
}

const summaryCards = computed<SummaryCard[]>(() => [
  {
    key: 'activeLeases',
    label: t('dashboard.cards.activeLeases'),
    description: t('dashboard.cardDescriptions.activeLeases'),
    value: summary.value.activeLeases,
    path: '/lease/contracts',
    functionCode: FUNCTION_CODES.leaseContract,
    tone: 'success',
  },
  {
    key: 'pendingLeaseApprovals',
    label: t('dashboard.cards.pendingLeaseApprovals'),
    description: t('dashboard.cardDescriptions.pendingLeaseApprovals'),
    value: summary.value.pendingLeaseApprovals,
    path: '/lease/contracts',
    functionCode: FUNCTION_CODES.leaseContract,
    tone: 'warning',
  },
  {
    key: 'pendingInvoiceApprovals',
    label: t('dashboard.cards.pendingInvoiceApprovals'),
    description: t('dashboard.cardDescriptions.pendingInvoiceApprovals'),
    value: summary.value.pendingInvoiceApprovals,
    path: '/billing/invoices',
    functionCode: FUNCTION_CODES.billingInvoice,
    tone: 'primary',
  },
  {
    key: 'openReceivables',
    label: t('dashboard.cards.openReceivables'),
    description: t('dashboard.cardDescriptions.openReceivables'),
    value: summary.value.openReceivables,
    path: '/billing/receivables',
    functionCode: FUNCTION_CODES.billingInvoice,
    tone: 'accent',
  },
  {
    key: 'overdueReceivables',
    label: t('dashboard.cards.overdueReceivables'),
    description: t('dashboard.cardDescriptions.overdueReceivables'),
    value: summary.value.overdueReceivables,
    path: '/billing/receivables',
    functionCode: FUNCTION_CODES.billingInvoice,
    tone: 'danger',
  },
  {
    key: 'pendingWorkflows',
    label: t('dashboard.cards.pendingWorkflows'),
    description: t('dashboard.cardDescriptions.pendingWorkflows'),
    value: summary.value.pendingWorkflows,
    path: '/workflow/admin',
    functionCode: FUNCTION_CODES.workflowAdmin,
    tone: 'info',
  },
])

const quickActions = computed<NavigationAction[]>(() => [
  {
    title: t('dashboard.quickActions.leaseContracts.title'),
    summary: t('dashboard.quickActions.leaseContracts.summary'),
    path: '/lease/contracts',
    functionCode: FUNCTION_CODES.leaseContract,
  },
  {
    title: t('dashboard.quickActions.billingInvoices.title'),
    summary: t('dashboard.quickActions.billingInvoices.summary'),
    path: '/billing/invoices',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
  {
    title: t('dashboard.quickActions.receivables.title'),
    summary: t('dashboard.quickActions.receivables.summary'),
    path: '/billing/receivables',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
  {
    title: t('dashboard.quickActions.workflowAdmin.title'),
    summary: t('dashboard.quickActions.workflowAdmin.summary'),
    path: '/workflow/admin',
    functionCode: FUNCTION_CODES.workflowAdmin,
  },
])

const approvalRows = computed<QueueRow[]>(() => [
  {
    label: t('dashboard.queueRows.pendingLeaseApprovals'),
    value: summary.value.pendingLeaseApprovals,
    path: '/lease/contracts',
    functionCode: FUNCTION_CODES.leaseContract,
  },
  {
    label: t('dashboard.queueRows.pendingInvoiceApprovals'),
    value: summary.value.pendingInvoiceApprovals,
    path: '/billing/invoices',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
])

const collectionRows = computed<QueueRow[]>(() => [
  {
    label: t('dashboard.queueRows.openReceivables'),
    value: summary.value.openReceivables,
    path: '/billing/receivables',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
  {
    label: t('dashboard.queueRows.overdueReceivables'),
    value: summary.value.overdueReceivables,
    path: '/billing/receivables',
    functionCode: FUNCTION_CODES.billingInvoice,
  },
])

onMounted(() => {
  void loadDashboard()
})
</script>

<template>
  <div class="dashboard-view" v-loading="isLoading" data-testid="dashboard-view">
    <PageSection
      :eyebrow="t('dashboard.eyebrow')"
      :title="t('dashboard.title')"
      :summary="t('dashboard.summary')"
    >
      <template #actions>
        <div class="dashboard-view__hero-actions">
          <el-tag effect="plain" type="info">{{ t('dashboard.lastUpdated', { value: lastUpdatedAt }) }}</el-tag>
          <el-button type="primary" plain :loading="isLoading" @click="loadDashboard">
            {{ t('dashboard.actions.refresh') }}
          </el-button>
        </div>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="dashboard-view__alert"
      show-icon
      :title="t('dashboard.alerts.dataUnavailable')"
      type="error"
      :description="errorMessage"
    />

    <section class="dashboard-view__section">
      <div class="dashboard-view__section-heading">
        <h2 class="dashboard-view__section-title">{{ t('dashboard.sections.summaryCards') }}</h2>
        <p class="dashboard-view__section-summary">{{ t('dashboard.sections.summaryCardsSummary') }}</p>
      </div>

      <div class="dashboard-view__summary-grid">
        <el-card
          v-for="card in summaryCards"
          :key="card.key"
          class="dashboard-view__summary-card"
          :class="`dashboard-view__summary-card--${card.tone}`"
          shadow="never"
        >
          <div class="dashboard-view__card-content">
            <div class="dashboard-view__card-header">
              <span class="dashboard-view__card-label">{{ card.label }}</span>
            </div>

            <p class="dashboard-view__card-description">{{ card.description }}</p>

            <div class="dashboard-view__card-value">
              <el-statistic v-if="card.value !== null" :value="card.value" />
              <span v-else class="dashboard-view__unavailable">{{ t('common.notAvailable') }}</span>
            </div>

            <div class="dashboard-view__card-footer">
              <el-button link type="primary" :disabled="!canOpen(card.functionCode)" @click="navigateTo(card.path)">
                {{ t('dashboard.actions.reviewQueue') }}
              </el-button>
            </div>
          </div>
        </el-card>
      </div>
    </section>

    <section class="dashboard-view__section">
      <div class="dashboard-view__section-heading">
        <h2 class="dashboard-view__section-title">{{ t('dashboard.sections.quickActions') }}</h2>
        <p class="dashboard-view__section-summary">{{ t('dashboard.sections.quickActionsSummary') }}</p>
      </div>

      <div class="dashboard-view__quick-grid">
        <el-card v-for="action in quickActions" :key="action.path" class="dashboard-view__quick-card" shadow="never">
          <div class="dashboard-view__quick-card-content">
            <div class="dashboard-view__quick-card-copy">
              <h3 class="dashboard-view__quick-card-title">{{ action.title }}</h3>
              <p class="dashboard-view__quick-card-summary">{{ action.summary }}</p>
            </div>

            <el-button type="primary" plain :disabled="!canOpen(action.functionCode)" @click="navigateTo(action.path)">
              {{ t('dashboard.actions.openWorkspace') }}
            </el-button>
          </div>
        </el-card>
      </div>
    </section>

    <section class="dashboard-view__section">
      <div class="dashboard-view__section-heading">
        <h2 class="dashboard-view__section-title">{{ t('dashboard.sections.priorityQueues') }}</h2>
        <p class="dashboard-view__section-summary">{{ t('dashboard.sections.priorityQueuesSummary') }}</p>
      </div>

      <div class="dashboard-view__panel-grid">
        <el-card class="dashboard-view__panel-card" shadow="never">
          <template #header>
            <div class="dashboard-view__panel-header">
              <span>{{ t('dashboard.panels.approvals.title') }}</span>
            </div>
          </template>

          <p class="dashboard-view__panel-summary">{{ t('dashboard.panels.approvals.summary') }}</p>

          <div class="dashboard-view__queue-list">
            <div v-for="row in approvalRows" :key="row.label" class="dashboard-view__queue-row">
              <div class="dashboard-view__queue-copy">
                <span class="dashboard-view__queue-label">{{ row.label }}</span>
                <span class="dashboard-view__queue-value">{{ row.value ?? t('common.notAvailable') }}</span>
              </div>

              <el-button link type="primary" :disabled="!canOpen(row.functionCode)" @click="navigateTo(row.path)">
                {{ t('dashboard.actions.reviewQueue') }}
              </el-button>
            </div>
          </div>
        </el-card>

        <el-card class="dashboard-view__panel-card" shadow="never">
          <template #header>
            <div class="dashboard-view__panel-header">
              <span>{{ t('dashboard.panels.collections.title') }}</span>
            </div>
          </template>

          <p class="dashboard-view__panel-summary">{{ t('dashboard.panels.collections.summary') }}</p>

          <div class="dashboard-view__queue-list">
            <div v-for="row in collectionRows" :key="row.label" class="dashboard-view__queue-row">
              <div class="dashboard-view__queue-copy">
                <span class="dashboard-view__queue-label">{{ row.label }}</span>
                <span class="dashboard-view__queue-value">{{ row.value ?? t('common.notAvailable') }}</span>
              </div>

              <el-button link type="primary" :disabled="!canOpen(row.functionCode)" @click="navigateTo(row.path)">
                {{ t('dashboard.actions.reviewQueue') }}
              </el-button>
            </div>
          </div>
        </el-card>
      </div>
    </section>
  </div>
</template>

<style scoped>
.dashboard-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-6);
}

.dashboard-view__hero-actions {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: var(--mi-space-3);
}

.dashboard-view__alert {
  margin-bottom: 0;
}

.dashboard-view__section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.dashboard-view__section-heading {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-2);
}

.dashboard-view__section-title {
  margin: 0;
  font-size: var(--mi-font-size-400);
  line-height: var(--mi-line-height-tight);
  color: var(--mi-color-text);
}

.dashboard-view__section-summary {
  margin: 0;
  font-size: var(--mi-font-size-200);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
}

.dashboard-view__summary-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.dashboard-view__summary-card,
.dashboard-view__quick-card,
.dashboard-view__panel-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.dashboard-view__summary-card {
  --dashboard-card-accent: var(--mi-color-primary);
  overflow: hidden;
}

.dashboard-view__summary-card--primary {
  --dashboard-card-accent: var(--mi-color-primary);
}

.dashboard-view__summary-card--accent {
  --dashboard-card-accent: var(--mi-color-accent);
}

.dashboard-view__summary-card--warning {
  --dashboard-card-accent: var(--mi-color-warning);
}

.dashboard-view__summary-card--success {
  --dashboard-card-accent: var(--mi-color-success);
}

.dashboard-view__summary-card--danger {
  --dashboard-card-accent: var(--mi-color-danger);
}

.dashboard-view__summary-card--info {
  --dashboard-card-accent: var(--mi-color-info);
}

.dashboard-view__summary-card :deep(.el-card__body) {
  padding: 0;
}

.dashboard-view__card-content {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  height: 100%;
  padding: var(--mi-space-5);
  border-top: var(--mi-space-1) solid var(--dashboard-card-accent);
}

.dashboard-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
}

.dashboard-view__card-label {
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.dashboard-view__card-description,
.dashboard-view__panel-summary,
.dashboard-view__quick-card-summary {
  margin: 0;
  font-size: var(--mi-font-size-200);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
}

.dashboard-view__card-value {
  min-height: calc(var(--mi-font-size-600) + var(--mi-space-3));
}

.dashboard-view__card-value :deep(.el-statistic__head) {
  display: none;
}

.dashboard-view__card-value :deep(.el-statistic__content) {
  font-size: var(--mi-font-size-600);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--dashboard-card-accent);
}

.dashboard-view__unavailable {
  display: inline-flex;
  align-items: center;
  min-height: calc(var(--mi-font-size-600) + var(--mi-space-3));
  font-size: var(--mi-font-size-400);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-muted);
}

.dashboard-view__card-footer {
  display: flex;
  justify-content: flex-start;
}

.dashboard-view__quick-grid,
.dashboard-view__panel-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.dashboard-view__quick-card-content {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: var(--mi-space-4);
  min-height: var(--mi-panel-min-height);
}

.dashboard-view__quick-card-copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-2);
}

.dashboard-view__quick-card-title {
  margin: 0;
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.dashboard-view__panel-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.dashboard-view__queue-list {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.dashboard-view__queue-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-4);
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-sm);
  background: var(--mi-color-surface);
}

.dashboard-view__queue-copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
}

.dashboard-view__queue-label {
  font-size: var(--mi-font-size-200);
  color: var(--mi-color-text);
}

.dashboard-view__queue-value {
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-primary);
}

@media (max-width: 52rem) {
  .dashboard-view__hero-actions {
    align-items: flex-start;
  }

  .dashboard-view__summary-grid,
  .dashboard-view__quick-grid,
  .dashboard-view__panel-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .dashboard-view__queue-row {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
