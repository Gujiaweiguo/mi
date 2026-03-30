using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.Dept;
using Base;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
public partial class RentableArea_Building_QueryBuilding : BasePage
{
    public string baseInfo;
    public string mallTitle;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode();
        if (!IsPostBack)
        {
            int[] statusBuilding = Building.GetBuildingStatus();
            for (int i = 0; i < statusBuilding.Length; i++)
            {
                cmbBuildingStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Building.GetBuildingStatusDesc(statusBuilding[i])), statusBuilding[i].ToString()));
            }

            int[] statusFloors = Floors.GetFloorStatus();
            for (int i = 0; i < statusFloors.Length; i++)
            {
                cmbFloorStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Floors.GetFloorStatusDesc(statusFloors[i])), statusFloors[i].ToString()));
            }

            int[] statusLocation = Location.GetLocationStatus();
            for (int i = 0; i < statusLocation.Length; i++)
            {
                this.cmbLocationStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Location.GetLocationStatusDesc(statusLocation[i])), statusLocation[i].ToString()));
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblBuildingtit");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            mallTitle = (String)GetGlobalResourceObject("BaseInfo", "Dept_MallNameQuery");
        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        Building building = new Building();
        Floors floors = new Floors();
        Location location = new Location();
        SelFloors selflooes = new SelFloors();
        Resultset rs = new Resultset();
        SelLocation sellocation = new SelLocation();
        BaseBO baseBO = new BaseBO();
        BaseBO baseBOwhere = new BaseBO();
        string buildingid = "";

        buildingid = deptid.Value;

        if (buildingid.Length == 6)
        {
            textlock();
            Session["BuildingID"] = deptid.Value;
            baseBO.WhereClause = "BuildingID=" + buildingid.Substring(3,3);
            rs = baseBO.Query(building);
            if (rs.Count == 1)
            {
                building = rs.Dequeue() as Building;
                txtBuildingCode.Text = building.BuildingCode;
                txtBuildingName.Text = building.BuildingName;
                cmbBuildingStatus.SelectedValue = building.BuildingStatus.ToString();
            }
        }
        else if (buildingid.Length == 9)
        {
            textlock();
            Session["BuildingID"] = deptid.Value;
            baseBO.WhereClause = "a.BuildingID=b.BuildingID and floorid=" + buildingid.Substring(buildingid.Length - 3);
            baseBO.GroupBy = "BuildingCode,BuildingName,FloorCode,FloorName,BuildingStatus,FloorStatus,b.StoreID";
            rs = baseBO.Query(selflooes);
            if (rs.Count == 1)
            {
                selflooes = rs.Dequeue() as SelFloors;
                txtBuildingCode.Text = selflooes.BuildingCode;
                txtBuildingName.Text = selflooes.BuildingName;
                cmbBuildingStatus.SelectedValue = building.BuildingStatus.ToString();

                txtFloorCode.Text = selflooes.FloorCode;
                txtFloorName.Text = selflooes.FloorName;
                cmbFloorStatus.SelectedValue = selflooes.FloorStatus.ToString();
            }
        }
        else if (buildingid.Length > 9)
        {
            textlock();
            Session["BuildingID"] = deptid.Value;
            baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + buildingid.Substring(buildingid.Length - 3);
            baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
            rs = baseBO.Query(sellocation);
            if (rs.Count == 1)
            {
                sellocation = rs.Dequeue() as SelLocation;
                txtBuildingCode.Text = sellocation.BuildingCode;
                txtBuildingName.Text = sellocation.BuildingName;
                cmbBuildingStatus.SelectedValue = building.BuildingStatus.ToString();

                txtFloorCode.Text = sellocation.FloorCode;
                txtFloorName.Text = sellocation.FloorName;
                cmbFloorStatus.SelectedValue = selflooes.FloorStatus.ToString();

                txtLocationCode.Text = sellocation.LocationCode;
                txtLocationName.Text = sellocation.LocationName;
                cmbLocationStatus.SelectedValue = sellocation.LocationStatus.ToString();
            }

        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void showtreenode()
     {
        string jsdept = "";
        BaseBO baseBOgrp = new BaseBO();
        BaseBO baseBO = new BaseBO();
        BaseBO baseBOBuilding = new BaseBO();
        BaseBO baseareaBO = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsgrp = new Resultset();
        Resultset rsf = new Resultset();
        Resultset rsl = new Resultset();
        Dept dept = new Dept();
        Dept deptGrp =new Dept();

        baseBOgrp.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        rsgrp = baseBOgrp.Query(dept);

        if (rsgrp.Count == 1)
        {
            deptGrp =rsgrp.Dequeue() as Dept;
            jsdept = deptGrp.DeptID + "|" + "0" + "|" + deptGrp.DeptName + "^";    //id + "|" + 父节点id (0为根节点)+ "|" + name + "^"
        }
        else
        {
            return;
        }

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        baseBO.OrderBy = "OrderID";
        rs = baseBO.Query(dept);
        baseBO.OrderBy = "";

        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + deptGrp.DeptID + "|" + store.DeptName + "^";

                baseBOBuilding.WhereClause = "StoreId=" + store.DeptID;
                rs = baseBOBuilding.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString()+building.BuildingID + "|" + store.DeptID + "|" + building.BuildingName + "^";

                        baseBO.WhereClause = "floors.BuildingID=" + building.BuildingID;
                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            baseBO.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        rsf = baseBO.Query(new floorsAuth());
                        foreach (floorsAuth floors in rsf)
                        {
                            if (floors.FloorStatus == Floors.FLOOR_STATUS_INVALID)
                            {
                                jsdept += store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString()+floors.BuildingID + "|" + floors.FloorName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                            }
                            else
                            {
                                jsdept += store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + "|" +store.DeptID.ToString()+floors.BuildingID + "|" + floors.FloorName + "|" + "" + "^";
                            }
                            baseBO.WhereClause = "FloorID=" + floors.FloorID;
                            rsl = baseBO.Query(new Location());

                            foreach (Location location in rsl)
                            {
                                if (location.LocationStatus == Location.LOCATION_STATUS_INVALID)
                                {
                                    jsdept += store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                                }
                                else
                                {
                                    jsdept += store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "|" + "" + "^";
                                }

                            }
                        }

                    }
                }

            }

        }
        else
        {
            return;
        }

 
        depttxt.Value = jsdept;
    }

    private void textlock()
    {
        txtBuildingCode.Text = "";

        txtBuildingName.Text = "";

        txtFloorCode.Text = "";

        txtFloorName.Text = "";

        txtLocationCode.Text = "";

        txtLocationName.Text = "";
    }
}
