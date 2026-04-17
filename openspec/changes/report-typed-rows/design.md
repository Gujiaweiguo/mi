## Context

The reporting service's `runReport()` method (service.go:128-351) contains a 220-line switch-case that maps 18 typed row structs into `[]map[string]any`. Each case follows the same pattern: call repository, get typed slice, allocate map slice, loop and build map literal, return columns and rows. The conversion keys (e.g., `"store_name"`, `"period_receivable"`) have no compile-time link to the struct field names.

## Goals / Non-Goals

**Goals:**
- Each row struct owns its own `ToMap()` method — the mapping is defined once, next to the struct.
- The `runReport()` switch-case is reduced to a thin dispatch that calls the repository and maps via `ToMap()`.
- `ReportR19` constant is added to match the pattern of all other report IDs.
- Identical JSON output — zero behavioral change.

**Non-Goals:**
- Changing the `Result.Rows` type from `[]map[string]any` to a generic type — the JSON API contract must stay the same.
- Reflection-based struct-to-map conversion — explicit is better for 18 report types.
- Changing the Excel export logic (it already reads `map[string]any` correctly).
- Adding new reports or changing report column definitions.

## Decisions

### Decision 1 — Explicit ToMap() methods over reflection

**Choice:** Each row struct gets a hand-written `ToMap() map[string]any` method.

**Rationale:** Reflection adds import complexity, hides field-to-key mappings, and makes debugging harder. With 18 reports, the explicit approach is manageable and grep-friendly. The key-to-field mapping is visible at a glance.

### Decision 2 — AgingBuckets gets its own ToMap()

**Choice:** `AgingBuckets.ToMap()` returns the 10-key aging map. R08/R09/R16/R17 compose it with their own fields.

**Rationale:** The aging helper functions (`agingCustomerRow`, `agingDepartmentRow`) currently live in service.go. Moving the core map logic to a method on `AgingBuckets` keeps it with the type definition.

### Decision 3 — Keep switch-case for dispatch, simplify bodies

**Choice:** Keep the switch-case structure in `runReport()` but reduce each case to 3-4 lines (call repo, build rows via `ToMap()`, return).

**Rationale:** A type-switch or interface dispatch would require changing the repository interface for all 18 methods. Not worth the complexity. The switch-case is clear and each branch is now trivial.

## Risks / Trade-offs

- **Behavioral equivalence risk** → Mitigated by existing integration tests that exercise all 19 reports through the service. If `ToMap()` produces different keys, tests will fail.
- **Duplication of key names** → The `ToMap()` method and the column labels function both define the same keys. This is inherent to the current architecture and not made worse by this change.
