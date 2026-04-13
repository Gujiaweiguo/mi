#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/db-restore.sh <production> <backup-archive> [--restore-runtime-files]

Restores the MySQL logical dump from a backup created by scripts/db-backup.sh.
Pass --restore-runtime-files to also replace the runtime logs/documents/uploads
directories plus the backed-up config/env snapshots on disk.
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-}
ARCHIVE_PATH=${2:-}
RESTORE_RUNTIME_FILES=${3:-}

if [[ -z "$ENVIRONMENT" || -z "$ARCHIVE_PATH" ]]; then
  usage
  exit 1
fi

case "$ENVIRONMENT" in
  production) ;;
  *)
    printf 'Unsupported environment: %s\n' "$ENVIRONMENT" >&2
    usage
    exit 1
    ;;
esac

if [[ ! -f "$ARCHIVE_PATH" ]]; then
  printf 'Backup archive not found: %s\n' "$ARCHIVE_PATH" >&2
  exit 1
fi

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
ENV_FILE="$ROOT_DIR/deploy/env/$ENVIRONMENT.env"
RUNTIME_DIR="$ROOT_DIR/deploy/runtime/$ENVIRONMENT"
CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"

for required in "$COMPOSE_FILE" "$ENV_FILE" "$RUNTIME_DIR"; do
  if [[ ! -e "$required" ]]; then
    printf 'Required path missing: %s\n' "$required" >&2
    exit 1
  fi
done

source "$ENV_FILE"

EXTRACT_DIR=$(mktemp -d)

cleanup() {
  rm -rf "$EXTRACT_DIR"
}
trap cleanup EXIT

tar -C "$EXTRACT_DIR" -xzf "$ARCHIVE_PATH"

if [[ ! -f "$EXTRACT_DIR/metadata.json" ]]; then
  printf 'Backup archive does not contain metadata.json: %s\n' "$ARCHIVE_PATH" >&2
  exit 1
fi

if [[ ! -f "$EXTRACT_DIR/database.sql" ]]; then
  printf 'Backup archive does not contain database.sql: %s\n' "$ARCHIVE_PATH" >&2
  exit 1
fi

python3 - "$EXTRACT_DIR/metadata.json" "$ENVIRONMENT" <<'PY'
import json
import sys
from pathlib import Path

metadata_path = Path(sys.argv[1])
target_environment = sys.argv[2]
metadata = json.loads(metadata_path.read_text(encoding="utf-8"))

required_keys = {
    "schema_version",
    "environment",
    "generated_at",
    "compose_file",
    "env_file",
    "config_file",
    "database",
    "artifacts",
    "restore_runtime_files_supported",
}
missing = sorted(required_keys - metadata.keys())
if missing:
    raise SystemExit(f"Backup metadata missing keys: {', '.join(missing)}")

if metadata["environment"] != target_environment:
    raise SystemExit(
        f"Backup archive environment mismatch: expected {target_environment}, got {metadata['environment']}"
    )

artifacts = metadata["artifacts"]
for key in ("database_dump", "runtime_directories", "config_snapshots"):
    if key not in artifacts:
        raise SystemExit(f"Backup metadata artifacts missing key: {key}")
PY

for required in \
  "$EXTRACT_DIR/config/backend/$ENVIRONMENT.yaml" \
  "$EXTRACT_DIR/config/deploy-env/$ENVIRONMENT.env"; do
  if [[ ! -f "$required" ]]; then
    printf 'Backup archive missing required config snapshot: %s\n' "$required" >&2
    exit 1
  fi
done

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T mysql sh -lc \
  'exec mysql -uroot -p"$MYSQL_ROOT_PASSWORD" "$MYSQL_DATABASE"' \
  < "$EXTRACT_DIR/database.sql"

if [[ "$RESTORE_RUNTIME_FILES" == "--restore-runtime-files" ]]; then
  for runtime_name in logs documents uploads; do
    if [[ -d "$EXTRACT_DIR/runtime/$runtime_name" ]]; then
      rm -rf "$RUNTIME_DIR/$runtime_name"
      mkdir -p "$RUNTIME_DIR/$runtime_name"
      cp -a "$EXTRACT_DIR/runtime/$runtime_name/." "$RUNTIME_DIR/$runtime_name/"
    fi
  done

  if [[ -f "$EXTRACT_DIR/config/backend/$ENVIRONMENT.yaml" ]]; then
    cp "$EXTRACT_DIR/config/backend/$ENVIRONMENT.yaml" "$CONFIG_FILE"
  fi

  if [[ -f "$EXTRACT_DIR/config/deploy-env/$ENVIRONMENT.env" ]]; then
    cp "$EXTRACT_DIR/config/deploy-env/$ENVIRONMENT.env" "$ENV_FILE"
  fi
fi

printf 'Restore completed for %s using %s\n' "$ENVIRONMENT" "$ARCHIVE_PATH"
