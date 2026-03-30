using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.User
{
   public class UserInfo:BasePO
    {
       private int userID = 0;
       private string userName = null;
       private string userCode = null;
       private string workNo = null;
       private string officeTel = null;
       private int userStatus = 0;
       private string userstatusstr = "";
       /**
        * 用户状态 - 有效
        */
       public static int USER_STATUS_VALID = 1;
       /**
        * 用户状态 - 离职
        */
       public static int USER_STATUS_LEAVE = 2;
       /**
        * 用户状态 - 冻结
        */
       public static int USER_STATUS_FREEZE = 3;

       public static int[] GetUserStatus()
       {
           int[] status = new int[3];
           status[0] = USER_STATUS_VALID;
           status[1] = USER_STATUS_LEAVE;
           status[2] = USER_STATUS_FREEZE;
           return status;
       }

       public static String GetUserStautsDesc(int stauts)
       {
           if (stauts == USER_STATUS_VALID)
           {
               return "User_StatusEnabled";
           }
           if (stauts == USER_STATUS_LEAVE)
           {
               return "User_StatusResigned";
           }
           if (stauts == USER_STATUS_FREEZE)
           {
               return "User_StatusDisabled";
           }
           return "Public_Sealed";
       }
       public String UserStautsDesc
       {
           get { return GetUserStautsDesc(UserStatus); }
       }



       public override String GetTableName()
       {
           return "";
       }

       public override String GetColumnNames()
       {
           return "UserID,UserName,UserCode,WorkNo,OfficeTel,UserStatus";
       }

       public override string GetUpdateColumnNames()
       {
           return "";
       }

       public override string GetQuerySql()
       {
           return "select a.UserID,UserName,a.UserCode,WorkNo,OfficeTel,UserStatus from Users a ,UserRole b ";
       }


       public int UserID
       {
           get { return userID; }
           set {  userID = value ; }
       }

       public string UserName
       {
           get { return userName; }
           set { userName =value; }
       }

       public string UserCode
       {
           get { return userCode; }
           set { userCode = value; }
       }

       public string WorkNo
       {
           get { return workNo; }
           set { workNo = value; }
       }

       public string OfficeTel
       {
           get { return officeTel; }
           set { officeTel = value; }
       }

       public int UserStatus
       {
           get { return userStatus; }
           set { userStatus = value; }
       }

       public string UserStatusStr
       {
           get { return userstatusstr; }
           set { userstatusstr = value; }
       }
    }
}
