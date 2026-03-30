using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;
using Lease.Contract;
using Invoice;

namespace Invoice
{
    /// <summary>
    /// 联营结算PO
    /// </summary>
    public class InvUnionReturnPO
    {
        /// <summary>
        /// 获取结算单号
        /// </summary>
        /// <param name="contractCode">合同号</param>
        /// <returns></returns>
        public static DataSet GetInvCode(string contractCode)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.InvCode from InvoiceHeader A ,Contract B " +
                             "where A.ContractID = B.ContractID " +
                             " and B.ContractCode = '" + contractCode +
                             "' and A.InvStatus = " + InvoiceHeader.INVSTATUS_VALID +
                             " and B.BizMode = " + Contract.BIZ_MODE_UNIT +
                             " and A.InvType = " + InvoiceHeader.INVTYPE_UNION +
                             " and ( B.ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR +
                             " or B.ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_END + ")";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单号获取返款结算单
        /// </summary>
        /// <param name="invCode">结算单号</param>
        /// <returns></returns>
        public static DataSet GetInvJVDetail(int invCode)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.InvJVDetailID,A.InvID,A.RentType,A.Period,A.InvStartDate,A.InvEndDate,A.InvCurTypeID,A.InvExRate,A.invSalesAmt,A.invSalesAmtL,A.invPcent * 100 as invPcent,A.InTaxRate as JVTaxRate,A.OutTaxRate as JVTaxRate,A.invJVCostAmt,A.invJVCostAmtL,A.InvPayAmt,A.InvPayAmtL,A.InvAdjAmt,A.InvAdjAmtL,A.invSalesAmt-A.InvPayAmt as InvActPayAmt,A.invSalesAmtL-A.InvPayAmtL as InvActPayAmtL,A.InvPaidAmt,A.InvPaidAmtL,A.InvDetStatus,A.Note,A.ChargeTypeID ,B.ChargeTypeName " +
                             " from InvoiceJVDetail A,ChargeType B where A.ChargeTypeID = B.ChargeTypeID and A.InvID = " + invCode ;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单号获取结算单明细
        /// </summary>
        /// <param name="invCode">结算单号</param>
        /// <returns></returns>
        public static DataSet GetInvDetailByInvID(int invCode)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.InvDetailID,A.ChargeTypeID,A.InvID,A.Period,A.InvStartDate,A.InvEndDate,A.InvCurTypeID,A.InvExRate,A.InvPayAmt,A.InvPayAmtL,A.InvAdjAmt,A.InvAdjAmtL," +
                             "A.InvDiscAmt,A.InvDiscAmtL,A.InvChgAmt,A.InvChgAmtL,A.InvActPayAmt,A.InvActPayAmtL,A.InvPaidAmt,A.InvPaidAmtL,A.InvType,A.InvDetStatus,A.Note,A.RentType,B.ChargeTypeName " +
                             " from InvoiceDetail A, ChargeType B where A.ChargeTypeID = B.ChargeTypeID and A.InvID = " + invCode;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单号获取开票总额
        /// </summary>
        /// <param name="invID">结算单号</param>
        /// <returns></returns>
        public static decimal SumInvJVCostAmtByInvID(int invID)
        {
            decimal invJVCostAmt = 0;
            BaseBO baseBO = new BaseBO();
            string str_sql = "select sum(InvJVCostAmt) as InvJVCostAmt from InvoiceJVDetail where InvID = " + invID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["InvJVCostAmt"].ToString() == "")
                return invJVCostAmt;
            else
                return Convert.ToDecimal(ds.Tables[0].Rows[0]["InvJVCostAmt"]);
        }

        /// <summary>
        /// 根据结算单号获取扣款总额
        /// </summary>
        /// <param name="invID">结算单号</param>
        /// <returns></returns>
        public static decimal SumInvActPayAmt(int invID)
        {
            decimal invActPayAmt = 0;
            BaseBO baseBO = new BaseBO();
            string str_sql = "select sum(InvActPayAmt) as InvActPayAmt from InvoiceDetail where InvID = " + invID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["InvActPayAmt"].ToString() == "")
                return invActPayAmt;
            else
                return Convert.ToDecimal(ds.Tables[0].Rows[0]["InvActPayAmt"]);
        }

        /// <summary>
        /// 根据合同号获取客户信息
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <returns></returns>
        public static DataSet GetCustomer(string concode)
        {
            string str_sql = "select A.CustCode,A.CustName,A.TaxCode,A.BankName,A.BankAcct from Customer A,Contract B "+
                             " where A.CustID = B.CustID and B.ContractCode = '" + concode + "'";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单号获取汇率
        /// </summary>
        /// <param name="invID"></param>
        /// <returns></returns>
        public static decimal GetExRateByInvID(int invID)
        {
            string str_sql = "select InvExRate from InvoiceHeader where InvID = " + invID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return Convert.ToDecimal(ds.Tables[0].Rows[0]["InvExRate"]);
        }

        /// <summary>
        /// 返款
        /// </summary>
        /// <param name="invID">结算单ID</param>
        /// <returns></returns>
        public static void ReturnMoney(int invID)
        {
            BaseBO baseBO = new BaseBO();
            decimal exRate = GetExRateByInvID(invID);
            DataSet invDetailDS = baseBO.QueryDataSet("select InvDetailID,InvActPayAmt,InvActPayAmtL from InvoiceDetail where InvID = " + invID);
            DataSet invJVDS = baseBO.QueryDataSet("select invJVDetailID,InvActPayAmt,InvActPayAmtL from InvoiceJVDetail where InvID = " + invID);
            int invDetailCount = invDetailDS.Tables[0].Rows.Count;
            int invJVDSCount = invJVDS.Tables[0].Rows.Count;
            BaseTrans trans = new BaseTrans();
            string str_sql_invHeader = "update InvoiceHeader set InvStatus = " + InvoiceHeader.INVSTATUS_CLOSING + " where InvID = " + invID;
            try
            {
                trans.BeginTrans();
                trans.ExecuteUpdate(str_sql_invHeader);
                for (int i = 0; i < invDetailCount; i++)
                {
                    trans.ExecuteUpdate("update InvoiceDetail set InvDetStatus = " + Invoice.InvoiceH.InvoiceDetail.INVOICEDETAIL_FULL_BACKING_OUT +
                                        " , InvPaidAmt = " + Convert.ToDecimal(invDetailDS.Tables[0].Rows[i]["InvActPayAmt"]) +
                                        " , InvPaidAmtL = " + Convert.ToDecimal(invDetailDS.Tables[0].Rows[i]["InvActPayAmtL"]) +
                                        " where InvDetailID = " + Convert.ToInt32(invDetailDS.Tables[0].Rows[i]["InvDetailID"]));
                    
                }
                for (int j = 0; j < invJVDSCount; j++)
                {
                    trans.ExecuteUpdate("update InvoiceJVDetail set InvDetStatus = " + InvoiceJVDetail.INVDETSTATUS_FULL_BACKING_OUT +
                                        " , InvPaidAmt = " + Convert.ToDecimal(invJVDS.Tables[0].Rows[j]["InvActPayAmt"]) +
                                        " , InvPaidAmtL = " + Convert.ToDecimal(invJVDS.Tables[0].Rows[j]["InvActPayAmtL"]) +
                                        " where invJVDetailID = " + Convert.ToInt32(invJVDS.Tables[0].Rows[j]["invJVDetailID"]));
                }
                trans.Commit();
            }
            catch(Exception ex)
            {
                trans.Rollback();
                trans.Commit();
                throw ex;
            }

        }

        /// <summary>
        /// 获取重打结算单信息
        /// </summary>
        /// <param name="invID">结算单号</param>
        /// <returns></returns>
        public static DataSet UnionAgainPrint(int invID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.InvID,A.InvCode,C.ContractCode,A.InvDate,B.invSalesAmt from InvoiceHeader A , InvoiceJVDetail B,Contract C "+
                             " where A.InvID = B.InvID and A.ContractID = C.ContractID and A.InvID = " + invID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取重打结算单信息
        /// </summary>
        /// <param name="contractCode">合同号</param>
        /// <returns></returns>
        public static DataSet UnionAgainPrintInfo(string contractCode)
        {
            string str_sql = "select A.CustCode,A.CustName,A.TaxCode,A.BankName,A.BankAcct,B.InvID,B.InvCode,B.InvDate,C.ContractCode" +
                             " from Customer A,InvoiceHeader B,Contract C " +
                             " where A.CustID = B.CustID and B.ContractID = C.ContractID and C.BizMode = " + Contract.BIZ_MODE_UNIT + " and C.ContractCode = '" + contractCode +"' and B.InvStatus != '" + InvoiceHeader.INVSTATUS_CANCEL + "' order by InvCode Desc";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
