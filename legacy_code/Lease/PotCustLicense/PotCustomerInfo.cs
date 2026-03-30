using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.PotCustLicense
{
   public class PotCustomerInfo:BasePO
    {
        private int custid = 0;
        private string custCode = "";
        private string custName = "";
        private string custShortName = "";
        private int custtype = 0;
        private int createuserid = 0;
        private string contactorName="";
        private string officetel ="";
       private string mobiletel = "";
       private int customerStatus = 0;
       private string userName = "";
       private string custTypeName = "";
        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "CustID,CustCode,CustName,CustShortName,CustType,CustTypeName,UserName,ContactorName,OfficeTel,MobileTel,CustomerStatus";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }

       public override string GetQuerySql()
       {
           return "select a.CustID,CustCode,CustName,CustShortName,a.CustTypeID,CustTypeName,UserName," +
            "ContactorName,b.OfficeTel,MobileTel,CustomerStatus from PotCustomer a left join  PotCustContact b on a.CustID=b.CustID "+
            "left join CustType c on a.CustTypeID=c.CustTypeID left join Users d on a.CommOper=d.Userid";
       }

       public int CustID
       {
           get { return custid; }
           set { custid = value; }
       }

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
            get { return custtype; }
            set { custtype = value; }
        }

       public int CreateUserID
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
       public string ContactorName
       {
           get { return contactorName; }
           set { contactorName = value; }
       }
       public string OfficeTel
       {
           get { return officetel; }
           set { officetel = value; }
       }
       public string MobileTel
       {
           get { return mobiletel; }
           set { mobiletel = value; }
       }
       public int CustomerStatus
       {
           get { return customerStatus; }
           set { customerStatus = value; }
       }

       public string UserName
       {
           get { return userName; }
           set { userName = value; }
       }
       public string CustTypeName
       {
           get { return custTypeName; }
           set { custTypeName = value; }
       }
    }
}
