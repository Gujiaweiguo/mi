#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: validate-release-readiness.sh <summary-file> [--commit-sha <sha>]

Validates a release-readiness summary artifact.
EOF
}

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
ROOT_DIR=$(cd "$SCRIPT_DIR/../.." && pwd)
SCHEMA_PATH="$ROOT_DIR/schemas/release-readiness-v1.json"
STRUCTURE_VALIDATOR="$SCRIPT_DIR/validate-evidence-structure.py"

if [[ $# -lt 1 ]]; then
  usage
  exit 2
fi

SUMMARY_FILE=$1
shift
COMMIT_SHA=""

while [[ $# -gt 0 ]]; do
  case "$1" in
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

if [[ ! -f "$SUMMARY_FILE" ]]; then
  echo "Summary file not found: $SUMMARY_FILE" >&2
  exit 2
fi

python3 "$STRUCTURE_VALIDATOR" "$SCHEMA_PATH" "$SUMMARY_FILE" >/dev/null

python3 - "$SUMMARY_FILE" "$COMMIT_SHA" <<'PY'
import json
import sys
from datetime import datetime, timezone
from pathlib import Path

summary_path = Path(sys.argv[1])
expected_sha = sys.argv[2]
data = json.loads(summary_path.read_text(encoding="utf-8"))

def parse_utc_timestamp(value: str):
    try:
        return datetime.strptime(value, "%Y-%m-%dT%H:%M:%SZ").replace(tzinfo=timezone.utc)
    except Exception:
        return None

started_at = parse_utc_timestamp(data["started_at"])
finished_at = parse_utc_timestamp(data["finished_at"])
if started_at is None or finished_at is None:
    raise SystemExit(f"malformed release-readiness summary: {summary_path}: timestamps must be valid UTC ISO 8601")
if started_at > finished_at:
    raise SystemExit(f"malformed release-readiness summary: {summary_path}: started_at must be <= finished_at")

if expected_sha and data["commit_sha"] != expected_sha:
    raise SystemExit(
        f"stale release-readiness summary: commit_sha {data['commit_sha']} does not match {expected_sha}"
    )

for entry in data["blockers"]:
    if entry["classification"] != "must-fix":
        raise SystemExit(f"malformed release-readiness summary: blocker {entry['id']} must use classification=must-fix")

if data["status"] == "GO":
    if data["gates"]["archive_ready"] != "passed":
        raise SystemExit("invalid GO release-readiness summary: archive_ready must be passed")
    if data["gates"]["rehearsal_ready"] != "passed":
        raise SystemExit("invalid GO release-readiness summary: rehearsal_ready must be passed")
    if data["blockers"]:
        raise SystemExit("invalid GO release-readiness summary: blockers must be empty")
    if not data["artifacts"]["rehearsal_result"]:
        raise SystemExit("invalid GO release-readiness summary: rehearsal_result is required")
else:
    if not data["blockers"]:
        raise SystemExit("invalid NO-GO release-readiness summary: at least one blocker is required")

print(f"passed release-readiness summary: {summary_path}")
PY
