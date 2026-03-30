using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;
namespace Associator.Associator
{
    public class AgePO
    {
        /// <summary>
        /// ƒÍ¡‰∂Œ
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAge()
        {
            string str_sql = "SELECT AgeID,Agestr  FROM Age";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
