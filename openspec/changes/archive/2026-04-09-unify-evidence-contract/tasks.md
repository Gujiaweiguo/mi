## 1. Canonical evidence contract definition

- [x] 1.1 Audit current evidence producers and validators for schema drift across `unit` / `integration` / `e2e`.
- [x] 1.2 Align evidence-writing entrypoints to emit canonical fields and consistent value types.
- [x] 1.3 Ensure canonical invariants are enforced at write time (stats consistency, timestamp ordering, required nested fields).

## 2. Gate validation alignment

- [x] 2.1 Tighten gate validation to reject non-canonical schemas with explicit diagnostics for each required evidence type.
- [x] 2.2 Preserve CI/archive gate hierarchy while enforcing canonical schema for commit-scoped evidence.
- [x] 2.3 Ensure archive-specific `e2e` artifact requirements are consistently validated.

## 3. Verification and regression coverage

- [x] 3.1 Add or update automated self-tests for schema-drift rejection and canonical-schema pass cases.
- [x] 3.2 Run unit/integration/e2e verification for implementation commit and generate machine-readable evidence under `artifacts/verification/<commit-sha>/`.
- [x] 3.3 Confirm CI and archive gates both pass for the same implementation commit SHA under canonical schema.
