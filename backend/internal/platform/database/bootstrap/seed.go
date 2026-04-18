package bootstrap

import (
	"context"
	"database/sql"
)

// Bootstrapper is the interface for database seed operations.
// Implementations provide a name and a Seed function that runs
// within a transaction.
type Bootstrapper interface {
	Name() string
	Seed(context.Context, *sql.Tx) error
}

type Seed struct {
	name string
	run  func(context.Context, *sql.Tx) error
}

func (s Seed) Name() string {
	return s.name
}

func (s Seed) Seed(ctx context.Context, tx *sql.Tx) error {
	return s.run(ctx, tx)
}

func New(name string, run func(context.Context, *sql.Tx) error) Bootstrapper {
	return Seed{name: name, run: run}
}

func All() []Bootstrapper {
	all := make([]Bootstrapper, 0)

	for _, seed := range Cutover() {
		all = append(all, seed)
	}
	for _, seed := range []Bootstrapper{seedDailySales(), seedCustomerTraffic(), seedUnitRentBudgets(), seedStoreRentBudgets(), seedUnitProspects()} {
		all = append(all, seed)
	}

	return all
}

func Cutover() []Bootstrapper {
	all := make([]Bootstrapper, 0)

	for _, seed := range OrgSeeds() {
		all = append(all, seed)
	}
	all = append(all, seedBusinessGroups(), seedNumberingSequences())
	for _, seed := range AccessSeeds() {
		all = append(all, seed)
	}
	for _, seed := range CutoverCommercialSeeds() {
		all = append(all, seed)
	}
	for _, seed := range workflowDefinitionSeeds() {
		all = append(all, seed)
	}

	return all
}
