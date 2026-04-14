#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/compose-preflight.sh <production> [--print-config] [--require-clean-runtime]

Validates the selected Compose environment before startup by checking:
- compose file presence
- deploy env file presence
- backend config file presence
- required runtime directories exist and are writable
- docker compose config renders successfully with the target env file

Pass --print-config to emit the rendered compose configuration.
Pass --require-clean-runtime to fail when runtime directories contain
entries other than `.gitkeep`.
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-}
shift || true

PRINT_CONFIG=false
REQUIRE_CLEAN_RUNTIME=false

while (($#)); do
  case "$1" in
    --print-config)
      PRINT_CONFIG=true
      ;;
    --require-clean-runtime)
      REQUIRE_CLEAN_RUNTIME=true
      ;;
    *)
      printf 'Unsupported option: %s\n' "$1" >&2
      usage
      exit 1
      ;;
  esac
  shift
done

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
CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"
DEFAULT_RUNTIME_DIR="$ROOT_DIR/deploy/runtime/$ENVIRONMENT"
RUNTIME_DIR="${MI_RUNTIME_BASE:-$DEFAULT_RUNTIME_DIR}"
RUNTIME_LOGS="${MI_RUNTIME_LOGS:-$RUNTIME_DIR/logs}"
RUNTIME_DOCUMENTS="${MI_RUNTIME_DOCUMENTS:-$RUNTIME_DIR/documents}"
RUNTIME_UPLOADS="${MI_RUNTIME_UPLOADS:-$RUNTIME_DIR/uploads}"
RUNTIME_MYSQL="${MI_RUNTIME_MYSQL:-$RUNTIME_DIR/mysql}"

require_path() {
  local path=$1
  local label=$2
  if [[ ! -e "$path" ]]; then
    printf 'Required %s missing: %s\n' "$label" "$path" >&2
    exit 1
  fi
}

require_writable_dir() {
  local path=$1
  require_path "$path" directory
  if [[ ! -d "$path" ]]; then
    printf 'Required runtime path is not a directory: %s\n' "$path" >&2
    exit 1
  fi
  local probe="$path/.compose-preflight-$$"
  if ! : > "$probe" 2>/dev/null; then
    printf 'Required runtime directory is not writable: %s\n' "$path" >&2
    exit 1
  fi
  rm -f "$probe"
}

require_container_writable_dir() {
  local path=$1
  local label=$2
  local container_user=$3
  require_path "$path" directory
  if [[ ! -d "$path" ]]; then
    printf 'Required runtime path is not a directory: %s\n' "$path" >&2
    exit 1
  fi
  if ! docker run --rm --user "$container_user" -v "$path:/target" alpine:3.20 sh -lc 'probe=/target/.compose-preflight-probe && : > "$probe" && rm -f "$probe"' >/dev/null 2>&1; then
    printf 'Required runtime directory is not writable for %s: %s\n' "$label" "$path" >&2
    exit 1
  fi
}

require_container_readable_dir() {
  local path=$1
  local label=$2
  local container_user=$3
  require_path "$path" directory
  if [[ ! -d "$path" ]]; then
    printf 'Required config path is not a directory: %s\n' "$path" >&2
    exit 1
  fi
  if ! docker run --rm --user "$container_user" -v "$path:/target:ro" alpine:3.20 sh -lc 'test -r /target && ls /target >/dev/null' >/dev/null 2>&1; then
    printf 'Required config directory is not readable for %s: %s\n' "$label" "$path" >&2
    exit 1
  fi
}

require_clean_runtime_dir() {
  local path=$1
  local label=$2
  local entry

  for entry in "$path"/* "$path"/.[!.]* "$path"/..?*; do
    [[ -e "$entry" ]] || continue
    if [[ "$(basename "$entry")" != ".gitkeep" ]]; then
      printf 'Runtime contamination detected in %s: %s\n' "$label" "$entry" >&2
      printf 'Expected a clean runtime baseline (only optional .gitkeep is allowed).\n' >&2
      exit 1
    fi
  done
}

require_path "$COMPOSE_FILE" compose-file
require_path "$ENV_FILE" env-file
require_path "$CONFIG_FILE" backend-config
require_path "$RUNTIME_DIR" runtime-root
require_container_readable_dir "${MI_RUNTIME_CONFIG:-$ROOT_DIR/backend/config}" backend-config-uid-10001 10001:10001

require_container_writable_dir "$RUNTIME_LOGS" backend-runtime-uid-10001 10001:10001
require_container_writable_dir "$RUNTIME_DOCUMENTS" backend-runtime-uid-10001 10001:10001
require_container_writable_dir "$RUNTIME_UPLOADS" backend-runtime-uid-10001 10001:10001
require_container_writable_dir "$RUNTIME_MYSQL" mysql-runtime-uid-999 999:999

if [[ "$REQUIRE_CLEAN_RUNTIME" == true ]]; then
  require_clean_runtime_dir "$RUNTIME_LOGS" logs-runtime
  require_clean_runtime_dir "$RUNTIME_DOCUMENTS" documents-runtime
  require_clean_runtime_dir "$RUNTIME_UPLOADS" uploads-runtime
  require_clean_runtime_dir "$RUNTIME_MYSQL" mysql-runtime
fi

COMPOSE_ARGS=(--env-file "$ENV_FILE" -f "$COMPOSE_FILE")
if [[ "$PRINT_CONFIG" == true ]]; then
  docker compose "${COMPOSE_ARGS[@]}" config
else
  docker compose "${COMPOSE_ARGS[@]}" config >/dev/null
  printf 'Compose preflight passed for %s\n' "$ENVIRONMENT"
fi
