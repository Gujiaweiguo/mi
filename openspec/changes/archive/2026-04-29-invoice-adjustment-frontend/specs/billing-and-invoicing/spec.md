## ADDED Requirements

### Requirement: Invoice adjustment SHALL be operator-reachable from the first-release frontend
The system SHALL expose invoice adjustment for adjustment-eligible approved billing documents from the authenticated frontend and SHALL let operators prepare replacement line amounts before submitting the adjustment request.

#### Scenario: Operator starts an adjustment from an eligible invoice document
- **WHEN** an operator opens an adjustment-eligible approved invoice document in the supported frontend review surfaces and selects the adjustment action
- **THEN** the system SHALL present an invoice adjustment drafting flow bound to the selected source document

#### Scenario: Adjustment draft preserves editable line context
- **WHEN** the adjustment drafting flow is displayed
- **THEN** the system SHALL show the source document lines with enough context for the operator to enter replacement amounts tied to the original billing charge lines

### Requirement: Invoice adjustment submission SHALL reuse the existing replacement-document flow
The frontend SHALL submit invoice adjustments through the existing invoice adjustment API contract and SHALL route the operator to the replacement draft document returned by the backend.

#### Scenario: Adjustment draft submits through the adjustment endpoint
- **WHEN** an operator submits a valid invoice adjustment draft from the frontend
- **THEN** the system SHALL call the invoice adjustment API for the selected source document instead of mutating the original document locally

#### Scenario: Successful adjustment opens the replacement draft
- **WHEN** the invoice adjustment API returns a replacement draft document successfully
- **THEN** the frontend SHALL navigate the operator to the returned replacement draft document so downstream review and resubmission can continue there

### Requirement: Adjusted invoice documents SHALL remain visible and understandable in review surfaces
Supported first-release invoice list and detail surfaces SHALL render adjusted document state and the relationship between an original document and its replacement draft.

#### Scenario: Adjusted invoice status remains visible in list and detail views
- **WHEN** an invoice document has entered the adjusted state
- **THEN** the frontend SHALL render that adjusted status with the same level of status visibility provided for other supported invoice lifecycle states

#### Scenario: Replacement draft shows its source document relationship
- **WHEN** a replacement draft document is viewed after a successful adjustment
- **THEN** the frontend SHALL display the originating adjusted document reference so the operator can understand the adjustment lineage
