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
    public class ComfirmInvDisc : IConfirmVoucher
    {
        /**
        * 确认单据后对数据生效的处理
        */
        public void ValidVoucher(int voucherID, int operType, BaseTrans baseTrans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                BaseBO baseBO = new BaseBO();
                Resultset rs = new Resultset();
                Resultset rsInvDetail = new Resultset();
                InvDiscAuditing invDiscAuditing = new InvDiscAuditing();
                InvDiscDetail invDiscDetail = new InvDiscDetail();
                decimal invDiscDetailHead = 0;
                decimal InvDiscAmtLlHead = 0;

                decimal TotalInvActPayAmt = 0;
                decimal TotalInvActPayAmtL = 0;

                int invID = 0;

                baseBO.WhereClause = "a.DiscID=" + voucherID;
                rs = baseBO.Query(invDiscAuditing);
                if (rs.Count > 0)
                {
                    foreach (InvDiscAuditing invDiscAuditings in rs)
                    {
                        invID = invDiscAuditings.InvID;
                        invDiscDetailHead = invDiscDetailHead + -(invDiscAuditings.DiscRate * (invDiscAuditings.InvPayAmt + invDiscAuditings.InvAdjAmt + invDiscAuditings.InvDiscAmt));
                        InvDiscAmtLlHead = InvDiscAmtLlHead + (invDiscDetailHead * invDiscAuditings.InvExRate);
                        
                        invDiscDetail.InvDiscAmt = - (invDiscAuditings.DiscRate * (invDiscAuditings.InvPayAmt + invDiscAuditings.InvAdjAmt + invDiscAuditings.InvDiscAmt)) + invDiscAuditings.InvDiscAmt;
                        invDiscDetail.InvDiscAmtL = invDiscDetail.InvDiscAmt * invDiscAuditings.InvExRate;

                        invDiscDetail.InvActPayAmt = invDiscAuditings.InvActPayAmt + invDiscDetail.InvDiscAmt;
                        invDiscDetail.InvActPayAmtL = invDiscAuditings.InvActPayAmtL + invDiscDetail.InvDiscAmtL;

                        TotalInvActPayAmt += invDiscDetail.InvActPayAmt;
                        TotalInvActPayAmtL += invDiscDetail.InvActPayAmtL;

                        baseTrans.WhereClause = "InvDetailID=" + invDiscAuditings.InvDetailID;
                        if (baseTrans.Update(invDiscDetail) < 1)
                        {
                            baseTrans.Rollback();
                            return;
                        }
                    }

                    DataTable dt = baseBO.QueryDataSet("Select InvDiscAmt,InvDiscAmtL,InvActPayAmt,InvActPayAmtL From InvoiceHeader Where InvID = " + invID).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        if (baseTrans.ExecuteUpdate("Update InvoiceHeader Set InvDiscAmt = " + (Convert.ToDecimal(dt.Rows[0]["InvDiscAmt"]) + invDiscDetailHead) + ",InvDiscAmtL = " + (Convert.ToDecimal(dt.Rows[0]["InvDiscAmtL"]) + InvDiscAmtLlHead) + ",InvActPayAmt = " + TotalInvActPayAmt + ",InvActPayAmtL = " + TotalInvActPayAmtL + " Where InvID=" + invID) == -1)
                        {
                            
                            baseTrans.Rollback();
                            return;
                        }
                    }

                    String sql = "Update InvDisc Set DiscStatus = " + InvDisc.INVDISC_UPDATE_LEASE_STATUS + " Where DiscID=" + voucherID;
                    if (baseTrans.ExecuteUpdate(sql) < 1)
                    {
                        baseTrans.Rollback();
                        return;
                    }
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
