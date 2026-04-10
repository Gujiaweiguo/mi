## 1. Reminder runner core implementation

- [x] 1.1 Implement reminder candidate evaluation for pending workflow instances with deterministic reminder window/key calculation.
- [x] 1.2 Implement idempotent reminder persistence so replayed runs do not duplicate emitted reminder side effects.

## 2. Audit and diagnostics implementation

- [x] 2.1 Persist auditable reminder outcomes for emitted and skipped evaluations with reason codes.
- [x] 2.2 Expose reminder history query path per workflow instance for operator diagnostics.

## 3. Verification and readiness

- [x] 3.1 Add integration tests covering emission, skip-reason, and replay-idempotency behavior.
- [x] 3.2 Run verification gates (unit/integration/e2e evidence + ci-ready/archive-ready) and confirm reminder implementation does not mutate approval decision state.
