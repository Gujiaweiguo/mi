using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace WorkFlow.WrkFlw
{
    public class QueryBizGrp: BasePO
    {
          /**
         * 业务组状态 - 无效
         */
        public static int BIZ_GRP_STATUS_INVALID = 0;
        /**
         * 业务组状态 - 有效
         */
        public static int BIZ_GRP_STATUS_VALID = 1;


        public static int[] GetBizGrpStatus()
        {
            int[] bizGrpStatus = new int[2];
            bizGrpStatus[0] = BIZ_GRP_STATUS_VALID;
            bizGrpStatus[1] = BIZ_GRP_STATUS_INVALID;
            return bizGrpStatus;
        }

        public static String GetBizGrpStatusDesc(int bizGrpStatus)
        {
            if (bizGrpStatus == BIZ_GRP_STATUS_INVALID)
            {
                return "无效";
            }
            if (bizGrpStatus == BIZ_GRP_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        private int bizGrpID = 0;
        private string bizGrpCode = "";
        private string bizGrpName = "";
        private int bizGrpStatus = 0;
        private string note = "";
        public override String GetTableName()
        {
            return "BizGrp";
        }

        public override String GetColumnNames()
        {
            return "BizGrpID,BizGrpCode,BizGrpName,BizGrpStatus,Note";
        }

        public override String GetInsertColumnNames()
        {
            return "BizGrpCode,BizGrpName,BizGrpStatus,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "BizGrpCode,BizGrpName,BizGrpStatus,Note";
        }

        public int BizGrpID
        {
            get { return this.bizGrpID; }
            set { this.bizGrpID = value; }
        }

        public string BizGrpCode
        {
            get { return this.bizGrpCode; }
            set { this.bizGrpCode = value; }
        }

        public string BizGrpName
        {
            get { return this.bizGrpName; }
            set { this.bizGrpName = value; }
        }

        public int BizGrpStatus
        {
            get { return this.bizGrpStatus; }
            set { this.bizGrpStatus = value; }
        }

        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
        //---------------------描述性属性-------------------------------
        public String BizGrpStatusDesc
        {
            get { return GetBizGrpStatusDesc(this.BizGrpStatus); }
        }
    }
}
