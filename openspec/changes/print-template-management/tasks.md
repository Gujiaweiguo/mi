## 1. API layer

- [ ] 1.1 Add an `UpsertPrintTemplateRequest` interface to `frontend/src/api/print.ts` with fields: `code` (string, required), `name` (string, required), `document_type` (string, required), `output_mode` (string, required), `title` (string, required), `subtitle` (string, optional), `header_lines` (string[], optional), `footer_lines` (string[], optional).
- [ ] 1.2 Add an `upsertPrintTemplate` function to `frontend/src/api/print.ts` that sends a `POST /print/templates` request with the typed payload and returns the backend response.

## 2. Dialog state and handlers

- [ ] 2.1 Add dialog visibility state, form model (code, name, document_type, output_mode, title, subtitle, header_lines, footer_lines), and an editing flag to `PrintPreviewView.vue` script setup to track whether the dialog is in create or edit mode.
- [ ] 2.2 Add an `openCreateDialog` function that resets the form model to empty defaults (empty strings, empty arrays) and sets the dialog visible.
- [ ] 2.3 Add an `openEditDialog` function that populates the form model from a selected `PrintTemplate` (spreading all fields including header_lines and footer_lines arrays) and sets the dialog visible in edit mode.
- [ ] 2.4 Add `addHeaderLine` and `removeHeaderLine` functions that append or remove a string from the `header_lines` array in the form model. Add matching `addFooterLine` and `removeFooterLine` functions for `footer_lines`.
- [ ] 2.5 Add `headerLineInput` and `footerLineInput` reactive refs to hold the current typed value before it is committed to the respective array.

## 3. Dialog template

- [ ] 3.1 Add an `el-dialog` to `PrintPreviewView.vue` template with a form containing: code (el-input), name (el-input), document_type (el-select with invoice/receipt/lease_contract options), output_mode (el-select with html/pdf options), title (el-input), subtitle (el-input), header_lines (dynamic el-tag list with inline input and add button), footer_lines (dynamic el-tag list with inline input and add button).
- [ ] 3.2 Add a "Submit" button in the dialog footer that calls `upsertPrintTemplate` with the form model, handles success (close dialog, reload templates, show success feedback) and error (show error feedback, keep dialog open).
- [ ] 3.3 Add an "Add Template" button to the templates card header (next to the existing total tag) that calls `openCreateDialog`, and wire row-click on the templates table to call `openEditDialog` with the clicked row's data.

## 4. i18n labels

- [ ] 4.1 Add i18n keys under `printPreview.dialog` in both `zh-CN.ts` and `en-US.ts` for: dialog title (create/edit), field labels (code, name, documentType, outputMode, title, subtitle, headerLines, footerLines), action labels (addTemplate, submit, addLine), document type options (invoice, receipt, leaseContract), output mode options (html, pdf), and success/error feedback messages for upsert.

## 5. Verification

- [ ] 5.1 Run the frontend typecheck and build to confirm no type errors or build regressions from the added dialog, API function, and i18n keys.
