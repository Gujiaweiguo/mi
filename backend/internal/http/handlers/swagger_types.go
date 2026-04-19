package handlers

type swaggerEnvelope struct{}

type swaggerMessageResponse struct {
	Message string `json:"message,omitempty"`
	Error   string `json:"error,omitempty"`
}
