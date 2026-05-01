import { expect, test } from '@playwright/test'
import type { Page } from '@playwright/test'

type PermissionFixture = {
  function_code: string
  permission_level: 'view' | 'edit' | 'approve'
  can_print: boolean
  can_export: boolean
}

type WorkflowDefinitionTemplateFixture = {
  id: number
  business_group_id: number
  code: string
  name: string
  process_class: string
  status: string
  published_definition_id?: number
  published_version_number?: number
}

type WorkflowDefinitionAssignmentRuleFixture = {
  id: number
  workflow_node_id: number
  strategy_type: string
  config_json: string
}

type WorkflowDefinitionNodeFixture = {
  ID: number
  WorkflowDefinitionID: number
  FunctionID: number
  RoleID: number
  StepOrder: number
  Code: string
  Name: string
  CanSubmitToManager: boolean
  ValidatesAfterConfirm: boolean
  PrintsAfterConfirm: boolean
  ProcessClass: string
  AssignmentRules: WorkflowDefinitionAssignmentRuleFixture[]
}

type WorkflowDefinitionTransitionFixture = {
  ID: number
  WorkflowDefinitionID: number
  FromNodeID: number | null
  ToNodeID: number
  Action: string
}

type WorkflowDefinitionDetailFixture = {
  ID: number
  BusinessGroupID: number
  WorkflowTemplateID: number
  Code: string
  Name: string
  VoucherType: string
  IsInitial: boolean
  Status: string
  ProcessClass: string
  VersionNumber: number
  LifecycleStatus: string
  PublishedAt: string | null
  TransitionsEnabled: boolean
  Nodes: WorkflowDefinitionNodeFixture[]
  Transitions: WorkflowDefinitionTransitionFixture[]
}

type SaveDraftRequest = {
  name: string
  voucher_type: string
  is_initial: boolean
  nodes: Array<{
    function_id: number
    role_id: number
    step_order: number
    code: string
    name: string
    can_submit_to_manager: boolean
    validates_after_confirm: boolean
    prints_after_confirm: boolean
    process_class: string
    assignment_rules: Array<{
      strategy_type: string
      config_json: string
    }>
  }>
  transitions: Array<{
    from_node_code: string | null
    to_node_code: string
    action: string
  }>
}

type RollbackRequest = {
  definition_id: number
}

const createVersions = (): WorkflowDefinitionDetailFixture[] => [
  {
    ID: 11,
    BusinessGroupID: 101,
    WorkflowTemplateID: 7,
    Code: 'lease-approval',
    Name: 'Lease Approval V1',
    VoucherType: 'LEASE',
    IsInitial: true,
    Status: 'active',
    ProcessClass: 'lease_contract',
    VersionNumber: 1,
    LifecycleStatus: 'published',
    PublishedAt: '2026-04-01T08:00:00Z',
    TransitionsEnabled: true,
    Nodes: [
      {
        ID: 101,
        WorkflowDefinitionID: 11,
        FunctionID: 101,
        RoleID: 201,
        StepOrder: 1,
        Code: 'SUBMIT',
        Name: 'Submit',
        CanSubmitToManager: false,
        ValidatesAfterConfirm: false,
        PrintsAfterConfirm: false,
        ProcessClass: 'lease_contract',
        AssignmentRules: [{ id: 1, workflow_node_id: 101, strategy_type: 'fixed_role', config_json: '{"role_id":201}' }],
      },
    ],
    Transitions: [{ ID: 201, WorkflowDefinitionID: 11, FromNodeID: null, ToNodeID: 101, Action: 'submit' }],
  },
  {
    ID: 12,
    BusinessGroupID: 101,
    WorkflowTemplateID: 7,
    Code: 'lease-approval',
    Name: 'Lease Approval V2 Draft',
    VoucherType: 'LEASE',
    IsInitial: false,
    Status: 'active',
    ProcessClass: 'lease_contract',
    VersionNumber: 2,
    LifecycleStatus: 'draft',
    PublishedAt: null,
    TransitionsEnabled: true,
    Nodes: [
      {
        ID: 102,
        WorkflowDefinitionID: 12,
        FunctionID: 101,
        RoleID: 202,
        StepOrder: 1,
        Code: 'SUBMIT',
        Name: 'Submit',
        CanSubmitToManager: false,
        ValidatesAfterConfirm: false,
        PrintsAfterConfirm: false,
        ProcessClass: 'lease_contract',
        AssignmentRules: [{ id: 2, workflow_node_id: 102, strategy_type: 'fixed_role', config_json: '{"role_id":202}' }],
      },
    ],
    Transitions: [{ ID: 202, WorkflowDefinitionID: 12, FromNodeID: null, ToNodeID: 102, Action: 'submit' }],
  },
]

