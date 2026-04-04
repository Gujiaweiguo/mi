# First-Release Generalize Report Inventory

This inventory is derived from extractable content in `legacy_docs/阳光商业MI.net系统设计.doc`.

Because the source file is a legacy binary `.doc`, the exact formatted Chinese report titles are not fully recoverable from direct text extraction in this environment. The list below is therefore normalized from the report SQL signatures and report-related structures visible in the document. These items define the bounded first-release `Generalize` scope unless the change artifacts are explicitly updated.

## Naming Rules

- `Report ID`: stable implementation and acceptance identifier for first-release scope.
- `中文业务名`: recommended business-facing report name for planning, tasks, and acceptance.
- `English Alias`: technical alias used in cross-language discussion.
- These names are **normalized names**, not guaranteed to be the exact original Word display titles.

## Included Report Inventory

| Report ID | 中文业务名 | English Alias |
|---|---|---|
| `R01` | 出租状态面积汇总表 | Lease Status Area Summary |
| `R02` | 合同台账报表 | Contract Ledger / Contract Master Report |
| `R03` | 业态销售租金分析表 | Shop Type Sales and Rent Analysis |
| `R04` | 商铺日销售分析表 | Daily Shop Sales Analysis |
| `R05` | 铺位预算租价招商对比表 | Unit Budget vs Lease Price vs Potential Tenant Comparison |
| `R06` | 门店租金预算执行表 | Store Rent Budget Execution Report |
| `R07` | 品牌年度销售分布表 | Brand Sales Annual Distribution Report |
| `R08` | 客户应收账龄汇总表 | Customer Accounts Receivable Aging Summary |
| `R09` | 客户应收账龄分费用表 | Customer Accounts Receivable Aging by Charge Type |
| `R10` | 客流年/月汇总表 | Traffic Annual/Monthly Summary Report |
| `R11` | 出租面积与总面积对比表 | Lease Area vs Total Area Report |
| `R12` | 出租空置结构分析表 | Occupancy / Unit Status Structure Report |
| `R13` | 业态销售同比环比分析表 | Shop Type Sales YoY / MoM Analysis |
| `R14` | 坪效分析表 | Sales Efficiency Analysis by Shop Type |
| `R15` | 销售与租金收入对比表 | Sales vs Rent Income Comparison by Shop Type |
| `R16` | 分公司应收账龄汇总表 | Subsidiary Accounts Receivable Aging Summary |
| `R17` | 分公司应收账龄分费用表 | Subsidiary Accounts Receivable Aging by Charge Type |
| `R18` | 客户门店品牌经营综合分析表 | Customer / Store / Brand Sales-Rent-Receivable Composite Report |
| `R19` | 商铺可视化分析输出 | Visual Shop Analysis / Graphic Layout Output |

### R01 — 出租状态面积汇总表
- **English Alias**: Lease Status Area Summary
- Derived from `SELECT aa.deptid,a.RentStatus,sum(a.useArea) useArea`
- Purpose: summarize leased area by store/department and rent status.

### R02 — 合同台账报表
- **English Alias**: Contract Ledger / Contract Master Report
- Derived from contract/customer/trade/subsidiary/store query including `contractcode`, `custcode`, `custname`, `tradename`, `shopcode`, `shopname`, `rentArea`, `brandname`, `shoptypename`, `subsname`, `storename`.
- Purpose: contract-centric operational ledger.

### R03 — 业态销售租金分析表
- **English Alias**: Shop Type Sales and Rent Analysis
- Derived from `select ShopType.ShopTypeName,sum(ConShop.RentArea) rentarea,sum(TransShopMth.PaidAmt) Sales...`
- Purpose: analyze sales, comparable sales, rent area, and rental burden by shop type.

### R04 — 商铺日销售分析表
- **English Alias**: Daily Shop Sales Analysis
- Derived from per-day pivot query over `TransShopDay` with day columns `1..31` and shop-level rental fields.
- Purpose: daily sales distribution by shop, with trade/rent context.

### R05 — 铺位预算租价招商对比表
- **English Alias**: Unit Budget vs Lease Price vs Potential Tenant Comparison
- Derived from query including `unitcode`, `floorarea`, `budget`, `leaseprice`, `potcustomer`, `tradeid`, `brandid`, `avgamt`, `rentalprice`, `rentinc`, `rentmonth`.
- Purpose: compare unit budget, current lease pricing, and potential-customer inputs.

