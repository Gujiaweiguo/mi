import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import BaseInfoAdminView from './BaseInfoAdminView.vue'

vi.mock('../api/baseinfo', () => ({
  createCurrencyType: vi.fn(),
  createShopType: vi.fn(),
  createStoreType: vi.fn(),
  createTradeDefinition: vi.fn(),
  listCurrencyTypes: vi.fn(),
  listShopTypes: vi.fn(),
  listStoreTypes: vi.fn(),
  listTradeDefinitions: vi.fn(),
  updateCurrencyType: vi.fn(),
  updateShopType: vi.fn(),
  updateStoreType: vi.fn(),
  updateTradeDefinition: vi.fn(),
}))

import {
  createStoreType,
  listCurrencyTypes,
  listShopTypes,
  listStoreTypes,
  listTradeDefinitions,
  updateCurrencyType,
} from '../api/baseinfo'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () =>
      h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary), h('div', slots.actions?.())])
  },
})

const ElDialogStub = defineComponent({
  name: 'ElDialog',
  props: {
    modelValue: { type: Boolean, default: false },
    title: { type: String, default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { slots }) {
    return () =>
      props.modelValue
        ? h('section', { 'data-testid': 'el-dialog-stub' }, [
            h('h2', props.title),
            slots.default?.(),
            h('footer', slots.footer?.()),
          ])
        : null
  },
})

const ElFormStub = defineComponent({
  name: 'ElForm',
  setup(_, { slots, expose }) {
    expose({ validate: () => Promise.resolve(true) })
    return () => h('form', slots.default?.())
  },
})

const ElFormItemStub = defineComponent({
  name: 'ElFormItem',
  setup(_, { slots }) {
    return () => h('div', slots.default?.())
  },
})

const ElInputStub = defineComponent({
  name: 'ElInput',
  inheritAttrs: false,
  props: {
    modelValue: { type: [String, Number], default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        value: props.modelValue ?? '',
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElInputNumberStub = defineComponent({
  name: 'ElInputNumber',
  inheritAttrs: false,
  props: {
    modelValue: { type: Number, default: undefined },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('div', { ...attrs }, [
        h('input', {
          type: 'number',
          value: props.modelValue ?? '',
          onInput: (event: Event) => {
            const rawValue = (event.target as HTMLInputElement).value
            emit('update:modelValue', rawValue === '' ? undefined : Number(rawValue))
          },
        }),
      ])
  },
})

const ElSelectStub = defineComponent({
  name: 'ElSelect',
  inheritAttrs: false,
  props: {
    modelValue: { type: [String, Number], default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'select',
        {
          ...attrs,
          value: props.modelValue ?? '',
          onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLSelectElement).value),
        },
        slots.default?.(),
      )
  },
})

const ElOptionStub = defineComponent({
  name: 'ElOption',
  props: {
    label: { type: String, default: '' },
    value: { type: [String, Number], default: '' },
  },
  setup(props) {
    return () => h('option', { value: props.value }, props.label)
  },
})

const ElSwitchStub = defineComponent({
  name: 'ElSwitch',
  inheritAttrs: false,
  props: {
    modelValue: { type: Boolean, default: false },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        type: 'checkbox',
        checked: props.modelValue,
        onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).checked),
      })
  },
})

const ElButtonStub = defineComponent({
  name: 'ElButton',
  inheritAttrs: false,
  props: {
    disabled: { type: Boolean, default: false },
    loading: { type: Boolean, default: false },
  },
  emits: ['click'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'button',
        {
          ...attrs,
          disabled: props.disabled || props.loading,
          onClick: () => emit('click'),
        },
        slots.default?.(),
      )
  },
})

const storeTypes = [
  { id: 1, code: 'OUTLET', name: 'Outlet', created_at: '2026-01-01T00:00:00Z', updated_at: '2026-01-01T00:00:00Z' },
]

