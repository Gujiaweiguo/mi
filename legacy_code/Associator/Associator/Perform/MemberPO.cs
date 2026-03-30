using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 会员PO
    /// </summary>
    public class MemberPO
    {
        /// <summary>
        /// 根据会员ID获取会员信息
        /// </summary>
        /// <param name="membID">会员ID</param>
        /// <returns></returns>
        public static Resultset GetMembInfoByID(int membID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "LCardId = " + membID;
            Resultset rs = baseBO.Query(new LCust());
            return rs;
        }

        /// <summary>
        /// 根据会员号获取会员信息
        /// </summary>
        /// <param name="membCode">会员号</param>
        /// <returns></returns>
        public static Resultset GetMembInfoByCode(int membID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "MembID = " + membID;
            Resultset rs = baseBO.Query(new LCust());
            return rs;
        }
    }
}
