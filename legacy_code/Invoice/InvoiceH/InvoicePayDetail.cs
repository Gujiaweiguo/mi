using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvoicePayDetail:BasePO
    {
        private int invPayDetID = 0;
        private int chargeTypeID = 0;
        private int invPayID = 0;
        private int invID = 0;
        private int invDetailID = 0;
        private decimal invActPayAmt = 0;
        private decimal invActPayAmtL = 0;
        private decimal invPaidAmt = 0;
        private decimal invPaidAmtL = 0;
        private int invPayDetStatus = 0;
        private decimal payOutAmtSum = 0;
        private string note = "";
        private string chargeTypeName = "";

        public static int INVOICEPAYDETAIL_AVAILABILITY = 1;//有效
        public static int INVOICEPAYDETAIL_CANCEL = 2;      //取消
        public static int INVOICEPAYDETAIL_PART_BACKING_OUT = 3;//部分返还
        public static int INVOICEPAYDETAIL_FULL_BACKING_OUT = 4;//全部返还

        public override string GetTableName()
        {
            return "InvoicePayDetail";
        }

        public override string GetColumnNames()
        {
            return "InvPayDetID,ChargeTypeID,InvPayID,InvID,InvDetailID,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvPayDetStatus,PayOutAmtSum,Note,ChargeTypeName";
        }

        public override string GetInsertColumnNames()
        {
            return "InvPayDetID,ChargeTypeID,InvPayID,InvID,InvDetailID,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvPayDetStatus,Note,PayOutAmtSum";
        }

        public override string GetUpdateColumnNames()
        {
            return "InvPayDetStatus,PayOutAmtSum";
        }

        public override string GetQuerySql()
        {
            return "select InvPayDetID,b.ChargeTypeID,b.InvPayID,InvID,InvActPayAmt,InvActPayAmtL,b.InvPaidAmt," +
                    "b.InvPaidAmtL,InvPayDetStatus,PayOutAmtSum,ChargeTypeName from InvoicePay a left join InvoicePayDetail b on a.InvPayID=b.InvPayID left join ChargeType c " +
                    " on b.ChargeTypeID=c.ChargeTypeID";
        }

        public int InvPayDetID
        {
            get { return invPayDetID; }
            set { invPayDetID = value; }
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public int InvPayID
        {
            get { return invPayID; }
            set { invPayID = value; }
        }

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
        }

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
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

        public decimal InvPaidAmt
        {
            get { return invPaidAmt; }
            set { invPaidAmt = value; }
        }

        public decimal InvPaidAmtL
        {
            get { return invPaidAmtL; }
            set { invPaidAmtL = value; }
        }

        public int InvPayDetStatus
        {
            get { return invPayDetStatus; }
            set { invPayDetStatus = value; }
        }

        public decimal PayOutAmtSum
        {
            get { return payOutAmtSum; }
            set { payOutAmtSum = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
        }
    }
}
