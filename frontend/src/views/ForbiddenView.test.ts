import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h } from 'vue'
import { beforeEach, describe, expect, it } from 'vitest'

import { i18n } from '../i18n'
import ForbiddenView from './ForbiddenView.vue'

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

describe('ForbiddenView', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
  })

  it('renders the forbidden view with translated content', () => {
    const wrapper = mount(ForbiddenView, {
      global: {
        plugins: [ElementPlus, i18n, createPinia()],
        stubs: { PageSection: PageSectionStub },
      },
    })

    expect(wrapper.get('[data-testid="forbidden-view"]')).toBeTruthy()
    expect(wrapper.findComponent(PageSectionStub).props()).toEqual({
      eyebrow: 'Permission boundary',
      title: 'Access not granted',
      summary: 'Your current session does not include the permissions required for this screen.',
    })
    expect(wrapper.get('.forbidden-view__card').text()).toContain(
      'Navigation and route guards automatically limit accessible areas based on backend permissions.',
    )
  })
})
