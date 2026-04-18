FROM golang:1.25-alpine AS builder

ENV GOPROXY=https://goproxy.cn,direct

WORKDIR /app

# Layer 1: Dependency manifests (cached unless go.mod/go.sum change)
COPY backend/go.mod backend/go.sum /app/
RUN go mod download

# Layer 2: Source code + build (rebuilds on any source change)
COPY backend /app
RUN go build -o /out/server ./cmd/server && go build -o /out/dbops ./cmd/dbops

FROM alpine:3.20

RUN adduser -D -u 10001 appuser
RUN apk add --no-cache chromium
WORKDIR /app

COPY --from=builder /out/server /app/server
COPY --from=builder /out/dbops /app/dbops
COPY backend/internal/platform/database/migrations /app/migrations

RUN mkdir -p /app/config /app/logs /app/generated-documents /app/uploads \
    && chown -R appuser:appuser /app \
    && chmod 0755 /app /app/config \
    && chmod 0775 /app/logs /app/generated-documents /app/uploads

USER appuser

EXPOSE 5180

HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=3 \
    CMD wget -q -O /dev/null http://127.0.0.1:5180/healthz || exit 1

CMD ["/app/server"]
