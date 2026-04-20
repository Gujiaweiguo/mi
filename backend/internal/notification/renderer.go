package notification

import (
	"bytes"
	"errors"
	"fmt"
	"html/template"
	"os"
	"path/filepath"
	"strings"
)

const (
	templateSectionSubject = "subject"
	templateSectionBody    = "body"
)

// Renderer caches notification templates loaded from disk.
type Renderer struct {
	templateDir string
	templates   map[string]*template.Template
}

// NewRenderer parses and caches all notification templates from the configured directory.
func NewRenderer(templateDir string) (*Renderer, error) {
	trimmedDir := strings.TrimSpace(templateDir)
	if trimmedDir == "" {
		return nil, errors.New("new notification renderer: template directory is required")
	}

	entries, err := os.ReadDir(trimmedDir)
	if err != nil {
		return nil, fmt.Errorf("read notification template directory: %w", err)
	}

	renderer := &Renderer{templateDir: trimmedDir, templates: make(map[string]*template.Template)}
	for _, entry := range entries {
		if entry.IsDir() || filepath.Ext(entry.Name()) != ".html" {
			continue
		}
		path := filepath.Join(trimmedDir, entry.Name())
		tmpl, parseErr := template.ParseFiles(path)
		if parseErr != nil {
			return nil, fmt.Errorf("parse notification template %s: %w", entry.Name(), parseErr)
		}
		if tmpl.Lookup(templateSectionSubject) == nil || tmpl.Lookup(templateSectionBody) == nil {
			return nil, fmt.Errorf("notification template %s must define %q and %q blocks", entry.Name(), templateSectionSubject, templateSectionBody)
		}
		name := strings.TrimSuffix(entry.Name(), filepath.Ext(entry.Name()))
		renderer.templates[name] = tmpl
	}

	if len(renderer.templates) == 0 {
		return nil, fmt.Errorf("notification template directory %s does not contain any .html templates", trimmedDir)
	}
	return renderer, nil
}

// RenderSubject renders the subject block for a template without external data.
func (r *Renderer) RenderSubject(templateName string) (string, error) {
	return r.renderSection(templateName, templateSectionSubject, nil)
}

// RenderSubjectWithData renders the subject block for a template with structured data.
func (r *Renderer) RenderSubjectWithData(templateName string, data any) (string, error) {
	return r.renderSection(templateName, templateSectionSubject, data)
}

// RenderBody renders the body block for a template with structured data.
func (r *Renderer) RenderBody(templateName string, data any) (string, error) {
	return r.renderSection(templateName, templateSectionBody, data)
}

func (r *Renderer) renderSection(templateName string, section string, data any) (string, error) {
	// Notification templates must define both {{define "subject"}}...{{end}} and
	// {{define "body"}}...{{end}} blocks in the same .html file.
	tmpl, err := r.lookupTemplate(templateName)
	if err != nil {
		return "", err
	}
	var buffer bytes.Buffer
	if execErr := tmpl.ExecuteTemplate(&buffer, section, data); execErr != nil {
		return "", fmt.Errorf("render notification template %s %s: %w", templateName, section, execErr)
	}
	return strings.TrimSpace(buffer.String()), nil
}

func (r *Renderer) lookupTemplate(templateName string) (*template.Template, error) {
	if r == nil {
		return nil, errors.New("notification renderer is nil")
	}
	name := strings.TrimSpace(strings.TrimSuffix(templateName, filepath.Ext(templateName)))
	tmpl, ok := r.templates[name]
	if !ok {
		return nil, fmt.Errorf("notification template %q not found in %s", templateName, r.templateDir)
	}
	return tmpl, nil
}
