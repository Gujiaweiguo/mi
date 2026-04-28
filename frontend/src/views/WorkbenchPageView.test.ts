import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h } from 'vue'
import { beforeEach, describe, expect, it } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import WorkbenchPageView from './WorkbenchPageView.vue'

const WorkbenchViewStub = defineComponent({
  props: {
    eyebrow: { type: String, required: true },
    title: { type: String, required: true },
    summary: { type: String, required: true },
    filterLabel: { type: String, required: true },
    filterPlaceholder: { type: String, required: true },
    rows: { type: Array, required: true },
    columns: { type: Array, required: true },
    testId: { type: String, required: true },
  },
  setup(props) {
    return () =>
      h('section', { 'data-testid': props.testId }, [
        h('span', props.eyebrow),
        h('h1', props.title),
        h('p', props.summary),
        h('label', props.filterLabel),
        h('input', { placeholder: props.filterPlaceholder }),
        h('strong', `columns:${props.columns.length}`),
        h('strong', `rows:${props.rows.length}`),
      ])
  },
})

describe('WorkbenchPageView', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('provides localized migration-closure queue props to the shared workbench', () => {
    const wrapper = mount(WorkbenchPageView, {
      global: {
        plugins: [i18n],
        stubs: {
          WorkbenchView: WorkbenchViewStub,
        },
      },
    })

    const workbench = wrapper.get('[data-testid="workbench-view"]')

    expect(workbench.text()).toContain('Migration closure')
    expect(workbench.text()).toContain('Workbench')
    expect(workbench.text()).toContain('Queue filter')
    expect(workbench.text()).toContain('columns:4')
    expect(workbench.text()).toContain('rows:3')
  })
})
