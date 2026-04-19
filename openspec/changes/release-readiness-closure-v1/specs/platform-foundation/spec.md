## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped release evidence for the actual release head
The project SHALL treat release-readiness as a property of the exact commit being evaluated for release rather than a historical verified commit. Current release status, release-ready summaries, and supporting evidence SHALL be regenerated or revalidated for the actual current HEAD before release decisions are made. Capability and mandatory-output closure SHALL be backed by explicit current-head audit artifacts rather than inferred from directory structure or older summaries.

#### Scenario: Current-head archive-ready evidence is required for release posture
- **WHEN** the project is evaluated for release readiness
- **THEN** the exact current HEAD SHALL have matching `unit`, `integration`, and `e2e` evidence artifacts that pass for that commit

#### Scenario: Historical release-ready summaries cannot stand in for current-head evidence
- **WHEN** release documentation references a validated commit older than the current HEAD
- **THEN** the project SHALL either refresh the release summary for the current HEAD or mark the release posture as blocked

#### Scenario: Preserved and redesigned capability closure is audited explicitly
- **WHEN** first-release scope is considered complete for go-live
- **THEN** each `PRESERVE` and `REDESIGN` capability SHALL map to concrete backend/frontend implementation surfaces and current evidence or be flagged as a gap

#### Scenario: Mandatory outputs are audited explicitly
- **WHEN** mandatory first-release outputs are considered closed for release
- **THEN** each output in the frozen output catalog SHALL map to an implementation surface and current evidence or be flagged as a gap with blocking status

### Requirement: The system SHALL publish a binary release-decision packet
The project SHALL produce an operator-readable release-decision packet for the evaluated current HEAD that includes verification status, rehearsal status, capability audit status, output audit status, unresolved blockers, and a binary `GO` / `NO-GO` recommendation.

#### Scenario: Release packet ends in a binary decision
- **WHEN** release readiness is summarized for the current HEAD
- **THEN** the resulting packet SHALL end with explicit `GO` or `NO-GO` and SHALL identify any blockers that drive a `NO-GO` outcome

#### Scenario: Release packet does not hide unresolved blockers
- **WHEN** one or more blocking issues remain open
- **THEN** the release packet SHALL list them directly and SHALL NOT recommend `GO`
