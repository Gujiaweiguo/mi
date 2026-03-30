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

public partial class Lease_AdContract_AdBoardTypeDefend : BasePage
{
    public string baseInfo;
    public string enterInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "AdContract_lblAdTypeDefend");
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
        AdBoardType objType = new AdBoardType();
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
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        
        AdBoardType objAdBorad = new AdBoardType();
        objAdBorad.AdBoardTypeID = BaseApp.GetID("AdBoardType", "AdBoardTypeID");
        objAdBorad.AdBoardTypeCode = this.txtAdBordCode.Text.Trim();
        objAdBorad.AdBoardTypeName = this.txtAdBordName.Text.Trim();
        try { objAdBorad.AdBoardTypeStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        objAdBorad.CreateTime = DateTime.Now;
        objAdBorad.CreateUserID = sessionUser.CreateUserID;
        objAdBorad.OprDeptID = sessionUser.OprDeptID;
        objAdBorad.OprRoleID = sessionUser.OprRoleID;
        if (objBaseBo.Insert(objAdBorad) == 1)
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
        AdBoardType objAdBoard = new AdBoardType();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        objAdBoard.AdBoardTypeCode = this.txtAdBordCode.Text.Trim();
        objAdBoard.AdBoardTypeName = this.txtAdBordName.Text.Trim();
        try { objAdBoard.AdBoardTypeStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        objAdBoard.ModifyTime = sessionUser.ModifyTime;
        objAdBoard.ModifyUserID = sessionUser.ModifyUserID;
        objAdBoard.OprDeptID = sessionUser.OprDeptID;
        objAdBoard.OprRoleID = sessionUser.OprRoleID;
        objBaseBo.WhereClause = "AdBoardTypeID='" + ViewState["AdBoardTypeID"].ToString() + "'";
        if(objBaseBo.Update(objAdBoard)==1)
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
        this.btnEdit.Enabled = false;
        BindGV();
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
        ViewState["AdBoardTypeID"] = this.gvAdBoard.SelectedRow.Cells[0].Text.Trim();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "AdBoardTypeID='" + this.gvAdBoard.SelectedRow.Cells[0].Text.Trim() + "'";
        Resultset rs = objBaseBo.Query(new AdBoardType());
        if (rs.Count == 1)
        {
            AdBoardType objType = rs.Dequeue() as AdBoardType;
            this.txtAdBordCode.Text = objType.AdBoardTypeCode;// this.gvAdBoard.SelectedRow.Cells[1].Text.Trim();
            this.txtAdBordName.Text = objType.AdBoardTypeName;// this.gvAdBoard.SelectedRow.Cells[2].Text.Trim();
            this.ddlStatus.SelectedValue = objType.AdBoardTypeStatus.ToString();// this.gvAdBoard.SelectedRow.Cells[5].Text.Trim();
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
