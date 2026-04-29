import { mount } from '@vue/test-utils'
import ElementPlus from 'element-plus'
import { nextTick } from 'vue'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import WorkflowAdminView from './WorkflowAdminView.vue'

const { confirmMock, messageErrorMock, messageSuccessMock } = vi.hoisted(() => ({
  confirmMock: vi.fn(),
  messageErrorMock: vi.fn(),
  messageSuccessMock: vi.fn(),
}))

vi.mock('element-plus', async () => {
  const actual = await vi.importActual<typeof import('element-plus')>('element-plus')

  return {
    ...actual,
    ElMessage: {
      success: messageSuccessMock,
      error: messageErrorMock,
    },
    ElMessageBox: {
      confirm: confirmMock,
    },
  }
})

vi.mock('../api/workflow', () => ({
  approveWorkflow: vi.fn(),
  getReminderHistory: vi.fn(),
  getWorkflowAuditHistory: vi.fn(),
  getWorkflowInstance: vi.fn(),
  listWorkflowDefinitions: vi.fn(),
  listWorkflowInstances: vi.fn(),
  rejectWorkflow: vi.fn(),
  resubmitWorkflow: vi.fn(),
  runReminders: vi.fn(),
}))

import {
  approveWorkflow,
  getReminderHistory,
  getWorkflowAuditHistory,
  getWorkflowInstance,
  listWorkflowDefinitions,
  listWorkflowInstances,
  rejectWorkflow,
  resubmitWorkflow,
  runReminders,
  type AuditEntry,
  type WorkflowDefinition,
  type WorkflowInstanceListItem,
} from '../api/workflow'

const definitions: WorkflowDefinition[] = [
  {
    ID: 1,
    Code: 'LEASE_APPROVAL',
    Name: 'Lease approval',
    ProcessClass: 'lease',
  },
]

const instances: WorkflowInstanceListItem[] = [
  {
    id: 101,
    workflow_definition_id: 1,
    document_type: 'lease_contract',
    document_id: 7001,
    status: 'pending',
    current_node_id: 3,
    current_step_order: 1,
    current_cycle: 1,
    version: 1,
    submitted_by: 10,
    submitted_at: '2026-02-01T00:00:00Z',
    completed_at: null,
    current_node_code: 'lease_review',
    created_at: '2026-02-01T00:00:00Z',
  },
  {
    id: 102,
    workflow_definition_id: 1,
    document_type: 'invoice',
    document_id: 7002,
    status: 'rejected',
    current_node_id: 4,
    current_step_order: 2,
    current_cycle: 1,
    version: 1,
    submitted_by: 11,
    submitted_at: '2026-02-02T00:00:00Z',
    completed_at: '2026-02-03T00:00:00Z',
    current_node_code: 'finance_review',
    created_at: '2026-02-02T00:00:00Z',
  },
  {
    id: 103,
    workflow_definition_id: 1,
    document_type: 'invoice',
    document_id: 7003,
    status: 'approved',
    current_node_id: 5,
    current_step_order: 3,
    current_cycle: 1,
    version: 1,
    submitted_by: 12,
    submitted_at: '2026-02-04T00:00:00Z',
    completed_at: '2026-02-05T00:00:00Z',
    current_node_code: 'archived',
    created_at: '2026-02-04T00:00:00Z',
  },
]

const auditHistory: AuditEntry[] = [
  {
    id: 501,
    workflow_instance_id: 101,
    action: 'approve',
    actor_user_id: 88,
    from_status: 'pending',
    to_status: 'approved',
    from_step_order: 1,
    to_step_order: 2,
    comment: 'Looks good',
    idempotency_key: 'audit-1',
    created_at: '2026-02-06T00:00:00Z',
  },
]

