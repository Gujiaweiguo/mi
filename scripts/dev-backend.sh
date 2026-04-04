#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
cd "$ROOT_DIR/backend"

export MI_CONFIG_FILE=${MI_CONFIG_FILE:-config/development.yaml}
go run ./cmd/server
