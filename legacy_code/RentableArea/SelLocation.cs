using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace RentableArea
{
    public class SelLocation:BasePO
    {
        private int buildingid = 0;
        private string buildingcode = "";
        private string buildingname = "";
        private int buildingStatus = 1;

        private int floorid = 0;
        private string floorcode = "";
        private string floorname = "";
        private int floorStatus = 1;

        private int locationid = 0;
        private string locationcode = "";
        private string locationname = "";
        private int locationStatus = 1;
        private int storeid = 0;

        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "BuildingID,FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,StoreID";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID from Building a, Floors b ,Location c";
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
        public int LocationStatus
        {
            get { return this.locationStatus; }
            set { this.locationStatus = value; }
        }

        public int LocationID
        {
            get { return locationid; }
            set { this.locationid = value; }
        }

        public int FloorID
        {
            get { return floorid; }
            set { this.floorid = value; }
        }

        public int BuildingID
        {
            get { return buildingid; }
            set { this.buildingid = value; }
        }
        public int StoreID
        {
            set { storeid = value; }
            get { return storeid; }
        }
    }
}
