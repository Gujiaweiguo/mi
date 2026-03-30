using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.Contract
{
    public class CustContract : BasePO
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
        private int contractTypeID;
        private int bizMode;
        private string custName = "";
        private string custShortName = "";
        private int conFormulaModID;
        private string modReason = "";
        private int subsID;

        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,BizMode,ContractTypeID,SubsID,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,EConURL,AdditionalItem,CustName,CustShortName,ConFormulaModID,ModReason";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override String GetInsertColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "Select a.ContractID,a.CustID,ContractCode,a.RefID,a.BizMode,a.ContractTypeID,a.SubsID,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,a.EConURL,AdditionalItem,CustName,CustShortName,ConFormulaModID,ModReason " +
                    "From Contract a left join  ConFormulaMod b on a.ContractID=b.ContractID left join Customer c on a.CustID=c.CustID";
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
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

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public string CustShortName
        {
            get { return custShortName; }
            set { custShortName = value; }
        }

        public int ConFormulaModID
        {
            get { return conFormulaModID; }
            set { conFormulaModID = value; }
        }

        public string ModReason
        {
            get { return modReason; }
            set { modReason = value; }
        }

        public int SubsID
        {
            set { subsID = value; }
            get { return subsID; }
        }
    }
}
