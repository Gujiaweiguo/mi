using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace WorkFlow.WrkFlw
{
    /**
     * 该类暂时未用
     */
    class WrkFlwTransfer : BasePO
    {
        /**
         * 转接状态 -- 无效
         */
        public static int TRANSFER_STATUS_INVALID = 0;

        /**
         * 转接状态 -- 有效
         */
        public static int TRANSFER_STATUS_VALID = 1;

        private int wrkFlwID = 0;
        private int nextWrkFlwID = 0;
        private int transferStatus = 0;


        public override String GetTableName()
        {
            return "WrkFlwTransfer";
        }

        public override String GetColumnNames()
        {
            return "WrkFlwID,NextWrkFlwID,TranferStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "NextWrkFlwID,TransitStatus";
        }

        public int WrkFlwID
        {
            get { return this.wrkFlwID; }
            set { this.wrkFlwID = value; }
        }

        public int NextWrkFlwID
        {
            get { return this.nextWrkFlwID; }
            set { this.nextWrkFlwID = value; }
        }
        public int TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }
    }
}
