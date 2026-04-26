# Legacy Capability Inventory

This document freezes the legacy behavior inventory for the first-release migration scope. It is organized by **business capability**, not by ASP.NET page or project namespace.

## Inventory Rules

- `legacy_code/` and `legacy_docs/` are **behavior references only**.
- This inventory identifies **first-release capabilities by business outcome**.
- Membership/Associator is explicitly excluded from first release.
- Cross-reference implementation and acceptance status in `docs/capability-traceability-matrix.md`.
- Reporting scope is bounded separately by:
  - `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`
  - `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`

## First-Release Capability Slices

### 1. Organization, Users, Roles, and Store Administration
**Business outcome**: Operators can log in, be assigned roles/permissions, operate within department/store scope, and maintain core administrative master data.

**Legacy evidence**
- `legacy_code/BaseInfo/User/` — user, session, user-role, user-auth, menu visibility
- `legacy_code/BaseInfo/Role/` — role, role-auth, contract/shop authorization, menu/function trees
- `legacy_code/BaseInfo/Dept/` — department hierarchy and department auth
- `legacy_code/BaseInfo/Store/` — store master, store license, traffic counter/door, POS/server task metadata
- `legacy_code/Web/BaseInfo/` — admin UI for dept, role, store, and user operations
- `legacy_code/Web/Login.aspx` + `legacy_code/Web/Main.aspx` — operator entry and main shell

**First-release notes**
- Required for all downstream modules
- Target system may redesign the permission model, but must preserve operational control points

---

### 2. Spatial / Commercial Master Data
**Business outcome**: Stores, buildings, floors, units, areas, shop types, trade categories, and related commercial dimensions can be maintained and reused by leasing, sales, and reporting.

**Legacy evidence**
- `legacy_code/RentableArea/` — building/floor/location/unit/area/trade/product relation hierarchy
- `legacy_code/Shop/` — shop type, fitment, tree operations
- `legacy_code/Web/RentableArea/` — UI for spatial structure maintenance
- `legacy_code/Web/Shop/` — shop UI

**First-release notes**
- Required as foundational master data for Lease, Bill, Invoice, Sell, and reports

---

### 3. Customer, Prospect, Brand, and Negotiation Management
**Business outcome**: The system can maintain formal customers, prospective customers, brands, licenses/contacts, and commercial negotiation context used before contract activation.

**Legacy evidence**
- `legacy_code/Lease/Customer/`, `legacy_code/Lease/Cust/` — customer entities
- `legacy_code/Lease/CustLicense/` — customer contacts, licenses, operator info
- `legacy_code/Lease/PotCust/` — prospect classification such as client level, credit level, source, process type
- `legacy_code/Lease/PotCustLicense/` — prospect records and prospect license info
- `legacy_code/Lease/Brand/` — brand master and prospect/customer brand relations
- `legacy_code/Web/Lease/Customer/` + `legacy_code/Web/Lease/PotCust/` — UI for customer/prospect operations

**First-release notes**
- Required because Lease initiation depends on customer/prospect and brand context

---

### 4. Lease Contract Lifecycle
**Business outcome**: Operators can create, approve, activate, amend, suspend, and terminate commercial contracts, including the business data needed to drive billing and invoicing.

**Legacy evidence**
- `legacy_code/Lease/Contract.cs` — root contract entity and status fields
- `legacy_code/Lease/ConLease.cs` — lease-specific billing cycle, tax, settlement, and currency fields
- `legacy_code/Lease/Contract/` — contract signing/confirmation/query
- `legacy_code/Lease/ContractMod/` — contract modification flows
- `legacy_code/Lease/ChangeLease/` — change-lease workflows
- `legacy_code/Lease/ConShop/` — shop/brand/unit binding to contract
- `legacy_code/Lease/ConTerminateBill/` — termination billing
- `legacy_code/Web/Lease/AuditingLease/`, `Web/Lease/ChangeLease/`, `Web/Lease/LeaseContractStop/` — UI for lifecycle handling

**Cross-module dependencies**
- depends on customer/prospect, brand, shop/unit, workflow, and invoice parameterization

**First-release notes**
- This is the anchor capability for the priority chain `Lease -> Bill / Invoice`

---

### 5. Joint Venture / Special Contract Modes
**Business outcome**: The system can handle non-standard commercial contract modes such as joint venture and advertising/area contracts where applicable.

**Legacy evidence**
- `legacy_code/Lease/Union/ConUnion.cs` — joint venture contract model
- `legacy_code/Lease/AdContract/` — ad board / area contract models and confirm flows
- `legacy_code/Web/Lease/LeaseConUnion/` — JV UI
- `legacy_code/Web/Lease/AdContract/` — ad/area contract UI

