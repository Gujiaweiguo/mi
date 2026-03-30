using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Base.Page
{
    public class OpenPageLog:BasePO
    {
        private int _createuserid = 0;
        private string _createusername = "";
        private DateTime _createtime = DateTime.Now;
        private int _oprroleid = 0;
        private string _oprrolename = "";
        private int _oprdeptid = 0;
        private string _oprdeptname = "";
        private string _pagepath = "";
        private string _pagename = "";
        private string _ipaddress = "";

        //得到表
        public override String GetTableName()
        {
            return "OpenPageLog";
        }

        //得到要查询的列名
        public override String GetColumnNames()
        {
            return "CreateUserID,CreateUserName,CreateTime,OprRoleID,OprRoleName,OprDeptID,OprDeptName,PagePath,PageName,IPAddress";
        }

        public override string GetInsertColumnNames()
        {
            return "CreateUserID,CreateUserName,CreateTime,OprRoleID,OprRoleName,OprDeptID,OprDeptName,PagePath,PageName,IPAddress";
        }

        //得到要修改的列名 
        public override string GetUpdateColumnNames()
        {
            return "";
        }
        //插入记录
        public void InsertRows()
        {
            Base.Biz.BaseBO baseBo = new Base.Biz.BaseBO();

            baseBo.ExecuteUpdate("Insert Into OpenPageLog(CreateUserID,CreateUserName,OprDeptID,OprRoleID,PagePath,CreateTime,IPAddress,OprRoleName,OprDeptName,PageName) Values ('" +
                                _createuserid + "','" + _createusername + "','" + _oprdeptid + "','" + _oprroleid + "','" +
                                 _pagepath + "','" + _createtime + "','" + _ipaddress + "','" + _oprrolename + "','" + _oprdeptname + "','" + _pagename + "')");
        }

        /// <summary>
        /// 
        /// </summary>
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName
        {
            set { _createusername = value; }
            get { return _createusername; }
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
        public int OprRoleID
        {
            set { _oprroleid = value; }
            get { return _oprroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OprRoleName
        {
            set { _oprrolename = value; }
            get { return _oprrolename; }
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
        public string OprDeptName
        {
            set { _oprdeptname = value; }
            get { return _oprdeptname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PagePath
        {
            set { _pagepath = value; }
            get { return _pagepath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PageName
        {
            set { _pagename = value; }
            get { return _pagename; }
        }

        public string IpAddress
        {
            set { _ipaddress = value; }
            get { return _ipaddress; }
        }
    }
}
