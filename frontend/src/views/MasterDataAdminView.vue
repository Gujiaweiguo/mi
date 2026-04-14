<script setup lang="ts">
import { computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import MasterDataBudgetProspectSection from '../components/masterdata/MasterDataBudgetProspectSection.vue'
import MasterDataCustomerBrandSection from '../components/masterdata/MasterDataCustomerBrandSection.vue'
import type { BudgetProspectSnapshot, CustomerBrandSnapshot, Feedback } from '../components/masterdata/types'
import PageSection from '../components/platform/PageSection.vue'

const { t } = useI18n()

const customerBrandSnapshot = ref<CustomerBrandSnapshot>({
  customers: [],
  customerTotal: 0,
  brands: [],
  brandTotal: 0,
})

const budgetProspectSnapshot = ref<BudgetProspectSnapshot>({
  unitRentBudgets: [],
  storeRentBudgets: [],
  unitProspects: [],
})

const customerBrandLoadFeedback = ref<Feedback | null>(null)
const budgetProspectLoadFeedback = ref<Feedback | null>(null)

const handleCustomerBrandSummaryChange = (snapshot: CustomerBrandSnapshot) => {
  customerBrandSnapshot.value = snapshot
}

const handleBudgetProspectSummaryChange = (snapshot: BudgetProspectSnapshot) => {
  budgetProspectSnapshot.value = snapshot
}

const handleCustomerBrandLoadFeedbackChange = (feedback: Feedback | null) => {
  customerBrandLoadFeedback.value = feedback
}

const handleBudgetProspectLoadFeedbackChange = (feedback: Feedback | null) => {
  budgetProspectLoadFeedback.value = feedback
}

const pageFeedback = computed<Feedback | null>(() => {
  const descriptions = [customerBrandLoadFeedback.value?.description, budgetProspectLoadFeedback.value?.description].filter(
    (value): value is string => Boolean(value),
  )

  if (descriptions.length === 0) {
    return null
  }

  return {
    type: 'error',
    title: t('masterdataAdmin.errors.masterDataUnavailable'),
    description: descriptions.join(' '),
  }
})
</script>

<template>
  <div class="masterdata-admin-view" data-testid="masterdata-admin-view">
    <PageSection
      :eyebrow="t('masterdataAdmin.eyebrow')"
      :title="t('masterdataAdmin.title')"
      :summary="t('masterdataAdmin.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('masterdataAdmin.tags.customers', { count: customerBrandSnapshot.customerTotal }) }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('masterdataAdmin.tags.brands', { count: customerBrandSnapshot.brandTotal }) }}</el-tag>
        <el-tag effect="plain" type="warning">{{ t('masterdataAdmin.tags.unitBudgets', { count: budgetProspectSnapshot.unitRentBudgets.length }) }}</el-tag>
        <el-tag effect="plain" type="danger">{{ t('masterdataAdmin.tags.unitProspects', { count: budgetProspectSnapshot.unitProspects.length }) }}</el-tag>
      </template>
    </PageSection>

    <el-alert
      v-if="pageFeedback"
      :closable="false"
      :title="pageFeedback.title"
      :type="pageFeedback.type"
      :description="pageFeedback.description"
      show-icon
    />

    <MasterDataCustomerBrandSection
      @summary-change="handleCustomerBrandSummaryChange"
      @load-feedback-change="handleCustomerBrandLoadFeedbackChange"
    />

    <MasterDataBudgetProspectSection
      :customers="customerBrandSnapshot.customers"
      :brands="customerBrandSnapshot.brands"
      @summary-change="handleBudgetProspectSummaryChange"
      @load-feedback-change="handleBudgetProspectLoadFeedbackChange"
    />
  </div>
</template>

<style scoped>
.masterdata-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}
</style>
