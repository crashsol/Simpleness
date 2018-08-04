import { login, logout } from '@/api/login'
import { getToken, setToken, removeToken } from '@/utils/auth'
import jwt from 'jsonwebtoken'
/* 用户状态管理器 */
const user = {
  state: {
    token: getToken(),
    userId: '',
    name: '',
    avatar: '',
    permissions: []
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_PERMISSION: (state, permissions) => {
      state.permissions = [...permissions]
    },
    SET_USERID: (state, userId) => {
      state.userId = userId
    }
  },

  actions: {
    // 登录
    Login({ commit }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        login(username, userInfo.password).then(data => {
          // 登录成功后，获取token,并加cookies保存起来
          setToken(data.token)
          commit('SET_TOKEN', data.token)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve) => {
        // 登录成功后，获取token,并加cookies保存起来
        var data = getToken()
        // 如果token为空
        if (data) {
          var decodeData = jwt.decode(data)
          console.log(decodeData)
          commit('SET_USERID', decodeData.sub)
          commit('SET_NAME', decodeData.name)
          commit('SET_AVATAR', decodeData.avatar)
          commit('SET_PERMISSION', decodeData.permission)
          resolve()
        }
      })
    },

    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        logout(state.token).then(() => {
          commit('SET_TOKEN', '')
          commit('SET_PERMISSION', [])
          removeToken()
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        removeToken()
        resolve()
      })
    }
  }
}

export default user
