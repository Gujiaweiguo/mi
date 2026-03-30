using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.Biz;
namespace Bill
{
    public class ConfirmVoucher:IConfirmVoucher
    {
        /**
         * 确认单据后对数据生效的处理
         */
        public void ValidVoucher(int voucherID,  int operType, BaseTrans trans)
        {
            BillInfo objBillInfo = new BillInfo();
            String sql = "Update Bill set ValidAfterConfirm = " + WrkFlwNode.VALID_AFTER_CONFIRM_YES + " where BillNum=" + voucherID;
            trans.ExecuteUpdate(sql);
        }
        /**
         * 确认单据后对数据打印的处理
         */
        public void PrintVoucher(int voucherID,  int operType, BaseTrans trans)
        {
            BillInfo objBillInfo = new BillInfo();
            String sql = "Update Bill set PrintAfterConfirm = " + WrkFlwNode.PRINT_AFTER_CONFIRM_YES + " where BillNum=" + voucherID;
            trans.ExecuteUpdate(sql); 
        }
    }
}
