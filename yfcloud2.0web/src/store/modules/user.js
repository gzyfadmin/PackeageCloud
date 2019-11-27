import { login, logout, getInfo, getButtons, getMsg } from "@/api/user";
import { getToken, setToken, removeToken } from "@/utils/auth";
import router, { resetRouter } from "@/router";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";

const state = {
  token: getToken(),
  name: "",
  avatar: "",
  imgUrlName: "",
  introduction: "",
  roles: [],
  permission: [],
  userPermission: [],
  userButtons: [],
  companyData: {},
  MsgNum: 0,
  ajaxUrl: "",
  msgData: [],
};

const mutations = {
  GETMSG: (state, data) => {
    if(data.length>0){
      data.reverse()
      state.msgData = data.slice(0, 3);
    }else{
      state.msgData=[];
    }
    
  },
  setAjaxUrl: (state, ajaxUrl) => {
    state.ajaxUrl = ajaxUrl;
  },
  setImgUrlName: (state, imgUrlName) => {
    state.imgUrlName = imgUrlName;
  },
  SET_TOKEN: (state, token) => {
    state.token = token;
  },
  SET_INTRODUCTION: (state, introduction) => {
    state.introduction = introduction;
  },
  SETMSGNUM: (state, len) => {
    state.MsgNum = len;
  },
  SET_NAME: (state, name) => {
    state.name = name;
  },
  SET_AVATAR: (state, avatar) => {
    state.avatar = avatar;
  },
  SET_ROLES: (state, roles) => {
    state.roles = roles;
  },
  SET_PERMISSION: (state, permission) => {
    state.permission = permission;
  },
  SETUSERPERMISSION: (state, userPermission) => {
    state.userPermission = userPermission;
  },
  SETBUTTONS: (state, btns) => {
    state.userButtons = btns;
  },
  GETCOMPANYDATA: (state, data) => {
    state.companyData = data;
  },
  SET_LOGNAME: (state, str) => {
    state.companyData.logName = str;
  },
  SET_LOGNAMEENGLISH: (state, str) => {
    state.companyData.logNameEngliash = str;
  },
  SET_LOGIMG: (state, str) => {
    state.companyData.logimg = str
  },
  CLEARROLE: (state, str) => {
    //清空权限
    state.name = "";
    state.avatar = "";
    state.introduction = "";
    state.roles = [];
    state.permission = [];
    state.userPermission = [];
    state.userButtons = [];
    state.companyData = {};
  },
};

const actions = {
  getMsg({ commit }) {
    getMsg().then(res => {
      if (res != "") {
        commit("SETMSGNUM", res.data.length);
        // this.MsgData = res.data.slice(0, 3);
        commit("GETMSG", res.data);
      }
    });

  },
  setAvatar({ commit }, avatar) {
    commit("SET_AVATAR", avatar);
  },
  setMsgNum({ commit }, len) {
    commit("SETMSGNUM", len);
  },
  clearRole({ commit }) {
    //清空权限
    commit('CLEARROLE')
  },
  setlogName({ commit }, n) {
    commit('SET_LOGNAME', n)
  },
  setlogNameEnglish({ commit }, n) {
    commit('SET_LOGNAMEENGLISH', n)
  },
  setlog({ commit }, n) {
    commit('SET_LOGIMG', n)
  },
  setCompanyData({ commit }, n) {
    commit('GETCOMPANYDATA', n)
  },
  setUserPermission({ commit }, userInfo) {
    getInfo().then(response => {
      commit("SETUSERPERMISSION", response.data.permissions);
    });
  },
  // user login
  login({ commit }, userInfo) {
    // const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      login(userInfo)
        .then(response => {
          if (response.code === -1) {
            reject("用户名或密码错误!");
            return;
          }
          const { data } = response;
          commit("SET_TOKEN", data.token);
          setToken(data.token);
          resolve();
        })
        .catch(error => {
          reject(error);
        });
    });
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo()
        .then(response => {
          if (response.code === 0) {
            let data = {
              roles: [response.data.roles],
              introduction: response.data.introduction,
              avatar: response.data.avatar,
              name: response.data.name,
              permission: response.data.permissions,
              logimg: response.data.tenantLogo == null ? "" : state.imgUrlName + response.data.tenantLogo,
              logName: response.data.companyName,
              logNameEngliash: response.data.tenantEngName
            };
            const { roles, name, avatar, introduction, permission, logimg, logName, logNameEngliash } = data;
            // console.log(logimg)
            // console.log(logName)

            // roles must be a non-empty array
            if (!roles || roles.length <= 0) {
              reject("getInfo: roles must be a non-null array!");
            }

            commit("GETCOMPANYDATA", { logimg, logName, logNameEngliash });
            commit("SET_ROLES", roles);
            commit("SET_NAME", name);
            commit("SET_AVATAR", avatar);
            commit("SET_INTRODUCTION", introduction);
            commit("SET_PERMISSION", permission);
            commit("SETUSERPERMISSION", response.data.permissions);
            resolve(data);
          } else {
            reject("获取权限信息失败!");
            return;
          }
        })
        .catch(error => {
          reject(error);
        });
    });
  },

  // user logout
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      logout(state.token)
        .then(() => {
          commit("SET_TOKEN", "");
          commit("SET_ROLES", []);
          removeToken();
          resetRouter();
          resolve();
        })
        .catch(error => {
          reject(error);
        });
    });
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      commit("SET_TOKEN", "");
      commit("SET_ROLES", []);
      removeToken();
      resolve();
    });
  },

  // dynamically modify permissions
  changeRoles({ commit, dispatch }, role) {
    return new Promise(async resolve => {
      const token = role + "-token";

      commit("SET_TOKEN", token);
      setToken(token);

      const { roles } = await dispatch("getInfo");

      resetRouter();

      // generate accessible routes map based on roles
      const accessRoutes = await dispatch("permission/generateRoutes", roles, {
        root: true
      });

      // dynamically add accessible routes
      router.addRoutes(accessRoutes);

      // reset visited views and cached views
      dispatch("tagsView/delAllViews", null, { root: true });

      resolve();
    });
  },
  setButtons({ commit, state }) {
    return new Promise((resolve, reject) => {
      getButtons().then(res => {
        commit("SETBUTTONS", res.data);
        resolve(res.data);
      });
    }).catch(error => {
      console.log(error);
    });
  }
};

export default {
  namespaced: true,
  state,
  mutations,
  actions
};
