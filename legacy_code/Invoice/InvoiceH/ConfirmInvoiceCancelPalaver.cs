using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow;
using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.Biz;

namespace Invoice.InvoiceH
{
    class ConfirmInvoiceCancelPalaver : IConfirmVoucher
    {
        /**
   * 确认单据后对数据生效的处理
   */
        public void ValidVoucher(int voucherID, int operType, BaseTrans trans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                //修改取消结算单状态
                string str_sql = "update InvCancel set InvCelStatus = " + InvCancel.INVCANCEL_UPDATE_LEASE_STATUS;
                //修改结算主表算单状态和是否首期
                string str_sql_invoiceH = "update InvoiceHeader set IsFirst = " + Invoice.InvoiceHeader.ISFIRST_NO + 
                                            " , InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_CEL + 
                                            " where InvID = " + voucherID;
                //将对应的结算明细导入结算取消表中               
                string str_sql_invoiceC = "insert into InvoiceCancel select InvDetailID,ChargeTypeID,InvID,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL," +
                                            "InvDiscAmt,InvDiscAmtL,InvChgAmt,InvChgAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvType,InvDetStatus,Note,RentType " +
                                            " from InvoiceDetail where InvID = " + voucherID;

                //将对应的费用表中的结算单号至为初始状态
                string str_sql_Charge = "update Charge set InvCode = '" + 0 + "' where InvCode = " + voucherID;

                //将对应的其它费用表中的结算单号至为初始状态
                string str_sql_OtherChargeH = "update OtherChargeH set InvCode = '" + 0 + "' where InvCode = " + voucherID; 

                string str_sql_invoiceD = "delete from InvoiceDetail where InvID = " + voucherID;

                //将滞纳金表中数据导入到invoiceIntCancel中
                string str_sql_invoiceIntCancel = "Insert Into invoiceIntCancel Select InterestID,ChargeTypeID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,LateInvID,LateInvDetailID,InvCode,IntStartDate,IntEndDate," +
                                                    "InterestDay,ExtendDay,LatePayAmt,InterestRate,InterestRate,Note,ContractID From invoiceInterest Where LateInvID = " + voucherID;
                //删除滞纳金表中数据
                string str_sql_DeleteinvoiceInterest = "Delete invoiceInterest Where LateInvID = " + voucherID;

                //将联营结算明细表中数据导入到InvoiceJVCancel中
                string str_sql_InvoiceJVCancel = "Insert Into InvoiceJVCancel Select invJVDetailID,InvID,RentType,Period,InvStartDate,InvEndDate,InvCurTypeID,ChargeTypeID,InvExRate,invSalesAmt,invSalesAmtL,invPcent,InTaxRate,OutTaxRate," +
                                                "invJVCostAmt,invJVCostAmtL,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvDetStatus,Note From InvoiceJVDetail Where InvID = " + voucherID;

                //删除联营结算明细表中数据
                string str_sql_DeleteInvoiceJVDetail = "Delete InvoiceJVDetail Where InvID = " + voucherID;

                trans.ExecuteUpdate(str_sql);
                trans.ExecuteUpdate(str_sql_invoiceH);
                trans.ExecuteUpdate(str_sql_invoiceC);
                trans.ExecuteUpdate(str_sql_invoiceD);
                trans.ExecuteUpdate(str_sql_Charge);
                trans.ExecuteUpdate(str_sql_OtherChargeH);
                trans.ExecuteUpdate(str_sql_invoiceIntCancel);
                trans.ExecuteUpdate(str_sql_DeleteinvoiceInterest);
                trans.ExecuteUpdate(str_sql_InvoiceJVCancel);
                trans.ExecuteUpdate(str_sql_DeleteInvoiceJVDetail);




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
