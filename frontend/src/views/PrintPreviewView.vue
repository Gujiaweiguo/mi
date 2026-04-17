<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  listPrintTemplates,
  renderPrintPdf,
  upsertPrintTemplate,
  type PrintTemplate,
  type UpsertPrintTemplateRequest,
} from '../api/print'
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
const dialogVisible = ref(false)
const isSubmitting = ref(false)
const isEditMode = ref(false)
const feedback = ref<Feedback | null>(null)

const selectedTemplateCode = ref('')
const documentIdsInput = ref('')
const headerLineInput = ref('')
const footerLineInput = ref('')

const formModel = ref({
  code: '',
  name: '',
  document_type: '',
  output_mode: '',
  title: '',
  subtitle: '',
  header_lines: [] as string[],
  footer_lines: [] as string[],
})

const documentTypeOptions = [
  { value: 'invoice', labelKey: 'printPreview.dialog.documentTypeOptions.invoice' },
  { value: 'receipt', labelKey: 'printPreview.dialog.documentTypeOptions.receipt' },
  { value: 'lease_contract', labelKey: 'printPreview.dialog.documentTypeOptions.leaseContract' },
] as const

const outputModeOptions = [
  { value: 'html', labelKey: 'printPreview.dialog.outputModeOptions.html' },
  { value: 'pdf', labelKey: 'printPreview.dialog.outputModeOptions.pdf' },
] as const

const dialogTitle = computed(() =>
  t(isEditMode.value ? 'printPreview.dialog.title.edit' : 'printPreview.dialog.title.create'),
)

const canSubmitDialog = computed(() => {
  return Boolean(
    formModel.value.code.trim() &&
      formModel.value.name.trim() &&
      formModel.value.document_type &&
      formModel.value.output_mode &&
      formModel.value.title.trim() &&
      formModel.value.subtitle.trim(),
  )
})

const resetFormModel = () => {
  formModel.value = {
    code: '',
    name: '',
    document_type: '',
    output_mode: '',
    title: '',
    subtitle: '',
    header_lines: [],
    footer_lines: [],
  }
  headerLineInput.value = ''
  footerLineInput.value = ''
}

const resolveOptionLabel = (value: string, options: ReadonlyArray<{ value: string; labelKey: string }>) => {
  const option = options.find((entry) => entry.value === value)

  return option ? t(option.labelKey) : value || t('common.emptyValue')
}

const resolveStatusLabel = (status: string) => {
  if (status === 'active') {
    return t('common.statuses.active')
  }

  return status
}

const resolveDocumentTypeLabel = (documentType: string) => {
  return resolveOptionLabel(documentType, documentTypeOptions)
}

const resolveOutputModeLabel = (outputMode: string) => {
  return resolveOptionLabel(outputMode, outputModeOptions)
}

const openCreateDialog = () => {
  isEditMode.value = false
  resetFormModel()
  dialogVisible.value = true
}

const openEditDialog = (template: PrintTemplate) => {
  isEditMode.value = true
  formModel.value = {
    code: template.code,
    name: template.name,
    document_type: template.document_type,
    output_mode: template.output_mode,
    title: template.title,
    subtitle: template.subtitle,
    header_lines: [...template.header_lines],
    footer_lines: [...template.footer_lines],
  }
  headerLineInput.value = ''
  footerLineInput.value = ''
  dialogVisible.value = true
}

const addHeaderLine = () => {
  const line = headerLineInput.value.trim()
  if (!line) {
    return
  }

  formModel.value.header_lines.push(line)
  headerLineInput.value = ''
}

const removeHeaderLine = (index: number) => {
  formModel.value.header_lines.splice(index, 1)
}

const addFooterLine = () => {
  const line = footerLineInput.value.trim()
  if (!line) {
    return
  }

  formModel.value.footer_lines.push(line)
  footerLineInput.value = ''
}

