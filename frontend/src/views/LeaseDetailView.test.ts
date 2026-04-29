import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import LeaseDetailView from './LeaseDetailView.vue'

const push = vi.fn()

vi.mock('vue-router', () => ({
  useRoute: () => ({ params: { id: '42' } }),
  useRouter: () => ({ push }),
}))

vi.mock('../stores/auth', () => ({
  useAuthStore: () => ({
    canAccess: () => true,
  }),
}))

vi.mock('../api/lease', () => ({
  getLease: vi.fn(),
  listOvertimeBills: vi.fn(),
  submitLease: vi.fn(),
  terminateLease: vi.fn(),
  createOvertimeBill: vi.fn(),
  submitOvertimeBill: vi.fn(),
  cancelOvertimeBill: vi.fn(),
  stopOvertimeBill: vi.fn(),
  generateOvertimeBillCharges: vi.fn(),
}))

import { getLease, listOvertimeBills } from '../api/lease'

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

const passthroughStub = (tag = 'div') =>
  defineComponent({
    setup(_, { slots, attrs }) {
      return () => h(tag, attrs, slots.default?.())
    },
  })

const ElAlertStub = defineComponent({
  props: { title: { type: String, default: '' }, description: { type: String, default: '' } },
  setup(props) {
    return () => h('div', `${props.title} ${props.description}`.trim())
  },
})

const ElTableStub = defineComponent({
  setup(_, { attrs }) {
    return () => h('div', { 'data-testid': attrs['data-testid'] ?? 'table-stub' })
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('LeaseDetailView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
  })

  it('loads lease detail, overtime bills, and amendment entry for active leases', async () => {
    vi.mocked(getLease).mockResolvedValue({
      data: {
        lease: {
          id: 42,
          amended_from_id: null,
          lease_no: 'L-042',
          subtype: 'standard',
          department_id: 1,
          store_id: 2,
          building_id: null,
          customer_id: null,
          brand_id: null,
          trade_id: null,
          management_type_id: null,
          tenant_name: 'Tenant A',
          start_date: '2026-01-01',
          end_date: '2026-12-31',
          status: 'active',
          workflow_instance_id: null,
          effective_version: 1,
          submitted_at: null,
          approved_at: null,
          billing_effective_at: null,
          terminated_at: null,
          created_by: 1,
          updated_by: 1,
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
          joint_operation: null,
          ad_boards: [],
          area_grounds: [],
          units: [],
          terms: [],
        },
      },
    } as never)

    vi.mocked(listOvertimeBills).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 100,
      },
    } as never)

    const wrapper = mount(LeaseDetailView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElAlert: ElAlertStub,
          ElButton: passthroughStub('button'),
          ElTag: passthroughStub('span'),
          ElCard: passthroughStub('section'),
          ElDescriptions: passthroughStub('dl'),
          ElDescriptionsItem: passthroughStub('div'),
          ElForm: passthroughStub('form'),
          ElFormItem: passthroughStub('div'),
          ElDatePicker: passthroughStub('input'),
          ElInput: passthroughStub('input'),
          ElInputNumber: passthroughStub('input'),
          ElSelect: passthroughStub('select'),
          ElOption: passthroughStub('option'),
          ElTable: ElTableStub,
          ElTableColumn: passthroughStub(),
          ElEmpty: passthroughStub(),
          ElSkeleton: passthroughStub(),
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(getLease).toHaveBeenCalledWith(42)
    expect(listOvertimeBills).toHaveBeenCalledWith({ lease_contract_id: 42, page: 1, page_size: 100 })
    expect(wrapper.get('[data-testid="lease-detail-view"]')).toBeTruthy()

    await wrapper.get('[data-testid="lease-amend-button"]').trigger('click')

    expect(push).toHaveBeenCalledWith({ name: 'lease-contracts-amend', params: { id: '42' } })
  })

  it('hides amendment entry when the lease is not amendment-eligible', async () => {
    vi.mocked(getLease).mockResolvedValue({
      data: {
        lease: {
          id: 42,
          amended_from_id: null,
          lease_no: 'L-042',
          subtype: 'standard',
          department_id: 1,
          store_id: 2,
          building_id: null,
          customer_id: null,
          brand_id: null,
          trade_id: null,
          management_type_id: null,
          tenant_name: 'Tenant A',
          start_date: '2026-01-01',
          end_date: '2026-12-31',
          status: 'draft',
          workflow_instance_id: null,
          effective_version: 1,
          submitted_at: null,
          approved_at: null,
          billing_effective_at: null,
          terminated_at: null,
          created_by: 1,
          updated_by: 1,
          created_at: '2026-01-01T00:00:00Z',
          updated_at: '2026-01-01T00:00:00Z',
          joint_operation: null,
          ad_boards: [],
          area_grounds: [],
          units: [],
          terms: [],
        },
      },
    } as never)

    vi.mocked(listOvertimeBills).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 100,
      },
    } as never)

    const wrapper = mount(LeaseDetailView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElAlert: ElAlertStub,
          ElButton: passthroughStub('button'),
          ElTag: passthroughStub('span'),
          ElCard: passthroughStub('section'),
          ElDescriptions: passthroughStub('dl'),
          ElDescriptionsItem: passthroughStub('div'),
          ElForm: passthroughStub('form'),
          ElFormItem: passthroughStub('div'),
          ElDatePicker: passthroughStub('input'),
          ElInput: passthroughStub('input'),
          ElInputNumber: passthroughStub('input'),
          ElSelect: passthroughStub('select'),
          ElOption: passthroughStub('option'),
          ElTable: ElTableStub,
          ElTableColumn: passthroughStub(),
          ElEmpty: passthroughStub(),
          ElSkeleton: passthroughStub(),
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.find('[data-testid="lease-amend-button"]').exists()).toBe(false)
  })
})
