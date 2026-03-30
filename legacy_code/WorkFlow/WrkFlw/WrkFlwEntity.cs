using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
using WorkFlow.Uiltil;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwEntity : BasePO
    {
        /**
         * 节点状态 - 正常流转待处理
         */
        public static int NODE_STATUS_NORMAL_PENDING = 1;
        /**
         * 节点状态 - 上级审批待处理
         */
        public static int NODE_STATUS_MGR_PENDING = 2;
        /**
         * 节点状态 - 驳回待处理
         */
        public static int NODE_STATUS_REJECT_PENDING = 3;
        /**
         * 节点状态 - 正常流转完成
         */
        public static int NODE_STATUS_NORMAL_COMPLETED = 4;
        /**
         * 节点状态 - 上级审批完成
         */
        public static int NODE_STATUS_MGR_COMPLETED = 5;
        /**
         * 节点状态 - 正常驳回处理完
         */
        public static int NODE_STATUS_REJECT_COMPLETED = 6;
        /**
         * 节点状态 - 上级驳回处理完
         */
        public static int NODE_STATUS_MGR_REJECT_COMPLETED = 7;
        /**
         * 节点状态 - 工作流正常流转完成（该节点为工作流末节点）
         */
        public static int NODE_STATUS_WRKFLW_NORMAL_COMPLETED = 8;
        /**
         * 节点状态 - 节点草稿状态
         */
        public static int NODE_STATUS_WRKFLW_DRAFT = 9;
        /**
         * 节点状态 - 作废状态
         */
        public static int NODE_STATUS_WRKFLW_BLANK_OUT = 10;

        /**
         * 节点状态 - 驳回状态
         */
        public static int NODE_STATUS_WRKFLW_OVERRULE = 11;

        private int wrkFlwID = 0;
        private int nodeID = 0;
        private int sequence = 0;
        private DateTime startTime = DateTime.Now;
        private DateTime completedTime = DateTime.Now;
        private int deptID = 0;
        private int userID = 0;
        private int voucherID = 0;
        private string voucherHints = null;
        private string voucherMemo = null;
        private int roleID = 0;
        private int nodeStatus = 0;
        private int preSequence = 0;
        private String preVoucherMemo = null;

        public WrkFlwEntity()
        {
        }

        public WrkFlwEntity(int wrkFlwID, int nodeID, int sequence)
        {
            this.wrkFlwID = wrkFlwID;
            this.nodeID = nodeID;
            this.sequence = sequence;
        }

        public override String GetTableName()
        {
            return "WrkFlwEntity";
        }

        public override String GetColumnNames()
        {
            return "WrkFlwID,NodeID,Sequence,StartTime,CompletedTime,DeptID,UserID,VoucherID,VoucherHints,VoucherMemo,RoleID,NodeStatus,PreSequence,PreVoucherMemo";
        }

        public override String GetInsertColumnNames()
        {
            return "WrkFlwID,NodeID,Sequence,StartTime,CompletedTime,DeptID,UserID,VoucherID,VoucherHints,VoucherMemo,RoleID,NodeStatus,PreSequence,PreVoucherMemo";
        }

        public override String GetUpdateColumnNames()
        {
            return "CompletedTime,VoucherHints,VoucherMemo,NodeStatus,UserID";
        }

        public void SetVoucherInfo(VoucherInfo voucherInfo)
        {
            this.VoucherID = voucherInfo.VoucherID;
            this.VoucherHints = voucherInfo.VoucherHints;
            this.VoucherMemo = voucherInfo.VoucherMemo;
            this.PreVoucherMemo = voucherInfo.VoucherMemo;
            this.DeptID = voucherInfo.DeptID;
            this.UserID = voucherInfo.UserID;
        }

        public static int[] GetNodeStatus()
        {
            int[] nodeStatus = new int[9];
            nodeStatus[0] = NODE_STATUS_NORMAL_PENDING;
            nodeStatus[1] = NODE_STATUS_MGR_PENDING;
            nodeStatus[2] = NODE_STATUS_REJECT_PENDING;
            nodeStatus[3] = NODE_STATUS_NORMAL_COMPLETED;
            nodeStatus[4] = NODE_STATUS_MGR_COMPLETED;
            nodeStatus[5] = NODE_STATUS_REJECT_COMPLETED;
            nodeStatus[6] = NODE_STATUS_MGR_REJECT_COMPLETED;
            nodeStatus[7] = NODE_STATUS_WRKFLW_NORMAL_COMPLETED;
            nodeStatus[8] = NODE_STATUS_WRKFLW_OVERRULE;
            return nodeStatus;
        }

        public static String GetNodeStatusDesc(int nodeStatus)
        {
            if (nodeStatus == NODE_STATUS_NORMAL_PENDING)
            {
                return "正常流转待处理";
            }
            if (nodeStatus == NODE_STATUS_MGR_PENDING)
            {
                return "上级审批待处理";
            }
            if (nodeStatus == NODE_STATUS_REJECT_PENDING)
            {
                return "驳回待处理";
            }
            if (nodeStatus == NODE_STATUS_NORMAL_COMPLETED)
            {
                return "正常流转完成";
            }
            if (nodeStatus == NODE_STATUS_MGR_COMPLETED)
            {
                return "上级审批完成";
            }
            if (nodeStatus == NODE_STATUS_REJECT_COMPLETED)
            {
                return "正常驳回处理完";
            }
            if (nodeStatus == NODE_STATUS_MGR_REJECT_COMPLETED)
            {
                return "上级驳回处理完";
            }
            if (nodeStatus == NODE_STATUS_WRKFLW_NORMAL_COMPLETED)
            {
                return "工作流正常流转完成";
            }
            if (nodeStatus == NODE_STATUS_WRKFLW_OVERRULE)
            {
                return "驳回";
            }

            return "未知";
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
        public int DeptID
        {
            get { return this.deptID; }
            set { this.deptID = value; }
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
        public int RoleID
        {
            get { return this.roleID; }
            set { this.roleID = value; }
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
        public String PreVoucherMemo
        {
            get { return this.preVoucherMemo; }
            set { this.preVoucherMemo = value; }
        }

        //---------------------描述性属性-------------------------------
        public String NodeStatusDesc
        {
            get { return GetNodeStatusDesc(this.NodeStatus); }
        }
    }
}
