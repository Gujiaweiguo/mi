//go:build integration

package http_test

import (
	"bytes"
	"context"
	"database/sql"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	httpapi "github.com/Gujiaweiguo/mi/backend/internal/http"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

func TestIntegrationMasterDataClosureRoutes(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	container, err := testcontainers.GenericContainer(ctx, testcontainers.GenericContainerRequest{
		ContainerRequest: testcontainers.ContainerRequest{
			Image:        "mysql:8.0",
			ExposedPorts: []string{"3306/tcp"},
			Env: map[string]string{
				"MYSQL_DATABASE":      "mi_integration",
				"MYSQL_USER":          "mi_user",
				"MYSQL_PASSWORD":      "mi_password",
				"MYSQL_ROOT_PASSWORD": "mi_root_password",
			},
			WaitingFor: wait.ForListeningPort("3306/tcp").WithStartupTimeout(3 * time.Minute),
		},
		Started: true,
	})
	if err != nil {
		t.Fatalf("start mysql container: %v", err)
	}
	defer func() { _ = container.Terminate(context.Background()) }()

	host, err := container.Host(ctx)
	if err != nil {
		t.Fatalf("resolve mysql host: %v", err)
	}

	port, err := container.MappedPort(ctx, "3306/tcp")
	if err != nil {
		t.Fatalf("resolve mysql port: %v", err)
	}

	db, err := sql.Open("mysql", platformdb.Config{Host: host, Port: port.Int(), Name: "mi_integration", User: "mi_user", Password: "mi_password"}.DSN())
	if err != nil {
		t.Fatalf("open mysql: %v", err)
	}
	defer db.Close()

	if err := waitForDatabase(ctx, db); err != nil {
		t.Fatalf("ping mysql: %v", err)
	}

	migrator := platformdb.NewMigrator(db, os.DirFS("../platform/database"), "migrations")
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("apply migrations: %v", err)
	}

	bootstrapRunner := platformdb.NewBootstrapRunner(db, bootstrap.All()...)
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("run bootstrap seeds: %v", err)
	}

	router := httpapi.NewRouter(&config.Config{App: config.AppConfig{Name: "mi-backend", Environment: "test"}, Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600}}, db)
	token := loginForIntegration(t, router)

	customerListRecorder := httptest.NewRecorder()
	customerListRequest := httptest.NewRequest(http.MethodGet, "/api/master-data/customers?query=ACME&page=1&page_size=5", nil)
	customerListRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(customerListRecorder, customerListRequest)
	if customerListRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from filtered customer list, got %d body=%s", customerListRecorder.Code, customerListRecorder.Body.String())
	}
	var customerListBody struct {
		Customers []struct {
			ID   int64  `json:"id"`
			Code string `json:"code"`
			Name string `json:"name"`
		} `json:"customers"`
		Total int `json:"total"`
	}
	if err := json.Unmarshal(customerListRecorder.Body.Bytes(), &customerListBody); err != nil {
		t.Fatalf("decode customer list response: %v", err)
	}
	if customerListBody.Total == 0 || len(customerListBody.Customers) == 0 {
		t.Fatalf("expected seeded customer list, got body=%s", customerListRecorder.Body.String())
	}

	customerUpdateRecorder := httptest.NewRecorder()
	customerUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/master-data/customers/101", bytes.NewBufferString(`{"code":"CUST-101","name":"ACME Retail Updated","trade_id":102,"department_id":101,"status":"inactive"}`))
	customerUpdateRequest.Header.Set("Content-Type", "application/json")
	customerUpdateRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(customerUpdateRecorder, customerUpdateRequest)
	if customerUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from customer update, got %d body=%s", customerUpdateRecorder.Code, customerUpdateRecorder.Body.String())
	}
	if !bytes.Contains(customerUpdateRecorder.Body.Bytes(), []byte("ACME Retail Updated")) || !bytes.Contains(customerUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated customer payload, got body=%s", customerUpdateRecorder.Body.String())
	}

	brandUpdateRecorder := httptest.NewRecorder()
	brandUpdateRequest := httptest.NewRequest(http.MethodPut, "/api/master-data/brands/101", bytes.NewBufferString(`{"code":"BR-101","name":"ACME Fashion Updated","status":"inactive"}`))
	brandUpdateRequest.Header.Set("Content-Type", "application/json")
	brandUpdateRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(brandUpdateRecorder, brandUpdateRequest)
	if brandUpdateRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from brand update, got %d body=%s", brandUpdateRecorder.Code, brandUpdateRecorder.Body.String())
	}
	if !bytes.Contains(brandUpdateRecorder.Body.Bytes(), []byte("ACME Fashion Updated")) || !bytes.Contains(brandUpdateRecorder.Body.Bytes(), []byte("inactive")) {
		t.Fatalf("expected updated brand payload, got body=%s", brandUpdateRecorder.Body.String())
	}

	unitBudgetRecorder := httptest.NewRecorder()
	unitBudgetRequest := httptest.NewRequest(http.MethodPost, "/api/master-data/unit-rent-budgets", bytes.NewBufferString(`{"unit_id":101,"fiscal_year":2027,"budget_price":123.45}`))
	unitBudgetRequest.Header.Set("Content-Type", "application/json")
	unitBudgetRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(unitBudgetRecorder, unitBudgetRequest)
	if unitBudgetRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from unit budget create, got %d body=%s", unitBudgetRecorder.Code, unitBudgetRecorder.Body.String())
	}

	storeBudgetRecorder := httptest.NewRecorder()
	storeBudgetRequest := httptest.NewRequest(http.MethodPost, "/api/master-data/store-rent-budgets", bytes.NewBufferString(`{"store_id":101,"fiscal_year":2027,"fiscal_month":5,"monthly_budget":12500}`))
	storeBudgetRequest.Header.Set("Content-Type", "application/json")
	storeBudgetRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(storeBudgetRecorder, storeBudgetRequest)
	if storeBudgetRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from store budget create, got %d body=%s", storeBudgetRecorder.Code, storeBudgetRecorder.Body.String())
	}

	prospectRecorder := httptest.NewRecorder()
	prospectRequest := httptest.NewRequest(http.MethodPost, "/api/master-data/unit-prospects", bytes.NewBufferString(`{"unit_id":101,"fiscal_year":2027,"potential_customer_id":101,"prospect_brand_id":101,"prospect_trade_id":102,"avg_transaction":300,"prospect_rent_price":145,"rent_increment":"3% yearly","prospect_term_months":24}`))
	prospectRequest.Header.Set("Content-Type", "application/json")
	prospectRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(prospectRecorder, prospectRequest)
	if prospectRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from unit prospect create, got %d body=%s", prospectRecorder.Code, prospectRecorder.Body.String())
	}

	budgetsListRecorder := httptest.NewRecorder()
	budgetsListRequest := httptest.NewRequest(http.MethodGet, "/api/master-data/unit-rent-budgets", nil)
	budgetsListRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(budgetsListRecorder, budgetsListRequest)
	if budgetsListRecorder.Code != http.StatusOK || !bytes.Contains(budgetsListRecorder.Body.Bytes(), []byte(`"fiscal_year":2027`)) {
		t.Fatalf("expected listed unit budgets, got %d body=%s", budgetsListRecorder.Code, budgetsListRecorder.Body.String())
	}

	prospectsListRecorder := httptest.NewRecorder()
	prospectsListRequest := httptest.NewRequest(http.MethodGet, "/api/master-data/unit-prospects", nil)
	prospectsListRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(prospectsListRecorder, prospectsListRequest)
	if prospectsListRecorder.Code != http.StatusOK || !bytes.Contains(prospectsListRecorder.Body.Bytes(), []byte(`"prospect_term_months":24`)) {
		t.Fatalf("expected listed unit prospects, got %d body=%s", prospectsListRecorder.Code, prospectsListRecorder.Body.String())
	}
}

func loginForIntegration(t *testing.T, router http.Handler) string {
	t.Helper()

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
		t.Fatalf("expected login token, got body=%s", loginRecorder.Body.String())
	}
	return loginBody.Token
}
