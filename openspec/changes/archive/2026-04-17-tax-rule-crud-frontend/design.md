## Context

The tax exports view (`TaxExportsView.vue`) currently renders two cards: a voucher export form (rule set selector, date range, export button) and a read-only table of available rule sets. The backend already supports `POST /tax/rule-sets` for creating or updating rule sets via `UpsertRuleSet`, but there is no frontend trigger for this endpoint. Operators can see which rule sets exist, but they cannot create new ones or edit existing ones without leaving the UI.

This is the last missing piece of the tax export operator workflow. All backend infrastructure (persistence, validation, voucher generation) is complete and tested. The gap is purely in the frontend management surface.

Stakeholders:

- Operators who need to define and maintain tax accounting rule sets.
- Acceptance reviewers who need to verify the complete tax export workflow end to end.

## Goals / Non-Goals

**Goals:**

- Add a dialog-based create/edit workflow for tax rule sets inside the existing `TaxExportsView.vue`.
- Support the full `UpsertRuleSet` payload: code, name, document type, and a variable-length rules table.
- Enable row-click editing so operators can adjust existing rule sets.
- Keep the implementation consistent with the codebase's existing dialog patterns (el-dialog, inline form, reactive state).

**Non-Goals:**

- No backend API changes. The existing `POST /tax/rule-sets` contract is the target.
- No delete functionality. The backend exposes only upsert, and there is no delete endpoint.
- No new Vue files. The dialog lives inline in the existing view.
- No routing changes. The dialog is triggered by UI interaction, not navigation.
- No validation beyond what the backend already enforces. Frontend validation is limited to required-field gating.

## Decisions

### Decision 1: Use el-dialog for the create/edit form

The rule set upsert dialog will use `el-dialog` rather than `el-drawer`.

Why:

- Other admin views in the codebase (customer, brand, budget) use `el-dialog` for similar create/edit flows. Matching the existing pattern keeps the UI consistent.
- The form content is bounded: a header section (3 fields) plus a rules table. A dialog is the right size for this.

Alternatives considered:

- `el-drawer`: would work but diverges from existing patterns without a clear benefit.
- Separate route/page: rejected because this is a management action within the tax exports workflow, not a distinct page.

### Decision 2: Rules table inside the dialog with add/remove row controls

The dialog will contain an `el-table` (or equivalent structured list) for rule entries, with "Add Row" and per-row "Remove" controls.

Why:

- Rules are a variable-length collection. Operators need to add and remove entries freely.
- The backend requires `sequence_no` to be set, so the frontend should auto-assign sequence numbers when adding rows.

Alternatives considered:

- Fixed number of rule slots: rejected because rule sets vary in length.
- Separate sub-dialog for rules: rejected because it adds unnecessary nesting.

### Decision 3: Document type uses fixed select options

The `document_type` field will use a fixed `el-select` with options `invoice` and `receipt`, matching the backend's expected values.

Why:

- The backend validates `document_type` as a required string, and the business domain currently recognizes only these two types.
- A fixed select prevents freeform input that would be rejected by the backend.

### Decision 4: Entry side uses debit/credit select

The `entry_side` field for each rule will use a fixed `el-select` with `debit` and `credit` options.

Why:

- The backend's `EntrySide` type expects exactly these two values.

### Decision 5: Row-click on the rule sets table opens the dialog in edit mode

Clicking a row in the available rule sets table will open the dialog pre-populated with that rule set's data.

Why:

- This follows the existing codebase convention where table rows are clickable for editing.
- The "Add Rule Set" button in the card header opens the dialog in create mode (empty form).

### Decision 6: After successful upsert, reload the rule sets list

On successful submission, the dialog closes and the rule sets list refreshes.

Why:

- The simplest way to reflect changes. No optimistic update complexity needed for a low-frequency admin action.

## Risks / Trade-offs

- **[Risk] The dialog may feel crowded with the rules table** → **Mitigation:** use a scrollable content area inside the dialog and sensible column sizing.
- **[Risk] Auto-assigned sequence numbers may confuse operators if they reorder rows** → **Mitigation:** keep sequence numbers auto-incremented on add and stable on edit; operators who need different ordering can delete and re-add.
- **[Risk] No delete endpoint means operators cannot remove unwanted rule sets** → **Mitigation:** accepted scope limitation. Upsert can overwrite existing rule sets by code, which covers the edit use case.

## Migration Plan

1. Add `upsertTaxRuleSet` to `tax.ts` with a typed request interface.
2. Add dialog state, form model, and handlers to `TaxExportsView.vue`.
3. Add the dialog template with header form and rules table.
4. Add i18n keys to both locale files.
5. Verify the full create/edit/refresh cycle works.

Rollback is straightforward: remove the dialog template, handlers, and API function. The view returns to its read-only state with no data or backend impact.

## Open Questions

- Should the rules table support drag-and-drop reordering for `sequence_no`, or is add/remove sufficient for first release?
- Should clicking a rule set row require a dedicated edit button/column, or is row-click sufficient?
