using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Lease.CustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using Lease.PotCust;
using Lease.Customer;
using Lease.Formula;

public partial class Lease_Customer_CustomerInfoModify : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;

    public string PotCustomer_Basic;
    public string PotCustomer_ClientCard;
    public string PotCustomer_lblTradeBrand;
    public string Customer_OprInfo;//经营概况

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
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
                HttpCookie cookies = new HttpCookie("Info1");//将客户编号存入Cookie中,info1为新建的Cookie的名称
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("custid", Request["custID"].ToString());
                Response.AppendCookie(cookies);

                QueryPotCustomerBaseInfo(Convert.ToInt32(Request["custID"].ToString()));
                tempCustID = Convert.ToInt32(Request["custID"].ToString());
            }
            else if (Request.Cookies["Info1"] != null)
            {
                QueryPotCustomerBaseInfo(Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]));
                tempCustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
            }

            ViewState["custID"] = tempCustID;

            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            PotCustomer_ClientCard = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ClientCard");
            PotCustomer_lblTradeBrand = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblTradeBrand");
            Customer_OprInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ManageSurvey");//经营概况
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Customer potCustomer = new Customer();
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
        potCustomer.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue.ToString());
        potCustomer.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);

        potCustomer.SourceTypeId = Int32.Parse(this.ddlSourceType.SelectedValue);
        potCustomer.CreditLevelId = Int32.Parse(this.ddlCreditLevel.SelectedValue);

        //CustContact potCustContact = new CustContact();
        //potCustContact.CustContactID = BaseApp.GetCustContactIDD();
        //potCustContact.ContactMan = txtContactorName.Text;
        //potCustContact.Title = txtTitle.Text;
        //potCustContact.OfficeTel = txtOfficeTel.Text;
        //potCustContact.MobileTel = txtMobileTel.Text;
        //potCustContact.EMail = txtEMail.Text;
        
        //potCustContact.CustID = potCustomer.CustID;
        //  potCustContact.OfficeAddr = txtOfficeAddr.Text;

        BaseTrans baseTrans = new BaseTrans();



        baseTrans.BeginTrans();
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        int result = baseTrans.Update(potCustomer);
        if (result != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        baseTrans.Commit();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Lease/Customer/CustomerInfoModify.aspx");
    }

    public void QueryPotCustomerBaseInfo(int custid)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "a.CustID=" + custid;
        rs = baseBO.Query(new CustomerQuery());
        if (rs != null)
        {
            CustomerQuery potCustomerQuery = rs.Dequeue() as CustomerQuery;
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
            txtPostCode.Text = potCustomerQuery.PostCode;
            txtWeb.Text = potCustomerQuery.WebURL;
            this.ddlCreditLevel.SelectedValue = potCustomerQuery.CreditLevelId.ToString();
            this.ddlSourceType.SelectedValue = potCustomerQuery.SourceTypeId.ToString();
            cmbCommOper.SelectedValue = potCustomerQuery.CommOper.ToString();
            DDownListCurrencyType.SelectedValue = potCustomerQuery.CurTypeID.ToString();
        }
    }

    protected void clearText()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
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
        txtOfficeAddr2.Text = "";
        txtOfficeAddr3.Text = "";
        txtPostAddr2.Text = "";
        txtPostAddr3.Text = "";
        this.ddlSourceType.SelectedIndex = 0;
        this.ddlCreditLevel.SelectedIndex = 0;
    }
    protected void btnPeople_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "window.showModalDialog('CustContact.aspx?look=no&custID=" + ViewState["custID"] + "','window','dialogWidth=700px;dialogHeight=460px')", true);
    }
}
