'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

//定义API请求地址
module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  BASE_API: '"http://localhost:5000"',
})
