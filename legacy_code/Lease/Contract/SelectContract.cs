using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.Contract
{
    public class SelectContract:BasePO
    {
        private int contractID;
        private int custID;
        private string contractCode;
        private int refID;
        private DateTime conStartDate;
        private DateTime conEndDate;
        private string penaltyItem;
        private DateTime chargeStartDate;
        private int tradeID;
        private int contractStatus;
        private string contractStatusName = "";
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

        public override string GetTableName()
        {
            return "Contract";
        }

        public override string GetColumnNames()
        {
            return "ContractID,CustID,ContractCode,RefID,BizMode,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,Note,CommOper,NorentDays,ContractTypeID";
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
            return "Select a.ContractID,a.CustID,ContractCode,a.RefID,a.BizMode,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,a.Note,a.CommOper,NorentDays,ContractTypeID,'' as ContractStatusName, " +
                    " d.CustShortName,c.BrandName,ConShop.RentArea,StoreName " +
                    "From contract a Left Join ConShop on a.ContractID=ConShop.ContractID Left Join ConShopBrand c on ConShop.BrandID=c.BrandID " +
                    "Left Join Customer d on a.CustID=d.CustID Inner join store on (ConShop.storeid=store.storeid)";
                    //" INNER JOIN ConShop ON ConShop.ContractID = a.ContractID";
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
        public string ContractStatusName
        {
            get { return contractStatusName; }
            set { contractStatusName = value; }
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
    }
}
