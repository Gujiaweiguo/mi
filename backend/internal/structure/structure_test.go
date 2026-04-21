package structure

import (
	"database/sql"
	"errors"
	"testing"

	mysql "github.com/go-sql-driver/mysql"
)

func strPtr(s string) *string { return &s }

func TestNormalizeStatus_Empty(t *testing.T) {
	if got := normalizeStatus(""); got != "active" {
		t.Fatalf("normalizeStatus('') = %q, want 'active'", got)
	}
}

func TestNormalizeStatus_Active(t *testing.T) {
	if got := normalizeStatus("active"); got != "active" {
		t.Fatalf("normalizeStatus('active') = %q, want 'active'", got)
	}
}

func TestNormalizeStatus_Inactive(t *testing.T) {
	if got := normalizeStatus("inactive"); got != "inactive" {
		t.Fatalf("normalizeStatus('inactive') = %q, want 'inactive'", got)
	}
}

func TestNormalizeStatus_Whitespace(t *testing.T) {
	if got := normalizeStatus("  active  "); got != "active" {
		t.Fatalf("normalizeStatus('  active  ') = %q, want 'active'", got)
	}
}

func TestTrimStringPointer_Nil(t *testing.T) {
	if got := trimStringPointer(nil); got != nil {
		t.Fatal("trimStringPointer(nil) should return nil")
	}
}

func TestTrimStringPointer_Whitespace(t *testing.T) {
	got := trimStringPointer(strPtr("  hello  "))
	if got == nil || *got != "hello" {
		t.Fatalf("trimStringPointer(strPtr(\"  hello  \")) = %v, want \"hello\"", got)
	}
}

func TestTrimStringPointer_Empty(t *testing.T) {
	if got := trimStringPointer(strPtr("")); got != nil {
		t.Fatalf("trimStringPointer(strPtr(\"\")) = %v, want nil", got)
	}
}

func TestIsDuplicateEntry_MySQL1062(t *testing.T) {
	err := &mysql.MySQLError{Number: 1062, Message: "Duplicate entry"}
	if !isDuplicateEntry(err) {
		t.Fatal("expected true for MySQL 1062")
	}
}

func TestIsDuplicateEntry_Nil(t *testing.T) {
	if isDuplicateEntry(nil) {
		t.Fatal("expected false for nil")
	}
}

func TestIsDuplicateEntry_OtherError(t *testing.T) {
	if isDuplicateEntry(errors.New("some error")) {
		t.Fatal("expected false for generic error")
	}
}

func TestIsForeignKeyViolation_MySQL1452(t *testing.T) {
	err := &mysql.MySQLError{Number: 1452, Message: "Cannot add or update a child row"}
	if !isForeignKeyViolation(err) {
		t.Fatal("expected true for MySQL 1452")
	}
}

func TestIsForeignKeyViolation_Nil(t *testing.T) {
	if isForeignKeyViolation(nil) {
		t.Fatal("expected false for nil")
	}
}

func TestIsForeignKeyViolation_OtherError(t *testing.T) {
	if isForeignKeyViolation(errors.New("some error")) {
		t.Fatal("expected false for generic error")
	}
}

func TestIsForeignKeyViolation_WrongNumber(t *testing.T) {
	err := &mysql.MySQLError{Number: 1062, Message: "Duplicate"}
	if isForeignKeyViolation(err) {
		t.Fatal("expected false for MySQL 1062")
	}
}

func TestMapRepositoryError_NilError(t *testing.T) {
	item := "test"
	result, err := mapRepositoryError(&item, nil)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result != &item {
		t.Fatal("expected same item back")
	}
}

func TestMapRepositoryError_DuplicateEntry(t *testing.T) {
	dbErr := &mysql.MySQLError{Number: 1062, Message: "Duplicate"}
	result, err := mapRepositoryError[string](nil, dbErr)
	if result != nil {
		t.Fatal("expected nil result")
	}
	if err != ErrDuplicateCode {
		t.Fatalf("expected ErrDuplicateCode, got %v", err)
	}
}

func TestMapRepositoryError_FKViolation(t *testing.T) {
	dbErr := &mysql.MySQLError{Number: 1452, Message: "FK violation"}
	result, err := mapRepositoryError[string](nil, dbErr)
	if result != nil {
		t.Fatal("expected nil result")
	}
	if err != ErrParentReferenceNotFound {
		t.Fatalf("expected ErrParentReferenceNotFound, got %v", err)
	}
}

func TestMapRepositoryError_NoRows(t *testing.T) {
	result, err := mapRepositoryError[string](nil, sql.ErrNoRows)
	if result != nil {
		t.Fatal("expected nil result")
	}
	if err != ErrReferenceNotFound {
		t.Fatalf("expected ErrReferenceNotFound, got %v", err)
	}
}

func TestMapRepositoryError_GenericError(t *testing.T) {
	genericErr := errors.New("something went wrong")
	result, err := mapRepositoryError[string](nil, genericErr)
	if result != nil {
		t.Fatal("expected nil result")
	}
	if err != genericErr {
		t.Fatalf("expected original error, got %v", err)
	}
}

