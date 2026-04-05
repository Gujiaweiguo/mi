import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

type WorkflowDefinitionFixture = {
  ID: number
  Code: string
  Name: string
  ProcessClass: string
}

type WorkflowInstanceFixture = {
  id: number
  workflow_definition_id: number
  document_type: string
  document_id: number
  status: string
  current_node_id: number | null
  current_node_code: string | null
  current_step_order: number | null
  current_cycle: number
  version: number
  submitted_by: number
  submitted_at: string
  created_at: string
  completed_at: string | null
}

type WorkflowActionRequest = {
  idempotency_key: string
  comment?: string
}

const attachWorkflowAdminMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'workflow.admin',
      permission_level: 'approve',
      can_print: false,
      can_export: false,
    },
  ]

  const definitions: WorkflowDefinitionFixture[] = [
    {
      ID: 101,
      Code: 'lease-approval',
      Name: 'Lease approval',
      ProcessClass: 'lease.contract',
    },
  ]

  let instances: WorkflowInstanceFixture[] = [
    {
      id: 9001,
      workflow_definition_id: 101,
      document_type: 'lease_contract',
      document_id: 5001,
      status: 'pending',
      current_node_id: 301,
      current_node_code: 'manager_review',
      current_step_order: 1,
      current_cycle: 1,
      version: 1,
      submitted_by: 1,
      submitted_at: '2026-04-03T08:00:00Z',
      created_at: '2026-04-03T08:00:00Z',
      completed_at: null,
    },
  ]

  const approveRequests: WorkflowActionRequest[] = []
  let listRequestCount = 0

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'workflow-admin',
          display_name: 'Workflow Admin',
          department_id: 101,
          roles: ['admin'],
          permissions,
        },
      }),
    })
  })

  await page.route('**/api/auth/login', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ token: 'playwright-token' }),
    })
  })

  await page.route('**/api/health', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ status: 'ok', service: 'frontend-platform-test' }),
    })
  })

  await page.route('**/api/workflow/definitions', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ definitions }),
    })
  })

  await page.route('**/api/workflow/instances**', async (route) => {
    const request = route.request()
    const url = new URL(request.url())
    const pathname = url.pathname

    if (request.method() === 'GET' && pathname.endsWith('/api/workflow/instances')) {
      listRequestCount += 1

      const status = url.searchParams.get('status') ?? ''
      const filteredInstances = status ? instances.filter((instance) => instance.status === status) : instances

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ instances: filteredInstances }),
      })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/approve')) {
      const payload = request.postDataJSON() as WorkflowActionRequest
      approveRequests.push(payload)

      const instanceId = Number(pathname.split('/').at(-2))
      instances = instances.map((instance) =>
        instance.id === instanceId
          ? {
              ...instance,
              status: 'approved',
              current_node_id: null,
              current_node_code: null,
              current_step_order: null,
              version: instance.version + 1,
              completed_at: '2026-04-03T08:05:00Z',
            }
          : instance,
      )

      const approvedInstance = instances.find((instance) => instance.id === instanceId)

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ instance: approvedInstance }),
      })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/reject')) {
      const instanceId = Number(pathname.split('/').at(-2))
      instances = instances.map((instance) =>
        instance.id === instanceId
          ? {
              ...instance,
              status: 'rejected',
              current_node_id: null,
              current_node_code: null,
              current_step_order: null,
              version: instance.version + 1,
              completed_at: '2026-04-03T08:06:00Z',
            }
          : instance,
      )

      const rejectedInstance = instances.find((instance) => instance.id === instanceId)

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ instance: rejectedInstance }),
      })
      return
    }

    await route.fallback()
  })

  return {
    getApproveRequests: () => approveRequests,
    getListRequestCount: () => listRequestCount,
  }
}

test('approves a pending workflow instance from workflow admin', async ({ page }) => {
  const mocks = await attachWorkflowAdminMocks(page)

  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('workflow-admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/health/)
  await page.goto('/workflow/admin')

  await expect(page).toHaveURL(/\/workflow\/admin/)
  await expect(page.getByTestId('workflow-admin-view')).toBeVisible()
  await expect(page.getByTestId('workflow-definitions-table')).toBeVisible()
  await expect(page.getByTestId('workflow-definitions-table')).toContainText('lease-approval')

  await expect(page.getByTestId('workflow-status-filter')).toBeVisible()
  await expect(page.getByTestId('workflow-instances-table')).toBeVisible()
  await expect(page.getByTestId('workflow-instances-table')).toContainText('lease_contract')
  await expect(page.getByTestId('workflow-instances-table')).toContainText('manager_review')

  const initialListRequestCount = mocks.getListRequestCount()
  await page.getByTestId('workflow-approve-button-9001').click()

  await expect(page.getByText('工作流实例已批准')).toBeVisible()
  await expect(page.getByTestId('workflow-instances-table')).not.toContainText('lease_contract')
  await expect.poll(() => mocks.getApproveRequests().length).toBe(1)
  await expect.poll(() => mocks.getListRequestCount()).toBeGreaterThan(initialListRequestCount)

  const [approveRequest] = mocks.getApproveRequests()
  expect(approveRequest.idempotency_key).toBeTruthy()
})
