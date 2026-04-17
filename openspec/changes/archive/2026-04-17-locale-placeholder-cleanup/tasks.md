# Tasks: locale-placeholder-cleanup

## T1 — Fix en-US.ts placeholder URL
- [ ] Change `floorPlanUrl: 'https://example.com/floor-1.png'` → `floorPlanUrl: ''` in `frontend/src/i18n/messages/en-US.ts`

## T2 — Fix zh-CN.ts placeholder URL
- [ ] Change `floorPlanUrl: 'https://example.com/floor-1.png'` → `floorPlanUrl: ''` in `frontend/src/i18n/messages/zh-CN.ts`

## T3 — Fix en-US.ts stub text
- [ ] Change `'No stub records match the current filters.'` → `'No records match the current filters.'` in `frontend/src/i18n/messages/en-US.ts`

## T4 — Fix commercial seed placeholder URL
- [ ] Change `'https://example.com/floor-101.png'` → `''` in `backend/internal/platform/database/bootstrap/commercial_seed.go`

## T5 — Fix e2e test placeholder URL
- [ ] Change `floor_plan_image_url: 'https://example.com/floor-plan-r19.png'` → `floor_plan_image_url: ''` in `frontend/e2e/task16-r19-visual.spec.ts`
