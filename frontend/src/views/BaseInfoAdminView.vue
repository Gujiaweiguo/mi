<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'

import {
  createCurrencyType,
  createShopType,
  createStoreType,
  createTradeDefinition,
  listCurrencyTypes,
  listShopTypes,
  listStoreTypes,
  listTradeDefinitions,
  updateCurrencyType,
  updateShopType,
  updateStoreType,
  updateTradeDefinition,
  type BaseInfoStatus,
  type ReferenceCatalogItem,
} from '../api/baseinfo'
import PageSection from '../components/platform/PageSection.vue'

type Feedback = {
  type: 'success' | 'error' | 'warning'
  title: string
  description: string
}

type StoreTypeForm = {
  code: string
  name: string
}

type ShopTypeForm = {
  code: string
  name: string
  color_hex: string
  status: BaseInfoStatus
}

type CurrencyTypeForm = {
  code: string
  name: string
  is_local: boolean
  status: BaseInfoStatus
}

type TradeDefinitionForm = {
  code: string
  name: string
  parent_id: number | undefined
  level: number | undefined
  status: BaseInfoStatus
}

const storeTypes = ref<ReferenceCatalogItem[]>([])
const shopTypes = ref<ReferenceCatalogItem[]>([])
const currencyTypes = ref<ReferenceCatalogItem[]>([])
const tradeDefinitions = ref<ReferenceCatalogItem[]>([])
const { t, locale } = useI18n()

const pageFeedback = ref<Feedback | null>(null)
const storeTypeFeedback = ref<Feedback | null>(null)
const shopTypeFeedback = ref<Feedback | null>(null)
const currencyTypeFeedback = ref<Feedback | null>(null)
const tradeDefinitionFeedback = ref<Feedback | null>(null)

const isLoading = ref(false)
const isStoreTypeSaving = ref(false)
const isShopTypeSaving = ref(false)
const isCurrencyTypeSaving = ref(false)
const isTradeDefinitionSaving = ref(false)

const storeTypeForm = reactive<StoreTypeForm>({
  code: '',
  name: '',
})

const shopTypeForm = reactive<ShopTypeForm>({
  code: '',
  name: '',
  color_hex: '',
  status: 'active',
})

const currencyTypeForm = reactive<CurrencyTypeForm>({
  code: '',
  name: '',
  is_local: false,
  status: 'active',
})

const tradeDefinitionForm = reactive<TradeDefinitionForm>({
  code: '',
  name: '',
  parent_id: undefined,
  level: undefined,
  status: 'active',
})

const storeTypeEditDialogOpen = ref(false)
const shopTypeEditDialogOpen = ref(false)
const currencyTypeEditDialogOpen = ref(false)
const tradeDefinitionEditDialogOpen = ref(false)

const isStoreTypeUpdating = ref(false)
const isShopTypeUpdating = ref(false)
const isCurrencyTypeUpdating = ref(false)
const isTradeDefinitionUpdating = ref(false)

const storeTypeEdit = reactive<StoreTypeForm & { id: number | null }>({
  id: null,
  code: '',
  name: '',
})

const shopTypeEdit = reactive<ShopTypeForm & { id: number | null }>({
  id: null,
  code: '',
  name: '',
  color_hex: '',
  status: 'active',
})

const currencyTypeEdit = reactive<CurrencyTypeForm & { id: number | null }>({
  id: null,
  code: '',
  name: '',
  is_local: false,
  status: 'active',
})

const tradeDefinitionEdit = reactive<TradeDefinitionForm & { id: number | null }>({
  id: null,
  code: '',
  name: '',
  parent_id: undefined,
  level: undefined,
  status: 'active',
})

const statusOptions: BaseInfoStatus[] = ['active', 'inactive']

const canCreateStoreType = computed(() => Boolean(storeTypeForm.code.trim() && storeTypeForm.name.trim()))
const canCreateShopType = computed(() => Boolean(shopTypeForm.code.trim() && shopTypeForm.name.trim()))
const canCreateCurrencyType = computed(() => Boolean(currencyTypeForm.code.trim() && currencyTypeForm.name.trim()))
const canCreateTradeDefinition = computed(() => Boolean(tradeDefinitionForm.code.trim() && tradeDefinitionForm.name.trim()))

const getErrorMessage = (error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)

const formatDate = (value: string) => {
  if (!value) {
    return t('common.emptyValue')
  }

  return new Intl.DateTimeFormat(locale.value, { dateStyle: 'medium' }).format(new Date(value))
}

