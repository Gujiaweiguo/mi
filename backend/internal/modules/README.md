# Backend Modules

This directory is a historical placeholder from earlier migration planning.

The current backend implementation does **not** place business modules under
`backend/internal/modules/`. Instead, first-release capabilities live directly
under `backend/internal/`, for example:

- `auth/`, `baseinfo/`, `structure/`
- `lease/`, `billing/`, `invoice/`, `workflow/`
- `reporting/`, `taxexport/`, `docoutput/`, `excelio/`
- `sales/`, `masterdata/`, `dashboard/`

When reviewing backend capability coverage, use the concrete domain directories
under `backend/internal/` and the current OpenSpec change artifacts rather than
this placeholder path.
