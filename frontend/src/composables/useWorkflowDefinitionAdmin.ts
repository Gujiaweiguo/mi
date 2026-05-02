import { computed, ref } from 'vue'

import {
  deactivateWorkflowDefinitionTemplate,
  getWorkflowDefinition,
  listWorkflowDefinitionTemplates,
  listWorkflowDefinitionVersions,
  publishWorkflowDefinition,
  rollbackWorkflowDefinitionTemplate,
  saveWorkflowDefinitionDraft,
  validateWorkflowDefinition,
  type SaveWorkflowDefinitionDraftRequest,
  type WorkflowDefinitionAssignmentRule,
  type WorkflowDefinitionDetail,
  type WorkflowDefinitionTemplate,
  type WorkflowDefinitionValidationResult,
} from '../api/workflow'

export interface EditableWorkflowDefinitionNode {
  function_id: number
  role_id: number
  step_order: number
  code: string
  name: string
  can_submit_to_manager: boolean
  validates_after_confirm: boolean
  prints_after_confirm: boolean
  process_class: string
  assignment_rules: WorkflowDefinitionAssignmentRule[]
}

export interface EditableWorkflowDefinitionTransition {
  from_node_code: string | null
  to_node_code: string
  action: string
}

export interface EditableWorkflowDefinitionDraft {
  definition_id: number
  workflow_template_id: number
  name: string
  voucher_type: string
  is_initial: boolean
  nodes: EditableWorkflowDefinitionNode[]
  transitions: EditableWorkflowDefinitionTransition[]
}

const normalizeDefinitionDraft = (definition: WorkflowDefinitionDetail): EditableWorkflowDefinitionDraft => {
  const nodeCodesById = new Map(definition.Nodes.map((node) => [node.ID, node.Code]))

  return {
    definition_id: definition.ID,
    workflow_template_id: definition.WorkflowTemplateID,
    name: definition.Name,
    voucher_type: definition.VoucherType,
    is_initial: definition.IsInitial,
    nodes: definition.Nodes.map((node) => ({
      function_id: node.FunctionID,
      role_id: node.RoleID,
      step_order: node.StepOrder,
      code: node.Code,
      name: node.Name,
      can_submit_to_manager: node.CanSubmitToManager,
      validates_after_confirm: node.ValidatesAfterConfirm,
      prints_after_confirm: node.PrintsAfterConfirm,
      process_class: node.ProcessClass,
      assignment_rules: node.AssignmentRules.map((rule) => ({ ...rule })),
    })),
    transitions: definition.Transitions.map((transition) => ({
      from_node_code: transition.FromNodeID === null ? null : (nodeCodesById.get(transition.FromNodeID) ?? null),
      to_node_code: nodeCodesById.get(transition.ToNodeID) ?? '',
      action: transition.Action,
    })),
  }
}

const buildSavePayload = (draft: EditableWorkflowDefinitionDraft): SaveWorkflowDefinitionDraftRequest => ({
  name: draft.name,
  voucher_type: draft.voucher_type,
  is_initial: draft.is_initial,
  nodes: draft.nodes.map((node) => ({
    function_id: node.function_id,
    role_id: node.role_id,
    step_order: node.step_order,
    code: node.code,
    name: node.name,
    can_submit_to_manager: node.can_submit_to_manager,
    validates_after_confirm: node.validates_after_confirm,
    prints_after_confirm: node.prints_after_confirm,
    process_class: node.process_class,
    assignment_rules: node.assignment_rules.map((rule) => ({
      strategy_type: rule.strategy_type,
      config_json: rule.config_json,
    })),
  })),
  transitions: draft.transitions.map((transition) => ({
    from_node_code: transition.from_node_code,
    to_node_code: transition.to_node_code,
    action: transition.action,
  })),
})

const extractValidationResult = (error: unknown): WorkflowDefinitionValidationResult | null => {
  if (typeof error !== 'object' || error === null || !('validation' in error)) {
    return null
  }

  const validation = (error as { validation?: unknown }).validation

  if (typeof validation !== 'object' || validation === null || !('valid' in validation) || !('issues' in validation)) {
    return null
  }

  const valid = (validation as { valid?: unknown }).valid
  const issues = (validation as { issues?: unknown }).issues

  return typeof valid === 'boolean' && Array.isArray(issues)
    ? (validation as WorkflowDefinitionValidationResult)
    : null
}

