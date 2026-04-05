<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

import { APP_LOCALE_OPTIONS } from '../../i18n'
import { useAppStore } from '../../stores/app'

const props = withDefaults(
  defineProps<{
    stacked?: boolean
    testId?: string
  }>(),
  {
    stacked: false,
    testId: 'locale-switcher',
  },
)

const appStore = useAppStore()
const { t } = useI18n()

const activeLocale = computed({
  get: () => appStore.locale,
  set: (locale) => {
    appStore.setLocale(locale)
  },
})
</script>

<template>
  <label class="locale-switcher" :class="{ 'locale-switcher--stacked': props.stacked }" :data-testid="props.testId">
    <span class="locale-switcher__label">{{ t('common.language') }}</span>

    <el-select v-model="activeLocale" class="locale-switcher__select" size="small">
      <el-option v-for="option in APP_LOCALE_OPTIONS" :key="option.value" :label="t(option.labelKey)" :value="option.value" />
    </el-select>
  </label>
</template>

<style scoped>
.locale-switcher {
  display: inline-flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.locale-switcher--stacked {
  flex-direction: column;
  align-items: flex-start;
}

.locale-switcher__label {
  font-size: var(--mi-font-size-100);
  color: var(--mi-color-muted);
}

.locale-switcher__select {
  min-width: 9rem;
}

@media (max-width: 52rem) {
  .locale-switcher {
    width: 100%;
  }

  .locale-switcher__select {
    width: 100%;
  }
}
</style>
