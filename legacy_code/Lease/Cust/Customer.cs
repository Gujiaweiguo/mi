using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.Customer
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class Customer : CommonInfoPO
    {
        private int custID = 0;
        private string custCode = null;
        private string custName = null;
        private string custShortName = null;
        private int custType = 0;
        private string legalRep = null;
        private string legalRepTitle = null;
        private string regCode = null;
        private string legalCode = null;
        private string taxCode = null;
        private int creditLevel = 0;
        private int isBlacklist = 0;
        private decimal regCap = 0;
        private string bankName = null;
        private string bankAcct = null;
        private string regAddr = null;
        private string officeAddr = null;
        private string postAddr = null;
        private string postCode = null;
        private int customerStatus = 0;
        private string officeTel = null;
        private string fax = null;
        private string web = null;
        private DateTime firstSigningDate = DateTime.Now;
        private string note = null;
        private int invoiceID = 0;
        private int custTypeID = 0;

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
        public int CustType
        {
            get { return custType; }
            set { custType = value; }
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
        public string PostAddr
        {
            get { return postAddr; }
            set { postAddr = value; }
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
        public string Web
        {
            get { return web; }
            set { web = value; }
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

        public override string GetTableName()
        {
            return "Customer";
        }

        public override string GetColumnNames()
        {
            return "CustID,CustCode,CustName,CustShortName,CustType,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,PostAddr,PostCode,Web," + GetCommonColumnNames();
        }

        public override string GetUpdateColumnNames()
        {
            return "CustCode,CustName,CustShortName,CustType,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,PostAddr,PostCode,Web";
        }


    }
}
