using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwDef : BasePO
    {
        private String bizGrpName = null;
        private int wrkFlwID = 0;
        private String wrkFlwName = null;
        private int nodeID = 0;
        private String nodeName = null;
        private int initVoucher = 0;
        private int wrkStep = 0;
        private int smtToMgr = 0;
        private int validAfterConfirm = 0;
        private int printAfterConfirm = 0;
        private String imageURL = null;
        private String funcURL = null;
        private int bizGrpID = 0;

        private int roleID = 0;

        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "BizGrpName, WrkFlwID, WrkFlwName, NodeID, NodeName, InitVoucher, WrkStep, SmtToMgr, ValidAfterConfirm, PrintAfterConfirm, ImageURL, FuncURL";
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
            return "select a.BizGrpName, b.WrkFlwID, b.WrkFlwName, c.NodeID, c.NodeName, b.InitVoucher, "
                + "c.WrkStep, c.SmtToMgr, c.ValidAfterConfirm, c.PrintAfterConfirm, c.ImageURL, d.FuncURL"
                + " from BizGrp a, WrkFlw b, WrkFlwNode c, Func d"
                + " where a.BizGrpID=b.BizGrpID  and b.WrkFlwID=c.WrkFlwID and c.FuncID=d.FuncID"
                + " and a.BizGrpStatus=" + BizGrp.BIZ_GRP_STATUS_VALID
                + " and b.WrkFlwStatus=" + WrkFlw.WRK_FLW__STATUS_VALID
                + " and c.RoleID=" + this.roleID + "and a.BizgrpID=" + bizGrpID;
        }

        public void SetRoleID(int roleID,int bizGrpID)
        {
            this.roleID = roleID;
            this.bizGrpID = bizGrpID;
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
        public String NodeName
        {
            get { return this.nodeName; }
            set { this.nodeName = value; }
        }
        public int InitVoucher
        {
            get { return this.initVoucher; }
            set { this.initVoucher = value; }
        }
        public int WrkStep 
        {
            get { return this.wrkStep; }
            set { this.wrkStep = value; }
        }
        public int SmtToMgr 
        {
            get { return this.smtToMgr; }
            set { this.smtToMgr = value; }
        }
        public int ValidAfterConfirm
        {
            get { return this.validAfterConfirm; }
            set { this.validAfterConfirm = value; }
        }
        public int PrintAfterConfirm
        {
            get { return this.printAfterConfirm; }
            set { this.printAfterConfirm = value; }
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
        public int BizGrpID
        {
            get { return this.bizGrpID; }
            set { this.bizGrpID = value; }
        }

    }
}
