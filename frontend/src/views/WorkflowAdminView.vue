<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

import {
  approveWorkflow,
  listWorkflowDefinitions,
  listWorkflowInstances,
  rejectWorkflow,
  type WorkflowDefinition,
  type WorkflowInstanceListItem,
} from '../api/workflow'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const definitions = ref<WorkflowDefinition[]>([])
const instances = ref<WorkflowInstanceListItem[]>([])
const isDefinitionsLoading = ref(false)
const isInstancesLoading = ref(false)
const definitionsErrorMessage = ref('')
const instanceFeedback = ref<Feedback | null>(null)
const instanceActionId = ref<number | null>(null)
const instanceAction = ref<'approve' | 'reject' | null>(null)

const { filters, isDirty, reset } = useFilterForm({
  search: '',
})

const { filters: instanceFilters } = useFilterForm({
  status: 'pending',
  document_type: '',
  document_id: '',
})

const workflowStatusOptions = [
  { label: 'Pending', value: 'pending' },
  { label: 'Approved', value: 'approved' },
  { label: 'Rejected', value: 'rejected' },
  { label: 'All statuses', value: '' },
]

const loadDefinitions = async () => {
  isDefinitionsLoading.value = true
  definitionsErrorMessage.value = ''

  try {
    const response = await listWorkflowDefinitions()
    definitions.value = response.data.definitions ?? []
  } catch (error) {
    definitionsErrorMessage.value = error instanceof Error ? error.message : 'Unable to load workflow definitions.'
    definitions.value = []
  } finally {
    isDefinitionsLoading.value = false
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
      title: 'Workflow instances unavailable',
      description: error instanceof Error ? error.message : 'Unable to load workflow instances.',
    }
  } finally {
    isInstancesLoading.value = false
  }
}

const filteredDefinitions = computed(() => {
  const search = filters.search.trim().toLowerCase()
  if (!search) return definitions.value

  return definitions.value.filter(
    (d) =>
      d.Code.toLowerCase().includes(search) ||
      d.Name.toLowerCase().includes(search) ||
      d.ProcessClass.toLowerCase().includes(search),
  )
})

const formatTimestamp = (value: string | null | undefined) => {
  if (!value) {
    return '—'
  }

  return new Intl.DateTimeFormat('en-US', {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value))
}

