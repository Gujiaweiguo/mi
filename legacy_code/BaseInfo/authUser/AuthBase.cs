using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;

namespace BaseInfo.authUser
{
    public class AuthBase
    {
        /// <summary>
        /// 商业项目权限
        /// </summary>
        public static string AUTH_SQL_STORE = "SELECT storeID FROM authUser WHERE  ConShop.ShopID = authUser.ShopID AND userID = ";

        /// <summary>
        /// 大楼权限
        /// </summary>
        public static string AUTH_SQL_BUILD = "SELECT buildingID FROM authUser WHERE  ConShop.ShopID = authUser.ShopID AND userID = ";

        /// <summary>
        /// 楼层权限
        /// </summary>
        public static string AUTH_SQL_FLOOR = "SELECT FloorID FROM authUser WHERE  ConShop.ShopID = authUser.ShopID AND userID = ";

        /// <summary>
        /// 合同权限
        /// </summary>
        public static string AUTH_SQL_CONTRACT = "SELECT ContractID FROM authUser WHERE  ConShop.ShopID = authUser.ShopID AND userID = ";

        /// <summary>
        /// 商铺权限
        /// </summary>
        public static string AUTH_SQL_SHOP = "SELECT ShopID FROM authUser WHERE  ConShop.ShopID = authUser.ShopID AND userID = ";


        /// <summary>
        /// 客户权限
        /// </summary>
        public static string AUTH_SQL_CUST = "SELECT CustID FROM authUser WHERE userID = ";


        /// <summary>
        /// 合同权限不加商铺
        /// </summary>
        public static string AUTH_SQL_CONTRACID = "SELECT ContractID FROM authUser WHERE userID = ";

        /// <summary>
        /// 单元权限
        /// </summary>
        public static string AUTH_SQL_UnitID = "Select UnitID From AuthUser Where UserID=  ";



        /// <summary>
        /// 根据用户ID判断用户权限
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static int GetAuthUser(int userID)
        {
            int count = 0;
            string str_sql = "SELECT userID FROM AuthUser WHERE userID = " + userID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            count = ds.Tables[0].Rows.Count;
            return count;
        }
    }
}
