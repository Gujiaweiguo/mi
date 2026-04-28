## 1. Backend workbench aggregate

- [x] 1.1 Define the Workbench aggregate response contract for approvals, receivables, overdue receivables, and active lease sections, including bounded preview rows and downstream route metadata.
- [x] 1.2 Implement backend aggregation logic that collects all Workbench queue sections in one authenticated request without changing the existing dashboard summary contract.
- [x] 1.3 Expose the new Workbench aggregate endpoint through the authenticated HTTP layer and document its response shape alongside the existing dashboard APIs.

## 2. Frontend workbench rendering

- [x] 2.1 Add frontend API/composable support to fetch the Workbench aggregate payload with one request when `/workbench` loads.
- [x] 2.2 Replace the current static Workbench shell props with real queue-section rendering, including counts, preview rows, and downstream navigation targets.
- [x] 2.3 Add stable empty-state and partial-data UI behavior so every queue section remains visible even when no rows are returned.

## 3. Verification

- [x] 3.1 Add backend automated tests that cover populated and empty aggregate responses for the new Workbench endpoint.
- [x] 3.2 Add frontend automated tests that cover Workbench loading, populated queues, and empty queue rendering behavior.
- [x] 3.3 Run the relevant backend and frontend test suites for the change; if this change is prepared for CI/archive evidence, record current-commit machine-readable artifacts under `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` according to the repo gate policy.
