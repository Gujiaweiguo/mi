import { defineConfig } from '@playwright/test'

const baseURL = process.env.PLAYWRIGHT_BASE_URL || 'http://127.0.0.1:80'

export default defineConfig({
  testDir: './e2e',
  testMatch: '**/live-stack-*.spec.ts',
  timeout: 60_000,
  retries: 0,
  use: {
    baseURL,
    headless: true,
    launchOptions: process.env.PLAYWRIGHT_EXECUTABLE_PATH
      ? {
          executablePath: process.env.PLAYWRIGHT_EXECUTABLE_PATH,
        }
      : undefined,
  },
})
