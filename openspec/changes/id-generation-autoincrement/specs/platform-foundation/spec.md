## MODIFIED Requirements

### Requirement: The system SHALL provide a modular monolith foundation for the new stack
The change SHALL introduce a Vue 3 frontend, a Go modular monolith backend, and a MySQL 8 database as the first-release runtime foundation. The foundation SHALL support local development for frontend/backend with an existing Docker MySQL 8 instance and SHALL support Docker Compose-based test and production topologies.

#### Scenario: Primary key generation is concurrency-safe
- **WHEN** two or more concurrent requests create records in the same table
- **THEN** each request SHALL receive a unique ID without race conditions or duplicate-key errors

#### Scenario: AUTO_INCREMENT is used for all user-facing entity tables
- **WHEN** a new record is inserted into stores, buildings, floors, areas, locations, units, or any baseinfo catalog table
- **THEN** the database SHALL assign the ID via AUTO_INCREMENT, and the Go code SHALL NOT manually compute IDs using MAX(id)+1
