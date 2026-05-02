<script setup lang="ts">
import { ElMessage } from 'element-plus'
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { FUNCTION_CODES } from '../../auth/permissions'
import type { WorkflowDefinitionAssignmentRule, WorkflowDefinitionDetail, WorkflowDefinitionTemplate } from '../../api/workflow'
import { useFilterForm } from '../../composables/useFilterForm'
import { getErrorMessage } from '../../composables/useErrorMessage'
import {
  useWorkflowDefinitionAdmin,
  type EditableWorkflowDefinitionNode,
  type EditableWorkflowDefinitionTransition,
} from '../../composables/useWorkflowDefinitionAdmin'
import { useAuthStore } from '../../stores/auth'
import { formatDate } from '../../utils/format'
import FilterForm from '../platform/FilterForm.vue'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const { t } = useI18n()
const authStore = useAuthStore()

const feedback = ref<Feedback | null>(null)
const templatesErrorMessage = ref('')
const versionsErrorMessage = ref('')
const definitionErrorMessage = ref('')

const { filters, isDirty, reset } = useFilterForm({
  search: '',
})

const admin = useWorkflowDefinitionAdmin()

const assignmentStrategyOptions = computed(() => [
  { label: t('workflow.definition.assignmentStrategies.fixedUser'), value: 'fixed_user' },
  { label: t('workflow.definition.assignmentStrategies.fixedRole'), value: 'fixed_role' },
  { label: t('workflow.definition.assignmentStrategies.departmentLeader'), value: 'department_leader' },
  { label: t('workflow.definition.assignmentStrategies.submitterContext'), value: 'submitter_context' },
  { label: t('workflow.definition.assignmentStrategies.documentField'), value: 'document_field' },
])

const filteredTemplates = computed(() => {
  const search = filters.search.trim().toLowerCase()
  if (!search) {
    return admin.templates.value
  }

  return admin.templates.value.filter((template) => {
    return [template.code, template.name, template.process_class, template.status].some((value) => value.toLowerCase().includes(search))
  })
})

const selectedTemplate = computed(() => {
  return admin.templates.value.find((template) => template.id === admin.selectedTemplateId.value) ?? null
})

const selectedVersion = computed(() => {
  return admin.versions.value.find((item) => item.ID === admin.selectedDefinitionId.value) ?? admin.definition.value
})

const nodeCodeOptions = computed(() => {
  return (
    admin.draft.value?.nodes.map((node) => ({
      label: `${node.code || t('common.emptyValue')} · ${node.name || t('common.emptyValue')}`,
      value: node.code,
    })) ?? []
  )
})

const canEditDraft = computed(() => admin.hasDraft.value && admin.draft.value !== null)
const canAddTransition = computed(() => (admin.draft.value?.nodes.length ?? 0) > 0)
const canViewDefinitions = computed(() => authStore.canAccess(FUNCTION_CODES.workflowDefinition, 'view'))
const canEditDefinitions = computed(() => authStore.canAccess(FUNCTION_CODES.workflowDefinition, 'edit'))
const canApproveDefinitions = computed(() => authStore.canAccess(FUNCTION_CODES.workflowDefinition, 'approve'))

const resolveTemplatePublicationLabel = (template: WorkflowDefinitionTemplate) => {
  if (template.published_version_number !== undefined) {
    return t('workflow.definition.templateStatus.publishedVersion', { version: template.published_version_number })
  }

  if (template.status === 'inactive') {
    return t('common.statuses.inactive')
  }

  return t('workflow.definition.templateStatus.unpublished')
}

const resolveTemplateStatusType = (template: WorkflowDefinitionTemplate) => {
  if (template.published_version_number !== undefined) {
    return 'success'
  }

  if (template.status === 'inactive') {
    return 'info'
  }

  return 'warning'
}

const resolveDefinitionLifecycleLabel = (definition: WorkflowDefinitionDetail) => {
  if (definition.LifecycleStatus === 'published') {
    return t('workflow.definition.versionStatus.published')
  }

  if (definition.LifecycleStatus === 'draft') {
    return t('common.statuses.draft')
  }

  return definition.LifecycleStatus || t('common.emptyValue')
}

const resolveDefinitionLifecycleType = (definition: WorkflowDefinitionDetail) => {
  if (definition.LifecycleStatus === 'published') {
    return 'success'
  }

  if (definition.LifecycleStatus === 'draft') {
    return 'warning'
  }

  return 'info'
}

const parseInteger = (value: string) => {
  const normalized = value.trim()
  if (!normalized) {
    return 0
  }

  const parsedValue = Number.parseInt(normalized, 10)
  return Number.isFinite(parsedValue) ? parsedValue : 0
}

