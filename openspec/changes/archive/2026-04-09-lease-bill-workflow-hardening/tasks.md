## 1. Lease lifecycle boundary hardening

- [x] 1.1 Enforce stale-state rejection for Lease lifecycle mutations while preserving latest valid state.
- [x] 1.2 Ensure replayed Lease lifecycle commands remain duplicate-safe and do not emit duplicate billing-trigger side effects.

## 2. Billing and invoice consistency hardening

- [x] 2.1 Enforce deterministic rejection for invalid or stale Lease-to-billing generation requests.
- [x] 2.2 Ensure replayed billing/invoice lifecycle and payment commands do not duplicate receivable mutations.

## 3. Verification and acceptance alignment

- [x] 3.1 Add or update integration/e2e coverage for stale-state rejection, duplicate-safe replay, and invalid transition paths across Lease → Bill/Invoice.
- [x] 3.2 Run verification gates and confirm hardened scenarios pass without introducing duplicate downstream effects.
