<script setup lang="ts">
const props = withDefaults(
  defineProps<{
    title?: string
    submitLabel?: string
    resetLabel?: string
    busy?: boolean
    resetDisabled?: boolean
    showActions?: boolean
  }>(),
  {
    showActions: true,
  },
)

const emit = defineEmits<{
  submit: []
  reset: []
}>()

const handleSubmit = () => emit('submit')
const handleReset = () => emit('reset')
</script>

<template>
  <el-card class="filter-form" shadow="never">
    <template #header>
      <div class="filter-form__header">
        <span>{{ props.title ?? 'Filters' }}</span>
      </div>
    </template>

    <el-form label-position="top" @submit.prevent>
      <div class="filter-form__grid">
        <slot />
      </div>

      <div v-if="props.showActions" class="filter-form__actions">
        <el-button :disabled="props.resetDisabled" @click="handleReset">{{ props.resetLabel ?? 'Reset' }}</el-button>
        <el-button type="primary" :loading="props.busy" @click="handleSubmit">{{ props.submitLabel ?? 'Apply filters' }}</el-button>
      </div>
    </el-form>
  </el-card>
</template>

<style scoped>
.filter-form {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.filter-form__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.filter-form__grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.filter-form__actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
  margin-top: var(--mi-space-4);
}

@media (max-width: 52rem) {
  .filter-form__grid {
    grid-template-columns: minmax(0, 1fr);
  }

  .filter-form__actions {
    justify-content: flex-start;
    flex-wrap: wrap;
  }
}
</style>
