import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import PrintPreviewView from './PrintPreviewView.vue'

vi.mock('vue-router', () => ({
  useRouter: () => ({ push: vi.fn() }),
}))

vi.mock('../api/print', () => ({
  listPrintTemplates: vi.fn(),
  renderPrintPdf: vi.fn(),
  renderPrintHtml: vi.fn(),
  upsertPrintTemplate: vi.fn(),
}))

import { listPrintTemplates, renderPrintHtml } from '../api/print'

const mockTemplates = [
  {
    id: 1,
    code: 'TPL-001',
    name: 'Invoice Template',
    document_type: 'invoice',
    output_mode: 'invoice_batch',
    status: 'active',
    title: 'Invoice',
    subtitle: '',
    header_lines: [],
    footer_lines: [],
    created_by: 1,
    updated_by: 1,
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(PrintPreviewView, {
    global: {
      plugins: [i18n, ElementPlus],
    },
  })

  await flushPromises()

  return wrapper
}

const setTemplateAndIds = async (wrapper: Awaited<ReturnType<typeof mountView>>) => {
  // Vue 3 auto-unwraps refs on vm, so direct assignment works
  ;(wrapper.vm as any).selectedTemplateCode = 'TPL-001'
  ;(wrapper.vm as any).documentIdsInput = '1, 2'
  await flushPromises()
}

describe('PrintPreviewView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')

    vi.mocked(listPrintTemplates).mockResolvedValue({
      data: {
        items: mockTemplates,
        total: mockTemplates.length,
        page: 1,
        page_size: 20,
      },
    } as never)
  })

  it('calls renderPrintHtml with correct payload and opens preview dialog', async () => {
    const htmlContent = '<html><body><h1>Test HTML</h1></body></html>'
    vi.mocked(renderPrintHtml).mockResolvedValue({ data: htmlContent } as never)

    const wrapper = await mountView()

    await setTemplateAndIds(wrapper)

    await wrapper.get('[data-testid="print-render-html-button"]').trigger('click')
    await flushPromises()

    expect(renderPrintHtml).toHaveBeenCalledWith({
      template_code: 'TPL-001',
      document_ids: [1, 2],
    })

    expect(wrapper.find('[data-testid="print-html-preview"]').exists()).toBe(true)
    expect(wrapper.get('[data-testid="print-preview-feedback"]').text()).toContain('HTML generated successfully')
  })

  it('shows error feedback when HTML generation fails', async () => {
    vi.mocked(renderPrintHtml).mockRejectedValue(new Error('Server error'))

    const wrapper = await mountView()

    await setTemplateAndIds(wrapper)

    await wrapper.get('[data-testid="print-render-html-button"]').trigger('click')
    await flushPromises()

    expect(renderPrintHtml).toHaveBeenCalledWith({
      template_code: 'TPL-001',
      document_ids: [1, 2],
    })
    expect(wrapper.get('[data-testid="print-preview-feedback"]').text()).toContain('Failed to generate HTML')
    expect(wrapper.get('[data-testid="print-preview-feedback"]').text()).toContain('Server error')
  })

  it('disables HTML button when no template or document IDs are selected', async () => {
    const wrapper = await mountView()

    expect((wrapper.get('[data-testid="print-render-html-button"]').element as HTMLButtonElement).disabled).toBe(true)
  })

  it('shows warning feedback when rendering HTML without required parameters', async () => {
    const wrapper = await mountView()

    const vm = wrapper.vm as unknown as {
      handleRenderHtml: () => Promise<void>
    }
    await vm.handleRenderHtml()
    await flushPromises()

    expect(renderPrintHtml).not.toHaveBeenCalled()
    expect(wrapper.get('[data-testid="print-preview-feedback"]').text()).toContain('Render parameters required')
  })
})
