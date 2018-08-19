<template>
  <div class="app-container">
    <el-row :gutter="30">
      <el-col :span="14">
        <el-input placeholder="输入关键字进行过滤" v-model="filterText"></el-input>
        <el-tree :data="departTree" default-expand-all node-key="id" style="padding-top:20px" :expand-on-click-node="false" :filter-node-method="filterNode" ref='departmentTree'>
          <span class="custom-tree-node" slot-scope="{ node, data }">
            <span>{{ node.label }}</span>
            <span>
              <el-button type="text" size="mini" @click="() => handleCreate(data)">
                添加
              </el-button>
              <el-button type="text" size="mini" @click="() => handleUpdate(node, data)">
                编辑
              </el-button>
              <el-button type="text" size="mini" @click="() => handleDelete(node, data)">
                删除
              </el-button>
              <el-button type="text" size="mini" @click="() => handleMember(data)">
                部门成员
              </el-button>
            </span>
          </span>
        </el-tree>

      </el-col>
      <el-col :span="10">
        <el-card :body-style="{ padding: '10px' , }">
          <div slot="header">
            <span>{{memberModel.title}}部门成员</span>
            <el-button type="text" style="float: right; padding: 3px 0" @click="member" >更新设置</el-button>
          </div>
          <el-transfer v-model="memberModel.userIds" :data="memberModel.items" filterable :titles="['未选择用户', '已选择用户']">
          </el-transfer>
        </el-card>
      </el-col>
    </el-row>
    <!-- 创建/更新部门 -->
    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" width="60%">
      <el-form :rules="formRules" ref="departmentForm" :model="form" label-width="80px">
        <el-form-item label="部门名称" prop="name">
          <el-input v-model="form.name"></el-input>
        </el-form-item>
        <el-form-item label="部门描述" prop="description">
          <el-input v-model="form.description" type="textarea" rows="5"></el-input>
        </el-form-item>
        <el-form-item label="部门排序" prop="order">
          <el-input v-model="form.order"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer">
        <el-button @click="dialogFormVisible = false">{{$t('table.cancel')}}</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create">{{$t('table.confirm')}}</el-button>
        <el-button v-else type="primary" @click="update">{{$t('table.confirm')}}</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import {
  departCreate,
  departDelete,
  departUpdate,
  updateDepartUsers,
  getDepartUsers,
  departTree
} from '../../api/department.js'
export default {
  data() {
    return {
      departTree: [],
      departments: [],
      filterText: '',
      form: {
        id: undefined,
        name: '',
        description: '',
        order: 99,
        pid: ''
      },
      formRules: {
        name: [
          { required: true, trigger: 'blur', message: '请输入部门名称' },
          {
            message: '最大长度80个字符',
            trigger: blur,
            max: 80
          }
        ],
        description: [
          { message: '最大长度255个字符', trigger: 'blur', max: 255 }
        ],
        pid: [{ required: true, trigger: 'blur', message: '请选择上级部门' }]
      },
      dialogStatus: '',
      dialogFormVisible: false,
      textMap: {
        create: '创建部门',
        update: '更新部门'
      },
      memberModel: {
        id: undefined,
        title: '',
        userIds: [],
        items: []
      },
      currentNode: Object
    }
  },
  watch: {
    filterText(val) {
      this.$refs.departmentTree.filter(val)
    }
  },
  methods: {
    filterNode(value, data) {
      if (!value) return true
      return data.label.indexOf(value) !== -1
    },
    async getDepartmentTree() {
      const data = await departTree()
      this.departTree = []
      this.departTree.push(data)
    },
    // 清空表单
    resetForm() {
      this.form = {
        id: null,
        name: '',
        description: '',
        order: 0
      }
    },
    handleCreate(data) {
      this.resetForm()
      // 保存当前节点
      this.currentNode = data
      this.form.pid = data.id
      if (data.children) {
        this.form.order = data.order + data.children.length
      } else {
        this.form.order = data.order + 1
      }
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['departmentForm'].clearValidate()
      })
    },
    create() {
      this.$refs['departmentForm'].validate(async valid => {
        if (valid) {
          const id = await departCreate(
            this.form.name,
            this.form.description,
            this.form.order,
            this.form.pid
          )
          this.form.id = id
          if (!this.currentNode.children) {
            this.$set(this.currentNode, 'children', [])
          }
          this.currentNode.children.push({
            ...this.form,
            label: this.form.name,
            children: []
          })
          this.dialogFormVisible = false
          this.$message({
            message: '创建成功',
            type: 'success'
          })
        }
      })
    },
    handleUpdate(node, data) {
      this.form.id = data.id
      this.form.pid = data.pid
      this.form.order = data.order
      this.form.description = data.description
      this.form.name = data.label
      this.currentNode = data
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['departmentForm'].clearValidate()
      })
    },
    update() {
      this.$refs['departmentForm'].validate(async valid => {
        const { id, name, description, order, pid } = this.form
        await departUpdate(id, name, description, order, pid)
        await this.getDepartmentTree()
        this.dialogFormVisible = false
        this.$message({
          message: '更新成功',
          type: 'success'
        })
      })
    },
    handleDelete(node, data) {
      var tempMsg = '确定删除 ' + data.label + ' 部门'
      this.$confirm(tempMsg, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        await departDelete(data.id)
        await this.getDepartmentTree()
        this.$message({
          message: '删除成功',
          type: 'success'
        })
      })
    },
    async handleMember(data) {
      const result = await getDepartUsers(data.id)
      this.memberModel.id = data.id
      this.memberModel.title = `设置 ${data.label} `
      this.memberModel.userIds = result.selectItems
      this.memberModel.items = result.items
    },
    member() {
      this.$confirm(this.memberModel.title, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await updateDepartUsers(
            this.memberModel.id,
            this.memberModel.userIds
          )
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
    this.getDepartmentTree()
  }
}
</script>

<style>
.custom-tree-node {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 14px;
  padding-right: 8px;
}
</style>
