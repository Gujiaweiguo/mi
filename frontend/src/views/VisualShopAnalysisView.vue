<script setup lang="ts">
import { computed, ref, type CSSProperties } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  exportReport,
  queryReport,
  type ReportQueryPayload,
  type VisualShopLegendItem,
  type VisualShopReportResponse,
  type VisualShopUnit,
} from '../api/reports'
import FilterForm from '../components/platform/FilterForm.vue'
import PageSection from '../components/platform/PageSection.vue'
import { useFilterForm } from '../composables/useFilterForm'
import { useAppStore } from '../stores/app'

type VisualShopFilters = {
  store_id: string
  floor_id: string
  area_id: string
}

type UnitColorStyle = CSSProperties & {
  '--visual-unit-color': string
}

type MarkerPresentation = {
  unit: VisualShopUnit
  style: UnitColorStyle
}

const appStore = useAppStore()
const { t } = useI18n()

const result = ref<VisualShopReportResponse | null>(null)
const errorMessage = ref('')
const isQuerying = ref(false)
const isExporting = ref(false)
const hasQueried = ref(false)
const selectedUnitId = ref<number | null>(null)

const { filters, reset } = useFilterForm<VisualShopFilters>({
  store_id: '',
  floor_id: '',
  area_id: '',
})

const parsePositiveInteger = (value: string) => {
  const normalized = value.trim()

  if (!normalized) {
    return undefined
  }

  const parsedValue = Number(normalized)
  if (Number.isInteger(parsedValue) && parsedValue > 0) {
    return parsedValue
  }

  return undefined
}

const buildPayload = (): ReportQueryPayload => {
  const payload: ReportQueryPayload = {}
  const storeId = parsePositiveInteger(filters.store_id)
  const floorId = parsePositiveInteger(filters.floor_id)
  const areaId = parsePositiveInteger(filters.area_id)

  if (storeId !== undefined) {
    payload.store_id = storeId
  }

  if (floorId !== undefined) {
    payload.floor_id = floorId
  }

  if (areaId !== undefined) {
    payload.area_id = areaId
  }

  return payload
}

const isHexColor = (value: string | null | undefined) => Boolean(value && /^#(?:[0-9a-fA-F]{3}){1,2}$/.test(value))

const resolveUnitColor = (value: string | null | undefined): string =>
  isHexColor(value) && typeof value === 'string' ? value : 'var(--mi-color-primary)'

const buildUnitColorStyle = (value: string | null | undefined): UnitColorStyle => ({
  '--visual-unit-color': resolveUnitColor(value),
})

const resolveCoordinate = (value: number, minimum: number, maximum: number) => {
  if (maximum === minimum) {
    return '50%'
  }

  const normalized = ((value - minimum) / (maximum - minimum)) * 100
  const clamped = Math.min(Math.max(normalized, 6), 94)

  return `${clamped.toFixed(2)}%`
}

const visualFloor = computed(() => result.value?.visual.floor ?? null)
const visualUnits = computed(() => result.value?.visual.units ?? [])
const visualLegend = computed(() => result.value?.visual.legend ?? [])

const markerUnits = computed<MarkerPresentation[]>(() => {
  if (!visualUnits.value.length) {
    return []
  }

  const xPositions = visualUnits.value.map((unit) => unit.pos_x)
  const yPositions = visualUnits.value.map((unit) => unit.pos_y)
  const minX = Math.min(...xPositions)
  const maxX = Math.max(...xPositions)
  const minY = Math.min(...yPositions)
  const maxY = Math.max(...yPositions)

  return visualUnits.value.map((unit) => ({
    unit,
    style: {
      left: resolveCoordinate(unit.pos_x, minX, maxX),
      top: resolveCoordinate(unit.pos_y, minY, maxY),
      ...buildUnitColorStyle(unit.color_hex),
    },
  }))
})

const selectedUnit = computed(() => {
  if (!visualUnits.value.length) {
    return null
  }

  return visualUnits.value.find((unit) => unit.unit_id === selectedUnitId.value) ?? visualUnits.value[0]
})

const canvasStyle = computed(() => {
  const floorPlanImageUrl = visualFloor.value?.floor_plan_image_url?.trim()

  if (!floorPlanImageUrl) {
    return {}
  }

  return {
    backgroundImage: `url("${floorPlanImageUrl}")`,
  }
})

const generatedAtLabel = computed(() => {
  if (!result.value?.generated_at) {
    return ''
  }

  const value = new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(result.value.generated_at))

  return t('visualShopAnalysis.meta.generatedAt', { value })
})

