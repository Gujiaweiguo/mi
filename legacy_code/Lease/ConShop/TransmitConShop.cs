using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ConShop
{
    public class TransmitConShop:BasePO
    {
        private int shopID = 0;
        private int shopCode = 0;
        private int contractID = 0;
        private string shopName = "";
        private int shopTypeID = 0;
        private decimal rentArea = 0;
        private int areaID = 0;
        private DateTime shopStartDate = DateTime.Now;
        private DateTime shopEndDate = DateTime.Now;

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public int ShopCode
        {
            get { return shopCode; }
            set { shopCode = value; }
        }

        public int ContractID
        {
            get { return this.contractID; }
            set { this.contractID = value; }
        }

        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
        }

        public int ShopTypeID
        {
            get { return shopTypeID; }
            set { shopTypeID = value; }
        }

        public decimal RentArea
        {
            get { return rentArea; }
            set { rentArea = value; }
        }

        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
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

        public override string GetTableName()
        {
            return "ConShop";
        }

        public override string GetColumnNames()
        {
            return "ShopID,ContractID,ShopName,ShopTypeID,RentArea,AreaID,ShopStartDate,ShopEndDate";
        }

        public override string GetInsertColumnNames()
        {
            return "ShopID,ShopCode,ContractID,ShopName,ShopTypeID,RentArea,AreaID,ShopStartDate,ShopEndDate";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
