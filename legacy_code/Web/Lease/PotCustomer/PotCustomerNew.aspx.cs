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

using Base.Page;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;

using Base.Biz;
using Base;
using Lease.PotCustLicense;
using BaseInfo.User;
using Base.DB;
using BaseInfo.Dept;
using Lease.PotCust;
using Lease.Formula;
using Lease.SMSPara;
using Base.Util;

public partial class Lease_PotCustomer_PotCustomerNew : BasePage
{
    private static int DISPROVE_UP = 1;
    private static int DISPROVE_IN = 2;
    public string errorMes;
    public string billOfDocumentDelete;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Import"] != null && Session["Import"]!="")
            {
                this.txtCustCode.Enabled = false;
                this.txtCustName.Enabled = false;
            }
            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();

            if (this.checkSMSParaCustCode())
            {
                this.txtCustCode.ReadOnly = true;
            }
            //绑定信用等级
            BaseInfo.BaseCommon.BindDropDownList("select CreditLevelId,CreditLevelName from CreditLevel where status=1", "CreditLevelId", "CreditLevelName", this.ddlCreditLevel);
            //绑定客户来源
            BaseInfo.BaseCommon.BindDropDownList("select SourceTypeId,SourceTypeName from SourceType where SourceTypeStatus=1", "SourceTypeId", "SourceTypeName", this.ddlSourceType);

            SessionUser sessionUsers = (SessionUser)Session["UserAccessInfo"];
            txtCustName.Attributes.Add("onblur", "TextIsNotNull(txtCustName,ImgCustName)");
            txtCustShortName.Attributes.Add("onblur", "TextIsNotNull(txtCustShortName,ImgCustShortName)");
            txtRegCap.Attributes.Add("onkeydown", "textleave()");
            errorMes = (String)GetGlobalResourceObject("BaseInfo", "Customer_labCustomerNameNotNull");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            btnBlankOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");

            Resultset rs = new Resultset();

            /*商户类型*/
            baseBO.WhereClause = "CustTypeStatus=" + CustType.CUST_TYPE_STATUS_VALID;
            rs = baseBO.Query(new CustType());
            foreach (CustType custtype in rs)
            {
                cmbCustType.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
            }

            /*币种*/
            baseBO.WhereClause = "";
            rs = baseBO.Query(new CurrencyType());
            foreach (CurrencyType curType in rs)
            {
                DDownListCurrencyType.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
            }

                
            string str_sql = "select signperson.userid,signperson.username from signperson";
            baseBO.WhereClause = "";
            //DataSet usersDS = baseBO.QueryDataSet(str_sql);

