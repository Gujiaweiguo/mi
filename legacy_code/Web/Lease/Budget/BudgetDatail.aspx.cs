using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using Lease.PotBargain;
using Invoice;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base;
using BaseInfo.User;
using WorkFlow.Uiltil;
using BaseInfo.authUser;

public partial class Lease_Budget_BudgetDatail : BasePage
{
    public string baseinfo;
    public string urlpara;
    /// <summary>
    /// 用于绑定的表
    /// </summary>
    protected DataTable ChargeDetailDT
    {
        set
        {
            ViewState["Sour"] = value;
        }
        get
        {
            return (DataTable)ViewState["Sour"];
        }
    }
    protected DataTable SaveChargeDetailDT
    {
        set
        {
            ViewState["SaveSour"] = value;
        }
        get
        {
            return (DataTable)ViewState["SaveSour"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BudgetDetail");
        if (!this.IsPostBack)
        {
            this.ShowTree();
            this.BindChargeType();//绑定费用类型
            this.BindYears();
            
            this.txtStartDate.Text = Convert.ToDateTime(this.ddlYears.SelectedValue.ToString()  + "-01-01").ToString("yyyy-MM-dd");//开始日期
            this.txtEndDate.Text = Convert.ToDateTime(this.ddlYears.SelectedValue.ToString() + "-12-31").ToString("yyyy-MM-dd");//结束日期
            ViewState["count"] = 0;
            ViewState["flag"] = "0";
            IniChargeDetailDT();
            if (Request.QueryString["VoucherID"] != null)
            {
                int chgID = Convert.ToInt32(Request["VoucherID"]);
                ViewState["chgID"] = chgID;

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("conID", Request["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());

                Response.AppendCookie(cookies);

                this.page(chgID);
                ViewState["flag"] = "2";
                btnAdd.Enabled = false;
                WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"].ToString()));
                string ss = objWrkFlwEntity.VoucherMemo;
                btnAdd.Enabled = false;
            }
            else
            {
                page(0);
                this.btnPutIn.Enabled = false;//提交
                btnBlankOut.Enabled = false;
            }
            urlpara = "Lease/Budget/BudgetDatail.aspx?WrkFlwID=" + Request.QueryString["WrkFlwID"].ToString() + "&NodeID=" + Request.QueryString["NodeID"].ToString() + "&Type=New";
        }
    }
    protected void IniChargeDetailDT()
    {



        ChargeDetailDT = new DataTable();
        ChargeDetailDT.Columns.Add("BudgetID");
        ChargeDetailDT.Columns.Add("ChargeTypeID");
        ChargeDetailDT.Columns.Add("ChgName");
        ChargeDetailDT.Columns.Add("BudgetYear");
        ChargeDetailDT.Columns.Add("StartDate");
        ChargeDetailDT.Columns.Add("EndDate");
        ChargeDetailDT.Columns.Add("UnitID");
        ChargeDetailDT.Columns.Add("UnitCode");
        ChargeDetailDT.Columns.Add("FloorArea");
        ChargeDetailDT.Columns.Add("UseArea");
        ChargeDetailDT.Columns.Add("ShopTypeID");
        ChargeDetailDT.Columns.Add("TradeID");
        ChargeDetailDT.Columns.Add("UnitTypeID");
        ChargeDetailDT.Columns.Add("ShopTypeName");
        ChargeDetailDT.Columns.Add("TradeName");
        ChargeDetailDT.Columns.Add("UnitTypeName");
        ChargeDetailDT.Columns.Add("RentType");
        ChargeDetailDT.Columns.Add("RentAmt");
        SaveChargeDetailDT = ChargeDetailDT.Clone();
    }
    /// <summary>
    /// 绑定费用类型
    /// </summary>
    private void BindChargeType()
    {
        ddlChargeType.Items.Clear();
        BaseBO objBaseBo = new BaseBO();
        //基本租金、推广费、物业费
        objBaseBo.WhereClause = "ChargeTypeid in (101,105,109)";  
        Resultset chageRs = objBaseBo.Query(new ChargeType());
        foreach (ChargeType chargeType in chageRs)
        {
            ddlChargeType.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
        }


    }
    private void BindYears()
    {
        ddlYears.Items.Clear();
        int year=Convert.ToInt32(DateTime.Now.Year.ToString());
        for (int i = year - 5; i < year + 5;i++ )
        {
            ddlYears.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        ddlYears.SelectedValue = year.ToString();


    }
    protected void page(int batchID)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = "BudgetDetail.BatchID = " + batchID;
        //DataSet ds = baseBO.QueryDataSet(new OtherChargeD());
        BudgetDetail objBudgetDetail = new BudgetDetail();
        string strSql = " SELECT  BudgetDetail.BudgetID,BudgetDetail.BatchID,BudgetDetail.UnitID,BudgetDetail.FloorArea,BudgetDetail.UseArea," +
                        "         BudgetDetail.ShopTypeID,BudgetDetail.TradeID,BudgetDetail.UnitTypeID, BudgetYear ," +
                        "         Convert(char(10),BudgetDetail.StartDate,120) as StartDate," +
                        "         Convert(char(10),BudgetDetail.EndDate,120) as EndDate," +
                        "         BudgetDetail.ChargeTypeID,BudgetDetail.RentType,BudgetDetail.RentAmt,BudgetDetail.BudgetStatus," +
                        "         BudgetDetail.ModifyUserId,BudgetDetail.ModifyTime,BudgetDetail.OprRoleID,BudgetDetail.OprDeptID," +
                        "         ShopType.ShopTypeName,TradeRelation.TradeName,UnitType.UnitTYpeName,Unit.UnitCode" +
                        " FROM    BudgetDetail INNER JOIN ShopType ON (BudgetDetail.ShopTypeID=ShopType.ShopTypeID) " +
                        "         INNER JOIN TradeRelation ON (BudgetDetail.TradeID=TradeRelation.TradeID) " +
                        "         INNER JOIN UnitType ON (BudgetDetail.UnitTypeID=UnitType.UnitTypeID)" +
                        "         INNER JOIN Unit ON(BudgetDetail.UnitID=Unit.UnitID)";

        objBudgetDetail.SetQuerySql(strSql);
        //BudgetID,BatchID,UnitID,FloorArea,UseArea,ShopTypeID,TradeID,UnitTypeID,BudgetYear,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,ChargeTypeID,RentType,RentAmt,BudgetStatus,ModifyUserId,ModifyTime,OprRoleID,OprDeptID
        DataSet ds = baseBO.QueryDataSet(objBudgetDetail);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            
            ddlYears.SelectedValue = dt.Rows[0]["BudgetYear"].ToString();
            txtStartDate.Text = dt.Rows[0]["StartDate"].ToString();
            txtEndDate.Text = dt.Rows[0]["EndDate"].ToString();
            ViewState["flag"] = "2";
        }
        //if (batchID != 0)
        //{
        //    ChargeDetailDT = dt;

        //}
        int count = dt.Rows.Count;

        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCharge.DataSource = dt;
        gvCharge.DataBind();
        ViewState["count"] = count;
#region
        //if (batchID != 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int l = 0; l < gvCharge.Rows.Count; l++)
        //        {

        //            if (gvCharge.Rows[l].FindControl("txtBudgetID").ToString() == dt.Rows[i]["BudgetID"].ToString())
        //            {
        //                /////////////////////////////////循环绑定DDL
        //                DropDownList ddlRentType = (DropDownList)this.gvCharge.Rows[i].FindControl("ddlRentType");
        //                ddlRentType.SelectedValue = dt.Rows[i]["RentType"].ToString();

        //                //TextBox ddlRentType = (TextBox)this.gvCharge.Rows[i].FindControl("ddlRentType");

        //            }
        //        }
        //    }
        //}
#endregion
    }
    private void ShowTree()
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

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
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

                        baseBO.WhereClause = "BuildingID=" + building.BuildingID;
                        //if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        //{
                        //    baseBO.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        //                         ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        //                         ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        //                         ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        //                         ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        //    //strAuth = strAuth.Replace("ConShop", "transshopday");
                        //}
                        rsf = baseBO.Query(new Floors());
                        foreach (Floors floors in rsf)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + floors.BuildingID.ToString() + "|" + floors.FloorName.ToString() + "^";

                            baseBO.WhereClause = "FloorID=" + floors.FloorID;
                            rsl = baseBO.Query(new Location());

                            foreach (Location location in rsl)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";

                                baseBO.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " and UnitStatus !=" + Units.BLANKOUT_STATUS_VALID;
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
    protected void gvCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
          TextBox txtBudgetID = (TextBox)e.Row.FindControl("txtBudgetID");
          TextBox txtUnitID = (TextBox)e.Row.FindControl("txtUnitID");
          if (txtBudgetID.Text.Trim() != "")
          {
              ((System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Checkbox")).Checked = true;
              ((System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Checkbox")).Enabled = false;
              ((System.Web.UI.WebControls.DropDownList)e.Row.FindControl("ddlRentType")).Enabled = true;
          }
          if (txtUnitID.Text.Trim()=="")
          {
              ((System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Checkbox")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitCode")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtFloorArea")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUseArea")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtShopTypeName")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtTradeName")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitTypeName")).Visible = false;
              ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtRentAmt")).Visible = false;
              ((System.Web.UI.WebControls.DropDownList)e.Row.FindControl("ddlRentType")).Visible = false;
          }

        }
        if (((DropDownList)e.Row.FindControl("ddlRentType")) != null)
        {
           // TextBox txtBudgetID = (TextBox)e.Row.FindControl("txtBudgetID");
            TextBox txtUnitID = (TextBox)e.Row.FindControl("txtUnitID");
            if (txtUnitID.Text.ToString() != "" && txtUnitID.Text.ToString() != null)
            {
              
                DropDownList ddlRentType = (DropDownList)e.Row.FindControl("ddlRentType");
                ddlRentType.Items.Clear();
                ddlRentType.Items.Add(new ListItem("日", "D"));
                ddlRentType.Items.Add(new ListItem("月", "M"));
                ddlRentType.Items.Add(new ListItem("年", "Y"));
                //DropDownList初始被选择的项
                ddlRentType.SelectedValue = ((HiddenField)e.Row.FindControl("hidRentType")).Value;
            }
        }



    }

    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        page(0);
        this.btnAdd.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ChargeDetailDT.Rows.Clear();
        SaveChargeDetailDT.Rows.Clear();
        ViewState["flag"] = "0";
        ViewState["checkeds"] = "";
        BaseBO objBaseBo = new BaseBO();
        this.btnPutIn.Enabled = false;
        btnBlankOut.Enabled = false;
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
        {
            #region 选择单元
            if (ViewState["ID"].ToString().Length > 12)//选择单元
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                string strUnitID = ViewState["ID"].ToString().Substring(12);
               // string strUnitCode = BaseInfo.BaseCommon.GetTextValueByID("UnitCode", "UnitID", "Unit", strUnitID);
               /////////////////////////
                string strSql = "select UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where UnitID=" + strUnitID + "and UnitStatus !=" + Units.BLANKOUT_STATUS_VALID;
                DataSet ds = objBaseBo.QueryDataSet(strSql);
                if (ds != null && ds.Tables[0].Rows.Count == 1)
                {




                    DataRow dr = ChargeDetailDT.NewRow();
                    string strShopTypeName = BaseInfo.BaseCommon.GetTextValueByID("ShopTypeName", "ShopTypeID", "ShopType", ds.Tables[0].Rows[0]["ShopTypeID"].ToString());
                    string strTradeName = BaseInfo.BaseCommon.GetTextValueByID("TradeName", "TradeID", "TradeRelation", ds.Tables[0].Rows[0]["Trade2ID"].ToString());
                    string strUnitTypeName = BaseInfo.BaseCommon.GetTextValueByID("UnitTypeName", "UnitTypeID", "UnitType", ds.Tables[0].Rows[0]["UnitTypeID"].ToString());

                    dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                    dr["ChgName"] = this.ddlChargeType.SelectedItem;
                    dr["BudgetYear"] = ddlYears.SelectedValue;
                    dr["StartDate"] = this.txtStartDate.Text.Trim();
                    dr["EndDate"] = this.txtEndDate.Text.Trim();
                    dr["UnitID"] = strUnitID;
                    dr["UnitCode"] = ds.Tables[0].Rows[0]["UnitCode"].ToString();
                    dr["FloorArea"] = ds.Tables[0].Rows[0]["FloorArea"].ToString();
                    dr["UseArea"] = ds.Tables[0].Rows[0]["UseArea"].ToString();
                    

                    dr["ShopTypeID"] = ds.Tables[0].Rows[0]["ShopTypeID"].ToString();
                    dr["TradeID"] = ds.Tables[0].Rows[0]["Trade2ID"].ToString();
                    dr["UnitTypeID"] = ds.Tables[0].Rows[0]["UnitTypeID"].ToString();
                    dr["ShopTypeName"] = strShopTypeName;
                    dr["TradeName"] = strTradeName;
                    dr["UnitTypeName"] = strUnitTypeName;
                    dr["BudgetID"] = "";
                    dr["RentType"] = "";
                    dr["RentAmt"] = "";


                    ChargeDetailDT.Rows.Add(dr);
                }
                for (int i = 1; i < 15; i++)
                {
                    ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
            #endregion
            #region 选择方位
            if (ViewState["ID"].ToString().Length == 12)//选择方位
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                string strSql = "select UnitID,UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where LocationID=" + strLocationID + "and UnitStatus !=" + Units.BLANKOUT_STATUS_VALID;
                DataSet ds = objBaseBo.QueryDataSet(strSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["count"] = "0";
                    ChargeDetailDT.Rows.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        DataRow dr = ChargeDetailDT.NewRow();
                        string strShopTypeName = BaseInfo.BaseCommon.GetTextValueByID("ShopTypeName", "ShopTypeID", "ShopType", ds.Tables[0].Rows[i]["ShopTypeID"].ToString());
                        string strTradeName = BaseInfo.BaseCommon.GetTextValueByID("TradeName", "TradeID", "TradeRelation", ds.Tables[0].Rows[i]["Trade2ID"].ToString());
                        string strUnitTypeName = BaseInfo.BaseCommon.GetTextValueByID("UnitTypeName", "UnitTypeID", "UnitType", ds.Tables[0].Rows[i]["UnitTypeID"].ToString());

                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                        dr["ChgName"] = this.ddlChargeType.SelectedItem;
                        dr["BudgetYear"] = ddlYears.SelectedValue;
                        dr["StartDate"] = this.txtStartDate.Text.Trim();
                        dr["EndDate"] = this.txtEndDate.Text.Trim();
                        dr["UnitID"] = ds.Tables[0].Rows[i]["UnitID"].ToString();
                        dr["UnitCode"] = ds.Tables[0].Rows[i]["UnitCode"].ToString();
                        dr["FloorArea"] = ds.Tables[0].Rows[i]["FloorArea"].ToString();
                        dr["UseArea"] = ds.Tables[0].Rows[i]["UseArea"].ToString();


                        dr["ShopTypeID"] = ds.Tables[0].Rows[i]["ShopTypeID"].ToString();
                        dr["TradeID"] = ds.Tables[0].Rows[i]["Trade2ID"].ToString();
                        dr["UnitTypeID"] = ds.Tables[0].Rows[i]["UnitTypeID"].ToString();
                        dr["ShopTypeName"] = strShopTypeName;
                        dr["TradeName"] = strTradeName;
                        dr["UnitTypeName"] = strUnitTypeName;
                        dr["BudgetID"] = "";
                        dr["RentType"] = "";
                        dr["RentAmt"] = "";
                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = (Convert.ToInt32(ViewState["count"]) % 15); i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
                        #endregion
            #region 选择楼层
            if (ViewState["ID"].ToString().Length == 9)//选择楼层
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                //string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                string strSql = "select UnitID,UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where FloorID=" + strFloorID + "and UnitStatus !=" + Units.BLANKOUT_STATUS_VALID;
                DataSet ds = objBaseBo.QueryDataSet(strSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["count"] = "0";
                    ChargeDetailDT.Rows.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        DataRow dr = ChargeDetailDT.NewRow();
                        string strShopTypeName = BaseInfo.BaseCommon.GetTextValueByID("ShopTypeName", "ShopTypeID", "ShopType", ds.Tables[0].Rows[i]["ShopTypeID"].ToString());
                        string strTradeName = BaseInfo.BaseCommon.GetTextValueByID("TradeName", "TradeID", "TradeRelation", ds.Tables[0].Rows[i]["Trade2ID"].ToString());
                        string strUnitTypeName = BaseInfo.BaseCommon.GetTextValueByID("UnitTypeName", "UnitTypeID", "UnitType", ds.Tables[0].Rows[i]["UnitTypeID"].ToString());

                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                        dr["ChgName"] = this.ddlChargeType.SelectedItem;
                        dr["BudgetYear"] = ddlYears.SelectedValue;
                        dr["StartDate"] = this.txtStartDate.Text.Trim();
                        dr["EndDate"] = this.txtEndDate.Text.Trim();
                        dr["UnitID"] = ds.Tables[0].Rows[i]["UnitID"].ToString();
                        dr["UnitCode"] = ds.Tables[0].Rows[i]["UnitCode"].ToString();
                        dr["FloorArea"] = ds.Tables[0].Rows[i]["FloorArea"].ToString();
                        dr["UseArea"] = ds.Tables[0].Rows[i]["UseArea"].ToString();


                        dr["ShopTypeID"] = ds.Tables[0].Rows[i]["ShopTypeID"].ToString();
                        dr["TradeID"] = ds.Tables[0].Rows[i]["Trade2ID"].ToString();
                        dr["UnitTypeID"] = ds.Tables[0].Rows[i]["UnitTypeID"].ToString();
                        dr["ShopTypeName"] = strShopTypeName;
                        dr["TradeName"] = strTradeName;
                        dr["UnitTypeName"] = strUnitTypeName;
                        dr["BudgetID"] = "";
                        dr["RentType"] = "";
                        dr["RentAmt"] = "";

                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = (Convert.ToInt32(ViewState["count"]) % 15); i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
            #endregion
            #region 选择大楼
            if (ViewState["ID"].ToString().Length == 6)//选择大楼
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);

                string strSql = "select UnitID,UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where BuildingID=" + strBuildingID + "and UnitStatus !=" + Units.BLANKOUT_STATUS_VALID;
                DataSet ds = objBaseBo.QueryDataSet(strSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["count"] = "0";
                    ChargeDetailDT.Rows.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        DataRow dr = ChargeDetailDT.NewRow();
                        string strShopTypeName = BaseInfo.BaseCommon.GetTextValueByID("ShopTypeName", "ShopTypeID", "ShopType", ds.Tables[0].Rows[i]["ShopTypeID"].ToString());
                        string strTradeName = BaseInfo.BaseCommon.GetTextValueByID("TradeName", "TradeID", "TradeRelation", ds.Tables[0].Rows[i]["Trade2ID"].ToString());
                        string strUnitTypeName = BaseInfo.BaseCommon.GetTextValueByID("UnitTypeName", "UnitTypeID", "UnitType", ds.Tables[0].Rows[i]["UnitTypeID"].ToString());

                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                        dr["ChgName"] = this.ddlChargeType.SelectedItem;
                        dr["BudgetYear"] = ddlYears.SelectedValue;
                        dr["StartDate"] = this.txtStartDate.Text.Trim();
                        dr["EndDate"] = this.txtEndDate.Text.Trim();
                        dr["UnitID"] = ds.Tables[0].Rows[i]["UnitID"].ToString();
                        dr["UnitCode"] = ds.Tables[0].Rows[i]["UnitCode"].ToString();
                        dr["FloorArea"] = ds.Tables[0].Rows[i]["FloorArea"].ToString();
                        dr["UseArea"] = ds.Tables[0].Rows[i]["UseArea"].ToString();


                        dr["ShopTypeID"] = ds.Tables[0].Rows[i]["ShopTypeID"].ToString();
                        dr["TradeID"] = ds.Tables[0].Rows[i]["Trade2ID"].ToString();
                        dr["UnitTypeID"] = ds.Tables[0].Rows[i]["UnitTypeID"].ToString();
                        dr["ShopTypeName"] = strShopTypeName;
                        dr["TradeName"] = strTradeName;
                        dr["UnitTypeName"] = strUnitTypeName;
                        dr["BudgetID"] = "";
                        dr["RentType"] = "";
                        dr["RentAmt"] = "";

                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = (Convert.ToInt32(ViewState["count"]) % 15); i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
            #endregion
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveChargeDetailDT.Rows.Clear();
        FindChecked();
        string strArr = "," + ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') + ",";
        if (ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            return;
        }
        string strDateISNull = "没有数据";
        string strDateISError = "数据错误";
        bool bDataISError = false;
        for (int i = 0; i < ChargeDetailDT.Rows.Count; i++)
        {
            if (ChargeDetailDT.Rows[i]["UnitID"].ToString() != "")
            {
                if (strArr.IndexOf("," + ChargeDetailDT.Rows[i]["UnitID"] + ",") >= 0)
                {
                    if (ChargeDetailDT.Rows[i]["RentAmt"].ToString() == "" || ChargeDetailDT.Rows[i]["RentAmt"].ToString() == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISNull + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                }
            }
        }
        BaseTrans baseTrans = new BaseTrans();
        OtherChargeD otherChgD = new OtherChargeD();
        BudgetDetail budgetDetail = new BudgetDetail();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseBO objBaseBo = new BaseBO();
        try
        {
            if (ViewState["flag"].ToString() == "0")//新增
            {
                #region 保存单元
                if (ViewState["ID"].ToString().Length > 12)//保存单元
                {

                    baseTrans.BeginTrans();
                    if (strArr.IndexOf("," + ChargeDetailDT.Rows[0]["UnitID"] + ",") >= 0)//判断是否选择了数据
                    {

                        budgetDetail.BatchID = BaseApp.GetID("BudgetDetail", "BatchID");
                        budgetDetail.BudgetID = BaseApp.GetID("BudgetDetail", "BudgetID");
                        budgetDetail.UnitID = Convert.ToInt32(ChargeDetailDT.Rows[0]["UnitID"]);
                        budgetDetail.FloorArea = Convert.ToDecimal(ChargeDetailDT.Rows[0]["FloorArea"]);
                        budgetDetail.UseArea = Convert.ToDecimal(ChargeDetailDT.Rows[0]["UseArea"]);
                        budgetDetail.ShopTypeID = Convert.ToInt32(ChargeDetailDT.Rows[0]["ShopTypeID"]);
                        budgetDetail.TradeID = Convert.ToInt32(ChargeDetailDT.Rows[0]["TradeID"]);
                        budgetDetail.UnitTypeID = Convert.ToInt32(ChargeDetailDT.Rows[0]["UnitTypeID"]);
                        budgetDetail.RentType = ((DropDownList)gvCharge.Rows[0].FindControl("ddlRentType")).SelectedValue;
                        budgetDetail.RentAmt = Convert.ToDecimal(ChargeDetailDT.Rows[0]["RentAmt"]);

                        budgetDetail.ChargeTypeID =Convert.ToInt32(ddlChargeType.SelectedValue);
                        budgetDetail.BudgetYear = Convert.ToInt32(ddlYears.SelectedValue);
                        budgetDetail.StartDate =Convert.ToDateTime(txtStartDate.Text.Trim());
                        budgetDetail.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                        budgetDetail.CreateUserId = objSessionUser.UserID;
                        budgetDetail.BudgetStatus = BudgetDetail.BDGSTATUS_TYPE_TEMP;
                        
                        ViewState["chgID"] = budgetDetail.BatchID;

                        if (budgetDetail.RentAmt > 0)//费用金额不为零时保存
                            {
                                if (baseTrans.Insert(budgetDetail) != -1)
                                {
                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                }
                            }
                            else
                            {
                                bDataISError = true;
                            }
                        
                    }
                    if (bDataISError == true)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }

                    int voucherID = budgetDetail.BatchID;
                    String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim() + (String)GetGlobalResourceObject("BaseInfo", "Menu_Budget");
                    ViewState["voucherHints"] = voucherHints;
                    String voucherMemo = "";
                   VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                    
                    WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);
                    baseTrans.Commit();
                    /*保存草稿提交的节点ID*/
                    HttpCookie cookies = new HttpCookie("Info");
                    cookies.Expires = System.DateTime.Now.AddDays(1);
                    cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                    cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                    cookies.Values.Add("conID", ViewState["chgID"].ToString());
                    cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                    Response.AppendCookie(cookies);
                    this.btnPutIn.Enabled = true;
                    btnBlankOut.Enabled = true;
                }
                #endregion
                #region 保存方位
                if (ViewState["ID"].ToString().Length == 12)//选择方位
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                    string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                    string strSql = "select UnitID,UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where LocationID=" + strLocationID;

                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                      
                        int BatchID = BaseInfo.BaseCommon.GetTextValueByID("max(BatchID)", "1", "BudgetDetail", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(BatchID)", "1", "BudgetDetail", "1")) + 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            baseTrans.BeginTrans();
                            ViewState["chgID"] = BatchID;


                                    for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                    {
                                        if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["UnitID"] + ",") >= 0)//判断是否选择
                                        {

                                        if (ChargeDetailDT.Rows[l]["UnitID"].ToString() == ds.Tables[0].Rows[i]["UnitID"].ToString())
                                        {
                                            budgetDetail.BatchID = BatchID;
                                            budgetDetail.BudgetID = BaseApp.GetID("BudgetDetail", "BudgetID");
                                            budgetDetail.UnitID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitID"]);
                                            budgetDetail.FloorArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["FloorArea"]);
                                            budgetDetail.UseArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["UseArea"]);
                                            budgetDetail.ShopTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ShopTypeID"]);
                                            budgetDetail.TradeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["TradeID"]);
                                            budgetDetail.UnitTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitTypeID"]);

                                            //budgetDetail.RentType = ((DropDownList)gvCharge.Rows[l].FindControl("ddlRentType")).SelectedValue;
                                            budgetDetail.RentType = ChargeDetailDT.Rows[l]["RentType"].ToString();
                                            budgetDetail.RentAmt = Convert.ToInt32(ChargeDetailDT.Rows[l]["RentAmt"]);

                                            budgetDetail.ChargeTypeID = Convert.ToInt32(ddlChargeType.SelectedValue);
                                            budgetDetail.BudgetYear = Convert.ToInt32(ddlYears.SelectedValue);
                                            budgetDetail.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                                            budgetDetail.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                                            budgetDetail.CreateUserId = objSessionUser.UserID;
                                            budgetDetail.BudgetStatus = BudgetDetail.BDGSTATUS_TYPE_TEMP;
                                            if (budgetDetail.RentAmt > 0)
                                            {
                                                if (baseTrans.Insert(budgetDetail) != -1)
                                                {
                                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                                }
                                            }
                                            else
                                            {
                                                bDataISError = true;
                                            }
                                        }
                                        //}
                                    }
                               // }
                                   
                            }
                            baseTrans.Commit();
                        }

                        if (bDataISError == true)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        ///*保存草稿提交的节点ID*/
                        BaseTrans objBaseTrans = new BaseTrans();
                        objBaseTrans.BeginTrans();
                        int voucherID = BatchID;
                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim()+(String)GetGlobalResourceObject("BaseInfo", "Menu_Budget");
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, objBaseTrans);
                        objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", BatchID.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                        btnBlankOut.Enabled = true;
                    }
                }
                                #endregion
                #region 保存楼层
                if (ViewState["ID"].ToString().Length == 9)//选择楼层
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strFloorID = ViewState["ID"].ToString().Substring(6, 3);

                    string strSql = "select UnitID,UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where FloorID=" + strFloorID;
                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int BatchID = BaseInfo.BaseCommon.GetTextValueByID("max(BatchID)", "1", "BudgetDetail", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(BatchID)", "1", "BudgetDetail", "1")) + 1;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            baseTrans.BeginTrans();
                            ViewState["chgID"] = BatchID;

                                    for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                    {
                                        if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["UnitID"].ToString() + ",") >= 0)//判断是否选择
                                        {
                                        if (ChargeDetailDT.Rows[l]["UnitID"].ToString() == ds.Tables[0].Rows[i]["UnitID"].ToString())
                                        {
                                            budgetDetail.BatchID = BatchID;
                                            budgetDetail.BudgetID = BaseApp.GetID("BudgetDetail", "BudgetID");
                                            budgetDetail.UnitID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitID"]);
                                            budgetDetail.FloorArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["FloorArea"]);
                                            budgetDetail.UseArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["UseArea"]);
                                            budgetDetail.ShopTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ShopTypeID"]);
                                            budgetDetail.TradeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["TradeID"]);
                                            budgetDetail.UnitTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitTypeID"]);
                                          //budgetDetail.RentType = ((DropDownList)gvCharge.Rows[l].FindControl("ddlRentType")).SelectedValue;
                                            budgetDetail.RentType = ChargeDetailDT.Rows[l]["RentType"].ToString();
                                            budgetDetail.RentAmt = Convert.ToInt32(ChargeDetailDT.Rows[l]["RentAmt"]);

