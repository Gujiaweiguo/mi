import { mount } from '@vue/test-utils'
import ElementPlus from 'element-plus'
import { nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import WorkflowAdminView from './WorkflowAdminView.vue'

const { canAccessMock } = vi.hoisted(() => ({
  canAccessMock: vi.fn(),
}))

vi.mock('../stores/auth', () => ({
  useAuthStore: () => ({
    canAccess: canAccessMock,
  }),
}))

const mountView = () =>
  mount(WorkflowAdminView, {
    global: {
      plugins: [i18n, ElementPlus],
      stubs: {
        WorkflowDefinitionAdminPanel: {
          template: '<div data-testid="workflow-definition-admin-panel-stub">definition panel</div>',
        },
        WorkflowRuntimeAdminPanel: {
          template: '<div data-testid="workflow-runtime-admin-panel-stub">runtime panel</div>',
        },
      },
    },
  })

describe('WorkflowAdminView', () => {
  beforeEach(() => {
    i18n.global.locale.value = 'en-US'
    canAccessMock.mockReset().mockImplementation((functionCode: string) => functionCode === 'workflow.definition' || functionCode === 'workflow.admin')
  })

  it('renders definition and runtime tabs and defaults to the definition tab', async () => {
    const wrapper = mountView()
    await nextTick()

    expect(wrapper.find('[data-testid="workflow-admin-tabs"]').exists()).toBe(true)
    expect(wrapper.get('[data-testid="workflow-admin-tab-label-definitions"]').text()).toContain('Definition Administration')
    expect(wrapper.get('[data-testid="workflow-admin-tab-label-runtime"]').text()).toContain('Runtime Instances')
    expect(wrapper.find('[data-testid="workflow-definition-admin-panel-stub"]').exists()).toBe(true)
    expect(wrapper.find('[data-testid="workflow-runtime-admin-panel-stub"]').exists()).toBe(false)
  })

  it('switches to the runtime tab', async () => {
    const wrapper = mountView()
    await wrapper.get('[data-testid="workflow-admin-tab-label-runtime"]').trigger('click')
    await nextTick()

    expect(wrapper.find('[data-testid="workflow-definition-admin-panel-stub"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-runtime-admin-panel-stub"]').exists()).toBe(true)
  })

  it('shows only the runtime tab for runtime-only users', async () => {
    canAccessMock.mockImplementation((functionCode: string) => functionCode === 'workflow.admin')

    const wrapper = mountView()
    await nextTick()

    expect(wrapper.find('[data-testid="workflow-admin-tab-label-definitions"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-admin-tab-label-runtime"]').exists()).toBe(true)
    expect(wrapper.find('[data-testid="workflow-runtime-admin-panel-stub"]').exists()).toBe(true)
  })

  it('shows only the definitions tab for definition-only users', async () => {
    canAccessMock.mockImplementation((functionCode: string) => functionCode === 'workflow.definition')

    const wrapper = mountView()
    await nextTick()

    expect(wrapper.find('[data-testid="workflow-admin-tab-label-definitions"]').exists()).toBe(true)
    expect(wrapper.find('[data-testid="workflow-admin-tab-label-runtime"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-definition-admin-panel-stub"]').exists()).toBe(true)
  })

  it('renders no workflow admin panels when no workflow permissions are granted', async () => {
    canAccessMock.mockReturnValue(false)

    const wrapper = mountView()
    await nextTick()

    expect(wrapper.find('[data-testid="workflow-admin-tab-label-definitions"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-admin-tab-label-runtime"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-definition-admin-panel-stub"]').exists()).toBe(false)
    expect(wrapper.find('[data-testid="workflow-runtime-admin-panel-stub"]').exists()).toBe(false)
  })
})
