using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;

namespace Associator.Perform
{
    public class GiftStockPO
    {
        /// <summary>
        /// 根据发放数量修改库存
        /// </summary>
        /// <param name="baseTrans">事物</param>
        /// <param name="counterID">服务台ID</param>
        /// <param name="giftID">赠品ID</param>
        /// <param name="stockCnt">赠品发放数量</param>
        public static void updateGiftStock(BaseTrans baseTrans, int counterID, int giftID, int stockCnt)
        {
            string str_sql = "update GiftStock set StockCnt = StockCnt -" +stockCnt +
                            " where CounterID = " + counterID +
                            "  and GiftID = " + giftID;
            baseTrans.ExecuteUpdate(str_sql);
        }

        public static DataSet GetGiftStockCnt(int giftID, int counterID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT StockCnt FROM GiftStock WHERE CounterID = " + counterID + " AND GiftID = " + giftID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        public static DataSet GetGiftStockCnt1(int actID, int counterID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT StockCnt FROM GiftStock,GiftActivity WHERE GiftStock.GiftID = GiftActivity.GiftID and CounterID = " + counterID + " AND GiftActivity.ActID = " + actID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
