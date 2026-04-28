package errutil

import (
	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/docoutput"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/masterdata"
	"github.com/Gujiaweiguo/mi/backend/internal/taxexport"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
)

func init() {
	Register(
		workflow.ErrDefinitionNotFound,
		workflow.ErrInvalidState,
		lease.ErrLeaseNotFound,
		lease.ErrInvalidLeaseState,
		lease.ErrLeaseAlreadySubmitted,
		lease.ErrLeaseHasBillingDocuments,
		lease.ErrDuplicateLeaseNo,
		lease.ErrLeaseIncompleteForSubmission,
		invoice.ErrDocumentNotFound,
		invoice.ErrInvalidDocumentState,
		invoice.ErrInvalidDocumentInput,
		invoice.ErrDocumentAlreadySubmitted,
		invoice.ErrChargeLineAlreadyDocumented,
		invoice.ErrReceivableContextInvalid,
		invoice.ErrPaymentAmountInvalid,
		invoice.ErrPaymentNotAllowed,
		invoice.ErrPaymentOverApplication,
		invoice.ErrDocumentHasRecordedPayments,
		invoice.ErrDiscountAmountInvalid,
		invoice.ErrDiscountReasonRequired,
		invoice.ErrDiscountNotAllowed,
		invoice.ErrDiscountOverApplication,
		invoice.ErrDiscountPendingApproval,
		invoice.ErrSurplusAmountInvalid,
		invoice.ErrSurplusNotAvailable,
		invoice.ErrSurplusInsufficient,
		invoice.ErrSurplusTargetNotAllowed,
		billing.ErrInvalidBillingWindow,
		taxexport.ErrRuleSetNotFound,
		taxexport.ErrInvalidRuleSet,
		taxexport.ErrInvalidExportWindow,
		taxexport.ErrInvalidTaxSetup,
		docoutput.ErrTemplateNotFound,
		docoutput.ErrInvalidTemplate,
		docoutput.ErrInvalidRenderInput,
		docoutput.ErrChromeUnavailable,
		masterdata.ErrDuplicateCode,
		masterdata.ErrInvalidMasterData,
		masterdata.ErrMasterDataNotFound,
	)
}
