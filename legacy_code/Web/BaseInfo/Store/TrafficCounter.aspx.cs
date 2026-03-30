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

public partial class BaseInfo_Store_TrafficCounter : BasePage
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
        Dept dept = new Dept();
        baseBo.WhereClause = "depttype=" + Dept.DEPT_TYPE_MALL;
        rs = baseBo.Query(dept);
        ddlStore.Items.Clear();
        foreach (Dept dep in rs)
        {
            ddlStore.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }

        Resultset rs1 = new Resultset();
        TrafficDoor trafficdoor = new TrafficDoor();
        baseBo.WhereClause = "storeid='" + ddlStore.SelectedValue + "'";
        rs1 = baseBo.Query(trafficdoor);
        ddlTrafficDoor.Items.Clear();
        foreach (TrafficDoor traffic in rs1)
        {
            ddlTrafficDoor.Items.Add(new ListItem(traffic.DoorName, traffic.DoorID.ToString()));
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
        string sql = @"select dept.deptid,trafficdoor.doorid,trafficcounter.status,dept.deptname,trafficdoor.doorname,trafficcounter.counterid,trafficcounter.name,case trafficcounter.status when 0 then '无效' when 1 then '有效' end statusname,trafficcounter.note
                        from trafficcounter
                        left join trafficdoor on trafficdoor.doorid=trafficcounter.doorid 
                        left join dept on dept.deptid=trafficcounter.storeid";
        DataTable dt = baseBo.QueryDataSet(sql).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvTrafficPoint.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTrafficPoint.DataSource = pds;
            gvTrafficPoint.DataBind();
        }
        else
        {
            this.gvTrafficPoint.DataSource = pds;
            this.gvTrafficPoint.DataBind();
            spareRow = gvTrafficPoint.Rows.Count;
            for (int i = 0; i < gvTrafficPoint.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTrafficPoint.DataSource = pds;
            gvTrafficPoint.DataBind();
        }
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
        ddlStore.SelectedValue = gvTrafficPoint.SelectedRow.Cells[0].Text;
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        TrafficDoor trafficdoor = new TrafficDoor();
        baseBo.WhereClause = "storeid='" + ddlStore.SelectedValue + "'";
        rs = baseBo.Query(trafficdoor);
        ddlTrafficDoor.Items.Clear();
        foreach (TrafficDoor traffic in rs)
        {
            ddlTrafficDoor.Items.Add(new ListItem(traffic.DoorName, traffic.DoorID.ToString()));
        }
        ddlTrafficDoor.SelectedValue = gvTrafficPoint.SelectedRow.Cells[1].Text;
        txtTrafficCounterID.Text = gvTrafficPoint.SelectedRow.Cells[5].Text;
        txtTrafficPoint.Text = gvTrafficPoint.SelectedRow.Cells[6].Text;
        ddlStatus.SelectedValue = gvTrafficPoint.SelectedRow.Cells[2].Text;
        if (gvTrafficPoint.SelectedRow.Cells[8].Text == "&nbsp;")
        {
            txtNote.Text = "";
        }
        else
        {
            txtNote.Text = gvTrafficPoint.SelectedRow.Cells[8].Text;
        }
        Session["id"] = gvTrafficPoint.SelectedRow.Cells[5].Text;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        clearSelect();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        TrafficCounter traffic = new TrafficCounter();
        baseBo.WhereClause = "counterid='" + txtTrafficCounterID.Text.Trim() + "'";
        ds = baseBo.QueryDataSet(traffic);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '编码已存在。'", true);
        }
        else
        {
            traffic.StoreID = Int32.Parse(ddlStore.SelectedValue.Trim());
            traffic.DoorID = Int32.Parse(ddlTrafficDoor.SelectedValue);
            traffic.CounterID = Int32.Parse(txtTrafficCounterID.Text);
            traffic.Name = txtTrafficPoint.Text;
            traffic.Status = Int32.Parse(ddlStatus.SelectedValue);
            traffic.Note = txtNote.Text.Trim();
            if (baseBo.Insert(traffic) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            BindGV();
            txtTrafficCounterID.Text = "";
            txtTrafficPoint.Text = "";
            txtNote.Text = "";
        }
        clearSelect();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        TrafficCounter traffic = new TrafficCounter();
        baseBo.WhereClause = "counterid='" + txtTrafficCounterID.Text.Trim() + "'";
        ds = baseBo.QueryDataSet(traffic);
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0]["counterid"].ToString().Trim() == Session["id"].ToString().Trim())
        {
            traffic.StoreID = Int32.Parse(ddlStore.SelectedValue);
            traffic.DoorID = Int32.Parse(ddlTrafficDoor.SelectedValue);
            traffic.CounterID = Int32.Parse(txtTrafficCounterID.Text);
            traffic.Name = txtTrafficPoint.Text;
            traffic.Status = Int32.Parse(ddlStatus.SelectedValue);
            traffic.Note = txtNote.Text;
            baseBo.WhereClause = "counterid='" + gvTrafficPoint.SelectedRow.Cells[5].Text + "'";
            if (baseBo.Update(traffic) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            BindGV();
            txtTrafficCounterID.Text = "";
            txtTrafficPoint.Text = "";
            txtNote.Text = "";
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = '编码已存在。'", true);
        }
        clearSelect();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrafficCounter.aspx");
    }
    private void clearSelect()
    {
        for (int i = 0; i < gvTrafficPoint.Rows.Count; i++)
        {
            if (gvTrafficPoint.Rows[i].Cells.Count > 1)
            {
                if (gvTrafficPoint.Rows[i].Cells[0].Text == "&nbsp;")
                {
                    gvTrafficPoint.Rows[i].Cells[9].Text = "";
                }
            }
        }
    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        TrafficDoor trafficdoor = new TrafficDoor();
        baseBo.WhereClause = "storeid='" + ddlStore.SelectedValue + "'";
        rs = baseBo.Query(trafficdoor);
        ddlTrafficDoor.Items.Clear();
        foreach (TrafficDoor traffic in rs)
        {
            ddlTrafficDoor.Items.Add(new ListItem(traffic.DoorName, traffic.DoorID.ToString()));
        }
        clearSelect();
    }
}
