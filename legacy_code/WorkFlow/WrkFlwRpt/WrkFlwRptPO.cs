using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace WorkFlow.WrkFlwRpt
{
    public class WrkFlwRptPO : BasePO
    {
        private int wrkFlwID = 0;
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
        private int longestDelay = 0;
        private string userName = "";
        private string roleName = "";
        private int stop = 0;
        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "WrkFlwID,NodeID,Sequence,StartTime,CompletedTime,UserID,VoucherID,VoucherHints,VoucherMemo,NodeStatus,PreSequence,UserName,LongestDelay";
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
            return "select a.WrkFlwID,a.NodeID,a.Sequence,a.StartTime,a.CompletedTime,a.UserID,a.VoucherID,"
                + " a.VoucherHints,a.VoucherMemo,a.NodeStatus,a.PreSequence,b.LongestDelay,d.UserName,f.RoleName"
                + " from WrkFlwentity a, WrkFlwNode b, Func c,Users d,Role f"
                + " where a.WrkFlwID=b.WrkFlwID and a.NodeID=b.NodeID and b.FuncID=c.FuncID and b.RoleID = f.RoleID and a.UserID = d.UserID";
        }

        public int WrkFlwID
        {
            get { return this.wrkFlwID; }
            set { this.wrkFlwID = value; }
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
        public int LongestDelay
        {
            get { return longestDelay; }
            set { longestDelay = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }
        public int Stop
        {
            get { return stop; }
            set { stop = value; }
        }
    }
}
