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
public partial class Lease_PotCustomer_PotCustomerBaseInfo : BasePage
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

            int tempCustID = 0;
            if (Request["custID"] != null)
            {
                HttpCookie cookies = new HttpCookie("Info1");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("custid", Request["custID"].ToString());
                Response.AppendCookie(cookies);

                QueryPotCustomerBaseInfo(Convert.ToInt32(Request["custID"].ToString()));
                tempCustID = Convert.ToInt32(Request["custID"].ToString());
            }
            else if(Request.Cookies["Info1"] != null)
            {
                tempCustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
                QueryPotCustomerBaseInfo(Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]));
            }
            ViewState["custID"] = tempCustID;
            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            PotCustomer_ClientCard = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ClientCard");
            PotCustomer_TitlePalaver = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblTradeBrand");
            PotCustomer_OprInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ManageSurvey");//经营概况
            //this.btnPeople.Attributes.Add("onclick", "ShowContact()");
        }
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
            txtPostCode.Text = potCustomerQuery.PostCode;
            txtWeb.Text = potCustomerQuery.WebURL;
            //txtContactorName.Text = potCustomerQuery.ContactorName;
            //txtTitle.Text = potCustomerQuery.Title;
            //txtOfficeTel.Text = potCustomerQuery.OfficeTel;
            //txtMobileTel.Text = potCustomerQuery.MobileTel;
            //txtEMail.Text = potCustomerQuery.EMail;
            txtCommOper.Text = potCustomerQuery.UserName;
            this.ddlSourceType.SelectedValue = potCustomerQuery.SourceTypeId.ToString();
            this.ddlCreditLevel.SelectedValue = potCustomerQuery.CreditLevelId.ToString();
            //BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        }
    }
    protected void btnPeople_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "window.showModalDialog('PotCustContactBaseInfo.aspx?look=yes&custID=" + ViewState["custID"] + "','window','dialogWidth=700px;dialogHeight=460px')", true);
    }
}
