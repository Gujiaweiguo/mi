using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.Contract
{
    public class ConLeaseCharge : BasePO
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

        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,BizMode,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,CommOper,NorentDays,ContractTypeID,StopDate,SubsID";
        }

        public override string GetUpdateColumnNames()
        {
            return "CustID,ContractCode,RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,RootTradeID,NorentDays,CommOper,SubsID,ContractTypeID";
        }
        public override String GetInsertColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,ConStartDate,ConEndDate,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,RootTradeID,CommOper,NorentDays,SigningMode,BizMode,SubsID,ContractTypeID";
        }
        public override string GetQuerySql()
        {

            return "SELECT " +
                   " Contract.ContractID,CustID,ContractCode,Contract.RefID,BizMode,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,CommOper,NorentDays,ContractTypeID,StopDate,SubsID " +
                   " FROM Contract INNER JOIN ConShop ON (Contract.ContractID=ConShop.ContractID) ";
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
    }
}
