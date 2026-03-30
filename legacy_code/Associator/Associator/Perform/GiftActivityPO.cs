using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;
using Associator.Perform;

namespace Associator.Perform
{
    /// <summary>
    /// 赠品发放活动PO
    /// </summary>
    public class GiftActivityPO
    {
        /// <summary>
        /// 获取免费赠品发放活动
        /// </summary>
        /// <returns></returns>
        private static DataSet GetFreeGiftActivity()
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT GiftActivity.ActID,GiftActivity.GiftID,GiftActivity.ActDesc,GiftActivity.ShopStartDate,GiftActivity.ShopEndDate,GiftActivity.GiftOption" +
                                " FROM GiftActivity,Gift" +
                                " WHERE Gift.GiftID = GiftActivity.GiftID" +
                                " AND Gift.FreeGift = " + Gift.FREEGIFT_YES;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据活动ID获取活动内容
        /// </summary>
        /// <param name="actID">活动ID</param>
        /// <param name="freeGift">是否免费发放</param>
        /// <returns></returns>
        public static DataSet GetGiftActivityByID(int actID,int freeGift)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT GiftActivity.ActID,GiftActivity.GiftID,GiftActivity.ActDesc,GiftActivity.ShopStartDate,GiftActivity.ShopEndDate,GiftActivity.GiftOption," +
                                " Gift.GiftDesc,Gift.GiftID " +
                                " FROM Gift,GiftActivity " +
                                " WHERE Gift.GiftID = GiftActivity.GiftID " +
                                " AND Gift.FreeGift = " + freeGift +
                                " AND GiftActivity.ActID = " + actID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
