#!/usr/bin/env bash
set -uo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/release-ready.sh [--commit-sha <sha>] [--findings <path>] [--rehearsal-result <path>]

Validates final first-release readiness for the selected commit and writes a
machine-readable summary under artifacts/release-readiness/<commit-sha>/.

The workflow validates:
- archive-ready gate evidence for the current commit
- production cutover rehearsal result for the same commit
- must-fix and deferred findings classification
EOF
}

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
COMMIT_SHA=""
FINDINGS_FILE=""
REHEARSAL_FILE=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --commit-sha)
      COMMIT_SHA="$2"
      shift 2
      ;;
    --findings)
      FINDINGS_FILE="$2"
      shift 2
      ;;
    --rehearsal-result)
      REHEARSAL_FILE="$2"
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

if [[ -z "$COMMIT_SHA" ]]; then
  COMMIT_SHA=$(git -C "$ROOT_DIR" rev-parse HEAD 2>/dev/null || true)
fi

if [[ -z "$COMMIT_SHA" ]]; then
  echo "Unable to determine commit SHA. Pass --commit-sha explicitly." >&2
  exit 2
fi

if [[ -z "$FINDINGS_FILE" ]]; then
  FINDINGS_FILE="$ROOT_DIR/artifacts/release-readiness/$COMMIT_SHA/findings.json"
fi

if [[ -z "$REHEARSAL_FILE" ]]; then
  latest_rehearsal=$(python3 - "$ROOT_DIR" "$COMMIT_SHA" <<'PY'
from pathlib import Path
import sys
root = Path(sys.argv[1])
sha = sys.argv[2]
paths = sorted((root / "artifacts" / "rehearsal" / sha).glob("cutover-rehearsal-production-*.json"))
print(paths[-1] if paths else "")
PY
)
  REHEARSAL_FILE="$latest_rehearsal"
fi

RESULT_DIR="$ROOT_DIR/artifacts/release-readiness/$COMMIT_SHA"
RESULT_FILE="$RESULT_DIR/release-readiness-summary.json"
mkdir -p "$RESULT_DIR"

STARTED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

archive_ready=failed
rehearsal_ready=failed
status=NO-GO

declare -a BLOCKERS=()
declare -a DEFERRED=()

add_blocker() {
  BLOCKERS+=("$1")
}

add_deferred() {
  DEFERRED+=("$1")
}

if "$ROOT_DIR/scripts/archive-ready.sh" --commit-sha "$COMMIT_SHA" >/dev/null 2>&1; then
  archive_ready=passed
else
  add_blocker '{"id":"archive-gate","classification":"must-fix","source":"archive-ready","summary":"Current commit is missing passing archive-ready evidence."}'
fi

if [[ -n "$REHEARSAL_FILE" && -f "$REHEARSAL_FILE" ]]; then
  if "$ROOT_DIR/scripts/verification/validate-rehearsal-result.sh" "$REHEARSAL_FILE" --commit-sha "$COMMIT_SHA" >/dev/null 2>&1; then
    rehearsal_status=$(python3 - "$REHEARSAL_FILE" <<'PY'
import json
import sys
from pathlib import Path
data = json.loads(Path(sys.argv[1]).read_text(encoding='utf-8'))
print(data.get('status', ''))
print(data.get('failing_gate') or '')
PY
)
    rehearsal_result_status=$(printf '%s\n' "$rehearsal_status" | sed -n '1p')
    rehearsal_failing_gate=$(printf '%s\n' "$rehearsal_status" | sed -n '2p')
    if [[ "$rehearsal_result_status" == "GO" ]]; then
      rehearsal_ready=passed
    else
      add_blocker "{\"id\":\"rehearsal-${rehearsal_failing_gate:-result}\",\"classification\":\"must-fix\",\"source\":\"cutover-rehearsal\",\"summary\":\"Production rehearsal for the current commit ended in NO-GO at gate '${rehearsal_failing_gate:-unknown}'.\"}"
    fi
  else
    add_blocker '{"id":"rehearsal-result","classification":"must-fix","source":"cutover-rehearsal","summary":"Production rehearsal result is present but invalid for the current commit."}'
  fi
else
  add_blocker '{"id":"missing-rehearsal","classification":"must-fix","source":"cutover-rehearsal","summary":"No production rehearsal result exists for the current commit."}'
fi

python3 - "$FINDINGS_FILE" <<'PY' > "$RESULT_DIR/.findings-normalized.json"
import json
import sys
from pathlib import Path

path = Path(sys.argv[1])
if not path.exists():
    print(json.dumps({"blockers": [], "deferred": []}))
    raise SystemExit(0)

data = json.loads(path.read_text(encoding="utf-8"))
if not isinstance(data, dict):
    raise SystemExit("findings file must be a JSON object")
blockers = data.get("blockers", [])
deferred = data.get("deferred", [])
if not isinstance(blockers, list) or not isinstance(deferred, list):
    raise SystemExit("findings file must provide list fields 'blockers' and 'deferred'")
for entry in blockers + deferred:
    if not isinstance(entry, dict):
        raise SystemExit("findings entries must be JSON objects")
    for key in ("id", "classification", "source", "summary"):
        if key not in entry or not isinstance(entry[key], str) or not entry[key].strip():
            raise SystemExit(f"findings entry missing non-empty string field: {key}")