const createAssignmentRule = (): WorkflowDefinitionAssignmentRule => ({
  id: 0,
  workflow_node_id: 0,
  strategy_type: 'fixed_role',
  config_json: '{}',
})

const createNode = (): EditableWorkflowDefinitionNode => ({
  function_id: 0,
  role_id: 0,
  step_order: (admin.draft.value?.nodes.length ?? 0) + 1,
  code: '',
  name: '',
  can_submit_to_manager: false,
  validates_after_confirm: false,
  prints_after_confirm: false,
  process_class: admin.definition.value?.ProcessClass ?? '',
  assignment_rules: [createAssignmentRule()],
})

const createTransition = (): EditableWorkflowDefinitionTransition => ({
  from_node_code: null,
  to_node_code: admin.draft.value?.nodes[0]?.code ?? '',
  action: '',
})

const setFeedback = (value: Feedback | null) => {
  feedback.value = value
}

const clearSelectionState = () => {
  admin.versions.value = []
  admin.definition.value = null
  admin.draft.value = null
  admin.selectedDefinitionId.value = null
  admin.validation.value = null
}

const renumberNodes = () => {
  admin.draft.value?.nodes.forEach((node, index) => {
    node.step_order = index + 1
  })
}

const handleNodeCodeChange = (node: EditableWorkflowDefinitionNode, value: string) => {
  if (!admin.draft.value) {
    return
  }

  const previousCode = node.code
  node.code = value.trim()

  if (!previousCode || previousCode === node.code) {
    return
  }

  admin.draft.value.transitions.forEach((transition) => {
    if (transition.from_node_code === previousCode) {
      transition.from_node_code = node.code || null
    }

    if (transition.to_node_code === previousCode) {
      transition.to_node_code = node.code
    }
  })
}

const addNode = () => {
  if (!admin.draft.value) {
    return
  }

   if (!canEditDefinitions.value) {
    return
  }

  admin.draft.value.nodes.push(createNode())
  renumberNodes()
}

const removeNode = (index: number) => {
  if (!admin.draft.value) {
    return
  }

  if (!canEditDefinitions.value) {
    return
  }

  const [removedNode] = admin.draft.value.nodes.splice(index, 1)
  if (removedNode) {
    admin.draft.value.transitions = admin.draft.value.transitions.filter(
      (transition) => transition.from_node_code !== removedNode.code && transition.to_node_code !== removedNode.code,
    )
  }
  renumberNodes()
}

const moveNode = (index: number, direction: 'up' | 'down') => {
  if (!admin.draft.value) {
    return
  }

  if (!canEditDefinitions.value) {
    return
  }

  const targetIndex = direction === 'up' ? index - 1 : index + 1
  if (targetIndex < 0 || targetIndex >= admin.draft.value.nodes.length) {
    return
  }

  const nextNodes = [...admin.draft.value.nodes]
  const [currentNode] = nextNodes.splice(index, 1)
  nextNodes.splice(targetIndex, 0, currentNode)
  admin.draft.value.nodes = nextNodes
  renumberNodes()
}

const addAssignmentRule = (node: EditableWorkflowDefinitionNode) => {
  if (!canEditDefinitions.value) {
    return
  }

  node.assignment_rules.push(createAssignmentRule())
}

const removeAssignmentRule = (node: EditableWorkflowDefinitionNode, index: number) => {
  if (!canEditDefinitions.value) {
    return
  }

  node.assignment_rules.splice(index, 1)
}

const addTransition = () => {
  if (!admin.draft.value) {
    return
  }

  if (!canEditDefinitions.value) {
    return
  }

  admin.draft.value.transitions.push(createTransition())
}

const removeTransition = (index: number) => {
  if (!canEditDefinitions.value) {
    return
  }

  admin.draft.value?.transitions.splice(index, 1)
}

const loadDefinition = async (definitionId: number) => {
  definitionErrorMessage.value = ''
  admin.validation.value = null
  setFeedback(null)

  try {
    await admin.loadDefinition(definitionId)
  } catch (error) {
    definitionErrorMessage.value = getErrorMessage(error, t('workflow.definition.errors.unableToLoadDefinition'))
  }
}

const openTemplate = async (template: WorkflowDefinitionTemplate) => {
  versionsErrorMessage.value = ''
  definitionErrorMessage.value = ''
  setFeedback(null)
  clearSelectionState()
  admin.selectedTemplateId.value = template.id

  try {
    const versions = await admin.loadVersions(template.id)
    const initialVersion =
      versions.find((item) => item.ID === template.published_definition_id) ?? versions[0] ?? null

    if (initialVersion) {
      await loadDefinition(initialVersion.ID)
    }
  } catch (error) {
    versionsErrorMessage.value = getErrorMessage(error, t('workflow.definition.errors.unableToLoadVersions'))
  }
}

