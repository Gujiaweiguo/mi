package errutil

import (
	"errors"
	"fmt"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/lease"
)

func TestSafeMessage_RegisteredError(t *testing.T) {
	got := SafeMessage(lease.ErrLeaseNotFound)
	want := lease.ErrLeaseNotFound.Error()
	if got != want {
		t.Errorf("SafeMessage(registered error) = %q, want %q", got, want)
	}
}

func TestSafeMessage_UnregisteredError(t *testing.T) {
	err := fmt.Errorf("some random error")
	got := SafeMessage(err)
	if got != "internal error" {
		t.Errorf("SafeMessage(unregistered error) = %q, want %q", got, "internal error")
	}
}

func TestSafeMessage_WrappedRegisteredError(t *testing.T) {
	err := fmt.Errorf("wrap: %w", lease.ErrLeaseNotFound)
	got := SafeMessage(err)
	if got != err.Error() {
		t.Errorf("SafeMessage(wrapped registered error) = %q, want %q", got, err.Error())
	}
}

func TestSafeMessage_NilError(t *testing.T) {
	got := SafeMessage(nil)
	if got != "internal error" {
		t.Errorf("SafeMessage(nil) = %q, want %q", got, "internal error")
	}
}

func TestSafeMessage_NonRegisteredSentinel(t *testing.T) {
	sentinel := errors.New("my custom sentinel")
	got := SafeMessage(sentinel)
	if got != "internal error" {
		t.Errorf("SafeMessage(non-registered sentinel) = %q, want %q", got, "internal error")
	}
}

func TestRegister_ThenSafeMessage(t *testing.T) {
	customErr := errors.New("custom domain error")
	Register(customErr)

	got := SafeMessage(customErr)
	if got != customErr.Error() {
		t.Errorf("SafeMessage(after Register) = %q, want %q", got, customErr.Error())
	}
}

func TestRegister_WrappedCustomError(t *testing.T) {
	customErr := errors.New("wrapped custom error")
	Register(customErr)

	wrapped := fmt.Errorf("outer: %w", customErr)
	got := SafeMessage(wrapped)
	if got != wrapped.Error() {
		t.Errorf("SafeMessage(wrapped custom registered) = %q, want %q", got, wrapped.Error())
	}
}
