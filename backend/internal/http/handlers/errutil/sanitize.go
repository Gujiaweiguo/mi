package errutil

import "errors"

// domainErrors is a registry of known domain-level sentinel errors whose
// messages are safe to expose to API clients. All other errors receive a
// generic "internal error" message to avoid leaking SQL queries, connection
// strings, file paths, or other internal details.
var domainErrors = map[error]bool{}

// Register marks a sentinel error as safe for client-facing messages.
func Register(errs ...error) {
	for _, err := range errs {
		domainErrors[err] = true
	}
}

// SafeMessage returns the error's own message if it matches a known domain
// sentinel error. Otherwise it returns "internal error".
func SafeMessage(err error) string {
	for domainErr := range domainErrors {
		if errors.Is(err, domainErr) {
			return err.Error()
		}
	}
	return "internal error"
}
