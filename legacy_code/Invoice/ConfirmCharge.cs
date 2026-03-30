using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow;
using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.Biz;

namespace Invoice
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfirmCharge : IConfirmVoucher
    {
        /**
     * 确认单据后对数据生效的处理
     */
        public void ValidVoucher(int voucherID, int operType, BaseTrans trans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                String sql = "Update Charge set ChgStatus = " + Charge.CHGSTATUS_TYPE_INGEAR + " where RangeCode =" + voucherID;
                trans.ExecuteUpdate(sql);
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
