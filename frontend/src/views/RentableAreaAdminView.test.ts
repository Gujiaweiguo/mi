import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import RentableAreaAdminView from './RentableAreaAdminView.vue'

vi.mock('../api/baseinfo', () => ({
  listShopTypes: vi.fn(),
  listUnitTypes: vi.fn(),
}))

vi.mock('../api/structure', () => ({
  listStructureAreas: vi.fn(),
  listStructureBuildings: vi.fn(),
  listStructureFloors: vi.fn(),
  listStructureStores: vi.fn(),
  listStructureUnits: vi.fn(),
  updateStructureUnit: vi.fn(),
}))

import { listShopTypes, listUnitTypes } from '../api/baseinfo'
import {
  listStructureAreas,
  listStructureBuildings,
  listStructureFloors,
  listStructureStores,
  listStructureUnits,
  updateStructureUnit,
} from '../api/structure'

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

const ElAlertStub = defineComponent({
  name: 'ElAlert',
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
    type: { type: String, default: 'info' },
  },
  setup(props) {
    return () => h('div', { 'data-type': props.type }, [h('strong', props.title), h('span', props.description)])
  },
})

const ElCardStub = defineComponent({
  name: 'ElCard',
  setup(_, { slots }) {
    return () => h('section', [slots.header?.(), slots.default?.()])
  },
})

