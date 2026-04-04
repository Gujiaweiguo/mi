<script setup lang="ts">
defineProps<{
  eyebrow?: string
  title: string
  summary: string
}>()
</script>

<template>
  <section class="page-section">
    <div class="page-section__copy">
      <span v-if="eyebrow" class="page-section__eyebrow">{{ eyebrow }}</span>
      <h1 class="page-section__title">{{ title }}</h1>
      <p class="page-section__summary">{{ summary }}</p>
    </div>

    <div v-if="$slots.actions" class="page-section__actions">
      <slot name="actions" />
    </div>
  </section>
</template>

<style scoped>
.page-section {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: var(--mi-space-6);
  padding: var(--mi-space-6);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-lg);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-md);
}

.page-section__copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
  max-width: var(--mi-copy-width);
}

.page-section__eyebrow {
  font-size: var(--mi-font-size-100);
  letter-spacing: var(--mi-letter-spacing-wide);
  text-transform: uppercase;
  color: var(--mi-color-muted);
}

.page-section__title {
  margin: 0;
  font-size: var(--mi-font-size-500);
  line-height: var(--mi-line-height-tight);
  color: var(--mi-color-text);
}

.page-section__summary {
  margin: 0;
  font-size: var(--mi-font-size-200);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
}

.page-section__actions {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: var(--mi-space-3);
}

@media (max-width: 52rem) {
  .page-section {
    flex-direction: column;
  }

  .page-section__actions {
    align-items: flex-start;
  }
}
</style>
