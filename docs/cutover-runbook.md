# Cutover Runbook

This runbook freezes the first-release cutover policy for `legacy-system-migration`.

## Cutover Model

- The release is a **fresh-start cutover**.
- No legacy business records are migrated.
- No legacy drafts, approvals, open invoices, open charges, or in-flight operational records are brought into the new system.
- Base/master data is initialized into the new system before go-live.
- The old system remains the historical reference system after cutover.

## What Must Exist Before Go-Live

### 1. Environment Readiness
- Test environment Compose stack is healthy.
- Production Compose stack is healthy.
- Mounted paths for config, logs, generated documents/uploads, and MySQL data are validated.
- Verification gate tooling is available:
  - `scripts/ci-ready.sh`
  - `scripts/archive-ready.sh`

### 2. Base Data Bootstrap Pack
At minimum, the first release must seed:

- departments / organization root
- roles
- users
- user-role bindings
- function / permission registry
- stores
- buildings / floors / units / areas
- shop types / trade categories / other required code tables
- workflow business groups
- workflow definitions and nodes
- print/export template configuration needed by first-release outputs

### 3. Operational Capability Readiness
- Lease contract lifecycle is operable
- bill / invoice lifecycle is operable
- workflow approval capability is operable
- mandatory print/document outputs are operable
- mandatory tax/voucher exports are operable
- mandatory Excel import/export flows are operable
- frozen reports `R01-R19` are operable within first-release scope

## Explicit Cutover Rules

### Legacy Data Policy
- Historical transaction migration is **not allowed** in first release.
- Legacy pending approvals are **not resumed** in the new system.
- Legacy open operational items are **not converted** into starting records.
- If the business needs any starting balance or opening-state data, that must be entered via approved bootstrap datasets rather than migrated transaction history.

### Authentication Bootstrap Rule
Before go-live, the new system must have:

- at least one admin user
- at least one active role
- at least one active department / organization root
- at least one valid user-role binding

Legacy login evidence shows login depends on user, role, and department all being valid. The new system may redesign the auth model, but go-live still requires a working admin baseline.

### Numbering Initialization Rule
Before go-live, the new system must define and initialize numbering for:

- contracts
- bills
- invoices
- any mandatory voucher/export identifiers

Numbering does **not** need to preserve legacy continuity, but it must be deterministic and documented before billing/invoice go-live.

### Workflow Initialization Rule
Before go-live, the system must define the initial workflow template set for first-release document types, including at minimum:

- Lease contract approval
- Lease change / amendment approval where required
- bill / invoice approval paths where required
- mandatory financial adjustment/cancel flows that are in first-release scope

Timeout/escalation automation is excluded and must not be treated as a launch dependency.

## Bootstrap Data Checklist

The following items must be prepared, reviewed, and loaded before the production cutover rehearsal is allowed to pass:

- [ ] organization root / mall root
- [ ] departments and hierarchy
- [ ] stores
- [ ] buildings / floors / units / areas
- [ ] trade / shop type / supporting code tables
- [ ] admin roles
- [ ] admin users
- [ ] user-role bindings
- [ ] function / permission mapping for first-release operations
- [ ] workflow business groups
- [ ] workflow definitions
- [ ] workflow nodes and role assignments
- [ ] numbering initialization values
- [ ] print template configuration
- [ ] tax/voucher export configuration
- [ ] Excel import/export template configuration

## Release Gates

### CI Ready
Must pass for the current commit:

- unit evidence
- integration evidence

Evidence path convention:

```text
artifacts/verification/<commit-sha>/unit.json
artifacts/verification/<commit-sha>/integration.json
```

### Archive Ready
Must pass for the current commit:

- unit evidence
- integration evidence
- e2e evidence

Evidence path convention:

```text
artifacts/verification/<commit-sha>/unit.json
artifacts/verification/<commit-sha>/integration.json
artifacts/verification/<commit-sha>/e2e.json
```

### Go-Live Ready
In addition to archive-ready status, go-live requires:

- mandatory outputs from `docs/output-catalog.md` are validated
- report scope `R01-R19` is validated against `report-acceptance-matrix.md`
- cutover rehearsal passes
- rollback procedure is documented and rehearsed

Rehearsal command:

```bash
scripts/cutover-rehearsal.sh test --build
```

The rehearsal now executes its operational checks in this order:

1. compose preflight for the target environment
2. archive-ready evidence validation for the current HEAD commit
3. clean runtime reset for the rehearsal environment
4. bootstrap and fresh-start verification
5. full stack smoke validation
6. backup bundle creation
7. restore rehearsal with `--restore-runtime-files`
8. post-restore verification and post-restore smoke validation
9. machine-readable GO/NO-GO result writeout

Result artifacts are written to:

```text
artifacts/rehearsal/<commit-sha>/cutover-rehearsal-test-<timestamp>.json
artifacts/rehearsal/<commit-sha>/cutover-rehearsal-test-<timestamp>.log
```

`status: "GO"` means the rehearsal passed all required checks. Any failed check produces `status: "NO-GO"`.

The rehearsal result artifact also records per-gate status and `failing_gate` so the blocking step is machine-identifiable.

## Cutover Sequence

### Phase A — Pre-Cutover Freeze
1. Confirm release scope still matches OpenSpec.
2. Confirm no unresolved blocking decisions remain.
3. Confirm mandatory outputs are complete.
4. Confirm archive-ready test gate status.
5. Freeze bootstrap data pack.

### Phase B — Environment Preparation
1. Start production stack.
2. Apply schema migrations.
3. Load deterministic bootstrap/base data.
4. Load workflow templates and permission mappings.
5. Initialize numbering.

### Phase C — Smoke Validation
1. Login with admin/operator user.
2. Validate organization/store/master data visibility.
3. Validate Lease creation and workflow submission.
4. Validate charge/invoice path.
5. Validate mandatory print/export/tax/Excel outputs.
6. Validate a minimal report sample from `R01-R19`.

### Phase D — Go/No-Go Decision
The cutover is **GO** only if all smoke checks pass and no blocking decision remains unresolved.

The cutover is **NO-GO** if any of the following happens:

- bootstrap data is incomplete
- login/admin baseline fails
- workflow templates are missing or invalid
- numbering is undefined or broken
- mandatory outputs fail
- test evidence / release-gate evidence is missing for the release commit

## Rollback Rule

If the cutover reaches a NO-GO state after deployment but before operational acceptance:

1. stop traffic to the new system
2. preserve logs and failure evidence
3. discard the failed production bootstrap dataset if needed
4. revert to the old system for active operations
5. record the failed gate and remediation plan before retrying

Because this is a fresh-start cutover, rollback does not involve replaying migrated transactional data.

## Blocking Decisions for Downstream Tasks

These decisions must be treated as blocking until explicitly resolved in implementation artifacts or bootstrap specs:

1. exact permission action map for first-release roles
2. initial workflow template inventory
3. contract / bill / invoice numbering format
4. mall / organization root initialization convention
5. bootstrap admin credential policy
6. exact tax/voucher export field layout and configuration binding
7. final approved bootstrap data pack contents

## Required References

- `openspec/changes/legacy-system-migration/design.md`
- `openspec/changes/legacy-system-migration/tasks.md`
- `openspec/changes/legacy-system-migration/test-evidence-contract.md`
- `docs/legacy-inventory.md`
- `docs/decision-log.md`
- `docs/output-catalog.md`
