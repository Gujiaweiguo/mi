using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace RentableArea
{
    public class SelUnit:BasePO
    {

        public static int RENT_ATUS_VALID = 1;

        public static int BLANKOUT_STATUS_INVALID = 0;     // 未出租
        public static int BLANKOUT_STATUS_VALID = 2;         // 已作废
        public static int BLANKOUT_STATUS_LEASEOUT = 1;         // 已出租
        public static string BLANKOUT_STATUS_LEASEOUTNAME = "已出租";
        //是否作废
        public static int[] GetBlankOutStatus()
        {
            int[] blankOutStaus = new int[2];
            blankOutStaus[0] = BLANKOUT_STATUS_INVALID;
            blankOutStaus[1] = BLANKOUT_STATUS_VALID;
            return blankOutStaus;
        }

        public static String GetBlankOutStatusDesc(int blankOutStaus)
        {
            if (blankOutStaus == BLANKOUT_STATUS_INVALID)
            {
                return "未出租";
            }
            if (blankOutStaus == BLANKOUT_STATUS_VALID)
            {
                return "已作废";
            }
            return "未知";
        }

        private int unitID = 0;
        private int buildingID = 0;
        private int unitTypeID = 0;
        private int areaID = 0;
        private int floorID = 0;
        private int locationID = 0;
        private string unitCode = null;
        private decimal floorArea = 0;
        private decimal useArea = 0;
        private int unitStatus = 1;
        private int trade2ID = 0;

        private string note = null;
        private int areaLevelID = 0;
        private string shopname = "";
        private int shopTypeID = 0;//商铺类型

        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "UnitID,BuildingID,AreaID,FloorID,LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,ShopName,Trade2ID,ShopTypeID";
        }

        public override String GetInsertColumnNames()
        {
            return "";
        }

        public override String GetUpdateColumnNames()
        {
            return "";
        }

        public override String GetQuerySql()
        {
            return "select a.UnitID,a.BuildingID,a.AreaID,a.FloorID,a.LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,ShopName,a.Trade2ID,a.ShopTypeID from Unit a,ConShopUnit b,ConShop c";
        }                    

        public int UnitID
        {
            get { return this.unitID; }
            set { this.unitID = value; }
        }
        public int BuildingID
        {
            get { return this.buildingID; }
            set { this.buildingID = value; }
        }
        public int UnitTypeID
        {
            get { return this.unitTypeID; }
            set { this.unitTypeID = value; }
        }
        public int AreaID
        {
            get { return this.areaID; }
            set { this.areaID = value; }
        }
        public int FloorID
        {
            get { return this.floorID; }
            set { this.floorID = value; }
        }
        public int LocationID
        {
            get { return this.locationID; }
            set { this.locationID = value; }
        }
        public string UnitCode
        {
            get { return this.unitCode; }
            set { this.unitCode = value; }
        }
        public decimal FloorArea
        {
            get { return this.floorArea; }
            set { this.floorArea = value; }
        }
        public decimal UseArea
        {
            get { return this.useArea; }
            set { this.useArea = value; }
        }
        public int UnitStatus
        {
            get { return this.unitStatus; }
            set { this.unitStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }

        public int AreaLevelID
        {
            get { return areaLevelID; }
            set { areaLevelID = value; }
        }
        public string ShopName
        {
            get { return shopname; }
            set { shopname = value; }
        }
        public int Trade2ID
        {
            get { return trade2ID; }
            set { trade2ID = value; }
        }
        public int ShopTypeID
        {
            set { shopTypeID = value; }
            get { return shopTypeID; }
        }
    }
}
