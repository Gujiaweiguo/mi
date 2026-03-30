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
using BaseInfo.User;
using BaseInfo;
using BaseInfo.Store;

public partial class PosSystem_TaskProcessClass : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Srore_TaskProcessClassManage");
            BindGV();
            BindDrop();
        }
        btnSave.Attributes.Add("onclick", "return InputValidator()");
    }

    private void BindDrop()
    {
        this.ddlTaskStatus.Items.Clear();
        int[] classStatus = TaskProcessClass.GetClassStatus();
        for (int i = 0; i < classStatus.Length; i++)
        {
            ddlTaskStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", TaskProcessClass.GetClassStatusDesc(classStatus[i])), classStatus[i].ToString()));
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        DataTable dt = baseBo.QueryDataSet(new TaskProcessClass()).Tables[0];

        int count = dt.Rows.Count;
        dt.Columns.Add("Status");
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["Status"] = (String)GetGlobalResourceObject("BaseInfo", TaskProcessClass.GetClassStatusDesc(Convert.ToInt32(dt.Rows[j]["ClassStatus"])));
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvTaskProcessClass.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTaskProcessClass.DataSource = pds;
            gvTaskProcessClass.DataBind();
        }
        else
        {
            this.gvTaskProcessClass.DataSource = pds;
            this.gvTaskProcessClass.DataBind();
            spareRow = gvTaskProcessClass.Rows.Count;
            for (int i = 0; i < gvTaskProcessClass.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTaskProcessClass.DataSource = pds;
            gvTaskProcessClass.DataBind();
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        TaskProcessClass tPC = new TaskProcessClass();
        DataSet ds = new DataSet();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        baseBO.WhereClause = "ClassName='" + txtClassName.Text.Trim() + "'";
        ds = baseBO.QueryDataSet(tPC);
        if (ds.Tables[0].Rows.Count == 0)
        {
            ds = baseBO.QueryDataSet("select max(ClassID) from TaskProcessClass");
            string a = ds.Tables[0].Rows[0][0].ToString();
            if (ds.Tables[0].Rows[0][0].ToString() == "")
            {
                tPC.ClassID = 101;
            }
            else
            {
                tPC.ClassID = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) + 1;
            }
            tPC.ClassName = txtClassName.Text.Trim();
            tPC.ClassDesc = txtTaskDesc.Text.Trim();
            tPC.ClassStatus = ddlTaskStatus.SelectedValue.Trim();
            tPC.Node = txtNote.Text.Trim();
            tPC.CreateUserID = sessionUser.UserID;
            if (baseBO.Insert(tPC) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            BindGV();
            clearTxt();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Store_ClassName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            BindGV();
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        SessionUser sessionuser = (SessionUser)Session["UserAccessInfo"];
        TaskProcessClass tPC = new TaskProcessClass();
        baseBO.WhereClause = "ClassName='" + txtClassName.Text.Trim() + "'";
        ds = baseBO.QueryDataSet(tPC);
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0]["ClassID"].ToString().Trim() == ViewState["select"].ToString().Trim())
        {
            tPC.ClassName = txtClassName.Text.Trim();
            tPC.ClassDesc = txtTaskDesc.Text.Trim();
            tPC.ClassStatus = ddlTaskStatus.SelectedValue;
            tPC.Node = txtNote.Text.Trim();
            tPC.ModifyUserID = sessionuser.UserID;
            baseBO.WhereClause = "ClassID=" + Convert.ToInt32(ViewState["select"]);
            if (baseBO.Update(tPC) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                BindGV();
                clearTxt();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                BindGV();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Store_ClassName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            BindGV();
        }
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        clearTxt();
        BindGV();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
    protected void gvTaskProcessClass_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
        }
    }
    private void clearTxt()
    {
        txtClassName.Text = "";
        txtNote.Text = "";
        txtTaskDesc.Text = "";
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
    }
    protected void gvTaskProcessClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        baseBO.WhereClause = "ClassID=" + Convert.ToInt32(gvTaskProcessClass.SelectedRow.Cells[0].Text);
        ds = baseBO.QueryDataSet(new TaskProcessClass());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["select"] = ds.Tables[0].Rows[0]["ClassID"].ToString();
            txtClassName.Text = ds.Tables[0].Rows[0]["ClassName"].ToString();
            txtTaskDesc.Text = ds.Tables[0].Rows[0]["ClassDesc"].ToString();
            ddlTaskStatus.SelectedValue = ds.Tables[0].Rows[0]["ClassStatus"].ToString();
            txtNote.Text = ds.Tables[0].Rows[0]["Node"].ToString();
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
        }
        BindGV();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
    protected void gvTaskProcessClass_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
    }
}
