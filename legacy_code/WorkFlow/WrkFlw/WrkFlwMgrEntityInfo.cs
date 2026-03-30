using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwMgrEntityInfo : BasePO
    {
        private int bizGrpID = 0;
        private String bizGrpName = null;
        private int wrkFlwID = 0;
        private String wrkFlwName = null;
        private int nodeID = 0;
        private int sequence = 0;
        private DateTime startTime = DateTime.Now;
        private DateTime completedTime = DateTime.Now;
        private int userID = 0;
        private int voucherID = 0;
        private string voucherHints = null;
        private string voucherMemo = null;
        private int nodeStatus = 0;
        private int preSequence = 0;
        private String imageURL = null;
        private String funcURL = null;

        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "BizGrpID,BizGrpName,WrkFlwID,WrkFlwName,NodeID,Sequence,StartTime,CompletedTime,UserID,VoucherID,VoucherHints,VoucherMemo,NodeStatus,PreSequence,ImageURL,FuncURL";
        }

        public override String GetUpdateColumnNames()
        {
            return "";
        }

        /**
         * ªÒ»°≤È—Øsql”Ôæ‰
         */
        public override String GetQuerySql()
        {
            return "select e.BizGrpID,e.BizGrpName,d.WrkFlwID,d.WrkFlwName,a.NodeID,a.Sequence,a.StartTime,a.CompletedTime,a.UserID,a.VoucherID,"
                + " a.VoucherHints,a.VoucherMemo,a.NodeStatus,a.PreSequence,b.ImageURL,c.FuncURL"
                + " from WrkFlwEntity a, WrkFlwNode b, Func c, WrkFlw d, BizGrp e"
                + " where a.WrkFlwID=b.WrkFlwID and a.NodeID=b.NodeID and b.FuncID=c.FuncID and b.WrkFlwID=d.WrkFlwID and d.BizGrpID=e.BizGrpID";
        }

        public int BizGrpID
        {
            get { return this.bizGrpID; }
            set { this.bizGrpID = value; }
        }
        public String BizGrpName
        {
            get { return this.bizGrpName; }
            set { this.bizGrpName = value; }
        }
        public int WrkFlwID
        {
            get { return this.wrkFlwID; }
            set { this.wrkFlwID = value; }
        }
        public String WrkFlwName
        {
            get { return this.wrkFlwName; }
            set { this.wrkFlwName = value; }
        }
        public int NodeID
        {
            get { return this.nodeID; }
            set { this.nodeID = value; }
        }
        public int Sequence
        {
            get { return this.sequence; }
            set { this.sequence = value; }
        }
        public DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }
        public DateTime CompletedTime
        {
            get { return this.completedTime; }
            set { this.completedTime = value; }
        }
        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }
        public int VoucherID
        {
            get { return this.voucherID; }
            set { this.voucherID = value; }
        }
        public string VoucherHints
        {
            get { return this.voucherHints; }
            set { this.voucherHints = value; }
        }
        public string VoucherMemo
        {
            get { return this.voucherMemo; }
            set { this.voucherMemo = value; }
        }
        public int NodeStatus
        {
            get { return this.nodeStatus; }
            set { this.nodeStatus = value; }
        }
        public int PreSequence
        {
            get { return this.preSequence; }
            set { this.preSequence = value; }
        }
        public String ImageURL
        {
            get { return this.imageURL; }
            set { this.imageURL = value; }
        }
        public String FuncURL
        {
            get { return this.funcURL; }
            set { this.funcURL = value; }
        }
    }
}
