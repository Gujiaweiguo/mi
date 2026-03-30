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
using BaseInfo.Dept;
using BaseInfo.Store;
using Base.Biz;
using Base.DB;
using Lease.PotCust;
using Lease.CustLicense;
using BaseInfo.User;
using System.Drawing;
using BaseInfo.authUser;

public partial class Store_StoreLicense : System.Web.UI.Page
{
    public string PotCustomer_Basic;
    public string Store_CardInfo;
    public string Store_ItemBackdrop;
    public string isbro;
    public string baseinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        //显示树形列表
        this.ShowTree();
        this.SetControlPro();
        if (!this.IsPostBack)
        {
            if (Request["look"] != null)
            {
                isbro = Request["look"].ToString();
                if (isbro.ToLower() == "yes")
                {
                    this.btnEdit.Visible = false;
                    this.btnCancel.Visible = false;
                    this.btnSave.Visible = false;
                    this.LockControl();
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfobrowse");
                }
                else
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfoMaintenance");
            }
            this.BIndData("");//绑定GridView
            this.BindLicenseType();//绑定证照类型
            this.BindUsage();//绑定居住类型
            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            Store_CardInfo = (String)GetGlobalResourceObject("BaseInfo", "Store_CardInfo");
            Store_ItemBackdrop = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemBackdrop");
            this.SetControlLock();//设置控件不可用
        }
    }
    private void LockControl()
    {
        this.ddlLicenseTypeId.Enabled = false;
        this.txtLicenseCode.Enabled = false;
        this.txtPropertyName.Enabled = false;
        this.txtPropertyOwner.Enabled = false;
        this.txtArea.Enabled = false;
        this.ddlUsage.Enabled = false;
        this.txtRegDate.Enabled = false;
        this.txtFiles.Enabled = false;
    }
    /// <summary>
    /// 绑定证照类型
    /// </summary>
    private void BindLicenseType()
    {
        BaseBO objBaseBo = new BaseBO();
        LicenseType objLicenseType = new LicenseType();
        objBaseBo.WhereClause = "status=1";
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, objLicenseType, "LicenseTypeId", "LicenseTypeName", this.ddlLicenseTypeId);
    }
    /// <summary>
    /// 绑定设计用途
    /// </summary>
    private void BindUsage()
    {
        int[] status = StoreLicense.GetUsageType();
        for (int i = 0; i < status.Length; i++)
        {
            this.ddlUsage.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", StoreLicense.GetUsageTypeDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "depttype = 6";
        objBaseBo.OrderBy = "OrderID";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select DeptName from Dept where DeptType=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            jsdept = "100" + "|" + "10" + "|" + ds.Tables[0].Rows[0][0] + "^";
        }
        else
        {
            jsdept = "100" + "|" + "10" + "|" + "商业项目" + "^";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        Resultset rs = objBaseBo.Query(new Dept());
        if (rs.Count > 0)
        {
            foreach (Dept objDept in rs)
            {
                jsdept += objDept.DeptID + "|" + "100" + "|" + objDept.DeptName + "^";
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        this.btnSave.Attributes.Add("onclick", "return CheckIsNull()");
        this.btnEdit.Attributes.Add("onclick", "return CheckIsNull()");
        this.txtLicenseCode.Attributes.Add("onblur", "TextIsNotNull(txtLicenseCode,ImgCustShortName)");
        this.txtPropertyName.Attributes.Add("onblur", "TextIsNotNull(txtPropertyName,Img1)");
    }
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    protected void BIndData(String strDeptID)
    {
        //BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        //PagedDataSource pds = new PagedDataSource();
        //int spareRow = 0;
        BaseBO objBaseBo = new BaseBO();
        if (strDeptID == "")
            objBaseBo.WhereClause = "StoreId=100";
        else
            objBaseBo.WhereClause = "StoreId=" + strDeptID;
        StoreLicense objLicense = new StoreLicense();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objLicense, this.GrdUser);
        #region
        //DataTable dt = baseBO.QueryDataSet(new StoreLicense()).Tables[0];
        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdUser.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdUser.DataSource = pds;
        //    GrdUser.DataBind();
        //}
        //else
        //{
        //    pds.AllowPaging = true;
        //    pds.PageSize = 11;
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
        //    this.GrdUser.DataSource = pds;
        //    this.GrdUser.DataBind();
        //    spareRow = GrdUser.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdUser.DataSource = pds;
        //    GrdUser.DataBind();
        //}
        #endregion
    }
    /// <summary>
    /// 设置控件不可用
    /// </summary>
    private void SetControlLock()
    {
        this.txtLicenseCode.Text = "";
        this.txtPropertyName.Text = "";
        this.txtPropertyOwner.Text = "";
        this.txtArea.Text = "";
        this.txtRegDate.Text = "";
        this.txtFiles.Text = "";
        this.btnEdit.Enabled = false;
        this.btnSave.Enabled = false;
    }
    /// <summary>
    /// 设置控件可用
    /// </summary>
    private void SetControlUNLock()
    {
        this.txtLicenseCode.Text = "";
        this.txtPropertyName.Text = "";
        this.txtPropertyOwner.Text = "";
        this.txtArea.Text = "";
        this.txtRegDate.Text = "";
        //this.txtFiles.CssClass = "";
        //this.txtFiles.ReadOnly = false;
        this.txtFiles.Text = "";
    }
    /// <summary>
    /// 添加一条记录
    /// </summary>
    private void SaveAdd()
    {
        if (ViewState["DeptID"] != null && ViewState["DeptID"].ToString() != "")
        {
            BaseBO objBaseBo = new BaseBO();
            StoreLicense objStoreLincense = new StoreLicense();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

            objStoreLincense.LicenseId = Base.BaseApp.GetCustumerID("StoreLicense", "LicenseId");
            objStoreLincense.StoreId = Int32.Parse(ViewState["DeptID"].ToString());
            objStoreLincense.LicenseTypeId = Int32.Parse(this.ddlLicenseTypeId.SelectedValue);
            objStoreLincense.LicenseCode = this.txtLicenseCode.Text.Trim();
            objStoreLincense.PropertyName = this.txtPropertyName.Text.Trim();
            objStoreLincense.PropertyOwner = this.txtPropertyOwner.Text.Trim();

            objStoreLincense.CreateUserId = objSessionUser.CreateUserID;
            objStoreLincense.CreateTime = DateTime.Now.Date;
            objStoreLincense.ModifyUserId = objSessionUser.ModifyUserID;
            objStoreLincense.ModifyTime = DateTime.Now.Date;
            objStoreLincense.OprRoleID = objSessionUser.OprRoleID;
            objStoreLincense.OprDeptID = objSessionUser.OprDeptID;

            if (this.txtArea.Text.Trim() != "")
                objStoreLincense.Area = decimal.Parse(this.txtArea.Text.Trim());
            else
                objStoreLincense.Area = 0;
            objStoreLincense.Usage = Int32.Parse(this.ddlUsage.SelectedValue);
            if (this.txtRegDate.Text.Trim() != "")
                objStoreLincense.RegDate = DateTime.Parse(this.txtRegDate.Text.Trim());
            else
                objStoreLincense.RegDate = DateTime.Now.Date;
            objStoreLincense.Files = this.txtFiles.Text.Trim();
            if (objBaseBo.Insert(objStoreLincense) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.BIndData(ViewState["DeptID"].ToString());
            this.SetControlLock();
            //ViewState["DeptID"] = "";
        }
        else
        {
            this.BIndData("");
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 更新一条记录
    /// </summary>
    private void SaveUpdate(string strLicenseId)
    {
        BaseBO objBaseBo = new BaseBO();
        StoreLicense objStoreLince = new StoreLicense();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objStoreLince.LicenseTypeId = Int32.Parse(this.ddlLicenseTypeId.SelectedValue);
        objStoreLince.LicenseCode = this.txtLicenseCode.Text.Trim();
        objStoreLince.PropertyName = this.txtPropertyName.Text.Trim();
        objStoreLince.PropertyOwner = this.txtPropertyOwner.Text.Trim();
        if(this.txtArea.Text.Trim()!="")
            objStoreLince.Area = decimal.Parse(this.txtArea.Text.Trim());
        objStoreLince.Usage = Int32.Parse(this.ddlUsage.SelectedValue);
        if (this.txtRegDate.Text.Trim() != "")
            objStoreLince.RegDate = DateTime.Parse(this.txtRegDate.Text.Trim());
        objStoreLince.Files = this.txtFiles.Text.Trim();
        objBaseBo.WhereClause = "licenseId="+strLicenseId;

        objStoreLince.CreateUserId = objSessionUser.CreateUserID;
        objStoreLince.CreateTime = DateTime.Now.Date;
        objStoreLince.ModifyUserId = objSessionUser.ModifyUserID;
        objStoreLince.ModifyTime = DateTime.Now.Date;
        objStoreLince.OprRoleID = objSessionUser.OprRoleID;
        objStoreLince.OprDeptID = objSessionUser.OprDeptID;

        if (objBaseBo.Update(objStoreLince) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        this.BIndData(ViewState["DeptID"].ToString());
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.SaveAdd();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/BaseInfo/Store/StoreLicense.aspx?look=no");
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["DeptID"] = deptid.Value;
        if (ViewState["DeptID"].ToString() != "100")
        {
            this.BIndData(ViewState["DeptID"].ToString());
            this.SetControlUNLock();
        }
        else
        {
            this.BIndData("");
            this.SetControlLock();
        }
        this.btnSave.Enabled = true;
        this.btnEdit.Enabled = false;
        foreach (GridViewRow gvr in this.GrdUser.Rows)
        {
            gvr.BackColor = Color.White;
        }
        ViewState["LicenseId"] = "";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["LicenseId"] != null && ViewState["LicenseId"].ToString()!="")
            this.SaveUpdate(ViewState["LicenseId"].ToString());
        else
            this.BIndData("");
        ViewState["LicenseId"] = "";
        this.SetControlLock();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        StoreLicense objStoreLince = new StoreLicense();
        BaseBO objBaseBo = new BaseBO();
        this.SetControlUNLock();
        objBaseBo.WhereClause = "LicenseId=" + GrdUser.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(new StoreLicense());
        if (rs.Count != 0)
        { 
            objStoreLince = rs.Dequeue() as StoreLicense;
            this.ddlLicenseTypeId.SelectedValue = objStoreLince.LicenseTypeId.ToString();
            this.txtLicenseCode.Text = objStoreLince.LicenseCode;
            this.txtPropertyName.Text = objStoreLince.PropertyName;
            this.txtPropertyOwner.Text = objStoreLince.PropertyOwner;
            this.txtArea.Text = objStoreLince.Area.ToString();
            this.ddlUsage.SelectedValue = objStoreLince.Usage.ToString();
            this.txtRegDate.Text = objStoreLince.RegDate.ToString();
            this.txtFiles.Text = objStoreLince.Files;
            if (objStoreLince.RegDate.ToString() != "")
                this.txtRegDate.Text = objStoreLince.RegDate.ToShortDateString();
            ViewState["LicenseId"] = objStoreLince.LicenseId.ToString();
            this.BIndData(objStoreLince.StoreId.ToString());
        }
        this.btnEdit.Enabled = true;
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void GrdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[2].Text == "&nbsp;" || e.Row.Cells[2].Text == "")
                e.Row.Cells[4].Text = "";
        }
    }
    protected void GrdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BIndData(ViewState["DeptID"].ToString());
        foreach (GridViewRow grv in this.GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.SetControlLock();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
}
