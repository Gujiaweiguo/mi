using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*电视*/
    public class TV:BasePO
    {
        private int tVID = 0;
        private string tVNm = "";
        private string addr = "";
        private string offPhone = "";
        private string fax = "";
        private string web = "";
        private string contactNm = "";
        private string title = "";
        private string phone = "";
        private int tvStatus = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*有效*/
        public static int TV_STATUS_YES = 1;
        /*无效*/
        public static int TV_STATUS_NO = 0;

        public static int[] GetTVStatus()
        {
            int[] tvStatus = new int[2];
            tvStatus[0] = TV_STATUS_YES;
            tvStatus[1] = TV_STATUS_NO;
            return tvStatus;
        }

        public static String GetTVStatusDesc(int tvStatus)
        {
            if (tvStatus == TV_STATUS_NO)
            {
                return "WrkFlw_Disabled";
            }
            if (tvStatus == TV_STATUS_YES)
            {
                return "WrkFlw_Enabled";
            }
            return "NULL";
        }

        public String TVStatusDesc
        {
            get { return GetTVStatusDesc(this.TVStatus); }
        }

        public override string GetTableName()
        {
            return "TV";
        }

        public override string GetColumnNames()
        {
            return "TVID,TVNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,TVStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "TVID,TVNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,TVStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "TVNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,TVStatus,ModifyUserID,ModifyTime";
        }

        public int TVID
        {
            get { return tVID; }
            set { tVID = value; }
        }

        public string TVNm
        {
            get { return tVNm; }
            set { tVNm = value; }
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

        public int TVStatus
        {
            get { return tvStatus; }
            set { tvStatus = value; }
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
