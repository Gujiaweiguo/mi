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
using BaseInfo.User;
using Lease.PotCustLicense;
using System.Text;

public partial class Lease_Customer_CustContact : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.SetControlPro();
            this.BindData();
            if (Request["look"] != null)
            {
                if (Request["look"] == "yes")
                {
                    this.btnAdd.Visible = false;
                    this.btnCancel.Visible = false;
                    this.btnEdit.Visible = false;
                }
            }
        }
    }
    /// <summary>
    /// 按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        //this.btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //this.btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //this.btnQuit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //this.btnQuit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        this.btnAdd.Attributes.Add("onclick", "return allTextBoxValidator()");
        this.btnEdit.Attributes.Add("onclick", "return allTextBoxValidator()");

    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        string strSlq = "select CustContactID,CustID,ContactMan,Title,OfficeTel,MobileTel,EMail,ManageArea,Fax from CustContact ";
        string strWhereClause = "";
        try
        {
            if (Request.Cookies["Info1"] != null)
            {
                strWhereClause = "where CustID=" + Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
            }
            else
            {
                strWhereClause = " where CustID=" + 0;
            }
        }
        catch
        {
            strWhereClause = " where CustID=" + 0;
        }
        strSlq = strSlq + strWhereClause;
        BaseInfo.BaseCommon.BindGridView(strSlq, this.GrdCustBrand);
        #region
        //DataTable dt = objBaseBo.QueryDataSet(strSlq).Tables[0];
        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < this.GrdCustBrand.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    this.GrdCustBrand.DataSource = pds;
        //    this.GrdCustBrand.DataBind();
        //}
        //else
        //{
        //    this.GrdCustBrand.EmptyDataText = "";
        //    pds.AllowPaging = true;
        //    pds.PageSize = 10;
        //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }
        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }
        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }
        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }
        //    this.GrdCustBrand.DataSource = pds;
        //    this.GrdCustBrand.DataBind();
        //    int spareRow = 0;
        //    spareRow = this.GrdCustBrand.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    this.GrdCustBrand.DataSource = pds;
        //    this.GrdCustBrand.DataBind();
        //}
        #endregion
    }
    /// <summary>
    /// 清空TextBox中的值
    /// </summary>
    private void ClearTextValue()
    {
        this.txtName.Text = "";
        this.txtDuty.Text = "";
        //this.txtChargeArea.Text = "";
        this.txtphone.Text = "";
        this.txtmobil.Text = "";
        this.txtEmail.Text = "";
        this.txtFax.Text = "";
        //this.txtpeople.Text = "";
        this.txtChargeArea.Text = "";
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.ClearTextValue();
        this.BindData();

    }
    /// <summary>
    /// 添加联系人事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if(this.SaveAdd())
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('Save Ok!')", true);
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('Save Error!')", true);
        this.ClearTextValue();
        this.BindData();
    }
    /// <summary>
    /// 新增联系人
    /// </summary>
    /// <returns></returns>
    private bool SaveAdd()
    {
        BaseBO objBaseBo = new BaseBO();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        PotCustContactInfo objPotCust = new PotCustContactInfo();
        string strName = this.txtName.Text.Trim();
        string strDuty = this.txtDuty.Text.Trim();
        string strArea = this.txtChargeArea.Text.Trim();
        string strPhone = this.txtphone.Text.Trim();
        string strFax = this.txtFax.Text.Trim();
        string strMobile = this.txtmobil.Text.Trim();
        string strEmail = this.txtEmail.Text.Trim();

        string strCreateUserID = objSessionUser.CreateUserID.ToString();
        DateTime dtCreate = DateTime.Now.Date;
        string strModifyUserID = objSessionUser.ModifyUserID.ToString();
        DateTime dtModify = DateTime.Now.Date;
        string strRoleID = objSessionUser.OprRoleID.ToString();
        string strDeptID = objSessionUser.OprDeptID.ToString();
        int strCustContactID = Base.BaseApp.GetID("CustContact", "CustContactID");
        string strCustID = Request.Cookies["Info1"].Values["custid"].ToString();
        StringBuilder sb = new StringBuilder();
        sb.Append("insert into CustContact (CustContactID,CustID,ContactMan,Title,OfficeTel,MobileTel,EMail,");
        sb.Append("ManageArea,Fax)");
        sb.Append(" values ('" + strCustContactID + "','" + strCustID + "','" + strName + "','" + strDuty + "','" + strPhone + "',");
        sb.Append("'" + strMobile + "','" + strEmail + "',");
        sb.Append("'" + strArea + "','" + strFax + "')");
        if (objBaseBo.ExecuteUpdate(sb.ToString()) <= 0)
            return false;
        else
            return true;
                
    }
    /// <summary>
    /// 更新联系人记录
    /// </summary>
    /// <returns></returns>
    private bool SaveUpdate()
    {
        BaseBO objBaseBo = new BaseBO();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        string strName = this.txtName.Text.Trim();
        string strDuty = this.txtDuty.Text.Trim();
        string strArea = this.txtChargeArea.Text.Trim();
        string strPhone = this.txtphone.Text.Trim();
        string strFax = this.txtFax.Text.Trim();
        string strMobile = this.txtmobil.Text.Trim();
        string strEmail = this.txtEmail.Text.Trim();

        string strCreateUserID = objSessionUser.CreateUserID.ToString();
        DateTime dtCreate = DateTime.Now.Date;
        string strModifyUserID = objSessionUser.ModifyUserID.ToString();
        DateTime dtModify = DateTime.Now.Date;
        string strRoleID = objSessionUser.OprRoleID.ToString();
        string strDeptID = objSessionUser.OprDeptID.ToString();
        string strCustContactID = ViewState["CustContactID"].ToString();
        StringBuilder sb = new StringBuilder();
        sb.Append("update CustContact set ContactMan='" + strName + "',Title='" + strDuty + "',");
        sb.Append("OfficeTel='" + strPhone + "',MobileTel='" + strMobile + "',EMail='" + strEmail + "',");
        sb.Append("ManageArea='" + strArea + "',Fax='" + strFax + "'");
        sb.Append("  where CustContactID='" + strCustContactID + "'");
        if (objBaseBo.ExecuteUpdate(sb.ToString()) <= 0)
            return false;
        else
            return true;
    }
    /// <summary>
    /// 更新事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["CustContactID"] == null || ViewState["CustContactID"].ToString() == "")
            return;
        if (this.SaveUpdate())
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('Save Ok!')", true);
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "alert('Save Error!')", true);
        this.btnEdit.Enabled = false;
        this.btnAdd.Enabled = true;
        ViewState["CustContactID"] = "";
        this.BindData();
        this.ClearTextValue();
    }
    protected void GrdCustBrand_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }
    protected void GrdCustBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        ViewState["CustContactID"] = this.GrdCustBrand.SelectedRow.Cells[0].Text.Trim();
        DataSet ds = objBaseBo.QueryDataSet("select CustContactID,CustID,ContactMan,Title,OfficeTel,MobileTel,EMail,ManageArea,Fax   from CustContact where CustContactID='" + ViewState["CustContactID"].ToString() + "'");
        if (ds.Tables[0].Rows.Count == 1)
        {
            this.txtName.Text = ds.Tables[0].Rows[0]["Contactman"].ToString();
            this.txtDuty.Text = ds.Tables[0].Rows[0]["Title"].ToString();
            this.txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
            this.txtphone.Text = ds.Tables[0].Rows[0]["OfficeTel"].ToString();
            this.txtmobil.Text = ds.Tables[0].Rows[0]["MobileTel"].ToString();
            this.txtEmail.Text = ds.Tables[0].Rows[0]["EMail"].ToString();
            this.txtChargeArea.Text = ds.Tables[0].Rows[0]["ManageArea"].ToString();
            this.btnAdd.Enabled = false;
            this.btnEdit.Enabled = true;
        }
        this.BindData();
    }
    protected void GrdCustBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
    }
}
