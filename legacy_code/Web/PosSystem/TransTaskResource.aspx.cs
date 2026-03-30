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
using System.Drawing;
using BaseInfo.Store;
using BaseInfo.User;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;

public partial class PosSystem_TransTaskResource : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowTree();
            BindGV();
            ViewState["code"] = "select dept.deptname,serverresource.* from serverresource,dept where resourceid=-1";
            BindGV1(ViewState["code"].ToString());
            ViewState["node"] = "";
            ViewState["selected"] = "";
        }
    }

    private void ShowTree()
    {
        string jsdept = "";
        BaseBO baseBo = new BaseBO();
        string strSql = @"SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
FROM 
		Dept where DeptType<7 and DeptType<>2
 Group  By PDeptID,CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
 ORDER BY Pdeptid,isnull(orderid,0) ";
        Dept objDept = new Dept();
        objDept.SetQuerySql(strSql);
        Resultset rs = baseBo.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept dept in rs)
            {
                jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                baseBo.WhereClause = "DeptID='" + dept.DeptID.ToString() + "'";
                Resultset rsServerRes = baseBo.Query(new ServerResource());
                if (rsServerRes.Count > 0)
                {
                    foreach (ServerResource serverRes in rsServerRes)
                    {
                        if (serverRes.STATUS.ToString() == "1")
                        {
                            jsdept += dept.DeptID.ToString() + serverRes.ResourceID.ToString() + "|" + dept.DeptID + "|" + serverRes.ResourceName + "|" + "" + "^";
                        }
                        else
                        {
                            jsdept += dept.DeptID.ToString() + serverRes.ResourceID.ToString() + "|" + dept.DeptID + "|" + serverRes.ResourceName + "|" + "../App_Themes/nlstree/img/node3.gif" + "^";
                        }
                    }
                }
            }
            depttxt.Value = jsdept;
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        DataTable dt = baseBo.QueryDataSet(new TransTask()).Tables[0];

        int count = dt.Rows.Count;
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvTransTask.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTransTask.DataSource = pds;
            gvTransTask.DataBind();
        }
        else
        {
            this.gvTransTask.DataSource = pds;
            this.gvTransTask.DataBind();
            spareRow = gvTransTask.Rows.Count;
            for (int i = 0; i < gvTransTask.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTransTask.DataSource = pds;
            gvTransTask.DataBind();
        }
    }

    private void BindGV1(string whereStr)
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        ServerResource serverResource = new ServerResource();
        DataSet ds = baseBo.QueryDataSet(whereStr);
        DataTable dt = ds.Tables[0];

        int count = dt.Rows.Count;
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvTransTaskRes.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTransTaskRes.DataSource = pds;
            gvTransTaskRes.DataBind();
        }
        else
        {
            this.gvTransTaskRes.DataSource = pds;
            this.gvTransTaskRes.DataBind();
            spareRow = gvTransTaskRes.Rows.Count;
            for (int i = 0; i < gvTransTaskRes.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTransTaskRes.DataSource = pds;
            gvTransTaskRes.DataBind();
        }
    }

    protected void gvTransTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[2].Text = "";
            }
        }
    }
    protected void gvTransTask_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindGV1(ViewState["code"].ToString());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void gvTransTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["selected"] = gvTransTask.SelectedRow.Cells[0].Text;
        btnSave.Enabled = true;
        BindGV();
        BindGV1(ViewState["code"].ToString());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["node"] = deptid.Value;
        BindGV();
        BindGV1("select dept.deptname,serverresource.* from serverresource,dept where resourceid=-1");
        foreach (GridViewRow grv in gvTransTask.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        if (ViewState["node"].ToString().Length == 6)
        {
            ViewState["code"] = "select dept.deptname,serverresource.* from serverresource,dept where serverresource.resourceid='" + ViewState["node"].ToString().Substring(3, 3) + "' and dept.deptid=serverresource.deptid";
            BindGV1(ViewState["code"].ToString());
        }
        if (ViewState["node"].ToString().Length == 3)
        {
            ViewState["code"] = "select dept.deptname,serverresource.* from serverresource,dept where serverresource.deptid=" + Convert.ToInt32(ViewState["node"].ToString()) + " and dept.deptid=serverresource.deptid";
            BindGV1(ViewState["code"].ToString());
        }
        if (ViewState["node"].ToString() == "")
        {
            BindGV1("select dept.deptname,serverresource.* from serverresource,dept where resourceid=-1");
        }
        BindGV();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void gvTransTaskRes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }

    protected void gvTransTaskRes_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        int i = baseBo.ExecuteUpdate("delete from serverresource where resourceid=" + Convert.ToInt32(gvTransTaskRes.SelectedRow.Cells[0].Text));
        if (i == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (string)GetGlobalResourceObject("BaseInfo", "Hidden_hidDelete") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (string)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
        }
        ViewState["node"] = "";
        BindGV();
        BindGV1(ViewState["code"].ToString());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //BindGV();
        //BindGV1("select dept.deptname,serverresource.* from serverresource,dept where resourceid=-1");
        //ShowTree();
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        Response.Redirect("TransTaskResource.aspx");

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        TransTaskRes transTaskRes = new TransTaskRes();
        if (ViewState["node"] == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请先选择添加数据源。'", true);
        }
        else if (ViewState["node"].ToString().Length == 6)
        {
            try
            {
                baseBo.ExecuteUpdate("delete from transtaskres where taskid=" + ViewState["selected"]);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + ex + "'", true);
            }
            transTaskRes.TaskID = Convert.ToInt32(ViewState["selected"].ToString());
            transTaskRes.ResourceID = Convert.ToInt32(ViewState["node"].ToString().Substring(3, 3));
            if (baseBo.Insert(transTaskRes) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else if (ViewState["node"].ToString().Length == 3)
        {
            DataSet ds = new DataSet();
            ds = baseBo.QueryDataSet("select resourceid from serverresource where deptid=" + Convert.ToInt32(ViewState["node"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    baseBo.ExecuteUpdate("delete from transtaskres where taskid=" + ViewState["selected"]);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + ex + "'", true);
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    transTaskRes.TaskID = Convert.ToInt32(ViewState["selected"].ToString());
                    transTaskRes.ResourceID = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    if (baseBo.Insert(transTaskRes) == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        return;
                    }
                }
            }
        }
        ViewState["selected"] = "";
        btnSave.Enabled = false;
        BindGV1(ViewState["code"].ToString());
        ViewState["node"] = "";
        BindGV();
        foreach (GridViewRow grv in gvTransTask.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
}
