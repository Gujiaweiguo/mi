using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;


namespace Invoice.InvoiceH
{
    /// <summary>
    /// 结算单取消PO
    /// </summary>
    public class InvoiceCancelPO
    {
        /// <summary>
        /// 根据结算单ID获取结算金额
        /// </summary>
        /// <param name="invID">结算单ID</param>
        /// <returns></returns>
        public static decimal GetInvPaidAmt(int invID)
        {
            string str_sql = "select sum(InvPaidAmt) as InvPaidAmt from InvoiceDetail where InvID = " + invID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            decimal invPaidAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["InvPaidAmt"]);
            return invPaidAmt;
        }
    }
}
