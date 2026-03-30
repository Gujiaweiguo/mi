using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow.Func;
using Base.Biz;
using Base.DB;
using Base;
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using Lease.ChangeLease;
using Lease.ConOvertimeBill;

namespace Lease.AdContract
{
    public class ConfirmArea : IConfirmVoucher
    {
        /**
        * 确认单据后对数据生效的处理
        */
        public void ValidVoucher(int voucherID, int operType, BaseTrans baseTrans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {

                baseTrans.ExecuteUpdate("Update Contract Set ContractStatus = " + Lease.Contract.Contract.CONTRACTSTATUS_TYPE_INGEAR + " where ContractID=" + voucherID);

                baseTrans.ExecuteUpdate("Update ConArea Set ConAreaStatus = " + ConArea.BLANKOUT_STATUS_LEASEOUT + " Where ContractID =" + voucherID);
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
