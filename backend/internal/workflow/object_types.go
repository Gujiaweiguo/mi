package workflow

type ObjectType string

const (
	ObjectTypeLeaseContract   ObjectType = "lease_contract"
	ObjectTypeLeaseChange     ObjectType = "lease_change"
	ObjectTypeInvoice         ObjectType = "invoice"
	ObjectTypeOvertimeBill    ObjectType = "overtime_bill"
	ObjectTypeInvoiceDiscount ObjectType = "invoice_discount"
)

type TemplateKey string

const (
	TemplateKeyLeaseApproval           TemplateKey = "lease-approval"
	TemplateKeyLeaseChangeApproval     TemplateKey = "lease-change"
	TemplateKeyInvoiceApproval         TemplateKey = "invoice-approval"
	TemplateKeyOvertimeApproval        TemplateKey = "overtime-approval"
	TemplateKeyInvoiceDiscountApproval TemplateKey = "invoice-discount-approval"
)

var defaultTemplateKeysByObjectType = map[ObjectType]TemplateKey{
	ObjectTypeLeaseContract:   TemplateKeyLeaseApproval,
	ObjectTypeLeaseChange:     TemplateKeyLeaseChangeApproval,
	ObjectTypeInvoice:         TemplateKeyInvoiceApproval,
	ObjectTypeOvertimeBill:    TemplateKeyOvertimeApproval,
	ObjectTypeInvoiceDiscount: TemplateKeyInvoiceDiscountApproval,
}

func IsValidObjectType(value string) bool {
	_, ok := defaultTemplateKeysByObjectType[ObjectType(value)]
	return ok
}

func DefaultTemplateKeyForObjectType(objectType ObjectType) (TemplateKey, bool) {
	key, ok := defaultTemplateKeysByObjectType[objectType]
	return key, ok
}
