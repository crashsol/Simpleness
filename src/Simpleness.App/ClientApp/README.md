# vueAdmin-template

> 使用使用 vueAdmin-template 模板


- 1、调整 axios后台请求API地址，修改 config 下 dev.env.js、prod.env.js 中的 
```
 BASE_API: '"http://localhost:5000"',
```
- 2、修改后台登录地址 api/login.js 
```
export function login(username, password) {
  return request({
    url: '/admin/account/login',
    method: 'post',
    data: {
      username,
      password
    }
  })
}
```
- 3、添加jwt 包依赖，用户解析后台回传的token
- 4、调整utils/request.js中的 axios 封装
```
...
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
      if (res.status === 400 || res.status === 404 || res.status === 500) {
        Message({
          message: res.data,
          type: 'error',
          duration: 5 * 1000
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
      console.log(error)
      Message({
        message: error.message || '网络异常',
        type: 'error',
        duration: 5 * 1000
      })
    }
    return Promise.reject(error)
  }
)

export default service
```


