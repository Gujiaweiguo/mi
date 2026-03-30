using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;

namespace Lease.PotBargain
{
    /// <summary>
    /// 费用类型PO
    /// </summary>
    public class ChargeTypePO
    {
        /// <summary>
        /// 根据费用类型ID获取费用类别名称
        /// </summary>
        /// <param name="chgTypeID">费用类型ID</param>
        /// <returns></returns>
        public static string GetChargeTypeNameByID(int chgTypeID)
        {
            string str_sql = "select ChargeTypeName from ChargeType where ChargeTypeID = " + chgTypeID;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        /// <summary>
        /// 获取租赁费用类型
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLeaseChargeType()
        {
            string str_sql = "SELECT ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber" + 
                             " FROM ChargeType" + 
                             " WHERE ChargeClass NOT IN (" + ChargeType.CHARGECLASS_UNION_INNERCARDRATE + "," +
                             ChargeType.CHARGECLASS_UNION_OUTERCARDRATE + "," + ChargeType.CHARGECLASS_UNION + ")";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取联营费用类型
        /// </summary>
        /// <returns></returns>
        public static DataSet GetJVLeaseChargeType()
        {
            string str_sql = "SELECT ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber" +
                             " FROM ChargeType" +
                             " WHERE ChargeClass NOT IN (" + ChargeType.CHARGECLASS_LEASE + ")";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
