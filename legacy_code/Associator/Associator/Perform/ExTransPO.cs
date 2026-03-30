using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.DB;
using Base.Biz;
using Associator.Perform;

namespace Associator.Perform
{
    /// <summary>
    /// 小票兑换记录PO
    /// </summary>
    public class ExTransPO
    {
        /// <summary>
        /// 检查小票是否可以兑换
        /// </summary>
        /// <param name="membID">会员号</param>
        /// <param name="giftID">赠品ID</param>
        /// <param name="limitOne">小票是否只能兑换一次</param>
        /// <param name="recepitID">小票号</param>
        /// <returns></returns>
        public static int GetExTransPOByID(int membID, int giftID, int limitOne,string recepitID)
        {
            int flag = 0;
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT Count(ExID) AS sCount FROM ExTrans WHERE MembID = " + membID + " AND GiftID = " + giftID + " AND ReceiptNum = " + recepitID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (limitOne == Gift.LIMITONE_NO)
            {
                flag = 0;
            }
            else if (limitOne == Gift.LIMITONE_YES)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["sCount"]) >= 1)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
            return flag;
        }
    }
}
