using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;

namespace Lease
{
    /// <summary>
    /// 审批意见
    /// </summary>
    public class CheckApproveMessage
    {
        /// <summary>
        /// 获取审批意见
        /// </summary>
        /// <param name="wrkFlwID">工作流ID</param>
        /// <param name="voucherID"></param>
        /// <returns></returns>
        public static DataSet GetCheckApproveMessage(string wrkFlwID, string voucherID)
        {
            string str_sql = "select users.userid,users.username,wrkflwentity.prevoucherMemo," +
                            " wrkflwentity.completedtime,wrkflwentity.NodeStatus,'' as NodeStatusName" +
                            " from wrkflwentity,users" +
                            " where wrkflwid = " + wrkFlwID +
                            " and voucherid = " + voucherID +
                            " and users.userid = wrkflwentity.userid" +
                            " order by completedtime";

            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
