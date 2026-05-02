#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMMIT_SHA=${1:-$(git -C "$ROOT_DIR" rev-parse HEAD)}
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SOURCE_KIND=${SOURCE_KIND:-${GITHUB_ACTIONS:+github-actions}}
SOURCE_KIND=${SOURCE_KIND:-local}
CHANGE_NAME=${CHANGE_NAME:-legacy-system-migration}
INTEGRATION_REPORT=${INTEGRATION_REPORT:-/tmp/mi-go-integration.jsonl}
GO_TEST_PARALLEL_PACKAGES=${GO_TEST_PARALLEL_PACKAGES:-4}
GO_TEST_TIMEOUT=${GO_TEST_TIMEOUT:-20m}

set +e
(cd "$ROOT_DIR/backend" && go test -p "$GO_TEST_PARALLEL_PACKAGES" -timeout "$GO_TEST_TIMEOUT" -json -tags=integration ./... | tee "$INTEGRATION_REPORT" >/dev/null)
INTEGRATION_EXIT_CODE=$?
set -e

FINISHED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

COUNTS=$(python3 "$ROOT_DIR/scripts/verification/collect-test-stats.py" go-jsonl "$INTEGRATION_REPORT" 2>/dev/null || echo '{"total":0,"passed":0,"failed":0,"skipped":0}')

read -r TOTAL PASSED FAILED SKIPPED <<EOF
$(python3 - "$COUNTS" <<'PY'
import json
import sys

counts = json.loads(sys.argv[1])
print(counts["total"], counts["passed"], counts["failed"], counts["skipped"])
PY
)
EOF

INTEGRATION_STATUS=passed
if [[ "$INTEGRATION_EXIT_CODE" -ne 0 ]]; then
  INTEGRATION_STATUS=failed
fi

python3 "$ROOT_DIR/scripts/verification/write-evidence.py" \
  --root "$ROOT_DIR" \
  --commit-sha "$COMMIT_SHA" \
  --change "$CHANGE_NAME" \
  --test-type integration \
  --status "$INTEGRATION_STATUS" \
  --source-kind "$SOURCE_KIND" \
  --workflow "${GITHUB_WORKFLOW:-integration}" \
  --run-id "${GITHUB_RUN_ID:-local}" \
  --started-at "$STARTED_AT" \
  --finished-at "$FINISHED_AT" \
  --total "$TOTAL" \
  --passed "$PASSED" \
  --failed "$FAILED" \
  --skipped "$SKIPPED"

if [[ "$INTEGRATION_EXIT_CODE" -ne 0 ]]; then
  exit "$INTEGRATION_EXIT_CODE"
fi
