import request from '@/utils/request'

export function roleList(pagesize, currentpage) {
  return request({
    url: `/api/role/list/${pagesize}/${currentpage}`,
    method: 'get'
  })
}
/* 创建角色 */
export function roleCreate(name, description) {
  return request({
    url: 'api/role/create',
    method: 'post',
    data: {
      name,
      description
    }
  })
}
/* 角色更新 */
export function roleUpdate(id, name, description) {
  return request({
    url: 'api/role/update',
    method: 'post',
    data: {
      id,
      name,
      description
    }
  })
}
/* 删除角色 */
export function roleDelete(id) {
  return request({
    url: 'api/role/delete/' + id,
    method: 'post'
  })
}
/* 获取角色所有成员 */
export function getRoleUsers(id) {
  return request({
    url: 'api/role/users/' + id,
    method: 'get'
  })
}

/* 更新角色所有成员 */
export function updateRoleUsers(id, userids) {
  return request({
    url: 'api/role/users',
    method: 'post',
    data: {
      id,
      userids
    }
  })
}
/* 获取所有角色所有权限 */
export function getRolePermissions(id) {
  return request({
    url: 'api/role/permission/' + id,
    method: 'get'
  })
}

/* 更新角色权限 */
export function updateRolePermissions(id, permissions) {
  return request({
    url: 'api/role/permission',
    method: 'post',
    data: {
      id,
      permissions
    }
  })
}
