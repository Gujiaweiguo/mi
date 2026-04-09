#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
bash "$SCRIPT_DIR/verification/self-test-schema.sh"
exec "$SCRIPT_DIR/verification/validate-gate.sh" ci "$@"
