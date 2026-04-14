## MODIFIED Requirements

### Requirement: The system SHALL externalize environment configuration and runtime mounts
The change SHALL define environment-specific configuration with file-based defaults and environment-variable overrides. Test and production environments SHALL mount runtime paths for configuration, logs, generated documents, uploads, and MySQL data. Production runtime mounts SHALL also enforce documented hygiene and permission assumptions so rehearsal and go-live validation are not considered valid under contaminated runtime baselines or unsupported container runtime behavior.

#### Scenario: Production runtime paths are configured
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** explicit mounts SHALL exist for configuration, logs, generated documents/uploads, and MySQL data

#### Scenario: Production runtime mount hygiene is validated
- **WHEN** production startup or rehearsal preflight evaluates runtime mount baselines
- **THEN** the workflow SHALL reject runtime baselines that violate documented clean-start and hygiene constraints for supported production validation

#### Scenario: Runtime mount permissions are validated for supported container behavior
- **WHEN** production startup or rehearsal validation checks mounted runtime paths
- **THEN** the workflow SHALL verify required writable paths under supported container runtime assumptions and SHALL fail when those assumptions are not met
