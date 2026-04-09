#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: validate-gate.sh <ci|archive> [--root <path>] [--commit-sha <sha>]

Validates commit-scoped test evidence under:
  artifacts/verification/<commit-sha>/

Gates:
  ci       requires unit + integration evidence
  archive  requires unit + integration + e2e evidence
EOF
}

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
DEFAULT_ROOT=$(cd "${SCRIPT_DIR}/../.." && pwd)

if [[ $# -lt 1 ]]; then
  usage
  exit 2
fi

GATE="$1"
shift

ROOT="$DEFAULT_ROOT"
COMMIT_SHA=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --root)
      ROOT="$2"
      shift 2
      ;;
    --commit-sha)
      COMMIT_SHA="$2"
      shift 2
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown argument: $1" >&2
      usage
      exit 2
      ;;
  esac
done

if [[ "$GATE" != "ci" && "$GATE" != "archive" ]]; then
  echo "Unsupported gate: $GATE" >&2
  usage
  exit 2
fi

if [[ -z "$COMMIT_SHA" ]]; then
  COMMIT_SHA=$(git -C "$ROOT" rev-parse HEAD 2>/dev/null || true)
fi

if [[ -z "$COMMIT_SHA" ]]; then
  echo "Unable to determine commit SHA. Pass --commit-sha explicitly." >&2
  exit 2
fi

EVIDENCE_DIR="$ROOT/artifacts/verification/$COMMIT_SHA"

if [[ "$GATE" == "ci" ]]; then
  REQUIRED_TYPES=(unit integration)
  GATE_LABEL="CI Ready"
else
  REQUIRED_TYPES=(unit integration e2e)
  GATE_LABEL="Archive Ready"
fi

declare -A STATUS=()
declare -A MESSAGE=()

validate_evidence() {
  local evidence_type="$1"
  local evidence_file="$2"

  if [[ ! -f "$evidence_file" ]]; then
    STATUS["$evidence_type"]="FAIL"
    MESSAGE["$evidence_type"]="missing evidence: ${evidence_file#$ROOT/}"
    return 1
  fi

  local output
  if ! output=$(python3 - "$evidence_file" "$evidence_type" "$COMMIT_SHA" <<'PY'
import json
import sys
from datetime import datetime, timezone
from pathlib import Path

path, expected_type, expected_sha = sys.argv[1:4]
required = [
    "schema_version",
    "project",
    "change",
    "commit_sha",
    "test_type",
    "status",
    "started_at",
    "finished_at",
    "source",
    "stats",
]

try:
    data = json.loads(Path(path).read_text(encoding="utf-8"))
except Exception as exc:
    print(f"malformed evidence: {path}: {exc}")
    sys.exit(1)

missing = [field for field in required if field not in data]
if missing:
    print(f"malformed evidence: {path}: missing fields {', '.join(missing)}")
    sys.exit(1)

if data["schema_version"] != "1":
    print(f"malformed evidence: {path}: schema_version must be '1'")
    sys.exit(1)

if not isinstance(data["source"], dict):
    print(f"malformed evidence: {path}: source must be an object")
    sys.exit(1)

for field in ("kind", "workflow", "run_id"):
    value = data["source"].get(field)
    if not isinstance(value, str) or not value.strip():
        print(f"malformed evidence: {path}: source.{field} must be a non-empty string")
        sys.exit(1)

if not isinstance(data["stats"], dict):
    print(f"malformed evidence: {path}: stats must be an object")
    sys.exit(1)

for field in ("total", "passed", "failed", "skipped"):
    value = data["stats"].get(field)
    if not isinstance(value, int) or value < 0:
        print(f"malformed evidence: {path}: stats.{field} must be a non-negative integer")
        sys.exit(1)

if data["stats"]["passed"] + data["stats"]["failed"] + data["stats"]["skipped"] > data["stats"]["total"]:
    print(f"malformed evidence: {path}: stats are inconsistent")
    sys.exit(1)

def parse_utc_timestamp(value):
    try:
        return datetime.strptime(value, "%Y-%m-%dT%H:%M:%SZ").replace(tzinfo=timezone.utc)
    except Exception:
        return None

started_at = parse_utc_timestamp(data["started_at"])
finished_at = parse_utc_timestamp(data["finished_at"])
if started_at is None or finished_at is None:
    print(f"malformed evidence: {path}: timestamps must be UTC ISO 8601 (YYYY-MM-DDTHH:MM:SSZ)")
    sys.exit(1)

if started_at > finished_at:
    print(f"malformed evidence: {path}: started_at must be <= finished_at")
    sys.exit(1)

artifacts = data.get("artifacts")
if artifacts is not None:
    if not isinstance(artifacts, list):
        print(f"malformed evidence: {path}: artifacts must be an array when present")
        sys.exit(1)
    for artifact in artifacts:
        if not isinstance(artifact, str) or not artifact.strip():
            print(f"malformed evidence: {path}: artifacts entries must be non-empty strings")
            sys.exit(1)

if expected_type == "e2e":
    if not isinstance(artifacts, list) or not artifacts:
        print(f"malformed evidence: {path}: artifacts must be a non-empty array for e2e evidence")
        sys.exit(1)

if data["commit_sha"] != expected_sha:
    print(f"stale evidence: {path}: commit_sha {data['commit_sha']} does not match {expected_sha}")
    sys.exit(1)

if data["test_type"] != expected_type:
    print(f"malformed evidence: {path}: test_type {data['test_type']} does not match {expected_type}")
    sys.exit(1)

if data["status"] != "passed":
    print(f"failed evidence: {path}: status={data['status']}")
    sys.exit(1)

print(f"passed evidence: {path}")
PY
); then
    STATUS["$evidence_type"]="FAIL"
    MESSAGE["$evidence_type"]="$output"
    return 1
  fi

  STATUS["$evidence_type"]="PASS"
  MESSAGE["$evidence_type"]="$output"
  return 0
}

echo "Gate: $GATE_LABEL"
echo "Commit: $COMMIT_SHA"
echo "Evidence root: ${EVIDENCE_DIR#$ROOT/}"

all_passed=true
for evidence_type in "${REQUIRED_TYPES[@]}"; do
  evidence_file="$EVIDENCE_DIR/$evidence_type.json"
  if ! validate_evidence "$evidence_type" "$evidence_file"; then
    all_passed=false
  fi
done

echo
for evidence_type in "${REQUIRED_TYPES[@]}"; do
  echo "- $evidence_type: ${STATUS[$evidence_type]} — ${MESSAGE[$evidence_type]}"
done

echo
if [[ "$all_passed" == true ]]; then
  echo "$GATE_LABEL: YES"
  exit 0
fi

echo "$GATE_LABEL: NO"
exit 1
