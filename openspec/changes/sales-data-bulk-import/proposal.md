## Why

The current Sell-domain workflow only supports one-row-at-a-time entry for daily sales and customer traffic, while a meaningful share of first-release Generalize reports depends on those datasets being loaded continuously and in volume. A bulk Excel import path is needed now so operators can refresh sales and traffic data at operational scale without manual row-by-row entry.

## What Changes

- Add downloadable Excel templates for daily shop sales and customer traffic imports, including the reference data operators need to prepare valid files.
- Add bulk import flows for daily shop sales and customer traffic with deterministic validation, row-level diagnostics, and idempotent upsert behavior.
- Extend the Sales admin experience so operators can upload import workbooks, review import results, and continue using the existing single-record entry path when needed.
- Define the import contract so report-driving sales and traffic datasets can be refreshed in batches without partial trusted writes on invalid files.
- Keep first-release scope bounded to non-membership operational sales and traffic data; no membership-related import paths are introduced.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `tax-document-and-excel-output`: expand the Excel import/export contract to include mandatory sales and customer-traffic template download plus batch import validation and diagnostics.
- `supporting-domain-management`: extend the Sell-domain operating model from single-record maintenance to operator-facing batch sales and traffic ingestion that supports downstream Generalize reporting.

## Impact

- Affects backend Excel I/O and sales ingestion paths under `backend/internal/excelio/` and `backend/internal/sales/`.
- Affects the operator-facing Sales admin UI under `frontend/src/views/SalesAdminView.vue` and related frontend API surfaces.
- Strengthens the data-loading contract for downstream Generalize reports that depend on `daily_shop_sales` and `customer_traffic`.
- Requires automated validation coverage for template download, invalid import rejection, diagnostic reporting, and successful bulk upsert flows.
