using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace RentableArea
{
    public class SelFloors:BasePO
    {
        private string buildingcode = "";
        private string buildingname = "";
        private int buildingStatus = 1;

        private string floorcode = "";
        private string floorname = "";
        private int floorStatus = 1;

        private string locationcode = "";
        private string locationname = "";

        private int storeid = 0;
        

        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "BuildingCode,BuildingName,FloorCode,FloorName,BuildingStatus,FloorStatus,StoreID";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select BuildingCode,BuildingName,FloorCode,FloorName,BuildingStatus,FloorStatus,b.StoreID from Building a, Floors b";
        }

        public string BuildingCode
        {
            set { buildingcode = value; }
            get { return buildingcode; }
        }

        public string BuildingName
        {
            set { buildingname = value; }
            get { return buildingname; }
        }

        public string FloorCode
        {
            set { floorcode = value; }
            get { return floorcode; }
        }

        public string FloorName
        {
            set { floorname = value; }
            get { return floorname; }
        }

        public string LocationCode
        {
            set { locationcode = value; }
            get { return locationcode; }
        }

        public string LocationName
        {
            set { locationname = value; }
            get { return locationname; }
        }

        public int BuildingStatus
        {
            get { return this.buildingStatus; }
            set { this.buildingStatus = value; }
        }
        public int FloorStatus
        {
            get { return this.floorStatus; }
            set { this.floorStatus = value; }
        }
        public int StoreID
        {
            set { storeid = value; }
            get { return storeid; }
        }
    }
}
