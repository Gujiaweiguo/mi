using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Associator.Perform;
using Base.Biz;

namespace Associator.Perform
{
    /// <summary>
    /// 免费发放记录PO
    /// </summary>
    public class FreeGiftTransPO
    {
        /// <summary>
        /// 根据会员卡号获取会员免费领取赠品记录
        /// </summary>
        /// <param name="cardID">会员卡ID</param>
        /// <param name="giftID">赠品ID</param>
        /// <param name="actID">活动ID</param>
        /// <returns></returns>
        public static int GetFreeGiftTransByID(int membID,int giftID,int actID,int giftOption)
        {
            string whereStr = "";
            if (giftOption == GiftActivity.GIFTACTIVITY_DAY)
            {
                whereStr = " AND ActDate = '" + DateTime.Now.ToShortDateString() + "'";
            }
            else if (giftOption == GiftActivity.GIFTACTIVITY_ONCE)
            {
                whereStr = " AND 1=1";
            }
            string str_sql = "SELECT FreeGiftTrans.GiftTransID,FreeGiftTrans.GiftID,FreeGiftTrans.ActID,FreeGiftTrans.MembID,FreeGiftTrans.ActDate,FreeGiftTrans.GiftQty" +
                            " FROM FreeGiftTrans WHERE FreeGiftTrans.MembID = " + membID +
                            " AND FreeGiftTrans.GiftID = " + giftID +
                            " AND FreeGiftTrans.ActID = " + actID + whereStr;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            int count = ds.Tables[0].Rows.Count;
            int flag = 0;
            if (count > 0)
            {
                flag = 1;
            }
            else if (count == 0)
            {
                flag = 0;
            }

            return flag;
        }

        public static DataSet GiftActivityByID(int counterID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT GiftActivity.ActID,GiftActivity.GiftID,GiftActivity.ActDesc,GiftActivity.ShopStartDate,GiftActivity.ShopEndDate,GiftActivity.GiftOption FROM GiftActivity,Gift,GiftStock WHERE GiftActivity.GiftID = Gift.GiftID AND GiftStock.StockCnt > 0 AND GiftStock.GiftID = GiftActivity.GiftID AND GiftStock.CounterID = " + counterID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
