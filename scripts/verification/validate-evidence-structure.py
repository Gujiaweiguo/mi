#!/usr/bin/env python3

import json
import sys
from pathlib import Path

from jsonschema import Draft7Validator


def format_error_path(error) -> str:
    if not error.path:
        return "<root>"
    return ".".join(str(part) for part in error.path)


def main() -> int:
    if len(sys.argv) != 3:
        print(
            "usage: validate-evidence-structure.py <schema-path> <evidence-path>",
            file=sys.stderr,
        )
        return 2

    schema_path = Path(sys.argv[1])
    evidence_path = Path(sys.argv[2])

    try:
        schema = json.loads(schema_path.read_text(encoding="utf-8"))
    except Exception as exc:
        print(f"schema load error: {schema_path}: {exc}")
        return 2

    try:
        evidence = json.loads(evidence_path.read_text(encoding="utf-8"))
    except Exception as exc:
        print(f"malformed evidence: {evidence_path}: {exc}")
        return 1

    validator = Draft7Validator(schema)
    errors = sorted(validator.iter_errors(evidence), key=lambda err: list(err.path))
    if errors:
        first = errors[0]
        print(
            f"malformed evidence: {evidence_path}: schema validation failed at {format_error_path(first)}: {first.message}"
        )
        return 1

    print(f"schema-valid evidence: {evidence_path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
