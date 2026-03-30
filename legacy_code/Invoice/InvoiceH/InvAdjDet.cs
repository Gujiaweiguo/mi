using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*调整结算明细实体*/
    [Serializable()]
    public class InvAdjDet
    {
        private int invAdjDetID = 0;
        private int invDetailID = 0;
        private int invAdjID = 0;
        private decimal adjAmt = 0;
        private decimal adjAmtL = 0;
        private string adjReason = "";

        public int InvAdjDetID
        {
            get { return invAdjDetID; }
            set { invAdjDetID = value; }
        }

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
        }

        public int InvAdjID
        {
            get { return invAdjID; }
            set { invAdjID = value; }
        }

        public decimal AdjAmt
        {
            get { return adjAmt; }
            set { adjAmt = value; }
        }

        public decimal AdjAmtL
        {
            get { return adjAmtL; }
            set { adjAmtL = value; }
        }

        public string AdjReason
        {
            get { return adjReason; }
            set { adjReason = value; }
        }
    }
}
