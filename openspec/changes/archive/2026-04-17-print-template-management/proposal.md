## Why

The backend `POST /print/templates` endpoint (`UpsertTemplate`) is fully implemented and accepts a complete template payload with code, name, document type, output mode, title, subtitle, header lines, and footer lines. However, the frontend `PrintPreviewView.vue` can only list templates and render PDFs. There is no way for operators to create or edit print templates from the UI. This blocks the full print output workflow: operators must have API-level access or rely on seed data to define the templates that drive document generation.

## What Changes

- Add an `upsertPrintTemplate` API function to `frontend/src/api/print.ts` that calls `POST /print/templates` with the full template payload.
- Add a create/edit dialog to `frontend/src/views/PrintPreviewView.vue` with form fields for all template properties, including dynamic header_lines and footer_lines lists, triggered by an "Add Template" button in the templates card header and by clicking a table row for editing.
- Add i18n labels for all dialog fields, buttons, and feedback messages to both `zh-CN.ts` and `en-US.ts`.
- No backend changes. No new routes. No new Vue files.

## Capabilities

### New Capabilities

- `print-output`: Frontend CRUD for print template management, enabling operators to create and edit print document templates directly from the print preview view.

### Modified Capabilities

- None. The print rendering workflow itself (PDF generation, template listing) is unchanged; this change adds the missing management surface.

## Impact

- `frontend/src/api/print.ts`: add `upsertPrintTemplate` function and request interface.
- `frontend/src/views/PrintPreviewView.vue`: add dialog component, dialog state, row-click handler, and submit logic.
- `frontend/src/i18n/messages/zh-CN.ts` and `en-US.ts`: add labels under the existing `printPreview` namespace for dialog fields, actions, and feedback.
- No backend, routing, or database changes.
