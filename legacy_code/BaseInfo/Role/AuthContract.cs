using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace BaseInfo.Role
{
    public class AuthContract:BasePO
    {
        private int authContractID = 0;
        private int authContractFlag = 1;
        private int userID = 0;
        private int createUserID = 0; //创建用户代码
        private DateTime createTime = DateTime.Now; //创建时间
        private int modifyUserID = 0; //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0; //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码


        public static int AuthContractFlag_OnlyOne = 1;
        public static int AuthContractFlag_OnlyDept = 2;
        public static int AuthContractFlag_OnlyAll = 3;

        public override String GetTableName()
        {
            return "AuthContract";
        }

        public override String GetColumnNames()
        {
            return "AuthContractID,AuthContractFlag,UserID";
        }

        public override string GetInsertColumnNames()
        {
            return "AuthContractID,AuthContractFlag,UserID,CreateUserID,CreateTime,ModifyUserID," +
                    "ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int AuthContractID
        {
            get { return authContractID; }
            set { authContractID = value; }
        }

        public int AuthContractFlag
        {
            get { return authContractFlag; }
            set { authContractFlag = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }
        public DateTime ModifyTime
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

    }
}
