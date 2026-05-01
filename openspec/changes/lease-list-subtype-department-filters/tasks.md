## 1. OpenSpec

- [x] 1.1 Create proposal, design, spec delta, and tasks for lease list subtype / department filtering

## 2. Backend lease list filters

- [x] 2.1 Extend lease list filter types to model optional `subtype` and `department_id`
- [x] 2.2 Update lease list handler parsing to accept `subtype` and `department_id` query parameters
- [x] 2.3 Update lease repository list queries to apply optional subtype and department constraints without changing current default behavior
- [x] 2.4 Add backend tests covering invalid `department_id` parsing and subtype/department filter application

## 3. Frontend lease list filters

- [x] 3.1 Extend frontend lease list API params with optional `subtype` and `department_id`
- [x] 3.2 Add subtype and department filter controls to `LeaseListView.vue`
- [x] 3.3 Pass subtype and department filters through the lease list request while preserving current pagination and reset behavior
- [x] 3.4 Add matching English and Chinese i18n strings
- [x] 3.5 Add or update unit tests covering new lease list filter request wiring

## 4. Verification

- [x] 4.1 Run focused backend tests for lease list filtering
- [x] 4.2 Run focused frontend tests for lease list filtering
- [x] 4.3 Run regression checks required by the touched files (`npm run lint`, `npm run build`, relevant Go tests)
