using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class StoreManageType : BasePO
    {
        private int typeid = 0;
        private string typename = "";
        private int status = 0;
        private string note = "";
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;
        public override String GetTableName()
        {
            return "StoreManageType";
        }
        public override String GetColumnNames()
        {
            return "TypeID,TypeName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "TypeName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public int TypeID
        {
            get { return typeid; }
            set { typeid = value; }
        }
        public string TypeName
        {
            get { return typename; }
            set { typename = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public int CreateUserId
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }
        public int ModifyUserId
        {
            get { return modifyuserid; }
            set { modifyuserid = value; }
        }
        public DateTime ModifyTime
        {
            get { return modifytime; }
            set { modifytime = value; }
        }
        public int OprRoleID
        {
            get { return oprroleid; }
            set { oprroleid = value; }
        }
        public int OprDeptID
        {
            get { return oprdeptid; }
            set { oprdeptid = value; }
        }
    }
}