const resolveStatusLabel = (status: string | undefined) => {
  if (status === 'active') {
    return t('common.statuses.active')
  }

  if (status === 'inactive') {
    return t('common.statuses.inactive')
  }

  return status ?? t('common.emptyValue')
}

const resolveStatusTag = (status: string | undefined) => {
  if (status === 'active') {
    return { label: resolveStatusLabel(status), type: 'success' as const }
  }

  if (status === 'inactive') {
    return { label: resolveStatusLabel(status), type: 'info' as const }
  }

  return { label: resolveStatusLabel(status), type: 'info' as const }
}

const resetStoreTypeForm = () => {
  storeTypeForm.code = ''
  storeTypeForm.name = ''
}

const resetShopTypeForm = () => {
  shopTypeForm.code = ''
  shopTypeForm.name = ''
  shopTypeForm.color_hex = ''
  shopTypeForm.status = 'active'
}

const resetCurrencyTypeForm = () => {
  currencyTypeForm.code = ''
  currencyTypeForm.name = ''
  currencyTypeForm.is_local = false
  currencyTypeForm.status = 'active'
}

const resetTradeDefinitionForm = () => {
  tradeDefinitionForm.code = ''
  tradeDefinitionForm.name = ''
  tradeDefinitionForm.parent_id = undefined
  tradeDefinitionForm.level = undefined
  tradeDefinitionForm.status = 'active'
}

const replaceRow = (rows: ReferenceCatalogItem[], updated: ReferenceCatalogItem) => {
  const index = rows.findIndex((row) => row.id === updated.id)
  if (index === -1) {
    return [updated, ...rows]
  }
  const next = rows.slice()
  next.splice(index, 1, updated)
  return next
}

const loadBaseInfo = async () => {
  isLoading.value = true
  pageFeedback.value = null

  const [storeTypesResult, shopTypesResult, currencyTypesResult, tradeDefinitionsResult] = await Promise.allSettled([
    listStoreTypes(),
    listShopTypes(),
    listCurrencyTypes(),
    listTradeDefinitions(),
  ])

  const loadErrors: string[] = []

  if (storeTypesResult.status === 'fulfilled') {
    storeTypes.value = storeTypesResult.value.data.store_types ?? []
  } else {
    storeTypes.value = []
    loadErrors.push(getErrorMessage(storeTypesResult.reason, t('baseinfoAdmin.errors.unableToLoadStoreTypes')))
  }

  if (shopTypesResult.status === 'fulfilled') {
    shopTypes.value = shopTypesResult.value.data.shop_types ?? []
  } else {
    shopTypes.value = []
    loadErrors.push(getErrorMessage(shopTypesResult.reason, t('baseinfoAdmin.errors.unableToLoadShopTypes')))
  }

  if (currencyTypesResult.status === 'fulfilled') {
    currencyTypes.value = currencyTypesResult.value.data.currency_types ?? []
  } else {
    currencyTypes.value = []
    loadErrors.push(getErrorMessage(currencyTypesResult.reason, t('baseinfoAdmin.errors.unableToLoadCurrencyTypes')))
  }

  if (tradeDefinitionsResult.status === 'fulfilled') {
    tradeDefinitions.value = tradeDefinitionsResult.value.data.trade_definitions ?? []
  } else {
    tradeDefinitions.value = []
    loadErrors.push(getErrorMessage(tradeDefinitionsResult.reason, t('baseinfoAdmin.errors.unableToLoadTradeDefinitions')))
  }

  if (loadErrors.length > 0) {
    pageFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.catalogsUnavailable'),
      description: loadErrors.join(' '),
    }
  }

  isLoading.value = false
}

const handleCreateStoreType = async () => {
  if (!canCreateStoreType.value) {
    storeTypeFeedback.value = {
      type: 'warning',
      title: t('baseinfoAdmin.feedback.storeTypeDetailsRequiredTitle'),
      description: t('baseinfoAdmin.feedback.storeTypeDetailsRequiredDescription'),
    }
    return
  }

  isStoreTypeSaving.value = true
  storeTypeFeedback.value = null

  try {
    const response = await createStoreType({
      code: storeTypeForm.code.trim(),
      name: storeTypeForm.name.trim(),
    })
    storeTypes.value = [response.data.store_type, ...storeTypes.value]
    storeTypeFeedback.value = {
      type: 'success',
      title: t('baseinfoAdmin.feedback.storeTypeCreatedTitle'),
      description: t('baseinfoAdmin.feedback.storeTypeCreatedDescription', { code: response.data.store_type.code }),
    }
    resetStoreTypeForm()
  } catch (error) {
    storeTypeFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.storeTypeCreationFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToCreateStoreType')),
    }
  } finally {
    isStoreTypeSaving.value = false
  }
}

