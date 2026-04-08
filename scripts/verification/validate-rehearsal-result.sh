#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: validate-rehearsal-result.sh <result-file> [--commit-sha <sha>]

Validates a cutover rehearsal result artifact.
EOF
}

if [[ $# -lt 1 ]]; then
  usage
  exit 2
fi

RESULT_FILE=$1
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

if [[ ! -f "$RESULT_FILE" ]]; then
  echo "Result file not found: $RESULT_FILE" >&2
  exit 2
fi

python3 - "$RESULT_FILE" "$COMMIT_SHA" <<'PY'
import json
import sys
from pathlib import Path

result_path = Path(sys.argv[1])
expected_sha = sys.argv[2]
required = [
    "schema_version",
    "change",
    "commit_sha",
    "environment",
    "started_at",
    "finished_at",
    "status",
    "checks",
    "artifacts",
]
required_checks = [
    "preflight",
    "archive_ready",
    "bootstrap",
    "fresh_start",
    "smoke",
    "backup",
    "restore",
    "restore_smoke",
]
required_artifacts = ["log", "backup_archive", "verification", "checklist"]

try:
    data = json.loads(result_path.read_text(encoding="utf-8"))
except Exception as exc:
    raise SystemExit(f"malformed rehearsal result: {result_path}: {exc}")

missing = [field for field in required if field not in data]
if missing:
    raise SystemExit(f"malformed rehearsal result: missing fields {', '.join(missing)}")

if data["status"] not in {"GO", "NO-GO"}:
    raise SystemExit(f"malformed rehearsal result: invalid status {data['status']}")

if expected_sha and data["commit_sha"] != expected_sha:
    raise SystemExit(
        f"stale rehearsal result: commit_sha {data['commit_sha']} does not match {expected_sha}"
    )

if not isinstance(data["checks"], dict):
    raise SystemExit("malformed rehearsal result: checks must be an object")
for key in required_checks:
    if key not in data["checks"]:
        raise SystemExit(f"malformed rehearsal result: checks missing {key}")

if not isinstance(data.get("artifacts"), dict):
    raise SystemExit("malformed rehearsal result: artifacts must be an object")
for key in required_artifacts:
    if key not in data["artifacts"]:
        raise SystemExit(f"malformed rehearsal result: artifacts missing {key}")

verification = data["artifacts"]["verification"]
if not isinstance(verification, list) or len(verification) != 3:
    raise SystemExit("malformed rehearsal result: verification must list unit/integration/e2e evidence")

if data["status"] == "GO":
    for key in required_checks:
      if data["checks"][key] != "passed":
        raise SystemExit(f"invalid GO rehearsal result: {key}={data['checks'][key]}")

if data["status"] == "NO-GO" and not data.get("failing_gate"):
    raise SystemExit("invalid NO-GO rehearsal result: failing_gate is required")

print(f"passed rehearsal result: {result_path}")
PY
