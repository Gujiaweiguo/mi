using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

/// <summary>
/// 编写人:hesijian
/// 编写时间：2009年6月18日
/// </summary>

namespace Associator.Perform
{
    /// <summary>
    /// 商铺积分率PO
    /// </summary>
    public class BonusGPPO
    {
        /// <summary>
        /// 根据商铺号获取商铺积分率
        /// </summary>
        /// <param name="shopID">商铺号</param>
        /// <returns></returns>
        public static DataSet GetBonusGpPerByShopID(int shopID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT BonusGpPer FROM BonusGp WHERE ShopID = " + shopID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 修改商铺积分率表
        /// </summary>
        /// <param name="shopID">商铺号</param>
        /// <param name="BonusGpPer">积分率</param>
        public static void UpdateBonusGp(int shopID, decimal newBonusRate, BaseTrans baseTrans)
        {
            string str_sql = "UPDATE BonusGp SET BonusGpPer = " + newBonusRate + " WHERE ShopID = " + shopID;
            baseTrans.ExecuteUpdate(str_sql);
        }

        /// <summary>
        /// 根据查询条件获取商铺积分率
        /// </summary>
        /// <param name="whrStr">查询条件</param>
        /// <returns></returns>
        public static DataSet GetBonusGpPerByWhereStr(string whrStr)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT BonusGp.ShopID,BonusGp.BonusGpPer FROM BonusGp WHERE 1=1 AND " + whrStr;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
       
    }
}
