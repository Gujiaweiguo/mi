package docoutput

import (
	"bytes"
	"context"
	"errors"
	"fmt"
	"html/template"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
)

var (
	ErrTemplateNotFound   = errors.New("print template not found")
	ErrInvalidTemplate    = errors.New("invalid print template")
	ErrInvalidRenderInput = errors.New("invalid print render input")
	ErrChromeUnavailable  = errors.New("chromium renderer unavailable")
)

type Service struct {
	repository     *Repository
	invoiceService *invoice.Service
	leaseService   *lease.Service
	storage        config.StorageConfig
}

func NewService(repository *Repository, invoiceService *invoice.Service, leaseService *lease.Service, storage config.StorageConfig) *Service {
	return &Service{repository: repository, invoiceService: invoiceService, leaseService: leaseService, storage: storage}
}

func (s *Service) UpsertTemplate(ctx context.Context, input UpsertTemplateInput) (*Template, error) {
	templateValue, err := templateFromInput(input)
	if err != nil {
		return nil, err
	}
	tx, err := s.repository.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin print template transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	if err := s.repository.UpsertTemplate(ctx, tx, templateValue); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit print template transaction: %w", err)
	}
	return s.repository.FindTemplateByCode(ctx, templateValue.Code)
}

func (s *Service) ListTemplates(ctx context.Context, filter ListFilter) (*pagination.ListResult[Template], error) {
	return s.repository.ListTemplates(ctx, filter)
}

func (s *Service) RenderHTML(ctx context.Context, input RenderInput) (*Artifact, error) {
	templateValue, documents, err := s.loadRenderableDocuments(ctx, input)
	if err != nil {
		return nil, err
	}
	htmlBytes, err := renderHTML(templateValue, documents)
	if err != nil {
		return nil, err
	}
	return &Artifact{FileName: fmt.Sprintf("%s.html", templateValue.Code), ContentType: "text/html; charset=utf-8", Body: htmlBytes}, nil
}

func (s *Service) RenderPDF(ctx context.Context, input RenderInput) (*Artifact, error) {
	htmlArtifact, err := s.RenderHTML(ctx, input)
	if err != nil {
		return nil, err
	}
	pdfBytes, err := renderPDF(ctx, s.storage, htmlArtifact.FileName, htmlArtifact.Body)
	if err != nil {
		return nil, err
	}
	return &Artifact{FileName: strings.TrimSuffix(htmlArtifact.FileName, ".html") + ".pdf", ContentType: "application/pdf", Body: pdfBytes}, nil
}

func (s *Service) loadRenderableDocuments(ctx context.Context, input RenderInput) (*Template, []printableDocument, error) {
	if strings.TrimSpace(input.TemplateCode) == "" || input.ActorUserID == 0 || len(input.DocumentIDs) == 0 {
		return nil, nil, ErrInvalidRenderInput
	}
	templateValue, err := s.repository.FindTemplateByCode(ctx, strings.TrimSpace(input.TemplateCode))
	if err != nil {
		return nil, nil, err
	}
	if templateValue == nil {
		return nil, nil, ErrTemplateNotFound
	}
	documents := make([]printableDocument, 0, len(input.DocumentIDs))
	for _, documentID := range normalizeDocumentIDs(input.DocumentIDs) {
		document, err := s.loadRenderableDocument(ctx, templateValue.DocumentType, documentID)
		if err != nil {
			return nil, nil, err
		}
		documents = append(documents, document)
	}
	if len(documents) == 0 {
		return nil, nil, ErrInvalidRenderInput
	}
	return templateValue, documents, nil
}

func (s *Service) loadRenderableDocument(ctx context.Context, documentType string, documentID int64) (printableDocument, error) {
	switch documentType {
	case string(invoice.DocumentTypeInvoice), string(invoice.DocumentTypeBill):
		if s.invoiceService == nil {
			return printableDocument{}, ErrInvalidRenderInput
		}
		document, err := s.invoiceService.GetDocument(ctx, documentID)
		if err != nil {
			return printableDocument{}, err
		}
		if document.Status != invoice.StatusApproved || string(document.DocumentType) != documentType {
			return printableDocument{}, ErrInvalidRenderInput
		}
		return printableFromInvoice(*document), nil
	case lease.DocumentTypeContract:
		if s.leaseService == nil {
			return printableDocument{}, ErrInvalidRenderInput
		}
		contract, err := s.leaseService.GetLease(ctx, documentID)
		if err != nil {
			return printableDocument{}, err
		}
		if contract.Status != lease.StatusActive {
			return printableDocument{}, ErrInvalidRenderInput
		}
		return printableFromLease(*contract), nil
	default:
		return printableDocument{}, ErrInvalidRenderInput
	}
}

