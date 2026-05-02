package workflow

import (
	"context"
	"testing"
)

func TestResolveFixedRole(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyFixedRole, ConfigJSON: `{"role_id":103}`},
		}},
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.RoleID != 103 {
		t.Errorf("expected role_id 103, got %d", r.RoleID)
	}
	if r.DepartmentID != 200 {
		t.Errorf("expected department_id 200, got %d", r.DepartmentID)
	}
	if r.UserID != nil {
		t.Errorf("expected nil user_id, got %d", *r.UserID)
	}
}

func TestResolveFixedUser(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyFixedUser, ConfigJSON: `{"user_id":101}`},
		}},
	}
	userDeptLookup := func(userID int64) (int64, bool) {
		if userID == 101 {
			return 300, true
		}
		return 0, false
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, userDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.UserID == nil || *r.UserID != 101 {
		t.Errorf("expected user_id 101, got %v", r.UserID)
	}
	if r.DepartmentID != 300 {
		t.Errorf("expected department_id 300, got %d", r.DepartmentID)
	}
	if r.RoleID != 100 {
		t.Errorf("expected role_id 100 (from node), got %d", r.RoleID)
	}
}

func TestResolveFixedUserUnknownDepartment(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyFixedUser, ConfigJSON: `{"user_id":999}`},
		}},
	}
	userDeptLookup := func(userID int64) (int64, bool) { return 0, false }
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, userDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.DepartmentID != 0 {
		t.Errorf("expected department_id 0 for unknown user, got %d", r.DepartmentID)
	}
	if r.UserID == nil || *r.UserID != 999 {
		t.Errorf("expected user_id 999, got %v", r.UserID)
	}
}

func TestResolveDepartmentLeader(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyDepartmentLead, ConfigJSON: `{}`},
		}},
	}
	deptLeaderLookup := func(deptID int64) (int64, int64, bool) {
		if deptID == 200 {
			return 55, 10, true
		}
		return 0, 0, false
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, deptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.UserID == nil || *r.UserID != 55 {
		t.Errorf("expected user_id 55, got %v", r.UserID)
	}
	if r.RoleID != 10 {
		t.Errorf("expected role_id 10, got %d", r.RoleID)
	}
	if r.DepartmentID != 200 {
		t.Errorf("expected department_id 200, got %d", r.DepartmentID)
	}
}

func TestResolveDepartmentLeaderNotFound(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyDepartmentLead, ConfigJSON: `{}`},
		}},
	}
	deptLeaderLookup := func(deptID int64) (int64, int64, bool) { return 0, 0, false }
	_, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, deptLeaderLookup, nil, "", 0)
	if err == nil {
		t.Fatal("expected error for missing department leader")
	}
}

func TestResolveSubmitterContext(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategySubmitter, ConfigJSON: `{}`},
		}},
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.UserID == nil || *r.UserID != 50 {
		t.Errorf("expected user_id 50 (submitter), got %v", r.UserID)
	}
	if r.DepartmentID != 200 {
		t.Errorf("expected department_id 200, got %d", r.DepartmentID)
	}
	if r.RoleID != 100 {
		t.Errorf("expected role_id 100 (from node), got %d", r.RoleID)
	}
}

func TestResolveNoRulesFallback(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 102, AssignmentRules: nil},
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.RoleID != 102 {
		t.Errorf("expected fallback role_id 102, got %d", r.RoleID)
	}
	if r.DepartmentID != 200 {
		t.Errorf("expected department_id 200, got %d", r.DepartmentID)
	}
	if r.UserID != nil {
		t.Errorf("expected nil user_id for fallback, got %d", *r.UserID)
	}
}

func TestResolveDocumentFieldFallback(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 105, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyDocumentField, ConfigJSON: `{"field_path":"department_id"}`},
		}},
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.RoleID != 105 {
		t.Errorf("expected fallback role_id 105, got %d", r.RoleID)
	}
	if r.DepartmentID != 200 {
		t.Errorf("expected department_id 200, got %d", r.DepartmentID)
	}
	if r.UserID != nil {
		t.Errorf("expected nil user_id for document_field fallback, got %d", *r.UserID)
	}
}

