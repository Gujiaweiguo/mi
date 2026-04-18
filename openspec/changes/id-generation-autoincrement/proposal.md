## Problem

Three backend modules generate primary-key IDs using `SELECT COALESCE(MAX(id), 0) + 1 FROM table`. Under concurrent writes, two requests can receive the same ID, causing duplicate-key errors or silent data corruption.

**Affected locations:**
- `backend/internal/baseinfo/baseinfo.go` — `nextID()` for all baseinfo catalog tables
- `backend/internal/structure/structure.go` — `nextID()` for stores, buildings, floors, areas, locations, units
- `backend/internal/excelio/repository.go` — `nextUnitID()` for bulk unit import

The underlying tables (`stores`, `buildings`, `floors`, `areas`, `locations`, `units`, `unit_types`, `shop_types`, `store_types`, `store_management_types`, `area_levels`, `trade_definitions`, `currency_types`) all define `id BIGINT PRIMARY KEY` without `AUTO_INCREMENT`.

## Proposal

Switch all affected tables to `AUTO_INCREMENT` on the `id` column. Modify Go INSERT statements to omit the `id` column and retrieve the generated ID via `LAST_INSERT_ID()` (MySQL driver support). This eliminates the race condition entirely and removes all `nextID`/`nextUnitID` helper functions.

## Scope

- New migration `000019_auto_increment_ids.up.sql` / `.down.sql` — ALTER tables to add AUTO_INCREMENT
- Go code changes in `baseinfo`, `structure`, `excelio` — remove nextID, adjust INSERT queries
- No frontend changes required
