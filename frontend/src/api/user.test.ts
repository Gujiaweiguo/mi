import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  createUser,
  listRoles,
  listUsers,
  resetPassword,
  setUserRoles,
  updateUser,
} from './user'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('user api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listUsers', () => {
    it('calls GET /users and returns the response', async () => {
      const response = {
        data: {
          users: [
            { id: 1, department_id: 10, username: 'admin', display_name: 'Admin', status: 'active' },
          ],
        },
      } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listUsers()

      expect(http.get).toHaveBeenCalledWith('/users')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listUsers()).rejects.toThrow('fail')
    })
  })

  describe('createUser', () => {
    it('calls POST /users with data and returns the response', async () => {
      const payload = {
        username: 'john',
        display_name: 'John',
        password: 'secret123',
        department_id: 5,
        role_ids: [1, 2],
      }
      const response = {
        data: { user: { id: 2, department_id: 5, username: 'john', display_name: 'John', status: 'active' } },
      } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createUser(payload)

      expect(http.post).toHaveBeenCalledWith('/users', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createUser({ username: 'x', display_name: 'X', password: 'p', department_id: 1, role_ids: [] }),
      ).rejects.toThrow('fail')
    })
  })

  describe('updateUser', () => {
    it('calls PUT /users/:id with data and returns the response', async () => {
      const payload = { display_name: 'John Updated', department_id: 6, status: 'inactive' }
      const response = {
        data: { user: { id: 2, department_id: 6, username: 'john', display_name: 'John Updated', status: 'inactive' } },
      } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await updateUser(2, payload)

      expect(http.put).toHaveBeenCalledWith('/users/2', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(updateUser(2, { display_name: 'X' })).rejects.toThrow('fail')
    })
  })

  describe('resetPassword', () => {
    it('calls POST /users/:id/reset-password with data and returns the response', async () => {
      const payload = { new_password: 'newpass456' }
      const response = { data: {} } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await resetPassword(2, payload)

      expect(http.post).toHaveBeenCalledWith('/users/2/reset-password', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(resetPassword(2, { new_password: 'x' })).rejects.toThrow('fail')
    })
  })

  describe('setUserRoles', () => {
    it('calls PUT /users/:id/roles with data and returns the response', async () => {
      const payload = { role_ids: [3, 4], department_id: 5 }
      const response = { data: {} } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await setUserRoles(2, payload)

      expect(http.put).toHaveBeenCalledWith('/users/2/roles', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)

      await expect(setUserRoles(2, { role_ids: [], department_id: 1 })).rejects.toThrow('fail')
    })
  })

  describe('listRoles', () => {
    it('calls GET /roles and returns the response', async () => {
      const response = {
        data: {
          roles: [
            { ID: 1, Code: 'admin', Name: 'Admin', Status: 'active', IsLeader: false },
          ],
        },
      } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listRoles()

      expect(http.get).toHaveBeenCalledWith('/roles')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listRoles()).rejects.toThrow('fail')
    })
  })
})
