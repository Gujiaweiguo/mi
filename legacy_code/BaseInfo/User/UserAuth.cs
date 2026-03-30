using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace BaseInfo.User
{
    public class UserAuth : BasePO
    {
        private int userID=0;
        private int roleID=0;
        private int deptID = 0;

        public override String GetTableName()
        {
            return "UserAuth";
        }

        public override String GetColumnNames()
        {
            return "UserID,RoleID,DeptID";
        }

        public override String GetUpdateColumnNames()
        {
            return "UserID,RoleID,DeptID";
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
        public int DeptID
        {
            get { return this.deptID; }
            set { this.deptID = value; }
        }

    }
}