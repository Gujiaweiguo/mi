## Context

The repository already has a working evidence pipeline: `run-unit.sh`, `run-integration.sh`, and `run-e2e.sh` write commit-scoped JSON evidence under `artifacts/verification/<commit-sha>/`. Downstream gates (CI-ready, archive-ready, cutover rehearsal) consume these files to decide pass/fail outcomes.

The `e2e` producer follows the correct pattern: it uses `set +e` before test execution, captures the exit code, writes evidence with a real `status` value (`passed` or `failed`), and then returns the original exit code. The `unit` and `integration` producers do not. They run under `set -euo pipefail`, hardcode `--status passed`, and exit immediately on test failure — which means no evidence file is written at all. Downstream gates can only report "missing evidence" instead of "failed evidence," weakening diagnostic value.

## Goals / Non-Goals

**Goals:**
- Make `run-unit.sh` and `run-integration.sh` always produce evidence for the evaluated commit, even when the underlying test command fails.
- Align their behavior with the existing `e2e` producer pattern: capture exit status, write accurate evidence, return the original exit code.
- Add self-check coverage that validates failed evidence production explicitly.

**Non-Goals:**
- Changing the evidence JSON schema or adding new evidence fields.
- Changing gate validation logic or gate behavior beyond what the corrected producers naturally enable.
- Introducing new business capability, deployment topology, or report/output scope.
- Refactoring the e2e producer, which already works correctly.

## Decisions

**Decision:** Adopt the `set +e` / capture-exit-code / `set -e` pattern already proven in `run-e2e.sh` for both `run-unit.sh` and `run-integration.sh`.

**Why:** The e2e producer is already tested in self-checks and production rehearsal. Reusing the same pattern keeps the verification surface consistent and avoids introducing a new failure-handling model.

**Alternatives considered:**
- Wrap each test command in a subshell with `|| true` and inspect `$?` afterward. Rejected because `set +e` is simpler, already proven, and avoids nested quoting issues.
- Refactor all three producers into a shared function library. Rejected for this change because it increases scope and risk; the current change is intentionally minimal.

**Decision:** Compute `STATUS` from the captured exit code and pass it to `write-evidence.py` instead of hardcoding `passed`.

**Why:** Evidence gates already check `status != "passed"` and reject accordingly. The only missing piece is that unit/integration producers never supply `status: "failed"`. Once they do, the existing gate logic works without modification.

**Decision:** For `run-unit.sh`, run backend and frontend test commands separately so one suite's failure does not prevent the other from producing counts.

**Why:** Currently both commands are in the same `set -euo pipefail` block. If backend tests fail, frontend tests never run, and no evidence is written. Separating them ensures both suites contribute to the aggregated stats and the evidence file always gets written.

## Risks / Trade-offs

- **[Risk] Test execution becomes slower because failures no longer short-circuit** → **Mitigation:** the `e2e` producer already runs both paths without short-circuiting and is acceptable; unit/integration suites are faster than e2e, so the impact is negligible.
- **[Risk] Self-checks that assert missing-evidence behavior need updating** → **Mitigation:** update self-checks in the same change to assert failed-evidence behavior instead.
- **[Risk] CI workflows that rely on immediate exit-on-failure semantics change observable behavior** → **Mitigation:** the scripts still exit non-zero after writing evidence; downstream gate diagnostics improve rather than degrade.
