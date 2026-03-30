using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace LeaseAreaManage
{
    public class Floor : BasePO
    {
        public static int AREA_STATUS_INVALID = 0;
        public static int AREA_STATUS_VALID = 1;

        public static int[] GetAreaStatus()
        {
            int[] areaStaus = new int[2];
            areaStaus[0] = AREA_STATUS_VALID;
            areaStaus[1] = AREA_STATUS_INVALID;
            return areaStaus;
        }

        public static String GetAreaStatusDesc(int areaStaus)
        {
            if (areaStaus == AREA_STATUS_INVALID)
            {
                return "无效";
            }
            if (areaStaus == AREA_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        private int floorID;
        private string floorCode;
        private string floorName;
        private int floorStatus;
        private string note;

        public override String GetTableName()
        {
            return "Floor";
        }

        public override String GetColumnNames()
        {
            return "FloorID,FloorCode,FloorName,FloorStatus,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "FloorID,FloorCode,FloorName,FloorStatus,Note";
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
    }
}