const removeFooterLine = (index: number) => {
  formModel.value.footer_lines.splice(index, 1)
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

const handleSubmit = async () => {
  if (!canSubmitDialog.value) {
    return
  }

  isSubmitting.value = true
  feedback.value = null

  const payload: UpsertPrintTemplateRequest = {
    code: formModel.value.code.trim(),
    name: formModel.value.name.trim(),
    document_type: formModel.value.document_type,
    output_mode: formModel.value.output_mode,
    title: formModel.value.title.trim(),
    subtitle: formModel.value.subtitle.trim(),
    header_lines: formModel.value.header_lines.map((line) => line.trim()).filter((line) => line.length > 0),
    footer_lines: formModel.value.footer_lines.map((line) => line.trim()).filter((line) => line.length > 0),
  }

  try {
    await upsertPrintTemplate(payload)
    dialogVisible.value = false
    selectedTemplateCode.value = payload.code
    await loadTemplates()
    feedback.value = {
      type: 'success',
      title: isEditMode.value ? t('printPreview.dialog.feedback.updated') : t('printPreview.dialog.feedback.created'),
      description: payload.name,
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('printPreview.dialog.feedback.failed'),
      description: error instanceof Error ? error.message : t('printPreview.errors.unableToLoadTemplates'),
    }
  } finally {
    isSubmitting.value = false
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
          <div class="print-preview-view__card-header-actions">
            <el-tag effect="plain" type="info">{{ t('common.total', { count: total }) }}</el-tag>
            <el-button type="primary" @click="openCreateDialog">
              {{ t('printPreview.dialog.actions.addTemplate') }}
            </el-button>
          </div>
        </div>
      </template>

      <el-table
        v-loading="isLoading"
        :data="templates"
        row-key="id"
        class="print-preview-view__table"
        :empty-text="t('printPreview.table.empty')"
        data-testid="print-templates-table"
        @row-click="openEditDialog"
      >
        <el-table-column prop="code" :label="t('printPreview.columns.code')" min-width="160" />
        <el-table-column prop="name" :label="t('printPreview.columns.name')" min-width="220" />
        <el-table-column :label="t('printPreview.columns.documentType')" min-width="140">
          <template #default="scope">
            {{ resolveDocumentTypeLabel(scope.row.document_type) }}
          </template>
        </el-table-column>
        <el-table-column :label="t('printPreview.columns.outputMode')" min-width="120">
          <template #default="scope">
            {{ resolveOutputModeLabel(scope.row.output_mode) }}
          </template>
        </el-table-column>
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

    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="52rem">
      <div class="print-preview-view__dialog-body">
        <el-form label-position="top" class="print-preview-view__dialog-form" @submit.prevent>
          <div class="print-preview-view__dialog-grid">
            <el-form-item :label="t('printPreview.dialog.fields.code')">
              <el-input v-model="formModel.code" />
            </el-form-item>

            <el-form-item :label="t('printPreview.dialog.fields.name')">
              <el-input v-model="formModel.name" />
            </el-form-item>

            <el-form-item :label="t('printPreview.dialog.fields.documentType')">
              <el-select v-model="formModel.document_type">
                <el-option
                  v-for="option in documentTypeOptions"
                  :key="option.value"
                  :label="t(option.labelKey)"
                  :value="option.value"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('printPreview.dialog.fields.outputMode')">
              <el-select v-model="formModel.output_mode">
                <el-option
                  v-for="option in outputModeOptions"
                  :key="option.value"
                  :label="t(option.labelKey)"
                  :value="option.value"
                />
              </el-select>
            </el-form-item>

            <el-form-item :label="t('printPreview.dialog.fields.title')">
              <el-input v-model="formModel.title" />
            </el-form-item>

            <el-form-item :label="t('printPreview.dialog.fields.subtitle')">
              <el-input v-model="formModel.subtitle" />
            </el-form-item>
          </div>
        </el-form>

        <div class="print-preview-view__dialog-section">
          <div class="print-preview-view__dialog-section-header">
            <span>{{ t('printPreview.dialog.fields.headerLines') }}</span>
          </div>

          <div class="print-preview-view__dialog-tag-list">
            <el-tag
              v-for="(line, index) in formModel.header_lines"
              :key="`header-${index}-${line}`"
              closable
              @close="removeHeaderLine(index)"
            >
              {{ line }}
            </el-tag>
          </div>

          <div class="print-preview-view__dialog-input-row">
            <el-input v-model="headerLineInput" @keyup.enter="addHeaderLine" />
            <el-button @click="addHeaderLine">{{ t('printPreview.dialog.actions.addLine') }}</el-button>
          </div>
        </div>

        <div class="print-preview-view__dialog-section">
          <div class="print-preview-view__dialog-section-header">
            <span>{{ t('printPreview.dialog.fields.footerLines') }}</span>
          </div>

          <div class="print-preview-view__dialog-tag-list">
            <el-tag
              v-for="(line, index) in formModel.footer_lines"
              :key="`footer-${index}-${line}`"
              closable
              @close="removeFooterLine(index)"
            >
              {{ line }}
            </el-tag>
          </div>

          <div class="print-preview-view__dialog-input-row">
            <el-input v-model="footerLineInput" @keyup.enter="addFooterLine" />
            <el-button @click="addFooterLine">{{ t('printPreview.dialog.actions.addLine') }}</el-button>
          </div>
        </div>
      </div>

      <template #footer>
        <el-button @click="dialogVisible = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isSubmitting" :disabled="!canSubmitDialog" @click="handleSubmit">
          {{ t('printPreview.dialog.actions.submit') }}
        </el-button>
      </template>
    </el-dialog>
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

.print-preview-view__card-header-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
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

.print-preview-view__table :deep(.el-table__row) {
  cursor: pointer;
}

.print-preview-view__dialog-body {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
  max-height: calc(100vh - var(--mi-space-6) * 4);
  overflow: auto;
}

.print-preview-view__dialog-form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.print-preview-view__dialog-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.print-preview-view__dialog-section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.print-preview-view__dialog-section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.print-preview-view__dialog-tag-list {
  display: flex;
  flex-wrap: wrap;
  gap: var(--mi-space-3);
}

.print-preview-view__dialog-input-row {
  display: grid;
  grid-template-columns: minmax(0, 1fr) auto;
  gap: var(--mi-space-3);
}

.print-preview-view__dialog-grid :deep(.el-input),
.print-preview-view__dialog-grid :deep(.el-select),
.print-preview-view__dialog-input-row :deep(.el-input) {
  width: 100%;
}

@media (max-width: 52rem) {
  .print-preview-view__render-grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .print-preview-view__card-header,
  .print-preview-view__card-header-actions {
    align-items: flex-start;
    flex-direction: column;
  }

  .print-preview-view__dialog-grid,
  .print-preview-view__dialog-input-row {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
