using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 结算单明细
    /// </summary>
    public class InvoiceDetail : BasePO
    {
        public static int RENTTYPE_NO_RENT = 0;   //非租金
        public static int RENTTYPE_FIXED_DAY = 1;   //固定日租金
        public static int RENTTYPE_FIXED_MONTH = 2;   //固定月租金
        public static int RENTTYPE_FIXED_P = 3;   //固定抽成租金
        public static int RENTTYPE_SINGLE_P = 4;   //单级抽成租金
        public static int RENTTYPE_MUNCH_P = 5;   //多级抽成租金
        public static int RENTTYPE_FIXED_M = 6;   //固定保底租金
        public static int RENTTYPE_LEVEL_M = 7;   //级别保底租金
        public static int RENTTYPE_ONCE = 8;   //一次性租金
        public static int RENTTYPE_BLANK_RECORD_P = 9;   //零记录

        private int invDetailID = 0;
        private int chargeTypeID = 0;
        private int invID = 0;
        private DateTime period = DateTime.Now;  //费用记账月
        private DateTime invStartDate = DateTime.Now;  //费用开始日期
        private DateTime invEndDate = DateTime.Now; //费用结束日期
        private int invCurTypeID = 1; //结算币种
        private decimal invExRate = 0; //结算汇率
        private decimal invPayAmt = 0; //费用应结金额
        private decimal invPayAmtL = 0; //费用应结本币金额
        private decimal invAdjAmt = 0; //费用调整金额
        private decimal invAdjAmtL = 0; //费用调整本币金额
        private decimal invDiscAmt = 0; //费用优惠金额
        private decimal invDiscAmtL = 0; //费用优惠本币金额
        private decimal invChgAmt = 0; //费用其他变动金额
        private decimal invChgAmtL = 0; //费用其他变动本币金额
        private decimal invActPayAmt = 0; //费用实际应结金额
        private decimal invActPayAmtL = 0; //费用实际应结本币金额
        private decimal invPaidAmt = 0; //费用已结金额
        private decimal invPaidAmtL = 0; //费用已结本币金额
        private int invType = 1; //结算类型
        private int invDetStatus = 1; //费用明细状态
        private string note = ""; //备注
        private int rentType;     //租金费用类别

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

        public decimal InvChgAmt
        {
            get { return invChgAmt; }
            set { invChgAmt = value; }
        }

        public decimal InvChgAmtL
        {
            get { return invChgAmtL; }
            set { invChgAmtL = value; }
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

        public int RentType
        {
            get { return rentType; }
            set { rentType = value; }
        }

        public override string GetTableName()
        {
            return "InvoiceDetail";
        }

        public override string GetColumnNames()
        {
            return "InvDetailID,ChargeTypeID,InvID,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL," +
                "InvDiscAmt,InvDiscAmtL,InvChgAmt,InvChgAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvType,InvDetStatus,Note,RentType";
        }

        public override string GetInsertColumnNames()
        {
            return "InvDetailID,ChargeTypeID,InvID,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL," +
                "InvDiscAmt,InvDiscAmtL,InvChgAmt,InvChgAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvType,InvDetStatus,Note,RentType";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
