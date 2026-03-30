using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;
using Associator.Perform;

namespace Associator.Perform
{
    /// <summary>
    /// 赠品PO
    /// </summary>
    public class GiftPO
    {
        /// <summary>
        /// 根据赠品ID获取赠品信息
        /// </summary>
        /// <param name="giftID"></param>
        /// <returns></returns>
        public static Resultset GetGiftByID(int giftID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "GiftID = " + giftID;
            Resultset rs = baseBO.Query(new Gift());
            return rs;
        }

        /// <summary>
        /// 根据不同条件获取赠品信息
        /// </summary>
        /// <param name="flag">1:小票兑换;2:积分兑换</param>
        /// <param name="counterID">服务台</param>
        /// <returns></returns>
        public static DataSet GetGift(int flag, int counterID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "";
            if (flag == 1)
            {
                str_sql = "SELECT Gift.GiftID,Gift.GiftDesc,Gift.ExByBonus,Gift.BonusCost,Gift.ExByReceipt,Gift.ReceiptMoney,Gift.LimitOne,Gift.FreeGift,Gift.ShopStartDate,Gift.ShopEndDate FROM Gift,GiftStock WHERE Gift.GiftID = GiftStock.GiftID AND GiftStock.StockCnt > 0 AND GiftStock.CounterID = " + counterID + " AND Gift.ExByReceipt = " + Gift.EXBYRECEIPT_YES;
                //baseBO.WhereClause = "ExByReceipt = " + Gift.EXBYRECEIPT_YES;
            }
            else if(flag == 2)
            {
                str_sql = "SELECT Gift.GiftID,Gift.GiftDesc,Gift.ExByBonus,Gift.BonusCost,Gift.ExByReceipt,Gift.ReceiptMoney,Gift.LimitOne,Gift.FreeGift,Gift.ShopStartDate,Gift.ShopEndDate FROM Gift,GiftStock WHERE Gift.GiftID = GiftStock.GiftID AND GiftStock.StockCnt > 0 AND GiftStock.CounterID = " + counterID + " AND Gift.ExByBonus = " + Gift.EXBYBONUS_YES;
                //baseBO.WhereClause = "ExByBonus = " + Gift.EXBYBONUS_YES;
            }
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据查询条件获取所有赠品兑换记录
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public static DataSet GetAllGift(string strWhere1,string strWhere2,string strWhere3)
        {
            string str_sql = "select Gift.giftDesc,0 as giftType,'' as giftTypeName,FreeGiftTrans.ActDate," +
                              " FreeGiftTrans.GiftQty,0 as RedeemAmt" +
                              " from FreeGiftTrans,Gift,Member,MembCard" +
                              " where Gift.giftid = FreeGiftTrans.giftid" +
                              " and member.membid = membcard.membid" +
                              " and FreeGiftTrans.membid = member.membid" + strWhere1 +
                              " union all" +
                              " select Gift.giftDesc,1 as giftType,'' as giftTypeName,RedeemH.RedeemDate," +
                              " RedeemH.GiftQty as GiftQty,RedeemH.RedeemAmt" +
                              " from RedeemH,Gift,Member,MembCard" +
                              " where Gift.giftid = RedeemH.giftid" +
                              " and member.membid = membcard.membid" +
                              " and RedeemH.membid = member.membid" + strWhere2 +
                              " union all" +
                              " select Gift.giftDesc,2 as giftType,'' as giftTypeName,ExTrans.ExDate," +
                              " ExTrans.GiftQty,0 as RedeemAmt" +
                              " from ExTrans,Gift,Member,MembCard" +
                              " where Gift.giftid = ExTrans.giftid" +
                              " and member.membid = membcard.membid" +
                              " and ExTrans.membid = member.membid" + strWhere3;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
