using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.DB;
using Base.Biz;

namespace Invoice.InvoiceH
{
    /// <summary>
    /// 结算主表Bo
    /// </summary>
    public class InvoiceHeaderBo
    {

        /// <summary>
        /// 合计结算主表费用实际应结金额
        /// </summary>
        /// <param name="invId">结算单ID</param>
        /// <returns></returns>
        public static decimal SumInvoiceHeaderPayAmtTotal(int invId)
        {
            decimal headerInvPayAmt = 0;
            BaseBO baseBo = new BaseBO();
            string str_sql = "select sum(InvActPayAmt) as onInvActPayAmt from InvoiceHeader where InvID = " + invId;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["onInvActPayAmt"].ToString() == "")
                return headerInvPayAmt;
            else
                return headerInvPayAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["onInvActPayAmt"]);
        }

        /// <summary>
        /// 合计结算主表费用已结金额
        /// </summary>
        /// <param name="invId">结算单ID</param>
        /// <returns></returns>
        public static decimal SumInvoiceHeaderPaidAmtTotal(int invId)
        {
            decimal headerInvPaidAmt = 0;
            BaseBO baseBo = new BaseBO();
            string str_sql = "select sum(InvPaidAmt) as onInvPaidAmt from InvoiceHeader where InvID = " + invId;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["onInvPaidAmt"].ToString() == "")
                return headerInvPaidAmt;
            else
                return headerInvPaidAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["onInvPaidAmt"]);
        }
    }
}
