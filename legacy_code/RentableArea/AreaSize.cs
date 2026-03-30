using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    public class AreaSize:BasePO
    {
        public static int AREASIZE_STATUS_INVALID = 0;
        public static int AREASIZE_STATUS_VALID = 1;

        public static int[] GetAreaSizeStatus()
        {
            int[] areasizeStaus = new int[2];
            areasizeStaus[0] = AREASIZE_STATUS_VALID;
            areasizeStaus[1] = AREASIZE_STATUS_INVALID;
            return areasizeStaus;
        }

        public static String GetAreaSizeStatusDesc(int areasizeStaus)
        {
            if (areasizeStaus == AREASIZE_STATUS_INVALID)
            {
                return "无效";
            }
            if (areasizeStaus == AREASIZE_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        public String AreaSizeStatusDesc
        {
            get { return GetAreaSizeStatusDesc(this.AreaSizeStatus); }
        }


        private int areasizeID = 0;
        private string areasizeCode = null;
        private string areasizeName = null;
        private int areasizeStatus = 1;
        private string note = null;

        public override String GetTableName()
        {
            return "AreaSize";
        }

        public override String GetColumnNames()
        {
            return "AreaSizeID,AreaSizeCode,AreaSizeName,AreaSizeStatus,Note";
        }

        public override String GetInsertColumnNames()
        {
            return "AreaSizeID,AreaSizeCode,AreaSizeName,AreaSizeStatus,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "AreaSizeCode,AreaSizeName,AreaSizeStatus,Note";
        }

        public int AreaSizeID
        {
            get { return this.areasizeID; }
            set { this.areasizeID = value; }
        }
        public string AreaSizeCode
        {
            get { return this.areasizeCode; }
            set { this.areasizeCode = value; }
        }
        public string AreaSizeName
        {
            get { return this.areasizeName; }
            set { this.areasizeName = value; }
        }
        public int AreaSizeStatus
        {
            get { return this.areasizeStatus; }
            set { this.areasizeStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
    }
}