**First-release notes**
- Included because first release covers all non-membership business capability
- Should be explicitly tracked as sub-slices under Lease rather than treated as optional add-ons

---

### 6. Formula, Pricing, and Billing Parameter Management
**Business outcome**: Contract-linked rent formulas, invoice parameters, rates, and pricing drivers can be configured and reused by downstream charge generation.

**Legacy evidence**
- `legacy_code/Lease/Formula/` — formula hierarchy (`ConFormulaH/M/P`), exchange rates, currency types
- `legacy_code/Lease/InvoicePara/` — invoice parameterization across lease/ad/area/JV
- `legacy_code/Lease/PotBargain/` — charge/pay type setup for negotiations

**First-release notes**
- Required for correct downstream billing and invoice generation

---

### 7. Payment, Collection, and Financial Operations
**Business outcome**: The system can track incoming/outgoing payments, deposits, surpluses, adjustments, discounts, interest, and linked financial movements around charges and invoices.

**Legacy evidence**
- `legacy_code/Lease/PayIn/` — PayIn / PayOut
- `legacy_code/Invoice/InvoiceH/` — invoice payment, deposit balance, surplus, discount, adjustment, cancellation
- `legacy_code/Invoice/InterestRate.cs`, `InvoiceInterest.cs` — interest handling
- `legacy_code/Invoice/BankCard/` — bank-card transaction reconciliation
- `legacy_code/Web/Invoice/` — UI for these operations

**First-release notes**
- Included as part of the financial chain, not just “invoice printing”

---

### 8. Charge Generation, Bill, and Invoice Lifecycle
**Business outcome**: The system can generate charges from approved commercial state, create bill/invoice records, confirm financial documents, support adjustments/cancellations, and preserve document status transitions.

**Legacy evidence**
- `legacy_code/Invoice/Charge.cs`, `ChargeDetail.cs`, `ChargeAccount.cs`, `ChargeCountLog.cs`
- `legacy_code/Invoice/InvoiceHeader.cs`, `InvoiceDetail.cs`, `InvoiceH/`
- `legacy_code/Bill/BillInfo.cs` — billing instrument record
- `legacy_code/Bill/ConfirmVoucher.cs` — post-approval bill side effects
- `legacy_code/Web/Lease/ChargeAccount/` — operational UI across charge/invoice actions
- `legacy_code/Web/Invoice/InvoiceHeader/`, `MakePoolVoucher/`, `BankCard/` — invoice financial UI

**Cross-module dependencies**
- depends on Lease contract status, formulas, workflow approval, payment handling, and outputs

**First-release notes**
- Highest-priority financial chain alongside Lease

---

### 9. Workflow / Approval Engine
**Business outcome**: The system can define approval flows, route documents through configurable workflow states, execute confirm handlers, and maintain workflow audit/report data.

**Legacy evidence**
- `legacy_code/WorkFlow/WrkFlw/` — workflow definition, node, entity, transfer, transit, group structures
- `legacy_code/WorkFlow/Func/IConfirmVoucher.cs` — confirm hook interface
- `legacy_code/WorkFlow/Func/ITransitWrkFlw.cs` — transition hook interface
- `legacy_code/WorkFlow/FuncApp.cs` — reflection-based runtime dispatch
- `legacy_code/WorkFlow/WrkFlwRpt/` — workflow reporting
- `legacy_code/Web/WorkFlow/`, `Web/WorkFlowEntity/`, `Web/WorkFlowRpt/` — workflow admin/query/report UI

**First-release notes**
- Mandatory capability
- Timeout/escalation automation exists in legacy semantics but is excluded from first release target

---

### 10. Sales / POS / Commercial Transaction Capture
**Business outcome**: The system can ingest and manage sales transactions, receipts, SKU-level details, media sales, and cashier/payment records used by reports and commercial analysis.

**Legacy evidence**
- `legacy_code/Sell/TransHeader.cs`, `TransDetail.cs`, `TransSku.cs`
- `legacy_code/Sell/SkuMaster.cs` — SKU master
- `legacy_code/Sell/ShopSellReceipt.cs`
- `legacy_code/Sell/Media*.cs` — media-related commercial records
- `legacy_code/Web/Sell/` — sales/POS UI and imports

**First-release notes**
- Included because report scope and operational analysis depend on sales data

---

### 11. Generalize / Media / Promotion Management
**Business outcome**: The system can maintain media/promotion channel data used by the Generalize business area.

