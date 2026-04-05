#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMMIT_SHA=${1:-$(git -C "$ROOT_DIR" rev-parse HEAD)}
STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SOURCE_KIND=${SOURCE_KIND:-${GITHUB_ACTIONS:+github-actions}}
SOURCE_KIND=${SOURCE_KIND:-local}
CHANGE_NAME=${CHANGE_NAME:-legacy-system-migration}

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

(cd "$ROOT_DIR/frontend" && npm run test:e2e)

FINISHED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

python3 "$ROOT_DIR/scripts/verification/write-evidence.py" \
  --root "$ROOT_DIR" \
  --commit-sha "$COMMIT_SHA" \
  --change "$CHANGE_NAME" \
  --test-type e2e \
  --status passed \
  --source-kind "$SOURCE_KIND" \
  --workflow "${GITHUB_WORKFLOW:-e2e}" \
  --run-id "${GITHUB_RUN_ID:-local}" \
  --started-at "$STARTED_AT" \
  --finished-at "$FINISHED_AT" \
  --total 1 \
  --passed 1 \
  --failed 0 \
  --skipped 0 \
  --artifact "frontend/playwright-report"
