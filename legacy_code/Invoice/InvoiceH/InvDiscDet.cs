using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvDiscDet:BasePO
    {
        private int discDetID = 0;
        private int discID = 0;
        private int invDetailID = 0;
        private decimal discAmt = 0;
        private decimal discAmtL = 0;
        private decimal discRate = 0;
        private string discReason = "";
        private int discType = 0;

        public static int INVDISCDET_DISCRATE = 1;
        public static int INVDISCDET_DISCAMT = 2;

        public override string GetTableName()
        {
            return "InvDiscDet";
        }

        public override string GetColumnNames()
        {
            return "DiscDetID,DiscID,InvDetailID,DiscAmt,DiscAmtL,DiscRate,DiscReason,DiscType";
        }

        public override string GetInsertColumnNames()
        {
            return "DiscDetID,DiscID,InvDetailID,DiscAmt,DiscAmtL,DiscRate,DiscReason,DiscType";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int DiscDetID
        {
            get { return discDetID; }
            set { discDetID = value; }
        }

        public int DiscID
        {
            get { return discID; }
            set { discID = value; }
        }

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
        }

        public decimal DiscAmt
        {
            get { return discAmt; }
            set { discAmt = value; }
        }

        public decimal DiscAmtL
        {
            get { return discAmtL; }
            set { discAmtL = value; }
        }

        public decimal DiscRate
        {
            get { return discRate; }
            set { discRate = value; }
        }

        public string DiscReason
        {
            get { return discReason; }
            set { discReason = value; }
        }

        public int DiscType
        {
            get { return discType; }
            set { discType = value; }
        }
    }
}
