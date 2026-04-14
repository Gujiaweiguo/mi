<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  createStoreRentBudget,
  createUnitProspect,
  createUnitRentBudget,
  listStoreRentBudgets,
  listUnitProspects,
  listUnitRentBudgets,
  updateStoreRentBudget,
  updateUnitProspect,
  updateUnitRentBudget,
  type Brand,
  type Customer,
  type StoreRentBudget,
  type UnitProspect,
  type UnitRentBudget,
} from '../../api/masterdata'
import type { BudgetProspectSnapshot, Feedback } from './types'

type UnitBudgetForm = {
  unit_id: number | undefined
  fiscal_year: number | undefined
  budget_price: number | undefined
}

type StoreBudgetForm = {
  store_id: number | undefined
  fiscal_year: number | undefined
  fiscal_month: number | undefined
  monthly_budget: number | undefined
}

type UnitProspectForm = {
  unit_id: number | undefined
  fiscal_year: number | undefined
  potential_customer_id: number | undefined
  prospect_brand_id: number | undefined
  prospect_trade_id: number | undefined
  avg_transaction: number | undefined
  prospect_rent_price: number | undefined
  rent_increment: string
  prospect_term_months: number | undefined
}

const props = defineProps<{
  customers: Customer[]
  brands: Brand[]
}>()

const emit = defineEmits<{
  'summary-change': [snapshot: BudgetProspectSnapshot]
  'load-feedback-change': [feedback: Feedback | null]
}>()

const { t } = useI18n()

const unitRentBudgets = ref<UnitRentBudget[]>([])
const storeRentBudgets = ref<StoreRentBudget[]>([])
const unitProspects = ref<UnitProspect[]>([])

const isUnitBudgetSaving = ref(false)
const isStoreBudgetSaving = ref(false)
const isUnitProspectSaving = ref(false)

const loadFeedback = ref<Feedback | null>(null)
const budgetFeedback = ref<Feedback | null>(null)
const prospectFeedback = ref<Feedback | null>(null)

const unitBudgetForm = reactive<UnitBudgetForm>({
  unit_id: undefined,
  fiscal_year: undefined,
  budget_price: undefined,
})

const storeBudgetForm = reactive<StoreBudgetForm>({
  store_id: undefined,
  fiscal_year: undefined,
  fiscal_month: undefined,
  monthly_budget: undefined,
})

const unitProspectForm = reactive<UnitProspectForm>({
  unit_id: undefined,
  fiscal_year: undefined,
  potential_customer_id: undefined,
  prospect_brand_id: undefined,
  prospect_trade_id: undefined,
  avg_transaction: undefined,
  prospect_rent_price: undefined,
  rent_increment: '',
  prospect_term_months: undefined,
})

const canSaveUnitBudget = computed(
  () => unitBudgetForm.unit_id !== undefined && unitBudgetForm.fiscal_year !== undefined && unitBudgetForm.budget_price !== undefined,
)
const canSaveStoreBudget = computed(
  () =>
    storeBudgetForm.store_id !== undefined &&
    storeBudgetForm.fiscal_year !== undefined &&
    storeBudgetForm.fiscal_month !== undefined &&
    storeBudgetForm.monthly_budget !== undefined,
)
const canSaveUnitProspect = computed(
  () => unitProspectForm.unit_id !== undefined && unitProspectForm.fiscal_year !== undefined,
)

const hasExistingUnitBudget = (unitId?: number, fiscalYear?: number) =>
  unitRentBudgets.value.some((item) => item.unit_id === unitId && item.fiscal_year === fiscalYear)

const hasExistingStoreBudget = (storeId?: number, fiscalYear?: number, fiscalMonth?: number) =>
  storeRentBudgets.value.some(
    (item) => item.store_id === storeId && item.fiscal_year === fiscalYear && item.fiscal_month === fiscalMonth,
  )

const hasExistingUnitProspect = (unitId?: number, fiscalYear?: number) =>
  unitProspects.value.some((item) => item.unit_id === unitId && item.fiscal_year === fiscalYear)

