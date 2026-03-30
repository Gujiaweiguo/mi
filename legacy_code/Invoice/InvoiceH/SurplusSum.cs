using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class SurplusSum:BasePO
    {
        private decimal surAmt = 0;
        private decimal payOutAmtSum = 0;

        public override string GetTableName()
        {
            return "Surplus";
        }

        public override string GetColumnNames()
        {
            return "SurAmt,PayOutAmtSum";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select sum(SurAmt) as SurAmt,sum(PayOutAmtSum) as PayOutAmtSum from Surplus";
        }

        public decimal SurAmt
        {
            get { return surAmt; }
            set { surAmt = value; }
        }

        public decimal PayOutAmtSum
        {
            get { return payOutAmtSum; }
            set { payOutAmtSum = value; }
        }
    }
}
