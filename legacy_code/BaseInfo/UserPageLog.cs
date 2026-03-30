using Base.Biz;
using BaseInfo.User;
using System.Data;
using Base.Page;
using System;


namespace BaseInfo.User
{
    /// <summary>
    /// 用户访问日志类
    /// </summary>
    public class UserPageLog
    {
        /// <summary>
        /// 创建用户登陆访问日志
        /// </summary>
        /// <param name="intUserID">用户ID</param>
        /// /// <param name="strIP">IP地址</param>
        public static void UserLoginLog(int intUserID,string strIP)
        {
            OpenPageLog openpageLog = new OpenPageLog();

            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "UserID=" + intUserID;

            DataTable dt = new DataTable();
            string strSql="select users.userid,users.username,role.roleid,role.rolename,dept.deptid,dept.deptname from users" +
                          " inner join userrole on (users.userid=userrole.userid)" +
                          " inner join role on (userrole.roleid=role.roleid)" +
                          " inner join dept on (userrole.deptid=dept.deptid)" +
                          " where users.userid=" + intUserID ;

            dt = baseBO.QueryDataSet(strSql).Tables[0];

            if (dt.Rows.Count == 1)
            {
                openpageLog.CreateUserID = intUserID;
                openpageLog.CreateUserName = dt.Rows[0]["username"].ToString();
                openpageLog.CreateTime = DateTime.Now;
                openpageLog.OprDeptID=Convert.ToInt32(dt.Rows[0]["deptid"].ToString());
                openpageLog.OprDeptName = dt.Rows[0]["deptname"].ToString();
                openpageLog.OprRoleID = Convert.ToInt32(dt.Rows[0]["roleid"].ToString());
                openpageLog.OprRoleName = dt.Rows[0]["rolename"].ToString();
                openpageLog.PagePath = "/Web/Login.aspx";
                openpageLog.PageName = "系统登陆";
                openpageLog.IpAddress = strIP ;

                openpageLog.InsertRows();

            }      
            
            
        }

    }
}