const unitBudgetSubmitLabel = computed(() =>
  hasExistingUnitBudget(unitBudgetForm.unit_id, unitBudgetForm.fiscal_year)
    ? t('masterdataAdmin.actions.updateUnitBudget')
    : t('masterdataAdmin.actions.createUnitBudget'),
)
const storeBudgetSubmitLabel = computed(() =>
  hasExistingStoreBudget(storeBudgetForm.store_id, storeBudgetForm.fiscal_year, storeBudgetForm.fiscal_month)
    ? t('masterdataAdmin.actions.updateStoreBudget')
    : t('masterdataAdmin.actions.createStoreBudget'),
)
const unitProspectSubmitLabel = computed(() =>
  hasExistingUnitProspect(unitProspectForm.unit_id, unitProspectForm.fiscal_year)
    ? t('masterdataAdmin.actions.updateUnitProspect')
    : t('masterdataAdmin.actions.createUnitProspect'),
)

const emitSnapshot = () => {
  emit('summary-change', {
    unitRentBudgets: unitRentBudgets.value,
    storeRentBudgets: storeRentBudgets.value,
    unitProspects: unitProspects.value,
  })
}

watch([unitRentBudgets, storeRentBudgets, unitProspects], emitSnapshot, { immediate: true })
watch(loadFeedback, (value) => emit('load-feedback-change', value), { immediate: true })

const getErrorMessage = (error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)

const resetUnitBudgetForm = () => {
  unitBudgetForm.unit_id = undefined
  unitBudgetForm.fiscal_year = undefined
  unitBudgetForm.budget_price = undefined
}

const resetStoreBudgetForm = () => {
  storeBudgetForm.store_id = undefined
  storeBudgetForm.fiscal_year = undefined
  storeBudgetForm.fiscal_month = undefined
  storeBudgetForm.monthly_budget = undefined
}

const resetUnitProspectForm = () => {
  unitProspectForm.unit_id = undefined
  unitProspectForm.fiscal_year = undefined
  unitProspectForm.potential_customer_id = undefined
  unitProspectForm.prospect_brand_id = undefined
  unitProspectForm.prospect_trade_id = undefined
  unitProspectForm.avg_transaction = undefined
  unitProspectForm.prospect_rent_price = undefined
  unitProspectForm.rent_increment = ''
  unitProspectForm.prospect_term_months = undefined
}

const loadSection = async () => {
  loadFeedback.value = null

  const [unitBudgetsResult, storeBudgetsResult, unitProspectsResult] = await Promise.allSettled([
    listUnitRentBudgets(),
    listStoreRentBudgets(),
    listUnitProspects(),
  ])

  const loadErrors: string[] = []

  if (unitBudgetsResult.status === 'fulfilled') {
    unitRentBudgets.value = unitBudgetsResult.value.data.unit_rent_budgets ?? []
  } else {
    unitRentBudgets.value = []
    loadErrors.push(getErrorMessage(unitBudgetsResult.reason, t('masterdataAdmin.errors.unableToLoadUnitBudgets')))
  }

  if (storeBudgetsResult.status === 'fulfilled') {
    storeRentBudgets.value = storeBudgetsResult.value.data.store_rent_budgets ?? []
  } else {
    storeRentBudgets.value = []
    loadErrors.push(getErrorMessage(storeBudgetsResult.reason, t('masterdataAdmin.errors.unableToLoadStoreBudgets')))
  }

  if (unitProspectsResult.status === 'fulfilled') {
    unitProspects.value = unitProspectsResult.value.data.unit_prospects ?? []
  } else {
    unitProspects.value = []
    loadErrors.push(getErrorMessage(unitProspectsResult.reason, t('masterdataAdmin.errors.unableToLoadUnitProspects')))
  }

  if (loadErrors.length > 0) {
    loadFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.masterDataUnavailable'),
      description: loadErrors.join(' '),
    }
  }
}