const refreshCurrentTemplate = async () => {
  if (admin.selectedTemplateId.value !== null) {
    try {
      await admin.loadVersions(admin.selectedTemplateId.value)
    } catch {
      // handled by prior action feedback
    }
  }

  try {
    await admin.loadTemplates()
  } catch {
    // handled by prior action feedback
  }
}

const handleSave = async () => {
  if (!admin.draft.value) {
    return
  }

  if (!canEditDefinitions.value) {
    return
  }

  try {
    const savedDefinition = await admin.saveDraft()
    await refreshCurrentTemplate()
    setFeedback({
      type: 'success',
      title: t('workflow.definition.feedback.savedTitle'),
      description: t('workflow.definition.feedback.savedDescription', { version: savedDefinition.VersionNumber }),
    })
    ElMessage.success(t('workflow.definition.feedback.savedToast'))
  } catch (error) {
    setFeedback({
      type: 'error',
      title: t('workflow.definition.errors.saveFailed'),
      description: getErrorMessage(error, t('workflow.definition.errors.unableToSaveDraft')),
    })
  }
}

const handleValidate = async () => {
  if (!canEditDefinitions.value) {
    return
  }

  try {
    const validation = await admin.runValidation()
    setFeedback(
      validation.valid
        ? {
            type: 'success',
            title: t('workflow.definition.feedback.validationPassedTitle'),
            description: t('workflow.definition.feedback.validationPassedDescription'),
          }
        : {
            type: 'warning',
            title: t('workflow.definition.feedback.validationFailedTitle'),
            description: t('workflow.definition.feedback.validationFailedDescription'),
          },
    )
  } catch (error) {
    setFeedback({
      type: 'warning',
      title: t('workflow.definition.feedback.validationFailedTitle'),
      description: admin.validation.value?.issues[0]?.message ?? getErrorMessage(error, t('workflow.definition.errors.unableToValidate')),
    })
  }
}

const handlePublish = async () => {
  if (!canApproveDefinitions.value) {
    return
  }

  try {
    const publishedDefinition = await admin.publish()
    await refreshCurrentTemplate()
    setFeedback({
      type: 'success',
      title: t('workflow.definition.feedback.publishedTitle'),
      description: t('workflow.definition.feedback.publishedDescription', { version: publishedDefinition.VersionNumber }),
    })
    ElMessage.success(t('workflow.definition.feedback.publishedToast'))
  } catch (error) {
    setFeedback({
      type: 'error',
      title: t('workflow.definition.errors.publishFailed'),
      description: admin.validation.value?.issues[0]?.message ?? getErrorMessage(error, t('workflow.definition.errors.unableToPublish')),
    })
  }
}

const handleDeactivate = async () => {
  if (!canApproveDefinitions.value) {
    return
  }

  try {
    await admin.deactivate()
    await refreshCurrentTemplate()
    setFeedback({
      type: 'success',
      title: t('workflow.definition.feedback.deactivatedTitle'),
      description: t('workflow.definition.feedback.deactivatedDescription'),
    })
    ElMessage.success(t('workflow.definition.feedback.deactivatedToast'))
  } catch (error) {
    setFeedback({
      type: 'error',
      title: t('workflow.definition.errors.deactivateFailed'),
      description: getErrorMessage(error, t('workflow.definition.errors.unableToDeactivate')),
    })
  }
}

const handleRollback = async (definitionId: number) => {
  if (!canApproveDefinitions.value) {
    return
  }

  try {
    const rolledBackDefinition = await admin.rollback(definitionId)
    await refreshCurrentTemplate()
    setFeedback({
      type: 'success',
      title: t('workflow.definition.feedback.rolledBackTitle'),
      description: t('workflow.definition.feedback.rolledBackDescription', { version: rolledBackDefinition.VersionNumber }),
    })
    ElMessage.success(t('workflow.definition.feedback.rolledBackToast'))
  } catch (error) {
    setFeedback({
      type: 'error',
      title: t('workflow.definition.errors.rollbackFailed'),
      description: getErrorMessage(error, t('workflow.definition.errors.unableToRollback')),
    })
  }
}

const handleFilterReset = () => {
  reset()
  templatesErrorMessage.value = ''
  setFeedback(null)
}

const loadTemplates = async () => {
  templatesErrorMessage.value = ''

  try {
    await admin.loadTemplates()
  } catch (error) {
    templatesErrorMessage.value = getErrorMessage(error, t('workflow.definition.errors.unableToLoadTemplates'))
  }
}

onMounted(() => {
  void loadTemplates()
})
</script>

