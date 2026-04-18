FROM node:20-alpine AS builder

WORKDIR /app

# Layer 1: Dependency manifests (cached unless package.json changes)
COPY frontend/package.json frontend/package-lock.json /app/
RUN npm ci

# Layer 2: Source code + build
COPY frontend /app
RUN npm run build

FROM nginx:1.27-alpine

COPY --from=builder /app/dist /usr/share/nginx/html
COPY deploy/nginx/frontend.conf /etc/nginx/conf.d/default.conf

USER nginx

EXPOSE 80

HEALTHCHECK --interval=10s --timeout=3s --start-period=3s --retries=3 \
    CMD wget -q -O /dev/null http://127.0.0.1/ || exit 1