const reminderHistory = [
  {
    id: 601,
    outcome: 'sent',
    reason_code: 'awaiting_manager',
    sent_at: '2026-02-07T00:00:00Z',
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const createDeferred = <T,>() => {
  let resolve!: (value: T) => void
  let reject!: (reason?: unknown) => void

  const promise = new Promise<T>((innerResolve, innerReject) => {
    resolve = innerResolve
    reject = innerReject
  })

  return { promise, resolve, reject }
}

const mountView = async () => {
  const wrapper = mount(WorkflowAdminView, {
    attachTo: document.body,
    global: {
      plugins: [i18n, ElementPlus],
      stubs: {
        ElDrawer: {
          name: 'ElDrawer',
          props: ['modelValue', 'title', 'direction', 'size'],
          template: '<div v-if="modelValue" data-testid="workflow-detail-drawer-stub"><slot /></div>',
        },
        teleport: true,
      },
    },
  })

  await flushPromises()

  return wrapper
}

describe('WorkflowAdminView', () => {
  let randomUUIDSpy: ReturnType<typeof vi.spyOn>

  beforeEach(() => {
    vi.clearAllMocks()
    i18n.global.locale.value = 'en-US'

    randomUUIDSpy = vi.spyOn(globalThis.crypto, 'randomUUID').mockReturnValue('uuid-123')

    vi.mocked(listWorkflowDefinitions).mockResolvedValue({
      data: { definitions },
    } as never)
    vi.mocked(listWorkflowInstances).mockResolvedValue({
      data: { instances },
    } as never)
    vi.mocked(getWorkflowInstance).mockResolvedValue({
      data: {
        instance: {
          ...instances[0],
          completed_at: '2026-02-08T00:00:00Z',
        },
      },
    } as never)
    vi.mocked(getWorkflowAuditHistory).mockResolvedValue({
      data: { history: auditHistory },
    } as never)
    vi.mocked(getReminderHistory).mockResolvedValue({
      data: { reminders: reminderHistory },
    } as never)
    vi.mocked(approveWorkflow).mockResolvedValue({ data: {} } as never)
    vi.mocked(rejectWorkflow).mockResolvedValue({ data: {} } as never)
    vi.mocked(resubmitWorkflow).mockResolvedValue({ data: {} } as never)
    vi.mocked(runReminders).mockResolvedValue({ data: {} } as never)
    confirmMock.mockResolvedValue(undefined)
  })

  afterEach(() => {
    randomUUIDSpy.mockRestore()
    document.body.innerHTML = ''
  })

  it('shows resubmit only for rejected instances and calls resubmitWorkflow', async () => {
    const wrapper = await mountView()

    expect(wrapper.find('[data-testid="workflow-resubmit-button-101"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-resubmit-button-103"]').exists()).toBe(false)
    expect(wrapper.get('[data-testid="workflow-resubmit-button-102"]').text()).toContain('Resubmit')

    await wrapper.get('[data-testid="workflow-resubmit-button-102"]').trigger('click')
    await flushPromises()

    expect(resubmitWorkflow).toHaveBeenCalledWith(102, { idempotency_key: 'uuid-123' })
    expect(listWorkflowInstances).toHaveBeenCalledTimes(2)
    expect(messageSuccessMock).toHaveBeenCalledWith('Resubmitted')
  })

  it('opens the detail drawer and loads audit plus reminder history', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="workflow-detail-button-101"]').trigger('click')
    await flushPromises()

    expect(getWorkflowInstance).toHaveBeenCalledWith(101)
    expect(getWorkflowAuditHistory).toHaveBeenCalledWith(101)
    expect(getReminderHistory).toHaveBeenCalledWith(101)
    expect(wrapper.getComponent({ name: 'ElDrawer' }).props('modelValue')).toBe(true)
    expect(wrapper.getComponent({ name: 'ElDrawer' }).props('title')).toBe('Workflow Instance Details')
    expect(wrapper.get('[data-testid="workflow-detail-current-node"]').text()).toContain('lease_review')
    expect(wrapper.get('[data-testid="workflow-detail-audit-501"]').text()).toContain('Looks good')
    expect(wrapper.get('[data-testid="workflow-detail-reminder-601"]').text()).toContain('awaiting_manager')
  })

  it('runs reminder evaluation after confirmation and disables the trigger while pending', async () => {
    const deferred = createDeferred<{ data: object }>()
    vi.mocked(runReminders).mockReturnValueOnce(deferred.promise as never)

    const wrapper = await mountView()
    const runButton = wrapper.get('[data-testid="workflow-run-reminders-button"]')

    const clickPromise = runButton.trigger('click')
    await flushPromises()

    expect(confirmMock).toHaveBeenCalledTimes(1)
    expect(runReminders).toHaveBeenCalledTimes(1)
    expect((runButton.element as HTMLButtonElement).disabled).toBe(true)

    deferred.resolve({ data: {} })
    await clickPromise
    await flushPromises()

    expect(listWorkflowInstances).toHaveBeenCalledTimes(2)
    expect(messageSuccessMock).toHaveBeenCalledWith('Reminder evaluation has been triggered successfully')
  })

  it('keeps approve and reject actions working', async () => {
    randomUUIDSpy.mockReturnValueOnce('approve-uuid').mockReturnValueOnce('reject-uuid')

    const wrapper = await mountView()

    await wrapper.get('[data-testid="workflow-approve-button-101"]').trigger('click')
    await flushPromises()
    await wrapper.get('[data-testid="workflow-reject-button-101"]').trigger('click')
    await flushPromises()

    expect(approveWorkflow).toHaveBeenCalledWith(101, { idempotency_key: 'approve-uuid' })
    expect(rejectWorkflow).toHaveBeenCalledWith(101, { idempotency_key: 'reject-uuid' })
  })

  it('keeps approve and reject disabled for non-pending instances', async () => {
    const wrapper = await mountView()

    expect((wrapper.get('[data-testid="workflow-approve-button-102"]').element as HTMLButtonElement).disabled).toBe(true)
    expect((wrapper.get('[data-testid="workflow-reject-button-103"]').element as HTMLButtonElement).disabled).toBe(true)
  })
})
