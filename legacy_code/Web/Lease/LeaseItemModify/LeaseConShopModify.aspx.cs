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

using System.Text.RegularExpressions;
using Base.Biz;
using Base.DB;
using Base;
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using RentableArea;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using BaseInfo.User;
using System.Text;
using Base.Page;
using Shop.ShopType;
using Base.Util;
using Lease.ContractMod;
using System.Drawing;

public partial class Lease_LeaseItemModify_LeaseConShopModify : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    private ConShopMod conShop;
    public string beginEndDate;
    private static int CONSHOPID_NULL = 1;
    private static int CONSHOPID_NOTNULL = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");

            btnShopSave.Attributes.Add("onclick", "return InputValidator(form1)");

            /*绑定商铺类型*/
            BindShopType();

            /*绑定主营品牌*/
            //BindBrand();

            /*绑定大楼*/
            BindBuilding();

            /*绑定经营区名称*/
            BindArea();

            /*获取合同对应的商铺*/
            GetAllShopInfo(1);

            /*获取单元信息*/
            //GetShopUnits();

            /*锁定控件*/
            TextEnabled(false);

            txtShopBrand.Attributes.Add("onclick", "selectShopBrand()");
        }
    }
    protected void btnShopAdd_Click(object sender, EventArgs e)
    {
        BindShopType();
        TextEnabled(true);
        TextClear();
        ViewState["shopFlag"] = "add";
        GetAllShopInfo(0);
    }
    protected void btnShopDel_Click(object sender, EventArgs e)
    {
        UnitsStutas unitsStutas = new UnitsStutas();
        ConShopUnit conShopUnit = new ConShopUnit();

        try
        {
            baseTrans.BeginTrans();
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "ShopID = " + Convert.ToInt32(ViewState["delShopID"]);
            if (baseTrans.Delete(new ConShop()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                baseTrans.Rollback();
                GetAllShopInfo(1);
                return;
            }

            //将删除的单元改成未出租状态


            baseBo.WhereClause = "";
            baseBo.WhereClause = "ShopID = " + ViewState["delShopID"];
            Resultset tempRs = baseBo.Query(conShopUnit);
            if (tempRs.Count > 0)
            {
                foreach (ConShopUnit shopUnit in tempRs)
                {
                    baseTrans.WhereClause = "";
                    baseTrans.WhereClause = "UnitID = " + shopUnit.UnitID;
                    unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_INVALID;
                    if (baseTrans.Update(unitsStutas) == 1 - 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                        GetAllShopInfo(1);
                        return;
                    }
                }
            }

            //先将商铺对应的单元删除


            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "ShopID = " + ViewState["delShopID"];

            if (baseTrans.Delete(conShopUnit) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                baseTrans.Rollback();
                GetAllShopInfo(1);
                return;
            }

            baseTrans.Commit();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

            GetAllShopInfo(1);

            TextEnabled(false);

            TextClear();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加商铺信息错误:", ex);
            baseTrans.Rollback();
        }
    }
    protected void btnShopSave_Click(object sender, EventArgs e)
    {
        try
        {
            baseTrans.BeginTrans();

            if (InsertOrUpdateShopBaseInfo() == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            if (SaveShopUnits() == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

            baseTrans.Commit();
            GetAllShopInfo(0);
            TextClear();
            TextEnabled(false);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加商铺信息错误:", ex);
            baseTrans.Rollback();
        }
    }
    protected void IBtnUnitsDel_Click(object sender, EventArgs e)
    {
        int Index = 0;
        for (int i = 0; i < ListBoxUnits.Items.Count; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            if (ListBoxUnits.Items[Index].Selected == true)
            {
                ListBoxUnits.Items.Remove(item);
            }
            Index++;
        }
        GetAllShopInfo(0);
    }
    protected void IBtnUnitsAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (DDownListUnit.SelectedValue.ToString() != "-1")
            {
                BaseBO basebo = new BaseBO();
                basebo.WhereClause = "UnitID = " + Convert.ToInt32(DDownListUnit.SelectedItem.Value);
                string sql = "select UnitID,UnitCode + ' : ' + cast(UseArea as varchar(50)) as UnitCodeArea from Unit where UnitID = " + Convert.ToInt32(DDownListUnit.SelectedValue.ToString());
                DataSet UnitDS = basebo.QueryDataSet(sql);

                ListBoxUnits.Items.Add(new ListItem(UnitDS.Tables[0].Rows[0]["UnitCodeArea"].ToString(), UnitDS.Tables[0].Rows[0]["UnitID"].ToString()));
                GetAllShopInfo(0);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_SelUserfullInfo") + "'", true);
                GetAllShopInfo(0);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加商铺单元信息错误:", ex);
        }
    }
    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.Cookies["Info"].Values["conID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["conID"]) != 0 && Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == 2)
        {
            int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
            ViewState["delShopID"] = shopId;
            GetShopBaseInfo(shopId);
            GetShopUnits();
        }
        else if (Request.Cookies["Info"].Values["ConOverTimeID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]) != 0 && Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == 1)
        {
            int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[4].Text);
            ViewState["delShopID"] = shopId;
            GetShopModBaseInfo(shopId);
            GetShopUnitsMod();
        }
        GetAllShopInfo(0);
        ViewState["shopFlag"] = "modify";
        TextEnabled(true);
    }
    protected void DDownListBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListBuilding.SelectedValue.ToString() != "-1")
        {
            DDownListFloors.Enabled = true;
            int builder = Convert.ToInt32(DDownListBuilding.SelectedValue);
            BindFollrs(builder);
        }
        else
        {
            DDownListFloors.Enabled = false;
            DDownListLocation.Enabled = false;
            DDownListUnit.Enabled = false;
            BindFollrs(-1);
            BindLocation(-1);
            BindUnits(-1);
        }
        GetAllShopInfo(0);
    }
    protected void DDownListFloors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListFloors.SelectedValue.ToString() != "-1")
        {
            DDownListLocation.Enabled = true;
            int floorID = Convert.ToInt32(DDownListFloors.SelectedValue);
            BindLocation(floorID);
        }
        else
        {
            DDownListLocation.Enabled = false;
            DDownListUnit.Enabled = false;
            BindLocation(-1);
            BindUnits(-1);
        }
        GetAllShopInfo(0);
    }
    protected void DDownListLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListLocation.SelectedValue.ToString() != "-1")
        {
            DDownListUnit.Enabled = true;
            int units = Convert.ToInt32(DDownListLocation.SelectedValue);
            BindUnits(units);
        }
        else
        {
            DDownListUnit.Enabled = false;
            BindUnits(-1);
        }
        GetAllShopInfo(0);
    }

    #region 初始化DropDownList
    //绑定商铺类型
    protected void BindShopType()
    {
        rs = baseBo.Query(new ShopType());
        DDownListShopType.Items.Clear();
        foreach (ShopType shopType in rs)
        {
            DDownListShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
        }
    }

    //绑定主营品牌
    //protected void BindBrand()
    //{
    //    string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
    //    baseBo.WhereClause = "";
    //    rs = baseBo.Query(new ConShopBrand());
    //    DDownListBrand.Items.Add(new ListItem(selected, "-1"));
    //    foreach (ConShopBrand shopBrand in rs)
    //        DDownListBrand.Items.Add(new ListItem(shopBrand.BrandName, shopBrand.BrandId.ToString()));
    //}

    //绑定大楼
    protected void BindBuilding()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        rs = baseBo.Query(new Building());
        DDownListBuilding.Items.Add(new ListItem(selected, "-1"));
        foreach (Building building in rs)
            DDownListBuilding.Items.Add(new ListItem(building.BuildingName, building.BuildingID.ToString()));
    }

    //绑定楼层名称
    protected void BindFollrs(int bulid)
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListFloors.Items.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "BuildingID = " + bulid;
        rs = baseBo.Query(new Floors());
        DDownListFloors.Items.Add(new ListItem(selected, "-1"));
        foreach (Floors floors in rs)
            DDownListFloors.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
    }

    //绑定方位名称
    protected void BindLocation(int floor)
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListLocation.Items.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FloorID = " + floor;
        rs = baseBo.Query(new Location());
        DDownListLocation.Items.Add(new ListItem(selected, "-1"));
        foreach (Location loca in rs)
            DDownListLocation.Items.Add(new ListItem(loca.LocationName, loca.LocationID.ToString()));

    }

    /*绑定单元*/
    protected void BindUnits(int tempUnits)
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListUnit.Items.Clear();
        baseBo.OrderBy = "";
        baseBo.WhereClause = "UnitStatus = " + Units.BLANKOUT_STATUS_INVALID + " and LocationID = " + tempUnits;
        rs = baseBo.Query(new Units());
        DDownListUnit.Items.Add(new ListItem(selected, "-1"));
        foreach (Units units in rs)
            DDownListUnit.Items.Add(new ListItem(units.UnitCode, units.UnitID.ToString()));

    }

    /*绑定经营区名称*/
    protected void BindArea()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        rs = baseBo.Query(new Area());
        DDownListAreaName.Items.Add(new ListItem(selected, "-1"));
        foreach (Area area in rs)
            DDownListAreaName.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
    }
    #endregion

    #region 获取合同对应的商铺


    private void GetAllShopInfo(int FirstStatus)
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        DataSet shopDs = new DataSet();
        DataTable shopDt = new DataTable();

        if (Request.Cookies["Info"].Values["conID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["conID"]) != 0 && Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == 2)
        {

            baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            shopDs = baseBo.QueryDataSet(new ConShop()); 
            shopDt = shopDs.Tables[0];
            ViewState["ContractID"] = Convert.ToInt32(shopDt.Rows[0]["ContractID"]);
        }
        else if (Request.Cookies["Info"].Values["ConOverTimeID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]) != 0 && Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == 1)
        {
            /*如果为商铺修改信息则绑定ShopModID字段到GridView*/
            BoundField boundField = new BoundField();
            boundField.DataField = "ShopModID";
            boundField.ItemStyle.CssClass = "hidden";
            boundField.HeaderStyle.CssClass = "hidden";
            boundField.FooterStyle.CssClass = "hidden";
            gvShop.Columns.Add(boundField);

            baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and ConModID = " + Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            shopDs = baseBo.QueryDataSet(new ConShopMod());
            shopDt = shopDs.Tables[0];
            FirstStatus = 2;
        }
        else
        {
            return;
        }

        int shopCount = shopDt.Rows.Count;

        decimal ss = 0;
        for (int j = 0; j < shopCount; j++)
        {
            ss += Convert.ToDecimal(shopDt.Rows[j]["RentArea"]);
        }
        ViewState["shopArea"] = ss;  //所有商铺面积之和



        int countNull = 13 - shopCount;
        for (int i = 0; i < countNull; i++)
        {
            shopDt.Rows.Add(shopDt.NewRow());
        }
        gvShop.DataSource = shopDt;
        gvShop.DataBind();
        int gvCount = gvShop.Rows.Count;
        for (int j = shopCount; j < gvCount; j++)
            gvShop.Rows[j].Cells[3].Text = "";

        //if (FirstStatus == 1)
        //{
        //    if (shopCount > 0)
        //    {
        //        GetShopBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ShopId"]));
        //    }
        //}
        //else
        //{
        //    if (shopCount > 0)
        //    {
        //        GetShopModBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ShopModID"]));
        //    }
        //}

        
    }
    #endregion

    #region 获取商铺基本信息
    protected void GetShopBaseInfo(int shopId)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and ShopId = " + shopId;
        //baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConShop());
        if (rs.Count == 1)
        {
            ConShop shop = rs.Dequeue() as ConShop;
            txtShopName.Text = shop.ShopName.ToString();
            DDownListShopType.SelectedValue = shop.ShopTypeID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            DDownListAreaName.SelectedValue = (shop.AreaId == 0 ? "-1" : shop.AreaId.ToString());
            txtStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");

            txtShopCode.Text = (shop.ShopCode == null ? "" : shop.ShopCode.ToString());
            txtContactName.Text = (shop.ContactorName == null ? "" : shop.ContactorName.ToString());
            txtContactTel.Text = (shop.Tel == null ? "" : shop.Tel.ToString());
            DDownListBuilding.SelectedValue = (shop.BuildingID == 0 ? "-1" : shop.BuildingID.ToString());
            //DDownListBrand.SelectedValue = (shop.BrandID == 0 ? "-1" : shop.BrandID.ToString());
            GetShopBrand(shop.BrandID);

            if (DDownListBuilding.SelectedValue.ToString() != "-1")
            {
                int bulid = Convert.ToInt32(DDownListBuilding.SelectedValue);
                BindFollrs(bulid);
                DDownListFloors.SelectedValue = (shop.FloorID == null ? "" : shop.FloorID.ToString());
            }
            else
                BindFollrs(-1);
            if (DDownListFloors.SelectedValue.ToString() != "-1")
            {
                int floor = Convert.ToInt32(DDownListFloors.SelectedValue);
                BindLocation(floor);
                DDownListLocation.SelectedValue = (shop.LocationID == null ? "" : shop.LocationID.ToString());
            }
            else
                BindLocation(-1);
            ViewState["ShopId"] = shop.ShopId;
            ViewState["delShopID"] = shopId;
        }
    }

    protected void GetShopModBaseInfo(int shopId)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and ShopModID = " + shopId;
        rs = baseBo.Query(new ConShopMod());
        if (rs.Count == 1)
        {
            ConShopMod shop = rs.Dequeue() as ConShopMod;
            txtShopName.Text = shop.ShopName.ToString();
            DDownListShopType.SelectedValue = shop.ShopTypeID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            DDownListAreaName.SelectedValue = (shop.AreaId == 0 ? "-1" : shop.AreaId.ToString());
            txtStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");

            txtShopCode.Text = (shop.ShopCode == null ? "" : shop.ShopCode.ToString());
            txtContactName.Text = (shop.ContactorName == null ? "" : shop.ContactorName.ToString());
            txtContactTel.Text = (shop.Tel == null ? "" : shop.Tel.ToString());
            DDownListBuilding.SelectedValue = (shop.BuildingID == 0 ? "-1" : shop.BuildingID.ToString());
            //DDownListBrand.SelectedValue = (shop.BrandID == 0 ? "-1" : shop.BrandID.ToString());
            GetShopBrand(shop.BrandID);

            if (DDownListBuilding.SelectedValue.ToString() != "-1")
            {
                int bulid = Convert.ToInt32(DDownListBuilding.SelectedValue);
                BindFollrs(bulid);
                DDownListFloors.SelectedValue = (shop.FloorID == null ? "" : shop.FloorID.ToString());
            }
            else
                BindFollrs(-1);
            if (DDownListFloors.SelectedValue.ToString() != "-1")
            {
                int floor = Convert.ToInt32(DDownListFloors.SelectedValue);
                BindLocation(floor);
                DDownListLocation.SelectedValue = (shop.LocationID == null ? "" : shop.LocationID.ToString());
            }
            else
                BindLocation(-1);
            ViewState["ShopId"] = shop.ShopModID;
            ViewState["delShopID"] = shop.ShopModID;
        }
    }
    #endregion

    #region 添加或修改商铺基本信息


    protected int InsertOrUpdateShopBaseInfo()
    {
        int result = 0;
        string aa = ViewState["shopFlag"].ToString();
        try
        {
            if (ViewState["shopFlag"].ToString() == "add")
            {
                /*添加商铺信息*/
                FillShopBaseInfo();
                conShop.ShopModID = BaseApp.GetConModShopID();
                ViewState["ShopId"] = conShop.ShopModID;
                result = baseTrans.Insert(conShop);
                ViewState["shopFlag"] = "modify";
            }
            else if (ViewState["shopFlag"].ToString() == "modify")
            {
                FillShopBaseInfo();
                baseTrans.WhereClause = "";
                baseTrans.WhereClause = "ShopModID = " + Convert.ToInt32(ViewState["ShopId"]);
                result = baseTrans.Update(conShop);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }
        return result;
    }
    #endregion

    #region 填充商铺基本信息
    protected void FillShopBaseInfo()
    {
        try
        {
            conShop = new ConShopMod();
            conShop.ConModID = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            conShop.ShopId = 0;
            conShop.ShopCode = txtShopCode.Text;
            conShop.ShopName = txtShopName.Text;
            conShop.ShopTypeID = Convert.ToInt32(DDownListShopType.SelectedValue);
            conShop.BrandID = Convert.ToInt32(ViewState["brandID"]);//Convert.ToInt32(DDownListBrand.SelectedValue);
            conShop.RentArea = Convert.ToDecimal(txtRentArea.Text);
            conShop.AreaId = Convert.ToInt32(DDownListAreaName.SelectedValue);
            conShop.ShopStartDate = Convert.ToDateTime(txtStartDate.Text);
            conShop.ShopEndDate = Convert.ToDateTime(txtEndDate.Text);
            conShop.ContactorName = txtContactName.Text;
            conShop.Tel = txtContactTel.Text;
            conShop.BuildingID = Convert.ToInt32(DDownListBuilding.SelectedValue);
            conShop.FloorID = Convert.ToInt32(DDownListFloors.SelectedValue);
            conShop.LocationID = Convert.ToInt32(DDownListLocation.SelectedValue);
            conShop.CreateTime = DateTime.Now;
            conShop.ModifyTime = DateTime.Now;
            conShop.ContractID = Convert.ToInt32(ViewState["ContractID"]);
            conShop.ShopStatus = ConShop.CONSHOP_TYPE_PAUSE;

            ViewState["RentArea"] = conShop.RentArea;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }
    }
    #endregion

    #region 获取商铺信息中的单元信息
    protected void GetShopUnits()
    {
        //移除原有的商铺信息



        int Index = 0;
        int unitCount = ListBoxUnits.Items.Count;
        for (int i = 0; i < unitCount; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            ListBoxUnits.Items.Remove(item);
            Index = 0;
        }

        baseBo.WhereClause = "";
        baseBo.WhereClause = "ShopID = " + Convert.ToInt32(ViewState["ShopId"]);
        DataSet shopDS = baseBo.QueryDataSet(new ConShopUnit());
        int count = shopDS.Tables[0].Rows.Count;
        if (count > 0)
        {
            DDownListUnit.Enabled = true;
            Resultset myrs = new Resultset();
            for (int i = 0; i < count; i++)
            {
                baseBo.WhereClause = "";
                //baseBo.WhereClause = "UnitID = " + shopDS.Tables[0].Rows[i]["UnitID"];
                string sql = "select UnitID,UnitCode + ' : ' + cast(UseArea as varchar(50)) as UnitCodeArea from Unit where UnitID = " + Convert.ToInt32(shopDS.Tables[0].Rows[i]["UnitID"]);
                DataSet unitDS = baseBo.QueryDataSet(sql);
                ListItem mylistItem = new ListItem();
                mylistItem.Text = unitDS.Tables[0].Rows[0]["UnitCodeArea"].ToString();
                mylistItem.Value = unitDS.Tables[0].Rows[0]["UnitID"].ToString();
                //DDownListUnit.Items.Add(mylistItem);
                ListBoxUnits.Items.Add(mylistItem);

            }
            ViewState["UnitID"] = shopDS.Tables[0].Rows[0]["UnitID"];
        }
        else
            DDownListUnit.Enabled = false;
    }
    protected void GetShopUnitsMod()
    {
        //移除原有的商铺信息

        int Index = 0;
        int unitCount = ListBoxUnits.Items.Count;
        for (int i = 0; i < unitCount; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            ListBoxUnits.Items.Remove(item);
            Index = 0;
        }

        baseBo.WhereClause = "";
        baseBo.WhereClause = "ShopModID = " + Convert.ToInt32(ViewState["ShopId"]);
        DataSet shopDS = baseBo.QueryDataSet(new ConShopUnitMod());
        int count = shopDS.Tables[0].Rows.Count;
        if (count > 0)
        {
            DDownListUnit.Enabled = true;
            Resultset myrs = new Resultset();
            for (int i = 0; i < count; i++)
            {
                baseBo.WhereClause = "";
                //baseBo.WhereClause = "UnitID = " + shopDS.Tables[0].Rows[i]["UnitID"];
                string sql = "select UnitID,UnitCode + ' : ' + cast(UseArea as varchar(50)) as UnitCodeArea from Unit where UnitID = " + Convert.ToInt32(shopDS.Tables[0].Rows[i]["UnitID"]);
                DataSet unitDS = baseBo.QueryDataSet(sql);
                ListItem mylistItem = new ListItem();
                mylistItem.Text = unitDS.Tables[0].Rows[0]["UnitCodeArea"].ToString();
                mylistItem.Value = unitDS.Tables[0].Rows[0]["UnitID"].ToString();
                //DDownListUnit.Items.Add(mylistItem);
                ListBoxUnits.Items.Add(mylistItem);

            }
            ViewState["UnitID"] = shopDS.Tables[0].Rows[0]["UnitID"];
        }
        else
            DDownListUnit.Enabled = false;
    }
    #endregion

    #region 商铺信息基本内容中的单元信息保存草稿

    //商铺信息基本内容中的单元信息保存草稿
    protected int SaveShopUnits()
    {
        int result = 0;
        Units units = new Units();
        UnitsStutas unitsStutas = new UnitsStutas();
        ConShopUnitMod conShopUnit = new ConShopUnitMod();
        try
        {
            //将删除的单元改成未出租状态


            baseBo.WhereClause = "";
            baseBo.WhereClause = "ShopModID = " + ViewState["ShopId"];
            Resultset tempRs = baseBo.Query(conShopUnit);
            if (tempRs.Count > 0)
            {
                foreach (ConShopUnitMod shopUnit in tempRs)
                {
                    baseTrans.WhereClause = "";
                    baseTrans.WhereClause = "UnitID = " + shopUnit.UnitID;
                    unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_INVALID;
                    int l = baseTrans.Update(unitsStutas);
                }
            }

            //先将商铺对应的单元删除,再插入已选好的单元z
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "ShopModID = " + ViewState["ShopId"];

            if (baseTrans.Delete(conShopUnit) != -1)
            {
                int count = ListBoxUnits.Items.Count;
                int index = 0;
                for (int i = 0; i < count; i++)
                {
                    conShopUnit.ShopModID = Convert.ToInt32(ViewState["ShopId"]);
                    conShopUnit.UnitID = Convert.ToInt32(ListBoxUnits.Items[index].Value);
                    //conShopUnit.RentArea = Convert.ToDecimal(ViewState["RentArea"]);
                    conShopUnit.RentStatus = ConShopUnit.RENTSTATUS_TYPE_YES;
                    result = baseTrans.Insert(conShopUnit);

                    //修改单元状态


                    baseTrans.WhereClause = "";
                    baseTrans.WhereClause = "UnitID = " + Convert.ToInt32(ListBoxUnits.Items[index].Value);
                    unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_LEASEOUT;
                    int x = baseTrans.Update(unitsStutas);
                    index++;

                }
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("更新商铺信息错误:", ex);
        }
        return result;
    }
    #endregion

    private void TextEnabled(Boolean boolean)
    {
        txtShopCode.Enabled = boolean;
        txtShopName.Enabled = boolean;
        DDownListShopType.Enabled = boolean;
        txtRentArea.Enabled = boolean;
        DDownListAreaName.Enabled = boolean;
        DDownListFloors.Enabled = boolean;
        txtStartDate.Enabled = boolean;
        txtEndDate.Enabled = boolean;
        txtContactName.Enabled = boolean;
        txtContactTel.Enabled = boolean;
        txtShopBrand.Enabled = boolean;
        //DDownListBrand.Enabled = boolean;
        DDownListBuilding.Enabled = boolean;
        DDownListLocation.Enabled = boolean;
        DDownListUnit.Enabled = boolean;
        ListBoxUnits.Enabled = boolean;
        btnShopSave.Enabled = boolean;
        btnShopDel.Enabled = boolean;
        IBtnUnitsDel.Enabled = boolean;
        IBtnUnitsAdd.Enabled = boolean;

    }

    private void TextClear()
    {
        txtShopCode.Text = "";
        txtShopName.Text = "";
        DDownListShopType.SelectedIndex = -1;
        txtRentArea.Text = "";
        DDownListAreaName.SelectedIndex = -1;
        DDownListFloors.Items.Clear();
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtContactName.Text = "";
        txtContactTel.Text = "";
        txtShopBrand.Text = "";
        //DDownListBrand.SelectedIndex = -1;
        DDownListBuilding.SelectedIndex = -1;
        DDownListLocation.Items.Clear();
        DDownListUnit.Items.Clear();
        ListBoxUnits.Items.Clear();
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string s = allvalue.Value.ToString();
        string[] ss = Regex.Split(s, ",");

        string brandID = ss[0].ToString();
        string brandName = ss[1].ToString();

        if (brandID == "")
        {
            return;
        }
        ViewState["brandID"] = brandID;
        txtShopBrand.Text = brandName;
        GetAllShopInfo(0);
    }

    private void GetShopBrand(int shopBrandID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "BrandID = " + shopBrandID;
        Resultset rs = baseBO.Query(new ConShopBrand());
        if (rs.Count == 1)
        {
            ConShopBrand shopBrand = rs.Dequeue() as ConShopBrand;
            txtShopBrand.Text = shopBrand.BrandName;
            ViewState["brandID"] = shopBrand.BrandId;
        }
        else
            txtShopBrand.Text = "";
    }
}
