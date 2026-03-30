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

using BaseInfo.User;
using BaseInfo.Store;
using Base.Biz;
using Base.DB;
using Base.Page;
public partial class PosSystem_TransTaskPlan : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();
            BindDrop();
            ViewState["flag"] = "";
        }
        btnSave.Attributes.Add("onclick", "return textBoxCheck()");
    }

    private void BindDrop()
    {
        this.ddlRunType.Items.Clear();
        int[] runType = TransTaskPlan.GetRunType();
        for (int i = 0; i < runType.Length; i++)
        {
            this.ddlRunType.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", TransTaskPlan.GetRunTypeDesc(runType[i])), runType[i].ToString()));
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
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        ds = baseBo.QueryDataSet("select * from TransTaskPlan where TaskID=" + Convert.ToInt32(gvTransTask.SelectedRow.Cells[0].Text.Trim()));
        clearText();
        txtTaskName.Text = gvTransTask.SelectedRow.Cells[1].Text.Trim();
        txtTaskName.Enabled = false;
        btnSave.Enabled = true;
        if (ds.Tables[0].Rows.Count == 0)
        {
            
        }
        else
        {
            ddlRunType.SelectedValue = ds.Tables[0].Rows[0]["RunType"].ToString().Trim();
            txtStartDate.Text =Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"]).ToShortDateString();
            if (ds.Tables[0].Rows[0]["EndDateFlag"].ToString()=="0")
            {
                chbNoEndDate.Checked = true;
                txtEndDate.Text = "";
            }
            else
            {
                txtEndDate.Text =Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"]).ToShortDateString();
                chbNoEndDate.Checked = false;
            }
            txtStartTimeH.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"]).ToLongTimeString().Substring(0, (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"]).ToLongTimeString().IndexOf(":")));
            txtStartTimeM.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"]).ToLongTimeString().Substring((Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"]).ToLongTimeString().IndexOf(":")) + 1, 2);
            txtEndTimeH.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]).ToLongTimeString().Substring(0, (Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]).ToLongTimeString().IndexOf(":")));
            txtEndTimeM.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]).ToLongTimeString().Substring((Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]).ToLongTimeString().IndexOf(":")) + 1, 2);
            ViewState["flag"] = 1;
        }
        BindGV();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        clearText();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(txtEndTimeH.Text) >= 24 || Convert.ToInt32(txtStartTimeH.Text) >= 24 || Convert.ToInt32(txtStartTimeM.Text) >= 60 || Convert.ToInt32(txtEndTimeM.Text) >= 60)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '时间范围有误！'", true);
            //BindGV();
        }
        else if (chbNoEndDate.Checked==false && txtEndDate.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '结束日期不能为空！'", true);
        }
        else
        {
            BaseBO baseBo = new BaseBO();
            TransTaskPlan transTaskPlan = new TransTaskPlan();
            transTaskPlan.TaskID = Convert.ToInt32(gvTransTask.SelectedRow.Cells[0].Text);
            transTaskPlan.PlanID = Convert.ToInt32(gvTransTask.SelectedRow.Cells[0].Text);
            transTaskPlan.RunType = ddlRunType.SelectedValue.ToString().Trim();
            transTaskPlan.StartDate = Convert.ToDateTime(txtStartDate.Text);
            if (chbNoEndDate.Checked == false)
            {
                transTaskPlan.EndDate = Convert.ToDateTime(txtEndDate.Text);
                transTaskPlan.EndTime = Convert.ToDateTime(txtEndDate.Text.Trim() + " " + txtEndTimeH.Text + ":" + txtEndTimeM.Text);
                transTaskPlan.EndDateFlag = 1;
            }
            else
            {
                transTaskPlan.EndTime = Convert.ToDateTime(txtStartDate.Text.Trim() + " " + txtEndTimeH.Text + ":" + txtEndTimeM.Text);
                transTaskPlan.EndDateFlag = 0;
            }
            transTaskPlan.StartTime = Convert.ToDateTime(txtStartDate.Text.Trim() + " " + txtStartTimeH.Text + ":" + txtStartTimeM.Text);
            baseBo.WhereClause = "TaskID=" + Convert.ToInt32(gvTransTask.SelectedRow.Cells[0].Text);

            if (((ViewState["flag"].ToString() == "") ? baseBo.Insert(transTaskPlan) : baseBo.Update(transTaskPlan)) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                clearText();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        foreach (GridViewRow gvr in gvTransTask.Rows)
        {
            if (gvr.Cells.Count > 1)
            {
                if (gvr.Cells[1].Text == "&nbsp;")
                {
                    gvr.Cells[2].Text = "";
                }
            }
        }
    }
    private void clearText()
    {
        txtTaskName.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtStartTimeH.Text = "";
        txtStartTimeM.Text = "";
        txtEndTimeH.Text = "";
        txtEndTimeM.Text = "";
        chbNoEndDate.Checked = false;
        btnSave.Enabled = false;
        BindGV();
    }
}
