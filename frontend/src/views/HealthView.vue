<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import { http } from '../api/http'
import PageSection from '../components/platform/PageSection.vue'
import { useAppStore } from '../stores/app'

type HealthState = 'idle' | 'loading' | 'ok' | 'error'

const appStore = useAppStore()
const { t } = useI18n()
const state = ref<HealthState>('idle')
const checkedAt = ref<string>(t('common.notCheckedYet'))
const responseCode = ref<string>(t('common.notAvailable'))
const responsePayload = ref<string>(t('health.defaults.noBackendResponse'))
const errorMessage = ref<string>('')

const statusLabel = computed(() => {
  switch (state.value) {
    case 'loading':
      return t('common.statuses.checking')
    case 'ok':
      return t('common.statuses.healthy')
    case 'error':
      return t('common.statuses.unavailable')
    default:
      return t('common.statuses.idle')
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
  new Intl.DateTimeFormat(appStore.locale, {
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
    responseCode.value = t('health.defaults.requestFailed')
    responsePayload.value = t('health.errors.backendUnreachable')
    errorMessage.value = error instanceof Error ? error.message : t('health.errors.unknown')
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
      :eyebrow="t('health.eyebrow')"
      :title="t('health.title')"
      :summary="t('health.summary')"
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
          {{ t('health.actions.refreshBackendCheck') }}
        </el-button>
      </div>
    </PageSection>

    <section class="health-view__grid">
      <el-card class="health-view__card" shadow="never">
        <template #header>
          <div class="health-view__card-header">
            <span>{{ t('health.cards.runtimeStatus') }}</span>
          </div>
        </template>

        <el-descriptions :column="1" border data-testid="health-runtime-summary">
          <el-descriptions-item :label="t('health.fields.appName')">{{ appStore.appName }}</el-descriptions-item>
          <el-descriptions-item :label="t('health.fields.version')">{{ appStore.appVersion }}</el-descriptions-item>
          <el-descriptions-item :label="t('health.fields.mode')">{{ appStore.mode }}</el-descriptions-item>
          <el-descriptions-item :label="t('health.fields.apiBaseUrl')">
            <span data-testid="health-api-base-url">{{ appStore.apiBaseUrl }}</span>
          </el-descriptions-item>
          <el-descriptions-item :label="t('health.fields.lastCheck')">{{ checkedAt }}</el-descriptions-item>
          <el-descriptions-item :label="t('health.fields.responseCode')">{{ responseCode }}</el-descriptions-item>
        </el-descriptions>
      </el-card>

      <el-card class="health-view__card" shadow="never">
        <template #header>
          <div class="health-view__card-header">
            <span>{{ t('health.cards.backendProbe') }}</span>
          </div>
        </template>

        <el-alert
          v-if="errorMessage"
          :closable="false"
          class="health-view__alert"
          show-icon
          :title="t('health.alerts.backendCheckFailed')"
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
