#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
ROOT_DIR=$(cd "${SCRIPT_DIR}/../.." && pwd)
SCHEMA_PATH="$ROOT_DIR/schemas/evidence-v1.json"
STRUCTURE_VALIDATOR="$SCRIPT_DIR/validate-evidence-structure.py"

# Canonical sample sources (shared with docs/evidence-contract.md)
CI_SAMPLE="$SCRIPT_DIR/testdata/pass-ci/artifacts/verification/1111111111111111111111111111111111111111/unit.json"
ARCHIVE_SAMPLE="$SCRIPT_DIR/testdata/pass-archive/artifacts/verification/2222222222222222222222222222222222222222/e2e.json"

echo "[SCHEMA-PARSE] schemas/evidence-v1.json"
python3 - "$SCHEMA_PATH" <<'PY'
import json
import sys
from pathlib import Path

schema = json.loads(Path(sys.argv[1]).read_text(encoding="utf-8"))
if not isinstance(schema, dict):
    raise SystemExit("schema root must be an object")
if "properties" not in schema:
    raise SystemExit("schema must define properties")
print("schema parses OK")
PY

echo "[CI-SAMPLE] unit.json"
python3 "$STRUCTURE_VALIDATOR" "$SCHEMA_PATH" "$CI_SAMPLE"

echo "[ARCHIVE-SAMPLE] e2e.json"
python3 "$STRUCTURE_VALIDATOR" "$SCHEMA_PATH" "$ARCHIVE_SAMPLE"

echo
echo "Schema self-check passed."
