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
using Base.Page;
using BaseInfo.User;
using BaseInfo.Store;
using BaseInfo.Dept;
using RentableArea;

public partial class BaseInfo_Store_TrafficDoor : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            BindGV();
            btnEdit.Enabled = false;
            BindDDl();
            btnSave.Attributes.Add("onclick", "return CheckData()");
            btnEdit.Attributes.Add("onclick", "return CheckData()");
            clearSelect();
        }
    }

    private void BindDDl()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        Dept dept=new Dept();
        baseBo.WhereClause = "depttype=" + Dept.DEPT_TYPE_MALL;
        rs = baseBo.Query(dept);
        ddlStore.Items.Clear();
        foreach (Dept dep in rs)
        {
            ddlStore.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }

        Resultset rs1 = new Resultset();
        Building building = new Building();
        baseBo.WhereClause = "Storeid='" + ddlStore.SelectedValue.Trim() + "'";
        rs1 = baseBo.Query(building);
        ddlBuilding.Items.Clear();
        foreach (Building build in rs1)
        {
            ddlBuilding.Items.Add(new ListItem(build.BuildingName, build.BuildingID.ToString()));
        }

        ddlStatus.Items.Clear();
        ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "SkuInfo_SkuYes"), "1"));
        ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "SkuInfo_SkuNo"), "0"));
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        string sql = @"select trafficdoor.doorid,dept.deptname,building.buildingname,trafficdoor.doorname,case trafficdoor.doorstatus when 0 then '无效' when 1 then '有效' end doorstatus,trafficdoor.note
                        from trafficdoor
                        left join dept on dept.deptid=trafficdoor.storeid
                        left join building on building.buildingid=trafficdoor.buildingid";
        DataTable dt = baseBo.QueryDataSet(sql).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvTrafficEntry.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTrafficEntry.DataSource = pds;
            gvTrafficEntry.DataBind();
        }
        else
        {
            this.gvTrafficEntry.DataSource = pds;
            this.gvTrafficEntry.DataBind();
            spareRow = gvTrafficEntry.Rows.Count;
            for (int i = 0; i < gvTrafficEntry.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTrafficEntry.DataSource = pds;
            gvTrafficEntry.DataBind();
        }
    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        Building building = new Building();
        baseBo.WhereClause = "Storeid='" + ddlStore.SelectedValue.Trim() + "'";
        rs = baseBo.Query(building);
        ddlBuilding.Items.Clear();
        foreach (Building build in rs)
        {
            ddlBuilding.Items.Add(new ListItem(build.BuildingName, build.BuildingID.ToString()));
        }
        clearSelect();
    }

    protected void gvTrafficEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindGV();
        clearSelect();
    }
    protected void gvTrafficEntry_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO basebo = new BaseBO();
        DataSet ds = new DataSet();
        basebo.WhereClause = "doorid='" + gvTrafficEntry.SelectedRow.Cells[0].Text.Trim() + "'";
        ds = basebo.QueryDataSet(new TrafficDoor());
        ddlStore.SelectedValue = ds.Tables[0].Rows[0]["storeid"].ToString().Trim();

        Resultset rs = new Resultset();
        Building building = new Building();
        basebo.WhereClause = "Storeid='" + ddlStore.SelectedValue.Trim() + "'";
        rs = basebo.Query(building);
        ddlBuilding.Items.Clear();
        foreach (Building build in rs)
        {
            ddlBuilding.Items.Add(new ListItem(build.BuildingName, build.BuildingID.ToString()));
        }
        ddlBuilding.SelectedValue = ds.Tables[0].Rows[0]["buildingid"].ToString().Trim();
        txtEntryName.Text = ds.Tables[0].Rows[0]["doorname"].ToString();
        txtNote.Text = ds.Tables[0].Rows[0]["note"].ToString();
        ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["doorstatus"].ToString();
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        clearSelect();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        TrafficDoor traffic = new TrafficDoor();
        traffic.StoreID = Int32.Parse(ddlStore.SelectedValue);
        traffic.BuildingID = Int32.Parse(ddlBuilding.SelectedValue);
        traffic.DoorName = txtEntryName.Text.Trim();
        traffic.DoorStatus = Int32.Parse(ddlStatus.SelectedValue);
        traffic.Note = txtNote.Text;
        if (baseBo.Insert(traffic) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
        txtEntryName.Text = "";
        txtNote.Text = "";
        clearSelect();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        TrafficDoor traffic = new TrafficDoor();
        traffic.StoreID = Int32.Parse(ddlStore.SelectedValue);
        traffic.BuildingID = Int32.Parse(ddlBuilding.SelectedValue);
        traffic.DoorName = txtEntryName.Text.Trim();
        traffic.DoorStatus = Int32.Parse(ddlStatus.SelectedValue);
        traffic.Note = txtNote.Text;
        baseBo.WhereClause = "doorid='" + gvTrafficEntry.SelectedRow.Cells[0].Text + "'";
        if (baseBo.Update(traffic) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
        txtEntryName.Text = "";
        txtNote.Text = "";
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        clearSelect();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrafficDoor.aspx");
    }

    private void clearSelect()
    {
        for (int i = 0; i < gvTrafficEntry.Rows.Count; i++)
        {
            if (gvTrafficEntry.Rows[i].Cells.Count>1)
            {
                if (gvTrafficEntry.Rows[i].Cells[0].Text == "&nbsp;")
                {
                    gvTrafficEntry.Rows[i].Cells[6].Text = "";
                }
            }
        }
    }
}
