using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.DB;
using Base.Biz;
using Base.Page;
using Base.Sys;
using Base.Util;
using BaseInfo.Dept;
using BaseInfo.User;
using BaseInfo.authUser;
using RentableArea;
using Lease.ConShop;

namespace Shop.ShopTreeOperate
{
    public class ShopTreeOperate
    {
        private bool NoOutDate = false;//商铺是否包含过期 false：包含过期，true：不包含过期
        private int UserID=0;//用户ID
        private string strSql="";//传入的商铺sql
        private bool bShowLocation = false;
        /// <summary>
        /// 生成商铺树
        /// </summary>
        /// <param name="bNoOutDate">商铺是否包含过期 false：包含过期，true：不包含过期</param>
        /// <param name="iUserID">用户ID</param>
        /// <param name="ShowLocaltion">是否显示方位</param>
        /// <param name="strShopSql"></param>
        public ShopTreeOperate(bool bNoOutDate,int iUserID,bool ShowLocaltion,string strShopSql)
        {
            this.NoOutDate = bNoOutDate;
            this.UserID = iUserID;
            this.strSql = strShopSql;
            this.bShowLocation = ShowLocaltion;
        }

        /// <summary>
        /// 生成商铺树
        /// </summary>
        /// <param name="bNoOutDate">商铺是否包含过期 false：包含过期，true：不包含过期</param>
        /// <param name="iUserID">用户ID</param>
        /// <param name="strShopSql">得到商铺的Sql</param>
        public ShopTreeOperate(bool bNoOutDate, int iUserID, string strShopSql)
        {
            this.NoOutDate = bNoOutDate;
            this.UserID = iUserID;
            this.strSql = strShopSql;
        }
        /// <summary>
        /// 获得楼层中的商铺信息(不显示方位)
        /// </summary>
        /// <param name="strStoreID"></param>
        /// <param name="strBuildingID"></param>
        /// <param name="strFloorID"></param>
        /// <returns></returns>
        private DataSet GetShopData(int strStoreID,int strBuildingID,int strFloorID)
        {
            BaseBO objBaseBo = new BaseBO();
            string strSql = "select storeid,buildingid,floorid,shopid,shopcode,shopname from conshop where 1=1 ";
            if (this.strSql != "")
                strSql = this.strSql;
            if (this.NoOutDate == true)
            {
                strSql += " and  conshop.ShopStartDate<='" + DateTime.Now.ToString() + "' and conshop.ShopEndDate >= '" + DateTime.Now.ToString() + "'";
            }
            strSql += "  and conshop.StoreId=" + strStoreID + " and conshop.FloorID=" + strFloorID + " and conshop.BuildingID=" + strBuildingID + "";
            return objBaseBo.QueryDataSet(strSql);
        }
        /// <summary>
        /// 获得楼层中的商铺信息(显示方位)
        /// </summary>
        /// <param name="strStoreID"></param>
        /// <param name="strBuildingID"></param>
        /// <param name="strFloorID"></param>
        /// <param name="strLocationID"></param>
        /// <returns></returns>
        private DataSet GetShopData(int strStoreID, int strBuildingID, int strFloorID, int strLocationID)
        {
            BaseBO objBaseBo = new BaseBO();
            string strSql = "select storeid,buildingid,floorid,shopid,shopcode,shopname from conshop where 1=1 ";
            if (this.strSql != "")
                strSql = this.strSql;
            if (this.NoOutDate == true)
            {
                strSql += " and  conshop.ShopStartDate<='" + DateTime.Now.ToString() + "' and conshop.ShopEndDate >= '" + DateTime.Now.ToString() + "'";
            }
            strSql += "  and conshop.StoreId=" + strStoreID + " and conshop.FloorID=" + strFloorID + " and conshop.BuildingID=" + strBuildingID + " and conshop.locationid=" + strLocationID + "";
            return objBaseBo.QueryDataSet(strSql);
        }
        /// <summary>
        /// 得到商铺树字符串
        /// </summary>
        /// <returns></returns>
        public string ShowShopTree()
        {
            BaseBO objBase = new BaseBO();
            Resultset rs = new Resultset();
            Dept objDept = new Dept();
            objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
            string jsdept = "";
            rs = objBase.Query(objDept);
            if (rs.Count == 1)
            {
                objDept = rs.Dequeue() as Dept;
                jsdept = objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
            }
            else
            {
                return "";
            }
            objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
            objBase.OrderBy = " orderid ";
            if (AuthBase.GetAuthUser(this.UserID) > 0)
            {
                objBase.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + this.UserID + ")";
            }
            rs = objBase.Query(objDept);
            objBase.OrderBy = "";
            if (rs.Count > 0)
            {
                foreach (Dept store in rs)
                {
                    jsdept += store.DeptID + "|" + objDept.DeptID + "|" + store.DeptName + "^";
                    objBase.WhereClause = "StoreId=" + store.DeptID;
                    rs = objBase.Query(new Building());
                    if (rs.Count > 0)
                    {
                        foreach (Building building in rs)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                            objBase.WhereClause = "floors.BuildingID=" + building.BuildingID;

                            if (AuthBase.GetAuthUser(this.UserID) > 0)
                            {
                                objBase.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + this.UserID +
                                                     ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + this.UserID +
                                                     ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + this.UserID +
                                                     ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + this.UserID +
                                                     ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + this.UserID + ")";
                            }
                            rs = objBase.Query(new floorsAuth());
                            foreach (floorsAuth floors in rs)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID + "|" + floors.FloorName + "^";

                                if (this.bShowLocation)//显示方位
                                {
                                    objBase.WhereClause = "FloorID=" + floors.FloorID;
                                    rs = objBase.Query(new Location());
                                    foreach (Location location in rs)
                                    {
                                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";
                                        DataSet ds = this.GetShopData(store.DeptID,building.BuildingID,floors.FloorID,location.LocationID)
;
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() +location.LocationID.ToString() + ds.Tables[0].Rows[i]["ShopID"].ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString()+location.LocationID.ToString() + "|" + ds.Tables[0].Rows[i]["shopcode"].ToString() + " " + ds.Tables[0].Rows[i]["ShopName"].ToString() + "^";
                                        }
                                    }
                                }
                                else
                                {
                                    objBase.WhereClause = "";
                                    DataSet ds = this.GetShopData(store.DeptID, building.BuildingID, floors.FloorID);
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + ds.Tables[0].Rows[i]["ShopID"].ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + ds.Tables[0].Rows[i]["shopcode"].ToString() + " " + ds.Tables[0].Rows[i]["ShopName"].ToString() + "^";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return jsdept;
        }
    }
}
