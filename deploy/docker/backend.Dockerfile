FROM golang:1.25-alpine AS builder

ENV GOPROXY=https://goproxy.cn,direct

WORKDIR /app
COPY backend /app
RUN go mod download && go build -o /out/server ./cmd/server

FROM alpine:3.20

RUN adduser -D -u 10001 appuser
WORKDIR /app

COPY --from=builder /out/server /app/server

USER appuser

EXPOSE 8080

CMD ["/app/server"]
