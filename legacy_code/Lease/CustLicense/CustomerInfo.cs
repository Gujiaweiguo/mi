using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.CustLicense
{
    public class CustomerInfo:BasePO
    {
        private int custid = 0;
        private string custCode = "";
        private string custName = "";
        private string custShortName = "";
        private int custtypeID = 0;
        private int createuserid = 0;
        private string contactMan = "";
        private string officetel ="";
        private string mobiletel = "";
        private int customerStatus = 0;
        private string userName = "";
        private string custTypeName = "";
        private string shopTypeName = "";

        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "CustID,CustCode,CustName,CustShortName,CustTypeID,CustTypeName,UserName,ContactorName,OfficeTel,MobileTel,CustomerStatus";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }

       //public override string GetQuerySql()
       //{
       //    //return "select a.CustID,CustCode,CustName,CustShortName,a.CustTypeID,CustTypeName,UserName," +
       //    // "ContactMan,b.OfficeTel,MobileTel,CustomerStatus from Customer a left join  CustContact b on a.CustID=b.CustID "+
       //    // "left join CustType c on a.CustTypeID=c.CustTypeID left join Users d on a.CommOper=d.Userid";

       //    return "select a.CustID,CustCode,CustName,CustShortName,a.CustTypeID,CustTypeName,UserName," +
       //             "ContactMan,b.OfficeTel,MobileTel,CustomerStatus,g.BrandID,g.BrandName,f.ShopTypeID,ShopTypeName " +
       //             "from Customer a " +
       //             "left join  CustContact b on a.CustID=b.CustID " +
       //             "left join CustType c on a.CustTypeID=c.CustTypeID " +
       //             "left join Users d on a.CommOper=d.Userid " +
       //             "left join Contract e on a.CustID = e.CustID " +
       //             "left join ConShop f on e.ContractID = f.ContractID " +
       //             "left join ConShopBrand g on f.BrandID = g.BrandID left join ShopType h on f.ShopTypeID = h.ShopTypeID";
       //}

        public override string GetQuerySql()
        {
            return "SELECT Customer.CustID,CustCode,CustName,CustShortName,Customer.CustTypeID,CustTypeName," +
                    "Users.UserName,CContact.ContactMan FROM Customer left join CustType on Customer.CustTypeID=CustType.CustTypeID " + 
                    "left join Users on Customer.CreateUserID=Users.Userid left join (SELECT CustID,MIN(ContactMan) AS ContactMan " +
                    "FROM CustContact GROUP BY CustID) AS CContact  ON (CContact.CustID = Customer.CustID)";
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
       public int CustTypeID
        {
            get { return custtypeID; }
            set { custtypeID = value; }
        }

       public int CreateUserID
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
        public string ContactMan
       {
           get { return contactMan; }
           set { contactMan = value; }
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
        public string ShopTypeName
       {
           get { return shopTypeName; }
           set { shopTypeName = value; }
       }

    }
}
