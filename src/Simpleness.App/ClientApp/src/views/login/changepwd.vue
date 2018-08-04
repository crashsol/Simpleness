<template>
    <el-container>
        <el-main>
            <el-row>
                <el-col :span="12">
                    <el-form ref="changepwdForm" :model="form" label-width="120px" :rules="rules" status-icon>
                        <el-form-item label="用户名" prop="userName">
                            <el-input v-model="form.userName" placeholder="请输入用户名"></el-input>
                        </el-form-item>
                        <el-form-item label="原密码" prop="oldPassword">
                            <el-input v-model="form.oldPassword" placeholder="请输入原密码" type="password"></el-input>
                        </el-form-item>
                        <el-form-item label="新密码" prop="newPassword">
                            <el-input v-model="form.newPassword" placeholder="请输入新密码" type="password"></el-input>
                        </el-form-item>
                        <el-form-item label="确认密码" prop="newPasswordComfirm">
                            <el-input v-model="form.newPasswordComfirm" placeholder="请再次输入" type="password"></el-input>
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" @click="submitForm('changepwdForm')">提交</el-button>
                            <el-button @click="resetForm('changepwdForm')">重置</el-button>
                        </el-form-item>

                    </el-form>
                </el-col>
            </el-row>
        </el-main>
    </el-container>
</template>

<script>
/*
    1、data中定义要验证的规则
    2、在form表单上添加验证规则绑定:rules="rules"
    3、在 el-form-item 绑定验证的属性名
    4、在提交时通过refs拿到需要验证的表单验证通过后在提交数据
     submitForm(formName) {
        this.$refs[formName].validate((valid) => {
          if (valid) {
            alert('submit!');
          } else {
            console.log('error submit!!');
            return false;
          }
        });
      },
      resetForm(formName) {
        this.$refs[formName].resetFields();
      }
    5、也可以在data()中自定义验证规则，并通过 { validator: validataorName, trigger: 'blur' }进行绑定
 */
export default {
  data() {
    /* 自定义验证规则 */
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请再次输入密码'))
      } else if (value !== this.form.newPassword) {
        callback(new Error('两次输入密码不一致!'))
      } else {
        callback()
      }
    }
    return {
      form: {
        userName: '',
        oldPassword: '',
        newPassword: '',
        newPasswordComfirm: ''
      },

      rules: {
        userName: [
          { required: true, message: '请输入用户名', trigger: 'blur' }
        ],
        oldPassword: [
          { required: true, message: '必须输入原密码', trigger: 'blur' },
          { min: 6, message: '至少6个字符', trigger: 'blur' }
        ],
        newPassword: [
          { required: true, message: '请输入新密码', trigger: 'blur' },
          { min: 6, message: '至少6个字符', trigger: 'blur' }
        ],
        newPasswordComfirm: [
          { validator: validatePass2, trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    /* 提交数据 */
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          this.$message({
            type: 'success',
            message: '提交表单'
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    /* 重置表单 */
    resetForm(formName) {
      this.$refs[formName].resetFields()
    }
  }
}
</script>

<style>
</style>
