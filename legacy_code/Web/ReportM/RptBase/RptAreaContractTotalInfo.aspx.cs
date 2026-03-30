using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using RentableArea;

/// <summary>
/// 编写人:hesijian 
/// 编写时间:2009年4月27日

/// </summary>
public partial class ReportM_RptBase_RptAreaContractTotalInfo : BasePage
{
    public string baseInfo = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptAreaContractTotalInfo");
        InitDDL();
    }



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

        //绑定一级经营类别

        BaseBO baseBo = new BaseBO();
        ddlTradeID.Items.Add(new ListItem("", ""));
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs)
        {
            ddlTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        }
        ////绑定二级经营类别
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //ddlTrade2ID.Items.Add(new ListItem("", ""));
        //Resultset rs1 = baseBo.Query(new TradeRelation());
        //foreach (TradeRelation tradeDef in rs1)
        //{
        //    ddlTrade2ID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        //}
    }

    //绑定数据
    private void BindData()
    {

        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[11];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[11];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].ParameterFieldName = "REXContractID";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXCustCode";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].Name = "REXCustName";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustShortName");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].Name = "REXBizMode";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_BizMode");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].Name = "REXContractStatus";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractStatus");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXFStartDate";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FStartDate");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "REXFEndDate";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FEndDate");
        Field[6].CurrentValues.Add(DiscreteValue[6]);


        Field[7] = new ParameterField();
        Field[7].Name = "REXTotalArea";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_TotalArea");
        Field[7].CurrentValues.Add(DiscreteValue[7]);


        Field[8] = new ParameterField();
        Field[8].Name = "REXTitle";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptAreaContractTotalInfo");
        Field[8].CurrentValues.Add(DiscreteValue[8]);


        Field[9] = new ParameterField();
        Field[9].Name = "REXMallTitle";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = Session["MallTitle"].ToString();
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].Name = "REXTotal";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        Field[10].CurrentValues.Add(DiscreteValue[10]);




        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }



        string str_sql = " select " +
                   " max(dbo.Contract.ContractCode) as ContractCode, max(dbo.Customer.CustCode) as CustCode," +
                   " max(dbo.Customer.CustName) as CustName," +
                   " Case Contract.BizMode when '4' then '" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_Area") + "' End as BizMode , " +
                   " Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + "' End as ContractStatus, " +
                   " max(ConStartDate) as ConStartDate,max(ConEndDate) as ConEndDate,max(ConFormulaH.TotalArea) as TotalArea,dept.deptname " +
                   " FROM dbo.Contract " +
                   " Left JOIN conarea on conarea.contractid=contract.contractid " +
                   " left join areamanage on areamanage.areaid=conarea.conareacode " +
                   " Left Join Dept on dept.deptid=areamanage.storeid " +
                   " Left JOIN ConFormulaH ON dbo.Contract.ContractID = dbo.ConFormulaH.ContractID " +
                   " left Join Customer On (Contract.CustId=Customer.CustId) " +
                   " where Contract.BizMode=4";

        //条件查询
        if (txtContractID.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode like '%" + txtContractID.Text + "%'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustCode.Text + "%'";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text + "%'";
        }

        if (ddlContractStatus.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractStatus = '" + ddlContractStatus.Text + "'";
        }
        if (ddlTradeID.Text != "")
        {
            str_sql = str_sql + " AND Contract.RootTradeID like '%" + ddlTradeID.Text + "%'";
        }
        //if (ddlTrade2ID.Text != "")
        //{
        //    str_sql = str_sql + " AND Contract.TradeID like '%" + ddlTrade2ID.Text + "%'";
        //}
        if (txtConStartDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConStartDate >= '" + txtConStartDate.Text + "'";
        }
        if (txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConEndDate <= '" + txtEndDate.Text + "'";
        }

        str_sql = str_sql + " Group By ContractCode,CustCode,CustName,Contract.BizMode,ContractStatus,ConStartDate,dept.deptname ";
        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptAreaContractTotalInfo.rpt";

    }


    private void ClearPage()
    {
        txtContractID.Text = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        ddlTradeID.SelectedIndex = 0;
        //ddlTrade2ID.SelectedIndex = 0;
        txtConStartDate.Text = "";
        txtEndDate.Text = "";
        ddlContractStatus.SelectedIndex = 0;
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
