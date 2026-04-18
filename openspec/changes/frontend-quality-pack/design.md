## Design

### 1. Extract getErrorMessage composable

Create `frontend/src/composables/useErrorMessage.ts`:
```typescript
export const getErrorMessage = (error: unknown, fallback: string): string =>
  error instanceof Error ? error.message : fallback
```

Remove the local `const getErrorMessage = ...` definition from all 4 views and add:
```typescript
import { getErrorMessage } from '@/composables/useErrorMessage'
```

### 2. Add v-loading states

For each view that makes API calls but lacks `v-loading`:

Pattern:
```vue
<script setup>
const isLoading = ref(true)
</script>

<template>
  <div v-loading="isLoading">
    <!-- existing content -->
  </div>
</template>
```

- Set `isLoading.value = true` before API calls
- Set `isLoading.value = false` after API calls complete (both success and error paths)
- Wrap the main content div with `v-loading="isLoading"`

### 3. Add form validation rules

For each admin view that has a create/edit dialog with an `el-form`:

Pattern:
```vue
<script setup>
const rules = {
  code: [{ required: true, message: 'Code is required', trigger: 'blur' }],
  name: [{ required: true, message: 'Name is required', trigger: 'blur' }],
}
</script>

<template>
  <el-form :model="form" :rules="rules" ref="formRef">
    <el-form-item label="Code" prop="code">
      <el-input v-model="form.code" />
    </el-form-item>
  </el-form>
</template>
```

Apply this pattern to all form dialogs in admin views. Views that already have `:rules` (LoginView, LeaseCreateView) should not be touched.
