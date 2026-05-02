package workflow

import "testing"

func TestValidateDefinition(t *testing.T) {
	tests := []struct {
		name       string
		definition *Definition
		valid      bool
		issueCode  string
	}{
		{
			name: "valid single step definition",
			definition: &Definition{
				ProcessClass: string(ObjectTypeInvoice),
				Nodes:        []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice)}},
				Transitions:  []Transition{{ToNodeID: 1, Action: ActionSubmit}},
			},
			valid: true,
		},
		{
			name: "valid multi step definition",
			definition: &Definition{
				ProcessClass: string(ObjectTypeLeaseContract),
				Nodes:        []Node{{ID: 1, Code: "manager", ProcessClass: string(ObjectTypeLeaseContract)}, {ID: 2, Code: "finance", ProcessClass: string(ObjectTypeLeaseContract)}},
				Transitions:  []Transition{{ToNodeID: 1, Action: ActionSubmit}, {FromNodeID: int64Pointer(1), ToNodeID: 2, Action: ActionApprove}},
			},
			valid: true,
		},
		{
			name:       "missing nodes",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "missing_nodes",
		},
		{
			name:       "missing transitions",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice)}}},
			issueCode:  "missing_transitions",
		},
		{
			name:       "missing submit transition",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice)}}, Transitions: []Transition{{FromNodeID: int64Pointer(1), ToNodeID: 1, Action: ActionApprove}}},
			issueCode:  "missing_submit_transition",
		},
		{
			name:       "invalid process class",
			definition: &Definition{ProcessClass: "unknown", Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: "unknown"}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "invalid_process_class",
		},
		{
			name:       "duplicate node code",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "dup", ProcessClass: string(ObjectTypeInvoice)}, {ID: 2, Code: "dup", ProcessClass: string(ObjectTypeInvoice)}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "duplicate_node_code",
		},
		{
			name:       "transition to invalid node",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice)}}, Transitions: []Transition{{ToNodeID: 99, Action: ActionSubmit}}},
			issueCode:  "invalid_transition_to_node",
		},
		{
			name:       "unsupported transition action",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice)}}, Transitions: []Transition{{ToNodeID: 1, Action: Action("escalate")}}},
			issueCode:  "unsupported_transition_action",
		},
		{
			name:       "unreachable node",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "entry", ProcessClass: string(ObjectTypeInvoice)}, {ID: 2, Code: "orphan", ProcessClass: string(ObjectTypeInvoice)}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "unreachable_node",
		},
		{
			name:       "missing terminal outcome",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "a", ProcessClass: string(ObjectTypeInvoice)}, {ID: 2, Code: "b", ProcessClass: string(ObjectTypeInvoice)}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}, {FromNodeID: int64Pointer(1), ToNodeID: 2, Action: ActionApprove}, {FromNodeID: int64Pointer(2), ToNodeID: 1, Action: ActionApprove}}},
			issueCode:  "missing_terminal_outcome",
		},
		{
			name:       "unsupported strategy",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice), AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyType("mystery"), ConfigJSON: `{"x":1}`}}}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "unsupported_assignment_strategy",
		},
		{
			name:       "empty assignment config",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice), AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyFixedRole, ConfigJSON: ``}}}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "empty_assignment_rule_config",
		},
		{
			name:       "fixed role missing role id",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice), AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyFixedRole, ConfigJSON: `{}`}}}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "invalid_assignment_rule_config",
		},
		{
			name:       "document field missing path",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice), AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyDocumentField, ConfigJSON: `{}`}}}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "missing_field_path_in_document_field_rule",
		},
		{
			name:       "unsupported document field path",
			definition: &Definition{ProcessClass: string(ObjectTypeInvoice), Nodes: []Node{{ID: 1, Code: "finance", ProcessClass: string(ObjectTypeInvoice), AssignmentRules: []AssignmentRule{{StrategyType: AssignmentStrategyDocumentField, ConfigJSON: `{"field_path":"unsupported_field"}`}}}}, Transitions: []Transition{{ToNodeID: 1, Action: ActionSubmit}}},
			issueCode:  "unsupported_field_reference",
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := validateDefinition(tt.definition)
			if result.Valid != tt.valid {
				t.Fatalf("validateDefinition valid = %v, want %v, issues=%#v", result.Valid, tt.valid, result.Issues)
			}
			if tt.issueCode != "" && !hasValidationIssue(result.Issues, tt.issueCode) {
				t.Fatalf("expected issue code %q, got %#v", tt.issueCode, result.Issues)
			}
		})
	}
}

func hasValidationIssue(issues []DefinitionValidationIssue, code string) bool {
	for _, issue := range issues {
		if issue.Code == code {
			return true
		}
	}
	return false
}

func int64Pointer(value int64) *int64 {
	return &value
}
