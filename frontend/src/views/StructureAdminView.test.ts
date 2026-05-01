import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import StructureAdminView from './StructureAdminView.vue'

vi.mock('../api/structure', () => ({
  createStructureArea: vi.fn(),
  createStructureBuilding: vi.fn(),
  createStructureFloor: vi.fn(),
  createStructureLocation: vi.fn(),
  createStructureStore: vi.fn(),
  createStructureUnit: vi.fn(),
  listStructureAreas: vi.fn(),
  listStructureBuildings: vi.fn(),
  listStructureFloors: vi.fn(),
  listStructureLocations: vi.fn(),
  listStructureStores: vi.fn(),
  listStructureUnits: vi.fn(),
  updateStructureArea: vi.fn(),
  updateStructureBuilding: vi.fn(),
  updateStructureFloor: vi.fn(),
  updateStructureLocation: vi.fn(),
  updateStructureStore: vi.fn(),
  updateStructureUnit: vi.fn(),
}))

vi.mock('../api/org', () => ({
  listDepartments: vi.fn(),
}))

vi.mock('../api/baseinfo', () => ({
  listAreaLevels: vi.fn(),
  listStoreManagementTypes: vi.fn(),
  listStoreTypes: vi.fn(),
  listUnitTypes: vi.fn(),
}))

import {
  createStructureBuilding,
  createStructureStore,
  listStructureAreas,
  listStructureBuildings,
  listStructureFloors,
  listStructureLocations,
  listStructureStores,
  listStructureUnits,
  updateStructureStore,
} from '../api/structure'
import { listDepartments } from '../api/org'
import { listAreaLevels, listStoreManagementTypes, listStoreTypes, listUnitTypes } from '../api/baseinfo'

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

