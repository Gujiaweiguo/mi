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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.CustLicense;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.PotCust;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;

public partial class RptBaseMenu_RptCustomerInfo : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }

    }


    /* 初始化下拉列表 */
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "storestatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

    }
    private void InitDDL()
    {
        //绑定客户类型
        BaseBO baseBo = new BaseBO();
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        ddlCustTypeID.Items.Add(new ListItem("", ""));
        Resultset rs1 = baseBo.Query(new CustType());
        foreach (CustType tradeDef in rs1)
            ddlCustTypeID.Items.Add(new ListItem(tradeDef.CustTypeName, tradeDef.CustTypeID.ToString()));
     
        //绑定证照类型
        int[] CustLicenseType = CustLicenseInfo.GetPotCustLicenseType();
        int L = CustLicenseType.Length + 1;
        ddlCustLicenseType.Items.Add(new ListItem("", ""));
        for (int j = 1; j < L; j++)
        {
            ddlCustLicenseType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustLicenseInfo.GetPotCustLicenseTypeDesc(CustLicenseType[j - 1])), CustLicenseType[j - 1].ToString()));
        }

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx"); ;
        
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[24];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[24];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXCustCode";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustShortName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustShortName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXCustTypeID";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_CustTypeID");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXLegalRep";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_LegalRep");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXLegalRepTitle";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_LegalRepTitle");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXRegCap";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblRegCap");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXBankName";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_BankName");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXBankAcct";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_BankAcct");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXRegCode";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_RegCode");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTaxCode";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_TaxCode");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXRegAddr";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_RegAddr");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXOfficeAddr";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_OfficeAddr");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXPostAddr";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_PostAddr");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXPostCode";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_PostCode");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXWebUrl";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_WebUrl");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXContractMan";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_ContractMan");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXMobileTel";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_MobileTel");
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXCustLicenseStartDate";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_CustLicenseStartDate");
        paraField[18].CurrentValues.Add(discreteValue[18]);

        paraField[19] = new ParameterField();
        paraField[19].Name = "REXCustLicenseEndDate";
        discreteValue[19] = new ParameterDiscreteValue();
        discreteValue[19].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_CustLicenseEndDate");
        paraField[19].CurrentValues.Add(discreteValue[19]);

        paraField[20] = new ParameterField();
        paraField[20].Name = "REXTitle";
        discreteValue[20] = new ParameterDiscreteValue();
        discreteValue[20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_Title");
        paraField[20].CurrentValues.Add(discreteValue[20]);

        paraField[21] = new ParameterField();
        paraField[21].Name = "REXTotalAmt";
        discreteValue[21] = new ParameterDiscreteValue();
        discreteValue[21].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[21].CurrentValues.Add(discreteValue[21]);

        paraField[22] = new ParameterField();
        paraField[22].Name = "REXMallTitle";
        discreteValue[22] = new ParameterDiscreteValue();
        discreteValue[22].Value = Session["MallTitle"].ToString();
        paraField[22].CurrentValues.Add(discreteValue[22]);

        paraField[23] = new ParameterField();
        paraField[23].Name = "REXBizProject";
        discreteValue[23] = new ParameterDiscreteValue();
        discreteValue[23].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[23].CurrentValues.Add(discreteValue[23]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string func_sql = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            func_sql = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        string str_sql = " SELECT store.storeshortname,store.orderid,Customer.CustCode,Customer.CustName, Customer.CustShortName,(Select CustTypeName from CustType where CustTypeID=Customer.CustTypeID) as CustTypeID , Customer.IsBlacklist, Customer.LegalRep,  " +
                         " Customer.LegalRepTitle, Customer.RegCap, Customer.BankName, Customer.BankAcct, Customer.RegCode, Customer.TaxCode, " +
                         " Customer.RegAddr, Customer.OfficeAddr, Customer.PostAddr, Customer.PostCode, Customer.WebUrl, CustContact.ContactMan, " +
                         " CustContact.MobileTel, CustLicense.CustLicenseStartDate,CustLicense.CustLicenseEndDate" +
                         " from Customer left join CustLicense On (Customer.CustId=CustLicense.CustId) " +
                         " Left Join CustContact On (Customer.CustId=CustContact.CustId) " +
                         "Left Join contract ON (Customer.CustId=contract.CustId)"+
                         "inner Join ConShop ON(contract.contractid=conshop.contractid)"+
                         "Left Join store ON (conshop.storeid=store.storeid)"+
                         " Where 1=1 and store.storestatus=1 " + func_sql;

        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustCode.Text + "%'";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text + "%'";
        }
        if (ddlCustTypeID.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustTypeID like '%" + ddlCustTypeID.Text + "%'";
        }
        if (ddlCustLicenseType.Text != "")
        {
            str_sql = str_sql + " AND CustLicense.CustLicenseType like '%" + ddlCustLicenseType.Text + "%'";
        }
        if (txtStartDate.Text != "")
        {
            //str_sql = str_sql + " AND CustLicense.CustLicenseStartDate like '%" + ddlTradeID.Text + "%'";
        }
        if (txtEndDate.Text != "")
        {
            //str_sql = str_sql + " AND CustLicense.CustLicenseEndDate like '%" + ddlTrade2ID.Text + "%'";
        }
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND store.storeid='" + ddlBizproject.SelectedValue + " '";
        }


        str_sql = str_sql + " Order by store.storeshortname,Customer.Custid ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\CustomerInfo.rpt";

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlBizproject.SelectedValue = null;
        txtCustCode.Text = "";
        txtCustName.Text = "";
        ddlCustTypeID.SelectedValue = null;
        ddlCustLicenseType.SelectedValue = null;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }
}