const cloneDefinition = (definition: WorkflowDefinitionDetailFixture): WorkflowDefinitionDetailFixture => JSON.parse(JSON.stringify(definition))

const attachWorkflowDefinitionAdminMocks = async (page: Page) => {
  const permissions: PermissionFixture[] = [
    {
      function_code: 'workflow.definition',
      permission_level: 'approve',
      can_print: false,
      can_export: false,
    },
  ]

  let templates: WorkflowDefinitionTemplateFixture[] = [
    {
      id: 7,
      business_group_id: 101,
      code: 'lease-approval',
      name: 'Lease Approval',
      process_class: 'lease_contract',
      status: 'active',
      published_definition_id: 11,
      published_version_number: 1,
    },
    {
      id: 8,
      business_group_id: 101,
      code: 'invoice-approval',
      name: 'Invoice Approval',
      process_class: 'invoice',
      status: 'active',
    },
  ]

  let versions = createVersions()
  const saveDraftRequests: SaveDraftRequest[] = []
  const publishRequests: number[] = []
  const rollbackRequests: RollbackRequest[] = []
  let validateCallCount = 0

  await page.route('**/api/auth/me', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          username: 'workflow-definition-admin',
          display_name: 'Workflow Definition Admin',
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

  await page.route('**/api/dashboard/summary', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        activeLeases: 0,
        pendingLeaseApprovals: 0,
        pendingInvoiceApprovals: 0,
        openReceivables: 0,
        overdueReceivables: 0,
        pendingWorkflows: 0,
      }),
    })
  })

  await page.route('**/api/health', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ status: 'ok', service: 'frontend-platform-test' }),
    })
  })

  await page.route('**/api/workflow/admin/templates**', async (route) => {
    const request = route.request()
    const url = new URL(request.url())
    const pathname = url.pathname

    if (request.method() === 'GET' && pathname.endsWith('/api/workflow/admin/templates')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ templates }),
      })
      return
    }

    if (request.method() === 'GET' && pathname.endsWith('/api/workflow/admin/templates/7/versions')) {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ definitions: versions.map(cloneDefinition) }),
      })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/api/workflow/admin/templates/7/rollback')) {
      const payload = request.postDataJSON() as RollbackRequest
      rollbackRequests.push(payload)

      versions = versions.map((definition) => {
        if (definition.ID === payload.definition_id) {
          return {
            ...definition,
            LifecycleStatus: 'published',
            PublishedAt: '2026-04-05T08:00:00Z',
          }
        }

        if (definition.WorkflowTemplateID === 7 && definition.ID !== payload.definition_id && definition.LifecycleStatus === 'published') {
          return {
            ...definition,
            LifecycleStatus: 'superseded',
          }
        }

        return definition
      })

      const rolledBack = versions.find((definition) => definition.ID === payload.definition_id)
      templates = templates.map((template) =>
        template.id === 7
          ? {
              ...template,
              published_definition_id: rolledBack?.ID,
              published_version_number: rolledBack?.VersionNumber,
            }
          : template,
      )

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ definition: rolledBack }),
      })
      return
    }

    await route.fallback()
  })

  await page.route('**/api/workflow/admin/definitions/**', async (route) => {
    const request = route.request()
    const url = new URL(request.url())
    const pathname = url.pathname
    const definitionId = Number(pathname.split('/').at(-1))
    const pathSegments = pathname.split('/')
    const targetDefinitionId = Number(pathSegments.at(pathSegments.length - 1) === 'publish' || pathSegments.at(pathSegments.length - 1) === 'validate'
      ? pathSegments.at(pathSegments.length - 2)
      : pathSegments.at(pathSegments.length - 1))

    if (request.method() === 'GET') {
      const definition = versions.find((item) => item.ID === definitionId)
      await route.fulfill({
        status: definition ? 200 : 404,
        contentType: 'application/json',
        body: JSON.stringify(definition ? { definition: cloneDefinition(definition) } : { message: 'Not found' }),
      })
      return
    }

    if (request.method() === 'PUT') {
      const payload = request.postDataJSON() as SaveDraftRequest
      saveDraftRequests.push(payload)

      versions = versions.map((definition) =>
        definition.ID === targetDefinitionId
          ? {
              ...definition,
              Name: payload.name,
              VoucherType: payload.voucher_type,
              IsInitial: payload.is_initial,
              Nodes: payload.nodes.map((node, index) => ({
                ID: 1000 + index,
                WorkflowDefinitionID: targetDefinitionId,
                FunctionID: node.function_id,
                RoleID: node.role_id,
                StepOrder: node.step_order,
                Code: node.code,
                Name: node.name,
                CanSubmitToManager: node.can_submit_to_manager,
                ValidatesAfterConfirm: node.validates_after_confirm,
                PrintsAfterConfirm: node.prints_after_confirm,
                ProcessClass: node.process_class,
                AssignmentRules: node.assignment_rules.map((rule, ruleIndex) => ({
                  id: 2000 + ruleIndex,
                  workflow_node_id: 1000 + index,
                  strategy_type: rule.strategy_type,
                  config_json: rule.config_json,
                })),
              })),
              Transitions: payload.transitions.map((transition, index) => ({
                ID: 3000 + index,
                WorkflowDefinitionID: targetDefinitionId,
                FromNodeID: null,
                ToNodeID: 1000,
                Action: transition.action,
              })),
            }
          : definition,
      )

      const updatedDefinition = versions.find((item) => item.ID === targetDefinitionId)
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ definition: updatedDefinition }),
      })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/validate')) {
      validateCallCount += 1
      const validation = validateCallCount === 1
        ? {
            valid: false,
            issues: [{ code: 'missing_terminal', field: 'transitions', message: 'Terminal transition is required' }],
          }
        : { valid: true, issues: [] }

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ validation }),
      })
      return
    }

    if (request.method() === 'POST' && pathname.endsWith('/publish')) {
      publishRequests.push(targetDefinitionId)

      versions = versions.map((definition) => {
        if (definition.ID === targetDefinitionId) {
          return {
            ...definition,
            LifecycleStatus: 'published',
            PublishedAt: '2026-04-04T08:00:00Z',
          }
        }

        if (definition.WorkflowTemplateID === 7 && definition.LifecycleStatus === 'published') {
          return {
            ...definition,
            LifecycleStatus: 'superseded',
          }
        }

        return definition
      })

      const publishedDefinition = versions.find((definition) => definition.ID === targetDefinitionId)
      templates = templates.map((template) =>
        template.id === 7
          ? {
              ...template,
              published_definition_id: publishedDefinition?.ID,
              published_version_number: publishedDefinition?.VersionNumber,
            }
          : template,
      )

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ definition: publishedDefinition }),
      })
      return
    }

    await route.fallback()
  })

  return {
    getSaveDraftRequests: () => saveDraftRequests,
    getPublishRequests: () => publishRequests,
    getRollbackRequests: () => rollbackRequests,
  }
}

