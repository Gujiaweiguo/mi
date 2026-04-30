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

vi.mock('../api/billing', () => ({
  listCharges: vi.fn(),
}))

vi.mock('../api/invoice', () => ({
  listInvoices: vi.fn(),
  listReceivables: vi.fn(),
}))

import { getLease, listOvertimeBills } from '../api/lease'
import { listCharges } from '../api/billing'
import { listInvoices, listReceivables } from '../api/invoice'

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

const ElCardStub = defineComponent({
  setup(_, { slots, attrs }) {
    return () => h('section', attrs, [slots.header?.(), slots.default?.()])
  },
})

const ElAlertStub = defineComponent({
  props: { title: { type: String, default: '' }, description: { type: String, default: '' } },
  setup(props, { attrs }) {
    return () => h('div', attrs, `${props.title} ${props.description}`.trim())
  },
})

const ElEmptyStub = defineComponent({
  props: { description: { type: String, default: '' } },
  setup(props, { attrs }) {
    return () => h('div', attrs, props.description)
  },
})

const ElSkeletonStub = defineComponent({
  setup(_, { attrs }) {
    return () => h('div', attrs, 'loading')
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

const buildLease = (status: string = 'active') => ({
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
  status,
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
})

const mountView = () =>
  mount(LeaseDetailView, {
    global: {
      plugins: [i18n],
      stubs: {
        PageSection: PageSectionStub,
        ElAlert: ElAlertStub,
        ElButton: passthroughStub('button'),
        ElTag: passthroughStub('span'),
        ElCard: ElCardStub,
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
        ElEmpty: ElEmptyStub,
        ElSkeleton: ElSkeletonStub,
      },
      directives: {
        loading: {},
      },
    },
  })

describe('LeaseDetailView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'

    vi.mocked(getLease).mockResolvedValue({
      data: {
        lease: buildLease(),
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

    vi.mocked(listCharges).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 10,
      },
    } as never)

    vi.mocked(listInvoices).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 10,
      },
    } as never)

    vi.mocked(listReceivables).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 10,
      },
    } as never)
  })

  it('loads lease detail, overtime bills, and amendment entry for active leases', async () => {
    const wrapper = mountView()

    await flushPromises()

    expect(getLease).toHaveBeenCalledWith(42)
    expect(listOvertimeBills).toHaveBeenCalledWith({ lease_contract_id: 42, page: 1, page_size: 100 })
    expect(listCharges).toHaveBeenCalledWith({ lease_contract_id: 42, page: 1, page_size: 10 })
    expect(listInvoices).toHaveBeenCalledWith({ lease_contract_id: 42, page: 1, page_size: 10 })
    expect(listReceivables).toHaveBeenCalledWith({ lease_contract_id: 42, page: 1, page_size: 10 })
    expect(wrapper.get('[data-testid="lease-detail-view"]')).toBeTruthy()
    expect(wrapper.get('[data-testid="lease-downstream-panel"]').text()).toContain('Downstream business review')

    await wrapper.get('[data-testid="lease-amend-button"]').trigger('click')

    expect(push).toHaveBeenCalledWith({ name: 'lease-contracts-amend', params: { id: '42' } })
  })

  it('hides amendment entry when the lease is not amendment-eligible', async () => {
    vi.mocked(getLease).mockResolvedValue({
      data: {
        lease: buildLease('draft'),
      },
    } as never)

    const wrapper = mountView()

    await flushPromises()

    expect(wrapper.find('[data-testid="lease-amend-button"]').exists()).toBe(false)
  })

  it('renders downstream summaries and quick links for lease-linked charges, invoices, and receivables', async () => {
    vi.mocked(listCharges).mockResolvedValue({
      data: {
        items: [
          {
            id: 701,
            billing_run_id: 1,
            lease_contract_id: 42,
            lease_no: 'L-042',
            tenant_name: 'Tenant A',
            lease_term_id: 10,
            charge_type: 'Rent',
            charge_source: 'standard',
            overtime_bill_id: null,
            overtime_formula_id: null,
            overtime_charge_id: null,
            period_start: '2026-03-01',
            period_end: '2026-03-31',
            quantity_days: 31,
            unit_amount: 100,
            amount: 3100,
            currency_type_id: 1,
            source_effective_version: 1,
            created_at: '2026-03-31T00:00:00Z',
          },
        ],
        total: 1,
        page: 1,
        page_size: 10,
      },
    } as never)

    vi.mocked(listInvoices).mockResolvedValue({
      data: {
        items: [
          {
            id: 801,
            document_type: 'invoice',
            document_no: 'INV-801',
            billing_run_id: 10,
            lease_contract_id: 42,
            tenant_name: 'Tenant A',
            period_start: '2026-03-01',
            period_end: '2026-03-31',
            total_amount: 3100,
            currency_type_id: 1,
            status: 'approved',
            workflow_instance_id: null,
            adjusted_from_id: null,
            submitted_at: '2026-04-01T00:00:00Z',
            approved_at: '2026-04-02T00:00:00Z',
            cancelled_at: null,
            created_by: 1,
            updated_by: 1,
            created_at: '2026-04-01T00:00:00Z',
            updated_at: '2026-04-02T00:00:00Z',
            lines: [],
          },
        ],
        total: 1,
        page: 1,
        page_size: 10,
      },
    } as never)

    vi.mocked(listReceivables).mockResolvedValue({
      data: {
        items: [
          {
            billing_document_id: 801,
            document_type: 'invoice',
            document_no: 'INV-801',
            tenant_name: 'Tenant A',
            document_status: 'approved',
            lease_contract_id: 42,
            customer_id: 3,
            department_id: 1,
            trade_id: null,
            earliest_due_date: '2026-04-15',
            latest_due_date: '2026-04-20',
            outstanding_amount: 1200,
            settlement_status: 'outstanding',
          },
        ],
        total: 1,
        page: 1,
        page_size: 10,
      },
    } as never)

    const wrapper = mountView()

    await flushPromises()

    const panelText = wrapper.get('[data-testid="lease-downstream-panel"]').text()

    expect(panelText).toContain('Downstream business review')
    expect(panelText).toContain('Rent')
    expect(panelText).toContain('INV-801')
    expect(panelText).toContain('Outstanding')

    await wrapper.get('[data-testid="lease-downstream-open-invoice-801"]').trigger('click')

    expect(push).toHaveBeenCalledWith({ name: 'billing-invoice-detail', params: { id: '801' } })
  })

  it('shows explicit downstream empty states when no linked records exist', async () => {
    const wrapper = mountView()

    await flushPromises()

    expect(wrapper.get('[data-testid="lease-downstream-charges-empty"]').text()).toContain(
      'No billing charges have been generated for this lease yet.',
    )
    expect(wrapper.get('[data-testid="lease-downstream-invoices-empty"]').text()).toContain(
      'No billing documents have been issued for this lease yet.',
    )
    expect(wrapper.get('[data-testid="lease-downstream-receivables-empty"]').text()).toContain(
      'No receivable records are linked to this lease yet.',
    )
  })

  it('keeps other downstream sections usable when one section fails', async () => {
    vi.mocked(listCharges).mockRejectedValue(new Error('charge service unavailable'))
    vi.mocked(listInvoices).mockResolvedValue({
      data: {
        items: [
          {
            id: 802,
            document_type: 'bill',
            document_no: 'BILL-802',
            billing_run_id: 10,
            lease_contract_id: 42,
            tenant_name: 'Tenant A',
            period_start: '2026-05-01',
            period_end: '2026-05-31',
            total_amount: 2200,
            currency_type_id: 1,
            status: 'draft',
            workflow_instance_id: null,
            adjusted_from_id: null,
            submitted_at: null,
            approved_at: null,
            cancelled_at: null,
            created_by: 1,
            updated_by: 1,
            created_at: '2026-05-01T00:00:00Z',
            updated_at: '2026-05-01T00:00:00Z',
            lines: [],
          },
        ],
        total: 1,
        page: 1,
        page_size: 10,
      },
    } as never)

    vi.mocked(listReceivables).mockResolvedValue({
      data: {
        items: [
          {
            billing_document_id: 802,
            document_type: 'bill',
            document_no: 'BILL-802',
            tenant_name: 'Tenant A',
            document_status: 'draft',
            lease_contract_id: 42,
            customer_id: 3,
            department_id: 1,
            trade_id: null,
            earliest_due_date: '2026-05-20',
            latest_due_date: '2026-05-20',
            outstanding_amount: 2200,
            settlement_status: 'outstanding',
          },
        ],
        total: 1,
        page: 1,
        page_size: 10,
      },
    } as never)

    const wrapper = mountView()

    await flushPromises()

    expect(wrapper.get('[data-testid="lease-downstream-charges-error"]').text()).toContain('charge service unavailable')
    expect(wrapper.get('[data-testid="lease-downstream-invoices-section"]').text()).toContain('BILL-802')
    expect(wrapper.get('[data-testid="lease-downstream-receivables-section"]').text()).toContain('2,200.00')
    expect(wrapper.get('[data-testid="lease-overtime-section"]')).toBeTruthy()
  })
})
