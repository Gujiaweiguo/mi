## 1. Schema self-check design

- [x] 1.1 Identify representative CI-style and archive/e2e-style evidence samples or fixtures for schema validation.
- [x] 1.2 Define the repository-native execution path for schema self-checks.

## 2. Implementation

- [x] 2.1 Add a schema self-check script that validates `schemas/evidence-v1.json` parsing and sample conformance.
- [x] 2.2 Update verification docs or entrypoints to mention the schema self-check.

## 3. Verification

- [x] 3.1 Run the schema self-check and confirm representative samples validate successfully.
- [x] 3.2 Re-run existing verification self-tests to confirm schema self-check additions do not conflict with gate validation coverage.
