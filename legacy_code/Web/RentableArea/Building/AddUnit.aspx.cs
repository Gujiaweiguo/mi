using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.DB;
using Base.Biz;
using BaseInfo.User;
using RentableArea;
using BaseInfo.Dept;
using Base;
using Base.Page;
using Shop.ShopType;
using BaseInfo.authUser;
public partial class RentableArea_Building_AddUnit : BasePage
{
    public string baseInfo;
    public string strFresh;
    private static int OPR_ADD = 1;
    private static int OPR_EDIT = 2;
    public string mesAdd = "";
    public string mesLocation = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);

        /*提示信息*/
        mesAdd = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd");
        mesLocation = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidSelectLocation");

        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        txtFloorArea.Attributes.Add("onkeydown", "TextLeave()");
        txtUseArea.Attributes.Add("onkeydown", "TextLeave()");
        txtUnitCode.Attributes.Add("onblur", "TextIsNotNull(txtUnitCode,ImgUnitCode)");
        txtFloorArea.Attributes.Add("onblur", "TextIsNotNull(txtFloorArea,ImgFloorArea)");
        txtUseArea.Attributes.Add("onblur", "TextIsNotNull(txtUseArea,ImgtxtUseArea)");
        this.btnEdit.Enabled = false;
        this.btnAdd.Enabled = false;
        #region
        //btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");
        #endregion
        if (!IsPostBack)
        {
            int[] status = BaseInfo.Role.Role.GetLeader();

            /*大楼内码*/
            baseBO.WhereClause = "BuildingStatus=" + Building.BUILDING_STATUS_VALID;
            rs = baseBO.Query(new Building());
            foreach (Building buildings in rs)
            {
                cmbBuildingID.Items.Add(new ListItem(buildings.BuildingName, buildings.BuildingID.ToString()));
            }

            /*楼层*/
            cmbFloorID.Items.Clear();
            baseBO.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID;
            rs = baseBO.Query(new Floors());
            foreach (Floors floors in rs)
            {
                cmbFloorID.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
            }

            /*方位内码*/
            baseBO.WhereClause = "LocationStatus=" + Location.LOCATION_STATUS_VALID;
            rs = baseBO.Query(new Location());
            foreach (Location locations in rs)
            {
                cmbLocationID.Items.Add(new ListItem(locations.LocationName, locations.LocationID.ToString()));
            }

            /*经营区域*/
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
                this.ddlShopType.Items.Add(new ListItem(objShopType.ShopTypeName,objShopType.ShopTypeID.ToString()));
            }
            /*租金水平*/
            //baseBO.WhereClause = "RentLevelStatus=" + RentLevel.RENTLEVEL_STATUS_VALID;
            //rs = baseBO.Query(new RentLevel());
            //foreach (RentLevel rentLevel in rs)
            //{
            //    cmbRentLevel.Items.Add(new ListItem(rentLevel.RentLevelDesc, rentLevel.RentLevelID.ToString()));
            //}

            /*单元类别*/
            baseBO.WhereClause = "";
            DataSet ds = baseBO.QueryDataSet(new UnitTypes());
            this.ddlUnitType.DataValueField = "UnitTypeID";
            this.ddlUnitType.DataTextField = "UnitTypeName";
            this.ddlUnitType.DataSource = ds.Tables[0];
            this.ddlUnitType.DataBind();
            //
            /*是否作废 IsBlankOut*/
            int[] IsBlankOutStatus = Units.GetBlankOutStatus();
            for (int i = 0; i < IsBlankOutStatus.Length; i++)
            {
                this.cmbUnitStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Units.GetBlankOutStatusDesc(IsBlankOutStatus[i])), IsBlankOutStatus[i].ToString()));
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_labUnitTitle");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    protected void cmbBuildingID_SelectedIndexChanged(object sender, EventArgs e)
    {

        BaseBO baseBO = new BaseBO();
        #region
        //楼层内码
        //cmbFloorID.Items.Clear();
        //baseBO.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID + " and BuildingID=" +cmbBuildingID.SelectedValue;
        //Resultset rs = baseBO.Query(new Floors());
        //foreach (Floors floors in rs)
        //{
        //    cmbFloorID.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
        //}
        //showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        #endregion
    }
    protected void cmbFloorID_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region
        ////方位内码
        //BaseBO baseBO = new BaseBO();
        //cmbLocationID.Items.Clear();
        //baseBO.WhereClause = "LocationStatus=" + Location.LOCATION_STATUS_VALID + " and FloorID=" + cmbFloorID.SelectedValue;
        //Resultset rs = baseBO.Query(new Location());
        //foreach (Location locations in rs)
        //{
        //    cmbLocationID.Items.Add(new ListItem(locations.LocationName, locations.LocationID.ToString()));
        //}
        //showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        #endregion
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //int intStore = Convert.ToInt32 ( ViewState["StoreID"].ToString());
        if (ViewState["StoreID"] != null && ViewState["StoreID"].ToString() != "")
        {
            textopen();
            ViewState["Flag"] = OPR_ADD;
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "AddUnits_NoStoreID") + "'", true);
            //showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
            //btnCancel_Click();
        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        this.btnEdit.Enabled = true;
        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        Units units = new Units();
        SelUnit selunit = new SelUnit();
        SelLocation sellocation = new SelLocation();
        DataSet dt = new DataSet();
        string unitid = "";
        //unitid:项目三位+大楼三位+楼层三位+方位三位+单元大于三位
        unitid = deptid.Value;

        ViewState["UnitID"] = deptid.Value;
        if (rbtnNoLeaseOut.Checked)
        {
            if (unitid.Length < 12)
            {
                this.btnAdd.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnSave.Enabled = false;
            }
            else if (unitid.Length == 12)//方位
            {
                textlock();
               // baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                rs = baseBO.Query(sellocation);
                if (rs.Count == 1)
                {
                    sellocation = rs.Dequeue() as SelLocation;
                    /*经营区域*/
                    BaseBO basebo = new BaseBO();
                    Resultset rs1 = new Resultset();
                    basebo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID + " and StoreID=" + sellocation.StoreID;
                    rs1 = basebo.Query(new Area());
                    cmbTradeRelation.Items.Clear();
                    foreach (Area area in rs1)
                    {
                        cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
                    }
                    ViewState["BuildingID"] = deptid.Value;
                    cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                    cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                    cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    ViewState["StoreID"] = sellocation.StoreID.ToString();
                }
                this.btnAdd.Enabled = true;
                this.btnSave.Enabled = false;
                this.btnEdit.Enabled = false;
            }
            else if (unitid.Length > 12)
            {
                //仅取后三位是不能保证是unitid的,默认13位以后的是unitid
                baseBO.WhereClause = "UnitID=" + unitid.Substring(12);
                rs = baseBO.Query(units);
                //dt=baseBO.QueryDataSet(new Units());
                if (rs.Count == 1)
                {
                    units = rs.Dequeue() as Units;
                    ViewState["StoreID"] = units.StoreID;
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
                        /*经营区域*/
                        BaseBO basebo = new BaseBO();
                        Resultset rs1 = new Resultset();
                        basebo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID + " and StoreID=" + sellocation.StoreID;
                        rs1 = basebo.Query(new Area());
                        cmbTradeRelation.Items.Clear();
                        foreach (Area area in rs1)
                        {
                            cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
                        }
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();                        
                    }
                    try { cmbTradeRelation.SelectedValue = units.AreaID.ToString(); }
                    catch { }
                    try { cmbTradeID.SelectedValue = units.Trade2ID.ToString(); } //units.Trade2ID.ToString(); }
                    catch { }

                    txtFloorArea.Text = units.FloorArea.ToString();
                    txtUseArea.Text = units.UseArea.ToString();
                    cmbUnitStatus.SelectedValue = units.UnitStatus.ToString();
                    txtNode.Text = units.Note;
                    try
                    { this.ddlUnitType.SelectedValue = units.UnitTypeID.ToString();}
                    catch {  }
                }
                btnSave.Enabled = false;
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;

            }
            else
            {
                /*提示信息-请选择方位信息*/
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + mesLocation + "'", true);
                txtUnitCode.Text = "";
                txtNode.Text = "";
                txtShopName.Text = "";
                txtFloorArea.Text = "";
                txtUseArea.Text = "";
            }
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            /*控件锁定*/
            txtFloorArea.ReadOnly = true;
            txtFloorArea.CssClass = "Enabledipt150px";
            txtNode.ReadOnly = true;
            txtNode.CssClass = "Enabledipt150px";
            this.cmbTradeID.Enabled = false;
            txtUnitCode.ReadOnly = true;
            txtUnitCode.CssClass = "Enabledipt150px";
            txtShopName.CssClass = "Enabledipt150px";
            txtUseArea.ReadOnly = true;
            txtUseArea.CssClass = "Enabledipt150px";
            cmbRentLevel.Enabled = false;
            cmbTradeRelation.Enabled = false;
            cmbUnitStatus.Enabled = false;
            this.ddlUnitType.Enabled = false;
            this.ddlShopType.Enabled = false;
        }
        else if (rbtnLeaseOut.Checked)    //已出租单元 不能添加，只能修改
        {
                if (unitid.Length < 12)
                {
                    this.btnAdd.Enabled = false;
                    this.btnEdit.Enabled = false;
                    this.btnSave.Enabled = false;
                }
                else if (unitid.Length == 12)
                {
                    textlock();
                    ViewState["BuildingID"] = deptid.Value;
                    baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                    baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                    rs = baseBO.Query(sellocation);
                    if (rs.Count == 1)
                    {
                        sellocation = rs.Dequeue() as SelLocation;
                        /*经营区域*/
                        BaseBO basebo = new BaseBO();
                        Resultset rs1 = new Resultset();
                        basebo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID + " and StoreID=" + sellocation.StoreID;
                        rs1 = basebo.Query(new Area());
                        cmbTradeRelation.Items.Clear();
                        foreach (Area area in rs1)
                        {
                            cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
                        }
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    }
                    this.btnAdd.Enabled = false;
                    this.btnEdit.Enabled = false;
                    this.btnSave.Enabled = false;
                }
                else if (unitid.Length > 12)
                {
                   // baseBO.WhereClause = "a.UnitID=" + unitid.Substring(unitid.Length - 3) + " and b.RentStatus=" + SelUnit.RENT_ATUS_VALID + " and a.UnitID=b.UnitID and b.ShopID=c.ShopID";
                    // 在已签订合同，但是合同没有审批的情况下，ConShopUnit。RentStatus=null导致不能查询到该商铺
                    baseBO.WhereClause = "a.UnitID=" + unitid.Substring(12) + " and a.UnitID=b.UnitID and b.ShopID=c.ShopID";
                    rs = baseBO.Query(new SelUnit());
                    dt = baseBO.QueryDataSet(new SelUnit());
                    
                    if (rs.Count == 1)
                    {
                        selunit = rs.Dequeue() as SelUnit;
                        txtUnitCode.Text = selunit.UnitCode;
                        ViewState["StoreID"] = units.StoreID;
                        baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(9, 3);
                        baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                        rs = baseBO.Query(sellocation);
                        if (rs.Count == 1)
                        {
                            sellocation = rs.Dequeue() as SelLocation;
                            /*经营区域*/
                            BaseBO basebo = new BaseBO();
                            Resultset rs1 = new Resultset();
                            basebo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID + " and StoreID=" + sellocation.StoreID;
                            rs1 = basebo.Query(new Area());
                            cmbTradeRelation.Items.Clear();
                            foreach (Area area in rs1)
                            {
                                cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
                            }
                            cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                            cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                            cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                        }

                        try { cmbTradeRelation.SelectedValue = selunit.AreaID.ToString(); }   //cmbTradeRelation 经营区域
                        catch { }
                        try { cmbTradeID.SelectedValue = selunit.Trade2ID.ToString(); }//dt.Tables[0].Rows[0]["Trade2ID"].ToString();
                        catch { }
                        try { cmbRentLevel.SelectedValue = selunit.AreaLevelID.ToString(); }
                        catch { }
                        try { this.ddlShopType.SelectedValue = selunit.ShopTypeID.ToString(); }
                        catch { }
                        txtFloorArea.Text = selunit.FloorArea.ToString(); 
                        txtUseArea.Text = selunit.UseArea.ToString(); 
                        //try { cmbUnitStatus.Items.Add(new ListItem(SelUnit.BLANKOUT_STATUS_LEASEOUTNAME, SelUnit.BLANKOUT_STATUS_LEASEOUT.ToString())); }
                        //catch { }
                        try { cmbUnitStatus.SelectedValue = SelUnit.BLANKOUT_STATUS_LEASEOUT.ToString(); }
                        catch { }
                        txtNode.Text = selunit.Note;
                        txtShopName.Text = selunit.ShopName;
                        this.btnEdit.Enabled = true;
                        this.btnAdd.Enabled = false;
                        this.btnSave.Enabled = false;
                    }
                }
                else
                {
                    /*提示信息-请选择方位信息*/
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + mesLocation + "'", true);
                    txtUnitCode.Text = "";
                    txtNode.Text = "";
                    txtShopName.Text = "";
                    txtFloorArea.Text = "";
                    txtUseArea.Text = "";
                }
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_LEASEOUT);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            /*控件锁定*/
            txtFloorArea.ReadOnly = true;
            txtFloorArea.CssClass = "Enabledipt150px";
            txtNode.ReadOnly = true;
            txtNode.CssClass = "Enabledipt150px";
            this.cmbTradeID.Enabled = false;
            txtUnitCode.ReadOnly = true;
            txtUnitCode.CssClass = "Enabledipt150px";
            txtShopName.CssClass = "Enabledipt150px";
            txtUseArea.ReadOnly = true;
            txtUseArea.CssClass = "Enabledipt150px";
            cmbRentLevel.Enabled = false;
            cmbTradeRelation.Enabled = false;
            cmbUnitStatus.Enabled = false;
            this.ddlUnitType.Enabled = false;
            this.ddlShopType.Enabled = false;
        }
        else if (rbtnBlankOut.Checked)    //已作废单元:不能新增单元
        {
            if (unitid.Length < 12)
            {
                this.btnAdd.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnSave.Enabled = false;
            }
            else if (unitid.Length == 12)
            {
                textlock();
                ViewState["BuildingID"] = deptid.Value;
                baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(unitid.Length - 3);
                baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                 rs = baseBO.Query(sellocation);
                if (rs.Count == 1)
                {
                    sellocation = rs.Dequeue() as SelLocation;
                    /*经营区域*/
                    BaseBO basebo = new BaseBO();
                    Resultset rs1 = new Resultset();
                    basebo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID + " and StoreID=" + sellocation.StoreID;
                    rs1 = basebo.Query(new Area());
                    cmbTradeRelation.Items.Clear();
                    foreach (Area area in rs1)
                    {
                        cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
                    }
                    cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                    cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                    cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                }
                this.btnAdd.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnSave.Enabled = false;
            }
            else if (unitid.Length > 12)
            {

                DataSet  dtUnit = baseBO.QueryDataSet("Select Unit.* From Unit where UnitStatus=" + Units.BLANKOUT_STATUS_VALID + " and UnitId=" + unitid.Substring(12) + "");
                if (dtUnit.Tables[0].Rows.Count == 1)
                {
                    txtUnitCode.Text = dtUnit.Tables[0].Rows[0]["UnitCode"].ToString();
                    ViewState["StoreID"] = dtUnit.Tables[0].Rows[0]["StoreID"].ToString(); 
                    try { cmbTradeID.SelectedValue = dtUnit.Tables[0].Rows[0]["Trade2ID"].ToString(); }
                    catch { }
                    try { cmbBuildingID.SelectedValue = dtUnit.Tables[0].Rows[0]["BuildingID"].ToString(); }
                    catch { }
                    try { cmbFloorID.SelectedValue = dtUnit.Tables[0].Rows[0]["FloorID"].ToString(); }
                    catch { }
                    try { cmbLocationID.SelectedValue = dtUnit.Tables[0].Rows[0]["LocationID"].ToString(); }
                    catch { }
                    try { cmbTradeRelation.SelectedValue = dtUnit.Tables[0].Rows[0]["AreaID"].ToString(); }
                    catch { }
                    try { ddlUnitType.SelectedValue = dtUnit.Tables[0].Rows[0]["UnitTypeID"].ToString(); }
                    catch { }
                    try { ddlShopType.SelectedValue = dtUnit.Tables[0].Rows[0]["ShopTypeID"].ToString(); }//商铺类型
                    catch { }
                    txtFloorArea.Text = dtUnit.Tables[0].Rows[0]["FloorArea"].ToString();
                    txtUseArea.Text = dtUnit.Tables[0].Rows[0]["UseArea"].ToString();
                    txtNode.Text = dtUnit.Tables[0].Rows[0]["Note"].ToString(); ;
                    try { this.cmbUnitStatus.SelectedValue = dtUnit.Tables[0].Rows[0]["UnitStatus"].ToString(); ; }
                    catch { }
                    this.btnEdit.Enabled = true;
                    this.btnAdd.Enabled = false;
                    this.btnSave.Enabled = false;
                }
                else
                {
                    baseBO.WhereClause = "b.floorid=c.floorid and a.buildingid=b.buildingid and c.locationid=" + unitid.Substring(9, 3);
                    baseBO.GroupBy = "a.BuildingID,b.FloorID,LocationID,BuildingCode,BuildingName,FloorCode,FloorName,LocationCode,LocationName,BuildingStatus,FloorStatus,LocationStatus,c.StoreID";
                    rs = baseBO.Query(sellocation);
                    if (rs.Count == 1)
                    {
                        sellocation = rs.Dequeue() as SelLocation;
                        /*经营区域*/
                        BaseBO basebo = new BaseBO();
                        Resultset rs1 = new Resultset();
                        basebo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID + " and StoreID=" + sellocation.StoreID;
                        rs1 = basebo.Query(new Area());
                        cmbTradeRelation.Items.Clear();
                        foreach (Area area in rs1)
                        {
                            cmbTradeRelation.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
                        }
                        cmbBuildingID.SelectedValue = sellocation.BuildingID.ToString();
                        cmbFloorID.SelectedValue = sellocation.FloorID.ToString();
                        cmbLocationID.SelectedValue = sellocation.LocationID.ToString();
                    }
                    this.btnAdd.Enabled = false;
                    this.btnEdit.Enabled = false;
                    this.btnSave.Enabled = false;
                }
            }
            else
            {
                /*提示信息-请选择方位信息*/
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + mesLocation + "'", true);
                txtUnitCode.Text = "";
                txtNode.Text = "";
                txtShopName.Text = "";
                txtFloorArea.Text = "";
                txtUseArea.Text = "";
            }
            showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_VALID);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            /*控件锁定*/
            txtFloorArea.ReadOnly = true;
            txtFloorArea.CssClass = "Enabledipt150px";
            txtNode.ReadOnly = true;
            txtNode.CssClass = "Enabledipt150px";
            this.cmbTradeID.Enabled = false;
            txtUnitCode.ReadOnly = true;
            txtUnitCode.CssClass = "Enabledipt150px";
            txtShopName.CssClass = "Enabledipt150px";
            txtUseArea.ReadOnly = true;
            txtUseArea.CssClass = "Enabledipt150px";
           // cmbRentLevel.Enabled = false;
            cmbTradeRelation.Enabled = false;
            cmbUnitStatus.Enabled = false;
            this.ddlUnitType.Enabled = false;
            this.ddlShopType.Enabled = false;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "clear", "ClearInfo()", true);
    }
        
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        txtFloorArea.ReadOnly = false;
        txtFloorArea.CssClass = "ipt150px";
        txtUseArea.ReadOnly = false;
        txtUseArea.CssClass = "ipt150px";
        txtNode.ReadOnly = false;
        txtNode.CssClass = "ipt150px";
        cmbTradeRelation.Enabled = true;
        //已出租单元不能修改状态
        if (Convert.ToInt32(cmbUnitStatus.SelectedValue) == Units.BLANKOUT_STATUS_INVALID || Convert.ToInt32(cmbUnitStatus.SelectedValue) == Units.BLANKOUT_STATUS_VALID )
        {
            cmbUnitStatus.Enabled = true;
            
        }
        else
        {
            cmbUnitStatus.Enabled = false ;
        }

        //cmbRentLevel.Enabled = true;
        this.ddlUnitType.Enabled = true;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        this.cmbTradeID.Enabled = true;
        this.ddlShopType.Enabled = true;
        ViewState["Flag"] = OPR_EDIT;
        this.rbtnNoLeaseOut.Checked = true;
        this.rbtnLeaseOut.Checked = false;
        this.rbtnBlankOut.Checked = false;
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        if (txtUnitCode.Text == "")
        {
            /*单元编码不能为空  */
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '单元编码不能空'", true);
            return;
        }
        BaseBO baseBO = new BaseBO();
        Units units = new Units();
        int oprFlag = Convert.ToInt32(ViewState["Flag"]);
        if (Convert.ToString(oprFlag) == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ExistUnitCodeError") + "'", true);
            return;
        }

        if (OPR_ADD == oprFlag)
        {
            baseBO.WhereClause = " UnitCode = '" + txtUnitCode.Text + "' And UnitStatus <> " + Units.BLANKOUT_STATUS_VALID + " And StoreID=" + ViewState["StoreID"].ToString();
            Resultset rs = baseBO.Query(units);
            baseBO.WhereClause = "";
            if (rs.Count > 0)
            {
                /*提示已存在该单元编码*/
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ExistUnitCode") + "'", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                return;
            }

            units.UnitID = BaseApp.GetUnitID();
            units.UnitCode = txtUnitCode.Text;
            try { units.BuildingID = Convert.ToInt32(cmbBuildingID.SelectedValue); }
            catch { units.BuildingID = 0; }
            try { units.FloorID = Convert.ToInt32(cmbFloorID.SelectedValue); }
            catch { units.FloorID = 0; }
            try { units.LocationID = Convert.ToInt32(cmbLocationID.SelectedValue); }
            catch { units.LocationID = 0; }
            try { units.AreaID = Convert.ToInt32(cmbTradeRelation.SelectedValue); }
            catch { units.AreaID = 0; }
            try { units.AreaLevelID = Convert.ToInt32(cmbRentLevel.SelectedValue); }
            catch { units.AreaLevelID = 0; }
            try { units.FloorArea = Convert.ToDecimal(txtFloorArea.Text); }
            catch { units.FloorArea = 0; }
            try { units.UseArea = Convert.ToDecimal(txtUseArea.Text); }
            catch { units.UseArea = 0; }
           // try { units.UnitStatus = Convert.ToInt32(cmbUnitStatus.SelectedValue); }
            units.UnitStatus = 0; //新增单元默认未出租
            units.Note = txtNode.Text.Trim();
            try { units.Trade2ID = Convert.ToInt32(cmbTradeID.SelectedValue); }
            catch { units.Trade2ID = 0; }
            //
            try { units.UnitTypeID = Convert.ToInt32(this.ddlUnitType.SelectedValue); }
            catch {  }
            units.OprDeptID = sessionUser.DeptID;
            units.OprRoleID = sessionUser.RoleID;
            units.CreateUserID = sessionUser.UserID;
            try { units.ShopTypeID = Int32.Parse(this.ddlShopType.SelectedValue); }
            catch{units.ShopTypeID=0;}
            if (ViewState["StoreID"] != null && ViewState["StoreID"].ToString() != "")
                units.StoreID = Int32.Parse(ViewState["StoreID"].ToString());
            if (baseBO.Insert(units) != -1)
            {
                /*提示添加成功信息*/
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + mesAdd + "'", true);
            }
        }
        else if (OPR_EDIT == oprFlag)
        {
            if (Convert.ToInt32(cmbUnitStatus.SelectedValue) == Units.BLANKOUT_STATUS_LEASEOUT)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "AddUnits_UnitStatusIsErr") + "'", true);
            }
            else
            {

                try { units.AreaID = Convert.ToInt32(cmbTradeRelation.SelectedValue); }
                catch { units.AreaID = 0; }
                try { units.AreaLevelID = Convert.ToInt32(cmbRentLevel.SelectedValue); }
                catch { units.AreaLevelID = 0; }
                try { units.FloorArea = Convert.ToDecimal(txtFloorArea.Text); }
                catch { units.FloorArea = 0; }
                try { units.UseArea = Convert.ToDecimal(txtUseArea.Text); }
                catch { units.UseArea = 0; }
                try { units.UnitStatus = Convert.ToInt32(cmbUnitStatus.SelectedValue); }
                catch { units.UnitStatus = 0; }
                try { units.Trade2ID = Convert.ToInt32(cmbTradeID.SelectedValue); }
                catch { units.Trade2ID = 0; }
                try { units.UnitTypeID = Int32.Parse(this.ddlUnitType.SelectedValue); }
                catch { }

                units.Note = txtNode.Text.Trim();
                units.ModifyUserID = sessionUser.UserID;
                units.OprDeptID = sessionUser.DeptID;
                units.OprRoleID = sessionUser.RoleID;
                try { units.ShopTypeID=Int32.Parse(this.ddlShopType.SelectedValue);}
                catch{units.ShopTypeID=0;}
                if (ViewState["StoreID"] != null && ViewState["StoreID"].ToString() != "")
                    units.StoreID = Int32.Parse(ViewState["StoreID"].ToString());
                baseBO.WhereClause = "UnitID=" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3);
                if (baseBO.Update(units) != -1)
                {
                    /*提示添加成功信息*/
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + mesAdd + "'", true);
                }
            }
        }
        ViewState["StoreID"] = "";
        textlock();
        this.rbtnNoLeaseOut.Checked = true;
        this.rbtnLeaseOut.Checked = false;
        this.rbtnBlankOut.Checked = false;
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        this.btnEdit.Enabled = false;
        btnAdd.Enabled = false;
    }
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
                        jsdept += store.DeptID.ToString()+building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";

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
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" +store.DeptID.ToString()+ floors.BuildingID.ToString() + "|" + floors.FloorName.ToString() + "^";

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
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnSave.Enabled = false;
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_LEASEOUT);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void textopen()
    {
        txtFloorArea.Text = "";
        txtFloorArea.ReadOnly = false;
        txtFloorArea.CssClass = "ipt150px";
        txtNode.Text = "";
        txtNode.ReadOnly = false;
        txtNode.CssClass = "ipt150px";
        txtUnitCode.Text = "";
        txtUnitCode.ReadOnly = false;
        txtUnitCode.CssClass = "ipt150px";
        txtUseArea.Text = "";
        txtUseArea.ReadOnly = false;
        txtUseArea.CssClass = "ipt150px";
        cmbRentLevel.Enabled = true;
        //cmbBuildingID.Enabled = true;

        //cmbFloorID.Enabled = true;

        //cmbLocationID.Enabled = true;

        cmbTradeRelation.Enabled = true;
        this.ddlUnitType.Enabled = true;
        this.ddlShopType.Enabled = true;
        cmbTradeID.Enabled = true;
        cmbUnitStatus.Enabled = true;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
    }
    private void textlock()
    {
        txtFloorArea.Text = "";
        txtFloorArea.ReadOnly = true;
        txtFloorArea.CssClass = "Enabledipt150px";
        txtNode.Text = "";
        txtNode.ReadOnly = true;
        txtNode.CssClass = "Enabledipt150px";
        txtUnitCode.Text = "";
        txtUnitCode.ReadOnly = true;
        txtUnitCode.CssClass = "Enabledipt150px";
        txtShopName.Text = "";
        txtShopName.CssClass = "Enabledipt150px";
        txtUseArea.Text = "";
        txtUseArea.ReadOnly = true;
        txtUseArea.CssClass = "Enabledipt150px";
        cmbRentLevel.Enabled = false;

        //cmbBuildingID.Enabled = false;

        //cmbFloorID.Enabled = false;

        //cmbLocationID.Enabled = false;

        cmbTradeRelation.Enabled = false;
        cmbTradeID.Enabled = false;
        cmbUnitStatus.Enabled = false;
        this.ddlUnitType.Enabled = false;
        this.ddlShopType.Enabled = false;
        btnSave.Enabled = false;
        btnAdd.Enabled = true;
        btnEdit.Enabled = true;

    }
    protected void rbtnNoLeaseOut_CheckedChanged(object sender, EventArgs e)
    {
        textlock();
        cmbUnitStatus.Items.Clear();
        int[] IsBlankOutStatus = Units.GetBlankOutStatus();
        for (int i = 0; i < IsBlankOutStatus.Length; i++)
        {
            this.cmbUnitStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Units.GetBlankOutStatusDesc(IsBlankOutStatus[i])), IsBlankOutStatus[i].ToString()));
        }
        btnAdd.Enabled = false;
        btnEdit.Enabled = false ;
        selectdeptid.Value = "";
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_INVALID);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void rbtnBlankOut_CheckedChanged(object sender, EventArgs e)
    {
        textlock();
        selectdeptid.Value = "";
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnSave.Enabled = false;
        showtreenode(" and UnitStatus=" + Units.BLANKOUT_STATUS_VALID);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void cmbLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RentableArea/Building/AddUnit.aspx");
    }
}
