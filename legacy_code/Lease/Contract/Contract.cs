using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.Contract
{
    public class Contract : BasePO
    {
        private int contractID;
        private int custID;
        private string contractCode;
        private string refID;
        private DateTime conStartDate;
        private DateTime conEndDate;
        private string penaltyItem;
        private DateTime chargeStartDate;
        private int tradeID;
        private int contractStatus;
        private int penalty;
        private int notice;
        private string eConUR;
        private string additionalItem;
        private string note;
        private int rootTradeID;
        private int commOper;
        private int norentDays = 0;
        private int contractTypeID;
        private int bizMode;
        private char signingMode;
        private DateTime stopDate;
        private int subsID = 0;
        private string exclusive;

        public const int NOTICE_PERIOD0 = 0;
        public const int NOTICE_PERIOD1 = 1;
        public const int NOTICE_PERIOD2 = 2;
        public const int NOTICE_PERIOD3 = 3;
        public const int NOTICE_PERIOD4 = 4;

        public static int[] GetNotices()
        {
            int[] notices = new int[5];
            notices[0] = NOTICE_PERIOD0;
            notices[1] = NOTICE_PERIOD1;
            notices[2] = NOTICE_PERIOD2;
            notices[3] = NOTICE_PERIOD3;
            notices[4] = NOTICE_PERIOD4;
            return notices;
        }

        //终约通知期限
        public static String GetNoticeDesc(int notice)
        {
            if (notice == NOTICE_PERIOD0)
            {
                return "NOTICE_PERIOD0";
            }
            if (notice == NOTICE_PERIOD1)
            {
                return "NOTICE_PERIOD1";
            }
            if (notice == NOTICE_PERIOD2)
            {
                return "NOTICE_PERIOD2";
            }
            if (notice == NOTICE_PERIOD3)
            {
                return "NOTICE_PERIOD3";
            }
            if (notice == NOTICE_PERIOD4)
            {
                return "NOTICE_PERIOD4";
            }

            return "NO";
        }


        public static int PENALTY_TYPE_YES = 1;
        public static int PENALTY_TYPE_NO = 0;


        public static int[] GetPenaltyTypeStatus()
        {
            int[] penaltyTypeStatus = new int[2];
            penaltyTypeStatus[0] = PENALTY_TYPE_YES;
            penaltyTypeStatus[1] = PENALTY_TYPE_NO;

            return penaltyTypeStatus;
        }

        //绑定是否处罚
        public static String GetPenaltyTypeStatusDesc(int penaltyTypeStatus)
        {
            if (penaltyTypeStatus == PENALTY_TYPE_YES)
            {
                return "PENALTY_TYPE_YES";
            }
            if (penaltyTypeStatus == PENALTY_TYPE_NO)
            {
                return "PENALTY_TYPE_NO";
            }
            return "NO";
        }

        /**
         * 经营方式 - 租赁客户
         */
        public static int BIZ_MODE_LEASE = 1;

        /**
         * 经营方式 - 联营客户
         */
        public static int BIZ_MODE_UNIT = 2;

        /**
         * 经营方式 - 广告位

         */
        public static int BIZ_MODE_AdBoard = 3;

        /**
         * 经营方式 - 场地
         */
        public static int BIZ_MODE_Area = 4;


        /**
         * 获取经营方式列表
         */
        public static int[] GetBizModes()
        {
            int[] bizModes = new int[4];
            bizModes[0] = BIZ_MODE_LEASE;
            bizModes[1] = BIZ_MODE_UNIT;
            bizModes[2] = BIZ_MODE_AdBoard;
            bizModes[3] = BIZ_MODE_Area;

            return bizModes;
        }
        /**
         * 获取经营方式描述
         */
        public static String GetBizModeDesc(int bizMode)
        {
            if (bizMode == BIZ_MODE_LEASE)
            {
                return "BIZ_MODE_LEASE"; //"租赁";
            }
            if (bizMode == BIZ_MODE_UNIT)
            {
                return "BIZ_MODE_UNIT";// "联营";
            }
            if (bizMode == BIZ_MODE_AdBoard)
            {
                return "Ad_lblFastAdBoardName";// "广告位";
            }
            if (bizMode == BIZ_MODE_Area)
            {
                return "Ad_lblFastArea";// "场地";
            }
            return "NO";
        }

        public static char CONTRACTSTATUS_TYPE_N = 'N';//新租约




        public static char CONTRACTSTATUS_TYPE_R = 'R';//重签
        public static char CONTRACTSTATUS_TYPE_P = 'P';//延长

        public static int CONTRACTSTATUS_TYPE_FIRST = 0;
        public static int CONTRACTSTATUS_TYPE_TEMP = 1;
        public static int CONTRACTSTATUS_TYPE_INGEAR = 2;
        public static int CONTRACTSTATUS_TYPE_ATTREM = 3;
        public static int CONTRACTSTATUS_TYPE_END = 4;
        public static int CONTRACTSTATUS_TYPE_PAUSE = 5;


        public static int[] GetContractTypeStatus()
        {
            int[] contractTypeStatus = new int[6];
            contractTypeStatus[0] = CONTRACTSTATUS_TYPE_FIRST;
            contractTypeStatus[1] = CONTRACTSTATUS_TYPE_TEMP;
            contractTypeStatus[2] = CONTRACTSTATUS_TYPE_INGEAR;
            contractTypeStatus[3] = CONTRACTSTATUS_TYPE_ATTREM;
            contractTypeStatus[4] = CONTRACTSTATUS_TYPE_END;
            contractTypeStatus[5] = CONTRACTSTATUS_TYPE_PAUSE;

            return contractTypeStatus;
        }

        //合同状态




        public static String GetContractTypeStatusDesc(int contractTypeStatus)
        {
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_FIRST)
            {
                return "CONTRACTSTATUS_TYPE_FIRST";// "初始状态";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_TEMP)
            {
                return "CONTRACTSTATUS_TYPE_TEMP";// "草稿状态";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_INGEAR)
            {
                return "CONTRACTSTATUS_TYPE_INGEAR";// "正常";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_ATTREM)
            {
                return "CONTRACTSTATUS_TYPE_ATTREM";// "到期";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_END)
            {
                return "CONTRACTSTATUS_TYPE_END";// "终止";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_PAUSE)
            {
                return "CONTRACTSTATUS_TYPE_PAUSE";// "暂停执行";
            }
            return "NO";
        }

        public override string GetTableName()
        {
            return "Contract";
        }

        public override string GetColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,BizMode,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,CommOper,NorentDays,ContractTypeID,StopDate,SubsID,Exclusive";
        }

        public override string GetUpdateColumnNames()
        {
            return "CustID,ContractCode,RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,RootTradeID,NorentDays,CommOper,SubsID,ContractTypeID,Exclusive";
        }
        public override String GetInsertColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,RootTradeID,CommOper,NorentDays,SigningMode,BizMode,SubsID,ContractTypeID,Exclusive";
        }


        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }

        public int ContractTypeID
        {
            get { return contractTypeID; }
            set { contractTypeID = value; }
        }

        public string RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public int BizMode
        {
            get { return bizMode; }
            set { bizMode = value; }
        }

        public DateTime ConStartDate
        {
            get { return conStartDate; }
            set { conStartDate = value; }
        }
        public DateTime ConEndDate
        {
            get { return conEndDate; }
            set { conEndDate = value; }
        }
        public string PenaltyItem
        {
            get { return penaltyItem; }
            set { penaltyItem = value; }
        }
        public DateTime ChargeStartDate
        {
            get { return chargeStartDate; }
            set { chargeStartDate = value; }
        }

        public DateTime StopDate
        {
            get { return stopDate; }
            set { stopDate = value; }
        }

        public int TradeID
        {
            get { return tradeID; }
            set { tradeID = value; }
        }
        public int ContractStatus
        {
            get { return contractStatus; }
            set { contractStatus = value; }
        }
        public int Penalty
        {
            get { return penalty; }
            set { penalty = value; }
        }
        public int Notice
        {
            get { return notice; }
            set { notice = value; }
        }

        public string EConURL
        {
            get { return eConUR; }
            set { eConUR = value; }
        }

        public string AdditionalItem
        {
            get { return additionalItem; }
            set { additionalItem = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public int RootTradeID
        {
            get { return rootTradeID; }
            set { rootTradeID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public int NorentDays
        {
            get { return norentDays; }
            set { norentDays = value; }
        }

        public int CommOper
        {
            get { return commOper; }
            set { commOper = value; }
        }

        public char SigningMode
        {
            get { return signingMode; }
            set { signingMode = value; }
        }

        public int SubsID
        {
            set { subsID = value; }
            get { return subsID; }
        }

        public string Exclusive
        {
            get { return exclusive; }
            set { exclusive = value; }
        }
    }
}
