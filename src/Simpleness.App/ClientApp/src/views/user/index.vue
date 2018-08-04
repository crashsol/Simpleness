<template>
    <div class="app-container">
        <el-form :inline="true" :model="queryParams">
            <el-form-item label="用户名">
                <el-input v-model="queryParams.name" placeholder="用户名"></el-input>
            </el-form-item>
            <el-form-item label="电子邮箱">
                <el-input v-model="queryParams.email" placeholder="电子邮箱"></el-input>
            </el-form-item>
            <el-form-item label="手机号">
                <el-input v-model="queryParams.phone" placeholder="手机号"></el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="onSubmit">查询</el-button>
            </el-form-item>
        </el-form>

        <el-table :data="users" border=''>
            <el-table-column prop="userName" label="用户名">
            </el-table-column>
            <el-table-column prop="nickName" label="昵称">
            </el-table-column>
            <el-table-column prop="email" label="电子邮箱">
            </el-table-column>
            <el-table-column prop="phone" label="联系方式">
            </el-table-column>
             <el-table-column prop="lockoutEnd" label="账号状态">
               <template slot-scope="scope">
                  <el-tag type="scope.row.lockoutEnd? 'danger':'success'">{{scope.row.lockoutEnd ?'锁定':'正常'}}</el-tag>   
                  <template v-if="scope.row.lockoutEnd">
                    <i class="el-icon-time"></i>                    
                  </template>            
               </template>
            </el-table-column>           
            <el-table-column label="操作" width="300">
                <template slot-scope="scope">
                    <el-button @click="handleEdit(scope.row.id)" type="success" size="mini">编辑</el-button>
                    <el-button type="warning" @click="handleLocked(scope.row.id)" size="mini">{{scope.row.lockoutEnabled?'解锁':'锁定'}}</el-button>
                    <el-button type="danger" @click="handleDelete(scope.row)" size="mini">删除</el-button>
                </template>
            </el-table-column>
        </el-table>

    </div>

</template>

<script>
import { getUsers } from '../../api/user.js'
export default {
  data() {
    return {
      users: [],
      queryParams: {
        name: '',
        phone: '',
        email: ''
      }
    }
  },
  methods: {
    /* 查询提交 */
    onSubmit() {},
    /* 编辑 */
    handleEdit(id) {
      this.$message({
        message: id,
        type: 'warning'
      })
    },
    /* 删除 */
    handleDelete(row) {
      var tempMsg = '确定删除 ' + row.userName + ' 用户'
      this.$confirm(tempMsg, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$message({
          type: 'success',
          message: '删除成功!'
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消删除'
        })
      })
    },
    /* 锁定/解锁 */
    handleLocked(id) {

    }
  },
  mounted() {
    getUsers().then(result => {
      this.users = result
    })
  }
}
</script>

<style>
</style>
