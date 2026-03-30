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

public partial class Lease_Budget_BudgetExamine : BasePage
{
    public string baseinfo = "";
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BudgetExamine");
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
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitTypeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitPrice")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtMonthAmt")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtYearAmt")).Visible = false;
                
            }

        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        BaseBO objBaseBo = new BaseBO();
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "" && ViewState["ID"].ToString().Length >= 6)
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
            
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    private void SetGvClear()
    {
    }
    protected void page(string strWhere)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        Budget objBudget = new Budget();
        string strSql = "SELECT Budget.BatchID,Budget.UnitID,Unit.UnitCode,Budget.FloorArea,Budget.UseArea,Budget.UnitTypeID, " +
                        "      UnitType.UnitTypeName,Budget.UnitPrice,Budget.MonthAmt,Budget.YearAmt "+
                       "FROM   Budget "+
                       "INNER JOIN Unit ON (Budget.UnitID=Unit.UnitID) "+
                       "INNER JOIN UnitType ON (Budget.UnitTypeID=UnitType.UnitTypeID) WHERE BudgetStatus=1 " + strWhere;

        objBudget.SetQuerySql(strSql);
        DataSet ds = baseBO.QueryDataSet(objBudget);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;

        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvBudget.DataSource = dt;
        gvBudget.DataBind();
    }
}
