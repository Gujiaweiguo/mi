# Workflow Admin / Approval Acceptance Summary (2026-04-11)

## Scope

This acceptance summary covers the workflow admin/approval hardening slice for `legacy-system-migration`, with focus on sequential retry idempotency for workflow start, concurrent start safety via partial unique index, full approval lifecycle (submit → approve → reject → resubmit), duplicate action deduplication for all transition operations, reminder automation correctness, and current-head verification evidence.

## Included commits

1. `49e141e` — `Deduplicate workflow start retries`
2. `9d3bdfe` — `Add concurrent safety for workflow start via partial unique index`

## Spec baseline

- `openspec/specs/workflow-approvals/spec.md`

## Acceptance evidence (current HEAD)

Current HEAD:

- `9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc`

Verification evidence:

- `artifacts/verification/9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc/unit.json` — PASS (50/50)
- `artifacts/verification/9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc/integration.json` — PASS (85/85)
- `artifacts/verification/9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc/e2e.json` — PASS (41/41)

## Executed checks

### Workflow integration tests

1. `TestWorkflowServiceApproveAndDeduplicate`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowServiceApproveAndDeduplicate -count=1`
   - Result: PASS
   - submit → approve step 1 → duplicate approve (idempotent) → approve step 2 → completed; verifies audit entries and outbox messages at each stage

2. `TestWorkflowServiceStartDeduplicatesByIdempotencyKey`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowServiceStartDeduplicatesByIdempotencyKey -count=1`
   - Result: PASS
   - start workflow → duplicate start with same idempotency key → only one instance/audit/outbox

3. `TestWorkflowServiceRejectAndResubmit`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowServiceRejectAndResubmit -count=1`
   - Result: PASS
   - start → reject → resubmit (cycle 2) → verifies 3 audit entries and 3 outbox messages

4. `TestWorkflowServiceListInstancesFilters`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowServiceListInstancesFilters -count=1`
   - Result: PASS
   - creates lease + invoice instances, filters by status and document_type, verifies newest-first ordering

5. `TestWorkflowReminderRunEmitsAndStaysReadOnly`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowReminderRunEmitsAndStaysReadOnly -count=1`
   - Result: PASS
   - start → run reminders after min pending age → verify emitted → verify workflow state preserved → replay → verify already_emitted skip → verify reminder history count

6. `TestWorkflowReminderRunSkipsWhenNotDue`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowReminderRunSkipsWhenNotDue -count=1`
   - Result: PASS
   - start → run reminders before min pending age → verify not_due skip → verify workflow state preserved

7. `TestWorkflowServiceStartPreventsConcurrentDuplicate`
   - Command:
     - `go test -tags=integration ./internal/workflow -run TestWorkflowServiceStartPreventsConcurrentDuplicate -count=1`
   - Result: PASS
   - two goroutines start workflow for same document with different idempotency keys simultaneously → exactly one instance survives → instance is pending

### Router integration test coverage

7. Workflow definitions endpoint returns 200
   - Command:
     - `go test -tags=integration ./internal/http -run TestIntegrationAuthAndOrgRoutes -count=1`
   - Result: PASS
   - Workflow definitions endpoint returns 200

8. Workflow start/approve/audit endpoints exercise full lifecycle through HTTP
   - Command:
     - `go test -tags=integration ./internal/http -run TestIntegrationAuthAndOrgRoutes -count=1`
   - Result: PASS
   - Workflow start/approve/audit endpoints exercise full lifecycle through HTTP

### E2E tests

9. `task15-workflow.spec.ts`
   - Command:
     - `npx playwright test e2e/task15-workflow.spec.ts`
   - Result: PASS
   - lease → billing → invoice → tax → print → excel end-to-end flow with workflow submission

10. `task19-workflow-admin.spec.ts`
    - Command:
      - `npx playwright test e2e/task19-workflow-admin.spec.ts`
    - Result: PASS
    - workflow admin view: definitions table, instances table, status filter, approve action with idempotency key, feedback, instance list refresh

### Full verification gates (current HEAD)

11. Unit verification
    - Command:
      - `./scripts/verification/run-unit.sh 9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc`
    - Result: PASS

12. Integration verification
    - Command:
      - `./scripts/verification/run-integration.sh 9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc`
    - Result: PASS (85/85)

13. E2E verification
    - Command:
      - `./scripts/verification/run-e2e.sh 9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc`
    - Result: PASS (41/41)

14. CI gate
    - Command:
      - `./scripts/ci-ready.sh`
    - Result: PASS (`CI Ready: YES`)

15. Archive gate
    - Command:
      - `./scripts/archive-ready.sh`
    - Result: PASS (`Archive Ready: YES`)

## Acceptance outcomes

- Workflow start is idempotent for sequential retries: duplicate start requests with the same idempotency key return the existing instance without creating duplicates.
- Workflow start is concurrently safe: two simultaneous start requests for the same `(definition, document_type, document_id)` produce exactly one instance, enforced by a partial unique index on active (non-completed) rows via MySQL generated column.
- All transition operations (approve, reject, resubmit) enforce idempotency via idempotency key deduplication in the audit table within a transaction.
- Full multi-step approval lifecycle works: submit → multi-step approve → completed, with correct audit trail and outbox messages at every transition.
- Reject → resubmit cycle works: creates new cycle steps, resets to pending, and records all transitions in audit history.
- Reminder automation emits for eligible pending instances and skips (already_emitted, not_due, not_pending) correctly, without mutating workflow decision state.
- Reminder replay within the same window is idempotent.
- Instance listing supports filtering by status and document type with newest-first ordering.
- Frontend workflow admin view allows viewing definitions, filtering instances by status, and performing approve/reject actions with idempotency keys.
- Current HEAD is both `CI Ready` and `Archive Ready`.

## Traceability notes

- Workflow domain logic: `backend/internal/workflow/service.go`
- Workflow data access: `backend/internal/workflow/repository.go`
- Workflow domain models: `backend/internal/workflow/model.go`
- Workflow integration tests: `backend/internal/workflow/service_integration_test.go`
- Workflow HTTP routes: `backend/internal/http/router.go`
- Workflow router integration tests: `backend/internal/http/router_integration_test.go`
- Workflow frontend admin view: `frontend/src/views/WorkflowAdminView.vue`
- Workflow frontend API client: `frontend/src/api/workflow.ts`
- Workflow admin E2E: `frontend/e2e/task19-workflow-admin.spec.ts`
- Lease-billing-workflow E2E: `frontend/e2e/task15-workflow.spec.ts`
- Workflow schema migrations: `000003_workflow_code_schema`, `000004_workflow_runtime_schema`, `000017_workflow_reminder_audit_schema`, `000019_workflow_active_instance_unique`

## Conclusion

The workflow admin/approval acceptance slice is accepted for the covered first-release scope. Sequential retry idempotency for start, concurrent start safety via partial unique index, full lifecycle transitions with audit and outbox, reminder automation correctness, and current-head verification gates all pass on HEAD `9d3bdfe3e8de98d544abe9872a638f8f44c5b1fc`.
