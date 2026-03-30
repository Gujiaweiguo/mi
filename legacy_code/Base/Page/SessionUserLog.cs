using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Page
{
    public class SessionUserLog
    {
        private int userID = 0;  //用户 ID 
        private int oprRoleID = 0; //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private string userName = null;//用户名称
        private int roleID = 0;
        private int deptID = 0;


        public int UserID
        {
            get { return userID; }
            set { userID = value; }
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
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public int RoleID
        {
            get { return this.roleID; }
            set { this.roleID = value; }
        }
        public int DeptID
        {
            get { return this.deptID; }
            set { this.deptID = value; }
        }
    }
}
