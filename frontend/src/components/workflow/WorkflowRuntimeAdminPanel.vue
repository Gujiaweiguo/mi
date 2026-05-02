<script setup lang="ts">
import { ElMessage, ElMessageBox } from 'element-plus'
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { FUNCTION_CODES } from '../../auth/permissions'
import {
  approveWorkflow,
  getReminderHistory,
  getWorkflowAuditHistory,
  getWorkflowInstance,
  listWorkflowInstances,
  rejectWorkflow,
  resubmitWorkflow,
  runReminders,
  type AuditEntry,
  type WorkflowInstanceListItem,
} from '../../api/workflow'
import { useFilterForm } from '../../composables/useFilterForm'
import { getErrorMessage } from '../../composables/useErrorMessage'
import { useAuthStore } from '../../stores/auth'
import { formatDate } from '../../utils/format'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type ReminderHistoryItem = {
  id?: number
  outcome?: string | null
  reason_code?: string | null
  created_at?: string | null
  sent_at?: string | null
  evaluated_at?: string | null
}

const { t } = useI18n()
const authStore = useAuthStore()

const instances = ref<WorkflowInstanceListItem[]>([])
const isInstancesLoading = ref(false)
const instanceFeedback = ref<Feedback | null>(null)
const instanceActionId = ref<number | null>(null)
const instanceAction = ref<'approve' | 'reject' | 'resubmit' | null>(null)
const selectedInstance = ref<WorkflowInstanceListItem | null>(null)
const showDetailDrawer = ref(false)
const auditHistory = ref<AuditEntry[]>([])
const reminderHistory = ref<ReminderHistoryItem[]>([])
const isDetailLoading = ref(false)
const isRunningReminders = ref(false)
const activeDrawerSections = ref(['reminders'])

const { filters: instanceFilters } = useFilterForm({
  status: 'pending',
  document_type: '',
  document_id: '',
})

const workflowStatusOptions = computed(() => [
  { label: t('common.statuses.pending'), value: 'pending' },
  { label: t('common.statuses.approved'), value: 'approved' },
  { label: t('common.statuses.rejected'), value: 'rejected' },
  { label: t('workflow.placeholders.allStatuses'), value: '' },
])
const canViewRuntime = computed(() => authStore.canAccess(FUNCTION_CODES.workflowAdmin, 'view'))
const canApproveRuntime = computed(() => authStore.canAccess(FUNCTION_CODES.workflowAdmin, 'approve'))

const resolveWorkflowStatusLabel = (status: string) => {
  switch (status) {
    case 'pending':
      return t('common.statuses.pending')
    case 'approved':
      return t('common.statuses.approved')
    case 'rejected':
      return t('common.statuses.rejected')
    default:
      return status
  }
}

const loadInstances = async (options?: { preserveFeedback?: boolean }) => {
  isInstancesLoading.value = true

  if (!options?.preserveFeedback) {
    instanceFeedback.value = null
  }

  try {
    const response = await listWorkflowInstances({
      status: instanceFilters.status || undefined,
      document_type: instanceFilters.document_type || undefined,
      document_id: instanceFilters.document_id ? Number(instanceFilters.document_id) : undefined,
    })
    instances.value = response.data.instances ?? []
  } catch (error) {
    instances.value = []
    instanceFeedback.value = {
      type: 'error',
      title: t('workflow.errors.instancesUnavailable'),
      description: getErrorMessage(error, t('workflow.errors.unableToLoadInstances')),
    }
  } finally {
    isInstancesLoading.value = false
  }
}

const resolveCurrentNodeCode = (instance: WorkflowInstanceListItem) => {
  if (instance.current_node_code?.trim()) {
    return instance.current_node_code
  }

  if (instance.current_step_order !== null) {
    return t('workflow.defaults.stepPrefix', { step: instance.current_step_order })
  }

  return t('common.emptyValue')
}

const statusTagType = (status: string) => {
  switch (status) {
    case 'pending':
      return 'warning'
    case 'approved':
      return 'success'
    case 'rejected':
      return 'danger'
    default:
      return 'info'
  }
}

const resolveAuditActionLabel = (action: string) => {
  const translationKey = `workflow.actions.${action}`
  const translation = t(translationKey)
  return translation === translationKey ? action : translation
}

const resolveAuditStatusChange = (entry: AuditEntry) => {
  const fromStatus = entry.from_status ? resolveWorkflowStatusLabel(entry.from_status) : t('common.emptyValue')
  const toStatus = entry.to_status ? resolveWorkflowStatusLabel(entry.to_status) : t('common.emptyValue')
  return `${fromStatus} → ${toStatus} · ${entry.from_step_order} → ${entry.to_step_order}`
}

