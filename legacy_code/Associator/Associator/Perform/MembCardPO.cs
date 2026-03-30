using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;
using Associator.Perform;

namespace Associator
{
    /// <summary>
    /// 会员卡PO
    /// </summary>
    public class MembCardPO
    {
        /// <summary>
        /// 根据会员卡号获取会员卡信息
        /// </summary>
        /// <param name="cardID">会员卡号</param>
        /// <returns></returns>
        public static Resultset GetMembCardInfoByID(string cardID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "MembCardId = '" + cardID + "'";
            Resultset rs = baseBO.Query(new MembCard());
            return rs;
        }

        /// <summary>
        /// 修改会员卡信息
        /// </summary>
        /// <param name="mbCard">卡信息</param>
        /// <returns></returns>
        public static int ModifyMembCardInfo(MembCard mbCard)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "MembCardId = '" + mbCard.MembCardId + "'";
            int result = baseBO.Update(mbCard);
            return result;
        }

        ///// <summary>
        ///// 根据会员卡号修改积分
        ///// </summary>
        ///// <param name="score">积分</param>
        ///// <param name="mbCardID">会员卡号</param>
        ///// <param name="baseTrans">事物</param>
        ///// <returns></returns>
        //public static int ModifyMembCardScoreByID(int score,string mbCardID,BaseTrans baseTrans)
        //{
        //    string str_sql = "update MembCard set TotalScore = " + score + " where MembCardID = '" + mbCardID+ "'";
        //    int result = baseTrans.ExecuteUpdate(str_sql);
        //    return result;
        //}


        /// <summary>
        /// 旧会员发卡，修改卡信息
        /// </summary>
        /// <param name="membCardID">旧会员卡号</param>
        /// <param name="newMembCardID">新会员卡号</param>
        /// <param name="modifyTime">修改时间</param>
        /// <param name="modifyUserID">修改人ID</param>
        /// <param name="cardStatusID">卡状态</param>
        /// <param name="baseTrans">事物</param>
        /// <returns></returns>
        public static int ModifyOldMembCardByID(string membCardID,string newMembCardID, DateTime modifyTime,int modifyUserID,string cardStatusID, BaseTrans baseTrans)
        {
            string str_sql = "update MembCard set NewMembCardID = '" + newMembCardID + "',ModifyTime  = '" + modifyTime + "', ModifyUserID = " + modifyUserID + ",CardStatusID = '" + cardStatusID + "' where MembCardID = '" + membCardID + "'";
            int result = baseTrans.ExecuteUpdate(str_sql);
            return result;
        }
    }
}
