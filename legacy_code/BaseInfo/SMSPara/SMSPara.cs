using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.SMSPara
{
    public class SMSPara:BasePO
    {
        private int sMSparaID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int autoContractCode = 0;
        private string nextContractCode = "";
        private int autoCustCode = 0;
        private string nextCustCode = "";
        private int autoSkuID = 0;
        private string nextSkuID = "";
        private int autoTPUserID = 0;
        private string nextTPUserID = "";
        private int autoShopCode = 0;
        private string mI_OUT = "";
        private string mI_IN = "";
        private string mailSMTP="";
        private string mailSMTPUser="";
        private string mailSMTPPassword = "";

        public static int AUTO_YES = 1;
        public static int AUTO_NO = 0;


        public override string GetTableName()
        {
            return "SMSPara";
        }

        public override string GetColumnNames()
        {
            return "SMSparaID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,AutoContractCode,NextContractCode,AutoCustCode,NextCustCode,AutoSkuID,NextSkuID,AutoTPUserID,NextTPUserID,AutoShopCode,MI_OUT,MI_IN,MailSMTP,MailSMTPUser,MailSMTPPassword";
        }

        public override string GetUpdateColumnNames()
        {
            return "ModifyUserID,ModifyTime,OprRoleID,OprDeptID,AutoContractCode,NextContractCode,AutoCustCode,NextCustCode,AutoSkuID,NextSkuID,AutoTPUserID,NextTPUserID,AutoShopCode,MI_OUT,MI_IN,MailSMTP,MailSMTPUser,MailSMTPPassword";
        }
        public override string GetInsertColumnNames()
        {
            return "SMSparaID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,AutoContractCode,NextContractCode,AutoCustCode,NextCustCode,AutoSkuID,NextSkuID,AutoTPUserID,NextTPUserID,AutoShopCode,MI_OUT,MI_IN,MailSMTP,MailSMTPUser,MailSMTPPassword";
        }

        public int SMSparaID
        {
            get { return sMSparaID; }
            set { sMSparaID = value; }
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

        public int AutoContractCode
        {
            get { return autoContractCode; }
            set { autoContractCode = value; }
        }

        public string NextContractCode
        {
            get { return nextContractCode; }
            set { nextContractCode = value; }
        }

        public int AutoCustCode
        {
            get { return autoCustCode; }
            set { autoCustCode = value; }
        }

        public string NextCustCode
        {
            get { return nextCustCode; }
            set { nextCustCode = value; }
        }

        public int AutoSkuID
        {
            get { return autoSkuID; }
            set { autoSkuID = value; }
        }

        public string NextSkuID
        {
            get { return nextSkuID; }
            set { nextSkuID = value; }
        }

        public int AutoTPUserID
        {
            get { return autoTPUserID; }
            set { autoTPUserID = value; }
        }

        public string NextTPUserID
        {
            get { return nextTPUserID; }
            set { nextTPUserID = value; }
        }

        public int AutoShopCode
        {
            get { return autoShopCode; }
            set { autoShopCode = value; }
        }

        public string MI_OUT
        {
            get { return mI_OUT; }
            set { mI_OUT = value; }
        }

        public string MI_IN
        {
            get { return mI_IN; }
            set { mI_IN = value; }
        }

        public string MailSMTP
        {
            get { return mailSMTP; }
            set { mailSMTP = value; }
        }

        public string MailSMTPUser
        {
            get { return mailSMTPUser; }
            set { mailSMTPUser = value; }
        }

        public string MailSMTPPassword
        {
            get { return mailSMTPPassword; }
            set { mailSMTPPassword = value; }
        }

    }
}
