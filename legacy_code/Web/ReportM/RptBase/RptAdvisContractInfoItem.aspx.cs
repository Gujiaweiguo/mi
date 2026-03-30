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
using Lease.AdContract;

public partial class RptBaseMenu_RptAdvisContractInfoItem : BasePage
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

        //绑定广告位状态

        //int[] AdcontractType = AdContract.GetAdType();
        //int L = AdcontractType.Length + 1;
        //ddlAdType.Items.Add(new ListItem("", ""));
        //for (int j = 1; j < L; j++)
        //{
        //    ddlAdType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",AdContract.GetAdTypeDesc(AdcontractType[j - 1])), AdcontractType[j - 1].ToString()));
        //}
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
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        //paraField[0] = new ParameterField();
        //paraField[0].ParameterFieldName = "REXAdContractCode";
        //discreteValue[0] = new ParameterDiscreteValue();
        //discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_AdContractCode");
        //paraField[0].CurrentValues.Add(discreteValue[0]);

        //paraField[1] = new ParameterField();
        //paraField[1].Name = "REXCustCode";
        //discreteValue[1] = new ParameterDiscreteValue();
        //discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        //paraField[1].CurrentValues.Add(discreteValue[1]);

        //paraField[2] = new ParameterField();
        //paraField[2].Name = "REXCustName";
        //discreteValue[2] = new ParameterDiscreteValue();
        //discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        //paraField[2].CurrentValues.Add(discreteValue[2]);

        //paraField[3] = new ParameterField();
        //paraField[3].Name = "REXSignDate";
        //discreteValue[3] = new ParameterDiscreteValue();
        //discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_SignDate");
        //paraField[3].CurrentValues.Add(discreteValue[3]);

        //paraField[4] = new ParameterField();
        //paraField[4].Name = "REXContractStatus";
        //discreteValue[4] = new ParameterDiscreteValue();
        //discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractStatus");
        //paraField[4].CurrentValues.Add(discreteValue[4]);

        //paraField[5] = new ParameterField();
        //paraField[5].Name = "REXEndDate";
        //discreteValue[5] = new ParameterDiscreteValue();
        //discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ConEndDate");
        //paraField[5].CurrentValues.Add(discreteValue[5]);

        //paraField[6] = new ParameterField();
        //paraField[6].Name = "REXAdType";
        //discreteValue[6] = new ParameterDiscreteValue();
        //discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_AdType");
        //paraField[6].CurrentValues.Add(discreteValue[6]);

        //paraField[7] = new ParameterField();
        //paraField[7].Name = "REXAdMaker";
        //discreteValue[7] = new ParameterDiscreteValue();
        //discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_AdMaker");
        //paraField[7].CurrentValues.Add(discreteValue[7]);

        //paraField[8] = new ParameterField();
        //paraField[8].Name = "REXAdBoardName";
        //discreteValue[8] = new ParameterDiscreteValue();
        //discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_AdBoardName");
        //paraField[8].CurrentValues.Add(discreteValue[8]);

        //paraField[9] = new ParameterField();
        //paraField[9].Name = "REXAdBoardCode";
        //discreteValue[9] = new ParameterDiscreteValue();
        //discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_AdBoardCode");
        //paraField[9].CurrentValues.Add(discreteValue[9]);

        //paraField[10] = new ParameterField();
        //paraField[10].Name = "REXPrepayment";
        //discreteValue[10] = new ParameterDiscreteValue();
        //discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_Prepayment");
        //paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        //paraField[12] = new ParameterField();
        //paraField[12].Name = "REXPaymentPerCycle";
        //discreteValue[12] = new ParameterDiscreteValue();
        //discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_PaymentPerCycle");
        //paraField[12].CurrentValues.Add(discreteValue[12]);

        //paraField[13] = new ParameterField();
        //paraField[13].Name = "REXPayingCycle";
        //discreteValue[13] = new ParameterDiscreteValue();
        //discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_PayingCycle");
        //paraField[13].CurrentValues.Add(discreteValue[13]);

        //paraField[14] = new ParameterField();
        //paraField[14].Name = "REXTotalAmt";
        //discreteValue[14] = new ParameterDiscreteValue();
        //discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        //paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = " select Contract.ContractCode,Customer.CustName,Customer.CustCode,Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + "' End as ContractStatus,Contract.ConStartDate,Contract.ConEndDate,AdBoardType.AdBoardTypeName, " +
                         " AdBoardManage.AdBoardName,AdBoardManage.AdBoardCode " +
                         " from Contract ,Customer ,AdBoardManage ,AdBoardType,conadboard " +
                         " where contract.bizmode=3 and Contract.CustID = Customer.CustID " +
                         " and contract.contractid=conadboard.contractid and Conadboard.AdBoardid = AdBoardManage.AdBoardid and AdBoardManage.AdBoardTypeID=AdBoardType.AdBoardTypeID ";
        if (txtAdContractCode.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode like '%" + txtAdContractCode.Text + "%'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustCode.Text + "%'";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text + "%'";
        }
        //if (txtSignDate.Text != "")
        //{
        //    str_sql = str_sql + " AND Contract.SignDate = '" + txtSignDate.Text + "'";
        //}
        if (ddlContractStatus.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractStatus = '" + ddlContractStatus.SelectedValue + "'";
        }
        //if (ddlAdType.Text != "")
        //{
        //    str_sql = str_sql + " AND AdContract.AdType = '" + ddlAdType.Text + "'";
        //}
        if (txtStartDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConStartDate >= '" + txtStartDate.Text + "'";
        }
        if (txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConEndDate <= '" + txtEndDate.Text + "'";
        }


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\AdvisContractInfoItem.rpt";

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptAdvisContractInfoItem.aspx");
    }
}
