## 1. Result-capture audit

- [x] 1.1 Audit current unit and integration verification commands to identify reliable machine-readable or parseable result formats.
- [x] 1.2 Decide how backend unit, frontend unit, and backend integration counts will map into existing evidence artifacts.

## 2. Implementation

- [x] 2.1 Update `scripts/verification/run-unit.sh` to aggregate real unit-test counts from the backend and frontend unit commands.
- [x] 2.2 Update `scripts/verification/run-integration.sh` to emit real integration-test counts from the backend integration command.
- [x] 2.3 Add any helper logic needed to parse and validate runner output while preserving the current evidence schema.

## 3. Verification

- [x] 3.1 Add or update regression coverage for the unit/integration stats capture path.
- [x] 3.2 Regenerate unit/integration evidence for an implementation commit and confirm CI/archive gates still accept the current canonical schema.