const handleCreateShopType = async () => {
  if (!canCreateShopType.value) {
    shopTypeFeedback.value = {
      type: 'warning',
      title: t('baseinfoAdmin.feedback.shopTypeDetailsRequiredTitle'),
      description: t('baseinfoAdmin.feedback.shopTypeDetailsRequiredDescription'),
    }
    return
  }

  isShopTypeSaving.value = true
  shopTypeFeedback.value = null

  try {
    const response = await createShopType({
      code: shopTypeForm.code.trim(),
      name: shopTypeForm.name.trim(),
      color_hex: shopTypeForm.color_hex.trim() ? shopTypeForm.color_hex.trim() : null,
      status: shopTypeForm.status,
    })
    shopTypes.value = [response.data.shop_type, ...shopTypes.value]
    shopTypeFeedback.value = {
      type: 'success',
      title: t('baseinfoAdmin.feedback.shopTypeCreatedTitle'),
      description: t('baseinfoAdmin.feedback.shopTypeCreatedDescription', { code: response.data.shop_type.code }),
    }
    resetShopTypeForm()
  } catch (error) {
    shopTypeFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.shopTypeCreationFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToCreateShopType')),
    }
  } finally {
    isShopTypeSaving.value = false
  }
}

const handleCreateCurrencyType = async () => {
  if (!canCreateCurrencyType.value) {
    currencyTypeFeedback.value = {
      type: 'warning',
      title: t('baseinfoAdmin.feedback.currencyTypeDetailsRequiredTitle'),
      description: t('baseinfoAdmin.feedback.currencyTypeDetailsRequiredDescription'),
    }
    return
  }

  isCurrencyTypeSaving.value = true
  currencyTypeFeedback.value = null

  try {
    const response = await createCurrencyType({
      code: currencyTypeForm.code.trim(),
      name: currencyTypeForm.name.trim(),
      is_local: currencyTypeForm.is_local,
      status: currencyTypeForm.status,
    })
    currencyTypes.value = [response.data.currency_type, ...currencyTypes.value]
    currencyTypeFeedback.value = {
      type: 'success',
      title: t('baseinfoAdmin.feedback.currencyTypeCreatedTitle'),
      description: t('baseinfoAdmin.feedback.currencyTypeCreatedDescription', { code: response.data.currency_type.code }),
    }
    resetCurrencyTypeForm()
  } catch (error) {
    currencyTypeFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.currencyTypeCreationFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToCreateCurrencyType')),
    }
  } finally {
    isCurrencyTypeSaving.value = false
  }
}

const handleCreateTradeDefinition = async () => {
  if (!canCreateTradeDefinition.value) {
    tradeDefinitionFeedback.value = {
      type: 'warning',
      title: t('baseinfoAdmin.feedback.tradeDefinitionDetailsRequiredTitle'),
      description: t('baseinfoAdmin.feedback.tradeDefinitionDetailsRequiredDescription'),
    }
    return
  }

  isTradeDefinitionSaving.value = true
  tradeDefinitionFeedback.value = null

  try {
    const response = await createTradeDefinition({
      code: tradeDefinitionForm.code.trim(),
      name: tradeDefinitionForm.name.trim(),
      parent_id: tradeDefinitionForm.parent_id ?? null,
      level: tradeDefinitionForm.level ?? null,
      status: tradeDefinitionForm.status,
    })
    tradeDefinitions.value = [response.data.trade_definition, ...tradeDefinitions.value]
    tradeDefinitionFeedback.value = {
      type: 'success',
      title: t('baseinfoAdmin.feedback.tradeDefinitionCreatedTitle'),
      description: t('baseinfoAdmin.feedback.tradeDefinitionCreatedDescription', { code: response.data.trade_definition.code }),
    }
    resetTradeDefinitionForm()
  } catch (error) {
    tradeDefinitionFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.tradeDefinitionCreationFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToCreateTradeDefinition')),
    }
  } finally {
    isTradeDefinitionSaving.value = false
  }
}

const openStoreTypeEdit = (item: ReferenceCatalogItem) => {
  storeTypeEdit.id = item.id
  storeTypeEdit.code = item.code
  storeTypeEdit.name = item.name
  storeTypeEditDialogOpen.value = true
}

const openShopTypeEdit = (item: ReferenceCatalogItem) => {
  shopTypeEdit.id = item.id
  shopTypeEdit.code = item.code
  shopTypeEdit.name = item.name
  shopTypeEdit.color_hex = item.color_hex ?? ''
  shopTypeEdit.status = (item.status as BaseInfoStatus | undefined) ?? 'active'
  shopTypeEditDialogOpen.value = true
}

