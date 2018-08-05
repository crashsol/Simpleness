import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import store from '../store'
import { getToken } from '@/utils/auth'

// 创建axios实例
const service = axios.create({
  baseURL: process.env.BASE_API, // api的base_url
  timeout: 5000 // 请求超时时间
})

// request拦截器
service.interceptors.request.use(config => {
  if (store.getters.token) {
    config.headers['Authorization'] = 'Bearer ' + getToken()
    // config.headers['X-Token'] = getToken() // 让每个请求携带自定义token 请根据实际情况自行修改
  }
  return config
}, error => {
  // Do something with request error
  console.log(error) // for debug
  Promise.reject(error)
})

// respone拦截器
service.interceptors.response.use(
  response => {
    console.log('请求成功：' + response.data)
    return response.data
  },
  error => {
    if (error.response) {
      var res = error.response
      // 400 404 ,500错误
      if (res.status === 400) {
        Message({
          message: res.data,
          type: 'error',
          duration: 5 * 3000
        })
      }

      if (res.status === 404) {
        Message({
          message: '访问地址不存在',
          type: 'error',
          duration: 5 * 3000
        })
      }
      if (res.status === 500) {
        Message({
          message: '服务器错误',
          type: 'error',
          duration: 5 * 3000
        })
      }
      // 未授权
      if (res.status === 401) {
        MessageBox.confirm('你已被登出，请重新登录', '确定登出', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('FedLogOut').then(() => {
            location.reload()// 为了重新实例化vue-router对象 避免bug
          })
        })
      }
      if (res.status === 403) {
        MessageBox.confirm('权限不足，请更换账号重新登录或者联系管理员', '确定登出', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('FedLogOut').then(() => {
            location.reload()// 为了重新实例化vue-router对象 避免bug
          })
        })
      }
    } else {
      Message({
        message: error.message || '网络异常',
        type: 'error',
        duration: 5 * 3000
      })
    }
    return Promise.reject(error)
  }
)

export default service
