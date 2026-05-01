import { mount } from '@vue/test-utils'
import ElementPlus from 'element-plus'
import { computed, nextTick, reactive, ref } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import type { WorkflowDefinitionDetail, WorkflowDefinitionTemplate, WorkflowDefinitionValidationResult } from '../../api/workflow'
import { i18n } from '../../i18n'
import WorkflowDefinitionAdminPanel from './WorkflowDefinitionAdminPanel.vue'

type DraftNode = {
  function_id: number
  role_id: number
  step_order: number
  code: string
  name: string
  can_submit_to_manager: boolean
  validates_after_confirm: boolean
  prints_after_confirm: boolean
  process_class: string
  assignment_rules: Array<{ id: number; workflow_node_id: number; strategy_type: string; config_json: string }>
}

type DraftTransition = {
  from_node_code: string | null
  to_node_code: string
  action: string
}

type DraftState = {
  definition_id: number
  workflow_template_id: number
  name: string
  voucher_type: string
  is_initial: boolean
  nodes: DraftNode[]
  transitions: DraftTransition[]
}

const { messageSuccessMock } = vi.hoisted(() => ({
  messageSuccessMock: vi.fn(),
}))

const { canAccessMock } = vi.hoisted(() => ({
  canAccessMock: vi.fn(),
}))

vi.mock('element-plus', async () => {
  const actual = await vi.importActual<typeof import('element-plus')>('element-plus')

  return {
    ...actual,
    ElMessage: {
      success: messageSuccessMock,
      error: vi.fn(),
    },
  }
})

const mockAdmin = {
  templates: ref<WorkflowDefinitionTemplate[]>([]),
  versions: ref<WorkflowDefinitionDetail[]>([]),
  definition: ref<WorkflowDefinitionDetail | null>(null),
  draft: ref<DraftState | null>(null),
  hasDraft: computed(() => false),
  selectedTemplateId: ref<number | null>(null),
  selectedDefinitionId: ref<number | null>(null),
  validation: ref<WorkflowDefinitionValidationResult | null>(null),
  isLoadingTemplates: ref(false),
  isLoadingVersions: ref(false),
  isLoadingDefinition: ref(false),
  isSavingDraft: ref(false),
  isValidating: ref(false),
  isPublishing: ref(false),
  isDeactivating: ref(false),
  isRollingBack: ref(false),
  loadTemplates: vi.fn(),
  loadVersions: vi.fn(),
  loadDefinition: vi.fn(),
  saveDraft: vi.fn(),
  runValidation: vi.fn(),
  publish: vi.fn(),
  deactivate: vi.fn(),
  rollback: vi.fn(),
}
mockAdmin.hasDraft = computed(() => mockAdmin.draft.value !== null)

vi.mock('../../composables/useFilterForm', () => ({
  useFilterForm: (initialValues: Record<string, string>) => ({
    filters: reactive({ ...initialValues }),
    isDirty: false,
    reset: vi.fn(),
  }),
}))

vi.mock('../../composables/useWorkflowDefinitionAdmin', () => ({
  useWorkflowDefinitionAdmin: () => mockAdmin,
}))

vi.mock('../../stores/auth', () => ({
  useAuthStore: () => ({
    canAccess: canAccessMock,
  }),
}))

const createDefinition = (): WorkflowDefinitionDetail => ({
  ID: 11,
  BusinessGroupID: 3,
  WorkflowTemplateID: 7,
  Code: 'lease-approval',
  Name: 'Lease Approval V2',
  VoucherType: 'LEASE',
  IsInitial: false,
  Status: 'active',
  ProcessClass: 'lease_contract',
  VersionNumber: 2,
  LifecycleStatus: 'draft',
  PublishedAt: null,
  TransitionsEnabled: true,
  Nodes: [],
  Transitions: [],
})

const createDraft = () => ({
  definition_id: 11,
  workflow_template_id: 7,
  name: 'Lease Approval V2',
  voucher_type: 'LEASE',
  is_initial: false,
  nodes: [
    {
      function_id: 1001,
      role_id: 2001,
      step_order: 1,
      code: 'SUBMIT',
      name: 'Submit',
      can_submit_to_manager: false,
      validates_after_confirm: false,
      prints_after_confirm: false,
      process_class: 'lease_contract',
      assignment_rules: [{ id: 1, workflow_node_id: 101, strategy_type: 'fixed_role', config_json: '{"role_id":2001}' }],
    },
  ],
  transitions: [{ from_node_code: null, to_node_code: 'SUBMIT', action: 'submit' }],
})

