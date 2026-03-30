using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    public class Location : BasePO
    {
        public static int LOCATION_STATUS_INVALID = 0;
        public static int LOCATION_STATUS_VALID = 1;

        public static int[] GetLocationStatus()
        {
            int[] locationStaus = new int[2];
            locationStaus[0] = LOCATION_STATUS_VALID;
            locationStaus[1] = LOCATION_STATUS_INVALID;
            return locationStaus;
        }

        public static String GetLocationStatusDesc(int locationStaus)
        {
            if (locationStaus == LOCATION_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";
            }
            if (locationStaus == LOCATION_STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            return "δ֪";
        }

        public String LocationStatusDesc
        {
            get { return GetLocationStatusDesc(this.LocationStatus); }
        }


        private int locationID=0;
        private string locationCode = null;
        private string locationName = null;
        private int locationStatus=1;
        private string note = null;
        private int floorID = 0;
        private int storeid = 0;
        public override String GetTableName()
        {
            return "Location";
        }

        public override String GetColumnNames()
        {
            return "LocationID,LocationCode,LocationName,FloorID,LocationStatus,StoreID";
        }
        public override String GetInsertColumnNames()
        {
            return "LocationID,LocationCode,LocationName,FloorID,LocationStatus,StoreID";
        }
        public override String GetUpdateColumnNames()
        {
            return "LocationCode,LocationName,LocationStatus";
        }

        public int LocationID
        {
            get { return this.locationID; }
            set { this.locationID = value; }
        }
        public string LocationCode
        {
            get { return this.locationCode; }
            set { this.locationCode = value; }
        }
        public string LocationName
        {
            get { return this.locationName; }
            set { this.locationName = value; }
        }
        public int LocationStatus
        {
            get { return this.locationStatus; }
            set { this.locationStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
        public int FloorID
        {
            get { return this.floorID; }
            set { this.floorID = value; }
        }
        public int StoreID
        {
            set { storeid = value; }
            get { return storeid;}
        }
    }
}
