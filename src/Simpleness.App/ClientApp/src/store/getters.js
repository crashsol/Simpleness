const getters = {
  sidebar: state => state.app.sidebar,
  device: state => state.app.device,
  token: state => state.user.token,
  avatar: state => state.user.avatar,
  name: state => state.user.name,
  /* 选择语言 */
  language: state => state.app.language,
  /* 获取用户权限列表 */
  permissions: state => state.user.permissions,
  /* 获取动态路由信息 */
  addRouters: state => state.permission.addRouters,
  /* 获取所有的路由信息 */
  allRouters: state => state.permission.routers,
  /* 访问过的标签组 */
  visitedViews: state => state.tagsView.visitedViews,
  /* 缓存过过得标签组  */
  cachedViews: state => state.tagsView.cachedViews

}
export default getters
