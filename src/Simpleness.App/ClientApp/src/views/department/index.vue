<template>
  <div class="app-container">
    <el-row>
      <el-col :span="12">
        <el-button type="primary" @click="handleCreate">创建部门</el-button>
      </el-col>
    </el-row>
    <el-table :data="departments">
      <el-table-column prop="name" label="部门名称">
      </el-table-column>
      <el-table-column prop="description" label="部门描述">
      </el-table-column>
      <el-table-column prop="fullPath" label="部门层级">
      </el-table-column>
      <el-table-column prop="order" label="部门排序">
      </el-table-column>
      <el-table-column label="操作" width="400px">
        <template slot-scope="scope">
          <el-button type="infor" @click="handleUpdate(scope.row)" size="mini">编辑</el-button>
          <el-button type="danger" @click="handleDelete(scope.row)" size="mini">删除</el-button>
          <el-button type="primary" @click="handleMember(scope.row)" size="mini">成员</el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 创建/更新部门 -->
    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" width="60%">
      <el-form :rules="formRules" ref="departmentForm" :model="form" label-width="80px">
        <el-input v-model="form.id" type="hidden"></el-input>
        <el-form-item label="上级部门" prop="pid">
          <el-select v-model="form.pid" placeholder="请选择上级部门">
            <el-option v-for="item in departments" :key="item.id" :label="item.fullPath" :value="item.id">
            </el-option>
          </el-select>
        </el-form-item>
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
  departList,
  departCreate,
  departDelete,
  departUpdate
} from '../../api/department.js'
export default {
  data() {
    return {
      departments: [],
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
      }
    }
  },
  methods: {
    async getDepartments() {
      const data = await departList()
      this.departments = data
    },
    // 清空表单
    resetForm() {
      this.form = {
        id: null,
        name: '',
        description: '',
        order: 99
      }
    },
    handleCreate() {
      this.dialogStatus = true
      this.resetForm()
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
            this.form.name.description,
            this.form.order,
            this.form.pid
          )
          this.form.id = id
          await this.getDepartments()
          this.dialogFormVisible = false
          this.$message({
            message: '创建成功',
            type: 'success'
          })
        }
      })
    },
    handleUpdate(row) {
      this.form = { ...row } // row copy
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
        await this.getDepartments()
        this.dialogFormVisible = false
        this.$message({
          message: '更新成功',
          type: 'success'
        })
      })
    },
    handleDelete(row) {
      var tempMsg = '确定删除 ' + row.name + ' 部门'
      this.$confirm(tempMsg, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await departDelete(row.id)
          await this.getDepartments()
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
    }
  },
  mounted() {
    this.getDepartments()
  }
}
</script>

<style>
</style>
