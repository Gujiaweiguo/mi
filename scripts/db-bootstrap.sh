#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)

usage() {
  cat <<'EOF'
Usage: scripts/db-bootstrap.sh <test|production> [cutover|all]

Applies migrations, loads deterministic seeds, and verifies the selected
environment database using the backend dbops CLI.
EOF
}

ENVIRONMENT=${1:-}
SEED_SET=${2:-cutover}

if [[ -z "$ENVIRONMENT" ]]; then
  usage
  exit 1
fi

case "$ENVIRONMENT" in
  test) DEFAULT_MYSQL_PORT=3307 ;;
  production) DEFAULT_MYSQL_PORT=3306 ;;
  *)
    printf 'Unsupported environment: %s\n' "$ENVIRONMENT" >&2
    usage
    exit 1
    ;;
esac

case "$SEED_SET" in
  cutover|all) ;;
  *)
    printf 'Unsupported seed set: %s\n' "$SEED_SET" >&2
    usage
    exit 1
    ;;
esac

COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"
MYSQL_PORT=${MI_MYSQL_PORT:-$DEFAULT_MYSQL_PORT}

docker compose -f "$COMPOSE_FILE" up -d mysql >/dev/null

for _ in $(seq 1 60); do
  CONTAINER_ID=$(docker compose -f "$COMPOSE_FILE" ps -q mysql)
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

(
  cd "$ROOT_DIR/backend"
  MI_CONFIG_FILE="$CONFIG_FILE" \
  MI_DATABASE_HOST=127.0.0.1 \
  MI_DATABASE_PORT="$MYSQL_PORT" \
  go run ./cmd/dbops migrate

  MI_CONFIG_FILE="$CONFIG_FILE" \
  MI_DATABASE_HOST=127.0.0.1 \
  MI_DATABASE_PORT="$MYSQL_PORT" \
  go run ./cmd/dbops bootstrap --seed-set="$SEED_SET"

  MI_CONFIG_FILE="$CONFIG_FILE" \
  MI_DATABASE_HOST=127.0.0.1 \
  MI_DATABASE_PORT="$MYSQL_PORT" \
  go run ./cmd/dbops verify --profile=migrate

  MI_CONFIG_FILE="$CONFIG_FILE" \
  MI_DATABASE_HOST=127.0.0.1 \
  MI_DATABASE_PORT="$MYSQL_PORT" \
  go run ./cmd/dbops verify --profile=bootstrap
)

if [[ "$SEED_SET" == "cutover" ]]; then
  (
    cd "$ROOT_DIR/backend"
    MI_CONFIG_FILE="$CONFIG_FILE" \
    MI_DATABASE_HOST=127.0.0.1 \
    MI_DATABASE_PORT="$MYSQL_PORT" \
    go run ./cmd/dbops verify --profile=fresh-start
  )
fi

printf 'Bootstrap completed for %s using %s seeds\n' "$ENVIRONMENT" "$SEED_SET"
