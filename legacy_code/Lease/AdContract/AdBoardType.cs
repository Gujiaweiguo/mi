using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    public class AdBoardType:BasePO
    {
        private int _adboardtypeid = 0;
        private DateTime _createtime = DateTime.Now;
        private int _modifyuserid = 0;
        private DateTime _modifytime = DateTime.Now;
        private int _oprroleid = 0;
        private int _oprdeptid = 0;
        private string _adboardtypecode = "";
        private string _adboardtypename = "";
        private int _adboardtypestatus = 0;
        private int _createUserID = 0;

        public override String GetTableName()
        {
            return "AdBoardType";
        }

        public override String GetColumnNames()
        {
            return "AdBoardTypeID,AdBoardTypeCode,AdBoardTypeName,AdBoardTypeStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override String GetInsertColumnNames()
        {
            return "AdBoardTypeID,AdBoardTypeCode,AdBoardTypeName,AdBoardTypeStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override String GetUpdateColumnNames()
        {
            return "AdBoardTypeCode,AdBoardTypeName,AdBoardTypeStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        /// <summary>
        /// 
        /// </summary>
        public int AdBoardTypeID
        {
            set { _adboardtypeid = value; }
            get { return _adboardtypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUserID
        {
            set { _createUserID = value; }
            get { return _createUserID; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OprRoleID
        {
            set { _oprroleid = value; }
            get { return _oprroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OprDeptID
        {
            set { _oprdeptid = value; }
            get { return _oprdeptid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AdBoardTypeCode
        {
            set { _adboardtypecode = value; }
            get { return _adboardtypecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AdBoardTypeName
        {
            set { _adboardtypename = value; }
            get { return _adboardtypename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AdBoardTypeStatus
        {
            set { _adboardtypestatus = value; }
            get { return _adboardtypestatus; }
        }
    }
}
