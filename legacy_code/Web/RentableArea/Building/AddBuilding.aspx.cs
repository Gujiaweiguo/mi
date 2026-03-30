using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.Dept;
using Base;
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
public partial class RentableArea_Building_AddBuilding : BasePage
{

    /**
   * 存放在session里,用来标志当前的操作
   */
    private static int OPR_ADD = 1;
    private static int OPR_EDIT = 2;
    public string baseInfo;
    public string strFresh;
    public string mallTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        txtBuildingCode.Attributes.Add("onblur", "TextIsNotNull(txtBuildingCode,hidMessage.value)");
        txtBuildingName.Attributes.Add("onblur", "TextIsNotNull(txtBuildingName,hidMessage.value)");
        #region
        //btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
        btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");
        mallTitle = (String)GetGlobalResourceObject("BaseInfo", "Dept_MallAdd");
        showtreenode();
        if (!IsPostBack)
        {
            int[] statusBuilding = Building.GetBuildingStatus();
            for (int i = 0; i < statusBuilding.Length; i++)
            {
                cmbBuildingStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Building.GetBuildingStatusDesc(statusBuilding[i])), statusBuilding[i].ToString()));
            }

            int[] statusFloors = Floors.GetFloorStatus();
            for (int i = 0; i < statusFloors.Length; i++)
            {
                cmbFloorStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Floors.GetFloorStatusDesc(statusFloors[i])), statusFloors[i].ToString()));
            }

            int[] statusLocation = Location.GetLocationStatus();
            for (int i = 0; i < statusLocation.Length; i++)
            {
                this.cmbLocationStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Location.GetLocationStatusDesc(statusLocation[i])), statusLocation[i].ToString()));
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblBuildingtit");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        btnAdd.Enabled = false;
        btnSave.Enabled = true;
        BaseBO baseBO = new BaseBO();
        Dept dept = new Dept();
        String oprFlag = Convert.ToString(ViewState["BuildingID"]);
        if (oprFlag == null || Convert.ToString(oprFlag) == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '请先选择部门。'", true);
            return;
        }
        if (ViewState["BuildingID"].ToString().Length == 6)
        {
            baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL + "and DeptID=" + ViewState["BuildingID"].ToString().Substring(3,3);
            Resultset rs = baseBO.Query(dept);
            if (rs.Count != 1)
            {
                txtBuildingCode.ReadOnly = false;
                txtBuildingName.ReadOnly = false;
                cmbBuildingStatus.Enabled = true;
            }
            else
            {
                /*需要提示信息-不是有效大楼*/
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidNoBuilding.Value + "'", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                return;
            }
        }
        else if (ViewState["BuildingID"].ToString().Length == 9)
        {
            txtFloorCode.ReadOnly = false;
            txtFloorName.ReadOnly = false;
            cmbFloorStatus.Enabled = true;
        }
        else if (ViewState["BuildingID"].ToString().Length > 9)
        {
            txtLocationCode.ReadOnly = false;
            txtLocationName.ReadOnly = false;
            cmbLocationStatus.Enabled = true;
        }
        ViewState["Flag"] = OPR_EDIT;
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RentableArea/Building/AddBuilding.aspx");
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        BaseTrans trans = new BaseTrans();
        Building buildingInfo = new Building();
        Floors floorInfo = new Floors();
        Location locationInfo = new Location();
        BaseBO baseBO = new BaseBO();

        int oprFlag = Convert.ToInt32(ViewState["Flag"]);
        if (Convert.ToString(oprFlag) == "")
        {
            return;
        }
        if (OPR_ADD == oprFlag)
        {
            if (ViewState["BuildingID"].ToString().Length == 6 || ViewState["BuildingID"].ToString().Length == 3)
            {
                if (ViewState["BuildingID"].ToString().Length == 6)
                    baseBO.WhereClause = "BuildingID=" + ViewState["BuildingID"].ToString().Substring(3,3);
                else
                    baseBO.WhereClause = "BuildingID=0";
                rs = baseBO.Query(buildingInfo);
                if (rs.Count == 1)
                {
                    baseBO.WhereClause = "FloorCode='" + txtFloorCode.Text.Trim() + "' and BuildingID=" + ViewState["BuildingID"].ToString().Substring(3,3);
                    rs = baseBO.Query(floorInfo);
                    if (rs.Count == 1)
                    {
                        /*需要提示信息-已有楼层编码*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidYesFloor.Value + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    trans.BeginTrans();
                    if (txtFloorCode.Text.Trim() != "" && this.txtFloorName.Text.Trim() != "")
                    {
                        floorInfo.FloorID = BaseApp.GetFloorsID();
                        floorInfo.FloorCode = this.txtFloorCode.Text.Trim();
                        floorInfo.FloorName = this.txtFloorName.Text.Trim();
                        floorInfo.BuildingID = Convert.ToInt32(ViewState["BuildingID"].ToString().Substring(3, 3));
                        floorInfo.FloorStatus = Convert.ToInt32(cmbFloorStatus.SelectedValue);

                        try { floorInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                        catch { floorInfo.StoreID = 0; }
                        trans.Insert(floorInfo);
                    }
                    else
                    {
                        /*需要提示信息--楼层不能为空*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidNotNullFloor.Value + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    if (txtLocationCode.Text != "" && txtLocationName.Text != "")
                    {
                        locationInfo.LocationID = BaseApp.GetLocationID();
                        locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
                        locationInfo.LocationName = this.txtLocationName.Text.Trim();
                        locationInfo.FloorID = floorInfo.FloorID;
                        locationInfo.LocationStatus = Convert.ToInt32(cmbBuildingStatus.SelectedValue);
                        try { locationInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                        catch { locationInfo.StoreID = 0; }
                        trans.Insert(locationInfo);
                    }
                    else
                    {
                        /*需要提示信息-方位不能为空*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "方位信息不能为空。" + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    trans.Commit();
                }
                else
                {
                    if (this.txtBuildingCode.Text.Trim() == "" || this.txtBuildingName.Text.Trim() == "")
                    {
                        /*需要提示信息-大楼不能为空*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "大楼信息不能为空。" + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    baseBO.WhereClause = "BuildingCode='" + txtBuildingCode.Text.Trim() + "'";
                    rs = baseBO.Query(buildingInfo);
                    if (rs.Count == 1)
                    {
                        /*需要提示信息-已有大楼编码*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidYesBulidingCode.Value + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    trans.BeginTrans();
                    buildingInfo.BuildingID = BaseApp.GetBuildingID();
                    buildingInfo.BuildingCode = this.txtBuildingCode.Text.Trim();
                    buildingInfo.BuildingName = this.txtBuildingName.Text.Trim();
                    buildingInfo.BuildingStatus = Convert.ToInt32(cmbBuildingStatus.SelectedValue);
                    try { buildingInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                    catch { buildingInfo.StoreID = 0; }
                    trans.Insert(buildingInfo);
                    if (txtFloorCode.Text.Trim() != "" && txtFloorName.Text.Trim() != "")
                    {
                        floorInfo.FloorID = BaseApp.GetFloorsID();
                        floorInfo.FloorCode = this.txtFloorCode.Text.Trim();
                        floorInfo.FloorName = this.txtFloorName.Text.Trim();
                        floorInfo.BuildingID = buildingInfo.BuildingID;
                        floorInfo.FloorStatus = Convert.ToInt32(cmbFloorStatus.SelectedValue);
                        try { floorInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                        catch { floorInfo.StoreID = 0; }
                        trans.Insert(floorInfo);

                        if (txtLocationCode.Text != "" && txtLocationName.Text != "")
                        {
                            locationInfo.LocationID = BaseApp.GetLocationID();
                            locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
                            locationInfo.LocationName = this.txtLocationName.Text.Trim();
                            locationInfo.FloorID = floorInfo.FloorID;
                            locationInfo.LocationStatus = Convert.ToInt32(cmbBuildingStatus.SelectedValue);
                            try { locationInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                            catch { locationInfo.StoreID = 0; }
                            trans.Insert(locationInfo);
                        }
                    }
                    trans.Commit();
                }
            }
            else if (ViewState["BuildingID"].ToString().Length == 9)
            {
                baseBO.WhereClause = "LocationCode ='" + txtLocationCode.Text.Trim() + "' and FloorID=" + ViewState["BuildingID"].ToString().Substring(6, 3);
                rs = baseBO.Query(locationInfo);
                if (rs.Count == 1)
                {
                    /*信息提示-已有方位编码*/
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidYesLocationCode.Value + "'", true);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                    return;
                }
                if (txtLocationCode.Text.Trim() != "" && this.txtLocationName.Text.Trim() != "")
                {
                    locationInfo.LocationID = BaseApp.GetLocationID();
                    locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
                    locationInfo.LocationName = this.txtLocationName.Text.Trim();
                    locationInfo.FloorID = Convert.ToInt32(ViewState["BuildingID"].ToString().Substring(6, 3));
                    locationInfo.LocationStatus = Convert.ToInt32(cmbBuildingStatus.SelectedValue);
                    try { locationInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                    catch { locationInfo.StoreID = 0; }
                    baseBO.Insert(locationInfo);
                }
                else
                {
                    /*需要提示信息-方位编码不能为空*/
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "方位信息不能为空。" + "'", true);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                    return;
                }
            }
            else if (ViewState["BuildingID"].ToString().Length > 9)
            {
                /*信息提示-已有方位编码*/
                baseBO.WhereClause = "LocationCode =" + txtLocationCode.Text.Trim();
                rs = baseBO.Query(locationInfo);
                if (rs.Count == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                    return;
                }
                if (txtLocationCode.Text.Trim() != "" && this.txtLocationName.Text.Trim() != "")
                {
                    locationInfo.LocationID = BaseApp.GetLocationID();
                    locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
                    locationInfo.LocationName = this.txtLocationName.Text.Trim();
                    locationInfo.FloorID = Convert.ToInt32(ViewState["BuildingID"].ToString().Substring(6, 3));
                    locationInfo.LocationStatus = Convert.ToInt32(cmbBuildingStatus.SelectedValue);
                    try { locationInfo.StoreID = Int32.Parse(ViewState["DeptID"].ToString()); }
                    catch { locationInfo.StoreID = 0; }
                    baseBO.Insert(locationInfo);
                }
                else
                {
                    /*需要提示信息-方位编码不能为空*/
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "方位信息不能为空。" + "'", true);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                    return;
                }
            }
        }
        else if (OPR_EDIT == oprFlag)
        {
            if (ViewState["BuildingID"].ToString().Length == 6)
            {
                baseBO.WhereClause = "BuildingID=" + ViewState["BuildingID"].ToString().Substring(3,3);
                rs = baseBO.Query(buildingInfo);
                if (rs.Count == 1)
                {
                    if (Convert.ToInt32(cmbBuildingStatus.SelectedValue) == Building.BUILDING_STATUS_INVALID)/*判断大楼下楼层是否有有效楼层*/
                    {
                        baseBO.WhereClause = "BuildingID=" + ViewState["BuildingID"].ToString().Substring(3,3) + " and FloorStatus=" + Floors.FLOOR_STATUS_VALID;
                        rs = baseBO.Query(floorInfo);
                        if (rs.Count > 1)/*如果有有效楼层则返回*/
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        else/*如果没有有效楼层修改*/
                        {
                            if (this.txtBuildingCode.Text == "" || this.txtBuildingName.Text.Trim() == "")
                            {
                                /*需要提示信息-大楼信息不能为空*/
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "大楼信息不能为空。" + "'", true);
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                                return;
                            }
                            buildingInfo.BuildingCode = this.txtBuildingCode.Text.Trim();
                            buildingInfo.BuildingName = this.txtBuildingName.Text.Trim();
                            buildingInfo.BuildingStatus = Convert.ToInt32(cmbBuildingStatus.SelectedValue);
                            baseBO.WhereClause = "BuildingID=" + ViewState["BuildingID"].ToString().Substring(3,3);
                            if (baseBO.Update(buildingInfo) == -1)
                            {
                                /*提示信息*/
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                                return;
                            }
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
                        }
                    }
                    else/*没有选择无效*/
                    {
                        if (this.txtBuildingCode.Text.Trim() == "" || this.txtBuildingName.Text.Trim() == "")
                        {
                            /*需要提示信息-大楼信息不能为空*/
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "大楼信息不能为空。" + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        buildingInfo.BuildingCode = this.txtBuildingCode.Text.Trim();
                        buildingInfo.BuildingName = this.txtBuildingName.Text.Trim();
                        baseBO.WhereClause = "BuildingID=" + ViewState["BuildingID"].ToString().Substring(3,3);
                        if (baseBO.Update(buildingInfo) == -1)
                        {
                            /*提示信息*/
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                            return;
                        }
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
                    }

                }
            }
            else if (ViewState["BuildingID"].ToString().Length == 9)
            {
                baseBO.WhereClause = "FloorID=" + ViewState["BuildingID"].ToString().Substring(6, 3);
                rs = baseBO.Query(floorInfo);
                if (rs.Count == 1)
                {
                    if (Convert.ToInt32(cmbFloorStatus.SelectedValue) == Floors.FLOOR_STATUS_INVALID)/*判断楼层下是否有有效方位*/
                    {
                        baseBO.WhereClause = "FloorID=" + ViewState["BuildingID"].ToString().Substring(6, 3) + " and FloorStatus=" + Floors.FLOOR_STATUS_INVALID;
                        rs = baseBO.Query(floorInfo);
                        if (rs.Count > 1)/*如果有有效方位则返回*/
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        else/*如果没有有效方位修改*/
                        {
                            if (this.txtFloorCode.Text.Trim() == "" || this.txtFloorName.Text.Trim() == "")
                            {
                                /*需要提示信息-大楼信息不能为空*/
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "楼层信息不能为空。" + "'", true);
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                                return;
                            }
                            floorInfo.FloorCode = txtFloorCode.Text.Trim();
                            floorInfo.FloorName = txtFloorName.Text.Trim();
                            floorInfo.FloorStatus = Convert.ToInt32(cmbFloorStatus.SelectedValue);
                            baseBO.WhereClause = "FloorID=" + ViewState["BuildingID"].ToString().Substring(6, 3);
                            if (baseBO.Update(floorInfo) == -1)
                            {
                                /*提示信息*/
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                                return;
                            }
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
                        }
                    }
                    else/*没有选择无效*/
                    {
                        if (this.txtFloorCode.Text.Trim() == "" || this.txtFloorName.Text.Trim() == "")
                        {
                            /*需要提示信息-楼层信息不能为空*/
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "楼层信息不能为空。" + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        floorInfo.FloorCode = txtFloorCode.Text.Trim();
                        floorInfo.FloorName = txtFloorName.Text.Trim();
                        baseBO.WhereClause = "FloorID=" + ViewState["BuildingID"].ToString().Substring(6, 3);
                        if (baseBO.Update(floorInfo) == -1)
                        {
                            /*提示信息*/
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                            return;
                        }
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
                    }

                }
            }
            else
            {
                baseBO.WhereClause = "LocationID=" + ViewState["BuildingID"].ToString().Substring(ViewState["BuildingID"].ToString().Length - 3);
                rs = baseBO.Query(locationInfo);
                if (rs.Count == 1)
                {
                    if (this.txtLocationCode.Text.Trim() == "" || this.txtLocationName.Text.Trim() == "")
                    {
                        /*需要提示信息-方位信息不能为空*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "方位信息不能为空。" + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
                    locationInfo.LocationName = this.txtLocationName.Text.Trim();
                    locationInfo.LocationStatus = Convert.ToInt32(cmbLocationStatus.SelectedValue);
                    if (baseBO.Update(locationInfo) == -1)
                    {
                        /*提示信息*/
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                        return;
                    }
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
                }
            }

        }
        ViewState["DeptID"] = "";
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        textlock();
    }

    private void textlock()
    {
        txtBuildingCode.Text = "";
        txtBuildingCode.ReadOnly = true;

        txtBuildingName.Text = "";
        txtBuildingName.ReadOnly = true;

        cmbBuildingStatus.Enabled = false;

        txtFloorCode.Text = "";
        txtFloorCode.ReadOnly = true;

        txtFloorName.Text = "";
        txtFloorName.ReadOnly = true;

        cmbFloorStatus.Enabled = false;

        txtLocationCode.Text = "";
        txtLocationCode.ReadOnly = true;

        txtLocationName.Text = "";
        txtLocationName.ReadOnly = true;

        cmbLocationStatus.Enabled = false;

        btnSave.Enabled = false;
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
    }

    private void textopen()
    {
        txtBuildingCode.Text = "";
        txtBuildingCode.ReadOnly = false;

        txtBuildingName.Text = "";
        txtBuildingName.ReadOnly = false;

        txtFloorCode.Text = "";
        txtFloorCode.ReadOnly = false;

        txtFloorName.Text = "";
        txtFloorName.ReadOnly = false;

        txtLocationCode.Text = "";
        txtLocationCode.ReadOnly = false;

        txtLocationName.Text = "";
        txtLocationName.ReadOnly = false;
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
            ViewState["BuildingID"] = deptid.Value;
            baseBO.WhereClause = "BuildingID=" + buildingid.Substring(3,3);
            rs = baseBO.Query(building);
            if (rs.Count == 1)
            {
                building = rs.Dequeue() as Building;
                txtBuildingCode.Text = building.BuildingCode;
                txtBuildingName.Text = building.BuildingName;
                cmbBuildingStatus.SelectedValue = building.BuildingStatus.ToString();
                ViewState["DeptID"] = building.StoreID.ToString();
            }
            btnAdd.Enabled = true;
        }
        else if (buildingid.Length == 9)//楼层
        {
            textlock();
            ViewState["BuildingID"] = deptid.Value;
            baseBO.WhereClause = "a.BuildingID=b.BuildingID and floorid=" + buildingid.Substring(6,3);
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
                ViewState["DeptID"] = selflooes.StoreID.ToString();
                btnAdd.Enabled = true;
            }
        }
        else if (buildingid.Length > 9)//方位
        {
            textlock();
            ViewState["BuildingID"] = deptid.Value;
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
                ViewState["DeptID"] = sellocation.StoreID.ToString();
                btnAdd.Enabled = false;
            }
        }
        else if (buildingid.Length == 3)
        {
            ViewState["DeptID"] = deptid.Value;
            ViewState["BuildingID"] = deptid.Value;
            btnAdd.Enabled = true;
        }
        btnEdit.Enabled = true;
        //baseBOwhere.WhereClause = "BuildingID=" + building.BuildingID;
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
                    foreach (Building building in rs)//大楼
                    {
                        jsdept += store.DeptID.ToString()+building.BuildingID.ToString() + "|" + store.DeptID + "|" + building.BuildingName + "^";

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
                        foreach (floorsAuth floors in rsf)//楼层
                        {
                            if (floors.FloorStatus == Floors.FLOOR_STATUS_INVALID)
                            {
                                jsdept +=store.DeptID.ToString()+ building.BuildingID.ToString() + floors.FloorID.ToString() + "|" +store.DeptID.ToString()+ floors.BuildingID.ToString() + "|" + floors.FloorName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                            }
                            else
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString()+floors.BuildingID.ToString() + "|" + floors.FloorName + "|" + "" + "^";
                            }
                            baseBO.WhereClause = "FloorID=" + floors.FloorID;
                            rsl = baseBO.Query(new Location());

                            foreach (Location location in rsl)//方位
                            {
                                if (location.LocationStatus == Location.LOCATION_STATUS_INVALID)
                                {
                                    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                                }
                                else
                                {
                                    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString()+building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "|" + "" + "^";
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        BaseBO baseBO = new BaseBO();
        Dept dept = new Dept();
        String oprFlag = Convert.ToString(ViewState["BuildingID"]);
        if (oprFlag == null || Convert.ToString(oprFlag) == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '请先选择部门。'", true);
            return;
        }
        if (ViewState["BuildingID"].ToString().Length == 6 || ViewState["BuildingID"].ToString().Length == 3)
        {
            baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL + "and DeptID=" + ViewState["BuildingID"].ToString().Substring(0,3);
            Resultset rs = baseBO.Query(dept);
            if (rs.Count == 1)
            {
                txtBuildingCode.ReadOnly = false;
                txtBuildingName.ReadOnly = false;
                cmbBuildingStatus.Enabled = true;

                txtFloorCode.ReadOnly = false;
                txtFloorName.ReadOnly = false;
                cmbFloorStatus.Enabled = true;

                txtLocationCode.ReadOnly = false;
                txtLocationName.ReadOnly = false;
                cmbLocationStatus.Enabled = true;
                ViewState["DeptID"] = ViewState["BuildingID"].ToString().Substring(0,3);
            }
            else
            {
                txtFloorCode.ReadOnly = false;
                txtFloorName.ReadOnly = false;
                cmbFloorStatus.Enabled = true;

                txtLocationCode.ReadOnly = false;
                txtLocationName.ReadOnly = false;
                cmbLocationStatus.Enabled = true;
            }
        }
        else if (ViewState["BuildingID"].ToString().Length == 9)
        {
            txtLocationCode.ReadOnly = false;
            txtLocationName.ReadOnly = false;
            cmbLocationStatus.Enabled = true;
        }
        else if (ViewState["BuildingID"].ToString().Length > 9)
        {
            txtLocationCode.ReadOnly = false;
            txtLocationName.ReadOnly = false;
            cmbLocationStatus.Enabled = true;
            txtLocationCode.Text = "";
            txtLocationName.Text = "";
        }
        ViewState["Flag"] = OPR_ADD;
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
}
