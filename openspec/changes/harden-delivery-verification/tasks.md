## 1. CI-ready prerequisite hardening

- [x] 1.1 Audit the current CI-ready entrypoints, workflow jobs, and repository validation commands to identify the supported frontend typecheck, backend static-analysis, and frontend build commands for the hardened path.
- [x] 1.2 Update the CI-ready execution path under `.github/workflows/` and `scripts/` so frontend typecheck, backend static analysis, and frontend build verification run as prerequisite steps before CI-ready success is reported.
- [x] 1.3 Ensure CI-ready failure behavior is explicit when prerequisite validation fails, while keeping the commit-scoped evidence convention unchanged under `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json`.

## 2. Live-stack verification path

- [x] 2.1 Select and document one stable real-stack operator flow that exercises the real frontend, backend, and MySQL seam under the supported runtime topology.
- [x] 2.2 Implement the supporting runtime/bootstrap path needed to run that live-stack flow without relying on mocked API interception for the primary application seam.
- [x] 2.3 Add automated verification for the chosen live-stack flow so release validation produces a clear pass/fail result when the real application boundary is exercised.
- [x] 2.4 Ensure release validation treats mock-only browser coverage as insufficient for the live-stack requirement and blocks readiness when the live-stack path fails.

## 3. Repository and runtime hygiene

- [x] 3.1 Identify tracked runtime or filesystem artifacts that interfere with repository tooling or verification reliability, including stale entries under `deploy/`.
- [x] 3.2 Remove, ignore, or guard against those artifacts in the supported validation workflow so normal file-based tooling and verification paths no longer break on repository hygiene issues.

## 4. Documentation and gate alignment

- [x] 4.1 Update verification and delivery documentation so the hardened CI-ready path clearly distinguishes prerequisite validation from evidence-gate evaluation.
- [x] 4.2 Update deployment or release-readiness documentation so the required live-stack verification path is described as a complement to existing mocked browser coverage and backend integration tests.
- [x] 4.3 Verify the documented gate expectations remain consistent with the current evidence convention: CI-ready requires current-commit `unit` and `integration` evidence, while archive-ready requires current-commit `unit`, `integration`, and `e2e` evidence under `artifacts/verification/<commit-sha>/`.

## 5. End-to-end validation of the change

- [x] 5.1 Run the hardened CI-ready path and confirm it fails on prerequisite validation problems before reporting readiness.
- [x] 5.2 Run the supported live-stack verification flow and confirm it produces a reliable pass/fail result against the real frontend, backend, and MySQL seam.
- [x] 5.3 Confirm the updated workflows and docs leave the `harden-delivery-verification` change in an archive-ready state with matching specs, design, and implementation behavior.
