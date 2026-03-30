using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConShop
{
    public class ConShop:BasePO
    {
        private int shopID=0;
        private int areaID=0;
        private int buildingID=0;
        private int brandID=0;
        private int unitTypeID=0;
        private int contractID=0;
        private int floorID=0;
        private int locationID=0;
        private int createUserID=0;
        private DateTime createTime=DateTime.Now.Date;
        private int modifyUserID=0;
        private DateTime modifyTime=DateTime.Now.Date;
        private int oprRoleID=0;
        private int oprDeptID=0;
        private string shopCode="";
        private string shopName="";
        private int refID=0;
        private decimal rentArea=0;
        private int shopStatus=0;
        private int shopTypeID=0;
        private string shopTypeName="";
        private DateTime shopStartDate=DateTime.Now.Date;
        private DateTime shopEndDate=DateTime.Now.Date;
        private string contactorName = "";
        private string tel = "";
        private int storeID = 0;


        /// <summary>
        /// 未审批状态
        /// </summary>
        public static int CONSHOP_TYPE_PAUSE = 0;

        /// <summary>
        /// 商铺有效使用状态
        /// </summary>
        public static int CONSHOP_TYPE_INGEAR = 1;

        /// <summary>
        /// 商铺终止状态
        /// </summary>
        public static int CONSHOP_TYPE_END = 2;
        //public static int POT_SHOP_MAIN = 1;
        //public static int POT_SHOP_ADMIRAL = 2;
        //public static int POT_SHOP_REFINEMENT = 3;

        //public static int[] GetShopTypeStatus()
        //{
        //    int[] shopTypeStatus = new int[3];
        //    shopTypeStatus[0] = POT_SHOP_MAIN;
        //    shopTypeStatus[1] = POT_SHOP_ADMIRAL;
        //    shopTypeStatus[2] = POT_SHOP_REFINEMENT;

        //    return shopTypeStatus;
        //}

        //public static String GetShopTypeStatusDesc(int shopTypeStatus)
        //{
        //    if (shopTypeStatus == POT_SHOP_MAIN)
        //    {
        //        return "POT_SHOP_MAIN";//主力店";
        //    }
        //    if (shopTypeStatus == POT_SHOP_ADMIRAL)
        //    {
        //        return "POT_SHOP_ADMIRAL";// "旗舰店";
        //    }
        //    if (shopTypeStatus == POT_SHOP_REFINEMENT)
        //    {
        //        return "POT_SHOP_REFINEMENT";// "精品店";
        //    }
        //    return "NO";
        //}

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

        public int StoreID
        {
            set { storeID=value; }
            get { return storeID; }
        }
        public override string GetTableName()
        {
            return "ConShop";
        }

        public override string GetColumnNames()
        {
            return "ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID,FloorID,LocationID,CreateUserID,CreateTime,ModifyUserID,"+
                    "ModifyTime,OprRoleID,OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel,StoreID,ShopTypeName";
        }

        public override string GetInsertColumnNames()
        {
            return "ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID,FloorID,LocationID,CreateUserID,CreateTime,ModifyUserID," +
                    "ModifyTime,OprRoleID,OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel,StoreID";
        }

        public override string GetUpdateColumnNames()
        {
            return "AreaId,BuildingID,BrandID,UnitTypeID,FloorID,LocationID,CreateUserID,CreateTime,ModifyUserID," +
                    "ModifyTime,OprRoleID,OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel,StoreID";
        }

        public override string GetQuerySql()
        {
            return "Select ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID,FloorID,LocationID,a.CreateUserID,a.CreateTime,a.ModifyUserID," +
                    "a.ModifyTime,a.OprRoleID,a.OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,a.ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel,StoreID,ShopTypeName From " +
                    "ConShop a,ShopType b";
        }
    }
}