const departments = [{ id: 9, code: 'OPS', name: 'Operations' }]
const referenceCatalogItems = [{ id: 1, code: 'STD', name: 'Standard', created_at: '2026-01-01T00:00:00Z', updated_at: '2026-01-01T00:00:00Z' }]

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
    {
      id: 22,
      store_id: 2,
      code: 'BLDG-22',
      name: 'South West',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  3: [],
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
  ],
  21: [
    {
      id: 211,
      building_id: 21,
      code: 'F-211',
      name: 'South East L1',
      status: 'active',
      floor_plan_image_url: null,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  22: [
    {
      id: 221,
      building_id: 22,
      code: 'F-221',
      name: 'South West L1',
      status: 'active',
      floor_plan_image_url: null,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
    {
      id: 222,
      building_id: 22,
      code: 'F-222',
      name: 'South West L2',
      status: 'active',
      floor_plan_image_url: null,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  23: [],
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
  3: [],
}

const locationsByFloor: Record<number, Array<Record<string, unknown>>> = {
  111: [
    {
      id: 401,
      store_id: 1,
      floor_id: 111,
      code: 'LOC-111',
      name: 'North Corridor',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  211: [
    {
      id: 411,
      store_id: 2,
      floor_id: 211,
      code: 'LOC-211',
      name: 'South East Hall',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  221: [
    {
      id: 421,
      store_id: 2,
      floor_id: 221,
      code: 'LOC-221',
      name: 'South West Hall',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  222: [
    {
      id: 422,
      store_id: 2,
      floor_id: 222,
      code: 'LOC-222',
      name: 'South West Atrium',
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
}

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
  ],
  21: [
    {
      id: 511,
      building_id: 21,
      floor_id: 211,
      location_id: 411,
      area_id: 302,
      unit_type_id: 1,
      shop_type_id: null,
      code: 'UNIT-211',
      floor_area: 20,
      use_area: 18,
      rent_area: 16,
      is_rentable: true,
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  22: [
    {
      id: 521,
      building_id: 22,
      floor_id: 221,
      location_id: 421,
      area_id: 302,
      unit_type_id: 1,
      shop_type_id: null,
      code: 'UNIT-221',
      floor_area: 22,
      use_area: 20,
      rent_area: 18,
      is_rentable: true,
      status: 'active',
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  23: [],
}

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(StructureAdminView, {
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

describe('StructureAdminView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'

    vi.mocked(listDepartments).mockResolvedValue({ data: { departments } } as never)
    vi.mocked(listStoreTypes).mockResolvedValue({ data: { store_types: referenceCatalogItems } } as never)
    vi.mocked(listStoreManagementTypes).mockResolvedValue({ data: { store_management_types: referenceCatalogItems } } as never)
    vi.mocked(listAreaLevels).mockResolvedValue({ data: { area_levels: referenceCatalogItems } } as never)
    vi.mocked(listUnitTypes).mockResolvedValue({ data: { unit_types: referenceCatalogItems } } as never)

    vi.mocked(listStructureStores).mockResolvedValue({ data: { stores } } as never)
    vi.mocked(listStructureBuildings).mockImplementation(async ({ store_id } = {}) => ({
      data: { buildings: store_id ? buildingsByStore[store_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureFloors).mockImplementation(async ({ building_id } = {}) => ({
      data: { floors: building_id ? floorsByBuilding[building_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureAreas).mockImplementation(async ({ store_id } = {}) => ({
      data: { areas: store_id ? areasByStore[store_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureLocations).mockImplementation(async ({ floor_id } = {}) => ({
      data: { locations: floor_id ? locationsByFloor[floor_id] ?? [] : [] },
    }) as never)
    vi.mocked(listStructureUnits).mockImplementation(async ({ building_id } = {}) => ({
      data: { units: building_id ? unitsByBuilding[building_id] ?? [] : [] },
    }) as never)

    vi.mocked(createStructureStore).mockResolvedValue({
      data: {
        store: {
          id: 3,
          department_id: 12,
          store_type_id: 4,
          management_type_id: 5,
          code: 'STORE-3',
          name: 'West Plaza',
          short_name: 'West',
          status: 'active',
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
      },
    } as never)
    vi.mocked(updateStructureStore).mockResolvedValue({
      data: {
        store: {
          ...stores[0],
          code: 'STORE-1-UPDATED',
          name: 'North Plaza Updated',
          short_name: 'North+',
        },
      },
    } as never)
    vi.mocked(createStructureBuilding).mockResolvedValue({
      data: {
        building: {
          id: 23,
          store_id: 2,
          code: 'BLDG-23',
          name: 'South Central',
          status: 'active',
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
        },
      },
    } as never)
  })

  it('renders and bootstraps the structure cascade on mount', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="structure-admin-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Structure admin')
    expect(listStructureStores).toHaveBeenCalledTimes(1)
    expect(listStructureBuildings).toHaveBeenCalledWith({ store_id: 1 })
    expect(listStructureFloors).toHaveBeenCalledWith({ building_id: 11 })
    expect(listStructureAreas).toHaveBeenCalledWith({ store_id: 1 })
    expect(listStructureLocations).toHaveBeenCalledWith({ store_id: 1, floor_id: 111 })
    expect(listStructureUnits).toHaveBeenCalledWith({
      building_id: 11,
      floor_id: 111,
      location_id: undefined,
      area_id: undefined,
    })
  })

  it('creates and updates stores through the admin forms', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="structure-store-code-input"]').setValue('STORE-3')
    await wrapper.get('[data-testid="structure-store-name-input"]').setValue('West Plaza')
    await wrapper.get('[data-testid="structure-store-short-name-input"]').setValue('West')
    await wrapper.get('[data-testid="structure-store-department-input"] input').setValue('12')
    await wrapper.get('[data-testid="structure-store-type-input"] input').setValue('4')
    await wrapper.get('[data-testid="structure-store-management-type-input"] input').setValue('5')
    await flushPromises()

    await wrapper.get('[data-testid="structure-store-create-button"]').trigger('click')
    await flushPromises()

    expect(createStructureStore).toHaveBeenCalledWith({
      department_id: 12,
      store_type_id: 4,
      management_type_id: 5,
      code: 'STORE-3',
      name: 'West Plaza',
      short_name: 'West',
      status: 'active',
    })
    expect(listStructureBuildings).toHaveBeenCalledWith({ store_id: 3 })
    expect(listStructureAreas).toHaveBeenCalledWith({ store_id: 3 })

    const storeRow = wrapper
      .get('[data-testid="structure-stores-table"]')
      .findAll('tbody tr')
      .find((row) => row.text().includes('North Plaza'))
    expect(storeRow).toBeTruthy()

    const storeEditButton = storeRow!.findAll('button').find((button) => button.text().includes('Edit'))
    expect(storeEditButton).toBeTruthy()

    await storeEditButton!.trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    const dialogInputs = dialog.findAllComponents({ name: 'ElInput' })

    await dialogInputs[0].get('input').setValue('STORE-1-UPDATED')
    await dialogInputs[1].get('input').setValue('North Plaza Updated')
    await dialogInputs[2].get('input').setValue('North+')
    await flushPromises()

    const saveButton = dialog.findAll('button').find((button) => button.text().includes('Save'))
    expect(saveButton).toBeTruthy()

    await saveButton!.trigger('click')
    await flushPromises()

    expect(updateStructureStore).toHaveBeenCalledWith(1, {
      department_id: 9,
      store_type_id: 1,
      management_type_id: 1,
      code: 'STORE-1-UPDATED',
      name: 'North Plaza Updated',
      short_name: 'North+',
      status: 'active',
    })
  })

  it('loads the next cascade level when selections change and creates buildings', async () => {
    const wrapper = await mountView()

    vi.mocked(listStructureBuildings).mockClear()
    vi.mocked(listStructureAreas).mockClear()
    vi.mocked(listStructureFloors).mockClear()
    vi.mocked(listStructureLocations).mockClear()
    vi.mocked(listStructureUnits).mockClear()

    const southStoreRow = wrapper
      .get('[data-testid="structure-stores-table"]')
      .findAll('tbody tr')
      .find((row) => row.text().includes('South Plaza'))
    expect(southStoreRow).toBeTruthy()

    const storeSelectButton = southStoreRow!.findAll('button').find((button) => button.text().includes('Select'))
    expect(storeSelectButton).toBeTruthy()

    await storeSelectButton!.trigger('click')
    await flushPromises()

    expect(listStructureBuildings).toHaveBeenCalledWith({ store_id: 2 })
    expect(listStructureAreas).toHaveBeenCalledWith({ store_id: 2 })

    vi.mocked(listStructureFloors).mockClear()
    vi.mocked(listStructureLocations).mockClear()
    vi.mocked(listStructureUnits).mockClear()

    const southWestBuildingRow = wrapper
      .get('[data-testid="structure-buildings-table"]')
      .findAll('tbody tr')
      .find((row) => row.text().includes('South West'))
    expect(southWestBuildingRow).toBeTruthy()

    const buildingSelectButton = southWestBuildingRow!.findAll('button').find((button) => button.text().includes('Select'))
    expect(buildingSelectButton).toBeTruthy()

    await buildingSelectButton!.trigger('click')
    await flushPromises()

    expect(listStructureFloors).toHaveBeenCalledWith({ building_id: 22 })
    expect(listStructureLocations).toHaveBeenCalledWith({ store_id: 2, floor_id: 221 })
    expect(listStructureUnits).toHaveBeenCalledWith({
      building_id: 22,
      floor_id: 221,
      location_id: undefined,
      area_id: undefined,
    })

    vi.mocked(listStructureLocations).mockClear()
    vi.mocked(listStructureUnits).mockClear()

    const secondFloorRow = wrapper
      .get('[data-testid="structure-floors-table"]')
      .findAll('tbody tr')
      .find((row) => row.text().includes('South West L2'))
    expect(secondFloorRow).toBeTruthy()

    const floorSelectButton = secondFloorRow!.findAll('button').find((button) => button.text().includes('Select'))
    expect(floorSelectButton).toBeTruthy()

    await floorSelectButton!.trigger('click')
    await flushPromises()

    expect(listStructureLocations).toHaveBeenCalledWith({ store_id: 2, floor_id: 222 })
    expect(listStructureUnits).toHaveBeenCalledWith({
      building_id: 22,
      floor_id: 222,
      location_id: undefined,
      area_id: undefined,
    })

    await wrapper.get('[data-testid="structure-building-code-input"]').setValue('BLDG-23')
    await wrapper.get('[data-testid="structure-building-name-input"]').setValue('South Central')
    await flushPromises()

    await wrapper.get('[data-testid="structure-building-create-button"]').trigger('click')
    await flushPromises()

    expect(createStructureBuilding).toHaveBeenCalledWith({
      store_id: 2,
      code: 'BLDG-23',
      name: 'South Central',
      status: 'active',
    })
    expect(listStructureFloors).toHaveBeenCalledWith({ building_id: 23 })
  })

  it('shows feedback when store loading fails', async () => {
    vi.mocked(listStructureStores).mockRejectedValueOnce(new Error('Store service unavailable'))

    const wrapper = await mountView()

    expect(wrapper.text()).toContain('Stores unavailable')
    expect(wrapper.text()).toContain('Store service unavailable')
  })
})
