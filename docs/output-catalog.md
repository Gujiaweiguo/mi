# Output Catalog

This catalog freezes the mandatory first-release outputs for `legacy-system-migration`.

## Output Rules

- First-release outputs include print/document outputs, voucher/tax-adjacent exports, Excel import/export flows, and frozen reports `R01-R19`.
- Membership outputs are excluded.
- Device/client tax integration is excluded.
- Email delivery is excluded.
- Historical record backfill outputs are excluded.

## Category A — Mandatory Operational Print / Document Outputs

| Output ID | Output | Scope | Legacy Evidence | Target Form |
|---|---|---|---|---|
| `P01` | Lease invoice batch print | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/InvoiceBancthPrint.aspx.cs` | HTML + PDF |
| `P02` | Lease invoice batch print (area) | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/InvoiceBancthPrintArea.aspx.cs` | HTML + PDF |
| `P03` | Lease invoice batch print (ad board) | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/InvoiceBancthPrintAdBoard.aspx.cs` | HTML + PDF |
| `P04` | JV / unit invoice batch print | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/InvoiceJVBancthPrint.aspx.cs` | HTML + PDF |
| `P05` | Lease invoice reprint | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/InvoiceAgainPrint.aspx.cs` | HTML + PDF |
| `P06` | JV / unit invoice reprint | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/UnionAgainPrint.aspx.cs` | HTML + PDF |
| `P07` | Interest invoice print | Lease/Bill/Invoice | `legacy_code/Web/Lease/ChargeAccount/InterestAgainPrint.aspx.cs` | HTML + PDF |
| `P08` | Paid voucher print (redesigned) | Financial ops | `legacy_code/Web/ReportM/RptInv/RptPaidVoucher.aspx.cs` | Supported through current voucher/export output surfaces rather than a standalone first-release print flow |
| `P09` | Accounting voucher report print (redesigned) | Voucher export support | `legacy_code/Web/Invoice/MakePoolVoucher/MakePoolVoucher.aspx.cs` + `RptAccountReport.rpt` | Supported through current voucher/export output surfaces rather than a standalone first-release print flow |
| `P10` | Invoice detail print | Lease/Bill/Invoice | `legacy_code/Web/ReportM/RptLeaseInv.aspx.cs` | HTML + PDF |
| `P11` | Bill post-approval printable state | Bill | `legacy_code/Bill/ConfirmVoucher.cs` | Printable document state |

### Print Output Rules
- Lease/Bill/Invoice print outputs are mandatory for first release.
- Legacy Crystal Report rendering is **not** the target implementation.
- Template configurability implied by `InvoicePara` / `InvoiceJVPara` must remain available as a business capability.
- Batch print and reprint are both required where the legacy system supports them for first-release financial documents.
- `P08` and `P09` are redesign boundaries. First release may satisfy the same operator outcome through the voucher-export and printable-output surfaces already present in the current tax/document stack, rather than reproducing a standalone legacy voucher-print subsystem.

## Category B — Mandatory Voucher / Tax-Adjacent Export Outputs

| Output ID | Output | Scope | Legacy Evidence | Target Form |
|---|---|---|---|---|
| `T01` | Kingdee-style voucher export | Finance / tax-adjacent export | `legacy_code/Invoice/MakePoolVoucher/KingdeeExcelOutPut.cs` | XLSX export |
| `T02` | Voucher/accounting export preview (redesigned) | Finance / tax-adjacent export | `legacy_code/Invoice/MakePoolVoucher/MakePoolVoucher.aspx.cs` | Supported through current tax rule set UI plus workbook generation flow |
| `T03` | Voucher configuration import (redesigned) | Finance setup | `legacy_code/Web/Invoice/MakePoolVoucher/VoucherInput.aspx.cs` | Supported through current tax rule set maintenance surfaces |

### Voucher Export Rules
- The business export outcome is mandatory.
- Legacy Kingdee field semantics must be preserved at the business/output level.
- Office Interop and `.xls` template binding are not preserved as implementation choices.
- The exact export field layout must be pinned before Task 11 is considered complete.
- `T02` and `T03` are redesign boundaries: first release may satisfy them through current rule-set maintenance plus workbook-generation/export flows, rather than separate legacy preview/import pages.

## Category C — Mandatory Excel Import / Export Flows

| Output ID | Output | Scope | Legacy Evidence | Target Form |
|---|---|---|---|---|
| `E01` | Generic tabular export from operational views | Shared | `legacy_code/Base/ToExcel.cs` | XLSX export |
| `E02` | Sales data import | Sell | `legacy_code/Web/Sell/SellData.aspx.cs` | Structured import |
| `E03` | Sales support master import (redesigned from legacy SKU master import) | Sell | `legacy_code/Web/Sell/SkuInput.aspx.cs` | Structured import |
| `E04` | Unit data template download + import | Spatial master | `legacy_code/Web/RentableArea/Building/UnitDataExport.aspx.cs` | XLSX template + import |
| `E05` | Voucher export workbook | Finance export | `legacy_code/Web/Invoice/MakePoolVoucher/MakePoolVoucher.aspx.cs` | XLSX export |

### Excel I/O Rules
- Excel import/export is mandatory in first release.
- `E03` is a redesign boundary: the legacy SKU-master import reflected the old POS/SKU model. First release may satisfy the same business outcome through the current sales-support import surfaces instead of reproducing a standalone SKU master module.
- All accepted imports must provide deterministic validation and row-level diagnostics where applicable.
- Server-side Office Interop and OleDb/Jet are not preserved as implementation choices.

## Category D — Frozen Report Outputs `R01-R19`

These report outputs are mandatory exactly as bounded by OpenSpec.

| Report ID | Report Name | Output Form | Reference |
|---|---|---|---|
| `R01` | 出租状态面积汇总表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R02` | 合同台账报表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R03` | 业态销售租金分析表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R04` | 商铺日销售分析表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R05` | 铺位预算租价招商对比表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R06` | 门店租金预算执行表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R07` | 品牌年度销售分布表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R08` | 客户应收账龄汇总表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R09` | 客户应收账龄分费用表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R10` | 客流年/月汇总表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R11` | 出租面积与总面积对比表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R12` | 出租空置结构分析表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R13` | 业态销售同比环比分析表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R14` | 坪效分析表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R15` | 销售与租金收入对比表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R16` | 分公司应收账龄汇总表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R17` | 分公司应收账龄分费用表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R18` | 客户门店品牌经营综合分析表 | Query + XLSX | `report-inventory.md`, `report-acceptance-matrix.md` |
| `R19` | 商铺可视化分析输出 | Visual output | `report-inventory.md`, `report-acceptance-matrix.md` |

### Report Rules
- `R01-R19` are the only authoritative first-release Generalize report outputs.
- `R01-R18` must at minimum support page query + `.xlsx` export.
- `R19` must support visual/graphical rendering consistent with the frozen acceptance matrix.
- Reports outside `R01-R19` are not part of first release.

## Explicit Exclusions

- membership-related outputs
- email-delivered outputs
- device/client tax integration outputs
- ad hoc reports outside `R01-R19`
- REPORTWEB legacy report estate outside the frozen first-release scope

## Output Acceptance References

- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`
- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`
- `docs/decision-log.md`
- `docs/cutover-runbook.md`

## First-Release Output Check

Task 2 output scope is complete only when:

- mandatory operational print outputs are listed
- mandatory voucher/tax-adjacent exports are listed
- mandatory Excel import/export flows are listed
- frozen report outputs `R01-R19` are listed
- exclusions are explicit
