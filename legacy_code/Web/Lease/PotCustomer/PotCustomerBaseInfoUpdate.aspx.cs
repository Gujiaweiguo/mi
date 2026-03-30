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
using Base;
using Lease.PotCustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using Lease.PotCust;
using Base.Page;
using Lease.Formula;
public partial class Lease_PotCustomer_PotCustomerBaseInfoUpdate : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;
    public string baseInfo;

    public string PotCustomer_Basic;
    public string PotCustomer_ClientCard;
    public string PotCustomer_TitlePalaver;
    public string PotCustomer_OprInfo;//经营概况
    private string strCustID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //绑定信用等级
            BaseInfo.BaseCommon.BindDropDownList("select CreditLevelId,CreditLevelName from CreditLevel where status=1", "CreditLevelId", "CreditLevelName", this.ddlCreditLevel);
            //绑定客户来源
            BaseInfo.BaseCommon.BindDropDownList("select SourceTypeId,SourceTypeName from SourceType where SourceTypeStatus=1", "SourceTypeId", "SourceTypeName", this.ddlSourceType);

            BaseBO baseBO = new BaseBO();
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

            /*招商员选择列表*/
            baseBO.WhereClause = "UserStatus=" + UserInfo.USER_STATUS_VALID;
            baseBO.GroupBy = "a.userid,UserName,a.UserCode,WorkNo,OfficeTel,UserStatus";
            rs = baseBO.Query(new UserInfo());
            foreach (UserInfo user in rs)
            {
                cmbCommOper.Items.Add(new ListItem(user.UserName, user.UserID.ToString()));
            }

            int tempCustID = 0;
            if (Request["custID"] != null)
            {
                HttpCookie cookies = new HttpCookie("Info1");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("custid", Request["custID"].ToString());
                Response.AppendCookie(cookies);

                QueryPotCustomerBaseInfo(Convert.ToInt32(Request["custID"].ToString()));
                tempCustID = Convert.ToInt32(Request["custID"].ToString());
                strCustID = Request["custID"].ToString();
            }
            else if (Request.Cookies["Info1"] != null)
            {
                tempCustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
                QueryPotCustomerBaseInfo(Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]));
            }
            ViewState["custID"] = tempCustID;

            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            PotCustomer_ClientCard = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ClientCard");
            PotCustomer_TitlePalaver = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblTradeBrand");
            PotCustomer_OprInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ManageSurvey");//经营概况
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //clearText();
        Response.Redirect("~/Lease/PotCustomer/PotCustomerBaseInfoUpdate.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        PotCustomer potCustomer = new PotCustomer();
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
        potCustomer.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);

        potCustomer.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue.ToString());
        //PotCustContactInfo potCustContact = new PotCustContactInfo();
        //potCustContact.CustContactID = BaseApp.GetCustContactID();
        //potCustContact.ContactorName = txtContactorName.Text;
        //potCustContact.Title = txtTitle.Text;
        //potCustContact.OfficeTel = txtOfficeTel.Text;
        //potCustContact.MobileTel = txtMobileTel.Text;
        //potCustContact.EMail = txtEMail.Text;
        potCustomer.CreditLevelId = Convert.ToInt32(this.ddlCreditLevel.SelectedValue);
        potCustomer.SourceTypeId = Convert.ToInt32(this.ddlSourceType.SelectedValue);
        //potCustContact.CustID = potCustomer.CustID;
        
        //  potCustContact.OfficeAddr = txtOfficeAddr.Text;

        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        int result = baseTrans.Update(potCustomer);
        if (result != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "chooseCard", "chooseCard(0);", true);
            //page("a.CustID= 0");
            //BindGridViewPotCustLicense("CustID=0");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        //baseTrans.Update(potCustContact);

        baseTrans.Commit();
        //clearText();
        Response.Redirect("~/Lease/PotCustomer/PotCustomerBaseInfoUpdate.aspx");
    }

    private void QueryPotCustomerBaseInfo(int custid)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "a.CustID=" + custid;
        //ViewState["custID"] = GrdCust.SelectedRow.Cells[0].Text;
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
            txtOfficeAddr2.Text = potCustomerQuery.OfficeAddr2;
            txtOfficeAddr3.Text = potCustomerQuery.OfficeAddr3;
            txtPostAddr.Text = potCustomerQuery.PostAddr;
            txtPostAddr2.Text = potCustomerQuery.PostAddr2;
            txtPostAddr3.Text = potCustomerQuery.PostAddr3;
            txtWeb.Text = potCustomerQuery.WebURL;
            txtPostCode.Text = potCustomerQuery.PostCode;
            this.ddlCreditLevel.SelectedValue = potCustomerQuery.CreditLevelId.ToString();
            this.ddlSourceType.SelectedValue = potCustomerQuery.SourceTypeId.ToString();
            //txtContactorName.Text = potCustomerQuery.ContactorName;
            //txtTitle.Text = potCustomerQuery.Title;
            //txtOfficeTel.Text = potCustomerQuery.OfficeTel;
            //txtMobileTel.Text = potCustomerQuery.MobileTel;
            //txtEMail.Text = potCustomerQuery.EMail;
            cmbCommOper.SelectedValue = potCustomerQuery.CommOper.ToString();
            DDownListCurrencyType.SelectedValue = potCustomerQuery.CurTypeID.ToString();
            //BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        }
    }

    protected void clearText()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        //potCustomer.CustType = cmbCustType.SelectedIndex;
        txtLegalRep.Text = "";
        txtLegalRepTitle.Text = "";
        txtRegCap.Text = "";
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
        txtOfficeAddr2.Text = "";
        txtOfficeAddr3.Text = "";
        txtPostAddr2.Text = "";
        txtPostAddr3.Text = "";
        this.ddlSourceType.SelectedIndex = 0;
        this.ddlCreditLevel.SelectedIndex = 0;

    }
    protected void btnPeople_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "window.showModalDialog('PotCustContactBaseInfo.aspx?look=no&custID=" + ViewState["custID"] + "','window','dialogWidth=700px;dialogHeight=460px')", true);
    }
}
