import { mount } from '@vue/test-utils'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import LeaseCreateView from './LeaseCreateView.vue'

const push = vi.fn()
const replace = vi.fn()
const mockRoute = {
  name: 'lease-contracts-amend',
  params: { id: '42' },
  fullPath: '/lease/contracts/42/amend',
}

vi.mock('vue-router', () => ({
  useRoute: () => mockRoute,
  useRouter: () => ({ push, replace }),
}))

vi.mock('../api/masterdata', () => ({
  listCustomers: vi.fn(),
  listBrands: vi.fn(),
}))

vi.mock('../api/org', () => ({
  listDepartments: vi.fn(),
  listStores: vi.fn(),
}))

vi.mock('../api/lease', () => ({
  amendLease: vi.fn(),
  createLease: vi.fn(),
  getLease: vi.fn(),
}))

import { listBrands, listCustomers } from '../api/masterdata'
import { amendLease, createLease, getLease } from '../api/lease'
import { listDepartments, listStores } from '../api/org'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () => h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary), slots.actions?.()])
  },
})

const ElCardStub = defineComponent({
  setup(_, { attrs, slots }) {
    return () => h('section', attrs, [slots.header?.(), slots.default?.()])
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props, { attrs }) {
    return () => h('div', attrs, `${props.title} ${props.description}`.trim())
  },
})

const ElButtonStub = defineComponent({
  setup(_, { attrs, slots }) {
    return () => h('button', attrs, slots.default?.())
  },
})

const ElTagStub = defineComponent({
  setup(_, { attrs, slots }) {
    return () => h('span', attrs, slots.default?.())
  },
})

const ElFormStub = defineComponent({
  setup(_, { attrs, slots, expose }) {
    expose({ validate: vi.fn().mockResolvedValue(true) })

    return () => h('form', attrs, slots.default?.())
  },
})

const ElFormItemStub = defineComponent({
  setup(_, { attrs, slots }) {
    return () => h('div', attrs, slots.default?.())
  },
})

const ElInputStub = defineComponent({
  props: {
    modelValue: { type: [String, Number], default: '' },
    disabled: { type: Boolean, default: false },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        disabled: props.disabled,
        value: props.modelValue ?? '',
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElInputNumberStub = defineComponent({
  props: {
    modelValue: { type: Number, default: null },
    disabled: { type: Boolean, default: false },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        disabled: props.disabled,
        value: props.modelValue ?? '',
        onInput: (event: Event) => {
          const value = (event.target as HTMLInputElement).value
          emit('update:modelValue', value === '' ? null : Number(value))
        },
      })
  },
})

const ElSelectStub = defineComponent({
  props: {
    modelValue: { type: [String, Number], default: '' },
    disabled: { type: Boolean, default: false },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit, slots }) {
    return () =>
      h(
        'select',
        {
          ...attrs,
          disabled: props.disabled,
          value: props.modelValue ?? '',
          onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLSelectElement).value),
        },
        slots.default?.(),
      )
  },
})

const ElOptionStub = defineComponent({
  props: {
    label: { type: String, default: '' },
    value: { type: [String, Number], required: true },
  },
  setup(props) {
    return () => h('option', { value: props.value }, props.label)
  },
})

const ElDatePickerStub = defineComponent({
  props: {
    modelValue: { type: String, default: '' },
    disabled: { type: Boolean, default: false },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        disabled: props.disabled,
        value: props.modelValue,
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElCheckboxStub = defineComponent({
  props: {
    modelValue: { type: Boolean, default: false },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit, slots }) {
    return () =>
      h('label', [
        h('input', {
          ...attrs,
          type: 'checkbox',
          checked: props.modelValue,
          onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).checked),
        }),
        slots.default?.(),
      ])
  },
})

