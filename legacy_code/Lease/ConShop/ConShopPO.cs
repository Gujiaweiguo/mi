using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;

namespace Lease.ConShop
{
    /// <summary>
    /// 商铺PO
    /// </summary>
    public class ConShopPO
    {
        /// <summary>
        /// 根据商铺ID获取商铺信息
        /// </summary>
        /// <param name="shopID">商铺ID</param>
        /// <returns></returns>
        public static DataSet GetConShopByID(int shopID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select ShopID,ShopCode,ShopName,BrandID from ConShop where ShopID = " + shopID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
