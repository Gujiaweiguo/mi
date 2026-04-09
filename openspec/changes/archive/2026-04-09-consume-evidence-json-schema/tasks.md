## 1. Validator/schema integration planning

- [x] 1.1 Audit the current validator to separate structural checks from gate-context checks.
- [x] 1.2 Choose and document the schema-validation integration path that fits the repository runtime.

## 2. Implementation

- [x] 2.1 Update `scripts/verification/validate-gate.sh` (and any helper it uses) to validate evidence structure via `schemas/evidence-v1.json`.
- [x] 2.2 Preserve contextual checks for commit SHA, evidence type, pass-status acceptance, stats arithmetic consistency, and e2e artifact requirements after schema validation succeeds.

## 3. Regression coverage

- [x] 3.1 Extend or adjust verification self-tests so schema-backed validation preserves existing pass/fail behavior.
- [x] 3.2 Run the verification self-test suite and confirm validator diagnostics remain actionable.
