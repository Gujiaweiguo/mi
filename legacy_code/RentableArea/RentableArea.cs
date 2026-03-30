using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace RentableArea
{
    public class RentableArea:BasePO
    {
        private int buildingcode = 0;
        private string buildingname = "";

        private int floorcode = 0;
        private string floorname = "";

        private int locationcode = 0;
        private string locationname = "";


        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName from Building a, Floors b ,Location c";
        }

        public int BuildingCode
        {
            set { buildingcode = value; }
            get { return buildingcode; }
        }

        public string BuildingName
        {
            set { buildingname = value; }
            get { return buildingname; }
        }

        public int FloorCode
        {
            set { floorcode = value; }
            get { return floorcode; }
        }

        public string FloorName
        {
            set { floorname = value; }
            get { return floorname; }
        }

        public int LocationCode
        {
            set { locationcode = value; }
            get { return locationcode; }
        }

        public string LocationName
        {
            set { locationname = value; }
            get { return locationname; }
        }
    }
}
