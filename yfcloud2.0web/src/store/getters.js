const getters = {
  sidebar: state => state.app.sidebar,
  size: state => state.app.size,
  device: state => state.app.device,
  visitedViews: state => state.tagsView.visitedViews,
  cachedViews: state => state.tagsView.cachedViews,
  token: state => state.user.token,
  avatar: state => state.user.avatar,
  name: state => state.user.name,
  introduction: state => state.user.introduction,
  roles: state => state.user.roles,
  permission_routes: state => state.permission.routes,
  errorLogs: state => state.errorLog.logs,
  permission_user: state => state.user.permission,
  userPermission: state => state.user.userPermission,//按钮权限
  userButtons: state => state.user.userButtons,//全部按钮
  addRoutes: state => state.permission.addRoutes,//全部按钮
}
export default getters
