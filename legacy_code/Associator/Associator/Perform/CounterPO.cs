using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 服务台PO
    /// </summary>
    public class CounterPO
    {
        /// <summary>
        /// 获取服务台信息
        /// </summary>
        /// <returns></returns>
        public static Resultset GetServiceDesk()
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = baseBO.Query(new Counter());
            return rs;
        }
    }
}
