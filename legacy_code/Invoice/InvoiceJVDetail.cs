using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 联营返款明细
    /// </summary>
    public class InvoiceJVDetail : BasePO
    {
        #region Model
        private int invjvdetailid;
        private int invid;
        private int renttype;
        private DateTime period;
        private DateTime invstartdate;
        private DateTime invenddate;
        private int invcurtypeid;
        private decimal invexrate;
        private decimal invsalesamt;
        private decimal invsalesamtl;
        private decimal invpcent;
        private decimal intaxrate;
        private decimal outtaxrate;
        private decimal invjvcostamt;
        private decimal invjvcostamtl;
        private decimal invpayamt;
        private decimal invpayamtl;
        private decimal invadjamt;
        private decimal invadjamtl;
        private decimal invactpayamt;
        private decimal invactpayamtl;
        private decimal invpaidamt;
        private decimal invpaidamtl;
        private int invdetstatus;
        private string note;
        private int chargeTypeID;

        /// <summary>
        /// 返款结算单明细ID
        /// </summary>
        public int invJVDetailID
        {
            set { invjvdetailid = value; }
            get { return invjvdetailid; }
        }
        /// <summary>
        /// 结算单ID
        /// </summary>
        public int InvID
        {
            set { invid = value; }
            get { return invid; }
        }
        /// <summary>
        /// 租金类别
        /// </summary>
        public int RentType
        {
            set { renttype = value; }
            get { return renttype; }
        }
        /// <summary>
        /// 费用记帐月
        /// </summary>
        public DateTime Period
        {
            set { period = value; }
            get { return period; }
        }
        /// <summary>
        /// 费用开始日期
        /// </summary>
        public DateTime InvStartDate
        {
            set { invstartdate = value; }
            get { return invstartdate; }
        }
        /// <summary>
        /// 费用结束日期
        /// </summary>
        public DateTime InvEndDate
        {
            set { invenddate = value; }
            get { return invenddate; }
        }
        /// <summary>
        /// 结算币种
        /// </summary>
        public int InvCurTypeID
        {
            set { invcurtypeid = value; }
            get { return invcurtypeid; }
        }
        /// <summary>
        /// 结算汇率
        /// </summary>
        public decimal InvExRate
        {
            set { invexrate = value; }
            get { return invexrate; }
        }
        /// <summary>
        /// 结算销售金额
        /// </summary>
        public decimal invSalesAmt
        {
            set { invsalesamt = value; }
            get { return invsalesamt; }
        }
        /// <summary>
        /// 结算销售金额本币
        /// </summary>
        public decimal invSalesAmtL
        {
            set { invsalesamtl = value; }
            get { return invsalesamtl; }
        }
        /// <summary>
        /// 结算抽成率
        /// </summary>
        public decimal invPcent
        {
            set { invpcent = value; }
            get { return invpcent; }
        }
        /// <summary>
        /// 进项税率
        /// </summary>
        public decimal InTaxRate
        {
            set { intaxrate = value; }
            get { return intaxrate; }
        }
        /// <summary>
        /// 销项税率
        /// </summary>
        public decimal OutTaxRate
        {
            set { outtaxrate = value; }
            get { return outtaxrate; }
        }
        /// <summary>
        /// 联营成本金额
        /// </summary>
        public decimal invJVCostAmt
        {
            set { invjvcostamt = value; }
            get { return invjvcostamt; }
        }
        /// <summary>
        /// 联营成本本币金额
        /// </summary>
        public decimal invJVCostAmtL
        {
            set { invjvcostamtl = value; }
            get { return invjvcostamtl; }
        }
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal InvPayAmt
        {
            set { invpayamt = value; }
            get { return invpayamt; }
        }
        /// <summary>
        /// 结算本币金额
        /// </summary>
        public decimal InvPayAmtL
        {
            set { invpayamtl = value; }
            get { return invpayamtl; }
        }
        /// <summary>
        /// 调整金额
        /// </summary>
        public decimal InvAdjAmt
        {
            set { invadjamt = value; }
            get { return invadjamt; }
        }
        /// <summary>
        /// 调整本币金额
        /// </summary>
        public decimal InvAdjAmtL
        {
            set { invadjamtl = value; }
            get { return invadjamtl; }
        }
        /// <summary>
        /// 实际应结金额
        /// </summary>
        public decimal InvActPayAmt
        {
            set { invactpayamt = value; }
            get { return invactpayamt; }
        }
        /// <summary>
        /// 实际应结本币金额
        /// </summary>
        public decimal InvActPayAmtL
        {
            set { invactpayamtl = value; }
            get { return invactpayamtl; }
        }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal InvPaidAmt
        {
            set { invpaidamt = value; }
            get { return invpaidamt; }
        }
        /// <summary>
        /// 付款本币金额
        /// </summary>
        public decimal InvPaidAmtL
        {
            set { invpaidamtl = value; }
            get { return invpaidamtl; }
        }
        /// <summary>
        /// 费用明细状态
        /// </summary>
        public int InvDetStatus
        {
            set { invdetstatus = value; }
            get { return invdetstatus; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            set { note = value; }
            get { return note; }
        }

        /// <summary>
        /// 费用类型ID
        /// </summary>
        public int ChargeTypeID
        {
            set { chargeTypeID = value; }
            get { return chargeTypeID; }
        }
        #endregion Model

        public static int RENTTYPE_NO_RENT = 0;   //非租金
        public static int RENTTYPE_FIXED_DAY = 1;   //固定日租金
        public static int RENTTYPE_FIXED_MONTH = 2;   //固定月租金
        public static int RENTTYPE_FIXED_P = 3;   //固定抽成租金
        public static int RENTTYPE_SINGLE_P = 4;   //单级抽成租金
        public static int RENTTYPE_MUNCH_P = 5;   //多级抽成租金
        public static int RENTTYPE_FIXED_M = 6;   //固定保底租金
        public static int RENTTYPE_LEVEL_M = 7;   //级别保底租金
        public static int RENTTYPE_ONCE = 8;   //一次性租金

        public static int INVDETSTATUS_AVAILABILITY = 1;//有效
        public static int INVDETSTATUS_PART_BACKING_OUT = 2;//部分结算
        public static int INVDETSTATUS_FULL_BACKING_OUT = 3;//全部结算
        public static int INVDETSTATUS_CANCEL = 4;      //取消
        

        public override string GetTableName()
        {
            return "InvoiceJVDetail";
        }

        public override string GetColumnNames()
        {
            return "InvJVDetailID,InvID,RentType,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,invSalesAmt,invSalesAmtL,invPcent,InTaxRate,OutTaxRate,invJVCostAmt,invJVCostAmtL,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvDetStatus,Note,ChargeTypeID";
        }

        public override string GetInsertColumnNames()
        {
            return "invJVDetailID,InvID,RentType,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,invSalesAmt,invSalesAmtL,invPcent,InTaxRate,OutTaxRate,invJVCostAmt,invJVCostAmtL,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvDetStatus,Note,ChargeTypeID";
        }

        public override string GetUpdateColumnNames()
        {
            return "InvID,RentType,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,invSalesAmt,invSalesAmtL,invPcent,InTaxRate,OutTaxRate,invJVCostAmt,invJVCostAmtL,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvDetStatus,Note,ChargeTypeID";
        }
    }
}
