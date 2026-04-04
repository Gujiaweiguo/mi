package bootstrap

import (
	"context"
	"database/sql"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
)

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

func New(name string, run func(context.Context, *sql.Tx) error) platformdb.Bootstrapper {
	return Seed{name: name, run: run}
}

func All() []platformdb.Bootstrapper {
	all := make([]platformdb.Bootstrapper, 0)

	for _, seed := range Cutover() {
		all = append(all, seed)
	}
	for _, seed := range []platformdb.Bootstrapper{seedDailySales(), seedCustomerTraffic(), seedUnitRentBudgets(), seedStoreRentBudgets(), seedUnitProspects()} {
		all = append(all, seed)
	}

	return all
}

func Cutover() []platformdb.Bootstrapper {
	all := make([]platformdb.Bootstrapper, 0)

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
