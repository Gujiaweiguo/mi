package middleware

import (
	"net/http"
	"sync"

	"github.com/gin-gonic/gin"
	"golang.org/x/time/rate"
)

type rateLimiterEntry struct {
	limiter *rate.Limiter
}

var (
	limiters   sync.Map
	limiterMu  sync.Mutex
)

// getLimiter returns a per-IP rate limiter (100 req/s, burst 200).
func getLimiter(ip string) *rate.Limiter {
	if v, ok := limiters.Load(ip); ok {
		return v.(*rateLimiterEntry).limiter
	}
	limiterMu.Lock()
	defer limiterMu.Unlock()
	// Double-check after acquiring lock.
	if v, ok := limiters.Load(ip); ok {
		return v.(*rateLimiterEntry).limiter
	}
	l := rate.NewLimiter(100, 200)
	limiters.Store(ip, &rateLimiterEntry{limiter: l})
	return l
}

// RateLimitMiddleware returns a Gin middleware that applies per-IP rate limiting.
// Each client IP gets 100 requests/second with a burst of 200.
// Returns HTTP 429 Too Many Requests when the limit is exceeded.
func RateLimitMiddleware() gin.HandlerFunc {
	return func(c *gin.Context) {
		ip := c.ClientIP()
		limiter := getLimiter(ip)
		if !limiter.Allow() {
			c.AbortWithStatusJSON(http.StatusTooManyRequests, gin.H{
				"error": "too many requests",
			})
			return
		}
		c.Next()
	}
}
