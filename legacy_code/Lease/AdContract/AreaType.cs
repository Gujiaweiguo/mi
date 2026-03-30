using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    public class AreaType:BasePO
    {
        private int areaTypeID = 0;
        private string areaTypeCode = "";
        private string areaTypeDesc = "";
        private int areaTypeStatus = 0;

        /**
        * 场地类型状态 - 无效
        */
        public static int AREA_TYPE_STATUS_INVALID = 0;
        /**
         * 场地类型状态 - 有效
         */
        public static int AREA_TYPE_STATUS_VALID = 1;

        public static int[] GetAreaTypeStatus()
        {
            int[] areaTypeStatus = new int[2];
            areaTypeStatus[0] = AREA_TYPE_STATUS_VALID;
            areaTypeStatus[1] = AREA_TYPE_STATUS_INVALID;
            return areaTypeStatus;
        }

        public static String GetAreaTypeStatussDesc(int areaTypeStatus)
        {
            if (areaTypeStatus == AREA_TYPE_STATUS_INVALID)
            {
                return "无效";
            }
            if (areaTypeStatus == AREA_TYPE_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }


        public int AreaTypeID
        {
            get { return areaTypeID; }
            set { areaTypeID = value; }
        }
        public string AreaTypeCode
        {
            get { return areaTypeCode; }
            set { areaTypeCode = value; }
        }
        public string AreaTypeDesc
        {
            get { return areaTypeDesc; }
            set { areaTypeDesc = value; }
        }
        public int AreaTypeStatus
        {
            get { return areaTypeStatus; }
            set { areaTypeStatus = value; }
        }

        public override String GetTableName()
        {
            return "AreaType";
        }

        public override String GetColumnNames()
        {
            return "AreaTypeID,AreaTypeCode,AreaTypeDesc,AreaTypeStatus";
        }

        public override String GetInsertColumnNames()
        {
            return "AreaTypeID,AreaTypeCode,AreaTypeDesc,AreaTypeStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "AreaTypeCode,AreaTypeDesc,AreaTypeStatus";
        }
    }
}
