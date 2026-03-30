using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ContractMod
{
    public class ConShopMod:BasePO
    {
        private int conModID = 0;
        private int shopModID = 0;
        private int shopID = 0;
        private int areaID = 0;
        private int buildingID = 0;
        private int brandID = 0;
        private int unitTypeID = 0;
        private int contractID = 0;
        private int floorID = 0;
        private int locationID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private string shopCode = "";
        private string shopName = "";
        private int refID = 0;
        private decimal rentArea = 0;
        private int shopStatus = 0;
        private int shopTypeID = 0;
        private string shopTypeName = "";
        private DateTime shopStartDate = DateTime.Now;
        private DateTime shopEndDate = DateTime.Now;
        private string contactorName = "";
        private string tel = "";

        public override string GetTableName()
        {
            return "ConShopMod";
        }

        public override string GetColumnNames()
        {
            return "ConModID,ShopModID,ShopId,AreaId,BuildingID,BrandID,ContractID,FloorID,LocationID," +
                    "ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,Tel,ShopTypeName,ContactorName";
        }

        public override string GetInsertColumnNames()
        {
            return "ConModID,ShopModID,ShopId,AreaId,BuildingID,BrandID,ContractID,FloorID,LocationID," +
                    "ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,Tel,ContactorName";
        }

        public override string GetUpdateColumnNames()
        {
            return "AreaId,BuildingID,BrandID,ContractID,FloorID,LocationID," +
                    "ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,Tel,ContactorName";
        }

        public override string GetQuerySql()
        {
            return "Select ConModID,ShopModID,ShopId,AreaId,BuildingID,BrandID,ContractID,FloorID,LocationID," +
                    "ShopCode,ShopName,RefID,RentArea,ShopStatus,a.ShopTypeID,ShopStartDate,ShopEndDate,Tel,ShopTypeName,ContactorName From " +
                    "ConShopMod a,ShopType b";
        }

        public int ConModID
        {
            get { return conModID; }
            set { conModID = value; }
        }

        public int ShopModID
        {
            get { return shopModID; }
            set { shopModID = value; }
        }

        public int ShopId
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public int AreaId
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public int BuildingID
        {
            get { return buildingID; }
            set { buildingID = value; }
        }

        public int BrandID
        {
            get { return brandID; }
            set { brandID = value; }
        }

        public int UnitTypeID
        {
            get { return unitTypeID; }
            set { unitTypeID = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int FloorID
        {
            get { return floorID; }
            set { floorID = value; }
        }

        public int LocationID
        {
            get { return locationID; }
            set { locationID = value; }
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

        public string ShopCode
        {
            get { return shopCode; }
            set { shopCode = value; }
        }

        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
        }

        public int RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public decimal RentArea
        {
            get { return rentArea; }
            set { rentArea = value; }
        }

        public int ShopStatus
        {
            get { return shopStatus; }
            set { shopStatus = value; }
        }

        public int ShopTypeID
        {
            get { return shopTypeID; }
            set { shopTypeID = value; }
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

        public string ContactorName
        {
            get { return contactorName; }
            set { contactorName = value; }
        }

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        public string ShopTypeName
        {
            get { return shopTypeName; }
            set { shopTypeName = value; }
        }
    }
}
