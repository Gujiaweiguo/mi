<script setup lang="ts">
import { computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { downloadUnitTemplate, importUnits, exportOperational, type ImportResult } from '../api/excel'
import PageSection from '../components/platform/PageSection.vue'
import { downloadBlob } from '../composables/useDownload'
import { getErrorMessage } from '../composables/useErrorMessage'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const feedback = ref<Feedback | null>(null)
const { t } = useI18n()
const isDownloadingTemplate = ref(false)
const isImporting = ref(false)
const isExporting = ref(false)
const selectedDataset = ref('')
const importFile = ref<File | null>(null)
const importResult = ref<ImportResult | null>(null)

const datasetOptions = computed(() => [
  { label: t('excel.datasets.billingCharges'), value: 'billing_charges' },
  { label: t('excel.datasets.leaseContracts'), value: 'lease_contracts' },
  { label: t('excel.datasets.unitData'), value: 'unit_data' },
  { label: t('excel.datasets.invoices'), value: 'invoices' },
])

const handleDownloadTemplate = async () => {
  isDownloadingTemplate.value = true
  feedback.value = null

  try {
    const response = await downloadUnitTemplate()
    const blob = new Blob([response.data as BlobPart], { type: 'application/octet-stream' })
    downloadBlob(blob, 'unit-data-template.xlsx')
    feedback.value = {
      type: 'success',
      title: t('excel.feedback.templateDownloaded'),
      description: t('excel.feedback.templateDownloadedDescription'),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('excel.errors.templateDownloadFailed'),
      description: getErrorMessage(error, t('excel.errors.unableToDownloadTemplate')),
    }
  } finally {
    isDownloadingTemplate.value = false
  }
}

const handleFileChange = (uploadFile: File) => {
  importFile.value = uploadFile
}

const handleImport = async () => {
  if (!importFile.value) {
    feedback.value = {
      type: 'warning',
      title: t('excel.errors.noFileSelected'),
      description: t('excel.errors.selectFileToImport'),
    }
    return
  }

  isImporting.value = true
  feedback.value = null
  importResult.value = null

  try {
    const response = await importUnits(importFile.value)
    importResult.value = response.data
    feedback.value = {
      type: 'success',
      title: t('excel.feedback.importCompleted'),
      description: t('excel.feedback.importedCount', { count: response.data.imported_count }),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('excel.errors.importFailed'),
      description: getErrorMessage(error, t('excel.errors.unableToImport')),
    }
  } finally {
    isImporting.value = false
  }
}

const handleExport = async () => {
  if (!selectedDataset.value) {
    feedback.value = {
      type: 'warning',
      title: t('excel.errors.noDatasetSelected'),
      description: t('excel.errors.selectDatasetToExport'),
    }
    return
  }

  isExporting.value = true
  feedback.value = null

  try {
    const response = await exportOperational(selectedDataset.value)
    const blob = new Blob([response.data as BlobPart], { type: 'application/octet-stream' })
    downloadBlob(blob, `${selectedDataset.value}-export.xlsx`)
    feedback.value = {
      type: 'success',
      title: t('excel.feedback.exportCompleted'),
      description: t('excel.feedback.exportCompletedDescription', { dataset: selectedDataset.value }),
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: t('excel.errors.exportFailed'),
      description: getErrorMessage(error, t('excel.errors.unableToExport')),
    }
  } finally {
    isExporting.value = false
  }
}
</script>

<template>
  <div
    class="excel-io-view"
    v-loading="isDownloadingTemplate || isImporting || isExporting"
    data-testid="excel-io-view"
  >
    <PageSection
      :eyebrow="t('excel.eyebrow')"
      :title="t('excel.title')"
      :summary="t('excel.summary')"
    />

    <el-alert
      v-if="feedback"
      :closable="false"
      :title="feedback.title"
      :type="feedback.type"
      :description="feedback.description"
      show-icon
    />

    <div class="excel-io-view__grid">
      <el-card class="excel-io-view__card" shadow="never">
        <template #header>
          <div class="excel-io-view__card-header">
            <span>{{ t('excel.cards.templateDownload') }}</span>
          </div>
        </template>

        <p class="excel-io-view__card-description">
          {{ t('excel.cards.templateDescription') }}
        </p>

        <el-button
          type="primary"
          :loading="isDownloadingTemplate"
          data-testid="excel-download-template"
          @click="handleDownloadTemplate"
        >
          {{ t('excel.actions.downloadTemplate') }}
        </el-button>
      </el-card>

      <el-card class="excel-io-view__card" shadow="never">
        <template #header>
          <div class="excel-io-view__card-header">
            <span>{{ t('excel.cards.importData') }}</span>
          </div>
        </template>

        <div class="excel-io-view__import-section">
          <el-upload
            :auto-upload="false"
            :limit="1"
            accept=".xlsx,.xls"
            :on-change="(file: any) => handleFileChange(file.raw)"
            data-testid="excel-upload-input"
          >
            <template #trigger>
              <el-button>{{ t('excel.actions.selectFile') }}</el-button>
            </template>
          </el-upload>

          <el-button
            type="primary"
            :loading="isImporting"
            :disabled="!importFile"
            data-testid="excel-import-button"
            @click="handleImport"
          >
            {{ t('excel.actions.importUnitData') }}
          </el-button>
        </div>

        <div v-if="importResult" class="excel-io-view__import-result">
          <el-tag effect="plain" type="success">
            {{ t('excel.feedback.importedTag', { count: importResult.imported_count }) }}
          </el-tag>

          <el-table
            v-if="importResult.diagnostics.length > 0"
            :data="importResult.diagnostics"
            row-key="row"
            size="small"
            class="excel-io-view__diagnostics-table"
          >
            <el-table-column prop="row" :label="t('excel.columns.row')" min-width="80" />
            <el-table-column prop="field" :label="t('excel.columns.field')" min-width="140" />
            <el-table-column prop="message" :label="t('excel.columns.message')" min-width="280" />
          </el-table>
        </div>
      </el-card>

      <el-card class="excel-io-view__card" shadow="never">
        <template #header>
          <div class="excel-io-view__card-header">
            <span>{{ t('excel.cards.exportDataset') }}</span>
          </div>
        </template>

        <div class="excel-io-view__export-section">
          <el-select
            v-model="selectedDataset"
            :placeholder="t('excel.placeholders.selectDataset')"
            clearable
            data-testid="excel-export-dataset"
          >
            <el-option
              v-for="opt in datasetOptions"
              :key="opt.value"
              :label="opt.label"
              :value="opt.value"
            />
          </el-select>

          <el-button
            type="primary"
            :loading="isExporting"
            :disabled="!selectedDataset"
            data-testid="excel-export-button"
            @click="handleExport"
          >
            {{ t('excel.actions.export') }}
          </el-button>
        </div>
      </el-card>
    </div>
  </div>
</template>

<style scoped>
.excel-io-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.excel-io-view__grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.excel-io-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.excel-io-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.excel-io-view__card-description {
  margin: 0 0 var(--mi-space-4);
  font-size: var(--mi-font-size-200);
  color: var(--mi-color-muted);
}

.excel-io-view__import-section,
.excel-io-view__export-section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.excel-io-view__import-result {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
  margin-top: var(--mi-space-3);
}

.excel-io-view__diagnostics-table {
  width: 100%;
}

@media (max-width: 52rem) {
  .excel-io-view__grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
