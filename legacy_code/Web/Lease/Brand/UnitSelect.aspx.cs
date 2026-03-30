using System;
using System.Data;
using System.Web.UI;

using RentableArea;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class Lease_TradeRelation_TradeRelationSelect : BasePage
{
    public string selectTradeLevel;
    private bool ShowArea = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["Area"] != null)
            ShowArea = true;
        if (!IsPostBack)
        {
            string strDeptID = Request["deptid"].ToString();
            if (Request["startDate"] != null & Request["endDate"] != null)
            {
                DateTime startDate = Convert.ToDateTime(Request["startDate"].ToString());
                DateTime endDate = Convert.ToDateTime(Request["endDate"].ToString());
                this.ShowNuitTree(strDeptID, startDate, endDate);
            }
            else
            {
                this.ShowNuitTree(strDeptID);
            }
            
            selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_IntentUnits");
        }
    }


 
    /// <summary>
    /// 根据项目ID，查询意向单元
    /// </summary>
    /// <param name="strDeptID">项目编号</param>
    private void ShowNuitTree(string strDeptID)
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        BaseBO objBaseBoBuilding = new BaseBO();
        BaseBO objBaseBoArea = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsFloor = new Resultset();
        Resultset rsLocation = new Resultset();
        Resultset rsUnit = new Resultset();
        Dept objDept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        objBaseBo.WhereClause = "deptid=" + strDeptID;
        rs = objBaseBo.Query(objDept);
        if (rs.Count == 1)
        {
            objDept = rs.Dequeue() as Dept;
            jsdept = "0" + objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
        }
        else
        {
            return;
        }
        objBaseBoBuilding.WhereClause = "StoreID =" + strDeptID + " and buildingstatus=" + Building.BUILDING_STATUS_VALID;//大楼条件
        rs = objBaseBoBuilding.Query(new Building());//大楼
        if (rs.Count > 0)
        {
            foreach (Building building in rs)
            {
                objBaseBo.WhereClause = "BuildingID=" + building.BuildingID  + " and StoreID='" + strDeptID + "' and unitstatus=0";//层条件
                DataSet dsFloor = objBaseBo.QueryDataSet(new Units());//
                if (dsFloor != null && dsFloor.Tables[0].Rows.Count > 0)//
                {
                    jsdept += building.BuildingID.ToString() + building.StoreID.ToString() + "|" + "0" + objDept.DeptID + "|" + building.BuildingName + "^";
                    objBaseBo.WhereClause = "BuildingID=" + building.BuildingID + " and StoreID=" + strDeptID + " and floorstatus=" + Floors.FLOOR_STATUS_VALID ;
                    if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                    {
                        objBaseBo.WhereClause += " and exists (SELECT FloorID FROM authUser WHERE  floors.floorid = authUser.floorid AND userID ='" + sessionUser.UserID + "')";
                    }
                    rsFloor = objBaseBo.Query(new Floors());//楼层
                    foreach (Floors floors in rsFloor)
                    {
                        objBaseBo.WhereClause = "FloorID=" + floors.FloorID + " and StoreID='" + strDeptID + "' and unitstatus=0";//方位条件
                        DataSet dsLocation = objBaseBo.QueryDataSet(new Units());//
                        if (dsLocation != null && dsLocation.Tables[0].Rows.Count > 0)//
                        {
                            jsdept += floors.FloorID.ToString() + building.BuildingID.ToString() + objDept.DeptID.ToString() + "|" + building.BuildingID.ToString() + building.StoreID.ToString() + "|" + floors.FloorName + "^";
                            objBaseBo.WhereClause = "FloorID=" + floors.FloorID + " and StoreID=" + strDeptID + " and locationstatus=" + Location.LOCATION_STATUS_VALID;
                            rsLocation = objBaseBo.Query(new Location());//方位
                            foreach (Location location in rsLocation)
                            {
                                objBaseBo.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and StoreID=" + strDeptID + " and unitstatus=0"; //
                                DataSet dsUnit = objBaseBo.QueryDataSet(new Units());//判断大楼是否有未出租的单元
                                if (dsUnit != null && dsUnit.Tables[0].Rows.Count > 0)
                                {
                                    jsdept += location.LocationID.ToString() + floors.FloorID.ToString() + building.BuildingID.ToString() +location.StoreID +"|" + floors.FloorID.ToString() + building.BuildingID.ToString() + objDept.DeptID.ToString() + "|" + location.LocationName + "^";
                                    objBaseBo.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and StoreID=" + strDeptID + " and unitstatus=" + Units.BLANKOUT_STATUS_INVALID; //
                                    rsUnit = objBaseBo.Query(new Units());
                                    if (rsUnit.Count > 0)
                                    {
                                        foreach (Units units in rsUnit)
                                        {
                                            if (this.ShowArea == true)//显示面积
                                                jsdept += units.UnitID + "|" + location.LocationID.ToString() + floors.FloorID.ToString() + building.BuildingID.ToString() + location.StoreID + "|" + units.UnitCode + " : " + units.FloorArea + "|" + 0 + "^";
                                            else
                                                jsdept += units.UnitID + "|" + location.LocationID.ToString() + floors.FloorID.ToString() + building.BuildingID.ToString() + location.StoreID + "|" + units.UnitCode + "|" + 0 + "^";
                                        }
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
    /// <summary>
    /// 根据项目ID、日期期间，查询意向单元
    /// </summary>
    /// <param name="strDeptID"></param>
    /// <param name="startDate"></param>
    /// <para name="endDate"></param>
    private void ShowNuitTree(string strDeptID,DateTime startDate,DateTime endDate)
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        BaseBO objBaseBoBuilding = new BaseBO();
        BaseBO objBaseBoArea = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsFloor = new Resultset();
        Resultset rsLocation = new Resultset();
        Resultset rsUnit = new Resultset();
        Dept objDept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        objBaseBo.WhereClause = "deptid=" + strDeptID;
        rs = objBaseBo.Query(objDept);
        if (rs.Count == 1)
        {
            objDept = rs.Dequeue() as Dept;
            jsdept = objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
        }
        else
        {
            return;
        }
        objBaseBoBuilding.WhereClause = "StoreID =" + strDeptID + " and buildingstatus=" + Building.BUILDING_STATUS_VALID; ;//大楼条件
        rs = objBaseBoBuilding.Query(new Building());//大楼
        if (rs.Count > 0)
        {
            foreach (Building building in rs)
            {
                objBaseBo.WhereClause = "BuildingID=" + building.BuildingID + " and StoreID='" + strDeptID + "' and unitstatus=0";//层条件
                DataSet dsFloor = objBaseBo.QueryDataSet(new Units());//
                if (dsFloor != null && dsFloor.Tables[0].Rows.Count > 0)//
                {
                    jsdept += building.BuildingID.ToString() + building.StoreID.ToString() + "|" + objDept.DeptID + "|" + building.BuildingName + "^";
                    objBaseBo.WhereClause = "BuildingID=" + building.BuildingID + " and StoreID=" + strDeptID + " and floorstatus=" + Floors.FLOOR_STATUS_VALID;
                    if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                    {
                        objBaseBo.WhereClause += " and exists (SELECT FloorID FROM authUser WHERE  floors.floorid = authUser.floorid AND userID ='" + sessionUser.UserID + "')";
                    }
                    rsFloor = objBaseBo.Query(new Floors());//楼层
                    foreach (Floors floors in rsFloor)
                    {
                        objBaseBo.WhereClause = "FloorID=" + floors.FloorID + " and StoreID='" + strDeptID + "' and unitstatus=0";//方位条件
                        DataSet dsLocation = objBaseBo.QueryDataSet(new Units());//
                        if (dsLocation != null && dsLocation.Tables[0].Rows.Count > 0)//
                        {
                            jsdept += floors.FloorID.ToString() + building.BuildingID.ToString() + objDept.DeptID.ToString() + "|" + building.BuildingID.ToString() + building.StoreID.ToString() + "|" + floors.FloorName + "^";
                            objBaseBo.WhereClause = "FloorID=" + floors.FloorID + " and StoreID=" + strDeptID + " and locationstatus=" + Location.LOCATION_STATUS_VALID;
                            rsLocation = objBaseBo.Query(new Location());//方位
                            foreach (Location location in rsLocation)
                            {
                                objBaseBo.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and StoreID=" + strDeptID + " and unitstatus=0"; //
                                DataSet dsUnit = objBaseBo.QueryDataSet(new Units());//判断大楼是否有未出租的单元
                                if (dsUnit != null && dsUnit.Tables[0].Rows.Count > 0)
                                {
                                    jsdept += location.LocationID.ToString() + floors.FloorID.ToString() + building.BuildingID.ToString() + location.StoreID + "|" + floors.FloorID.ToString() + building.BuildingID.ToString() + objDept.DeptID.ToString() + "|" + location.LocationName + "^";
                                    string strWhere = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and StoreID=" + strDeptID;
                                    //如果开始结束日期都有，则按照开始结束日期进行查询意向商铺，否则则取当前状态
                                    if (startDate != null & endDate != null)
                                    {
                                        strWhere += " and UnitID not in (select conshopunit.unitid from conshopunit inner join conshop on (conshop.shopid=conshopunit.shopid) " +
                                                    "where conshop.shopenddate >='" + startDate  + "')"  + " and unitstatus !=" + Units.BLANKOUT_STATUS_VALID;
        
                                    }
                                    else
                                    {
                                        //if (startDate != null)
                                        //{
                                        //    strWhere += " and UnitID not in (select conshopunit.unitid from conshopunit inner join conshop on (conshop.shopid=conshopunit.shopid) " +
                                        //            "where conshop.shopstartdate >= '" + startDate + "')";
                                        //}
                                        //if (endDate != null)
                                        //{
                                        //    strWhere += " and UnitID not in (select conshopunit.unitid from conshopunit inner join conshop on (conshop.shopid=conshopunit.shopid) " +
                                        //            "where conshop.shopenddate <= '" + endDate + "')";
                                        //}
                                        //if (startDate == null & endDate == null)
                                        //{
                                            strWhere += " and unitstatus=" + Units.BLANKOUT_STATUS_INVALID;
                                        //}
                                    }
                                    
                                    objBaseBo.WhereClause=strWhere;
                                    rsUnit = objBaseBo.Query(new Units());
                                    if (rsUnit.Count > 0)
                                    {
                                        foreach (Units units in rsUnit)
                                        {
                                            if (this.ShowArea == true)//显示面积
                                                jsdept += units.UnitID + "|" + location.LocationID.ToString() + floors.FloorID.ToString() + building.BuildingID.ToString() + location.StoreID + "|" + units.UnitCode + " : " + units.FloorArea + "|" + 0 + "^";
                                            else
                                                jsdept += units.UnitID + "|" + location.LocationID.ToString() + floors.FloorID.ToString() + building.BuildingID.ToString() + location.StoreID + "|" + units.UnitCode + "|" + 0 + "^";
                                        }
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "window.close();", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        char[] treenodeid = new char[] { ',' };
        string treestr = deptid.Value;
        string strUnitCode = "";
        string strUnitID = "";
        string strBuiFloorLoca = "";
        foreach (string substr in treestr.Split(treenodeid))
        {
            if (substr != "")
            {
                try
                {
                    strUnitCode =strUnitCode+ this.GetUnitCode(substr) + ",";
                    strUnitID = strUnitID + substr + ",";
                    strBuiFloorLoca = strBuiFloorLoca + this.GetBuildFloorLocation(substr) + ",";
                }
                catch
                { }
            }
        }
        this.txtUnitCode.Text = strUnitCode;//UnitCode
        this.txtUnitID.Text = strUnitID;//UnitID
        this.txtStore.Text = strBuiFloorLoca;//得到楼、楼层、方位ID,用“;”将之分开
        
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "ReturnValue()", true);
    }
    /// <summary>
    /// 得到单元的Code
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    private string GetUnitCode(string strID)
    { 
        BaseBO objbaseBo = new BaseBO();
        objbaseBo.WhereClause = "unitid="+strID;
        Resultset rs = objbaseBo.Query(new Units());
        if (rs.Count == 1)
        {
            Units objUints = rs.Dequeue() as Units;
            return objUints.UnitCode;
        }
        else
            return "";
    }
    /// <summary>
    /// 得到楼、楼层、方位ID,用“;”将之分开
    /// </summary>
    /// <returns></returns>
    private string GetBuildFloorLocation(string strID)
    { 
        BaseBO objbaseBo = new BaseBO();
        objbaseBo.WhereClause = "unitid="+strID;
        Resultset rs = objbaseBo.Query(new Units());
        if (rs.Count == 1)
        {
            Units objUints = rs.Dequeue() as Units;
            return objUints.BuildingID + ";" + objUints.FloorID + ";" + objUints.LocationID;
        }
        else
            return "";
    }

}
