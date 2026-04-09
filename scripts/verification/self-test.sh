#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
VALIDATOR="$SCRIPT_DIR/validate-gate.sh"
TESTDATA_DIR="$SCRIPT_DIR/testdata"

expect_success() {
  local name="$1"
  shift
  echo "[PASS-EXPECTED] $name"
  "$@"
}

expect_failure() {
  local name="$1"
  shift
  echo "[FAIL-EXPECTED] $name"
  if "$@"; then
    echo "Expected failure but command passed: $name" >&2
    exit 1
  fi
}

expect_success \
  "ci gate passes with unit+integration evidence" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/pass-ci" --commit-sha 1111111111111111111111111111111111111111

expect_success \
  "archive gate passes with unit+integration+e2e evidence" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/pass-archive" --commit-sha 2222222222222222222222222222222222222222

expect_failure \
  "ci gate fails when integration evidence is missing" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/missing-integration" --commit-sha 3333333333333333333333333333333333333333

expect_failure \
  "ci gate fails when evidence commit sha is stale" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/stale-sha" --commit-sha 4444444444444444444444444444444444444444

expect_failure \
  "archive gate fails on malformed evidence" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/malformed-json" --commit-sha 5555555555555555555555555555555555555555

expect_failure \
  "archive gate fails on failed e2e status" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/failed-status" --commit-sha 6666666666666666666666666666666666666666

expect_failure \
  "ci gate fails on test_type mismatch" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/test-type-mismatch" --commit-sha 7777777777777777777777777777777777777777

expect_failure \
  "archive gate fails when e2e stats fields are missing" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/missing-stats-fields" --commit-sha 8888888888888888888888888888888888888888

expect_failure \
  "archive gate fails when e2e stats are inconsistent" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/stats-impossible" --commit-sha 9999999999999999999999999999999999999999

expect_failure \
  "archive gate fails when e2e artifacts field is missing" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/missing-artifacts-e2e" --commit-sha aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

expect_failure \
  "archive gate fails when timestamps are reversed" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/timestamp-reversed" --commit-sha bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb

expect_failure \
  "ci gate fails when schema_version type drifts" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/schema-version-type" --commit-sha cccccccccccccccccccccccccccccccccccccccc

expect_failure \
  "ci gate fails when source is not an object" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/source-not-object" --commit-sha dddddddddddddddddddddddddddddddddddddddd

expect_failure \
  "archive gate fails when stats is not an object" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/stats-not-object" --commit-sha eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee

expect_failure \
  "archive gate fails when e2e artifacts is empty array" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/artifacts-empty-array" --commit-sha ffffffffffffffffffffffffffffffffffffffff

expect_failure \
  "ci gate fails when unit status is failed" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/unit-failed-status" --commit-sha 1212121212121212121212121212121212121212

expect_failure \
  "archive gate fails on invalid timestamp format" \
  "$VALIDATOR" archive --root "$TESTDATA_DIR/timestamp-bad-format" --commit-sha abababababababababababababababababababab

expect_failure \
  "ci gate fails when project is not a string" \
  "$VALIDATOR" ci --root "$TESTDATA_DIR/project-not-string" --commit-sha cdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcd

echo
echo "All verification self-tests passed."
