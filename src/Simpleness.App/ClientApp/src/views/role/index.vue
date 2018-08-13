<template>
  <div class="app-container">
    <el-row>
      <el-col :span="12">
        <el-button type="primary" @click="handleCreate">创建角色</el-button>
      </el-col>
    </el-row>
    <el-table :data="roles">
      <el-table-column prop="name" label="角色名称">
      </el-table-column>
      <el-table-column prop="description" label="角色描述">
      </el-table-column>
      <el-table-column label="操作" width="400px">
        <template slot-scope="scope">
          <el-button type="infor" @click="handleUpdate(scope.row)" size="mini">编辑</el-button>
          <el-button type="danger" @click="handleDelete(scope.row)" size="mini">删除</el-button>
          <el-button type="primary" @click="handleMember(scope.row)" size="mini">成员</el-button>
          <el-button type="success" @click="handlePermission(scope.row)" size="mini">授权</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" width="60%">
      <el-form :rules="formRules" ref="roleForm" :model="form" label-width="80px">
        <el-input v-model="form.id" type="hidden"></el-input>
        <el-form-item label="角色名称" prop="name">
          <el-input v-model="form.name"></el-input>
        </el-form-item>
        <el-form-item label="角色描述" prop="description">
          <el-input v-model="form.description" type="textarea" rows="5"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer">
        <el-button @click="dialogFormVisible = false">{{$t('table.cancel')}}</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create">{{$t('table.confirm')}}</el-button>
        <el-button v-else type="primary" @click="update">{{$t('table.confirm')}}</el-button>
      </span>
    </el-dialog>

    <!-- 角色权限 -->
    <el-dialog title="设置角色权限" :visible.sync="permissionStatus" width="40%">
      <el-tree :data="permissionModel.permissionData" show-checkbox  default-expand-all node-key="id" ref='permissionTree' >
      </el-tree>
      <span slot="footer">
        <el-button @click="permissionStatus = false">取 消</el-button>
        <el-button type="primary" @click="permission">确 定</el-button>
      </span>
    </el-dialog>

    <!-- 角色成员 -->
    <el-dialog title="memberModel.title" :visible.sync="memberStatus" width="40%">
      <div style="text-align: center">
        <el-transfer style="text-align: left; display: inline-block" v-model="memberModel.userIds" :data="memberModel.items" filterable :titles="['未选择用户', '已选择用户']">
        </el-transfer>
      </div>
      <span slot="footer">
        <el-button @click="memberStatus = false">取 消</el-button>
        <el-button type="primary" @click="member">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import {
  roleList,
  roleCreate,
  roleUpdate,
  roleDelete,
  getRoleUsers,
  updateRoleUsers,
  getRolePermissions,
  updateRolePermissions
} from '../../api/role.js'
export default {
  data() {
    return {
      roles: [],
      form: {
        id: undefined,
        name: '',
        description: ''
      },
      formRules: {
        name: [
          { required: true, trigger: 'blur', message: '请输入角色名称' },
          {
            message: '最大长度80个字符',
            trigger: blur,
            max: 80
          }
        ],
        description: [
          { message: '最大长度255个字符', trigger: 'blur', max: 255 }
        ]
      },
      dialogStatus: '',
      dialogFormVisible: false,
      textMap: {
        create: '创建角色',
        update: '更新角色'
      },
      permissionStatus: false,
      permissionModel: {
        id: undefined,
        title: '',
        permissionData: [],
        permissions: []
      },
      memberStatus: false,
      memberModel: {
        id: undefined,
        title: '',
        userIds: [],
        items: []
      }
    }
  },
  methods: {
    async getRoles() {
      const result = await roleList()
      this.roles = result
    },
    // 清空表单
    resetForm() {
      this.form = {
        id: null,
        name: '',
        description: ''
      }
    },
    handleCreate() {
      this.resetForm()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['roleForm'].clearValidate()
      })
    },
    // 创建角色
    create() {
      this.$refs['roleForm'].validate(async valid => {
        if (valid) {
          const id = await roleCreate(
            this.form.name,
            this.form.name.description
          )
          this.form.id = id
          this.roles.unshift(this.form)
          this.dialogFormVisible = false
          this.$message({
            message: '创建成功',
            type: 'success'
          })
        }
      })
    },
    handleDelete(row) {
      var tempMsg = '确定删除 ' + row.name + ' 角色'
      this.$confirm(tempMsg, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await roleDelete(row.id)
          const index = this.roles.indexOf(row)
          this.roles.splice(index, 1)
          this.$message({
            message: '删除成功',
            type: 'success'
          })
        })
        .catch(() => {
          this.$message({
            message: '取消删除',
            type: 'warning'
          })
        })
    },
    // 更新信息
    handleUpdate(row) {
      this.form = { ...row } // row copy
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['roleForm'].clearValidate()
      })
    },
    update() {
      this.$refs['roleForm'].validate(async valid => {
        const { id, name, description } = this.form
        await roleUpdate(id, name, description)
        for (const v of this.roles) {
          if (v.id === this.form.id) {
            const index = this.roles.indexOf(v)
            this.roles.splice(index, 1, this.form)
            break
          }
        }
        this.dialogFormVisible = false
        this.$message({
          message: '更新成功',
          type: 'success'
        })
      })
    },
    // 获取角色成员
    async handleMember(row) {
      const result = await getRoleUsers(row.id)
      this.memberModel.id = row.id
      this.memberModel.title = `设置 ${row.name} 角色成员`
      this.memberModel.userIds = result.selectItems
      this.memberModel.items = result.items
      this.memberStatus = true
    },
    // 更新角色成员
    member() {
      this.$confirm(this.memberModel.title, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await updateRoleUsers(this.memberModel.id, this.memberModel.userIds)
          this.memberStatus = false
          this.$message({
            message: '设置成功',
            type: 'success'
          })
        })
        .catch(() => {
          this.$message({
            message: '取消设置',
            type: 'warning'
          })
        })
    },
    // 获取角色权限
    async handlePermission(row) {
      this.permissionModel.id = row.id
      this.permissionModel.title = `设置 ${row.name} 角色权限`
      const result = await getRolePermissions(row.id)
      this.permissionModel.permissionData = [result.tree]
      this.permissionStatus = true
      this.$nextTick(() => {
        // 设置勾选当前所有权限
        this.$refs['permissionTree'].setCheckedKeys(result.selectKeys)
      })
    },
    // 更新角色授权
    permission() {
      this.$confirm(this.permissionModel.title, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          const permissions = this.$refs['permissionTree'].getCheckedKeys()
          await updateRolePermissions(this.permissionModel.id, permissions)
          this.permissionStatus = false
          this.$message({
            message: '设置成功',
            type: 'success'
          })
        })
        .catch(() => {
          this.$message({
            message: '取消设置',
            type: 'warning'
          })
        })
    }
  },
  mounted() {
    this.getRoles()
  }
}
</script>

<style>
</style>
