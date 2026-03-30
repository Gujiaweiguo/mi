using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace BaseInfo
{
    public class CommonInfoPO:BasePO
    {
     
        private int createUserID =0;
        private DateTime ceateTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public String GetCommonColumnNames()
        {
            return "CreateUserID,CeateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "CreateUserID,CeateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }
        
        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public DateTime CeateTime
        {
            get { return ceateTime; }
            set { ceateTime = value; }
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
