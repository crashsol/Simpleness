<template>
    <div>
      Role teable
    </div>
</template>

<script>
import { roleList, roleCreate, roleUpdate
  , roleDelete, getRoleUsers, updateRoleUsers,
  getRolePermissions, updateRolePermissions
} from '../../api/role.js'
export default {
  data() {
    return {
      roles: []
    }
  },
  methods: {
    getRoles: async() => {
      const result = await roleList()
      console.log(result)
    }
  },
  mounted() {
    this.getRoles()
  }
}
</script>

<style>

</style>
