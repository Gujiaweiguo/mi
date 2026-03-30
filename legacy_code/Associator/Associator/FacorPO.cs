using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

namespace Associator.Associator
{
    /// <summary>
    /// 个人爱好PO
    /// </summary>
    public class FacorPO
    {
        /// <summary>
        /// 获取有效的个人爱好
        /// </summary>
        /// <returns></returns>
        public static DataSet GetFavorItem()
        {
            string str_sql = "SELECT FItemID,FItemName  FROM FavorItem WHERE FItemStatus = 1";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
