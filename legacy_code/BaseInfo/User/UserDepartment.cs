using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.User
{
    /// <summary>
    /// 用户的部门信息，在用户调动时使用 Add by lcp at 2009-3-5
    /// </summary>
    public class UserDepartment : BasePO
    {
        private int userID = 0;
        private string userName = null;
        private string userCode = null;
        private int deptID = 0;
        private string deptName = null;
        /// <summary>
        /// 获得表名
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "[userrole]";
        }
        public override string GetColumnNames()
        {
            return "a.UserID,UserName,UserCode,b.DeptID,DeptName,pDeptName";
        }
        public override string GetUpdateColumnNames()
        {
            return "deptid";
        }
        public override string GetQuerySql()
        {
            return "select a.UserID,UserName,UserCode,b.DeptID,DeptName,(select deptname from dept where deptid=c.pdeptid)as pDeptName from users a,userrole b,dept c where a.userid=b.userid and b.deptid=c.deptid";
        }
        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }
        public string UserCode
        {
            get { return this.userCode; }
            set { this.userCode = value; }
        }
        public int DeptID
        {
            get { return this.deptID; }
            set { this.deptID = value; }
        }
        public string DeptName
        {
            get { return this.deptName; }
            set { this.deptName = value; }
        }
    }
}
