#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
ROOT_DIR=$(cd "$SCRIPT_DIR/../.." && pwd)
PROVE_TMPDIR=$(mktemp -d)
trap 'rm -rf "$PROVE_TMPDIR"' EXIT

FAKE_SHA="deadbeefdeadbeefdeadbeefdeadbeefdeadbeef"
REAL_GO=$(command -v go)
REAL_NPM=$(command -v npm)
FAKE_BIN="$PROVE_TMPDIR/bin"
mkdir -p "$FAKE_BIN"

evidence_status() {
  python3 - "$1" <<'PY'
import json, sys
print(json.loads(open(sys.argv[1]).read())["status"])
PY
}

make_fake_go() {
  local want_exit="$1"
  local action="pass"
  if [[ "$want_exit" -ne 0 ]]; then action="fail"; fi
  cat > "$FAKE_BIN/go" <<GOEOF
#!/usr/bin/env bash
if [[ "\$*" == *"test"* && "\$*" == *"-json"* ]]; then
  echo '{"Time":"2026-01-01T00:00:00Z","Action":"${action}","Test":"TestFake","Package":"fake"}'
  exit ${want_exit}
fi
exec ${REAL_GO} "\$@"
GOEOF
  chmod +x "$FAKE_BIN/go"
}

make_fake_npm() {
  local want_exit="$1"
  local passed=1 failed=0
  if [[ "$want_exit" -ne 0 ]]; then passed=0; failed=1; fi
  cat > "$FAKE_BIN/npm" <<NPMEOF
#!/usr/bin/env bash
if [[ "\$*" == *"test:unit"* ]]; then
  output_file=""
  for arg in "\$@"; do
    if [[ "\$arg" == --outputFile=* ]]; then
      output_file="\${arg#--outputFile=}"
    fi
  done
  if [[ -n "\$output_file" ]]; then
    printf '{"numTotalTests":1,"numPassedTests":${passed},"numFailedTests":${failed},"numPendingTests":0,"numTodoTests":0}' > "\$output_file"
  fi
  exit ${want_exit}
fi
exec ${REAL_NPM} "\$@"
NPMEOF
  chmod +x "$FAKE_BIN/npm"
}

cleanup_evidence() {
  rm -rf "$ROOT_DIR/artifacts/verification/$FAKE_SHA"
}

echo "=== Producer self-test ==="
echo

echo "[TEST] run-unit.sh writes failed evidence on backend failure"
make_fake_go 1
make_fake_npm 0
cleanup_evidence
PATH="$FAKE_BIN:$PATH" bash "$SCRIPT_DIR/run-unit.sh" "$FAKE_SHA" && exit_code=0 || exit_code=$?
status=$(evidence_status "$ROOT_DIR/artifacts/verification/$FAKE_SHA/unit.json")
[[ "$status" == "failed" ]] || { echo "FAIL: expected status=failed, got=$status"; exit 1; }
[[ "$exit_code" -ne 0 ]] || { echo "FAIL: should exit non-zero"; exit 1; }
echo "  ✓ status=$status exit=$exit_code"
cleanup_evidence

echo "[TEST] run-unit.sh writes failed evidence on frontend failure"
make_fake_go 0
make_fake_npm 1
cleanup_evidence
PATH="$FAKE_BIN:$PATH" bash "$SCRIPT_DIR/run-unit.sh" "$FAKE_SHA" && exit_code=0 || exit_code=$?
status=$(evidence_status "$ROOT_DIR/artifacts/verification/$FAKE_SHA/unit.json")
[[ "$status" == "failed" ]] || { echo "FAIL: expected status=failed, got=$status"; exit 1; }
[[ "$exit_code" -ne 0 ]] || { echo "FAIL: should exit non-zero"; exit 1; }
echo "  ✓ status=$status exit=$exit_code"
cleanup_evidence

echo "[TEST] run-unit.sh writes passed evidence when all pass"
make_fake_go 0
make_fake_npm 0
cleanup_evidence
PATH="$FAKE_BIN:$PATH" bash "$SCRIPT_DIR/run-unit.sh" "$FAKE_SHA" && exit_code=0 || exit_code=$?
status=$(evidence_status "$ROOT_DIR/artifacts/verification/$FAKE_SHA/unit.json")
[[ "$status" == "passed" ]] || { echo "FAIL: expected status=passed, got=$status"; exit 1; }
[[ "$exit_code" -eq 0 ]] || { echo "FAIL: should exit zero"; exit 1; }
echo "  ✓ status=$status exit=$exit_code"
cleanup_evidence

echo "[TEST] run-integration.sh writes failed evidence on failure"
make_fake_go 1
cleanup_evidence
PATH="$FAKE_BIN:$PATH" bash "$SCRIPT_DIR/run-integration.sh" "$FAKE_SHA" && exit_code=0 || exit_code=$?
status=$(evidence_status "$ROOT_DIR/artifacts/verification/$FAKE_SHA/integration.json")
[[ "$status" == "failed" ]] || { echo "FAIL: expected status=failed, got=$status"; exit 1; }
[[ "$exit_code" -ne 0 ]] || { echo "FAIL: should exit non-zero"; exit 1; }
echo "  ✓ status=$status exit=$exit_code"
cleanup_evidence

echo "[TEST] run-integration.sh writes passed evidence when all pass"
make_fake_go 0
cleanup_evidence
PATH="$FAKE_BIN:$PATH" bash "$SCRIPT_DIR/run-integration.sh" "$FAKE_SHA" && exit_code=0 || exit_code=$?
status=$(evidence_status "$ROOT_DIR/artifacts/verification/$FAKE_SHA/integration.json")
[[ "$status" == "passed" ]] || { echo "FAIL: expected status=passed, got=$status"; exit 1; }
[[ "$exit_code" -eq 0 ]] || { echo "FAIL: should exit zero"; exit 1; }
echo "  ✓ status=$status exit=$exit_code"
cleanup_evidence

echo
echo "=== All producer self-tests passed. ==="