func templateFromInput(input UpsertTemplateInput) (*Template, error) {
	code := strings.TrimSpace(input.Code)
	name := strings.TrimSpace(input.Name)
	documentType := strings.TrimSpace(input.DocumentType)
	title := strings.TrimSpace(input.Title)
	if code == "" || name == "" || title == "" || input.ActorUserID == 0 {
		return nil, ErrInvalidTemplate
	}
	if !validDocumentType(documentType) {
		return nil, ErrInvalidTemplate
	}
	if !validOutputMode(documentType, input.OutputMode) {
		return nil, ErrInvalidTemplate
	}
	return &Template{Code: code, Name: name, DocumentType: documentType, OutputMode: input.OutputMode, Status: TemplateStatusActive, Title: title, Subtitle: strings.TrimSpace(input.Subtitle), HeaderLines: input.HeaderLines, FooterLines: input.FooterLines, CreatedBy: input.ActorUserID, UpdatedBy: input.ActorUserID}, nil
}

func validDocumentType(documentType string) bool {
	switch documentType {
	case string(invoice.DocumentTypeInvoice), string(invoice.DocumentTypeBill), lease.DocumentTypeContract:
		return true
	default:
		return false
	}
}

func validOutputMode(documentType string, outputMode OutputMode) bool {
	switch documentType {
	case string(invoice.DocumentTypeInvoice):
		return outputMode == OutputModeInvoiceBatch || outputMode == OutputModeInvoiceDetail
	case string(invoice.DocumentTypeBill):
		return outputMode == OutputModeBillState
	case lease.DocumentTypeContract:
		return outputMode == OutputModeLeaseContract
	default:
		return false
	}
}

type printableDocument struct {
	DocumentNo   string
	DocumentType string
	TenantName   string
	Status       string
	PeriodStart  time.Time
	PeriodEnd    time.Time
	TotalAmount  float64
	Lines        []printableLine
}

type printableLine struct {
	ChargeType   string
	ChargeSource string
	PeriodStart  time.Time
	PeriodEnd    time.Time
	QuantityDays int
	UnitAmount   float64
	Amount       float64
}

type renderData struct {
	Template  *Template
	Documents []printableDocument
	Generated string
}

func renderHTML(templateValue *Template, documents []printableDocument) ([]byte, error) {
	tmpl, err := template.New("document-output").Funcs(template.FuncMap{"formatDate": func(value interface{}) string {
		switch v := value.(type) {
		case string:
			return v
		case interface{ Format(string) string }:
			return v.Format(DateLayout)
		default:
			return ""
		}
	}}).Parse(documentHTMLTemplate)
	if err != nil {
		return nil, fmt.Errorf("parse document output template: %w", err)
	}
	buffer := bytes.NewBuffer(nil)
	if err := tmpl.Execute(buffer, renderData{Template: templateValue, Documents: documents, Generated: "print-ready"}); err != nil {
		return nil, fmt.Errorf("execute document output template: %w", err)
	}
	return buffer.Bytes(), nil
}

func printableFromInvoice(document invoice.Document) printableDocument {
	documentNo := ""
	if document.DocumentNo != nil {
		documentNo = *document.DocumentNo
	}
	lines := make([]printableLine, 0, len(document.Lines))
	for _, line := range document.Lines {
		lines = append(lines, printableLine{ChargeType: line.ChargeType, ChargeSource: string(line.ChargeSource), PeriodStart: line.PeriodStart, PeriodEnd: line.PeriodEnd, QuantityDays: line.QuantityDays, UnitAmount: line.UnitAmount, Amount: line.Amount})
	}
	return printableDocument{DocumentNo: documentNo, DocumentType: string(document.DocumentType), TenantName: document.TenantName, Status: string(document.Status), PeriodStart: document.PeriodStart, PeriodEnd: document.PeriodEnd, TotalAmount: document.TotalAmount, Lines: lines}
}

func printableFromLease(contract lease.Contract) printableDocument {
	lines := make([]printableLine, 0, len(contract.Terms))
	totalAmount := 0.0
	for _, term := range contract.Terms {
		quantityDays := int(term.EffectiveTo.Sub(term.EffectiveFrom).Hours()/24) + 1
		lines = append(lines, printableLine{ChargeType: string(term.TermType), ChargeSource: string(billing.ChargeSourceStandard), PeriodStart: term.EffectiveFrom, PeriodEnd: term.EffectiveTo, QuantityDays: quantityDays, UnitAmount: term.Amount, Amount: term.Amount})
		totalAmount += term.Amount
	}
	return printableDocument{DocumentNo: contract.LeaseNo, DocumentType: lease.DocumentTypeContract, TenantName: contract.TenantName, Status: string(contract.Status), PeriodStart: contract.StartDate, PeriodEnd: contract.EndDate, TotalAmount: totalAmount, Lines: lines}
}

