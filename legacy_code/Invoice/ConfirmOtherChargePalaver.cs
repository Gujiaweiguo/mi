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
    /// 其它费用审批接口类
    /// </summary>
    public class ConfirmOtherChargePalaver : IConfirmVoucher
    {
        /**
      * 确认单据后对数据生效的处理
      */
        public void ValidVoucher(int voucherID, int operType, BaseTrans trans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                //String sql = "Update OtherChargeH set ChgStatus = " + OtherChargeH.CHGSTATUS_TYPE_ATTREM + " where OtherChargeHID =" + voucherID;
                String sql = "Update OtherChargeH set ChgStatus = " + OtherChargeH.CHGSTATUS_TYPE_ATTREM + " where RangeCode =" + voucherID;//对OtherChargeH表中的数据状态进行批量修改（原因是录入的时候是批量录入的）
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
