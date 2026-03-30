using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.DB;
using Base.Biz;
using Lease.Contract;
using Invoice.BankCard;
using BaseInfo.User;
using Base;

namespace Invoice.InvoiceH
{
    /// <summary>
    /// PayIn操作类
    /// </summary>
    public class PayInPO
    {
        /// <summary>
        /// 生成代收款
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public static DataSet TotalBankAmtToPayInByDate(string startDate,string endDate)
        {
            //string str_sql = "SELECT BankTransDet.shopid,sum(BankTransDet.BankAmt) as BankAmt,Convert(varchar(10),BankTransTime,120) as BankTransTime" +
            //                   " FROM BankTransDet,ConShop,Contract" +
            //                   " WHERE BankTransDet.shopID = ConShop.ShopID"+
            //                   " AND ConShop.ContractID = Contract.ContractID"+
            //                   " AND Contract.BizMode = "+ Contract.BIZ_MODE_LEASE +
            //                   " AND Convert(varchar(10),BankTransDet.BankTransTime,120) >= '"+ startDate + "'" +
            //                   " AND Convert(varchar(10),BankTransDet.BankTransTime,120) <= '"+ endDate + "'" +
            //                   " AND PayInID=0" +
            //                   " AND ReconcType = " + BankTransDet.BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES +
            //                   " GROUP BY BankTransDet.shopid,Convert(varchar(10),BankTransDet.BankTransTime,120)"+
            //                   " ORDER BY BankTransDet.shopid";

            string str_sql = "SELECT TransSkuMedia.ShopID,sum(TransSkuMedia.NetAmt) as BankAmt,sum(TransSkuMedia.PaidAmt) as PaidAmt,sum(TransSkuMedia.CommRate) as CommRate,bizdate," +
		                                " 1 as CardType"+
                                " FROM TransSkuMedia,ConShop,Contract"+
                                " WHERE ConShop.ContractID = Contract.ContractID"+
                                " and ConShop.shopid = TransSkuMedia.ShopID"+
                                " AND Contract.BizMode = 1"+
                                " AND bizdate >= '" + startDate + "'"+
                                " AND bizdate <= '"+ endDate + "'" +
                                " AND TransSkuMedia.MediaMNo in (401,501,601,701)"+
                                " GROUP BY TransSkuMedia.shopid,bizdate"+
                                " union all"+
                                " SELECT TransSkuMedia.ShopID,sum(TransSkuMedia.NetAmt) as BankAmt,sum(TransSkuMedia.PaidAmt) as PaidAmt,sum(TransSkuMedia.CommRate) as CommRate,bizdate," +
                                       "  2 as CardType" +
                               "  FROM TransSkuMedia,ConShop,Contract"+
                                " WHERE ConShop.ContractID = Contract.ContractID"+
                                " and ConShop.shopid = TransSkuMedia.ShopID"+
                                " AND Contract.BizMode = 1"+
                                " AND bizdate >= '" + startDate + "'" +
                                " AND bizdate <= '" + endDate + "'" +
                                " AND TransSkuMedia.MediaMNo in (402,502,602,702)"+
                                " GROUP BY TransSkuMedia.shopid,bizdate"+
                                " ORDER BY TransSkuMedia.shopid";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据商铺号获取未返还和未抵扣的代收款
        /// </summary>
        /// <param name="shopid">商铺ID</param>
        /// <returns></returns>
        public static decimal GetPayInAmtSum(int shopid)
        {
            string str_sql = "SELECT SUM(PayInAmt - PayOutAmtSum) as PayInAmtSum FROM PayIn WHERE ShopID = " + shopid + " AND PayInAmt != PayOutAmtSum AND PayIn.PayInStatus in (" + PayIn.PAYIN_NO_BALANCE_IN_HAND + "," + PayIn.PAYIN_PART_BALANCE_IN_HAND + ")";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            decimal payInAmtSum = 0;
            if (ds.Tables[0].Rows[0]["payInAmtSum"].GetType().ToString() != "System.DBNull")
                payInAmtSum = Convert.ToDecimal(ds.Tables[0].Rows[0]["PayInAmtSum"]);
            return payInAmtSum;
        }

        /// <summary>
        /// 根据商铺号获取未返还的代收款记录
        /// </summary>
        /// <param name="shopID">商铺号</param>
        /// <returns></returns>
        public static DataSet GetPayInByShopID(int shopID)
        {
            string str_sql = "SELECT PayInID,ShopID,PayInAmt,PayInAmt - PayOutAmtSum AS xPayInAmt,PayOutAmtSum FROM PayIn WHERE PayInAmt != PayOutAmtSum AND ShopID = " + shopID + " AND PayIn.PayInStatus in (" + PayIn.PAYIN_NO_BALANCE_IN_HAND + "," + PayIn.PAYIN_PART_BALANCE_IN_HAND + ") ORDER BY PayInID";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 代收款抵扣、返还
        /// </summary>
        /// <param name="shopID">商铺ID</param>
        /// <param name="PayInAmt">抵扣、返还金额</param>
        /// <param name="baseTeans">事务</param>
        /// <param name="sessionUser">创建人</param>
        /// <param name="invPayID">付款单号</param>
        /// <returns></returns>
        public static int InvPayOutAndPayIn(int shopID, decimal @PayInAmt, BaseTrans baseTeans, SessionUser sessionUser, int invPayID)
        {
            PayIn payIn = new PayIn();
            PayOut payOut = new PayOut();
            DataSet payInDS = PayInPO.GetPayInByShopID(shopID);
            int payInCount = payInDS.Tables[0].Rows.Count;
            decimal tempPaidAmt = @PayInAmt;
            for (int x = 0; x < payInCount; x++)
            {
                decimal tempPayInAmt = Convert.ToDecimal(payInDS.Tables[0].Rows[x]["xPayInAmt"]);
                decimal tempPayOutAmt = Convert.ToDecimal(payInDS.Tables[0].Rows[x]["PayOutAmtSum"]);
                if (tempPaidAmt > 0)
                {
                    if (tempPaidAmt < tempPayInAmt)
                    {
                        payIn.PayOutAmtSum = tempPaidAmt + tempPayOutAmt;
                        payOut.PayOutAmt = tempPaidAmt;
                        tempPaidAmt = tempPaidAmt - tempPaidAmt;
                    }
                    else
                    {
                        payOut.PayOutAmt = tempPayInAmt;
                        payIn.PayOutAmtSum = tempPayInAmt + tempPayOutAmt;
                        tempPaidAmt = tempPaidAmt - tempPayInAmt;
                    }

                    payIn.ModifyTime = DateTime.Now;
                    payIn.ModifyUserID = sessionUser.UserID;

                    if (payIn.PayOutAmtSum < Convert.ToDecimal(payInDS.Tables[0].Rows[x]["PayInAmt"]))
                    {
                        payIn.PayInStatus = PayIn.PAYIN_PART_BALANCE_IN_HAND;
                    }
                    if (payIn.PayOutAmtSum == Convert.ToDecimal(payInDS.Tables[0].Rows[x]["PayInAmt"]))
                    {
                        payIn.PayInStatus = PayIn.PAYIN_BALANCE_IN_HAND;
                    }

                    baseTeans.WhereClause = "PayInID = " + Convert.ToInt32(payInDS.Tables[0].Rows[x]["PayInID"]);
                    if (baseTeans.Update(payIn) < 1)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                        baseTeans.Rollback();
                        return -1;
                    }

                    //*插入代收款返还信息
                    payOut.PayOutID = BaseApp.GetPayOutID();
                    payOut.PayInID = Convert.ToInt32(payInDS.Tables[0].Rows[x]["PayInID"]);
                    payOut.CreateUserID = sessionUser.UserID;
                    payOut.CreateTime = DateTime.Now;
                    payOut.ModifyUserID = sessionUser.UserID;
                    payOut.ModifyTime = DateTime.Now;
                    payOut.OprDeptID = sessionUser.DeptID;
                    payOut.OprRoleID = sessionUser.RoleID;
                    payOut.PayOutDate = DateTime.Now;
                    payOut.InvPayID = invPayID;
                    if (invPayID != 0)
                    {
                        payOut.PayOutType = PayOut.PAYOUT_MORTAGAGE;
                    }
                    else
                    {
                        payOut.PayOutType = PayOut.PAYOUT_BACKING_OUT_SHOP;
                    }
                    payOut.PayOutStatus = PayOut.PAYOUT_UP_TO_SNUFF;

                    if (baseTeans.Insert(payOut) < 1)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                        baseTeans.Rollback();
                        return -1;
                    }
                }
            }
            return 1;
        }
    }
}
