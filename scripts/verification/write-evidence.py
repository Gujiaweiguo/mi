#!/usr/bin/env python3
import argparse
import json
from datetime import datetime, timezone
from pathlib import Path


def timestamp(value: str | None) -> str:
    if value:
        return value
    return (
        datetime.now(timezone.utc)
        .replace(microsecond=0)
        .isoformat()
        .replace("+00:00", "Z")
    )


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--root", required=True)
    parser.add_argument("--commit-sha", required=True)
    parser.add_argument("--test-type", required=True)
    parser.add_argument("--status", required=True)
    parser.add_argument("--project", default="mi")
    parser.add_argument("--change", default="legacy-system-migration")
    parser.add_argument("--source-kind", default="local")
    parser.add_argument("--workflow", default="manual")
    parser.add_argument("--run-id", default="local")
    parser.add_argument("--started-at")
    parser.add_argument("--finished-at")
    parser.add_argument("--total", type=int, default=1)
    parser.add_argument("--passed", type=int, default=0)
    parser.add_argument("--failed", type=int, default=0)
    parser.add_argument("--skipped", type=int, default=0)
    parser.add_argument("--artifact", action="append", default=[])
    args = parser.parse_args()

    evidence_dir = Path(args.root) / "artifacts" / "verification" / args.commit_sha
    evidence_dir.mkdir(parents=True, exist_ok=True)
    evidence_file = evidence_dir / f"{args.test_type}.json"

    payload = {
        "schema_version": "1",
        "project": args.project,
        "change": args.change,
        "commit_sha": args.commit_sha,
        "test_type": args.test_type,
        "status": args.status,
        "started_at": timestamp(args.started_at),
        "finished_at": timestamp(args.finished_at),
        "source": {
            "kind": args.source_kind,
            "workflow": args.workflow,
            "run_id": args.run_id,
        },
        "stats": {
            "total": args.total,
            "passed": args.passed,
            "failed": args.failed,
            "skipped": args.skipped,
        },
        "artifacts": args.artifact,
    }

    evidence_file.write_text(json.dumps(payload, indent=2) + "\n", encoding="utf-8")
    print(evidence_file)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
