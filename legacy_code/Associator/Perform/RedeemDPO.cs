using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 积分兑赠品明细表PO
    /// </summary>
    public class RedeemDPO
    {
        /// <summary>
        /// 添加积分兑换赠品记录明细表
        /// </summary>
        /// <param name="redeemD">积分兑换赠品明细信息</param>
        /// <param name="baseTrans">事物</param>
        /// <returns></returns>
        public static int insertRedeem(RedeemD redeemD,BaseTrans baseTrans)
        {
            int result = baseTrans.Insert(redeemD);
            return result;
        }
    }
}
