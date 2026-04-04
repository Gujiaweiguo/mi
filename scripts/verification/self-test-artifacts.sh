#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)
COMPARE="$ROOT_DIR/scripts/verification/compare-artifacts.sh"
TESTDATA_DIR="$ROOT_DIR/scripts/verification/testdata-artifacts"

"$COMPARE" "$TESTDATA_DIR/expected.txt" "$TESTDATA_DIR/actual-match.txt"

if "$COMPARE" "$TESTDATA_DIR/expected.txt" "$TESTDATA_DIR/actual-mismatch.txt"; then
  echo "expected mismatch comparison to fail" >&2
  exit 1
fi

echo "artifact comparison self-tests passed"
