## MODIFIED Requirements

### Requirement: Production Dockerfiles SHALL use layer caching optimization
Backend and frontend Dockerfiles SHALL copy dependency manifests first, download dependencies, then copy source code. This ensures dependency layers are cached across source-only changes.

#### Scenario: Dependency layer is cached on source-only changes
- **WHEN** only source code changes (not go.mod/go.sum or package.json)
- **THEN** Docker SHALL reuse the cached dependency download layer

### Requirement: Production nginx SHALL enforce static asset caching and rate limiting
The nginx production configuration SHALL set Cache-Control headers for hashed static assets and SHALL rate-limit authentication endpoints.

#### Scenario: Static assets have long cache lifetime
- **WHEN** a browser requests a hashed JS/CSS file
- **THEN** nginx SHALL return Cache-Control: public, immutable with 1-year expiry

#### Scenario: Login endpoint is rate-limited
- **WHEN** an IP sends more than 10 login requests per minute
- **THEN** nginx SHALL return HTTP 429
