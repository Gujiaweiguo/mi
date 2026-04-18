package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestStructureCreateStoreRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/stores", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateStore(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureCreateStoreRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/stores", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateStore(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateStoreRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/stores/bad-id", bytes.NewBufferString(`{"department_id":1,"store_type_id":1,"management_type_id":1,"code":"S01","name":"Store","short_name":"S"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateStore(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateStoreRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/stores/1", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateStore(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureCreateBuildingRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/buildings", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBuilding(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateBuildingRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/buildings/bad-id", bytes.NewBufferString(`{"store_id":1,"code":"B01","name":"Building"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateBuilding(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureCreateFloorRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/floors", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateFloor(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateFloorRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/floors/bad-id", bytes.NewBufferString(`{"building_id":1,"code":"F01","name":"Floor"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateFloor(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureCreateAreaRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/areas", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateArea(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateAreaRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/areas/bad-id", bytes.NewBufferString(`{"store_id":1,"area_level_id":1,"code":"A01","name":"Area"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateArea(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureCreateLocationRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/locations", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateLocation(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateLocationRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/locations/bad-id", bytes.NewBufferString(`{"store_id":1,"floor_id":1,"code":"L01","name":"Location"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateLocation(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureCreateUnitRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/structures/units", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateUnit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureUpdateUnitRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/structures/units/bad-id", bytes.NewBufferString(`{"building_id":1,"floor_id":1,"location_id":1,"area_id":1,"unit_type_id":1,"code":"U01","floor_area":100,"use_area":90,"rent_area":80}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpdateUnit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureListBuildingsRejectsInvalidStoreID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/structures/buildings?store_id=bad", nil)

	handler.ListBuildings(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureListFloorsRejectsInvalidBuildingID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/structures/floors?building_id=bad", nil)

	handler.ListFloors(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureListAreasRejectsInvalidStoreID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/structures/areas?store_id=bad", nil)

	handler.ListAreas(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureListLocationsRejectsInvalidStoreID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/structures/locations?store_id=bad", nil)

	handler.ListLocations(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureListLocationsRejectsInvalidFloorID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/structures/locations?floor_id=bad", nil)

	handler.ListLocations(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestStructureListUnitsRejectsInvalidBuildingID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewStructureHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/structures/units?building_id=bad", nil)

	handler.ListUnits(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
