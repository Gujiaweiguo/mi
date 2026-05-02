package workflow

import (
	"context"
	"encoding/json"
	"fmt"
	"strings"
)

// AssigneeResolution holds the concrete assignee data for a single workflow step.
type AssigneeResolution struct {
	RoleID       int64
	DepartmentID int64
	UserID       *int64 // nil unless a specific user is resolved
}

// ResolvedStepAssignees maps node ID → resolved assignee data.
type ResolvedStepAssignees map[int64]AssigneeResolution

// UserDepartmentLookup returns the department ID for a given user.
type UserDepartmentLookup func(userID int64) (deptID int64, ok bool)

// DepartmentLeaderLookup returns the leader userID and roleID for a given department.
type DepartmentLeaderLookup func(deptID int64) (userID int64, roleID int64, ok bool)

// DocumentFieldLookup resolves a configured workflow document field path to a
// runtime value for the current business document.
type DocumentFieldLookup func(ctx context.Context, documentType string, documentID int64, fieldPath string) (value any, ok bool, err error)

// ResolveAssigneesForNodes resolves each node to concrete assignee data based on
// the node's first assignment rule strategy. Nodes without rules (or with
// document_field strategy) fall back to the node's RoleID + submitterDeptID.
func ResolveAssigneesForNodes(
	ctx context.Context,
	nodes []Node,
	submitterUserID int64,
	submitterDeptID int64,
	userDeptLookup UserDepartmentLookup,
	deptLeaderLookup DepartmentLeaderLookup,
	documentFieldLookup DocumentFieldLookup,
	documentType string,
	documentID int64,
) (ResolvedStepAssignees, error) {
	resolved := make(ResolvedStepAssignees, len(nodes))
	for _, node := range nodes {
		resolution, err := resolveNode(ctx, node, submitterUserID, submitterDeptID, userDeptLookup, deptLeaderLookup, documentFieldLookup, documentType, documentID)
		if err != nil {
			return nil, fmt.Errorf("resolve assignee for node %d (%s): %w", node.ID, node.Code, err)
		}
		resolved[node.ID] = resolution
	}
	return resolved, nil
}

func resolveNode(
	ctx context.Context,
	node Node,
	submitterUserID int64,
	submitterDeptID int64,
	userDeptLookup UserDepartmentLookup,
	deptLeaderLookup DepartmentLeaderLookup,
	documentFieldLookup DocumentFieldLookup,
	documentType string,
	documentID int64,
) (AssigneeResolution, error) {
	if len(node.AssignmentRules) == 0 {
		return fallbackResolution(node, submitterDeptID), nil
	}

	rule := node.AssignmentRules[0]
	switch rule.StrategyType {
	case AssignmentStrategyFixedRole:
		return resolveFixedRole(rule, submitterDeptID)
	case AssignmentStrategyFixedUser:
		return resolveFixedUser(rule, node.RoleID, userDeptLookup)
	case AssignmentStrategyDepartmentLead:
		return resolveDepartmentLeader(submitterDeptID, deptLeaderLookup)
	case AssignmentStrategySubmitter:
		return resolveSubmitterContext(submitterUserID, submitterDeptID, node.RoleID), nil
	case AssignmentStrategyDocumentField:
		return resolveDocumentField(ctx, rule, node.RoleID, submitterDeptID, userDeptLookup, documentFieldLookup, documentType, documentID)
	default:
		return fallbackResolution(node, submitterDeptID), nil
	}
}

func fallbackResolution(node Node, submitterDeptID int64) AssigneeResolution {
	return AssigneeResolution{
		RoleID:       node.RoleID,
		DepartmentID: submitterDeptID,
	}
}

func resolveFixedRole(rule AssignmentRule, submitterDeptID int64) (AssigneeResolution, error) {
	config := make(map[string]any)
	if err := json.Unmarshal([]byte(rule.ConfigJSON), &config); err != nil {
		return AssigneeResolution{}, fmt.Errorf("parse fixed_role config: %w", err)
	}
	roleID := int64(0)
	if v, ok := config["role_id"]; ok {
		switch n := v.(type) {
		case float64:
			roleID = int64(n)
		case int:
			roleID = int64(n)
		case int64:
			roleID = n
		}
	}
	if roleID <= 0 {
		return AssigneeResolution{}, fmt.Errorf("fixed_role config requires positive role_id")
	}
	return AssigneeResolution{
		RoleID:       roleID,
		DepartmentID: submitterDeptID,
	}, nil
}

