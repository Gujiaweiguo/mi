using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Invoice.MakePoolVoucher
{
    public class AccountReport : BasePO
    {
        private int accountreportid = 0;
        private int paratype = 0;
        private int accountid = 0;
        private string accountnumber = "";
        private string accountname = "";
        private string accountdesc = "";
        private decimal debitamount = 0;
        private decimal creditamount = 0;

        public int paratype_pretext = 1;  //借
        public int paratype_loan = 2;     //贷


        public override String GetTableName()
        {
            return "AccountReport";
        }
        public override String GetColumnNames()
        {
            return "AccountReportID,ParaType,AccountID,AccountNumber,AccountName,AccountDesc,debitAmount,creditAmount";
        }
        public override String GetUpdateColumnNames()
        {
            return "AccountReportID,ParaType,AccountID,AccountNumber,AccountName,AccountDesc,debitAmount,creditAmount";
        }
        public int AccountReportID
        {
            get { return accountreportid; }
            set { accountreportid = value; }
        }
        public int ParaType
        {
            get { return paratype; }
            set { paratype = value; }
        }
        public int AccountID
        {
            get { return accountid; }
            set { accountid = value; }
        }
        public string AccountNumber
        {
            get { return accountnumber; }
            set { accountnumber = value; }
        }
        public string AccountName
        {
            get { return accountname; }
            set { accountname = value; }
        }
        public string AccountDesc
        {
            get { return accountdesc; }
            set { accountdesc = value; }
        }
        public decimal debitAmount
        {
            get { return debitamount; }
            set { debitamount = value; }
        }
        public decimal creditAmount
        {
            get { return creditamount; }
            set { creditamount = value; }
        }
    }
}