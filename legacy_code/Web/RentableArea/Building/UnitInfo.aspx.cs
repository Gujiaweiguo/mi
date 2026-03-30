using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.Dept;
using BaseInfo.Func;
using BaseInfo.Role;
using BaseInfo.Store;
using Shop.ShopType;
using BaseInfo.authUser;
using BaseInfo.Dept;
using BaseInfo.Store;
using BaseInfo.User;
using BaseInfo.Role;
using Base;

public partial class RentableArea_Building_UnitInfo : BasePage
{
    private string UnitID = "";
    private string strType = "";//操作类型（Add/Edit）
    private string strBuildingID = "";//大楼
    private string strFloorID = "";//楼层
    private string strLocationID = "";//方位
    private string strStoreID = "";//项目
    protected void Page_Load(object sender, EventArgs e)
    {
        this.UnitID = Request["UnitID"].ToString();
        this.strType = Request["Type"].ToString();
        if (this.UnitID.ToString() != "")
        {
            this.strStoreID = UnitID.Substring(0, 3);//项目
            this.strLocationID = UnitID.Substring(9, 3);// 方位
            this.strFloorID = UnitID.Substring(6, 3);//楼层
            this.strBuildingID = UnitID.Substring(3, 3);//大楼
        }
        if (!this.IsPostBack)
        {
            this.BindBuilding();//绑定大楼
            this.BindFloor();//绑定楼层
            this.BindLocation();//绑定方位
            this.BindArea();//绑定经营区域
            this.BindTrade();//绑定业态
            this.BindShopType();//绑定商铺类型
            this.BindStatus();//绑定单元状态
            this.BindUnitType();//绑定单元类别
            this.AddControlPro();//控件添加属性
            if (this.strType.ToLower() == "browse")
                this.btnSave.Visible = false;
            if (this.strType.ToLower() == "add")
            {
                this.BindDate("0");
                this.ddlBuildingID.SelectedValue = this.strBuildingID.ToString();
                this.ddlFloorID.SelectedValue = this.strFloorID.ToString();
                this.ddlLocationID.SelectedValue = this.strLocationID.ToString();
            }
            else
                this.BindDate(UnitID.Substring(UnitID.Length - 3));
        }
    }
    /// <summary>
    /// 控件添加属性
    /// </summary>
    private void AddControlPro()
    {
        txtFloorArea.Attributes.Add("onkeydown", "TextLeave()");
        txtUseArea.Attributes.Add("onkeydown", "TextLeave()");
        txtUnitCode.Attributes.Add("onblur", "TextIsNotNull(txtUnitCode,ImgUnitCode)");
        txtFloorArea.Attributes.Add("onblur", "TextIsNotNull(txtFloorArea,ImgFloorArea)");
        txtUseArea.Attributes.Add("onblur", "TextIsNotNull(txtUseArea,ImgtxtUseArea)");
    }
    /// <summary>
    /// 绑定单元信息
    /// </summary>
    private void BindDate(string strUnitID)
    {
        BaseBO objBaseBo = new BaseBO();
        string strSql = @"select unit.UnitID,unit.BuildingID,unit.AreaID,unit.FloorID,unit.LocationID,unit.UnitCode,unit.AreaLevelID,unit.FloorArea,unit.UseArea,unit.Note,unit.UnitStatus,unit.Trade2ID,unit.UnitTypeID,unit.StoreID,unit.ShopTypeID,Conshop.ShopName,conshopunit.unitid,conshopunit.shopid from unit left join ConShopUnit on ConShopUnit.UnitID=unit.unitid
        left join conshop on conshop.shopid=ConShopUnit.shopid where 1=1 and unit.UnitID='" + strUnitID + "'";
        DataSet ds = objBaseBo.QueryDataSet(strSql);
        if(ds!=null&&ds.Tables[0].Rows.Count>0)
        {
            txtUnitCode.Text = ds.Tables[0].Rows[0]["UnitCode"].ToString();
            try { this.ddlUnitType.SelectedValue = ds.Tables[0].Rows[0]["UnitTypeID"].ToString(); }
            catch { }
            try { this.ddlBuildingID.SelectedValue = ds.Tables[0].Rows[0]["BuildingID"].ToString(); }
            catch { }
            try { this.ddlFloorID.SelectedValue = ds.Tables[0].Rows[0]["FloorID"].ToString(); }
            catch { }
            try { this.ddlLocationID.SelectedValue = ds.Tables[0].Rows[0]["LocationID"].ToString(); }
            catch { }
            try { this.ddlArea.SelectedValue = ds.Tables[0].Rows[0]["AreaID"].ToString(); }
            catch { }
            try { this.ddlTradeID.SelectedValue = ds.Tables[0].Rows[0]["Trade2ID"].ToString(); }
            catch { }
            try { this.ddlShopType.SelectedValue = ds.Tables[0].Rows[0]["ShopTypeID"].ToString(); }
            catch { }
            this.txtFloorArea.Text = ds.Tables[0].Rows[0]["FloorArea"].ToString();
            this.txtUseArea.Text = ds.Tables[0].Rows[0]["UseArea"].ToString();
            this.txtShopName.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
            try { this.ddlUnitStatus.SelectedValue = ds.Tables[0].Rows[0]["UnitStatus"].ToString(); }
            catch { }
            txtNode.Text = ds.Tables[0].Rows[0]["Note"].ToString();
        }
    }
    /// <summary>
    /// 绑定大楼
    /// </summary>
    private void BindBuilding()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "BuildingStatus=" + Building.BUILDING_STATUS_VALID;
        Resultset rs = objBaseBo.Query(new Building());
        foreach (Building buildings in rs)
        {
            this.ddlBuildingID.Items.Add(new ListItem(buildings.BuildingName, buildings.BuildingID.ToString()));
        }
    }
    /// <summary>
    /// 绑定楼层
    /// </summary>
    private void BindFloor()
    {
        BaseBO objBaseBo = new BaseBO();
        this.ddlFloorID.Items.Clear();
        objBaseBo.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID;
        Resultset rs = objBaseBo.Query(new Floors());
        foreach (Floors floors in rs)
        {
            this.ddlFloorID.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
        }
    }
    /// <summary>
    /// 绑定方位
    /// </summary>
    private void BindLocation()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "LocationStatus=" + Location.LOCATION_STATUS_VALID;
        Resultset rs = objBaseBo.Query(new Location());
        foreach (Location locations in rs)
        {
            this.ddlLocationID.Items.Add(new ListItem(locations.LocationName, locations.LocationID.ToString()));
        }
    }
    /// <summary>
    /// 绑定经营区域
    /// </summary>
    private void BindArea()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "AreaStatus=" + Location.LOCATION_STATUS_VALID;
        Resultset rs = objBaseBo.Query(new Area());
        foreach (Area area in rs)
        {
            this.ddlArea.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
        }    
    }
    /// <summary>
    /// 绑定业态
    /// </summary>
    private void BindTrade()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs = objBaseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs)
        {
            this.ddlTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        }
    }
    /// <summary>
    /// 绑定商铺类型
    /// </summary>
    private void BindShopType()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "ShopTypeStatus =1";
        Resultset rs = objBaseBo.Query(new ShopType());
        foreach (ShopType objShopType in rs)
        {
            this.ddlShopType.Items.Add(new ListItem(objShopType.ShopTypeName, objShopType.ShopTypeID.ToString()));
        }
    }
    /// <summary>
    /// 绑定单元类别
    /// </summary>
    private void BindUnitType()
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet(new UnitTypes());
        this.ddlUnitType.DataValueField = "UnitTypeID";
        this.ddlUnitType.DataTextField = "UnitTypeName";
        this.ddlUnitType.DataSource = ds.Tables[0];
        this.ddlUnitType.DataBind();
    }
    /// <summary>
    /// 绑定单元状态
    /// </summary>
    private void BindStatus()
    {
        int[] IsBlankOutStatus = Units.GetBlankOutStatus();
        for (int i = 0; i < IsBlankOutStatus.Length; i++)
        {
            this.ddlUnitStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Units.GetBlankOutStatusDesc(IsBlankOutStatus[i])), IsBlankOutStatus[i].ToString()));
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (txtUnitCode.Text == "")/*单元编码不能为空*/
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + "单元编码不能空" + "')", true);
            return;
        }
        if (this.txtFloorArea.Text == "")/*建筑面积不能为空*/
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + "建筑面积不能空" + "')", true);
            return;
        }
        if (this.txtUseArea.Text == "")/*出租面积不能为空*/
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + "可出租面积不能空" + "')", true);
            return;
        }
        BaseBO objBaseBo = new BaseBO();
        Units objUnits = new Units();
        if(this.strType.ToString()=="")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ExistUnitCodeError") + "')", true);
            return;
        }
        if(this.strType.ToLower()=="add")
        {
            objBaseBo.WhereClause = " UnitCode = '" + txtUnitCode.Text + "' And UnitStatus <> " + Units.BLANKOUT_STATUS_VALID;
            Resultset rs = objBaseBo.Query(objUnits);
            objBaseBo.WhereClause = "";
            if (rs.Count > 0)/*提示已存在该单元编码*/
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ExistUnitCode") + "')", true);
                return;
            }
            objUnits.UnitID = BaseApp.GetUnitID();
            objUnits.UnitCode = txtUnitCode.Text;
            try { objUnits.BuildingID = Convert.ToInt32(this.ddlBuildingID.SelectedValue); }
            catch { objUnits.BuildingID = 0; }
            try { objUnits.FloorID = Convert.ToInt32(this.ddlFloorID.SelectedValue); }
            catch { objUnits.FloorID = 0; }
            try { objUnits.LocationID = Convert.ToInt32(this.ddlLocationID.SelectedValue); }
            catch { objUnits.LocationID = 0; }
            try { objUnits.AreaID = Convert.ToInt32(this.ddlArea.SelectedValue); }
            catch { objUnits.AreaID = 0; }
            try { objUnits.FloorArea = Convert.ToDecimal(txtFloorArea.Text); }
            catch { objUnits.FloorArea = 0; }
            try { objUnits.UseArea = Convert.ToDecimal(txtUseArea.Text); }
            catch { objUnits.UseArea = 0; }
            objUnits.UnitStatus = 0; //新增单元默认未出租
            objUnits.Note = txtNode.Text.Trim();
            try { objUnits.Trade2ID = Convert.ToInt32(this.ddlTradeID.SelectedValue); }
            catch { objUnits.Trade2ID = 0; }
            try { objUnits.UnitTypeID = Convert.ToInt32(this.ddlUnitType.SelectedValue); }
            catch { }
            objUnits.OprDeptID = sessionUser.DeptID;
            objUnits.OprRoleID = sessionUser.RoleID;
            objUnits.CreateUserID = sessionUser.UserID;
            try { objUnits.ShopTypeID = Int32.Parse(this.ddlShopType.SelectedValue); }
            catch { objUnits.ShopTypeID = 0; }
            objUnits.StoreID = Int32.Parse(this.strStoreID.ToString());
            if (objBaseBo.Insert(objUnits) != -1)/*提示添加成功信息*/
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "')", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "window.close()", true);
            }
        }
        else  if(this.strType.ToLower()=="edit")
        {
            if (Convert.ToInt32(this.ddlUnitStatus.SelectedValue) == Units.BLANKOUT_STATUS_LEASEOUT)//已出租
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "AddUnits_UnitStatusIsErr") + "')", true);
            }
            else
            {
                try { objUnits.AreaID = Convert.ToInt32(this.ddlArea.SelectedValue); }
                catch { objUnits.AreaID = 0; }
                try { objUnits.FloorArea = Convert.ToDecimal(txtFloorArea.Text); }
                catch { objUnits.FloorArea = 0; }
                try { objUnits.UseArea = Convert.ToDecimal(txtUseArea.Text); }
                catch { objUnits.UseArea = 0; }
                try { objUnits.UnitStatus = Convert.ToInt32(this.ddlUnitStatus.SelectedValue); }
                catch { objUnits.UnitStatus = 0; }
                try { objUnits.Trade2ID = Convert.ToInt32(this.ddlTradeID.SelectedValue); }
                catch { objUnits.Trade2ID = 0; }
                try { objUnits.UnitTypeID = Int32.Parse(this.ddlUnitType.SelectedValue); }
                catch { }
                objUnits.Note = txtNode.Text.Trim();
                objUnits.ModifyUserID = sessionUser.UserID;
                objUnits.OprDeptID = sessionUser.DeptID;
                objUnits.OprRoleID = sessionUser.RoleID;
                try { objUnits.ShopTypeID = Int32.Parse(this.ddlShopType.SelectedValue); }
                catch { objUnits.ShopTypeID = 0; }
                objUnits.StoreID = Int32.Parse(this.strStoreID.ToString());
                objBaseBo.WhereClause = "UnitID=" + UnitID.Substring(UnitID.Length - 3);
                if (objBaseBo.Update(objUnits) != -1)/*提示添加成功信息*/
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "')", true);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "window.close()", true);
                }
            }
        }
    }
}
