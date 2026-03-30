using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using WorkFlow;
using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.DB;
using WorkFlow.Uiltil;

namespace Invoice.InvoiceH
{
    public class ConfirmInvAdjCancel : IConfirmVoucher
    {
        /**
        * 确认单据后对数据生效的处理
        */
        public void ValidVoucher(int voucherID, int operType, BaseTrans baseTrans)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            InvoiceHeaderCancel invoiceHeaderCancel = new InvoiceHeaderCancel();
            InvCancel invCancel = new InvCancel();

            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                baseBO.WhereClause = "InvCelID=" + voucherID;
                rs = baseBO.Query(invCancel);
                if (rs.Count == 1)
                {
                    invCancel = rs.Dequeue() as InvCancel;
                    invoiceHeaderCancel.InvStatus = InvoiceHeader.INVOICEHEADER_CANCEL;
                    baseTrans.WhereClause = "InvID= " + invCancel.InvID;
                    if (baseTrans.Update(invoiceHeaderCancel) < 1)
                    {
                        baseTrans.Rollback();
                        return;
                    }
                }

                baseTrans.WhereClause = "InvCelID=" + voucherID;
                invCancel.InvCelStatus = InvCancel.INVCANCEL_UPDATE_LEASE_STATUS;
                if (baseTrans.Update(invCancel) < 1)
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
