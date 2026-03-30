using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace WorkFlow.WrkFlw
{
    /**
     * 该类暂时未用
     */
    class WrkFlwTransit : BasePO
    {
        /**
         * 转接状态 -- 无效
         */
        public static int TRANSIT_STATUS_INVALID = 0;

        /**
         * 转接状态 -- 有效
         */
        public static int TRANSIT_STATUS_VALID = 1;

        private int preWrkFlwID = 0;
        private int nextWrkFlwID = 0;
        private int transitStatus = 0;


        public override String GetTableName()
        {
            return "WrkFlwTransit";
        }

        public override String GetColumnNames()
        {
            return "PreWrkFlwID,NextWrkFlwID,TransitStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "NextWrkFlwID,TransitStatus";
        }

        public int PreWrkFlwID
        {
            get { return this.preWrkFlwID; }
            set { this.preWrkFlwID = value; }
        }

        public int NextWrkFlwID
        {
            get { return this.nextWrkFlwID; }
            set { this.nextWrkFlwID = value; }
        }
        public int TransitStatus
        {
            get { return this.transitStatus; }
            set { this.transitStatus = value; }
        }
    }
}
