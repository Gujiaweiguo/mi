import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import TaxExportsView from './TaxExportsView.vue'

vi.mock('../api/tax', () => ({
  exportTaxVouchers: vi.fn(),
  listTaxRuleSets: vi.fn(),
  upsertTaxRuleSet: vi.fn(),
}))

vi.mock('../composables/useDownload', () => ({
  downloadBlob: vi.fn(),
}))

import { downloadBlob } from '../composables/useDownload'
import { exportTaxVouchers, listTaxRuleSets, upsertTaxRuleSet } from '../api/tax'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props) {
    return () => h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary)])
  },
})

const FilterFormStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    busy: { type: Boolean, default: false },
    resetDisabled: { type: Boolean, default: false },
  },
  emits: ['reset', 'submit'],
  setup(props, { slots }) {
    return () => h('section', { 'data-testid': 'tax-filter-form-stub' }, [props.title, slots.default?.()])
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

const ElDatePickerStub = defineComponent({
  name: 'ElDatePicker',
  inheritAttrs: false,
  props: {
    modelValue: { type: String, default: '' },
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

const ElCheckboxStub = defineComponent({
  name: 'ElCheckbox',
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

const ruleSets = [
  {
    id: 1,
    code: 'RULE-001',
    name: 'Invoice rules',
    document_type: 'invoice',
    status: 'active',
    created_by: 1,
    updated_by: 1,
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
    rules: [
      {
        id: 101,
        rule_set_id: 1,
        sequence_no: 1,
        entry_side: 'debit',
        charge_type_filter: 'rent',
        account_number: '6001',
        account_name: 'Rental Income',
        explanation_template: 'Rent income',
        use_tenant_name: false,
        is_balancing_entry: false,
        created_at: '2026-01-01T00:00:00Z',
        updated_at: '2026-01-01T00:00:00Z',
      },
    ],
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(TaxExportsView, {
    global: {
      plugins: [i18n, ElementPlus],
      stubs: {
        PageSection: PageSectionStub,
        FilterForm: FilterFormStub,
        ElDialog: ElDialogStub,
        ElForm: ElFormStub,
        ElFormItem: ElFormItemStub,
        ElInput: ElInputStub,
        ElInputNumber: ElInputNumberStub,
        ElSelect: ElSelectStub,
        ElOption: ElOptionStub,
        ElDatePicker: ElDatePickerStub,
        ElCheckbox: ElCheckboxStub,
        ElButton: ElButtonStub,
      },
    },
  })

  await flushPromises()

  return wrapper
}

const getDialog = (wrapper: ReturnType<typeof mount>) => {
  const dialog = wrapper.getComponent({ name: 'ElDialog' })
  if (!dialog.props('modelValue')) {
    throw new Error('Expected tax rule set dialog to be open')
  }
  return dialog
}

describe('TaxExportsView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'

    vi.mocked(listTaxRuleSets).mockResolvedValue({
      data: {
        items: ruleSets,
        total: ruleSets.length,
        page: 1,
        page_size: 20,
      },
    } as never)
    vi.mocked(upsertTaxRuleSet).mockResolvedValue({ data: {} } as never)
    vi.mocked(exportTaxVouchers).mockResolvedValue({ data: new Uint8Array([1, 2, 3]) } as never)
  })

  it('renders and loads available rule sets on mount', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="tax-exports-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Tax exports')
    expect(listTaxRuleSets).toHaveBeenCalledTimes(1)
    expect(wrapper.text()).toContain('RULE-001')
  })

  it('creates a tax rule set through the dialog form', async () => {
    const wrapper = await mountView()

    const addRuleSetButton = wrapper.findAll('button').find((button) => button.text().includes('Add rule set'))
    expect(addRuleSetButton).toBeTruthy()

    await addRuleSetButton!.trigger('click')
    await flushPromises()

    const dialog = getDialog(wrapper)
    const inputs = dialog.findAllComponents({ name: 'ElInput' })
    const selects = dialog.findAllComponents({ name: 'ElSelect' })

    await inputs[0].get('input').setValue('RULE-002')
    await inputs[1].get('input').setValue('Receipt rules')
    selects[0].vm.$emit('update:modelValue', 'receipt')
    await inputs[2].get('input').setValue('service')
    await inputs[3].get('input').setValue('7001')
    await inputs[4].get('input').setValue('Receivables')
    await inputs[5].get('input').setValue('Receipt export')
    await flushPromises()

    const submitButton = dialog.findAll('button').find((button) => button.text().includes('Submit'))
    expect(submitButton).toBeTruthy()
    expect((submitButton!.element as HTMLButtonElement).disabled).toBe(false)

    await submitButton!.trigger('click')
    await flushPromises()

    expect(upsertTaxRuleSet).toHaveBeenCalledWith({
      code: 'RULE-002',
      name: 'Receipt rules',
      document_type: 'receipt',
      rules: [
        {
          sequence_no: 1,
          entry_side: 'debit',
          charge_type_filter: 'service',
          account_number: '7001',
          account_name: 'Receivables',
          explanation_template: 'Receipt export',
          use_tenant_name: false,
          is_balancing_entry: false,
        },
      ],
    })
    expect(listTaxRuleSets).toHaveBeenCalledTimes(2)
    expect(wrapper.text()).toContain('Tax rule set created')
  })

  it('exports vouchers for the selected rule set and date range', async () => {
    const wrapper = await mountView()

    const selects = wrapper.findAllComponents({ name: 'ElSelect' })
    const datePickers = wrapper.findAllComponents({ name: 'ElDatePicker' })

    selects[0].vm.$emit('update:modelValue', 'RULE-001')
    datePickers[0].vm.$emit('update:modelValue', '2026-01-01')
    datePickers[1].vm.$emit('update:modelValue', '2026-01-31')
    await flushPromises()

    await wrapper.get('[data-testid="tax-export-button"]').trigger('click')
    await flushPromises()

    expect(exportTaxVouchers).toHaveBeenCalledWith({
      rule_set_code: 'RULE-001',
      from_date: '2026-01-01',
      to_date: '2026-01-31',
    })
    expect(downloadBlob).toHaveBeenCalledWith(expect.any(Blob), 'tax-vouchers-RULE-001.xlsx')
  })

  it('keeps the rule-set submit action disabled until required values are provided', async () => {
    const wrapper = await mountView()

    const addRuleSetButton = wrapper.findAll('button').find((button) => button.text().includes('Add rule set'))
    expect(addRuleSetButton).toBeTruthy()

    await addRuleSetButton!.trigger('click')
    await flushPromises()

    const dialog = getDialog(wrapper)
    const submitButton = dialog.findAll('button').find((button) => button.text().includes('Submit'))
    expect(submitButton).toBeTruthy()
    expect((submitButton!.element as HTMLButtonElement).disabled).toBe(true)

    const inputs = dialog.findAllComponents({ name: 'ElInput' })
    inputs[0].vm.$emit('update:modelValue', 'RULE-003')
    inputs[1].vm.$emit('update:modelValue', 'Draft rules')
    await flushPromises()

    expect((submitButton!.element as HTMLButtonElement).disabled).toBe(true)
    expect(upsertTaxRuleSet).not.toHaveBeenCalled()
  })
})
