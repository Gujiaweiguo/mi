using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    public class Floors : BasePO
    {
        public static int FLOOR_STATUS_INVALID = 0;
        public static int FLOOR_STATUS_VALID = 1;
        

        public static int[] GetFloorStatus()
        {
            int[] floorStaus = new int[2];
            floorStaus[0] = FLOOR_STATUS_VALID;
            floorStaus[1] = FLOOR_STATUS_INVALID;
            return floorStaus;
        }

        public static String GetFloorStatusDesc(int floorStaus)
        {
            if (floorStaus == FLOOR_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";
            }
            if (floorStaus == FLOOR_STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            return "δ֪";
        }

        public String FloorStatusDesc
        {
            get { return GetFloorStatusDesc(this.FloorStatus); }
        }


        private int floorID=0;
        private string floorCode=null;
        private string floorName = null;
        private int floorStatus=1;
        private string note=null;
        private int buildingID = 0;
        private int storeid = 0;
        private string img = "";//楼层缩微图

        public override String GetTableName()
        {
            return "Floors";
        }

        public override String GetColumnNames()
        {
            return "FloorID,FloorCode,FloorName,BuildingID,FloorStatus,StoreID,Img";
        }

        public override String GetUpdateColumnNames()
        {
            return "FloorCode,FloorName,FloorStatus";
        }
        public override String GetInsertColumnNames()
        {
            return "FloorID,FloorCode,FloorName,BuildingID,FloorStatus,StoreID,Img";
        }
        public int FloorID
        {
            get { return this.floorID; }
            set { this.floorID = value; }
        }
        public string FloorCode
        {
            get { return this.floorCode; }
            set { this.floorCode = value; }
        }
        public string FloorName
        {
            get { return this.floorName; }
            set { this.floorName = value; }
        }
        public int FloorStatus
        {
            get { return this.floorStatus; }
            set { this.floorStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
        public int BuildingID
        {
            get { return this.buildingID; }
            set { this.buildingID = value; }
        }
        public int StoreID
        {
            set { storeid = value; }
            get { return storeid; }
        }

        public string Img
        {
            set { this.img = value; }
            get { return this.img; }
        }
    }
}