func renderPDF(ctx context.Context, storage config.StorageConfig, baseFileName string, htmlBytes []byte) ([]byte, error) {
	chromeBinary, err := resolveChromeBinary()
	if err != nil {
		return nil, err
	}
	baseDir := strings.TrimSpace(storage.GeneratedDocumentsPath)
	if baseDir == "" {
		baseDir = os.TempDir()
	}
	if err := os.MkdirAll(baseDir, 0o755); err != nil {
		return nil, fmt.Errorf("create generated documents directory: %w", err)
	}
	workingDir, err := os.MkdirTemp(baseDir, "docoutput-")
	if err != nil {
		return nil, fmt.Errorf("create document output temp directory: %w", err)
	}
	defer func() { _ = os.RemoveAll(workingDir) }()
	htmlPath := filepath.Join(workingDir, baseFileName)
	pdfPath := strings.TrimSuffix(htmlPath, ".html") + ".pdf"
	if err := os.WriteFile(htmlPath, htmlBytes, 0o644); err != nil {
		return nil, fmt.Errorf("write document output html: %w", err)
	}
	command := exec.CommandContext(ctx, chromeBinary, "--headless", "--disable-gpu", "--no-sandbox", "--print-to-pdf="+pdfPath, "file://"+htmlPath)
	if output, err := command.CombinedOutput(); err != nil {
		return nil, fmt.Errorf("render document output pdf: %w output=%s", err, strings.TrimSpace(string(output)))
	}
	pdfBytes, err := os.ReadFile(pdfPath)
	if err != nil {
		return nil, fmt.Errorf("read rendered document output pdf: %w", err)
	}
	return pdfBytes, nil
}

func resolveChromeBinary() (string, error) {
	for _, candidate := range []string{"google-chrome", "chromium", "chromium-browser"} {
		if path, err := exec.LookPath(candidate); err == nil {
			return path, nil
		}
	}
	return "", ErrChromeUnavailable
}

func normalizeDocumentIDs(ids []int64) []int64 {
	seen := make(map[int64]struct{}, len(ids))
	result := make([]int64, 0, len(ids))
	for _, id := range ids {
		if id == 0 {
			continue
		}
		if _, ok := seen[id]; ok {
			continue
		}
		seen[id] = struct{}{}
		result = append(result, id)
	}
	return result
}

const documentHTMLTemplate = `<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>{{ .Template.Title }}</title>
  <style>
    body { font-family: Arial, sans-serif; margin: 24px; color: #222; }
    h1 { margin-bottom: 4px; }
    h2 { margin-top: 24px; margin-bottom: 8px; }
    .subtitle { color: #555; margin-bottom: 16px; }
    .meta { margin-bottom: 16px; }
    .meta p { margin: 4px 0; }
    table { width: 100%; border-collapse: collapse; margin-top: 12px; }
    th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
    th { background: #f7f7f7; }
    .amount { text-align: right; }
    .footer { margin-top: 24px; font-size: 12px; color: #555; }
  </style>
</head>
<body>
  <h1>{{ .Template.Title }}</h1>
  {{ if .Template.Subtitle }}<div class="subtitle">{{ .Template.Subtitle }}</div>{{ end }}
  {{ range .Template.HeaderLines }}<div class="meta">{{ . }}</div>{{ end }}
  {{ range .Documents }}
  <section>
    <h2>{{ .DocumentNo }}</h2>
    <p>Document Type: {{ .DocumentType }}</p>
    <p>Tenant: {{ .TenantName }}</p>
    <p>Status: {{ .Status }}</p>
    <p>Period: {{ formatDate .PeriodStart }} - {{ formatDate .PeriodEnd }}</p>
    <p>Total Amount: {{ printf "%.2f" .TotalAmount }}</p>
    <table>
      <thead>
        <tr>
          <th>Charge Type</th>
          <th>Source</th>
          <th>Period Start</th>
          <th>Period End</th>
          <th>Quantity Days</th>
          <th>Unit Amount</th>
          <th>Total Amount</th>
        </tr>
      </thead>
      <tbody>
        {{ range .Lines }}
        <tr>
          <td>{{ .ChargeType }}</td>
          <td>{{ .ChargeSource }}</td>
          <td>{{ formatDate .PeriodStart }}</td>
          <td>{{ formatDate .PeriodEnd }}</td>
          <td>{{ .QuantityDays }}</td>
          <td class="amount">{{ printf "%.2f" .UnitAmount }}</td>
          <td class="amount">{{ printf "%.2f" .Amount }}</td>
        </tr>
        {{ end }}
      </tbody>
    </table>
  </section>
  {{ end }}
  <div class="footer">
    {{ range .Template.FooterLines }}<div>{{ . }}</div>{{ end }}
  </div>
</body>
</html>`
