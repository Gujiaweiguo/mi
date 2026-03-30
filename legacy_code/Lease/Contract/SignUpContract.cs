using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.Contract
{
    public class SignUpContract:BasePO
    {
        private int contractID = 0;
        private int custID = 0;
        private int contractStatus = 0;
        private int bizMode = 0;
        private DateTime conStartDate = DateTime.Now;
        private DateTime conEndDate = DateTime.Now;
        private DateTime chargeStartDate = DateTime.Now;
        private char signingMode = 'N';
        private int commOper;
        private int norentDays = 0;
        private int contractTypeID = 0;
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
        public int ContractStatus
        {
            get { return contractStatus; }
            set { contractStatus = value; }
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
        public DateTime ChargeStartDate
        {
            get { return chargeStartDate; }
            set { chargeStartDate = value; }
        }
        public char SigningMode
        {
            get { return signingMode; }
            set { signingMode = value; }
        }
        public int CommOper
        {
            get { return commOper; }
            set { commOper = value; }
        }
        public int NorentDays
        {
            get { return norentDays; }
            set { norentDays = value; }
        }
        public int ContractTypeID
        {
            get { return contractTypeID; }
            set { contractTypeID = value; }
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
            return "ContractID,CustID,ContractStatus,ConStartDate,ConEndDate,ChargeStartDate,BizMode,NorentDays,SigningMode,CommOper,ContractTypeID";
        }

    }
}