print(json.dumps({"blockers": blockers, "deferred": deferred}))
PY

normalized_findings="$RESULT_DIR/.findings-normalized.json"
if [[ -f "$normalized_findings" ]]; then
  while IFS= read -r line; do
    [[ -n "$line" ]] && add_blocker "$line"
  done < <(python3 - "$normalized_findings" <<'PY'
import json
import sys
data = json.load(open(sys.argv[1], encoding='utf-8'))
for entry in data['blockers']:
    if entry.get('classification') != 'must-fix':
        raise SystemExit("blockers entries must use classification=must-fix")
    print(json.dumps(entry, separators=(',', ':')))
PY
)
  while IFS= read -r line; do
    [[ -n "$line" ]] && add_deferred "$line"
  done < <(python3 - "$normalized_findings" <<'PY'
import json
import sys
data = json.load(open(sys.argv[1], encoding='utf-8'))
for entry in data['deferred']:
    if entry.get('classification') not in {'deferred', 'out-of-scope'}:
        raise SystemExit("deferred entries must use classification=deferred or out-of-scope")
    print(json.dumps(entry, separators=(',', ':')))
PY
)
fi

if [[ ${#BLOCKERS[@]} -eq 0 && "$archive_ready" == "passed" && "$rehearsal_ready" == "passed" ]]; then
  status=GO
fi

printf '%s\n' "${BLOCKERS[@]}" > "$RESULT_DIR/.blockers.jsonl"
printf '%s\n' "${DEFERRED[@]}" > "$RESULT_DIR/.deferred.jsonl"

FINISHED_AT=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
UNIT_EVIDENCE="artifacts/verification/$COMMIT_SHA/unit.json"
INTEGRATION_EVIDENCE="artifacts/verification/$COMMIT_SHA/integration.json"
E2E_EVIDENCE="artifacts/verification/$COMMIT_SHA/e2e.json"
REHEARSAL_REL=""
if [[ -n "$REHEARSAL_FILE" ]]; then
  REHEARSAL_REL=${REHEARSAL_FILE#"$ROOT_DIR/"}
fi
FINDINGS_REL=""
if [[ -f "$FINDINGS_FILE" ]]; then
  FINDINGS_REL=${FINDINGS_FILE#"$ROOT_DIR/"}
fi

python3 - "$RESULT_FILE" "$COMMIT_SHA" "$STARTED_AT" "$FINISHED_AT" "$status" "$archive_ready" "$rehearsal_ready" "$REHEARSAL_REL" "$FINDINGS_REL" "$UNIT_EVIDENCE" "$INTEGRATION_EVIDENCE" "$E2E_EVIDENCE" <<'PY'
import json
import sys
from pathlib import Path

(result_file, commit_sha, started_at, finished_at, status, archive_ready, rehearsal_ready,
 rehearsal_rel, findings_rel, unit_ev, int_ev, e2e_ev) = sys.argv[1:13]

result_dir = Path(result_file).parent

def load_lines(name):
    path = result_dir / name
    if not path.exists():
        return []
    return [json.loads(line) for line in path.read_text(encoding='utf-8').splitlines() if line.strip()]

blockers = load_lines('.blockers.jsonl')
deferred = load_lines('.deferred.jsonl')

payload = {
    'schema_version': '1',
    'project': 'mi',
    'change': 'first-release-acceptance-closure',
    'commit_sha': commit_sha,
    'started_at': started_at,
    'finished_at': finished_at,
    'status': status,
    'scope': {
        'release_boundary': 'first-release non-membership scope',
        'report_inventory': 'R01-R19',
        'excluded_domains': [
            'membership/Associator',
            'legacy transaction migration',
            'reports outside R01-R19',
            'email delivery',
            'device/client tax integration'
        ]
    },
    'gates': {
        'archive_ready': archive_ready,
        'rehearsal_ready': rehearsal_ready,
    },
    'blockers': blockers,
    'deferred': deferred,
    'artifacts': {
        'verification': [unit_ev, int_ev, e2e_ev],
        'rehearsal_result': rehearsal_rel or None,
        'findings': findings_rel or None,
    },
}

Path(result_file).write_text(json.dumps(payload, indent=2) + '\n', encoding='utf-8')
PY

python3 - "$RESULT_FILE" "$COMMIT_SHA" <<'PY'
import json
import sys
from pathlib import Path

summary = Path(sys.argv[1])
commit_sha = sys.argv[2]
data = json.loads(summary.read_text(encoding='utf-8'))
if data['commit_sha'] != commit_sha:
    raise SystemExit('release-readiness summary was written with a stale commit SHA')
print(f"Release readiness: {data['status']}")
print(f"Summary: {summary}")
PY

bash "$ROOT_DIR/scripts/verification/validate-release-readiness.sh" "$RESULT_FILE" --commit-sha "$COMMIT_SHA" >/dev/null

rm -f "$RESULT_DIR/.blockers.jsonl" "$RESULT_DIR/.deferred.jsonl" "$RESULT_DIR/.findings-normalized.json"

if [[ "$status" == "GO" ]]; then
  exit 0
fi

exit 1
