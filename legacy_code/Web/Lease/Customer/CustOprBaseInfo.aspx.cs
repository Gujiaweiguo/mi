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
using Lease.PotCust;
using Lease.CustLicense;
using Base;
using BaseInfo.User;
using Base.Util;
using Lease.Customer;

public partial class Lease_Customer_CustOprBaseInfo : BasePage
{
    private static int SELECTED = -1;
    private static int DISPROVE_UP = 1;
    private static int DISPROVE_IN = 2;
    public string errorMes;
    protected void Page_Load(object sender, EventArgs e)
    {
        errorMes = (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error");//信息错误
        if (!this.IsPostBack)
        {
            if (Request["browse"] != null && Request["browse"].ToString() == "yes")//如果进行查询，则保存按钮、修改按钮、取消按钮不可见
            {
                SetControlLock(); 
            }
            this.btnAdd.Attributes.Add("onclick", "return InputValidator()");
            this.BindCustType();//绑定商户类型
            this.BindData();//绑定数据
        }
    }
    /// <summary>
    /// 设置控件不可写
    /// </summary>
    private void SetControlLock()
    {
        this.txtOperateAreas.Enabled = false;
        this.txtShopNumber.Enabled = false;
        this.txtAreaSalesRate.Enabled = false;
        this.txtBaseDiscount.Enabled = false;
        this.txtPromoteArea.Enabled = false;
        this.txtPromoteCost.Enabled = false;
        this.txtplanShopNumber.Enabled = false;
        this.txtplanArea.Enabled = false;
        this.txtPlanDate.Enabled = false;
        this.ddlType.Enabled = false;
        this.btnAdd.Visible = false;
        this.btnCancel.Visible = false;
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        CustOprInfo objCustOprInfo = new CustOprInfo();
        try
        {
            if (Request.Cookies["Info1"].Values["custid"] != "")
            {
                ViewState["CustID"] = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
                //baseBO.WhereClause = "a.CustID=" + ViewState["CustID"];
                //rs = baseBO.Query(new CustomerQuery());
                //if (rs.Count == 1)
                //{
                //    CustomerQuery CustomerInfo = rs.Dequeue() as CustomerQuery;
                //    txtCreateUserID.Text = CustomerInfo.CustCode.ToString();
                //    txtCustName.Text = CustomerInfo.CustName;
                //    txtCustShortName.Text = CustomerInfo.CustShortName;
                //    ViewState["CustomerStatus"] = CustomerInfo.CustomerStatus;
                //}
                baseBO.WhereClause = "CustID=" + ViewState["CustID"];
                rs = baseBO.Query(new Customer());
                if (rs.Count == 1)
                {
                    Customer objCustomer = rs.Dequeue() as Customer;
                    this.txtCreateUserID.Text = objCustomer.CustCode.ToString();
                    this.txtCustName.Text = objCustomer.CustName;
                    this.txtCustShortName.Text = objCustomer.CustShortName;
                    ViewState["CustomerStatus"] = objCustomer.CustomerStatus;
                }
                baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"]);
                rs = baseBO.Query(objCustOprInfo);
                if (rs.Count == 1)
                {
                    objCustOprInfo = rs.Dequeue() as CustOprInfo;
                    this.txtOperateAreas.Text = objCustOprInfo.OperateAreas;
                    this.txtShopNumber.Text = objCustOprInfo.ShopNumber.ToString();
                    this.txtAreaSalesRate.Text = objCustOprInfo.AreaSalesRate.ToString();
                    this.txtBaseDiscount.Text = objCustOprInfo.BaseDiscount.ToString();
                    this.txtPromoteArea.Text = objCustOprInfo.PromoteArea;
                    this.txtPromoteCost.Text = objCustOprInfo.PromoteCost;
                    this.txtplanShopNumber.Text = objCustOprInfo.planShopNumber.ToString();
                    this.txtplanArea.Text = objCustOprInfo.planArea;
                    this.txtPlanDate.Text = objCustOprInfo.planDate.ToShortDateString();
                    this.ddlType.SelectedValue = objCustOprInfo.CustTypeID.ToString();
                    ViewState["Disprove"] = DISPROVE_UP;
                }
                else
                {
                    ViewState["Disprove"] = DISPROVE_IN;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("读取经营概况信息错误:", ex);
            btnAdd.Enabled = false;
        }
    }
    /// <summary>
    /// 绑定商户类型
    /// </summary>
    private void BindCustType()
    {
        BaseBO objBaseBo = new BaseBO();
        CustType objCustType = new CustType();
        objBaseBo.WhereClause = "CustTypeStatus=1";
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, objCustType, "CustTypeID", "CustTypeName", this.ddlType);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["Disprove"]) == DISPROVE_IN)
            AddCustOprInfo();
        else if (Convert.ToInt32(ViewState["Disprove"]) == DISPROVE_UP)//已经存在数据
            UpdateCustOprInfo();
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
    }
    /// <summary>
    /// 添加经营概况
    /// </summary>
    private void AddCustOprInfo()
    {
        BaseBO baseBO = new BaseBO();
        CustOprInfo objCustOpr = new CustOprInfo();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        try
        {
            objCustOpr.CustOprInfoId = BaseApp.GetCustumerID("CustOprInfo", "CustOprInfoId");
            objCustOpr.CustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
            objCustOpr.CustTypeID = Int32.Parse(this.ddlType.SelectedValue);
            objCustOpr.OperateAreas = this.txtOperateAreas.Text.Trim();
            if (this.txtShopNumber.Text.Trim() != "")
            {
                try { objCustOpr.ShopNumber = Int32.Parse(this.txtShopNumber.Text.Trim()); }
                catch { objCustOpr.ShopNumber = 0; }
            }
            if (this.txtAreaSalesRate.Text.Trim() != "")
            {
                try { objCustOpr.AreaSalesRate = decimal.Parse(this.txtAreaSalesRate.Text.Trim()); }
                catch { objCustOpr.AreaSalesRate = 0; }
            }
            if (this.txtBaseDiscount.Text.Trim() != "")
            {
                try { objCustOpr.BaseDiscount = decimal.Parse(this.txtBaseDiscount.Text.Trim()); }
                catch { objCustOpr.BaseDiscount = 0; }
            }
            objCustOpr.PromoteArea = this.txtPromoteArea.Text.Trim();
            objCustOpr.PromoteCost = this.txtPromoteCost.Text.Trim();
            objCustOpr.planArea = this.txtplanArea.Text.Trim();
            if (this.txtplanShopNumber.Text.Trim() != "")//开店数量
            {
                try { objCustOpr.planShopNumber = Int32.Parse(this.txtplanShopNumber.Text.Trim()); }
                catch { objCustOpr.planShopNumber = 0; }
            }
            if (this.txtPlanDate.Text.Trim() != "")
            {
                try { objCustOpr.planDate = DateTime.Parse(this.txtPlanDate.Text.Trim()); }
                catch { objCustOpr.planDate = DateTime.Now.Date; }
            }
            objCustOpr.CreateUserId = objSessionUser.CreateUserID;
            objCustOpr.CreateTime = DateTime.Now;
            objCustOpr.OprDeptID = objSessionUser.OprDeptID;
            objCustOpr.OprRoleID = objSessionUser.OprRoleID;
            if (baseBO.Insert(objCustOpr) != -1)
            {
                ViewState["Disprove"] = DISPROVE_UP;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("经营概况信息错误:", ex);
        }
    }
    /// <summary>
    /// 更新经营概况
    /// </summary>
    private void UpdateCustOprInfo()
    { 
        BaseBO baseBO = new BaseBO();
        BaseTrans baseTrans = new BaseTrans();
        CustOprInfo objCustOpr = new CustOprInfo();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objCustOpr.CustTypeID = Int32.Parse(this.ddlType.SelectedValue);
        objCustOpr.OperateAreas = this.txtOperateAreas.Text.Trim();
        if (this.txtShopNumber.Text.Trim() != "")
        {
            try { objCustOpr.ShopNumber = Int32.Parse(this.txtShopNumber.Text.Trim()); }
            catch { objCustOpr.ShopNumber = 0; }
        }
        if (this.txtAreaSalesRate.Text.Trim() != "")
        {
            try { objCustOpr.AreaSalesRate = decimal.Parse(this.txtAreaSalesRate.Text.Trim()); }
            catch { objCustOpr.AreaSalesRate = 0; }
        }
        if (this.txtBaseDiscount.Text.Trim() != "")
        {
            try { objCustOpr.BaseDiscount = decimal.Parse(this.txtBaseDiscount.Text.Trim()); }
            catch { objCustOpr.BaseDiscount = 0; }
        }
        objCustOpr.PromoteArea = this.txtPromoteArea.Text.Trim();
        objCustOpr.PromoteCost = this.txtPromoteCost.Text.Trim();
        objCustOpr.planArea = this.txtplanArea.Text.Trim();
        if (this.txtplanShopNumber.Text.Trim() != "")//开店数量
        {
            try { objCustOpr.planShopNumber = Int32.Parse(this.txtplanShopNumber.Text.Trim()); }
            catch { objCustOpr.planShopNumber = 0; }
        }
        if (this.txtPlanDate.Text.Trim() != "")
        {
            try { objCustOpr.planDate = DateTime.Parse(this.txtPlanDate.Text.Trim()); }
            catch { objCustOpr.planDate = DateTime.Now.Date; }
        }
        objCustOpr.CreateUserId = objSessionUser.CreateUserID;
        objCustOpr.CreateTime = DateTime.Now;
        objCustOpr.OprDeptID = objSessionUser.OprDeptID;
        objCustOpr.OprRoleID = objSessionUser.OprRoleID;

        baseTrans.BeginTrans();
        baseTrans.WhereClause = "custid=" + ViewState["CustID"];
        if (baseTrans.Update(objCustOpr) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
            baseTrans.Rollback();
            return;
        }
        baseTrans.Commit();
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/Customer/CustOprBaseInfo.aspx");
    }
}
