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
using Lease.PotBargain;
using RentableArea;
using Base.Util;

public partial class ReportM_Customer_CustomerQuery : System.Web.UI.Page
{
    private CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument1;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
        }
    }

    private void InitDDL()
    {
        //绑定客户类型
        BaseBO baseBo = new BaseBO();
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        ddlCustTypeID.Items.Add(new ListItem("", ""));
        Resultset rs1 = baseBo.Query(new ChargeType());
        foreach (ChargeType tradeDef in rs1)
            ddlCustTypeID.Items.Add(new ListItem(tradeDef.ChargeTypeName, tradeDef.ChargeTypeID.ToString()));

    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[11];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[11];
        ParameterRangeValue rangeValue = new ParameterRangeValue(); 
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "CustCodeREX";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[0].CurrentValues.Add(discreteValue[0]); 

        paraField[1] = new ParameterField();
        paraField[1].Name = "CustNameREX";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "CustShortNameREX";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustShortName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "CustTypeIDREX";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_CustTypeID");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "LegalRepTitleREX";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_LegalRepTitle");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "RegCapREX";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_RegCap");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "BankNameREX";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_BankName");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "BankAcctREX";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_BankAcct");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "RegCodeREX";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_RegCode");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "LegalRepREX";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_LegalRep");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "CRTitelREX";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerQuery_Title");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "SELECT a.CustCode,a.CustName, a.CustShortName, a.CustTypeID, a.IsBlacklist, a.LegalRep," +
                      "a.LegalRepTitle, a.RegCap, a.BankName, a.BankAcct, a.RegCode, a.TaxCode," +
                      "a.RegAddr, a.OfficeAddr, a.PostAddr, a.PostCode, a.WebUrl, c.ContactMan," +
                      "c.MobileTel, b.CustLicenseStartDate, b.CustLicenseEndDate " +
                      "FROM Customer AS a Left JOIN " +
                      "CustLicense AS b ON a.CustID = b.CustID Left JOIN " +
                      "CustContact AS c ON a.CustID = c.CustID" +
                      " WHERE 1=1";
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND a.CustCode like '%" + txtCustCode.Text + "%'";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND a.CustName like '%" + txtCustName.Text + "%'";
        }
        if (ddlCustTypeID.Text != "")
        {
            str_sql = str_sql + " AND b.CustLicenseType like '%" + ddlCustTypeID.Text + "%'";
        }
        if (ddlCustLicenseType.Text != "")
        {
            str_sql = str_sql + " AND b.CustLicenseType like '%" + ddlCustLicenseType.Text + "%'";
        }
        if (txtStartDate.Text != "" && txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND b.CustLicenseStartDate between  '" + txtStartDate.Text + "' And '" + txtEndDate.Text + "'";
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\CustomerCR.rpt";
        
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
}
