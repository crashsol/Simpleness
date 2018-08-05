import request from '@/utils/request'
/* 获取用户列表API */
export function getUsers() {
  return request({
    url: '/api/user/list',
    method: 'get'
  })
}
