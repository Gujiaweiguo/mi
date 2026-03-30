using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{

    public class Unit : BasePO
    {
        public static int UNIT_STATUS_INVALID = 0;
        public static int UNIT_STATUS_VALID = 1;

        public static int[] GetUnitStatus()
        {
            int[] unitStaus = new int[2];
            unitStaus[0] = UNIT_STATUS_VALID;
            unitStaus[1] = UNIT_STATUS_INVALID;
            return unitStaus;
        }

        public static String GetUnitStatusDesc(int unitStaus)
        {
            if (unitStaus == UNIT_STATUS_INVALID)
            {
                return "无效";
            }
            if (unitStaus == UNIT_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        private int unitID;
        private int buildingID;
        private int unitTypeID;
        private int areaID;
        private int floorID;
        private int locationID;
        private string unitCode;
        private string unitDesc;
        private string unitTel;
        private float floorArea;
        private float useArea;
        private float rentArea;
        private string plan;
        private int isRentable;
        private int unitStatus;
        private int isBlankOut;
        private string note;

        public override String GetTableName()
        {
            return "Unit";
        }

        public override String GetColumnNames()
        {
            return "UnitID,BuildingID,UnitTypeID,AreaID,FloorID,LocationID,UnitCode,UnitDesc,UnitTel,FloorArea,UseArea,RentArea,Plan,IsRentable,UnitStatus,IsBlankOut,Note";
        }

        public override String GetInsertColumnNames()
        {
            return "BuildingID,UnitTypeID,AreaID,FloorID,LocationID,UnitCode,UnitDesc,UnitTel,FloorArea,UseArea,RentArea,Plan,IsRentable,UnitStatus,IsBlankOut,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "BuildingID,UnitTypeID,AreaID,FloorID,LocationID,UnitCode,UnitDesc,UnitTel,FloorArea,UseArea,RentArea,Plan,IsRentable,UnitStatus,IsBlankOut,Note";
        }

        public int UnitID
        {
            get{return this.unitID;}
            set{this.unitID=value;}
        }
        public int BuildingID
        {
            get{return this.buildingID;}
            set{this.buildingID=value;}
        }
        public int UnitTypeID
        {
            get{return this.unitTypeID;}
            set{this.unitTypeID=value;}
        }
        public int AreaID
        {
            get{return this.areaID;}
            set{this.areaID=value;}
        }
        public int FloorID
        {
            get{return this.floorID;}
            set{this.floorID=value;}
        }
        public int LocationID
        {
            get{return this.locationID;}
            set{this.locationID=value;}
        }
        public string UnitCode
        {
            get{return this.unitCode;}
            set{this.unitCode=value;}
        }
        public string UnitDesc
        {
            get{return this.unitDesc;}
            set{this.unitDesc=value;}
        }
        public string UnitTel
        {
            get{return this.unitTel;}
            set{this.unitTel=value;}
        }
        public float FloorArea
        {
            get{return this.floorArea;}
            set{this.floorArea=value;}
        }
        public float UseArea
        {
            get{return this.useArea;}
            set{this.useArea=value;}
        }
        public float RentArea
        {
            get{return this.rentArea;}
            set{this.rentArea=value;}
        }
        public string Plan
        {
            get{return this.plan;}
            set{this.plan=value;}
        }
        public int IsRentable
        {
            get{return this.isRentable;}
            set{this.isRentable=value;}
        }
        public int UnitStatus
        {
            get{return this.unitStatus;}
            set{this.unitStatus=value;}
        }
        public int IsBlankOut
        {
            get{return this.isBlankOut;}
            set{this.isBlankOut=value;}
        }
        public string Note
        {
            get{return this.note;}
            set{this.note=value;}
        }
    }
}
