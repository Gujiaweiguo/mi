## Why

The current system is a single-server ASP.NET WebForms + SQL Server application with legacy workflow, print, Excel, and tax behaviors embedded across server-rendered pages, database-centric logic, and old output tooling. A first-release replacement is needed now to move to a maintainable frontend-backend separated architecture on Vue, Go, and MySQL 8 while preserving required business operations for all modules except membership.

## What Changes

- Replace the legacy WebForms runtime with a Vue 3 frontend and Go modular monolith backend.
- Switch production persistence to MySQL 8 at go-live with fresh base-data initialization and no historical transaction migration.
- Rebuild first-release business capability for all non-membership domains, with Lease, Bill, and Invoice as the highest-priority operational chain.
- Introduce redesigned auth/permission, approval workflow, tax rule/export, Excel import/export, and print/document generation capabilities.
- Add separate development, test, and production operating modes, with Docker Compose for test/production and mounted config/runtime data.
- Add TDD-first verification, automated artifact checks, and cutover/rollback runbooks.
- **BREAKING**: membership (`Associator`) is excluded from the first release.
- **BREAKING**: email delivery is deferred from the first release.
- **BREAKING**: legacy UI/process flow and permission model are not preserved one-to-one; they are intentionally redesigned while keeping required business outcomes.

## Capabilities

### New Capabilities
- `platform-foundation`: frontend/backend project scaffold, configuration, test harness, and MySQL bootstrap foundations for the new stack.
- `organization-access-control`: fresh-init users, roles, departments, shops/buildings, and redesigned scoped permissions.
- `lease-contract-management`: Lease contract lifecycle, amendments, terminations, and billing prerequisites.
- `billing-and-invoicing`: charge generation, bill/invoice lifecycle, numbering, and financial state transitions.
- `workflow-approvals`: configurable approval templates, transitions, audit history, and idempotent side-effect dispatch.
- `tax-document-and-excel-output`: tax rule/export, print/document generation, and Excel import/export for mandatory first-release outputs.
- `supporting-domain-management`: non-membership supporting modules such as BaseInfo/admin, Shop, Sell, RentableArea, and frozen Generalize outputs.
- `deployment-and-cutover-operations`: Docker Compose environments, mounted runtime operations, bootstrap scripts, rehearsal, and rollback criteria.

### Modified Capabilities
- None.

## Impact

- Affects the legacy replacement scope rooted in `legacy_code/MI_net.sln` and the business modules under Lease, Bill, Invoice, WorkFlow, BaseInfo, Shop, Sell, RentableArea, and Generalize.
- Introduces a new Vue 3 + Go + MySQL 8 stack with Docker Compose-based test/production operations.
- Requires new specs for business capability slices, outputs, and cutover operations.
- Establishes a new acceptance contract based on executable tests and generated evidence rather than screen-by-screen parity.
