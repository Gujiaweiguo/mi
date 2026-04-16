## ADDED Requirements

### Requirement: Production secrets hygiene SHALL reject documented placeholder defaults
The production deployment workflow SHALL treat the documented placeholder defaults in `deploy/env/production.env` as blocked values for release-oriented validation. The blocked-value contract SHALL include `MYSQL_PASSWORD=change-me`, `MYSQL_ROOT_PASSWORD=change-me-root`, `MI_DB_PASSWORD=change-me`, and `MI_JWT_SECRET=change-me-production-secret`, and those values SHALL NOT pass supported production preflight or rehearsal execution.

#### Scenario: Documented placeholder database passwords are blocked
- **WHEN** production-oriented validation evaluates an env file that still contains `MYSQL_PASSWORD=change-me`, `MYSQL_ROOT_PASSWORD=change-me-root`, or `MI_DB_PASSWORD=change-me`
- **THEN** the workflow SHALL reject the configuration as not production-ready before container startup

#### Scenario: Documented placeholder JWT secret is blocked
- **WHEN** production-oriented validation evaluates an env file that still contains `MI_JWT_SECRET=change-me-production-secret`
- **THEN** the workflow SHALL reject the configuration as not production-ready before container startup

### Requirement: Production secrets hygiene SHALL stay scoped and actionable
The production secrets-hygiene gate SHALL apply to production-oriented validation only, and SHALL report the specific blocked keys that caused failure so operators can remediate the evaluated configuration without guessing.

#### Scenario: Development defaults are not treated as production-secret violations
- **WHEN** development or test-oriented workflows use their documented local defaults outside the supported production validation path
- **THEN** the production secrets-hygiene gate SHALL NOT block those workflows solely because the values would be disallowed for production

#### Scenario: Production rejection identifies blocked keys
- **WHEN** production-oriented validation fails because blocked placeholder values remain in the evaluated configuration
- **THEN** the workflow SHALL identify each offending production secret key in its failure output and SHALL report a no-go outcome for release-oriented execution
