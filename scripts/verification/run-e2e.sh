#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMMIT_SHA=${1:-$(git -C "$ROOT_DIR" rev-parse HEAD)}
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SOURCE_KIND=${SOURCE_KIND:-${GITHUB_ACTIONS:+github-actions}}
SOURCE_KIND=${SOURCE_KIND:-local}
CHANGE_NAME=${CHANGE_NAME:-legacy-system-migration}
PLAYWRIGHT_JSON_OUTPUT_NAME=${PLAYWRIGHT_JSON_OUTPUT_NAME:-test-results/e2e-results.json}
PLAYWRIGHT_JSON_REL=${PLAYWRIGHT_JSON_OUTPUT_NAME#/}
PLAYWRIGHT_JSON_FILE="$ROOT_DIR/frontend/$PLAYWRIGHT_JSON_REL"

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

set +e
(cd "$ROOT_DIR/frontend" && PLAYWRIGHT_JSON_OUTPUT_NAME="$PLAYWRIGHT_JSON_OUTPUT_NAME" npm run test:e2e -- --reporter=json,list)
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
  --workflow "${GITHUB_WORKFLOW:-e2e}" \
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
