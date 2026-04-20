package notification

import (
	"os"
	"path/filepath"
	"testing"
)

func TestNewRenderer_EmptyDir(t *testing.T) {
	dir := t.TempDir()
	_, err := NewRenderer(dir)
	if err == nil {
		t.Fatal("expected error for empty template directory")
	}
}

func TestNewRenderer_MissingDir(t *testing.T) {
	_, err := NewRenderer("/nonexistent/path")
	if err == nil {
		t.Fatal("expected error for missing template directory")
	}
}

func TestNewRenderer_TemplateMissingBodyBlock(t *testing.T) {
	dir := t.TempDir()
	os.WriteFile(filepath.Join(dir, "bad.html"), []byte(`{{define "subject"}}Hello{{end}}`), 0644)
	_, err := NewRenderer(dir)
	if err == nil {
		t.Fatal("expected error for template missing body block")
	}
}

func TestRenderer_RenderSubjectAndBody(t *testing.T) {
	dir := t.TempDir()
	os.WriteFile(filepath.Join(dir, "test_template.html"), []byte(
		`{{define "subject"}}Hello {{.Name}}{{end}}{{define "body"}}<p>Welcome {{.Name}}</p>{{end}}`,
	), 0644)

	r, err := NewRenderer(dir)
	if err != nil {
		t.Fatalf("NewRenderer: %v", err)
	}

	subject, err := r.RenderSubjectWithData("test_template", map[string]string{"Name": "World"})
	if err != nil {
		t.Fatalf("RenderSubjectWithData: %v", err)
	}
	if subject != "Hello World" {
		t.Fatalf("expected subject 'Hello World', got %q", subject)
	}

	body, err := r.RenderBody("test_template", map[string]string{"Name": "World"})
	if err != nil {
		t.Fatalf("RenderBody: %v", err)
	}
	if body != "<p>Welcome World</p>" {
		t.Fatalf("unexpected body: %q", body)
	}
}

func TestRenderer_MissingTemplate(t *testing.T) {
	dir := t.TempDir()
	os.WriteFile(filepath.Join(dir, "existing.html"), []byte(
		`{{define "subject"}}S{{end}}{{define "body"}}B{{end}}`,
	), 0644)
	r, err := NewRenderer(dir)
	if err != nil {
		t.Fatal(err)
	}
	_, err = r.RenderBody("nonexistent", nil)
	if err == nil {
		t.Fatal("expected error for missing template")
	}
}