const resolveReminderTimestamp = (entry: ReminderHistoryItem) => entry.sent_at ?? entry.evaluated_at ?? entry.created_at ?? null

const handleInstanceAction = async (action: 'approve' | 'reject' | 'resubmit', instance: WorkflowInstanceListItem) => {
  const requiresPendingState = action === 'approve' || action === 'reject'
  if (requiresPendingState && instance.status !== 'pending') {
    return
  }

  if (action === 'resubmit' && instance.status !== 'rejected') {
    return
  }

  if (!canApproveRuntime.value) {
    return
  }

  instanceActionId.value = instance.id
  instanceAction.value = action
  instanceFeedback.value = null

  try {
    const payload = { idempotency_key: crypto.randomUUID() }
    const completedActionLabel =
      action === 'approve'
        ? t('workflow.feedback.approved')
        : action === 'reject'
          ? t('workflow.feedback.rejected')
          : t('workflow.feedback.resubmitted')

    if (action === 'approve') {
      await approveWorkflow(instance.id, payload)
    } else if (action === 'reject') {
      await rejectWorkflow(instance.id, payload)
    } else {
      await resubmitWorkflow(instance.id, payload)
    }

    await loadInstances({ preserveFeedback: true })
    instanceFeedback.value = {
      type: 'success',
      title: t('workflow.feedback.instanceActioned', { action: completedActionLabel }),
      description: t('workflow.feedback.instanceActionedDescription', { id: instance.id, action: completedActionLabel }),
    }

    if (action === 'resubmit') {
      ElMessage.success(t('workflow.feedback.resubmitted'))
    }
  } catch (error) {
    const description = getErrorMessage(error, t('workflow.errors.unableToAction', { id: instance.id, action: t(`workflow.actions.${action}`) }))

    instanceFeedback.value = {
      type: 'error',
      title: t('workflow.errors.actionFailed', { action: t(`workflow.actions.${action}`) }),
      description,
    }

    if (action === 'resubmit') {
      ElMessage.error(description)
    }
  } finally {
    instanceActionId.value = null
    instanceAction.value = null
  }
}

const openDetailDrawer = async (instance: WorkflowInstanceListItem) => {
  selectedInstance.value = instance
  showDetailDrawer.value = true
  isDetailLoading.value = true
  auditHistory.value = []
  reminderHistory.value = []

  try {
    const [instanceResponse, auditResponse, reminderResponse] = await Promise.all([
      getWorkflowInstance(instance.id),
      getWorkflowAuditHistory(instance.id),
      getReminderHistory(instance.id),
    ])

    selectedInstance.value = {
      ...instance,
      ...instanceResponse.data.instance,
      current_node_code: instance.current_node_code ?? null,
      created_at: instance.created_at ?? instanceResponse.data.instance.submitted_at,
    }
    auditHistory.value = auditResponse.data.history ?? []
    reminderHistory.value = (reminderResponse.data.reminders ?? []) as ReminderHistoryItem[]
  } catch (error) {
    ElMessage.error(getErrorMessage(error, t('workflow.errors.unableToLoadDetail')))
  } finally {
    isDetailLoading.value = false
  }
}

const handleRunReminders = async () => {
  if (isRunningReminders.value) {
    return
  }

  if (!canApproveRuntime.value) {
    return
  }

  try {
    await ElMessageBox.confirm(
      t('workflow.confirmations.runRemindersDescription'),
      t('workflow.confirmations.runRemindersTitle'),
      {
        type: 'warning',
        confirmButtonText: t('workflow.actions.runReminders'),
        cancelButtonText: t('common.actions.cancel'),
      },
    )
  } catch {
    return
  }

  isRunningReminders.value = true

  try {
    await runReminders()
    await loadInstances({ preserveFeedback: true })
    instanceFeedback.value = {
      type: 'success',
      title: t('workflow.feedback.remindersTriggered'),
      description: t('workflow.feedback.remindersTriggeredDescription'),
    }
    ElMessage.success(t('workflow.feedback.remindersTriggeredDescription'))
  } catch (error) {
    const description = getErrorMessage(error, t('workflow.errors.reminderFailedDescription'))

    instanceFeedback.value = {
      type: 'error',
      title: t('workflow.errors.reminderFailed'),
      description,
    }
    ElMessage.error(description)
  } finally {
    isRunningReminders.value = false
  }
}

onMounted(() => {
  void loadInstances()
})
</script>

