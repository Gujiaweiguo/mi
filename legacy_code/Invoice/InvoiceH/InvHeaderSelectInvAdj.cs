using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice.InvoiceH
{
    public class InvHeaderSelectInvAdj:BasePO
    {
        #region 私有属性
        /// <summary>
        /// 结算调整单ID
        /// </summary>
        private int invAdjID = 0;

        /// <summary>
        /// 结算单编码
        /// </summary>
        private string invCode = "";

        /// <summary>
        /// 用户名称
        /// </summary>
        private string custName = "";

        /// <summary>
        /// 结算单记账月
        /// </summary>
        private DateTime invPeriod = DateTime.Now;

        /// <summary>
        /// 结算单生成日期
        /// </summary>
        private DateTime invDate = DateTime.Now;

        /// <summary>
        /// 结算金额
        /// </summary>
        private decimal thisPaid = 0;

        /// <summary>
        /// 调整总金额
        /// </summary>
        private decimal adjAmt = 0;

        #endregion

        #region 公共属性
        /// <summary>
        /// 结算调整单ID
        /// </summary>
        public int InvAdjID
        {
            get { return invAdjID; }
            set { invAdjID = value; }
        }

        /// <summary>
        /// 结算单编码
        /// </summary>
        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        /// <summary>
        /// 结算单记账月
        /// </summary>
        public DateTime InvPeriod
        {
            get { return invPeriod; }
            set { invPeriod = value; }
        }

       /// <summary>
        /// 结算单生成日期
        /// </summary>
        public DateTime InvDate
        {
            get { return invDate; }
            set { invDate = value; }
        }

       /// <summary>
        /// 结算金额
        /// </summary>
        public decimal ThisPaid
        {
            get { return thisPaid; }
            set { thisPaid = value; }
        }

       /// <summary>
        /// 调整金额
        /// </summary>
        public decimal AdjAmt
        {
            get { return adjAmt; }
            set { adjAmt = value; }
        }
        #endregion

        #region 公共方法
        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "InvAdjID,InvCode,CustName,InvDate,InvPeriod,AdjAmt,ThisPaid";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "Select InvAdjID,InvCode,CustName,InvDate,InvPeriod,AdjAmt,InvPayAmt+InvAdjAmt+InvDiscAmt+InvChngAmt as InvActPayAmt, " +
                    "InvPayAmtL+InvAdjAmtL+InvDiscAmtL+InvChngAmtL as InvActPayAmtL, " +
                    "InvActPayAmt - InvPaidAmt as ThisPaid From Invadj a Left Join InvoiceHeader b on a.InvID=b.InvID";
        }
        #endregion
    }
}
