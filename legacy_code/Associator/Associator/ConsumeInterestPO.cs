using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

namespace Associator.Associator
{
    /// <summary>
    /// 消费兴趣PO
    /// </summary>
    public class ConsumeInterestPO
    {
        /// <summary>
        /// 获取有效的消费兴趣
        /// </summary>
        /// <returns></returns>
        public static DataSet GetInterestItem()
        {
            string str_sql = "SELECT IItemID,IItemName FROM InterestItem WHERE IItemStatus = 1";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