<template>
  <div v-if="canViewRuntime" class="workflow-runtime-admin-panel" data-testid="workflow-runtime-admin-panel">
    <el-alert
      v-if="instanceFeedback"
      :closable="false"
      class="workflow-runtime-admin-panel__alert"
      :title="instanceFeedback.title"
      :type="instanceFeedback.type"
      show-icon
      :description="instanceFeedback.description"
    />

    <el-card class="workflow-runtime-admin-panel__table-card" shadow="never">
      <template #header>
        <div class="workflow-runtime-admin-panel__table-header workflow-runtime-admin-panel__table-header--actions">
          <div class="workflow-runtime-admin-panel__table-heading">
            <span>{{ t('workflow.table.instancesTitle') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: instances.length }) }}</el-tag>
          </div>

          <div class="workflow-runtime-admin-panel__table-controls">
            <el-button
              :loading="isRunningReminders"
              :disabled="isInstancesLoading || isRunningReminders || !canApproveRuntime"
              data-testid="workflow-run-reminders-button"
              @click="handleRunReminders"
            >
              {{ t('workflow.actions.runReminders') }}
            </el-button>

            <el-select
              v-model="instanceFilters.status"
              class="workflow-runtime-admin-panel__status-filter"
              :placeholder="t('workflow.placeholders.filterStatus')"
              data-testid="workflow-status-filter"
            >
              <el-option
                v-for="option in workflowStatusOptions"
                :key="option.value || 'all-statuses'"
                :label="option.label"
                :value="option.value"
              />
            </el-select>

            <el-button
              :loading="isInstancesLoading"
              data-testid="workflow-instances-refresh-button"
              @click="loadInstances"
            >
              {{ t('common.actions.refresh') }}
            </el-button>
          </div>
        </div>
      </template>

      <el-table
        :data="instances"
        row-key="id"
        v-loading="isInstancesLoading"
        class="workflow-runtime-admin-panel__table"
        :empty-text="t('workflow.table.instancesEmpty')"
        data-testid="workflow-instances-table"
      >
        <el-table-column prop="id" :label="t('workflow.columns.id')" min-width="90" />
        <el-table-column prop="document_type" :label="t('workflow.columns.documentType')" min-width="180" />
        <el-table-column prop="document_id" :label="t('workflow.columns.documentId')" min-width="120" />
        <el-table-column :label="t('common.columns.status')" min-width="130">
          <template #default="scope">
            <el-tag :type="statusTagType(scope.row.status)" effect="plain">
              {{ resolveWorkflowStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('workflow.columns.currentNode')" min-width="180">
          <template #default="scope">
            {{ resolveCurrentNodeCode(scope.row) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.createdAt')" min-width="200">
          <template #default="scope">
            {{ formatDate(scope.row.created_at ?? scope.row.submitted_at) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="320" fixed="right">
          <template #default="scope">
            <div class="workflow-runtime-admin-panel__row-actions">
              <el-button
                link
                type="primary"
                :data-testid="`workflow-detail-button-${scope.row.id}`"
                @click="openDetailDrawer(scope.row)"
              >
                {{ t('workflow.actions.details') }}
              </el-button>

              <el-button
                link
                type="primary"
                :loading="instanceActionId === scope.row.id && instanceAction === 'approve'"
                :disabled="isInstancesLoading || instanceActionId !== null || scope.row.status !== 'pending' || !canApproveRuntime"
                :data-testid="`workflow-approve-button-${scope.row.id}`"
                @click="handleInstanceAction('approve', scope.row)"
              >
                {{ t('workflow.actions.approve') }}
              </el-button>

              <el-button
                link
                type="danger"
                :loading="instanceActionId === scope.row.id && instanceAction === 'reject'"
                :disabled="isInstancesLoading || instanceActionId !== null || scope.row.status !== 'pending' || !canApproveRuntime"
                :data-testid="`workflow-reject-button-${scope.row.id}`"
                @click="handleInstanceAction('reject', scope.row)"
              >
                {{ t('workflow.actions.reject') }}
              </el-button>

              <el-button
                v-if="scope.row.status === 'rejected'"
                link
                type="warning"
                :loading="instanceActionId === scope.row.id && instanceAction === 'resubmit'"
                :disabled="isInstancesLoading || instanceActionId !== null || scope.row.status !== 'rejected' || !canApproveRuntime"
                :data-testid="`workflow-resubmit-button-${scope.row.id}`"
                @click="handleInstanceAction('resubmit', scope.row)"
              >
                {{ t('workflow.actions.resubmit') }}
              </el-button>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-drawer
      v-model="showDetailDrawer"
      :title="t('workflow.detail.title')"
      direction="rtl"
      size="480px"
      data-testid="workflow-detail-drawer"
    >
      <el-skeleton :loading="isDetailLoading" animated>
        <template #template>
          <el-skeleton-item variant="p" style="width: 100%; height: 6rem" />
          <el-skeleton-item variant="p" style="width: 100%; height: 10rem; margin-top: 1rem" />
          <el-skeleton-item variant="p" style="width: 100%; height: 8rem; margin-top: 1rem" />
        </template>

        <template #default>
          <div v-if="selectedInstance" class="workflow-runtime-admin-panel__drawer-body">
            <section class="workflow-runtime-admin-panel__detail-summary">
              <div class="workflow-runtime-admin-panel__detail-summary-header">
                <span>{{ selectedInstance.document_type }}</span>
                <el-tag :type="statusTagType(selectedInstance.status)" effect="plain">
                  {{ resolveWorkflowStatusLabel(selectedInstance.status) }}
                </el-tag>
              </div>

              <dl class="workflow-runtime-admin-panel__detail-grid">
                <div class="workflow-runtime-admin-panel__detail-item">
                  <dt>{{ t('workflow.detail.documentType') }}</dt>
                  <dd>{{ selectedInstance.document_type }}</dd>
                </div>
                <div class="workflow-runtime-admin-panel__detail-item">
                  <dt>{{ t('workflow.detail.documentId') }}</dt>
                  <dd>{{ selectedInstance.document_id }}</dd>
                </div>
                <div class="workflow-runtime-admin-panel__detail-item">
                  <dt>{{ t('common.columns.status') }}</dt>
                  <dd>{{ resolveWorkflowStatusLabel(selectedInstance.status) }}</dd>
                </div>
                <div class="workflow-runtime-admin-panel__detail-item">
                  <dt>{{ t('workflow.columns.currentNode') }}</dt>
                  <dd data-testid="workflow-detail-current-node">{{ resolveCurrentNodeCode(selectedInstance) }}</dd>
                </div>
                <div class="workflow-runtime-admin-panel__detail-item">
                  <dt>{{ t('workflow.detail.submittedAt') }}</dt>
                  <dd>{{ formatDate(selectedInstance.submitted_at) }}</dd>
                </div>
                <div class="workflow-runtime-admin-panel__detail-item">
                  <dt>{{ t('workflow.detail.completedAt') }}</dt>
                  <dd>{{ formatDate(selectedInstance.completed_at) }}</dd>
                </div>
              </dl>
            </section>

            <section class="workflow-runtime-admin-panel__detail-section">
              <div class="workflow-runtime-admin-panel__detail-section-header">
                <span>{{ t('workflow.detail.auditHistory') }}</span>
                <el-tag effect="plain" type="info">{{ t('common.total', { count: auditHistory.length }) }}</el-tag>
              </div>

              <div v-if="!auditHistory.length" class="workflow-runtime-admin-panel__detail-empty" data-testid="workflow-detail-empty-audit">
                {{ t('workflow.detail.noAuditHistory') }}
              </div>

              <el-timeline v-else>
                <el-timeline-item
                  v-for="entry in auditHistory"
                  :key="entry.id"
                  :timestamp="formatDate(entry.created_at)"
                  placement="top"
                  :type="statusTagType(entry.to_status)"
                >
                  <div :data-testid="`workflow-detail-audit-${entry.id}`" class="workflow-runtime-admin-panel__timeline-card">
                    <div class="workflow-runtime-admin-panel__timeline-row">
                      <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.auditAction') }}</span>
                      <span>{{ resolveAuditActionLabel(entry.action) }}</span>
                    </div>
                    <div class="workflow-runtime-admin-panel__timeline-row">
                      <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.auditActor') }}</span>
                      <span>#{{ entry.actor_user_id }}</span>
                    </div>
                    <div class="workflow-runtime-admin-panel__timeline-row">
                      <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.auditStatusChange') }}</span>
                      <span>{{ resolveAuditStatusChange(entry) }}</span>
                    </div>
                    <div class="workflow-runtime-admin-panel__timeline-row">
                      <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.auditComment') }}</span>
                      <span>{{ entry.comment || t('common.emptyValue') }}</span>
                    </div>
                  </div>
                </el-timeline-item>
              </el-timeline>
            </section>

            <section class="workflow-runtime-admin-panel__detail-section">
              <el-collapse v-model="activeDrawerSections">
                <el-collapse-item :title="t('workflow.detail.reminderHistory')" name="reminders">
                  <div
                    v-if="!reminderHistory.length"
                    class="workflow-runtime-admin-panel__detail-empty"
                    data-testid="workflow-detail-empty-reminders"
                  >
                    {{ t('workflow.detail.noReminderHistory') }}
                  </div>

                  <ul v-else class="workflow-runtime-admin-panel__reminder-list">
                    <li
                      v-for="(entry, index) in reminderHistory"
                      :key="entry.id ?? `${resolveReminderTimestamp(entry)}-${index}`"
                      :data-testid="`workflow-detail-reminder-${entry.id ?? index}`"
                      class="workflow-runtime-admin-panel__reminder-item"
                    >
                      <div class="workflow-runtime-admin-panel__timeline-row">
                        <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.reminderOutcome') }}</span>
                        <span>{{ entry.outcome || t('common.emptyValue') }}</span>
                      </div>
                      <div class="workflow-runtime-admin-panel__timeline-row">
                        <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.reminderReasonCode') }}</span>
                        <span>{{ entry.reason_code || t('common.emptyValue') }}</span>
                      </div>
                      <div class="workflow-runtime-admin-panel__timeline-row">
                        <span class="workflow-runtime-admin-panel__timeline-label">{{ t('workflow.detail.reminderTimestamp') }}</span>
                        <span>{{ formatDate(resolveReminderTimestamp(entry)) }}</span>
                      </div>
                    </li>
                  </ul>
                </el-collapse-item>
              </el-collapse>
            </section>
          </div>
        </template>
      </el-skeleton>
    </el-drawer>
  </div>
</template>

<style scoped>
.workflow-runtime-admin-panel {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.workflow-runtime-admin-panel__alert {
  margin-bottom: 0;
}

.workflow-runtime-admin-panel__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.workflow-runtime-admin-panel__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.workflow-runtime-admin-panel__table-header--actions {
  flex-wrap: wrap;
}

.workflow-runtime-admin-panel__table-heading,
.workflow-runtime-admin-panel__table-controls,
.workflow-runtime-admin-panel__row-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.workflow-runtime-admin-panel__table-controls {
  flex-wrap: wrap;
  justify-content: flex-end;
}

.workflow-runtime-admin-panel__status-filter {
  min-width: 12rem;
}

.workflow-runtime-admin-panel__table {
  width: 100%;
}

.workflow-runtime-admin-panel__drawer-body,
.workflow-runtime-admin-panel__detail-section,
.workflow-runtime-admin-panel__timeline-card,
.workflow-runtime-admin-panel__reminder-item {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.workflow-runtime-admin-panel__drawer-body {
  gap: var(--mi-space-5);
}

.workflow-runtime-admin-panel__detail-summary,
.workflow-runtime-admin-panel__detail-empty,
.workflow-runtime-admin-panel__timeline-card,
.workflow-runtime-admin-panel__reminder-item {
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-md);
  background: var(--mi-surface-gradient);
}

.workflow-runtime-admin-panel__detail-summary-header,
.workflow-runtime-admin-panel__detail-section-header,
.workflow-runtime-admin-panel__timeline-row {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: var(--mi-space-3);
}

.workflow-runtime-admin-panel__detail-section-header,
.workflow-runtime-admin-panel__detail-summary-header {
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.workflow-runtime-admin-panel__detail-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
  margin: 0;
}

.workflow-runtime-admin-panel__detail-item {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
}

.workflow-runtime-admin-panel__detail-item dt,
.workflow-runtime-admin-panel__timeline-label {
  font-size: var(--mi-font-size-100);
  color: var(--mi-color-muted);
}

.workflow-runtime-admin-panel__detail-item dd {
  margin: 0;
  font-size: var(--mi-font-size-200);
  color: var(--mi-color-text);
}

.workflow-runtime-admin-panel__detail-empty {
  color: var(--mi-color-muted);
}

.workflow-runtime-admin-panel__timeline-row {
  flex-wrap: wrap;
}

.workflow-runtime-admin-panel__reminder-list {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
  margin: 0;
  padding: 0;
  list-style: none;
}

@media (max-width: 52rem) {
  .workflow-runtime-admin-panel__table-header {
    align-items: flex-start;
    flex-direction: column;
  }

  .workflow-runtime-admin-panel__table-controls {
    justify-content: flex-start;
  }

  .workflow-runtime-admin-panel__row-actions {
    flex-wrap: wrap;
  }

  .workflow-runtime-admin-panel__detail-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