const handleSaveUnitBudget = async () => {
  if (!canSaveUnitBudget.value) {
    budgetFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.unitBudgetRequiredTitle'),
      description: t('masterdataAdmin.feedback.unitBudgetRequiredDescription'),
    }
    return
  }

  isUnitBudgetSaving.value = true
  budgetFeedback.value = null

  try {
    const payload = {
      unit_id: unitBudgetForm.unit_id!,
      fiscal_year: unitBudgetForm.fiscal_year!,
      budget_price: unitBudgetForm.budget_price!,
    }

    if (hasExistingUnitBudget(payload.unit_id, payload.fiscal_year)) {
      await updateUnitRentBudget(payload)
    } else {
      await createUnitRentBudget(payload)
    }

    budgetFeedback.value = {
      type: 'success',
      title: t('masterdataAdmin.feedback.unitBudgetSavedTitle'),
      description: t('masterdataAdmin.feedback.unitBudgetSavedDescription', {
        unitId: payload.unit_id,
        fiscalYear: payload.fiscal_year,
      }),
    }
    await loadSection()
    resetUnitBudgetForm()
  } catch (error) {
    budgetFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.unitBudgetSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveUnitBudget')),
    }
  } finally {
    isUnitBudgetSaving.value = false
  }
}

const handleSaveStoreBudget = async () => {
  if (!canSaveStoreBudget.value) {
    budgetFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.storeBudgetRequiredTitle'),
      description: t('masterdataAdmin.feedback.storeBudgetRequiredDescription'),
    }
    return
  }

  isStoreBudgetSaving.value = true
  budgetFeedback.value = null

  try {
    const payload = {
      store_id: storeBudgetForm.store_id!,
      fiscal_year: storeBudgetForm.fiscal_year!,
      fiscal_month: storeBudgetForm.fiscal_month!,
      monthly_budget: storeBudgetForm.monthly_budget!,
    }

    if (hasExistingStoreBudget(payload.store_id, payload.fiscal_year, payload.fiscal_month)) {
      await updateStoreRentBudget(payload)
    } else {
      await createStoreRentBudget(payload)
    }

    budgetFeedback.value = {
      type: 'success',
      title: t('masterdataAdmin.feedback.storeBudgetSavedTitle'),
      description: t('masterdataAdmin.feedback.storeBudgetSavedDescription', {
        storeId: payload.store_id,
        fiscalYear: payload.fiscal_year,
        fiscalMonth: payload.fiscal_month,
      }),
    }
    await loadSection()
    resetStoreBudgetForm()
  } catch (error) {
    budgetFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.storeBudgetSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveStoreBudget')),
    }
  } finally {
    isStoreBudgetSaving.value = false
  }
}

const handleSaveUnitProspect = async () => {
  if (!canSaveUnitProspect.value) {
    prospectFeedback.value = {
      type: 'warning',
      title: t('masterdataAdmin.feedback.unitProspectRequiredTitle'),
      description: t('masterdataAdmin.feedback.unitProspectRequiredDescription'),
    }
    return
  }

  isUnitProspectSaving.value = true
  prospectFeedback.value = null

  try {
    const payload = {
      unit_id: unitProspectForm.unit_id!,
      fiscal_year: unitProspectForm.fiscal_year!,
      potential_customer_id: unitProspectForm.potential_customer_id ?? null,
      prospect_brand_id: unitProspectForm.prospect_brand_id ?? null,
      prospect_trade_id: unitProspectForm.prospect_trade_id ?? null,
      avg_transaction: unitProspectForm.avg_transaction ?? null,
      prospect_rent_price: unitProspectForm.prospect_rent_price ?? null,
      rent_increment: unitProspectForm.rent_increment.trim() || null,
      prospect_term_months: unitProspectForm.prospect_term_months ?? null,
    }

    if (hasExistingUnitProspect(payload.unit_id, payload.fiscal_year)) {
      await updateUnitProspect(payload)
    } else {
      await createUnitProspect(payload)
    }

    prospectFeedback.value = {
      type: 'success',
      title: t('masterdataAdmin.feedback.unitProspectSavedTitle'),
      description: t('masterdataAdmin.feedback.unitProspectSavedDescription', {
        unitId: payload.unit_id,
        fiscalYear: payload.fiscal_year,
      }),
    }
    await loadSection()
    resetUnitProspectForm()
  } catch (error) {
    prospectFeedback.value = {
      type: 'error',
      title: t('masterdataAdmin.errors.unitProspectSaveFailed'),
      description: getErrorMessage(error, t('masterdataAdmin.errors.unableToSaveUnitProspect')),
    }
  } finally {
    isUnitProspectSaving.value = false
  }
}

