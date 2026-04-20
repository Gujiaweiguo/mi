import { createRouter, createWebHistory } from 'vue-router'

import { FUNCTION_CODES } from '../auth/permissions'
import LoginView from '../views/LoginView.vue'
import ForbiddenView from '../views/ForbiddenView.vue'
import DashboardView from '../views/DashboardView.vue'
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
      component: () => import('../views/HealthView.vue'),
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
      component: () => import('../views/WorkflowAdminView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.workflowAdmin,
      },
    },
    {
      path: '/notifications',
      name: 'notifications',
      component: () => import('../views/NotificationsView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.notificationAdmin,
      },
    },
    {
      path: '/admin/master-data',
      name: 'masterdata-admin',
      component: () => import('../views/MasterDataAdminView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.masterdataAdmin,
      },
    },
    {
      path: '/admin/sales',
      name: 'sales-admin',
      component: () => import('../views/SalesAdminView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.salesAdmin,
      },
    },
    {
      path: '/admin/base-info',
      name: 'baseinfo-admin',
      component: () => import('../views/BaseInfoAdminView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.baseinfoAdmin,
      },
    },
    {
      path: '/admin/structure',
      name: 'structure-admin',
      component: () => import('../views/StructureAdminView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.structureAdmin,
      },
    },
    {
      path: '/admin/rentable-areas',
      name: 'rentable-area-admin',
      component: () => import('../views/RentableAreaAdminView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.structureAdmin,
      },
    },
    {
      path: '/lease/contracts',
      name: 'lease-contracts',
      component: () => import('../views/LeaseListView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.leaseContract,
      },
    },
    {
      path: '/lease/contracts/new',
      name: 'lease-contracts-new',
      component: () => import('../views/LeaseCreateView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.leaseContract,
      },
    },
    {
      path: '/lease/contracts/:id',
      name: 'lease-contract-detail',
      component: () => import('../views/LeaseDetailView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.leaseContract,
      },
    },
    {
      path: '/billing/charges',
      name: 'billing-charges',
      component: () => import('../views/BillingChargesView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingCharge,
      },
    },
    {
      path: '/billing/invoices',
      name: 'billing-invoices',
      component: () => import('../views/BillingInvoicesView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingInvoice,
      },
    },
    {
      path: '/billing/receivables',
      name: 'billing-receivables',
      component: () => import('../views/ReceivablesView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingInvoice,
      },
    },
    {
      path: '/billing/invoices/:id',
      name: 'billing-invoice-detail',
      component: () => import('../views/InvoiceDetailView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.billingInvoice,
      },
    },
    {
      path: '/tax/exports',
      name: 'tax-exports',
      component: () => import('../views/TaxExportsView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.taxExport,
      },
    },
    {
      path: '/reports/generalize',
      name: 'generalize-reports',
      component: () => import('../views/GeneralizeReportsView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.generalizeReport,
      },
    },
    {
      path: '/reports/visual-shop',
      name: 'visual-shop-analysis',
      component: () => import('../views/VisualShopAnalysisView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.generalizeReport,
      },
    },
    {
      path: '/excel/io',
      name: 'excel-io',
      component: () => import('../views/ExcelIOView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.excelIo,
      },
    },
    {
      path: '/print/preview',
      name: 'print-preview',
      component: () => import('../views/PrintPreviewView.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/admin/users',
      name: 'user-management',
      component: () => import('../views/UserManagementView.vue'),
      meta: {
        requiresAuth: true,
        permissionCode: FUNCTION_CODES.baseinfoAdmin,
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
