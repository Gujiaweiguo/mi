using System;

using RentableArea;
using Base.Biz;
using Base.DB;
using Lease.ConShop;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class Lease_Shop_SelectShop : BasePage
{
    public string selectTradeLevel;
    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode();
        selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
    }

    private void showtreenode()
    {
        string jsdept = "";
        Resultset rs = new Resultset();
        Resultset rsf = new Resultset();
        Resultset rsl = new Resultset();
        Resultset rsu = new Resultset();
        Dept dept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        //
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "DeptType=1";//集团
        Resultset rsD = objBaseBo.Query(new Dept());
        Dept objD = rsD.Dequeue() as Dept;
        jsdept = objD.DeptID + "|" + objD.PDeptID + "|" + objD.DeptName + "^";
        //

        objBaseBo.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and exists (SELECT storeID FROM authUser WHERE  dept.deptid = authUser.storeid AND userID ='" + sessionUser.UserID + "')";
        }

        rs = objBaseBo.Query(dept);
        if (rs.Count > 0)
        {
            foreach (Dept objDept in rs)
            {
                jsdept += objDept.DeptID + "|" + objD.DeptID + "|" + objDept.DeptName + "^";

                objBaseBo.WhereClause = "StoreId=" + objDept.DeptID;
                rs = objBaseBo.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += objDept.DeptID.ToString() + building.BuildingID.ToString() + "|" + objDept.DeptID + "|" + building.BuildingName + "^";

                        objBaseBo.WhereClause = "StoreId=" + objDept.DeptID + " and buildingId=" + building.BuildingID;
                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            objBaseBo.WhereClause += " and exists (SELECT FloorID FROM authUser WHERE  floors.floorid = authUser.floorid AND userID ='" + sessionUser.UserID + "')";
                        }
                        rsf = objBaseBo.Query(new Floors());
                        if (rsf.Count > 0)
                        {
                            foreach (Floors floors in rsf)
                            {
                                jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + "1|" + objDept.DeptID.ToString() + building.BuildingID.ToString() + "|" + floors.FloorName + "^";
                                objBaseBo.WhereClause = "StoreId=" + objDept.DeptID + "and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and ( ShopStatus = " + ConShop.CONSHOP_TYPE_PAUSE + " or ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + ") and a.ShopTypeID = b.ShopTypeID order by Shopcode";
                                rsl = objBaseBo.Query(new ConShop());
                                foreach (ConShop conShop in rsl)
                                {
                                    jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + conShop.ShopId.ToString() + "|" + building.BuildingID.ToString() + floors.FloorID.ToString() + "1|" + conShop.ShopCode + "(" + conShop.ShopName + ")^";
                                }
                            }
                        }
                    }
                }
                #region 原逻辑
                //if (rs.Count >= 1)
                //{
                //    dept = rs.Dequeue() as Dept;
                //    jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "^";
                //}
                //else
                //{
                //    return;
                //}

                //rs = baseBOBuilding.Query(new Building());

                //if (rs.Count > 0)
                //{
                //    foreach (Building building in rs)
                //    {
                //        jsdept += building.BuildingID + "|" + dept.DeptID + "|" + building.BuildingName + "^";

                //        baseBO.WhereClause = "BuildingID=" + building.BuildingID;
                //        rsf = baseBO.Query(new Floors());
                //        foreach (Floors floors in rsf)
                //        {
                //                jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + floors.BuildingID + "|" + floors.FloorName + "^";

                //            baseBO.WhereClause = "FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and ( ShopStatus = " + ConShop.CONSHOP_TYPE_PAUSE + " or ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + ") and a.ShopTypeID = b.ShopTypeID order by Shopcode";
                //            rsl = baseBO.Query(new ConShop());
                //            foreach (ConShop conShop in rsl)
                //            {
                //                jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + conShop.ShopId.ToString() + "|" + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + conShop.ShopCode + "(" + conShop.ShopName + ")^";
                //            }
                //        }

                //    }
                //}
                #endregion
            }
        }
        depttxt.Value = jsdept;
    }
}
