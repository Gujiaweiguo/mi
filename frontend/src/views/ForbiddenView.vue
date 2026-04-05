<script setup lang="ts">
import { useI18n } from 'vue-i18n'

import PageSection from '../components/platform/PageSection.vue'

const { t } = useI18n()
</script>

<template>
  <div class="forbidden-view" data-testid="forbidden-view">
    <PageSection
      :eyebrow="t('forbidden.eyebrow')"
      :title="t('forbidden.title')"
      :summary="t('forbidden.summary')"
    />

    <el-card class="forbidden-view__card" shadow="never">
      <p class="forbidden-view__copy">
        {{ t('forbidden.description') }}
      </p>
    </el-card>
  </div>
</template>

<style scoped>
.forbidden-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.forbidden-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.forbidden-view__copy {
  margin: 0;
  color: var(--mi-color-muted);
}
</style>
