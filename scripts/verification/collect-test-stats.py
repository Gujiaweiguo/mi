#!/usr/bin/env python3

import json
import sys
from pathlib import Path


def load_go_jsonl(path: Path) -> tuple[int, int, int, int]:
    total = passed = failed = skipped = 0
    for raw_line in path.read_text(encoding="utf-8").splitlines():
        if not raw_line.strip():
            continue
        event = json.loads(raw_line)
        if "Test" not in event:
            continue
        action = event.get("Action")
        if action == "pass":
            total += 1
            passed += 1
        elif action == "fail":
            total += 1
            failed += 1
        elif action == "skip":
            total += 1
            skipped += 1
    return total, passed, failed, skipped


def load_vitest_json(path: Path) -> tuple[int, int, int, int]:
    report = json.loads(path.read_text(encoding="utf-8"))
    total = int(report.get("numTotalTests", 0) or 0)
    passed = int(report.get("numPassedTests", 0) or 0)
    failed = int(report.get("numFailedTests", 0) or 0)
    skipped = int(report.get("numPendingTests", 0) or 0) + int(
        report.get("numTodoTests", 0) or 0
    )
    return total, passed, failed, skipped


def main() -> int:
    if len(sys.argv) < 3:
        print(
            "usage: collect-test-stats.py <go-jsonl|vitest-json> <path> [<path> ...]",
            file=sys.stderr,
        )
        return 2

    mode = sys.argv[1]
    paths = [Path(value) for value in sys.argv[2:]]

    total = passed = failed = skipped = 0
    for path in paths:
        if mode == "go-jsonl":
            counts = load_go_jsonl(path)
        elif mode == "vitest-json":
            counts = load_vitest_json(path)
        else:
            print(f"unsupported mode: {mode}", file=sys.stderr)
            return 2

        total += counts[0]
        passed += counts[1]
        failed += counts[2]
        skipped += counts[3]

    print(
        json.dumps(
            {
                "total": total,
                "passed": passed,
                "failed": failed,
                "skipped": skipped,
            }
        )
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
