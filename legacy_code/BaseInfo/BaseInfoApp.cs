using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Base.Biz;
using Base.DB;
using BaseInfo.Role;

namespace BaseInfo
{
    public class BaseInfoApp
    {
        /**
         * 存放角色的哈希表
         */
        private static Hashtable roles = new Hashtable();

        /**
         * 获得角色名称
         */
        public static String GetRoleName(int roleID)
        {
            if (roles.Count == 0)
            {
                //从数据库装入角色
                BaseBO bo = new BaseBO();
                bo.OrderBy = "RoleID";
                Resultset rs = bo.Query(new Role.Role());
                foreach (Role.Role role in rs)
                {
                    roles.Add(role.RoleID, role.RoleName);
                }
            }
            return roles[roleID] as String;
        }
    }
}