const editUnitBudget = (item: UnitRentBudget) => {
  unitBudgetForm.unit_id = item.unit_id
  unitBudgetForm.fiscal_year = item.fiscal_year
  unitBudgetForm.budget_price = item.budget_price
}

const editStoreBudget = (item: StoreRentBudget) => {
  storeBudgetForm.store_id = item.store_id
  storeBudgetForm.fiscal_year = item.fiscal_year
  storeBudgetForm.fiscal_month = item.fiscal_month
  storeBudgetForm.monthly_budget = item.monthly_budget
}

const editUnitProspect = (item: UnitProspect) => {
  unitProspectForm.unit_id = item.unit_id
  unitProspectForm.fiscal_year = item.fiscal_year
  unitProspectForm.potential_customer_id = item.potential_customer_id ?? undefined
  unitProspectForm.prospect_brand_id = item.prospect_brand_id ?? undefined
  unitProspectForm.prospect_trade_id = item.prospect_trade_id ?? undefined
  unitProspectForm.avg_transaction = item.avg_transaction ?? undefined
  unitProspectForm.prospect_rent_price = item.prospect_rent_price ?? undefined
  unitProspectForm.rent_increment = item.rent_increment ?? ''
  unitProspectForm.prospect_term_months = item.prospect_term_months ?? undefined
}

onMounted(() => {
  void loadSection()
})
</script>

