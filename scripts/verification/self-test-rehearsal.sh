#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
VALIDATOR="$ROOT_DIR/scripts/verification/validate-rehearsal-result.sh"
PREFLIGHT="$ROOT_DIR/scripts/compose-preflight.sh"
WORK_DIR=$(mktemp -d)
trap 'rm -rf "$WORK_DIR"' EXIT

DOCKER_BIN_DIR="$WORK_DIR/bin"
mkdir -p "$DOCKER_BIN_DIR"
cat <<'EOF' > "$DOCKER_BIN_DIR/docker"
#!/usr/bin/env bash
set -euo pipefail

case "${1:-}" in
  run)
    exit 0
    ;;
  compose)
    shift
    while (($#)); do
      if [[ "$1" == "config" ]]; then
        exit 0
      fi
      shift
    done
    exit 0
    ;;
  *)
    printf 'Unexpected docker command in self-test: %s\n' "$*" >&2
    exit 1
    ;;
esac
EOF
chmod +x "$DOCKER_BIN_DIR/docker"

TEMP_RUNTIME_ROOT="$WORK_DIR/runtime"
mkdir -p "$TEMP_RUNTIME_ROOT/logs" "$TEMP_RUNTIME_ROOT/documents" "$TEMP_RUNTIME_ROOT/uploads" "$TEMP_RUNTIME_ROOT/mysql"

PLACEHOLDER_ENV="$WORK_DIR/production-placeholder.env"
VALID_ENV="$WORK_DIR/production-valid.env"
cp "$ROOT_DIR/deploy/env/production.env" "$PLACEHOLDER_ENV"
cp "$ROOT_DIR/deploy/env/production.env" "$VALID_ENV"
python3 - "$VALID_ENV" <<'PY'
from pathlib import Path
import sys

path = Path(sys.argv[1])
text = path.read_text(encoding="utf-8")
replacements = {
    "MYSQL_PASSWORD=change-me": "MYSQL_PASSWORD=prod-db-password-2026",
    "MYSQL_ROOT_PASSWORD=change-me-root": "MYSQL_ROOT_PASSWORD=prod-root-password-2026",
    "MI_DATABASE_PASSWORD=change-me": "MI_DATABASE_PASSWORD=prod-app-password-2026",
    "MI_AUTH_JWT_SECRET=change-me-production-secret": "MI_AUTH_JWT_SECRET=prod-jwt-secret-2026",
}
for old, new in replacements.items():
    text = text.replace(old, new)
path.write_text(text, encoding="utf-8")
PY

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

expect_failure \
  "compose preflight rejects blocked production placeholder secrets" \
  env \
  MI_COMPOSE_ENV_FILE="$PLACEHOLDER_ENV" \
  "$PREFLIGHT" production

PRECHECK_OUTPUT=$(env MI_COMPOSE_ENV_FILE="$PLACEHOLDER_ENV" "$PREFLIGHT" production 2>&1 >/dev/null || true)
if [[ "$PRECHECK_OUTPUT" != *"MYSQL_PASSWORD=change-me"* || "$PRECHECK_OUTPUT" != *"MI_AUTH_JWT_SECRET=change-me-production-secret"* || "$PRECHECK_OUTPUT" != *"MI_DATABASE_PASSWORD=change-me"* ]]; then
  printf 'Expected placeholder-secret preflight failure to name offending keys, got:\n%s\n' "$PRECHECK_OUTPUT" >&2
  exit 1
fi

expect_success \
  "compose preflight accepts overridden production secrets" \
  env \
  PATH="$DOCKER_BIN_DIR:$PATH" \
  MI_COMPOSE_ENV_FILE="$VALID_ENV" \
  MI_RUNTIME_BASE="$TEMP_RUNTIME_ROOT" \
  "$PREFLIGHT" production

echo
echo "rehearsal verification self-tests passed"
