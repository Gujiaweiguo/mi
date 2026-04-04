<script setup lang="ts">
import PageSection from '../components/platform/PageSection.vue'
</script>

<template>
  <div class="forbidden-view" data-testid="forbidden-view">
    <PageSection
      eyebrow="Permission boundary"
      title="Access not granted"
      summary="Your current session does not include the function permission required for this route."
    />

    <el-card class="forbidden-view__card" shadow="never">
      <p class="forbidden-view__copy">
        Navigation and route guards now use backend function codes and permission levels from the authenticated session user.
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