func TestResolveMultipleNodes(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "entry", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyFixedRole, ConfigJSON: `{"role_id":103}`},
		}},
		{ID: 2, Code: "finance", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategySubmitter, ConfigJSON: `{}`},
		}},
		{ID: 3, Code: "manager", RoleID: 104, AssignmentRules: nil},
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if len(resolved) != 3 {
		t.Fatalf("expected 3 resolved nodes, got %d", len(resolved))
	}
	if resolved[1].RoleID != 103 {
		t.Errorf("node 1: expected role_id 103, got %d", resolved[1].RoleID)
	}
	if resolved[2].UserID == nil || *resolved[2].UserID != 50 {
		t.Errorf("node 2: expected user_id 50, got %v", resolved[2].UserID)
	}
	if resolved[3].RoleID != 104 {
		t.Errorf("node 3: expected fallback role_id 104, got %d", resolved[3].RoleID)
	}
}

func TestResolveFixedRoleInvalidConfig(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyFixedRole, ConfigJSON: `{"role_id":0}`},
		}},
	}
	_, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err == nil {
		t.Fatal("expected error for zero role_id in fixed_role config")
	}
}

func TestResolveFixedUserInvalidConfig(t *testing.T) {
	nodes := []Node{
		{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{
			{StrategyType: AssignmentStrategyFixedUser, ConfigJSON: `{"user_id":0}`},
		}},
	}
	_, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, nil, "", 0)
	if err == nil {
		t.Fatal("expected error for zero user_id in fixed_user config")
	}
}

func TestResolveDocumentFieldDepartmentID(t *testing.T) {
	nodes := []Node{{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyDocumentField, ConfigJSON: `{"field_path":"department_id"}`}}}}
	documentFieldLookup := func(ctx context.Context, documentType string, documentID int64, fieldPath string) (any, bool, error) {
		if documentType != string(ObjectTypeLeaseContract) || documentID != 501 || fieldPath != "department_id" {
			return nil, false, nil
		}
		return int64(300), true, nil
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, documentFieldLookup, string(ObjectTypeLeaseContract), 501)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.RoleID != 100 || r.DepartmentID != 300 || r.UserID != nil {
		t.Fatalf("unexpected resolution: %#v", r)
	}
}

func TestResolveDocumentFieldCreatedBy(t *testing.T) {
	nodes := []Node{{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyDocumentField, ConfigJSON: `{"field_path":"created_by"}`}}}}
	userDeptLookup := func(userID int64) (int64, bool) {
		if userID == 88 {
			return 410, true
		}
		return 0, false
	}
	documentFieldLookup := func(ctx context.Context, documentType string, documentID int64, fieldPath string) (any, bool, error) {
		return int64(88), true, nil
	}
	resolved, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, userDeptLookup, noopDeptLeaderLookup, documentFieldLookup, string(ObjectTypeInvoice), 9001)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	r := resolved[1]
	if r.UserID == nil || *r.UserID != 88 || r.DepartmentID != 410 || r.RoleID != 100 {
		t.Fatalf("unexpected resolution: %#v", r)
	}
}

func TestResolveDocumentFieldResolverError(t *testing.T) {
	nodes := []Node{{ID: 1, Code: "review", RoleID: 100, AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyDocumentField, ConfigJSON: `{"field_path":"department_id"}`}}}}
	documentFieldLookup := func(ctx context.Context, documentType string, documentID int64, fieldPath string) (any, bool, error) {
		return nil, false, context.DeadlineExceeded
	}
	_, err := ResolveAssigneesForNodes(context.Background(), nodes, 50, 200, noopUserDeptLookup, noopDeptLeaderLookup, documentFieldLookup, string(ObjectTypeLeaseContract), 501)
	if err == nil {
		t.Fatal("expected error for document field resolver failure")
	}
}

func noopUserDeptLookup(int64) (int64, bool) { return 0, false }

func noopDeptLeaderLookup(int64) (int64, int64, bool) { return 0, 0, false }
