#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
WORK_DIR=$(mktemp -d)
trap 'rm -rf "$WORK_DIR"' EXIT

expect_success() {
  local name="$1"
  shift
  echo "[PASS-EXPECTED] $name"
  "$@"
}

expect_failure() {
  local name="$1"
  shift
  echo "[FAIL-EXPECTED] $name"
  if "$@"; then
    echo "Expected failure but command passed: $name" >&2
    exit 1
  fi
}

cat <<'EOF' > "$WORK_DIR/summary-go.json"
{
  "schema_version": "1",
  "project": "mi",
  "change": "first-release-acceptance-closure",
  "commit_sha": "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
  "started_at": "2026-04-28T00:00:00Z",
  "finished_at": "2026-04-28T00:10:00Z",
  "status": "GO",
  "scope": {
    "release_boundary": "first-release non-membership scope",
    "report_inventory": "R01-R19",
    "excluded_domains": ["membership/Associator"]
  },
  "gates": {
    "archive_ready": "passed",
    "rehearsal_ready": "passed"
  },
  "blockers": [],
  "deferred": [
    {
      "id": "future-item",
      "classification": "deferred",
      "source": "manual-review",
      "summary": "Allowed post-release item."
    }
  ],
  "artifacts": {
    "verification": [
      "artifacts/verification/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/unit.json",
      "artifacts/verification/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/integration.json",
      "artifacts/verification/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/e2e.json"
    ],
    "rehearsal_result": "artifacts/rehearsal/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/cutover-rehearsal-production.json",
    "findings": "artifacts/release-readiness/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/findings.json"
  }
}
EOF

cat <<'EOF' > "$WORK_DIR/summary-stale.json"
{
  "schema_version": "1",
  "project": "mi",
  "change": "first-release-acceptance-closure",
  "commit_sha": "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb",
  "started_at": "2026-04-28T00:00:00Z",
  "finished_at": "2026-04-28T00:10:00Z",
  "status": "GO",
  "scope": {
    "release_boundary": "first-release non-membership scope",
    "report_inventory": "R01-R19",
    "excluded_domains": ["membership/Associator"]
  },
  "gates": {
    "archive_ready": "passed",
    "rehearsal_ready": "passed"
  },
  "blockers": [],
  "deferred": [],
  "artifacts": {
    "verification": [
      "artifacts/verification/bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb/unit.json",
      "artifacts/verification/bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb/integration.json",
      "artifacts/verification/bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb/e2e.json"
    ],
    "rehearsal_result": "artifacts/rehearsal/bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb/cutover-rehearsal-production.json",
    "findings": null
  }
}
EOF

cat <<'EOF' > "$WORK_DIR/summary-invalid-go.json"
{
  "schema_version": "1",
  "project": "mi",
  "change": "first-release-acceptance-closure",
  "commit_sha": "cccccccccccccccccccccccccccccccccccccccc",
  "started_at": "2026-04-28T00:00:00Z",
  "finished_at": "2026-04-28T00:10:00Z",
  "status": "GO",
  "scope": {
    "release_boundary": "first-release non-membership scope",
    "report_inventory": "R01-R19",
    "excluded_domains": ["membership/Associator"]
  },
  "gates": {
    "archive_ready": "failed",
    "rehearsal_ready": "passed"
  },
  "blockers": [],
  "deferred": [],
  "artifacts": {
    "verification": [
      "artifacts/verification/cccccccccccccccccccccccccccccccccccccccc/unit.json",
      "artifacts/verification/cccccccccccccccccccccccccccccccccccccccc/integration.json",
      "artifacts/verification/cccccccccccccccccccccccccccccccccccccccc/e2e.json"
    ],
    "rehearsal_result": "artifacts/rehearsal/cccccccccccccccccccccccccccccccccccccccc/cutover-rehearsal-production.json",
    "findings": null
  }
}
EOF

cat <<'EOF' > "$WORK_DIR/summary-no-go-no-blocker.json"
{
  "schema_version": "1",
  "project": "mi",
  "change": "first-release-acceptance-closure",
  "commit_sha": "dddddddddddddddddddddddddddddddddddddddd",
  "started_at": "2026-04-28T00:00:00Z",
  "finished_at": "2026-04-28T00:10:00Z",
  "status": "NO-GO",
  "scope": {
    "release_boundary": "first-release non-membership scope",
    "report_inventory": "R01-R19",
    "excluded_domains": ["membership/Associator"]
  },
  "gates": {
    "archive_ready": "passed",
    "rehearsal_ready": "failed"
  },
  "blockers": [],
  "deferred": [],
  "artifacts": {
    "verification": [
      "artifacts/verification/dddddddddddddddddddddddddddddddddddddddd/unit.json",
      "artifacts/verification/dddddddddddddddddddddddddddddddddddddddd/integration.json",
      "artifacts/verification/dddddddddddddddddddddddddddddddddddddddd/e2e.json"
    ],
    "rehearsal_result": null,
    "findings": null
  }
}
EOF

VALIDATOR="$ROOT_DIR/scripts/verification/validate-release-readiness.sh"

expect_success \
  "release-readiness validator accepts passing GO result" \
  bash "$VALIDATOR" "$WORK_DIR/summary-go.json" --commit-sha aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

expect_failure \
  "release-readiness validator rejects stale commit sha" \
  bash "$VALIDATOR" "$WORK_DIR/summary-stale.json" --commit-sha aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

expect_failure \
  "release-readiness validator rejects GO result with failed gate" \
  bash "$VALIDATOR" "$WORK_DIR/summary-invalid-go.json" --commit-sha cccccccccccccccccccccccccccccccccccccccc

expect_failure \
  "release-readiness validator rejects NO-GO result without blockers" \
  bash "$VALIDATOR" "$WORK_DIR/summary-no-go-no-blocker.json" --commit-sha dddddddddddddddddddddddddddddddddddddddd

echo
echo "release-readiness verification self-tests passed"
