import Vue from 'vue'
import axios from 'axios'

import Cookies from 'js-cookie'

import 'normalize.css/normalize.css' // a modern alternative to CSS resets

import Element from 'element-ui'
import './styles/element-variables.scss'
import './styles/element-ui.scss'

import '@/styles/index.scss' // global css

import App from './App'
import store from './store'
import router from './router'

import './icons' // icon
import './permission' // permission control
import './utils/error-log' // error log

import { Heartbeat } from '@/utils/Heartbeat'// 心跳

import VueCropper from 'vue-cropper'
Vue.use(VueCropper)

import * as filters from './filters' // global filters
import '@/utils/directive.js'

import FullCalendar from 'vue-full-calendar'
Vue.use(FullCalendar)
import serverConfig from '@/serverConfig'
import { btoa, atob } from '@/utils/encrypt'

import Print from '@/utils/print'
Vue.use(Print) // 注册
// import Print from 'vue-print-nb'
// Vue.use(Print);

// import htmlToPdf from '@/utils/htmlToPdf' // 路径仅为示例
// Vue.use(htmlToPdf)


// Vue.prototype.imgUrlName = "http://47.107.135.203:20200/"
/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online! ! !
 */
import { mockXHR } from '../mock'
if (process.env.NODE_ENV === 'production') {
  mockXHR()
}

Vue.use(Element, {
  size: Cookies.get('size') || 'small' // set element-ui default size
})

// register global utility filters
Object.keys(filters).forEach(key => {
  Vue.filter(key, filters[key])
})

Vue.config.productionTip = false
Vue.prototype.tableLen = 15; //设置全局table条数
Vue.prototype.$ajaxUrl = serverConfig.production; //设置全局
Vue.prototype.imgUrlName = serverConfig.imgUrlName; //设置全局
Vue.prototype.domain = serverConfig.domain; //设置全局
Vue.prototype.btoa = btoa;
Vue.prototype.atob = atob;
store.commit('user/setAjaxUrl', serverConfig.production);//存储到vuex中
store.commit('user/setImgUrlName', serverConfig.imgUrlName);//存储到vuex中

Heartbeat()
// function getServerConfig() {
//   return new Promise((resolve, reject) => {
//     axios.get('./serverConfig.json').then((result) => {
//       let config = result.data;

//       let ajaxUrl = process.env.NODE_ENV == 'production' ? config.production : config.develop;
//       Vue.prototype.$ajaxUrl = ajaxUrl; //设置全局
//       store.commit('user/setAjaxUrl', ajaxUrl);//存储到vuex中
//       store.commit('user/setImgUrlName', config.imgUrlName);//存储到vuex中
//       resolve();
//     }).catch((error) => {
//       reject()
//     })
//   })
// }
// async function init() {
// await getServerConfig();
new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})
// }
// init();

