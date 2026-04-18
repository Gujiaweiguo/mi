//go:build integration

package http_test

import (
	"bytes"
	"context"
	"database/sql"
	"encoding/json"
	"mime/multipart"
	"net/http"
	"net/http/httptest"
	"os"
	"strconv"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	httpapi "github.com/Gujiaweiguo/mi/backend/internal/http"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/xuri/excelize/v2"
	"go.uber.org/zap"
)

func TestIntegrationAuthAndOrgRoutes(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))

	router := httpapi.NewRouter(&config.Config{
		App:  config.AppConfig{Name: "mi-backend", Environment: "test"},
		Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600},
	}, db, zap.NewNop())

	loginRecorder := httptest.NewRecorder()
	loginRequest := httptest.NewRequest(http.MethodPost, "/api/auth/login", bytes.NewBufferString(`{"username":"admin","password":"password"}`))
	loginRequest.Header.Set("Content-Type", "application/json")
	router.ServeHTTP(loginRecorder, loginRequest)

	if loginRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from login, got %d body=%s", loginRecorder.Code, loginRecorder.Body.String())
	}

	var loginBody struct {
		Token string `json:"token"`
	}
	if err := json.Unmarshal(loginRecorder.Body.Bytes(), &loginBody); err != nil {
		t.Fatalf("decode login response: %v", err)
	}
	if loginBody.Token == "" {
		t.Fatal("expected login token")
	}

	meRecorder := httptest.NewRecorder()
	meRequest := httptest.NewRequest(http.MethodGet, "/api/auth/me", nil)
	meRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(meRecorder, meRequest)
	if meRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from /api/auth/me, got %d body=%s", meRecorder.Code, meRecorder.Body.String())
	}

	orgRecorder := httptest.NewRecorder()
	orgRequest := httptest.NewRequest(http.MethodGet, "/api/org/departments", nil)
	orgRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(orgRecorder, orgRequest)
	if orgRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from protected departments endpoint, got %d body=%s", orgRecorder.Code, orgRecorder.Body.String())
	}

	workflowDefinitionsRecorder := httptest.NewRecorder()
	workflowDefinitionsRequest := httptest.NewRequest(http.MethodGet, "/api/workflow/definitions", nil)
	workflowDefinitionsRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(workflowDefinitionsRecorder, workflowDefinitionsRequest)
	if workflowDefinitionsRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from workflow definitions endpoint, got %d body=%s", workflowDefinitionsRecorder.Code, workflowDefinitionsRecorder.Body.String())
	}

	customersRecorder := httptest.NewRecorder()
	customersRequest := httptest.NewRequest(http.MethodGet, "/api/master-data/customers", nil)
	customersRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(customersRecorder, customersRequest)
	if customersRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from customers endpoint, got %d body=%s", customersRecorder.Code, customersRecorder.Body.String())
	}

	brandsRecorder := httptest.NewRecorder()
	brandsRequest := httptest.NewRequest(http.MethodGet, "/api/master-data/brands", nil)
	brandsRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(brandsRecorder, brandsRequest)
	if brandsRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from brands endpoint, got %d body=%s", brandsRecorder.Code, brandsRecorder.Body.String())
	}

	storeTypesListRecorder := httptest.NewRecorder()
	storeTypesListRequest := httptest.NewRequest(http.MethodGet, "/api/base-info/store-types", nil)
	storeTypesListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(storeTypesListRecorder, storeTypesListRequest)
	if storeTypesListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from store types endpoint, got %d body=%s", storeTypesListRecorder.Code, storeTypesListRecorder.Body.String())
	}
	var storeTypesListBody struct {
		StoreTypes []struct {
			ID   int64  `json:"id"`
			Code string `json:"code"`
			Name string `json:"name"`
		} `json:"store_types"`
	}
	if err := json.Unmarshal(storeTypesListRecorder.Body.Bytes(), &storeTypesListBody); err != nil {
		t.Fatalf("decode store types list response: %v", err)
	}
	if len(storeTypesListBody.StoreTypes) == 0 {
		t.Fatal("expected seeded store types")
	}

	storeTypeCreateRecorder := httptest.NewRecorder()
	storeTypeCreateRequest := httptest.NewRequest(http.MethodPost, "/api/base-info/store-types", bytes.NewBufferString(`{"code":"outlet","name":"Outlet"}`))
	storeTypeCreateRequest.Header.Set("Content-Type", "application/json")
	storeTypeCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(storeTypeCreateRecorder, storeTypeCreateRequest)
	if storeTypeCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from store type create endpoint, got %d body=%s", storeTypeCreateRecorder.Code, storeTypeCreateRecorder.Body.String())
	}
	var storeTypeCreateBody struct {
		StoreType struct {
			ID   int64  `json:"id"`
			Code string `json:"code"`
			Name string `json:"name"`
		} `json:"store_type"`
	}
	if err := json.Unmarshal(storeTypeCreateRecorder.Body.Bytes(), &storeTypeCreateBody); err != nil {
		t.Fatalf("decode store type create response: %v", err)
	}
	if storeTypeCreateBody.StoreType.ID == 0 || storeTypeCreateBody.StoreType.Code != "outlet" {
		t.Fatalf("expected created store type payload, got body=%s", storeTypeCreateRecorder.Body.String())
	}

	storeTypeUpdateRecorder := httptest.NewRecorder()
	storeTypeUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/base-info/store-types/"+strconv.FormatInt(storeTypeCreateBody.StoreType.ID, 10), bytes.NewBufferString(`{"code":"outlet","name":"Outlet Center"}`))
	storeTypeUpdateRequest.Header.Set("Content-Type", "application/json")
	storeTypeUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(storeTypeUpdateRecorder, storeTypeUpdateRequest)
	if storeTypeUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from store type update endpoint, got %d body=%s", storeTypeUpdateRecorder.Code, storeTypeUpdateRecorder.Body.String())
	}
	if !bytes.Contains(storeTypeUpdateRecorder.Body.Bytes(), []byte("Outlet Center")) {
		t.Fatalf("expected updated store type name, got body=%s", storeTypeUpdateRecorder.Body.String())
	}

	shopTypeCreateRecorder := httptest.NewRecorder()
	shopTypeCreateRequest := httptest.NewRequest(http.MethodPost, "/api/base-info/shop-types", bytes.NewBufferString(`{"code":"lifestyle","name":"Lifestyle","color_hex":"#112233","status":"active"}`))
	shopTypeCreateRequest.Header.Set("Content-Type", "application/json")
	shopTypeCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(shopTypeCreateRecorder, shopTypeCreateRequest)
	if shopTypeCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from shop type create endpoint, got %d body=%s", shopTypeCreateRecorder.Code, shopTypeCreateRecorder.Body.String())
	}
	var shopTypeCreateBody struct {
		ShopType struct {
			ID       int64   `json:"id"`
			ColorHex *string `json:"color_hex"`
			Status   string  `json:"status"`
		} `json:"shop_type"`
	}
	if err := json.Unmarshal(shopTypeCreateRecorder.Body.Bytes(), &shopTypeCreateBody); err != nil {
		t.Fatalf("decode shop type create response: %v", err)
	}
	if shopTypeCreateBody.ShopType.ID == 0 || shopTypeCreateBody.ShopType.ColorHex == nil || *shopTypeCreateBody.ShopType.ColorHex != "#112233" || shopTypeCreateBody.ShopType.Status != "active" {
		t.Fatalf("expected created shop type payload, got body=%s", shopTypeCreateRecorder.Body.String())
	}

	shopTypeUpdateRecorder := httptest.NewRecorder()
	shopTypeUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/base-info/shop-types/"+strconv.FormatInt(shopTypeCreateBody.ShopType.ID, 10), bytes.NewBufferString(`{"code":"lifestyle","name":"Lifestyle Plus","color_hex":"#445566","status":"inactive"}`))
	shopTypeUpdateRequest.Header.Set("Content-Type", "application/json")
	shopTypeUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(shopTypeUpdateRecorder, shopTypeUpdateRequest)
	if shopTypeUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from shop type update endpoint, got %d body=%s", shopTypeUpdateRecorder.Code, shopTypeUpdateRecorder.Body.String())
	}
	if !bytes.Contains(shopTypeUpdateRecorder.Body.Bytes(), []byte("#445566")) || !bytes.Contains(shopTypeUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated shop type fields, got body=%s", shopTypeUpdateRecorder.Body.String())
	}

	currencyTypeCreateRecorder := httptest.NewRecorder()
	currencyTypeCreateRequest := httptest.NewRequest(http.MethodPost, "/api/base-info/currency-types", bytes.NewBufferString(`{"code":"USD","name":"US Dollar","is_local":false,"status":"active"}`))
	currencyTypeCreateRequest.Header.Set("Content-Type", "application/json")
	currencyTypeCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(currencyTypeCreateRecorder, currencyTypeCreateRequest)
	if currencyTypeCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from currency type create endpoint, got %d body=%s", currencyTypeCreateRecorder.Code, currencyTypeCreateRecorder.Body.String())
	}
	var currencyTypeCreateBody struct {
		CurrencyType struct {
			ID      int64  `json:"id"`
			IsLocal *bool  `json:"is_local"`
			Status  string `json:"status"`
		} `json:"currency_type"`
	}
	if err := json.Unmarshal(currencyTypeCreateRecorder.Body.Bytes(), &currencyTypeCreateBody); err != nil {
		t.Fatalf("decode currency type create response: %v", err)
	}
	if currencyTypeCreateBody.CurrencyType.ID == 0 || currencyTypeCreateBody.CurrencyType.IsLocal == nil || *currencyTypeCreateBody.CurrencyType.IsLocal || currencyTypeCreateBody.CurrencyType.Status != "active" {
		t.Fatalf("expected created currency type payload, got body=%s", currencyTypeCreateRecorder.Body.String())
	}

	currencyTypeListRecorder := httptest.NewRecorder()
	currencyTypeListRequest := httptest.NewRequest(http.MethodGet, "/api/base-info/currency-types", nil)
	currencyTypeListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(currencyTypeListRecorder, currencyTypeListRequest)
	if currencyTypeListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from currency types endpoint, got %d body=%s", currencyTypeListRecorder.Code, currencyTypeListRecorder.Body.String())
	}
	if !bytes.Contains(currencyTypeListRecorder.Body.Bytes(), []byte("USD")) {
		t.Fatalf("expected listed currency type, got body=%s", currencyTypeListRecorder.Body.String())
	}

	currencyTypeUpdateRecorder := httptest.NewRecorder()
	currencyTypeUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/base-info/currency-types/"+strconv.FormatInt(currencyTypeCreateBody.CurrencyType.ID, 10), bytes.NewBufferString(`{"code":"USD","name":"US Dollar Updated","is_local":true,"status":"inactive"}`))
	currencyTypeUpdateRequest.Header.Set("Content-Type", "application/json")
	currencyTypeUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(currencyTypeUpdateRecorder, currencyTypeUpdateRequest)
	if currencyTypeUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from currency type update endpoint, got %d body=%s", currencyTypeUpdateRecorder.Code, currencyTypeUpdateRecorder.Body.String())
	}
	if !bytes.Contains(currencyTypeUpdateRecorder.Body.Bytes(), []byte(`"is_local":true`)) || !bytes.Contains(currencyTypeUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated currency type fields, got body=%s", currencyTypeUpdateRecorder.Body.String())
	}

	tradeDefinitionCreateRecorder := httptest.NewRecorder()
	tradeDefinitionCreateRequest := httptest.NewRequest(http.MethodPost, "/api/base-info/trade-definitions", bytes.NewBufferString(`{"code":"SPORTS","name":"Sports","parent_id":101,"level":2,"status":"active"}`))
	tradeDefinitionCreateRequest.Header.Set("Content-Type", "application/json")
	tradeDefinitionCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(tradeDefinitionCreateRecorder, tradeDefinitionCreateRequest)
	if tradeDefinitionCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from trade definition create endpoint, got %d body=%s", tradeDefinitionCreateRecorder.Code, tradeDefinitionCreateRecorder.Body.String())
	}
	var tradeDefinitionCreateBody struct {
		TradeDefinition struct {
			ID       int64  `json:"id"`
			ParentID *int64 `json:"parent_id"`
			Level    *int   `json:"level"`
			Status   string `json:"status"`
		} `json:"trade_definition"`
	}
	if err := json.Unmarshal(tradeDefinitionCreateRecorder.Body.Bytes(), &tradeDefinitionCreateBody); err != nil {
		t.Fatalf("decode trade definition create response: %v", err)
	}
	if tradeDefinitionCreateBody.TradeDefinition.ID == 0 || tradeDefinitionCreateBody.TradeDefinition.ParentID == nil || *tradeDefinitionCreateBody.TradeDefinition.ParentID != 101 || tradeDefinitionCreateBody.TradeDefinition.Level == nil || *tradeDefinitionCreateBody.TradeDefinition.Level != 2 || tradeDefinitionCreateBody.TradeDefinition.Status != "active" {
		t.Fatalf("expected created trade definition payload, got body=%s", tradeDefinitionCreateRecorder.Body.String())
	}

	tradeDefinitionListRecorder := httptest.NewRecorder()
	tradeDefinitionListRequest := httptest.NewRequest(http.MethodGet, "/api/base-info/trade-definitions", nil)
	tradeDefinitionListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(tradeDefinitionListRecorder, tradeDefinitionListRequest)
	if tradeDefinitionListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from trade definitions endpoint, got %d body=%s", tradeDefinitionListRecorder.Code, tradeDefinitionListRecorder.Body.String())
	}
	var tradeDefinitionListBody struct {
		TradeDefinitions []struct {
			Code  string `json:"code"`
			Level *int   `json:"level"`
		} `json:"trade_definitions"`
	}
	if err := json.Unmarshal(tradeDefinitionListRecorder.Body.Bytes(), &tradeDefinitionListBody); err != nil {
		t.Fatalf("decode trade definitions list response: %v", err)
	}
	if len(tradeDefinitionListBody.TradeDefinitions) < 3 || tradeDefinitionListBody.TradeDefinitions[0].Code != "RETAIL" {
		t.Fatalf("expected ordered trade definitions list, got body=%s", tradeDefinitionListRecorder.Body.String())
	}

	tradeDefinitionUpdateRecorder := httptest.NewRecorder()
	tradeDefinitionUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/base-info/trade-definitions/"+strconv.FormatInt(tradeDefinitionCreateBody.TradeDefinition.ID, 10), bytes.NewBufferString(`{"code":"SPORTS","name":"Sports & Outdoor","parent_id":101,"level":3,"status":"inactive"}`))
	tradeDefinitionUpdateRequest.Header.Set("Content-Type", "application/json")
	tradeDefinitionUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(tradeDefinitionUpdateRecorder, tradeDefinitionUpdateRequest)
	if tradeDefinitionUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from trade definition update endpoint, got %d body=%s", tradeDefinitionUpdateRecorder.Code, tradeDefinitionUpdateRecorder.Body.String())
	}
	if !bytes.Contains(tradeDefinitionUpdateRecorder.Body.Bytes(), []byte(`"level":3`)) || !bytes.Contains(tradeDefinitionUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated trade definition fields, got body=%s", tradeDefinitionUpdateRecorder.Body.String())
	}

	structureStoreListRecorder := httptest.NewRecorder()
	structureStoreListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/stores", nil)
	structureStoreListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(structureStoreListRecorder, structureStoreListRequest)
	if structureStoreListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from structure store list endpoint, got %d body=%s", structureStoreListRecorder.Code, structureStoreListRecorder.Body.String())
	}
	var structureStoreListBody struct {
		Stores []struct {
			ID               int64  `json:"id"`
			DepartmentID     int64  `json:"department_id"`
			StoreTypeID      int64  `json:"store_type_id"`
			ManagementTypeID int64  `json:"management_type_id"`
			Code             string `json:"code"`
			ShortName        string `json:"short_name"`
			Status           string `json:"status"`
		} `json:"stores"`
	}
	if err := json.Unmarshal(structureStoreListRecorder.Body.Bytes(), &structureStoreListBody); err != nil {
		t.Fatalf("decode structure store list response: %v", err)
	}
	if len(structureStoreListBody.Stores) == 0 || structureStoreListBody.Stores[0].StoreTypeID == 0 || structureStoreListBody.Stores[0].ManagementTypeID == 0 {
		t.Fatalf("expected seeded structure stores with reference ids, got body=%s", structureStoreListRecorder.Body.String())
	}

	structureStoreCreateRecorder := httptest.NewRecorder()
	structureStoreCreateRequest := httptest.NewRequest(http.MethodPost, "/api/structure/stores", bytes.NewBufferString(`{"department_id":101,"store_type_id":101,"management_type_id":101,"code":"MI-HTTP-201","name":"HTTP Store","short_name":"HTTP","status":""}`))
	structureStoreCreateRequest.Header.Set("Content-Type", "application/json")
	structureStoreCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(structureStoreCreateRecorder, structureStoreCreateRequest)
	if structureStoreCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from structure store create endpoint, got %d body=%s", structureStoreCreateRecorder.Code, structureStoreCreateRecorder.Body.String())
	}
	var structureStoreCreateBody struct {
		Store struct {
			ID        int64  `json:"id"`
			Code      string `json:"code"`
			ShortName string `json:"short_name"`
			Status    string `json:"status"`
		} `json:"store"`
	}
	if err := json.Unmarshal(structureStoreCreateRecorder.Body.Bytes(), &structureStoreCreateBody); err != nil {
		t.Fatalf("decode structure store create response: %v", err)
	}
	if structureStoreCreateBody.Store.ID == 0 || structureStoreCreateBody.Store.Code != "MI-HTTP-201" || structureStoreCreateBody.Store.ShortName != "HTTP" || structureStoreCreateBody.Store.Status != "active" {
		t.Fatalf("expected created structure store payload, got body=%s", structureStoreCreateRecorder.Body.String())
	}

	structureStoreUpdateRecorder := httptest.NewRecorder()
	structureStoreUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/structure/stores/"+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10), bytes.NewBufferString(`{"department_id":101,"store_type_id":101,"management_type_id":101,"code":"MI-HTTP-201","name":"HTTP Store Updated","short_name":"HTTP-UPD","status":"inactive"}`))
	structureStoreUpdateRequest.Header.Set("Content-Type", "application/json")
	structureStoreUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(structureStoreUpdateRecorder, structureStoreUpdateRequest)
	if structureStoreUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from structure store update endpoint, got %d body=%s", structureStoreUpdateRecorder.Code, structureStoreUpdateRecorder.Body.String())
	}
	if !bytes.Contains(structureStoreUpdateRecorder.Body.Bytes(), []byte("HTTP-UPD")) || !bytes.Contains(structureStoreUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated structure store fields, got body=%s", structureStoreUpdateRecorder.Body.String())
	}

	buildingListRecorder := httptest.NewRecorder()
	buildingListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/buildings?store_id="+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10), nil)
	buildingListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(buildingListRecorder, buildingListRequest)
	if buildingListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from building list endpoint, got %d body=%s", buildingListRecorder.Code, buildingListRecorder.Body.String())
	}
	var buildingListBody struct {
		Buildings []any `json:"buildings"`
	}
	if err := json.Unmarshal(buildingListRecorder.Body.Bytes(), &buildingListBody); err != nil {
		t.Fatalf("decode building list response: %v", err)
	}
	if len(buildingListBody.Buildings) != 0 {
		t.Fatalf("expected empty building list for new store, got body=%s", buildingListRecorder.Body.String())
	}

	buildingCreateRecorder := httptest.NewRecorder()
	buildingCreateRequest := httptest.NewRequest(http.MethodPost, "/api/structure/buildings", bytes.NewBufferString(`{"store_id":`+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+`,"code":"BLD-HTTP-201","name":"HTTP Building","status":""}`))
	buildingCreateRequest.Header.Set("Content-Type", "application/json")
	buildingCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(buildingCreateRecorder, buildingCreateRequest)
	if buildingCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from building create endpoint, got %d body=%s", buildingCreateRecorder.Code, buildingCreateRecorder.Body.String())
	}
	var buildingCreateBody struct {
		Building struct {
			ID      int64  `json:"id"`
			StoreID int64  `json:"store_id"`
			Code    string `json:"code"`
			Status  string `json:"status"`
		} `json:"building"`
	}
	if err := json.Unmarshal(buildingCreateRecorder.Body.Bytes(), &buildingCreateBody); err != nil {
		t.Fatalf("decode building create response: %v", err)
	}
	if buildingCreateBody.Building.ID == 0 || buildingCreateBody.Building.StoreID != structureStoreCreateBody.Store.ID || buildingCreateBody.Building.Code != "BLD-HTTP-201" || buildingCreateBody.Building.Status != "active" {
		t.Fatalf("expected created building payload, got body=%s", buildingCreateRecorder.Body.String())
	}

	buildingFilteredListRecorder := httptest.NewRecorder()
	buildingFilteredListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/buildings?store_id="+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10), nil)
	buildingFilteredListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(buildingFilteredListRecorder, buildingFilteredListRequest)
	if buildingFilteredListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from filtered building list endpoint, got %d body=%s", buildingFilteredListRecorder.Code, buildingFilteredListRecorder.Body.String())
	}
	var buildingFilteredListBody struct {
		Buildings []struct {
			ID      int64  `json:"id"`
			StoreID int64  `json:"store_id"`
			Code    string `json:"code"`
		} `json:"buildings"`
	}
	if err := json.Unmarshal(buildingFilteredListRecorder.Body.Bytes(), &buildingFilteredListBody); err != nil {
		t.Fatalf("decode filtered building list response: %v", err)
	}
	if len(buildingFilteredListBody.Buildings) != 1 || buildingFilteredListBody.Buildings[0].StoreID != structureStoreCreateBody.Store.ID {
		t.Fatalf("expected one filtered building result, got body=%s", buildingFilteredListRecorder.Body.String())
	}

	buildingUpdateRecorder := httptest.NewRecorder()
	buildingUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/structure/buildings/"+strconv.FormatInt(buildingCreateBody.Building.ID, 10), bytes.NewBufferString(`{"store_id":`+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+`,"code":"BLD-HTTP-201","name":"HTTP Building Updated","status":"inactive"}`))
	buildingUpdateRequest.Header.Set("Content-Type", "application/json")
	buildingUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(buildingUpdateRecorder, buildingUpdateRequest)
	if buildingUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from building update endpoint, got %d body=%s", buildingUpdateRecorder.Code, buildingUpdateRecorder.Body.String())
	}
	if !bytes.Contains(buildingUpdateRecorder.Body.Bytes(), []byte("HTTP Building Updated")) || !bytes.Contains(buildingUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated building fields, got body=%s", buildingUpdateRecorder.Body.String())
	}

	floorListRecorder := httptest.NewRecorder()
	floorListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/floors?building_id="+strconv.FormatInt(buildingCreateBody.Building.ID, 10), nil)
	floorListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(floorListRecorder, floorListRequest)
	if floorListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from floor list endpoint, got %d body=%s", floorListRecorder.Code, floorListRecorder.Body.String())
	}
	var floorListBody struct {
		Floors []any `json:"floors"`
	}
	if err := json.Unmarshal(floorListRecorder.Body.Bytes(), &floorListBody); err != nil {
		t.Fatalf("decode floor list response: %v", err)
	}
	if len(floorListBody.Floors) != 0 {
		t.Fatalf("expected empty floor list for new building, got body=%s", floorListRecorder.Body.String())
	}

	floorCreateRecorder := httptest.NewRecorder()
	floorCreateRequest := httptest.NewRequest(http.MethodPost, "/api/structure/floors", bytes.NewBufferString(`{"building_id":`+strconv.FormatInt(buildingCreateBody.Building.ID, 10)+`,"code":"F-HTTP-201","name":"HTTP Floor","status":"","floor_plan_image_url":"https://example.com/http-floor.png"}`))
	floorCreateRequest.Header.Set("Content-Type", "application/json")
	floorCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(floorCreateRecorder, floorCreateRequest)
	if floorCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from floor create endpoint, got %d body=%s", floorCreateRecorder.Code, floorCreateRecorder.Body.String())
	}
	var floorCreateBody struct {
		Floor struct {
			ID                int64   `json:"id"`
			BuildingID        int64   `json:"building_id"`
			Code              string  `json:"code"`
			Status            string  `json:"status"`
			FloorPlanImageURL *string `json:"floor_plan_image_url"`
		} `json:"floor"`
	}
	if err := json.Unmarshal(floorCreateRecorder.Body.Bytes(), &floorCreateBody); err != nil {
		t.Fatalf("decode floor create response: %v", err)
	}
	if floorCreateBody.Floor.ID == 0 || floorCreateBody.Floor.BuildingID != buildingCreateBody.Building.ID || floorCreateBody.Floor.Code != "F-HTTP-201" || floorCreateBody.Floor.Status != "active" || floorCreateBody.Floor.FloorPlanImageURL == nil || *floorCreateBody.Floor.FloorPlanImageURL != "https://example.com/http-floor.png" {
		t.Fatalf("expected created floor payload, got body=%s", floorCreateRecorder.Body.String())
	}

	floorFilteredListRecorder := httptest.NewRecorder()
	floorFilteredListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/floors?building_id="+strconv.FormatInt(buildingCreateBody.Building.ID, 10), nil)
	floorFilteredListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(floorFilteredListRecorder, floorFilteredListRequest)
	if floorFilteredListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from filtered floor list endpoint, got %d body=%s", floorFilteredListRecorder.Code, floorFilteredListRecorder.Body.String())
	}
	var floorFilteredListBody struct {
		Floors []struct {
			ID         int64  `json:"id"`
			BuildingID int64  `json:"building_id"`
			Code       string `json:"code"`
		} `json:"floors"`
	}
	if err := json.Unmarshal(floorFilteredListRecorder.Body.Bytes(), &floorFilteredListBody); err != nil {
		t.Fatalf("decode filtered floor list response: %v", err)
	}
	if len(floorFilteredListBody.Floors) != 1 || floorFilteredListBody.Floors[0].BuildingID != buildingCreateBody.Building.ID {
		t.Fatalf("expected one filtered floor result, got body=%s", floorFilteredListRecorder.Body.String())
	}

	floorUpdateRecorder := httptest.NewRecorder()
	floorUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/structure/floors/"+strconv.FormatInt(floorCreateBody.Floor.ID, 10), bytes.NewBufferString(`{"building_id":`+strconv.FormatInt(buildingCreateBody.Building.ID, 10)+`,"code":"F-HTTP-201","name":"HTTP Floor Updated","status":"inactive","floor_plan_image_url":"https://example.com/http-floor-updated.png"}`))
	floorUpdateRequest.Header.Set("Content-Type", "application/json")
	floorUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(floorUpdateRecorder, floorUpdateRequest)
	if floorUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from floor update endpoint, got %d body=%s", floorUpdateRecorder.Code, floorUpdateRecorder.Body.String())
	}
	if !bytes.Contains(floorUpdateRecorder.Body.Bytes(), []byte("http-floor-updated.png")) || !bytes.Contains(floorUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated floor fields, got body=%s", floorUpdateRecorder.Body.String())
	}

	areaListRecorder := httptest.NewRecorder()
	areaListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/areas?store_id="+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10), nil)
	areaListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(areaListRecorder, areaListRequest)
	if areaListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from area list endpoint, got %d body=%s", areaListRecorder.Code, areaListRecorder.Body.String())
	}
	var areaListBody struct {
		Areas []any `json:"areas"`
	}
	if err := json.Unmarshal(areaListRecorder.Body.Bytes(), &areaListBody); err != nil {
		t.Fatalf("decode area list response: %v", err)
	}
	if len(areaListBody.Areas) != 0 {
		t.Fatalf("expected empty area list for new store, got body=%s", areaListRecorder.Body.String())
	}

	areaCreateRecorder := httptest.NewRecorder()
	areaCreateRequest := httptest.NewRequest(http.MethodPost, "/api/structure/areas", bytes.NewBufferString(`{"store_id":`+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+`,"area_level_id":101,"code":"AR-HTTP-201","name":"HTTP Area","status":""}`))
	areaCreateRequest.Header.Set("Content-Type", "application/json")
	areaCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(areaCreateRecorder, areaCreateRequest)
	if areaCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from area create endpoint, got %d body=%s", areaCreateRecorder.Code, areaCreateRecorder.Body.String())
	}
	var areaCreateBody struct {
		Area struct {
			ID          int64  `json:"id"`
			StoreID     int64  `json:"store_id"`
			AreaLevelID int64  `json:"area_level_id"`
			Code        string `json:"code"`
			Status      string `json:"status"`
		} `json:"area"`
	}
	if err := json.Unmarshal(areaCreateRecorder.Body.Bytes(), &areaCreateBody); err != nil {
		t.Fatalf("decode area create response: %v", err)
	}
	if areaCreateBody.Area.ID == 0 || areaCreateBody.Area.StoreID != structureStoreCreateBody.Store.ID || areaCreateBody.Area.AreaLevelID != 101 || areaCreateBody.Area.Code != "AR-HTTP-201" || areaCreateBody.Area.Status != "active" {
		t.Fatalf("expected created area payload, got body=%s", areaCreateRecorder.Body.String())
	}

	areaUpdateRecorder := httptest.NewRecorder()
	areaUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/structure/areas/"+strconv.FormatInt(areaCreateBody.Area.ID, 10), bytes.NewBufferString(`{"store_id":`+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+`,"area_level_id":101,"code":"AR-HTTP-201","name":"HTTP Area Updated","status":"inactive"}`))
	areaUpdateRequest.Header.Set("Content-Type", "application/json")
	areaUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(areaUpdateRecorder, areaUpdateRequest)
	if areaUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from area update endpoint, got %d body=%s", areaUpdateRecorder.Code, areaUpdateRecorder.Body.String())
	}
	if !bytes.Contains(areaUpdateRecorder.Body.Bytes(), []byte("HTTP Area Updated")) || !bytes.Contains(areaUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated area fields, got body=%s", areaUpdateRecorder.Body.String())
	}

	locationListRecorder := httptest.NewRecorder()
	locationListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/locations?store_id="+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+"&floor_id="+strconv.FormatInt(floorCreateBody.Floor.ID, 10), nil)
	locationListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(locationListRecorder, locationListRequest)
	if locationListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from location list endpoint, got %d body=%s", locationListRecorder.Code, locationListRecorder.Body.String())
	}
	var locationListBody struct {
		Locations []any `json:"locations"`
	}
	if err := json.Unmarshal(locationListRecorder.Body.Bytes(), &locationListBody); err != nil {
		t.Fatalf("decode location list response: %v", err)
	}
	if len(locationListBody.Locations) != 0 {
		t.Fatalf("expected empty location list for new floor, got body=%s", locationListRecorder.Body.String())
	}

	locationCreateRecorder := httptest.NewRecorder()
	locationCreateRequest := httptest.NewRequest(http.MethodPost, "/api/structure/locations", bytes.NewBufferString(`{"store_id":`+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+`,"floor_id":`+strconv.FormatInt(floorCreateBody.Floor.ID, 10)+`,"code":"LOC-HTTP-201","name":"HTTP Location","status":""}`))
	locationCreateRequest.Header.Set("Content-Type", "application/json")
	locationCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(locationCreateRecorder, locationCreateRequest)
	if locationCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from location create endpoint, got %d body=%s", locationCreateRecorder.Code, locationCreateRecorder.Body.String())
	}
	var locationCreateBody struct {
		Location struct {
			ID      int64  `json:"id"`
			StoreID int64  `json:"store_id"`
			FloorID int64  `json:"floor_id"`
			Code    string `json:"code"`
			Status  string `json:"status"`
		} `json:"location"`
	}
	if err := json.Unmarshal(locationCreateRecorder.Body.Bytes(), &locationCreateBody); err != nil {
		t.Fatalf("decode location create response: %v", err)
	}
	if locationCreateBody.Location.ID == 0 || locationCreateBody.Location.StoreID != structureStoreCreateBody.Store.ID || locationCreateBody.Location.FloorID != floorCreateBody.Floor.ID || locationCreateBody.Location.Code != "LOC-HTTP-201" || locationCreateBody.Location.Status != "active" {
		t.Fatalf("expected created location payload, got body=%s", locationCreateRecorder.Body.String())
	}

	locationUpdateRecorder := httptest.NewRecorder()
	locationUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/structure/locations/"+strconv.FormatInt(locationCreateBody.Location.ID, 10), bytes.NewBufferString(`{"store_id":`+strconv.FormatInt(structureStoreCreateBody.Store.ID, 10)+`,"floor_id":`+strconv.FormatInt(floorCreateBody.Floor.ID, 10)+`,"code":"LOC-HTTP-201","name":"HTTP Location Updated","status":"inactive"}`))
	locationUpdateRequest.Header.Set("Content-Type", "application/json")
	locationUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(locationUpdateRecorder, locationUpdateRequest)
	if locationUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from location update endpoint, got %d body=%s", locationUpdateRecorder.Code, locationUpdateRecorder.Body.String())
	}
	if !bytes.Contains(locationUpdateRecorder.Body.Bytes(), []byte("HTTP Location Updated")) || !bytes.Contains(locationUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated location fields, got body=%s", locationUpdateRecorder.Body.String())
	}

	unitListRecorder := httptest.NewRecorder()
	unitListRequest := httptest.NewRequest(http.MethodGet, "/api/structure/units?building_id="+strconv.FormatInt(buildingCreateBody.Building.ID, 10)+"&floor_id="+strconv.FormatInt(floorCreateBody.Floor.ID, 10)+"&location_id="+strconv.FormatInt(locationCreateBody.Location.ID, 10)+"&area_id="+strconv.FormatInt(areaCreateBody.Area.ID, 10), nil)
	unitListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(unitListRecorder, unitListRequest)
	if unitListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from unit list endpoint, got %d body=%s", unitListRecorder.Code, unitListRecorder.Body.String())
	}
	var unitListBody struct {
		Units []any `json:"units"`
	}
	if err := json.Unmarshal(unitListRecorder.Body.Bytes(), &unitListBody); err != nil {
		t.Fatalf("decode unit list response: %v", err)
	}
	if len(unitListBody.Units) != 0 {
		t.Fatalf("expected empty unit list for new structure branch, got body=%s", unitListRecorder.Body.String())
	}

	unitCreateRecorder := httptest.NewRecorder()
	unitCreateRequest := httptest.NewRequest(http.MethodPost, "/api/structure/units", bytes.NewBufferString(`{"building_id":`+strconv.FormatInt(buildingCreateBody.Building.ID, 10)+`,"floor_id":`+strconv.FormatInt(floorCreateBody.Floor.ID, 10)+`,"location_id":`+strconv.FormatInt(locationCreateBody.Location.ID, 10)+`,"area_id":`+strconv.FormatInt(areaCreateBody.Area.ID, 10)+`,"unit_type_id":101,"shop_type_id":101,"code":"U-HTTP-201","floor_area":120.5,"use_area":110.25,"rent_area":105.75,"is_rentable":true,"status":""}`))
	unitCreateRequest.Header.Set("Content-Type", "application/json")
	unitCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(unitCreateRecorder, unitCreateRequest)
	if unitCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from unit create endpoint, got %d body=%s", unitCreateRecorder.Code, unitCreateRecorder.Body.String())
	}
	var unitCreateBody struct {
		Unit struct {
			ID         int64   `json:"id"`
			BuildingID int64   `json:"building_id"`
			FloorID    int64   `json:"floor_id"`
			LocationID int64   `json:"location_id"`
			AreaID     int64   `json:"area_id"`
			UnitTypeID int64   `json:"unit_type_id"`
			ShopTypeID *int64  `json:"shop_type_id"`
			Code       string  `json:"code"`
			FloorArea  float64 `json:"floor_area"`
			UseArea    float64 `json:"use_area"`
			RentArea   float64 `json:"rent_area"`
			IsRentable bool    `json:"is_rentable"`
			Status     string  `json:"status"`
		} `json:"unit"`
	}
	if err := json.Unmarshal(unitCreateRecorder.Body.Bytes(), &unitCreateBody); err != nil {
		t.Fatalf("decode unit create response: %v", err)
	}
	if unitCreateBody.Unit.ID == 0 || unitCreateBody.Unit.BuildingID != buildingCreateBody.Building.ID || unitCreateBody.Unit.FloorID != floorCreateBody.Floor.ID || unitCreateBody.Unit.LocationID != locationCreateBody.Location.ID || unitCreateBody.Unit.AreaID != areaCreateBody.Area.ID || unitCreateBody.Unit.UnitTypeID != 101 || unitCreateBody.Unit.ShopTypeID == nil || *unitCreateBody.Unit.ShopTypeID != 101 || unitCreateBody.Unit.Code != "U-HTTP-201" || unitCreateBody.Unit.Status != "active" {
		t.Fatalf("expected created unit payload, got body=%s", unitCreateRecorder.Body.String())
	}

	unitUpdateRecorder := httptest.NewRecorder()
	unitUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/structure/units/"+strconv.FormatInt(unitCreateBody.Unit.ID, 10), bytes.NewBufferString(`{"building_id":`+strconv.FormatInt(buildingCreateBody.Building.ID, 10)+`,"floor_id":`+strconv.FormatInt(floorCreateBody.Floor.ID, 10)+`,"location_id":`+strconv.FormatInt(locationCreateBody.Location.ID, 10)+`,"area_id":`+strconv.FormatInt(areaCreateBody.Area.ID, 10)+`,"unit_type_id":101,"shop_type_id":101,"code":"U-HTTP-201","floor_area":121.5,"use_area":111.25,"rent_area":106.75,"is_rentable":false,"status":"inactive"}`))
	unitUpdateRequest.Header.Set("Content-Type", "application/json")
	unitUpdateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(unitUpdateRecorder, unitUpdateRequest)
	if unitUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from unit update endpoint, got %d body=%s", unitUpdateRecorder.Code, unitUpdateRecorder.Body.String())
	}
	if !bytes.Contains(unitUpdateRecorder.Body.Bytes(), []byte(`"is_rentable":false`)) || !bytes.Contains(unitUpdateRecorder.Body.Bytes(), []byte(`"shop_type_id":101`)) || !bytes.Contains(unitUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated unit fields, got body=%s", unitUpdateRecorder.Body.String())
	}

	reportQueryRecorder := httptest.NewRecorder()
	reportQueryRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r01/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101}`))
	reportQueryRequest.Header.Set("Content-Type", "application/json")
	reportQueryRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportQueryRecorder, reportQueryRequest)
	if reportQueryRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report query endpoint, got %d body=%s", reportQueryRecorder.Code, reportQueryRecorder.Body.String())
	}
	var reportQueryBody struct {
		Report struct {
			Rows []any `json:"rows"`
		} `json:"report"`
	}
	if err := json.Unmarshal(reportQueryRecorder.Body.Bytes(), &reportQueryBody); err != nil {
		t.Fatalf("decode report query response: %v", err)
	}

	reportExportRecorder := httptest.NewRecorder()
	reportExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r12/export", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"shop_type_id":101}`))
	reportExportRequest.Header.Set("Content-Type", "application/json")
	reportExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportExportRecorder, reportExportRequest)
	if reportExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report export endpoint, got %d body=%s", reportExportRecorder.Code, reportExportRecorder.Body.String())
	}
	workbook, err := excelize.OpenReader(bytes.NewReader(reportExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open report export workbook: %v", err)
	}
	defer workbook.Close()
	rows, err := workbook.GetRows(workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read report export rows: %v", err)
	}
	if len(rows) < 2 {
		t.Fatalf("expected report export rows, got %v", rows)
	}

	var reportAuditCount int
	if err := db.QueryRowContext(ctx, `SELECT COUNT(*) FROM report_audit_logs WHERE report_id IN ('r01', 'r12') AND action IN ('query', 'export')`).Scan(&reportAuditCount); err != nil {
		t.Fatalf("count report audit rows: %v", err)
	}
	if reportAuditCount != 2 {
		t.Fatalf("expected 2 report audit rows after first query/export pair, got %d", reportAuditCount)
	}

	var (
		r01Action          string
		r01AuditActor      int64
		r01AuditPeriod     sql.NullString
		r01AuditRowCount   int
		r01AuditExportSize int
	)
	if err := db.QueryRowContext(ctx, `SELECT action, actor_user_id, JSON_UNQUOTE(JSON_EXTRACT(request_payload, '$.period')), row_count, export_size_bytes FROM report_audit_logs WHERE report_id = 'r01' ORDER BY id DESC LIMIT 1`).Scan(&r01Action, &r01AuditActor, &r01AuditPeriod, &r01AuditRowCount, &r01AuditExportSize); err != nil {
		t.Fatalf("load report query audit row: %v", err)
	}
	if r01Action != "query" {
		t.Fatalf("expected latest r01 audit action query, got %q", r01Action)
	}
	if r01AuditActor <= 0 {
		t.Fatalf("expected positive report query audit actor id, got %d", r01AuditActor)
	}
	if !r01AuditPeriod.Valid || r01AuditPeriod.String != "2026-04" {
		t.Fatalf("expected report query audit period 2026-04, got %#v", r01AuditPeriod)
	}
	if r01AuditRowCount != len(reportQueryBody.Report.Rows) {
		t.Fatalf("expected report query audit row_count %d, got %d", len(reportQueryBody.Report.Rows), r01AuditRowCount)
	}
	if r01AuditExportSize != 0 {
		t.Fatalf("expected report query audit export_size_bytes 0, got %d", r01AuditExportSize)
	}

	var (
		r12Action          string
		r12AuditActor      int64
		r12AuditPeriod     sql.NullString
		r12AuditRowCount   int
		r12AuditExportSize int
	)
	if err := db.QueryRowContext(ctx, `SELECT action, actor_user_id, JSON_UNQUOTE(JSON_EXTRACT(request_payload, '$.period')), row_count, export_size_bytes FROM report_audit_logs WHERE report_id = 'r12' ORDER BY id DESC LIMIT 1`).Scan(&r12Action, &r12AuditActor, &r12AuditPeriod, &r12AuditRowCount, &r12AuditExportSize); err != nil {
		t.Fatalf("load report export audit row: %v", err)
	}
	if r12Action != "export" {
		t.Fatalf("expected latest r12 audit action export, got %q", r12Action)
	}
	if r12AuditActor <= 0 {
		t.Fatalf("expected positive report export audit actor id, got %d", r12AuditActor)
	}
	if !r12AuditPeriod.Valid || r12AuditPeriod.String != "2026-04" {
		t.Fatalf("expected report export audit period 2026-04, got %#v", r12AuditPeriod)
	}
	if r12AuditRowCount != len(rows)-1 {
		t.Fatalf("expected report export audit row_count %d, got %d", len(rows)-1, r12AuditRowCount)
	}
	if r12AuditExportSize != len(reportExportRecorder.Body.Bytes()) {
		t.Fatalf("expected report export audit export_size_bytes %d, got %d", len(reportExportRecorder.Body.Bytes()), r12AuditExportSize)
	}

	dailySaleCreateRecorder := httptest.NewRecorder()
	dailySaleCreateRequest := httptest.NewRequest(http.MethodPost, "/api/sales/daily", bytes.NewBufferString(`{"store_id":101,"unit_id":101,"sale_date":"2026-05-01","sales_amount":6000}`))
	dailySaleCreateRequest.Header.Set("Content-Type", "application/json")
	dailySaleCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(dailySaleCreateRecorder, dailySaleCreateRequest)
	if dailySaleCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from daily sale create endpoint, got %d body=%s", dailySaleCreateRecorder.Code, dailySaleCreateRecorder.Body.String())
	}

	dailySaleListRecorder := httptest.NewRecorder()
	dailySaleListRequest := httptest.NewRequest(http.MethodGet, "/api/sales/daily?store_id=101", nil)
	dailySaleListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(dailySaleListRecorder, dailySaleListRequest)
	if dailySaleListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from daily sale list endpoint, got %d body=%s", dailySaleListRecorder.Code, dailySaleListRecorder.Body.String())
	}
	var dailySaleListBody struct {
		DailySales []any `json:"daily_sales"`
	}
	if err := json.Unmarshal(dailySaleListRecorder.Body.Bytes(), &dailySaleListBody); err != nil {
		t.Fatalf("decode daily sale list response: %v", err)
	}
	if len(dailySaleListBody.DailySales) == 0 {
		t.Fatal("expected daily sales results")
	}

	trafficCreateRecorder := httptest.NewRecorder()
	trafficCreateRequest := httptest.NewRequest(http.MethodPost, "/api/sales/traffic", bytes.NewBufferString(`{"store_id":101,"traffic_date":"2026-05-01","inbound_count":500}`))
	trafficCreateRequest.Header.Set("Content-Type", "application/json")
	trafficCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(trafficCreateRecorder, trafficCreateRequest)
	if trafficCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from traffic create endpoint, got %d body=%s", trafficCreateRecorder.Code, trafficCreateRecorder.Body.String())
	}

	trafficListRecorder := httptest.NewRecorder()
	trafficListRequest := httptest.NewRequest(http.MethodGet, "/api/sales/traffic?store_id=101", nil)
	trafficListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(trafficListRecorder, trafficListRequest)
	if trafficListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from traffic list endpoint, got %d body=%s", trafficListRecorder.Code, trafficListRecorder.Body.String())
	}
	var trafficListBody struct {
		CustomerTraffic []any `json:"customer_traffic"`
	}
	if err := json.Unmarshal(trafficListRecorder.Body.Bytes(), &trafficListBody); err != nil {
		t.Fatalf("decode traffic list response: %v", err)
	}
	if len(trafficListBody.CustomerTraffic) == 0 {
		t.Fatal("expected customer traffic results")
	}

	dailyTemplateRecorder := httptest.NewRecorder()
	dailyTemplateRequest := httptest.NewRequest(http.MethodGet, "/api/excel/templates/daily-sales", nil)
	dailyTemplateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(dailyTemplateRecorder, dailyTemplateRequest)
	if dailyTemplateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from daily sales template endpoint, got %d body=%s", dailyTemplateRecorder.Code, dailyTemplateRecorder.Body.String())
	}

	dailyImportBody := &bytes.Buffer{}
	dailyImportWriter := multipart.NewWriter(dailyImportBody)
	dailyImportFile, err := dailyImportWriter.CreateFormFile("file", "daily-sales.xlsx")
	if err != nil {
		t.Fatalf("create daily import form file: %v", err)
	}
	dailyImportWorkbook := excelize.NewFile()
	_ = dailyImportWorkbook.SetSheetName(dailyImportWorkbook.GetSheetName(0), "DailySales")
	_ = dailyImportWorkbook.SetSheetRow("DailySales", "A1", &[]string{"store_code", "unit_code", "sale_date", "sales_amount"})
	_ = dailyImportWorkbook.SetSheetRow("DailySales", "A2", &[]string{"MI-001", "U-101", "2026-05-02", "6200"})
	if _, err := dailyImportWorkbook.WriteTo(dailyImportFile); err != nil {
		t.Fatalf("write daily import workbook: %v", err)
	}
	_ = dailyImportWorkbook.Close()
	if err := dailyImportWriter.Close(); err != nil {
		t.Fatalf("close daily import writer: %v", err)
	}
	dailyImportRecorder := httptest.NewRecorder()
	dailyImportRequest := httptest.NewRequest(http.MethodPost, "/api/excel/imports/daily-sales", dailyImportBody)
	dailyImportRequest.Header.Set("Content-Type", dailyImportWriter.FormDataContentType())
	dailyImportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(dailyImportRecorder, dailyImportRequest)
	if dailyImportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from daily sales import endpoint, got %d body=%s", dailyImportRecorder.Code, dailyImportRecorder.Body.String())
	}

	trafficTemplateRecorder := httptest.NewRecorder()
	trafficTemplateRequest := httptest.NewRequest(http.MethodGet, "/api/excel/templates/customer-traffic", nil)
	trafficTemplateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(trafficTemplateRecorder, trafficTemplateRequest)
	if trafficTemplateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from traffic template endpoint, got %d body=%s", trafficTemplateRecorder.Code, trafficTemplateRecorder.Body.String())
	}

	trafficImportBody := &bytes.Buffer{}
	trafficImportWriter := multipart.NewWriter(trafficImportBody)
	trafficImportFile, err := trafficImportWriter.CreateFormFile("file", "customer-traffic.xlsx")
	if err != nil {
		t.Fatalf("create traffic import form file: %v", err)
	}
	trafficImportWorkbook := excelize.NewFile()
	_ = trafficImportWorkbook.SetSheetName(trafficImportWorkbook.GetSheetName(0), "CustomerTraffic")
	_ = trafficImportWorkbook.SetSheetRow("CustomerTraffic", "A1", &[]string{"store_code", "traffic_date", "inbound_count"})
	_ = trafficImportWorkbook.SetSheetRow("CustomerTraffic", "A2", &[]string{"MI-001", "2026-05-02", "550"})
	if _, err := trafficImportWorkbook.WriteTo(trafficImportFile); err != nil {
		t.Fatalf("write traffic import workbook: %v", err)
	}
	_ = trafficImportWorkbook.Close()
	if err := trafficImportWriter.Close(); err != nil {
		t.Fatalf("close traffic import writer: %v", err)
	}
	trafficImportRecorder := httptest.NewRecorder()
	trafficImportRequest := httptest.NewRequest(http.MethodPost, "/api/excel/imports/customer-traffic", trafficImportBody)
	trafficImportRequest.Header.Set("Content-Type", trafficImportWriter.FormDataContentType())
	trafficImportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(trafficImportRecorder, trafficImportRequest)
	if trafficImportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from traffic import endpoint, got %d body=%s", trafficImportRecorder.Code, trafficImportRecorder.Body.String())
	}

	reportR03QueryRecorder := httptest.NewRecorder()
	reportR03QueryRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r03/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"shop_type_id":101}`))
	reportR03QueryRequest.Header.Set("Content-Type", "application/json")
	reportR03QueryRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR03QueryRecorder, reportR03QueryRequest)
	if reportR03QueryRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r03 query endpoint, got %d body=%s", reportR03QueryRecorder.Code, reportR03QueryRecorder.Body.String())
	}

	reportR04QueryRecorder := httptest.NewRecorder()
	reportR04QueryRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r04/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101}`))
	reportR04QueryRequest.Header.Set("Content-Type", "application/json")
	reportR04QueryRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR04QueryRecorder, reportR04QueryRequest)
	if reportR04QueryRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r04 query endpoint, got %d body=%s", reportR04QueryRecorder.Code, reportR04QueryRecorder.Body.String())
	}

	reportR10QueryRecorder := httptest.NewRecorder()
	reportR10QueryRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r10/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101}`))
	reportR10QueryRequest.Header.Set("Content-Type", "application/json")
	reportR10QueryRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR10QueryRecorder, reportR10QueryRequest)
	if reportR10QueryRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r10 query endpoint, got %d body=%s", reportR10QueryRecorder.Code, reportR10QueryRecorder.Body.String())
	}

	reportR10ExportRecorder := httptest.NewRecorder()
	reportR10ExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r10/export", bytes.NewBufferString(`{"period":"2026-04","store_id":101}`))
	reportR10ExportRequest.Header.Set("Content-Type", "application/json")
	reportR10ExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR10ExportRecorder, reportR10ExportRequest)
	if reportR10ExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r10 export endpoint, got %d body=%s", reportR10ExportRecorder.Code, reportR10ExportRecorder.Body.String())
	}
	reportR10Workbook, err := excelize.OpenReader(bytes.NewReader(reportR10ExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open report r10 export workbook: %v", err)
	}
	defer reportR10Workbook.Close()
	reportR10Rows, err := reportR10Workbook.GetRows(reportR10Workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read report r10 export rows: %v", err)
	}
	if len(reportR10Rows) < 2 {
		t.Fatalf("expected report r10 export rows, got %v", reportR10Rows)
	}

	reportR07QueryRecorder := httptest.NewRecorder()
	reportR07QueryRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r07/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101}`))
	reportR07QueryRequest.Header.Set("Content-Type", "application/json")
	reportR07QueryRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR07QueryRecorder, reportR07QueryRequest)
	if reportR07QueryRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r07 query endpoint, got %d body=%s", reportR07QueryRecorder.Code, reportR07QueryRecorder.Body.String())
	}

	reportR13QueryRecorder := httptest.NewRecorder()
	reportR13QueryRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r13/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"shop_type_id":101}`))
	reportR13QueryRequest.Header.Set("Content-Type", "application/json")
	reportR13QueryRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR13QueryRecorder, reportR13QueryRequest)
	if reportR13QueryRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r13 query endpoint, got %d body=%s", reportR13QueryRecorder.Code, reportR13QueryRecorder.Body.String())
	}

	reportR14ExportRecorder := httptest.NewRecorder()
	reportR14ExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r14/export", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"shop_type_id":101}`))
	reportR14ExportRequest.Header.Set("Content-Type", "application/json")
	reportR14ExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(reportR14ExportRecorder, reportR14ExportRequest)
	if reportR14ExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from report r14 export endpoint, got %d body=%s", reportR14ExportRecorder.Code, reportR14ExportRecorder.Body.String())
	}
	reportR14Workbook, err := excelize.OpenReader(bytes.NewReader(reportR14ExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open report r14 export workbook: %v", err)
	}
	defer reportR14Workbook.Close()
	reportR14Rows, err := reportR14Workbook.GetRows(reportR14Workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read report r14 export rows: %v", err)
	}
	if len(reportR14Rows) < 2 {
		t.Fatalf("expected report r14 export rows, got %v", reportR14Rows)
	}

	workflowStartRecorder := httptest.NewRecorder()
	workflowStartRequest := httptest.NewRequest(http.MethodPost, "/api/workflow/instances", bytes.NewBufferString(`{"definition_code":"lease-approval","document_type":"lease_contract","document_id":9001}`))
	workflowStartRequest.Header.Set("Content-Type", "application/json")
	workflowStartRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(workflowStartRecorder, workflowStartRequest)
	if workflowStartRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from workflow start endpoint, got %d body=%s", workflowStartRecorder.Code, workflowStartRecorder.Body.String())
	}

	var workflowStartBody struct {
		Instance struct {
			ID int64 `json:"id"`
		} `json:"instance"`
	}
	if err := json.Unmarshal(workflowStartRecorder.Body.Bytes(), &workflowStartBody); err != nil {
		t.Fatalf("decode workflow start response: %v", err)
	}
	if workflowStartBody.Instance.ID == 0 {
		t.Fatal("expected workflow instance id")
	}

	workflowApproveRecorder := httptest.NewRecorder()
	workflowApproveRequest := httptest.NewRequest(http.MethodPost, "/api/workflow/instances/"+strconv.FormatInt(workflowStartBody.Instance.ID, 10)+"/approve", bytes.NewBufferString(`{"idempotency_key":"approve-route-1"}`))
	workflowApproveRequest.Header.Set("Content-Type", "application/json")
	workflowApproveRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(workflowApproveRecorder, workflowApproveRequest)
	if workflowApproveRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from workflow approve endpoint, got %d body=%s", workflowApproveRecorder.Code, workflowApproveRecorder.Body.String())
	}

	workflowAuditRecorder := httptest.NewRecorder()
	workflowAuditRequest := httptest.NewRequest(http.MethodGet, "/api/workflow/instances/"+strconv.FormatInt(workflowStartBody.Instance.ID, 10)+"/audit", nil)
	workflowAuditRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(workflowAuditRecorder, workflowAuditRequest)
	if workflowAuditRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from workflow audit endpoint, got %d body=%s", workflowAuditRecorder.Code, workflowAuditRecorder.Body.String())
	}

	leaseCreateRecorder := httptest.NewRecorder()
	leaseCreateRequest := httptest.NewRequest(http.MethodPost, "/api/leases", bytes.NewBufferString(`{"lease_no":"CON-201","department_id":101,"store_id":101,"building_id":101,"customer_id":101,"brand_id":101,"trade_id":102,"management_type_id":101,"tenant_name":"ACME Retail","start_date":"2026-04-01","end_date":"2027-03-31","units":[{"unit_id":101,"rent_area":118}],"terms":[{"term_type":"rent","billing_cycle":"monthly","currency_type_id":101,"amount":12000,"effective_from":"2026-04-01","effective_to":"2027-03-31"}]}`))
	leaseCreateRequest.Header.Set("Content-Type", "application/json")
	leaseCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseCreateRecorder, leaseCreateRequest)
	if leaseCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from lease create endpoint, got %d body=%s", leaseCreateRecorder.Code, leaseCreateRecorder.Body.String())
	}

	var leaseCreateBody struct {
		Lease struct {
			ID int64 `json:"id"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(leaseCreateRecorder.Body.Bytes(), &leaseCreateBody); err != nil {
		t.Fatalf("decode lease create response: %v", err)
	}
	if leaseCreateBody.Lease.ID == 0 {
		t.Fatal("expected lease id")
	}

	leaseListRecorder := httptest.NewRecorder()
	leaseListRequest := httptest.NewRequest(http.MethodGet, "/api/leases", nil)
	leaseListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseListRecorder, leaseListRequest)
	if leaseListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from lease list endpoint, got %d body=%s", leaseListRecorder.Code, leaseListRecorder.Body.String())
	}

	leaseSubmitRecorder := httptest.NewRecorder()
	leaseSubmitRequest := httptest.NewRequest(http.MethodPost, "/api/leases/"+strconv.FormatInt(leaseCreateBody.Lease.ID, 10)+"/submit", bytes.NewBufferString(`{"idempotency_key":"lease-submit-201"}`))
	leaseSubmitRequest.Header.Set("Content-Type", "application/json")
	leaseSubmitRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseSubmitRecorder, leaseSubmitRequest)
	if leaseSubmitRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from lease submit endpoint, got %d body=%s", leaseSubmitRecorder.Code, leaseSubmitRecorder.Body.String())
	}

	var leaseSubmitBody struct {
		Lease struct {
			WorkflowInstanceID *int64 `json:"workflow_instance_id"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(leaseSubmitRecorder.Body.Bytes(), &leaseSubmitBody); err != nil {
		t.Fatalf("decode lease submit response: %v", err)
	}
	if leaseSubmitBody.Lease.WorkflowInstanceID == nil {
		t.Fatal("expected lease workflow instance id")
	}

	leaseApproveStep1Recorder := httptest.NewRecorder()
	leaseApproveStep1Request := httptest.NewRequest(http.MethodPost, "/api/workflow/instances/"+strconv.FormatInt(*leaseSubmitBody.Lease.WorkflowInstanceID, 10)+"/approve", bytes.NewBufferString(`{"idempotency_key":"lease-approve-step-1"}`))
	leaseApproveStep1Request.Header.Set("Content-Type", "application/json")
	leaseApproveStep1Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseApproveStep1Recorder, leaseApproveStep1Request)
	if leaseApproveStep1Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from lease workflow approve step 1 endpoint, got %d body=%s", leaseApproveStep1Recorder.Code, leaseApproveStep1Recorder.Body.String())
	}

	leaseApproveStep2Recorder := httptest.NewRecorder()
	leaseApproveStep2Request := httptest.NewRequest(http.MethodPost, "/api/workflow/instances/"+strconv.FormatInt(*leaseSubmitBody.Lease.WorkflowInstanceID, 10)+"/approve", bytes.NewBufferString(`{"idempotency_key":"lease-approve-step-2"}`))
	leaseApproveStep2Request.Header.Set("Content-Type", "application/json")
	leaseApproveStep2Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseApproveStep2Recorder, leaseApproveStep2Request)
	if leaseApproveStep2Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from lease workflow approve step 2 endpoint, got %d body=%s", leaseApproveStep2Recorder.Code, leaseApproveStep2Recorder.Body.String())
	}

	leaseGetRecorder := httptest.NewRecorder()
	leaseGetRequest := httptest.NewRequest(http.MethodGet, "/api/leases/"+strconv.FormatInt(leaseCreateBody.Lease.ID, 10), nil)
	leaseGetRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseGetRecorder, leaseGetRequest)
	if leaseGetRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from lease get endpoint, got %d body=%s", leaseGetRecorder.Code, leaseGetRecorder.Body.String())
	}

	var leaseGetBody struct {
		Lease struct {
			Status             string  `json:"status"`
			BillingEffectiveAt *string `json:"billing_effective_at"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(leaseGetRecorder.Body.Bytes(), &leaseGetBody); err != nil {
		t.Fatalf("decode lease get response: %v", err)
	}
	if leaseGetBody.Lease.Status != "active" || leaseGetBody.Lease.BillingEffectiveAt == nil {
		t.Fatalf("expected active billing-effective lease, got body=%s", leaseGetRecorder.Body.String())
	}

	for _, entry := range []struct {
		chargeType string
		dueDate    string
		amount     float64
		isDeposit  bool
	}{
		{chargeType: "rent", dueDate: "2026-04-15", amount: 500, isDeposit: false},
		{chargeType: "rent", dueDate: "2026-03-10", amount: 1000, isDeposit: false},
		{chargeType: "service", dueDate: "2026-02-10", amount: 2000, isDeposit: false},
		{chargeType: "deposit", dueDate: "2026-01-01", amount: 3000, isDeposit: true},
	} {
		if _, err := db.ExecContext(ctx, `
			INSERT INTO ar_open_items (lease_contract_id, customer_id, department_id, trade_id, charge_type, due_date, outstanding_amount, is_deposit)
			VALUES (?, ?, ?, ?, ?, ?, ?, ?)
		`, leaseCreateBody.Lease.ID, 101, 101, 102, entry.chargeType, entry.dueDate, entry.amount, entry.isDeposit); err != nil {
			t.Fatalf("seed ar open item for router test: %v", err)
		}
	}

	var leaseTermID int64
	if err := db.QueryRowContext(ctx, `SELECT id FROM lease_contract_terms WHERE lease_contract_id = ? AND term_type = 'rent' ORDER BY id LIMIT 1`, leaseCreateBody.Lease.ID).Scan(&leaseTermID); err != nil {
		t.Fatalf("load lease term id for router budget test: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO unit_rent_budgets (unit_id, fiscal_year, budget_price) VALUES (101, 2026, 95.00) ON DUPLICATE KEY UPDATE budget_price = VALUES(budget_price)`); err != nil {
		t.Fatalf("seed unit rent budget for router test: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO unit_prospects (unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id, avg_transaction, prospect_rent_price, rent_increment, prospect_term_months) VALUES (101, 2026, 101, 101, 102, 280.00, 110.00, '5% yearly', 36) ON DUPLICATE KEY UPDATE potential_customer_id = VALUES(potential_customer_id), prospect_brand_id = VALUES(prospect_brand_id), prospect_trade_id = VALUES(prospect_trade_id), avg_transaction = VALUES(avg_transaction), prospect_rent_price = VALUES(prospect_rent_price), rent_increment = VALUES(rent_increment), prospect_term_months = VALUES(prospect_term_months)`); err != nil {
		t.Fatalf("seed unit prospect for router test: %v", err)
	}
	for month := 1; month <= 12; month++ {
		if _, err := db.ExecContext(ctx, `INSERT INTO store_rent_budgets (store_id, fiscal_year, fiscal_month, monthly_budget) VALUES (?, 2026, ?, 10000.00) ON DUPLICATE KEY UPDATE monthly_budget = VALUES(monthly_budget)`, 101, month); err != nil {
			t.Fatalf("seed store budget month %d for router test: %v", month, err)
		}
	}
	billingRunResult, err := db.ExecContext(ctx, `INSERT INTO billing_runs (period_start, period_end, status, triggered_by, generated_count, skipped_count) VALUES ('2026-04-01', '2026-04-30', 'completed', 101, 1, 0)`)
	if err != nil {
		t.Fatalf("seed billing run for router budget test: %v", err)
	}
	billingRunID, err := billingRunResult.LastInsertId()
	if err != nil {
		t.Fatalf("billing run id for router budget test: %v", err)
	}
	chargeResult, err := db.ExecContext(ctx, `INSERT INTO billing_charge_lines (billing_run_id, lease_contract_id, lease_term_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount, currency_type_id, source_effective_version) VALUES (?, ?, ?, 'rent', '2026-04-01', '2026-04-30', 30, 12000.00, 12000.00, 101, 1)`, billingRunID, leaseCreateBody.Lease.ID, leaseTermID)
	if err != nil {
		t.Fatalf("seed billing charge for router budget test: %v", err)
	}
	chargeLineID, err := chargeResult.LastInsertId()
	if err != nil {
		t.Fatalf("charge line id for router budget test: %v", err)
	}
	documentResult, err := db.ExecContext(ctx, `INSERT INTO billing_documents (document_type, document_no, billing_run_id, lease_contract_id, tenant_name, period_start, period_end, total_amount, currency_type_id, status, approved_at, created_by, updated_by) VALUES ('invoice', 'INV-ROUTER-2026-04', ?, ?, 'ACME Retail', '2026-04-01', '2026-04-30', 9000.00, 101, 'approved', NOW(), 101, 101)`, billingRunID, leaseCreateBody.Lease.ID)
	if err != nil {
		t.Fatalf("seed billing document for router budget test: %v", err)
	}
	documentID, err := documentResult.LastInsertId()
	if err != nil {
		t.Fatalf("billing document id for router budget test: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO billing_document_lines (billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount) VALUES (?, ?, 'rent', '2026-04-01', '2026-04-30', 30, 12000.00, 9000.00)`, documentID, chargeLineID); err != nil {
		t.Fatalf("seed billing document line for router budget test: %v", err)
	}

	r02Recorder := httptest.NewRecorder()
	r02Request := httptest.NewRequest(http.MethodPost, "/api/reports/r02/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"status":"active","customer_id":101,"trade_id":102,"management_type_id":101}`))
	r02Request.Header.Set("Content-Type", "application/json")
	r02Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r02Recorder, r02Request)
	if r02Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R02 query endpoint, got %d body=%s", r02Recorder.Code, r02Recorder.Body.String())
	}
	if !bytes.Contains(r02Recorder.Body.Bytes(), []byte("CUST-101")) || !bytes.Contains(r02Recorder.Body.Bytes(), []byte("ACME Fashion")) {
		t.Fatalf("expected R02 query result to include linked customer and brand, got body=%s", r02Recorder.Body.String())
	}

	r05Recorder := httptest.NewRecorder()
	r05Request := httptest.NewRequest(http.MethodPost, "/api/reports/r05/query", bytes.NewBufferString(`{"period":"2026-01","store_id":101,"floor_id":101,"unit_id":101}`))
	r05Request.Header.Set("Content-Type", "application/json")
	r05Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r05Recorder, r05Request)
	if r05Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R05 query endpoint, got %d body=%s", r05Recorder.Code, r05Recorder.Body.String())
	}

	r06ExportRecorder := httptest.NewRecorder()
	r06ExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r06/export", bytes.NewBufferString(`{"period":"2026-04","store_id":101}`))
	r06ExportRequest.Header.Set("Content-Type", "application/json")
	r06ExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r06ExportRecorder, r06ExportRequest)
	if r06ExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R06 export endpoint, got %d body=%s", r06ExportRecorder.Code, r06ExportRecorder.Body.String())
	}
	r06Workbook, err := excelize.OpenReader(bytes.NewReader(r06ExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open R06 export workbook: %v", err)
	}
	defer r06Workbook.Close()
	r06Rows, err := r06Workbook.GetRows(r06Workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read R06 export rows: %v", err)
	}
	if len(r06Rows) < 2 {
		t.Fatalf("expected R06 export rows, got %v", r06Rows)
	}

	r15Recorder := httptest.NewRecorder()
	r15Request := httptest.NewRequest(http.MethodPost, "/api/reports/r15/query", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"shop_type_id":101}`))
	r15Request.Header.Set("Content-Type", "application/json")
	r15Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r15Recorder, r15Request)
	if r15Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R15 query endpoint, got %d body=%s", r15Recorder.Code, r15Recorder.Body.String())
	}

	r18ExportRecorder := httptest.NewRecorder()
	r18ExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r18/export", bytes.NewBufferString(`{"period":"2026-04","store_id":101,"customer_id":101,"brand_id":101,"unit_id":101}`))
	r18ExportRequest.Header.Set("Content-Type", "application/json")
	r18ExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r18ExportRecorder, r18ExportRequest)
	if r18ExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R18 export endpoint, got %d body=%s", r18ExportRecorder.Code, r18ExportRecorder.Body.String())
	}
	r18Workbook, err := excelize.OpenReader(bytes.NewReader(r18ExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open R18 export workbook: %v", err)
	}
	defer r18Workbook.Close()
	r18Rows, err := r18Workbook.GetRows(r18Workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read R18 export rows: %v", err)
	}
	if len(r18Rows) < 2 {
		t.Fatalf("expected R18 export rows, got %v", r18Rows)
	}

	r19Recorder := httptest.NewRecorder()
	r19Request := httptest.NewRequest(http.MethodPost, "/api/reports/r19/query", bytes.NewBufferString(`{"store_id":101,"floor_id":101,"area_id":101}`))
	r19Request.Header.Set("Content-Type", "application/json")
	r19Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r19Recorder, r19Request)
	if r19Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R19 query endpoint, got %d body=%s", r19Recorder.Code, r19Recorder.Body.String())
	}
	if !bytes.Contains(r19Recorder.Body.Bytes(), []byte(`"visual"`)) {
		t.Fatalf("expected R19 response to include visual payload, got body=%s", r19Recorder.Body.String())
	}

	r19ExportRecorder := httptest.NewRecorder()
	r19ExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r19/export", bytes.NewBufferString(`{"store_id":101,"floor_id":101,"area_id":101}`))
	r19ExportRequest.Header.Set("Content-Type", "application/json")
	r19ExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r19ExportRecorder, r19ExportRequest)
	if r19ExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R19 export endpoint, got %d body=%s", r19ExportRecorder.Code, r19ExportRecorder.Body.String())
	}
	r19Workbook, err := excelize.OpenReader(bytes.NewReader(r19ExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open R19 export workbook: %v", err)
	}
	defer r19Workbook.Close()
	r19Rows, err := r19Workbook.GetRows(r19Workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read R19 export rows: %v", err)
	}
	if len(r19Rows) < 2 {
		t.Fatalf("expected R19 export rows, got %v", r19Rows)
	}

	r08Recorder := httptest.NewRecorder()
	r08Request := httptest.NewRequest(http.MethodPost, "/api/reports/r08/query", bytes.NewBufferString(`{"period":"2026-04","department_id":101,"customer_id":101,"trade_id":102}`))
	r08Request.Header.Set("Content-Type", "application/json")
	r08Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r08Recorder, r08Request)
	if r08Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R08 query endpoint, got %d body=%s", r08Recorder.Code, r08Recorder.Body.String())
	}

	r09Recorder := httptest.NewRecorder()
	r09Request := httptest.NewRequest(http.MethodPost, "/api/reports/r09/query", bytes.NewBufferString(`{"period":"2026-04","department_id":101,"customer_id":101,"trade_id":102,"charge_type":"rent"}`))
	r09Request.Header.Set("Content-Type", "application/json")
	r09Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r09Recorder, r09Request)
	if r09Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R09 query endpoint, got %d body=%s", r09Recorder.Code, r09Recorder.Body.String())
	}

	r16Recorder := httptest.NewRecorder()
	r16Request := httptest.NewRequest(http.MethodPost, "/api/reports/r16/query", bytes.NewBufferString(`{"period":"2026-04","department_id":101}`))
	r16Request.Header.Set("Content-Type", "application/json")
	r16Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r16Recorder, r16Request)
	if r16Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R16 query endpoint, got %d body=%s", r16Recorder.Code, r16Recorder.Body.String())
	}

	r17ExportRecorder := httptest.NewRecorder()
	r17ExportRequest := httptest.NewRequest(http.MethodPost, "/api/reports/r17/export", bytes.NewBufferString(`{"period":"2026-04","department_id":101,"charge_type":"rent"}`))
	r17ExportRequest.Header.Set("Content-Type", "application/json")
	r17ExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(r17ExportRecorder, r17ExportRequest)
	if r17ExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from R17 export endpoint, got %d body=%s", r17ExportRecorder.Code, r17ExportRecorder.Body.String())
	}
	r17Workbook, err := excelize.OpenReader(bytes.NewReader(r17ExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open R17 export workbook: %v", err)
	}
	defer r17Workbook.Close()
	r17Rows, err := r17Workbook.GetRows(r17Workbook.GetSheetName(0))
	if err != nil {
		t.Fatalf("read R17 export rows: %v", err)
	}
	if len(r17Rows) < 2 {
		t.Fatalf("expected R17 export rows, got %v", r17Rows)
	}

	leaseAmendRecorder := httptest.NewRecorder()
	leaseAmendRequest := httptest.NewRequest(http.MethodPost, "/api/leases/"+strconv.FormatInt(leaseCreateBody.Lease.ID, 10)+"/amend", bytes.NewBufferString(`{"lease_no":"CON-201A","department_id":101,"store_id":101,"building_id":101,"customer_id":101,"brand_id":101,"trade_id":102,"management_type_id":101,"tenant_name":"ACME Retail","start_date":"2026-05-01","end_date":"2027-03-31","units":[{"unit_id":101,"rent_area":118}],"terms":[{"term_type":"rent","billing_cycle":"monthly","currency_type_id":101,"amount":15000,"effective_from":"2026-05-01","effective_to":"2027-03-31"}]}`))
	leaseAmendRequest.Header.Set("Content-Type", "application/json")
	leaseAmendRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseAmendRecorder, leaseAmendRequest)
	if leaseAmendRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from lease amend endpoint, got %d body=%s", leaseAmendRecorder.Code, leaseAmendRecorder.Body.String())
	}

	var leaseAmendBody struct {
		Lease struct {
			ID int64 `json:"id"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(leaseAmendRecorder.Body.Bytes(), &leaseAmendBody); err != nil {
		t.Fatalf("decode lease amend response: %v", err)
	}
	if leaseAmendBody.Lease.ID == 0 {
		t.Fatal("expected amendment lease id")
	}

	leaseAmendSubmitRecorder := httptest.NewRecorder()
	leaseAmendSubmitRequest := httptest.NewRequest(http.MethodPost, "/api/leases/"+strconv.FormatInt(leaseAmendBody.Lease.ID, 10)+"/submit", bytes.NewBufferString(`{"idempotency_key":"lease-submit-201a"}`))
	leaseAmendSubmitRequest.Header.Set("Content-Type", "application/json")
	leaseAmendSubmitRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseAmendSubmitRecorder, leaseAmendSubmitRequest)
	if leaseAmendSubmitRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from amendment submit endpoint, got %d body=%s", leaseAmendSubmitRecorder.Code, leaseAmendSubmitRecorder.Body.String())
	}

	var leaseAmendSubmitBody struct {
		Lease struct {
			WorkflowInstanceID *int64 `json:"workflow_instance_id"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(leaseAmendSubmitRecorder.Body.Bytes(), &leaseAmendSubmitBody); err != nil {
		t.Fatalf("decode amendment submit response: %v", err)
	}
	if leaseAmendSubmitBody.Lease.WorkflowInstanceID == nil {
		t.Fatal("expected amendment workflow instance id")
	}

	leaseAmendApproveStep1Recorder := httptest.NewRecorder()
	leaseAmendApproveStep1Request := httptest.NewRequest(http.MethodPost, "/api/workflow/instances/"+strconv.FormatInt(*leaseAmendSubmitBody.Lease.WorkflowInstanceID, 10)+"/approve", bytes.NewBufferString(`{"idempotency_key":"lease-amend-approve-step-1"}`))
	leaseAmendApproveStep1Request.Header.Set("Content-Type", "application/json")
	leaseAmendApproveStep1Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseAmendApproveStep1Recorder, leaseAmendApproveStep1Request)
	if leaseAmendApproveStep1Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from amendment approve step 1 endpoint, got %d body=%s", leaseAmendApproveStep1Recorder.Code, leaseAmendApproveStep1Recorder.Body.String())
	}

	leaseAmendApproveStep2Recorder := httptest.NewRecorder()
	leaseAmendApproveStep2Request := httptest.NewRequest(http.MethodPost, "/api/workflow/instances/"+strconv.FormatInt(*leaseAmendSubmitBody.Lease.WorkflowInstanceID, 10)+"/approve", bytes.NewBufferString(`{"idempotency_key":"lease-amend-approve-step-2"}`))
	leaseAmendApproveStep2Request.Header.Set("Content-Type", "application/json")
	leaseAmendApproveStep2Request.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseAmendApproveStep2Recorder, leaseAmendApproveStep2Request)
	if leaseAmendApproveStep2Recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from amendment approve step 2 endpoint, got %d body=%s", leaseAmendApproveStep2Recorder.Code, leaseAmendApproveStep2Recorder.Body.String())
	}

	originalLeaseGetRecorder := httptest.NewRecorder()
	originalLeaseGetRequest := httptest.NewRequest(http.MethodGet, "/api/leases/"+strconv.FormatInt(leaseCreateBody.Lease.ID, 10), nil)
	originalLeaseGetRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(originalLeaseGetRecorder, originalLeaseGetRequest)
	if originalLeaseGetRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from original lease get after amendment, got %d body=%s", originalLeaseGetRecorder.Code, originalLeaseGetRecorder.Body.String())
	}
	var originalLeaseBody struct {
		Lease struct {
			Status             string  `json:"status"`
			BillingEffectiveAt *string `json:"billing_effective_at"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(originalLeaseGetRecorder.Body.Bytes(), &originalLeaseBody); err != nil {
		t.Fatalf("decode original lease response after amendment: %v", err)
	}
	if originalLeaseBody.Lease.Status != "terminated" || originalLeaseBody.Lease.BillingEffectiveAt == nil {
		t.Fatalf("expected original lease terminated while preserving billing-effective timestamp after amendment, got body=%s", originalLeaseGetRecorder.Body.String())
	}

	amendedLeaseGetRecorder := httptest.NewRecorder()
	amendedLeaseGetRequest := httptest.NewRequest(http.MethodGet, "/api/leases/"+strconv.FormatInt(leaseAmendBody.Lease.ID, 10), nil)
	amendedLeaseGetRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(amendedLeaseGetRecorder, amendedLeaseGetRequest)
	if amendedLeaseGetRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from amended lease get endpoint, got %d body=%s", amendedLeaseGetRecorder.Code, amendedLeaseGetRecorder.Body.String())
	}
	var amendedLeaseBody struct {
		Lease struct {
			Status             string  `json:"status"`
			BillingEffectiveAt *string `json:"billing_effective_at"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(amendedLeaseGetRecorder.Body.Bytes(), &amendedLeaseBody); err != nil {
		t.Fatalf("decode amended lease get response: %v", err)
	}
	if amendedLeaseBody.Lease.Status != "active" || amendedLeaseBody.Lease.BillingEffectiveAt == nil {
		t.Fatalf("expected amended lease active and billing-effective, got body=%s", amendedLeaseGetRecorder.Body.String())
	}

	billingGenerateRecorder := httptest.NewRecorder()
	billingGenerateRequest := httptest.NewRequest(http.MethodPost, "/api/billing/charges/generate", bytes.NewBufferString(`{"period_start":"2026-05-01","period_end":"2026-05-31"}`))
	billingGenerateRequest.Header.Set("Content-Type", "application/json")
	billingGenerateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(billingGenerateRecorder, billingGenerateRequest)
	if billingGenerateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from billing generation endpoint, got %d body=%s", billingGenerateRecorder.Code, billingGenerateRecorder.Body.String())
	}

	var billingGenerateBody struct {
		Totals struct {
			Generated int `json:"generated"`
			Skipped   int `json:"skipped"`
		} `json:"totals"`
		Lines []struct {
			ID              int64   `json:"id"`
			LeaseContractID int64   `json:"lease_contract_id"`
			Amount          float64 `json:"amount"`
		} `json:"lines"`
	}
	if err := json.Unmarshal(billingGenerateRecorder.Body.Bytes(), &billingGenerateBody); err != nil {
		t.Fatalf("decode billing generate response: %v", err)
	}
	if billingGenerateBody.Totals.Generated != 1 || len(billingGenerateBody.Lines) != 1 || billingGenerateBody.Lines[0].LeaseContractID != leaseAmendBody.Lease.ID || billingGenerateBody.Lines[0].Amount != 15000 {
		t.Fatalf("expected one amended lease billing charge line, got body=%s", billingGenerateRecorder.Body.String())
	}

	billingListRecorder := httptest.NewRecorder()
	billingListRequest := httptest.NewRequest(http.MethodGet, "/api/billing/charges?lease_contract_id="+strconv.FormatInt(leaseAmendBody.Lease.ID, 10), nil)
	billingListRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(billingListRecorder, billingListRequest)
	if billingListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from billing list endpoint, got %d body=%s", billingListRecorder.Code, billingListRecorder.Body.String())
	}

	var billingListBody struct {
		Total int `json:"total"`
	}
	if err := json.Unmarshal(billingListRecorder.Body.Bytes(), &billingListBody); err != nil {
		t.Fatalf("decode billing list response: %v", err)
	}
	if billingListBody.Total != 1 {
		t.Fatalf("expected one listed billing charge, got body=%s", billingListRecorder.Body.String())
	}

	invoiceCreateRecorder := httptest.NewRecorder()
	invoiceCreateRequest := httptest.NewRequest(http.MethodPost, "/api/invoices", bytes.NewBufferString(`{"document_type":"invoice","billing_charge_line_ids":[`+strconv.FormatInt(billingGenerateBody.Lines[0].ID, 10)+`]}`))
	invoiceCreateRequest.Header.Set("Content-Type", "application/json")
	invoiceCreateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invoiceCreateRecorder, invoiceCreateRequest)
	if invoiceCreateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from invoice create endpoint, got %d body=%s", invoiceCreateRecorder.Code, invoiceCreateRecorder.Body.String())
	}

	var invoiceCreateBody struct {
		Document struct {
			ID int64 `json:"id"`
		} `json:"document"`
	}
	if err := json.Unmarshal(invoiceCreateRecorder.Body.Bytes(), &invoiceCreateBody); err != nil {
		t.Fatalf("decode invoice create response: %v", err)
	}
	if invoiceCreateBody.Document.ID == 0 {
		t.Fatal("expected invoice document id")
	}

	invoiceSubmitRecorder := httptest.NewRecorder()
	invoiceSubmitRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(invoiceCreateBody.Document.ID, 10)+"/submit", bytes.NewBufferString(`{"idempotency_key":"invoice-submit-201a"}`))
	invoiceSubmitRequest.Header.Set("Content-Type", "application/json")
	invoiceSubmitRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invoiceSubmitRecorder, invoiceSubmitRequest)
	if invoiceSubmitRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from invoice submit endpoint, got %d body=%s", invoiceSubmitRecorder.Code, invoiceSubmitRecorder.Body.String())
	}

	var invoiceSubmitBody struct {
		Document struct {
			WorkflowInstanceID *int64 `json:"workflow_instance_id"`
		} `json:"document"`
	}
	if err := json.Unmarshal(invoiceSubmitRecorder.Body.Bytes(), &invoiceSubmitBody); err != nil {
		t.Fatalf("decode invoice submit response: %v", err)
	}
	if invoiceSubmitBody.Document.WorkflowInstanceID == nil {
		t.Fatal("expected invoice workflow instance id")
	}

	invoiceApproveRecorder := httptest.NewRecorder()
	invoiceApproveRequest := httptest.NewRequest(http.MethodPost, "/api/workflow/instances/"+strconv.FormatInt(*invoiceSubmitBody.Document.WorkflowInstanceID, 10)+"/approve", bytes.NewBufferString(`{"idempotency_key":"invoice-approve-201a"}`))
	invoiceApproveRequest.Header.Set("Content-Type", "application/json")
	invoiceApproveRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invoiceApproveRecorder, invoiceApproveRequest)
	if invoiceApproveRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from invoice approve endpoint, got %d body=%s", invoiceApproveRecorder.Code, invoiceApproveRecorder.Body.String())
	}

	invoiceGetRecorder := httptest.NewRecorder()
	invoiceGetRequest := httptest.NewRequest(http.MethodGet, "/api/invoices/"+strconv.FormatInt(invoiceCreateBody.Document.ID, 10), nil)
	invoiceGetRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invoiceGetRecorder, invoiceGetRequest)
	if invoiceGetRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from invoice get endpoint, got %d body=%s", invoiceGetRecorder.Code, invoiceGetRecorder.Body.String())
	}

	var invoiceGetBody struct {
		Document struct {
			Status     string  `json:"status"`
			DocumentNo *string `json:"document_no"`
		} `json:"document"`
	}
	if err := json.Unmarshal(invoiceGetRecorder.Body.Bytes(), &invoiceGetBody); err != nil {
		t.Fatalf("decode invoice get response: %v", err)
	}
	if invoiceGetBody.Document.Status != "approved" || invoiceGetBody.Document.DocumentNo == nil || *invoiceGetBody.Document.DocumentNo != "INV-101" {
		t.Fatalf("expected approved invoice with number, got body=%s", invoiceGetRecorder.Body.String())
	}

	invoiceAuditRecorder := httptest.NewRecorder()
	invoiceAuditRequest := httptest.NewRequest(http.MethodGet, "/api/workflow/instances/"+strconv.FormatInt(*invoiceSubmitBody.Document.WorkflowInstanceID, 10)+"/audit", nil)
	invoiceAuditRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invoiceAuditRecorder, invoiceAuditRequest)
	if invoiceAuditRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from invoice workflow audit endpoint, got %d body=%s", invoiceAuditRecorder.Code, invoiceAuditRecorder.Body.String())
	}
	taxExportFromDate := "2026-01-01"
	taxExportToDate := "2026-12-31"

	taxRuleSetRecorder := httptest.NewRecorder()
	taxRuleSetRequest := httptest.NewRequest(http.MethodPost, "/api/tax/rule-sets", bytes.NewBufferString(`{"code":"kingdee-http","name":"Kingdee HTTP","document_type":"invoice","rules":[{"sequence_no":1,"entry_side":"debit","charge_type_filter":"rent","account_number":"1122","account_name":"应收账款","explanation_template":"YYYYMMDD-YYYYMMDD ITEMCODE","use_tenant_name":true},{"sequence_no":2,"entry_side":"credit","charge_type_filter":"rent","account_number":"6001","account_name":"租金收入","explanation_template":"SYYYYMM ITEMCODE","use_tenant_name":false}]}`))
	taxRuleSetRequest.Header.Set("Content-Type", "application/json")
	taxRuleSetRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(taxRuleSetRecorder, taxRuleSetRequest)
	if taxRuleSetRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from tax rule set endpoint, got %d body=%s", taxRuleSetRecorder.Code, taxRuleSetRecorder.Body.String())
	}

	taxExportRecorder := httptest.NewRecorder()
	taxExportRequest := httptest.NewRequest(http.MethodPost, "/api/tax/exports/vouchers", bytes.NewBufferString(`{"rule_set_code":"kingdee-http","from_date":"`+taxExportFromDate+`","to_date":"`+taxExportToDate+`"}`))
	taxExportRequest.Header.Set("Content-Type", "application/json")
	taxExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(taxExportRecorder, taxExportRequest)
	if taxExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from tax export endpoint, got %d body=%s", taxExportRecorder.Code, taxExportRecorder.Body.String())
	}
	if contentType := taxExportRecorder.Header().Get("Content-Type"); contentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" {
		t.Fatalf("expected xlsx content type, got %q", contentType)
	}
	taxWorkbook, err := excelize.OpenReader(bytes.NewReader(taxExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open tax export workbook: %v", err)
	}
	defer func() { _ = taxWorkbook.Close() }()
	sheet := taxWorkbook.GetSheetName(0)
	if value, err := taxWorkbook.GetCellValue(sheet, "A1"); err != nil || value != "FDate" {
		t.Fatalf("expected tax export workbook header FDate, got %q err=%v", value, err)
	}
	taxRows, err := taxWorkbook.GetRows(sheet)
	if err != nil {
		t.Fatalf("load tax export workbook rows: %v", err)
	}
	foundInvoiceNumber := false
	for _, row := range taxRows {
		for _, cell := range row {
			if cell == "INV-101" {
				foundInvoiceNumber = true
				break
			}
		}
		if foundInvoiceNumber {
			break
		}
	}
	if !foundInvoiceNumber {
		t.Fatalf("expected tax export workbook to contain INV-101, got rows=%v", taxRows)
	}

	printTemplateRecorder := httptest.NewRecorder()
	printTemplateRequest := httptest.NewRequest(http.MethodPost, "/api/print/templates", bytes.NewBufferString(`{"code":"invoice-print-http","name":"Invoice Print HTTP","document_type":"invoice","output_mode":"invoice_detail","title":"Invoice Detail Print","subtitle":"Printable invoice detail","header_lines":["Sunshine Commercial MI"],"footer_lines":["Printable invoice output"]}`))
	printTemplateRequest.Header.Set("Content-Type", "application/json")
	printTemplateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(printTemplateRecorder, printTemplateRequest)
	if printTemplateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from print template endpoint, got %d body=%s", printTemplateRecorder.Code, printTemplateRecorder.Body.String())
	}

	printHTMLRecorder := httptest.NewRecorder()
	printHTMLRequest := httptest.NewRequest(http.MethodPost, "/api/print/render/html", bytes.NewBufferString(`{"template_code":"invoice-print-http","document_ids":[`+strconv.FormatInt(invoiceCreateBody.Document.ID, 10)+`]}`))
	printHTMLRequest.Header.Set("Content-Type", "application/json")
	printHTMLRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(printHTMLRecorder, printHTMLRequest)
	if printHTMLRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from print html endpoint, got %d body=%s", printHTMLRecorder.Code, printHTMLRecorder.Body.String())
	}
	if !bytes.Contains(printHTMLRecorder.Body.Bytes(), []byte("Invoice Detail Print")) || !bytes.Contains(printHTMLRecorder.Body.Bytes(), []byte("INV-101")) {
		t.Fatalf("expected printable html to include title and document number, got body=%s", printHTMLRecorder.Body.String())
	}

	printPDFRecorder := httptest.NewRecorder()
	printPDFRequest := httptest.NewRequest(http.MethodPost, "/api/print/render/pdf", bytes.NewBufferString(`{"template_code":"invoice-print-http","document_ids":[`+strconv.FormatInt(invoiceCreateBody.Document.ID, 10)+`]}`))
	printPDFRequest.Header.Set("Content-Type", "application/json")
	printPDFRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(printPDFRecorder, printPDFRequest)
	if printPDFRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from print pdf endpoint, got %d body=%s", printPDFRecorder.Code, printPDFRecorder.Body.String())
	}
	if !bytes.HasPrefix(printPDFRecorder.Body.Bytes(), []byte("%PDF")) {
		t.Fatalf("expected pdf output, got first bytes=%q", printPDFRecorder.Body.Bytes()[:4])
	}

	excelTemplateRecorder := httptest.NewRecorder()
	excelTemplateRequest := httptest.NewRequest(http.MethodGet, "/api/excel/templates/unit-data", nil)
	excelTemplateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(excelTemplateRecorder, excelTemplateRequest)
	if excelTemplateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from excel unit template endpoint, got %d body=%s", excelTemplateRecorder.Code, excelTemplateRecorder.Body.String())
	}
	excelTemplateWorkbook, err := excelize.OpenReader(bytes.NewReader(excelTemplateRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open excel template workbook: %v", err)
	}
	defer func() { _ = excelTemplateWorkbook.Close() }()
	if value, err := excelTemplateWorkbook.GetCellValue("Units", "A1"); err != nil || value != "code" {
		t.Fatalf("expected excel template header code, got %q err=%v", value, err)
	}

	unitImportWorkbook := excelize.NewFile()
	_ = unitImportWorkbook.SetSheetName(unitImportWorkbook.GetSheetName(0), "Units")
	headers := []string{"code", "building_code", "floor_code", "location_code", "area_code", "unit_type_code", "floor_area", "use_area", "rent_area", "is_rentable", "status"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = unitImportWorkbook.SetCellValue("Units", cell, header)
	}
	rowValues := []string{"U-HTTP-201", "BLD-A", "F1", "L1", "A01", "shop", "130", "128", "128", "true", "active"}
	for index, value := range rowValues {
		cell, _ := excelize.CoordinatesToCellName(index+1, 2)
		_ = unitImportWorkbook.SetCellValue("Units", cell, value)
	}
	unitImportBuffer, err := unitImportWorkbook.WriteToBuffer()
	if err != nil {
		t.Fatalf("write unit import workbook: %v", err)
	}
	_ = unitImportWorkbook.Close()

	multipartBody := bytes.NewBuffer(nil)
	multipartWriter := multipart.NewWriter(multipartBody)
	fileWriter, err := multipartWriter.CreateFormFile("file", "unit-import.xlsx")
	if err != nil {
		t.Fatalf("create multipart file writer: %v", err)
	}
	if _, err := fileWriter.Write(unitImportBuffer.Bytes()); err != nil {
		t.Fatalf("write multipart workbook: %v", err)
	}
	if err := multipartWriter.Close(); err != nil {
		t.Fatalf("close multipart writer: %v", err)
	}

	excelImportRecorder := httptest.NewRecorder()
	excelImportRequest := httptest.NewRequest(http.MethodPost, "/api/excel/imports/unit-data", multipartBody)
	excelImportRequest.Header.Set("Content-Type", multipartWriter.FormDataContentType())
	excelImportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(excelImportRecorder, excelImportRequest)
	if excelImportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from excel import endpoint, got %d body=%s", excelImportRecorder.Code, excelImportRecorder.Body.String())
	}
	var excelImportBody struct {
		ImportedCount int `json:"imported_count"`
	}
	if err := json.Unmarshal(excelImportRecorder.Body.Bytes(), &excelImportBody); err != nil {
		t.Fatalf("decode excel import response: %v", err)
	}
	if excelImportBody.ImportedCount != 1 {
		t.Fatalf("expected one imported unit, got body=%s", excelImportRecorder.Body.String())
	}

	excelExportRecorder := httptest.NewRecorder()
	excelExportRequest := httptest.NewRequest(http.MethodGet, "/api/excel/exports/operational?dataset=invoices", nil)
	excelExportRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(excelExportRecorder, excelExportRequest)
	if excelExportRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from excel export endpoint, got %d body=%s", excelExportRecorder.Code, excelExportRecorder.Body.String())
	}
	excelExportWorkbook, err := excelize.OpenReader(bytes.NewReader(excelExportRecorder.Body.Bytes()))
	if err != nil {
		t.Fatalf("open excel export workbook: %v", err)
	}
	defer func() { _ = excelExportWorkbook.Close() }()
	excelExportSheet := excelExportWorkbook.GetSheetName(0)
	if value, err := excelExportWorkbook.GetCellValue(excelExportSheet, "A1"); err != nil || value != "document_no" {
		t.Fatalf("expected excel export header document_no, got %q err=%v", value, err)
	}

	invoiceCancelRecorder := httptest.NewRecorder()
	invoiceCancelRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(invoiceCreateBody.Document.ID, 10)+"/cancel", bytes.NewBufferString(`{}`))
	invoiceCancelRequest.Header.Set("Content-Type", "application/json")
	invoiceCancelRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invoiceCancelRecorder, invoiceCancelRequest)
	if invoiceCancelRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from invoice cancel endpoint, got %d body=%s", invoiceCancelRecorder.Code, invoiceCancelRecorder.Body.String())
	}

	leaseTerminateRecorder := httptest.NewRecorder()
	leaseTerminateRequest := httptest.NewRequest(http.MethodPost, "/api/leases/"+strconv.FormatInt(leaseAmendBody.Lease.ID, 10)+"/terminate", bytes.NewBufferString(`{"terminated_at":"2026-10-01"}`))
	leaseTerminateRequest.Header.Set("Content-Type", "application/json")
	leaseTerminateRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(leaseTerminateRecorder, leaseTerminateRequest)
	if leaseTerminateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from lease terminate endpoint, got %d body=%s", leaseTerminateRecorder.Code, leaseTerminateRecorder.Body.String())
	}

	terminatedLeaseGetRecorder := httptest.NewRecorder()
	terminatedLeaseGetRequest := httptest.NewRequest(http.MethodGet, "/api/leases/"+strconv.FormatInt(leaseAmendBody.Lease.ID, 10), nil)
	terminatedLeaseGetRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(terminatedLeaseGetRecorder, terminatedLeaseGetRequest)
	if terminatedLeaseGetRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from terminated lease get endpoint, got %d body=%s", terminatedLeaseGetRecorder.Code, terminatedLeaseGetRecorder.Body.String())
	}
	var terminatedLeaseBody struct {
		Lease struct {
			Status             string  `json:"status"`
			BillingEffectiveAt *string `json:"billing_effective_at"`
		} `json:"lease"`
	}
	if err := json.Unmarshal(terminatedLeaseGetRecorder.Body.Bytes(), &terminatedLeaseBody); err != nil {
		t.Fatalf("decode terminated lease get response: %v", err)
	}
	if terminatedLeaseBody.Lease.Status != "terminated" || terminatedLeaseBody.Lease.BillingEffectiveAt == nil {
		t.Fatalf("expected terminated lease to preserve billing-effective timestamp for proration, got body=%s", terminatedLeaseGetRecorder.Body.String())
	}

	unauthorizedRecorder := httptest.NewRecorder()
	unauthorizedRequest := httptest.NewRequest(http.MethodGet, "/api/org/stores", nil)
	router.ServeHTTP(unauthorizedRecorder, unauthorizedRequest)
	if unauthorizedRecorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401 from protected stores endpoint without token, got %d", unauthorizedRecorder.Code)
	}
}

func TestIntegrationWorkflowInstanceListRoute(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))

	router := httpapi.NewRouter(&config.Config{
		App:  config.AppConfig{Name: "mi-backend", Environment: "test"},
		Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600},
	}, db, zap.NewNop())

	loginRecorder := httptest.NewRecorder()
	loginRequest := httptest.NewRequest(http.MethodPost, "/api/auth/login", bytes.NewBufferString(`{"username":"admin","password":"password"}`))
	loginRequest.Header.Set("Content-Type", "application/json")
	router.ServeHTTP(loginRecorder, loginRequest)
	if loginRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from login, got %d body=%s", loginRecorder.Code, loginRecorder.Body.String())
	}

	var loginBody struct {
		Token string `json:"token"`
	}
	if err := json.Unmarshal(loginRecorder.Body.Bytes(), &loginBody); err != nil {
		t.Fatalf("decode login response: %v", err)
	}
	if loginBody.Token == "" {
		t.Fatal("expected login token")
	}

	for _, payload := range []string{
		`{"definition_code":"lease-approval","document_type":"lease_contract","document_id":9301}`,
		`{"definition_code":"invoice-approval","document_type":"invoice","document_id":9302}`,
	} {
		startRecorder := httptest.NewRecorder()
		startRequest := httptest.NewRequest(http.MethodPost, "/api/workflow/instances", bytes.NewBufferString(payload))
		startRequest.Header.Set("Content-Type", "application/json")
		startRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
		router.ServeHTTP(startRecorder, startRequest)
		if startRecorder.Code != http.StatusCreated {
			t.Fatalf("expected 201 from workflow start endpoint, got %d body=%s", startRecorder.Code, startRecorder.Body.String())
		}
	}

	listRecorder := httptest.NewRecorder()
	listRequest := httptest.NewRequest(http.MethodGet, "/api/workflow/instances?document_type=lease_contract", nil)
	listRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(listRecorder, listRequest)
	if listRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from workflow instance list endpoint, got %d body=%s", listRecorder.Code, listRecorder.Body.String())
	}

	var listBody struct {
		Instances []struct {
			ID           int64  `json:"id"`
			DocumentType string `json:"document_type"`
			DocumentID   int64  `json:"document_id"`
			Status       string `json:"status"`
		} `json:"instances"`
	}
	if err := json.Unmarshal(listRecorder.Body.Bytes(), &listBody); err != nil {
		t.Fatalf("decode workflow instance list response: %v", err)
	}
	if len(listBody.Instances) != 1 {
		t.Fatalf("expected 1 filtered workflow instance, got body=%s", listRecorder.Body.String())
	}
	if listBody.Instances[0].DocumentType != "lease_contract" || listBody.Instances[0].DocumentID != 9301 || listBody.Instances[0].Status != "pending" {
		t.Fatalf("expected lease workflow instance payload, got body=%s", listRecorder.Body.String())
	}

	invalidRecorder := httptest.NewRecorder()
	invalidRequest := httptest.NewRequest(http.MethodGet, "/api/workflow/instances?document_id=bad", nil)
	invalidRequest.Header.Set("Authorization", "Bearer "+loginBody.Token)
	router.ServeHTTP(invalidRecorder, invalidRequest)
	if invalidRecorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400 from invalid workflow instance list request, got %d body=%s", invalidRecorder.Code, invalidRecorder.Body.String())
	}
}
