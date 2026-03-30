using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 积分PO
    /// </summary>
    public class BonusPO
    {
        /// <summary>
        /// 根据会员号获取总积分
        /// </summary>
        /// <param name="membID">会员号</param>
        /// <returns></returns>
        public static DataSet GetBonusByMembID(int membID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT BonusTotal FROM Bonus WHERE MembID = " + membID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 修改积分表
        /// </summary>
        /// <param name="membID">会员</param>
        /// <param name="overBonus">剩余积分</param>
        /// <param name="baseTrans">事物</param>
        public static void AccountBonus(int membID,decimal overBonus,BaseTrans baseTrans)
        {
            string str_sql = "UPDATE Bonus SET BonusTotal = " + overBonus + " WHERE MembID = " + membID;
            baseTrans.ExecuteUpdate(str_sql);
        }

        /// <summary>
        /// 根据查询条件获取会员总积分
        /// </summary>
        /// <param name="whrStr">查询条件</param>
        /// <returns></returns>
        public static DataSet GetBonusByWhereStr(string whrStr)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT Bonus.BonusTotal,Member.MembID,Member.MembCode,Member.MemberName,Member.HomePhone,Member.MobilPhone,Member.DateJoint,MembCard.MembCardId FROM Bonus,Member,MembCard WHERE Member.MembID = MembCard.MembID AND Bonus.MembID = Member.MembID AND " + whrStr;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
