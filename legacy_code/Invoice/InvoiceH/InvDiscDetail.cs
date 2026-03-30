using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*结算单优惠更新*/
    public class InvDiscDetail:BasePO
    {
        private decimal invDiscAmt = 0;
        private decimal invDiscAmtL = 0;
        private decimal invActPayAmt = 0;
        private decimal invActPayAmtL = 0;


        public override string GetTableName()
        {
            return "InvoiceDetail";
        }

        public override string GetColumnNames()
        {
            return "InvDiscAmt,InvDiscAmtL,InvActPayAmt,InvActPayAmtL";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "InvDiscAmt,InvDiscAmtL,InvActPayAmt,InvActPayAmtL";
        }


        public decimal InvDiscAmt
        {
            get { return invDiscAmt; }
            set { invDiscAmt = value; }
        }

        public decimal InvDiscAmtL
        {
            get { return invDiscAmtL; }
            set { invDiscAmtL = value; }
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
