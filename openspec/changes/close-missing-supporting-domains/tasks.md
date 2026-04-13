## 1. Supporting-domain gap audit and contract alignment

- [x] 1.1 Audit the current supporting-domain implementation to identify the exact missing budget/prospect workflows and the current lifecycle/usability gaps in customer and brand administration.
- [x] 1.2 Confirm the bounded first-release closure contract for this change by mapping the audited gaps to the updated `supporting-domain-management` and `billing-and-invoicing` delta specs.

## 2. Budget/prospect backend and domain closure

- [x] 2.1 Implement the backend domain, persistence, and API surface required to create, list, and update budget/prospect records through supported application workflows.
- [x] 2.2 Ensure budget/prospect records become available to any supported downstream query or report paths that already depend on that domain.
- [x] 2.3 Add backend validation and tests that prove budget/prospect closure does not require direct database edits to keep the domain operational.

## 3. Master-data lifecycle completion

- [x] 3.1 Extend customer and brand administration so operators can update and manage those records through a supported full lifecycle rather than create-only flows.
- [x] 3.2 Add list-scale retrieval behavior for customer and brand administration so operators can locate and maintain records without relying on one unbounded list surface.
- [x] 3.3 Define and implement safe lifecycle handling for records with downstream references so master-data closure does not create inconsistent operational state.

## 4. Frontend administration and dependent workflow alignment

- [x] 4.1 Add or update frontend administration surfaces for budget/prospect so operators can create, review, and update those records through supported UI workflows.
- [x] 4.2 Update the master-data frontend surfaces to expose the completed lifecycle and list-scale usability behaviors for customer and brand management.
- [x] 4.3 Add the narrow billing/invoicing validation or dependency handling needed so financial workflows fail safely when required supporting master data is missing or operationally invalid.

## 5. Verification and acceptance closure

- [x] 5.1 Add or update automated tests covering budget/prospect administration, customer/brand lifecycle maintenance, and the bounded billing dependency checks introduced by this change.
- [x] 5.2 Verify the implementation against the repository evidence convention so CI-ready validation still depends on current-commit `unit` and `integration` evidence under `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json`.
- [x] 5.3 Confirm archive-ready validation for this change with current-commit `unit`, `integration`, and `e2e` evidence under `artifacts/verification/<commit-sha>/` and ensure the resulting implementation matches the updated supporting-domain closure specs.
