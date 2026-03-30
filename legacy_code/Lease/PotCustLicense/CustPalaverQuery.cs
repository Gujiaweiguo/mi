using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PotCustLicense
{
    public class CustPalaverQuery:BasePO
    {
        private string custCode = "";
        private string custName = "";
        private string custShortName = "";
        private int shopTypeID = 0;
        private string potShopName = "";
        private string mainBrand = "";
        private DateTime shopStartDate = DateTime.Now;
        private DateTime shopEndDate = DateTime.Now;
        private int areaID = 0;
        private decimal rentArea ;
        private decimal rentalPrice;
        private string note = "";
        private string userName = "";


        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "CustCode,CustName,CustShortName,ShopTypeID,PotShopName,MainBrand,ShopStartDate,ShopEndDate,AreaID,RentalPrice,Note,RentArea,UserName";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select a.CustCode,a.CustName,a.CustShortName,b.ShopTypeID,b.PotShopName,b.MainBrand,b.ShopStartDate,b.ShopEndDate,c.AreaID,b.RentalPrice,b.Note,b.RentArea,UserName  from PotCustomer a inner join PotShop b on a.CustID=b.CustID  inner join Area c on b.AreaID=c.AreaID left join Users d on a.CommOper=d.Userid";
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
        public int ShopTypeID
        {
            get { return shopTypeID; }
            set { shopTypeID = value; }
        }
        public string PotShopName
        {
            get { return potShopName; }
            set { potShopName = value; }
        }
        public string MainBrand
        {
            get { return mainBrand; }
            set { mainBrand = value; }
        }
        public DateTime ShopStartDate
        {
            get { return shopStartDate; }
            set { shopStartDate = value; }
        }
        public DateTime ShopEndDate
        {
            get { return shopEndDate; }
            set { shopEndDate = value; }
        }
        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }
        public decimal RentArea
        {
            get { return rentArea; }
            set { rentArea = value; }
        }
        public decimal RentalPrice
        {
            get { return rentalPrice; }
            set { rentalPrice = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
