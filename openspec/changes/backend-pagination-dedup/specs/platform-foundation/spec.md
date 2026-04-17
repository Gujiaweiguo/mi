## MODIFIED Requirements

### Requirement: Pagination types SHALL be consolidated into a shared package
All backend modules that support paginated list endpoints SHALL use a shared `pagination` package for the `ListResult` type, `NormalizePage` function, and pagination constants instead of defining their own local duplicates. The shared package SHALL use `int64` for total count and `20` for default page size.

#### Scenario: Shared pagination package exists
- **WHEN** a module needs a paginated result type
- **THEN** it SHALL use `pagination.ListResult[T]` instead of defining its own struct

#### Scenario: Shared normalize function exists
- **WHEN** a repository needs to clamp page/pageSize values
- **THEN** it SHALL call `pagination.NormalizePage()` instead of a local duplicate
