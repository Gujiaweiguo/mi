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

COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
ENV_FILE="${MI_COMPOSE_ENV_FILE:-$ROOT_DIR/deploy/env/$ENVIRONMENT.env}"
CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d mysql >/dev/null

for _ in $(seq 1 60); do
  CONTAINER_ID=$(docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" ps -q mysql)
  if [[ -n "$CONTAINER_ID" ]]; then
    STATUS=$(docker inspect --format '{{if .State.Health}}{{.State.Health.Status}}{{else}}{{.State.Status}}{{end}}' "$CONTAINER_ID")
    if [[ "$STATUS" == "healthy" ]]; then
      break
    fi
  fi
  sleep 2
done

if [[ ${STATUS:-} != "healthy" ]]; then
  printf 'MySQL did not become healthy for %s\n' "$ENVIRONMENT" >&2
  exit 1
fi

MYSQL_CONTAINER_IP=$(docker inspect --format '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' "$CONTAINER_ID")
if [[ -z "$MYSQL_CONTAINER_IP" ]]; then
  printf 'Unable to resolve MySQL container IP for %s\n' "$ENVIRONMENT" >&2
  exit 1
fi

source "$ENV_FILE"

DB_NAME=${MI_DATABASE_NAME:-${MYSQL_DATABASE:-mi_prod}}
DB_USER=${MI_DATABASE_USER:-${MYSQL_USER:-mi_prod}}
DB_PASSWORD=${MI_DATABASE_PASSWORD:-${MI_DB_PASSWORD:-${MYSQL_PASSWORD:-}}}

(
  cd "$ROOT_DIR/backend"
  MI_CONFIG_FILE="$CONFIG_FILE" \
  MI_DATABASE_HOST="$MYSQL_CONTAINER_IP" \
  MI_DATABASE_PORT=3306 \
  MI_DATABASE_NAME="$DB_NAME" \
  MI_DATABASE_USER="$DB_USER" \
  MI_DATABASE_PASSWORD="$DB_PASSWORD" \
  go run ./cmd/dbops verify --profile="$PROFILE"
)

printf 'Verification profile %s passed for %s\n' "$PROFILE" "$ENVIRONMENT"
