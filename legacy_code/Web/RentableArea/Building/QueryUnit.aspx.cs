using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.Dept;
using Base.Page;
using Shop.ShopType;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class RentableArea_Building_QueryUnit : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);

        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        if (!IsPostBack)
        {
            int[] status = BaseInfo.Role.Role.GetLeader();

            //大楼内码
            baseBO.WhereClause = "BuildingStatus=" + Building.BUILDING_STATUS_VALID;
            rs = baseBO.Query(new Building());
            foreach (Building buildings in rs)
            {
                cmbBuildingID.Items.Add(new ListItem(buildings.BuildingName, buildings.BuildingID.ToString()));
            }

            cmbFloorID.Items.Clear();
            baseBO.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID;
            rs = baseBO.Query(new Floors());
            foreach (Floors floors in rs)
            {
                cmbFloorID.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
            }

            //方位内码
            baseBO.WhereClause = "LocationStatus=" + Location.LOCATION_STATUS_VALID;
            rs = baseBO.Query(new Location());
            foreach (Location locations in rs)
            {
                cmbLocationID.Items.Add(new ListItem(locations.LocationName, locations.LocationID.ToString()));
            }

            //经营区域
            baseBO.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID;
            rs = baseBO.Query(new Area());
            foreach (Area area in rs)
            {
                cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
            }

            /*经营类别*/
            baseBO.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
            rs = baseBO.Query(new TradeRelation());
            foreach (TradeRelation tradeDef in rs)
            {
                cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
            }
            //商铺类型
            baseBO.WhereClause = "ShopTypeStatus =1";
            rs = baseBO.Query(new ShopType());
            foreach (ShopType objShopType in rs)
            {
                this.ddlShopType.Items.Add(new ListItem(objShopType.ShopTypeName, objShopType.ShopTypeID.ToString()));
            }
            //租金水平
            //baseBO.WhereClause = "AreaLevelStatus=" + AreaLevel.AREALEVEL_STATUS_VALID;
            //rs = baseBO.Query(new AreaLevel());
            //foreach (AreaLevel areaLevels in rs)
            //{
            //    cmbAreaLevel.Items.Add(new ListItem(areaLevels.AreaLevelDesc, areaLevels.AreaLevelID.ToString()));
            //}

            /*单元类别*/
            baseBO.WhereClause = "";
            ////BaseInfo.BaseCommon.BindDropDownList(baseBO, new UnitTypes(), "UnitTypeID", "UnitTypeName", this.ddlUnitType);
            DataSet ds = baseBO.QueryDataSet(new UnitTypes());
            DataRow dr = ds.Tables[0].NewRow();
            dr["UnitTypeID"] = "0";
            dr["UnitTypeName"] = "----";
            ds.Tables[0].Rows.Add(dr);
            ds.AcceptChanges();
            this.ddlUnitType.DataValueField = "UnitTypeID";
            this.ddlUnitType.DataTextField = "UnitTypeName";
            this.ddlUnitType.DataSource = ds.Tables[0];
            this.ddlUnitType.DataBind();

            //是否作废 IsBlankOut
            int[] IsBlankOutStatus = Units.GetBlankOutStatus();
                for (int i = 0; i < IsBlankOutStatus.Length; i++)
                {
                    this.cmbUnitStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Units.GetBlankOutStatusDesc(IsBlankOutStatus[i])), IsBlankOutStatus[i].ToString()));
                }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_labUnitTitle");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        Units units = new Units();
        SelUnit selunit = new SelUnit();
        SelLocation sellocation = new SelLocation();
        string unitid = "";
        DataSet dt = new DataSet();
        unitid = deptid.Value;

        Session["UnitID"] = deptid.Value;
        if (rbtnNoLeaseOut.Checked)
        {

            if (unitid.Length == 12)    //点击的节点：非unit
            {
                textlock();
                baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                rs = baseBO.Query(sellocation);
                if (rs.Count == 1)
                {
                    Session["BuildingID"] = deptid.Value;
                    sellocation = rs.Dequeue() as SelLocation;
                    cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                    cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                    cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                }

            }
            else if (unitid.Length > 12)  //点击unit
            {
                baseBO.WhereClause = "UnitID=" + unitid.Substring(unitid.Length - 3);
                rs = baseBO.Query(units);
                dt=baseBO.QueryDataSet(new Units());
                if (rs.Count == 1)
                {
                    units = rs.Dequeue() as Units;
                    txtUnitCode.Text = units.UnitCode;
                    try { cmbTradeID.SelectedValue = units.Trade2ID.ToString(); }
                    catch { }
                    try { this.ddlShopType.SelectedValue = units.ShopTypeID.ToString(); }
                    catch { }
                    baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(9,3);
                    baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                    rs = baseBO.Query(sellocation);
                    if (rs.Count == 1)
                    {
                        sellocation = rs.Dequeue() as SelLocation;
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    }

                    try { cmbTradeRelation.SelectedValue = units.AreaID.ToString(); }
                    catch { }
                    //try { cmbAreaLevel.SelectedValue = units.AreaLevelID.ToString(); }
                    //catch { }

                    txtFloorArea.Text = units.FloorArea.ToString();
                    txtUseArea.Text = units.UseArea.ToString();
                    try { cmbUnitStatus.SelectedValue = units.UnitStatus.ToString(); }
                    catch { }
                    txtNode.Text = units.Note;
                    try { this.ddlUnitType.SelectedValue = units.UnitTypeID.ToString(); }
                    catch { this.ddlUnitType.SelectedValue = "0"; }//单元类别
                }
            }
            else
            {
                /*提示信息-请选择方位信息*/
                txtUnitCode.Text = "";
                txtNode.Text = "";
                txtShopName.Text = "";
                txtFloorArea.Text = "";
                txtUseArea.Text = "";
            }
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
        else if (rbtnLeaseOut.Checked)
        {
            if (unitid.Length == 12)
            {
                textlock();
                Session["BuildingID"] = deptid.Value;
                baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";

                rs = baseBO.Query(sellocation);
                if (rs.Count == 1)
                {
                    sellocation = rs.Dequeue() as SelLocation;
                    cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                    cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                    cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                }
            }
            else if (unitid.Length > 12)
            {
                baseBO.WhereClause = "a.UnitID=" + unitid.Substring(unitid.Length - 3) + " and a.UnitID=b.UnitID and b.ShopID=c.ShopID";
                rs = baseBO.Query(selunit);
                //dt = baseBO.QueryDataSet(new Units());
                if (rs.Count == 1)
                {
                    selunit = rs.Dequeue() as SelUnit;
                    txtUnitCode.Text = selunit.UnitCode;
                    try { cmbTradeID.SelectedValue = selunit.Trade2ID.ToString(); }
                    catch { }
                    try { this.ddlShopType.SelectedValue = selunit.ShopTypeID.ToString(); }
                    catch { }
                    baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(9, 3);
                    baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";

                    rs = baseBO.Query(sellocation);
                    if (rs.Count == 1)
                    {
                        sellocation = rs.Dequeue() as SelLocation;
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    }
                    try { cmbTradeID.SelectedValue = units.Trade2ID.ToString(); } //units.Trade2ID.ToString(); }
                    catch { }

                    try { cmbTradeRelation.SelectedValue = selunit.AreaID.ToString(); }
                    catch { }
                    try { cmbAreaLevel.SelectedValue = selunit.AreaLevelID.ToString(); }
                    catch { }
                    txtFloorArea.Text = selunit.FloorArea.ToString();
                    txtUseArea.Text = selunit.UseArea.ToString();
                    cmbUnitStatus.Items.Add(new ListItem(SelUnit.BLANKOUT_STATUS_LEASEOUTNAME, SelUnit.BLANKOUT_STATUS_LEASEOUT.ToString()));
                    try { cmbUnitStatus.SelectedValue = SelUnit.BLANKOUT_STATUS_LEASEOUT.ToString(); }
                    catch { }
                    txtNode.Text = selunit.Note;
                    txtShopName.Text = selunit.ShopName;
                }
            }
            else
            {
                /*提示信息-请选择方位信息*/
                txtUnitCode.Text = "";
                txtNode.Text = "";
                txtShopName.Text = "";
                txtFloorArea.Text = "";
                txtUseArea.Text = "";
            }
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_LEASEOUT);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
        else if (rbtnBlankOut.Checked)
        {
            if (unitid.Length == 12)
            {
                textlock();
                Session["BuildingID"] = deptid.Value;
                baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                rs = baseBO.Query(sellocation);
                if (rs.Count == 1)
                {
                    sellocation = rs.Dequeue() as SelLocation;
                    cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                    cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                    cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                }
            }
            else if (unitid.Length > 12)
            {
                baseBO.WhereClause = "a.UnitID=" + unitid.Substring(unitid.Length - 3) + " and b.RentStatus=" + SelUnit.RENT_ATUS_VALID + " and a.UnitID=b.UnitID and b.ShopID=c.ShopID";
                rs = baseBO.Query(selunit);
                dt = baseBO.QueryDataSet(new SelUnit());
                if (rs.Count == 1)
                {
                    selunit = rs.Dequeue() as SelUnit;
                    txtUnitCode.Text = selunit.UnitCode;

                    baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(9, 3);
                    baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                    rs = baseBO.Query(sellocation);
                    if (rs.Count == 1)
                    {
                        sellocation = rs.Dequeue() as SelLocation;
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    }
                    try { cmbTradeID.SelectedValue = dt.Tables[0].Rows[0]["Trade2ID"].ToString(); } //selunit.Trade2ID.ToString(); }
                    catch { }
                    try { cmbTradeRelation.SelectedValue = selunit.AreaID.ToString(); }
                    catch { }
                    try { cmbAreaLevel.SelectedValue = selunit.AreaLevelID.ToString(); }
                    catch { }
                    try { this.ddlShopType.SelectedValue = selunit.ShopTypeID.ToString(); }
                    catch { }
                    txtFloorArea.Text = selunit.FloorArea.ToString();
                    txtUseArea.Text = selunit.UseArea.ToString();
                    try { cmbUnitStatus.Items.Add(new ListItem(SelUnit.BLANKOUT_STATUS_LEASEOUTNAME, SelUnit.BLANKOUT_STATUS_LEASEOUT.ToString())); }
                    catch { }
                    txtNode.Text = selunit.Note;
                    txtShopName.Text = selunit.ShopName;
                }
                else
                {
                    baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(9, 3);
                    baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                    rs = baseBO.Query(sellocation);
                    if (rs.Count == 1)
                    {
                        sellocation = rs.Dequeue() as SelLocation;
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    }

                    try { cmbTradeRelation.SelectedValue = selunit.AreaID.ToString(); }
                    catch { }
                    try { cmbAreaLevel.SelectedValue = selunit.AreaLevelID.ToString(); }
                    catch { }
                    try { this.ddlShopType.SelectedValue = selunit.ShopTypeID.ToString(); }
                    catch { }
                    txtFloorArea.Text = selunit.FloorArea.ToString();
                    txtUseArea.Text = selunit.UseArea.ToString();
                    cmbUnitStatus.Items.Add(new ListItem(SelUnit.BLANKOUT_STATUS_LEASEOUTNAME, SelUnit.BLANKOUT_STATUS_LEASEOUT.ToString()));
                }
            }
            else
            {
                /*提示信息-请选择方位信息*/
                txtUnitCode.Text = "";
                txtNode.Text = "";
                txtShopName.Text = "";
                txtFloorArea.Text = "";
                txtUseArea.Text = "";
            }
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_VALID);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
    }

    #region
    //private void showtreenode(string unitstatus)
    //{
    //    string jsdept = "";
    //    BaseBO baseBO = new BaseBO();
    //    BaseBO baseBOBuilding = new BaseBO();
    //    BaseBO baseareaBO = new BaseBO();
    //    Resultset rs = new Resultset();
    //    Resultset rsd = new Resultset();
    //    Resultset rsf = new Resultset();
    //    Resultset rsl = new Resultset();
    //    Resultset rsu = new Resultset();
    //    Dept dept = new Dept();
    //    Dept deptGrp = new Dept();

    //    baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团


    //    //baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;

    //    rs = baseBO.Query(dept);

    //    if (rs.Count == 1)
    //    {
    //        deptGrp = rs.Dequeue() as Dept;
    //        jsdept = deptGrp.DeptID + "|" + "0" + "|" + deptGrp.DeptName + "^";
    //    }
    //    else
    //    {
    //        return;
    //    }
    //    baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
    //    rsd = baseBO.Query(dept);
    //    if (rsd.Count > 0)
    //    {
    //        foreach (Dept store in rsd)
    //        {
    //            jsdept += store.DeptID + "|" + deptGrp.DeptID + "|" + store.DeptName + "^";

    //            baseBOBuilding.WhereClause = "StoreId=" + store.DeptID;


    //            rs = baseBOBuilding.Query(new Building());

    //            if (rs.Count > 0)
    //            {
    //                foreach (Building building in rs)
    //                {
    //                    jsdept += store.DeptID + building.BuildingID + "|" + store.DeptID + "|" + building.BuildingName + "^";

    //                    baseBO.WhereClause = "BuildingID=" + building.BuildingID;
    //                    rsf = baseBO.Query(new Floors());
    //                    foreach (Floors floors in rsf)
    //                    {
    //                        jsdept += store.DeptID + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID + floors.BuildingID + "|" + floors.FloorName + "^";

    //                        baseBO.WhereClause = "FloorID=" + floors.FloorID;
    //                        rsl = baseBO.Query(new Location());

    //                        foreach (Location location in rsl)
    //                        {
    //                            jsdept += store.DeptID + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";

    //                            baseBO.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + unitstatus;
    //                            rsu = baseBO.Query(new Units());
    //                            foreach (Units units in rsu)
    //                            {
    //                                jsdept += store.DeptID + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + units.UnitID + "|" + store.DeptID + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + units.UnitCode + "^";
    //                            }
    //                        }
    //                    }

    //                }
    //            }
    //        }
    //    }
    //    depttxt.Value = jsdept;
    //}
