using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.DB;
using Base.Biz;
using Lease.ConShop;

namespace Sell
{
    /// <summary>
    /// 取消销售导入
    /// </summary>
    public class CancelTransPO
    {
        /// <summary>
        /// 根据日期查询交易日报
        /// </summary>
        /// <param name="bizDate">计帐日期</param>
        /// <returns></returns>
        public static DataSet GetTransDailyByDate(DateTime bizDate)
        {
            string str_sql = "select ShopID,ShopName,Batchid,min(transid) as minTransID,max(transid) as maxTransID,Count(*) as transNumber,sum(PaidAmt) as sumPaidAmt " +
                             " from transskumedia where BizDate = '" + bizDate + 
                             "' group by ShopID,ShopName,Batchid";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据商铺ID查询商铺号
        /// </summary>
        /// <param name="shopID">商铺ID</param>
        /// <returns></returns>
        public static string GetShopCodeByShopID(int shopID)
        {
            string shopCode = "";
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ShopID = " + shopID;
            Resultset rs = baseBO.Query(new ConShop());
            if (rs.Count == 1)
            {
                ConShop conShop = rs.Dequeue() as ConShop;
                shopCode = conShop.ShopCode;
            }
            return shopCode;
        }
    }
}
