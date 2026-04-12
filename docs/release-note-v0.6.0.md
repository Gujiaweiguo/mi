# Release Note v0.6.0

## Summary

This milestone closes the first-release migration scope for the MI replacement system. The repository is in a release-ready state with aligned code, documentation, verification evidence, and deployment defaults.

## Included outcomes

- Reporting acceptance closure for the frozen `R01-R19` first-release report set
- Workflow reminder scheduler hardening and observability closure
- Workflow admin and approval closure, including sequential retry idempotency and concurrent start safety
- Payment and accounts receivable closure, including receivable booking, settlement flow, and over-application guards
- Tax export and document output closure, including Kingdee voucher export, HTML golden checks, and PDF generation
- Deployment defaults documented for the supported `development` and `production` environments
- Backend port aligned to `5180` for both supported environments

## Verified milestone commit

- Green milestone commit: `729c57596bed8a123c149d72b0747b31849e60d7`
- Verification root: `artifacts/verification/729c57596bed8a123c149d72b0747b31849e60d7/`
- Unit: PASS
- Integration: PASS
- E2E: PASS
- CI Ready: YES
- Archive Ready: YES

## Deployment defaults

- Development backend port: `5180`
- Production backend port: `5180`
- Production external HTTP port: `80`
- Production environment file: `deploy/env/production.env`
- Deployment defaults reference: `docs/deployment-defaults.md`

## Notes

- The repository supports `development` and `production` deployment conventions only.
- The release-ready summary remains the canonical consolidated acceptance overview: `docs/release-ready-summary-2026-04-11.md`.
