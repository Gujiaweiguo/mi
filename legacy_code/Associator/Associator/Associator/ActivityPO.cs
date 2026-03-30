using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

namespace Associator.Associator
{
    /// <summary>
    /// 活动讯息PO
    /// </summary>
    public class ActivityPO
    {
        /// <summary>
        /// 获取有效的活动讯息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetActivityItem()
        {
            string str_sql = "SELECT AItemID,AItemName  FROM ActivityItem WHERE AItemStatus = 1";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
