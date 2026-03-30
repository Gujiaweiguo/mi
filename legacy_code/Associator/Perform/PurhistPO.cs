using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

/// <summary>
/// 更改人：hesijian
/// 更改时间：2009年6月18日
/// 新增了Purhist.TransId 查询字段
/// </summary>

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
            string str_sql = "SELECT Purhist.MembCardID,Purhist.ShopID,ConShop.ShopCode,Purhist.TransDt,Purhist.NetAmt,Purhist.TransId,Purhist.BonusAmt,Purhist.EntryBy" +
                            " FROM Purhist,ConShop " +
                            " WHERE Purhist.ShopID = ConShop.ShopID " + strWhere;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
