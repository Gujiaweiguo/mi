## Context

Six modules duplicate pagination boilerplate. Five have identical `normalizePage()` functions. Masterdata deviates with different defaults and missing JSON tags.

## Goals / Non-Goals

**Goals:**
- Single shared `pagination` package with generic `ListResult[T]`, `NormalizePage()`, and constants.
- All 6 modules import from the shared package.
- Fix masterdata inconsistencies (Total type, default page size, JSON tags).
- Identical JSON API output.

**Non-Goals:**
- Changing the `ListFilter` structs — each module has domain-specific filter fields embedded alongside Page/PageSize, so embedding a shared struct adds complexity for minimal gain.
- Extracting SQL null-pointer helpers into a separate package (separate concern).
- Changing handler-level pagination parsing (each handler reads query params differently).

## Decisions

### Decision 1 — Generic ListResult[T] over shared interface

**Choice:** Use Go 1.18+ generics: `ListResult[T any]` with `Items []T`, `Total int64`, `Page int`, `PageSize int`.

**Rationale:** Each module currently has its own typed result struct that only differs in the Items slice type. Generics eliminate all of them cleanly.

### Decision 2 — Keep ListFilter in each module

**Choice:** Don't extract ListFilter — each module keeps its own with domain-specific fields.

**Rationale:** The filter structs embed different domain fields (lease has LeaseNo/Status, billing has PeriodStart/PeriodEnd). Extracting just Page/PageSize into an embedded struct saves ~2 lines per module but adds import complexity and makes the struct definition harder to read.

### Decision 3 — int64 for Total, 20 for DefaultPageSize

**Choice:** Standardize on `int64` for Total and `20` for DefaultPageSize (the majority choice).

**Rationale:** 5 of 6 modules use int64 and default page size 20. Masterdata is the outlier and should be aligned.

## Risks / Trade-offs

- **Masterdata page size change** → From 10 to 20. This changes behavior for the masterdata list endpoints. Acceptable because 20 is the standard across the rest of the API and 10 was an accidental deviation.
- **Masterdata Total type change** → From `int` to `int64`. JSON serialization of int64 produces the same JSON number for values within int range, so no API contract change.
