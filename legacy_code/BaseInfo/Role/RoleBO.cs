using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

using Base.Biz;

namespace BaseInfo.Role
{
    public class RoleBO : BaseBO
    {
        /**
         * 获得制定部门的管理角色代码
         * 返回值：管理角色代码，-1：该部门没有管理角色
         */
        public int GetMgrRoleID(int deptID)
        {
            String sql = "select a.RoleID from UserAuth a,Role b where a.RoleID=b.RoleID and a.DeptID=" + deptID + " and b.IsLeader=" + Role.IS_LEADER_YES;
            DataSet ds = QueryDataSet(sql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0] != null)
            {
                return ((int)ds.Tables[0].Rows[0][0]);
            }
            return -1;
        }
    }
}
