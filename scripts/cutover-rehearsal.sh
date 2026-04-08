#!/usr/bin/env bash
set -uo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/cutover-rehearsal.sh [test|production] [--build]

Runs a cutover rehearsal for the selected environment and writes a machine-
readable GO/NO-GO result under artifacts/rehearsal/<commit-sha>/.

The script validates:
- archive-ready gate evidence for the current commit
- database bootstrap with the fresh-start cutover seed set
- database verification, including fresh-start checks
- full compose smoke validation
- backup + restore rehearsal
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-test}
BUILD_FLAG=${2:-}

case "$ENVIRONMENT" in
  test) ;;
  production)
    printf 'Use the test environment for destructive cutover rehearsal work.\n' >&2
    exit 1
    ;;
  *)
    printf 'Unsupported environment: %s\n' "$ENVIRONMENT" >&2
    usage
    exit 1
    ;;
esac

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
ENV_FILE="$ROOT_DIR/deploy/env/$ENVIRONMENT.env"
COMMIT_SHA=$(git -C "$ROOT_DIR" rev-parse HEAD)
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
STAMP=$(date -u +"%Y%m%dT%H%M%SZ")
RESULT_DIR="$ROOT_DIR/artifacts/rehearsal/$COMMIT_SHA"
RESULT_FILE="$RESULT_DIR/cutover-rehearsal-$ENVIRONMENT-$STAMP.json"
LOG_FILE="$RESULT_DIR/cutover-rehearsal-$ENVIRONMENT-$STAMP.log"
PREFLIGHT_SCRIPT="$ROOT_DIR/scripts/compose-preflight.sh"
EVIDENCE_DIR="artifacts/verification/$COMMIT_SHA"
BACKUP_DIR="$ROOT_DIR/artifacts/backups/$ENVIRONMENT"
UNIT_EVIDENCE="$EVIDENCE_DIR/unit.json"
INTEGRATION_EVIDENCE="$EVIDENCE_DIR/integration.json"
E2E_EVIDENCE="$EVIDENCE_DIR/e2e.json"
CHECKLIST_PATH="docs/go-live-checklist.md"
BUILD_ARG=()

if [[ "$BUILD_FLAG" == "--build" ]]; then
  BUILD_ARG=(--build)
fi

mkdir -p "$RESULT_DIR"

preflight=not_run
archive_ready=not_run
bootstrap=not_run
fresh_start=not_run
smoke=not_run
backup=not_run
restore=not_run
restore_smoke=not_run
status=NO-GO
failing_gate=
backup_archive=

cleanup_stack() {
  docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
}

write_result() {
  local finished_at
  finished_at=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

  python3 - "$RESULT_FILE" "$COMMIT_SHA" "$ENVIRONMENT" "$STARTED_AT" "$finished_at" "$status" "$preflight" "$archive_ready" "$bootstrap" "$fresh_start" "$smoke" "$backup" "$restore" "$restore_smoke" "$failing_gate" "$LOG_FILE" "$backup_archive" "$UNIT_EVIDENCE" "$INTEGRATION_EVIDENCE" "$E2E_EVIDENCE" "$CHECKLIST_PATH" <<'PY'
import json
import sys
from pathlib import Path

(
    result_file,
    commit_sha,
    environment,
    started_at,
    finished_at,
    status,
    preflight,
    archive_ready,
    bootstrap,
    fresh_start,
    smoke,
    backup,
    restore,
    restore_smoke,
    failing_gate,
    log_file,
    backup_archive,
    unit_evidence,
    integration_evidence,
    e2e_evidence,
    checklist_path,
) = sys.argv[1:22]

result = {
    "schema_version": 1,
    "change": "deployment-and-cutover-hardening",
    "commit_sha": commit_sha,
    "environment": environment,
    "started_at": started_at,
    "finished_at": finished_at,
    "status": status,
    "failing_gate": failing_gate or None,
    "checks": {
        "preflight": preflight,
        "archive_ready": archive_ready,
        "bootstrap": bootstrap,
        "fresh_start": fresh_start,
        "smoke": smoke,
        "backup": backup,
        "restore": restore,
        "restore_smoke": restore_smoke,
    },
    "artifacts": {
        "log": log_file,
        "backup_archive": backup_archive,
        "verification": [
            unit_evidence,
            integration_evidence,
            e2e_evidence,
        ],
        "checklist": checklist_path,
    },
}

Path(result_file).write_text(json.dumps(result, indent=2) + "\n", encoding="utf-8")
PY
}

trap 'cleanup_stack; write_result' EXIT

exec > >(tee "$LOG_FILE") 2>&1

run_step() {
  local name=$1
  shift
  if "$@"; then
    printf '%s: passed\n' "$name"
    printf -v "$name" '%s' passed
    return 0
  fi

  printf '%s: failed\n' "$name" >&2
  printf -v "$name" '%s' failed
  failing_gate=$name
  return 1
}

if ! run_step preflight "$PREFLIGHT_SCRIPT" "$ENVIRONMENT"; then
  exit 1
fi

if ! run_step archive_ready "$ROOT_DIR/scripts/archive-ready.sh"; then
  exit 1
fi

cleanup_stack
for runtime_name in mysql documents uploads logs; do
  docker run --rm -v "$ROOT_DIR/deploy/runtime/$ENVIRONMENT/$runtime_name:/target" alpine:3.20 sh -lc 'rm -rf /target/* /target/.[!.]* /target/..?*'
done

if ! run_step bootstrap "$ROOT_DIR/scripts/db-bootstrap.sh" "$ENVIRONMENT" cutover; then
  exit 1
fi

if ! run_step fresh_start "$ROOT_DIR/scripts/db-verify.sh" "$ENVIRONMENT" all; then
  exit 1
fi

SMOKE_ARGS=("$ENVIRONMENT" "--keep-running")
if [[ ${#BUILD_ARG[@]} -gt 0 ]]; then
  SMOKE_ARGS+=("${BUILD_ARG[@]}")
fi

if ! run_step smoke "$ROOT_DIR/scripts/compose-smoke-test.sh" "${SMOKE_ARGS[@]}"; then
  exit 1
fi

if run_step backup "$ROOT_DIR/scripts/db-backup.sh" "$ENVIRONMENT"; then
  backup_archive=$(python3 - <<PY
from pathlib import Path
paths = sorted(Path(${BACKUP_DIR@Q}).glob("*.tar.gz"), key=lambda p: p.stat().st_mtime)
print(paths[-1] if paths else "")
PY
)
else
  exit 1
fi

if [[ -z "$backup_archive" ]]; then
  backup=failed
  exit 1
fi

if ! run_step restore "$ROOT_DIR/scripts/db-restore.sh" "$ENVIRONMENT" "$backup_archive" --restore-runtime-files; then
  exit 1
fi

if ! run_step fresh_start "$ROOT_DIR/scripts/db-verify.sh" "$ENVIRONMENT" all; then
  exit 1
fi

if ! run_step restore_smoke "$ROOT_DIR/scripts/compose-smoke-test.sh" "$ENVIRONMENT" --keep-running; then
  exit 1
fi

status=GO