func TestNormalizeStoreInput_Valid(t *testing.T) {
	input := StoreInput{
		DepartmentID: 1, StoreTypeID: 2, ManagementTypeID: 3,
		Code: "  S001  ", Name: "  Store A  ", ShortName: "  SA  ",
	}
	result, err := normalizeStoreInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "S001" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
	if result.Name != "Store A" {
		t.Fatalf("Name not trimmed: got %q", result.Name)
	}
	if result.ShortName != "SA" {
		t.Fatalf("ShortName not trimmed: got %q", result.ShortName)
	}
}

func TestNormalizeStoreInput_MissingDepartmentID(t *testing.T) {
	input := StoreInput{StoreTypeID: 2, ManagementTypeID: 3, Code: "A", Name: "B", ShortName: "C"}
	_, err := normalizeStoreInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeStoreInput_EmptyCode(t *testing.T) {
	input := StoreInput{
		DepartmentID: 1, StoreTypeID: 2, ManagementTypeID: 3,
		Code: "", Name: "B", ShortName: "C",
	}
	_, err := normalizeStoreInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeBuildingInput_Valid(t *testing.T) {
	input := BuildingInput{StoreID: 1, Code: "  B001  ", Name: "  Building A  "}
	result, err := normalizeBuildingInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "B001" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
	if result.Name != "Building A" {
		t.Fatalf("Name not trimmed: got %q", result.Name)
	}
}

func TestNormalizeBuildingInput_InvalidStoreID(t *testing.T) {
	input := BuildingInput{StoreID: 0, Code: "A", Name: "B"}
	_, err := normalizeBuildingInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeFloorInput_Valid(t *testing.T) {
	input := FloorInput{
		BuildingID: 1, Code: "  F001  ", Name: "  Floor 1  ",
		FloorPlanImageURL: strPtr("  http://img.png  "),
	}
	result, err := normalizeFloorInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "F001" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
	if result.Name != "Floor 1" {
		t.Fatalf("Name not trimmed: got %q", result.Name)
	}
	if result.FloorPlanImageURL == nil || *result.FloorPlanImageURL != "http://img.png" {
		t.Fatalf("FloorPlanImageURL not trimmed: got %v", result.FloorPlanImageURL)
	}
}

func TestNormalizeFloorInput_InvalidBuildingID(t *testing.T) {
	input := FloorInput{BuildingID: 0, Code: "A", Name: "B"}
	_, err := normalizeFloorInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeAreaInput_Valid(t *testing.T) {
	input := AreaInput{StoreID: 1, AreaLevelID: 2, Code: "  A001  ", Name: "  Area 1  "}
	result, err := normalizeAreaInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "A001" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
	if result.Name != "Area 1" {
		t.Fatalf("Name not trimmed: got %q", result.Name)
	}
}

func TestNormalizeAreaInput_InvalidStoreID(t *testing.T) {
	input := AreaInput{StoreID: 0, AreaLevelID: 2, Code: "A", Name: "B"}
	_, err := normalizeAreaInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeLocationInput_Valid(t *testing.T) {
	input := LocationInput{StoreID: 1, FloorID: 2, Code: "  L001  ", Name: "  Loc 1  "}
	result, err := normalizeLocationInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "L001" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
	if result.Name != "Loc 1" {
		t.Fatalf("Name not trimmed: got %q", result.Name)
	}
}

func TestNormalizeLocationInput_InvalidFloorID(t *testing.T) {
	input := LocationInput{StoreID: 1, FloorID: 0, Code: "A", Name: "B"}
	_, err := normalizeLocationInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeUnitInput_Valid(t *testing.T) {
	input := UnitInput{
		BuildingID: 1, FloorID: 2, LocationID: 3, AreaID: 4, UnitTypeID: 5,
		Code: "  U001  ", FloorArea: 100, UseArea: 80, RentArea: 90,
	}
	result, err := normalizeUnitInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "U001" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
}

func TestNormalizeUnitInput_MissingBuildingID(t *testing.T) {
	input := UnitInput{
		FloorID: 2, LocationID: 3, AreaID: 4, UnitTypeID: 5, Code: "A",
	}
	_, err := normalizeUnitInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure, got %v", err)
	}
}

func TestNormalizeUnitInput_NegativeFloorArea(t *testing.T) {
	input := UnitInput{
		BuildingID: 1, FloorID: 2, LocationID: 3, AreaID: 4, UnitTypeID: 5,
		Code: "A", FloorArea: -1,
	}
	_, err := normalizeUnitInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure for negative FloorArea, got %v", err)
	}
}

func TestNormalizeUnitInput_EmptyCode(t *testing.T) {
	input := UnitInput{
		BuildingID: 1, FloorID: 2, LocationID: 3, AreaID: 4, UnitTypeID: 5,
		Code: "",
	}
	_, err := normalizeUnitInput(input)
	if err != ErrInvalidStructure {
		t.Fatalf("expected ErrInvalidStructure for empty Code, got %v", err)
	}
}

func TestNewService_Nil(t *testing.T) {
	svc := NewService(nil)
	if svc == nil {
		t.Fatal("NewService(nil) should return non-nil")
	}
}

func TestNewRepository_Nil(t *testing.T) {
	repo := NewRepository(nil)
	if repo == nil {
		t.Fatal("NewRepository(nil) should return non-nil")
	}
}