const inferFileExtension = (contentType: string | undefined) => {
  if (!contentType) {
    return 'xlsx'
  }

  const normalized = contentType.toLowerCase()

  if (normalized.includes('csv')) {
    return 'csv'
  }

  if (normalized.includes('pdf')) {
    return 'pdf'
  }

  if (normalized.includes('json')) {
    return 'json'
  }

  if (normalized.includes('excel') || normalized.includes('spreadsheetml')) {
    return 'xlsx'
  }

  return 'xlsx'
}

const buildFileName = () => {
  const storeLabel = filters.store_id.trim() || 'all-stores'
  const floorLabel = filters.floor_id.trim() || 'all-floors'
  const areaLabel = filters.area_id.trim() || 'all-areas'
  const stamp = new Date().toISOString().slice(0, 10)

  return `visual-shop-r19-${storeLabel}-${floorLabel}-${areaLabel}-${stamp}`
}

const loadVisualShopReport = async () => {
  hasQueried.value = true
  isQuerying.value = true
  errorMessage.value = ''

  try {
    const response = await queryReport<VisualShopReportResponse>('r19', buildPayload())
    result.value = response.data
    selectedUnitId.value = response.data.visual.units[0]?.unit_id ?? null
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('visualShopAnalysis.errors.unableToLoad')
    result.value = null
    selectedUnitId.value = null
  } finally {
    isQuerying.value = false
  }
}

const handleExport = async () => {
  isExporting.value = true
  errorMessage.value = ''

  try {
    const response = await exportReport('r19', buildPayload())
    const blob = response.data instanceof Blob ? response.data : new Blob([response.data], { type: 'application/octet-stream' })
    const extension = inferFileExtension(response.headers['content-type'])
    const url = URL.createObjectURL(blob)
    const link = document.createElement('a')

    link.href = url
    link.download = `${buildFileName()}.${extension}`
    link.click()
    URL.revokeObjectURL(url)
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('visualShopAnalysis.errors.unableToExport')
  } finally {
    isExporting.value = false
  }
}

const handleReset = () => {
  reset()
  hasQueried.value = false
  errorMessage.value = ''
  result.value = null
  selectedUnitId.value = null
}

const selectUnit = (unitId: number) => {
  selectedUnitId.value = unitId
}

const formatArea = (value: number | null) => {
  if (typeof value !== 'number') {
    return t('common.emptyValue')
  }

  const formatted = new Intl.NumberFormat(appStore.locale, {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  }).format(value)

  return `${formatted} ㎡`
}

const getDetailValue = (value: string | null | undefined) => {
  const normalized = value?.trim()
  return normalized ? normalized : t('common.emptyValue')
}

const legendItems = computed<VisualShopLegendItem[]>(() => {
  if (visualLegend.value.length) {
    return visualLegend.value
  }

  const seen = new Set<string>()

  return visualUnits.value.flatMap((unit) => {
    const label = unit.rent_status.trim() || t('visualShopAnalysis.defaults.unknownLegendItem')
    if (seen.has(label)) {
      return []
    }

    seen.add(label)
    return [{ label, color_hex: unit.color_hex }]
  })
})
</script>

