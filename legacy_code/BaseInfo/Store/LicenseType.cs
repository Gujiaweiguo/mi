using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class LicenseType : BasePO
    {
        private int licensetypeid = 0;
        private string licensetypename = "";
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
            return "LicenseType";
        }
        public override String GetColumnNames()
        {
            return "LicenseTypeId,LicenseTypeName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "LicenseTypeName,Status,Note,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetInsertColumnNames()
        {
            return "LicenseTypeId,LicenseTypeName,Status,Note,CreateUserId,CreateTime,OprRoleID,OprDeptID";
        }
        public int LicenseTypeId
        {
            get { return licensetypeid; }
            set { licensetypeid = value; }
        }
        public string LicenseTypeName
        {
            get { return licensetypename; }
            set { licensetypename = value; }
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