**Legacy evidence**
- `legacy_code/Generalize/Medium/` — print/radio/TV/internet/activity/display/theme models
- `legacy_code/Web/Generalize/Medium/` — UI for media operations

**First-release notes**
- Included as a business area separate from Generalize reports
- Reporting scope is bounded separately by `R01-R19`

---

### 12. Reporting and Visual Analysis
**Business outcome**: Operators can query, render, print, and export the frozen first-release Generalize report set plus related invoice/sales/base/traffic analysis outputs.

**Legacy evidence**
- `legacy_code/Web/ReportM/` — central in-app report hub
- `legacy_code/Web/ReportM/RptInv/` — invoice/charge/aging reports
- `legacy_code/Web/ReportM/RptSale/` — sales/commercial reports
- `legacy_code/Web/ReportM/RptBase/` — base/master/contract reports
- `legacy_code/Web/ReportM/RptGroup/` — grouped/store/company reports
- `legacy_code/Web/ReportM/RptTraf/` — traffic reports
- `legacy_code/Web/VisualAnalysis/` — visual analysis, floor-plan/graphic outputs
- `legacy_code/REPORTWEB/` — additional Crystal Reports web app and template store
- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md` — first-release report set `R01-R19`

**First-release notes**
- Only `R01-R19` are authoritative for first release
- Membership reports are excluded

---

### 13. Print, Voucher Export, and Excel I/O Outputs
**Business outcome**: The system can produce mandatory operational outputs: invoice/settlement printouts, accounting voucher exports, and structured Excel imports/exports.

**Legacy evidence**
- **Print / documents**
  - `legacy_code/Invoice/InvoiceH/InvoicePrintPO.cs`
  - `legacy_code/Web/ReportM/RptLeaseInv.aspx.cs`
  - `legacy_code/Web/Lease/ChargeAccount/*Print*.aspx.cs`
  - `legacy_code/Web/ReportM/CrystalReport.aspx.cs`, `ReportShow.aspx.cs`
- **Voucher / tax-adjacent export**
  - `legacy_code/Invoice/MakePoolVoucher/KingdeeExcelOutPut.cs`
  - `legacy_code/Invoice/MakePoolVoucher/KingdeeExcel.cs`
  - `legacy_code/Web/Invoice/MakePoolVoucher/MakePoolVoucher.aspx.cs`
- **Excel export / import**
  - `legacy_code/Base/ToExcel.cs`
  - `legacy_code/YYControls/SmartGridView/Export/Export.cs`
  - `legacy_code/Web/RentableArea/Building/UnitDataExport.aspx.cs`
  - `legacy_code/Web/Sell/SkuInput.aspx.cs`, `SellData.aspx.cs`

**First-release notes**
- Treated as first-class workstreams, not incidental page features

---

## Explicit First-Release Exclusions

### Membership / Associator
**Business outcome**: membership, cards, bonus points, gifts, redemption, demographic loyalty operations

**Legacy evidence**
- `legacy_code/Associator/Associator/`
- `legacy_code/Associator/Perform/`
- `legacy_code/Web/Associator/`
- `legacy_code/Web/ReportM/RptMember/`

**First-release status**
- Excluded entirely from the first release

---

## Shared Infrastructure / Technical Foundations in Legacy System

These are not business capabilities themselves, but they explain how legacy behavior is currently implemented:

- `legacy_code/Base/DB/` — DB access / pooled datasource / PO base classes
- `legacy_code/Base/Biz/` — BO / transaction / tree helpers
- `legacy_code/Base/Page/` — ASP.NET page infrastructure
- `legacy_code/Base/Sys/` — XML/config helpers
- `legacy_code/Base/XML/` — chart XML generation

These should inform behavior extraction only; they are not the target implementation architecture.

---

## Cross-Capability Dependency Notes

- **Lease** depends on customer/prospect, brand, shop/unit, workflow, pricing formulas, and invoice parameters.
- **Bill / Invoice** depend on approved Lease state, workflow, and payment/financial operations.
- **Workflow** is a hub capability used by Lease, Bill, and Invoice through confirm/transition handlers.
- **Reports** consume data across Lease, Invoice, Sell, BaseInfo, RentableArea, and workflow-related operational states.
- **Print / voucher export / Excel I/O** sit across Lease + Invoice + reports and must be treated as cross-cutting outputs.

---

## First-Release Inventory Check

This inventory includes:

- all non-membership business capability slices
- the priority operational chain `Lease -> Bill / Invoice`
- approval/workflow behavior
- report/visual analysis scope references
- print / voucher / Excel output capability references

This inventory excludes:

- page-by-page ASP.NET migration planning
- legacy implementation technology as target architecture
- membership capability
