#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMMIT_SHA=${1:-$(git -C "$ROOT_DIR" rev-parse HEAD)}
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SOURCE_KIND=${SOURCE_KIND:-${GITHUB_ACTIONS:+github-actions}}
SOURCE_KIND=${SOURCE_KIND:-local}
CHANGE_NAME=${CHANGE_NAME:-legacy-system-migration}
BACKEND_REPORT=${BACKEND_REPORT:-/tmp/mi-go-unit.jsonl}
FRONTEND_REPORT=${FRONTEND_REPORT:-/tmp/mi-vitest-unit.json}

set +e
(cd "$ROOT_DIR/backend" && go test -json ./... | tee "$BACKEND_REPORT" >/dev/null)
BACKEND_EXIT_CODE=$?
(cd "$ROOT_DIR/frontend" && npm run test:unit -- --reporter=json --outputFile="$FRONTEND_REPORT")
FRONTEND_EXIT_CODE=$?
set -e

FINISHED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

BACKEND_COUNTS=$(python3 "$ROOT_DIR/scripts/verification/collect-test-stats.py" go-jsonl "$BACKEND_REPORT" 2>/dev/null || echo '{"total":0,"passed":0,"failed":0,"skipped":0}')
FRONTEND_COUNTS=$(python3 "$ROOT_DIR/scripts/verification/collect-test-stats.py" vitest-json "$FRONTEND_REPORT" 2>/dev/null || echo '{"total":0,"passed":0,"failed":0,"skipped":0}')

read -r TOTAL PASSED FAILED SKIPPED <<EOF
$(python3 - "$BACKEND_COUNTS" "$FRONTEND_COUNTS" <<'PY'
import json
import sys

backend = json.loads(sys.argv[1])
frontend = json.loads(sys.argv[2])

print(
    backend["total"] + frontend["total"],
    backend["passed"] + frontend["passed"],
    backend["failed"] + frontend["failed"],
    backend["skipped"] + frontend["skipped"],
)
PY
)
EOF

UNIT_STATUS=passed
if [[ "$BACKEND_EXIT_CODE" -ne 0 ]] || [[ "$FRONTEND_EXIT_CODE" -ne 0 ]]; then
  UNIT_STATUS=failed
fi

python3 "$ROOT_DIR/scripts/verification/write-evidence.py" \
  --root "$ROOT_DIR" \
  --commit-sha "$COMMIT_SHA" \
  --change "$CHANGE_NAME" \
  --test-type unit \
  --status "$UNIT_STATUS" \
  --source-kind "$SOURCE_KIND" \
  --workflow "${GITHUB_WORKFLOW:-unit}" \
  --run-id "${GITHUB_RUN_ID:-local}" \
  --started-at "$STARTED_AT" \
  --finished-at "$FINISHED_AT" \
  --total "$TOTAL" \
  --passed "$PASSED" \
  --failed "$FAILED" \
  --skipped "$SKIPPED"

if [[ "$BACKEND_EXIT_CODE" -ne 0 ]] || [[ "$FRONTEND_EXIT_CODE" -ne 0 ]]; then
  exit 1
fi
