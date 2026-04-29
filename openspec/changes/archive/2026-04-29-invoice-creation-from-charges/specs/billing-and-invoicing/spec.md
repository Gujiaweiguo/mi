## ADDED Requirements

### Requirement: Operators SHALL create bill or invoice documents from selected charge lines
The billing charges view SHALL allow operators to select one or more charge lines from the charges table and create a bill or invoice document from those selected lines. The system SHALL present a document-type chooser before submission, call the existing invoice creation API, and provide success or validation feedback.

#### Scenario: Operator selects charge lines and creates a document
- **WHEN** the operator selects one or more charge lines in the billing charges table and activates the create-document action
- **THEN** the system SHALL present a dialog requiring the operator to choose between bill and invoice document types

#### Scenario: Operator confirms document creation
- **WHEN** the operator selects a document type in the dialog and confirms
- **THEN** the system SHALL call the invoice creation API with the selected document type and charge line IDs, set the action to a loading state, and prevent duplicate submissions

#### Scenario: Successful document creation
- **WHEN** the API returns a created document
- **THEN** the system SHALL display success feedback including the document ID and SHALL provide a navigation link to the created document detail view

#### Scenario: Ineligible charge lines produce validation feedback
- **WHEN** the API rejects the creation request because one or more selected charge lines are not eligible for invoicing
- **THEN** the system SHALL display error feedback with the API-provided validation message and SHALL NOT navigate away from the billing charges view

#### Scenario: Create action disabled when no rows selected
- **WHEN** no charge lines are selected in the table
- **THEN** the create-document action SHALL be disabled and non-interactive

#### Scenario: Selection cleared after successful creation
- **WHEN** a document is created successfully from selected charge lines
- **THEN** the system SHALL clear the table selection and refresh the charge line list to reflect updated state
