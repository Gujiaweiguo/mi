## ADDED Requirements

### Requirement: The system SHALL allow operators to create tax rule sets from the frontend

The system SHALL provide a dialog in the tax exports view that lets operators create a new tax rule set by entering code, name, document type, and a variable number of rule entries, then submitting the full payload to the backend.

#### Scenario: Operator opens the create dialog and submits a new rule set
- **WHEN** the operator clicks the "Add Rule Set" button in the rule sets card header
- **THEN** the system SHALL open an empty dialog with fields for code, name, document type (select: invoice/receipt), and a rules table with add/remove row controls
- **AND** when the operator fills all required fields and submits, the system SHALL call `POST /tax/rule-sets` with the complete payload
- **AND** on success, the dialog SHALL close and the rule sets list SHALL refresh to include the newly created rule set

#### Scenario: Operator adds rule entries to the dialog
- **WHEN** the operator clicks "Add Row" inside the rules table area of the dialog
- **THEN** the system SHALL append a new empty rule entry with an auto-assigned `sequence_no`
- **AND** each rule entry SHALL expose fields for entry_side (debit/credit), charge_type_filter, account_number, account_name, explanation_template, use_tenant_name (checkbox), and is_balancing_entry (checkbox)

#### Scenario: Operator removes a rule entry from the dialog
- **WHEN** the operator clicks the remove control on a rule entry row
- **THEN** the system SHALL remove that row from the dialog's rule entries
- **AND** the remaining entries SHALL retain their original `sequence_no` values without renumbering

### Requirement: The system SHALL allow operators to edit existing tax rule sets from the frontend

The system SHALL allow operators to click a row in the rule sets table to open the dialog pre-populated with that rule set's current data, edit the fields and rules, and submit the updated payload.

#### Scenario: Operator clicks a rule set row to edit
- **WHEN** the operator clicks a row in the available rule sets table
- **THEN** the system SHALL open the dialog with the code, name, document type, and all rule entries pre-populated from the selected rule set
- **AND** the code field SHALL be populated with the existing rule set's code so the backend identifies this as an update

#### Scenario: Operator edits rule entries and submits
- **WHEN** the operator modifies fields or rule entries in the edit dialog and submits
- **THEN** the system SHALL call `POST /tax/rule-sets` with the modified payload
- **AND** on success, the dialog SHALL close and the rule sets list SHALL refresh to reflect the updated rule set

### Requirement: The system SHALL display feedback on upsert success or failure

The system SHALL render success or error feedback after the upsert call completes, using the existing feedback alert mechanism in the tax exports view.

#### Scenario: Upsert succeeds
- **WHEN** the backend responds with 201 Created
- **THEN** the system SHALL display a success feedback alert and refresh the rule sets list

#### Scenario: Upsert fails with validation error
- **WHEN** the backend responds with 400 Bad Request
- **THEN** the system SHALL display an error feedback alert with the backend's error message and keep the dialog open so the operator can correct the input
