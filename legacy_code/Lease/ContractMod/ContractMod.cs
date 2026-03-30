using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ContractMod
{
    public class ContractMod:BasePO
    {
        private int conModID = 0;
        private int contractID;
        private string contractCode = "";
        private int refID;
        private DateTime conStartDate;
        private DateTime conEndDate;
        private string penaltyItem;
        private DateTime chargeStartDate;
        private int tradeID;
        private int contractStatus = 0;
        private int penalty;
        private int notice;
        private string eConUR;
        private string additionalItem;
        private string note;
        private int rootTradeID;
        private int commOper;
        private int norentDays = 0;
        private int contractTypeID;
        private char signingMode;
        private DateTime stopDate;

        public override string GetTableName()
        {
            return "ContractMod";
        }

        public override string GetColumnNames()
        {
            return "ConModID,ContractID,ContractCode,RefID,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,Penalty,Notice,AdditionalItem,EConURL,Note,CommOper,NorentDays";
        }

        public override string GetUpdateColumnNames()
        {
            return "RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,Penalty,Notice,AdditionalItem,EConURL,Note,CommOper,NorentDays";
        }
        public override String GetInsertColumnNames()
        {
            return "ConModID,ContractID,ContractCode,RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,Penalty,Notice,AdditionalItem,EConURL,Note,RootTradeID,NorentDays";
        }


        public int ConModID
        {
            get { return conModID; }
            set { conModID = value; }
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
    }
}
