## ADDED Requirements

### Requirement: The frontend SHALL provide a reusable pagination composable for list views
The frontend SHALL expose a `usePagination` composable that manages page, pageSize, and total refs. The composable SHALL provide a `paginationParams` computed property returning `{ page, page_size }` suitable for spreading into API call parameters. The composable SHALL provide a `resetPage()` function that sets page to 1.

#### Scenario: Composable initializes with default page size
- **WHEN** `usePagination()` is called without arguments
- **THEN** it SHALL return `page` ref initialized to 1, `pageSize` ref initialized to 20, `total` ref initialized to 0

#### Scenario: Pagination params are computed for API calls
- **WHEN** a view spreads `paginationParams` into an API call
- **THEN** the API call SHALL receive `page` and `page_size` query parameters with the current values

#### Scenario: Page resets when filters change
- **WHEN** `resetPage()` is called
- **THEN** `page` SHALL be set to 1

### Requirement: Business list views SHALL pass pagination parameters to paginated API endpoints
LeaseListView, BillingInvoicesView, BillingChargesView, and ReceivablesView SHALL pass `page` and `page_size` parameters to their respective list API calls. When the API returns a `PaginatedResponse`, the view SHALL update the composable's total from `response.data.total`.

#### Scenario: Lease list view passes pagination params
- **WHEN** LeaseListView calls `listLeases()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

#### Scenario: Invoice list view passes pagination params
- **WHEN** BillingInvoicesView calls `listInvoices()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

#### Scenario: Charges list view passes pagination params
- **WHEN** BillingChargesView calls `listCharges()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

#### Scenario: Receivables list view passes pagination params
- **WHEN** ReceivablesView calls `listReceivables()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

### Requirement: Business list views SHALL render pagination controls
LeaseListView, BillingInvoicesView, BillingChargesView, and ReceivablesView SHALL render an `<el-pagination>` component below their data table. The pagination component SHALL display page numbers, support page size selection, and show the total count. Clicking a page number SHALL reload data for that page.

#### Scenario: User navigates to page 2
- **WHEN** the user clicks page 2 in the pagination control
- **THEN** the view SHALL set page to 2 and reload data with the updated pagination parameters

#### Scenario: User changes page size
- **WHEN** the user selects a different page size from the pagination control
- **THEN** the view SHALL reset page to 1, update page_size, and reload data
