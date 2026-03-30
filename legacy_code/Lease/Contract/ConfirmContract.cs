using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow;
using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.Biz;

namespace Lease.Contract
{
    /// <summary>
    /// 合同审批同意接口类
    /// </summary>
    public class ConfirmContract:IConfirmVoucher
    {
        /**
        * 确认单据后对数据生效的处理
        */
        public void ValidVoucher(int voucherID, int operType, BaseTrans trans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                String sql = "";

                sql = "Update Contract Set ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR + " where ContractID=" + voucherID;
                trans.ExecuteUpdate(sql);

                sql = "Update ConShop Set ShopStatus = " + ConShop.ConShop.CONSHOP_TYPE_INGEAR + " where ContractID=" + voucherID;
                trans.ExecuteUpdate(sql);

                //分摊签约面积到conshopunit.rentarea
                sql = "Exec SPMI_ComputerConShopRentArea " + voucherID.ToString();
                trans.ExecuteUpdate(sql);
                
            }
        }

        /**
        * 确认单据后对数据打印的处理
        */
        public void PrintVoucher(int voucherID,int operType, BaseTrans trans)
        {
            //BillInfo objBillInfo = new BillInfo();
            //String sql = "Update Bill set PrintAfterConfirm = " + WrkFlwNode.PRINT_AFTER_CONFIRM_YES + " where BillNum=" + voucherID;
            //trans.ExecuteUpdate(sql);
        }
    }
}
