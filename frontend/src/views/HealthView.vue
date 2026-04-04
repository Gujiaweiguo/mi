<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

import { http } from '../api/http'
import PageSection from '../components/platform/PageSection.vue'
import { useAppStore } from '../stores/app'

type HealthState = 'idle' | 'loading' | 'ok' | 'error'

const appStore = useAppStore()
const state = ref<HealthState>('idle')
const checkedAt = ref<string>('Not checked yet')
const responseCode = ref<string>('n/a')
const responsePayload = ref<string>('No backend response yet.')
const errorMessage = ref<string>('')

const statusLabel = computed(() => {
  switch (state.value) {
    case 'loading':
      return 'Checking'
    case 'ok':
      return 'Healthy'
    case 'error':
      return 'Unavailable'
    default:
      return 'Idle'
  }
})

const statusTagType = computed(() => {
  switch (state.value) {
    case 'ok':
      return 'success'
    case 'error':
      return 'danger'
    default:
      return 'info'
  }
})

const formatTimestamp = (value: Date) =>
  new Intl.DateTimeFormat('en-US', {
    dateStyle: 'medium',
    timeStyle: 'medium',
  }).format(value)

const runHealthCheck = async () => {
  state.value = 'loading'
  errorMessage.value = ''

  try {
    const response = await http.get('/health')

    responseCode.value = String(response.status)
    responsePayload.value = JSON.stringify(response.data, null, 2)
    state.value = 'ok'
  } catch (error) {
    responseCode.value = 'request_failed'
    responsePayload.value = 'Backend health endpoint did not respond successfully.'
    errorMessage.value = error instanceof Error ? error.message : 'Unknown error'
    state.value = 'error'
  } finally {
    checkedAt.value = formatTimestamp(new Date())
  }
}

onMounted(runHealthCheck)
</script>

<template>
  <div class="health-view" data-testid="health-view">
    <PageSection
      eyebrow="Task 14 platform shell"
      title="Frontend platform health"
      summary="Authenticated shell, shared frontend primitives, and centralized API wiring are now in place for later business slices."
    >
      <div class="health-view__hero-actions">
        <el-tag :type="statusTagType" effect="dark" data-testid="health-status-tag">
          {{ statusLabel }}
        </el-tag>
        <el-button
          type="primary"
          plain
          data-testid="health-refresh-button"
          @click="runHealthCheck"
        >
          Refresh backend check
        </el-button>
      </div>
    </PageSection>

    <section class="health-view__grid">
      <el-card class="health-view__card" shadow="never">
        <template #header>
          <div class="health-view__card-header">
            <span>Runtime status</span>
          </div>
        </template>

        <el-descriptions :column="1" border data-testid="health-runtime-summary">
          <el-descriptions-item label="App name">{{ appStore.appName }}</el-descriptions-item>
          <el-descriptions-item label="Version">{{ appStore.appVersion }}</el-descriptions-item>
          <el-descriptions-item label="Mode">{{ appStore.mode }}</el-descriptions-item>
          <el-descriptions-item label="API base URL">
            <span data-testid="health-api-base-url">{{ appStore.apiBaseUrl }}</span>
          </el-descriptions-item>
          <el-descriptions-item label="Last check">{{ checkedAt }}</el-descriptions-item>
          <el-descriptions-item label="Response code">{{ responseCode }}</el-descriptions-item>
        </el-descriptions>
      </el-card>

      <el-card class="health-view__card" shadow="never">
        <template #header>
          <div class="health-view__card-header">
            <span>Backend probe</span>
          </div>
        </template>

        <el-alert
          v-if="errorMessage"
          :closable="false"
          class="health-view__alert"
          show-icon
          title="Backend check failed"
          type="error"
          :description="errorMessage"
          data-testid="health-error-alert"
        />

        <pre class="health-view__payload" data-testid="health-response-payload">{{ responsePayload }}</pre>
      </el-card>
    </section>
  </div>
</template>

<style scoped>
.health-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-6);
}

.health-view__hero-actions {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: var(--mi-space-3);
}

.health-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.health-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.health-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.health-view__alert {
  margin-bottom: var(--mi-space-4);
}

.health-view__payload {
  margin: 0;
  padding: var(--mi-space-4);
  min-height: var(--mi-panel-min-height);
  overflow: auto;
  border-radius: var(--mi-radius-sm);
  background: var(--mi-color-panel);
  color: var(--mi-color-text);
  font-size: var(--mi-font-size-100);
  line-height: var(--mi-line-height-base);
  font-family: var(--mi-font-family-mono);
}

@media (max-width: 52rem) {
  .health-view__hero-actions {
    align-items: flex-start;
  }

  .health-view__grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
