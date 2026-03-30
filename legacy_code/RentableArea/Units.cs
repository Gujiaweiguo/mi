using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{

    public class Units : BasePO
    {
        //是否可出租
        public static int RENTABLE_STATUS_INVALID = 0;     // 不可出租
        public static int RENTABLE_STATUS_VALID = 1;         // 可出租

        //单元状态
        public static int UNIT_STATUS_INVALID = 0;     // 无效
        public static int UNIT_STATUS_VALID = 1;         // 有效

        //是否作废  BlankOut
        public static int BLANKOUT_STATUS_INVALID = 0;     // 未出租
        public static int BLANKOUT_STATUS_VALID = 2;         // 已作废
        public static int BLANKOUT_STATUS_LEASEOUT = 1;         // 已出租
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

        //是否作废 
        public static int[] GetBlankOutStatus()
        {
            int[] blankOutStaus = new int[3];
            blankOutStaus[0] = BLANKOUT_STATUS_INVALID;
            blankOutStaus[1] = BLANKOUT_STATUS_VALID;
            blankOutStaus[2] = BLANKOUT_STATUS_LEASEOUT;
            return blankOutStaus;
        }

        public static String GetBlankOutStatusDesc(int blankOutStaus)
        {
            if (blankOutStaus == BLANKOUT_STATUS_INVALID)
            {
                return "Unit_NoLeaseOut";
            }
            if (blankOutStaus == BLANKOUT_STATUS_VALID)
            {
                return "Unit_Nonrentable";
            }
            if (blankOutStaus == BLANKOUT_STATUS_LEASEOUT)
            {
                return "Unit_LeaseOut";
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
        private string unitDesc = null;
        private string unitTel = null;
        private decimal floorArea = 0;
        private decimal useArea = 0;
        private decimal rentArea = 0;
        private string planUrl = null;
        private int isRentable = 1;
        private int unitStatus = 1;
        private int isBlankOut = 1;
        private int trade2ID = 0;
        private int areaSizeID = 0;
        private int trade3ID = 0;
        private string note = null;
        private int areaLevelID = 0;
        private int createUserID = 0;  //创建用户代码
        private DateTime createTime = DateTime.Now;  //创建时间
        private int modifyUserID = 0;  //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0;  //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private int storeid = 0;
        private int shopTypeID = 0;//商铺类型
        private int mapx = 0;
        private int mapy = 0;//可视化坐标
        private string map = "";//可视化形状

        public override String GetTableName()
        {
            return "Unit";
        }

        public override String GetColumnNames()
        {
            return "UnitID,BuildingID,AreaID,FloorID,LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,Trade2ID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,UnitTypeID,StoreID,ShopTypeID,X,Y,Map";
        }

        public override String GetInsertColumnNames()
        {
            return "UnitID,BuildingID,AreaID,FloorID,LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,Trade2ID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,UnitTypeID,StoreID,ShopTypeID,X,Y,Map";
        }

        public override String GetUpdateColumnNames()
        {
            return "AreaID,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,Trade2ID,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,UnitTypeID,StoreID,ShopTypeID,X,Y,Map";
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

        public int AreaSizeID
        {
            get { return areaSizeID; }
            set { areaSizeID = value; }
        }

        public int Trade3ID
        {
            get { return trade3ID; }
            set { trade3ID = value; }
        }

        public int AreaLevelID
        {
            get { return areaLevelID; }
            set { areaLevelID = value; }
        }

        public int Trade2ID
        {
            get { return trade2ID; }
            set { trade2ID = value; }
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
        public int StoreID
        {
            set { storeid = value; }
            get { return storeid; }
        }
        public int ShopTypeID
        {
            set { shopTypeID = value; }
            get { return shopTypeID; }
        }

        public int X
        {
            set { mapx = value; }
            get { return this.mapx; }
        }
        public int Y
        {
            set { mapy = value; }
            get { return this.mapy; }
        }
        public string Map
        {
            set { map = value; }
            get { return this.map; }
        }
    }
}
