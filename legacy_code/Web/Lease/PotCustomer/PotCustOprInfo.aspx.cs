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
using Lease.PotCustLicense;
using Base;
using BaseInfo.User;
using Base.Util;

public partial class Lease_PotCustomer_PotCustOprInfo :BasePage
{
    private static int SELECTED = -1;
    private static int DISPROVE_UP = 1;
    private static int DISPROVE_IN = 2;
    public string errorMes;
    protected void Page_Load(object sender, EventArgs e)
    {
        errorMes = (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error");//信息错误
        if (Request["look"] != null)
        {
            if (Request["look"] == "yes")
            {
                this.btnSave.Visible = false;
                this.btnCancel.Visible = false;
                this.ddlType.Enabled = false;
            }
        }
        if (!this.IsPostBack)
        {
            this.btnSave.Attributes.Add("onclick", "return InputValidator()");
            this.BindCustType();//绑定商户类型
            this.BindData();//绑定数据
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PotCustOprInfo objPotCustOprInfo = new PotCustOprInfo();
        try
        {
            if (Request.Cookies["Custumer"].Values["CustumerID"] != "")
            {
                ViewState["CustID"] = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
                baseBO.WhereClause = "a.CustID=" + ViewState["CustID"];
                rs = baseBO.Query(new PotCustomerInfo());
                if (rs.Count == 1)
                {
                    PotCustomerInfo potCustomerInfo = rs.Dequeue() as PotCustomerInfo;
                    txtCreateUserID.Text = potCustomerInfo.CustCode.ToString();
                    txtCustName.Text = potCustomerInfo.CustName;
                    txtCustShortName.Text = potCustomerInfo.CustShortName;
                    ViewState["CustomerStatus"] = potCustomerInfo.CustomerStatus;
                }
                baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"]);
                rs = baseBO.Query(objPotCustOprInfo);
                if (rs.Count == 1)
                {
                    objPotCustOprInfo = rs.Dequeue() as PotCustOprInfo;
                    this.txtOperateAreas.Text = objPotCustOprInfo.OperateAreas;
                    this.txtShopNumber.Text = objPotCustOprInfo.ShopNumber.ToString();
                    this.txtAreaSalesRate.Text = objPotCustOprInfo.AreaSalesRate.ToString();
                    this.txtBaseDiscount.Text = objPotCustOprInfo.BaseDiscount.ToString();
                    this.txtPromoteArea.Text = objPotCustOprInfo.PromoteArea;
                    this.txtPromoteCost.Text = objPotCustOprInfo.PromoteCost;
                    this.txtplanShopNumber.Text = objPotCustOprInfo.planShopNumber.ToString();
                    this.txtplanArea.Text = objPotCustOprInfo.planArea;
                    this.txtPlanDate.Text = objPotCustOprInfo.planDate.ToShortDateString();
                    this.ddlType.SelectedValue = objPotCustOprInfo.CustTypeID.ToString();
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
            btnSave.Enabled = false;
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
            AddPotCustOprInfo();
        else if (Convert.ToInt32(ViewState["Disprove"]) == DISPROVE_UP)//已经存在数据
            UpdatePotCustOprInfo();
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
    }
    /// <summary>
    /// 添加经营概况
    /// </summary>
    private void AddPotCustOprInfo()
    {
        BaseBO baseBO = new BaseBO();
        PotShop potShop = new PotShop();
        PotCustOprInfo objPotCustOpr = new PotCustOprInfo();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        try
        {
            objPotCustOpr.CustOprInfoId = BaseApp.GetID("PotCustOprInfo", "CustOprInfoId");
            objPotCustOpr.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            objPotCustOpr.CustTypeID = Int32.Parse(this.ddlType.SelectedValue);
            objPotCustOpr.OperateAreas = this.txtOperateAreas.Text.Trim();
            if (this.txtShopNumber.Text.Trim() != "")
            {
                try { objPotCustOpr.ShopNumber = Int32.Parse(this.txtShopNumber.Text.Trim()); }
                catch { objPotCustOpr.ShopNumber = 0; }
            }
            if (this.txtAreaSalesRate.Text.Trim() != "")
            {
                try { objPotCustOpr.AreaSalesRate = decimal.Parse(this.txtAreaSalesRate.Text.Trim()); }
                catch { objPotCustOpr.AreaSalesRate = 0; }
            }
            if (this.txtBaseDiscount.Text.Trim() != "")
            {
                try { objPotCustOpr.BaseDiscount = decimal.Parse(this.txtBaseDiscount.Text.Trim()); }
                catch { objPotCustOpr.BaseDiscount = 0; }
            }
            objPotCustOpr.PromoteArea = this.txtPromoteArea.Text.Trim();
            objPotCustOpr.PromoteCost = this.txtPromoteCost.Text.Trim();
            objPotCustOpr.planArea = this.txtplanArea.Text.Trim();
            if (this.txtplanShopNumber.Text.Trim() != "")//开店数量
            {
                try { objPotCustOpr.planShopNumber = Int32.Parse(this.txtplanShopNumber.Text.Trim()); }
                catch { objPotCustOpr.planShopNumber = 0; }
            }
            if (this.txtPlanDate.Text.Trim() != "")
            {
                try { objPotCustOpr.planDate =DateTime.Parse(this.txtPlanDate.Text.Trim()); }
                catch { objPotCustOpr.planDate = DateTime.Now.Date; }
            }
            objPotCustOpr.CreateUserId = objSessionUser.CreateUserID;
            objPotCustOpr.CreateTime = DateTime.Now;
            objPotCustOpr.OprDeptID = objSessionUser.OprDeptID;
            objPotCustOpr.OprRoleID = objSessionUser.OprRoleID;
            if (baseBO.Insert(objPotCustOpr) != -1)
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
    private void UpdatePotCustOprInfo()
    { 
        BaseBO baseBO = new BaseBO();
        BaseTrans baseTrans = new BaseTrans();
        PotCustOprInfo objPotCustOpr = new PotCustOprInfo();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        //objPotCustOpr.CustOprInfoId = BaseApp.GetCustumerID("PotCustOprInfo", "CustOprInfoId");
        //objPotCustOpr.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        objPotCustOpr.CustTypeID = Int32.Parse(this.ddlType.SelectedValue);
        objPotCustOpr.OperateAreas = this.txtOperateAreas.Text.Trim();
        if (this.txtShopNumber.Text.Trim() != "")
        {
            try { objPotCustOpr.ShopNumber = Int32.Parse(this.txtShopNumber.Text.Trim()); }
            catch { objPotCustOpr.ShopNumber = 0; }
        }
        if (this.txtAreaSalesRate.Text.Trim() != "")
        {
            try { objPotCustOpr.AreaSalesRate = decimal.Parse(this.txtAreaSalesRate.Text.Trim()); }
            catch { objPotCustOpr.AreaSalesRate = 0; }
        }
        if (this.txtBaseDiscount.Text.Trim() != "")
        {
            try { objPotCustOpr.BaseDiscount = decimal.Parse(this.txtBaseDiscount.Text.Trim()); }
            catch { objPotCustOpr.BaseDiscount = 0; }
        }
        objPotCustOpr.PromoteArea = this.txtPromoteArea.Text.Trim();
        objPotCustOpr.PromoteCost = this.txtPromoteCost.Text.Trim();
        objPotCustOpr.planArea = this.txtplanArea.Text.Trim();
        if (this.txtplanShopNumber.Text.Trim() != "")//开店数量
        {
            try { objPotCustOpr.planShopNumber =Int32.Parse(this.txtplanShopNumber.Text.Trim()); }
            catch { objPotCustOpr.planShopNumber = 0; }
        }
        if (this.txtPlanDate.Text.Trim() != "")
        {
            try { objPotCustOpr.planDate = DateTime.Parse(this.txtPlanDate.Text.Trim()); }
            catch { objPotCustOpr.planDate = DateTime.Now.Date; }
        }
        objPotCustOpr.CreateUserId = objSessionUser.CreateUserID;
        objPotCustOpr.CreateTime = DateTime.Now;
        objPotCustOpr.OprDeptID = objSessionUser.OprDeptID;
        objPotCustOpr.OprRoleID = objSessionUser.OprRoleID;

        baseTrans.BeginTrans();
        baseTrans.WhereClause = "custid=" + ViewState["CustID"];
        if (baseTrans.Update(objPotCustOpr) != -1)
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
        this.Response.Redirect("~/Lease/PotCustomer/PotCustOprInfo.aspx");
    }
}