                                            budgetDetail.ChargeTypeID = Convert.ToInt32(ddlChargeType.SelectedValue);
                                            budgetDetail.BudgetYear = Convert.ToInt32(ddlYears.SelectedValue);
                                            budgetDetail.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                                            budgetDetail.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                                            budgetDetail.CreateUserId = objSessionUser.UserID;
                                            budgetDetail.BudgetStatus = BudgetDetail.BDGSTATUS_TYPE_TEMP;
                                            if (budgetDetail.RentAmt > 0)
                                            {
                                                if (baseTrans.Insert(budgetDetail) != -1)
                                                {
                                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                                }
                                            }
                                            else
                                            {
                                                bDataISError = true;
                                            }
                                        }
                                        }
                                    }
                                baseTrans.Commit();
                        }
                        if (bDataISError == true)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        ///*保存草稿提交的节点ID*/
                        BaseTrans objBaseTrans = new BaseTrans();
                        objBaseTrans.BeginTrans();
                        int voucherID = BatchID;
                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim() + (String)GetGlobalResourceObject("BaseInfo", "Menu_Budget");
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, objBaseTrans);
                        objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", BatchID.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                        btnBlankOut.Enabled = true;
                    }
                }
                #endregion
                #region 保存大楼
                if (ViewState["ID"].ToString().Length == 6)//选择大楼
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strSql = "select UnitID,UnitCode,FloorArea,UseArea,ShopTypeID,Trade2ID,UnitTypeID from unit where BuildingID=" + strBuildingID;
                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int BatchID = BaseInfo.BaseCommon.GetTextValueByID("max(BatchID)", "1", "BudgetDetail", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(BatchID)", "1", "BudgetDetail", "1")) + 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            baseTrans.BeginTrans();
                            ViewState["chgID"] = BatchID;

                                for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                {
                                    if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["UnitID"] + ",") >= 0)//判断是否选择
                                    {
                                        if (ChargeDetailDT.Rows[l]["UnitID"].ToString() == ds.Tables[0].Rows[i]["UnitID"].ToString())
                                        {
                                            budgetDetail.BatchID = BatchID;
                                            budgetDetail.BudgetID = BaseApp.GetID("BudgetDetail", "BudgetID");
                                            budgetDetail.UnitID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitID"]);
                                            budgetDetail.FloorArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["FloorArea"]);
                                            budgetDetail.UseArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["UseArea"]);
                                            budgetDetail.ShopTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ShopTypeID"]);
                                            budgetDetail.TradeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["TradeID"]);
                                            budgetDetail.UnitTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitTypeID"]);
                                            //budgetDetail.RentType = ((DropDownList)gvCharge.Rows[l].FindControl("ddlRentType")).SelectedValue;
                                            budgetDetail.RentType = ChargeDetailDT.Rows[l]["RentType"].ToString();
                                            budgetDetail.RentAmt = Convert.ToDecimal (ChargeDetailDT.Rows[l]["RentAmt"]);

                                            budgetDetail.ChargeTypeID = Convert.ToInt32(ddlChargeType.SelectedValue);
                                            budgetDetail.BudgetYear = Convert.ToInt32(ddlYears.SelectedValue);
                                            budgetDetail.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                                            budgetDetail.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                                            budgetDetail.CreateUserId = objSessionUser.UserID;
                                            budgetDetail.BudgetStatus = BudgetDetail.BDGSTATUS_TYPE_TEMP;
                                            if (budgetDetail.RentAmt > 0)
                                            {
                                                if (baseTrans.Insert(budgetDetail) != -1)
                                                {
                                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                                }
                                            }
                                            else
                                            {
                                                bDataISError = true;
                                            }
                                        }
                                    }
                                }
                            //}
                            baseTrans.Commit();
                            //}
                        }
                        if (bDataISError == true)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        /*保存草稿提交的节点ID*/
                        BaseTrans objBaseTrans = new BaseTrans();
                        objBaseTrans.BeginTrans();
                        int voucherID = BatchID;

                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim() + (String)GetGlobalResourceObject("BaseInfo", "Menu_Budget");
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, objBaseTrans);
                        objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", BatchID.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                        btnBlankOut.Enabled = true;
                    }
                }
                #endregion
            }
            else//更新
            {
                for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                {
                    string txtUnitID = ChargeDetailDT.Rows[l]["UnitID"].ToString();
                    if (strArr.IndexOf("," + txtUnitID + ",") >= 0)//判断是否选择了数据
                    {

                        budgetDetail.BatchID = Convert.ToInt32(ChargeDetailDT.Rows[l]["BatchID"]);
                        budgetDetail.BudgetID = Convert.ToInt32(ChargeDetailDT.Rows[l]["BudgetID"]);
                        budgetDetail.UnitID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitID"]);
                        budgetDetail.FloorArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["FloorArea"]);
                        budgetDetail.UseArea = Convert.ToDecimal(ChargeDetailDT.Rows[l]["UseArea"]);
                        budgetDetail.ShopTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ShopTypeID"]);
                        budgetDetail.TradeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["TradeID"]);
                        budgetDetail.UnitTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["UnitTypeID"]);
                      //budgetDetail.RentType = ((DropDownList)gvCharge.Rows[l].FindControl("ddlRentType")).SelectedValue;
                        budgetDetail.RentType = ChargeDetailDT.Rows[l]["RentType"].ToString();
                        budgetDetail.RentAmt = Convert.ToDecimal(ChargeDetailDT.Rows[l]["RentAmt"]);

                        budgetDetail.ChargeTypeID = Convert.ToInt32(ddlChargeType.SelectedValue);
                        budgetDetail.BudgetYear = Convert.ToInt32(ddlYears.SelectedValue);
                        budgetDetail.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                        budgetDetail.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                        budgetDetail.ModifyUserId = objSessionUser.UserID;







                        if (budgetDetail.RentAmt > 0)
                        {
                            string BudgetID = ChargeDetailDT.Rows[l]["BudgetID"].ToString();
                            BaseTrans objBaseTrans = new BaseTrans();
                            objBaseTrans.BeginTrans();
                            objBaseTrans.WhereClause = "BudgetID=" + BudgetID;
                            if (objBaseTrans.Update(budgetDetail) != -1)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                            }
                            objBaseTrans.Commit();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            Response.Write(ex);
        }

        btnAdd.Enabled = false;
        this.btnPutIn.Enabled = true;
        this.page(Int32.Parse(ViewState["chgID"].ToString()));

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 记录每页选中的情况
    /// </summary>
    /// <param name="strHaveSelects"></param>
    public void SetDataGridSelectRecords(string strHaveSelects)
    {
        strHaveSelects = "," + strHaveSelects.TrimEnd(',').TrimStart(',') + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {
            TextBox txtUnitID = (TextBox)this.gvCharge.Rows[i].FindControl("txtUnitID");
            string strtemp = txtUnitID.Text.Trim();
            if (strtemp != "")
            {
                if (strHaveSelects.IndexOf("," + strtemp + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                }
            }
        }
    }
    /// <summary>
    /// 记录表中选中记录的情况
    /// </summary>
    /// <returns></returns>
    private void FindChecked()
    {
        string checkeds = "";
        string strShopChecks = "";
        if (ViewState["checkeds"] != null)
            checkeds = "," + ViewState["checkeds"].ToString() + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {

            TextBox txtUnitID = (TextBox)this.gvCharge.Rows[i].FindControl("txtUnitID");
            TextBox txtRentAmt = (TextBox)this.gvCharge.Rows[i].FindControl("txtRentAmt");
            DropDownList ddlRentType = (DropDownList)this.gvCharge.Rows[i].FindControl("ddlRentType");

            string strUnitID = txtUnitID.Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
            {
                if (checkeds.IndexOf("," + strUnitID + ",") < 0)
                {
                    checkeds += strUnitID + ",";
                    
                    for (int j = 0; j < ChargeDetailDT.Rows.Count; j++)
                    {
                        if (ChargeDetailDT.Rows[j]["UnitID"].ToString() == strUnitID)
                        {
                            ChargeDetailDT.Rows[j]["RentAmt"] = txtRentAmt.Text.Trim();

                            ChargeDetailDT.Rows[j]["RentType"] = ddlRentType.SelectedValue; 

                        }
                    }
                }
                else
                {
                    for (int j = 0; j < ChargeDetailDT.Rows.Count; j++)
                    {
                        if (ChargeDetailDT.Rows[j]["UnitID"].ToString() == strUnitID)
                        {
                            ChargeDetailDT.Rows[j]["RentAmt"] = txtRentAmt.Text.Trim();

                            ChargeDetailDT.Rows[j]["RentType"] = ddlRentType.SelectedValue; 


                        }
                    }
                }
            }
            else
            {
                //如果没选中则在串中去掉
                checkeds = checkeds.Replace("," + strUnitID + ",", ",");
            }
        }
        checkeds = checkeds.TrimEnd(',').TrimStart(',');
        ViewState["checkeds"] = checkeds;
    }
    protected void gvCharge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FindChecked();//记录选择
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        gvCharge.DataSource = ChargeDetailDT;
        gvCharge.DataBind();
        SetDataGridSelectRecords(ViewState["checkeds"].ToString());//设置选择项
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 提交审批
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPutIn_Click(object sender, EventArgs e)
    {

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["chgID"]);
        int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
        String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
        if (voucherHints == "") voucherHints = ViewState["voucherHints"].ToString();
        String voucherMemo = "";
        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
        try
        {
            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.QueryString["Sequence"].ToString()) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
            }
            String sql = "Update BudgetDetail set BudgetStatus = " + BudgetDetail.BDGSTATUS_TYPE_INGEAR + " where BatchID =" + voucherID;
            baseTrans.ExecuteUpdate(sql);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
        baseTrans.Commit();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        page(0);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ConfirmOK") + "'", true);
        this.btnPutIn.Enabled = false;
        this.btnSave.Enabled = false;
        btnBlankOut.Enabled = false;
    }

    protected void btnBlankOut_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        try
        {
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;

            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
            int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
            int voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
            String voucherMemo = "";

            if (baseTrans.ExecuteUpdate("UPDATE BudgetDetail SET BudgetStatus = " + BudgetDetail.BDGSTATUS_TYPE_END + " WHERE BatchID=" + voucherID) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                baseTrans.Rollback();
                return;
            }

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

            baseTrans.Commit();

            //删除cookies
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            Response.AppendCookie(cookies);
            this.btnPutIn.Enabled = false;
            this.btnBlankOut.Enabled = false;
            page(0);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
    }
    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        this.txtStartDate.Text = Convert.ToDateTime(this.ddlYears.SelectedValue.ToString() + "-01-01").ToString("yyyy-MM-dd");//开始日期
        this.txtEndDate.Text = Convert.ToDateTime(this.ddlYears.SelectedValue.ToString() + "-12-31").ToString("yyyy-MM-dd");//结束日期
    }

}

