#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 2 ]]; then
  echo "Usage: compare-artifacts.sh <expected> <actual>" >&2
  exit 2
fi

EXPECTED=$1
ACTUAL=$2

if ! cmp -s "$EXPECTED" "$ACTUAL"; then
  echo "artifact mismatch: $EXPECTED != $ACTUAL" >&2
  exit 1
fi

echo "artifacts match: $EXPECTED == $ACTUAL"
