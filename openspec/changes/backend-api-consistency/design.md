## Design

### 1. Reporting Response Wrapper

Change `c.JSON(http.StatusOK, result)` to `c.JSON(http.StatusOK, gin.H{"report": result})`.

This matches the pattern used by all other handlers (e.g., lease returns `gin.H{"lease": ...}`, billing returns `gin.H{"charges": ...}`).

### 2. IN-clause Placeholder Helper

Add to `sqlutil/helpers.go`:

```go
func InPlaceholders(n int) string {
    if n <= 0 {
        return ""
    }
    parts := make([]string, n)
    for i := range parts {
        parts[i] = "?"
    }
    return strings.Join(parts, ", ")
}
```

Refactor `billing/repository.go:GetChargeLinesByIDs` and `invoice/repository.go:CountReservedChargeLines` to use it.
