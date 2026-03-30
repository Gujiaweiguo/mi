using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.DB;
using Base.Biz;


namespace Invoice
{
    /// <summary>
    /// 滞纳金利率PO
    /// </summary>
    public class InterestRatePO
    {
        /// <summary>
        /// 添加滞纳金利率
        /// </summary>
        /// <param name="basePO">滞纳金参数PO</param>
        /// <returns></returns>
        public static int InsertInterestRate(BasePO basePO)
        {
            BaseBO baseBO = new BaseBO();
            int result = baseBO.Insert(basePO);
            return result;
        }

        /// <summary>
        /// 修改滞纳金利率
        /// </summary>
        /// <param name="interestRate">滞纳金信息</param>
        /// <returns></returns>
        public static int UpdateInterestRate(InterestRate interestRate)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "InterestRateID = " + interestRate.InterestRateID;
            int result = baseBO.Update(interestRate);
            return result;
        }

        /// <summary>
        /// 删除滞纳金利率
        /// </summary>
        /// <param name="interestRateID">滞纳金利率ID</param>
        /// <returns></returns>
        public static int DelInterestRate(int interestRateID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "InterestRateID = " + interestRateID;
            int result = baseBO.Delete(new InterestRate());
            return result;
        }

        /// <summary>
        /// 根据ID获取纳金利率信息
        /// </summary>
        /// <param name="interestRateID">纳金利率ID</param>
        /// <returns></returns>
        public static Resultset GetInterestRateByID(int interestRateID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "InterestRateID = " + interestRateID;
            Resultset rs = baseBO.Query(new InterestRate());
            return rs;
        }

        /// <summary>
        /// 获取滞纳金利率信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetInterestRate()
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.ContractCode,B.ChargeTypeName,C.IntRate * 1000 as IntRate,C.InterestRateID " + 
                             " from InterestRate C,Contract A,ChargeType B " +
                             " where A.ContractID = C.ContractID " +
                             " and B.ChargeTypeID = C.ChargeTypeID ORDER BY A.ContractCode";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取批量结算单号
        /// </summary>
        /// <returns></returns>
        public static DataSet GetInvH()
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select InvID,ContractID from invoiceheader";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
       
    }
}
