using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Role
{
    public class RoleQuery:BasePO
    {
        /**
         * 是否领导  否
         */
        public static int IS_LEADER_NO = 0;

        /**
         * 是否领导  是
         */
        public static int IS_LEADER_YES = 1;

        /**
        *状态     无效
        */
        public static int IS_ROLESTATUS_NO = 0;

        /**
        * 状态  有效
        */
        public static int IS_ROLESTATUS_YES = 1;

        private int roleID = 0;
        private int createUserID = 0;
        private System.DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private System.DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private string roleCode = null;
        private string roleName = null;
        private int roleStatus = 0;
        private int isLeader;
        private string roleStatusName = "";
        public override string GetTableName()
        {
            return "Role";
        }

        public override String GetColumnNames()
        {
            return "RoleID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,RoleCode,RoleName,(case when RoleStatus=1 then '有效' when RoleStatus=0 then '无效' else '未知' end) as RoleStatusName,RoleStatus,IsLeader";
        }
        public override string GetUpdateColumnNames()
        {
            return "ModifyUserID,ModifyTime,RoleCode,RoleName,RoleStatus,IsLeader";
        }

        public static int[] GetRoleStatus()
        {
            int[] rolestatus = new int[2];
            rolestatus[0] = IS_ROLESTATUS_YES;
            rolestatus[1] = IS_ROLESTATUS_NO;
            return rolestatus;
        }

        public static string GetRoleStatusDesc(int status)
        {
            if (status == IS_ROLESTATUS_NO)
            {
                return "无效";
            }
            if (status == IS_ROLESTATUS_YES)
            {
                return "有效";
            }
            return"未知";
        }

        public String RoleStatusDesc
        {
            get { return GetRoleStatusDesc(roleStatus); }
        }


        public static int[] GetLeader()
        {
            int[] status = new int[2];
            status[0] = IS_LEADER_NO;
            status[1] = IS_LEADER_YES;
            return status;
        }

        public static String GetLeaderDesc(int stauts)
        {
            if (stauts == IS_LEADER_NO)
            {
                return "否";
            }
            if (stauts == IS_LEADER_YES)
            {
                return "是";
            }
            return "未知";
        }

        public String LeaderDesc
        {
            get { return GetLeaderDesc(isLeader); }
        }


        #region 角色

        public int RoleID
        {
            get { return this.roleID; }
            set { this.roleID = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public System.DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        public System.DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }

        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }

        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }

        public string RoleCode
        {
            get { return roleCode; }
            set { roleCode = value; }
        }

        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        public int RoleStatus
        {
            get { return roleStatus; }
            set { roleStatus = value; }
        }

        public int IsLeader
        {
            get { return isLeader; }
            set { isLeader = value; }
        }
        public string RoleStatusName
        {
            get { return roleStatusName; }
            set { roleStatusName = value; }
        }
        #endregion
    }
}