func resolveFixedUser(rule AssignmentRule, nodeRoleID int64, userDeptLookup UserDepartmentLookup) (AssigneeResolution, error) {
	config := make(map[string]any)
	if err := json.Unmarshal([]byte(rule.ConfigJSON), &config); err != nil {
		return AssigneeResolution{}, fmt.Errorf("parse fixed_user config: %w", err)
	}
	userID := int64(0)
	if v, ok := config["user_id"]; ok {
		switch n := v.(type) {
		case float64:
			userID = int64(n)
		case int:
			userID = int64(n)
		case int64:
			userID = n
		}
	}
	if userID <= 0 {
		return AssigneeResolution{}, fmt.Errorf("fixed_user config requires positive user_id")
	}
	deptID, ok := userDeptLookup(userID)
	if !ok {
		return AssigneeResolution{
			RoleID:       nodeRoleID,
			DepartmentID: 0,
			UserID:       &userID,
		}, nil
	}
	return AssigneeResolution{
		RoleID:       nodeRoleID,
		DepartmentID: deptID,
		UserID:       &userID,
	}, nil
}

func resolveDepartmentLeader(submitterDeptID int64, deptLeaderLookup DepartmentLeaderLookup) (AssigneeResolution, error) {
	userID, roleID, ok := deptLeaderLookup(submitterDeptID)
	if !ok {
		return AssigneeResolution{}, fmt.Errorf("no department leader found for department %d", submitterDeptID)
	}
	return AssigneeResolution{
		RoleID:       roleID,
		DepartmentID: submitterDeptID,
		UserID:       &userID,
	}, nil
}

func resolveSubmitterContext(submitterUserID, submitterDeptID, nodeRoleID int64) AssigneeResolution {
	return AssigneeResolution{
		RoleID:       nodeRoleID,
		DepartmentID: submitterDeptID,
		UserID:       &submitterUserID,
	}
}

func resolveDocumentField(
	ctx context.Context,
	rule AssignmentRule,
	nodeRoleID int64,
	submitterDeptID int64,
	userDeptLookup UserDepartmentLookup,
	documentFieldLookup DocumentFieldLookup,
	documentType string,
	documentID int64,
) (AssigneeResolution, error) {
	if documentFieldLookup == nil {
		return AssigneeResolution{RoleID: nodeRoleID, DepartmentID: submitterDeptID}, nil
	}
	config := make(map[string]any)
	if err := json.Unmarshal([]byte(rule.ConfigJSON), &config); err != nil {
		return AssigneeResolution{}, fmt.Errorf("parse document_field config: %w", err)
	}
	fieldPath, ok := config["field_path"].(string)
	if !ok || strings.TrimSpace(fieldPath) == "" {
		return AssigneeResolution{}, fmt.Errorf("document_field config requires non-empty field_path")
	}
	value, ok, err := documentFieldLookup(ctx, documentType, documentID, strings.TrimSpace(fieldPath))
	if err != nil {
		return AssigneeResolution{}, err
	}
	if !ok {
		return AssigneeResolution{}, fmt.Errorf("document_field %q is not resolvable for %s:%d", fieldPath, documentType, documentID)
	}
	resolvedID, ok := toPositiveInt64(value)
	if !ok {
		return AssigneeResolution{}, fmt.Errorf("document_field %q resolved to non-positive identifier", fieldPath)
	}
	switch strings.TrimSpace(fieldPath) {
	case "department_id":
		return AssigneeResolution{RoleID: nodeRoleID, DepartmentID: resolvedID}, nil
	case "created_by", "updated_by":
		deptID, ok := userDeptLookup(resolvedID)
		if !ok {
			return AssigneeResolution{RoleID: nodeRoleID, DepartmentID: 0, UserID: &resolvedID}, nil
		}
		return AssigneeResolution{RoleID: nodeRoleID, DepartmentID: deptID, UserID: &resolvedID}, nil
	default:
		return AssigneeResolution{}, fmt.Errorf("document_field %q is not runtime-supported", fieldPath)
	}
}

func toPositiveInt64(value any) (int64, bool) {
	switch v := value.(type) {
	case int:
		if v > 0 {
			return int64(v), true
		}
	case int64:
		if v > 0 {
			return v, true
		}
	case float64:
		if v > 0 {
			return int64(v), true
		}
	}
	return 0, false
}
