using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;

namespace Lease.ConShop
{
    /// <summary>
    /// 硬件信息PO
    /// </summary>
    public class ShopHdwPO
    {
        /// <summary>
        /// 保存硬件信息
        /// </summary>
        /// <param name="basePO">硬件信息PO</param>
        /// <returns></returns>
        public static int InsertShopHdw(BasePO basePO)
        {
            BaseBO baseBO = new BaseBO();
            int result = baseBO.Insert(basePO);
            return result;
        }

        /// <summary>
        /// 修改硬件信息
        /// </summary>
        /// <param name="shopHdw">硬件信息</param>
        /// <returns></returns>
        public static int UpdateShopHdwByID(ShopHdw shopHdw)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "HdwID = " + shopHdw.HdwID;
            int result = baseBO.Update(shopHdw);
            return result;
        }

        /// <summary>
        /// 根据硬件ID获取硬件信息
        /// </summary>
        /// <param name="shopHdwID">硬件ID</param>
        /// <returns></returns>
        public static Resultset GetShopHdwByID(int shopHdwID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "HdwID = " + shopHdwID;
            Resultset rs = baseBO.Query(new ShopHdw());
            return rs;
        }


    }
}
