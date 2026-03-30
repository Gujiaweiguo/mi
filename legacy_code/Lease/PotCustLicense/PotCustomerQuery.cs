using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PotCustLicense
{
    public class PotCustomerQuery:BasePO
    {
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
        private string webURL = "";
        private DateTime firstSigningDate = DateTime.Now;
        private string note = "";
        private int createUserID=0;
        private int commOper = 0;
        private string userName = "";

        private int custLicenseID = 0;
        private string custLicenseName = "";
        private int custLicenseType = 0;
        private DateTime custLicenseStartDate =  DateTime.Now;
        private DateTime custLicenseEndDate = DateTime.Now;

        
        private string contactorName = "";
        private string title = "";
      //  private string officeTel = null;
        private string mobileTel = "";
        private string eMail = "";
        private int curTypeID = 0;

        private int sourceTypeId = 0;//商户来源
        private int creditLevelId = 0;//等级


        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
       //     return "CustID,CustCode,CustName,CustShortName,CustType,CreateUserID,ContactorName";
            return "CustID,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3," +
                "PostCode,CreateUserID,ContactorName,Title,OfficeTel,MobileTel,EMail,CustLicenseID,CustLicenseName,CustLicenseType,CustLicenseStartDate,"+
                "CustLicenseEndDate,WebURL,CommOper,UserName,Fax,CurTypeID,SourceTypeId,CreditLevelId";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetQuerySql()
        {
            return "select a.CustID,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct," +
                    "a.OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,a.CreateUserID,ContactorName,Title,b.OfficeTel,b.MobileTel,b.EMail,CustLicenseID,CustLicenseName,CustLicenseType," +
                    "CustLicenseStartDate,CustLicenseEndDate,WebURL,CommOper,UserName,a.Fax,CurTypeID,SourceTypeId,CreditLevelId from PotCustomer a left join PotCustContact b on a.CustID=b.CustID left " +
                    "join PotCustLicense c on a.CustID=c.CustID left join Users d on a.CommOper=d.UserID";
        }
        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

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
        public string WebURL
        {
            get { return webURL; }
            set { webURL = value; }
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
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        //public int CustContactID
        //{
        //    get { return custContactID; }
        //    set { custContactID = value; }
        //}

        public string ContactorName
        {
            get { return contactorName; }
            set { contactorName = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        //public string OfficeTel
        //{
        //    get { return officeTel; }
        //    set { officeTel = value; }
        //}
        public string MobileTel
        {
            get { return mobileTel; }
            set { mobileTel = value; }
        }
        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }

        public int CustLicenseID
        {
            get { return custLicenseID; }
            set { custLicenseID = value; }
        }

        public string CustLicenseName
        {
            get { return custLicenseName; }
            set { custLicenseName = value; }
        }

        public int CustLicenseType
        {
            get { return custLicenseType; }
            set { custLicenseType = value; }
        }
        public DateTime CustLicenseStartDate
        {
            get { return custLicenseStartDate; }
            set { custLicenseStartDate = value; }
        }

        public DateTime CustLicenseEndDate
        {
            get { return custLicenseEndDate; }
            set { custLicenseEndDate = value; }
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
    }
}
