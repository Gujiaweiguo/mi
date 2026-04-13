#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
VALIDATOR="$ROOT_DIR/scripts/verification/validate-rehearsal-result.sh"
PREFLIGHT="$ROOT_DIR/scripts/compose-preflight.sh"
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

python3 - "$WORK_DIR/pass.json" <<'PY'
import json, sys
from pathlib import Path
Path(sys.argv[1]).write_text(json.dumps({
    "schema_version": 1,
    "change": "deployment-and-cutover-hardening",
    "commit_sha": "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
    "environment": "production",
    "started_at": "2026-04-08T00:00:00Z",
    "finished_at": "2026-04-08T00:10:00Z",
    "status": "GO",
    "failing_gate": None,
    "checks": {
        "preflight": "passed",
        "archive_ready": "passed",
        "bootstrap": "passed",
        "fresh_start": "passed",
        "smoke": "passed",
        "backup": "passed",
        "restore": "passed",
        "restore_smoke": "passed",
    },
    "artifacts": {
        "log": "artifacts/rehearsal/aaaaaaaa/log.log",
        "backup_archive": "artifacts/backups/production/backup.tar.gz",
        "verification": ["unit.json", "integration.json", "e2e.json"],
        "checklist": "docs/go-live-checklist.md",
    },
}, indent=2) + "\n", encoding="utf-8")
PY

python3 - "$WORK_DIR/stale.json" <<'PY'
import json, sys
from pathlib import Path
Path(sys.argv[1]).write_text(json.dumps({
    "schema_version": 1,
    "change": "deployment-and-cutover-hardening",
    "commit_sha": "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb",
    "environment": "production",
    "started_at": "2026-04-08T00:00:00Z",
    "finished_at": "2026-04-08T00:10:00Z",
    "status": "GO",
    "failing_gate": None,
    "checks": {
        "preflight": "passed",
        "archive_ready": "passed",
        "bootstrap": "passed",
        "fresh_start": "passed",
        "smoke": "passed",
        "backup": "passed",
        "restore": "passed",
        "restore_smoke": "passed",
    },
    "artifacts": {
        "log": "artifacts/rehearsal/bbbbbbbb/log.log",
        "backup_archive": "artifacts/backups/production/backup.tar.gz",
        "verification": ["unit.json", "integration.json", "e2e.json"],
        "checklist": "docs/go-live-checklist.md",
    },
}, indent=2) + "\n", encoding="utf-8")
PY

python3 - "$WORK_DIR/malformed.json" <<'PY'
from pathlib import Path
Path(__import__('sys').argv[1]).write_text('{"status":"GO"}\n', encoding='utf-8')
PY

python3 - "$WORK_DIR/failed-go.json" <<'PY'
import json, sys
from pathlib import Path
Path(sys.argv[1]).write_text(json.dumps({
    "schema_version": 1,
    "change": "deployment-and-cutover-hardening",
    "commit_sha": "cccccccccccccccccccccccccccccccccccccccc",
    "environment": "production",
    "started_at": "2026-04-08T00:00:00Z",
    "finished_at": "2026-04-08T00:10:00Z",
    "status": "GO",
    "failing_gate": None,
    "checks": {
        "preflight": "passed",
        "archive_ready": "passed",
        "bootstrap": "passed",
        "fresh_start": "passed",
        "smoke": "passed",
        "backup": "failed",
        "restore": "passed",
        "restore_smoke": "passed",
    },
    "artifacts": {
        "log": "artifacts/rehearsal/cccccccc/log.log",
        "backup_archive": "artifacts/backups/production/backup.tar.gz",
        "verification": ["unit.json", "integration.json", "e2e.json"],
        "checklist": "docs/go-live-checklist.md",
    },
}, indent=2) + "\n", encoding="utf-8")
PY

expect_success \
  "rehearsal validator accepts passing GO result" \
  "$VALIDATOR" "$WORK_DIR/pass.json" --commit-sha aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

expect_failure \
  "rehearsal validator rejects stale commit sha" \
  "$VALIDATOR" "$WORK_DIR/stale.json" --commit-sha aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

expect_failure \
  "rehearsal validator rejects malformed result" \
  "$VALIDATOR" "$WORK_DIR/malformed.json"

expect_failure \
  "rehearsal validator rejects GO result with failed checks" \
  "$VALIDATOR" "$WORK_DIR/failed-go.json" --commit-sha cccccccccccccccccccccccccccccccccccccccc

expect_failure \
  "compose preflight rejects unsupported environment" \
  "$PREFLIGHT" invalid

echo
echo "rehearsal verification self-tests passed"
