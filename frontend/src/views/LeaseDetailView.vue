<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute, useRouter } from 'vue-router'

import { getLease, submitLease, terminateLease, type LeaseContract } from '../api/lease'
import PageSection from '../components/platform/PageSection.vue'
import { useAppStore } from '../stores/app'

const route = useRoute()
const router = useRouter()
const { t } = useI18n()
const appStore = useAppStore()

const lease = ref<LeaseContract | null>(null)
const errorMessage = ref('')
const successMessage = ref('')
const isLoading = ref(false)
const isSubmitting = ref(false)
const isTerminating = ref(false)

const terminateForm = reactive({
  terminated_at: '',
})

const today = () => new Date().toISOString().slice(0, 10)

const leaseId = computed(() => {
  const rawId = route.params.id
  const normalizedId = Array.isArray(rawId) ? rawId[0] : rawId
  const parsedId = Number(normalizedId)

  return Number.isFinite(parsedId) && parsedId > 0 ? parsedId : null
})

const pageTitle = computed(() => lease.value?.lease_no ?? t('leaseDetail.title'))

const submitDisabled = computed(() => {
  if (!lease.value) {
    return true
  }

  return isSubmitting.value || lease.value.status !== 'draft'
})

const terminateDisabled = computed(() => !lease.value || isTerminating.value || lease.value.status !== 'active')

const formatDate = (value: string | null) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
  }).format(new Date(`${value}T00:00:00`))
}

const formatTimestamp = (value: string | null) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(appStore.locale, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value))
}

const formatDecimal = (value: number) =>
  new Intl.NumberFormat(appStore.locale, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)

const resolveStatusLabel = (status: string) => {
  switch (status) {
    case 'draft':
      return t('common.statuses.draft')
    case 'pending_approval':
      return t('common.statuses.pendingApproval')
    case 'active':
      return t('common.statuses.active')
    case 'rejected':
      return t('common.statuses.rejected')
    case 'approved':
      return t('common.statuses.approved')
    case 'terminated':
      return t('common.statuses.terminated')
    default:
      return status
  }
}

const resolveTermTypeLabel = (termType: string) => {
  switch (termType) {
    case 'rent':
      return t('leaseDetail.options.termTypes.rent')
    case 'deposit':
      return t('leaseDetail.options.termTypes.deposit')
    default:
      return termType
  }
}

const resolveBillingCycleLabel = (billingCycle: string) => {
  switch (billingCycle) {
    case 'monthly':
      return t('leaseDetail.options.billingCycles.monthly')
    case 'quarterly':
      return t('leaseDetail.options.billingCycles.quarterly')
    case 'yearly':
      return t('leaseDetail.options.billingCycles.yearly')
    default:
      return billingCycle
  }
}

const loadLease = async () => {
  if (!leaseId.value) {
    errorMessage.value = t('leaseDetail.errors.invalidId')
    lease.value = null
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await getLease(leaseId.value)
    lease.value = response.data.lease
    terminateForm.terminated_at = response.data.lease.terminated_at?.slice(0, 10) ?? today()
  } catch (error) {
    lease.value = null
    errorMessage.value = error instanceof Error ? error.message : t('leaseDetail.errors.unableToLoad')
  } finally {
    isLoading.value = false
  }
}

const handleBack = async () => {
  await router.push({ name: 'lease-contracts' })
}

const handleSubmitForApproval = async () => {
  if (!lease.value) {
    return
  }

  isSubmitting.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const response = await submitLease(lease.value.id, {
      idempotency_key: crypto.randomUUID(),
    })

    lease.value = response.data.lease
    successMessage.value = t('leaseDetail.feedback.submitted')
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('leaseDetail.errors.unableToSubmit')
  } finally {
    isSubmitting.value = false
  }
}

const handleTerminate = async () => {
  if (!lease.value || !terminateForm.terminated_at) {
    return
  }

  isTerminating.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const response = await terminateLease(lease.value.id, {
      terminated_at: terminateForm.terminated_at,
    })

    lease.value = response.data.lease
    terminateForm.terminated_at = response.data.lease.terminated_at?.slice(0, 10) ?? terminateForm.terminated_at
    successMessage.value = t('leaseDetail.feedback.terminated')
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('leaseDetail.errors.unableToTerminate')
  } finally {
    isTerminating.value = false
  }
}

onMounted(() => {
  void loadLease()
})

watch(
  () => route.params.id,
  () => {
    void loadLease()
  },
)
</script>

