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
using Base.Page;
using BaseInfo.Store;
using Lease.PotCust;
using BaseInfo.User;
using System.Drawing;

public partial class BaseInfo_Store_StoreManageType : BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetControlPro();//添加按钮属性
        if (!this.IsPostBack)
        {
            this.BindSourceTypeStatus();//绑定是否有效下拉框
            this.BindData();//绑定GridView
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreManageType");
            btnEdit.Enabled = false;
        }
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        this.btnSave.Attributes.Add("onclick", "return allTextBoxValidator()");
        this.btnEdit.Attributes.Add("onclick", "return allTextBoxValidator()");
    }
    /// <summary>
    /// 绑定是否有效下拉框
    /// </summary>
    private void BindSourceTypeStatus()
    {
        int[] status = CustType.GetCustTypeStatus();
        this.ddlTypeStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            this.ddlTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        StoreManageType objType = new StoreManageType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objType, this.GrdVewCreditLevel);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        StoreManageType objType = new StoreManageType();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objType.TypeID = Base.BaseApp.GetID("LicenseType", "LicenseTypeId");
        objType.TypeName = this.txtName.Text.Trim();
        objType.Status = Int32.Parse(this.ddlTypeStatus.SelectedValue.ToString());
        objType.Note = this.txtNote.Text.Trim();
        objType.CreateUserId = objSessionUser.CreateUserID;
        objType.CreateTime = DateTime.Now.Date;
        objType.OprDeptID = objSessionUser.OprDeptID;
        objType.OprRoleID = objSessionUser.OprRoleID;
        if (objBaseBo.Insert(objType) <= 0)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        this.BindData();
        this.ClearText();
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtName.Text = "";
        this.txtNote.Text = "";
        this.ddlTypeStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["TypeId"] != null && ViewState["TypeId"].ToString() != "")
        {
            StoreManageType objType = new StoreManageType();
            BaseBO objBaseBo = new BaseBO();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objBaseBo.WhereClause = "TypeId=" + ViewState["TypeId"].ToString();
            objType.TypeName = this.txtName.Text.Trim();
            objType.Status = Int32.Parse(this.ddlTypeStatus.SelectedValue.ToString());
            objType.Note = this.txtNote.Text.Trim();
            objType.ModifyUserId = objSessionUser.ModifyUserID;
            objType.ModifyTime = DateTime.Now.Date;
            objType.OprDeptID = objSessionUser.OprDeptID;
            objType.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Update(objType) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
            this.ClearText();
        }
        ViewState["TypeId"] = "";
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        this.BindData();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/BaseInfo/Store/StoreManageType.aspx");
    }
    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")//如果当前行没有数据，选择按钮不显示出来
                e.Row.Cells[4].Text = "";
            if (e.Row.Cells[3].Text.Length == 1)
            {
                if (e.Row.Cells[3].Text.Equals(CustType.CUST_TYPE_STATUS_INVALID.ToString()))
                {
                    e.Row.Cells[3].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Disabled");
                }
                else if (e.Row.Cells[3].Text.Equals(CustType.CUST_TYPE_STATUS_VALID.ToString()))
                {
                    e.Row.Cells[3].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Enabled");
                }
            }
        }
    }
    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        StoreManageType objType = new StoreManageType();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "TypeId=" + this.GrdVewCreditLevel.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(objType);
        if (rs.Count == 1)
        {
            objType = rs.Dequeue() as StoreManageType;
            ViewState["TypeId"] = objType.TypeID;
            this.txtName.Text = objType.TypeName;
            this.txtNote.Text = objType.Note;
            this.ddlTypeStatus.SelectedValue = objType.Status.ToString();
        }
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        this.BindData();
    }
    protected void GrdVewCreditLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in this.GrdVewCreditLevel.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.BindData();
    }
}
