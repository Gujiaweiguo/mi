#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMMIT_SHA=${1:-$(git -C "$ROOT_DIR" rev-parse HEAD)}
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SOURCE_KIND=${SOURCE_KIND:-live-stack}
CHANGE_NAME=${CHANGE_NAME:-legacy-system-migration}
PLAYWRIGHT_JSON_OUTPUT_NAME=${PLAYWRIGHT_JSON_OUTPUT_NAME:-test-results/e2e-live-results.json}
PLAYWRIGHT_JSON_REL=${PLAYWRIGHT_JSON_OUTPUT_NAME#/}
PLAYWRIGHT_JSON_FILE="$ROOT_DIR/frontend/$PLAYWRIGHT_JSON_REL"
ENVIRONMENT=${ENVIRONMENT:-production}
ENV_FILE="$ROOT_DIR/deploy/env/$ENVIRONMENT.env"
COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"

export MI_HTTP_PORT=${MI_HTTP_PORT:-18080}
export MI_MYSQL_PORT=${MI_MYSQL_PORT:-33306}
OVERRIDE_MI_HTTP_PORT=$MI_HTTP_PORT
OVERRIDE_MI_MYSQL_PORT=$MI_MYSQL_PORT
TEMP_RUNTIME_ROOT=$(mktemp -d)
TEMP_ENV_FILE=$(mktemp)
TEMP_DBOPS_CONFIG_FILE=$(mktemp --suffix=.yaml)

export MI_RUNTIME_BASE="$TEMP_RUNTIME_ROOT"
export MI_RUNTIME_LOGS="$TEMP_RUNTIME_ROOT/logs"
export MI_RUNTIME_DOCUMENTS="$TEMP_RUNTIME_ROOT/documents"
export MI_RUNTIME_UPLOADS="$TEMP_RUNTIME_ROOT/uploads"
export MI_RUNTIME_MYSQL="$TEMP_RUNTIME_ROOT/mysql"
export MI_COMPOSE_ENV_FILE="$TEMP_ENV_FILE"
export MI_DBOPS_CONFIG_FILE="$TEMP_DBOPS_CONFIG_FILE"

mkdir -p "$MI_RUNTIME_LOGS" "$MI_RUNTIME_DOCUMENTS" "$MI_RUNTIME_UPLOADS" "$MI_RUNTIME_MYSQL"

cp "$ENV_FILE" "$TEMP_ENV_FILE"
cat <<EOF >> "$TEMP_ENV_FILE"
MI_HTTP_PORT=$MI_HTTP_PORT
MI_MYSQL_PORT=$MI_MYSQL_PORT
MI_RUNTIME_BASE=$MI_RUNTIME_BASE
MI_RUNTIME_LOGS=$MI_RUNTIME_LOGS
MI_RUNTIME_DOCUMENTS=$MI_RUNTIME_DOCUMENTS
MI_RUNTIME_UPLOADS=$MI_RUNTIME_UPLOADS
MI_RUNTIME_MYSQL=$MI_RUNTIME_MYSQL
EOF

python3 - <<'PY'
from pathlib import Path
import os
import re

source = Path('/opt/code/mi/backend/config/production.yaml').read_text(encoding='utf-8')
source = re.sub(
    r"database:\n  host: mysql\n  port: 3306\n",
    f"database:\n  host: 127.0.0.1\n  port: {os.environ['MI_MYSQL_PORT']}\n",
    source,
    count=1,
)
Path(os.environ['MI_DBOPS_CONFIG_FILE']).write_text(source, encoding='utf-8')
PY

if [[ -z "${PLAYWRIGHT_EXECUTABLE_PATH:-}" ]]; then
  if [[ -x "/usr/bin/google-chrome" ]]; then
    export PLAYWRIGHT_EXECUTABLE_PATH="/usr/bin/google-chrome"
  elif [[ -x "/usr/bin/google-chrome-stable" ]]; then
    export PLAYWRIGHT_EXECUTABLE_PATH="/usr/bin/google-chrome-stable"
  elif [[ -x "/usr/bin/chromium" ]]; then
    export PLAYWRIGHT_EXECUTABLE_PATH="/usr/bin/chromium"
  elif [[ -x "/usr/bin/chromium-browser" ]]; then
    export PLAYWRIGHT_EXECUTABLE_PATH="/usr/bin/chromium-browser"
  fi
fi

source "$MI_COMPOSE_ENV_FILE"
export PLAYWRIGHT_BASE_URL="http://127.0.0.1:${MI_HTTP_PORT:-80}"

teardown() {
  docker compose --env-file "$MI_COMPOSE_ENV_FILE" -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker run --rm -v "$TEMP_RUNTIME_ROOT:/runtime" alpine:3.20 sh -lc 'rm -rf /runtime/* /runtime/.[!.]* /runtime/..?* 2>/dev/null || true' >/dev/null 2>&1 || true
  rm -rf "$TEMP_RUNTIME_ROOT"
  rm -f "$TEMP_ENV_FILE"
  rm -f "$TEMP_DBOPS_CONFIG_FILE"
}
trap teardown EXIT

"$ROOT_DIR/scripts/db-bootstrap.sh" "$ENVIRONMENT" cutover
"$ROOT_DIR/scripts/compose-smoke-test.sh" "$ENVIRONMENT" --build --keep-running

set +e
(
  cd "$ROOT_DIR/frontend"
  PLAYWRIGHT_JSON_OUTPUT_NAME="$PLAYWRIGHT_JSON_OUTPUT_NAME" npm run test:e2e -- --config=playwright.live.config.ts --reporter=json,list
)
PLAYWRIGHT_EXIT_CODE=$?
set -e

FINISHED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

read -r TOTAL PASSED FAILED SKIPPED <<EOF
$(python3 - "$PLAYWRIGHT_JSON_FILE" <<'PY'
import json
import sys
from pathlib import Path

report_path = Path(sys.argv[1])

if not report_path.exists():
    print("0 0 0 0")
    raise SystemExit(0)

try:
    report = json.loads(report_path.read_text(encoding="utf-8"))
except Exception:
    print("0 0 0 0")
    raise SystemExit(0)

stats = report.get("stats")
if not isinstance(stats, dict):
    print("0 0 0 0")
    raise SystemExit(0)

passed = int(stats.get("expected", 0) or 0)
failed = int(stats.get("unexpected", 0) or 0)
skipped = int(stats.get("skipped", 0) or 0)
flaky = int(stats.get("flaky", 0) or 0)
total = passed + failed + skipped + flaky

print(f"{total} {passed} {failed} {skipped}")
PY
)
EOF

E2E_STATUS=passed
if [[ "$PLAYWRIGHT_EXIT_CODE" -ne 0 ]]; then
  E2E_STATUS=failed
fi

python3 "$ROOT_DIR/scripts/verification/write-evidence.py" \
  --root "$ROOT_DIR" \
  --commit-sha "$COMMIT_SHA" \
  --change "$CHANGE_NAME" \
  --test-type e2e \
  --status "$E2E_STATUS" \
  --source-kind "$SOURCE_KIND" \
  --workflow "${GITHUB_WORKFLOW:-e2e-live}" \
  --run-id "${GITHUB_RUN_ID:-local}" \
  --started-at "$STARTED_AT" \
  --finished-at "$FINISHED_AT" \
  --total "$TOTAL" \
  --passed "$PASSED" \
  --failed "$FAILED" \
  --skipped "$SKIPPED" \
  --artifact "frontend/$PLAYWRIGHT_JSON_REL"

if [[ "$PLAYWRIGHT_EXIT_CODE" -ne 0 ]]; then
  exit "$PLAYWRIGHT_EXIT_CODE"
fi
