package sqlutil

import (
	"database/sql"
	"time"
)

// Null scanning helpers: convert sql.Null* to *type (nil if not Valid)

func NullInt64Pointer(v sql.NullInt64) *int64 {
	if v.Valid {
		return &v.Int64
	}
	return nil
}

func NullIntPointer(v sql.NullInt64) *int {
	if v.Valid {
		i := int(v.Int64)
		return &i
	}
	return nil
}

func NullFloat64Pointer(v sql.NullFloat64) *float64 {
	if v.Valid {
		return &v.Float64
	}
	return nil
}

func NullStringPointer(v sql.NullString) *string {
	if v.Valid {
		return &v.String
	}
	return nil
}

func NullTimePointer(v sql.NullTime) *time.Time {
	if v.Valid {
		return &v.Time
	}
	return nil
}

// Pointer value helpers: convert *type to any (nil if pointer is nil)

func Int64PointerValue(v *int64) any {
	if v != nil {
		return *v
	}
	return nil
}

func StringPointerValue(v *string) any {
	if v != nil {
		return *v
	}
	return nil
}

func BoolPointerValue(v *bool) any {
	if v != nil {
		return *v
	}
	return false
}

func Float64PointerValue(v *float64) any {
	if v != nil {
		return *v
	}
	return nil
}

func IntPointerValue(v *int) any {
	if v != nil {
		return *v
	}
	return 0
}

func TimePointerValue(v *time.Time) any {
	if v != nil {
		return *v
	}
	return nil
}

func TimePointerDateString(v *time.Time) any {
	if v != nil {
		return v.Format("2006-01-02")
	}
	return nil
}
