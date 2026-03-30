using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 费用计算日志PO
    /// </summary>
    public class ChargeCountLogPO
    {
        /// <summary>
        /// 插入费用计算日志PO
        /// </summary>
        /// <param name="basePo">费用计算日志PO</param>
        /// <returns></returns>
        public static int InsertChargeCountLog(BasePO basePo)
        {
            BaseBO baseBo = new BaseBO();
            int result = baseBo.Insert(basePo);
            return result;
        }

        /// <summary>
        /// 获取费用类型所生成的金额
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="period">费用记帐月</param>
        /// <param name="chgTypeID">费用ID</param>
        /// <returns></returns>
        public static DataSet GetPayAmt(int contractID, DateTime period, int chgTypeID)
        {
            string str_sql = "select A.InvPayAmt from InvoiceDetail A,InvoiceHeader B " +
                             " where A.InvID = B.InvID " +
                             " and B.ContractID = " + contractID +
                             " and A.Period = '" + period + "'" +
                             " and A.ChargeTypeID = " + chgTypeID;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds;
        }
    }
}
