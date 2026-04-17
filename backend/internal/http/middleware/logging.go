package middleware

import (
	"time"

	"github.com/gin-gonic/gin"
	"go.uber.org/zap"
)

func StructuredLogger(logger *zap.Logger) gin.HandlerFunc {
	if logger == nil {
		logger = zap.NewNop()
	}

	return func(c *gin.Context) {
		startedAt := time.Now()
		c.Next()

		path := c.FullPath()
		if path == "" {
			path = c.Request.URL.Path
		}

		fields := []zap.Field{
			zap.String("method", c.Request.Method),
			zap.String("path", path),
			zap.Int("status", c.Writer.Status()),
			zap.Int64("latency_ms", time.Since(startedAt).Milliseconds()),
			zap.String("client_ip", c.ClientIP()),
		}

		if requestID, ok := c.Get(RequestIDKey); ok {
			if rid, ok := requestID.(string); ok && rid != "" {
				fields = append(fields, zap.String("request_id", rid))
			}
		}

		if sessionUser, ok := CurrentSessionUser(c); ok {
			fields = append(fields, zap.Int64("user_id", sessionUser.ID))
		}

		status := c.Writer.Status()
		switch {
		case status >= 500:
			logger.Error("request completed", fields...)
		case status >= 400:
			logger.Warn("request completed", fields...)
		default:
			logger.Info("request completed", fields...)
		}
	}
}
