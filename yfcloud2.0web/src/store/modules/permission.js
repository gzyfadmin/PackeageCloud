import { asyncRoutes, constantRoutes } from '@/router'
import Layout from '@/layout'
import store from '@/store/index'
import lazyLoading from '@/utils/lazyLoading'

// 导入开发组件开始
// import viewsSysSettingsDeptindex from '@/views/SysSettings/Dept/index.vue'
// import viewsSysSettingsUserindex from '@/views/SysSettings/User/index.vue'
// import viewsSysSettingsUserAuthindex from '@/views/SysSettings/UserAuth/index.vue'
// import viewsSysSettingsSystemMsgindex from '@/views/SysSettings/SystemMsg/index.vue'
// import viewsSysSettingsRoleindex from '@/views/SysSettings/Role/index.vue'
// import viewsSysSettingsdeptAdminindex from '@/views/SysSettings/deptAdmin/index.vue'
// import viewsSysSettingsPermissionindex from '@/views/SysSettings/Permission/index.vue'
// import viewsSysSettingsRoleControlindex from '@/views/SysSettings/RoleControl/index.vue'
// import viewsReserveAdminDataDictionaryindex from '@/views/ReserveAdmin/dataDictionary/index.vue'
// import viewsSysSettingslogManagementlogInsindex from '@/views/SysSettings/logManagement/logIns/index.vue'
// import viewsSysSettingslogManagementoPerationindex from '@/views/SysSettings/logManagement/oPeration/index.vue'
// import viewsReserveAdminWarehouseindex from '@/views/ReserveAdmin/Warehouse/index'
// import viewsReserveAdminMaterielindex from '@/views/ReserveAdmin/Materiel/index'
// import viewsinventoryProcurementPutindex from '@/views/inventory/ProcurementPut/index'
// import viewsinventoryProcurementPutViewindex from '@/views/inventory/ProcurementPutView/index'
// import viewsinventoryProcurementOutViewindex from '@/views/inventory/ProcurementOutView/index'
// import viewsinventoryOtherPutPutindex from '@/views/inventory/OtherPut/index'
// import viewsinventoryCheckout from '@/views/inventory/Checkout/index'
// import viewsinventoryOtherOutindex from '@/views/inventory/OtherOut/index'
// import viewsinventoryOverflowOrderindex from '@/views/inventory/OverflowOrder/index'
// import viewsinventoryOverflowPutindex from '@/views/inventory/OverflowPut/index'
// import viewsinventoryInventoryCountindex from '@/views/inventory/InventoryCount/index'
// import viewsinventoryInventorycheckindex from '@/views/inventory/Inventorycheck/index'
// import viewsinventoryInventorycheckListindex from '@/views/inventory/InventorycheckList/index'
// import viewsinventoryInventoryLoss from '@/views/inventory/InventoryLoss/index'
// import viewsinventoryInventoryLossSlip from '@/views/inventory/InventoryLossSlip/index'
// import viewsSalesorders from '@/views/Sales/Salesorders/index'
// import viewsSalesorders_ from '@/views/Sales/Salesorders_/index'
// import viewsSalesordersList from '@/views/Sales/SalesordersList/index'
// import viewsSalesordersList_ from '@/views/Sales/SalesordersList_/index'
// import viewsSalesordersListPlus from '@/views/Sales/SalesordersListPlus/index'
// import viewsSalesReceipt from '@/views/inventory/SalesReceipt/index'
// import viewsSalesReceipt1 from '@/views/inventory/SalesReceipt1/index'
// import viewsSalesOut from '@/views/inventory/SalesOut/index'
// import viewsSalesOut1 from '@/views/inventory/SalesOut1/index'
// import viewsReserveAdminconnection from '@/views/ReserveAdmin/connection/index'
// import viewsinventoryPurchasing from '@/views/inventory/Purchasing/index'
// import viewsinventoryPurchaseOrder from '@/views/inventory/PurchaseOrder/index'
// import viewsPurchasePurchasingorder from '@/views/Purchase/Purchasingorder/index'
// import viewsPurchasePurchasingorderList from '@/views/Purchase/PurchasingorderList/index'
// import viewsPurchaseProduceOrder from '@/views/Purchase/ProduceOrder/index'
// import viewsReserveAdminSupplier from '@/views/ReserveAdmin/Supplier/index'
// import viewsReserveAdminEnvelopeType from '@/views/ReserveAdmin/EnvelopeType/index'
// import viewsProductProductBom from '@/views/Product/ProductBom/index' 
// import viewsProductProductBomNo from '@/views/Product/ProductBomNo/index' 
// import viewsProductFormula from '@/views/Product/Formula/index' 
// import viewsProductColorscheme from '@/views/Product/Colorscheme/index' 
// import viewsProductManageProductorders from '@/views/ProductManage/Productorders/index' 
// import viewsProductManageProductordersList from '@/views/ProductManage/ProductordersList/index' 
// import viewsProductManageMRP from '@/views/ProductManage/MRP/index' 
// import viewsProductManageMRPNo from '@/views/ProductManage/MRPNo/index' 
// import viewsinventoryrequisitionOrder from '@/views/inventory/requisitionOrder/index' 
// import viewsinventoryrequisitionOrderList from '@/views/inventory/requisitionOrderList/index' 
// import viewsinventoryProductionoutOrder from '@/views/inventory/ProductionoutOrder/index' 
// import viewsinventoryProductionoutOrderList from '@/views/inventory/ProductionoutOrderList/index' 
// import viewsinventoryProductionReceipt from '@/views/inventory/ProductionReceipt/index' 
// import viewsinventoryProductionPut from '@/views/inventory/ProductionPut/index' 
// 导入开发组件结束
// var viewsSysSettingsDeptindex =  resolve => require([ '@/views/SysSettings/Dept/index.vue'],resolve);
// var viewsSysSettingsUserindex =  resolve => require([ '@/views/SysSettings/User/index.vue'],resolve);
// var viewsSysSettingsUserAuthindex =  resolve => require([ '@/views/SysSettings/UserAuth/index.vue'],resolve);
// var viewsSysSettingsSystemMsgindex =  resolve => require([ '@/views/SysSettings/SystemMsg/index.vue'],resolve);
// var viewsSysSettingsRoleindex =  resolve => require([ '@/views/SysSettings/Role/index.vue'],resolve);
// var viewsSysSettingsdeptAdminindex =  resolve => require([ '@/views/SysSettings/deptAdmin/index.vue'],resolve);
// var viewsSysSettingsPermissionindex =  resolve => require([ '@/views/SysSettings/Permission/index.vue'],resolve);
// var viewsSysSettingsRoleControlindex =  resolve => require([ '@/views/SysSettings/RoleControl/index.vue'],resolve);
// var viewsReserveAdminDataDictionaryindex =  resolve => require([ '@/views/ReserveAdmin/dataDictionary/index.vue'],resolve);
// var viewsSysSettingsLogInsindex =  resolve => require([ '@/views/SysSettings/LogIns/index.vue'],resolve);
// var viewsSysSettingsOPerationindex =  resolve => require([ '@/views/SysSettings/OPeration/index.vue'],resolve);
// var viewsReserveAdminWarehouseindex =  resolve => require([ '@/views/ReserveAdmin/Warehouse/index'],resolve);
// var viewsReserveAdminMaterielindex =  resolve => require([ '@/views/ReserveAdmin/Materiel/index'],resolve);
// var viewsinventoryProcurementPutindex =  resolve => require([ '@/views/inventory/ProcurementPut/index'],resolve);
// var viewsinventoryProcurementPutViewindex =  resolve => require([ '@/views/inventory/ProcurementPutView/index'],resolve);
// var viewsinventoryProcurementOutViewindex =  resolve => require([ '@/views/inventory/ProcurementOutView/index'],resolve);
// var viewsinventoryOtherPutPutindex =  resolve => require([ '@/views/inventory/OtherPut/index'],resolve);
// var viewsinventoryCheckout =  resolve => require([ '@/views/inventory/Checkout/index'],resolve);
// var viewsinventoryOtherOutindex =  resolve => require([ '@/views/inventory/OtherOut/index'],resolve);
// var viewsinventoryOverflowOrderindex =  resolve => require([ '@/views/inventory/OverflowOrder/index'],resolve);
// var viewsinventoryOverflowPutindex =  resolve => require([ '@/views/inventory/OverflowPut/index'],resolve);
// var viewsinventoryInventoryCountindex =  resolve => require([ '@/views/inventory/InventoryCount/index'],resolve);
// var viewsinventoryInventorycheckindex =  resolve => require([ '@/views/inventory/Inventorycheck/index'],resolve);
// var viewsinventoryInventorycheckListindex =  resolve => require([ '@/views/inventory/InventorycheckList/index'],resolve);
// var viewsinventoryInventoryLoss =  resolve => require([ '@/views/inventory/InventoryLoss/index'],resolve);
// var viewsinventoryInventoryLossSlip =  resolve => require([ '@/views/inventory/InventoryLossSlip/index'],resolve);
// var viewsSalesorders =  resolve => require([ '@/views/Sales/Salesorders/index'],resolve);
// var viewsSalesorders_ =  resolve => require([ '@/views/Sales/Salesorders_/index'],resolve);
// var viewsSalesordersList =  resolve => require([ '@/views/Sales/SalesordersList/index'],resolve);
// var viewsSalesordersList_ =  resolve => require([ '@/views/Sales/SalesordersList_/index'],resolve);
// var viewsSalesordersListPlus =  resolve => require([ '@/views/Sales/SalesordersListPlus/index'],resolve);
// var viewsSalesReceipt =  resolve => require([ '@/views/inventory/SalesReceipt/index'],resolve);
// var viewsSalesReceipt1 =  resolve => require([ '@/views/inventory/SalesReceipt1/index'],resolve);
// var viewsSalesOut =  resolve => require([ '@/views/inventory/SalesOut/index'],resolve);
// var viewsSalesOut1 =  resolve => require([ '@/views/inventory/SalesOut1/index'],resolve);
// var viewsReserveAdminconnection =  resolve => require([ '@/views/ReserveAdmin/connection/index'],resolve);
// var viewsinventoryPurchasing =  resolve => require([ '@/views/inventory/Purchasing/index'],resolve);
// var viewsinventoryPurchaseOrder =  resolve => require([ '@/views/inventory/PurchaseOrder/index'],resolve);
// var viewsPurchasePurchasingorder =  resolve => require([ '@/views/Purchase/Purchasingorder/index'],resolve);
// var viewsPurchasePurchasingorderList =  resolve => require([ '@/views/Purchase/PurchasingorderList/index'],resolve);
// var viewsPurchasePurchaseListPlus =  resolve => require([ '@/views/Purchase/PurchaseListPlus/index'],resolve);
// var viewsPurchaseProduceOrder =  resolve => require([ '@/views/Purchase/ProduceOrder/index'],resolve);
// var viewsReserveAdminSupplier =  resolve => require([ '@/views/ReserveAdmin/Supplier/index'],resolve);
// var viewsReserveAdminEnvelopeType =  resolve => require([ '@/views/ReserveAdmin/EnvelopeType/index'],resolve);
// var viewsProductProductBom =  resolve => require([ '@/views/Product/ProductBom/index' ],resolve);
// var viewsProductProductBomNo =  resolve => require([ '@/views/Product/ProductBomNo/index' ],resolve);
// var viewsProductFormula =  resolve => require([ '@/views/Product/Formula/index' ],resolve);
// var viewsProductColorscheme =  resolve => require([ '@/views/Product/Colorscheme/index' ],resolve);
// var viewsProductManageProductorders =  resolve => require([ '@/views/ProductManage/Productorders/index' ],resolve);
// var viewsProductManageProductordersList =  resolve => require([ '@/views/ProductManage/ProductordersList/index' ],resolve);
// var viewsProductManageMRP =  resolve => require([ '@/views/ProductManage/MRP/index' ],resolve);
// var viewsProductManageMRPNo =  resolve => require([ '@/views/ProductManage/MRPNo/index' ],resolve);
// var viewsinventoryrequisitionOrder =  resolve => require([ '@/views/inventory/requisitionOrder/index' ],resolve);
// var viewsinventoryrequisitionOrderList =  resolve => require([ '@/views/inventory/requisitionOrderList/index' ],resolve);
// var viewsinventoryProductionoutOrder =  resolve => require([ '@/views/inventory/ProductionoutOrder/index' ],resolve);
// var viewsinventoryProductionoutOrderList =  resolve => require([ '@/views/inventory/ProductionoutOrderList/index' ],resolve);
// var viewsinventoryProductionReceipt =  resolve => require([ '@/views/inventory/ProductionReceipt/index' ],resolve);
// var viewsinventoryProductionPut =  resolve => require([ '@/views/inventory/ProductionPut/index' ],resolve);



