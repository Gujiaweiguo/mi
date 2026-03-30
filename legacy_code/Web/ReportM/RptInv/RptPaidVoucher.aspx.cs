using System;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using CrystalDecisions.Shared;
using Base.Biz;
 

public partial class ReportM_RptInv_RptPaidVoucher : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvPaidPrint");
            txtInvStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtInvEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh"); 
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string str = "";
        if (txtContractCode.Text != "")
        {
            str += " AND Contract.ContractCode ='" + txtContractCode.Text.Trim() + "'";
        }
        if (txtCustCode.Text.Trim() != "")
        {
            str += " AND customer.custcode = '" + txtCustCode.Text.Trim() + "'  ";
        }
        if (txtCustName.Text.Trim() != "")
        {
            str += " AND customer.custname = '" + txtCustName.Text.Trim() + "'  ";
        }
        if (txtInvStartDate.Text.Trim() != "")
        {
            str += " AND invoicepay.InvPayDate >= '" + txtInvStartDate.Text.Trim() + "'  ";
        }
        if (txtInvEndDate.Text.Trim() != "")
        {
            str += " AND invoicepay.InvPayDate <= '" + txtInvEndDate.Text.Trim() + "'  ";
        }

        BindData(str);
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtContractCode.Text = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtInvStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtInvEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }
    private void BindData(string whereStr)
    {

        BaseBO baseBO = new BaseBO();
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[10];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[10];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvPaidPrint");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXInvComTitile";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvComTitile");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXContractID";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXChargeTypeName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ChargeTypeName");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXInvPeriod";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXPaidSum";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PaidSum");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXInvPayDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPayDate");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXMallTitle";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = Session["MallTitle"].ToString();
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXAmount";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }



        string str_sql = @"select subsidiary.subsname,  --公司名称
	                       contract.contractcode, --合同编码
	                       customer.custname,     --商户名称
	                       chargetype.chargetypename, --费用类型
	                       invoicepaydetail.InvPaidAmtl,  --付款金额
	                       convert(char(10),invoicepay.InvPayDate,120) as InvPayDate,  --付款日期
	                       convert(char(7),invoicedetail.period,120) as period  --记账月	 
                           from invoicepaydetail
                           inner join invoicepay on invoicepay.invpayid=invoicepaydetail.invpayid
                           inner join chargetype on invoicepaydetail.chargetypeid=chargetype.chargetypeid
                           inner join customer on invoicepay.custid=customer.custid
                           inner join contract on invoicepay.contractid=contract.contractid
                           inner join invoicedetail on invoicedetail.invdetailid=invoicepaydetail.invdetailid
                           inner join subsidiary on contract.subsid=subsidiary.subsid
                           left join conshop on (conshop.contractid=contract.contractid)
                           where 1=1 " + whereStr;


        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql + strAuth ;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\PaidVoucher.rpt";

        this.Response.Redirect("../ReportShow.aspx");
    }
}
