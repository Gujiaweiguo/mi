using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using WorkFlow;
using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.DB;
using WorkFlow.Uiltil;
using System.Data;

namespace Invoice.InvoiceH
{        
    /// <summary>
        /// 结算单变更同意接口类
        /// </summary>
    public class ConfirmInvAdj : IConfirmVoucher
    {

        /**
        * 确认单据后对数据生效的处理
        */
        public void ValidVoucher(int voucherID, int operType, BaseTrans baseTrans)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            Resultset rsInvDetail = new Resultset();
            InvAdjDetIns invAdjDetIns = new InvAdjDetIns();
            InvAdjDetail invAdjDetail = new InvAdjDetail();
            decimal invAdjAmt = 0;
            decimal invAdjAmtL = 0;
            decimal invActPayAmt = 0;
            decimal invActPayAmtL = 0;

            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                baseBO.WhereClause = "a.InvAdjID=" + voucherID;
                rs = baseBO.Query(invAdjDetIns);
                if (rs.Count > 0)
                {
                    foreach (InvAdjDetIns invAdjDetInss in rs)
                    {
                        baseBO.WhereClause = "InvDetailID = " + invAdjDetInss.InvDetailID;
                        rsInvDetail = baseBO.Query(invAdjDetail);
                        if (rsInvDetail.Count == 1)
                        {
                            invAdjDetail = rsInvDetail.Dequeue() as InvAdjDetail;
                            invAdjAmt = invAdjDetail.InvAdjAmt;
                            invAdjAmtL = invAdjDetail.InvAdjAmtL;
                            invActPayAmt = invAdjDetail.InvActPayAmt;
                            invActPayAmtL = invAdjDetail.InvActPayAmtL;
                        }

                        invAdjDetail.InvAdjAmt = invAdjAmt + invAdjDetInss.AdjAmt;
                        invAdjDetail.InvAdjAmtL = invAdjAmtL + invAdjDetInss.AdjAmtL;

                        invAdjDetail.InvActPayAmt = invActPayAmt + invAdjDetInss.AdjAmt;
                        invAdjDetail.InvActPayAmtL = invActPayAmtL + invAdjDetInss.AdjAmtL;

                        baseTrans.WhereClause = "InvDetailID = " + invAdjDetInss.InvDetailID;
                        if (baseTrans.Update(invAdjDetail) < 1)
                        {
                            baseTrans.Rollback();
                            return;
                        }
                    }

                    DataTable dt = baseBO.QueryDataSet("Select AdjAmt,AdjAmtL,InvID From InvAdj Where InvAdjID=" + voucherID).Tables[0];

                    if (dt.Rows.Count == 1)
                    {
                        DataTable dtHeader = baseBO.QueryDataSet("Select InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL From InvoiceHeader Where InvID =" + dt.Rows[0]["InvID"]).Tables[0];

                        if (dtHeader.Rows.Count == 1)
                        {
                            decimal invAdjAmtH = Convert.ToDecimal(dt.Rows[0]["AdjAmt"]) + Convert.ToDecimal(dtHeader.Rows[0]["InvAdjAmt"]);
                            decimal invAdjAmtLH = Convert.ToDecimal(dt.Rows[0]["AdjAmtL"]) + Convert.ToDecimal(dtHeader.Rows[0]["InvAdjAmtL"]);

                            decimal invActPayAmtH = Convert.ToDecimal(dt.Rows[0]["AdjAmt"]) + Convert.ToDecimal(dtHeader.Rows[0]["InvActPayAmt"]);
                            decimal invActPayAmtLH = Convert.ToDecimal(dt.Rows[0]["AdjAmtL"]) + Convert.ToDecimal(dtHeader.Rows[0]["InvActPayAmtL"]);
                            if (baseTrans.ExecuteUpdate("Update InvoiceHeader Set InvAdjAmt=" + invAdjAmtH + ",InvAdjAmtL=" + invAdjAmtLH + ",InvActPayAmt = " + invActPayAmtH + ",InvActPayAmtL = " + invActPayAmtLH + " Where InvID =" + dt.Rows[0]["InvID"]) == -1)
                            {
                                baseTrans.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            baseTrans.Rollback();
                            return;
                        }
                    }
                    else
                    {
                        baseTrans.Rollback();
                        return;
                    }
                }

                String sql = "Update InvAdj Set AdjStatus = " + InvAdj.INVADJ_UPDATE_LEASE_STATUS + " Where InvAdjID=" + voucherID;
                if (baseTrans.ExecuteUpdate(sql) < 1)
                {
                    baseTrans.Rollback();
                    return;
                }
            }
        }

        /**
        * 确认单据后对数据打印的处理
        */
        public void PrintVoucher(int voucherID, int operType, BaseTrans trans)
        {
            //BillInfo objBillInfo = new BillInfo();
            //String sql = "Update Bill set PrintAfterConfirm = " + WrkFlwNode.PRINT_AFTER_CONFIRM_YES + " where BillNum=" + voucherID;
            //trans.ExecuteUpdate(sql);
        }
    }
}
