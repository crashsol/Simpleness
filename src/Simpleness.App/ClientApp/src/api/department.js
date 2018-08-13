import request from '@/utils/request'

/* 获取全部部门信息 */
export function departList() {
  return request({
    url: 'api/department/list',
    method: 'get'
  })
}

/* 获取创建部门 */
export function departCreate(name, description, order, pid) {
  return request({
    url: 'api/department/create',
    method: 'post',
    data: {
      name,
      description,
      order,
      pid
    }
  })
}

/* 更新部门信息 */
export function departUpdate(id, name, description, order, pid) {
  return request({
    url: 'api/department/update',
    method: 'post',
    data: {
      id,
      name,
      description,
      order,
      pid
    }
  })
}
/* 删除部门 */
export function departDelete(id) {
  return request({
    url: 'api/department/delete/' + id,
    method: 'post'
  })
}

/* 获取部门成员 */
export function getDepartUsers(id) {
  return request({
    url: 'api/department/users/' + id,
    method: 'get'
  })
}
/* 获取部门成员 */
export function updateDepartUsers(id, userIds) {
  return request({
    url: 'api/department/users',
    method: 'post',
    data: {
      id,
      userIds
    }
  })
}

