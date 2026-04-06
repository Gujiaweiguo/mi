<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { listPrintTemplates, renderPrintPdf, type PrintTemplate } from '../api/print'
import PageSection from '../components/platform/PageSection.vue'
import { useAppStore } from '../stores/app'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const appStore = useAppStore()
const { t } = useI18n()

const templates = ref<PrintTemplate[]>([])
const total = ref(0)
const isLoading = ref(false)
const isRendering = ref(false)
const feedback = ref<Feedback | null>(null)

const selectedTemplateCode = ref('')
const documentIdsInput = ref('')

const resolveStatusLabel = (status: string) => {
  if (status === 'active') {
    return t('common.statuses.active')
  }

  return status
}

const loadTemplates = async () => {
  isLoading.value = true
  feedback.value = null

  try {
    const response = await listPrintTemplates()
    templates.value = response.data.items
    total.value = response.data.total
  } catch (error) {
    templates.value = []
    total.value = 0
    feedback.value = {
      type: 'error',
      title: t('printPreview.errors.templatesUnavailable'),
      description: error instanceof Error ? error.message : t('printPreview.errors.unableToLoadTemplates'),
    }
  } finally {
    isLoading.value = false
  }
}

const parseDocumentIds = () => {
  return documentIdsInput.value
    .split(',')
    .map((s) => s.trim())
    .filter((s) => s.length > 0)
    .map(Number)
    .filter((n) => Number.isFinite(n) && n > 0)
}

const canRender = () => Boolean(selectedTemplateCode.value && parseDocumentIds().length > 0)

const formatDate = (value: string) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(appStore.locale, { dateStyle: 'medium' }).format(new Date(value))
}

const handleRenderPdf = async () => {
  const ids = parseDocumentIds()
  if (!selectedTemplateCode.value || ids.length === 0) {
    feedback.value = {
      type: 'warning',
      title: t('printPreview.feedback.parametersRequiredTitle'),
      description: t('printPreview.feedback.parametersRequiredDescription'),
    }
    return
  }

  isRendering.value = true
  feedback.value = null

  try {
    const response = await renderPrintPdf({
      template_code: selectedTemplateCode.value,
      document_ids: ids,
    })

    const blob = new Blob([response.data as unknown as BlobPart], { type: 'application/pdf' })
    const url = URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = 'print-output.pdf'
    link.click()
    URL.revokeObjectURL(url)

    feedback.value = {
      type: 'success',
      title: t('printPreview.feedback.generatedTitle'),
      description: t('printPreview.feedback.generatedDescription'),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('printPreview.errors.generationFailed'),
      description: error instanceof Error ? error.message : t('printPreview.errors.unableToGeneratePdf'),
    }
  } finally {
    isRendering.value = false
  }
}

onMounted(() => {
  void loadTemplates()
})
</script>

<template>
  <div class="print-preview-view" data-testid="print-preview-view">
    <PageSection
      :eyebrow="t('printPreview.eyebrow')"
      :title="t('printPreview.title')"
      :summary="t('printPreview.summary')"
    />

    <el-alert
      v-if="feedback"
      data-testid="print-preview-feedback"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <el-card class="print-preview-view__card" shadow="never">
      <template #header>
        <div class="print-preview-view__card-header">
          <span>{{ t('printPreview.cards.renderPdf') }}</span>
        </div>
      </template>

      <div class="print-preview-view__render-form">
        <el-form label-position="top">
          <div class="print-preview-view__render-grid">
            <el-form-item :label="t('printPreview.fields.template')">
              <el-select
                v-model="selectedTemplateCode"
                :placeholder="t('printPreview.placeholders.selectTemplate')"
                clearable
                filterable
                data-testid="print-template-select"
              >
                <el-option
                  v-for="template in templates"
                  :key="template.id"
                  :label="template.code + ' — ' + template.name"
                  :value="template.code"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('printPreview.fields.documentIds')">
              <el-input
                v-model="documentIdsInput"
                :placeholder="t('printPreview.placeholders.enterDocumentIds')"
                clearable
                data-testid="print-document-ids"
              />
            </el-form-item>
          </div>

          <el-button
            type="primary"
            :loading="isRendering"
            :disabled="!canRender()"
            data-testid="print-render-pdf-button"
            @click="handleRenderPdf"
          >
            {{ t('printPreview.actions.generatePdf') }}
          </el-button>
        </el-form>
      </div>
    </el-card>

    <el-card class="print-preview-view__card" shadow="never">
      <template #header>
        <div class="print-preview-view__card-header">
          <span>{{ t('printPreview.cards.availableTemplates') }}</span>
          <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
        </div>
      </template>

      <el-table
        v-loading="isLoading"
        :data="templates"
        row-key="id"
        class="print-preview-view__table"
        :empty-text="t('printPreview.table.empty')"
        data-testid="print-templates-table"
      >
        <el-table-column prop="code" :label="t('printPreview.columns.code')" min-width="160" />
        <el-table-column prop="name" :label="t('printPreview.columns.name')" min-width="220" />
        <el-table-column prop="document_type" :label="t('printPreview.columns.documentType')" min-width="140" />
        <el-table-column prop="output_mode" :label="t('printPreview.columns.outputMode')" min-width="120" />
        <el-table-column :label="t('common.columns.status')" min-width="120">
          <template #default="scope">
            <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
              {{ resolveStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="t('common.columns.createdAt')" min-width="160">
          <template #default="scope">
            {{ formatDate(scope.row.created_at) }}
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.print-preview-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.print-preview-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.print-preview-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.print-preview-view__render-form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.print-preview-view__render-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.print-preview-view__table {
  width: 100%;
}

@media (max-width: 52rem) {
  .print-preview-view__render-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
