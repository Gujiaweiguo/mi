using System;
using System.Collections.Generic;
using System.Text;

using com.basis.po;

namespace BaseInfo.User
{
    public class UserAuth : BasePO
    {
        private int deptID;
        private int userID;
        private int roleID;

        public override String GetTableName()
        {
            return "UserAuth";
        }

        public override String GetColumnNames()
        {
            return "DeptID,UserID,RoleID";
        }

        public override String GetUpdateColumnNames()
        {
            return "DeptID,UserID,RoleID";
        }

        public int DeptID
        {
            get{return this.deptID;}
            set{this.deptID=value;}
        }
        public int UserID
        {
            get{return this.userID;}
            set{this.userID=value;}
        }
        public int RoleID
        {
            get{return this.roleID;}
            set{this.roleID=value;}
        }
    }
}