<template>
  <div class="lease-detail-view" data-testid="lease-detail-view">
    <PageSection
      :eyebrow="t('lease.eyebrow')"
      :title="pageTitle"
      :summary="t('leaseDetail.summary')"
    >
      <template #actions>
        <el-tag v-if="lease" effect="plain" type="info">{{ resolveStatusLabel(lease.status) }}</el-tag>
        <el-button data-testid="lease-detail-back-button" @click="handleBack">{{ t('leaseDetail.actions.backToList') }}</el-button>
      </template>
    </PageSection>

    <el-alert
        v-if="errorMessage"
        :closable="false"
        :title="t('leaseDetail.errors.detailUnavailable')"
        type="error"
        show-icon
        :description="errorMessage"
        data-testid="lease-detail-error-alert"
    />

    <el-alert
        v-if="successMessage"
        :closable="false"
        :title="t('leaseDetail.feedback.actionCompleted')"
        type="success"
        show-icon
        :description="successMessage"
        data-testid="lease-detail-success-alert"
    />

    <el-skeleton v-if="isLoading" :rows="6" animated />

    <template v-else-if="lease">
      <section class="lease-detail-view__grid">
        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.overview') }}</span>
            </div>
          </template>

          <el-descriptions :column="2" border>
            <el-descriptions-item :label="t('leaseDetail.fields.leaseNumber')">{{ lease.lease_no }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.tenant')">{{ lease.tenant_name }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.department')">{{ lease.department_id }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.store')">{{ lease.store_id }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.startDate')">{{ formatDate(lease.start_date) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.endDate')">{{ formatDate(lease.end_date) }}</el-descriptions-item>
            <el-descriptions-item :label="t('common.columns.status')">{{ resolveStatusLabel(lease.status) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.workflowInstance')">
              {{ lease.workflow_instance_id ?? t('leaseDetail.defaults.notCreatedYet') }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.submittedAt')">{{ formatTimestamp(lease.submitted_at) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.approvedAt')">{{ formatTimestamp(lease.approved_at) }}</el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.billingEffectiveAt')">
              {{ formatTimestamp(lease.billing_effective_at) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('leaseDetail.fields.terminatedAt')">{{ formatTimestamp(lease.terminated_at) }}</el-descriptions-item>
          </el-descriptions>
        </el-card>

        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.workflowActions') }}</span>
            </div>
          </template>

          <div class="lease-detail-view__actions">
            <el-button
              type="primary"
              :loading="isSubmitting"
              :disabled="submitDisabled"
              data-testid="lease-submit-button"
              @click="handleSubmitForApproval"
            >
              {{ t('leaseDetail.actions.submitForApproval') }}
            </el-button>

            <div class="lease-detail-view__terminate-panel">
              <el-form label-position="top">
                <el-form-item :label="t('leaseDetail.fields.terminateOn')">
                  <el-date-picker
                    v-model="terminateForm.terminated_at"
                    type="date"
                    value-format="YYYY-MM-DD"
                    :placeholder="t('leaseDetail.placeholders.selectTerminationDate')"
                    data-testid="lease-terminate-date-input"
                  />
                </el-form-item>
              </el-form>

              <el-button
                type="danger"
                plain
                :loading="isTerminating"
                :disabled="terminateDisabled || !terminateForm.terminated_at"
                data-testid="lease-terminate-button"
                @click="handleTerminate"
              >
                {{ t('leaseDetail.actions.terminateLease') }}
              </el-button>
            </div>
          </div>
        </el-card>
      </section>

      <section class="lease-detail-view__grid lease-detail-view__grid--secondary">
        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.units') }}</span>
            </div>
          </template>

          <el-table :data="lease.units" row-key="id" :empty-text="t('leaseDetail.table.unitsEmpty')">
            <el-table-column prop="unit_id" :label="t('leaseDetail.fields.unitId')" min-width="120" />
            <el-table-column :label="t('leaseDetail.fields.rentArea')" min-width="140">
              <template #default="scope">
                {{ formatDecimal(scope.row.rent_area) }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>

        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>{{ t('leaseDetail.cards.terms') }}</span>
            </div>
          </template>

          <el-table :data="lease.terms" row-key="id" :empty-text="t('leaseDetail.table.termsEmpty')">
            <el-table-column :label="t('leaseDetail.fields.termType')" min-width="140">
              <template #default="scope">
                {{ resolveTermTypeLabel(scope.row.term_type) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('leaseDetail.fields.billingCycle')" min-width="140">
              <template #default="scope">
                {{ resolveBillingCycleLabel(scope.row.billing_cycle) }}
              </template>
            </el-table-column>
            <el-table-column prop="currency_type_id" :label="t('leaseDetail.fields.currencyTypeId')" min-width="120" />
            <el-table-column :label="t('leaseDetail.fields.amount')" min-width="120">
              <template #default="scope">
                {{ formatDecimal(scope.row.amount) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('leaseDetail.fields.effectiveFrom')" min-width="140">
              <template #default="scope">
                {{ formatDate(scope.row.effective_from) }}
              </template>
            </el-table-column>
            <el-table-column :label="t('leaseDetail.fields.effectiveTo')" min-width="140">
              <template #default="scope">
                {{ formatDate(scope.row.effective_to) }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </section>
    </template>
  </div>
</template>

<style scoped>
.lease-detail-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.lease-detail-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.lease-detail-view__grid--secondary {
  align-items: start;
}

.lease-detail-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}

.lease-detail-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.lease-detail-view__actions {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
}

.lease-detail-view__terminate-panel {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-3);
}

.lease-detail-view__terminate-panel :deep(.el-date-editor) {
  width: 100%;
}

@media (max-width: 52rem) {
  .lease-detail-view__grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
</style>
