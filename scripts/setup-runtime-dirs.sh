#!/usr/bin/env bash
# Create runtime directories for production deployment.
# Usage: bash scripts/setup-runtime-dirs.sh
set -euo pipefail

BASE="${1:-deploy/runtime/production}"

echo "Creating runtime directories under ${BASE}..."

for dir in logs documents uploads mysql; do
  mkdir -p "${BASE}/${dir}"
done

# Backend container runs as uid 10001 (appuser).
chown -R 10001:10001 "${BASE}" 2>/dev/null || {
  echo "WARNING: Could not chown to uid 10001. Run with sudo or adjust manually:"
  echo "  sudo chown -R 10001:10001 ${BASE}"
}

echo "Done. Directories created:"
ls -la "${BASE}/"
