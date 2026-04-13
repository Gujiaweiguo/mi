import { expect, test } from '@playwright/test'

test('authenticates against the live stack and renders the health view', async ({ page }) => {
  await page.goto('/login')

  await page.getByTestId('login-username-input').fill('admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await expect(page.getByTestId('health-view')).toBeVisible()
  await expect(page.getByTestId('nav--health')).toBeVisible()
  await expect(page.getByTestId('nav--lease-contracts')).toBeVisible()
  await expect(page.getByTestId('nav--workflow-admin')).toBeVisible()
  await expect(page.getByTestId('global-error-alert')).toHaveCount(0)
  await expect(page.getByTestId('health-error-alert')).toHaveCount(0)
  await expect(page.getByTestId('health-response-payload')).toContainText('"status": "ok"')
  await expect(page.getByTestId('health-response-payload')).toContainText('"environment"')
})
