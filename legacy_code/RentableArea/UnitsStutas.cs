using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace RentableArea
{
    /// <summary>
    /// 对单元信息的标志修改
    /// </summary>
    public class UnitsStutas : BasePO
    {
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

        public override String GetTableName()
        {
            return "Unit";
        }

        public override String GetColumnNames()
        {
            return "UnitID,BuildingID,AreaID,FloorID,LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,Trade2ID";
        }

        public override String GetInsertColumnNames()
        {
            return "UnitID,BuildingID,AreaID,FloorID,LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,Trade2ID";
        }

        public override String GetUpdateColumnNames()
        {
            return "UnitStatus";
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

    }
}
