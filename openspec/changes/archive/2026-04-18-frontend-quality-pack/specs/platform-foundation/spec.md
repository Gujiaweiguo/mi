## ADDED Requirements

### Requirement: Frontend error message extraction SHALL use a shared composable
All Vue views that need to extract error messages from caught exceptions SHALL use the shared `getErrorMessage` function from `@/composables/useErrorMessage` instead of defining their own local copies.

#### Scenario: getErrorMessage is imported from shared composable
- **WHEN** a view needs to extract an error message from a caught exception
- **THEN** it SHALL import and use `getErrorMessage` from `@/composables/useErrorMessage`

### Requirement: All data-loading views SHALL display loading indicators
All Vue views that fetch data from the API SHALL use `v-loading` to display a loading indicator while data is being fetched. The loading state SHALL be set to true before the API call and false after completion (success or error).

#### Scenario: Loading indicator shown during data fetch
- **WHEN** a user navigates to a view that fetches data
- **THEN** a loading spinner SHALL be displayed until the data fetch completes

### Requirement: All form dialogs SHALL validate required fields client-side
All Vue views with create/edit form dialogs SHALL use Element Plus `:rules` for client-side validation of required fields (code, name, etc.) before submitting to the API.

#### Scenario: Form validation prevents empty required fields
- **WHEN** a user submits a form with empty required fields
- **THEN** the form SHALL display validation errors without making an API call
