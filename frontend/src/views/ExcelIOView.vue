<script setup lang="ts">
import { ref } from 'vue'

import { downloadUnitTemplate, importUnits, exportOperational, type ImportResult } from '../api/excel'
import PageSection from '../components/platform/PageSection.vue'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

const feedback = ref<Feedback | null>(null)
const isDownloadingTemplate = ref(false)
const isImporting = ref(false)
const isExporting = ref(false)
const selectedDataset = ref('')
const importFile = ref<File | null>(null)
const importResult = ref<ImportResult | null>(null)

const datasetOptions = [
  { label: 'Billing charges', value: 'billing_charges' },
  { label: 'Lease contracts', value: 'lease_contracts' },
  { label: 'Unit data', value: 'unit_data' },
]

const downloadBlob = (data: unknown, filename: string) => {
  const blob = new Blob([data as BlobPart], { type: 'application/octet-stream' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = filename
  link.click()
  URL.revokeObjectURL(url)
}

const handleDownloadTemplate = async () => {
  isDownloadingTemplate.value = true
  feedback.value = null

  try {
    const response = await downloadUnitTemplate()
    downloadBlob(response.data, 'unit-data-template.xlsx')
    feedback.value = {
      type: 'success',
      title: 'Template downloaded',
      description: 'Unit data template has been downloaded successfully.',
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: 'Template download failed',
      description: error instanceof Error ? error.message : 'Unable to download the template.',
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
      title: 'No file selected',
      description: 'Please select an Excel file to import.',
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
      title: 'Import completed',
      description: `Imported ${response.data.imported_count} records successfully.`,
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: 'Import failed',
      description: error instanceof Error ? error.message : 'Unable to import the file.',
    }
  } finally {
    isImporting.value = false
  }
}

const handleExport = async () => {
  if (!selectedDataset.value) {
    feedback.value = {
      type: 'warning',
      title: 'No dataset selected',
      description: 'Please select a dataset to export.',
    }
    return
  }

  isExporting.value = true
  feedback.value = null

  try {
    const response = await exportOperational(selectedDataset.value)
    downloadBlob(response.data, `${selectedDataset.value}-export.xlsx`)
    feedback.value = {
      type: 'success',
      title: 'Export completed',
      description: `Dataset "${selectedDataset.value}" has been exported successfully.`,
    }
  } catch (error) {
    feedback.value = {
      type: 'error',
      title: 'Export failed',
      description: error instanceof Error ? error.message : 'Unable to export the dataset.',
    }
  } finally {
    isExporting.value = false
  }
}
</script>

<template>
  <div class="excel-io-view" data-testid="excel-io-view">
    <PageSection
      eyebrow="Data exchange"
      title="Excel import / export"
      summary="Download templates, import operational data from Excel files, and export datasets for offline processing."
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
            <span>Template download</span>
          </div>
        </template>

        <p class="excel-io-view__card-description">
          Download the standard unit data template to prepare your import file.
        </p>

        <el-button
          type="primary"
          :loading="isDownloadingTemplate"
          data-testid="excel-download-template"
          @click="handleDownloadTemplate"
        >
          Download unit data template
        </el-button>
      </el-card>

      <el-card class="excel-io-view__card" shadow="never">
        <template #header>
          <div class="excel-io-view__card-header">
            <span>Import data</span>
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
              <el-button>Select file</el-button>
            </template>
          </el-upload>

          <el-button
            type="primary"
            :loading="isImporting"
            :disabled="!importFile"
            data-testid="excel-import-button"
            @click="handleImport"
          >
            Import unit data
          </el-button>
        </div>

        <div v-if="importResult" class="excel-io-view__import-result">
          <el-tag effect="plain" type="success">
            {{ importResult.imported_count }} records imported
          </el-tag>

          <el-table
            v-if="importResult.diagnostics.length > 0"
            :data="importResult.diagnostics"
            row-key="row"
            size="small"
            class="excel-io-view__diagnostics-table"
          >
            <el-table-column prop="row" label="Row" min-width="80" />
            <el-table-column prop="field" label="Field" min-width="140" />
            <el-table-column prop="message" label="Message" min-width="280" />
          </el-table>
        </div>
      </el-card>

      <el-card class="excel-io-view__card" shadow="never">
        <template #header>
          <div class="excel-io-view__card-header">
            <span>Export dataset</span>
          </div>
        </template>

        <div class="excel-io-view__export-section">
          <el-select
            v-model="selectedDataset"
            placeholder="Select a dataset"
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
            Export
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
