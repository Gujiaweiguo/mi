using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;

namespace Lease.InvoicePara
{
    public class InvoiceJVParaPO
    {
        /// <summary>
        /// 获取联营结算单默认报表参数
        /// </summary>
        /// <returns></returns>
        public static DataSet GetInvoiceJVParaDefault()
        {
            string str_sql = "SELECT TOP 1 InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7" +
                             " FROM InvoiceJVPara " +
                             " WHERE IsDefault = 1 and ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据参数ID获取结算单报表参数
        /// </summary>
        /// <param name="invoiceParaID">报表参数ID</param>
        /// <returns></returns>
        public static DataSet GetInvoiceJVParaByID(int invoiceParaID)
        {
            string str_sql = "SELECT InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7" +
                             " FROM InvoiceJVPara " +
                             " WHERE ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES +
                             " AND InvoiceJVParaID = " + invoiceParaID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