<template>
  <div class="visual-shop-analysis-view" data-testid="visual-shop-analysis-view">
    <PageSection
      :eyebrow="t('visualShopAnalysis.eyebrow')"
      :title="t('visualShopAnalysis.title')"
      :summary="t('visualShopAnalysis.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('visualShopAnalysis.tags.batch') }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('visualShopAnalysis.tags.coverage') }}</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      class="visual-shop-analysis-view__alert"
      :title="t('visualShopAnalysis.errors.requestFailed')"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <FilterForm :title="t('visualShopAnalysis.filters.title')" :show-actions="false">
      <el-form-item :label="t('visualShopAnalysis.fields.storeId')">
        <el-input
          v-model="filters.store_id"
          :placeholder="t('visualShopAnalysis.placeholders.enterStoreId')"
          clearable
          data-testid="visual-shop-store-input"
        />
      </el-form-item>

      <el-form-item :label="t('visualShopAnalysis.fields.floorId')">
        <el-input
          v-model="filters.floor_id"
          :placeholder="t('visualShopAnalysis.placeholders.enterFloorId')"
          clearable
          data-testid="visual-shop-floor-input"
        />
      </el-form-item>

      <el-form-item :label="t('visualShopAnalysis.fields.areaId')">
        <el-input
          v-model="filters.area_id"
          :placeholder="t('visualShopAnalysis.placeholders.enterAreaId')"
          clearable
          data-testid="visual-shop-area-input"
        />
      </el-form-item>

      <div class="visual-shop-analysis-view__filter-actions">
        <el-button @click="handleReset">{{ t('filterForm.reset') }}</el-button>
        <el-button type="primary" :loading="isQuerying" data-testid="visual-shop-query-button" @click="loadVisualShopReport">
          {{ t('common.actions.query') }}
        </el-button>
        <el-button
          type="success"
          plain
          :loading="isExporting"
          data-testid="visual-shop-export-button"
          @click="handleExport"
        >
          {{ t('common.actions.export') }}
        </el-button>
      </div>
    </FilterForm>

    <div class="visual-shop-analysis-view__grid">
      <el-card class="visual-shop-analysis-view__canvas-card" shadow="never">
        <template #header>
          <div class="visual-shop-analysis-view__card-header">
            <div class="visual-shop-analysis-view__card-copy">
              <span>{{ t('visualShopAnalysis.cards.floorGraphic') }}</span>
              <p>
                {{ visualFloor?.name ?? t('visualShopAnalysis.defaults.floorGraphicUnavailable') }}
              </p>
            </div>

            <el-tag v-if="generatedAtLabel" effect="plain" type="info">{{ generatedAtLabel }}</el-tag>
          </div>
        </template>

        <div class="visual-shop-analysis-view__canvas-shell">
          <div class="visual-shop-analysis-view__canvas-frame">
            <div class="visual-shop-analysis-view__canvas-meta">
              <el-tag effect="plain" type="success">
                {{ t('visualShopAnalysis.meta.unitCount', { count: visualUnits.length }) }}
              </el-tag>
              <el-tag effect="plain" type="info">{{ visualFloor?.name ?? t('visualShopAnalysis.defaults.noFloorSelected') }}</el-tag>
            </div>

            <div class="visual-shop-analysis-view__canvas" :style="canvasStyle" data-testid="visual-shop-canvas">
              <button
                v-for="marker in markerUnits"
                :key="marker.unit.unit_id"
                type="button"
                class="visual-shop-analysis-view__unit-marker"
                :class="{ 'visual-shop-analysis-view__unit-marker--active': marker.unit.unit_id === selectedUnit?.unit_id }"
                :style="marker.style"
                data-testid="visual-unit-marker"
                @click="selectUnit(marker.unit.unit_id)"
              >
                <span class="visual-shop-analysis-view__unit-code">{{ marker.unit.unit_code }}</span>
                <span class="visual-shop-analysis-view__unit-status">{{ marker.unit.rent_status }}</span>
              </button>

              <div v-if="hasQueried && !markerUnits.length" class="visual-shop-analysis-view__empty-state">
                {{ t('visualShopAnalysis.emptyStates.canvas') }}
              </div>
            </div>
          </div>

          <aside class="visual-shop-analysis-view__detail-panel">
            <div class="visual-shop-analysis-view__detail-header">
              <span>{{ t('visualShopAnalysis.fields.selectedUnit') }}</span>
              <strong data-testid="visual-shop-selected-unit-code">{{ selectedUnit?.unit_code ?? t('common.emptyValue') }}</strong>
            </div>

            <dl v-if="selectedUnit" class="visual-shop-analysis-view__detail-list">
              <div>
                <dt>{{ t('visualShopAnalysis.fields.unitName') }}</dt>
                <dd>{{ getDetailValue(selectedUnit.unit_name) }}</dd>
              </div>
              <div>
                <dt>{{ t('common.columns.status') }}</dt>
                <dd>{{ getDetailValue(selectedUnit.rent_status) }}</dd>
              </div>
              <div>
                <dt>{{ t('visualShopAnalysis.fields.brand') }}</dt>
                <dd>{{ getDetailValue(selectedUnit.brand_name) }}</dd>
              </div>
              <div>
                <dt>{{ t('visualShopAnalysis.fields.customer') }}</dt>
                <dd>{{ getDetailValue(selectedUnit.customer_name) }}</dd>
              </div>
              <div>
                <dt>{{ t('visualShopAnalysis.fields.shopType') }}</dt>
                <dd>{{ getDetailValue(selectedUnit.shop_type_name) }}</dd>
              </div>
              <div>
                <dt>{{ t('visualShopAnalysis.fields.floorArea') }}</dt>
                <dd>{{ formatArea(selectedUnit.floor_area) }}</dd>
              </div>
              <div>
                <dt>{{ t('visualShopAnalysis.fields.rentArea') }}</dt>
                <dd>{{ formatArea(selectedUnit.rent_area) }}</dd>
              </div>
            </dl>

            <p v-else class="visual-shop-analysis-view__detail-empty">
              {{ t('visualShopAnalysis.emptyStates.detail') }}
            </p>
          </aside>
        </div>
      </el-card>

      <el-card class="visual-shop-analysis-view__legend-card" shadow="never">
        <template #header>
          <div class="visual-shop-analysis-view__card-header">
            <div class="visual-shop-analysis-view__card-copy">
              <span>{{ t('visualShopAnalysis.cards.legend') }}</span>
              <p>{{ t('visualShopAnalysis.cards.legendSummary') }}</p>
            </div>
          </div>
        </template>

        <div class="visual-shop-analysis-view__legend" data-testid="visual-shop-legend">
          <div v-for="item in legendItems" :key="item.label" class="visual-shop-analysis-view__legend-item">
            <span class="visual-shop-analysis-view__legend-swatch" :style="buildUnitColorStyle(item.color_hex)" />
            <span>{{ item.label }}</span>
          </div>

          <p v-if="!legendItems.length" class="visual-shop-analysis-view__legend-empty">
            {{ t('visualShopAnalysis.emptyStates.legend') }}
          </p>
        </div>
      </el-card>
    </div>
  </div>
