using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
using BaseInfo;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwNode : BasePO
    {
        /**
         * 是否可送上级审批 - 否
         */
        public static int SMT_TO_MGR_NO = 0;
        /**
         * 是否可送上级审批 - 是
         */
        public static int SMT_TO_MGR_YES = 1;

        public static int[] GetSmtToMgr()
        {
            int[] smtToMgr = new int[2];
            smtToMgr[0] = SMT_TO_MGR_NO;
            smtToMgr[1] = SMT_TO_MGR_YES;
            return smtToMgr;
        }

        public static String GetSmtToMgrDesc(int smtToMgr)
        {
            if (smtToMgr == SMT_TO_MGR_NO)
            {
                return "PENALTY_TYPE_NO";
            }
            if (smtToMgr == SMT_TO_MGR_YES)
            {
                return "PENALTY_TYPE_YES";
            }

            return "未知";

        }

        /**
         * 确认后数据是否生效 - 不生效
         */
        public static int VALID_AFTER_CONFIRM_NO = 0;
        /**
         * 确认后数据是否生效 - 生效
         */
        public static int VALID_AFTER_CONFIRM_YES = 1;

        public static int[] GetValidAfterConfirm()
        {
            int[] validAfterConfirm = new int[2];
            validAfterConfirm[0] = VALID_AFTER_CONFIRM_NO;
            validAfterConfirm[1] = VALID_AFTER_CONFIRM_YES;
            return validAfterConfirm;
        }

        public static String GetValidAfterConfirmDesc(int validAfterConfirm)
        {
            if (validAfterConfirm == VALID_AFTER_CONFIRM_NO)
            {
                return "PENALTY_TYPE_NO";
            }
            if (validAfterConfirm == VALID_AFTER_CONFIRM_YES)
            {
                return "PENALTY_TYPE_YES";
            }

            return "未知";
        }

        /**
         * 确认后数据是否可打印 - 不可以打印
         */
        public static int PRINT_AFTER_CONFIRM_NO = 0;
        /**
         * 确认后数据是否可打印 - 可以打印
         */
        public static int PRINT_AFTER_CONFIRM_YES = 1;

        public static int[] GetPrintAfterConfirm()
        {
            int[] printAfterConfirm = new int[2];
            printAfterConfirm[0] = PRINT_AFTER_CONFIRM_NO;
            printAfterConfirm[1] = PRINT_AFTER_CONFIRM_YES;
            return printAfterConfirm;
        }

        public static String GetPrintAfterConfirmDesc(int getPrintAfterConfirm)
        {
            if (getPrintAfterConfirm == PRINT_AFTER_CONFIRM_NO)
            {
                return "PENALTY_TYPE_NO";
            }
            if (getPrintAfterConfirm == PRINT_AFTER_CONFIRM_YES)
            {
                return "PENALTY_TYPE_YES";
            }

            return "未知";
        }

        /**
         * 超时后自动处理 - 驳回到工作流起点
         */
        public static int TIMEOUT_HANDLER_REJECT = 1;
        /**
         * 超时后自动处理 - 提交上级审批
         */
        public static int TIMEOUT_HANDLER_MGR = 2;
        /**
         * 超时后自动处理 - 自动通过
         */
        public static int TIMEOUT_HANDLER_AUTO = 3;
        /**
         * 超时后自动处理 - 不做处理
         */
        public static int TIMEOUT_HANDLER_NOTHING = 4;

        public static int[] GetTimeoutHandler()
        {
            int[] getTimeoutHandler = new int[4];
            getTimeoutHandler[0] = TIMEOUT_HANDLER_REJECT;
            getTimeoutHandler[1] = TIMEOUT_HANDLER_MGR;
            getTimeoutHandler[2] = TIMEOUT_HANDLER_AUTO;
            getTimeoutHandler[3] = TIMEOUT_HANDLER_NOTHING;
            return getTimeoutHandler;
        }

        public static String GetTimeoutHandlerDesc(int getTimeoutHandler)
        {
            if (getTimeoutHandler == TIMEOUT_HANDLER_REJECT)
            {
                return "WrkFlwNode_Back";
            }
            if (getTimeoutHandler == TIMEOUT_HANDLER_MGR)
            {
                return "WrkFlwNode_Submit";
            }
            if (getTimeoutHandler == TIMEOUT_HANDLER_AUTO)
            {
                return "WrkFlwNode_Proceed";
            }
            if (getTimeoutHandler == TIMEOUT_HANDLER_NOTHING)
            {
                return "WrkFlwNode_Pending";
            }

            return "未知";
        }

        /**
         * 工作流中首节点的步数
         */
        public static int FIRST_WRK_STEP = 1;

        private int wrkFlwID = 0;
        private int nodeID = 0;
        private int funcID = 0;
        private int roleID = 0;
        private string nodeName = null;
        private int wrkStep = 0;
        private int smtToMgr = 0;
        private int validAfterConfirm = 0;
        private int printAfterConfirm = 0;
        private int longestDelay = 0;
        private int timeoutHandler = 0;
        private String processClass = null;
        private String imageURL = null;
        private int wrkFlwMailID = 0;
        public override String GetTableName()
        {
            return "WrkFlwNode";
        }

        public override String GetColumnNames()
        {
            return "WrkFlwID,NodeID,FuncID,RoleID,NodeName,WrkStep,SmtToMgr,ValidAfterConfirm,PrintAfterConfirm,LongestDelay,TimeoutHandler,ProcessClass,ImageURL,WrkFlwMailID";
        }

        public override String GetInsertColumnNames()
        {
            return "WrkFlwID,NodeID,FuncID,RoleID,NodeName,WrkStep,SmtToMgr,ValidAfterConfirm,PrintAfterConfirm,LongestDelay,TimeoutHandler,ProcessClass,ImageURL,WrkFlwMailID";
        }

        public override String GetUpdateColumnNames()
        {
            return "FuncID,RoleID,NodeName,WrkStep,SmtToMgr,ValidAfterConfirm,PrintAfterConfirm,LongestDelay,TimeoutHandler,ProcessClass,ImageURL,WrkFlwMailID";
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
        public int FuncID
        {
            get { return this.funcID; }
            set { this.funcID = value; }
        }
        public int RoleID
        {
            get { return this.roleID; }
            set { this.roleID = value; }
        }
        public string NodeName
        {
            get { return this.nodeName; }
            set { this.nodeName = value; }
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
        public int LongestDelay
        {
            get { return this.longestDelay; }
            set { this.longestDelay = value; }
        }
        public int TimeoutHandler
        {
            get { return this.timeoutHandler; }
            set { this.timeoutHandler = value; }
        }
        public String ProcessClass
        {
            get { return this.processClass; }
            set { this.processClass = value; }
        }
        public String ImageURL
        {
            get { return this.imageURL; }
            set { this.imageURL = value; }
        }

        public int WrkFlwMailID
        {
            get { return this.wrkFlwMailID; }
            set { this.wrkFlwMailID = value; }
        }
        //---------------------描述性属性-------------------------------
        public String SmtToMgrDesc
        {
            get { return GetSmtToMgrDesc(this.SmtToMgr); }
        }
        public String ValidAfterConfirmDesc
        {
            get { return GetValidAfterConfirmDesc(this.ValidAfterConfirm); }
        }
        public String PrintAfterConfirmDesc
        {
            get { return GetPrintAfterConfirmDesc(this.PrintAfterConfirm); }
        }
        public String TimeoutHandlerDesc
        {
            get { return GetTimeoutHandlerDesc(this.TimeoutHandler); }
        }
        public String RoleName
        {
            get { return BaseInfoApp.GetRoleName(this.RoleID); }
        }

    }
}
