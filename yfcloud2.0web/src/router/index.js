import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/* Router Modules */

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar
    noCache: true                if set true, the page will no be cached(default is false)
    affix: true                  if set true, the tag will affix in the tags-view
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/redirect',
    component: Layout,
    hidden: true,
    children: [
      {
        path: '/redirect/:path*',
        // component: () => import('@/views/redirect/index'),
        component:resolve => require([ '@/views/redirect/index' ],resolve)
      }
    ]
  },
  {
    path: '/signIn',
    // component: () => import('@/views/signIn/index.vue'),
    component:resolve => require([ '@/views/signIn/index.vue' ],resolve),
    name: 'signIn',
    hidden: true
  },
  {
    path: '/termsService',
    // component: () => import('@/views/TermsService/index.vue'),
    component:resolve => require([ '@/views/TermsService/index.vue' ],resolve),
    name: 'termsService',
    hidden: true
  },
  {
    path: '/login',
    // component: () => import('@/views/login/index'),
    component:resolve => require([ '@/views/login/index' ],resolve),
    hidden: true
  },
  {
    path: '/auth-redirect',
    // component: () => import('@/views/login/auth-redirect'),
    component:resolve => require([ '@/views/login/auth-redirect' ],resolve),
    hidden: true
  },
  {
    path: '/password',
    // component: () => import('@/views/login/password'),
    component:resolve => require([ '@/views/login/password' ],resolve),
    hidden: true
  },
  {
    path: '/404',
    // component: () => import('@/views/error-page/404'),
    component:resolve => require([ '@/views/error-page/404' ],resolve),
    hidden: true
  },
  {
    path: '/401',
    // component: () => import('@/views/error-page/401'),
    component:resolve => require([ '@/views/error-page/401' ],resolve),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    hidden: true,
    children: [
      {
        path: '/dashboard',
        // component: () => import('@/views/dashboard/index'),
        component:resolve => require([ '@/views/dashboard/index' ],resolve),
        name: 'Dashboard',
        meta: { title: '首页', icon: 'dashboard', affix: true }
      },
      {
        path: '/KCHQtk',
        // component: () => import('@/views/inventory/ProcurementPutView/index'),
        component:resolve => require([ '@/views/inventory/ProcurementPutView/index' ],resolve),

        name: 'KCHQtk',
        meta: { title: '其他入库单', icon: '' }
      },
      {
        path: '/HHHFuM',
        // component: () => import('@/views/inventory/ProcurementOutView/index'),
        component:resolve => require([ '@/views/inventory/ProcurementOutView/index' ],resolve),
        name: 'HHHFuM',
        meta: { title: '其他出库单', icon: '' }
      },
      {
        path: '/HHHXSCK',
        // component: () => import('@/views/inventory/SalesReceiptView/index'),
        component:resolve => require([ '@/views/inventory/SalesReceiptView/index' ],resolve),
        name: 'HHHXSCK',
        meta: { title: '销售出库单', icon: '' }
      },
      {
        path: '/HHHXSCK1',
        // component: () => import('@/views/inventory/SalesReceiptView1/index'),
        component:resolve => require([ '@/views/inventory/SalesReceiptView1/index' ],resolve),
        name: 'HHHXSCK1',
        meta: { title: '销售单出库单', icon: '' }
      },
      {
        path: '/KCHPyK',
        // component: () => import('@/views/inventory/OverflowOrderView/index'),
        component:resolve => require([ '@/views/inventory/OverflowOrderView/index' ],resolve),
        name: 'KCHPyK',
        meta: { title: '盘盈入库单', icon: '' }
      },
      {
        path: '/YKCKD',
        // component: () => import('@/views/inventory/InventoryLossSlipGet/index'),
        component:resolve => require([ '@/views/inventory/InventoryLossSlipGet/index' ],resolve),
        name: 'YKCKD',
        meta: { title: '盘亏出库单', icon: '' }
      },
      {
        path: '/HLDYY',
        // component: () => import('@/views/inventory/InventorycheckGet/index'),
        component:resolve => require([ '@/views/inventory/InventorycheckGet/index' ],resolve),
        name: 'HLDYY',
        meta: { title: '盘点单', icon: '' }
      },
      {
        path: '/HLDYC',
        // component: () => import('@/views/Sales/SalesordersGet/index'),
        component:resolve => require([ '@/views/Sales/SalesordersGet/index' ],resolve),
        name: 'HLDYC',
        meta: { title: '销售订单', icon: '' }
      },
      {
        path: '/HLDYCY',
        // component: () => import('@/views/Sales/SalesordersGet_/index'),
        component:resolve => require([ '@/views/Sales/SalesordersGet_/index' ],resolve),
        name: 'HLDYCY',
        meta: { title: '销售单', icon: '' }
      },
      {
        path: '/HLDYD',
        // component: () => import('@/views/Purchase/PurchasingorderGet/index'),
        component:resolve => require([ '@/views/Purchase/PurchasingorderGet/index' ],resolve),
        name: 'HLDYD',
        meta: { title: '采购订单', icon: '' }
      },
      {
        path: '/CGRKD',
        // component: () => import('@/views/inventory/PurchasingLook/index'),
        component:resolve => require([ '@/views/inventory/PurchasingLook/index' ],resolve),
        name: 'CGRKD',
        meta: { title: '采购入库单', icon: '' }
      },


      {
        path: '/HLDYE',
        // component: () => import('@/views/inventory/requisitionOrderGet/index'),
        component:resolve => require([ '@/views/inventory/requisitionOrderGet/index' ],resolve),
        name: 'HLDYE',
        meta: { title: '普通领料单', icon: '' }
      },
      {
        path: '/HLDYF',
        // component: () => import('@/views/inventory/ProductionoutOrderGet/index'),
        component:resolve => require([ '@/views/inventory/ProductionoutOrderGet/index' ],resolve),
        name: 'HLDYF',
        meta: { title: '生产出库单', icon: '' }
      },
      {
        path: '/HLDYG',
        // component: () => import('@/views/ProductManage/ProductorderGet/index'),
        component:resolve => require([ '@/views/ProductManage/ProductorderGet/index' ],resolve),
        name: 'HLDYG',
        meta: { title: '生产单', icon: '' }
      },
      {
        path: '/HHSCRK',
        // component: () => import('@/views/inventory/ProductionReceiptView/index'),
        component:resolve => require([ '@/views/inventory/ProductionReceiptView/index' ],resolve),
        name: 'HHSCRK',
        meta: { title: '生产入库单', icon: '' }
      },


      // {
      //   path: '/EXCEL',
      //   component: () => import('@/views/inventory/EXCEL/index'),
      //   name: 'EXCEL',
      //   meta: { title: 'OtherPut2', icon: '' }
      // },

    ]
  }
  // {
  //   path: '/SysSettings',
  //   component: Layout,
  //   alwaysShow: true,
  //   meta: { title: '系统管理', icon: '' },
  //   children: [
  //     {
  //       path: '/User',
  //       component: () => import('@/views/SysSettings/User/index'),
  //       name: 'User',
  //       meta: { title: '用户维护', icon: '' }
  //     },
  //     {
  //       path: '/Dept',
  //       component: () => import('@/views/SysSettings/Dept/index'),
  //       name: 'Dept',
  //       meta: { title: '部门维护', icon: '' }
  //     },
  //     {
  //       path: '/Role',
  //       component: () => import('@/views/SysSettings/Role/index'),
  //       name: 'Role',
  //       meta: { title: '角色维护', icon: '' }
  //     },
  //     {
  //       path: '/RoleControl',
  //       component: () => import('@/views/SysSettings/RoleControl/index'),
  //       name: 'RoleControl',
  //       meta: { title: '角色管理', icon: '' }
  //     },
  //     {
  //       path: '/Permission',
  //       component: () => import('@/views/SysSettings/Permission/index'),
  //       name: 'Permission',
  //       meta: { title: '权限管理', icon: '' }
  //     },
  //     {
  //       path: '/deptAdmin',
  //       component: () => import('@/views/SysSettings/deptAdmin/index'),
  //       name: 'deptAdmin',
  //       meta: { title: '部门管理', icon: '' }
  //     }
  //   ]
  // },
  // {
  //   path: '/stockControl',
  //   component: Layout,
  //   alwaysShow: true,
  //   meta: { title: '库存管理', icon: '' },
  //   children: [
  //     {
  //       path: '/dataDictionary',
  //       component: () => import('@/views/stockControl/dataDictionary/index'),
  //       name: 'dataDictionary',
  //       meta: { title: '数据字典', icon: '' }
  //     }
  //   ]
  // }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
export const asyncRoutes = [
  /** when your routing map is too long, you can split it into small modules **/

  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () =>
  new Router({
    // mode: 'history', // require service support
    scrollBehavior: () => ({ y: 0 }),
    routes: constantRoutes
  })

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
