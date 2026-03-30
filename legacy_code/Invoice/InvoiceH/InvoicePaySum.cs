using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvoicePaySum:BasePO
    {
        private decimal invPaidAmt = 0;
        private decimal payOutAmtSum = 0;

        public override string GetTableName()
        {
            return "InvoicePayDetail";
        }

        public override string GetColumnNames()
        {
            return "InvPaidAmt,PayOutAmtSum";
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
            return "select sum(b.InvPaidAmt) as InvPaidAmt,sum(PayOutAmtSum) as PayOutAmtSum from  InvoicePay a left join InvoicePayDetail b on a.InvPayID=b.InvPayID";
        }

        public decimal InvPaidAmt
        {
            get { return invPaidAmt; }
            set { invPaidAmt = value; }
        }

        public decimal PayOutAmtSum
        {
            get { return payOutAmtSum; }
            set { payOutAmtSum = value; }
        }
    }
}
