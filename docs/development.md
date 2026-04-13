# Development Setup

## Local Development Mode

Task 3 establishes the local development convention:

- frontend runs locally
- backend runs locally
- MySQL 8 runs from the existing local Docker instance

## Backend

Run:

```bash
scripts/dev-backend.sh
```

Default config file:

```text
backend/config/development.yaml
```

Override with:

```bash
MI_CONFIG_FILE=backend/config/development.yaml
```

## Frontend

The frontend lives under `frontend/` and uses Vite env variables for runtime configuration.

Typical local API target:

```text
http://localhost:5180
```

## Compose Render Checks

Render production topology:

```bash
scripts/compose-production-config.sh
```

## Config Rules

- file defaults live in `backend/config/*.yaml`
- environment variables override file values
- secrets must not be hardcoded into committed production config