export const useWorkflowDefinitionAdmin = () => {
  const templates = ref<WorkflowDefinitionTemplate[]>([])
  const versions = ref<WorkflowDefinitionDetail[]>([])
  const definition = ref<WorkflowDefinitionDetail | null>(null)
  const draft = ref<EditableWorkflowDefinitionDraft | null>(null)
  const selectedTemplateId = ref<number | null>(null)
  const selectedDefinitionId = ref<number | null>(null)
  const validation = ref<WorkflowDefinitionValidationResult | null>(null)

  const isLoadingTemplates = ref(false)
  const isLoadingVersions = ref(false)
  const isLoadingDefinition = ref(false)
  const isSavingDraft = ref(false)
  const isValidating = ref(false)
  const isPublishing = ref(false)
  const isDeactivating = ref(false)
  const isRollingBack = ref(false)

  const hasDraft = computed(() => draft.value !== null)

  const applyDefinition = (value: WorkflowDefinitionDetail) => {
    definition.value = value
    draft.value = normalizeDefinitionDraft(value)
    selectedTemplateId.value = value.WorkflowTemplateID
    selectedDefinitionId.value = value.ID
  }

  const replaceVersion = (value: WorkflowDefinitionDetail) => {
    const next = [...versions.value]
    const index = next.findIndex((item) => item.ID === value.ID)

    if (index === -1) {
      next.unshift(value)
    } else {
      next[index] = value
    }

    versions.value = next
  }

  const replaceTemplate = (value: WorkflowDefinitionTemplate) => {
    templates.value = templates.value.map((template) => (template.id === value.id ? value : template))
  }

  const loadTemplates = async () => {
    isLoadingTemplates.value = true

    try {
      const response = await listWorkflowDefinitionTemplates()
      templates.value = response.data.templates
      return response.data.templates
    } finally {
      isLoadingTemplates.value = false
    }
  }

  const loadVersions = async (templateId: number) => {
    isLoadingVersions.value = true
    selectedTemplateId.value = templateId

    try {
      const response = await listWorkflowDefinitionVersions(templateId)
      versions.value = response.data.definitions
      return response.data.definitions
    } finally {
      isLoadingVersions.value = false
    }
  }

  const loadDefinition = async (definitionId: number) => {
    isLoadingDefinition.value = true

    try {
      const response = await getWorkflowDefinition(definitionId)
      applyDefinition(response.data.definition)
      return response.data.definition
    } finally {
      isLoadingDefinition.value = false
    }
  }

  const saveDraft = async () => {
    if (draft.value === null) {
      throw new Error('workflow definition draft is not loaded')
    }

    isSavingDraft.value = true

    try {
      const response = await saveWorkflowDefinitionDraft(draft.value.definition_id, buildSavePayload(draft.value))
      applyDefinition(response.data.definition)
      replaceVersion(response.data.definition)
      return response.data.definition
    } finally {
      isSavingDraft.value = false
    }
  }

  const runValidation = async () => {
    if (selectedDefinitionId.value === null) {
      throw new Error('workflow definition is not selected')
    }

    isValidating.value = true

    try {
      const response = await validateWorkflowDefinition(selectedDefinitionId.value)
      validation.value = response.data.validation
      return response.data.validation
    } catch (error) {
      const validationResult = extractValidationResult(error)
      if (validationResult !== null) {
        validation.value = validationResult
      }
      throw error
    } finally {
      isValidating.value = false
    }
  }

  const publish = async () => {
    if (selectedDefinitionId.value === null) {
      throw new Error('workflow definition is not selected')
    }

    isPublishing.value = true

    try {
      const response = await publishWorkflowDefinition(selectedDefinitionId.value)
      applyDefinition(response.data.definition)
      replaceVersion(response.data.definition)
      return response.data.definition
    } catch (error) {
      const validationResult = extractValidationResult(error)
      if (validationResult !== null) {
        validation.value = validationResult
      }
      throw error
    } finally {
      isPublishing.value = false
    }
  }

  const deactivate = async (templateId = selectedTemplateId.value) => {
    if (templateId === null) {
      throw new Error('workflow template is not selected')
    }

    isDeactivating.value = true

    try {
      const response = await deactivateWorkflowDefinitionTemplate(templateId)
      replaceTemplate(response.data.template)
      return response.data.template
    } finally {
      isDeactivating.value = false
    }
  }

  const rollback = async (definitionId: number, templateId = selectedTemplateId.value) => {
    if (templateId === null) {
      throw new Error('workflow template is not selected')
    }

    isRollingBack.value = true

    try {
      const response = await rollbackWorkflowDefinitionTemplate(templateId, { definition_id: definitionId })
      applyDefinition(response.data.definition)
      replaceVersion(response.data.definition)
      return response.data.definition
    } finally {
      isRollingBack.value = false
    }
  }

  return {
    templates,
    versions,
    definition,
    draft,
    selectedTemplateId,
    selectedDefinitionId,
    validation,
    hasDraft,
    isLoadingTemplates,
    isLoadingVersions,
    isLoadingDefinition,
    isSavingDraft,
    isValidating,
    isPublishing,
    isDeactivating,
    isRollingBack,
    loadTemplates,
    loadVersions,
    loadDefinition,
    saveDraft,
    runValidation,
    publish,
    deactivate,
    rollback,
    buildSavePayload,
  }
}

export { buildSavePayload, normalizeDefinitionDraft }
