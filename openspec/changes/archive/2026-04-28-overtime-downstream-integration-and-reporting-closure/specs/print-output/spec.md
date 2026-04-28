## ADDED Requirements

### Requirement: Supported financial document output SHALL preserve overtime-derived billing data
The system SHALL include overtime-derived financial lines and trusted attribution in any supported invoice-facing or equivalent downstream document output whose rendered financial content is sourced from billing, invoice, or receivable composition.

#### Scenario: Supported financial document renders overtime-derived line items
- **WHEN** an authorized operator renders a supported financial document that includes billed overtime-derived charges
- **THEN** the system SHALL include the overtime-derived financial lines in the rendered HTML/PDF output with the same trusted composition path used for other financial line items

#### Scenario: Document output does not silently omit downstream overtime data
- **WHEN** a supported print or document output depends on downstream financial records that include overtime-derived charges
- **THEN** the system SHALL either render those overtime-backed values correctly or reject the unsupported render path instead of silently producing misleading output
