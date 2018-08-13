import Vue from 'vue'
import Router from 'vue-router'

// in development-env not use lazy-loading, because lazy-loading too many pages will cause webpack hot update too slow. so only in production use lazy-loading;
// detail: https://panjiachen.github.io/vue-element-admin-site/#/lazy-loading

Vue.use(Router)

/* Layout */
import Layout from '../views/layout/Layout'

/**
* hidden: true                   if `hidden:true` will not show in the sidebar(default is false)       //是否显示在侧边栏
* alwaysShow: true               if set true, will always show the root menu, whatever its child routes length  //始终显示，不管是否具有子路由
*                                if not set alwaysShow, only more than one route under the children
*                                it will becomes nested mode, otherwise not show the root menu
* redirect: noredirect           if `redirect:noredirect` will no redirct in the breadcrumb            // breadcrumb 不具有跳转效果
* name:'router-name'             the name is used by <keep-alive> (must set!!!)                        //缓存的名称，必须设置
* meta : {
    title: 'title'               the name show in submenu and breadcrumb (recommend set)  //显示的名称
    icon: 'svg-name'             the icon show in the sidebar,                            //侧边栏图标
  }
**/
/* 系统权限，不要进进行权限验证的 */
export const constantRouterMap = [
  { path: '/login', component: () => import('@/views/login/index'), hidden: true },
  { path: '/404', component: () => import('@/views/errorPages/404'), hidden: true },
  {
    path: '',
    component: Layout,
    redirect: '/dashboard',
    hidden: true,
    children: [{
      path: 'dashboard',
      component: () => import('@/views/dashboard/index'),
      name: 'dashboard',
      meta: { title: 'dashboard', icon: 'dashboard', noCache: true }
    }]
  },
  {
    path: '/account',
    component: Layout,
    hidden: true,
    children: [{
      path: 'userinfo',
      name: 'userinfo',
      component: () => import('@/views/login/userinfo'),
      meta: { title: 'userinfo', icon: 'user' }
    },
    {
      path: 'changepwd',
      name: 'changepwd',
      component: () => import('@/views/login/changepwd'),
      meta: { title: 'changepwd', icon: 'user' }
    }]
  }

]

/* 定义需要验证权限的路由信息 */
export const asyncRouterMap = [
  {
    path: '/admin',
    component: Layout,
    name: 'admin',
    redirect: 'noredirect',
    alwaysShow: true,
    meta: { title: 'system', icon: 'example' },
    children: [
      {
        path: 'user',
        name: 'user',
        component: () => import('@/views/user/index'),
        meta: { title: 'users', icon: 'user', permission: 'Users' }
      },
      {
        path: 'role',
        name: 'role',
        component: () => import('@/views/role/index'),
        meta: { title: 'roles', icon: 'role', permission: 'Roles' }
      },
      {
        path: 'departments',
        name: 'departments',
        component: () => import('@/views/department/index'),
        meta: { title: 'departments', icon: 'department', permission: 'Departments' }
      }
    ]
  },
  {
    path: '/form',
    component: Layout,
    children: [
      {
        path: 'index',
        name: 'Form',
        component: () => import('@/views/form/index'),
        meta: { title: 'Form', icon: 'form' }
      }
    ]
  },
  /* 404 */
  { path: '*', redirect: '/404', hidden: true }
]

export default new Router({
  mode: 'history', // 后端支持可开
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})

