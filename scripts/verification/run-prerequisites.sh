#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)

echo "[prerequisite] backend static analysis"
(cd "$ROOT_DIR/backend" && go vet ./...)

echo "[prerequisite] backend lint"
(cd "$ROOT_DIR/backend" && golangci-lint run ./...)

echo "[prerequisite] frontend typecheck"
(cd "$ROOT_DIR/frontend" && npm run typecheck)

echo "[prerequisite] frontend build"
(cd "$ROOT_DIR/frontend" && npm run build)

echo "[prerequisite] frontend audit"
(cd "$ROOT_DIR/frontend" && npm audit --omit=dev --audit-level=high --registry=https://registry.npmjs.org)
