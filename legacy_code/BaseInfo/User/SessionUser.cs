using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Base.DB;

namespace BaseInfo.User
{
    public class SessionUser : BasePO
    {
        public static String LANGUAGE_ZH_CN = "zh-cn";
        public static String LANGUAGE_EN_US = "en-us";

        private int userID = 0;  //用户 ID 
        private int createUserID = 0; //创建用户代码
        private DateTime createTime = DateTime.Now; //创建时间
        private int modifyUserID = 0; //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0; //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private string userCode = null;  //用户编码
        private string userName = null;//用户名称
        private string password = null;//用户密码
        private string identityNo = null;//身份认证
        private string workNo = null; //工牌号
        private string mobile1 = null; //移动电话1
        private string mobile2 = null;//移动电话2
        private string officeTel = null;//办公电话
        private string eMail = null; //电子邮箱
        private int userStatus = 1; //用户状态
        private byte[] photo = null; //照片
        private DateTime validDate = DateTime.Now;  //有效期
        private string note = null;  //备注
        private int roleID = 0;
        private int deptID = 0;

        private DateTime lastAccessTime = DateTime.Now;    //最后访问时间
        private String language = null;

        //得到表
        public override String GetTableName()
        {
            return "";
        }

        //得到要查询的列名
        public override String GetColumnNames()
        {
            return "UserID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,UserCode,UserName,"
                + " Password,IdentityNo,WorkNo,Mobile1,Mobile2,OfficeTel,EMail,UserStatus,Photo,ValidDate,Note";
        }

        //得到要修改的列名 
        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select UserID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,UserCode,UserName,"
                + " Password,IdentityNo,WorkNo,Mobile1,Mobile2,OfficeTel,EMail,UserStatus,photo,ValidDate,Note"
                + " from Users";
        }


        public DateTime LastAccessTime
        {
            get { return lastAccessTime; }
            set { lastAccessTime = value; }
        }

        public static String[] GetLanguages()
        {
            String[] langs = new String[2];
            langs[0] = LANGUAGE_ZH_CN;
            langs[1] = LANGUAGE_EN_US;
            return langs;
        }

        public static String GetLanguageDesc(String lang)
        {
            if (lang.Equals(LANGUAGE_ZH_CN))
            {
                return "中文";
            }
            if (lang.Equals(LANGUAGE_EN_US))
            {
                return "English";
            }
            return "未知";
        }
        
        #region  用户信息

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
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
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string IdentityNo
        {
            get { return identityNo; }
            set { identityNo = value; }
        }
        public string WorkNo
        {
            get { return workNo; }
            set { workNo = value; }
        }
        public string Mobile1
        {
            get { return mobile1; }
            set { mobile1 = value; }
        }
        public string Mobile2
        {
            get { return mobile2; }
            set { mobile2 = value; }
        }
        public string OfficeTel
        {
            get { return officeTel; }
            set { officeTel = value; }
        }
        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }
        public int UserStatus
        {
            get { return userStatus; }
            set { userStatus = value; }
        }
        public byte[] Photo
        {
            get { return photo; }
            set { photo = value; }
        }
        public DateTime ValidDate
        {
            get { return validDate; }
            set { validDate = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
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
        public String Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        #endregion


    }
}
