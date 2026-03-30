using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    /// <summary>
    /// 租金水平
    /// </summary>
    public class RentLevel:BasePO
    {
        public static int RENTLEVEL_STATUS_INVALID = 0;
        public static int RENTLEVEL_STATUS_VALID = 1;

        public static int[] GetRentLevelStatus()
        {
            int[] rentLevelStaus = new int[2];
            rentLevelStaus[0] = RENTLEVEL_STATUS_VALID;
            rentLevelStaus[1] = RENTLEVEL_STATUS_INVALID;
            return rentLevelStaus;
        }

        public static String GetRentLevelStatusDesc(int rentLevelStaus)
        {
            if (rentLevelStaus == RENTLEVEL_STATUS_INVALID)
            {
                return "无效";
            }
            if (rentLevelStaus == RENTLEVEL_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        public String RentLevelStatusDesc
        {
            get { return GetRentLevelStatusDesc(this.RentLevelStatus); }
        }


        private int rentLevelID = 0;
        private string rentLevelCode = null;
        private string rentLevelDesc = null;
        private int rentLevelStatus = 1;
        private string note = null;

        public override String GetTableName()
        {
            return "RentLevel";
        }

        public override String GetColumnNames()
        {
            return "RentLevelID,RentLevelCode,RentLevelDesc,RentLevelStatus,Note";
        }

        public override string GetQuerySql()
        {
            return "select RentLevelID,RentLevelCode,RentLevelDesc,RentLevelStatus,Note,'' as RentLevelStatusName from RentLevel";
        }

        public override String GetInsertColumnNames()
        {
            return "RentLevelID,RentLevelCode,RentLevelDesc,RentLevelStatus,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "RentLevelCode,RentLevelDesc,RentLevelStatus,Note";
        }

        public int RentLevelID
        {
            get { return this.rentLevelID; }
            set { this.rentLevelID = value; }
        }
        public string RentLevelCode
        {
            get { return this.rentLevelCode; }
            set { this.rentLevelCode = value; }
        }
        public string RentLevelDesc
        {
            get { return this.rentLevelDesc; }
            set { this.rentLevelDesc = value; }
        }
        public int RentLevelStatus
        {
            get { return this.rentLevelStatus; }
            set { this.rentLevelStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
    }
}