<template>
  <div v-if="canViewDefinitions" class="workflow-definition-admin-panel" data-testid="workflow-definition-admin-panel">
    <el-alert
      v-if="feedback"
      :closable="false"
      class="workflow-definition-admin-panel__alert"
      :title="feedback.title"
      :type="feedback.type"
      show-icon
      :description="feedback.description"
    />

    <el-alert
      v-if="templatesErrorMessage"
      :closable="false"
      class="workflow-definition-admin-panel__alert"
      :title="t('workflow.definition.errors.templatesUnavailable')"
      type="error"
      show-icon
      :description="templatesErrorMessage"
    />

    <el-alert
      v-if="versionsErrorMessage"
      :closable="false"
      class="workflow-definition-admin-panel__alert"
      :title="t('workflow.definition.errors.versionsUnavailable')"
      type="error"
      show-icon
      :description="versionsErrorMessage"
    />

    <el-alert
      v-if="definitionErrorMessage"
      :closable="false"
      class="workflow-definition-admin-panel__alert"
      :title="t('workflow.definition.errors.definitionUnavailable')"
      type="error"
      show-icon
      :description="definitionErrorMessage"
    />

    <FilterForm
      :title="t('workflow.definition.filters.title')"
      :busy="admin.isLoadingTemplates.value"
      :reset-disabled="!isDirty"
      @reset="handleFilterReset"
      @submit="loadTemplates"
    >
      <el-form-item :label="t('workflow.fields.search')">
        <el-input
          v-model="filters.search"
          :placeholder="t('workflow.definition.placeholders.searchTemplates')"
          clearable
          data-testid="workflow-definition-search-input"
        />
      </el-form-item>
    </FilterForm>

    <section class="workflow-definition-admin-panel__overview-grid">
      <el-card class="workflow-definition-admin-panel__card" shadow="never">
        <template #header>
          <div class="workflow-definition-admin-panel__card-header">
            <span>{{ t('workflow.definition.cards.templates') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: filteredTemplates.length }) }}</el-tag>
          </div>
        </template>

        <el-table
          :data="filteredTemplates"
          row-key="id"
          v-loading="admin.isLoadingTemplates.value"
          :empty-text="t('workflow.definition.table.templatesEmpty')"
          data-testid="workflow-definition-templates-table"
        >
          <el-table-column prop="code" :label="t('workflow.columns.code')" min-width="180" />
          <el-table-column prop="name" :label="t('workflow.columns.name')" min-width="220" />
          <el-table-column prop="process_class" :label="t('workflow.columns.processClass')" min-width="180" />
          <el-table-column :label="t('common.columns.status')" min-width="180">
            <template #default="scope">
              <el-tag :type="resolveTemplateStatusType(scope.row)" effect="plain">
                {{ resolveTemplatePublicationLabel(scope.row) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
              <el-button
                link
                type="primary"
                :data-testid="`workflow-definition-open-template-${scope.row.id}`"
                @click="openTemplate(scope.row)"
              >
                {{ t('workflow.definition.actions.open') }}
              </el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="workflow-definition-admin-panel__card" shadow="never">
        <template #header>
          <div class="workflow-definition-admin-panel__card-header">
            <span>{{ t('workflow.definition.cards.versions') }}</span>
            <el-tag effect="plain" type="info">{{ t('common.total', { count: admin.versions.value.length }) }}</el-tag>
          </div>
        </template>

        <el-table
          :data="admin.versions.value"
          row-key="ID"
          v-loading="admin.isLoadingVersions.value"
          :empty-text="t('workflow.definition.table.versionsEmpty')"
          data-testid="workflow-definition-versions-table"
        >
          <el-table-column :label="t('workflow.definition.columns.version')" min-width="110">
            <template #default="scope">v{{ scope.row.VersionNumber }}</template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="140">
            <template #default="scope">
              <el-tag :type="resolveDefinitionLifecycleType(scope.row)" effect="plain">
                {{ resolveDefinitionLifecycleLabel(scope.row) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.updatedAt')" min-width="180">
            <template #default="scope">
              {{ formatDate(scope.row.PublishedAt) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="150" fixed="right">
            <template #default="scope">
              <div class="workflow-definition-admin-panel__row-actions">
                <el-button
                  link
                  type="primary"
                  :data-testid="`workflow-definition-load-version-${scope.row.ID}`"
                  @click="loadDefinition(scope.row.ID)"
                >
                  {{ t('workflow.definition.actions.load') }}
                </el-button>
                <el-button
                  link
                  type="warning"
                  :disabled="!canApproveDefinitions"
                  :loading="admin.isRollingBack.value && admin.selectedDefinitionId.value === scope.row.ID"
                  :data-testid="`workflow-definition-rollback-version-${scope.row.ID}`"
                  @click="handleRollback(scope.row.ID)"
                >
                  {{ t('workflow.definition.actions.rollback') }}
                </el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </section>

    <el-card class="workflow-definition-admin-panel__card" shadow="never">
      <template #header>
        <div class="workflow-definition-admin-panel__card-header workflow-definition-admin-panel__card-header--actions">
          <div class="workflow-definition-admin-panel__card-title-group">
            <span>{{ t('workflow.definition.cards.actionBar') }}</span>
            <small>
              {{
                selectedVersion
                  ? t('workflow.definition.meta.selectedVersion', { version: selectedVersion.VersionNumber })
                  : t('workflow.definition.defaults.selectTemplate')
              }}
            </small>
          </div>

          <div class="workflow-definition-admin-panel__toolbar">
            <el-button
              type="primary"
              :disabled="!canEditDraft || !canEditDefinitions"
              :loading="admin.isSavingDraft.value"
              data-testid="workflow-definition-save-button"
              @click="handleSave"
            >
              {{ t('common.actions.save') }}
            </el-button>
            <el-button
              :disabled="!canEditDraft || !canEditDefinitions"
              :loading="admin.isValidating.value"
              data-testid="workflow-definition-validate-button"
              @click="handleValidate"
            >
              {{ t('workflow.definition.actions.validate') }}
            </el-button>
            <el-button
              type="success"
              :disabled="!canEditDraft || !canApproveDefinitions"
              :loading="admin.isPublishing.value"
              data-testid="workflow-definition-publish-button"
              @click="handlePublish"
            >
              {{ t('workflow.definition.actions.publish') }}
            </el-button>
            <el-button
              type="danger"
              :disabled="selectedTemplate === null || !canApproveDefinitions"
              :loading="admin.isDeactivating.value"
              data-testid="workflow-definition-deactivate-button"
              @click="handleDeactivate"
            >
              {{ t('workflow.definition.actions.deactivate') }}
            </el-button>
          </div>
        </div>
      </template>

      <div class="workflow-definition-admin-panel__meta-grid">
        <div class="workflow-definition-admin-panel__meta-item">
          <span>{{ t('workflow.columns.code') }}</span>
          <strong>{{ admin.definition.value?.Code || t('common.emptyValue') }}</strong>
        </div>
        <div class="workflow-definition-admin-panel__meta-item">
          <span>{{ t('workflow.columns.processClass') }}</span>
          <strong>{{ admin.definition.value?.ProcessClass || t('common.emptyValue') }}</strong>
        </div>
        <div class="workflow-definition-admin-panel__meta-item">
          <span>{{ t('common.columns.status') }}</span>
          <el-tag v-if="selectedVersion" :type="resolveDefinitionLifecycleType(selectedVersion)" effect="plain">
            {{ resolveDefinitionLifecycleLabel(selectedVersion) }}
          </el-tag>
          <strong v-else>{{ t('common.emptyValue') }}</strong>
        </div>
        <div class="workflow-definition-admin-panel__meta-item">
          <span>{{ t('workflow.definition.fields.publishedAt') }}</span>
          <strong>{{ formatDate(admin.definition.value?.PublishedAt) }}</strong>
        </div>
      </div>
    </el-card>

    <div v-if="admin.draft.value" class="workflow-definition-admin-panel__editor-stack">
      <el-card class="workflow-definition-admin-panel__card" shadow="never">
        <template #header>
          <div class="workflow-definition-admin-panel__card-header">
            <span>{{ t('workflow.definition.cards.metadata') }}</span>
          </div>
        </template>

          <el-form label-position="top" class="workflow-definition-admin-panel__form-grid">
          <el-form-item :label="t('workflow.columns.name')">
            <el-input v-model="admin.draft.value.name" :disabled="!canEditDefinitions" data-testid="workflow-definition-name-input" />
          </el-form-item>
          <el-form-item :label="t('workflow.definition.fields.voucherType')">
            <el-input v-model="admin.draft.value.voucher_type" :disabled="!canEditDefinitions" data-testid="workflow-definition-voucher-type-input" />
          </el-form-item>
          <el-form-item :label="t('workflow.definition.fields.isInitial')">
            <el-switch v-model="admin.draft.value.is_initial" :disabled="!canEditDefinitions" data-testid="workflow-definition-is-initial-input" />
          </el-form-item>
        </el-form>
      </el-card>

      <el-card class="workflow-definition-admin-panel__card" shadow="never">
        <template #header>
          <div class="workflow-definition-admin-panel__card-header workflow-definition-admin-panel__card-header--actions">
            <span>{{ t('workflow.definition.cards.nodes') }}</span>
            <el-button type="primary" plain :disabled="!canEditDefinitions" data-testid="workflow-definition-add-node-button" @click="addNode">
              {{ t('workflow.definition.actions.addNode') }}
            </el-button>
          </div>
        </template>

        <div class="workflow-definition-admin-panel__node-stack">
          <section
            v-for="(node, nodeIndex) in admin.draft.value.nodes"
            :key="`${node.code}-${nodeIndex}`"
            class="workflow-definition-admin-panel__node-card"
            :data-testid="`workflow-definition-node-${nodeIndex}`"
          >
            <div class="workflow-definition-admin-panel__node-header">
              <div>
                <strong>{{ t('workflow.definition.meta.nodeStep', { step: node.step_order }) }}</strong>
                <p>{{ node.code || t('workflow.definition.defaults.unsavedNode') }}</p>
              </div>

              <div class="workflow-definition-admin-panel__row-actions">
                <el-button link :disabled="nodeIndex === 0 || !canEditDefinitions" @click="moveNode(nodeIndex, 'up')">
                  <span :data-testid="`workflow-definition-move-node-up-${nodeIndex}`">{{ t('workflow.definition.actions.moveUp') }}</span>
                </el-button>
                <el-button link :disabled="nodeIndex === admin.draft.value.nodes.length - 1 || !canEditDefinitions" @click="moveNode(nodeIndex, 'down')">
                  <span :data-testid="`workflow-definition-move-node-down-${nodeIndex}`">{{ t('workflow.definition.actions.moveDown') }}</span>
                </el-button>
                <el-button link type="danger" :disabled="!canEditDefinitions" :data-testid="`workflow-definition-remove-node-${nodeIndex}`" @click="removeNode(nodeIndex)">
                  {{ t('workflow.definition.actions.removeNode') }}
                </el-button>
              </div>
            </div>

            <div class="workflow-definition-admin-panel__node-grid">
              <el-form-item :label="t('workflow.columns.code')">
                <el-input
                  :model-value="node.code"
                  :data-testid="nodeIndex === 0 ? 'workflow-definition-node-code-0' : undefined"
                  :disabled="!canEditDefinitions"
                  @update:model-value="handleNodeCodeChange(node, $event)"
                />
              </el-form-item>
              <el-form-item :label="t('workflow.columns.name')">
                <el-input v-model="node.name" :disabled="!canEditDefinitions" data-testid="workflow-definition-node-name-0" />
              </el-form-item>
              <el-form-item :label="t('workflow.definition.fields.functionId')">
                <el-input
                  :model-value="String(node.function_id)"
                  :disabled="!canEditDefinitions"
                  @update:model-value="node.function_id = parseInteger($event)"
                />
              </el-form-item>
              <el-form-item :label="t('workflow.definition.fields.roleId')">
                <el-input
                  :model-value="String(node.role_id)"
                  :data-testid="nodeIndex === 0 ? 'workflow-definition-node-role-id-0' : undefined"
                  :disabled="!canEditDefinitions"
                  @update:model-value="node.role_id = parseInteger($event)"
                />
              </el-form-item>
              <el-form-item :label="t('workflow.columns.processClass')">
                <el-input v-model="node.process_class" :disabled="!canEditDefinitions" />
              </el-form-item>
            </div>

            <div class="workflow-definition-admin-panel__toggle-row">
              <el-checkbox v-model="node.can_submit_to_manager" :disabled="!canEditDefinitions">{{ t('workflow.definition.fields.canSubmitToManager') }}</el-checkbox>
              <el-checkbox v-model="node.validates_after_confirm" :disabled="!canEditDefinitions">{{ t('workflow.definition.fields.validatesAfterConfirm') }}</el-checkbox>
              <el-checkbox v-model="node.prints_after_confirm" :disabled="!canEditDefinitions">{{ t('workflow.definition.fields.printsAfterConfirm') }}</el-checkbox>
            </div>

            <div class="workflow-definition-admin-panel__assignment-section">
              <div class="workflow-definition-admin-panel__section-header">
                <span>{{ t('workflow.definition.cards.assignmentRules') }}</span>
                <el-button link type="primary" :disabled="!canEditDefinitions" :data-testid="`workflow-definition-add-assignment-rule-${nodeIndex}`" @click="addAssignmentRule(node)">
                  {{ t('workflow.definition.actions.addAssignmentRule') }}
                </el-button>
              </div>

              <div
                v-for="(rule, ruleIndex) in node.assignment_rules"
                :key="`${rule.strategy_type}-${ruleIndex}`"
                class="workflow-definition-admin-panel__assignment-card"
              >
                <div class="workflow-definition-admin-panel__assignment-grid">
                  <el-form-item :label="t('workflow.definition.fields.strategyType')">
                     <el-select v-model="rule.strategy_type" :disabled="!canEditDefinitions" :data-testid="nodeIndex === 0 && ruleIndex === 0 ? 'workflow-definition-assignment-strategy-0-0' : undefined">
                      <el-option
                        v-for="option in assignmentStrategyOptions"
                        :key="option.value"
                        :label="option.label"
                        :value="option.value"
                      />
                    </el-select>
                  </el-form-item>
                  <el-form-item :label="t('workflow.definition.fields.configJson')">
                    <el-input
                      v-model="rule.config_json"
                      type="textarea"
                       :rows="3"
                       :disabled="!canEditDefinitions"
                      :data-testid="nodeIndex === 0 && ruleIndex === 0 ? 'workflow-definition-assignment-config-0-0' : undefined"
                    />
                  </el-form-item>
                </div>

                <div class="workflow-definition-admin-panel__row-actions">
                  <el-button link type="danger" :disabled="!canEditDefinitions" :data-testid="`workflow-definition-remove-assignment-rule-${nodeIndex}-${ruleIndex}`" @click="removeAssignmentRule(node, ruleIndex)">
                    {{ t('workflow.definition.actions.removeAssignmentRule') }}
                  </el-button>
                </div>
              </div>
            </div>
          </section>

          <div v-if="!admin.draft.value.nodes.length" class="workflow-definition-admin-panel__empty-state">
            {{ t('workflow.definition.defaults.noNodes') }}
          </div>
        </div>
      </el-card>

      <el-card class="workflow-definition-admin-panel__card" shadow="never">
        <template #header>
          <div class="workflow-definition-admin-panel__card-header workflow-definition-admin-panel__card-header--actions">
            <span>{{ t('workflow.definition.cards.transitions') }}</span>
            <el-button type="primary" plain :disabled="!canAddTransition || !canEditDefinitions" data-testid="workflow-definition-add-transition-button" @click="addTransition">
              {{ t('workflow.definition.actions.addTransition') }}
            </el-button>
          </div>
        </template>

        <el-table
          :data="admin.draft.value.transitions"
          row-key="action"
          :empty-text="t('workflow.definition.defaults.noTransitions')"
          data-testid="workflow-definition-transitions-table"
        >
          <el-table-column :label="t('workflow.definition.fields.fromNode')" min-width="180">
            <template #default="scope">
               <el-select v-model="scope.row.from_node_code" :disabled="!canEditDefinitions" clearable>
                <el-option :label="t('workflow.definition.defaults.entryTransition')" :value="null" />
                <el-option v-for="option in nodeCodeOptions" :key="option.value" :label="option.label" :value="option.value" />
              </el-select>
            </template>
          </el-table-column>
          <el-table-column :label="t('workflow.definition.fields.toNode')" min-width="180">
            <template #default="scope">
               <el-select v-model="scope.row.to_node_code" :disabled="!canEditDefinitions" :data-testid="scope.$index === 0 ? 'workflow-definition-transition-to-node-0' : undefined">
                <el-option v-for="option in nodeCodeOptions" :key="option.value" :label="option.label" :value="option.value" />
              </el-select>
            </template>
          </el-table-column>
          <el-table-column :label="t('workflow.definition.fields.transitionAction')" min-width="200">
            <template #default="scope">
              <el-input v-model="scope.row.action" :disabled="!canEditDefinitions" :data-testid="scope.$index === 0 ? 'workflow-definition-transition-action-0' : undefined" />
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
                <el-button link type="danger" :disabled="!canEditDefinitions" :data-testid="`workflow-definition-remove-transition-${scope.$index}`" @click="removeTransition(scope.$index)">
                  {{ t('workflow.definition.actions.removeTransition') }}
                </el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="workflow-definition-admin-panel__card" shadow="never">
        <template #header>
          <div class="workflow-definition-admin-panel__card-header">
            <span>{{ t('workflow.definition.cards.validation') }}</span>
            <el-tag
              effect="plain"
              :type="admin.validation.value?.valid ? 'success' : admin.validation.value ? 'danger' : 'info'"
            >
              {{
                admin.validation.value === null
                  ? t('common.notCheckedYet')
                  : admin.validation.value.valid
                    ? t('workflow.definition.validation.valid')
                    : t('workflow.definition.validation.invalid')
              }}
            </el-tag>
          </div>
        </template>

        <div v-if="admin.validation.value === null" class="workflow-definition-admin-panel__empty-state">
          {{ t('workflow.definition.defaults.validationPending') }}
        </div>

        <div v-else-if="admin.validation.value.issues.length === 0" class="workflow-definition-admin-panel__validation-pass">
          {{ t('workflow.definition.validation.noIssues') }}
        </div>

        <ul v-else class="workflow-definition-admin-panel__validation-list" data-testid="workflow-definition-validation-issues">
          <li
            v-for="(issue, index) in admin.validation.value.issues"
            :key="`${issue.code}-${index}`"
            class="workflow-definition-admin-panel__validation-item"
            :data-testid="`workflow-definition-validation-issue-${index}`"
          >
            <div class="workflow-definition-admin-panel__validation-item-header">
              <strong>{{ issue.code }}</strong>
              <span>{{ issue.field || t('common.emptyValue') }}</span>
            </div>
            <p>{{ issue.message }}</p>
          </li>
        </ul>
      </el-card>
    </div>

    <el-card v-else class="workflow-definition-admin-panel__card" shadow="never">
      <div class="workflow-definition-admin-panel__empty-state">
        {{ t('workflow.definition.defaults.selectTemplate') }}
      </div>
    </el-card>
  </div>
</template>

<style scoped>
.workflow-definition-admin-panel {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.workflow-definition-admin-panel__alert {
  margin-bottom: 0;
}

.workflow-definition-admin-panel__overview-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.workflow-definition-admin-panel__editor-stack,
.workflow-definition-admin-panel__node-stack,
.workflow-definition-admin-panel__assignment-section,
.workflow-definition-admin-panel__validation-list {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.workflow-definition-admin-panel__card,
.workflow-definition-admin-panel__node-card,
.workflow-definition-admin-panel__assignment-card,
.workflow-definition-admin-panel__validation-item,
.workflow-definition-admin-panel__validation-pass,
.workflow-definition-admin-panel__empty-state {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.workflow-definition-admin-panel__node-card,
.workflow-definition-admin-panel__assignment-card,
.workflow-definition-admin-panel__validation-item,
.workflow-definition-admin-panel__validation-pass,
.workflow-definition-admin-panel__empty-state {
  padding: var(--mi-space-4);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  background: var(--mi-surface-gradient);
}

.workflow-definition-admin-panel__card-header,
.workflow-definition-admin-panel__card-title-group,
.workflow-definition-admin-panel__toolbar,
.workflow-definition-admin-panel__row-actions,
.workflow-definition-admin-panel__node-header,
.workflow-definition-admin-panel__toggle-row,
.workflow-definition-admin-panel__section-header,
.workflow-definition-admin-panel__validation-item-header {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.workflow-definition-admin-panel__card-header {
  justify-content: space-between;
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.workflow-definition-admin-panel__card-header--actions,
.workflow-definition-admin-panel__node-header,
.workflow-definition-admin-panel__section-header,
.workflow-definition-admin-panel__validation-item-header {
  flex-wrap: wrap;
  justify-content: space-between;
}

.workflow-definition-admin-panel__card-title-group {
  flex-direction: column;
  align-items: flex-start;
}

.workflow-definition-admin-panel__card-title-group small,
.workflow-definition-admin-panel__meta-item span,
.workflow-definition-admin-panel__node-header p,
.workflow-definition-admin-panel__validation-item p,
.workflow-definition-admin-panel__empty-state {
  color: var(--mi-color-muted);
}

.workflow-definition-admin-panel__toolbar,
.workflow-definition-admin-panel__toggle-row {
  flex-wrap: wrap;
}

.workflow-definition-admin-panel__meta-grid,
.workflow-definition-admin-panel__form-grid,
.workflow-definition-admin-panel__node-grid,
.workflow-definition-admin-panel__assignment-grid {
  display: grid;
  gap: var(--mi-space-4);
}

.workflow-definition-admin-panel__meta-grid {
  grid-template-columns: repeat(4, minmax(0, 1fr));
}

.workflow-definition-admin-panel__meta-item {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
}

.workflow-definition-admin-panel__meta-item strong,
.workflow-definition-admin-panel__node-header strong,
.workflow-definition-admin-panel__validation-item strong {
  color: var(--mi-color-text);
}

.workflow-definition-admin-panel__form-grid,
.workflow-definition-admin-panel__node-grid {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.workflow-definition-admin-panel__assignment-grid {
  grid-template-columns: minmax(14rem, 16rem) minmax(0, 1fr);
}

.workflow-definition-admin-panel__validation-list {
  list-style: none;
  margin: 0;
  padding: 0;
}

.workflow-definition-admin-panel__validation-item p,
.workflow-definition-admin-panel__node-header p {
  margin: 0;
}

@media (max-width: 64rem) {
  .workflow-definition-admin-panel__overview-grid,
  .workflow-definition-admin-panel__meta-grid,
  .workflow-definition-admin-panel__form-grid,
  .workflow-definition-admin-panel__node-grid,
  .workflow-definition-admin-panel__assignment-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