const ElEmptyStub = defineComponent({
  setup(_, { attrs }) {
    return () => h('div', attrs)
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

const buildSourceLeaseResponse = () => ({
  data: {
    lease: {
      id: 42,
      amended_from_id: null,
      lease_no: 'SRC-LEASE-42',
      subtype: 'joint_operation',
      department_id: 7,
      store_id: 12,
      building_id: 88,
      customer_id: 1001,
      brand_id: 2002,
      trade_id: 3003,
      management_type_id: 4004,
      tenant_name: 'Tenant A',
      start_date: '2026-01-01',
      end_date: '2026-12-31',
      status: 'active',
      workflow_instance_id: null,
      effective_version: 1,
      submitted_at: '2026-01-02T00:00:00Z',
      approved_at: '2026-01-03T00:00:00Z',
      billing_effective_at: '2026-01-03T00:00:00Z',
      terminated_at: null,
      created_by: 1,
      updated_by: 1,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-03T00:00:00Z',
      joint_operation: {
        bill_cycle: 30,
        rent_inc: ' 5% annual ',
        account_cycle: 30,
        tax_rate: 0.06,
        tax_type: 1,
        settlement_currency_type_id: 1,
        in_tax_rate: 0.03,
        out_tax_rate: 0.06,
        month_settle_days: 10,
        late_pay_interest_rate: 0.01,
        interest_grace_days: 5,
      },
      ad_boards: [],
      area_grounds: [],
      units: [
        {
          id: 1,
          lease_contract_id: 42,
          unit_id: 101,
          rent_area: 88.5,
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
        {
          id: 2,
          lease_contract_id: 42,
          unit_id: 202,
          rent_area: 64.25,
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
      ],
      terms: [
        {
          id: 11,
          lease_contract_id: 42,
          term_type: 'rent',
          billing_cycle: 'monthly',
          currency_type_id: 1,
          amount: 12000,
          effective_from: '2026-01-01',
          effective_to: '2026-06-30',
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
        {
          id: 12,
          lease_contract_id: 42,
          term_type: 'deposit',
          billing_cycle: 'monthly',
          currency_type_id: 1,
          amount: 24000,
          effective_from: '2026-01-01',
          effective_to: '2026-12-31',
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
      ],
    },
  },
})

const mountView = () =>
  mount(LeaseCreateView, {
    global: {
      plugins: [i18n],
      stubs: {
        PageSection: PageSectionStub,
        ElAlert: ElAlertStub,
        ElButton: ElButtonStub,
        ElTag: ElTagStub,
        ElCard: ElCardStub,
        ElForm: ElFormStub,
        ElFormItem: ElFormItemStub,
        ElInput: ElInputStub,
        ElInputNumber: ElInputNumberStub,
        ElSelect: ElSelectStub,
        ElOption: ElOptionStub,
        ElDatePicker: ElDatePickerStub,
        ElCheckbox: ElCheckboxStub,
        ElEmpty: ElEmptyStub,
      },
      directives: {
        loading: {},
      },
    },
  })

describe('LeaseCreateView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    i18n.global.locale.value = 'en-US'

    mockRoute.name = 'lease-contracts-amend'
    mockRoute.params = { id: '42' }
    mockRoute.fullPath = '/lease/contracts/42/amend'

    vi.mocked(listCustomers).mockResolvedValue({ data: { customers: [] } } as never)
    vi.mocked(listBrands).mockResolvedValue({ data: { brands: [] } } as never)
    vi.mocked(listDepartments).mockResolvedValue({
      data: { departments: [{ id: 7, code: 'D07', name: 'Dept 7', level: 1, status: 'active', parent_id: null, type_id: 1 }] },
    } as never)
    vi.mocked(listStores).mockResolvedValue({
      data: { stores: [{ id: 12, department_id: 7, code: 'S12', name: 'Store 12', short_name: 'Store 12', status: 'active' }] },
    } as never)
    vi.mocked(getLease).mockResolvedValue(buildSourceLeaseResponse() as never)
  })

  it('prefills amendment mode from the source lease including subtype, unit, and term data', async () => {
    const wrapper = mountView()

    await flushPromises()

    expect(getLease).toHaveBeenCalledWith(42)
    expect(wrapper.text()).toContain('Create lease amendment draft')
    expect(wrapper.text()).toContain('Source lease SRC-LEASE-42')
    expect((wrapper.get('[data-testid="lease-number-input"]').element as HTMLInputElement).value).toBe('SRC-LEASE-42')
    expect((wrapper.get('[data-testid="lease-tenant-name-input"]').element as HTMLInputElement).value).toBe('Tenant A')
    expect((wrapper.get('[data-testid="lease-unit-id-input"]').element as HTMLInputElement).value).toBe('101')
    expect((wrapper.get('[data-testid="lease-rent-area-input"]').element as HTMLInputElement).value).toBe('88.5')
    expect((wrapper.get('[data-testid="lease-amount-input"]').element as HTMLInputElement).value).toBe('12000')
    expect((wrapper.get('[data-testid="lease-effective-from-input"]').element as HTMLInputElement).value).toBe('2026-01-01')
    expect((wrapper.get('[data-testid="lease-effective-to-input"]').element as HTMLInputElement).value).toBe('2026-06-30')
    expect((wrapper.get('[data-testid="lease-joint-rent-inc-input"]').element as HTMLInputElement).value).toBe(' 5% annual ')
  })

  it('submits amendment drafts through amendLease and opens the amended contract detail', async () => {
    vi.mocked(amendLease).mockResolvedValue({
      data: {
        lease: {
          id: 108,
        },
      },
    } as never)

    const wrapper = mountView()

    await flushPromises()

    await wrapper.get('[data-testid="lease-number-input"]').setValue('SRC-LEASE-42-A')
    await wrapper.get('[data-testid="lease-unit-id-input"]').setValue('303')
    await wrapper.get('[data-testid="lease-rent-area-input"]').setValue('91.25')
    await wrapper.get('[data-testid="lease-amount-input"]').setValue('15000')
    await wrapper.get('[data-testid="lease-joint-rent-inc-input"]').setValue(' 8% annual ')
    await wrapper.get('[data-testid="lease-create-submit-button"]').trigger('click')

    expect(createLease).not.toHaveBeenCalled()
    expect(amendLease).toHaveBeenCalledWith(
      42,
      expect.objectContaining({
        lease_no: 'SRC-LEASE-42-A',
        building_id: 88,
        units: [
          { unit_id: 303, rent_area: 91.25 },
          { unit_id: 202, rent_area: 64.25 },
        ],
        terms: [
          {
            term_type: 'rent',
            billing_cycle: 'monthly',
            currency_type_id: 1,
            amount: 15000,
            effective_from: '2026-01-01',
            effective_to: '2026-06-30',
          },
          {
            term_type: 'deposit',
            billing_cycle: 'monthly',
            currency_type_id: 1,
            amount: 24000,
            effective_from: '2026-01-01',
            effective_to: '2026-12-31',
          },
        ],
        joint_operation: expect.objectContaining({
          rent_inc: '8% annual',
        }),
      }),
    )
    expect(replace).toHaveBeenCalledWith({ name: 'lease-contract-detail', params: { id: '108' } })
  })
})
