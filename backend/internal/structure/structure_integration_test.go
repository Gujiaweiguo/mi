//go:build integration

package structure_test

import (
	"context"
	"os"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/structure"
)

func TestStructureServiceIntegrationOperations(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := structure.NewService(structure.NewRepository(db))
	storeID := int64(101)
	areaLevelID := int64(101)
	unitTypeID := int64(101)
	shopTypeID := int64(101)

	stores, err := service.ListStores(ctx)
	if err != nil {
		t.Fatalf("list stores: %v", err)
	}
	if len(stores) == 0 || stores[0].ID != 101 || stores[0].Code != "MI-001" {
		t.Fatalf("expected seeded store 101, got %+v", stores)
	}

	building, err := service.CreateBuilding(ctx, structure.BuildingInput{StoreID: storeID, Code: "BLD-IT", Name: "Integration Building", Status: "active"})
	if err != nil {
		t.Fatalf("create building: %v", err)
	}
	buildings, err := service.ListBuildings(ctx, structure.BuildingFilter{StoreID: &storeID})
	if err != nil {
		t.Fatalf("list buildings: %v", err)
	}
	assertBuildingPresent(t, buildings, building.ID, "Integration Building")

	updatedBuilding, err := service.UpdateBuilding(ctx, building.ID, structure.BuildingInput{StoreID: storeID, Code: "BLD-IT-2", Name: "Integration Building Updated", Status: "inactive"})
	if err != nil {
		t.Fatalf("update building: %v", err)
	}
	if updatedBuilding.Code != "BLD-IT-2" || updatedBuilding.Name != "Integration Building Updated" || updatedBuilding.Status != "inactive" {
		t.Fatalf("unexpected updated building: %+v", updatedBuilding)
	}

	floorPlan := "https://example.test/floor.png"
	floor, err := service.CreateFloor(ctx, structure.FloorInput{BuildingID: building.ID, Code: "F-IT", Name: "Integration Floor", Status: "active", FloorPlanImageURL: &floorPlan})
	if err != nil {
		t.Fatalf("create floor: %v", err)
	}
	floors, err := service.ListFloors(ctx, structure.FloorFilter{BuildingID: &building.ID})
	if err != nil {
		t.Fatalf("list floors: %v", err)
	}
	assertFloorPresent(t, floors, floor.ID, "Integration Floor")

	updatedFloorPlan := "https://example.test/floor-updated.png"
	updatedFloor, err := service.UpdateFloor(ctx, floor.ID, structure.FloorInput{BuildingID: building.ID, Code: "F-IT-2", Name: "Integration Floor Updated", Status: "inactive", FloorPlanImageURL: &updatedFloorPlan})
	if err != nil {
		t.Fatalf("update floor: %v", err)
	}
	if updatedFloor.Code != "F-IT-2" || updatedFloor.Name != "Integration Floor Updated" || updatedFloor.Status != "inactive" || updatedFloor.FloorPlanImageURL == nil || *updatedFloor.FloorPlanImageURL != updatedFloorPlan {
		t.Fatalf("unexpected updated floor: %+v", updatedFloor)
	}

	area, err := service.CreateArea(ctx, structure.AreaInput{StoreID: storeID, AreaLevelID: areaLevelID, Code: "AR-IT", Name: "Integration Area", Status: "active"})
	if err != nil {
		t.Fatalf("create area: %v", err)
	}
	areas, err := service.ListAreas(ctx, structure.AreaFilter{StoreID: &storeID})
	if err != nil {
		t.Fatalf("list areas: %v", err)
	}
	assertAreaPresent(t, areas, area.ID, "Integration Area")

	location, err := service.CreateLocation(ctx, structure.LocationInput{StoreID: storeID, FloorID: floor.ID, Code: "LOC-IT", Name: "Integration Location", Status: "active"})
	if err != nil {
		t.Fatalf("create location: %v", err)
	}
	locations, err := service.ListLocations(ctx, structure.LocationFilter{StoreID: &storeID, FloorID: &floor.ID})
	if err != nil {
		t.Fatalf("list locations: %v", err)
	}
	assertLocationPresent(t, locations, location.ID, "Integration Location")

	unit, err := service.CreateUnit(ctx, structure.UnitInput{BuildingID: building.ID, FloorID: floor.ID, LocationID: location.ID, AreaID: area.ID, UnitTypeID: unitTypeID, ShopTypeID: &shopTypeID, Code: "UNIT-IT", FloorArea: 100, UseArea: 95, RentArea: 90, IsRentable: true, Status: "active"})
	if err != nil {
		t.Fatalf("create unit: %v", err)
	}
	units, err := service.ListUnits(ctx, structure.UnitFilter{BuildingID: &building.ID, FloorID: &floor.ID, LocationID: &location.ID, AreaID: &area.ID})
	if err != nil {
		t.Fatalf("list units: %v", err)
	}
	assertUnitPresent(t, units, unit.ID, "UNIT-IT")
}

func assertBuildingPresent(t *testing.T, items []structure.Building, id int64, wantName string) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected building %d name %q, got %+v", id, wantName, item)
			}
			return
		}
	}
	t.Fatalf("expected building %d in list", id)
}

func assertFloorPresent(t *testing.T, items []structure.Floor, id int64, wantName string) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected floor %d name %q, got %+v", id, wantName, item)
			}
			return
		}
	}
	t.Fatalf("expected floor %d in list", id)
}

func assertAreaPresent(t *testing.T, items []structure.Area, id int64, wantName string) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected area %d name %q, got %+v", id, wantName, item)
			}
			return
		}
	}
	t.Fatalf("expected area %d in list", id)
}

func assertLocationPresent(t *testing.T, items []structure.Location, id int64, wantName string) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Name != wantName {
				t.Fatalf("expected location %d name %q, got %+v", id, wantName, item)
			}
			return
		}
	}
	t.Fatalf("expected location %d in list", id)
}

func assertUnitPresent(t *testing.T, items []structure.Unit, id int64, wantCode string) {
	t.Helper()
	for _, item := range items {
		if item.ID == id {
			if item.Code != wantCode {
				t.Fatalf("expected unit %d code %q, got %+v", id, wantCode, item)
			}
			return
		}
	}
	t.Fatalf("expected unit %d in list", id)
}
