import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import HealthView from './HealthView.vue'

vi.mock('../api/http', () => ({
  http: {
    get: vi.fn(),
  },
}))

import { http } from '../api/http'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () =>
      h('section', [
        h('span', props.eyebrow),
        h('h1', props.title),
        h('p', props.summary),
        h('div', slots.default?.()),
      ])
  },
})

const ElTagStub = defineComponent({
  props: {
    type: { type: String, default: '' },
    effect: { type: String, default: '' },
  },
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const ElButtonStub = defineComponent({
  props: {
    disabled: { type: Boolean, default: false },
  },
  emits: ['click'],
  setup(props, { emit, slots }) {
    return () =>
      h(
        'button',
        {
          disabled: props.disabled,
          onClick: (event: MouseEvent) => emit('click', event),
        },
        slots.default?.(),
      )
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
    type: { type: String, default: '' },
    closable: { type: Boolean, default: true },
    showIcon: { type: Boolean, default: false },
  },
  setup(props) {
    return () =>
      h('div', { 'data-testid': 'health-error-alert' }, `${props.title}: ${props.description}`.trim())
  },
})

const ElCardStub = defineComponent({
  setup(_, { slots }) {
    return () => h('section', [h('div', slots.header?.()), h('div', slots.default?.())])
  },
})

const ElDescriptionsStub = defineComponent({
  props: {
    column: { type: Number, default: 1 },
    border: { type: Boolean, default: false },
  },
  setup(_, { slots }) {
    return () => h('dl', slots.default?.())
  },
})

const ElDescriptionsItemStub = defineComponent({
  props: {
    label: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () => h('div', [h('dt', props.label), h('dd', slots.default?.())])
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

const mountHealthView = () =>
  mount(HealthView, {
    global: {
      plugins: [i18n],
      stubs: {
        PageSection: PageSectionStub,
        ElTag: ElTagStub,
        ElButton: ElButtonStub,
        ElAlert: ElAlertStub,
        ElCard: ElCardStub,
        ElDescriptions: ElDescriptionsStub,
        ElDescriptionsItem: ElDescriptionsItemStub,
      },
      directives: {
        loading: () => {},
      },
    },
  })

describe('HealthView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('loads and shows healthy state on mount', async () => {
    vi.mocked(http.get).mockResolvedValue({
      status: 200,
      data: { status: 'ok' },
    })

    const wrapper = mountHealthView()
    await flushPromises()

    expect(http.get).toHaveBeenCalledWith('/health')
    expect(http.get).toHaveBeenCalledTimes(1)

    // Status tag should show "Healthy"
    const statusTag = wrapper.get('[data-testid="health-status-tag"]')
    expect(statusTag.text()).toContain('Healthy')

    // Response payload should show the JSON
    const payload = wrapper.get('[data-testid="health-response-payload"]')
    expect(payload.text()).toContain('"status": "ok"')

    // No error alert
    expect(wrapper.find('[data-testid="health-error-alert"]').exists()).toBe(false)
  })

  it('shows error state when health check fails', async () => {
    vi.mocked(http.get).mockRejectedValue(new Error('Connection refused'))

    const wrapper = mountHealthView()
    await flushPromises()

    // Status tag should show "Unavailable"
    const statusTag = wrapper.get('[data-testid="health-status-tag"]')
    expect(statusTag.text()).toContain('Unavailable')

    // Error alert should be visible
    const errorAlert = wrapper.get('[data-testid="health-error-alert"]')
    expect(errorAlert.text()).toContain('Connection refused')

    // Reaching this assertion through get() already proves the alert exists.
    expect(errorAlert.text().length).toBeGreaterThan(0)
  })

  it('refresh button re-runs health check', async () => {
    vi.mocked(http.get).mockResolvedValue({
      status: 200,
      data: { status: 'ok' },
    })

    const wrapper = mountHealthView()
    await flushPromises()

    expect(http.get).toHaveBeenCalledTimes(1)

    // Click refresh button
    await wrapper.get('[data-testid="health-refresh-button"]').trigger('click')
    await flushPromises()

    expect(http.get).toHaveBeenCalledTimes(2)
    expect(http.get).toHaveBeenNthCalledWith(2, '/health')
  })
})
