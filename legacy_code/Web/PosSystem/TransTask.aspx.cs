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

using BaseInfo.Store;
using BaseInfo.User;
using Base.DB;
using Base.Page;
using Base;
using Base.Biz;

public partial class PosSystem_TransTask : BasePage
{
    public string baseInfo;
    public string source;
    public string planTask;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (string)GetGlobalResourceObject("BaseInfo", "Store_Task");
            source = (string)GetGlobalResourceObject("BaseInfo", "Store_ServerResource");
            planTask = (string)GetGlobalResourceObject("BaseInfo", "Store_TaskPlan");
            BindGV();
            BindDrop();
            ViewState["Selected"] = "";
        }
        this.btnSave.Attributes.Add("onclick", "return textBoxEmCheck()");
    }

    private void BindDrop()
    {
        this.ddlTaskType.Items.Clear();
        int[] taskType = TransTask.GetTaskType();
        for (int i = 0; i < taskType.Length; i++)
        {
            ddlTaskType.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", TransTask.GetTaskTypeDec(taskType[i])), taskType[i].ToString()));
        }

        this.ddlTaskStatus.Items.Clear();
        int[] taskStatus = TransTask.GetTaskStatus();
        for (int i = 0; i < taskStatus.Length; i++)
        {
            ddlTaskStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", TransTask.GetTaskStatusDesc(taskStatus[i])), taskStatus[i].ToString()));
        }

        this.ddlPriority.Items.Clear();
        int[] priority = TransTask.GetPriority();
        for (int i = 0; i < priority.Length; i++)
        {
            ddlPriority.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", TransTask.GetPriorityDec(priority[i])), priority[i].ToString()));
        }

        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ClassStatus=" + TaskProcessClass.CLASSSTATUS_YES;
        DataSet ds = new DataSet();
        ds = baseBO.QueryDataSet(new TaskProcessClass());
        this.ddlProcessClass.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlProcessClass.DataSource = ds.Tables[0];
            ddlProcessClass.DataValueField = ds.Tables[0].Columns["ClassID"].ToString();
            ddlProcessClass.DataTextField = ds.Tables[0].Columns["ClassDesc"].ToString();
            ddlProcessClass.DataBind();
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
    }
    protected void gvTransTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        baseBo.WhereClause = "TaskName='" + gvTransTask.SelectedRow.Cells[1].Text.ToString() + "'";
        ds = baseBo.QueryDataSet(new TransTask());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Selected"] = ds.Tables[0].Rows[0]["TaskID"].ToString();
            txtTaskName.Text = ds.Tables[0].Rows[0]["TaskName"].ToString();
            ddlProcessClass.SelectedValue = ds.Tables[0].Rows[0]["ClassID"].ToString();
            txtNote.Text = ds.Tables[0].Rows[0]["Node"].ToString();
            ddlTaskType.SelectedValue = ds.Tables[0].Rows[0]["TaskType"].ToString();
            ddlTaskStatus.SelectedValue = ds.Tables[0].Rows[0]["TaskStatus"].ToString();
            ddlPriority.SelectedValue = ds.Tables[0].Rows[0]["Priority"].ToString();
            if (ds.Tables[0].Rows[0]["Retry"].ToString() == "0")
            {
                rbtRetryNo.Checked = true;
                rbtRetryYes.Checked = false;
            }
            else if (Convert.ToInt32(ds.Tables[0].Rows[0]["Retry"]) > 0)
            {
                rbtRetryYes.Checked = true;
                rbtRetryNo.Checked = false;
            }
        }
        BindGV();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        clearText();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        TransTask transTask = new TransTask();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        if (ViewState["Selected"].ToString() == "")
        {
            baseBo.WhereClause = "TaskName='" + txtTaskName.Text.Trim() + "'";
            ds = baseBo.QueryDataSet(transTask);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Store_TaskName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
            }
            else
            {
                ds = baseBo.QueryDataSet("select max(TaskID) from TransTask");
                if (ds.Tables[0].Rows[0][0].ToString()=="")
                {
                    transTask.TaskID = 101;
                }
                else
                {
                    transTask.TaskID = Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1;
                }
                transTask.TaskName = txtTaskName.Text.Trim();
                transTask.ClassID = Convert.ToInt32(ddlProcessClass.SelectedValue.Trim());
                transTask.Node = txtNote.Text.Trim();
                transTask.TaskType = ddlTaskType.SelectedValue.ToString().Trim();
                transTask.TaskStatus = ddlTaskStatus.SelectedValue.ToString().Trim();
                transTask.Priority = Convert.ToInt32(ddlPriority.SelectedValue.ToString().Trim());
                transTask.Retry = (rbtRetryNo.Checked == true) ? 0 : 1;
                transTask.CreateUserID = sessionUser.UserID;
                if (baseBo.Insert(transTask) == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                    clearText();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
        }
        else
        {
            baseBo.WhereClause = "TaskName='" + txtTaskName.Text.Trim() + "'";
            ds=baseBo.QueryDataSet(transTask);
            if (ds.Tables[0].Rows.Count == 0 || ViewState["Selected"].ToString().Trim() == ds.Tables[0].Rows[0]["TaskID"].ToString().Trim())
            {
                transTask.TaskName = txtTaskName.Text.Trim();
                transTask.ClassID = Convert.ToInt32(ddlProcessClass.SelectedValue.Trim());
                transTask.Node = txtNote.Text.Trim();
                transTask.TaskType = ddlTaskType.SelectedValue.ToString().Trim();
                transTask.TaskStatus = ddlTaskStatus.SelectedValue.ToString().Trim();
                transTask.Priority = Convert.ToInt32(ddlPriority.SelectedValue.ToString().Trim());
                transTask.Retry = (rbtRetryNo.Checked == true) ? 0 : 1;
                transTask.ModifyUserID = sessionUser.UserID;
                baseBo.WhereClause = "TaskID='" + ViewState["Selected"] + "'";
                if (baseBo.Update(transTask) == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                    clearText();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Store_TaskName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
            }
        }
        BindGV();
    }
    private void clearText()
    {
        txtTaskName.Text = "";
        txtNote.Text = "";
        ddlTaskType.SelectedValue = "0";
        ddlTaskStatus.SelectedValue = "0";
        ddlPriority.SelectedValue = "0";
        ViewState["Selected"] = "";
        BindGV();
    }
}
