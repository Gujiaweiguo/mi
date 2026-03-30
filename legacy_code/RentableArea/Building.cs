using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    public class Building: BasePO
    {
        public static int BUILDING_STATUS_VALID = 1;
        public static int BUILDING_STATUS_INVALID = 0;

        public static int[] GetBuildingStatus()
        {
            int[] buildingStaus = new int[2];
            buildingStaus[0] = BUILDING_STATUS_VALID;
            buildingStaus[1] = BUILDING_STATUS_INVALID;
            return buildingStaus;
        }

        public static String GetBuildingStatusDesc(int buildingStaus)
        {
            if (buildingStaus == BUILDING_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";
            }
            if (buildingStaus == BUILDING_STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            return "δ֪";
        }

        public String BuildingStatusDesc
        {
            get { return GetBuildingStatusDesc(this.BuildingStatus); }
        }

        private int buildingID=0;
        private string buildingCode = null;
        private string buildingName = null;
        private string buildingAddr = null;
        private string postCode = null;
        private int buildingStatus=1;
        private string note = null;
        private int storeID = 0;
        private string img = "";

        public override String GetTableName()
        {
            return "Building";
        }

        public override String GetColumnNames()
        {
            return "BuildingID,BuildingCode,BuildingName,BuildingStatus,StoreID,Img";
        }
        public override String GetInsertColumnNames()
        {
            return "BuildingID,BuildingCode,BuildingName,BuildingStatus,StoreID,Img";
        }

        public override String GetUpdateColumnNames()
        {
            return "BuildingCode,BuildingName,BuildingStatus";
        }

        public int BuildingID
        {
            get { return this.buildingID; }
            set { this.buildingID = value; }
        }
        public string BuildingCode
        {
            get { return this.buildingCode; }
            set { this.buildingCode = value; }
        }
        public string BuildingName
        {
            get { return this.buildingName; }
            set { this.buildingName = value; }
        }
        public string BuildingAddr
        {
            get { return this.buildingAddr; }
            set { this.buildingAddr = value; }
        }
        public string PostCode
        {
            get { return this.postCode; }
            set { this.postCode = value; }
        }
        public int BuildingStatus
        {
            get { return this.buildingStatus; }
            set { this.buildingStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
        public int StoreID
        {
            set { storeID = value; }
            get { return storeID; }
        }
        public string Img
        {
            set { this.img = value; }
            get { return this.img; }
        }
    }
}
