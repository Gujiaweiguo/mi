using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 积分兑赠品主表PO
    /// </summary>
    public class RedeemHPO
    {
        /// <summary>
        /// 添加积分兑换赠品记录主表
        /// </summary>
        /// <param name="redeemD">积分兑换赠品主表信息</param>
        /// <param name="baseTrans">事物</param>
        /// <returns></returns>
        public static int insertRedeem(RedeemH redeemH, BaseTrans baseTrans)
        {
            int result = baseTrans.Insert(redeemH);
            return result;
        }
    }
}
