## 1. Report inventory and acceptance baseline

- [x] 1.1 Reconcile the delivered report surfaces against the frozen archived `R01-R19` inventory and map each report into a verification family.
- [x] 1.2 Confirm the current implementation anchor for each report ID, including query surface, export path, and any visual-only handling for `R19`.

## 2. Family-by-family acceptance verification

- [x] 2.1 Verify the lease/area and contract-ledger reports (`R01`, `R02`, `R11`, `R12`) against matrix fields, filters, output form, and reconciliation checks.
- [x] 2.2 Verify the sales/traffic and budget/pricing reports (`R03`, `R04`, `R05`, `R06`, `R07`, `R10`, `R13`, `R14`, `R15`) against matrix fields, filters, output form, and calculation rules.
- [x] 2.3 Verify the aging/receivable and composite reports (`R08`, `R09`, `R16`, `R17`, `R18`) against matrix fields, filters, bucket or total reconciliation, and cross-report consistency rules.
- [x] 2.4 Verify the visual report `R19` against its output-specific mapping and presentation contract.

## 3. Gap handling and closure contract

- [x] 3.1 Classify any failed matrix checks as required fixes or explicit non-go-live exceptions, and update the change artifacts to reflect those decisions.
- [x] 3.2 Tighten supporting-domain report acceptance behavior and any necessary test coverage so report closure is executable rather than implied.

## 4. Verification and evidence

- [x] 4.1 Run the report-acceptance verification suite needed for the current commit, including any unit/integration/e2e checks or report-focused evidence required to demonstrate closure.
- [x] 4.2 Record machine-readable evidence using `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` for the current commit.
- [x] 4.3 Confirm the change satisfies current gate expectations: CI requires passing `unit` and `integration` evidence for the current commit, while archive requires passing `unit`, `integration`, and `e2e` evidence for the current commit.
