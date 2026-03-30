using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.PotCust
{
    public class ClientLevel : BasePO
    {
        private int clientlevelid = 0;
        private string clientlevelname = "";
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
            return "ClientLevel";
        }
        public override String GetColumnNames()
        {
            return "ClientLevelId,ClientLevelName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "ClientLevelName,Status,Note,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetInsertColumnNames()
        {
            return "ClientLevelId,ClientLevelName,Status,Note,CreateUserId,CreateTime,OprRoleID,OprDeptID";
        }
        public int ClientLevelId
        {
            get { return clientlevelid; }
            set { clientlevelid = value; }
        }
        public string ClientLevelName
        {
            get { return clientlevelname; }
            set { clientlevelname = value; }
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