const openCurrencyTypeEdit = (item: ReferenceCatalogItem) => {
  currencyTypeEdit.id = item.id
  currencyTypeEdit.code = item.code
  currencyTypeEdit.name = item.name
  currencyTypeEdit.is_local = item.is_local ?? false
  currencyTypeEdit.status = (item.status as BaseInfoStatus | undefined) ?? 'active'
  currencyTypeEditDialogOpen.value = true
}

const openTradeDefinitionEdit = (item: ReferenceCatalogItem) => {
  tradeDefinitionEdit.id = item.id
  tradeDefinitionEdit.code = item.code
  tradeDefinitionEdit.name = item.name
  tradeDefinitionEdit.parent_id = item.parent_id
  tradeDefinitionEdit.level = item.level
  tradeDefinitionEdit.status = (item.status as BaseInfoStatus | undefined) ?? 'active'
  tradeDefinitionEditDialogOpen.value = true
}

const handleUpdateStoreType = async () => {
  if (storeTypeEdit.id === null || !storeTypeEdit.code.trim() || !storeTypeEdit.name.trim()) {
    return
  }

  isStoreTypeUpdating.value = true
  try {
    const response = await updateStoreType(storeTypeEdit.id, {
      code: storeTypeEdit.code.trim(),
      name: storeTypeEdit.name.trim(),
    })
    storeTypes.value = replaceRow(storeTypes.value, response.data.store_type)
    storeTypeEditDialogOpen.value = false
  } catch (error) {
    storeTypeFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.storeTypeUpdateFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToUpdateStoreType')),
    }
  } finally {
    isStoreTypeUpdating.value = false
  }
}

const handleUpdateShopType = async () => {
  if (shopTypeEdit.id === null || !shopTypeEdit.code.trim() || !shopTypeEdit.name.trim()) {
    return
  }

  isShopTypeUpdating.value = true
  try {
    const response = await updateShopType(shopTypeEdit.id, {
      code: shopTypeEdit.code.trim(),
      name: shopTypeEdit.name.trim(),
      color_hex: shopTypeEdit.color_hex.trim() ? shopTypeEdit.color_hex.trim() : null,
      status: shopTypeEdit.status,
    })
    shopTypes.value = replaceRow(shopTypes.value, response.data.shop_type)
    shopTypeEditDialogOpen.value = false
  } catch (error) {
    shopTypeFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.shopTypeUpdateFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToUpdateShopType')),
    }
  } finally {
    isShopTypeUpdating.value = false
  }
}

const handleUpdateCurrencyType = async () => {
  if (currencyTypeEdit.id === null || !currencyTypeEdit.code.trim() || !currencyTypeEdit.name.trim()) {
    return
  }

  isCurrencyTypeUpdating.value = true
  try {
    const response = await updateCurrencyType(currencyTypeEdit.id, {
      code: currencyTypeEdit.code.trim(),
      name: currencyTypeEdit.name.trim(),
      is_local: currencyTypeEdit.is_local,
      status: currencyTypeEdit.status,
    })
    currencyTypes.value = replaceRow(currencyTypes.value, response.data.currency_type)
    currencyTypeEditDialogOpen.value = false
  } catch (error) {
    currencyTypeFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.currencyTypeUpdateFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToUpdateCurrencyType')),
    }
  } finally {
    isCurrencyTypeUpdating.value = false
  }
}

const handleUpdateTradeDefinition = async () => {
  if (tradeDefinitionEdit.id === null || !tradeDefinitionEdit.code.trim() || !tradeDefinitionEdit.name.trim()) {
    return
  }

  isTradeDefinitionUpdating.value = true
  try {
    const response = await updateTradeDefinition(tradeDefinitionEdit.id, {
      code: tradeDefinitionEdit.code.trim(),
      name: tradeDefinitionEdit.name.trim(),
      parent_id: tradeDefinitionEdit.parent_id ?? null,
      level: tradeDefinitionEdit.level ?? null,
      status: tradeDefinitionEdit.status,
    })
    tradeDefinitions.value = replaceRow(tradeDefinitions.value, response.data.trade_definition)
    tradeDefinitionEditDialogOpen.value = false
  } catch (error) {
    tradeDefinitionFeedback.value = {
      type: 'error',
      title: t('baseinfoAdmin.errors.tradeDefinitionUpdateFailed'),
      description: getErrorMessage(error, t('baseinfoAdmin.errors.unableToUpdateTradeDefinition')),
    }
  } finally {
    isTradeDefinitionUpdating.value = false
  }
}

