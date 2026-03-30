using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 结算单主表
    /// </summary>
    public class InvoiceHeader:BasePO
    {
        private int invID = 0;
        private int currTypeID = 0;
        private string invCode = "";
        private string custName = "";
        private DateTime invDate = DateTime.Now;  //结算单生成日
        private DateTime invPeriod = DateTime.Now; //结算单记账月
        private int invStatus = 0; //结算单状态
        private int invPayStatus = 0; //结算单结算标志
        private int invType; //结算类型
        private int isFirst; //是否首期
        private int invCurrTypeID; //结算币种
        private decimal invExRate = 0; //结算汇率
        private decimal invPayAmt = 0; //结算金额
        private decimal invPayAmtL = 0; //结算本币金额
        private decimal invAdjAmt = 0; //调整金额
        private decimal invAdjAmtL = 0; //调整本币金额
        private decimal invDiscAmt = 0; //优惠金额
        private decimal invDiscAmtL = 0; //优惠本币金额
        private decimal invChngAmt = 0; //其它变动金额
        private decimal invChngAmtL = 0; //其它变动本币金额
        private decimal invActPayAmt = 0; //实际应结金额
        private decimal invActPayAmtL = 0; //实际应结本币金额
        private decimal invPaidAmt = 0; //已结金额
        private decimal invPaidAmtL = 0; //已结本币金额
        private string note = ""; //备注


        public int InvID
        {
            get { return invID; }
            set { invID = value; }
        }

        public int CurrTypeID
        {
            get { return currTypeID; }
            set { currTypeID = value; }
        }

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public DateTime InvDate
        {
            get { return invDate; }
            set { invDate = value; }
        }

        public DateTime InvPeriod
        {
            get { return invPeriod; }
            set { invPeriod = value; }
        }

        public int InvStatus
        {
            get { return invStatus; }
            set { invStatus = value; }
        }

        public int InvPayStatus
        {
            get { return invPayStatus; }
            set { invPayStatus = value; }
        }

        public int InvType
        {
            get { return invType; }
            set { invType = value; }
        }

        public int IsFirst
        {
            get { return isFirst; }
            set { isFirst = value; }
        }

        public int InvCurrTypeID
        {
            get { return invCurrTypeID; }
            set { invCurrTypeID = value; }
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

        public string Note
        {
            get { return note; }
            set { note = value; }
        }


        public override string GetTableName()
        {
            return "InvoiceHeader";
        }

        public override string GetColumnNames()
        {
            return "InvID,CurrTypeID,InvCode,CustName,InvDate,InvPeriod,InvStatus,InvPayStatus,InvType,IsFirst,InvCurrTypeID,InvExRate,InvPayAmt,InvPayAmtL," +
                    "InvAdjAmt,InvAdjAmtL,InvDiscAmt,InvDiscAmtL,InvChngAmt,InvChngAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "InvID,CurrTypeID,InvCode,CustName,InvDate,InvPeriod,InvStatus,InvPayStatus,InvType,IsFirst,InvCurrTypeID,InvExRate,InvPayAmt,InvPayAmtL," +
                   "InvAdjAmt,InvAdjAmtL,InvDiscAmt,InvDiscAmtL,InvChngAmt,InvChngAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        //是否首其
        public static int ISFIRST_NO = 0;
        public static int ISFIRST_YES = 1;
    }
}
