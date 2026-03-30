using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvoiceDetailSum:BasePO
    {
        private decimal invPayAmt = 0;
        private decimal invPaidAmt = 0;

        public override string GetTableName()
        {
            return "InvoiceDetail";
        }

        public override string GetColumnNames()
        {
            return "InvPayAmt,InvPaidAmt";
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
            return "select sum(InvPayAmt) as InvPayAmt,sum(InvPaidAmt) as InvPaidAmt from InvoiceDetail";
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public decimal InvPaidAmt
        {
            get { return invPaidAmt; }
            set { invPaidAmt = value; }
        }
    }
}
