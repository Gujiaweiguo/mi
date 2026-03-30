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
    public class PayOutPO
    {
        /// <summary>
        /// 获取未抵扣和返还的代收款金额
        /// </summary>
        /// <param name="contractCode">合同号</param>
        /// <param name="shopID">商铺ID</param>
        /// <returns></returns>
        public static DataSet GetPayInByID(string contractCode,string shopID)
        {
            string str_sql = "SELECT  PayIn.PayInID,PayIn.PayInCode,PayIn.PayInAmt-PayIn.PayOutAmtSum AS PayInAmt," +
                                    " PayIn.PayInDate,PayIn.PayOutAmtSum," +
                                    " ConShop.ShopID,ConShop.ShopCode," +
                                    " Customer.CustName,Contract.ContractCode,Contract.ContractID" +
                               " FROM PayIn,Conshop,Customer,Contract" +
                            " WHERE PayIn.ShopID = Conshop.ShopID" +
                            " AND Customer.CustID = Contract.CustID" +
                            " AND Contract.ContractID = Conshop.ContractID"+
                            " AND PayIn.PayInAmt > 0 " +
                            " AND PayIn.PayInStatus in (" + PayIn.PAYIN_NO_BALANCE_IN_HAND + "," + PayIn.PAYIN_PART_BALANCE_IN_HAND + ")";
            BaseBO baseBO = new BaseBO();
            if (contractCode != "")
            {
                str_sql += " AND Contract.ContractCode = '" + contractCode + "'";
            }
            if (shopID != "")
            {
                str_sql += " AND Conshop.ShopID = '" + shopID + "'";
            }
            if (contractCode == "" && shopID == "")
            {
                str_sql += " AND 1=0";
            }
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
