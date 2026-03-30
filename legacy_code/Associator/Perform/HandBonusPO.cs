using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

/// <summary>
/// 更改人：hesijian
/// 更改时间：2009年6月18日
/// 增加了Receiptid插入字段
/// </summary>

namespace Associator.Perform
{
    /// <summary>
    /// 补登积分PO
    /// </summary>
    public class HandBonusPO
    {
        /// <summary>
        /// 获取会员积分
        /// </summary>
        /// <param name="membID">会员号</param>
        /// <returns></returns>
        public static DataSet GetBonusByMembID(int membID)
        {
            string str_sql = "SELECT BonusID,MembID,BonusTotal,BonusPrev,BonusCurr,BonusBook,BonusReset,TotBonusReset,ResetDate,TransID,Updated"+
                            " FROM Bonus WHERE MembID = " + membID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 补登积分
        /// </summary>
        /// <param name="flag">修改、添加Bonus标志，1：修改；0：添加</param>
        /// <param name="membID">会员号</param>
        /// <param name="cardID">会员卡号</param>
        /// <param name="bonus">积分</param>
        /// <param name="transid">交易号</param>
        /// <param name="transDT">交易时间</param>
        /// <param name="netAmt">交易金额</param>
        /// <param name="userID">用户ID</param>
        /// <param name="shopID">商铺ID</param>
        public static void UpdateOrInsertBouns(int flag, int membID, string cardID, decimal bonus, string transid, DateTime transDT, decimal netAmt, int userID,int shopID,string receiptid,DateTime entryAt)
        {
            BaseTrans baseTrans = new BaseTrans();
            baseTrans.BeginTrans();
            try
            {
                string str_sql1 = "INSERT INTO purhist Values ('" + cardID + "'," + membID + "," + shopID + "," + transid + ",'" + transDT +
                                    "', ''," + netAmt + "," + bonus + ",'" + receiptid + "','" + entryAt + "'," + userID + ")";
                if (flag == 1)
                {
                    string str_sql = "UPDATE Bonus SET BonusBook = BonusBook + " + bonus + ",BonusTotal = BonusTotal + " + bonus + ",BonusPrev = " + bonus +
                                        ", BonusCurr = BonusCurr + " + bonus +
                                        " WHERE MembID = " + membID;
                    baseTrans.ExecuteUpdate(str_sql);
                    
                }
                else if (flag == 0)
                {
                    string str_sql2 = "INSERT INTO Bonus Values (" + membID + "," + bonus + "," + bonus + "," + bonus + "," + bonus + "," + 0 + "," + 0 + ",'" + transDT + "'," + transid + ",'" + entryAt + "')";
                    baseTrans.ExecuteUpdate(str_sql2);
                }
                baseTrans.ExecuteUpdate(str_sql1);
            }
            catch(Exception ex)
            {
                baseTrans.Rollback();
                throw ex;
            }
            baseTrans.Commit();
        }
    }
}
