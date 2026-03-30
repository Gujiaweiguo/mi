using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 结算单明细
    /// </summary>
    class InvoiceDetail : BasePO
    {
        private int invDetailID = 0;
        private int chargeTypeID = 0;
        private int invID = 0;
        private DateTime period = DateTime.Now;  //费用记账月
        private DateTime invStartDate = DateTime.Now;  //费用开始日期
        private DateTime invEndDate = DateTime.Now; //费用结束日期
        private int invCurTypeID = 0; //结算币种
        private decimal invExRate = 0; //结算汇率
        private decimal invPayAmt = 0; //费用应结金额
        private decimal invPayAmtL = 0; //费用应结本币金额
        private decimal invAdjAmt = 0; //费用调整金额
        private decimal invAdjAmtL = 0; //费用调整本币金额
        private decimal invDiscAmt = 0; //费用优惠金额
        private decimal invDiscAmtL = 0; //费用优惠本币金额
        private decimal invChngAmt = 0; //费用其他变动金额
        private decimal invChngAmtL = 0; //费用其他变动本币金额
        private decimal invActPayAmt = 0; //费用实际应结金额
        private decimal invActPayAmtL = 0; //费用实际应结本币金额
        private decimal invPaidAmt = 0; //费用已结金额
        private decimal invPaidAmtL = 0; //费用已结本币金额
        private int invType = 1; //结算类型
        private int invDetStatus = 1; //费用明细状态
        private string note = ""; //备注

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
        }

        public DateTime Period
        {
            get { return period; }
            set { period = value; }
        }

        public DateTime InvStartDate
        {
            get { return invStartDate; }
            set { invStartDate = value; }
        }

        public DateTime InvEndDate
        {
            get { return invEndDate; }
            set { invEndDate = value; }
        }

        public int InvCurTypeID
        {
            get { return invCurTypeID; }
            set { invCurTypeID = value; }
        }

        public decimal InvExRate
        {
            get { return invExRate; }
            set { invExRate = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public decimal InvPayAmtL
        {
            get { return invPayAmtL; }
            set { invPayAmtL = value; }
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

        public decimal InvChngAmt
        {
            get { return invChngAmt; }
            set { invChngAmt = value; }
        }

        public decimal InvChngAmtL
        {
            get { return invChngAmtL; }
            set { invChngAmtL = value; }
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

        public int InvType
        {
            get { return invType; }
            set { invType = value; }
        }

        public int InvDetStatus
        {
            get { return invDetStatus; }
            set { invDetStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public override string GetTableName()
        {
            return "InvoiceDetail";
        }

        public override string GetColumnNames()
        {
            return "InvDetailID,ChargeTypeID,InvID,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL," +
                "InvDiscAmt,InvDiscAmtL,InvChngAmt,InvChngAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvType,InvDetStatus,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "InvDetailID,ChargeTypeID,InvID,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL," +
                "InvDiscAmt,InvDiscAmtL,InvChngAmt,InvChngAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvType,InvDetStatus,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
