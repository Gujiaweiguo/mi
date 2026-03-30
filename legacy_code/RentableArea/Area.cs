using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    public class Area: BasePO
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
                return "WrkFlw_Disabled";
            }
            if (areaStaus == AREA_STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            return "δ֪";
        }

        public String AreaStatusDesc
        {
            get { return GetAreaStatusDesc(this.AreaStatus); }
        }

        private int areaID=0;
        private string areaCode=null;
        private string areaName=null;
        private int areaStatus=1;
        private string note=null;
        private int storeID = 0;

        public override String GetTableName()
        {
            return "Area";
        }

        public override String GetColumnNames()
        {
            return "AreaID,AreaCode,AreaName,AreaStatus,Note,StoreID";
        }

        public override String GetInsertColumnNames()
        {
            return "AreaCode,AreaName,AreaStatus,Note,StoreID";
        }

        public override String GetUpdateColumnNames()
        {
            return "AreaCode,AreaName,AreaStatus,Note,StoreID";
        }

        public int AreaID
        {
            get { return this.areaID; }
            set { this.areaID = value; }
        }
        public string AreaCode
        {
            get { return this.areaCode; }
            set { this.areaCode = value; }
        }
        public string AreaName
        {
            get { return this.areaName; }
            set { this.areaName = value; }
        }
        public int AreaStatus
        {
            get { return this.areaStatus; }
            set { this.areaStatus = value; }
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
    }
}
