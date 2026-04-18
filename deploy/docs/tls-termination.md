# TLS Termination

## Recommended Approach

The MI production nginx listens on HTTP port 80. TLS termination should be handled by an external load balancer or reverse proxy deployed in front of the Docker Compose stack.

### Option A: Cloud Provider Load Balancer (Recommended)

- Configure your cloud LB (AWS ALB, GCP LB, Azure LB) to terminate TLS
- Forward plain HTTP to the Docker host on the configured port (default 80)
- The LB handles certificate management and renewal

### Option B: Nginx with Certificates

Mount certificate files and modify `deploy/nginx/production.conf`:

```nginx
server {
    listen 443 ssl;
    ssl_certificate /etc/nginx/certs/tls.crt;
    ssl_certificate_key /etc/nginx/certs/tls.key;
    # ... rest of config
}
```

Add volume mount in `docker-compose.production.yml`:

```yaml
volumes:
  - ../certs:/etc/nginx/certs:ro
```

### Option C: Caddy or Traefik

Add a Caddy or Traefik service to the Docker Compose stack for automatic certificate management with Let's Encrypt.

## HSTS

The nginx config already sets the `Strict-Transport-Security` header. This is effective once TLS is active upstream of or at the nginx layer.
