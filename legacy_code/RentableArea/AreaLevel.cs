using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace RentableArea
{
    /// <summary>
    /// 面积级别 
    /// </summary>
    public class AreaLevel:BasePO
    {
        public static int AREALEVEL_STATUS_INVALID = 0;
        public static int AREALEVEL_STATUS_VALID = 1;

        public static int[] GetAreaLevelStatus()
        {
            int[] areaLevelStaus = new int[2];
            areaLevelStaus[0] = AREALEVEL_STATUS_INVALID;
            areaLevelStaus[1] = AREALEVEL_STATUS_VALID;
            return areaLevelStaus;
        }

        public static String GetAreaLevelStatusDesc(int areaLevelStaus)
        {
            if (areaLevelStaus == AREALEVEL_STATUS_VALID)
            {
                return "有效";
            }
            if (areaLevelStaus == AREALEVEL_STATUS_INVALID)
            {
                return "无效";
            }
            return "未知";
        }

        public String AreaLevelStatusDesc
        {
            get { return GetAreaLevelStatusDesc(this.AreaLevelStatus); }
        }
        private int areaLevelID = 0;
        private string areaLevelCode = null;
        private string areaLevelName = null;
        private int areaLevelStatus = 1;
        private string note = null;

        public override String GetTableName()
        {
            return "AreaLevel";
        }

        public override String GetColumnNames()
        {
            return "AreaLevelID,AreaLevelCode,AreaLevelDesc,AreaLevelStatus,Note";
        }

        public override string GetQuerySql()
        {
            return "select AreaLevelID,AreaLevelCode,AreaLevelDesc,AreaLevelStatus,Note,'' as AreaLevelStatusName from AreaLevel";
        }

        public override String GetInsertColumnNames()
        {
            return "AreaLevelID,AreaLevelCode,AreaLevelDesc,AreaLevelStatus,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "AreaLevelCode,AreaLevelDesc,AreaLevelStatus,Note";
        }

        public int AreaLevelID
        {
            get { return this.areaLevelID; }
            set { this.areaLevelID = value; }
        }
        public string AreaLevelCode
        {
            get { return this.areaLevelCode; }
            set { this.areaLevelCode = value; }
        }
        public string AreaLevelDesc
        {
            get { return this.areaLevelName; }
            set { this.areaLevelName = value; }
        }
        public int AreaLevelStatus
        {
            get { return this.areaLevelStatus; }
            set { this.areaLevelStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
    }
}
