using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 会员消费查询
    /// </summary>
    public class PurhistPO
    {
        /// <summary>
        /// 根据查询条件获取会员消费记录
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public static DataSet GetPurhistByWhere(string strWhere)
        {
            string str_sql = "SELECT Purhist.MembCardID,ConShop.ShopCode,Purhist.TransDt,Purhist.NetAmt,Purhist.BonusAmt,Purhist.EntryBy" +
                            " FROM Purhist,ConShop " +
                            " WHERE Purhist.ShopID = ConShop.ShopID " + strWhere;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
