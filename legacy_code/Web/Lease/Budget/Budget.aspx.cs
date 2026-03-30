using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Invoice;
using RentableArea;
using BaseInfo.Dept;
using BaseInfo.authUser;
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Base.Page;

public partial class Lease_Budget_Budget : BasePage
{
    public string baseinfo = "";
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BudgetInsert");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            ViewState["WhereStr"] = " AND Unit.UnitID='-11'";
            page(ViewState["WhereStr"].ToString());
            ShowTree();
        }
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
    // 保存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        InsertBudget(ViewState["WhereStr"].ToString());
    }
    protected void gvBudget_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            
            TextBox txtUnitID = (TextBox)e.Row.FindControl("txtUnitID");
            if (txtUnitID.Text.Trim() == "")
            {

                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitCode")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtFloorArea")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUseArea")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtShopTypeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtTradeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitTypeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtRentAmt")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtRentType")).Visible = false;
            }

        }
    }
    // 添加
    protected void btnAdd_Click(object sender, EventArgs e)
    {     
        BaseBO objBaseBo = new BaseBO();
        string strNum = "";
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "" && ViewState["ID"].ToString().Length>=6)
        { 
            //选择单元
            if (ViewState["ID"].ToString().Length > 12)//选择单元
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                string strUnitID = ViewState["ID"].ToString().Substring(12);
                ViewState["WhereStr"] = " AND Unit.UnitID='" + strUnitID + "'";
            }
            if (ViewState["ID"].ToString().Length == 12)//选择方位
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                ViewState["WhereStr"] = " AND Unit.LocationID='" + strLocationID + "'";
            }
            // 选择楼层
            if (ViewState["ID"].ToString().Length == 9)//选择楼层
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                ViewState["WhereStr"] = " AND Unit.FloorID='" + strFloorID + "'";
            }
            // 选择大楼
            if (ViewState["ID"].ToString().Length == 6)//选择大楼
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                ViewState["WhereStr"] = " AND Unit.BuildingID='" + strBuildingID + "'";
            }
            page(ViewState["WhereStr"].ToString());
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        if (ViewState["ID"].ToString().Length >= 6)
        {
            this.btnQuery.Enabled = true;
            this.btnSave.Enabled = true;
        }
        else
        {
            this.btnQuery.Enabled = false;
            this.btnSave.Enabled = false;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        page(" AND Unit.UnitID='-11'");
    }
    private void SetGvClear()
    {
    }
    protected void page(string strWhere)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        BudgetDetail objBudgetDetail = new BudgetDetail();
        string strSql = " SELECT  BudgetDetail.BudgetID,BudgetDetail.BatchID,BudgetDetail.UnitID,BudgetDetail.FloorArea,BudgetDetail.UseArea," +
                        "         BudgetDetail.ShopTypeID,BudgetDetail.TradeID,BudgetDetail.UnitTypeID, BudgetYear ," +
                        "         Convert(char(10),BudgetDetail.StartDate,120) as StartDate," +
                        "         Convert(char(10),BudgetDetail.EndDate,120) as EndDate," +
                        "         BudgetDetail.ChargeTypeID,BudgetDetail.RentAmt,BudgetDetail.BudgetStatus," +
                        " Case                                                                                 " +
                        " When    BudgetDetail.RentType='D' Then '日' " +
                        " When    BudgetDetail.RentType='M' Then '月'" +
                        " When    BudgetDetail.RentType='Y' Then '年' " +
                        " End As RentType," +
                        "         ShopType.ShopTypeName,TradeRelation.TradeName,UnitType.UnitTYpeName,Unit.UnitCode" +
                        " FROM    BudgetDetail INNER JOIN ShopType ON (BudgetDetail.ShopTypeID=ShopType.ShopTypeID) " +
                        "         INNER JOIN TradeRelation ON (BudgetDetail.TradeID=TradeRelation.TradeID) " +
                        "         INNER JOIN UnitType ON (BudgetDetail.UnitTypeID=UnitType.UnitTypeID)" +
                        "         INNER JOIN Unit ON(BudgetDetail.UnitID=Unit.UnitID)" +
                        " WHERE   BudgetDetail.BudgetStatus=2 " + strWhere;

        objBudgetDetail.SetQuerySql(strSql);
        DataSet ds = baseBO.QueryDataSet(objBudgetDetail);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;

        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvBudget.DataSource = dt;
        gvBudget.DataBind();
    }
    private void InsertBudget(string strWhere)
    {
        DataSet ds = new DataSet();
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        Budget budget = new Budget();
        BaseBO baseBO = new BaseBO();
        String WhereSql = "";
        WhereSql = "Select " +
                        " BudgetDetail.BudgetID,BudgetDetail.BatchID,BudgetDetail.UnitID,Unit.StoreID,Unit.BuildingID,Unit.FloorID," +
                        " Unit.AreaID,Unit.LocationID,Unit.MainLocationID,BudgetDetail.TradeID,BudgetDetail.ShopTypeID," +
                        " BudgetDetail.UnitTypeID,BudgetDetail.FloorArea,BudgetDetail.UseArea,BudgetDetail.BudgetYear," +
                        " BudgetDetail.StartDate,BudgetDetail.EndDate,BudgetDetail.ChargeTypeID," +
                        " Case " +
                        " When BudgetDetail.RentType='D' Then BudgetDetail.UseArea*BudgetDetail.RentAmt*365/12 " +
                        " When BudgetDetail.RentType='M' Then BudgetDetail.UseArea*BudgetDetail.RentAmt" +
                        " When BudgetDetail.RentType='Y' Then BudgetDetail.UseArea*BudgetDetail.RentAmt/12 " +
                        " End As MonthAmt," +
                        " Case " +
                        " When BudgetDetail.RentType='D' Then BudgetDetail.UseArea*BudgetDetail.RentAmt*365" +
                        " When BudgetDetail.RentType='M' Then BudgetDetail.UseArea*BudgetDetail.RentAmt*12" +
                        " When BudgetDetail.RentType='Y' Then BudgetDetail.UseArea*BudgetDetail.RentAmt" +
                        " End As YearAmt," +
                        " Case " +
                        " When BudgetDetail.RentType='D' Then BudgetDetail.UseArea*BudgetDetail.RentAmt*365/365/BudgetDetail.UseArea" +
                        " When BudgetDetail.RentType='M' Then BudgetDetail.UseArea*BudgetDetail.RentAmt*12/365/BudgetDetail.UseArea" +
                        " When BudgetDetail.RentType='Y' Then BudgetDetail.UseArea*BudgetDetail.RentAmt/365/BudgetDetail.UseArea" +
                        " End As UnitPrice," +
                        " BudgetDetail.BudgetStatus,BudgetDetail.CreateUserID,BudgetDetail.CreateTime,BudgetDetail.ModifyUserID," +
                        " BudgetDetail.ModifyTime,BudgetDetail.OprRoleID,BudgetDetail.OprDeptID" +
                    " From " +
                        " BudgetDetail INNER JOIN Unit ON (BudgetDetail.UnitID=Unit.UnitID)" +
                    " Where BudgetDetail.BudgetStatus=2" + strWhere;
        ds = baseBO.QueryDataSet(WhereSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            baseTrans.BeginTrans();
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                budget.BatchID = Convert.ToInt32(ds.Tables[0].Rows[j]["BatchID"]);
                budget.UnitID = Convert.ToInt32(ds.Tables[0].Rows[j]["UnitID"]);
                budget.StoreID = Convert.ToInt32(ds.Tables[0].Rows[j]["StoreID"]);
                budget.BuildingID = Convert.ToInt32(ds.Tables[0].Rows[j]["BuildingID"]);
                budget.FloorID = Convert.ToInt32(ds.Tables[0].Rows[j]["FloorID"]);
                budget.AreaID = Convert.ToInt32(ds.Tables[0].Rows[j]["AreaID"]);
                budget.LocationID = Convert.ToInt32(ds.Tables[0].Rows[j]["LocationID"]);
                budget.MainLocationID = Convert.ToInt32(ds.Tables[0].Rows[j]["MainLocationID"]);
                budget.TradeID = Convert.ToInt32(ds.Tables[0].Rows[j]["TradeID"]);
                budget.ShopTypeID = Convert.ToInt32(ds.Tables[0].Rows[j]["ShopTypeID"]);
                budget.UnitTypeID = Convert.ToInt32(ds.Tables[0].Rows[j]["UnitTypeID"]);
                budget.FloorArea = Convert.ToInt32(ds.Tables[0].Rows[j]["FloorArea"]);
                budget.UseArea = Convert.ToInt32(ds.Tables[0].Rows[j]["UseArea"]);
                budget.BudgetYear = Convert.ToInt32(ds.Tables[0].Rows[j]["BudgetYear"]);
                budget.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[j]["StartDate"]);
                budget.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[j]["EndDate"]);
                budget.ChargeTypeID = Convert.ToInt32(ds.Tables[0].Rows[j]["ChargeTypeID"]);
                budget.MonthAmt = Convert.ToInt32(ds.Tables[0].Rows[j]["MonthAmt"]);
                budget.YearAmt = Convert.ToInt32(ds.Tables[0].Rows[j]["YearAmt"]);
                budget.UnitPrice = Convert.ToInt32(ds.Tables[0].Rows[j]["UnitPrice"]);
                budget.BudgetStatus = Convert.ToInt32(Budget.BudgetSTATUS_YES);
                budget.MonthAmt = Convert.ToInt32(ds.Tables[0].Rows[j]["MonthAmt"]);

                budget.CreateUserId = sessionUser.UserID;
                budget.CreateTime = DateTime.Now;
                budget.ModifyUserId = sessionUser.UserID;
                budget.ModifyTime = DateTime.Now;
                budget.OprRoleID = sessionUser.OprRoleID;
                budget.OprDeptID = sessionUser.OprDeptID;

                if (baseTrans.Insert(budget) < 0)
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                    return;
                }
                String sql = "Update BudgetDetail set BudgetStatus = " + BudgetDetail.BDGSTATUS_TYPE_CREATE + " where BudgetID =" + Convert.ToInt32(ds.Tables[0].Rows[j]["BudgetID"]);
                if (baseTrans.ExecuteUpdate(sql) < 0)
                {
                    baseTrans.Rollback();
                    return;
                }
            }

            baseTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            page(" AND Unit.UnitID='-11'");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        page(" AND Unit.UnitID='-11'");
    }
}

