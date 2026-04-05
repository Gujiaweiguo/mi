<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute } from 'vue-router'
import { useRouter } from 'vue-router'

import LocaleSwitcher from './components/platform/LocaleSwitcher.vue'
import { resolveElementPlusLocale } from './i18n'
import { resolveNavigationItems } from './auth/permissions'
import { useAppStore } from './stores/app'
import { useAuthStore } from './stores/auth'
import { LOGIN_PATH } from './router/auth-guard'

const route = useRoute()
const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()
const authStore = useAuthStore()
const globalErrorMessage = ref('')
const globalErrorKey = ref('')

const isAuthenticated = computed(() => authStore.isAuthenticated)
const elementPlusLocale = computed(() => resolveElementPlusLocale(appStore.locale))
const sessionLabel = computed(() => authStore.userDisplayName || t('app.session.signedIn'))
const navigationItems = computed(() => resolveNavigationItems(authStore.sessionUser, (key) => t(key)))
const showShellNavigation = computed(() => isAuthenticated.value && route.path !== LOGIN_PATH)
const globalError = computed(() => globalErrorMessage.value || (globalErrorKey.value ? t(globalErrorKey.value) : ''))

const dismissGlobalError = () => {
  globalErrorMessage.value = ''
  globalErrorKey.value = ''
}

const handleGlobalHttpError = (event: Event) => {
  const detail = (event as CustomEvent<{ message?: string }>).detail
  globalErrorMessage.value = detail?.message ?? ''
  globalErrorKey.value = detail?.message ? '' : 'app.errors.platformRequestFailed'
}

const handleAuthExpired = async () => {
  authStore.logout()
  globalErrorMessage.value = ''
  globalErrorKey.value = 'app.errors.authExpired'

  if (route.path !== LOGIN_PATH) {
    await router.push({ path: LOGIN_PATH, query: { redirect: route.fullPath } })
  }
}

onMounted(() => {
  window.addEventListener('mi:http-error', handleGlobalHttpError as EventListener)
  window.addEventListener('mi:http-auth-expired', handleAuthExpired as EventListener)
})

onBeforeUnmount(() => {
  window.removeEventListener('mi:http-error', handleGlobalHttpError as EventListener)
  window.removeEventListener('mi:http-auth-expired', handleAuthExpired as EventListener)
})

const handleLogout = async () => {
  authStore.logout()
  await router.push(LOGIN_PATH)
}
</script>

<template>
  <el-config-provider :locale="elementPlusLocale">
    <el-container class="app-shell">
      <el-header class="app-shell__header">
        <div class="app-shell__brand">
          <span class="app-shell__eyebrow">{{ t('app.eyebrow') }}</span>
          <strong class="app-shell__title">{{ appStore.appName }}</strong>
        </div>

        <div class="app-shell__controls">
          <div v-if="isAuthenticated" class="app-shell__session">
            <LocaleSwitcher test-id="app-locale-switcher" />
            <el-tag effect="plain" type="info">{{ sessionLabel }}</el-tag>
            <el-button text data-testid="app-logout-button" @click="handleLogout">{{ t('app.session.logout') }}</el-button>
          </div>

          <el-tag v-else effect="plain" type="warning">{{ t('app.session.authRequired') }}</el-tag>
        </div>
      </el-header>

      <el-container class="app-shell__body">
        <el-aside v-if="showShellNavigation" class="app-shell__aside" width="18rem">
          <el-menu :default-active="route.path" class="app-shell__nav" router>
            <el-menu-item v-for="item in navigationItems" :key="item.path" :index="item.path" :data-testid="`nav-${item.path.replace(/\//g, '-')}`">
              {{ item.label }}
            </el-menu-item>
            <el-menu-item index="/forbidden">{{ t('app.navigation.accessNotices') }}</el-menu-item>
          </el-menu>
        </el-aside>

        <el-main class="app-shell__main">
          <div class="app-shell__content">
            <el-alert
              v-if="globalError"
              class="app-shell__alert"
              type="error"
              :closable="true"
              show-icon
              :title="globalError"
              data-testid="global-error-alert"
              @close="dismissGlobalError"
            />

            <router-view />
          </div>
        </el-main>
      </el-container>
    </el-container>
  </el-config-provider>
</template>

<style scoped>
.app-shell {
  min-height: 100vh;
}

.app-shell__body {
  min-height: calc(100vh - var(--mi-header-height));
}

.app-shell__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-6);
  min-height: var(--mi-header-height);
  padding: var(--mi-space-4) var(--mi-space-6);
  background: var(--mi-color-surface);
  border-bottom: var(--mi-border-width-thin) solid var(--mi-color-border);
  box-shadow: var(--mi-shadow-sm);
}

.app-shell__brand {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
}

.app-shell__eyebrow {
  font-size: var(--mi-font-size-100);
  letter-spacing: var(--mi-letter-spacing-wide);
  text-transform: uppercase;
  color: var(--mi-color-muted);
}

.app-shell__title {
  font-size: var(--mi-font-size-400);
  line-height: var(--mi-line-height-tight);
  color: var(--mi-color-text);
}

.app-shell__controls {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: var(--mi-space-4);
  flex-wrap: wrap;
}

.app-shell__session {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.app-shell__main {
  padding: var(--mi-space-6);
}

.app-shell__aside {
  padding: var(--mi-space-5) var(--mi-space-4);
  border-right: var(--mi-border-width-thin) solid var(--mi-color-border);
  background: rgba(255, 255, 255, 0.72);
  backdrop-filter: blur(8px);
}

.app-shell__nav {
  border-right: none;
  background: transparent;
}

.app-shell__content {
  max-width: var(--mi-layout-width);
  margin: 0 auto;
}

.app-shell__alert {
  margin-bottom: var(--mi-space-5);
}

@media (max-width: 52rem) {
  .app-shell__header {
    flex-direction: column;
    align-items: flex-start;
    padding: var(--mi-space-5);
  }

  .app-shell__main {
    padding: var(--mi-space-5);
  }

  .app-shell__body {
    flex-direction: column;
  }

  .app-shell__aside {
    width: 100% !important;
    padding: var(--mi-space-4) var(--mi-space-5) 0;
    border-right: none;
  }

  .app-shell__controls {
    width: 100%;
    justify-content: flex-start;
  }

  .app-shell__session {
    flex-wrap: wrap;
  }
}
</style>
