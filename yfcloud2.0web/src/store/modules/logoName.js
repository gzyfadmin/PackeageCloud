import serverConfig from '@/serverConfig'
const state = {
  logimg: serverConfig.imgUrlName+'20190802/c9602b085d354f73a74582eee9f8adea.png',
  logName: '云飞科技',
  avatar: ''
}
const mutations = {
  setLogImg: (state, n) => {
    state.logimg = n
  },
  setLogName: (state, n) => {
    state.logName = n
  }
}
const actions = {
  getInfo({ commit, state }) {
    // return new Promise((resolve,reject)=>{
    //   this.getInfo().then(response=>{
    //     console.log(response)
    //   })
    // })
  },
  // getInfo().then(response => {
  //   commit('SETUSERPERMISSION', response.data.permissions)
  // })
  setLogImg({ commit }, n) {
    commit('setLogImg', n)
  },
  setLogName({ commit }, n) {
    commit('setLogName', n)
  }
}
export default {
  namespaced: true,
  state,
  mutations,
  actions
}
