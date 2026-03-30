using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*平面广告*/
    public class Prints:BasePO
    {
        private int printsID = 0;
        private string printsNm = "";
        private string addr = "";
        private string offPhone = "";
        private string fax = "";
        private string web = "";
        private string contactNm = "";
        private string title = "";
        private string phone = "";
        private int printsStatus = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*有效*/
        public static int PRINTS_STATUS_YES = 1;
        /*无效*/
        public static int PRINTS_STATUS_NO = 0;

        public static int[] GetPrintsStatus()
        {
            int[] printsStatus = new int[2];
            printsStatus[0] = PRINTS_STATUS_YES;
            printsStatus[1] = PRINTS_STATUS_NO;
            return printsStatus;
        }

        public static String GetPrintsStatusDesc(int printsStatus)
        {
            if (printsStatus == PRINTS_STATUS_NO)
            {
                return "WrkFlw_Disabled";
            }
            if (printsStatus == PRINTS_STATUS_YES)
            {
                return "WrkFlw_Enabled";
            }
            return "NULL";
        }

        public String PrintsStatusDesc
        {
            get { return GetPrintsStatusDesc(this.PrintsStatus); }
        }

        public override string GetTableName()
        {
            return "Prints";
        }

        public override string GetColumnNames()
        {
            return "PrintsID,PrintsNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,PrintsStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "PrintsID,PrintsNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,PrintsStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "PrintsNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,PrintsStatus,ModifyUserID,ModifyTime";
        }

        public int PrintsID
        {
            get { return printsID; }
            set { printsID = value; }
        }

        public string PrintsNm
        {
            get { return printsNm; }
            set { printsNm = value; }
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

        public int PrintsStatus
        {
            get { return printsStatus; }
            set { printsStatus = value; }
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
