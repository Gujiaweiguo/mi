using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class PayInSum:BasePO
    {
        private decimal payInAmt = 0;
        private decimal payOutAmtSum = 0;

        public override string GetTableName()
        {
            return "PayIn";
        }

        public override string GetColumnNames()
        {
            return "PayInAmt,PayOutAmtSum";
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
            return "select sum(PayInAmt) as PayInAmt,sum(PayOutAmtSum) as PayOutAmtSum  from  Contract a,ConShop b,PayIn c";
        }

        public decimal PayInAmt
        {
            get { return payInAmt; }
            set { payInAmt = value; }
        }

        public decimal PayOutAmtSum
        {
            get { return payOutAmtSum; }
            set { payOutAmtSum = value; }
        }
    }
}
