package middleware

import (
	"fmt"
	"runtime/debug"

	"github.com/gin-gonic/gin"
	"go.uber.org/zap"
)

func StructuredRecovery(logger *zap.Logger) gin.HandlerFunc {
	if logger == nil {
		logger = zap.NewNop()
	}

	return func(c *gin.Context) {
		defer func() {
			if recovered := recover(); recovered != nil {
				path := c.FullPath()
				if path == "" {
					path = c.Request.URL.Path
				}

				fields := []zap.Field{
					zap.String("method", c.Request.Method),
					zap.String("path", path),
					zap.String("client_ip", c.ClientIP()),
					zap.String("error", fmt.Sprint(recovered)),
					zap.ByteString("stack", debug.Stack()),
				}

				if requestID, ok := c.Get(RequestIDKey); ok {
					if rid, ok := requestID.(string); ok && rid != "" {
						fields = append(fields, zap.String("request_id", rid))
					}
				}

				logger.Error("panic recovered", fields...)
				c.AbortWithStatusJSON(500, gin.H{"message": "internal server error"})
			}
		}()

		c.Next()
	}
}
