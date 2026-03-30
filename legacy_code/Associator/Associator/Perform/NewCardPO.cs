using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base;
using Base.DB;
using Base.Biz;

namespace Associator.Perform
{
    /// <summary>
    /// 发行新卡PO
    /// </summary>
    public class NewCardPO
    {
        /// <summary>
        /// 获取未发卡会员
        /// </summary>
        /// <returns></returns>
        public static DataSet GetNoCardMember()
        {
            string str_sql = "SELECT MembID,MembCode,MemberName,HomePhone,MobilPhone,DateJoint" + 
                            " FROM Member " +
                            " WHERE MembID " +
                            " NOT IN" +
                            " (SELECT MembID FROM MembCard)";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
