## 1. CI entrypoint wiring

- [x] 1.1 Audit the current CI-ready path and determine where the schema self-check should run.
- [x] 1.2 Update the CI-ready entrypoint to execute the schema self-check before CI gate validation.

## 2. Documentation alignment

- [x] 2.1 Update verification documentation to explain that the CI-ready path now includes the schema self-check.

## 3. Verification

- [x] 3.1 Run the schema self-check and the updated CI-ready path to confirm behavior remains coherent.
- [x] 3.2 Re-run relevant verification self-tests to confirm the wiring change does not break existing validation coverage.
