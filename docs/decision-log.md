# Decision Log

This document records first-release migration decisions against legacy behavior using three statuses:

- `PRESERVE` — business outcome must remain available in first release
- `REDESIGN` — business outcome remains, but implementation model or operator flow changes
- `DROP` — out of first-release scope

## Decision Rules

- Preserve or redesign is decided by **business capability**, not by whether a specific ASPX page exists.
- Legacy technical mechanisms (Crystal Reports, Office Interop, WebForms, SQL Server-specific patterns) are not preserved unless explicitly stated.
- Membership remains dropped from first release.

## Capability Decisions

| Area | Decision | Rationale |
|---|---|---|
| User / role / dept / store admin | REDESIGN | Capability is required, but auth/permission model is explicitly allowed to change. |
| Spatial / shop / area master data | PRESERVE | Required business data foundation for leasing, billing, sales, and reports. |
| Customer / prospect / brand management | PRESERVE | Required to support contract and commercial setup. |
| Lease contract lifecycle | PRESERVE | Core first-release business capability. |
| JV / ad / area contract variants | PRESERVE | Included because first release covers all non-membership business capability. |
| Formula / pricing / invoice parameter management | PRESERVE | Required to reproduce financial outcomes. |
| Payment / collection / financial operations | PRESERVE | Needed to support invoice/billing lifecycle. |
| Charge generation / bill / invoice lifecycle | PRESERVE | Core first-release financial chain. |
| Workflow approval engine | REDESIGN | Approval capability is mandatory, but workflow rules and implementation may change. |
| Sales / POS / commercial transaction capture | PRESERVE | Needed for commercial reports and operational analysis. |
| Generalize media/promotion management | REDESIGN | Legacy media concepts split across sales transaction media/payment details and lease/reporting-facing ad/visual outputs; first release preserves the business outcomes through those domains rather than a standalone module. |
| Generalize reports `R01-R19` | PRESERVE | Explicitly frozen in OpenSpec. |
| Reports outside `R01-R19` | DROP | Out of first-release scope unless OpenSpec changes. |
| Membership / Associator capability | DROP | Explicit first-release exclusion. |
| Membership reports | DROP | Explicit first-release exclusion. |
| Email delivery | DROP | Explicit first-release exclusion. |
| Workflow timeout / escalation automation | DROP | Explicit first-release exclusion. |
| Historical transaction migration | DROP | Fresh-start cutover policy. |

## Output and Integration Decisions

### Print / Document Outputs

| Legacy behavior | Decision | Evidence | Rationale |
|---|---|---|---|
| Lease / invoice / settlement print outputs | PRESERVE | `legacy_code/Web/ReportM/RptLeaseInv.aspx.cs`, `legacy_code/Invoice/InvoiceH/InvoicePrintPO.cs` | Mandatory first-release operator output. |
| Crystal Reports rendering engine | REDESIGN | `legacy_code/Web/ReportM/CrystalReport.aspx.cs`, `.rpt` templates | Output outcome remains; rendering technology changes. |
| Batch print / reprint flows | PRESERVE | `legacy_code/Web/Lease/ChargeAccount/*Print*.aspx.cs` | Operationally important for first release. |

### Voucher / Tax / Export Outputs

| Legacy behavior | Decision | Evidence | Rationale |
|---|---|---|---|
| Kingdee-style voucher/accounting export outcome | PRESERVE | `legacy_code/Invoice/MakePoolVoucher/KingdeeExcelOutPut.cs` | Business export outcome remains required. |
| Office Interop Excel generation for voucher export | REDESIGN | `legacy_code/Invoice/MakePoolVoucher/OutPutExcel.cs`, `Web/凭证模板.xls` | Export outcome remains; COM/Interop mechanism changes. |
| Device/client tax integration | DROP | No first-release target requirement; explicitly excluded in project rules | Out of scope. |

### Excel Import / Export

| Legacy behavior | Decision | Evidence | Rationale |
|---|---|---|---|
| Structured Excel export from operational data | PRESERVE | `legacy_code/Base/ToExcel.cs`, `YYControls/.../Export.cs` | First-release Excel export is mandatory. |
| Structured Excel import for operational/master datasets | PRESERVE | `legacy_code/Web/RentableArea/Building/UnitDataExport.aspx.cs`, `Web/Sell/*.aspx.cs` | First-release Excel import is mandatory. |
| Office Interop / OleDb-based Excel handling | REDESIGN | `Base/ToExcel.cs`, `UnitDataExport.aspx.cs` | Capability remains, technology changes. |

## Workflow Decisions

| Legacy behavior | Decision | Evidence | Rationale |
|---|---|---|---|
| Configurable workflow definitions, nodes, transitions | PRESERVE | `legacy_code/WorkFlow/WrkFlw/` | Approval capability is mandatory. |
| Reflection-based `IConfirmVoucher` / `ITransitWrkFlw` plugin model | REDESIGN | `legacy_code/WorkFlow/FuncApp.cs` | Hook-based side effects remain, but runtime mechanism can change. |
| Workflow audit/reporting semantics | PRESERVE | `legacy_code/WorkFlow/WrkFlwRpt/`, `Lease/CheckApproveMessage.cs` | Needed for operational traceability. |
| Workflow mail notifications | DROP | `legacy_code/WorkFlow/WorkFlowMail/` | Email is deferred from first release. |
| Timeout / escalation handling | DROP | `legacy_code/WorkFlow/WrkFlwNode.cs` timeout fields | Explicitly excluded from first release. |

## Reporting Decisions

| Legacy behavior | Decision | Evidence | Rationale |
|---|---|---|---|
| Invoice, sales, base, group, and traffic reporting outcomes within `R01-R19` | PRESERVE | `legacy_code/Web/ReportM/RptInv/`, `RptSale/`, `RptBase/`, `RptGroup/`, `RptTraf/`; `report-inventory.md` | Frozen first-release report scope. |
| Visual shop analysis output `R19` | PRESERVE | `legacy_code/Web/VisualAnalysis/`, `report-inventory.md` | Included in frozen report scope. |
| Report rendering via Crystal `.rpt` engine | REDESIGN | `legacy_code/Web/ReportM/ReportShow.aspx.cs`, `legacy_code/REPORTWEB/` | Output remains; rendering technology changes. |
| Member report inventory | DROP | `legacy_code/Web/ReportM/RptMember/` | Membership scope excluded. |
| Additional ad hoc BI/reporting beyond `R01-R19` | DROP | OpenSpec report boundary | Out of first-release scope. |

## UI / Interaction Decisions

| Legacy behavior | Decision | Rationale |
|---|---|---|
| Business capability presence | PRESERVE | Users still need the same business outcomes. |
| ASP.NET WebForms page structure | REDESIGN | Target architecture is Vue frontend + Go backend. |
| Operator interaction flow details | REDESIGN | UI and process redesign are explicitly allowed. |

## Explicit Non-Target Legacy Mechanisms

These legacy mechanisms are **not** preserved as implementation choices:

- ASP.NET WebForms page model
- Crystal Reports runtime
- Office Interop Excel generation
- OleDb/Jet Excel ingestion on server
- SQL Server-specific stored/runtime patterns as target architecture

## First-Release Exclusion Check

- Membership capability and reports are marked `DROP`
- Email and workflow timeout/escalation are marked `DROP`
- Historical transaction migration is marked `DROP`
- All required first-release business capabilities are either `PRESERVE` or `REDESIGN`