            //ds.Clear;
            ds = baseBO.QueryDataSet(str_sql); 
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                cmbCommOper.Items.Add(new ListItem(ds.Tables[0].Rows[i]["UserName"].ToString(), ds.Tables[0].Rows[i]["UserID"].ToString()));
            }
            cmbCommOper.SelectedValue = sessionUsers.UserID.ToString();

            #region 查询否有驳回的单据和页面跳转后返回的数据显示操作
            if (Request.QueryString["VoucherID"] != null)
                {
                    showCustomerInfo(Convert.ToInt32(Request.QueryString["VoucherID"]));

                    /*把商户ID存入Cookies*/
                    HttpCookie cookiesCustumer = new HttpCookie("Custumer");

                    cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                    cookiesCustumer.Values.Add("CustumerID", Request.QueryString["VoucherID"]);
                    Response.AppendCookie(cookiesCustumer);

                    /*把驳回状态存入Cookies*/
                    HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                    cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                    cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                    Response.AppendCookie(cookiesDisprove);

                    /*把节点编号存入Cookies*/
                    HttpCookie cookiesSequence = new HttpCookie("Sequence");

                    cookiesSequence.Expires = System.DateTime.Now.AddHours(1);
                    cookiesSequence.Values.Add("SequenceID", Request.QueryString["Sequence"]);
                    Response.AppendCookie(cookiesSequence);

                    //txtCustName.Enabled = false;
                    //txtCustName.CssClass = "Enabledipt160px";
                    //txtCustShortName.Enabled = false;
                    //txtCustShortName.CssClass = "Enabledipt160px";
                    btnBlankOut.Enabled = true;
                }
                else if (Request.QueryString["Type"] == "New")
                {
                 
                    //Response.Write("<script language=javascript>alert('操作成功!!');</script>");
                    /*删除Cookies商户ID*/
                    HttpCookie cookiesCustumer = new HttpCookie("Custumer");

                    cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                    cookiesCustumer.Values.Add("CustumerID", "");
                    Response.AppendCookie(cookiesCustumer);

                    /*把驳回状态存入Cookies*/
                    HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                    cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                    cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
                    Response.AppendCookie(cookiesDisprove);

                    //txtCustCode.Text = BaseApp.GetCustumerID("PotCustomer", "CustCode").ToString();
                    btnPutIn.Enabled = false;
                    btnMessage.Enabled = false;
                    this.btnPeople.Enabled = false;
                }
                else
                {
                    if (Request.Cookies["Custumer"].Values["CustumerID"] != "")
                    {
                        showCustomerInfo(Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]));
                        /*把驳回状态存入Cookies*/
                        HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                        cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                        cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                        Response.AppendCookie(cookiesDisprove);
                        btnPutIn.Enabled = true;
                        btnBlankOut.Enabled = true;
                    }
                    else
                    {
                        /*把驳回状态存入Cookies*/
                        HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                        cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                        cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
                        Response.AppendCookie(cookiesDisprove);

                        //txtCustCode.Text = BaseApp.GetCustumerID("PotCustomer", "CustCode").ToString();
                        btnPutIn.Enabled = false;
                    }
                }
            #endregion

            /*把工作流ID和节点ID存入Cookies*/
            
            if (Request.Cookies["WorkFlow"].Values["WorkFlowID"] == "")
            {
                HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                Response.AppendCookie(cookiesWorkFlow);
            }


            if (Request.QueryString["WrkFlwID"] != null)
            {
                HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
            }
            else
            {
                HidenWrkID.Value = Request.Cookies["WorkFlow"].Values["WorkFlowID"].ToString();
            }
            if (Request.QueryString["VoucherID"] != null)
            {
                HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
            }
            else
            {
                HidenVouchID.Value = Request.Cookies["Custumer"].Values["CustumerID"].ToString();
            }

            btnMessage.Attributes.Add("onclick", "ShowMessage()");
            this.btnPeople.Attributes.Add("onclick", "ShowContact()");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //clearText();
        Response.Redirect("~/Lease/PotCustomer/PotCustomerNew.aspx");
    }

    #region 显示商户信息
    private void showCustomerInfo(int custID)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = "a.CustID=" + custID;
        baseBO.GroupBy = "";
        rs = baseBO.Query(new PotCustomerQuery());
        if (rs != null)
        {
            PotCustomerQuery potCustomerQuery = rs.Dequeue() as PotCustomerQuery;
            txtCustCode.Text = potCustomerQuery.CustCode;
            txtCustName.Text = potCustomerQuery.CustName;
            txtCustShortName.Text = potCustomerQuery.CustShortName;
            cmbCustType.SelectedValue = potCustomerQuery.CustTypeID.ToString();
            txtLegalRep.Text = potCustomerQuery.LegalRep;
            txtLegalRepTitle.Text = potCustomerQuery.LegalRepTitle;
            txtRegCap.Text = potCustomerQuery.RegCap.ToString();
            txtRegAddr.Text = potCustomerQuery.RegAddr;
            txtRegCode.Text = potCustomerQuery.RegCode;
            txtTaxCode.Text = potCustomerQuery.TaxCode;
            txtBankName.Text = potCustomerQuery.BankName;
            txtBankAcct.Text = potCustomerQuery.BankAcct;
            txtOfficeAddr.Text = potCustomerQuery.OfficeAddr;
            txtPostAddr.Text = potCustomerQuery.PostAddr;
            txtPostCode.Text = potCustomerQuery.PostCode;
            txtOfficeAddr.Text = potCustomerQuery.OfficeAddr;
            txtOfficeAddr2.Text = potCustomerQuery.OfficeAddr2;
            txtOfficeAddr3.Text = potCustomerQuery.OfficeAddr3;
            txtPostAddr.Text = potCustomerQuery.PostAddr;
            txtPostAddr2.Text = potCustomerQuery.PostAddr2;
            txtPostAddr3.Text = potCustomerQuery.PostAddr3;
            txtWeb.Text = potCustomerQuery.WebURL;
            this.ddlSourceType.SelectedValue = potCustomerQuery.SourceTypeId.ToString();
            this.ddlCreditLevel.SelectedValue = potCustomerQuery.CreditLevelId.ToString();
            //txtContactorName.Text = potCustomerQuery.ContactorName;
            //txtTitle.Text = potCustomerQuery.Title;
            //txtOfficeTel.Text = potCustomerQuery.OfficeTel;
            //txtMobileTel.Text = potCustomerQuery.MobileTel;
            //txtEMail.Text = potCustomerQuery.EMail;
            //txtFax.Text = potCustomerQuery.Fax;
            DDownListCurrencyType.SelectedValue = potCustomerQuery.CurTypeID.ToString();
            cmbCommOper.SelectedValue = potCustomerQuery.CommOper.ToString();

        }
    }
    #endregion



    private bool checkSMSParaCustCode() 
    {
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();

        ds = baseBO.QueryDataSet("Select AutoCustCode From SMSPara");

        if (Convert.ToInt32(ds.Tables[0].Rows[0]["AutoCustCode"]) == SMSPara.AUTO_YES)
        {
            return true;    
        }
        return false; 
    }


    protected void clearText()
    {
        //txtCustCode.Text = BaseApp.GetCustumerID("PotCustomer", "CustCode").ToString();
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        //potCustomer.CustType = cmbCustType.SelectedIndex;
        txtLegalRep.Text = "";
        txtLegalRepTitle.Text = "";
        txtRegCap.Text = "0";
        txtRegAddr.Text = "";
        txtRegCode.Text = "";
        txtTaxCode.Text = "";
        txtBankName.Text = "";
        txtBankAcct.Text = "";
        txtOfficeAddr.Text = "";
        txtPostAddr.Text = "";
        txtPostCode.Text = "";
        txtWeb.Text = "";
        //txtContactorName.Text = "";
        //txtTitle.Text = "";
        //txtOfficeTel.Text = "";
        //txtMobileTel.Text = "";
        //txtEMail.Text = "";
        //txtFax.Text = "";

        txtOfficeAddr2.Text = "";
        txtOfficeAddr3.Text = "";
        txtPostAddr2.Text = "";
        txtPostAddr3.Text = "";
    }
    /// <summary>
    /// 判断意向商铺是否输入完整
    /// </summary>
    private bool CheckPotShop()
    {
        BaseBO objBaseBo = new BaseBO();
        //objBaseBo.WhereClause = "custid=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        objBaseBo.WhereClause = "custid=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]) + " and shopstatus=1";
        Resultset rs = objBaseBo.Query(new PotShop());
        if (rs.Count < 1)
            return false;

        PotShop objPotShop = rs.Dequeue() as PotShop;
        if (objPotShop.RentalPrice == 0)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckRentalPrice") + "'", true);
            return false;
        }
        if (objPotShop.RentArea == 0) 
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckRentArea") + "'", true);
            return false; 
        }
        if (objPotShop.RentInc == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckRentInc") + "'", true);
            return false;
        }
        if (objPotShop.Pcent == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckPcent") + "'", true);
            return false;
        }
        if (objPotShop.MainBrand == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckMainBrand") + "'", true);
            return false;
        }
        if (DateTime.Compare(objPotShop.ShopStartDate, objPotShop.ShopEndDate) > 0)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckSEDate") + "'", true);
            return false;
        }
        //if (objPotShop.ShopEndDate == "") return false;
        if (objPotShop.UnitId == "")
        {
            return false;
        }
        if (objPotShop.PotShopName == "")
        {
            return false;
        }
        return true;
    }
    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        if (!this.CheckPotShop())//检查是否保存了意向商铺
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CheckPotShopFalse") + "'", true);
            return;
        }
        try
        {
            int voucherID = 0;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";
            voucherID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);

            if (Request.Cookies["Sequence"].Values["SequenceID"] != "")
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Sequence"].Values["SequenceID"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
            }
            btnPutIn.Enabled = false;
            clearCookies();
            clearText();
            Session["Import"] = "";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("潜在商户提交审批信息错误:", ex);
        }
    }
    /// <summary>
    /// 检查客户编号是否已存在
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    private bool CheckCode(string strCode)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select CustCode from potcustomer where CustCode='" + strCode + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
            return false;
    }
    private int CheckUpdateCode(string strCode)
    {
        BaseBO objBaseBo = new BaseBO();
        int intCustID=new int();
        DataSet ds = objBaseBo.QueryDataSet("select CustId,CustCode from potcustomer where CustCode='" + strCode + "'");
        if (ds.Tables[0].Rows.Count == 1)
        {
            intCustID=Convert.ToInt32(ds.Tables[0].Rows[0]["CustId"]);
            return intCustID;
        }
        else if (ds.Tables[0].Rows.Count == 0)
        {
            return 0;
        }
        else 
        {
            return -1;
        }
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        Session["Import"] = "";
        if (this.checkSMSParaCustCode())
        {
            if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_UP)
            {
                UpdatePotCustumer();
            }
            else if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_IN)
            {
                InsertPotCistumer();
            }
        }
        else
        {
            if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_UP)
            {
                int intCustID = Convert.ToInt32 (Request.Cookies["Custumer"].Values["CustumerID"]);
                int intRet=this.CheckUpdateCode(this.txtCustCode.Text.Trim());

                if (intRet ==intCustID || intRet ==0 )    
                {
                    UpdatePotCustumer();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
                    //this.txtCustCode.Text = "";
                    return;
                }
            }
            else if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_IN)
            {
                if (this.txtCustCode.Text.Trim() == "")
                {
                    string strError = "对不起,该编码不能为空!";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strError + "'", true);
                    return;
                }
                if (this.CheckCode(this.txtCustCode.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
                    //this.txtCustCode.Text = "";
                    return;
                }
                InsertPotCistumer();
            }
        }


        btnPutIn.Enabled = true;
        btnBlankOut.Enabled = true;
    }


    private void clearCookies()
    {

        /*删除Cookies商户ID*/
        HttpCookie cookiesCustumer = new HttpCookie("Custumer");

        cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
        cookiesCustumer.Values.Add("CustumerID", "");
        Response.AppendCookie(cookiesCustumer);

        /*删除Cookies驳回状态*/
        HttpCookie cookiesDisprove = new HttpCookie("Disprove");

        cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
        cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
        Response.AppendCookie(cookiesDisprove);


        /*删除Cookies工作流ID和节点ID*/
        HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

        cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
        cookiesWorkFlow.Values.Add("WorkFlowID", "");
        cookiesWorkFlow.Values.Add("NodeID", "");
        Response.AppendCookie(cookiesWorkFlow);

        /*删除节点编号存入Cookies*/
        HttpCookie cookiesSequence = new HttpCookie("Sequence");

        cookiesSequence.Expires = System.DateTime.Now.AddHours(1);
        cookiesSequence.Values.Add("SequenceID", "");
        Response.AppendCookie(cookiesSequence);

        /*删除草稿节点编号存入Cookies*/
        HttpCookie cookiesReturnSequence = new HttpCookie("Info");

        cookiesReturnSequence.Expires = System.DateTime.Now.AddDays(1);
        cookiesReturnSequence.Values.Add("ReturnSequence", "");

        Response.AppendCookie(cookiesReturnSequence);
    }

    private void InsertPotCistumer()
    {
        int custcomerID = 0;
       // PotCustContactInfo potCustContact = new PotCustContactInfo();
        PotCustomer potCustomer = new PotCustomer();
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        String voucherHints = txtCustShortName.Text.Trim();
        String voucherMemo = "";

        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "CustCode='" + txtCustCode.Text.Trim() + "'";
        Resultset rs = baseBO.Query(potCustomer);
        if (rs.Count > 1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            txtCustCode.Focus();
            return;
        }

        try
        {
            potCustomer.CustID = BaseApp.GetCustID();

            if (this.checkSMSParaCustCode())
            {
                potCustomer.CustCode = BaseApp.GetSMSParaNextCustCode().ToString();
            }
            else
            {
                potCustomer.CustCode = txtCustCode.Text.Trim();
            }
            
            potCustomer.CustName = txtCustName.Text.Trim();
            potCustomer.CustShortName = txtCustShortName.Text.Trim();
            potCustomer.CustTypeID = Convert.ToInt32(cmbCustType.SelectedValue);
            potCustomer.LegalRep = txtLegalRep.Text.Trim();
            potCustomer.LegalRepTitle = txtLegalRepTitle.Text.Trim();
            potCustomer.RegCap = Convert.ToDecimal(txtRegCap.Text);
            potCustomer.RegAddr = txtRegAddr.Text.Trim();
            potCustomer.RegCode = txtRegCode.Text.Trim();
            potCustomer.TaxCode = txtTaxCode.Text.Trim();
            potCustomer.BankName = txtBankName.Text.Trim();
            potCustomer.BankAcct = txtBankAcct.Text.Trim();
            potCustomer.OfficeAddr = txtOfficeAddr.Text.Trim();
            potCustomer.OfficeAddr2 = txtOfficeAddr2.Text.Trim();
            potCustomer.OfficeAddr3 = txtOfficeAddr3.Text.Trim();
            potCustomer.PostAddr = txtPostAddr.Text.Trim();
            potCustomer.PostAddr2 = txtPostAddr2.Text.Trim();
            potCustomer.PostAddr3 = txtPostAddr3.Text.Trim();
            potCustomer.PostCode = txtPostCode.Text.Trim();
            potCustomer.WebUrl = txtWeb.Text.Trim();
            potCustomer.CreateUserID = sessionUser.UserID;
            potCustomer.CreateTime = DateTime.Now;
            potCustomer.CustomerStatus = PotCustomer.POTCUSTOMER_DRAFT;
            try { potCustomer.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue.ToString()); }
            catch { potCustomer.CommOper = 0; }
            try { potCustomer.SourceTypeId = Convert.ToInt32(this.ddlSourceType.SelectedValue.ToString()); }
            catch { potCustomer.SourceTypeId = 0; }
            try { potCustomer.CreditLevelId = Convert.ToInt32(this.ddlCreditLevel.SelectedValue.ToString()); }
            catch { potCustomer.CreditLevelId = 0; }
            //potCustomer.Fax = txtFax.Text.Trim();
            potCustomer.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);

            //potCustContact.CustContactID = BaseApp.GetCustContactID();
            //potCustContact.ContactorName = txtContactorName.Text;
            //potCustContact.Title = txtTitle.Text;
            //potCustContact.OfficeTel = txtOfficeTel.Text;
            //potCustContact.MobileTel = txtMobileTel.Text;
            //potCustContact.EMail = txtEMail.Text;
            //potCustContact.CustID = potCustomer.CustID;
            Session["import"] = "";//不是导入的信息
            baseTrans.BeginTrans();

            //if (baseTrans.Insert(potCustomer) == -1 || baseTrans.Insert(potCustContact) == -1)
            if (baseTrans.Insert(potCustomer) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                baseTrans.Rollback();
                return;
            }
       
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);

            voucherID = potCustomer.CustID;
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);

            WrkFlwApp.CommitVoucherDraft(wrkFlwID, nodeID, WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);

            /*保存草稿提交的节点ID*/
            HttpCookie cookies = new HttpCookie("Info");

            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());

            Response.AppendCookie(cookies);

            /*把节点编号存入Cookies*/
            HttpCookie cookiesSequence = new HttpCookie("Sequence");

            cookiesSequence.Expires = System.DateTime.Now.AddHours(1);
            cookiesSequence.Values.Add("SequenceID", WrkFlwApp.returnSequence.ToString());
            Response.AppendCookie(cookiesSequence);

            /*把商户ID存入Cookies*/
            HttpCookie cookiesCustumerID = new HttpCookie("Custumer");

            cookiesCustumerID.Expires = System.DateTime.Now.AddHours(1);
            cookiesCustumerID.Values.Add("CustumerID", potCustomer.CustID.ToString());
            Response.AppendCookie(cookiesCustumerID);

            /*保存完信息后,把更新的状态存入Cookies*/
            HttpCookie cookiesDisprove = new HttpCookie("Disprove");

            cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
            Response.AppendCookie(cookiesDisprove);

            baseTrans.Commit();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "    " + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustCode") + ":" + potCustomer.CustCode + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.LogBiz("添加潜在商户信息错误:", ex);
            baseTrans.Rollback();
        }
    }

    private void UpdatePotCustumer()
    {
        int custcomerID = 0;
        //PotCustContactInfo potCustContact = new PotCustContactInfo();
        PotCustomer potCustomer = new PotCustomer();
        BaseTrans baseTrans = new BaseTrans();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        String voucherHints = txtCustShortName.Text.Trim();

        try
        {
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);
            int sequenceID = Convert.ToInt32(Request.Cookies["Sequence"].Values["SequenceID"]);

            potCustomer.CustCode = txtCustCode.Text;
            potCustomer.CustName = txtCustName.Text;
            potCustomer.CustShortName = txtCustShortName.Text;
            potCustomer.CustTypeID = Convert.ToInt32(cmbCustType.SelectedValue);
            potCustomer.LegalRep = txtLegalRep.Text;
            potCustomer.LegalRepTitle = txtLegalRepTitle.Text;
            potCustomer.RegCap = Convert.ToDecimal(txtRegCap.Text);
            potCustomer.RegAddr = txtRegAddr.Text;
            potCustomer.RegCode = txtRegCode.Text;
            potCustomer.TaxCode = txtTaxCode.Text;
            potCustomer.BankName = txtBankName.Text;
            potCustomer.BankAcct = txtBankAcct.Text;
            potCustomer.OfficeAddr = txtOfficeAddr.Text;
            potCustomer.OfficeAddr2 = txtOfficeAddr2.Text;
            potCustomer.OfficeAddr3 = txtOfficeAddr3.Text;
            potCustomer.PostAddr = txtPostAddr.Text;
            potCustomer.PostAddr2 = txtPostAddr2.Text;
            potCustomer.PostAddr3 = txtPostAddr3.Text;
            potCustomer.PostCode = txtPostCode.Text;
            potCustomer.WebUrl = txtWeb.Text;
            try { potCustomer.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue.ToString()); }
            catch { potCustomer.CommOper = 0; }
            try { potCustomer.SourceTypeId = Convert.ToInt32(this.ddlSourceType.SelectedValue.ToString()); }
            catch { potCustomer.SourceTypeId = 0; }
            try { potCustomer.CreditLevelId = Convert.ToInt32(this.ddlCreditLevel.SelectedValue.ToString()); }
            catch { potCustomer.CreditLevelId = 0; }
            //potCustomer.Fax = txtFax.Text.Trim();
            potCustomer.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);


            //potCustContact.CustContactID = BaseApp.GetCustContactID();
            //potCustContact.ContactorName = txtContactorName.Text;
            //potCustContact.Title = txtTitle.Text;
            //potCustContact.OfficeTel = txtOfficeTel.Text;
            //potCustContact.MobileTel = txtMobileTel.Text;
            //potCustContact.EMail = txtEMail.Text;
            //potCustContact.CustID = potCustomer.CustID;

            baseTrans.BeginTrans();

            baseTrans.WhereClause = "CustID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            int result = baseTrans.Update(potCustomer);
            if (result == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                baseTrans.Rollback();
                return;
            }


            baseTrans.WhereClause = "CustID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            //if (baseTrans.Update(potCustContact) == -1)保存联系人
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            //    baseTrans.Rollback();
            //    return;
            //}

            if (baseTrans.ExecuteUpdate("Update WrkFlwEntity Set VoucherHints = '" + potCustomer.CustShortName + "' Where WrkFlwID= " + wrkFlwID + " And NodeID=" + nodeID + " And Sequence=" + sequenceID) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                baseTrans.Rollback();
                return;
            }

            baseTrans.Commit();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.LogBiz("添加潜在商户信息错误:", ex);
            baseTrans.Rollback();
        }
    }
    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
        Session["Import"] = "";
        try
        {
            int voucherID = 0;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";
            voucherID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);

            if (Request.Cookies["Sequence"].Values["SequenceID"] != "")
            {
                WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Sequence"].Values["SequenceID"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            btnPutIn.Enabled = false;
            clearCookies();
            clearText();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("潜在商户提交审批信息错误:", ex);
        }
    }
    protected void btnMessage_Click(object sender, EventArgs e)
    {

    }
    protected void btnPeople_Click(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 得到商户信息
    /// </summary>
    private void GetPotCustomerInfo(string strType,string strValue)
    {
        BaseBO objBaseBo = new BaseBO();
        string strSql = "";
        if (strType == "Code")
        {
            strSql = "select custid,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,WebUrl,CurTypeID,SourceTypeId,CreditLevelId,CommOper from  PotCustomer  where CustCode='" + strValue + "'";
        }
        else
            strSql = "select custid,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,WebUrl,CurTypeID,SourceTypeId,CreditLevelId,CommOper from PotCustomer where CustName='" + strValue + "'";

        DataSet ds = objBaseBo.QueryDataSet(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            this.txtCustCode.Text = ds.Tables[0].Rows[0]["CustCode"].ToString();
            this.txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            this.txtCustShortName.Text = ds.Tables[0].Rows[0]["CustShortName"].ToString();
            try { this.cmbCustType.SelectedValue = ds.Tables[0].Rows[0]["CustTypeID"].ToString(); }
            catch { }
            this.txtLegalRep.Text = ds.Tables[0].Rows[0]["LegalRep"].ToString();//法人
            this.txtLegalRepTitle.Text = ds.Tables[0].Rows[0]["LegalRepTitle"].ToString();//法人职务
            this.txtRegCap.Text = ds.Tables[0].Rows[0]["RegCap"].ToString();//注册资金
            try { this.DDownListCurrencyType.SelectedValue = ds.Tables[0].Rows[0]["CurTypeID"].ToString(); }//币种
            catch { }
            this.txtRegAddr.Text = ds.Tables[0].Rows[0]["RegAddr"].ToString();//注册地
            this.txtRegCode.Text = ds.Tables[0].Rows[0]["RegCode"].ToString();//工商注册号
            this.txtTaxCode.Text = ds.Tables[0].Rows[0]["TaxCode"].ToString();//税号
            this.txtBankName.Text = ds.Tables[0].Rows[0]["BankName"].ToString();//开户银行
            this.txtBankAcct.Text = ds.Tables[0].Rows[0]["BankAcct"].ToString();//银行账号
            this.txtOfficeAddr.Text = ds.Tables[0].Rows[0]["OfficeAddr"].ToString();//办公地址
            this.txtOfficeAddr2.Text = ds.Tables[0].Rows[0]["OfficeAddr2"].ToString();//办公地址2
            this.txtOfficeAddr3.Text = ds.Tables[0].Rows[0]["OfficeAddr3"].ToString();//办公地址3
            this.txtPostAddr.Text = ds.Tables[0].Rows[0]["PostAddr"].ToString();//邮寄地址
            this.txtPostAddr2.Text = ds.Tables[0].Rows[0]["PostAddr2"].ToString();//邮寄地址2
            this.txtPostAddr3.Text = ds.Tables[0].Rows[0]["PostAddr3"].ToString();//邮寄地址3
            this.txtPostCode.Text = ds.Tables[0].Rows[0]["PostCode"].ToString();//邮政编码
            this.txtWeb.Text = ds.Tables[0].Rows[0]["WebUrl"].ToString();//企业主页
            try { this.ddlCreditLevel.SelectedValue = ds.Tables[0].Rows[0]["CreditLevelId"].ToString(); }//信用等级
            catch { }
            try { this.ddlSourceType.SelectedValue = ds.Tables[0].Rows[0]["SourceTypeId"].ToString(); }//商户来源
            catch { }
            try { this.cmbCommOper.SelectedValue = ds.Tables[0].Rows[0]["CommOper"].ToString(); }//招商员
            catch { }
            //添加工作流节点
            BaseTrans objBaseTrans = new BaseTrans();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            objBaseTrans.BeginTrans();
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);

            int voucherID = Int32.Parse(ds.Tables[0].Rows[0]["custid"].ToString());
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);

            WrkFlwApp.CommitVoucherDraft(wrkFlwID, nodeID, WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, objBaseTrans);
            objBaseTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);

            //把状态存入cookies
            HttpCookie cookiesDisprove = new HttpCookie("Disprove");
            cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
            Response.AppendCookie(cookiesDisprove);

            /*把商户ID存入Cookies*/
            HttpCookie cookiesCustumer = new HttpCookie("Custumer");

            cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
            cookiesCustumer.Values.Add("CustumerID", ds.Tables[0].Rows[0]["custid"].ToString());
            Response.AppendCookie(cookiesCustumer);

            /*把节点编号存入Cookies*/
            HttpCookie cookiesSequence = new HttpCookie("Sequence");

            cookiesSequence.Expires = System.DateTime.Now.AddHours(1);
            cookiesSequence.Values.Add("SequenceID", WrkFlwApp.returnSequence.ToString());
            Response.AppendCookie(cookiesSequence);

            /*工作流id和节点id存入Cookies*/
            HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");
            cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
            cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
            cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
            Response.AppendCookie(cookiesWorkFlow);

            
            this.txtCustCode.Enabled = false;
            this.txtCustName.Enabled = false;
            Session["Import"] = "true";
            btnPutIn.Enabled = true;
            btnMessage.Enabled = true;
            this.btnPeople.Enabled = true;
            this.btnBlankOut.Enabled = true;
        }
    }
    /// <summary>
    /// 商户编号自动检索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtCustCode_TextChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] == "New" || Request.QueryString["Type"] == null)
        {
            if (this.txtCustCode.Text.Trim() != "")
            {
                BaseBO objBaseBo = new BaseBO();
                if (objBaseBo.QueryDataSet("select custcode from potcustomer where custcode='" + this.txtCustCode.Text.Trim() + "'").Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirm", "conf()", true);
                }
            }
        }
    }
    /// <summary>
    /// 商户名称自动检索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtCustName_TextChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] == "New" || Request.QueryString["Type"] == null)
        {
            if (this.txtCustName.Text.Trim() != "")
            {
                BaseBO objBaseBo = new BaseBO();
                if (objBaseBo.QueryDataSet("select custname from potcustomer where custname='" + this.txtCustName.Text.Trim() + "'").Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirm", "conf()", true);
                }
            }
        }
    }
    /// <summary>
    /// 检索数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchData_Click(object sender, EventArgs e)
    {
        this.SearchData();
    }
    private void SearchData()
    {
        if (Request.QueryString["Type"] == "New" || Request.QueryString["Type"] == null)
        {
            if(this.txtCustCode.Text.Trim()!="")
                this.GetPotCustomerInfo("Code", this.txtCustCode.Text.Trim());
            else if (this.txtCustName.Text.Trim() != "")
                this.GetPotCustomerInfo("Name", this.txtCustName.Text.Trim());
        }
    }
}
