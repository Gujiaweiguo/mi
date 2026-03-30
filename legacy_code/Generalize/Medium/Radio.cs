using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*电台*/
    public class Radio:BasePO
    {
        private int radioID = 0;
        private string radioNm = "";
        private string addr = "";
        private string offPhone = "";
        private string fax = "";
        private string web = "";
        private string contactNm = "";
        private string title = "";
        private string phone = "";
        private int radioStatus = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*有效*/
        public static int RADIO_STATUS_YES = 1;
        /*无效*/
        public static int RADIO_STATUS_NO = 0;

        public static int[] GetRadioStatus()
        {
            int[] radioStatus = new int[2];
            radioStatus[0] = RADIO_STATUS_YES;
            radioStatus[1] = RADIO_STATUS_NO;
            return radioStatus;
        }

        public static String GetRadioStatusDesc(int radioStatus)
        {
            if (radioStatus == RADIO_STATUS_NO)
            {
                return "WrkFlw_Disabled";
            }
            if (radioStatus == RADIO_STATUS_YES)
            {
                return "WrkFlw_Enabled";
            }
            return "NULL";
        }

        public String RadioStatusDesc
        {
            get { return GetRadioStatusDesc(this.RadioStatus); }
        }

        public override string GetTableName()
        {
            return "Radio";
        }

        public override string GetColumnNames()
        {
            return "RadioID,RadioNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,RadioStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "RadioID,RadioNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,RadioStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "RadioNm,Addr,OffPhone,Fax,Web,ContactNm,Title,Phone,RadioStatus,ModifyUserID,ModifyTime";
        }

        public int RadioID
        {
            get { return radioID; }
            set { radioID = value; }
        }

        public string RadioNm
        {
            get { return radioNm; }
            set { radioNm = value; }
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

        public int RadioStatus
        {
            get { return radioStatus; }
            set { radioStatus = value; }
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
