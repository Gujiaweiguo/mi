import { beforeEach, describe, expect, it, vi } from 'vitest'

import type {
  WorkflowDefinitionDetail,
  WorkflowDefinitionTemplate,
  WorkflowDefinitionValidationResult,
} from '../api/workflow'

vi.mock('../api/workflow', () => ({
  deactivateWorkflowDefinitionTemplate: vi.fn(),
  getWorkflowDefinition: vi.fn(),
  listWorkflowDefinitionTemplates: vi.fn(),
  listWorkflowDefinitionVersions: vi.fn(),
  publishWorkflowDefinition: vi.fn(),
  rollbackWorkflowDefinitionTemplate: vi.fn(),
  saveWorkflowDefinitionDraft: vi.fn(),
  validateWorkflowDefinition: vi.fn(),
}))

import {
  deactivateWorkflowDefinitionTemplate,
  getWorkflowDefinition,
  listWorkflowDefinitionTemplates,
  listWorkflowDefinitionVersions,
  publishWorkflowDefinition,
  rollbackWorkflowDefinitionTemplate,
  saveWorkflowDefinitionDraft,
  validateWorkflowDefinition,
} from '../api/workflow'
import { useWorkflowDefinitionAdmin } from './useWorkflowDefinitionAdmin'

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
  Nodes: [
    {
      ID: 101,
      WorkflowDefinitionID: 11,
      FunctionID: 1001,
      RoleID: 2001,
      StepOrder: 1,
      Code: 'SUBMIT',
      Name: 'Submit',
      CanSubmitToManager: false,
      ValidatesAfterConfirm: false,
      PrintsAfterConfirm: false,
      ProcessClass: 'lease_contract',
      AssignmentRules: [{ id: 301, workflow_node_id: 101, strategy_type: 'fixed_role', config_json: '{"role_id":2001}' }],
    },
    {
      ID: 102,
      WorkflowDefinitionID: 11,
      FunctionID: 1002,
      RoleID: 2002,
      StepOrder: 2,
      Code: 'MANAGER_APPROVE',
      Name: 'Manager Approve',
      CanSubmitToManager: true,
      ValidatesAfterConfirm: true,
      PrintsAfterConfirm: false,
      ProcessClass: 'lease_contract',
      AssignmentRules: [],
    },
  ],
  Transitions: [
    { ID: 401, WorkflowDefinitionID: 11, FromNodeID: null, ToNodeID: 101, Action: 'submit' },
    { ID: 402, WorkflowDefinitionID: 11, FromNodeID: 101, ToNodeID: 102, Action: 'approve' },
  ],
})

