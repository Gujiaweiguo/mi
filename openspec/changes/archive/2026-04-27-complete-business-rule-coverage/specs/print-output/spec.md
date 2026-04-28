## ADDED Requirements

### Requirement: Lease contract print output SHALL match supported document types
The system SHALL either fully support `lease_contract` as a renderable document type or remove it from operator-selectable print template configuration.

#### Scenario: Lease contract template renders contract data
- **WHEN** an authorized operator renders a `lease_contract` document for an approved lease
- **THEN** the system SHALL load lease contract, customer, unit, term, subtype, and approval data into the template and produce trusted HTML/PDF output

#### Scenario: Unsupported document type is not advertised
- **WHEN** a document type is not implemented by the rendering backend
- **THEN** the frontend and backend SHALL not advertise it as a selectable trusted print template type
