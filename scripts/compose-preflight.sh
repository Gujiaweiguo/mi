#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: scripts/compose-preflight.sh <test|production> [--print-config]

Validates the selected Compose environment before startup by checking:
- compose file presence
- deploy env file presence
- backend config file presence
- required runtime directories exist and are writable
- docker compose config renders successfully with the target env file

Pass --print-config to emit the rendered compose configuration.
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" ]]; then
  usage
  exit 0
fi

ENVIRONMENT=${1:-}
PRINT_CONFIG=${2:-}

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

if [[ -n "$PRINT_CONFIG" && "$PRINT_CONFIG" != "--print-config" ]]; then
  printf 'Unsupported option: %s\n' "$PRINT_CONFIG" >&2
  usage
  exit 1
fi

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
COMPOSE_FILE="$ROOT_DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml"
ENV_FILE="$ROOT_DIR/deploy/env/$ENVIRONMENT.env"
CONFIG_FILE="$ROOT_DIR/backend/config/$ENVIRONMENT.yaml"
RUNTIME_DIR="$ROOT_DIR/deploy/runtime/$ENVIRONMENT"

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

require_path "$COMPOSE_FILE" compose-file
require_path "$ENV_FILE" env-file
require_path "$CONFIG_FILE" backend-config
require_path "$RUNTIME_DIR" runtime-root

for runtime_name in logs documents uploads mysql; do
  require_writable_dir "$RUNTIME_DIR/$runtime_name"
done

COMPOSE_ARGS=(--env-file "$ENV_FILE" -f "$COMPOSE_FILE")
if [[ "$PRINT_CONFIG" == "--print-config" ]]; then
  docker compose "${COMPOSE_ARGS[@]}" config
else
  docker compose "${COMPOSE_ARGS[@]}" config >/dev/null
  printf 'Compose preflight passed for %s\n' "$ENVIRONMENT"
fi
