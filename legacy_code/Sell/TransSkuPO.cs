using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using System.Data;
using Base.Biz;

namespace Sell
{
    public class TransSkuPO
    {
        /// <summary>
        /// 根据条件获取交易金额
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <returns></returns>
        public static DataSet GetTransSkuPaidAmt(string strWhere)
        {
            string str_sql = "SELECT TransID,SUM(PaidAmt) as PaidAmt FROM TransSku WHERE " + strWhere + " GROUP BY TransID";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
