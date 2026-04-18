## Design

### sqlutil tests

Test each of the 13 exported functions:

**Null scanning helpers** (return *type, nil if not Valid):
- `NullInt64Pointer` — Valid → returns pointer, !Valid → returns nil
- `NullIntPointer` — Valid → returns pointer to int, !Valid → returns nil
- `NullFloat64Pointer` — Valid → returns pointer, !Valid → returns nil
- `NullStringPointer` — Valid → returns pointer, !Valid → returns nil
- `NullTimePointer` — Valid → returns pointer, !Valid → returns nil

**Pointer value helpers** (return any, nil/zero if pointer is nil):
- `Int64PointerValue` — nil → nil, non-nil → dereferenced value
- `StringPointerValue` — nil → nil, non-nil → dereferenced value
- `BoolPointerValue` — nil → false, non-nil → dereferenced value
- `Float64PointerValue` — nil → nil, non-nil → dereferenced value
- `IntPointerValue` — nil → 0, non-nil → dereferenced value
- `TimePointerValue` — nil → nil, non-nil → dereferenced value
- `TimePointerDateString` — nil → nil, non-nil → formatted "2006-01-02"

**IN-clause helper**:
- `InPlaceholders` — 0 → "", 1 → "?", 3 → "?, ?, ?", negative → ""

### pagination tests

- `NormalizePage` — zero/negative page → 1, zero/negative pageSize → DefaultPageSize
- `NormalizePage` — valid values pass through
- `NormalizePage` — pageSize > MaxPageSize → MaxPageSize
- `ListResult[T]` — generic struct with Items, Total, Page, PageSize
- Constants — DefaultPageSize = 20, MaxPageSize = 100
