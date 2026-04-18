## Context

Eight files define 25 file-private helper functions for converting between SQL nullable types and Go pointer types. All implementations are identical except `timePointerValue` which has two variants.

## Goals / Non-Goals

**Goals:**
- Single `sqlutil` package with all helpers.
- All 8 modules import from the shared package.
- Identical behavior.

**Non-Goals:**
- Using `sql.Null*` types in model structs.
- Changing the `timePointerValue` semantics for sales module.
- Adding generic versions.

## Decisions

### Decision 1 — Exported functions with clear names

**Choice:** `NullInt64Pointer`, `NullStringPointer`, `NullTimePointer`, `NullFloat64Pointer`, `NullIntPointer`, `Int64PointerValue`, `StringPointerValue`, `TimePointerValue`, `TimePointerDateString`, `BoolPointerValue`, `Float64PointerValue`.

**Rationale:** Exported names follow Go convention. The `Null*` prefix clearly indicates SQL null scanning. The `*Value` suffix indicates the reverse direction (pointer to any).

### Decision 2 — Keep both timePointerValue variants

**Choice:** `TimePointerValue(*time.Time) any` returns the time value, `TimePointerDateString(*time.Time) any` returns formatted string.

**Rationale:** Sales module formats dates as strings for display/export. The others store raw time values. Both behaviors are valid; just needs different names.