const shopTypes = [
  {
    id: 2,
    code: 'LIFE',
    name: 'Lifestyle',
    color_hex: '#112233',
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const currencyTypes = [
  {
    id: 3,
    code: 'USD',
    name: 'US Dollar',
    is_local: false,
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const tradeDefinitions = [
  {
    id: 4,
    code: 'SPORTS',
    name: 'Sports',
    parent_id: null,
    level: 1,
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(BaseInfoAdminView, {
    global: {
      plugins: [i18n, ElementPlus],
      stubs: {
        PageSection: PageSectionStub,
        ElDialog: ElDialogStub,
        ElForm: ElFormStub,
        ElFormItem: ElFormItemStub,
        ElInput: ElInputStub,
        ElInputNumber: ElInputNumberStub,
        ElSelect: ElSelectStub,
        ElOption: ElOptionStub,
        ElSwitch: ElSwitchStub,
        ElButton: ElButtonStub,
      },
    },
  })

  await flushPromises()

  return wrapper
}

const getOpenDialog = (wrapper: ReturnType<typeof mount>) => {
  const dialog = wrapper.findAllComponents({ name: 'ElDialog' }).find((candidate) => candidate.props('modelValue'))
  if (!dialog) {
    throw new Error('Expected an open dialog')
  }
  return dialog
}

describe('BaseInfoAdminView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'

    vi.mocked(listStoreTypes).mockResolvedValue({ data: { store_types: storeTypes } } as never)
    vi.mocked(listShopTypes).mockResolvedValue({ data: { shop_types: shopTypes } } as never)
    vi.mocked(listCurrencyTypes).mockResolvedValue({ data: { currency_types: currencyTypes } } as never)
    vi.mocked(listTradeDefinitions).mockResolvedValue({ data: { trade_definitions: tradeDefinitions } } as never)

    vi.mocked(createStoreType).mockResolvedValue({
      data: {
        store_type: {
          id: 5,
          code: 'FLAGSHIP',
          name: 'Flagship',
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
      },
    } as never)

    vi.mocked(updateCurrencyType).mockResolvedValue({
      data: {
        currency_type: {
          ...currencyTypes[0],
          code: 'USDX',
          name: 'US Dollar Indexed',
        },
      },
    } as never)
  })

  it('renders and loads all base info catalogs on mount', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="baseinfo-admin-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Base info admin')
    expect(listStoreTypes).toHaveBeenCalledTimes(1)
    expect(listShopTypes).toHaveBeenCalledTimes(1)
    expect(listCurrencyTypes).toHaveBeenCalledTimes(1)
    expect(listTradeDefinitions).toHaveBeenCalledTimes(1)
  })

  it('creates store types from the primary catalog form', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="baseinfo-store-type-code-input"]').setValue('FLAGSHIP')
    await wrapper.get('[data-testid="baseinfo-store-type-name-input"]').setValue('Flagship')
    await flushPromises()

    await wrapper.get('[data-testid="baseinfo-store-type-create-button"]').trigger('click')
    await flushPromises()

    expect(createStoreType).toHaveBeenCalledWith({
      code: 'FLAGSHIP',
      name: 'Flagship',
    })
    expect(wrapper.text()).toContain('Store type created')
    expect(wrapper.text()).toContain('FLAGSHIP')
  })

  it('updates a second catalog type through the currency dialog flow', async () => {
    const wrapper = await mountView()

    const currencyTable = wrapper.findAllComponents({ name: 'ElTable' })[2]
    const currencyEditButton = currencyTable.findAll('button').find((button) => button.text().includes('Edit'))
    expect(currencyEditButton).toBeTruthy()

    await currencyEditButton!.trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    const dialogInputs = dialog.findAllComponents({ name: 'ElInput' })

    await dialogInputs[0].get('input').setValue('USDX')
    await dialogInputs[1].get('input').setValue('US Dollar Indexed')
    await flushPromises()

    const saveButton = dialog.findAll('button').find((button) => button.text().includes('Save'))
    expect(saveButton).toBeTruthy()

    await saveButton!.trigger('click')
    await flushPromises()

    expect(updateCurrencyType).toHaveBeenCalledWith(3, {
      code: 'USDX',
      name: 'US Dollar Indexed',
      is_local: false,
      status: 'active',
    })
  })
})
