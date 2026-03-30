using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;

namespace Lease.InvoicePara
{
    public class InvoiceParaPO
    {
        /// <summary>
        /// 获取租赁结算单默认报表参数
        /// </summary>
        /// <returns></returns>
        public static DataSet GetInvoiceParaDefault()
        {
            string str_sql = "SELECT TOP 1 InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7" +
                             " FROM InvoicePara " +
                             " WHERE IsDefault = 1 and ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取子公司租赁结算单报表参数
        /// </summary>
        /// <param name="intSubsID">子公司ID</param>
        /// <returns></returns>
        public static DataSet GetInvoiceParaDefault(int intSubsID)
        {
            string str_sql = "";
            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();

            str_sql = "SELECT TOP 1 InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7"+
                      " FROM InvoicePara" +
                      " WHERE ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES + " And SubsID=" + intSubsID ;
            ds = baseBO.QueryDataSet(str_sql);

            //如果子公司没有结算单或参数为0时，使用默认
            if (ds.Tables[0].Rows.Count != 1)
            {
                str_sql = "SELECT TOP 1 InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7" +
                         " FROM InvoicePara" +
                         " WHERE IsDefault = 1 and ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES;
                ds = baseBO.QueryDataSet(str_sql);
            }
            
            return ds;
        }

        /// <summary>
        /// 根据参数ID获取结算单报表参数
        /// </summary>
        /// <param name="invoiceParaID">报表参数ID</param>
        /// <returns></returns>
        public static DataSet GetInvoiceParaByID(int invoiceParaID)
        {
            string str_sql = "SELECT InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7" +
                             " FROM InvoicePara " +
                             " WHERE ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES +
                             " AND InvoiceParaID = " + invoiceParaID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
