package pagination

import "testing"

func TestNormalizePage_DefaultsOnZero(t *testing.T) {
	page, pageSize := NormalizePage(0, 0)
	if page != DefaultPage {
		t.Errorf("page: got %d, want %d", page, DefaultPage)
	}
	if pageSize != DefaultPageSize {
		t.Errorf("pageSize: got %d, want %d", pageSize, DefaultPageSize)
	}
}

func TestNormalizePage_DefaultsOnNegative(t *testing.T) {
	page, pageSize := NormalizePage(-1, -5)
	if page != DefaultPage {
		t.Errorf("page: got %d, want %d", page, DefaultPage)
	}
	if pageSize != DefaultPageSize {
		t.Errorf("pageSize: got %d, want %d", pageSize, DefaultPageSize)
	}
}

func TestNormalizePage_ClampsMaxPageSize(t *testing.T) {
	_, pageSize := NormalizePage(1, 500)
	if pageSize != MaxPageSize {
		t.Errorf("pageSize: got %d, want %d", pageSize, MaxPageSize)
	}
}

func TestNormalizePage_ValidValuesPassThrough(t *testing.T) {
	page, pageSize := NormalizePage(3, 50)
	if page != 3 {
		t.Errorf("page: got %d, want 3", page)
	}
	if pageSize != 50 {
		t.Errorf("pageSize: got %d, want 50", pageSize)
	}
}

func TestNormalizePage_PageOneValidPageSize(t *testing.T) {
	page, pageSize := NormalizePage(1, MaxPageSize)
	if page != 1 {
		t.Errorf("page: got %d, want 1", page)
	}
	if pageSize != MaxPageSize {
		t.Errorf("pageSize: got %d, want %d", pageSize, MaxPageSize)
	}
}

func TestNormalizePage_PageOneExactlyMaxNotClamped(t *testing.T) {
	_, pageSize := NormalizePage(1, MaxPageSize)
	if pageSize != MaxPageSize {
		t.Errorf("pageSize should not be clamped when exactly MaxPageSize; got %d, want %d", pageSize, MaxPageSize)
	}
}

func TestListResult_Fields(t *testing.T) {
	items := []int{1, 2, 3}
	result := ListResult[int]{
		Items:    items,
		Total:    3,
		Page:     1,
		PageSize: 20,
	}
	if len(result.Items) != 3 {
		t.Errorf("Items length: got %d, want 3", len(result.Items))
	}
	if result.Total != 3 {
		t.Errorf("Total: got %d, want 3", result.Total)
	}
	if result.Page != 1 {
		t.Errorf("Page: got %d, want 1", result.Page)
	}
	if result.PageSize != 20 {
		t.Errorf("PageSize: got %d, want 20", result.PageSize)
	}
}

func TestListResult_Empty(t *testing.T) {
	result := ListResult[string]{
		Items:    []string{},
		Total:    0,
		Page:     1,
		PageSize: DefaultPageSize,
	}
	if len(result.Items) != 0 {
		t.Errorf("expected empty Items slice")
	}
	if result.Total != 0 {
		t.Errorf("Total: got %d, want 0", result.Total)
	}
}

func TestConstants(t *testing.T) {
	if DefaultPage != 1 {
		t.Errorf("DefaultPage = %d, want 1", DefaultPage)
	}
	if DefaultPageSize != 20 {
		t.Errorf("DefaultPageSize = %d, want 20", DefaultPageSize)
	}
	if MaxPageSize != 100 {
		t.Errorf("MaxPageSize = %d, want 100", MaxPageSize)
	}
}
