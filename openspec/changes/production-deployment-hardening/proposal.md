## Why

The production Docker Compose infrastructure is functional but has several deployment-readiness gaps:
1. **Dockerfile layer caching** is suboptimal — both Dockerfiles copy all source before downloading dependencies, causing full re-download on every source change
2. **No per-directory `.dockerignore`** — build contexts include unnecessary files
3. **Nginx lacks static asset caching** — no `Cache-Control` headers for hashed frontend assets
4. **No API rate limiting** — login endpoint is unprotected against brute force
5. **No named Docker network** — compose uses default bridge, limiting future extensibility
6. **No HEALTHCHECK in Dockerfiles** — containers run standalone without health checks
7. **No TLS documentation** — nginx listens on plain HTTP with no guidance for TLS termination

## What Changes

### Dockerfile Optimization
- Split `COPY` in both Dockerfiles: copy dependency manifests first → download → copy source
- Add `HEALTHCHECK` instructions to both Dockerfiles
- Add per-directory `.dockerignore` files

### Nginx Hardening
- Add `Cache-Control` headers for hashed static assets (1 year)
- Add `limit_req_zone` rate limiting on `/api/auth/login` (10 req/min per IP)
- Add named `upstream` blocks instead of inline proxy_pass

### Docker Compose
- Add explicit named network (`mi-net`)
- Keep existing functionality unchanged

### TLS Documentation
- Add `deploy/docs/tls-termination.md` documenting the recommended approach for TLS (external LB or nginx with certs)

## Scope

Modified files:
- `deploy/docker/backend.Dockerfile`
- `deploy/docker/frontend.Dockerfile`
- `deploy/nginx/production.conf`
- `deploy/compose/docker-compose.production.yml`

New files:
- `backend/.dockerignore`
- `frontend/.dockerignore`
- `deploy/docs/tls-termination.md`
