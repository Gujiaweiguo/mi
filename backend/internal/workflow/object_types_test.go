package workflow

import "testing"

func TestIsValidObjectType(t *testing.T) {
	tests := []struct {
		name  string
		value string
		want  bool
	}{
		{name: "lease contract", value: string(ObjectTypeLeaseContract), want: true},
		{name: "invoice discount", value: string(ObjectTypeInvoiceDiscount), want: true},
		{name: "unknown", value: "membership_card", want: false},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			if got := IsValidObjectType(tt.value); got != tt.want {
				t.Fatalf("IsValidObjectType(%q) = %v, want %v", tt.value, got, tt.want)
			}
		})
	}
}

func TestDefaultTemplateKeyForObjectType(t *testing.T) {
	tests := []struct {
		name       string
		objectType ObjectType
		want       TemplateKey
		ok         bool
	}{
		{name: "lease contract", objectType: ObjectTypeLeaseContract, want: TemplateKeyLeaseApproval, ok: true},
		{name: "lease change", objectType: ObjectTypeLeaseChange, want: TemplateKeyLeaseChangeApproval, ok: true},
		{name: "invoice", objectType: ObjectTypeInvoice, want: TemplateKeyInvoiceApproval, ok: true},
		{name: "overtime", objectType: ObjectTypeOvertimeBill, want: TemplateKeyOvertimeApproval, ok: true},
		{name: "invoice discount", objectType: ObjectTypeInvoiceDiscount, want: TemplateKeyInvoiceDiscountApproval, ok: true},
		{name: "unknown", objectType: ObjectType("unknown"), want: "", ok: false},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got, ok := DefaultTemplateKeyForObjectType(tt.objectType)
			if ok != tt.ok {
				t.Fatalf("DefaultTemplateKeyForObjectType(%q) ok = %v, want %v", tt.objectType, ok, tt.ok)
			}
			if got != tt.want {
				t.Fatalf("DefaultTemplateKeyForObjectType(%q) = %q, want %q", tt.objectType, got, tt.want)
			}
		})
	}
}