#endregion
    private void showtreenode(string unitstatus)
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        BaseBO baseBOBuilding = new BaseBO();
        BaseBO baseareaBO = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsd = new Resultset();
        Resultset rsf = new Resultset();
        Resultset rsl = new Resultset();
        Resultset rsu = new Resultset();
        Dept dept = new Dept();
        Dept deptGrp = new Dept();


        //baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        rs = baseBO.Query(dept);
        if (rs.Count == 1)
        {
            deptGrp = rs.Dequeue() as Dept;
            jsdept = deptGrp.DeptID + "|" + "0" + "|" + deptGrp.DeptName + "^";
        }
        else
        {
            return;
        }
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }   
        rsd = baseBO.Query(dept);
        if (rsd.Count > 0)
        {
            foreach (Dept store in rsd)
            {
                jsdept += store.DeptID + "|" + deptGrp.DeptID + "|" + store.DeptName + "^";

                baseBOBuilding.WhereClause = "StoreId=" + store.DeptID;

                rs = baseBOBuilding.Query(new Building());

                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";

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
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + floors.BuildingID.ToString() + "|" + floors.FloorName.ToString() + "^";

                            baseBO.WhereClause = "FloorID=" + floors.FloorID;
                            rsl = baseBO.Query(new Location());

                            foreach (Location location in rsl)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";

                                baseBO.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + unitstatus;
                                rsu = baseBO.Query(new Units());
                                foreach (Units units in rsu)
                                {
                                    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + units.UnitID + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + units.UnitCode + "^";
                                }
                            }
                        }

                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    protected void rbtnLeaseOut_CheckedChanged(object sender, EventArgs e)
    {
        textlock();
        selectdeptid.Value = "";
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_LEASEOUT);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    private void textlock()
    {
        txtFloorArea.Text = "";
        txtNode.Text = "";
        txtUnitCode.Text = "";
        txtShopName.Text = "";
        txtUseArea.Text = "";
    }
    protected void rbtnNoLeaseOut_CheckedChanged(object sender, EventArgs e)
    {
        textlock();
        cmbUnitStatus.Items.Clear();
        int[] IsBlankOutStatus = Units.GetBlankOutStatus();
        for (int i = 0; i < IsBlankOutStatus.Length; i++)
        {
            this.cmbUnitStatus.Items.Add(new ListItem(Units.GetBlankOutStatusDesc(IsBlankOutStatus[i]), IsBlankOutStatus[i].ToString()));
        }
        selectdeptid.Value = "";
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void rbtnBlankOut_CheckedChanged(object sender, EventArgs e)
    {
        textlock();
        selectdeptid.Value = "";
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_VALID);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void cmbLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
