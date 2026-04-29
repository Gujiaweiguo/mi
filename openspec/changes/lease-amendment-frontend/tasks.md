## 1. Amendment entry and routing

- [x] 1.1 Add a dedicated frontend amendment route that reuses `LeaseCreateView.vue` for `/lease/contracts/:id/amend`.
- [x] 1.2 Update `LeaseDetailView.vue` to show an amendment action for amendment-eligible contracts and navigate to the amendment route.

## 2. Amendment drafting flow

- [x] 2.1 Extend `LeaseCreateView.vue` to detect amendment mode, load the source lease, and normalize the lease payload into the existing create-form state including subtype-specific fields, units, and terms.
- [x] 2.2 Submit amendment drafts through `amendLease()` instead of `createLease()` when amendment mode is active, and redirect to the returned amended contract detail page on success.
- [x] 2.3 Add amendment-specific page copy and error handling so operators can distinguish create vs amend behavior without changing the backend contract.

## 3. Verification

- [x] 3.1 Add or update Vue unit tests that cover amendment entry from lease detail, amendment-form prefilling, and amendment submission success behavior.
- [x] 3.2 Run the relevant frontend verification commands for the touched surface, including unit tests and typecheck, and fix any change-caused failures.
- [ ] 3.3 If this change is prepared for CI or archive evidence, record current-commit machine-readable verification artifacts under `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`, remembering that CI requires current-commit unit + integration evidence while archive additionally requires current-commit e2e evidence.
