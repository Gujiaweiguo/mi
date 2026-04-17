## Context

The print preview view (`PrintPreviewView.vue`) currently renders two cards: a PDF render form (template selector, document IDs input, generate button) and a read-only table of available templates. The backend already supports `POST /print/templates` for creating or updating templates via `UpsertTemplate`, but there is no frontend trigger for this endpoint. Operators can see which templates exist, but they cannot create new ones or edit existing ones without leaving the UI.

This is the last missing piece of the print template operator workflow. All backend infrastructure (persistence, validation, rendering) is complete and tested. The gap is purely in the frontend management surface.

Stakeholders:

- Operators who need to define and maintain print document templates.
- Acceptance reviewers who need to verify the complete print output workflow end to end.

## Goals / Non-Goals

**Goals:**

- Add a dialog-based create/edit workflow for print templates inside the existing `PrintPreviewView.vue`.
- Support the full `UpsertTemplate` payload: code, name, document_type, output_mode, title, subtitle, header_lines, and footer_lines.
- Enable row-click editing so operators can adjust existing templates.
- Keep the implementation consistent with the codebase's existing dialog patterns (el-dialog, inline form, reactive state).

**Non-Goals:**

- No backend API changes. The existing `POST /print/templates` contract is the target.
- No delete functionality. The backend exposes only upsert, and there is no delete endpoint.
- No new Vue files. The dialog lives inline in the existing view.
- No routing changes. The dialog is triggered by UI interaction, not navigation.
- No validation beyond what the backend already enforces. Frontend validation is limited to required-field gating.

## Decisions

### Decision 1: Use el-dialog for the create/edit form

The template upsert dialog will use `el-dialog` rather than `el-drawer`.

Why:

- Other admin views in the codebase (customer, brand, budget, tax rule sets) use `el-dialog` for similar create/edit flows. Matching the existing pattern keeps the UI consistent.
- The form content is bounded: a header section (6 fields) plus two dynamic tag lists. A dialog is the right size for this.

Alternatives considered:

- `el-drawer`: would work but diverges from existing patterns without a clear benefit.
- Separate route/page: rejected because this is a management action within the print preview workflow, not a distinct page.

### Decision 2: header_lines and footer_lines use el-tag with dynamic add/remove

The `header_lines` and `footer_lines` fields will render as dynamic lists of `el-tag` elements with an inline input for adding new entries and a close icon for removing them.

Why:

- These are simple string arrays. A tag-based input is the standard Element Plus pattern for managing lists of short strings (company names, addresses, notice text).
- Each line is a discrete value, not a structured object. Tags are the right abstraction.

Alternatives considered:

- Textarea with line-break parsing: rejected because it hides the list nature of the data and makes editing individual lines awkward.
- Separate sub-dialog for lines: rejected because it adds unnecessary nesting for a simple string list.

### Decision 3: Document type and output mode use fixed select options

The `document_type` field will use a fixed `el-select` with options `invoice`, `receipt`, and `lease_contract`. The `output_mode` field will use a fixed `el-select` with options `html` and `pdf`.

Why:

- The backend validates these as required strings with a known set of acceptable values.
- Fixed selects prevent freeform input that would be rejected by the backend.

### Decision 4: Row-click on the templates table opens the dialog in edit mode

Clicking a row in the available templates table will open the dialog pre-populated with that template's data.

Why:

- This follows the existing codebase convention where table rows are clickable for editing, matching the pattern used in TaxExportsView.
- The "Add Template" button in the card header opens the dialog in create mode (empty form).

### Decision 5: After successful upsert, reload the templates list

On successful submission, the dialog closes and the templates list refreshes.

Why:

- The simplest way to reflect changes. No optimistic update complexity needed for a low-frequency admin action.

### Decision 6: No delete button

The dialog and table will not include any delete controls.

Why:

- The backend does not expose a delete endpoint. Adding a delete button would require backend changes that are out of scope.
- Upsert can overwrite existing templates by code, which covers the edit use case.

## Risks / Trade-offs

- **[Risk] The dialog may feel tall with 6 header fields plus two dynamic lists** → **Mitigation:** use a scrollable content area inside the dialog. Group the form into logical sections (identity, output settings, header/footer lines).
- **[Risk] Tag-based line editing is unfamiliar for long text entries** → **Mitigation:** header_lines and footer_lines are typically short (company names, addresses). If operators need longer text, the inline input supports typing before committing.
- **[Risk] No delete endpoint means operators cannot remove unwanted templates** → **Mitigation:** accepted scope limitation. Upsert can overwrite existing templates by code, which covers the edit use case.

## Migration Plan

1. Add `upsertPrintTemplate` to `print.ts` with a typed request interface.
2. Add dialog state, form model, and handlers to `PrintPreviewView.vue`.
3. Add the dialog template with header form and dynamic tag lists.
4. Add i18n keys to both locale files.
5. Verify the full create/edit/refresh cycle works.

Rollback is straightforward: remove the dialog template, handlers, and API function. The view returns to its read-only state with no data or backend impact.

## Open Questions

- Should `header_lines` and `footer_lines` support reordering via drag, or is add/remove sufficient for first release?
- Should the document_type select be extensible (driven by backend enum) or hardcoded for first release?
