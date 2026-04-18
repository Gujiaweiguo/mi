## ADDED Requirements

### Requirement: SQL null-pointer helpers SHALL be consolidated into a shared package
All backend modules that scan SQL nullable types into Go pointers SHALL use a shared `sqlutil` package for null-to-pointer and pointer-to-value conversion functions instead of defining their own local duplicates.

#### Scenario: Shared sqlutil package provides all null helpers
- **WHEN** a repository needs to scan nullable SQL columns into Go pointers
- **THEN** it SHALL use `sqlutil.NullInt64Pointer()`, `sqlutil.NullStringPointer()`, etc. instead of local helper functions
