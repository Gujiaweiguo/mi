#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMMIT_SHA=${1:-$(git -C "$ROOT_DIR" rev-parse HEAD)}
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SOURCE_KIND=${SOURCE_KIND:-${GITHUB_ACTIONS:+github-actions}}
SOURCE_KIND=${SOURCE_KIND:-local}
CHANGE_NAME=${CHANGE_NAME:-legacy-system-migration}

(cd "$ROOT_DIR/backend" && go test ./...)
(cd "$ROOT_DIR/frontend" && npm run test:unit)

FINISHED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

python3 "$ROOT_DIR/scripts/verification/write-evidence.py" \
  --root "$ROOT_DIR" \
  --commit-sha "$COMMIT_SHA" \
  --change "$CHANGE_NAME" \
  --test-type unit \
  --status passed \
  --source-kind "$SOURCE_KIND" \
  --workflow "${GITHUB_WORKFLOW:-unit}" \
  --run-id "${GITHUB_RUN_ID:-local}" \
  --started-at "$STARTED_AT" \
  --finished-at "$FINISHED_AT" \
  --total 2 \
  --passed 2 \
  --failed 0 \
  --skipped 0
