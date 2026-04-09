## Context

The repository already enforces commit-scoped evidence gates, but historical evidence producers and manual updates introduced schema drift (`schema_version` type differences, incompatible `stats` keys, and inconsistent `source` structures). This change defines one canonical contract so all required evidence files are validated and produced under the same rules.

## Goals / Non-Goals

**Goals:**
- Define canonical evidence structure and invariants for `unit`, `integration`, and `e2e`.
- Ensure CI/archive gating relies on consistent schema semantics for the evaluated commit SHA.
- Clarify producer-side requirements so verification entry scripts cannot emit contract-breaking payloads.

**Non-Goals:**
- No expansion of test scope or new business scenarios.
- No replacement of current test frameworks.
- No relaxation of commit-scoped gate policy.

## Decisions

### 1. Keep scope in `platform-foundation`
Evidence schema consistency is a platform verification foundation concern; this change modifies existing platform requirements only.

### 2. Canonical evidence fields and value constraints are normative
The contract requires fixed top-level keys and normalized nested fields (`source.kind/workflow/run_id`, `stats.total/passed/failed/skipped`) with strict typing and consistency checks.

### 3. Producer and validator requirements are aligned
Evidence producers must emit canonical fields and valid values; gate validators must reject schema-drift payloads with explicit diagnostics.

### 4. Preserve gate hierarchy while removing schema ambiguity
CI still requires `unit`+`integration`; archive still requires `unit`+`integration`+`e2e`. This change tightens evidence validity criteria without changing gate topology.

## Risks / Trade-offs

- **[Risk] Legacy evidence snapshots may no longer pass stricter validation** → **Mitigation:** scope enforcement to current-commit gating and document canonical format for regeneration.
- **[Risk] Overly rigid schema may slow quick local rehearsals** → **Mitigation:** keep required fields minimal but explicit; preserve local-friendly `source` values.
- **[Risk] Partial producer updates can create temporary mismatch** → **Mitigation:** require producer+validator alignment in same implementation slice.

## Migration Plan

1. Define delta requirements for canonical evidence contract behavior in `platform-foundation`.
2. Implement producer and validator alignment to emit and enforce canonical schema.
3. Add/expand automated checks for schema drift rejection and consistent gate behavior.
4. Regenerate commit-scoped evidence and confirm CI/archive gates on implementation commit.

## Open Questions

- Should future contract evolution use explicit `schema_version` migration policy (e.g., versioned backward-compat window), or remain strict single-version for first release?
- Should canonical evidence include optional richer metrics beyond gate-critical fields in a dedicated extension section?
