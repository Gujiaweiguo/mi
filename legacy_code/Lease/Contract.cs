using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease
{
    public class Contract:BasePO
    {
        private int contractID;
        private int custID;
        private int contractCode;
        private int refID;
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

        public static int CONTRACT_TYPE_T1 = 0;
        public static int CONTRACT_TYPE_T2 = 1;


        public static int[] GetTradeTypeStatus()
        {
            int[] tradeTypeStatus = new int[2];
            tradeTypeStatus[0] = CONTRACT_TYPE_T1;
            tradeTypeStatus[1] = CONTRACT_TYPE_T1;

            return tradeTypeStatus;
        }

        //终约通知期限
        public static String GetTradeTypeStatusDesc(int tradeTypeStatus)
        {
            if (tradeTypeStatus == CONTRACT_TYPE_T1)
            {
                return "aaa";
            }
            if (tradeTypeStatus == CONTRACT_TYPE_T2)
            {
                return "bbb";
            }
            return "未知";
        }


        public static int PENALTY_TYPE_YES = 0;
        public static int PENALTY_TYPE_NO = 1;


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
                return "是";
            }
            if (penaltyTypeStatus == PENALTY_TYPE_NO)
            {
                return "否";
            }
            return "未知";
        }





        public static int CONTRACTSTATUS_TYPE_INGEAR = 0;
        public static int CONTRACTSTATUS_TYPE_ATTREM = 1;
        public static int CONTRACTSTATUS_TYPE_END = 2;
        public static int CONTRACTSTATUS_TYPE_PAUSE = 3;


        public static int[] GetContractTypeStatus()
        {
            int[] contractTypeStatus = new int[4];
            contractTypeStatus[0] = CONTRACTSTATUS_TYPE_INGEAR;
            contractTypeStatus[1] = CONTRACTSTATUS_TYPE_ATTREM;
            contractTypeStatus[2] = CONTRACTSTATUS_TYPE_END;
            contractTypeStatus[3] = CONTRACTSTATUS_TYPE_PAUSE;

            return contractTypeStatus;
        }

        //合同状态
        public static String GetContractTypeStatusDesc(int contractTypeStatus)
        {
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_INGEAR)
            {
                return "政策";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_ATTREM)
            {
                return "到期";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_END)
            {
                return "终止";
            }
            if (contractTypeStatus == CONTRACTSTATUS_TYPE_PAUSE)
            {
                return "暂停执行";
            }
            return "未知";
        }

        public override string GetTableName()
        {
            return "Contract";
        }

        public override string GetColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override String GetInsertColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice";
        }


        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }

        public int RefID
        {
            get { return refID; }
            set { refID = value; }
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

        public string EConUR
        {
            get {return eConUR;}
            set {eConUR = value;}
        }

        public string AdditionalItem
        {
            get {return additionalItem;}
            set {additionalItem = value;}
        }

        public string Note
        {
            get {return note;}
            set {note = value;}
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
    }
}
