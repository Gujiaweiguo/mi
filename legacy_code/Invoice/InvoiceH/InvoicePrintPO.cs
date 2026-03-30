using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;

namespace Invoice.InvoiceH
{
    /// <summary>
    /// 结算单打印PO
    /// </summary>
    public class InvoicePrintPO
    {
        /// <summary>
        /// 获取批量打印信息
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="bizMode">合同方式</param>
        /// <returns></returns>
        public static DataSet GetInvoiceBacthPrint(DateTime startDate,DateTime endDate,int bizMode)
        {
            string str_sql = "SELECT MIN(InvoiceHeader.CreateTime) as CreateTime,BancthID FROM InvoiceHeader,Contract Where InvoiceHeader.CreateTime >= '" + startDate + "' and InvoiceHeader.CreateTime <= '" + endDate + "' and Contract.BizMode = " + bizMode + " and Contract.contractid = InvoiceHeader.contractid GROUP BY BancthID";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
