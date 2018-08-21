import request from '@/utils/request'

export function login(username, password) {
  return request({
    url: '/api/account/login',
    method: 'post',
    data: {
      username,
      password
    }
  })
}
/* 新用户注册 */
export function register(email, password) {
  return request({
    url: '/api/account/register',
    method: 'post',
    data: {
      email,
      password
    }
  })
}
/* 忘记密码，发送邮件 */
export function forgotPwd(email) {
  return request({
    url: 'api/account/forgotpwd/' + email,
    method: 'post'
  })
}
/* 使用code 重新设置密码 */
export function resetPwd(email, password, code) {
  return request({
    url: 'api/account/ResetPassword',
    method: 'post',
    data: {
      email,
      password,
      code
    }
  })
}

/* 验证邮箱地址 */
export function comfirmedEmail(id, code) {
  return request({
    url: `api/account/ConfirmEmail?userId= ${id}&code=${code}`,
    method: 'post'
  })
}
