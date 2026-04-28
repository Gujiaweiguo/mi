## 1. Backend overtime downstream integration

- [x] 1.1 Trace the current overtime-generated charge model into the shared billing/invoice composition path and identify the provenance fields needed for downstream attribution.
- [x] 1.2 Extend backend billing generation and invoice/receivable selection logic so approved overtime-generated charges are eligible downstream inputs while cancelled, unapproved, or duplicate overtime output remains excluded.
- [x] 1.3 Preserve overtime source attribution and approval lineage in downstream financial records and queries so operators can distinguish overtime-derived lines from standard lease-generated lines.

## 2. Reporting and document-output visibility

- [x] 2.1 Update bounded first-release financial review and reporting datasets that depend on billing or receivable state so overtime-derived amounts are included instead of omitted.
- [x] 2.2 Update any supported invoice-facing or equivalent financial document loaders/renderers that would otherwise omit overtime-derived billed lines.
- [x] 2.3 Keep the change inside the frozen first-release report inventory and reject any implementation that widens scope into unrelated legacy reports or membership flows.

## 3. Frontend operator surfaces

- [x] 3.1 Expose overtime attribution in the affected billing, invoice, receivable, or report-facing frontend views wherever downstream financial records now include overtime-derived lines.
- [x] 3.2 Add or update frontend API types and operator flows needed to present overtime-backed downstream financial state without side-channel reconciliation.

## 4. Verification and evidence

- [x] 4.1 Add backend unit and integration coverage for overtime downstream eligibility, duplicate safety, attribution, and bounded reporting/output behavior.
- [x] 4.2 Add frontend unit and E2E coverage for operator-visible overtime downstream billing, invoice, receivable, or reporting flows changed by this slice.
- [x] 4.3 For CI readiness, produce passing commit-scoped evidence files at `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json` for the current HEAD commit.
- [x] 4.4 Before archive, produce passing commit-scoped evidence files at `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` for the current HEAD commit.
