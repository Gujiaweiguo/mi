## Design

### 1. Backend Dockerfile Layer Caching

Current:
```dockerfile
COPY backend /app
RUN cd /app && go mod download && go build ...
```

Optimized:
```dockerfile
COPY backend/go.mod backend/go.sum /app/
RUN cd /app && go mod download
COPY backend /app
RUN cd /app && go build ...
```

This ensures the `go mod download` layer is cached unless `go.mod` or `go.sum` changes.

### 2. Frontend Dockerfile Layer Caching

Current:
```dockerfile
COPY frontend /app
RUN cd /app && npm install && npm run build
```

Optimized:
```dockerfile
COPY frontend/package.json frontend/package-lock.json /app/
RUN cd /app && npm ci
COPY frontend /app
RUN cd /app && npm run build
```

`npm ci` is preferred over `npm install` in CI/Docker for reproducible builds.

### 3. Per-Directory .dockerignore

**`backend/.dockerignore`**:
```
*.test.go
*_test.go
config/development.yaml
```

**`frontend/.dockerignore`**:
```
node_modules/
dist/
*.test.ts
*.spec.ts
e2e/
coverage/
```

### 4. Nginx Production Hardening

**Static asset caching** (for Vite's hashed filenames):
```nginx
location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff2?)$ {
    proxy_pass http://frontend;
    proxy_set_header Host $host;
    expires 1y;
    add_header Cache-Control "public, immutable";
}
```

**Rate limiting on login**:
```nginx
limit_req_zone $binary_remote_addr zone=login:10m rate=10r/m;

location = /api/auth/login {
    limit_req zone=login burst=5 nodelay;
    limit_req_status 429;
    proxy_pass http://backend;
    # ... existing proxy headers
}
```

**Named upstreams**:
```nginx
upstream backend {
    server backend:5180;
}
upstream frontend {
    server frontend:80;
}
```

### 5. Docker Compose Named Network

```yaml
networks:
  mi-net:
    driver: bridge
```

All services use `networks: [mi-net]`.

### 6. HEALTHCHECK in Dockerfiles

**Backend**:
```dockerfile
HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=3 \
    CMD wget -qO- http://127.0.0.1:5180/healthz || exit 1
```

**Frontend**:
```dockerfile
HEALTHCHECK --interval=10s --timeout=3s --start-period=3s --retries=3 \
    CMD wget -qO- http://127.0.0.1/ || exit 1
```

### 7. TLS Documentation

Create `deploy/docs/tls-termination.md` documenting:
- Recommended approach: external load balancer (cloud provider LB, Traefik, Caddy) handles TLS termination
- Alternative: nginx with mounted certs + `listen 443 ssl`
- HSTS header is already set in nginx config

### Verification

- `docker compose -f deploy/compose/docker-compose.production.yml config` validates
- Backend and frontend Dockerfiles build successfully
