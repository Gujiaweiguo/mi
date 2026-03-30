using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvoiceHeaderCancel:BasePO
    {
        private int invStatus = 0;


        public override string GetTableName()
        {
            return "InvoiceHeader";
        }

        public override string GetColumnNames()
        {
            return "";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "InvStatus";
        }

        public int InvStatus
        {
            get { return invStatus; }
            set { invStatus = value; }
        }

    }
}
