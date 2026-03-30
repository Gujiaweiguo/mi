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
using BaseInfo.Dept;

public partial class Lease_ShopBaseInfoPalaver : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    private ConShop conShop;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            /*绑定商业项目*/
            this.BindBusinessItem();
            /*绑定商铺类型*/
            BindShopType();
            //绑定主营品牌
            BindBrand();
            //绑定大楼
            //BindBuilding();
            //绑定面积
            BindArea();

            GetAllShopInfo(1);
            GetShopUnits();
        }
    }
    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        GetShopBaseInfo(shopId);
        GetShopUnits();
        GetAllShopInfo(0);
    }

    #region 初始化DropDownList
    //绑定商铺类型
    protected void BindShopType()
    {
        rs = baseBo.Query(new ShopType());
        DDownListShopType.Text = "";
        foreach (ShopType shopType in rs)
        {
            DDownListShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
        }
    }

    //绑定主营品牌
    protected void BindBrand()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        rs = baseBo.Query(new ConShopBrand());
        DDownListBrand.Items.Add(new ListItem(selected, "-1"));
        foreach (ConShopBrand shopBrand in rs)
            DDownListBrand.Items.Add(new ListItem(shopBrand.BrandName, shopBrand.BrandId.ToString()));
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    private void BindBusinessItem()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        ddlStore.Items.Add(new ListItem(selected, "-1"));
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "depttype=" + Dept.DEPT_TYPE_MALL;
        Resultset rsBusinessItem = objBaseBo.Query(new Dept());
        foreach (Dept objDeptType in rsBusinessItem)
        {
            this.ddlStore.Items.Add(new ListItem(objDeptType.DeptName, objDeptType.DeptID.ToString()));
        }
    }
    #region
    ////绑定大楼
    //protected void BindBuilding(int Storeid)
    //{
    //    string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
    //    baseBo.WhereClause = "StoreID=" + Storeid;
    //    rs = baseBo.Query(new Building());
    //    DDownListBuilding.Items.Add(new ListItem(selected, "-1"));
    //    foreach (Building building in rs)
    //        DDownListBuilding.Items.Add(new ListItem(building.BuildingName, building.BuildingID.ToString()));
    //}

    ////绑定楼层名称
    //protected void BindFollrs(int bulid)
    //{
    //    string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
    //    DDownListFloors.Items.Clear();
    //    baseBo.WhereClause = "";
    //    baseBo.WhereClause = "BuildingID = " + bulid;
    //    rs = baseBo.Query(new Floors());
    //    DDownListFloors.Items.Add(new ListItem(selected, "-1"));
    //    foreach (Floors floors in rs)
    //        DDownListFloors.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
    //}

    ////绑定方位名称
    //protected void BindLocation(int floor)
    //{
    //    string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
    //    DDownListLocation.Items.Clear();
    //    baseBo.WhereClause = "";
    //    baseBo.WhereClause = "FloorID = " + floor;
    //    rs = baseBo.Query(new Location());
    //    DDownListLocation.Items.Add(new ListItem(selected, "-1"));
    //    foreach (Location loca in rs)
    //        DDownListLocation.Items.Add(new ListItem(loca.LocationName, loca.LocationID.ToString()));

    //}

    ////绑定单元
    //protected void BindUnits(int tempUnits)
    //{
    //    string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
    //    DDownListUnit.Items.Clear();
    //    baseBo.OrderBy = "";
    //    baseBo.WhereClause = "UnitStatus = " + Units.BLANKOUT_STATUS_INVALID + " and LocationID = " + tempUnits;
    //    rs = baseBo.Query(new Units());
    //    DDownListUnit.Items.Add(new ListItem(selected, "-1"));
    //    foreach (Units units in rs)
    //        DDownListUnit.Items.Add(new ListItem(units.UnitCode, units.UnitID.ToString()));
    //}
    #endregion
    /// <summary>
    /// 绑定经营区名称
    /// </summary>    protected void BindArea()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        rs = baseBo.Query(new Area());
        DDownListAreaName.Items.Add(new ListItem(selected, "-1"));
        foreach (Area area in rs)
            DDownListAreaName.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
    }
    #endregion
    #region 获取合同对应的商铺    private void GetAllShopInfo(int FirstStatus)
    {
        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            baseBo.WhereClause = "";
            baseBo.OrderBy = "";
            baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and  ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        }
        else
        {
            return;
        }
        DataSet shopDs = baseBo.QueryDataSet(new ConShop());
        DataTable shopDt = shopDs.Tables[0];
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

        if (FirstStatus == 1)
        {
            if (shopCount > 0)
            {
                GetShopBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ShopID"]));
            }
        }
        if (shopCount > 0)
        {
            ViewState["shopFlag"] = "modify";
        }
        else
        {
            ViewState["shopFlag"] = "add";
        }
    }
    #endregion

    #region 获取商铺信息中的单元信息
    protected void GetShopUnits()
    {
        //移除原有的商铺信息        int Index = 0;
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
            //DDownListUnit.Enabled = true;
            Resultset myrs = new Resultset();
            for (int i = 0; i < count; i++)
            {
                baseBo.WhereClause = "";
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
        //else
        //    DDownListUnit.Enabled = false;
    }
    #endregion

    #region 获取商铺基本信息
    protected void GetShopBaseInfo(int shopId)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and  ShopId = " + shopId;
        //baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConShop());
        if (rs.Count == 1)
        {
            ConShop shop = rs.Dequeue() as ConShop;
            txtShopName.Text = shop.ShopName.ToString();
            DDownListShopType.SelectedValue = shop.ShopTypeID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            try { DDownListAreaName.SelectedValue = shop.AreaId.ToString(); }
            catch { }
            txtStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");
            txtShopCode.Text = (shop.ShopCode == null ? "" : shop.ShopCode.ToString());
            txtContactName.Text = (shop.ContactorName == null ? "" : shop.ContactorName.ToString());
            txtContactTel.Text = (shop.Tel == null ? "" : shop.Tel.ToString());
            ddlStore.SelectedValue = (shop.StoreID == 0 ? "" : shop.StoreID.ToString());
            DDownListBrand.SelectedValue = (shop.BrandID == 0 ? "-1" : shop.BrandID.ToString());
            #region
            //if (ddlStore.SelectedValue.ToString() != "-1")
            //{
            //    BindBuilding(Convert.ToInt32(ddlStore.SelectedValue));
            //    DDownListBuilding.SelectedValue = (shop.BuildingID == null ? "" : shop.BuildingID.ToString());
            //}
            //else
            //    BindBuilding(-1);

            //if (DDownListBuilding.SelectedValue.ToString() != "-1")
            //{
            //    int bulid = Convert.ToInt32(DDownListBuilding.SelectedValue);
            //    BindFollrs(bulid);
            //    DDownListFloors.SelectedValue = (shop.FloorID == null ? "" : shop.FloorID.ToString());
            //}
            //else
            //    BindFollrs(-1);
            //if (DDownListFloors.SelectedValue.ToString() != "-1")
            //{
            //    int floor = Convert.ToInt32(DDownListFloors.SelectedValue);
            //    BindLocation(floor);
            //    DDownListLocation.SelectedValue = (shop.LocationID == null ? "" : shop.LocationID.ToString());
            //}
            //else
            //    BindLocation(-1);
            #endregion
            ViewState["ShopId"] = shop.ShopId;
            ViewState["delShopID"] = shopId;
        }
    }
    #endregion
}