</template>

<style scoped>
.visual-shop-analysis-view {
  --visual-shop-canvas-min-height: calc(var(--mi-space-6) * 15);
  --visual-shop-marker-width: calc(var(--mi-space-6) * 3.75);
  --visual-shop-marker-height: calc(var(--mi-space-6) * 2);
  --visual-shop-marker-top-band: var(--mi-space-2);

  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.visual-shop-analysis-view__alert {
  margin-bottom: 0;
}

.visual-shop-analysis-view__filter-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
  grid-column: 1 / -1;
}

.visual-shop-analysis-view__grid {
  display: grid;
  grid-template-columns: minmax(0, 2.2fr) minmax(0, 1fr);
  gap: var(--mi-space-5);
}

.visual-shop-analysis-view__canvas-card,
.visual-shop-analysis-view__legend-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.visual-shop-analysis-view__card-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: var(--mi-space-4);
}

.visual-shop-analysis-view__card-copy {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
  color: var(--mi-color-text);
}

.visual-shop-analysis-view__card-copy span {
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
}

.visual-shop-analysis-view__card-copy p {
  margin: 0;
  font-size: var(--mi-font-size-100);
  color: var(--mi-color-muted);
}

.visual-shop-analysis-view__canvas-shell {
  display: grid;
  grid-template-columns: minmax(0, 1.8fr) minmax(0, 1fr);
  gap: var(--mi-space-4);
}

.visual-shop-analysis-view__canvas-frame,
.visual-shop-analysis-view__detail-panel,
.visual-shop-analysis-view__legend {
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  border-radius: var(--mi-radius-md);
  background: var(--mi-surface-gradient);
}

.visual-shop-analysis-view__canvas-frame {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
  padding: var(--mi-space-4);
}

.visual-shop-analysis-view__canvas-meta {
  display: flex;
  flex-wrap: wrap;
  gap: var(--mi-space-2);
}

.visual-shop-analysis-view__canvas {
  position: relative;
  min-height: var(--visual-shop-canvas-min-height);
  overflow: hidden;
  border-radius: var(--mi-radius-md);
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  background:
    linear-gradient(135deg, var(--mi-color-surface-alt) 0%, var(--mi-color-surface) 100%);
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  box-shadow: inset 0 0 0 var(--mi-border-width-thin) var(--mi-color-surface-alt);
}

