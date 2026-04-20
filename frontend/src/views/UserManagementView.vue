<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { ElMessage } from 'element-plus'

import { listUsers, createUser, updateUser, resetPassword, listRoles, type UserItem, type RoleItem } from '../api/user'
import { listDepartments } from '../api/org'
import PageSection from '../components/platform/PageSection.vue'

const { t } = useI18n()

const users = ref<UserItem[]>([])
const roles = ref<RoleItem[]>([])
const deptMap = ref(new Map<number, string>())
const loading = ref(false)

const createDialogVisible = ref(false)
const editDialogVisible = ref(false)
const resetDialogVisible = ref(false)

const createForm = ref({ username: '', display_name: '', password: '', department_id: 0, role_ids: [] as number[] })
const editForm = ref<{ id: number; display_name: string; department_id: number; status: string }>({ id: 0, display_name: '', department_id: 0, status: '' })
const resetForm = ref<{ id: number; new_password: string }>({ id: 0, new_password: '' })

const loadUsers = async () => {
  loading.value = true
  try {
    const resp = await listUsers()
    users.value = resp.data.users ?? []
  } catch {
    ElMessage.error('加载用户列表失败')
  } finally {
    loading.value = false
  }
}

const roleNames = (user: UserItem) => {
  const ids = (user as any).role_ids as number[] | undefined
  if (!ids) return ''
  return ids.map(id => roles.value.find(r => r.ID === id)?.Name ?? id).join(', ')
}

const openCreate = () => {
  createForm.value = { username: '', display_name: '', password: '', department_id: 0, role_ids: [] }
  createDialogVisible.value = true
}

const handleCreate = async () => {
  try {
    await createUser(createForm.value)
    ElMessage.success('创建用户成功')
    createDialogVisible.value = false
    await loadUsers()
  } catch {
    ElMessage.error('创建用户失败')
  }
}

const openEdit = (row: UserItem) => {
  editForm.value = { id: row.id, display_name: row.display_name, department_id: row.department_id, status: row.status }
  editDialogVisible.value = true
}

const handleEdit = async () => {
  try {
    await updateUser(editForm.value.id, { display_name: editForm.value.display_name, department_id: editForm.value.department_id, status: editForm.value.status })
    ElMessage.success('更新用户成功')
    editDialogVisible.value = false
    await loadUsers()
  } catch {
    ElMessage.error('更新用户失败')
  }
}

const openReset = (row: UserItem) => {
  resetForm.value = { id: row.id, new_password: '' }
  resetDialogVisible.value = true
}

const handleReset = async () => {
  try {
    await resetPassword(resetForm.value.id, { new_password: resetForm.value.new_password })
    ElMessage.success('重置密码成功')
    resetDialogVisible.value = false
  } catch {
    ElMessage.error('重置密码失败')
  }
}

onMounted(async () => {
  void loadUsers()
  try {
    const [deptResp, roleResp] = await Promise.all([listDepartments(), listRoles()])
    deptResp.data.departments?.forEach((d) => deptMap.value.set(d.id, d.name))
    roles.value = roleResp.data.roles ?? []
  } catch { /* non-critical */ }
})
</script>

<template>
  <div class="user-management-view" v-loading="loading">
    <PageSection eyebrow="系统管理" title="用户管理" summary="管理系统用户账号、角色分配及密码重置">
      <template #actions>
        <el-button type="primary" @click="openCreate">新建用户</el-button>
      </template>
    </PageSection>

    <el-card shadow="never" class="user-management-view__table-card">
      <el-table :data="users" row-key="id" style="width: 100%">
        <el-table-column prop="username" label="用户名" min-width="140" />
        <el-table-column prop="display_name" label="显示名称" min-width="140" />
        <el-table-column label="部门" min-width="120">
          <template #default="scope">{{ deptMap.get(scope.row.department_id) ?? scope.row.department_id }}</template>
        </el-table-column>
        <el-table-column label="状态" min-width="100">
          <template #default="scope">
            <el-tag :type="scope.row.status === 'active' ? 'success' : 'danger'" effect="plain">
              {{ scope.row.status === 'active' ? '启用' : '停用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="角色" min-width="160">
          <template #default="scope">{{ roleNames(scope.row) }}</template>
        </el-table-column>
        <el-table-column label="操作" min-width="160" fixed="right">
          <template #default="scope">
            <el-button link type="primary" @click="openEdit(scope.row)">编辑</el-button>
            <el-button link type="warning" @click="openReset(scope.row)">重置密码</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="createDialogVisible" title="新建用户" width="520px" destroy-on-close>
      <el-form label-width="80px">
        <el-form-item label="用户名"><el-input v-model="createForm.username" /></el-form-item>
        <el-form-item label="显示名称"><el-input v-model="createForm.display_name" /></el-form-item>
        <el-form-item label="密码"><el-input v-model="createForm.password" type="password" show-password /></el-form-item>
        <el-form-item label="部门">
          <el-select v-model="createForm.department_id" placeholder="请选择部门">
            <el-option v-for="[id, name] of deptMap" :key="id" :label="name" :value="id" />
          </el-select>
        </el-form-item>
        <el-form-item label="角色">
          <el-select v-model="createForm.role_ids" multiple placeholder="请选择角色">
            <el-option v-for="r in roles" :key="r.ID" :label="r.Name" :value="r.ID" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="createDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleCreate">确定</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="editDialogVisible" title="编辑用户" width="520px" destroy-on-close>
      <el-form label-width="80px">
        <el-form-item label="显示名称"><el-input v-model="editForm.display_name" /></el-form-item>
        <el-form-item label="部门">
          <el-select v-model="editForm.department_id" placeholder="请选择部门">
            <el-option v-for="[id, name] of deptMap" :key="id" :label="name" :value="id" />
          </el-select>
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="editForm.status">
            <el-option label="启用" value="active" />
            <el-option label="停用" value="inactive" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="editDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleEdit">确定</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="resetDialogVisible" title="重置密码" width="420px" destroy-on-close>
      <el-form label-width="80px">
        <el-form-item label="新密码"><el-input v-model="resetForm.new_password" type="password" show-password /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="resetDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleReset">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<style scoped>
.user-management-view {
  display: flex;
  flex-direction: column;
  gap: var(--mi-space-5);
}

.user-management-view__table-card {
  border-radius: var(--mi-radius-md);
  border-color: var(--mi-color-border);
}
</style>
