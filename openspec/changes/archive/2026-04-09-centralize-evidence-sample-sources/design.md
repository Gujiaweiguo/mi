## Context

Canonical evidence examples currently exist in at least two forms: prose examples inside `docs/evidence-contract.md` and representative fixture files under `scripts/verification/testdata/`. Schema self-checks already rely on fixtures, which are more realistic and executable than doc-only examples. Without a single authoritative source, these examples can drift.

## Goals / Non-Goals

**Goals:**
- Define one repository-owned source of truth for representative evidence samples.
- Repoint docs and self-check tooling to that canonical source where appropriate.
- Preserve clarity for contributors while reducing duplicate updates.

**Non-Goals:**
- No change to evidence schema rules.
- No change to gate pass/fail semantics.
- No requirement to centralize every negative fixture; this change targets representative canonical samples only.

## Decisions

### 1. Reuse maintained fixtures as the canonical sample source
The existing passing fixtures under `scripts/verification/testdata/pass-ci/` and `scripts/verification/testdata/pass-archive/` are already executable and validated. They should become the authoritative sample source instead of separately maintained inline JSON examples.

### 2. Documentation should point to canonical sample files instead of duplicating full payloads
The human-readable contract doc can still explain fields and rules, but representative full JSON examples should come from canonical sample files to avoid drift.

### 3. Keep negative fixtures out of scope for centralization
Negative fixtures serve regression coverage rather than canonical documentation. This change only centralizes positive representative samples.

## Risks / Trade-offs

- **[Risk] Docs become slightly less self-contained** → **Mitigation:** keep concise field explanations in docs and link directly to the canonical sample files.
- **[Risk] Fixture moves could break existing self-tests** → **Mitigation:** prefer referencing existing paths rather than relocating files unless necessary.

## Migration Plan

1. Designate representative CI and archive/e2e fixture files as canonical sample sources.
2. Update `docs/evidence-contract.md` and schema self-check references to point to those samples.
3. Verify schema self-check and existing gate self-tests still pass.

## Open Questions

- Should the canonical sample files eventually live in a dedicated `samples/` directory, or is retaining them inside verified fixture directories the better low-drift option?
