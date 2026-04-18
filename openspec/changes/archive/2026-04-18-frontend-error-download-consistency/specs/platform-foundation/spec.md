## ADDED Requirements

### Requirement: All frontend error message handling SHALL use the shared composable
All Vue views SHALL import `getErrorMessage` from the shared composable instead of using inline ternary expressions.

#### Scenario: No inline error ternaries remain
- **WHEN** a view needs to extract an error message
- **THEN** it SHALL use the shared `getErrorMessage` from `@/composables/useErrorMessage`

### Requirement: Blob download logic SHALL use a shared composable
All Vue views that download blob data SHALL use the shared `downloadBlob` function from `@/composables/useDownload` instead of inlining the logic.

#### Scenario: downloadBlob is used for all file downloads
- **WHEN** a view needs to trigger a file download from a Blob
- **THEN** it SHALL call `downloadBlob(blob, filename)` from the shared composable