const resetAdminState = () => {
  mockAdmin.templates.value = [
    {
      id: 7,
      business_group_id: 3,
      code: 'lease-approval',
      name: 'Lease Approval',
      process_class: 'lease_contract',
      status: 'active',
      published_definition_id: 11,
      published_version_number: 2,
    },
  ]
  mockAdmin.versions.value = [createDefinition()]
  mockAdmin.definition.value = createDefinition()
  mockAdmin.draft.value = createDraft()
  mockAdmin.selectedTemplateId.value = 7
  mockAdmin.selectedDefinitionId.value = 11
  mockAdmin.validation.value = null
  mockAdmin.loadTemplates.mockReset().mockResolvedValue(mockAdmin.templates.value)
  mockAdmin.loadVersions.mockReset().mockResolvedValue(mockAdmin.versions.value)
  mockAdmin.loadDefinition.mockReset().mockImplementation(async (definitionId: number) => {
    mockAdmin.selectedDefinitionId.value = definitionId
    return createDefinition()
  })
  mockAdmin.saveDraft.mockReset().mockResolvedValue(createDefinition())
  mockAdmin.runValidation.mockReset().mockImplementation(async () => {
    mockAdmin.validation.value = { valid: false, issues: [{ code: 'transition_gap', field: 'transitions', message: 'Transition gap' }] }
    return mockAdmin.validation.value
  })
  mockAdmin.publish.mockReset().mockResolvedValue({ ...createDefinition(), LifecycleStatus: 'published' })
  mockAdmin.deactivate.mockReset().mockResolvedValue({ ...mockAdmin.templates.value[0], published_definition_id: undefined, published_version_number: undefined })
  mockAdmin.rollback.mockReset().mockResolvedValue({ ...createDefinition(), VersionNumber: 1, LifecycleStatus: 'published' })
}

const mountPanel = () =>
  mount(WorkflowDefinitionAdminPanel, {
    global: {
      plugins: [i18n, ElementPlus],
    },
  })

