<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'

import LocaleSwitcher from '../components/platform/LocaleSwitcher.vue'
import { DEFAULT_AUTHENTICATED_PATH } from '../router/auth-guard'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()
const { t } = useI18n()

const loginFormRef = ref<FormInstance>()
const errorMessage = ref('')
const form = reactive({
  username: '',
  password: '',
})

const rules = computed<FormRules<typeof form>>(() => ({
  username: [
    {
      required: true,
      message: t('login.validation.usernameRequired'),
      trigger: 'blur',
    },
  ],
  password: [
    {
      required: true,
      message: t('login.validation.passwordRequired'),
      trigger: 'blur',
    },
  ],
}))

const redirectTarget = computed(() => {
  const candidate = route.query.redirect

  return typeof candidate === 'string' && candidate.startsWith('/')
    ? candidate
    : DEFAULT_AUTHENTICATED_PATH
})

const submitLabel = computed(() => (authStore.isBusy ? t('login.actions.submitting') : t('login.actions.submit')))

const handleSubmit = async () => {
  errorMessage.value = ''
  const isValid = await loginFormRef.value?.validate().catch(() => false)

  if (isValid !== true) {
    return
  }

  try {
    await authStore.login({ ...form })
    await router.replace(redirectTarget.value)
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('login.errors.unableToSignIn')
  }
}
</script>

<template>
  <div class="login-view" data-testid="login-view">
    <div class="login-view__hero">
      <div class="login-view__hero-copy">
        <span class="login-view__eyebrow">{{ t('login.eyebrow') }}</span>
        <h1 class="login-view__title">{{ t('login.title') }}</h1>
        <p class="login-view__summary">
          {{ t('login.summary') }}
        </p>
      </div>

      <LocaleSwitcher stacked test-id="login-locale-switcher" />
    </div>

    <el-card class="login-view__card" shadow="never">
      <template #header>
        <div class="login-view__card-header">
          <span>{{ t('login.cardTitle') }}</span>
          <el-tag type="info" effect="plain">{{ t('login.cardTag') }}</el-tag>
        </div>
      </template>

      <el-alert
        v-if="errorMessage"
        :closable="false"
        class="login-view__alert"
        type="error"
        show-icon
        :description="errorMessage"
        :title="t('login.errorTitle')"
        data-testid="login-error-alert"
      />

      <el-form
        ref="loginFormRef"
        :model="form"
        :rules="rules"
        label-position="top"
        class="login-view__form"
      >
        <el-form-item :label="t('login.fields.username')" prop="username">
          <el-input
            v-model="form.username"
            autocomplete="username"
            :placeholder="t('login.placeholders.username')"
            data-testid="login-username-input"
          />
        </el-form-item>

        <el-form-item :label="t('login.fields.password')" prop="password">
          <el-input
            v-model="form.password"
            type="password"
            show-password
            autocomplete="current-password"
            :placeholder="t('login.placeholders.password')"
            data-testid="login-password-input"
            @keyup.enter="handleSubmit"
          />
        </el-form-item>

        <div class="login-view__actions">
          <el-button
            type="primary"
            :loading="authStore.isBusy"
            data-testid="login-submit-button"
            @click="handleSubmit"
          >
            {{ submitLabel }}
          </el-button>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<style scoped>
.login-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-6);
  max-width: var(--mi-copy-width);
  margin: 0 auto;
}

.login-view__hero {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: var(--mi-space-4);
}

.login-view__hero-copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.login-view__eyebrow {
  font-size: var(--mi-font-size-100);
  letter-spacing: var(--mi-letter-spacing-wide);
  text-transform: uppercase;
  color: var(--mi-color-muted);
}

.login-view__title {
  margin: 0;
  font-size: var(--mi-font-size-500);
  line-height: var(--mi-line-height-tight);
  color: var(--mi-color-text);
}

.login-view__summary {
  margin: 0;
  font-size: var(--mi-font-size-200);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
}

.login-view__card {
  border-color: var(--mi-color-border);
  border-radius: var(--mi-radius-md);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.login-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.login-view__alert {
  margin-bottom: var(--mi-space-4);
}

.login-view__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-2);
}

.login-view__actions {
  display: flex;
  justify-content: flex-end;
}

@media (max-width: 52rem) {
  .login-view__hero {
    flex-direction: column;
  }

  .login-view__card-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .login-view__actions {
    justify-content: flex-start;
  }

  .login-view__actions :deep(.el-button) {
    width: 100%;
  }
}
</style>
