import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it } from 'vitest'

import { i18n } from '../i18n'
import MasterDataAdminView from './MasterDataAdminView.vue'

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
        h('div', slots.actions?.()),
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
    title: { type: String, required: true },
    description: { type: String, required: true },
  },
  setup(props) {
    return () => h('div', { 'data-testid': 'page-feedback-alert' }, `${props.title} ${props.description}`)
  },
})

const CustomerBrandSectionStub = defineComponent({
  emits: ['summary-change', 'load-feedback-change'],
  setup(_, { emit }) {
    emit('summary-change', {
      customers: [{ id: 101, code: 'CUST-101', name: 'ACME Retail', trade_id: 102, department_id: 101, status: 'active', created_at: '2026-04-02T08:00:00Z', updated_at: '2026-04-02T08:00:00Z' }],
      customerTotal: 1,
      brands: [{ id: 201, code: 'BR-201', name: 'ACME Fashion', status: 'active', created_at: '2026-04-02T08:00:00Z', updated_at: '2026-04-02T08:00:00Z' }],
      brandTotal: 1,
    })
    emit('load-feedback-change', {
      type: 'error',
      title: 'ignored',
      description: 'customers unavailable',
    })

    return () => h('div', { 'data-testid': 'customer-brand-section-stub' })
  },
})

const BudgetProspectSectionStub = defineComponent({
  props: {
    customers: { type: Array, required: true },
    brands: { type: Array, required: true },
  },
  emits: ['summary-change', 'load-feedback-change'],
  setup(props, { emit }) {
    emit('summary-change', {
      unitRentBudgets: [{ unit_id: 101, fiscal_year: 2026, budget_price: 95, created_at: '2026-04-02T08:00:00Z', updated_at: '2026-04-02T08:00:00Z' }],
      storeRentBudgets: [],
      unitProspects: [{ unit_id: 101, fiscal_year: 2026, potential_customer_id: 101, prospect_brand_id: 201, prospect_trade_id: 102, avg_transaction: 280, prospect_rent_price: 110, rent_increment: '5% yearly', prospect_term_months: 36, created_at: '2026-04-02T08:00:00Z', updated_at: '2026-04-02T08:00:00Z' }],
    })
    emit('load-feedback-change', {
      type: 'error',
      title: 'ignored',
      description: 'prospects unavailable',
    })

    return () => h('div', { 'data-testid': 'budget-prospect-section-stub' }, `${props.customers.length}|${props.brands.length}`)
  },
})

describe('MasterDataAdminView', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
  })

  it('keeps the route entrypoint and composes section snapshots into page-level UI', async () => {
    const wrapper = mount(MasterDataAdminView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElTag: ElTagStub,
          ElAlert: ElAlertStub,
          MasterDataCustomerBrandSection: CustomerBrandSectionStub,
          MasterDataBudgetProspectSection: BudgetProspectSectionStub,
        },
      },
    })

    await nextTick()
    await nextTick()

    expect(wrapper.get('[data-testid="masterdata-admin-view"]')).toBeTruthy()
    expect(wrapper.get('h1').text()).toBe('Master data admin')
    expect(wrapper.text()).toContain('1 customers')
    expect(wrapper.text()).toContain('1 brands')
    expect(wrapper.text()).toContain('1 unit budgets')
    expect(wrapper.text()).toContain('1 unit prospects')
    expect(wrapper.get('[data-testid="budget-prospect-section-stub"]').text()).toBe('1|1')
    expect(wrapper.get('[data-testid="page-feedback-alert"]').text()).toContain('customers unavailable prospects unavailable')
  })
})
