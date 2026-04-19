#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/compose-smoke-test.sh <production> [--build] [--keep-running]

Starts the selected compose stack, waits for nginx/frontend/backend/mysql to
report healthy, and verifies:
- backend /healthz inside the backend container
- frontend root inside the frontend container
- nginx root and proxied /api/healthz inside the nginx container

By default the stack is stopped at the end. Pass --keep-running to leave it up.
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-}
BUILD_FLAG=${2:-}
KEEP_RUNNING_FLAG=${3:-}

if [[ -z "$ENVIRONMENT" ]]; then
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

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
ENV_FILE="${MI_COMPOSE_ENV_FILE:-$ROOT_DIR/deploy/env/$ENVIRONMENT.env}"
OVERRIDE_MI_HTTP_PORT=${MI_HTTP_PORT:-}
OVERRIDE_MI_MYSQL_PORT=${MI_MYSQL_PORT:-}
OVERRIDE_MI_RUNTIME_BASE=${MI_RUNTIME_BASE:-}
OVERRIDE_MI_RUNTIME_LOGS=${MI_RUNTIME_LOGS:-}
OVERRIDE_MI_RUNTIME_DOCUMENTS=${MI_RUNTIME_DOCUMENTS:-}
OVERRIDE_MI_RUNTIME_UPLOADS=${MI_RUNTIME_UPLOADS:-}
OVERRIDE_MI_RUNTIME_MYSQL=${MI_RUNTIME_MYSQL:-}

if [[ ! -f "$COMPOSE_FILE" ]]; then
  printf 'Compose file missing: %s\n' "$COMPOSE_FILE" >&2
  exit 1
fi

"$ROOT_DIR/scripts/compose-preflight.sh" "$ENVIRONMENT"
source "$ENV_FILE"

if [[ -n "$OVERRIDE_MI_HTTP_PORT" ]]; then
  export MI_HTTP_PORT="$OVERRIDE_MI_HTTP_PORT"
fi

if [[ -n "$OVERRIDE_MI_MYSQL_PORT" ]]; then
  export MI_MYSQL_PORT="$OVERRIDE_MI_MYSQL_PORT"
fi

if [[ -n "$OVERRIDE_MI_RUNTIME_BASE" ]]; then
  export MI_RUNTIME_BASE="$OVERRIDE_MI_RUNTIME_BASE"
fi

if [[ -n "$OVERRIDE_MI_RUNTIME_LOGS" ]]; then
  export MI_RUNTIME_LOGS="$OVERRIDE_MI_RUNTIME_LOGS"
fi

if [[ -n "$OVERRIDE_MI_RUNTIME_DOCUMENTS" ]]; then
  export MI_RUNTIME_DOCUMENTS="$OVERRIDE_MI_RUNTIME_DOCUMENTS"
fi

if [[ -n "$OVERRIDE_MI_RUNTIME_UPLOADS" ]]; then
  export MI_RUNTIME_UPLOADS="$OVERRIDE_MI_RUNTIME_UPLOADS"
fi

if [[ -n "$OVERRIDE_MI_RUNTIME_MYSQL" ]]; then
  export MI_RUNTIME_MYSQL="$OVERRIDE_MI_RUNTIME_MYSQL"
fi

BUILD_ARG=()
if [[ "$BUILD_FLAG" == "--build" || "$KEEP_RUNNING_FLAG" == "--build" ]]; then
  BUILD_ARG=(--build)
fi

KEEP_RUNNING=false
if [[ "$BUILD_FLAG" == "--keep-running" || "$KEEP_RUNNING_FLAG" == "--keep-running" ]]; then
  KEEP_RUNNING=true
fi

teardown() {
  if [[ "$KEEP_RUNNING" == false ]]; then
    docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  fi
}
trap teardown EXIT

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d mysql

wait_for_health() {
  local service=$1
  local attempts=0
  local max_attempts=60

  while (( attempts < max_attempts )); do
    local container_id
    container_id=$(docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" ps -q "$service")
    if [[ -n "$container_id" ]]; then
      local status
      status=$(docker inspect --format '{{if .State.Health}}{{.State.Health.Status}}{{else}}{{.State.Status}}{{end}}' "$container_id")
      if [[ "$status" == "healthy" ]]; then
        return 0
      fi
    fi

    attempts=$((attempts + 1))
    sleep 2
  done

  printf 'Timed out waiting for %s to become healthy\n' "$service" >&2
  return 1
}

wait_for_health mysql

MYSQL_CONTAINER_ID=$(docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" ps -q mysql)
MI_DATABASE_HOST=$(docker inspect --format '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' "$MYSQL_CONTAINER_ID")
if [[ -z "$MI_DATABASE_HOST" ]]; then
  printf 'Unable to determine MySQL container IP for smoke test\n' >&2
  exit 1
fi
export MI_DATABASE_HOST

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d "${BUILD_ARG[@]}" migrate backend frontend nginx

for service in backend frontend nginx; do
  wait_for_health "$service"
done

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T mysql sh -lc 'test -w /var/lib/mysql && mysqladmin ping -h localhost -uroot -p"$MYSQL_ROOT_PASSWORD" --silent'
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T backend sh -lc "test -w /app/logs && test -w /app/generated-documents && test -w /app/uploads && wget -q -O /dev/null http://127.0.0.1:${MI_SERVER_PORT:-5180}/healthz"
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T frontend sh -lc 'wget -q -O /dev/null http://127.0.0.1/'
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T nginx sh -lc 'wget -q -O /dev/null http://127.0.0.1/'
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T nginx sh -lc 'wget -q -O /dev/null http://127.0.0.1/api/healthz'

printf 'Compose smoke test passed for %s\n' "$ENVIRONMENT"
