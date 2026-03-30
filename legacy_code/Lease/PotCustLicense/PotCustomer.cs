using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PotCustLicense
{
    public class PotCustomer : CommonInfoPO
    {
        public static int POTCUSTOMER_YES_PUT_IN_NO_UPDATE_LEASE_STATUS = 0;//待审批
        public static int POTCUSTOMER_PASS = 1;//已审批
        public static int POTCUSTOMER_REFUSE_UPDATE_LEASE_STATUS = 2;//审批拒绝
        public static int POTCUSTOMER_CANCEL_EFUSE_UPDATE_LEASE_STATUS = 3;//取消立项
        public static int POTCUSTOMER_DRAFT = 4;//草稿
        //public static int POTCUSTOMER_NO_ITEM = 2;//未立项
        //public static int POTCUSTOMER_YES_ITEM_PUT_IN = 3;//已立项，未提交
        //public static int POTCUSTOMER_YES_PUT_IN_NO_UPDATE_LEASE_STATUS = 4;//已提交，待审批

        public static int[] GetCustTypeStatus()
        {
            int[] custTypeStatus = new int[2];
            custTypeStatus[0] = POTCUSTOMER_CANCEL_EFUSE_UPDATE_LEASE_STATUS;
            custTypeStatus[1] = POTCUSTOMER_PASS;
            return custTypeStatus;
        }


        public static String GetCustTypeStatusDesc(int custTypeStatus)
        {
            if (custTypeStatus == POTCUSTOMER_CANCEL_EFUSE_UPDATE_LEASE_STATUS)
            {
                return "WrkFlw_Disabled";// "无效";
            }
            if (custTypeStatus == POTCUSTOMER_PASS)
            {
                return "WrkFlw_Enabled";//"有效";
            }
            return "NO";
        }

        private int custID = 0;
        private string custCode = "";
        private string custName = "";
        private string custShortName = "";
        private int custTypeID = 0;
        private string legalRep = "";
        private string legalRepTitle = "";
        private string regCode = "";
        private string legalCode = "";
        private string taxCode = "";
        private int creditLevel = 0;
        private int isBlacklist = 0;
        private decimal regCap = 0;
        private string bankName = "";
        private string bankAcct = "";
        private string regAddr = "";
        private string officeAddr = "";
        private string officeAddr2 = "";
        private string officeAddr3 = "";
        private string postAddr = "";
        private string postAddr2 = "";
        private string postAddr3 = "";
        private string postCode = "";
        private int customerStatus = 0;
        private string officeTel = "";
        private string fax = "";
        private string weburl = "";
        private DateTime firstSigningDate = DateTime.Now;
        private string note = "";
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int commOper = 0;
        private int curTypeID = 0;

        private int sourceTypeId = 0;//商户来源
        private int creditLevelId = 0;//等级


        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        //public int CreateUserID
        //{
        //    get { return createUserID; }
        //    set { createUserID = value; }
        //}

        //public DateTime CreateTime
        //{
        //    get { return createTime; }
        //    set { createTime = value; }
        //}
        //public int ModifyUserID
        //{
        //    get { return modifyUserID; }
        //    set { modifyUserID = value; }
        //}
        //public DateTime ModifyTime
        //{
        //    get { return modifyTime; }
        //    set { modifyTime = value; }
        //}
        //public int OprRoleID
        //{
        //    get { return oprRoleID; }
        //    set { oprRoleID = value; }
        //}
        //public int OprDeptID
        //{
        //    get { return oprDeptID; }
        //    set { oprDeptID = value; }
        //}
        public string CustCode
        {
            get { return custCode; }
            set { custCode = value; }
        }
        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }
        public string CustShortName
        {
            get { return custShortName; }
            set { custShortName = value; }
        }
        public int CustTypeID
        {
            get { return custTypeID; }
            set { custTypeID = value; }
        }
        public string LegalRep
        {
            get { return legalRep; }
            set { legalRep = value; }
        }

        public string LegalRepTitle
        {
            get { return legalRepTitle; }
            set { legalRepTitle = value; }
        }
        public string RegCode
        {
            get { return regCode; }
            set { regCode = value; }
        }
        public string LegalCode
        {
            get { return legalCode; }
            set { legalCode = value; }
        }

        public string TaxCode
        {
            get { return taxCode; }
            set { taxCode = value; }
        }
        public int CreditLevel
        {
            get { return creditLevel; }
            set { creditLevel = value; }
        }
        public int IsBlacklist
        {
            get { return isBlacklist; }
            set { isBlacklist = value; }
        }
        public decimal RegCap
        {
            get { return regCap; }
            set { regCap = value; }
        }
        public string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }
        public string BankAcct
        {
            get { return bankAcct; }
            set { bankAcct = value; }
        }
        public string RegAddr
        {
            get { return regAddr; }
            set { regAddr = value; }
        }

        public string OfficeAddr
        {
            get { return officeAddr; }
            set { officeAddr = value; }
        }

        public string OfficeAddr2
        {
            get { return officeAddr2; }
            set { officeAddr2 = value; }
        }

        public string OfficeAddr3
        {
            get { return officeAddr3; }
            set { officeAddr3 = value; }
        }

        public string PostAddr
        {
            get { return postAddr; }
            set { postAddr = value; }
        }

        public string PostAddr2
        {
            get { return postAddr2; }
            set { postAddr2 = value; }
        }

        public string PostAddr3
        {
            get { return postAddr3; }
            set { postAddr3 = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }
        public int CustomerStatus
        {
            get { return customerStatus; }
            set { customerStatus = value; }
        }
        public string OfficeTel
        {
            get { return officeTel; }
            set { officeTel = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        public string WebUrl
        {
            get { return weburl; }
            set { weburl = value; }
        }

        public DateTime FirstSigningDate
        {
            get { return firstSigningDate; }
            set { firstSigningDate = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public int CommOper
        {
            get { return commOper; }
            set { commOper = value; }
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public int SourceTypeId
        {
            set { sourceTypeId = value; }
            get { return sourceTypeId; }
        }
        public int CreditLevelId
        {
            set { creditLevelId = value; }
            get { return creditLevelId; }
        }

        public override string GetTableName()
        {
            return "PotCustomer";
        }

        public override string GetColumnNames()
        {
            return "CustID,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,WebUrl,CustomerStatus,CommOper,Fax,CurTypeID," + GetCommonColumnNames() + ",SourceTypeId,CreditLevelId";
        }

        public override string GetUpdateColumnNames()
        {
            return "CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,WebUrl,CustomerStatus,CommOper,Fax,CurTypeID,SourceTypeId,CreditLevelId";
        }
    }
}
