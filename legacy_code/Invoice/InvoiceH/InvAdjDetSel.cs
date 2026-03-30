using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*调整结算明细审批查询*/
    public class InvAdjDetSel:BasePO
    {
        private decimal adjAmt = 0;
        private string adjReason = "";
        private DateTime period = DateTime.Now;

        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "Period,AdjAmt,AdjReason";
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
            return "select Period,AdjAmt,AdjReason from InvAdjDet a,InvoiceDetail b";
        }

        public decimal AdjAmt
        {
            get { return adjAmt; }
            set { adjAmt = value; }
        }

        public string AdjReason
        {
            get { return adjReason; }
            set { adjReason = value; }
        }

        public DateTime Period
        {
            get { return period; }
            set { period = value; }
        }
    }
}