### R06 — 门店租金预算执行表
- **English Alias**: Store Rent Budget Execution Report
- Derived from store/period query joining `invoicedetail`, `invoiceheader`, `conshop`, and `budget/yearbudget`.
- Purpose: compare invoiced/actual rent collection against monthly and yearly budget.

### R07 — 品牌年度销售分布表
- **English Alias**: Brand Sales Annual Distribution Report
- Derived from `select dept.deptname,aa.brandname,year(aa.month) year,sum(aa.saleamt)...` with month bucket columns.
- Purpose: annual/monthly brand sales breakdown by store.

### R08 — 客户应收账龄汇总表
- **English Alias**: Customer Accounts Receivable Aging Summary
- Derived from aging query grouped by `custid`, `custname`, `tradeid`, `tradename`, `subsid`, `subsname`, `contractid`, `deposit`.
- Purpose: aging buckets for outstanding receivables at customer/contract level.

### R09 — 客户应收账龄分费用表
- **English Alias**: Customer Accounts Receivable Aging by Charge Type
- Derived from aging query adding `chargetypename`.
- Purpose: aging analysis broken down by fee/charge type.

### R10 — 客流年/月汇总表
- **English Alias**: Traffic Annual/Monthly Summary Report
- Derived from `trafficdata` aggregation query with yearly and monthly bucket columns.
- Purpose: summarize inbound traffic by store and month.

### R11 — 出租面积与总面积对比表
- **English Alias**: Lease Area vs Total Area Report
- Derived from query selecting `leasearea` and `totalarea` by store/period.
- Purpose: measure leased area against total available area.

### R12 — 出租空置结构分析表
- **English Alias**: Occupancy / Unit Status Structure Report
- Derived from queries using `unitstatus`, `shoptypeid`, `floorarea`, and store hierarchy joins.
- Purpose: summarize occupied/vacant area and structure, including shop-type dimension.

### R13 — 业态销售同比环比分析表
- **English Alias**: Shop Type Sales YoY / MoM Analysis
- Derived from query with `SalesAmt`, `yearSalesAmt`, `lastmonthamt`, `lastyearSalesAmt` by `shoptypeid`.
- Purpose: compare current sales to last month and last year by shop type.

### R14 — 坪效分析表
- **English Alias**: Sales Efficiency Analysis by Shop Type
- Derived from query computing `SalesAmt / monthdays / floorarea`.
- Purpose: analyze sales efficiency per area.

### R15 — 销售与租金收入对比表
- **English Alias**: Sales vs Rent Income Comparison by Shop Type
- Derived from query combining `SalesAmt` and `InvPayAmt` by `shoptypeid` and month.
- Purpose: compare operational sales with rental income.

### R16 — 分公司应收账龄汇总表
- **English Alias**: Subsidiary Accounts Receivable Aging Summary
- Derived from subsidiary-level aging query grouped by `subsid`, `subsname`, `deposit`.
- Purpose: company/subsidiary-level aging summary.

### R17 — 分公司应收账龄分费用表
- **English Alias**: Subsidiary Accounts Receivable Aging by Charge Type
- Derived from subsidiary-level aging query adding `chargetypename`.
- Purpose: subsidiary-level aging analysis broken down by fee/charge type.

### R18 — 客户门店品牌经营综合分析表
- **English Alias**: Customer / Store / Brand Sales-Rent-Receivable Composite Report
- Derived from query selecting `custname`, `storename`, `shopname`, `brandname`, `month`, `rentarea`, `saleamt`, `ppsaleamt`, `lysaleamt`, `InvAmt`, `oweAmt`, `allInvAmt`, `allOweAmt`, and derived坪效 fields.
- Purpose: combined commercial performance and receivable view.

### R19 — 商铺可视化分析输出
- **English Alias**: Visual Shop Analysis / Graphic Layout Output
- Derived from `Web\VisualAnalysis\VAGraphic\Img` and `ShopXml` references.
- Purpose: visual/graphical shop analysis output if the first-release inventory requires the documented visual analysis surface.

## Explicit Exclusions

- Any `Generalize` report not traceable to the extracted design document content above.
- Membership-related reports.
- Any ad hoc BI/dashboard/reporting work beyond this bounded inventory.

## Evidence Notes

- Source extraction files:
  - `.sisyphus/tmp/mi_doc_strings.txt`
  - `.sisyphus/tmp/mi_doc_strings_utf16.txt`
- Key evidence includes report SQL signatures around:
  - `mi_doc_strings_utf16.txt:877-1279`
  - `mi_doc_strings.txt:546-758`
