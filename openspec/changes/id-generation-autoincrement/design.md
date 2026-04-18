## Design

### Approach: AUTO_INCREMENT

MySQL `AUTO_INCREMENT` is the standard, safe mechanism for generating unique sequential IDs. It guarantees uniqueness even under high concurrency without requiring application-level locking.

### Migration Strategy

Create a new migration (`000019_auto_increment_ids.up.sql`) that:
1. For each affected table, executes `ALTER TABLE <table> MODIFY COLUMN id BIGINT NOT NULL AUTO_INCREMENT`
2. Sets `AUTO_INCREMENT` to the current MAX(id)+1 to preserve existing ID sequences

The down-migration reverses by removing AUTO_INCREMENT.

**Affected tables** (13 total from migrations 000001 and 000002):
- `store_types`, `store_management_types`, `stores`, `area_levels`, `areas`
- `unit_types`, `buildings`, `floors`, `locations`, `shop_types`
- `trade_definitions`, `currency_types`, `units`

### Go Code Changes

1. **baseinfo/baseinfo.go**:
   - Delete `nextID()` method
   - Modify `Create()` to INSERT without `id` column
   - Use `result.LastInsertId()` to get the new ID
   - Update `buildInsertQuery()` to not include `id`

2. **structure/structure.go**:
   - Delete `nextID()` method
   - Modify all 6 Create methods (`CreateStore`, `CreateBuilding`, `CreateFloor`, `CreateArea`, `CreateLocation`, `CreateUnit`) to INSERT without `id`
   - Use `result.LastInsertId()` to get the new ID

3. **excelio/repository.go**:
   - Delete `nextUnitID()` function
   - Modify `UpsertUnits()` to INSERT without `id` for new units, using `result.LastInsertId()` per row

### Safety Considerations

- `AUTO_INCREMENT` values are never reused for InnoDB tables (even after DELETE), preventing ID collision
- `LAST_INSERT_ID()` is connection-scoped, so concurrent inserts don't interfere
- The migration can be applied to tables with existing data without data loss

### Rollback Plan

The down-migration removes AUTO_INCREMENT. If issues arise, the old `nextID` code can be temporarily restored.