.visual-shop-analysis-view__unit-marker {
  position: absolute;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  gap: var(--mi-space-1);
  width: var(--visual-shop-marker-width);
  min-height: var(--visual-shop-marker-height);
  padding: calc(var(--mi-space-3) + var(--visual-shop-marker-top-band)) var(--mi-space-2) var(--mi-space-2);
  border: var(--mi-border-width-thin) solid var(--mi-color-text);
  border-radius: var(--mi-radius-sm);
  background:
    linear-gradient(
      180deg,
      var(--visual-unit-color) 0%,
      var(--visual-unit-color) var(--visual-shop-marker-top-band),
      var(--mi-color-surface) var(--visual-shop-marker-top-band),
      var(--mi-color-surface) 100%
    );
  color: var(--mi-color-text);
  box-shadow: var(--mi-shadow-sm);
  cursor: pointer;
  transform: translate(-50%, -50%);
  transition:
    transform var(--mi-transition-base),
    box-shadow var(--mi-transition-base),
    border-color var(--mi-transition-base);
}

.visual-shop-analysis-view__unit-marker:hover,
.visual-shop-analysis-view__unit-marker:focus-visible,
.visual-shop-analysis-view__unit-marker--active {
  border-color: var(--mi-color-primary);
  box-shadow: var(--mi-shadow-md);
  transform: translate(-50%, -50%) translateY(calc(var(--mi-space-1) * -1));
}

.visual-shop-analysis-view__unit-code {
  font-size: var(--mi-font-size-100);
  font-weight: var(--mi-font-weight-semibold);
}

.visual-shop-analysis-view__unit-status {
  font-size: var(--mi-font-size-100);
  color: var(--mi-color-muted);
  text-transform: capitalize;
}

.visual-shop-analysis-view__empty-state,
.visual-shop-analysis-view__detail-empty,
.visual-shop-analysis-view__legend-empty {
  margin: 0;
  font-size: var(--mi-font-size-200);
  line-height: var(--mi-line-height-base);
  color: var(--mi-color-muted);
}

.visual-shop-analysis-view__empty-state {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: var(--mi-space-6);
  text-align: center;
}

.visual-shop-analysis-view__detail-panel {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  padding: var(--mi-space-4);
}

.visual-shop-analysis-view__detail-header {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-1);
}

.visual-shop-analysis-view__detail-header span {
  font-size: var(--mi-font-size-100);
  letter-spacing: var(--mi-letter-spacing-wide);
  text-transform: uppercase;
  color: var(--mi-color-muted);
}

.visual-shop-analysis-view__detail-header strong {
  font-size: var(--mi-font-size-400);
  line-height: var(--mi-line-height-tight);
  color: var(--mi-color-text);
}

.visual-shop-analysis-view__detail-list {
  display: grid;
  gap: var(--mi-space-3);
  margin: 0;
}

.visual-shop-analysis-view__detail-list div {
  display: grid;
  gap: var(--mi-space-1);
  padding-bottom: var(--mi-space-3);
  border-bottom: var(--mi-border-width-thin) solid var(--mi-color-border);
}

.visual-shop-analysis-view__detail-list div:last-child {
  padding-bottom: 0;
  border-bottom: none;
}

.visual-shop-analysis-view__detail-list dt {
  font-size: var(--mi-font-size-100);
  letter-spacing: var(--mi-letter-spacing-wide);
  text-transform: uppercase;
  color: var(--mi-color-muted);
}

.visual-shop-analysis-view__detail-list dd {
  margin: 0;
  font-size: var(--mi-font-size-200);
  color: var(--mi-color-text);
}

.visual-shop-analysis-view__legend {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
  padding: var(--mi-space-4);
}

.visual-shop-analysis-view__legend-item {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-200);
  color: var(--mi-color-text);
}

.visual-shop-analysis-view__legend-swatch {
  display: inline-flex;
  width: calc(var(--mi-space-6) * 0.75);
  height: calc(var(--mi-space-6) * 0.75);
  border-radius: 50%;
  border: var(--mi-border-width-thin) solid var(--mi-color-text);
  background: var(--visual-unit-color);
  box-shadow: var(--mi-shadow-sm);
}

@media (max-width: 72rem) {
  .visual-shop-analysis-view__grid,
  .visual-shop-analysis-view__canvas-shell {
    grid-template-columns: minmax(0, 1fr);
  }
}

@media (max-width: 52rem) {
  .visual-shop-analysis-view__filter-actions,
  .visual-shop-analysis-view__card-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .visual-shop-analysis-view__unit-marker {
    width: calc(var(--mi-space-6) * 3.25);
  }
}
</style>
