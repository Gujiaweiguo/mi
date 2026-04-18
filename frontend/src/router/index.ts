import { createRouter, createWebHistory } from 'vue-router'

import { FUNCTION_CODES } from '../auth/permissions'
import LoginView from '../views/LoginView.vue'
import ForbiddenView from '../views/ForbiddenView.vue'
import BillingChargesView from '../views/BillingChargesView.vue'
import BillingInvoicesView from '../views/BillingInvoicesView.vue'
import ReceivablesView from '../views/ReceivablesView.vue'
import DashboardView from '../views/DashboardView.vue'
import HealthView from '../views/HealthView.vue'
import InvoiceDetailView from '../views/InvoiceDetailView.vue'
import LeaseCreateView from '../views/LeaseCreateView.vue'
import LeaseDetailView from '../views/LeaseDetailView.vue'
import LeaseListView from '../views/LeaseListView.vue'
import GeneralizeReportsView from '../views/GeneralizeReportsView.vue'
import VisualShopAnalysisView from '../views/VisualShopAnalysisView.vue'
import TaxExportsView from '../views/TaxExportsView.vue'
import ExcelIOView from '../views/ExcelIOView.vue'
import PrintPreviewView from '../views/PrintPreviewView.vue'
import MasterDataAdminView from '../views/MasterDataAdminView.vue'
import WorkflowAdminView from '../views/WorkflowAdminView.vue'
import SalesAdminView from '../views/SalesAdminView.vue'
import BaseInfoAdminView from '../views/BaseInfoAdminView.vue'
import StructureAdminView from '../views/StructureAdminView.vue'
import RentableAreaAdminView from '../views/RentableAreaAdminView.vue'
import { useAuthStore } from '../stores/auth'
import { pinia } from '../stores/pinia'
import { resolveAuthRedirect, resolveRootRedirect } from './auth-guard'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: () => {
        const authStore = useAuthStore(pinia)

        return resolveRootRedirect({
          isAuthenticated: authStore.isAuthenticated,
          user: authStore.sessionUser,
        })
      },
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
      meta: {
        guestOnly: true,
      },
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: DashboardView,
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/health',
      name: 'health',
      component: HealthView,
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/forbidden',
      name: 'forbidden',
      component: ForbiddenView,
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/workflow/admin',
      name: 'workflow-admin',
      component: WorkflowAdminView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.workflowAdmin,
      },
    },
    {
      path: '/admin/master-data',
      name: 'masterdata-admin',
      component: MasterDataAdminView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.masterdataAdmin,
      },
    },
    {
      path: '/admin/sales',
      name: 'sales-admin',
      component: SalesAdminView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.salesAdmin,
      },
    },
    {
      path: '/admin/base-info',
      name: 'baseinfo-admin',
      component: BaseInfoAdminView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.baseinfoAdmin,
      },
    },
    {
      path: '/admin/structure',
      name: 'structure-admin',
      component: StructureAdminView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.structureAdmin,
      },
    },
    {
      path: '/admin/rentable-areas',
      name: 'rentable-area-admin',
      component: RentableAreaAdminView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.structureAdmin,
      },
    },
    {
      path: '/lease/contracts',
      name: 'lease-contracts',
      component: LeaseListView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.leaseContract,
      },
    },
    {
      path: '/lease/contracts/new',
      name: 'lease-contracts-new',
      component: LeaseCreateView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.leaseContract,
      },
    },
    {
      path: '/lease/contracts/:id',
      name: 'lease-contract-detail',
      component: LeaseDetailView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.leaseContract,
      },
    },
    {
      path: '/billing/charges',
      name: 'billing-charges',
      component: BillingChargesView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingCharge,
      },
    },
    {
      path: '/billing/invoices',
      name: 'billing-invoices',
      component: BillingInvoicesView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingInvoice,
      },
    },
    {
      path: '/billing/receivables',
      name: 'billing-receivables',
      component: ReceivablesView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingInvoice,
      },
    },
    {
      path: '/billing/invoices/:id',
      name: 'billing-invoice-detail',
      component: InvoiceDetailView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingInvoice,
      },
    },
    {
      path: '/tax/exports',
      name: 'tax-exports',
      component: TaxExportsView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.taxExport,
      },
    },
    {
      path: '/reports/generalize',
      name: 'generalize-reports',
      component: GeneralizeReportsView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.generalizeReport,
      },
    },
    {
      path: '/reports/visual-shop',
      name: 'visual-shop-analysis',
      component: VisualShopAnalysisView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.generalizeReport,
      },
    },
    {
      path: '/excel/io',
      name: 'excel-io',
      component: ExcelIOView,
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.excelIo,
      },
    },
    {
      path: '/print/preview',
      name: 'print-preview',
      component: PrintPreviewView,
      meta: {
        requiresAuth: true,
      },
    },
  ],
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore(pinia)

  await authStore.initialize()

  return resolveAuthRedirect(to, {
    isAuthenticated: authStore.isAuthenticated,
    user: authStore.sessionUser,
  })
})

export default router
