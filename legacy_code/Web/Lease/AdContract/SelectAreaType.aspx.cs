using System;

using RentableArea;
using Base.Biz;
using Base.DB;
using Lease.ConShop;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.authUser;
using BaseInfo.User;
using Lease.AdContract;
using System.Data;

public partial class Lease_AdContract_SelectAreaType : BasePage
{
    public string strBase;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.IsPostBack)
            ShowTree();
        strBase = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Area") + (String)GetGlobalResourceObject("BaseInfo", "Rpt_List");
    }

    private void ShowTree()
    {
        string jsdept = "";
        Resultset rs = new Resultset();
        Dept dept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "DeptType=1";//集团
        Resultset rsD = objBaseBo.Query(new Dept());
        Dept objD = rsD.Dequeue() as Dept;
        jsdept = objD.DeptID + "|" + objD.PDeptID + "|" + objD.DeptName + "^";

        objBaseBo.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and exists (SELECT storeID FROM authUser WHERE  dept.deptid = authUser.storeid AND userID ='" + sessionUser.UserID + "')";
        }

        rs = objBaseBo.Query(dept);
        if (rs.Count > 0)
        {
            foreach (Dept objStore in rs)
            {
                jsdept += objStore.DeptID + "|" + objD.DeptID + "|" + objStore.DeptName + "^";

                objBaseBo.WhereClause = "StoreId=" + objStore.DeptID;
                rs = objBaseBo.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += objStore.DeptID.ToString() + building.BuildingID.ToString() + "|" + objStore.DeptID + "|" + building.BuildingName + "^";

                        objBaseBo.WhereClause = "";
                        string strSql = @"select AreaID,AreaCode,AreaName,AreaManage.AreaTypeID,AreaType.AreaTypeDesc,AreaStatus,Note,StoreID,BuildingID from AreaManage inner join AreaType on AreaType.AreaTypeID = AreaManage.AreaTypeID  where StoreId='" + objStore.DeptID + "' and buildingId='" + building.BuildingID + "'";
                        DataSet ds = objBaseBo.QueryDataSet(strSql);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                jsdept += objStore.DeptID.ToString() + building.BuildingID.ToString() + ds.Tables[0].Rows[i]["AreaTypeID"].ToString() + "|" + objStore.DeptID.ToString() + building.BuildingID.ToString() + "|" + ds.Tables[0].Rows[i]["AreaTypeDesc"].ToString() + "^";
                                string strSqlAd = "select AreaID,AreaCode,AreaName,AreaTypeID,AreaStatus,Note,StoreID,BuildingID from AreaManage where AreaTypeID='" + ds.Tables[0].Rows[i]["AreaTypeID"].ToString() + "'";
                                DataSet dsAd = objBaseBo.QueryDataSet(strSqlAd);
                                if (dsAd != null && dsAd.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < dsAd.Tables[0].Rows.Count; j++)
                                    {
                                        jsdept += objStore.DeptID.ToString() + building.BuildingID.ToString() + ds.Tables[0].Rows[i]["AreaTypeID"].ToString() + dsAd.Tables[0].Rows[j]["AreaID"].ToString() + "|" + objStore.DeptID.ToString() + building.BuildingID.ToString() + ds.Tables[0].Rows[i]["AreaTypeID"].ToString() + "|" + ds.Tables[0].Rows[j]["AreaName"].ToString() + "^";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
}