const resolveCurrentNodeCode = (instance: WorkflowInstanceListItem) => {
  if (instance.current_node_code?.trim()) {
    return instance.current_node_code
  }

  if (instance.current_step_order !== null) {
    return `Step ${instance.current_step_order}`
  }

  return '—'
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

const handleInstanceAction = async (action: 'approve' | 'reject', instance: WorkflowInstanceListItem) => {
  if (instance.status !== 'pending') {
    return
  }

  instanceActionId.value = instance.id
  instanceAction.value = action
  instanceFeedback.value = null

  try {
    const payload = { idempotency_key: crypto.randomUUID() }
    const completedActionLabel = action === 'approve' ? 'approved' : 'rejected'

    if (action === 'approve') {
      await approveWorkflow(instance.id, payload)
    } else {
      await rejectWorkflow(instance.id, payload)
    }

    await loadInstances({ preserveFeedback: true })
    instanceFeedback.value = {
      type: 'success',
      title: `Workflow instance ${completedActionLabel}`,
      description: `Instance #${instance.id} was ${completedActionLabel} and the queue was refreshed.`,
    }
  } catch (error) {
    instanceFeedback.value = {
      type: 'error',
      title: `Workflow ${action} failed`,
      description: error instanceof Error ? error.message : `Unable to ${action} workflow instance #${instance.id}.`,
    }
  } finally {
    instanceActionId.value = null
    instanceAction.value = null
  }
}

const handleReset = () => {
  reset()
  definitionsErrorMessage.value = ''
}

onMounted(() => {
  void loadDefinitions()
  void loadInstances()
})
</script>

<template>
  <div class="workflow-admin-view" data-testid="workflow-admin-view">
    <PageSection
      eyebrow="System administration"
      title="Workflow administration"
      summary="Review available workflow definitions and manage approval processes across the system."
    />

    <el-alert
      v-if="definitionsErrorMessage"
      :closable="false"
      class="workflow-admin-view__alert"
      title="Workflow definitions unavailable"
      type="error"
      show-icon
      :description="definitionsErrorMessage"
    />

    <FilterForm
      title="Definition filters"
      :busy="isDefinitionsLoading"
      :reset-disabled="!isDirty"
      @reset="handleReset"
      @submit="loadDefinitions"
    >
      <el-form-item label="Search">
        <el-input
          v-model="filters.search"
          placeholder="Search code, name, or process class"
          clearable
          data-testid="workflow-search-input"
        />
      </el-form-item>
    </FilterForm>

    <el-card class="workflow-admin-view__table-card" shadow="never">
      <template #header>
        <div class="workflow-admin-view__table-header">
          <span>Workflow definitions</span>
          <el-tag effect="plain" type="info">{{ filteredDefinitions.length }} total</el-tag>
        </div>
      </template>

      <el-table
        :data="filteredDefinitions"
        row-key="ID"
        class="workflow-admin-view__table"
        empty-text="No workflow definitions available."
        data-testid="workflow-definitions-table"
      >
        <el-table-column prop="Code" label="Code" min-width="160" />
        <el-table-column prop="Name" label="Name" min-width="220" />
        <el-table-column prop="ProcessClass" label="Process class" min-width="200" />
        <el-table-column label="ID" min-width="80">
          <template #default="scope">
            {{ scope.row.ID }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-alert
      v-if="instanceFeedback"
      :closable="false"
      class="workflow-admin-view__alert"
      :title="instanceFeedback.title"
      :type="instanceFeedback.type"
      show-icon
      :description="instanceFeedback.description"
    />

    <el-card class="workflow-admin-view__table-card" shadow="never">
      <template #header>
        <div class="workflow-admin-view__table-header workflow-admin-view__table-header--actions">
          <div class="workflow-admin-view__table-heading">
            <span>Pending instances</span>
            <el-tag effect="plain" type="info">{{ instances.length }} total</el-tag>
          </div>

          <div class="workflow-admin-view__table-controls">
            <el-select
              v-model="instanceFilters.status"
              class="workflow-admin-view__status-filter"
              placeholder="Filter status"
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
              Refresh
            </el-button>
          </div>
        </div>
      </template>

      <el-table
        :data="instances"
        row-key="id"
        v-loading="isInstancesLoading"
        class="workflow-admin-view__table"
        empty-text="No workflow instances match the current filters."
        data-testid="workflow-instances-table"
      >
        <el-table-column prop="id" label="ID" min-width="90" />
        <el-table-column prop="document_type" label="Document type" min-width="180" />
        <el-table-column prop="document_id" label="Document ID" min-width="120" />
        <el-table-column label="Status" min-width="130">
          <template #default="scope">
            <el-tag :type="statusTagType(scope.row.status)" effect="plain">
              {{ scope.row.status }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="Current node" min-width="180">
          <template #default="scope">
            {{ resolveCurrentNodeCode(scope.row) }}
          </template>
        </el-table-column>
        <el-table-column label="Created at" min-width="200">
          <template #default="scope">
            {{ formatTimestamp(scope.row.created_at ?? scope.row.submitted_at) }}
          </template>
        </el-table-column>
        <el-table-column label="Actions" min-width="220" fixed="right">
          <template #default="scope">
            <div class="workflow-admin-view__row-actions">
              <el-button
                link
                type="primary"
                :loading="instanceActionId === scope.row.id && instanceAction === 'approve'"
                :disabled="isInstancesLoading || instanceActionId !== null || scope.row.status !== 'pending'"
                :data-testid="`workflow-approve-button-${scope.row.id}`"
                @click="handleInstanceAction('approve', scope.row)"
              >
                Approve
              </el-button>

              <el-button
                link
                type="danger"
                :loading="instanceActionId === scope.row.id && instanceAction === 'reject'"
                :disabled="isInstancesLoading || instanceActionId !== null || scope.row.status !== 'pending'"
                :data-testid="`workflow-reject-button-${scope.row.id}`"
                @click="handleInstanceAction('reject', scope.row)"
              >
                Reject
              </el-button>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.workflow-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.workflow-admin-view__alert {
  margin-bottom: 0;
}

.workflow-admin-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.workflow-admin-view__table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.workflow-admin-view__table-header--actions {
  flex-wrap: wrap;
}

.workflow-admin-view__table-heading,
.workflow-admin-view__table-controls,
.workflow-admin-view__row-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.workflow-admin-view__table-controls {
  flex-wrap: wrap;
  justify-content: flex-end;
}

.workflow-admin-view__status-filter {
  min-width: 12rem;
}

.workflow-admin-view__table {
  width: 100%;
}

@media (max-width: 52rem) {
  .workflow-admin-view__table-header {
    align-items: flex-start;
    flex-direction: column;
  }

  .workflow-admin-view__table-controls {
    justify-content: flex-start;
  }

  .workflow-admin-view__row-actions {
    flex-wrap: wrap;
  }
}
</style>