const loginAndOpenWorkflowDefinitionAdmin = async (page: Page) => {
  await page.goto('/login')
  await page.getByTestId('login-username-input').fill('workflow-definition-admin')
  await page.getByTestId('login-password-input').fill('password')
  await page.getByTestId('login-submit-button').click()

  await expect(page).toHaveURL(/\/dashboard/)
  await page.goto('/workflow/admin')

  await expect(page).toHaveURL(/\/workflow\/admin/)
  await expect(page.getByTestId('workflow-admin-view')).toBeVisible()
  await expect(page.getByTestId('workflow-admin-tab-label-definitions')).toBeVisible()
  await expect(page.getByTestId('workflow-definition-admin-panel')).toBeVisible()
}

const openDraftVersion = async (page: Page) => {
  await page.getByTestId('workflow-definition-open-template-7').click()
  await expect(page.getByTestId('workflow-definition-versions-table')).toContainText('v1')
  await expect(page.getByTestId('workflow-definition-versions-table')).toContainText('v2')
  await page.getByTestId('workflow-definition-load-version-12').click()
  await expect(page.getByTestId('workflow-definition-name-input')).toHaveValue('Lease Approval V2 Draft')
}

const definitionAdminPanel = (page: Page) => page.getByTestId('workflow-definition-admin-panel')

