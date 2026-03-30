using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace WorkFlow.WrkFlw
{
    public class WrkFlw : BasePO
    {

        /**
         * 工作流状态 - 草稿
         */
        public static int WRK_FLW_STATUS_DRAFT = 0;
        /**
         * 工作流状态 - 有效
         */
        public static int WRK_FLW__STATUS_VALID = 1;
        /**
         * 工作流状态 - 无效
         */
        public static int WRK_FLW_STATUS_INVALID = 2;

        public static int[] GetWrkFlwStatus()
        {
            int[] wrkFlwStatus = new int[3];
            wrkFlwStatus[0] = WRK_FLW_STATUS_DRAFT;
            wrkFlwStatus[1] = WRK_FLW__STATUS_VALID;
            wrkFlwStatus[2] = WRK_FLW_STATUS_INVALID;
            return wrkFlwStatus;
        }

        public static String GetWrkFlwStatusDesc(int wrkFlwStatus)
        {
            if (wrkFlwStatus == WRK_FLW_STATUS_DRAFT)
            {
                return "WrkFlw_Draft";
            }
            if (wrkFlwStatus == WRK_FLW__STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            if (wrkFlwStatus == WRK_FLW_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";
            }
            return "未知";
        }

        /**
         * 首节点是否制作单据 - 不制单
         */
        public static int INIT_VOUCHER_NO = 0;
        /**
         * 首节点是否制作单据 - 制单
         */
        public static int INIT_VOUCHER_YES = 1;

        public static int[] GetInitVoucher()
        {
            int[] initVoucher = new int[2];
            initVoucher[0] = INIT_VOUCHER_NO;
            initVoucher[1] = INIT_VOUCHER_YES;
            return initVoucher;
        }

        public static String GetInitVoucherDesc(int initVoucher)
        {
            if (initVoucher == INIT_VOUCHER_NO)
            {
                return "WrkFlw_VouTriggerredNO";
            }
            if (initVoucher == INIT_VOUCHER_YES)
            {
                return "WrkFlw_VouTriggerredYES";
            }
            return "未知";
        }
        /**
         * 是否转接 - 否
         */
        public static int IF_TRANSIT_NO=0;        
        /**
         * 是否转接 - 是
         */
        public static int IF_TRANSIT_YES=1;

        public static int[] GetIfTransit()
        {
            int[] ifTransit = new int[2];
            ifTransit[0] = IF_TRANSIT_NO;
            ifTransit[1] = IF_TRANSIT_YES;
            return ifTransit;
        }

        public static String GetIfTransitDesc(int ifTransit)
        {
            if (ifTransit == IF_TRANSIT_NO)
            {
                return "WrkFlw_IfTransitNO";
            }
            if (ifTransit == IF_TRANSIT_YES)
            {
                return "WrkFlw_IfTransitYES";
            }
            return "未知";
        }


        /**
         *   否
         */
        public static int IF_NO = 0;
        /**
         *  是
         */
        public static int IF_YES = 1;

        public static int[] GetIf()
        {
            int[] ifYESorNO = new int[2];
            ifYESorNO[0] = IF_NO;
            ifYESorNO[1] = IF_YES;
            return ifYESorNO;
        }

        public static String GetIfYESorNODesc(int ifYESorNO)
        {
            if (ifYESorNO == IF_NO)
            {
                return "PENALTY_TYPE_NO";
            }
            if (ifYESorNO == IF_YES)
            {
                return "PENALTY_TYPE_YES";
            }
            return "未知";
        }

        private int wrkFlwID = 0;
        private int bizGrpID = 0;
        private int voucherTypeID = 0;
        private string wrkFlwName = null;
        private int initVoucher = 0;
        private int efficiency = 0;
        private int traceDays = 0;
        private int wrkFlwStatus = 0;
        private String processClass = null;
        private int ifTransit=0;
        private string workFlowDrawing = null;
        private string workFlowNode = null;
        private string processClassOne = "";
        private string processClassTwo = "";

        public override String GetTableName()
        {
            return "WrkFlw";
        }

        public override String GetColumnNames()
        {
            return "WrkFlwID,BizGrpID,VoucherTypeID,WrkFlwName,InitVoucher,Efficiency,TraceDays,WrkFlwStatus,ProcessClass,IfTransit,WorkFlowDrawing,WorkFlowNode,ProcessClassOne,ProcessClassTwo";
        }

        public override String GetInsertColumnNames()
        {
            return "WrkFlwID,BizGrpID,VoucherTypeID,WrkFlwName,InitVoucher,Efficiency,TraceDays,WrkFlwStatus,ProcessClass,IfTransit,WorkFlowDrawing,WorkFlowNode";
        }

        public override String GetUpdateColumnNames()
        {
            return "VoucherTypeID,WrkFlwName,InitVoucher,Efficiency,TraceDays,WrkFlwStatus,ProcessClass,IfTransit,WorkFlowDrawing,WorkFlowNode";
        }

        public override string GetQuerySql()
        {
            return "Select WrkFlwID,BizGrpID,VoucherTypeID,WrkFlwName,InitVoucher,Efficiency,TraceDays,WrkFlwStatus,ProcessClass,IfTransit,WorkFlowDrawing,WorkFlowNode,ProcessClassOne,ProcessClassTwo,'' as InitVoucherName,'' as IfTransitName,'' as WrkFlwStatusName from WrkFlw";
        }

        public int WrkFlwID
        {
            get{return this.wrkFlwID;}
            set{this.wrkFlwID=value;}
        }
        public int BizGrpID
        {
            get{return this.bizGrpID;}
            set{this.bizGrpID=value;}
        }
        public int VoucherTypeID
        {
            get{return this.voucherTypeID;}
            set{this.voucherTypeID=value;}
        }
        public string WrkFlwName
        {
            get{return this.wrkFlwName;}
            set{this.wrkFlwName=value;}
        }
        public int InitVoucher
        {
            get{return this.initVoucher;}
            set{this.initVoucher=value;}
        }
        public int Efficiency
        {
            get{return this.efficiency;}
            set{this.efficiency=value;}
        }
        public int TraceDays
        {
            get{return this.traceDays;}
            set{this.traceDays=value;}
        }
        public int WrkFlwStatus
        {
            get{return this.wrkFlwStatus;}
            set{this.wrkFlwStatus=value;}
        }
        public String ProcessClass
        {
            get { return this.processClass; }
            set { this.processClass = value; }
        }
        public int IfTransit{
            get{return this.ifTransit;}
            set{this.ifTransit=value;}
        }

        public string WorkFlowDrawing
        {
            get { return workFlowDrawing; }
            set { workFlowDrawing = value; }
        }

        public string WorkFlowNode
        {
            get { return workFlowNode; }
            set { workFlowNode = value; }
        }

        public string ProcessClassOne
        {
            get { return processClassOne; }
            set { processClassOne = value; }
        }

        public string ProcessClassTwo
        {
            get { return processClassTwo; }
            set { processClassTwo = value; }
        }

        //---------------------描述性属性-------------------------------
        public String WrkFlwStatusDesc
        {
            get { return GetWrkFlwStatusDesc(this.WrkFlwStatus); }
        }
        public String InitVoucherDesc
        {
            get { return GetInitVoucherDesc(this.InitVoucher); }
        }
        public String IfTransitDesc
        {
            get { return GetIfTransitDesc(this.IfTransit); }
        }



        //临时方法
        public static int VOUCHER_TYPE_ID_1 = 1;
        public static int VOUCHER_TYPE_ID_2 = 2;
        public static int[] GetVoucherTypeID()
        {
            int[] voucherTypeID = new int[2];
            voucherTypeID[0] = VOUCHER_TYPE_ID_1;
            voucherTypeID[1] = VOUCHER_TYPE_ID_2;
            return voucherTypeID;
        }
        public static String GetVoucherTypeIDDesc(int voucherTypeID)
        {
            if (voucherTypeID == VOUCHER_TYPE_ID_1)
            {
                return "Application_Form";
            }
            if (voucherTypeID == VOUCHER_TYPE_ID_2)
            {
                return "Change_Request_Form";
            }
            return "Null";
        }
    }
}
