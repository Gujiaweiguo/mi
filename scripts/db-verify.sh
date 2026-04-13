#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)

usage() {
  cat <<'EOF'
Usage: scripts/db-verify.sh <production> [migrate|bootstrap|fresh-start|all]

Runs the selected verification profile against the target environment database.
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-}
PROFILE=${2:-all}

if [[ -z "$ENVIRONMENT" ]]; then
  usage
  exit 1
fi

case "$ENVIRONMENT" in
  production) DEFAULT_MYSQL_PORT=3306 ;;
  *)
    printf 'Unsupported environment: %s\n' "$ENVIRONMENT" >&2
    usage
    exit 1
    ;;
esac

case "$PROFILE" in
  migrate|bootstrap|fresh-start|all) ;;
  *)
    printf 'Unsupported verification profile: %s\n' "$PROFILE" >&2
    usage
    exit 1
    ;;
esac

CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"
MYSQL_PORT=${MI_MYSQL_PORT:-$DEFAULT_MYSQL_PORT}

(
  cd "$ROOT_DIR/backend"
  MI_CONFIG_FILE="$CONFIG_FILE" \
  MI_DATABASE_HOST=127.0.0.1 \
  MI_DATABASE_PORT="$MYSQL_PORT" \
  go run ./cmd/dbops verify --profile="$PROFILE"
)

printf 'Verification profile %s passed for %s\n' "$PROFILE" "$ENVIRONMENT"
