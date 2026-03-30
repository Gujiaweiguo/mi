using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*结算单调整更新*/
    public class InvAdjDetail:BasePO
    {
        private decimal invAdjAmt = 0;
        private decimal invAdjAmtL = 0;
        private decimal invActPayAmt = 0;
        private decimal invActPayAmtL = 0;


        public override string GetTableName()
        {
            return "InvoiceDetail";
        }

        public override string GetColumnNames()
        {
            return "InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL";
        }


        public decimal InvAdjAmt
        {
            get { return invAdjAmt; }
            set { invAdjAmt = value; }
        }

        public decimal InvAdjAmtL
        {
            get { return invAdjAmtL; }
            set { invAdjAmtL = value; }
        }

        public decimal InvActPayAmt
        {
            get { return invActPayAmt; }
            set { invActPayAmt = value; }
        }

        public decimal InvActPayAmtL
        {
            get { return invActPayAmtL; }
            set { invActPayAmtL = value; }
        }
    }
}
