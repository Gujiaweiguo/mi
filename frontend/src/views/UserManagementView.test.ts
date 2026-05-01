import ElementPlus, { ElMessage } from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick, provide, inject, type PropType, type Ref, toRef } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import UserManagementView from './UserManagementView.vue'

vi.mock('../api/user', () => ({
  listUsers: vi.fn(),
  createUser: vi.fn(),
  updateUser: vi.fn(),
  resetPassword: vi.fn(),
  setUserRoles: vi.fn(),
  listRoles: vi.fn(),
}))

vi.mock('../api/org', () => ({
  listDepartments: vi.fn(),
}))

import { createUser, listRoles, listUsers, resetPassword, updateUser } from '../api/user'
import { listDepartments } from '../api/org'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () => h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary), h('div', slots.actions?.())])
  },
})

const ElCardStub = defineComponent({
  name: 'ElCard',
  setup(_, { slots }) {
    return () => h('section', slots.default?.())
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
        ? h('section', { 'data-testid': 'el-dialog-stub' }, [h('h2', props.title), slots.default?.(), h('footer', slots.footer?.())])
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
    return () => h('label', slots.default?.())
  },
})

const coerceValue = (value: string) => {
  if (value === '') {
    return ''
  }

  const numericValue = Number(value)
  return Number.isNaN(numericValue) ? value : numericValue
}

