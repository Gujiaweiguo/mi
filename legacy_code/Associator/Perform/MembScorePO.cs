using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Biz;
using Base.DB;
using Lease.ConShop;

namespace Associator.Perform
{
    /// <summary>
    /// 会员积分
    /// </summary>
    public class MembScorePO
    {
        /// <summary>
        /// 获取商铺信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetConShop()
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID,FloorID,LocationID,CreateUserID,CreateTime,ModifyUserID,"+
                            "ModifyTime,OprRoleID,OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel from ConShop";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
    }
}
