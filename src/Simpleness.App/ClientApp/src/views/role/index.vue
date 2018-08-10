<template>
    <div>
      Role teable
    </div>
</template>

<script>
import { roleList } from '../../api/role.js'
export default {
  data() {
    return {
      roles: []
    }
  },
  methods: {
    async getRoles() {
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