describe('WorkflowDefinitionAdminPanel', () => {
  beforeEach(() => {
    i18n.global.locale.value = 'en-US'
    messageSuccessMock.mockReset()
    canAccessMock.mockReset().mockImplementation((_functionCode: string, action?: string) => {
      if (action === 'approve') {
        return true
      }

      return true
    })
    resetAdminState()
  })

  it('loads templates on mount and renders the selected version context', async () => {
    const wrapper = mountPanel()
    await nextTick()

    expect(mockAdmin.loadTemplates).toHaveBeenCalledTimes(1)
    expect(wrapper.text()).toContain('Selected version: v2')
    expect(wrapper.text()).toContain('Workflow templates')
    expect(wrapper.text()).toContain('Version history')
  })

  it('supports node, assignment-rule, and transition editing controls', async () => {
    const wrapper = mountPanel()
    await nextTick()

    await wrapper.get('[data-testid="workflow-definition-add-node-button"]').trigger('click')
    const draftAfterAddNode = mockAdmin.draft.value
    expect(draftAfterAddNode).not.toBeNull()
    expect(draftAfterAddNode?.nodes).toHaveLength(2)
    expect(draftAfterAddNode?.nodes.map((node) => node.step_order)).toEqual([1, 2])

    if (draftAfterAddNode) {
      draftAfterAddNode.nodes[1].code = 'MANAGER_APPROVE'
      draftAfterAddNode.transitions[0].to_node_code = 'SUBMIT'
    }

    await wrapper.get('[data-testid="workflow-definition-node-code-0"]').setValue('SUBMIT_RENAMED')
    await nextTick()

    const draftAfterRename = mockAdmin.draft.value
    expect(draftAfterRename?.transitions[0].to_node_code).toBe('SUBMIT_RENAMED')

    await wrapper.get('[data-testid="workflow-definition-move-node-down-0"]').trigger('click')
    await nextTick()

    const draftAfterMove = mockAdmin.draft.value
    expect(draftAfterMove?.nodes.map((node) => node.code)).toEqual(['MANAGER_APPROVE', 'SUBMIT_RENAMED'])
    expect(draftAfterMove?.nodes.map((node) => node.step_order)).toEqual([1, 2])

    await wrapper.get('[data-testid="workflow-definition-add-assignment-rule-0"]').trigger('click')
    const draftAfterAddRule = mockAdmin.draft.value
    expect(draftAfterAddRule?.nodes[0].assignment_rules).toHaveLength(2)

    await wrapper.get('[data-testid="workflow-definition-remove-assignment-rule-0-0"]').trigger('click')
    const draftAfterRemoveRule = mockAdmin.draft.value
    expect(draftAfterRemoveRule?.nodes[0].assignment_rules).toHaveLength(1)

    await wrapper.get('[data-testid="workflow-definition-add-transition-button"]').trigger('click')
    const draftAfterAddTransition = mockAdmin.draft.value
    expect(draftAfterAddTransition?.transitions).toHaveLength(2)

    await wrapper.get('[data-testid="workflow-definition-remove-transition-0"]').trigger('click')
    const draftAfterRemoveTransition = mockAdmin.draft.value
    expect(draftAfterRemoveTransition?.transitions).toHaveLength(1)

    await wrapper.get('[data-testid="workflow-definition-remove-node-0"]').trigger('click')
    await nextTick()

    const draftAfterRemoveNode = mockAdmin.draft.value
    expect(draftAfterRemoveNode?.nodes).toHaveLength(1)
    expect(draftAfterRemoveNode?.nodes[0].step_order).toBe(1)
  })

  it('saves drafts and renders validation diagnostics', async () => {
    const wrapper = mountPanel()
    await nextTick()

    await wrapper.get('[data-testid="workflow-definition-name-input"]').setValue('Lease Approval V3')
    await wrapper.get('[data-testid="workflow-definition-save-button"]').trigger('click')
    await nextTick()

    expect(mockAdmin.saveDraft).toHaveBeenCalledTimes(1)
    expect(messageSuccessMock).toHaveBeenCalledWith('Workflow definition draft saved')

    await wrapper.get('[data-testid="workflow-definition-validate-button"]').trigger('click')
    await nextTick()

    expect(mockAdmin.runValidation).toHaveBeenCalledTimes(1)
    expect(wrapper.get('[data-testid="workflow-definition-validation-issue-0"]').text()).toContain('Transition gap')
  })

  it('shows success feedback when validation passes with no issues', async () => {
    mockAdmin.runValidation.mockReset().mockImplementation(async () => {
      mockAdmin.validation.value = { valid: true, issues: [] }
      return mockAdmin.validation.value
    })

    const wrapper = mountPanel()
    await nextTick()

    await wrapper.get('[data-testid="workflow-definition-validate-button"]').trigger('click')
    await nextTick()

    expect(mockAdmin.runValidation).toHaveBeenCalledTimes(1)
    expect(wrapper.text()).toContain('Validation passed')
    expect(wrapper.text()).toContain('Validation passed with no issues.')
  })

  it('renders validation diagnostics when publish is rejected', async () => {
    mockAdmin.publish.mockReset().mockImplementation(async () => {
      mockAdmin.validation.value = {
        valid: false,
        issues: [{ code: 'missing_terminal', field: 'transitions', message: 'Terminal transition is required' }],
      }

      throw new Error('publish failed')
    })

    const wrapper = mountPanel()
    await nextTick()

    await wrapper.get('[data-testid="workflow-definition-publish-button"]').trigger('click')
    await nextTick()

    expect(mockAdmin.publish).toHaveBeenCalledTimes(1)
    expect(wrapper.get('[data-testid="workflow-definition-validation-issue-0"]').text()).toContain('Terminal transition is required')
    expect(wrapper.text()).toContain('Publication failed')
  })

  it('publishes, deactivates, and rolls back definitions from the action area', async () => {
    const wrapper = mountPanel()
    await nextTick()

    await wrapper.get('[data-testid="workflow-definition-publish-button"]').trigger('click')
    await nextTick()
    expect(mockAdmin.publish).toHaveBeenCalledTimes(1)
    expect(mockAdmin.loadTemplates).toHaveBeenCalledTimes(2)
    expect(mockAdmin.loadVersions).toHaveBeenCalledTimes(1)
    expect(messageSuccessMock).toHaveBeenCalledWith('Workflow definition published')

    await wrapper.get('[data-testid="workflow-definition-deactivate-button"]').trigger('click')
    await nextTick()
    expect(mockAdmin.deactivate).toHaveBeenCalledTimes(1)
    expect(messageSuccessMock).toHaveBeenCalledWith('Workflow template deactivated')

    await wrapper.get('[data-testid="workflow-definition-rollback-version-11"]').trigger('click')
    await nextTick()
    expect(mockAdmin.rollback).toHaveBeenCalledWith(11)
    expect(messageSuccessMock).toHaveBeenCalledWith('Workflow definition rollback complete')
  })

  it('disables edit and publish controls when permissions are missing', async () => {
    canAccessMock.mockImplementation((_functionCode: string, action?: string) => action === 'view')

    const wrapper = mountPanel()
    await nextTick()

    expect((wrapper.get('[data-testid="workflow-definition-save-button"]').element as HTMLButtonElement).disabled).toBe(true)
    expect((wrapper.get('[data-testid="workflow-definition-validate-button"]').element as HTMLButtonElement).disabled).toBe(true)
    expect((wrapper.get('[data-testid="workflow-definition-publish-button"]').element as HTMLButtonElement).disabled).toBe(true)
    expect((wrapper.get('[data-testid="workflow-definition-deactivate-button"]').element as HTMLButtonElement).disabled).toBe(true)
    expect((wrapper.get('[data-testid="workflow-definition-name-input"]').element as HTMLInputElement).disabled).toBe(true)
    expect((wrapper.get('[data-testid="workflow-definition-voucher-type-input"]').element as HTMLInputElement).disabled).toBe(true)
    expect(wrapper.get('[data-testid="workflow-definition-is-initial-input"]').classes()).toContain('is-disabled')
  })

  it('hides the definition panel when definition view permission is missing', async () => {
    canAccessMock.mockReturnValue(false)

    const wrapper = mountPanel()
    await nextTick()

    expect(wrapper.find('[data-testid="workflow-definition-admin-panel"]').exists()).toBe(false)
  })
})
