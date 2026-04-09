#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
PARSER="$SCRIPT_DIR/collect-test-stats.py"
TESTDATA_DIR="$SCRIPT_DIR/testdata-stats"

expect_json() {
  local name="$1"
  local actual="$2"
  local expected="$3"
  echo "[EXPECT] $name"
  if [[ "$actual" != "$expected" ]]; then
    echo "Expected $expected but got $actual for $name" >&2
    exit 1
  fi
}

GO_COUNTS=$(python3 "$PARSER" go-jsonl "$TESTDATA_DIR/go-unit-sample.jsonl")
VITEST_COUNTS=$(python3 "$PARSER" vitest-json "$TESTDATA_DIR/vitest-unit-sample.json")

expect_json "go unit sample counts" "$GO_COUNTS" '{"total": 3, "passed": 1, "failed": 1, "skipped": 1}'
expect_json "vitest sample counts" "$VITEST_COUNTS" '{"total": 5, "passed": 4, "failed": 1, "skipped": 0}'

echo
echo "All stats self-tests passed."
