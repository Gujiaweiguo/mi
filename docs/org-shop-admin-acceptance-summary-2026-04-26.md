# Organization / Shop Admin Acceptance Summary (2026-04-26)

## Scope

This acceptance summary covers the first-release organization, user, role, department, store-administration, and shop/base-info administration surfaces required to bootstrap and operate the replacement system under scoped permissions.

## Spec baseline

- `openspec/specs/organization-access-control/spec.md`
- `openspec/specs/supporting-domain-management/spec.md`

## Validated head

- Validated repository head: `e0ba41653750c59b270ad99635a3ab79747b9a7c`
- Verification root: `artifacts/verification/e0ba41653750c59b270ad99635a3ab79747b9a7c/`
  - `unit.json` — PASS (`766/766`)
  - `integration.json` — PASS (`581/581`)
  - `e2e.json` — PASS (`42/42`)

## Executed checks

### Auth and shell E2E coverage

1. `task14-shell.spec.ts` — guest redirect and login surface
   - Verifies protected dashboard routes redirect unauthenticated operators to the login page.
   - Result: PASS

2. `task14-shell.spec.ts` — permission-aware navigation and shared workbench behavior
   - Verifies scoped navigation visibility after login and operator-facing filtering behavior on protected routes.
   - Result: PASS

3. `task14-shell.spec.ts` — unauthorized route handling and centralized API error surfacing
   - Verifies forbidden redirects and centralized error visibility for unavailable backend responses.
   - Result: PASS

4. `live-stack-auth.spec.ts`
   - Verifies authentication against the live stack and successful dashboard rendering on the validated head.
   - Result: PASS

### Base-info / shop-admin E2E coverage

5. `task16-baseinfo.spec.ts`
   - Verifies store-type creation from the base-info admin view after authenticated entry through the shared shell.
   - Result: PASS

### Integration and permission coverage

6. `TestIntegrationAuthAndOrgRoutes`
   - Verifies login, `/api/auth/me`, protected org routes, and seeded base-info endpoints through the full router stack.
   - Verifies create/update flows for store type, shop type, currency type, and trade definition under authenticated access.
   - Result: PASS

7. `backend/internal/http/middleware/auth_test.go`
   - Verifies permission middleware rejects missing session context and forbidden actions on protected routes.
   - Result: PASS

### Request-validation coverage

8. `backend/internal/http/handlers/baseinfo_test.go`
   - Verifies base-info create/update endpoints reject invalid JSON, invalid IDs, and missing required fields across store/shop-related admin surfaces.
   - Result: PASS

9. `backend/internal/http/handlers/org_test.go`
   - Covers org handler construction and route wiring smoke-level validation.
   - Result: PASS

### Full verification gates on validated head

10. `scripts/verification/run-unit.sh e0ba41653750c59b270ad99635a3ab79747b9a7c`
    - Result: PASS

11. `scripts/verification/run-integration.sh e0ba41653750c59b270ad99635a3ab79747b9a7c`
    - Result: PASS

12. `scripts/verification/run-e2e.sh e0ba41653750c59b270ad99635a3ab79747b9a7c`
    - Result: PASS

13. `scripts/archive-ready.sh`
    - Result: PASS (`Archive Ready: YES`)

## Acceptance outcomes

- Fresh-start org/auth bootstrap provides working login entry, authenticated shell access, and protected route enforcement.
- Scoped permission handling blocks unauthorized actions while allowing permitted operator navigation and workflow entry points.
- Seeded organization and base-info data remain reachable through authenticated routes.
- Store/shop-oriented base-info administration supports supported create/update maintenance with validation failures surfaced before bad writes are accepted.
- The combined organization + shop-admin slice is covered by current commit-scoped verification evidence on the validated head.

## Traceability notes

- Auth domain: `backend/internal/auth/`
- Router integration: `backend/internal/http/router_integration_test.go`
- Permission middleware tests: `backend/internal/http/middleware/auth_test.go`
- Base-info handler tests: `backend/internal/http/handlers/baseinfo_test.go`
- Frontend auth and shell surfaces: `frontend/src/views/LoginView.vue`, `frontend/src/views/UserManagementView.vue`
- Frontend base-info admin surface: `frontend/src/views/BaseInfoAdminView.vue`
- E2E coverage: `frontend/e2e/task14-shell.spec.ts`, `frontend/e2e/live-stack-auth.spec.ts`, `frontend/e2e/task16-baseinfo.spec.ts`

## Conclusion

The organization / shop-admin slice is accepted for the governed first-release scope on validated head `e0ba41653750c59b270ad99635a3ab79747b9a7c`. Fresh-start authentication, scoped protected-route behavior, and supported base-info/store/shop administration all pass with current verification evidence.
