import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAuthStore } from '../stores/auth'
import LoginView from './LoginView.vue'

const { replaceMock, routeState, validateMock } = vi.hoisted(() => ({
  replaceMock: vi.fn(),
  routeState: { query: {} as Record<string, unknown> },
  validateMock: vi.fn(),
}))

vi.mock('vue-router', () => ({
  useRouter: () => ({ replace: replaceMock }),
  useRoute: () => routeState,
}))

const LocaleSwitcherStub = defineComponent({
  setup() {
    return () => h('div', { 'data-testid': 'login-locale-switcher-stub' })
  },
})

const ElCardStub = defineComponent({
  setup(_, { slots }) {
    return () =>
      h('section', [
        h('div', slots.header?.()),
        h('div', slots.default?.()),
      ])
  },
})

const ElTagStub = defineComponent({
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props, { attrs }) {
    return () =>
      h(
        'div',
        { 'data-testid': attrs['data-testid'] ?? 'login-alert-stub' },
        `${props.title} ${props.description}`.trim(),
      )
  },
})

const ElFormStub = defineComponent({
  setup(_, { slots, expose }) {
    expose({ validate: validateMock })

    return () =>
      h(
        'form',
        {
          onSubmit: (event: Event) => event.preventDefault(),
        },
        slots.default?.(),
      )
  },
})

const ElFormItemStub = defineComponent({
  setup(_, { slots }) {
    return () => h('div', slots.default?.())
  },
})

const ElInputStub = defineComponent({
  props: {
    modelValue: { type: String, default: '' },
    type: { type: String, default: 'text' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        type: props.type,
        value: props.modelValue,
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElButtonStub = defineComponent({
  props: {
    disabled: { type: Boolean, default: false },
    loading: { type: Boolean, default: false },
  },
  emits: ['click'],
  setup(props, { attrs, emit, slots }) {
    return () =>
      h(
        'button',
        {
          ...attrs,
          disabled: props.disabled || props.loading,
          onClick: (event: MouseEvent) => emit('click', event),
        },
        slots.default?.(),
      )
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('LoginView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    routeState.query = {}
    validateMock.mockResolvedValue(true)
  })

  it('mounts without error', () => {
    const wrapper = mount(LoginView, {
      global: {
        plugins: [i18n],
        stubs: {
          LocaleSwitcher: LocaleSwitcherStub,
          ElCard: ElCardStub,
          ElTag: ElTagStub,
          ElAlert: ElAlertStub,
          ElForm: ElFormStub,
          ElFormItem: ElFormItemStub,
          ElInput: ElInputStub,
          ElButton: ElButtonStub,
        },
      },
    })

    expect(wrapper.get('[data-testid="login-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Sign in to continue')
  })

  it('submits credentials through the auth store and redirects to the route target', async () => {
    routeState.query = { redirect: '/lease/contracts' }

    const authStore = useAuthStore()
    const loginSpy = vi.spyOn(authStore, 'login').mockResolvedValue({ id: 1 } as never)

    const wrapper = mount(LoginView, {
      global: {
        plugins: [i18n],
        stubs: {
          LocaleSwitcher: LocaleSwitcherStub,
          ElCard: ElCardStub,
          ElTag: ElTagStub,
          ElAlert: ElAlertStub,
          ElForm: ElFormStub,
          ElFormItem: ElFormItemStub,
          ElInput: ElInputStub,
          ElButton: ElButtonStub,
        },
      },
    })

    await wrapper.get('[data-testid="login-username-input"]').setValue('alice')
    await wrapper.get('[data-testid="login-password-input"]').setValue('secret')
    await wrapper.get('[data-testid="login-submit-button"]').trigger('click')
    await flushPromises()

    expect(validateMock).toHaveBeenCalledTimes(1)
    expect(loginSpy).toHaveBeenCalledWith({ username: 'alice', password: 'secret' })
    expect(replaceMock).toHaveBeenCalledWith('/lease/contracts')
  })

  it('renders an error alert when login fails', async () => {
    const authStore = useAuthStore()
    vi.spyOn(authStore, 'login').mockRejectedValue(new Error('Invalid credentials'))

    const wrapper = mount(LoginView, {
      global: {
        plugins: [i18n],
        stubs: {
          LocaleSwitcher: LocaleSwitcherStub,
          ElCard: ElCardStub,
          ElTag: ElTagStub,
          ElAlert: ElAlertStub,
          ElForm: ElFormStub,
          ElFormItem: ElFormItemStub,
          ElInput: ElInputStub,
          ElButton: ElButtonStub,
        },
      },
    })

    await wrapper.get('[data-testid="login-username-input"]').setValue('alice')
    await wrapper.get('[data-testid="login-password-input"]').setValue('wrong-password')
    await wrapper.get('[data-testid="login-submit-button"]').trigger('click')
    await flushPromises()

    expect(replaceMock).not.toHaveBeenCalled()
    expect(wrapper.get('[data-testid="login-error-alert"]').text()).toContain('Invalid credentials')
  })
})
