using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace RentableArea
{
    public class UnitInfo:BasePO
    {
        //是否可出租
        public static int RENTABLE_STATUS_INVALID = 0;     // 不可出租
        public static int RENTABLE_STATUS_VALID = 1;         // 可出租

        //单元状态
        public static int UNIT_STATUS_INVALID = 0;     // 无效
        public static int UNIT_STATUS_VALID = 1;         // 有效

        //是否作废  BlankOut
        public static int BLANKOUT_STATUS_INVALID = 0;     // 未作废
        public static int BLANKOUT_STATUS_VALID = 1;         // 已作废

        public static int[] GetRentableStatus()
        {
            int[] rentableStaus = new int[2];
            rentableStaus[0] = RENTABLE_STATUS_VALID;
            rentableStaus[1] = RENTABLE_STATUS_INVALID;
            return rentableStaus;
        }

        public static String GetRentableStatusDesc(int rentableStaus)
        {
            if (rentableStaus == RENTABLE_STATUS_INVALID)
            {
                return "不可出租";
            }
            if (rentableStaus == RENTABLE_STATUS_VALID)
            {
                return "可出租";
            }
            return "未知";
        }

        public String RentableStatusDesc
        {
            get { return GetRentableStatusDesc(IsRentable); }
        }

        //单元状态
        public static int[] GetUnitStatus()
        {
            int[] blankOutStaus = new int[2];
            blankOutStaus[0] = UNIT_STATUS_VALID;
            blankOutStaus[1] = UNIT_STATUS_INVALID;
            return blankOutStaus;
        }

        public static String GetUnitStatusDesc(int blankOutStaus)
        {
            if (blankOutStaus == UNIT_STATUS_INVALID)
            {
                return "无效";
            }
            if (blankOutStaus == UNIT_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        public String UnitStatusDesc
        {
            get { return GetUnitStatusDesc(UnitStatus); }
        }

        //是否作废
        public static int[] GetBlankOutStatus()
        {
            int[] blankOutStaus = new int[2];
            blankOutStaus[1] = BLANKOUT_STATUS_INVALID;
            blankOutStaus[0] = BLANKOUT_STATUS_VALID;
            return blankOutStaus;
        }

        public static String GetBlankOutStatusDesc(int blankOutStaus)
        {
            if (blankOutStaus == BLANKOUT_STATUS_INVALID)
            {
                return "未作废";
            }
            if (blankOutStaus == BLANKOUT_STATUS_VALID)
            {
                return "已作废";
            }
            return "未知";
        }


        public String BlankOutStatusDesc
        {
            get { return GetBlankOutStatusDesc(IsBlankOut); }
        }

        private int unitID = 0;
        private string buildingName = null;
        private string unitTypeName = null;
        private string areaName = null;
        private string floorName = null;
        private string locationName = null;
        private string unitCode = null;
        private string unitDesc = null;
        private string unitTel = null;
        private decimal floorArea = 0;
        private decimal useArea = 0;
        private decimal rentArea = 0;
        private string planUrl = null;
        private int isRentable = 1;
        private int unitStatus = 1;
        private int isBlankOut = 1;
        private string note = null;
        private string tradeName = null;
        private string areaSizeName = null;
        private string areaLevelName = null;
        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "UnitID,BuildingName,UnitTypeName,AreaName,FloorName,LocationName,UnitCode,UnitDesc,UnitTel,FloorArea,UseArea,RentArea,PlanUrl,IsRentable,UnitStatus,IsBlankOut,Note,TradeName,AreaSizeName,AreaLevelName";
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
            return "select UnitID,BuildingName,UnitTypeName,AreaName,FloorName,LocationName,UnitCode,UnitDesc,UnitTel,FloorArea,UseArea,RentArea,"
                    + " PlanUrl,IsRentable,UnitStatus,IsBlankOut,a.Note,TradeName,AreaSizeName,AreaLevelName from Unit a,Building b,UnitType c,Area d,Floors e,"
                    + " Location f,TradeRelation g,AreaSize h,AreaLevel i  where a.BuildingID=b.BuildingID and a.UnitTypeID=c.UnitTypeID and a.AreaID=d.AreaID and a.FloorID=e.FloorID"
                    + " and a.LocationID=f.LocationID and a.Trade3ID=g.TradeID and a.AreaSizeID=h.AreaSizeID and a.AreaLevelID=i.AreaLevelID";
        }

        public int UnitID
        {
            get { return this.unitID; }
            set { this.unitID = value; }
        }
        public string BuildingName
        {
            get { return buildingName; }
            set { buildingName = value; }
        }
        public string UnitTypeName
        {
            get { return this.unitTypeName; }
            set { this.unitTypeName = value; }
        }
        public string AreaName
        {
            get { return this.areaName; }
            set { this.areaName = value; }
        }
        public string FloorName
        {
            get { return this.floorName; }
            set { this.floorName = value; }
        }
        public string LocationName
        {
            get { return this.locationName; }
            set { this.locationName = value; }
        }
        public string UnitCode
        {
            get { return this.unitCode; }
            set { this.unitCode = value; }
        }
        public string UnitDesc
        {
            get { return this.unitDesc; }
            set { this.unitDesc = value; }
        }
        public string UnitTel
        {
            get { return this.unitTel; }
            set { this.unitTel = value; }
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
        public decimal RentArea
        {
            get { return this.rentArea; }
            set { this.rentArea = value; }
        }
        public string PlanUrl
        {
            get { return this.planUrl; }
            set { this.planUrl = value; }
        }
        public int IsRentable
        {
            get { return this.isRentable; }
            set { this.isRentable = value; }
        }
        public int UnitStatus
        {
            get { return this.unitStatus; }
            set { this.unitStatus = value; }
        }
        public int IsBlankOut
        {
            get { return this.isBlankOut; }
            set { this.isBlankOut = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
        public string TradeName
        {
            get { return this.tradeName; }
            set { this.tradeName = value; }
        }
        public string AreaSizeName
        {
            get { return this.areaSizeName; }
            set { this.areaSizeName = value; }
        }
        public string AreaLevelName
        {
            get { return this.areaLevelName; }
            set { this.areaLevelName = value; }
        }
    }
}
