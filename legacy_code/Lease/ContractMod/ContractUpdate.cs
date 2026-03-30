using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ContractMod
{
    public class ContractUpdate:BasePO
    {
        private int refID;
        private DateTime conStartDate;
        private DateTime conEndDate;
        private string penaltyItem;
        private DateTime chargeStartDate;
        private int tradeID;
        private int penalty;
        private int notice;
        private string eConUR;
        private string additionalItem;
        private string note;
        private int rootTradeID;
        private int norentDays = 0;
        private int contractTypeID;

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
            return "RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,Penalty,Notice,AdditionalItem,EConURL,Note,RootTradeID,NorentDays";
        }
        public override String GetInsertColumnNames()
        {
            return "";
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

        public int TradeID
        {
            get { return tradeID; }
            set { tradeID = value; }
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

    }
}
