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

public partial class BaseInfo_Store_LicenseType : BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetControlPro();//添加按钮属性
        if (!this.IsPostBack)
        {
            this.BindSourceTypeStatus();//绑定是否有效下拉框
            this.BindData();//绑定GridView
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Store_LicenseCardVindicate");
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
        this.cmbCustTypeStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            cmbCustTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        LicenseType objLicenseType = new LicenseType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objLicenseType, this.GrdVewCreditLevel);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        LicenseType objLicenseType = new LicenseType();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select LicenseTypeName from LicenseType where LicenseTypeName='" + txtLevelName.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_LevelName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLevelName.select()", true);
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objLicenseType.LicenseTypeId = Base.BaseApp.GetID("LicenseType", "LicenseTypeId");
            objLicenseType.LicenseTypeName = this.txtLevelName.Text.Trim();
            objLicenseType.Status = Int32.Parse(this.cmbCustTypeStatus.SelectedValue.ToString());
            objLicenseType.Note = this.txtNote.Text.Trim();
            objLicenseType.CreateUserId = objSessionUser.UserID;
            //objLicenseType.CreateTime = DateTime.Now;
            //objLicenseType.ModifyUserId = objSessionUser.UserID;
            //objLicenseType.ModifyTime = DateTime.Now;
            objLicenseType.OprDeptID = objSessionUser.OprDeptID;
            objLicenseType.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objLicenseType) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            }

            this.ClearText();
        }
        this.BindData();

        foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtLevelName.Text = "";
        this.txtNote.Text = "";
        this.cmbCustTypeStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["LicenseTypeId"] != null && ViewState["LicenseTypeId"].ToString() != "")
        {
            LicenseType objLicenseType = new LicenseType();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select LicenseTypeName from LicenseType where LicenseTypeName='" + txtLevelName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "LicenseTypeId=" + ViewState["LicenseTypeId"].ToString();
                objLicenseType.LicenseTypeName = this.txtLevelName.Text.Trim();
                objLicenseType.Status = Int32.Parse(this.cmbCustTypeStatus.SelectedValue.ToString());
                objLicenseType.Note = this.txtNote.Text.Trim();
                objLicenseType.ModifyUserId = objSessionUser.UserID;
                //objLicenseType.ModifyTime = DateTime.Now;
                objLicenseType.OprDeptID = objSessionUser.OprDeptID;
                objLicenseType.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objLicenseType) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["LicenseTypeId"] = "";
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                this.BindData();
                foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_LevelName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLevelName.select()", true);
                this.BindData();
            }
        }
        
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/BaseInfo/Store/LicenseType.aspx");
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
        BaseBO objBaseBo = new BaseBO();
        LicenseType objLicenseType = new LicenseType();
        objBaseBo.WhereClause = "LicenseTypeId=" + this.GrdVewCreditLevel.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(objLicenseType);
        if (rs.Count == 1)
        {
            objLicenseType = rs.Dequeue() as LicenseType;
            ViewState["LicenseTypeId"] = objLicenseType.LicenseTypeId;
            this.txtLevelName.Text = objLicenseType.LicenseTypeName;
            this.txtNote.Text = objLicenseType.Note;
            this.cmbCustTypeStatus.SelectedValue = objLicenseType.Status.ToString();
        }
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        Session["editLog"] = txtLevelName.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
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
        this.BindData();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
