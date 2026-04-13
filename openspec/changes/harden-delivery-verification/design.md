## Context

The repository already has a strong verification and delivery foundation: commit-scoped evidence gates are documented, CI-ready and archive-ready workflows exist, Compose-based runtime operations are defined, and most first-release business slices already have executable test coverage. The current weakness is narrower and more practical: the default delivery path still leaves room for regressions that are neither business-scope gaps nor architecture omissions.

Specifically, the CI-ready path does not yet require explicit frontend typechecking or backend static analysis, the browser end-to-end suite is dominated by API-mocked flows rather than a live frontend-backend-MySQL seam, and repository/runtime hygiene issues can still interfere with normal tooling. This change is therefore cross-cutting but bounded: it hardens how release confidence is established without widening first-release business scope.

## Goals / Non-Goals

**Goals:**
- Extend the default CI-ready path so it validates frontend type safety, backend static analysis, and build health alongside existing test evidence.
- Add a real-stack verification path that exercises the supported frontend, backend, and MySQL integration seam rather than relying only on API-mocked browser tests.
- Make delivery hardening fit the existing commit-scoped evidence model instead of introducing a second release-readiness system.
- Tighten repository/runtime hygiene checks so tracked artifacts that break normal tooling are detected and corrected early.
- Keep the change aligned to existing capabilities: `platform-foundation` and `deployment-and-cutover-operations`.

**Non-Goals:**
- No new operator-facing business modules or first-release scope expansion.
- No replacement of the current evidence-gate model with a different release process.
- No wholesale rewrite of the frontend E2E suite from mocked to fully live-stack coverage.
- No backend architecture migration from raw SQL to Gorm, or other broad implementation-style changes.
- No production topology redesign beyond the documented Compose-based model.

## Decisions

### 1. Treat static validation as a CI-ready prerequisite, not as optional local hygiene

**Decision:** The CI-ready path will explicitly run frontend typechecking, backend static analysis, and executable build validation before or alongside the existing unit/integration evidence flow.

**Why:** Test evidence proves behavior for covered paths, but it does not guarantee that type drift, compile-only regressions, or obvious static-analysis failures are blocked before merge. This repository is mature enough that these failures should be caught in the default path contributors already trust.

**Alternatives considered:**
- Leave typecheck/build/static analysis as optional local commands. Rejected because optional checks are exactly the kind of gaps that escape routine push/PR validation.
- Make archive-ready the only place where static validation runs. Rejected because that would delay cheap, deterministic failures until a later gate.

### 2. Keep evidence gates commit-scoped, but allow hardening checks to fail the CI-ready path before evidence validation completes

**Decision:** The repository will continue to use the current commit-scoped `unit`, `integration`, and `e2e` evidence model. Hardening checks such as typecheck, vet, and build validation will be treated as prerequisite steps in the CI-ready execution path rather than as replacements for evidence files.

**Why:** The existing evidence contract is already coherent and documented. The gap is that CI-ready currently reaches the gate without first proving the codebase is statically healthy. Preserving the evidence model while tightening preconditions improves rigor without creating a second source of truth.

**Alternatives considered:**
- Introduce new evidence artifact types for typecheck or vet. Rejected for now because the primary need is stronger default enforcement, not a larger evidence taxonomy.
- Fold static-check outcomes into unit evidence semantics. Rejected because test-result evidence and prerequisite validation are different concepts and should remain distinguishable.

### 3. Add one authoritative real-stack E2E path instead of converting the entire browser suite at once

**Decision:** The change will require at least one end-to-end verification path that runs against the real frontend, backend, and MySQL stack, while allowing the rest of the existing mocked Playwright coverage to remain in place.

**Why:** The highest-value gap is the missing validation of the integration seam itself. One stable, representative live-stack path closes that risk much faster than attempting to convert the entire browser suite in one hardening change.

**Alternatives considered:**
- Convert every Playwright spec to real-stack execution. Rejected because it is too large for a hardening-focused change and risks destabilizing existing coverage.
- Keep all E2E tests mocked and rely on backend integration tests alone. Rejected because it leaves the real FE↔BE contract untested in a user-level flow.

### 4. Anchor the real-stack path to existing Compose and runtime assumptions

**Decision:** The real-stack E2E path should run through the already supported local/Compose runtime model instead of inventing a parallel test topology.

**Why:** This repository already has documented runtime assumptions for frontend, backend, and MySQL. Reusing them reduces drift between delivery verification and actual deployment expectations.

**Alternatives considered:**
- Create a bespoke one-off E2E topology separate from the documented runtime model. Rejected because it would create a verification environment that is easier to run but less representative of real operations.

### 5. Treat repository/runtime hygiene as part of delivery hardening when it affects verification trustworthiness

**Decision:** The change will explicitly cover cleanup or prevention of tracked runtime artifacts that break repository tooling or obscure release confidence, but only where those artifacts interfere with normal verification or developer operations.

**Why:** Delivery confidence is not only about test scripts. If ordinary repository reads or automation are disrupted by stale runtime entries, the verification surface becomes less trustworthy.

**Alternatives considered:**
- Ignore repository hygiene as unrelated maintenance. Rejected because some hygiene issues directly affect the default validation path and day-to-day tooling reliability.

## Risks / Trade-offs

- **[Risk] CI-ready becomes slower and contributors feel more friction** → **Mitigation:** keep the added checks deterministic and bounded, and reserve heavier live-stack coverage for a small authoritative path instead of the entire suite.
- **[Risk] A real-stack E2E path becomes flaky because it depends on more infrastructure than mocked browser tests** → **Mitigation:** keep the live-stack scenario narrow, use the supported runtime topology, and choose a stable business path with well-understood fixtures.
- **[Risk] Hardening checks drift from documented evidence-gate rules** → **Mitigation:** update capability specs and delivery documentation in the same change so the CI-ready and operational expectations remain synchronized.
- **[Risk] Teams may over-interpret one real-stack E2E path as full end-to-end coverage** → **Mitigation:** document it as a minimum seam-validation contract, not a claim that all browser flows now run against a live backend.
- **[Risk] Hygiene cleanup could accidentally widen into general repository refactoring** → **Mitigation:** keep hygiene scope limited to artifacts that interfere with validation, build, or tool reliability.

## Migration Plan

1. Audit the current CI-ready path, local validation commands, and workflow files to identify where typecheck, backend static analysis, and build verification fit without weakening evidence-gate semantics.
2. Define the minimum real-stack end-to-end path, including which user flow, runtime topology, and fixture/bootstrap assumptions make it representative and stable.
3. Update verification entrypoints and workflows so hardening checks run in the default CI-ready path and fail fast with explicit stage boundaries.
4. Add the live-stack verification path and document how it complements, rather than replaces, mocked browser coverage and backend integration tests.
5. Clean or guard against repository/runtime artifacts that interfere with normal verification and file-based tooling.
6. Align platform and deployment specs plus verification/deployment docs so the repository describes the hardened workflow exactly.

Rollback is low-risk because this design is additive and procedural. If a new check or live-stack path proves unstable, the repository can temporarily revert that hardening step while keeping the existing evidence-gate model intact.

## Open Questions

- Which exact CI-ready static-analysis command should be treated as the supported backend baseline: `go vet` only, or a broader repo-native static-check command if one is introduced?
- Which operator flow is the best single candidate for the required live-stack end-to-end path: login plus shell health, or a narrow Lease → Invoice seam slice?
- Should build validation be required in both CI-ready and archive-ready paths, or only CI-ready with archive inheriting the stricter prerequisite implicitly?
