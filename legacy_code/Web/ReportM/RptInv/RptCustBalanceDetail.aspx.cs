using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Page;
using Lease.Contract;

public partial class RptBaseMenu_RptCustBalanceDetail : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }

    }


    /* 判断数据空值,返回默认值
     * 
     * 
     */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }
    /* 初始化下拉列表
     * 
     * 
     */
    private void InitDDL()
    {

        //绑定合同状态


        int[] contractStutas = Contract.GetContractTypeStatus();
        int k = contractStutas.Length + 1;
        ddlContractStatus.Items.Add(new ListItem("", ""));
        for (int j = 1; j < k; j++)
        {
            ddlContractStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(contractStutas[j - 1])), contractStutas[j - 1].ToString()));
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    /* 取得表头资源
     * 
     * 
     */ 
    private String GetRptResx()
    {
        String s = "%23Rpt_lblCustBalanceDetail";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labContractCode";
        s += "%23" + "LeaseholdContract_labContractStatus";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "Rpt_InvPeriod";
        s += "%23" + "Account_lblChargeMoney";
        s += "%23" + "Rpt_InvPaidAmt";
        s += "%23" + "Rpt_ChangeAmt";
        s += "%23" + "Rpt_SurplusAmt";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        sCon += "%26ContractCode=" + GetStrNull(this.txtContractID.Text);
        sCon += "%26CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26ContractStatus=" + GetStrNull(this.ddlContractStatus.Text);
        sCon += "%26" + "InvStartDate=" + GetdateNull(this.txtInvStartDate.Text);
        sCon += "%26" + "InvEndDate=" + GetdateNull(this.txtInvEndDate.Text);
        return sCon;
    }

    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-25" : s;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractID";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXShopName";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXConstractStatus";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractStatus");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXShopCode";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvPeriod";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXInvPayAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPayAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXInvPaidAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPaidAmt");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXAdjDisAmt";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAdjAmt");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXOtherAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_OtherAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustBalanceDetail_Title");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXTotalAmt";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[12].CurrentValues.Add(discreteValue[12]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = " select distinct Customer.CustCode,Customer.CustName,Contract.ContractCode,Contract.ContractStatus,InvoiceHeader.InvPeriod,InvoiceHeader.InvPayAmt,InvoiceHeader.InvPaidAmt,InvoiceHeader.InvAdjAmt+InvoiceHeader.InvDiscAmt AdjDisAmt,InvoiceHeader.InvPayAmt-InvoiceHeader.InvPaidAmt-InvoiceHeader.InvAdjAmt-InvoiceHeader.InvDiscAmt otherAmt" +
                 " from Contract,Customer,InvoiceHeader,InvoiceDetail " +
                 " where Contract.CustID = Customer.CustID " +
                 " and Contract.ContractID = InvoiceHeader.ContractID" +
                 " and Customer.CustID = InvoiceHeader.CustID" +
                 " and InvoiceHeader.invid = InvoiceDetail.invid";
        if (txtContractID.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode ='" + txtContractID.Text + "'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        }
        if (txtInvStartDate.Text != "")
        {
            str_sql = str_sql + " AND InvoiceDetail.InvStartDate >='" + txtInvStartDate.Text + "'";
        }
        if (txtInvEndDate.Text != "")
        {
            str_sql = str_sql + " AND InvoiceDetail.InvEndDate <='" + txtInvEndDate.Text + "'";
        }
        if (ddlContractStatus.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractStatus = '" + ddlContractStatus.Text + "'";
        }
        str_sql = str_sql + "  ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\CustBalanceDetail.rpt";

    }
}
