FROM golang:1.25-alpine AS builder

ENV GOPROXY=https://goproxy.cn,direct

WORKDIR /app
COPY backend /app
RUN go mod download && go build -o /out/server ./cmd/server

FROM alpine:3.20

RUN adduser -D -u 10001 appuser
RUN apk add --no-cache chromium
WORKDIR /app

COPY --from=builder /out/server /app/server

RUN mkdir -p /app/config /app/logs /app/generated-documents /app/uploads \
    && chown -R appuser:appuser /app \
    && chmod 0755 /app /app/config \
    && chmod 0775 /app/logs /app/generated-documents /app/uploads

USER appuser

EXPOSE 5180

CMD ["/app/server"]
