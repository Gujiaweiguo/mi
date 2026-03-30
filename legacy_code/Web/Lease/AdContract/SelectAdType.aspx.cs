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

public partial class Lease_AdContract_SelectAdType : BasePage
{
    public string strBase;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.IsPostBack)
            ShowTree();
        //selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
        strBase = (String)GetGlobalResourceObject("BaseInfo", "Rpt_AdUnit") + (String)GetGlobalResourceObject("BaseInfo", "Rpt_List");
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
                        string strSql = @"select AdBoardID,AdBoardCode,AdBoardName,AdBoardManage.AdBoardTypeID,AdBoardtype.AdBoardtypeName,AdBoardStatus,Note,StoreID,BuildingID from AdBoardManage inner join AdBoardtype on AdBoardtype.AdBoardtypeID = AdBoardManage.AdBoardtypeID where StoreId='" + objStore.DeptID + "' and buildingId='" + building.BuildingID + "'";
                        DataSet ds = objBaseBo.QueryDataSet(strSql);
                        if(ds!=null&&ds.Tables[0].Rows.Count>0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                jsdept += objStore.DeptID.ToString() + building.BuildingID.ToString() + ds.Tables[0].Rows[i]["AdBoardTypeID"].ToString() + "|" + objStore.DeptID.ToString() + building.BuildingID.ToString() + "|" + ds.Tables[0].Rows[i]["AdBoardtypeName"].ToString() + "^";
                                string strSqlAd = "select AdBoardID,AdBoardCode,AdBoardName,AdBoardTypeID,AdBoardStatus,Note,StoreID,BuildingID from AdBoardManage where AdBoardTypeID='" + ds.Tables[0].Rows[i]["AdBoardTypeID"].ToString() + "' and buildingid='" + building.BuildingID + "'";
                                DataSet dsAd = objBaseBo.QueryDataSet(strSqlAd);
                                if (dsAd != null && dsAd.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < dsAd.Tables[0].Rows.Count; j++)
                                    {
                                        jsdept += objStore.DeptID.ToString() + building.BuildingID.ToString() + ds.Tables[0].Rows[i]["AdBoardTypeID"].ToString() + dsAd.Tables[0].Rows[j]["AdBoardID"].ToString() + "|" + objStore.DeptID.ToString() + building.BuildingID.ToString() + ds.Tables[0].Rows[i]["AdBoardTypeID"].ToString() + "|" + dsAd.Tables[0].Rows[j]["AdBoardName"].ToString() + "^";
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
