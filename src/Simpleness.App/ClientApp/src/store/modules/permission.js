/*
  权限状态管理
  根据用户权限列表和动态路由配置，判断用户具有的访问URL
*/
/* 导入路由器信息 */
import { asyncRouterMap, constantRouterMap } from '../../router/index'

/*
    通过 route.meta.permission 判断当前用户是否具有该路由权限
    permissions: 当前用户具备的所有权限列表
    route: 路由信息
*/
function hasPermission(permissions, route) {
  if (route.meta && route.meta.permission) {
    return permissions.indexOf(route.meta.permission) > -1
  } else {
    return true
  }
}

/*
    根据用户具备的权限，返回系统菜单
    递归调用
*/
function filterAsyncRouter(asyncRouterMap, permissions) {
  const accessRouters = asyncRouterMap.filter(route => {
    if (hasPermission(permissions, route)) {
      /* 检查下级路由 */
      if (route.children && route.children.length) {
        route.children = filterAsyncRouter(route.children, permissions)
      }
      return true
    }
    return false
  })
  return accessRouters
}

/* permission 状态管理器 */

const permission = {
  state: {
    /* 静态路由，（后续会加上addRouters ） */
    routers: constantRouterMap,
    /* 动态添加的路由 */
    addRouters: []
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      /* 设置动态路由 */
      state.addRouters = routers
      /* 组合成最终的路由信息 */
      state.routers = constantRouterMap.concat(routers)
    }
  },
  actions: {
    GenerateRoutes({ commit, rootState }) {
      // 从根状态组件 rootState  获取user 状态下的permission
      var permissions = rootState.user.permissions
      console.log(permissions)
      return new Promise(resolve => {
        const accessedRouters = filterAsyncRouter(asyncRouterMap, permissions)
        commit('SET_ROUTERS', accessedRouters)
        resolve()
      })
    }
  }
}
export default permission
