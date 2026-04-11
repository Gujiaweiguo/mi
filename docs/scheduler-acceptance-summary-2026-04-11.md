# Scheduler Acceptance Summary (2026-04-11)

## Scope

This acceptance summary covers the workflow reminder scheduler hardening slice for `legacy-system-migration`, with focus on distributed lock correctness, built-in scheduler configuration coverage, current-head verification evidence, and test-environment cutover readiness.

## Included commits

1. `308f0f5` — `Hold scheduler lock through reminder execution`
2. `e44432d` — `Verify scheduler config coverage across built-in YAML files`

## Spec baseline

- `openspec/changes/archive/2026-04-10-add-workflow-reminder-schedule-runner/`
- `openspec/changes/archive/2026-04-10-add-workflow-reminder-scheduler-distributed-lock/`
- `openspec/changes/archive/2026-04-10-add-workflow-reminder-scheduler-observability/`
- `openspec/changes/archive/2026-04-10-add-workflow-reminder-trigger/`
- `openspec/changes/archive/2026-04-10-implement-workflow-reminder-runner/`
- `openspec/changes/archive/2026-04-10-workflow-auto-reminder-poc/`

## Acceptance evidence (current HEAD)

Current HEAD:

- `e44432d349eb936b48e18e1301dbb8b5c7740125`

Verification evidence:

- `artifacts/verification/e44432d349eb936b48e18e1301dbb8b5c7740125/unit.json`
- `artifacts/verification/e44432d349eb936b48e18e1301dbb8b5c7740125/integration.json`
- `artifacts/verification/e44432d349eb936b48e18e1301dbb8b5c7740125/e2e.json`

Cutover rehearsal artifact:

- `artifacts/rehearsal/e44432d349eb936b48e18e1301dbb8b5c7740125/cutover-rehearsal-test-20260411T081806Z.json`

## Executed checks

### Scheduler hardening verification

1. Scheduler/app unit tests
   - Command:
     - `go test ./internal/app ./internal/config`
   - Result: PASS

2. Distributed lock integration validation
   - Command:
     - `go test -tags=integration ./internal/app -run TestMySQLWorkflowReminderDistributedLockerHoldsLockUntilFunctionReturns -count=1`
   - Result: PASS

3. Reminder handler unit coverage
   - Command:
     - `go test ./internal/http/handlers -run 'TestRunReminders|TestWorkflowReminderHistory'`
   - Result: PASS

4. Reminder workflow integration coverage
   - Command:
     - `go test -tags=integration ./internal/workflow -run 'TestWorkflowReminderRunEmitsAndStaysReadOnly|TestWorkflowReminderRunSkipsWhenNotDue' -count=1`
   - Result: PASS

5. Static verification
   - Command:
     - `go vet ./internal/app ./internal/config`
   - Result: PASS

### Full verification gates (current HEAD)

6. Unit verification evidence
   - Command:
     - `./scripts/verification/run-unit.sh e44432d349eb936b48e18e1301dbb8b5c7740125`
   - Result: PASS

7. Integration verification evidence
   - Command:
     - `./scripts/verification/run-integration.sh e44432d349eb936b48e18e1301dbb8b5c7740125`
   - Result: PASS

8. E2E verification evidence
   - Command:
     - `./scripts/verification/run-e2e.sh e44432d349eb936b48e18e1301dbb8b5c7740125`
   - Result: PASS (`41/41`)

9. CI gate
   - Command:
     - `./scripts/ci-ready.sh`
   - Result: PASS (`CI Ready: YES`)

10. Archive gate
    - Command:
      - `./scripts/archive-ready.sh`
    - Result: PASS (`Archive Ready: YES`)

### Cutover rehearsal

11. Test-environment cutover rehearsal
    - Command:
      - `./scripts/cutover-rehearsal.sh test`
    - Result: PASS (`GO`)

## Acceptance outcomes

- The distributed scheduler lock now covers the actual reminder execution critical section.
- A real MySQL integration test verifies that a second lock attempt does not run while the first callback is still executing, and succeeds after release.
- Built-in `test`, `development`, and `production` config files load valid scheduler settings.
- Current HEAD is both `CI Ready` and `Archive Ready`.
- Test cutover rehearsal completed with overall result `GO`, including bootstrap, smoke, backup, restore, and restore smoke validation.

## Traceability notes

- Scheduler lock lifecycle is implemented in:
  - `backend/internal/app/app.go`
- Real MySQL lock-holding validation is implemented in:
  - `backend/internal/app/app_integration_test.go`
- Built-in scheduler config coverage is implemented in:
  - `backend/internal/config/config_test.go`

## Conclusion

The workflow reminder scheduler hardening slice is accepted for the covered first-release scope. The distributed lock lifetime bug is fixed, scheduler configuration coverage is verified across built-in configs, current-head verification gates pass, and the test cutover rehearsal reports `GO` on HEAD `e44432d349eb936b48e18e1301dbb8b5c7740125`.
