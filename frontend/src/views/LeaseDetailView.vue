<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

import { getLease, submitLease, terminateLease, type LeaseContract } from '../api/lease'
import PageSection from '../components/platform/PageSection.vue'

const route = useRoute()
const router = useRouter()

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

const submitDisabled = computed(() => {
  if (!lease.value) {
    return true
  }

  return isSubmitting.value || lease.value.status === 'submitted' || lease.value.status === 'approved' || lease.value.status === 'terminated'
})

const terminateDisabled = computed(() => !lease.value || isTerminating.value || lease.value.status === 'terminated')

const loadLease = async () => {
  if (!leaseId.value) {
    errorMessage.value = 'The requested lease contract id is invalid.'
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
    errorMessage.value = error instanceof Error ? error.message : 'Unable to load the lease contract.'
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
    successMessage.value = 'Lease submitted for approval.'
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unable to submit the lease for approval.'
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
    successMessage.value = 'Lease termination recorded.'
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unable to terminate the lease contract.'
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
      eyebrow="Lease delivery runway"
      :title="lease ? lease.lease_no : 'Lease contract detail'"
      summary="Review contract details, workflow state, and perform the next operational actions for the selected lease."
    >
      <template #actions>
        <el-tag v-if="lease" effect="plain" type="info">{{ lease.status }}</el-tag>
        <el-button @click="handleBack">Back to list</el-button>
      </template>
    </PageSection>

    <el-alert
      v-if="errorMessage"
      :closable="false"
      title="Lease detail unavailable"
      type="error"
      show-icon
      :description="errorMessage"
    />

    <el-alert
      v-if="successMessage"
      :closable="false"
      title="Lease action completed"
      type="success"
      show-icon
      :description="successMessage"
    />

    <el-skeleton v-if="isLoading" :rows="6" animated />

    <template v-else-if="lease">
      <section class="lease-detail-view__grid">
        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>Contract overview</span>
            </div>
          </template>

          <el-descriptions :column="2" border>
            <el-descriptions-item label="Lease no.">{{ lease.lease_no }}</el-descriptions-item>
            <el-descriptions-item label="Tenant">{{ lease.tenant_name }}</el-descriptions-item>
            <el-descriptions-item label="Department">{{ lease.department_id }}</el-descriptions-item>
            <el-descriptions-item label="Store">{{ lease.store_id }}</el-descriptions-item>
            <el-descriptions-item label="Start date">{{ lease.start_date }}</el-descriptions-item>
            <el-descriptions-item label="End date">{{ lease.end_date }}</el-descriptions-item>
            <el-descriptions-item label="Status">{{ lease.status }}</el-descriptions-item>
            <el-descriptions-item label="Workflow instance">
              {{ lease.workflow_instance_id ?? 'Not created yet' }}
            </el-descriptions-item>
            <el-descriptions-item label="Submitted at">{{ lease.submitted_at ?? '—' }}</el-descriptions-item>
            <el-descriptions-item label="Approved at">{{ lease.approved_at ?? '—' }}</el-descriptions-item>
            <el-descriptions-item label="Billing effective at">{{ lease.billing_effective_at ?? '—' }}</el-descriptions-item>
            <el-descriptions-item label="Terminated at">{{ lease.terminated_at ?? '—' }}</el-descriptions-item>
          </el-descriptions>
        </el-card>

        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>Workflow actions</span>
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
              Submit for approval
            </el-button>

            <div class="lease-detail-view__terminate-panel">
              <el-form label-position="top">
                <el-form-item label="Terminate on">
                  <el-date-picker
                    v-model="terminateForm.terminated_at"
                    type="date"
                    value-format="YYYY-MM-DD"
                    placeholder="Select termination date"
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
                Terminate lease
              </el-button>
            </div>
          </div>
        </el-card>
      </section>

      <section class="lease-detail-view__grid lease-detail-view__grid--secondary">
        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>Units</span>
            </div>
          </template>

          <el-table :data="lease.units" row-key="id" empty-text="No units attached.">
            <el-table-column prop="unit_id" label="Unit id" min-width="120" />
            <el-table-column prop="rent_area" label="Rent area" min-width="140" />
          </el-table>
        </el-card>

        <el-card class="lease-detail-view__card" shadow="never">
          <template #header>
            <div class="lease-detail-view__card-header">
              <span>Terms</span>
            </div>
          </template>

          <el-table :data="lease.terms" row-key="id" empty-text="No terms attached.">
            <el-table-column prop="term_type" label="Term type" min-width="140" />
            <el-table-column prop="billing_cycle" label="Billing cycle" min-width="140" />
            <el-table-column prop="currency_type_id" label="Currency" min-width="120" />
            <el-table-column prop="amount" label="Amount" min-width="120" />
            <el-table-column prop="effective_from" label="Effective from" min-width="140" />
            <el-table-column prop="effective_to" label="Effective to" min-width="140" />
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
