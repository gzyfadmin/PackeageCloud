import request from '@/utils/request'
import RequestObject from "@/utils/requestObject";

export function login(data) {
  return request({
    url: '/systemlogin/api/login',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  // return request({
  //   url: '/systemlogin/api/login',
  //   method: 'get',
  //   params: { token }
  // })
  return request({
    url: '/systemlogin/api/Login/GetInfo',
    method: 'get'
  })
}

export function getButtons() {
  return request({
    url: '/platform/api/tbuttons',
    method: 'get',
    params: {
      requestObject: JSON.stringify(RequestObject.CreateGetObject())
    }
  })
}

export function logout() {
  return request({
    url: '/systemlogin/api/logout',
    method: 'post'
  })
}
export function getMsg() {
  //获取列表
  var list = [];
  list.push({
    column: "isRead",
    content: 0,
    condition: 0
  });
  var rqs = RequestObject.CreateGetObject(
    false,
    0,
    0,
    list
  );
  return request({
    url: "/report/api/StaOnlineTime/GetToDoModel",
    method: "GET",
    params: { RequestObject: JSON.stringify(rqs) }
  })
}