const ElInputStub = defineComponent({
  name: 'ElInput',
  inheritAttrs: false,
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

const ElSelectStub = defineComponent({
  name: 'ElSelect',
  inheritAttrs: false,
  props: {
    modelValue: { type: [String, Number, Array] as PropType<string | number | Array<string | number>>, default: '' },
    multiple: { type: Boolean, default: false },
  },
  emits: ['update:modelValue', 'change'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'select',
        {
          ...attrs,
          multiple: props.multiple,
          value: props.modelValue,
          onChange: (event: Event) => {
            const target = event.target as HTMLSelectElement
            const nextValue = props.multiple
              ? Array.from(target.selectedOptions).map((option) => coerceValue(option.value))
              : coerceValue(target.value)
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

const tableDataKey = Symbol('table-data')

const ElTableStub = defineComponent({
  name: 'ElTable',
  props: {
    data: { type: Array as PropType<Array<Record<string, unknown>>>, default: () => [] },
  },
  setup(props, { slots }) {
    provide(tableDataKey, toRef(props, 'data'))
    return () => h('section', { 'data-testid': 'el-table-stub' }, slots.default?.())
  },
})

const ElTableColumnStub = defineComponent({
  name: 'ElTableColumn',
  props: {
    label: { type: String, default: '' },
    prop: { type: String, default: '' },
  },
  setup(props, { slots }) {
    const rows = inject<Ref<Array<Record<string, unknown>>>>(tableDataKey)

    return () =>
      h('div', { 'data-column-label': props.label }, [
        h('strong', props.label),
        ...(rows?.value ?? []).map((row, index) =>
          h(
            'div',
            { key: `${props.label}-${index}` },
            slots.default ? slots.default({ row, $index: index }) : String(props.prop ? (row[props.prop] ?? '') : ''),
          ),
        ),
      ])
  },
})

const users = [
  { id: 1, username: 'alice', display_name: 'Alice Zhang', department_id: 10, status: 'active', role_ids: [1, 2] },
  { id: 2, username: 'bob', display_name: 'Bob Li', department_id: 20, status: 'inactive', role_ids: [2] },
]

const roles = [
  { ID: 1, Code: 'ADMIN', Name: '管理员', Status: 'active', IsLeader: false },
  { ID: 2, Code: 'OPS', Name: '运营', Status: 'active', IsLeader: false },
]

const departments = [
  { id: 10, code: 'D10', name: '招商部', level: 1, status: 'active', parent_id: null, type_id: 1 },
  { id: 20, code: 'D20', name: '财务部', level: 1, status: 'active', parent_id: null, type_id: 1 },
]

const flushPromises = async () => {
  for (let index = 0; index < 6; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(UserManagementView, {
    global: {
      plugins: [ElementPlus, i18n, createPinia()],
      stubs: {
        PageSection: PageSectionStub,
        ElButton: ElButtonStub,
        ElCard: ElCardStub,
        ElDialog: ElDialogStub,
        ElForm: ElFormStub,
        ElFormItem: ElFormItemStub,
        ElInput: ElInputStub,
        ElOption: ElOptionStub,
        ElSelect: ElSelectStub,
        ElTable: ElTableStub,
        ElTableColumn: ElTableColumnStub,
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

describe('UserManagementView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'zh-CN'
    mockValidate.mockResolvedValue(true)

    vi.spyOn(ElMessage, 'success').mockImplementation(() => '')
    vi.spyOn(ElMessage, 'error').mockImplementation(() => '')

    vi.mocked(listUsers).mockResolvedValue({ data: { users } } as never)
    vi.mocked(listRoles).mockResolvedValue({ data: { roles } } as never)
    vi.mocked(listDepartments).mockResolvedValue({ data: { departments } } as never)
    vi.mocked(createUser).mockResolvedValue({ data: { user: users[0] } } as never)
    vi.mocked(updateUser).mockResolvedValue({ data: { user: users[0] } } as never)
    vi.mocked(resetPassword).mockResolvedValue({ data: {} } as never)
  })

  it('loads users, roles, and department names on mount', async () => {
    const wrapper = await mountView()

    expect(listUsers).toHaveBeenCalledTimes(1)
    expect(listDepartments).toHaveBeenCalledTimes(1)
    expect(listRoles).toHaveBeenCalledTimes(1)
    expect(wrapper.text()).toContain('Alice Zhang')
    expect(wrapper.text()).toContain('招商部')
    expect(wrapper.text()).toContain('启用')
    expect(wrapper.text()).toContain('停用')
    expect(wrapper.text()).toContain('管理员, 运营')
  })

  it('opens the create dialog and submits selected department and roles', async () => {
    const wrapper = await mountView()

    await wrapper.get('button').trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    const inputs = dialog.findAll('input')

    await inputs[0].setValue('new-user')
    await inputs[1].setValue('New User')
    await inputs[2].setValue('secret123')

    const selects = dialog.findAll('select')
    await selects[0].setValue('20')
    Array.from(selects[1].element.options).forEach((option) => {
      option.selected = ['1', '2'].includes(option.value)
    })
    await selects[1].trigger('change')
    await flushPromises()

    await dialog.findAll('button')[1].trigger('click')
    await flushPromises()

    expect(mockValidate).toHaveBeenCalledTimes(1)
    expect(createUser).toHaveBeenCalledWith({
      username: 'new-user',
      display_name: 'New User',
      password: 'secret123',
      department_id: 20,
      role_ids: [1, 2],
    })
    expect(ElMessage.success).toHaveBeenCalledWith('创建用户成功')
    expect(listUsers).toHaveBeenCalledTimes(2)
    expect(wrapper.find('[data-testid="el-dialog-stub"]').exists()).toBe(false)
  })

  it('does not create a user when validation fails', async () => {
    mockValidate.mockResolvedValueOnce(false)

    const wrapper = await mountView()

    await wrapper.get('button').trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    await dialog.findAll('button')[1].trigger('click')
    await flushPromises()

    expect(createUser).not.toHaveBeenCalled()
    expect(wrapper.find('[data-testid="el-dialog-stub"]').exists()).toBe(true)
  })

  it('opens the edit dialog and updates the selected user', async () => {
    const wrapper = await mountView()

    const editButton = wrapper.findAll('button').find((button) => button.text() === '编辑')
    expect(editButton).toBeTruthy()

    await editButton!.trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    const inputs = dialog.findAll('input')
    const selects = dialog.findAll('select')

    expect(inputs[0].element.value).toBe('Alice Zhang')
    await inputs[0].setValue('Alice Updated')
    await selects[0].setValue('20')
    await selects[1].setValue('inactive')
    await flushPromises()

    await dialog.findAll('button')[1].trigger('click')
    await flushPromises()

    expect(updateUser).toHaveBeenCalledWith(1, {
      display_name: 'Alice Updated',
      department_id: 20,
      status: 'inactive',
    })
    expect(ElMessage.success).toHaveBeenCalledWith('更新用户成功')
    expect(listUsers).toHaveBeenCalledTimes(2)
  })

  it('opens the reset dialog and submits a new password', async () => {
    const wrapper = await mountView()

    const resetButton = wrapper.findAll('button').find((button) => button.text() === '重置密码')
    expect(resetButton).toBeTruthy()

    await resetButton!.trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    await dialog.get('input').setValue('new-secret')
    await dialog.findAll('button')[1].trigger('click')
    await flushPromises()

    expect(resetPassword).toHaveBeenCalledWith(1, { new_password: 'new-secret' })
    expect(ElMessage.success).toHaveBeenCalledWith('重置密码成功')
    expect(wrapper.find('[data-testid="el-dialog-stub"]').exists()).toBe(false)
  })

  it('shows an error when loading users fails', async () => {
    vi.mocked(listUsers).mockRejectedValueOnce(new Error('load failed'))

    await mountView()

    expect(ElMessage.error).toHaveBeenCalledWith('加载用户列表失败')
  })

  it('shows an error when user creation fails', async () => {
    vi.mocked(createUser).mockRejectedValueOnce(new Error('create failed'))

    const wrapper = await mountView()

    await wrapper.get('button').trigger('click')
    await flushPromises()

    const dialog = getOpenDialog(wrapper)
    const inputs = dialog.findAll('input')

    await inputs[0].setValue('new-user')
    await inputs[1].setValue('New User')
    await inputs[2].setValue('secret123')
    await dialog.findAll('button')[1].trigger('click')
    await flushPromises()

    expect(ElMessage.error).toHaveBeenCalledWith('创建用户失败')
    expect(wrapper.find('[data-testid="el-dialog-stub"]').exists()).toBe(true)
  })
})