test('edits and saves a workflow definition draft', async ({ page }) => {
  const mocks = await attachWorkflowDefinitionAdminMocks(page)

  await loginAndOpenWorkflowDefinitionAdmin(page)
  await expect(page.getByTestId('workflow-definition-templates-table')).toContainText('lease-approval')

  await openDraftVersion(page)

  await page.getByTestId('workflow-definition-name-input').fill('Lease Approval V2 Updated')
  await page.getByTestId('workflow-definition-add-node-button').click()
  await expect(page.getByTestId('workflow-definition-node-1')).toBeVisible()
  await page.getByTestId('workflow-definition-add-transition-button').click()
  await page.getByTestId('workflow-definition-save-button').click()

  await expect(definitionAdminPanel(page).getByText('草稿已保存', { exact: true })).toBeVisible()
  await expect(page.getByText('工作流定义草稿已保存')).toBeVisible()
  await expect.poll(() => mocks.getSaveDraftRequests().length).toBe(1)

  const [saveDraftRequest] = mocks.getSaveDraftRequests()
  expect(saveDraftRequest.name).toBe('Lease Approval V2 Updated')
  expect(saveDraftRequest.nodes).toHaveLength(2)
  expect(saveDraftRequest.transitions).toHaveLength(2)
})

test('shows validation diagnostics when workflow definition validation fails', async ({ page }) => {
  await attachWorkflowDefinitionAdminMocks(page)

  await loginAndOpenWorkflowDefinitionAdmin(page)
  await openDraftVersion(page)

  await page.getByTestId('workflow-definition-validate-button').click()

  await expect(definitionAdminPanel(page).getByText('校验未通过', { exact: true })).toBeVisible()
  await expect(page.getByTestId('workflow-definition-validation-issues')).toBeVisible()
  await expect(page.getByTestId('workflow-definition-validation-issue-0')).toContainText('Terminal transition is required')
})

test('publishes a workflow definition draft successfully', async ({ page }) => {
  const mocks = await attachWorkflowDefinitionAdminMocks(page)

  await loginAndOpenWorkflowDefinitionAdmin(page)
  await openDraftVersion(page)

  await page.getByTestId('workflow-definition-publish-button').click()

  await expect(definitionAdminPanel(page).getByText('定义已发布', { exact: true })).toBeVisible()
  await expect(page.getByText('工作流定义已发布')).toBeVisible()
  await expect(page.getByTestId('workflow-definition-templates-table')).toContainText('已发布 v2')
  await expect(page.getByTestId('workflow-definition-versions-table')).toContainText('已发布')
  await expect.poll(() => mocks.getPublishRequests()).toEqual([12])
})

test('shows rollback action and rolls back to a prior workflow definition version', async ({ page }) => {
  const mocks = await attachWorkflowDefinitionAdminMocks(page)

  await loginAndOpenWorkflowDefinitionAdmin(page)
  await openDraftVersion(page)

  await expect(page.getByTestId('workflow-definition-rollback-version-11')).toBeVisible()
  await page.getByTestId('workflow-definition-rollback-version-11').click()

  await expect(definitionAdminPanel(page).getByText('回滚完成', { exact: true })).toBeVisible()
  await expect(page.getByText('工作流定义回滚完成')).toBeVisible()
  await expect(page.getByTestId('workflow-definition-templates-table')).toContainText('已发布 v1')
  await expect.poll(() => mocks.getRollbackRequests()).toEqual([{ definition_id: 11 }])
})
