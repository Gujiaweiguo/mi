using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*网络*/
    public class Internet:BasePO
    {
        private int internetID = 0;
        private string internetNm = "";
        private string addr = "";
        private string offPhone = "";
        private string fax = "";
        private string web = "";
        private string contactNm = "";
        private string title = "";
        private string phone = "";
        private int internetStatus = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*有效*/
        public static int INTERNET_STATUS_YES = 1;
        /*无效*/
        public static int INTERNET_STATUS_NO = 0;

        public static int[] GetInternetStatus()
        {
            int[] internetStatus = new int[2];
            internetStatus[0] = INTERNET_STATUS_YES;
            internetStatus[1] = INTERNET_STATUS_NO;
            return internetStatus;
        }

        public static String GetInternetStatusDesc(int internetStatus)
        {
            if (internetStatus == INTERNET_STATUS_NO)
            {
                return "WrkFlw_Disabled";
            }
            if (internetStatus == INTERNET_STATUS_YES)
            {
                return "WrkFlw_Enabled";
            }
            return "NULL";
        }

        public String InternetStatusDesc
        {
            get { return GetInternetStatusDesc(this.InternetStatus); }
        }

        public override string GetTableName()
        {
            return "Internet";
        }

        public override string GetColumnNames()
        {
            return "InternetID,InternetNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,InternetStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "InternetID,InternetNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,InternetStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "InternetNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,InternetStatus,ModifyUserID,ModifyTime";
        }

        public int InternetID
        {
            get { return internetID; }
            set { internetID = value; }
        }

        public string InternetNm
        {
            get { return internetNm; }
            set { internetNm = value; }
        }

        public string Addr
        {
            get { return addr; }
            set { addr = value; }
        }

        public string OffPhone
        {
            get { return offPhone; }
            set { offPhone = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string Web
        {
            get { return web; }
            set { web = value; }
        }

        public string ContactNm
        {
            get { return contactNm; }
            set { contactNm = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int InternetStatus
        {
            get { return internetStatus; }
            set { internetStatus = value; }
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
    }
}
