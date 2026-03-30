using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Base.Biz;
using Base.DB;
using WorkFlow.Func;
using WorkFlow;
using Lease.ConTerminateBill;
namespace Lease.AdContract
{
    public class ConfirmAdBoardStop : IConfirmVoucher
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
                DataTable dataTable = new DataTable();

                UpdateContractStopDate updateContractStopDate = new UpdateContractStopDate();

                baseBO.WhereClause = "ConTerID=" + voucherID;
                rs = baseBO.Query(updateContractStopDate);

                if (rs.Count == 1)
                {
                    updateContractStopDate = rs.Dequeue() as UpdateContractStopDate;

                    if (updateContractStopDate.ConEndDate >= updateContractStopDate.TerDate)
                    {
                        baseTrans.WhereClause = "ContractID = " + updateContractStopDate.ContractID;

                        updateContractStopDate.ContractStatus = Contract.Contract.CONTRACTSTATUS_TYPE_END;
                        updateContractStopDate.StopDate = updateContractStopDate.TerDate;

                        if (baseTrans.Update(updateContractStopDate) == -1)
                        {
                            baseTrans.Rollback();
                            return;
                        }

                        if (baseTrans.ExecuteUpdate("Update ConAdBoard Set ConAdBoardStatus = " + ConAdBoard.BLANKOUT_STATUS_INVALID + " Where ContractID =" + updateContractStopDate.ContractID) == -1)
                        {
                            baseTrans.Rollback();
                            return;
                        }
                    }
                }
            }
        }

        /**
        * 确认单据后对数据打印的处理
        */
        public void PrintVoucher(int voucherID, int operType, BaseTrans trans)
        {

        }
    }
}
