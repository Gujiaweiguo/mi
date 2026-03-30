using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentableArea;
using BaseInfo.Dept;
using BaseInfo.authUser;
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Base.Page;
using Lease.PotCustLicense;
public partial class Lease_PotCustomer_PotContrast : BasePage
{
    public string baseinfo = "";
    public string pageTitle = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_PotContrast");
        if (!this.IsPostBack)
        {
            ViewState["WhereStr"] = " potshopUnit.unitid ='-112' ";
            page(ViewState["WhereStr"].ToString());
            unitpage("-111");
            ShowTree();
            if (Session["UnitID"] !=null)
            {
                string strdeptid = Session["UnitID"].ToString();
                if (strdeptid.ToString().Trim() != "")
                {
                    deptid.Value = strdeptid.ToString().Trim();
                    this.treeClick_Click();
                }
            }
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
        //if (e.Row.Cells.Count > 1)
        //{
        //    if (e.Row.Cells[0].Text != "&nbsp;")
        //    {
        //        e.Row.Cells[5].Text = "";
        //    }
            
        //}
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        BaseBO objBaseBo = new BaseBO();
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "" && ViewState["ID"].ToString().Length >= 12)
        {
            //选择location
            if (ViewState["ID"].ToString().Length == 12)
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9);
                ViewState["WhereStr"] = " potshopUnit.locationid ='" + strLocationID + "' ";
            }

            //选择单元
            if (ViewState["ID"].ToString().Length > 12)//选择单元
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                string strUnitID = ViewState["ID"].ToString().Substring(12);
                ViewState["WhereStr"] = " potshopUnit.unitid ='" + strUnitID + "' ";
            }
            page(ViewState["WhereStr"].ToString());

        }
        else
        {
            page(" potshopUnit.unitid ='-112' ");
        }
        unitpage("-111");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ClearGridViewSelect();
    }
    protected void treeClick_Click()
    {
        ViewState["ID"] = deptid.Value;
        BaseBO objBaseBo = new BaseBO();
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "" && ViewState["ID"].ToString().Length >= 12)
        {
            //选择location
            if (ViewState["ID"].ToString().Length == 12)
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9);
                ViewState["WhereStr"] = " potshopUnit.locationid ='" + strLocationID + "' ";
            }

            //选择单元
            if (ViewState["ID"].ToString().Length > 12)//选择单元
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strLocationID = ViewState["ID"].ToString().Substring(9, 3);
                string strUnitID = ViewState["ID"].ToString().Substring(12);
                ViewState["WhereStr"] = " potshopUnit.unitid ='" + strUnitID + "' ";
            }
            page(ViewState["WhereStr"].ToString());

        }
        else
        {
            page(" potshopUnit.unitid ='-112' ");
        }
        unitpage("-111");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ClearGridViewSelect();
    }
    
    private void SetGvClear()
    {
    }
    protected void page(string strWhere)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        PotShop objpotshop = new PotShop();
        string strSql = "select potshop.CustID,CustCode,CustShortName, Convert(char(10),potshop.ShopStartDate,120) as ShopStartDate," +
                        "       Convert(char(10),potshop.ShopEndDate,120) as ShopEndDate,RentalPrice " +
                        "       from potshop inner join potshopunit on(potshop.potshopID=potshopUnit.potshopid)" +
                        "       inner join customer on (potshop.custid=customer.custid)" +
                        "       where " + strWhere + " order by RentalPrice desc";

        objpotshop.SetQuerySql(strSql);
        DataSet ds = baseBO.QueryDataSet(objpotshop);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;

        for (int i = count; i < 16; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvpotshop.DataSource = dt;
        gvpotshop.DataBind();
        ClearGridViewSelect();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);

    }
    protected void gvpotshop_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sss = gvpotshop.SelectedRow.Cells[0].Text.ToString();
        unitpage("-111");
        unitpage(sss);
        ClearGridViewSelect();
    }
    protected void unitpage(string where)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        PotShop objpotshop = new PotShop();
        string strSql = "select potshopunit.unitid,unitcode,budget.chargetypeid,chargetypename,Unitprice from potshop " +
                       " inner join potshopunit on (potshop.potshopid=potshopunit.potshopid) " +
                       " inner join budget on (potshopunit.unitid=budget.unitid) " +
                       " inner join chargetype on (budget.chargetypeid=chargetype.chargetypeid) where potshop.custid='" + where + "'";

        objpotshop.SetQuerySql(strSql);
        DataSet ds = baseBO.QueryDataSet(objpotshop);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;

        for (int i = count; i < 16; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    
    }
    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in gvpotshop.Rows)
        {
            if (gvr.Cells[0].Text == "&nbsp;")
            {
                gvr.Cells[6].Text = "";
            }
        }
    }
}