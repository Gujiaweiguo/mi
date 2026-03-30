using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;

namespace Lease.ConShop
{
    /// <summary>
    /// 硬件类型PO
    /// </summary>
    public class HdwTypePO
    {
        /// <summary>
        /// 添加硬件类型信息
        /// </summary>
        /// <param name="basePO">硬件类型信息</param>
        /// <returns></returns>
        public static int InsertHdwType(BasePO basePO)
        {
            BaseBO baseBO = new BaseBO();
            int result = baseBO.Insert(basePO);
            return result;
        }

        /// <summary>
        /// 根据硬件类型ID修改硬件类型信息
        /// </summary>
        /// <param name="hdwType">硬件类型信息</param>
        /// <returns></returns>
        public static int UpdateHdwTypeByID(HdwType hdwType)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "HdwTypeID = " + hdwType.HdwTypeID;
            int result = baseBO.Update(hdwType);
            return result;
        }

        /// <summary>
        /// 根据硬件类型ID获取硬件类型信息
        /// </summary>
        /// <param name="hdwTypeID">硬件类型ID</param>
        /// <returns></returns>
        public static Resultset GetHdwTypeByID(int hdwTypeID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "HdwTypeID = " + hdwTypeID;
            Resultset rs = baseBO.Query(new HdwType());
            return rs;
        }
    }
}