onMounted(() => {
  void loadBaseInfo()
})
</script>

<template>
  <div class="baseinfo-admin-view" data-testid="baseinfo-admin-view">
    <PageSection
      :eyebrow="t('baseinfoAdmin.eyebrow')"
      :title="t('baseinfoAdmin.title')"
      :summary="t('baseinfoAdmin.summary')"
    >
      <template #actions>
        <el-tag effect="plain" type="info">{{ t('baseinfoAdmin.tags.storeTypes', { count: storeTypes.length }) }}</el-tag>
        <el-tag effect="plain" type="success">{{ t('baseinfoAdmin.tags.shopTypes', { count: shopTypes.length }) }}</el-tag>
        <el-tag effect="plain" type="warning">{{ t('baseinfoAdmin.tags.currencies', { count: currencyTypes.length }) }}</el-tag>
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

    <div class="baseinfo-admin-view__grid">
      <el-card class="baseinfo-admin-view__card" shadow="never">
        <template #header>
          <div class="baseinfo-admin-view__card-header">
            <span>{{ t('baseinfoAdmin.cards.storeTypes') }}</span>
            <div class="baseinfo-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: storeTypes.length }) }}</el-tag>
              <el-button :loading="isLoading" @click="loadBaseInfo">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="storeTypeFeedback"
          :closable="false"
          class="baseinfo-admin-view__feedback"
          :title="storeTypeFeedback.title"
          :type="storeTypeFeedback.type"
          :description="storeTypeFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="baseinfo-admin-view__form" @submit.prevent>
          <div class="baseinfo-admin-view__form-grid">
            <el-form-item :label="t('baseinfoAdmin.fields.code')">
              <el-input
                v-model="storeTypeForm.code"
                :placeholder="t('baseinfoAdmin.placeholders.storeTypeCode')"
                data-testid="baseinfo-store-type-code-input"
              />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.name')">
              <el-input
                v-model="storeTypeForm.name"
                :placeholder="t('baseinfoAdmin.placeholders.storeTypeName')"
                data-testid="baseinfo-store-type-name-input"
              />
            </el-form-item>
          </div>

          <div class="baseinfo-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isStoreTypeSaving"
              :disabled="!canCreateStoreType"
              data-testid="baseinfo-store-type-create-button"
              @click="handleCreateStoreType"
            >
              {{ t('baseinfoAdmin.actions.createStoreType') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="storeTypes"
          row-key="id"
          class="baseinfo-admin-view__table"
          :empty-text="isLoading ? t('baseinfoAdmin.table.loadingStoreTypes') : t('baseinfoAdmin.table.emptyStoreTypes')"
        >
          <el-table-column prop="code" :label="t('baseinfoAdmin.fields.code')" min-width="140" />
          <el-table-column prop="name" :label="t('baseinfoAdmin.fields.name')" min-width="220" />
          <el-table-column :label="t('baseinfoAdmin.fields.updated')" min-width="160">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
              <el-button size="small" @click="openStoreTypeEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="baseinfo-admin-view__card" shadow="never">
        <template #header>
          <div class="baseinfo-admin-view__card-header">
            <span>{{ t('baseinfoAdmin.cards.shopTypes') }}</span>
            <div class="baseinfo-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: shopTypes.length }) }}</el-tag>
              <el-button :loading="isLoading" @click="loadBaseInfo">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="shopTypeFeedback"
          :closable="false"
          class="baseinfo-admin-view__feedback"
          :title="shopTypeFeedback.title"
          :type="shopTypeFeedback.type"
          :description="shopTypeFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="baseinfo-admin-view__form" @submit.prevent>
          <div class="baseinfo-admin-view__form-grid baseinfo-admin-view__form-grid--wide">
            <el-form-item :label="t('baseinfoAdmin.fields.code')">
              <el-input v-model="shopTypeForm.code" :placeholder="t('baseinfoAdmin.placeholders.shopTypeCode')" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.name')">
              <el-input v-model="shopTypeForm.name" :placeholder="t('baseinfoAdmin.placeholders.shopTypeName')" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.colorHex')">
              <div class="baseinfo-admin-view__color-input">
                <span
                  class="baseinfo-admin-view__color-swatch"
                  :style="{ background: shopTypeForm.color_hex.trim() || 'transparent' }"
                />
                <el-input v-model="shopTypeForm.color_hex" :placeholder="t('baseinfoAdmin.placeholders.colorHex')" />
              </div>
            </el-form-item>
            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="shopTypeForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="baseinfo-admin-view__form-actions">
            <el-button type="primary" :loading="isShopTypeSaving" :disabled="!canCreateShopType" @click="handleCreateShopType">
              {{ t('baseinfoAdmin.actions.createShopType') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="shopTypes"
          row-key="id"
          class="baseinfo-admin-view__table"
          :empty-text="isLoading ? t('baseinfoAdmin.table.loadingShopTypes') : t('baseinfoAdmin.table.emptyShopTypes')"
        >
          <el-table-column prop="code" :label="t('baseinfoAdmin.fields.code')" min-width="140" />
          <el-table-column prop="name" :label="t('baseinfoAdmin.fields.name')" min-width="200" />
          <el-table-column :label="t('baseinfoAdmin.fields.colorHex')" min-width="120">
            <template #default="scope">
              <div class="baseinfo-admin-view__color-cell">
                <span
                  class="baseinfo-admin-view__color-swatch"
                  :style="{ background: scope.row.color_hex ?? 'transparent' }"
                />
                <span class="baseinfo-admin-view__muted">{{ scope.row.color_hex ?? t('common.emptyValue') }}</span>
              </div>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="120">
            <template #default="scope">
              <el-tag :type="resolveStatusTag(scope.row.status).type" effect="plain">
                {{ resolveStatusTag(scope.row.status).label }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('baseinfoAdmin.fields.updated')" min-width="160">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
              <el-button size="small" @click="openShopTypeEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="baseinfo-admin-view__card" shadow="never">
        <template #header>
          <div class="baseinfo-admin-view__card-header">
            <span>{{ t('baseinfoAdmin.cards.currencyTypes') }}</span>
            <div class="baseinfo-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: currencyTypes.length }) }}</el-tag>
              <el-button :loading="isLoading" @click="loadBaseInfo">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="currencyTypeFeedback"
          :closable="false"
          class="baseinfo-admin-view__feedback"
          :title="currencyTypeFeedback.title"
          :type="currencyTypeFeedback.type"
          :description="currencyTypeFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="baseinfo-admin-view__form" @submit.prevent>
          <div class="baseinfo-admin-view__form-grid baseinfo-admin-view__form-grid--wide">
            <el-form-item :label="t('baseinfoAdmin.fields.code')">
              <el-input v-model="currencyTypeForm.code" :placeholder="t('baseinfoAdmin.placeholders.currencyCode')" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.name')">
              <el-input v-model="currencyTypeForm.name" :placeholder="t('baseinfoAdmin.placeholders.currencyName')" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.localCurrency')">
              <el-switch v-model="currencyTypeForm.is_local" />
            </el-form-item>
            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="currencyTypeForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="baseinfo-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isCurrencyTypeSaving"
              :disabled="!canCreateCurrencyType"
              @click="handleCreateCurrencyType"
            >
              {{ t('baseinfoAdmin.actions.createCurrency') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="currencyTypes"
          row-key="id"
          class="baseinfo-admin-view__table"
          :empty-text="isLoading ? t('baseinfoAdmin.table.loadingCurrencies') : t('baseinfoAdmin.table.emptyCurrencies')"
        >
          <el-table-column prop="code" :label="t('baseinfoAdmin.fields.code')" min-width="120" />
          <el-table-column prop="name" :label="t('baseinfoAdmin.fields.name')" min-width="220" />
          <el-table-column :label="t('baseinfoAdmin.fields.local')" min-width="100">
            <template #default="scope">
              <el-tag :type="scope.row.is_local ? 'success' : 'info'" effect="plain">
                {{ scope.row.is_local ? t('common.values.yes') : t('common.values.no') }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="120">
            <template #default="scope">
              <el-tag :type="resolveStatusTag(scope.row.status).type" effect="plain">
                {{ resolveStatusTag(scope.row.status).label }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('baseinfoAdmin.fields.updated')" min-width="160">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
              <el-button size="small" @click="openCurrencyTypeEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>

      <el-card class="baseinfo-admin-view__card" shadow="never">
        <template #header>
          <div class="baseinfo-admin-view__card-header">
            <span>{{ t('baseinfoAdmin.cards.tradeDefinitions') }}</span>
            <div class="baseinfo-admin-view__card-actions">
              <el-tag effect="plain" type="info">{{ t('common.total', { count: tradeDefinitions.length }) }}</el-tag>
              <el-button :loading="isLoading" @click="loadBaseInfo">{{ t('common.actions.refresh') }}</el-button>
            </div>
          </div>
        </template>

        <el-alert
          v-if="tradeDefinitionFeedback"
          :closable="false"
          class="baseinfo-admin-view__feedback"
          :title="tradeDefinitionFeedback.title"
          :type="tradeDefinitionFeedback.type"
          :description="tradeDefinitionFeedback.description"
          show-icon
        />

        <el-form label-position="top" class="baseinfo-admin-view__form" @submit.prevent>
          <div class="baseinfo-admin-view__form-grid baseinfo-admin-view__form-grid--wide">
            <el-form-item :label="t('baseinfoAdmin.fields.code')">
              <el-input v-model="tradeDefinitionForm.code" :placeholder="t('baseinfoAdmin.placeholders.tradeDefinitionCode')" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.name')">
              <el-input v-model="tradeDefinitionForm.name" :placeholder="t('baseinfoAdmin.placeholders.tradeDefinitionName')" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.parentId')">
              <el-input-number v-model="tradeDefinitionForm.parent_id" :min="1" controls-position="right" />
            </el-form-item>
            <el-form-item :label="t('baseinfoAdmin.fields.level')">
              <el-input-number v-model="tradeDefinitionForm.level" :min="1" controls-position="right" />
            </el-form-item>
            <el-form-item :label="t('common.columns.status')">
              <el-select v-model="tradeDefinitionForm.status">
                <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
              </el-select>
            </el-form-item>
          </div>

          <div class="baseinfo-admin-view__form-actions">
            <el-button
              type="primary"
              :loading="isTradeDefinitionSaving"
              :disabled="!canCreateTradeDefinition"
              @click="handleCreateTradeDefinition"
            >
              {{ t('baseinfoAdmin.actions.createTradeDefinition') }}
            </el-button>
          </div>
        </el-form>

        <el-table
          :data="tradeDefinitions"
          row-key="id"
          class="baseinfo-admin-view__table"
          :empty-text="isLoading ? t('baseinfoAdmin.table.loadingTradeDefinitions') : t('baseinfoAdmin.table.emptyTradeDefinitions')"
        >
          <el-table-column prop="code" :label="t('baseinfoAdmin.fields.code')" min-width="140" />
          <el-table-column prop="name" :label="t('baseinfoAdmin.fields.name')" min-width="200" />
          <el-table-column :label="t('baseinfoAdmin.fields.parent')" min-width="120">
            <template #default="scope">
              {{ scope.row.parent_id ?? t('common.emptyValue') }}
            </template>
          </el-table-column>
          <el-table-column :label="t('baseinfoAdmin.fields.level')" min-width="100">
            <template #default="scope">
              {{ scope.row.level ?? t('common.emptyValue') }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.status')" min-width="120">
            <template #default="scope">
              <el-tag :type="resolveStatusTag(scope.row.status).type" effect="plain">
                {{ resolveStatusTag(scope.row.status).label }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="t('baseinfoAdmin.fields.updated')" min-width="160">
            <template #default="scope">
              {{ formatDate(scope.row.updated_at) }}
            </template>
          </el-table-column>
          <el-table-column :label="t('common.columns.actions')" min-width="120" fixed="right">
            <template #default="scope">
              <el-button size="small" @click="openTradeDefinitionEdit(scope.row)">{{ t('common.actions.edit') }}</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </div>

    <el-dialog v-model="storeTypeEditDialogOpen" :title="t('baseinfoAdmin.dialogs.editStoreType')" width="32rem">
      <el-form label-position="top" @submit.prevent>
        <el-form-item :label="t('baseinfoAdmin.fields.code')">
          <el-input v-model="storeTypeEdit.code" />
        </el-form-item>
        <el-form-item :label="t('baseinfoAdmin.fields.name')">
          <el-input v-model="storeTypeEdit.name" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="storeTypeEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isStoreTypeUpdating" @click="handleUpdateStoreType">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="shopTypeEditDialogOpen" :title="t('baseinfoAdmin.dialogs.editShopType')" width="34rem">
      <el-form label-position="top" @submit.prevent>
        <div class="baseinfo-admin-view__dialog-grid">
          <el-form-item :label="t('baseinfoAdmin.fields.code')">
            <el-input v-model="shopTypeEdit.code" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.name')">
            <el-input v-model="shopTypeEdit.name" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.colorHex')">
            <div class="baseinfo-admin-view__color-input">
              <span
                class="baseinfo-admin-view__color-swatch"
                :style="{ background: shopTypeEdit.color_hex.trim() || 'transparent' }"
              />
              <el-input v-model="shopTypeEdit.color_hex" :placeholder="t('baseinfoAdmin.placeholders.colorHex')" />
            </div>
          </el-form-item>
          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="shopTypeEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>
      <template #footer>
        <el-button @click="shopTypeEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isShopTypeUpdating" @click="handleUpdateShopType">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="currencyTypeEditDialogOpen" :title="t('baseinfoAdmin.dialogs.editCurrency')" width="34rem">
      <el-form label-position="top" @submit.prevent>
        <div class="baseinfo-admin-view__dialog-grid">
          <el-form-item :label="t('baseinfoAdmin.fields.code')">
            <el-input v-model="currencyTypeEdit.code" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.name')">
            <el-input v-model="currencyTypeEdit.name" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.localCurrency')">
            <el-switch v-model="currencyTypeEdit.is_local" />
          </el-form-item>
          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="currencyTypeEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>
      <template #footer>
        <el-button @click="currencyTypeEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isCurrencyTypeUpdating" @click="handleUpdateCurrencyType">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="tradeDefinitionEditDialogOpen" :title="t('baseinfoAdmin.dialogs.editTradeDefinition')" width="36rem">
      <el-form label-position="top" @submit.prevent>
        <div class="baseinfo-admin-view__dialog-grid baseinfo-admin-view__dialog-grid--trade">
          <el-form-item :label="t('baseinfoAdmin.fields.code')">
            <el-input v-model="tradeDefinitionEdit.code" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.name')">
            <el-input v-model="tradeDefinitionEdit.name" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.parentId')">
            <el-input-number v-model="tradeDefinitionEdit.parent_id" :min="1" controls-position="right" />
          </el-form-item>
          <el-form-item :label="t('baseinfoAdmin.fields.level')">
            <el-input-number v-model="tradeDefinitionEdit.level" :min="1" controls-position="right" />
          </el-form-item>
          <el-form-item :label="t('common.columns.status')">
            <el-select v-model="tradeDefinitionEdit.status">
              <el-option v-for="option in statusOptions" :key="option" :label="resolveStatusLabel(option)" :value="option" />
            </el-select>
          </el-form-item>
        </div>
      </el-form>
      <template #footer>
        <el-button @click="tradeDefinitionEditDialogOpen = false">{{ t('common.actions.cancel') }}</el-button>
        <el-button type="primary" :loading="isTradeDefinitionUpdating" @click="handleUpdateTradeDefinition">{{ t('common.actions.save') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<style scoped>
.baseinfo-admin-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.baseinfo-admin-view__grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-5);
}

.baseinfo-admin-view__card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
  background: var(--mi-surface-gradient);
  box-shadow: var(--mi-shadow-sm);
}

.baseinfo-admin-view__card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--mi-space-3);
  font-size: var(--mi-font-size-300);
  font-weight: var(--mi-font-weight-semibold);
  color: var(--mi-color-text);
}

.baseinfo-admin-view__card-actions {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
  flex-wrap: wrap;
  justify-content: flex-end;
}

.baseinfo-admin-view__feedback {
  margin-bottom: var(--mi-space-4);
}

.baseinfo-admin-view__form {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-4);
  margin-bottom: var(--mi-space-4);
}

.baseinfo-admin-view__form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.baseinfo-admin-view__form-grid--wide {
  grid-template-columns: repeat(4, minmax(0, 1fr));
}

.baseinfo-admin-view__form-actions {
  display: flex;
  justify-content: flex-end;
  gap: var(--mi-space-3);
}

.baseinfo-admin-view__table,
.baseinfo-admin-view__form-grid :deep(.el-input-number),
.baseinfo-admin-view__form-grid :deep(.el-select) {
  width: 100%;
}

.baseinfo-admin-view__muted {
  color: var(--mi-color-muted);
}

.baseinfo-admin-view__color-input {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.baseinfo-admin-view__color-cell {
  display: flex;
  align-items: center;
  gap: var(--mi-space-3);
}

.baseinfo-admin-view__color-swatch {
  width: 0.875rem;
  height: 0.875rem;
  border-radius: 999px;
  border: var(--mi-border-width-thin) solid var(--mi-color-border);
  box-shadow: 0 0.25rem 0.75rem rgba(26, 35, 55, 0.18);
}

.baseinfo-admin-view__dialog-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: var(--mi-space-4);
}

.baseinfo-admin-view__dialog-grid--trade {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

@media (max-width: 52rem) {
  .baseinfo-admin-view__grid,
  .baseinfo-admin-view__form-grid,
  .baseinfo-admin-view__form-grid--wide,
  .baseinfo-admin-view__dialog-grid,
  .baseinfo-admin-view__dialog-grid--trade {
    grid-template-columns: minmax(0, 1fr);
  }

  .baseinfo-admin-view__card-header,
  .baseinfo-admin-view__form-actions {
    align-items: flex-start;
    flex-direction: column;
  }

  .baseinfo-admin-view__card-actions {
    justify-content: flex-start;
  }
}
</style>
