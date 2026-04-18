## 1. Dockerfile Optimization

- [x] 1.1 Optimize `deploy/docker/backend.Dockerfile` for layer caching (COPY go.mod first)
- [x] 1.2 Optimize `deploy/docker/frontend.Dockerfile` for layer caching (COPY package.json first)
- [x] 1.3 Add `backend/.dockerignore`
- [x] 1.4 Add `frontend/.dockerignore`
- [x] 1.5 Add HEALTHCHECK to backend Dockerfile
- [x] 1.6 Add HEALTHCHECK to frontend Dockerfile

## 2. Nginx Hardening

- [x] 2.1 Add named upstream blocks in `deploy/nginx/production.conf`
- [x] 2.2 Add static asset caching headers for hashed files
- [x] 2.3 Add rate limiting on `/api/auth/login`

## 3. Docker Compose

- [x] 3.1 Add named network `mi-net` to `deploy/compose/docker-compose.production.yml`

## 4. Documentation

- [x] 4.1 Create `deploy/docs/tls-termination.md`

## 5. Verification

- [x] 5.1 `docker compose -f deploy/compose/docker-compose.production.yml config` validates
