using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace WorkFlow.WrkFlw
{
    public class BizGrp : BasePO
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
                return "BizGrp_NO";
            }
            if (bizGrpStatus == BIZ_GRP_STATUS_VALID)
            {
                return "BizGrp_YES";
            }
            return "未知";
        }

        private int bizGrpID = 0;
        private string bizGrpCode = "";
        private string bizGrpName = "";
        private int bizGrpStatus = 0;
        private string note = "";
        private string bizGrpStatusName = "";
        public override String GetTableName()
        {
            return "BizGrp";
        }

        public override String GetColumnNames()
        {
            return "BizGrpID,BizGrpCode,BizGrpName,(case when BizGrpStatus=1 then '有效' when BizGrpStatus=0 then '无效' else '未知' end) as BizGrpStatusName,BizGrpStatus,Note";
        }

        public override string GetQuerySql()
        {
            return "select BizGrpID,BizGrpCode,BizGrpName,BizGrpStatus, '' as BizGrpStatusName,BizGrpStatus,Note from BizGrp";
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

        public string BizGrpStatusName
        {
            get { return this.bizGrpStatusName; }
            set { this.bizGrpStatusName = value; }
        }
        //---------------------描述性属性-------------------------------
        public String BizGrpStatusDesc
        {
            get { return GetBizGrpStatusDesc(this.BizGrpStatus); }
        }
    }
}
