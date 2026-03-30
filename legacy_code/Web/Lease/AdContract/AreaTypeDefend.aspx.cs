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
using Lease.Formula;
using Base.Page;
using Lease.AdContract;
using BaseInfo.authUser;
using BaseInfo.User;
using System.Drawing;
using Lease.PotCust;

public partial class Lease_AdContract_AreaTypeDefend : BasePage
{
    public string baseInfo;
    public string enterInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "LeaseAreaType_AreaTypeDefend");
            this.btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            this.btnEdit.Attributes.Add("onclick", "return InputValidator(form1)");
            enterInfo = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            this.BindDDL();//绑定下拉列表框
            this.BindGV();
            this.btnEdit.Enabled = false;
        }
    }
    /// <summary>
    /// 绑定下拉列表框
    /// </summary>
    private void BindDDL()
    {
        int[] status = CustType.GetCustTypeStatus();
        this.ddlStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            this.ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindGV()
    {
        BaseBO objBaseBo = new BaseBO();
        AreaType objType = new AreaType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objType,this.gvAdBoard);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();

        AreaType objType = new AreaType();
        objType.AreaTypeID = BaseApp.GetID("AreaType", "AreaTypeID");
        objType.AreaTypeCode = this.txtAdBordCode.Text.Trim();
        objType.AreaTypeDesc = this.txtAdBordName.Text.Trim();
        try { objType.AreaTypeStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        if (objBaseBo.Insert(objType) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        BindGV();
        this.txtAdBordCode.Text = "";
        this.txtAdBordName.Text = "";
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        AreaType objType = new AreaType();
        objType.AreaTypeCode = this.txtAdBordCode.Text.Trim();
        objType.AreaTypeDesc = this.txtAdBordName.Text.Trim();
        try { objType.AreaTypeStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        objBaseBo.WhereClause = "AreaTypeID='" + ViewState["AreaTypeID"].ToString() + "'";
        if (objBaseBo.Update(objType) == 1)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        BindGV();
        this.txtAdBordCode.Text = "";
        this.txtAdBordName.Text = "";
        this.btnEdit.Enabled = false;
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.txtAdBordCode.Text = "";
        this.txtAdBordName.Text = "";
        BindGV();
        this.btnEdit.Enabled = false;
    }
    
    protected void gvAdBoard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
            if (e.Row.Cells[3].Text == "1")
                e.Row.Cells[4].Text = "有效";
            if (e.Row.Cells[3].Text == "0")
                e.Row.Cells[4].Text = "无效";
        }
    }
    protected void gvAdBoard_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["AreaTypeID"] = this.gvAdBoard.SelectedRow.Cells[0].Text.Trim();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "AreaTypeID='" + this.gvAdBoard.SelectedRow.Cells[0].Text.Trim() + "'";
        Resultset rs = objBaseBo.Query(new AreaType());
        if (rs.Count == 1)
        {
            AreaType objType = rs.Dequeue() as AreaType;
            this.txtAdBordCode.Text = objType.AreaTypeCode;// this.gvAdBoard.SelectedRow.Cells[1].Text.Trim();
            this.txtAdBordName.Text = objType.AreaTypeDesc;// this.gvAdBoard.SelectedRow.Cells[2].Text.Trim();
            this.ddlStatus.SelectedValue = objType.AreaTypeStatus.ToString();// this.gvAdBoard.SelectedRow.Cells[5].Text.Trim();
        }
        this.btnEdit.Enabled = true;
        this.BindGV();
    }
    protected void gvAdBoard_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindGV();
        foreach (GridViewRow grv in this.gvAdBoard.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