/**
 * Use meta.role to determine if the current user has permission
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  },
  CLEARROLE: (state, str) => {
    //清空权限
    state.routes = [];
    state.addRoutes = [];
  },
}

const routerMap = {}
routerMap['Layout'] = Layout
// 导入开发组件开始
routerMap['/views/SysSettings/Dept/index.vue'] = resolve => require(['@/views/SysSettings/Dept/index.vue'], resolve);
routerMap['/views/SysSettings/User/index.vue'] = resolve => require(['@/views/SysSettings/User/index.vue'], resolve);
routerMap['/views/SysSettings/UserAuth/index.vue'] = resolve => require(['@/views/SysSettings/UserAuth/index.vue'], resolve);
routerMap['/views/SysSettings/SystemMsg/index.vue'] = resolve => require(['@/views/SysSettings/SystemMsg/index.vue'], resolve);
routerMap['/views/SysSettings/Role/index.vue'] = resolve => require(['@/views/SysSettings/Role/index.vue'], resolve);
routerMap['/views/SysSettings/deptAdmin/index.vue'] = resolve => require(['@/views/SysSettings/deptAdmin/index.vue'], resolve);
routerMap['/views/SysSettings/Permission/index.vue'] = resolve => require(['@/views/SysSettings/Permission/index.vue'], resolve);
routerMap['/views/SysSettings/RoleControl/index.vue'] = resolve => require(['@/views/SysSettings/RoleControl/index.vue'], resolve);
routerMap['/views/ReserveAdmin/dataDictionary/index.vue'] = resolve => require(['@/views/ReserveAdmin/dataDictionary/index.vue'], resolve);
routerMap['/views/SysSettings/LogIns/index.vue'] = resolve => require(['@/views/SysSettings/LogIns/index.vue'], resolve);
routerMap['/views/SysSettings/OPeration/index.vue'] = resolve => require(['@/views/SysSettings/OPeration/index.vue'], resolve);
routerMap['/views/ReserveAdmin/Warehouse/index.vue'] = resolve => require(['@/views/ReserveAdmin/Warehouse/index'], resolve);
routerMap['/views/ReserveAdmin/Materiel/index.vue'] = resolve => require(['@/views/ReserveAdmin/Materiel/index'], resolve);
routerMap['/views/inventory/ProcurementPut/index.vue'] = resolve => require(['@/views/inventory/ProcurementPut/index'], resolve);
routerMap['/views/inventory/ProcurementPutView/index.vue'] = resolve => require(['@/views/inventory/ProcurementPutView/index'], resolve);
routerMap['/views/inventory/ProcurementOutView/index.vue'] = resolve => require(['@/views/inventory/ProcurementOutView/index'], resolve);

routerMap['/views/inventory/OtherPut/index.vue'] = resolve => require(['@/views/inventory/OtherPut/index'], resolve);
routerMap['/views/inventory/Checkout/index.vue'] = resolve => require(['@/views/inventory/Checkout/index'], resolve);
routerMap['/views/inventory/OtherOut/index.vue'] = resolve => require(['@/views/inventory/OtherOut/index'], resolve);
routerMap['/views/inventory/OverflowPut/index.vue'] = resolve => require(['@/views/inventory/OverflowPut/index'], resolve);
routerMap['/views/inventory/OverflowOrder/index.vue'] = resolve => require(['@/views/inventory/OverflowOrder/index'], resolve);
routerMap['/views/inventory/InventoryCount/index.vue'] = resolve => require(['@/views/inventory/InventoryCount/index'], resolve);
routerMap['/views/inventory/Inventorycheck/index.vue'] = resolve => require(['@/views/inventory/Inventorycheck/index'], resolve);
routerMap['/views/inventory/InventorycheckList/index.vue'] = resolve => require(['@/views/inventory/InventorycheckList/index'], resolve);

routerMap['/views/inventory/InventoryLoss/index.vue'] = resolve => require(['@/views/inventory/InventoryLoss/index'], resolve);
routerMap['/views/inventory/InventoryLossSlip/index.vue'] = resolve => require(['@/views/inventory/InventoryLossSlip/index'], resolve);
routerMap['/views/inventory/InventoryReport/index.vue'] = resolve => require(['@/views/inventory/InventoryReport/index'], resolve);

routerMap['/views/Sales/Salesorders/index.vue'] = resolve => require(['@/views/Sales/Salesorders/index'], resolve);
routerMap['/views/Sales/Salesorders_/index.vue'] = resolve => require(['@/views/Sales/Salesorders_/index'], resolve);
routerMap['/views/Sales/SalesordersList/index.vue'] = resolve => require(['@/views/Sales/SalesordersList/index'], resolve);
routerMap['/views/Sales/SalesordersList_/index.vue'] = resolve => require(['@/views/Sales/SalesordersList_/index'], resolve);
routerMap['/views/Sales/SalesordersListPlus/index.vue'] = resolve => require(['@/views/Sales/SalesordersListPlus/index'], resolve);

routerMap['/views/inventory/SalesReceipt/index.vue'] = resolve => require(['@/views/inventory/SalesReceipt/index'], resolve);
routerMap['/views/inventory/SalesReceipt1/index.vue'] = resolve => require(['@/views/inventory/SalesReceipt1/index'], resolve);
routerMap['/views/inventory/SalesOut/index.vue'] = resolve => require(['@/views/inventory/SalesOut/index'], resolve);
routerMap['/views/inventory/SalesOut1/index.vue'] = resolve => require(['@/views/inventory/SalesOut1/index'], resolve);

routerMap['/views/ReserveAdmin/connection/index.vue'] = resolve => require(['@/views/ReserveAdmin/connection/index'], resolve);

routerMap['/views/inventory/Purchasing/index.vue'] = resolve => require(['@/views/inventory/Purchasing/index'], resolve);
routerMap['/views/inventory/PurchaseOrder/index.vue'] = resolve => require(['@/views/inventory/PurchaseOrder/index'], resolve);

routerMap['/views/Purchase/Purchasingorder/index.vue'] = resolve => require(['@/views/Purchase/Purchasingorder/index'], resolve);
routerMap['/views/Purchase/PurchasingorderList/index.vue'] = resolve => require(['@/views/Purchase/PurchasingorderList/index'], resolve);
routerMap['/views/Purchase/PurchaseListPlus/index.vue'] = resolve => require(['@/views/Purchase/PurchaseListPlus/index'], resolve);

routerMap['/views/Purchase/ProduceOrder/index.vue'] = resolve => require(['@/views/Purchase/ProduceOrder/index'], resolve);

routerMap['/views/ReserveAdmin/Supplier/index.vue'] = resolve => require(['@/views/ReserveAdmin/Supplier/index'], resolve);
routerMap['/views/ReserveAdmin/EnvelopeType/index.vue'] = resolve => require(['@/views/ReserveAdmin/EnvelopeType/index'], resolve);

routerMap['/views/Product/ProductBom/index.vue'] = resolve => require(['@/views/Product/ProductBom/index'], resolve);
routerMap['/views/Product/ProductBomNo/index.vue'] = resolve => require(['@/views/Product/ProductBomNo/index'], resolve);
routerMap['/views/Product/Formula/index.vue'] = resolve => require(['@/views/Product/Formula/index'], resolve);
routerMap['/views/Product/Colorscheme/index.vue'] = resolve => require(['@/views/Product/Colorscheme/index'], resolve);
routerMap['/views/Product/Colorscheme_bak/index.vue'] = resolve => require(['@/views/Product/Colorscheme_bak/index'], resolve);
routerMap['/views/Product/ProductionReport/index.vue'] = resolve => require(['@/views/Product/ProductionReport/index'], resolve);

routerMap['/views/ProductManage/Productorders/index.vue'] = resolve => require(['@/views/ProductManage/Productorders/index'], resolve);
routerMap['/views/ProductManage/ProductordersList/index.vue'] = resolve => require(['@/views/ProductManage/ProductordersList/index'], resolve);
routerMap['/views/ProductManage/MRP/index.vue'] = resolve => require(['@/views/ProductManage/MRP/index'], resolve);
routerMap['/views/ProductManage/MRPNo/index.vue'] = resolve => require(['@/views/ProductManage/MRPNo/index'], resolve);

routerMap['/views/inventory/requisitionOrder/index.vue'] = resolve => require(['@/views/inventory/requisitionOrder/index'], resolve);
routerMap['/views/inventory/requisitionOrderList/index.vue'] = resolve => require(['@/views/inventory/requisitionOrderList/index'], resolve);

routerMap['/views/inventory/ProductionoutOrder/index.vue'] = resolve => require(['@/views/inventory/ProductionoutOrder/index'], resolve);
routerMap['/views/inventory/ProductionoutOrderList/index.vue'] = resolve => require(['@/views/inventory/ProductionoutOrderList/index'], resolve);

routerMap['/views/inventory/ProductionReceipt/index.vue'] = resolve => require(['@/views/inventory/ProductionReceipt/index'], resolve);
routerMap['/views/inventory/ProductionPut/index.vue'] = resolve => require(['@/views/inventory/ProductionPut/index'], resolve);
// 导入开发组件结束


const actions = {
  clearRole({ commit }) {
    //清空权限
    commit('CLEARROLE')
  },
  generateRoutes({ commit }, roles) {
    return new Promise(resolve => {
      let accessedRoutes
      accessedRoutes = asyncRoutes || []
      const userPermission = GetUserPermission()['result']
      const cloneRoute = GetUserPermission()['cloneRoute']
      // let userPermission = []
      accessedRoutes.forEach((v) => {
        cloneRoute.push(v)
      })
      userPermission.forEach((v) => {
        accessedRoutes.push(v)
      })

      commit('SET_ROUTES', accessedRoutes)
      resolve({ accessedRoutes, cloneRoute })
    })
  }
}

function GetUserPermission() {
  const result = []
  for (let i = 0; i < store.getters.permission_user.length; i++) {
    const currMenu = GetMenu(store.getters.permission_user[i])
    result.push(currMenu)
  }
  var cloneRoute = [...result]
  return { result, cloneRoute }
}

function GetMenu(userPermise) {
  var result = {
    path: userPermise.path,
    // component: routerMap[userPermise.component],
    component: routerMap[userPermise.component],
    name: userPermise.name,
    meta: userPermise.meta,
    alwaysShow: userPermise.alwaysShow,
    children: null
  }
  if (userPermise.children && userPermise.children.length > 0) {
    GetChildren(result, userPermise.children)
  }
  return result
}

function GetChildren(parent, children) {
  const childList = []
  for (let i = 0; i < children.length; i++) {
    var l = children[i].component.split('/')
    // console.log(l)
    var a = l[2]
    var b = l[3]
    const currchild = {
      path: children[i].path,
      component: routerMap[children[i].component],
      // component: lazyLoading(a,b),//动态加载（打包文件过大会打到同一个文件里面，放弃使用）
      name: children[i].name,
      meta: children[i].meta
      // children: null
    }
    if (children[i].children && children[i].children.length > 0) {
      GetChildren(currchild, children[i].children)
    }
    childList.push(currchild)
  }
  parent.children = childList
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