<template>
  <div class="masterdata-budget-prospect-section">
    <div class="masterdata-budget-prospect-section__budget-grid">
      <el-card class="masterdata-budget-prospect-section__card" shadow="never">
        <template #header><div class="masterdata-budget-prospect-section__card-header"><span>{{ t('masterdataAdmin.cards.unitBudgets') }}</span></div></template>
        <el-alert v-if="budgetFeedback" :closable="false" class="masterdata-budget-prospect-section__feedback" :title="budgetFeedback.title" :type="budgetFeedback.type" :description="budgetFeedback.description" show-icon />
        <el-form label-position="top" class="masterdata-budget-prospect-section__form" @submit.prevent>
          <div class="masterdata-budget-prospect-section__form-grid">
            <el-form-item :label="t('masterdataAdmin.fields.unitId')"><el-input-number v-model="unitBudgetForm.unit_id" :min="1" controls-position="right" data-testid="unit-budget-unit-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.fiscalYear')"><el-input-number v-model="unitBudgetForm.fiscal_year" :min="2000" controls-position="right" data-testid="unit-budget-year-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.budgetPrice')"><el-input-number v-model="unitBudgetForm.budget_price" :min="0" :precision="2" controls-position="right" data-testid="unit-budget-price-input" /></el-form-item>
          </div>
          <div class="masterdata-budget-prospect-section__form-actions">
            <el-button type="primary" :loading="isUnitBudgetSaving" :disabled="!canSaveUnitBudget" data-testid="unit-budget-save-button" @click="handleSaveUnitBudget">{{ unitBudgetSubmitLabel }}</el-button>
            <el-button @click="resetUnitBudgetForm">{{ t('common.actions.reset') }}</el-button>
          </div>
        </el-form>
        <el-table :data="unitRentBudgets" :row-key="(row: UnitRentBudget) => `${row.unit_id}-${row.fiscal_year}`" class="masterdata-budget-prospect-section__table" data-testid="unit-budgets-table">
          <el-table-column prop="unit_id" :label="t('masterdataAdmin.columns.unitId')" min-width="110" />
          <el-table-column prop="fiscal_year" :label="t('masterdataAdmin.columns.fiscalYear')" min-width="120" />
          <el-table-column prop="budget_price" :label="t('masterdataAdmin.columns.budgetPrice')" min-width="140" />
          <el-table-column :label="t('common.columns.actions')" min-width="120"><template #default="scope"><el-button link type="primary" data-testid="unit-budget-edit-button" @click="editUnitBudget(scope.row)">{{ t('common.actions.edit') }}</el-button></template></el-table-column>
        </el-table>
      </el-card>

      <el-card class="masterdata-budget-prospect-section__card" shadow="never">
        <template #header><div class="masterdata-budget-prospect-section__card-header"><span>{{ t('masterdataAdmin.cards.storeBudgets') }}</span></div></template>
        <el-form label-position="top" class="masterdata-budget-prospect-section__form" @submit.prevent>
          <div class="masterdata-budget-prospect-section__form-grid">
            <el-form-item :label="t('masterdataAdmin.fields.storeId')"><el-input-number v-model="storeBudgetForm.store_id" :min="1" controls-position="right" data-testid="store-budget-store-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.fiscalYear')"><el-input-number v-model="storeBudgetForm.fiscal_year" :min="2000" controls-position="right" data-testid="store-budget-year-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.fiscalMonth')"><el-input-number v-model="storeBudgetForm.fiscal_month" :min="1" :max="12" controls-position="right" data-testid="store-budget-month-input" /></el-form-item>
            <el-form-item :label="t('masterdataAdmin.fields.monthlyBudget')"><el-input-number v-model="storeBudgetForm.monthly_budget" :min="0" :precision="2" controls-position="right" data-testid="store-budget-value-input" /></el-form-item>
          </div>
          <div class="masterdata-budget-prospect-section__form-actions">
            <el-button type="primary" :loading="isStoreBudgetSaving" :disabled="!canSaveStoreBudget" data-testid="store-budget-save-button" @click="handleSaveStoreBudget">{{ storeBudgetSubmitLabel }}</el-button>
            <el-button @click="resetStoreBudgetForm">{{ t('common.actions.reset') }}</el-button>
          </div>
        </el-form>
        <el-table :data="storeRentBudgets" :row-key="(row: StoreRentBudget) => `${row.store_id}-${row.fiscal_year}-${row.fiscal_month}`" class="masterdata-budget-prospect-section__table" data-testid="store-budgets-table">
          <el-table-column prop="store_id" :label="t('masterdataAdmin.columns.storeId')" min-width="110" />
          <el-table-column prop="fiscal_year" :label="t('masterdataAdmin.columns.fiscalYear')" min-width="120" />
          <el-table-column prop="fiscal_month" :label="t('masterdataAdmin.columns.fiscalMonth')" min-width="120" />
          <el-table-column prop="monthly_budget" :label="t('masterdataAdmin.columns.monthlyBudget')" min-width="150" />
          <el-table-column :label="t('common.columns.actions')" min-width="120"><template #default="scope"><el-button link type="primary" data-testid="store-budget-edit-button" @click="editStoreBudget(scope.row)">{{ t('common.actions.edit') }}</el-button></template></el-table-column>
        </el-table>
      </el-card>
    </div>

    <el-card class="masterdata-budget-prospect-section__card" shadow="never">
      <template #header><div class="masterdata-budget-prospect-section__card-header"><span>{{ t('masterdataAdmin.cards.unitProspects') }}</span></div></template>
      <el-alert v-if="prospectFeedback" :closable="false" class="masterdata-budget-prospect-section__feedback" :title="prospectFeedback.title" :type="prospectFeedback.type" :description="prospectFeedback.description" show-icon />
      <el-form label-position="top" class="masterdata-budget-prospect-section__form" @submit.prevent>
        <div class="masterdata-budget-prospect-section__form-grid masterdata-budget-prospect-section__form-grid--wide">
          <el-form-item :label="t('masterdataAdmin.fields.unitId')"><el-input-number v-model="unitProspectForm.unit_id" :min="1" controls-position="right" data-testid="unit-prospect-unit-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.fiscalYear')"><el-input-number v-model="unitProspectForm.fiscal_year" :min="2000" controls-position="right" data-testid="unit-prospect-year-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.potentialCustomer')"><el-select v-model="unitProspectForm.potential_customer_id" clearable filterable data-testid="unit-prospect-customer-input"><el-option v-for="customer in props.customers" :key="customer.id" :label="`${customer.code} — ${customer.name}`" :value="customer.id" /></el-select></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectBrand')"><el-select v-model="unitProspectForm.prospect_brand_id" clearable filterable data-testid="unit-prospect-brand-input"><el-option v-for="brand in props.brands" :key="brand.id" :label="`${brand.code} — ${brand.name}`" :value="brand.id" /></el-select></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectTradeId')"><el-input-number v-model="unitProspectForm.prospect_trade_id" :min="1" controls-position="right" data-testid="unit-prospect-trade-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.avgTransaction')"><el-input-number v-model="unitProspectForm.avg_transaction" :min="0" :precision="2" controls-position="right" data-testid="unit-prospect-avg-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectRentPrice')"><el-input-number v-model="unitProspectForm.prospect_rent_price" :min="0" :precision="2" controls-position="right" data-testid="unit-prospect-price-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.rentIncrement')"><el-input v-model="unitProspectForm.rent_increment" data-testid="unit-prospect-increment-input" /></el-form-item>
          <el-form-item :label="t('masterdataAdmin.fields.prospectTermMonths')"><el-input-number v-model="unitProspectForm.prospect_term_months" :min="1" controls-position="right" data-testid="unit-prospect-term-input" /></el-form-item>
        </div>
        <div class="masterdata-budget-prospect-section__form-actions">
          <el-button type="primary" :loading="isUnitProspectSaving" :disabled="!canSaveUnitProspect" data-testid="unit-prospect-save-button" @click="handleSaveUnitProspect">{{ unitProspectSubmitLabel }}</el-button>
          <el-button @click="resetUnitProspectForm">{{ t('common.actions.reset') }}</el-button>
        </div>
      </el-form>

      <el-table :data="unitProspects" :row-key="(row: UnitProspect) => `${row.unit_id}-${row.fiscal_year}`" class="masterdata-budget-prospect-section__table" data-testid="unit-prospects-table">
        <el-table-column prop="unit_id" :label="t('masterdataAdmin.columns.unitId')" min-width="110" />
        <el-table-column prop="fiscal_year" :label="t('masterdataAdmin.columns.fiscalYear')" min-width="120" />
        <el-table-column :label="t('masterdataAdmin.columns.potentialCustomer')" min-width="180"><template #default="scope">{{ scope.row.potential_customer_id ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('masterdataAdmin.columns.prospectBrand')" min-width="160"><template #default="scope">{{ scope.row.prospect_brand_id ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('masterdataAdmin.columns.prospectRentPrice')" min-width="160"><template #default="scope">{{ scope.row.prospect_rent_price ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('masterdataAdmin.columns.prospectTermMonths')" min-width="160"><template #default="scope">{{ scope.row.prospect_term_months ?? t('common.emptyValue') }}</template></el-table-column>
        <el-table-column :label="t('common.columns.actions')" min-width="120"><template #default="scope"><el-button link type="primary" data-testid="unit-prospect-edit-button" @click="editUnitProspect(scope.row)">{{ t('common.actions.edit') }}</el-button></template></el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style scoped>
.masterdata-budget-prospect-section {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.masterdata-budget-prospect-section__budget-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.masterdata-budget-prospect-section__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.masterdata-budget-prospect-section__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.masterdata-budget-prospect-section__feedback {
  margin-bottom: var(--mi-space-4);
}

.masterdata-budget-prospect-section__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.masterdata-budget-prospect-section__form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.masterdata-budget-prospect-section__form-grid--wide {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.masterdata-budget-prospect-section__form-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.masterdata-budget-prospect-section__table,
.masterdata-budget-prospect-section__form-grid :deep(.el-input-number),
.masterdata-budget-prospect-section__form-grid :deep(.el-select) {
  width: 100%;
}

@media (max-width: 52rem) {
  .masterdata-budget-prospect-section__budget-grid,
  .masterdata-budget-prospect-section__form-grid,
  .masterdata-budget-prospect-section__form-grid--wide {
    grid-template-columns: minmax(0, 1fr);
  }

  .masterdata-budget-prospect-section__card-header,
  .masterdata-budget-prospect-section__form-actions {
    align-items: flex-start;
    flex-direction: column;
  }
}
</style>
