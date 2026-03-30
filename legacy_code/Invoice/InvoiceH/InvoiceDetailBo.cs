using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.DB;
using Base.Biz;

namespace Invoice.InvoiceH
{
    /// <summary>
    /// 结算明细Bo
    /// </summary>
    public class InvoiceDetailBo
    {
        /// <summary>
        /// 合计结算明细费用实际应结金额
        /// </summary>
        /// <param name="invId">结算单ID</param>
        /// <returns></returns>
        public static decimal SumInvoiceDetailPayAmtTotal(int invId)
        {
            decimal detailInvPayAmt = 0;
            BaseBO baseBo = new BaseBO();
            string str_sql = "select sum(InvActPayAmt) as onInvActPayAmt from InvoiceDetail where InvID = " + invId;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["onInvActPayAmt"].ToString() == "")
                return detailInvPayAmt;
            else
                return detailInvPayAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["onInvActPayAmt"]);
        }

        /// <summary>
        /// 合计结算明细费用已结金额
        /// </summary>
        /// <param name="invId">结算单ID</param>
        /// <returns></returns>
        public static decimal SumInvoiceDetailPaidAmtTotal(int invId)
        {
            decimal detailInvPaidAmt = 0;
            BaseBO baseBo = new BaseBO();
            string str_sql = "select sum(InvPaidAmt) as onInvPaidAmt from InvoiceDetail where InvID = " + invId;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["onInvPaidAmt"].ToString() == "")
                return detailInvPaidAmt;
            else
                return detailInvPaidAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["onInvPaidAmt"]);
        }
    }
}
