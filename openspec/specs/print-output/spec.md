# print-output Specification

## Purpose
TBD - created by archiving change print-template-management. Update Purpose after archive.
## Requirements
### Requirement: The system SHALL allow operators to create print templates from the frontend

The system SHALL provide a dialog in the print preview view that lets operators create a new print template by entering code, name, document type, output mode, title, subtitle, and dynamic lists of header lines and footer lines, then submitting the full payload to the backend.

#### Scenario: Operator opens the create dialog and submits a new template
- **WHEN** the operator clicks the "Add Template" button in the templates card header
- **THEN** the system SHALL open an empty dialog with fields for code, name, document type (select: invoice/receipt/lease_contract), output mode (select: html/pdf), title, subtitle, and dynamic tag lists for header_lines and footer_lines
- **AND** when the operator fills all required fields and submits, the system SHALL call `POST /print/templates` with the complete payload
- **AND** on success, the dialog SHALL close and the templates list SHALL refresh to include the newly created template

#### Scenario: Operator manages header lines in the dialog
- **WHEN** the operator types a line in the header lines input and confirms (Enter or add button)
- **THEN** the system SHALL append the typed string as a new tag to the header_lines list
- **AND** each tag SHALL display a close icon that removes the line when clicked
- **AND** the header_lines list SHALL be included in the upsert payload as a string array

#### Scenario: Operator manages footer lines in the dialog
- **WHEN** the operator types a line in the footer lines input and confirms (Enter or add button)
- **THEN** the system SHALL append the typed string as a new tag to the footer_lines list
- **AND** each tag SHALL display a close icon that removes the line when clicked
- **AND** the footer_lines list SHALL be included in the upsert payload as a string array

### Requirement: The system SHALL allow operators to edit existing print templates from the frontend

The system SHALL allow operators to click a row in the templates table to open the dialog pre-populated with that template's current data, edit the fields and lines, and submit the updated payload.

#### Scenario: Operator clicks a template row to edit
- **WHEN** the operator clicks a row in the available templates table
- **THEN** the system SHALL open the dialog with code, name, document_type, output_mode, title, subtitle, header_lines, and footer_lines pre-populated from the selected template
- **AND** the code field SHALL be populated with the existing template's code so the backend identifies this as an update

#### Scenario: Operator edits fields and submits
- **WHEN** the operator modifies fields, header lines, or footer lines in the edit dialog and submits
- **THEN** the system SHALL call `POST /print/templates` with the modified payload
- **AND** on success, the dialog SHALL close and the templates list SHALL refresh to reflect the updated template

### Requirement: The system SHALL display feedback on upsert success or failure

The system SHALL render success or error feedback after the upsert call completes, using the existing feedback alert mechanism in the print preview view.

#### Scenario: Upsert succeeds
- **WHEN** the backend responds with success
- **THEN** the system SHALL display a success feedback alert and refresh the templates list

#### Scenario: Upsert fails with validation error
- **WHEN** the backend responds with 400 Bad Request
- **THEN** the system SHALL display an error feedback alert with the backend's error message and keep the dialog open so the operator can correct the input

### Requirement: Lease contract print output SHALL match supported document types
The system SHALL either fully support `lease_contract` as a renderable document type or remove it from operator-selectable print template configuration.

#### Scenario: Lease contract template renders contract data
- **WHEN** an authorized operator renders a `lease_contract` document for an approved lease
- **THEN** the system SHALL load lease contract, customer, unit, term, subtype, and approval data into the template and produce trusted HTML/PDF output

#### Scenario: Unsupported document type is not advertised
- **WHEN** a document type is not implemented by the rendering backend
- **THEN** the frontend and backend SHALL not advertise it as a selectable trusted print template type

### Requirement: Supported financial document output SHALL preserve overtime-derived billing data
The system SHALL include overtime-derived financial lines and trusted attribution in any supported invoice-facing or equivalent downstream document output whose rendered financial content is sourced from billing, invoice, or receivable composition.

#### Scenario: Supported financial document renders overtime-derived line items
- **WHEN** an authorized operator renders a supported financial document that includes billed overtime-derived charges
- **THEN** the system SHALL include the overtime-derived financial lines in the rendered HTML/PDF output with the same trusted composition path used for other financial line items

#### Scenario: Document output does not silently omit downstream overtime data
- **WHEN** a supported print or document output depends on downstream financial records that include overtime-derived charges
- **THEN** the system SHALL either render those overtime-backed values correctly or reject the unsupported render path instead of silently producing misleading output
