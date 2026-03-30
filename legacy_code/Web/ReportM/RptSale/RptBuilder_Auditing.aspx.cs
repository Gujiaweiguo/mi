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
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class ReportM_RptSale_RptBuilder_Auditing : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBuilder_Auditing");
        }
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtContractCode.Text = "";
        txtCustCode.Text = "";
    }

    private void InitDDL()
    {
        txtBizSDate.Text = DateTime.Now.Year + "-01-01";
        txtBizEDate.Text = DateTime.Now.Year + "-12-31";
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBuilder_Auditing");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXContractCode";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXShopCode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rdo_ShopCode");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXChargeTypeName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Inv_lblChargeTypeName");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXOperationTime";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "BuilderAuditing_OperationTime");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXDebtor";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "BuilderAuditing_Debtor");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXCreditSide";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "BuilderAuditing_CreditSide");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXBalance";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "InvoiceHeader_lblBalance");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXSdate";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXtxtBizSDate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = txtBizSDate.Text.ToString();
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXtxtBizEDate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = txtBizEDate.Text.ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXMallTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = Session["MallTitle"].ToString();
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXCustCode";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseAreaType_CustCode");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXShopName";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXInvID";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("BaseInfo", "InvoiceHeader_lblInvID");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string strWhere = "";
        string authWhere = "";
        string str_sql = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            AuthBase.AUTH_SQL_SHOP = AuthBase.AUTH_SQL_SHOP;
            AuthBase.AUTH_SQL_BUILD = AuthBase.AUTH_SQL_BUILD;
            AuthBase.AUTH_SQL_FLOOR = AuthBase.AUTH_SQL_FLOOR;
            AuthBase.AUTH_SQL_CONTRACT = AuthBase.AUTH_SQL_CONTRACT;

            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }


        if (txtBizSDate.Text != "")
        {
            strWhere = " AND InvPeriod > = '" + txtBizSDate.Text + "'";
        }

        if (txtBizSDate.Text != "")
        {
            strWhere = strWhere + " AND InvPeriod < = '" + txtBizEDate.Text + "'";
        }

        if (txtCustCode.Text != "")
        {
            strWhere = strWhere + " AND CustCode ='" + txtCustCode.Text + "'";
        }

        if (txtContractCode.Text != "")
        {
            strWhere = strWhere + " AND ContractCode ='" + txtContractCode.Text + "'";
        }

        //strWhere = strWhere + authWhere;

        str_sql = "Select InvID,ChargeTypeName,CustCode,CustName,ContractCode,ShopCode,ShopName,InvPeriod,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Balance,BalanceL From BuilderAuditing Where 1=1 " + strWhere + 
                         " Order By ContractCode,InvPeriod,Status";
                            

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptBuilder_Auditing.rpt";

    }
}