const ElTagStub = defineComponent({
  name: 'ElTag',
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
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

const mockValidate = vi.fn<() => Promise<boolean>>()

const ElFormStub = defineComponent({
  name: 'ElForm',
  setup(_, { slots, expose }) {
    expose({ validate: mockValidate })
    return () => h('form', slots.default?.())
  },
})

const ElFormItemStub = defineComponent({
  name: 'ElFormItem',
  setup(_, { slots }) {
    return () => h('div', slots.default?.())
  },
})

const coerceValue = (value: string) => {
  if (value === '') {
    return ''
  }

  const numericValue = Number(value)
  return Number.isNaN(numericValue) ? value : numericValue
}

const ElSelectStub = defineComponent({
  name: 'ElSelect',
  inheritAttrs: false,
  props: {
    modelValue: { type: [String, Number], default: '' },
  },
  emits: ['update:modelValue', 'change'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'select',
        {
          ...attrs,
          value: props.modelValue ?? '',
          onChange: (event: Event) => {
            const nextValue = coerceValue((event.target as HTMLSelectElement).value)
            emit('update:modelValue', nextValue)
            emit('change', nextValue)
          },
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

const ElInputNumberStub = defineComponent({
  name: 'ElInputNumber',
  inheritAttrs: false,
  props: {
    modelValue: { type: Number, default: undefined },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        type: 'number',
        value: props.modelValue ?? '',
        onInput: (event: Event) => {
          const rawValue = (event.target as HTMLInputElement).value
          emit('update:modelValue', rawValue === '' ? undefined : Number(rawValue))
        },
      })
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

const stores = [
  {
    id: 1,
    department_id: 9,
    store_type_id: 1,
    management_type_id: 1,
    code: 'STORE-1',
    name: 'North Plaza',
    short_name: 'North',
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
  {
    id: 2,
    department_id: 9,
    store_type_id: 1,
    management_type_id: 1,
    code: 'STORE-2',
    name: 'South Plaza',
    short_name: 'South',
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const buildingsByStore: Record<number, Array<Record<string, unknown>>> = {
  1: [
    {
      id: 11,
      store_id: 1,
      code: 'BLDG-11',
      name: 'North Tower',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  2: [
    {
      id: 21,
      store_id: 2,
      code: 'BLDG-21',
      name: 'South East',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
}

const floorsByBuilding: Record<number, Array<Record<string, unknown>>> = {
  11: [
    {
      id: 111,
      building_id: 11,
      code: 'F-111',
      name: 'North L1',
      status: 'active',
      floor_plan_image_url: null,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
    {
      id: 112,
      building_id: 11,
      code: 'F-112',
      name: 'North L2',
      status: 'active',
      floor_plan_image_url: null,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  21: [
    {
      id: 211,
      building_id: 21,
      code: 'F-211',
      name: 'South L1',
      status: 'active',
      floor_plan_image_url: null,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
}

const areasByStore: Record<number, Array<Record<string, unknown>>> = {
  1: [
    {
      id: 301,
      store_id: 1,
      area_level_id: 1,
      code: 'AREA-1',
      name: 'North Area',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  2: [
    {
      id: 302,
      store_id: 2,
      area_level_id: 1,
      code: 'AREA-2',
      name: 'South Area',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
}

const unitTypes = [{ id: 1, code: 'STD', name: 'Standard', created_at: '2026-01-01T00:00:00Z', updated_at: '2026-01-01T00:00:00Z' }]
const shopTypes = [
  { id: 2, code: 'FOOD', name: 'Food', created_at: '2026-01-01T00:00:00Z', updated_at: '2026-01-01T00:00:00Z' },
  { id: 3, code: 'LIFE', name: 'Lifestyle', created_at: '2026-01-01T00:00:00Z', updated_at: '2026-01-01T00:00:00Z' },
]

const unitsByBuilding: Record<number, Array<Record<string, unknown>>> = {
  11: [
    {
      id: 501,
      building_id: 11,
      floor_id: 111,
      location_id: 401,
      area_id: 301,
      unit_type_id: 1,
      shop_type_id: null,
      code: 'UNIT-111',
      floor_area: 10,
      use_area: 9,
      rent_area: 8,
      is_rentable: true,
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
    {
      id: 502,
      building_id: 11,
      floor_id: 112,
      location_id: 402,
      area_id: 301,
      unit_type_id: 1,
      shop_type_id: 2,
      code: 'UNIT-112',
      floor_area: 20,
      use_area: 18,
      rent_area: 16,
      is_rentable: false,
      status: 'inactive',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  21: [
    {
      id: 511,
      building_id: 21,
      floor_id: 211,
      location_id: 411,
      area_id: 302,
      unit_type_id: 1,
      shop_type_id: 3,
      code: 'UNIT-211',
      floor_area: 30,
      use_area: 27,
      rent_area: 24,
      is_rentable: true,
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
}

const flushPromises = async () => {
  for (let index = 0; index < 6; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(RentableAreaAdminView, {
    global: {
      plugins: [ElementPlus, i18n, createPinia()],
      stubs: {
        PageSection: PageSectionStub,
        ElAlert: ElAlertStub,
        ElButton: ElButtonStub,
        ElCard: ElCardStub,
        ElDialog: ElDialogStub,
        ElForm: ElFormStub,
        ElFormItem: ElFormItemStub,
        ElInputNumber: ElInputNumberStub,
        ElOption: ElOptionStub,
        ElSelect: ElSelectStub,
        ElSwitch: ElSwitchStub,
        ElTag: ElTagStub,
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

describe('RentableAreaAdminView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    mockValidate.mockResolvedValue(true)

    vi.mocked(listUnitTypes).mockResolvedValue({ data: { unit_types: unitTypes } } as never)
    vi.mocked(listShopTypes).mockResolvedValue({ data: { shop_types: shopTypes } } as never)
    vi.mocked(listStructureStores).mockResolvedValue({ data: { stores } } as never)
    vi.mocked(listStructureBuildings).mockImplementation(async (params?: { store_id?: number }) => ({
      data: { buildings: params?.store_id ? buildingsByStore[params.store_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureFloors).mockImplementation(async (params?: { building_id?: number }) => ({
      data: { floors: params?.building_id ? floorsByBuilding[params.building_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureAreas).mockImplementation(async (params?: { store_id?: number }) => ({
      data: { areas: params?.store_id ? areasByStore[params.store_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureUnits).mockImplementation(async (params?: { building_id?: number; floor_id?: number; area_id?: number }) => ({
      data: {
        units: (params?.building_id ? unitsByBuilding[params.building_id] ?? [] : []).filter((unit: Record<string, unknown>) => {
          const floorMatches = params?.floor_id ? unit.floor_id === params.floor_id : true
          const areaMatches = params?.area_id ? unit.area_id === params.area_id : true
          return floorMatches && areaMatches
        }),
      },
    }) as never)
    vi.mocked(updateStructureUnit).mockResolvedValue({
      data: {
        unit: {
          ...unitsByBuilding[11][0],
          shop_type_id: 2,
          rent_area: 12.5,
          is_rentable: false,
          status: 'inactive',
        },
      },
    } as never)
  })

  it('bootstraps reference data and initial unit list on mount', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="rentable-area-admin-view"]')).toBeTruthy()
    expect(listUnitTypes).toHaveBeenCalledTimes(1)
    expect(listShopTypes).toHaveBeenCalledTimes(1)
    expect(listStructureStores).toHaveBeenCalledTimes(1)
    expect(listStructureBuildings).toHaveBeenCalledWith({ store_id: 1 })
    expect(listStructureFloors).toHaveBeenCalledWith({ building_id: 11 })
    expect(listStructureAreas).toHaveBeenCalledWith({ store_id: 1 })
    expect(listStructureUnits).toHaveBeenCalledWith({
      building_id: 11,
      floor_id: 111,
      area_id: undefined,
    })
    expect(wrapper.text()).toContain('UNIT-111')
    expect(wrapper.text()).toContain('North')
  })

  it('updates filter cascades and reloads units when applying filters', async () => {
    const wrapper = await mountView()

    vi.mocked(listStructureBuildings).mockClear()
    vi.mocked(listStructureFloors).mockClear()
    vi.mocked(listStructureAreas).mockClear()
    vi.mocked(listStructureUnits).mockClear()

    await wrapper.get('[data-testid="rentable-area-store-filter"]').setValue('2')
    await flushPromises()

    expect(listStructureBuildings).toHaveBeenCalledWith({ store_id: 2 })
    expect(listStructureAreas).toHaveBeenCalledWith({ store_id: 2 })

    await wrapper.get('[data-testid="rentable-area-building-filter"]').setValue('21')
    await flushPromises()

    expect(listStructureFloors).toHaveBeenCalledWith({ building_id: 21 })

    await wrapper.get('[data-testid="rentable-area-floor-filter"]').setValue('211')
    await wrapper.get('[data-testid="rentable-area-area-filter"]').setValue('302')
    await wrapper.get('[data-testid="rentable-area-apply-button"]').trigger('click')
    await flushPromises()

    expect(listStructureUnits).toHaveBeenCalledWith({
      building_id: 21,
      floor_id: 211,
      area_id: 302,
    })
    expect(wrapper.text()).toContain('UNIT-211')
  })

  it('filters visible rows locally and restores baseline filters on reset', async () => {
    const wrapper = await mountView()

    expect(wrapper.text()).toContain('UNIT-111')
    expect(wrapper.text()).not.toContain('UNIT-112')

    vi.mocked(listStructureUnits).mockClear()

    await wrapper.get('[data-testid="rentable-area-floor-filter"]').setValue('')
    await wrapper.get('[data-testid="rentable-area-apply-button"]').trigger('click')
    await flushPromises()

    expect(wrapper.text()).toContain('UNIT-111')
    expect(wrapper.text()).toContain('UNIT-112')

    await wrapper.get('[data-testid="rentable-area-rentable-switch"]').setValue(true)
    await flushPromises()

    expect(wrapper.text()).toContain('UNIT-111')
    expect(wrapper.text()).not.toContain('UNIT-112')

    await wrapper.get('[data-testid="rentable-area-status-filter"]').setValue('inactive')
    await flushPromises()

    expect(wrapper.text()).not.toContain('UNIT-111')
    expect(wrapper.text()).not.toContain('UNIT-112')

    await wrapper.get('[data-testid="rentable-area-reset-button"]').trigger('click')
    await flushPromises()

    expect(listStructureUnits).toHaveBeenCalledWith({
      building_id: 11,
      floor_id: 111,
      area_id: undefined,
    })
    expect(wrapper.text()).toContain('UNIT-111')
    expect(wrapper.text()).not.toContain('UNIT-112')
  })

  it('opens, closes, and saves the edit dialog', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="rentable-area-edit-button-501"]').trigger('click')
    await flushPromises()

    let dialog = getOpenDialog(wrapper)
    expect(dialog.text()).toContain('UNIT-111')
    expect(dialog.text()).toContain('North L1')

    const cancelButton = dialog.findAll('button').find((button) => button.text().includes('Cancel'))
    expect(cancelButton).toBeTruthy()

    await cancelButton!.trigger('click')
    await flushPromises()

    expect(wrapper.find('[data-testid="rentable-area-edit-dialog"]').exists()).toBe(false)

    await wrapper.get('[data-testid="rentable-area-edit-button-501"]').trigger('click')
    await flushPromises()

    dialog = getOpenDialog(wrapper)
    await dialog.get('[data-testid="rentable-area-unit-edit-rent-area"]').setValue('12.5')
    await dialog.get('[data-testid="rentable-area-unit-edit-status"]').setValue('inactive')
    await dialog.get('[data-testid="rentable-area-edit-shop-type-select"]').setValue('2')
    await dialog.get('[data-testid="rentable-area-unit-edit-rentable"]').setValue(false)
    await flushPromises()

    await dialog.get('[data-testid="rentable-area-unit-edit-save"]').trigger('click')
    await flushPromises()

    expect(mockValidate).toHaveBeenCalledTimes(1)
    expect(updateStructureUnit).toHaveBeenCalledWith(501, {
      building_id: 11,
      floor_id: 111,
      location_id: 401,
      area_id: 301,
      unit_type_id: 1,
      shop_type_id: 2,
      code: 'UNIT-111',
      floor_area: 10,
      use_area: 9,
      rent_area: 12.5,
      is_rentable: false,
      status: 'inactive',
    })
    expect(wrapper.find('[data-testid="rentable-area-edit-dialog"]').exists()).toBe(false)
    expect(wrapper.text()).toContain('UNIT-111')
    expect(wrapper.get('[data-testid="rentable-area-feedback-alert"]').text()).toContain('UNIT-111')
  })

  it('does not submit when form validation fails', async () => {
    mockValidate.mockResolvedValueOnce(false)

    const wrapper = await mountView()

    await wrapper.get('[data-testid="rentable-area-edit-button-501"]').trigger('click')
    await flushPromises()

    await wrapper.get('[data-testid="rentable-area-unit-edit-save"]').trigger('click')
    await flushPromises()

    expect(updateStructureUnit).not.toHaveBeenCalled()
    expect(wrapper.find('[data-testid="rentable-area-edit-dialog"]').exists()).toBe(true)
  })

  it('shows feedback when updating a unit fails', async () => {
    vi.mocked(updateStructureUnit).mockRejectedValueOnce(new Error('Update failed'))

    const wrapper = await mountView()

    await wrapper.get('[data-testid="rentable-area-edit-button-501"]').trigger('click')
    await flushPromises()

    await wrapper.get('[data-testid="rentable-area-unit-edit-save"]').trigger('click')
    await flushPromises()

    expect(wrapper.get('[data-testid="rentable-area-feedback-alert"]').text()).toContain('Update failed')
    expect(wrapper.find('[data-testid="rentable-area-edit-dialog"]').exists()).toBe(true)
  })
})