describe('useWorkflowDefinitionAdmin', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('loads templates and versions into local state', async () => {
    const templates: WorkflowDefinitionTemplate[] = [
      { id: 7, business_group_id: 3, code: 'lease-approval', name: 'Lease Approval', process_class: 'lease_contract', status: 'active' },
    ]
    const versions = [createDefinition()]
    const admin = useWorkflowDefinitionAdmin()

    vi.mocked(listWorkflowDefinitionTemplates).mockResolvedValue({ data: { templates } } as never)
    vi.mocked(listWorkflowDefinitionVersions).mockResolvedValue({ data: { definitions: versions } } as never)

    await admin.loadTemplates()
    await admin.loadVersions(7)

    expect(admin.templates.value).toEqual(templates)
    expect(admin.versions.value).toEqual(versions)
    expect(admin.selectedTemplateId.value).toBe(7)
  })

  it('normalizes fetched transitions to node codes for editing and save payloads', async () => {
    const definition = createDefinition()
    const admin = useWorkflowDefinitionAdmin()

    vi.mocked(getWorkflowDefinition).mockResolvedValue({ data: { definition } } as never)
    vi.mocked(saveWorkflowDefinitionDraft).mockResolvedValue({ data: { definition } } as never)

    await admin.loadDefinition(11)

    expect(admin.draft.value).toEqual({
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
          assignment_rules: [{ id: 301, workflow_node_id: 101, strategy_type: 'fixed_role', config_json: '{"role_id":2001}' }],
        },
        {
          function_id: 1002,
          role_id: 2002,
          step_order: 2,
          code: 'MANAGER_APPROVE',
          name: 'Manager Approve',
          can_submit_to_manager: true,
          validates_after_confirm: true,
          prints_after_confirm: false,
          process_class: 'lease_contract',
          assignment_rules: [],
        },
      ],
      transitions: [
        { from_node_code: null, to_node_code: 'SUBMIT', action: 'submit' },
        { from_node_code: 'SUBMIT', to_node_code: 'MANAGER_APPROVE', action: 'approve' },
      ],
    })

    await admin.saveDraft()

    expect(saveWorkflowDefinitionDraft).toHaveBeenCalledWith(11, {
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
          assignment_rules: [{ strategy_type: 'fixed_role', config_json: '{"role_id":2001}' }],
        },
        {
          function_id: 1002,
          role_id: 2002,
          step_order: 2,
          code: 'MANAGER_APPROVE',
          name: 'Manager Approve',
          can_submit_to_manager: true,
          validates_after_confirm: true,
          prints_after_confirm: false,
          process_class: 'lease_contract',
          assignment_rules: [],
        },
      ],
      transitions: [
        { from_node_code: null, to_node_code: 'SUBMIT', action: 'submit' },
        { from_node_code: 'SUBMIT', to_node_code: 'MANAGER_APPROVE', action: 'approve' },
      ],
    })
  })

  it('preserves validation payloads on validate failures', async () => {
    const validation: WorkflowDefinitionValidationResult = {
      valid: false,
      issues: [{ code: 'missing_nodes', field: 'nodes', message: 'workflow definition must contain at least one node' }],
    }
    const admin = useWorkflowDefinitionAdmin()

    vi.mocked(getWorkflowDefinition).mockResolvedValue({ data: { definition: createDefinition() } } as never)
    vi.mocked(validateWorkflowDefinition).mockRejectedValue(Object.assign(new Error('invalid'), { validation }) as never)

    await admin.loadDefinition(11)

    await expect(admin.runValidation()).rejects.toThrow('invalid')
    expect(admin.validation.value).toEqual(validation)
  })

  it('preserves validation payloads on publish failures', async () => {
    const validation: WorkflowDefinitionValidationResult = {
      valid: false,
      issues: [{ code: 'transition_gap', field: 'transitions', message: 'at least one approval transition is required' }],
    }
    const admin = useWorkflowDefinitionAdmin()

    vi.mocked(getWorkflowDefinition).mockResolvedValue({ data: { definition: createDefinition() } } as never)
    vi.mocked(publishWorkflowDefinition).mockRejectedValue(Object.assign(new Error('publish failed'), { validation }) as never)

    await admin.loadDefinition(11)

    await expect(admin.publish()).rejects.toThrow('publish failed')
    expect(admin.validation.value).toEqual(validation)
  })

  it('updates local state after deactivate and rollback flows', async () => {
    const activeTemplate: WorkflowDefinitionTemplate = {
      id: 7,
      business_group_id: 3,
      code: 'lease-approval',
      name: 'Lease Approval',
      process_class: 'lease_contract',
      status: 'active',
      published_definition_id: 11,
      published_version_number: 2,
    }
    const deactivatedTemplate: WorkflowDefinitionTemplate = {
      ...activeTemplate,
      published_definition_id: undefined,
      published_version_number: undefined,
    }
    const rolledBackDefinition: WorkflowDefinitionDetail = {
      ...createDefinition(),
      LifecycleStatus: 'published',
      VersionNumber: 1,
    }
    const admin = useWorkflowDefinitionAdmin()

    vi.mocked(listWorkflowDefinitionTemplates).mockResolvedValue({ data: { templates: [activeTemplate] } } as never)
    vi.mocked(getWorkflowDefinition).mockResolvedValue({ data: { definition: createDefinition() } } as never)
    vi.mocked(deactivateWorkflowDefinitionTemplate).mockResolvedValue({ data: { template: deactivatedTemplate } } as never)
    vi.mocked(rollbackWorkflowDefinitionTemplate).mockResolvedValue({ data: { definition: rolledBackDefinition } } as never)

    await admin.loadTemplates()
    await admin.loadDefinition(11)
    await admin.deactivate()
    await admin.rollback(11)

    expect(admin.templates.value).toEqual([deactivatedTemplate])
    expect(admin.definition.value).toEqual(rolledBackDefinition)
    expect(admin.selectedDefinitionId.value).toBe(11)
  })
})
