#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/db-backup.sh <test|production> [output-dir]

Creates a timestamped backup bundle containing:
- a MySQL logical dump from the running compose stack
- snapshots of runtime logs/documents/uploads
- the mounted backend config file for the selected environment
- the compose env file used for that environment

The resulting archive is written to artifacts/backups/<env>/ by default.
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-}
OUTPUT_DIR=${2:-}

if [[ -z "$ENVIRONMENT" ]]; then
  usage
  exit 1
fi

case "$ENVIRONMENT" in
  test|production) ;;
  *)
    printf 'Unsupported environment: %s\n' "$ENVIRONMENT" >&2
    usage
    exit 1
    ;;
esac

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
ENV_FILE="$ROOT_DIR/deploy/env/$ENVIRONMENT.env"
RUNTIME_DIR="$ROOT_DIR/deploy/runtime/$ENVIRONMENT"
CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"
DEFAULT_OUTPUT_DIR="$ROOT_DIR/artifacts/backups/$ENVIRONMENT"
TARGET_OUTPUT_DIR=${OUTPUT_DIR:-$DEFAULT_OUTPUT_DIR}

for required in "$COMPOSE_FILE" "$ENV_FILE" "$RUNTIME_DIR" "$CONFIG_FILE"; do
  if [[ ! -e "$required" ]]; then
    printf 'Required path missing: %s\n' "$required" >&2
    exit 1
  fi
done

source "$ENV_FILE"

mkdir -p "$TARGET_OUTPUT_DIR"

TIMESTAMP=$(date -u +"%Y%m%dT%H%M%SZ")
STAGING_DIR=$(mktemp -d)
ARCHIVE_NAME="mi-${ENVIRONMENT}-backup-${TIMESTAMP}.tar.gz"
ARCHIVE_PATH="$TARGET_OUTPUT_DIR/$ARCHIVE_NAME"

cleanup() {
  rm -rf "$STAGING_DIR"
}
trap cleanup EXIT

mkdir -p \
  "$STAGING_DIR/runtime" \
  "$STAGING_DIR/config/backend" \
  "$STAGING_DIR/config/deploy-env"

python3 - <<PY > "$STAGING_DIR/metadata.json"
import json

print(json.dumps({
    "schema_version": 1,
    "environment": ${ENVIRONMENT@Q},
    "generated_at": ${TIMESTAMP@Q},
    "compose_file": ${COMPOSE_FILE@Q},
    "env_file": ${ENV_FILE@Q},
    "config_file": ${CONFIG_FILE@Q},
    "database": ${MYSQL_DATABASE@Q},
    "artifacts": {
        "database_dump": "database.sql",
        "runtime_directories": ["logs", "documents", "uploads"],
        "config_snapshots": [
            "config/backend/${ENVIRONMENT}.yaml",
            "config/deploy-env/${ENVIRONMENT}.env",
        ],
    },
    "restore_runtime_files_supported": True,
}, indent=2))
PY

docker compose -f "$COMPOSE_FILE" exec -T mysql sh -lc \
  'exec mysqldump -uroot -p"$MYSQL_ROOT_PASSWORD" --single-transaction --routines --triggers --databases "$MYSQL_DATABASE"' \
  > "$STAGING_DIR/database.sql"

cp "$CONFIG_FILE" "$STAGING_DIR/config/backend/$ENVIRONMENT.yaml"
cp "$ENV_FILE" "$STAGING_DIR/config/deploy-env/$ENVIRONMENT.env"

for runtime_name in logs documents uploads; do
  if [[ -d "$RUNTIME_DIR/$runtime_name" ]]; then
    cp -a "$RUNTIME_DIR/$runtime_name" "$STAGING_DIR/runtime/$runtime_name"
  fi
done

tar -C "$STAGING_DIR" -czf "$ARCHIVE_PATH" .

printf 'Backup written to %s\n' "$ARCHIVE_PATH"
