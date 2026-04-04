<script setup lang="ts">
import { onMounted, ref } from 'vue'

import { listPrintTemplates, renderPrintPdf, type PrintTemplate } from '../api/print'
import PageSection from '../components/platform/PageSection.vue'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const templates = ref<PrintTemplate[]>([])
const total = ref(0)
const isLoading = ref(false)
const isRendering = ref(false)
const feedback = ref<Feedback | null>(null)

const selectedTemplateCode = ref('')
const documentIdsInput = ref('')

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
      title: 'Print templates unavailable',
      description: error instanceof Error ? error.message : 'Unable to load print templates.',
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
  if (!value) return '\u2014'
  return new Intl.DateTimeFormat('en-US', { dateStyle: 'medium' }).format(new Date(value))
}

const handleRenderPdf = async () => {
  const ids = parseDocumentIds()
  if (!selectedTemplateCode.value || ids.length === 0) {
    feedback.value = {
      type: 'warning',
      title: 'Render parameters required',
      description: 'Select a template and enter at least one document ID.',
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
      title: 'PDF generated',
      description: 'Document has been rendered and downloaded successfully.',
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: 'PDF generation failed',
      description: error instanceof Error ? error.message : 'Unable to generate PDF output.',
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
      eyebrow="Document output"
      title="Print preview"
      summary="Select a print template and document IDs to generate PDF output for billing documents."
    />

    <el-alert
      v-if="feedback"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <el-card class="print-preview-view__card" shadow="never">
      <template #header>
        <div class="print-preview-view__card-header">
          <span>Render PDF</span>
        </div>
      </template>

      <div class="print-preview-view__render-form">
        <el-form label-position="top">
          <div class="print-preview-view__render-grid">
            <el-form-item label="Template">
              <el-select
                v-model="selectedTemplateCode"
                placeholder="Select a print template"
                clearable
                filterable
                data-testid="print-template-select"
              >
                <el-option
                  v-for="t in templates"
                  :key="t.id"
                  :label="t.code + ' \u2014 ' + t.name"
                  :value="t.code"
                />
              </el-select>
            </el-form-item>

            <el-form-item label="Document IDs">
              <el-input
                v-model="documentIdsInput"
                placeholder="Comma-separated document IDs (e.g. 1, 2, 3)"
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
            Generate PDF
          </el-button>
        </el-form>
      </div>
    </el-card>

    <el-card class="print-preview-view__card" shadow="never">
      <template #header>
        <div class="print-preview-view__card-header">
          <span>Available templates</span>
          <el-tag effect="plain" type="info">{{ total }} total</el-tag>
        </div>
      </template>

      <el-table
        :data="templates"
        row-key="id"
        class="print-preview-view__table"
        empty-text="No print templates available."
        data-testid="print-templates-table"
      >
        <el-table-column prop="code" label="Code" min-width="160" />
        <el-table-column prop="name" label="Name" min-width="220" />
        <el-table-column prop="document_type" label="Document type" min-width="140" />
        <el-table-column prop="output_mode" label="Output mode" min-width="120" />
        <el-table-column prop="status" label="Status" min-width="120">
          <template #default="scope">
            <el-tag :type="scope.row.status === 'active' ? 'success' : 'info'" effect="plain">
              {{ scope.row.status }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="Created" min-width="160">
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
